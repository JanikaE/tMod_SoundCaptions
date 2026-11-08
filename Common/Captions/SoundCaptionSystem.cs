using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using SoundCaptions.Common.Audio;
using SoundCaptions.Common.Config;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace SoundCaptions.Common.Captions;

/// <summary>更新客户端音效与音乐字幕，并在普通模式下绘制。</summary>
public sealed class SoundCaptionSystem : ModSystem
{
    private static ClientConfig Config => ModContent.GetInstance<ClientConfig>();

    private readonly CaptionStore store = new();
    private readonly CaptionClock clock = new();
    private int worldResetPending;
    private int sampledMusicSlot;
    private string sampledMusicKey;
    private SoundPlayback[] stressEvents;

    internal MusicCaptionState Music { get; } = new();

    internal int ActiveCount => store.Count;
    internal LinkedListNode<CaptionEntry> BottomEntry => store.BottomEntry;
    internal double Now => clock.Now;

    public override void Load()
    {
        if (!Main.dedServ)
            On_Main.Update += UpdateCaptions;
    }

    public override void Unload()
    {
        if (!Main.dedServ)
            On_Main.Update -= UpdateCaptions;

        Reset();
    }

    // 世界切换只发出重置信号。实际清理由客户端更新或播放回调执行，避免跨线程修改字幕集合。
    public override void OnWorldLoad() => Interlocked.Exchange(ref worldResetPending, 1);

    public override void OnWorldUnload() => Interlocked.Exchange(ref worldResetPending, 1);

    /// <summary>在客户端消费世界切换信号，清除旧世界的字幕与采样。</summary>
    private void ApplyWorldReset()
    {
        if (Interlocked.Exchange(ref worldResetPending, 0) != 0)
            Reset();
    }

    /// <summary>清除字幕、时钟、当前曲目缓存和诊断事件。</summary>
    private void Reset()
    {
        store.Clear();
        clock.Reset();
        Music.Clear();
        sampledMusicSlot = 0;
        sampledMusicKey = null;
        stressEvents = null;
    }

    /// <summary>在游戏更新后采样曲目与时钟，并维护当前有效字幕。</summary>
    private void UpdateCaptions(On_Main.orig_Update orig, Main game, GameTime gameTime)
    {
        orig(game, gameTime);
        ApplyWorldReset();

        if (Main.gameMenu)
        {
            Reset();
            return;
        }

        bool paused = Main.netMode == NetmodeID.SinglePlayer && Main.gamePaused;
        clock.Sample(Stopwatch.GetTimestamp(), paused);
        ClientConfig config = Config;
        int slot = SoundEngine.IsAudioSupported ? Main.curMusic : 0;
        if (slot != sampledMusicSlot)
        {
            sampledMusicSlot = slot;
            sampledMusicKey = MusicTitleCatalog.ResolveKey(slot);
        }

        Music.Update(sampledMusicKey, clock.Now, config.ShowMusicCaptions, config.MusicMode,
            CaptionDuration(config.MusicDurationSeconds));

        if (!config.ShowSoundCaptions)
            store.Clear();
        else if (!paused)
            store.Update(clock.Now, CaptionDuration(config.SoundDurationSeconds));
    }

    /// <summary>以当前字幕时钟记录一次本机已接受的播放。</summary>
    internal void Record(SoundPlayback playback, SoundPlayer player)
    {
        ApplyWorldReset();
        clock.Sample(Stopwatch.GetTimestamp(), Main.netMode == NetmodeID.SinglePlayer && Main.gamePaused);
        store.Record(playback, player, clock.Now, CaptionDuration(Config.SoundDurationSeconds));
    }

    /// <summary>规范化字幕时长，并为无效数值使用默认时长。</summary>
    private static double CaptionDuration(float duration) =>
        float.IsFinite(duration) ? Math.Clamp((double)duration, 0.5, 15.0) : ClientConfig.DefaultCaptionDurationSeconds;

    /// <summary>一次写入指定数量的不同合成字幕。</summary>
    internal void RecordDiagnosticBatch(int count)
    {
        ApplyWorldReset();
        clock.Sample(Stopwatch.GetTimestamp(), Main.netMode == NetmodeID.SinglePlayer && Main.gamePaused);
        double duration = CaptionDuration(Config.SoundDurationSeconds);
        for (int i = 1; i <= count; i++)
            store.Record(CreateDiagnosticPlayback(FormattableString.Invariant($"{i:00}")), null, clock.Now, duration);
    }

