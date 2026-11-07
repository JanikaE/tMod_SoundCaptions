using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using SoundCaptions.Common.Captions;
using SoundCaptions.Common.Config;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace SoundCaptions.Common.Audio;

/// <summary>观察本机标准播放链路中引擎实际接受的音效。</summary>
public sealed class SoundCaptureSystem : ModSystem
{
    internal static bool DiagnosticsEnabled => ModContent.GetInstance<ClientConfig>().EnableDiagnostics;

    internal int Accepted { get; private set; }
    internal int Rejected { get; private set; }
    internal PlaybackDiagnostic? Latest { get; private set; }
    private int fallback;
    private string lastRejectedPath;
    private string[] panelLines;
    private int worldResetPending;

    public override void Load()
    {
        if (!Main.dedServ)
            On_SoundPlayer.Play_Inner += Capture;
    }

    public override void Unload()
    {
        if (!Main.dedServ)
            On_SoundPlayer.Play_Inner -= Capture;

        Clear();
    }

    // 先标记世界边界，在客户端接受下一次播放前清理旧统计，避免把新世界首声清掉。
    public override void OnWorldLoad() => Interlocked.Exchange(ref worldResetPending, 1);

    public override void OnWorldUnload() => Interlocked.Exchange(ref worldResetPending, 1);

    public override void PostUpdateEverything()
    {
        ApplyWorldReset();
        if ((Accepted != 0 || Rejected != 0) && !DiagnosticsEnabled)
            Clear();
    }

    /// <summary>在客户端更新或绘制时消费世界切换信号，清除旧诊断状态。</summary>
    private void ApplyWorldReset()
    {
        if (Interlocked.Exchange(ref worldResetPending, 0) != 0)
            Clear();
    }

    /// <summary>清空诊断计数、最近事件和绘制缓存。</summary>
    internal void Clear()
    {
        Accepted = 0;
        Rejected = 0;
        fallback = 0;
        Latest = null;
        lastRejectedPath = null;
        panelLines = null;
    }

    /// <summary>在原生播放完成后读取已接受的实例并记录字幕与诊断信息。</summary>
    private SlotId Capture(On_SoundPlayer.orig_Play_Inner orig, SoundPlayer player,
        ref SoundStyle style, Vector2? position, SoundUpdateCallback callback)
    {
        // 直接播放与排队播放都在原生主线程入口汇合。先让引擎处理变体、实例上限和回调，
        // 再读取返回的实例，避免为未被接受的请求生成字幕。
        SlotId slot = orig(player, ref style, position, callback);
        if (Main.gameMenu)
            return slot;

        ClientConfig config = ModContent.GetInstance<ClientConfig>();
        bool diagnostics = config.EnableDiagnostics;
        if (!config.ShowSoundCaptions && !diagnostics)
            return slot;

        if (diagnostics)
            ApplyWorldReset();
        if (!slot.IsValid || !player.TryGetActiveSound(slot, out ActiveSound active) ||
            active.Sound is not { IsDisposed: false } instance)
        {
            if (diagnostics)
            {
                Rejected++;
                lastRejectedPath = style.SoundPath;
            }
        }
        else
        {
            SoundPlayback playback = SoundPlayback.Create(slot, active.Style, active.Position);
            if (config.ShowSoundCaptions)
                ModContent.GetInstance<SoundCaptionSystem>().Record(playback, player);

            if (diagnostics)
            {
                Accepted++;
                if (CaptionCatalog.Resolve(playback.VolumeKey).IsFallback)
                    fallback++;
                Latest = new PlaybackDiagnostic(playback, instance.Volume, instance.Pan, instance.Pitch);
            }
        }

        panelLines = null;
        return slot;
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (Main.dedServ || Main.gameMenu || !DiagnosticsEnabled)
            return;

        int index = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
        if (index >= 0)
            layers.Insert(index, new LegacyGameInterfaceLayer("Sound Captions: Diagnostics", DrawDiagnostics, InterfaceScaleType.UI));
    }

    /// <summary>绘制当前音效捕获和音乐诊断信息。</summary>
    private bool DrawDiagnostics()
    {
        ApplyWorldReset();
        panelLines ??= BuildPanelLines();
        Vector2 position = new(18, 120);
        foreach (string line in panelLines)
        {
            Utils.DrawBorderString(Main.spriteBatch, line, position, Color.White, 0.8f);
            position.Y += 22;
        }

        Utils.DrawBorderString(Main.spriteBatch, ModContent.GetInstance<MusicProbeSystem>().Status,
            position, Color.White, 0.8f);
        position.Y += 22f;
        Utils.DrawBorderString(Main.spriteBatch,
            Text("CaptionCount", ModContent.GetInstance<SoundCaptionSystem>().ActiveCount), position, Color.White, 0.8f);
        return true;
    }

    /// <summary>用最新的诊断状态生成可复用的面板文字。</summary>
    private string[] BuildPanelLines()
    {
        string counts = Text("Counts", Accepted, Rejected) + " | " + Text("NameCounts", Accepted - fallback, fallback);
        if (Latest is not { } latest)
            return [counts, Text("LastRejected", lastRejectedPath ?? "-")];

        return [
            counts,
            Text("LastRejected", lastRejectedPath ?? "-"),
            Text("LastAccepted", latest.Playback.RawKey, DescribePosition(latest.Playback.Position)),
            Text("Identity", latest.Playback.CaptionKey, latest.Playback.VolumeKey),
            Text("Output", latest.OutputVolume, latest.Pan, latest.Pitch)
        ];
    }

    /// <summary>将世界坐标或无坐标状态格式化为诊断文字。</summary>
    internal static string DescribePosition(Vector2? position) => position is Vector2 point
        ? $"{point.X:0.0}, {point.Y:0.0}"
        : Text("NoPosition");

    /// <summary>获取并格式化诊断本地化文本。</summary>
    internal static string Text(string key, params object[] args) =>
        Language.GetTextValue($"Mods.SoundCaptions.Diagnostics.{key}", args);
}

/// <summary>记录一次成功播放的稳定身份、初始输出、声像和音高。</summary>
internal readonly record struct PlaybackDiagnostic(SoundPlayback Playback, float OutputVolume, float Pan, float Pitch);
