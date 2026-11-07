using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Audio;
using MonoMod.Cil;
using SoundCaptions.Common.Captions;
using SoundCaptions.Common.Config;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace SoundCaptions.Common.Audio;

/// <summary>在标准背景音乐的播放和淡化过程中应用逐曲音量。</summary>
public sealed class MusicVolumeSystem : ModSystem
{
    private static ClientConfig Config => ModContent.GetInstance<ClientConfig>();
    // 槽位只在本次加载有效。用它缓存查找结果，写入配置和跨重载匹配仍使用稳定曲目键。
    private readonly Dictionary<int, string> keysBySlot = new();
    private readonly ConditionalWeakTable<CueAudioTrack, MutedCue> mutedCues = new();
    private (string Key, int Percent)? preview;
    // 更新与淡出共用此上下文。每次调用结束都恢复原值，避免嵌套更新或异常让倍率串到其他曲目。
    private (IAudioTrack Track, float Gain) streamWrite;
    internal bool StreamReady { get; private set; }

    public override void Load()
    {
        if (Main.dedServ)
            return;

        On_LegacyAudioSystem.UpdateCommonTrack += UpdateTrack;
        On_LegacyAudioSystem.UpdateCommonTrackTowardStopping += FadeTrack;
        On_CueAudioTrack.Resume += ResumeCue;
        IL_ASoundEffectBasedAudioTrack.SetVariable += PatchStreamOutput;
    }

    public override void Unload()
    {
        if (!Main.dedServ)
        {
            IL_ASoundEffectBasedAudioTrack.SetVariable -= PatchStreamOutput;
            On_CueAudioTrack.Resume -= ResumeCue;
            On_LegacyAudioSystem.UpdateCommonTrackTowardStopping -= FadeTrack;
            On_LegacyAudioSystem.UpdateCommonTrack -= UpdateTrack;
        }

        keysBySlot.Clear();
        mutedCues.Clear();
        preview = null;
        streamWrite = default;
        StreamReady = false;
    }

    /// <summary>检查指定曲目是否使用可调整的原版 Cue 或流式后端。</summary>
    internal bool Supports(string key)
    {
        if (Main.audioSystem is not LegacyAudioSystem engine)
            return false;

        IAudioTrack track = TrackAt(engine, MusicTitleCatalog.ResolveSlot(key));
        return track is CueAudioTrack || StreamReady && track is ASoundEffectBasedAudioTrack;
    }

    /// <summary>返回该曲目的试听百分比或已保存的百分比。</summary>
    internal int PercentFor(string key) =>
        preview is { } pending && pending.Key == key ? pending.Percent : Config.GetMusicVolume(key) ?? ClientConfig.DefaultVolumePercent;

    /// <summary>在指定曲目上临时试听音量。</summary>
    internal void Preview(string key, int percent) => preview = (key, Math.Clamp(percent, 0, ClientConfig.MaxVolumePercent));

    /// <summary>取消当前音乐试听并恢复已保存设置。</summary>
    internal void CancelPreview() => preview = null;

    /// <summary>结束试听，并保存受支持曲目的新音量。</summary>
    internal bool Commit(string key, int percent)
    {
        CancelPreview();
        if (!Supports(key))
            return false;

        percent = Math.Clamp(percent, 0, ClientConfig.MaxVolumePercent);
        ClientConfig config = Config;
        if ((config.GetMusicVolume(key) ?? ClientConfig.DefaultVolumePercent) == percent)
            return true;

        var pending = (ClientConfig)config.Clone();
        pending.SetMusicVolume(key, percent);
        return config.SaveVolumeChanges(pending);
    }

    /// <summary>按槽位读取标准后端中的曲目实例。</summary>
    private static IAudioTrack TrackAt(LegacyAudioSystem engine, int slot) =>
        slot > 0 && slot < engine.AudioTracks.Length ? engine.AudioTracks[slot] : null;

    /// <summary>按当前槽位的稳定曲目键解析试听、配置或诊断倍率。</summary>
    private float GainFor(int slot)
    {
        ClientConfig config = Config;
        if (config == null)
            return 1f;

        if (preview != null || config.MusicVolumes.Count != 0)
        {
            if (!keysBySlot.TryGetValue(slot, out string key))
            {
                key = MusicTitleCatalog.ResolveKey(slot);
                keysBySlot.Add(slot, key);
            }

            if (key != null)
            {
                if (preview is { } pending && pending.Key == key)
                    return pending.Percent / (float)ClientConfig.DefaultVolumePercent;

                if (config.GetMusicVolume(key) is int percent)
                    return percent / (float)ClientConfig.DefaultVolumePercent;
            }
        }

        return config.EnableDiagnostics && !Main.gameMenu
            ? ModContent.GetInstance<MusicProbeSystem>()?.GainFor(slot) ?? 1f
            : 1f;
    }