    /// <summary>复用合成事件执行同族与多族字幕压力写入。</summary>
    internal void RecordDiagnosticStress(int repetitions, int batch)
    {
        ApplyWorldReset();
        clock.Sample(Stopwatch.GetTimestamp(), Main.netMode == NetmodeID.SinglePlayer && Main.gamePaused);
        double now = clock.Now;
        double duration = CaptionDuration(Config.SoundDurationSeconds);
        if (stressEvents == null || stressEvents.Length <= batch)
        {
            stressEvents = new SoundPlayback[batch + 1];
            stressEvents[0] = CreateDiagnosticPlayback("Stress");
            for (int i = 1; i < stressEvents.Length; i++)
                stressEvents[i] = CreateDiagnosticPlayback(FormattableString.Invariant($"{i:00}"));
        }

        for (int i = 0; i < repetitions; i++)
            store.Record(stressEvents[0], null, now, duration);
        for (int i = 1; i <= batch; i++)
            store.Record(stressEvents[i], null, now, duration);
    }

    /// <summary>写入用于验证窄窗换行的长字幕。</summary>
    internal void RecordDiagnosticLong() => Record(CreateDiagnosticPlayback("Long"), null);

    /// <summary>按名称构造不播放实际音频的诊断字幕事件。</summary>
    private static SoundPlayback CreateDiagnosticPlayback(string name)
    {
        string path = $"SoundCaptions/Diagnostic/{name}";
        var target = new VolumeTargetKey(path, null, null);
        return new SoundPlayback(SlotId.Invalid, new RawSoundKey(path), new CaptionKey(target), target, null, false);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (Main.dedServ || Main.gameMenu || Main.hideUI)
            return;

        if (ModContent.GetInstance<CaptionEditSystem>().Visible)
            return;

        ClientConfig config = Config;
        bool drawSound = config.ShowSoundCaptions && store.Count != 0;
        bool drawMusic = config.ShowMusicCaptions && Music.VisibleKey != null;
        if (!drawSound && !drawMusic)
            return;

        int index = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
        if (index >= 0)
            layers.Insert(index, new LegacyGameInterfaceLayer("Sound Captions: Captions", Draw, InterfaceScaleType.UI));
    }

    /// <summary>按配置绘制音乐基线及从窗口底部排入的音效字幕。</summary>
    private bool Draw()
    {
        ApplyWorldReset();
        // UI 层已经换算屏幕尺寸，直接使用界面坐标可避免缩放时重复换算。
        Rectangle bounds = CaptionLayout.GetBounds(Main.screenWidth, Main.screenHeight,
            ModContent.GetInstance<CaptionLayoutConfig>());
        if (bounds.Width == 0 || bounds.Height == 0)
            return true;

        var renderer = new CaptionRenderer(Main.spriteBatch, FontAssets.MouseText.Value, TextureAssets.MagicPixel.Value);
        ClientConfig config = Config;
        if (config.ShowMusicCaptions)
        {
            var music = CaptionAppearance.Create(config.MusicTextColor, config.MusicBackgroundColor,
                config.MusicBackgroundOpacity, config.MusicTextScale);
            int reserved = renderer.MusicRowHeight(music) + CaptionRenderer.RowGap;
            if (Music.CurrentKey is string key)
            {
                int used = renderer.DrawRow(bounds, MusicTitleCatalog.GetTitle(key), music,
                    music: true, opacity: Music.Opacity(clock.Now));
                reserved = Math.Max(reserved, used);
            }

            // 音乐短显消失后仍预留行高，避免音效字幕随切曲出现和消失而上下跳动。
            bounds.Height = Math.Max(0, bounds.Height - reserved);
        }

        if (!config.ShowSoundCaptions)
            return true;

        var sound = CaptionAppearance.Create(config.SoundTextColor, config.SoundBackgroundColor,
            config.SoundBackgroundOpacity, config.SoundTextScale);
        float? playerX = Main.LocalPlayer is { active: true } player ? player.Center.X : null;
        int threshold = Math.Clamp(config.DirectionThreshold, 0, 160);
        for (var node = store.BottomEntry; node != null && bounds.Height > 0; node = node.Previous)
        {
            CaptionEntry entry = node.Value;
            int direction = GetDirection(entry.Position, playerX, threshold);
            float opacity = CaptionRenderer.TimedOpacity(clock.Now, entry.ExpiresAt);
            int used = renderer.DrawRow(bounds, entry.Label.Text, sound, direction, opacity: opacity);
            if (used == 0)
                break;

            bounds.Height -= used;
        }

        return true;
    }

    /// <summary>按声源与本机角色的水平差计算左、右或无方向。</summary>
    internal static int GetDirection(Vector2? position, float? playerX, int threshold)
    {
        if (position is not Vector2 source || playerX is not float x)
            return 0;

        float difference = source.X - x;
        return difference < -threshold ? -1 : difference > threshold ? 1 : 0;
    }

}
