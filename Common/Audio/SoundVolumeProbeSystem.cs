using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static SoundCaptions.Common.Audio.SoundCaptureSystem;

namespace SoundCaptions.Common.Audio;

/// <summary>管理仅在诊断模式中使用的短声倍率和循环音效实例。</summary>
public sealed class SoundVolumeProbeSystem : ModSystem
{
    internal const string LoopIdentifier = "SoundCaptions/VolumeLoopProbe";
    private SlotId loop = SlotId.Invalid;
    private SlotId secondLoop = SlotId.Invalid;
    private bool HasLoop => loop.IsValid || secondLoop.IsValid;
    private float shortGain = 1f;
    private float loopGain = 1f;

    public override void Unload()
    {
        if (HasLoop)
            Main.RunOnMainThread(StopLoop).GetAwaiter().GetResult();
    }

    public override void PostUpdateEverything()
    {
        if (!Main.dedServ && (HasLoop || shortGain != 1f || loopGain != 1f) &&
            (Main.gameMenu || !DiagnosticsEnabled))
        {
            StopLoop();
            shortGain = loopGain = 1f;
        }
    }

    public override void OnWorldUnload()
    {
        if (HasLoop)
            Main.RunOnMainThread(StopLoop).GetAwaiter().GetResult();

        shortGain = loopGain = 1f;
    }

    /// <summary>为可调整的短声或循环探针设置测试倍率。</summary>
    internal bool SetGain(bool looping, float gain)
    {
        if (!ModContent.GetInstance<SoundVolumeSystem>().Ready)
            return false;

        if (looping)
            loopGain = gain;
        else
            shortGain = gain;

        return true;
    }

    /// <summary>启动一个全局循环或一对左右循环，并在失败时清理已启动的实例。</summary>
    internal bool StartLoop(bool spatialPair = false)
    {
        StopLoop();
        var style = SoundID.Item1 with
        {
            Identifier = LoopIdentifier,
            IsLooped = true,
            MaxInstances = 0,
            PauseBehavior = PauseBehavior.PauseWithGame,
            Volume = 0.25f
        };
        if (!spatialPair)
        {
            loop = PlayLoop(in style, null);
            return loop.IsValid;
        }

        Vector2 center = Main.LocalPlayer.Center;
        loop = PlayLoop(in style, center - new Vector2(320, 0));
        secondLoop = PlayLoop(in style, center + new Vector2(320, 0));
        if (loop.IsValid && secondLoop.IsValid)
            return true;

        StopLoop();
        return false;
    }

    /// <summary>播放仅在世界内且诊断开启时继续运行的测试循环。</summary>
    private static SlotId PlayLoop(in SoundStyle style, Vector2? position) =>
        SoundEngine.PlaySound(in style, position, static _ => !Main.gameMenu && DiagnosticsEnabled);

    /// <summary>停止并释放当前循环探针的句柄。</summary>
    internal void StopLoop()
    {
        Stop(loop);
        Stop(secondLoop);
        loop = secondLoop = SlotId.Invalid;
    }

    /// <summary>解除指定循环的探针回调并停止其活动实例。</summary>
    private static void Stop(SlotId slot)
    {
        if (slot.IsValid && SoundEngine.TryGetActiveSound(slot, out ActiveSound active))
        {
            active.Callback = null;
            active.Stop();
        }
    }

    internal string LoopStatus
    {
        get
        {
            if (loop.IsValid && SoundEngine.TryGetActiveSound(loop, out ActiveSound active) &&
                active.Sound is { IsDisposed: false } instance && active.IsPlayingOrPaused)
            {
                VolumeTargetKey key = VolumeTargetKey.FromStyle(active.Style);
                int percent = ModContent.GetInstance<SoundVolumeSystem>().PercentFor(key);
                string status = Text("LoopOutput", percent == 100 ? loopGain : percent / 100f, instance.Volume);
                if (secondLoop.IsValid && SoundEngine.TryGetActiveSound(secondLoop, out ActiveSound second) &&
                    second.Sound is { IsDisposed: false } secondInstance)
                    status += " · " + Text("LoopPairOutput", instance.Pan, secondInstance.Volume, secondInstance.Pan);

                return status;
            }

            return Text("LoopStopped");
        }
    }

    /// <summary>为匹配的测试音效返回短声或循环倍率。</summary>
    internal float GetGain(SoundStyle style)
    {
        if (style.Identifier == LoopIdentifier)
            return loopGain;

        return style.Identifier == null && style.SoundPath == SoundID.Item1.SoundPath &&
            style.Variants.SequenceEqual(SoundID.Item1.Variants) ? shortGain : 1f;
    }
}