    /// <summary>在常规更新中给当前轨道应用自己的音量并维持原有播放状态。</summary>
    private void UpdateTrack(On_LegacyAudioSystem.orig_UpdateCommonTrack orig,
        LegacyAudioSystem engine, bool active, int slot, float volume, ref float fade)
    {
        IAudioTrack track = TrackAt(engine, slot);
        float gain = track is CueAudioTrack or ASoundEffectBasedAudioTrack ? GainFor(slot) : 1f;
        if (track is CueAudioTrack cue)
        {
            if (gain == 0f && engine.WaveBank?.IsPrepared == true)
                PauseMutedCue(cue, slot, volume);

            // 静音曲目尚未播放时保持未启动。已经暂停的 Cue 保留原实例，恢复时才能延续进度。
            active = active && gain != 0f && !(mutedCues.TryGetValue(cue, out _) && cue.IsPaused);
            volume = AdjustCueVolume(volume, gain);
        }

        var previous = streamWrite;
        streamWrite = (track, gain);
        try
        {
            orig(engine, active, slot, volume, ref fade);
        }
        finally
        {
            streamWrite = previous;
        }
    }

    /// <summary>在淡出时使用退出曲目自己的音量倍率。</summary>
    private void FadeTrack(On_LegacyAudioSystem.orig_UpdateCommonTrackTowardStopping orig,
        LegacyAudioSystem engine, int slot, float volume, ref float fade, bool mainAudible)
    {
        IAudioTrack track = TrackAt(engine, slot);
        float gain = track is CueAudioTrack or ASoundEffectBasedAudioTrack ? GainFor(slot) : 1f;
        if (track is CueAudioTrack cue)
        {
            if (gain == 0f && engine.WaveBank?.IsPrepared == true)
                PauseMutedCue(cue, slot, volume);

            volume = AdjustCueVolume(volume, gain);
        }

        var previous = streamWrite;
        streamWrite = (track, gain);
        try
        {
            orig(engine, slot, volume, ref fade, mainAudible);
        }
        finally
        {
            streamWrite = previous;
        }
    }

    /// <summary>暂停已在播放的原版 Cue，并保存恢复时所需的基准音量。</summary>
    private void PauseMutedCue(CueAudioTrack cue, int slot, float volume)
    {
        bool paused = cue.IsPaused;
        if (!paused && !cue.IsPlaying)
        {
            mutedCues.Remove(cue);
            return;
        }

        mutedCues.GetValue(cue, _ => new MutedCue(slot)).Volume = volume;
        if (!paused)
            cue.Pause();
    }

    /// <summary>在允许播放且取消静音后恢复原有 Cue。</summary>
    private void ResumeCue(On_CueAudioTrack.orig_Resume orig, CueAudioTrack cue)
    {
        if (mutedCues.TryGetValue(cue, out MutedCue state))
        {
            float gain = GainFor(state.Slot);
            if (gain == 0f || Main.musicVolume <= 0f)
                return;

            cue.SetVariable("Volume", AdjustCueVolume(state.Volume, gain));
            mutedCues.Remove(cue);
        }
        else if (Main.audioSystem is LegacyAudioSystem engine)
        {
            // 游戏可能先因失焦暂停，再由音量挂钩观察到静音。此时仍要按当前曲目设置决定是否恢复。
            int slot = Array.IndexOf(engine.AudioTracks, cue);
            if (slot > 0 && GainFor(slot) == 0f)
                return;
        }

        orig(cue);
    }

    /// <summary>根据原版 Cue 的衰减曲线计算等效音量输入。</summary>
    internal static float AdjustCueVolume(float volume, float gain)
    {
        if (gain == 1f)
            return volume;

        // 原版音乐的 XACT Volume 曲线定义在 0～1，每个输入单位对应 31 dB。
        return gain == 0f ? 0f : Math.Clamp(volume + (20f / 31f) * MathF.Log10(gain), 0f, 1f);
    }

    /// <summary>在流式后端的实际音量写入处注册倍率处理。</summary>
    private void PatchStreamOutput(ILContext il)
    {
        var cursor = new ILCursor(il);
        int sites = 0;
        while (cursor.TryGotoNext(MoveType.Before, instruction => instruction.MatchCallvirt<SoundEffectInstance>("set_Volume")))
        {
            cursor.EmitLdarg0();
            cursor.EmitDelegate<Func<float, ASoundEffectBasedAudioTrack, float>>(ScaleStreamOutput);
            cursor.Index++;
            sites++;
        }

        // 预期包含常规音量与直写音量。匹配不完整时使缩放委托保持透传，避免只在一条分支生效。
        StreamReady = sites == 2;
        if (!StreamReady)
            Mod.Logger.Warn($"Sound Captions: expected two streaming volume writes; found {sites}.");
    }

    /// <summary>仅在当前更新范围内为对应流式轨道缩放引擎提供的基准音量。</summary>
    private float ScaleStreamOutput(float volume, ASoundEffectBasedAudioTrack track) =>
        StreamReady && ReferenceEquals(track, streamWrite.Track) ? volume * streamWrite.Gain : volume;

    /// <summary>记录静音暂停的 Cue 所属槽位及其最近一次原始音量。</summary>
    private sealed class MutedCue(int slot)
    {
        internal int Slot { get; } = slot;
        internal float Volume;
    }
}
