using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Audio;
using MonoMod.Cil;
using SoundCaptions.Common.Config;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace SoundCaptions.Common.Audio;

/// <summary>在标准音效的启动和更新阶段应用单项音量配置。</summary>
public sealed class SoundVolumeSystem : ModSystem
{
    private static ClientConfig Config => ModContent.GetInstance<ClientConfig>();
    // 缓存与活动实例共用生命周期，重复更新可复用稳定键，声音结束后也不会被缓存强引用。
    private readonly ConditionalWeakTable<ActiveSound, Target> targets = new();
    private (VolumeTargetKey Key, int Percent)? preview;
    private bool outputHook;
    private bool startHook;

    internal bool Ready => outputHook && startHook;

    public override void Load()
    {
        if (Main.dedServ)
            return;

        // 只在客户端成对装卸这两个挂钩，重载后每次基准音量仍只缩放一次。
        IL_ActiveSound.Update += PatchOutput;
        IL_ActiveSound.Play += PatchStart;
    }

    public override void Unload()
    {
        if (!Main.dedServ)
        {
            IL_ActiveSound.Play -= PatchStart;
            IL_ActiveSound.Update -= PatchOutput;
        }

        outputHook = startHook = false;
        preview = null;
        targets.Clear();
    }

    /// <summary>返回指定声音族的试听百分比或已保存的百分比。</summary>
    internal int PercentFor(VolumeTargetKey key) =>
        preview is { } pending && pending.Key == key ? pending.Percent : Config.GetSoundVolume(key) ?? ClientConfig.DefaultVolumePercent;

    /// <summary>为一个声音族设置临时试听百分比。</summary>
    internal void Preview(VolumeTargetKey key, int percent) => preview = (key, Math.Clamp(percent, 0, ClientConfig.MaxVolumePercent));

    /// <summary>清除试听倍率并恢复已保存的音量设置。</summary>
    internal void CancelPreview() => preview = null;

    /// <summary>结束试听，并在挂钩可用时保存变更后的声音族音量。</summary>
    internal bool Commit(VolumeTargetKey key, int percent)
    {
        CancelPreview();
        if (!Ready)
            return false;

        percent = Math.Clamp(percent, 0, ClientConfig.MaxVolumePercent);
        ClientConfig config = Config;
        if ((config.GetSoundVolume(key) ?? ClientConfig.DefaultVolumePercent) == percent)
            return true;

        var pending = (ClientConfig)config.Clone();
        pending.SetSoundVolume(key, percent);
        return config.SaveVolumeChanges(pending);
    }

    /// <summary>在活动实例写入输出音量前接入独立倍率。</summary>
    private void PatchOutput(ILContext il)
    {
        var cursor = new ILCursor(il);
        outputHook = cursor.TryGotoNext(MoveType.Before,
            instruction => instruction.MatchCallvirt<SoundEffectInstance>("set_Volume"));
        if (!outputHook)
        {
            Mod.Logger.Warn("Sound Captions: ActiveSound.Update has no matching volume write.");
            return;
        }

        cursor.EmitLdarg0();
        cursor.EmitDelegate<Func<float, ActiveSound, float>>(ScaleOutput);
    }

    /// <summary>在原生音频实例开始播放之前接入首声保护。</summary>
    private void PatchStart(ILContext il)
    {
        var cursor = new ILCursor(il);
        startHook = cursor.TryGotoNext(MoveType.Before, instruction => instruction.MatchCallvirt<SoundEffectInstance>("Play"));
        if (!startHook)
        {
            Mod.Logger.Warn("Sound Captions: ActiveSound.Play has no matching playback call.");
            return;
        }

        cursor.EmitDup();
        cursor.EmitLdarg0();
        cursor.EmitDelegate<Action<SoundEffectInstance, ActiveSound>>(PrimeOutput);
    }

    /// <summary>在引擎计算首个基准音量前防止受衰减的实例以原音量出声。</summary>
    private void PrimeOutput(SoundEffectInstance instance, ActiveSound active)
    {
        // 原生声音先启动，引擎随后才算出首个位置音量。受衰减实例先以零音量启动，避免首声泄露。
        if (GainFor(active) < 1f)
            instance.Volume = 0f;
    }

    /// <summary>将原生本次计算的输出与有效倍率相乘。</summary>
    private float ScaleOutput(float volume, ActiveSound active) => volume * GainFor(active);

    /// <summary>按活动实例的稳定目标解析试听、配置或诊断倍率。</summary>
    private float GainFor(ActiveSound active)
    {
        ClientConfig config = Config;
        // 只有启动和更新两处都匹配时才启用。签名变化时继续使用引擎的音量，避免首声与后续输出不一致。
        if (!Ready || config == null)
            return 1f;

        if (preview != null || config.HasSoundVolumes)
        {
            VolumeTargetKey key = targets.GetValue(active, static sound =>
            {
                SoundStyle style = sound.Style;
                return new Target(VolumeTargetKey.FromStyle(in style));
            }).Key;
            if (preview is { } pending && pending.Key == key)
                return pending.Percent / (float)ClientConfig.DefaultVolumePercent;
            if (config.GetSoundVolume(key) is int percent)
                return percent / (float)ClientConfig.DefaultVolumePercent;
        }

        return config.EnableDiagnostics && !Main.gameMenu
            ? ModContent.GetInstance<SoundVolumeProbeSystem>()?.GetGain(active.Style) ?? 1f
            : 1f;
    }

    /// <summary>保存随活动实例回收的音量目标键缓存。</summary>
    private sealed record Target(VolumeTargetKey Key);
}
