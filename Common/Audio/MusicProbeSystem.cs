using SoundCaptions.Common.Captions;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static SoundCaptions.Common.Audio.SoundCaptureSystem;

namespace SoundCaptions.Common.Audio;

/// <summary>提供原版曲目与流式曲目的客户端诊断探针。</summary>
public sealed class MusicProbeSystem : ModSystem
{
    private const int CueSlot = MusicID.OverworldDay;
    private const string StreamPath = "Music/DiagnosticLoop";
    private int streamSlot;
    private int? forcedSlot;
    private float cueGain = 1f;
    private float streamGain = 1f;

    public override void Load()
    {
        if (Main.dedServ)
            return;

        On_LegacyAudioSystem.UpdateMisc += ApplySelection;
    }

    public override void PostSetupContent()
    {
        if (!Main.dedServ)
            streamSlot = MusicLoader.GetMusicSlot(Mod, StreamPath);
    }

    public override void Unload()
    {
        if (Main.dedServ)
            return;

        On_LegacyAudioSystem.UpdateMisc -= ApplySelection;
        Reset();
        streamSlot = 0;
    }

    public override void OnWorldUnload() => Reset();

    /// <summary>清除强制选曲和测试倍率，避免探针状态跨世界生效。</summary>
    private void Reset()
    {
        forcedSlot = null;
        cueGain = streamGain = 1f;
    }

    internal string Status
    {
        get
        {
            MusicCaptionState music = ModContent.GetInstance<SoundCaptionSystem>().Music;
            return Text("MusicStatus", music.CurrentKey ?? Text("NoTrack"), music.SelectionChanges,
                FadeAt(CueSlot), FadeAt(streamSlot));
        }
    }

    /// <summary>读取有效音乐槽位的当前淡入淡出比例。</summary>
    private static float FadeAt(int slot) =>
        slot > 0 && slot < Main.musicFade.Length ? Main.musicFade[slot] : 0f;

    /// <summary>切换指定的测试曲目或恢复游戏自动选曲。</summary>
    internal bool Select(string sample)
    {
        switch (sample)
        {
            case "auto": forcedSlot = null; return true;
            case "none": forcedSlot = 0; return true;
            case "cue" when TrackAt(CueSlot) is CueAudioTrack: forcedSlot = CueSlot; return true;
            case "stream" when TrackAt(streamSlot) is ASoundEffectBasedAudioTrack: forcedSlot = streamSlot; return true;
            default: return false;
        }
    }

    /// <summary>为可用的测试曲目设置独立诊断倍率。</summary>
    internal bool SetGain(string sample, float gain)
    {
        switch (sample)
        {
            case "cue" when TrackAt(CueSlot) is CueAudioTrack: cueGain = gain; return true;
            case "stream" when ModContent.GetInstance<MusicVolumeSystem>().StreamReady && TrackAt(streamSlot) is ASoundEffectBasedAudioTrack: streamGain = gain; return true;
            default: return false;
        }
    }

    /// <summary>在标准音频后端按槽位查找已加载的测试曲目。</summary>
    private static IAudioTrack TrackAt(int slot) =>
        Main.audioSystem is LegacyAudioSystem engine && slot > 0 && slot < engine.AudioTracks.Length
            ? engine.AudioTracks[slot]
            : null;

    /// <summary>在游戏确定候选曲目后应用探针选择，保留原有选曲提交和淡化流程。</summary>
    private void ApplySelection(On_LegacyAudioSystem.orig_UpdateMisc orig, LegacyAudioSystem engine)
    {
        if (Main.gameMenu || !DiagnosticsEnabled)
            Reset();

        // 探针在环境、八音盒和总音量参与选曲后、提交当前槽位前接入，便于验证原有淡化流程。
        if (forcedSlot is int slot && Main.musicVolume > 0f && slot < engine.AudioTracks.Length)
            Main.newMusic = slot;

        orig(engine);
    }

    /// <summary>返回指定测试曲目的诊断倍率，其他曲目使用原倍率。</summary>
    internal float GainFor(int slot) => slot == CueSlot ? cueGain : slot == streamSlot ? streamGain : 1f;
}
