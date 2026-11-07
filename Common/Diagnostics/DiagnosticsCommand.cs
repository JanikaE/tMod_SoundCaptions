using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using SoundCaptions.Common.Audio;
using SoundCaptions.Common.Captions;
using SoundCaptions.Common.Config;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static SoundCaptions.Common.Audio.SoundCaptureSystem;

namespace SoundCaptions.Common.Diagnostics;

/// <summary>提供受客户端诊断开关控制的字幕、音效和音乐检查命令。</summary>
public sealed class DiagnosticsCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;
    public override string Command => "soundcaptionsdiag";
    public override string Usage => "/soundcaptionsdiag [clear | left | center | near | right | global | repeat | mixed | reject | attenuated | far | music | loop | gain | captions | catalog | stress]";
    public override string Description => Text("Description");

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (Main.dedServ)
            return;

        if (!DiagnosticsEnabled)
        {
            caller.Reply(Text("Disabled"));
            return;
        }

        var capture = ModContent.GetInstance<SoundCaptureSystem>();
        if (args.Length == 0)
        {
            var mod = (SoundCaptions)Mod;
            caller.Reply(Text(mod.ToggleEditMode != null ? "KeybindReady" : "KeybindUnavailable"));
            caller.Reply(Text(SoundEngine.IsAudioSupported ? "AudioAvailable" : "AudioUnavailable", Main.soundVolume));
            caller.Reply(Usage);
            return;
        }

        if (args[0] == "captions")
        {
            RunCaptionProbe(caller, args);
            return;
        }

        if (args[0] == "stress")
        {
            var stress = ModContent.GetInstance<CaptionStressSystem>();
            caller.Reply(args.Length == 2 ? args[1] switch
            {
                "baseline" => stress.Start(load: false),
                "run" => stress.Start(load: true),
                "status" => stress.Status(),
                "stop" => stress.Stop(),
                _ => "/soundcaptionsdiag stress [baseline | run | status | stop]"
            } : "/soundcaptionsdiag stress [baseline | run | status | stop]");
            return;
        }

        if (RunAudioProbe(caller, args))
            return;

        if (args.Length != 1)
        {
            caller.Reply(Usage);
            return;
        }

        var style = SoundID.Item1 with { MaxInstances = 0 };
        Vector2 center = Main.LocalPlayer.Center;
        switch (args[0])
        {
            case "clear":
                capture.Clear();
                caller.Reply(Text("Cleared"));
                break;
            case "left":
                PlayProbe(caller, capture, style, center - new Vector2(320, 0));
                break;
            case "center":
                PlayProbe(caller, capture, style, center);
                break;
            case "near":
                PlayProbe(caller, capture, style, center + new Vector2(16, 0));
                break;
            case "right":
                PlayProbe(caller, capture, style, center + new Vector2(320, 0));
                break;
            case "global":
                PlayProbe(caller, capture, style, null);
                break;
            case "repeat":
                PlayProbe(caller, capture, style, center);
                PlayProbe(caller, capture, style, center, nullableOverload: true);
                break;
            case "catalog":
                PlayProbe(caller, capture, new SoundStyle("SoundCaptions/Sounds/CatalogChime"), center);
                break;
            case "mixed":
                PlayProbe(caller, capture, style, center);
                PlayProbe(caller, capture, SoundID.DoorOpen with { MaxInstances = 0 }, center);
                break;
            case "reject":
                ProbeInstanceLimit(caller, capture, center);
                break;
            case "attenuated":
                PlayProbe(caller, capture, style, Main.Camera.Center + new Vector2(5000, 0));
                break;
            case "far":
                Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                PlayProbe(caller, capture, style, screenCenter + new Vector2(12000, 0));
                break;
            default:
                caller.Reply(Usage);
                break;
        }
    }

    /// <summary>按子命令报告条目数，或生成批量与长文本测试字幕。</summary>
    private static void RunCaptionProbe(CommandCaller caller, string[] args)
    {
        var captions = ModContent.GetInstance<SoundCaptionSystem>();
        if (args.Length == 1 || (args.Length == 2 && args[1] == "count"))
        {
            caller.Reply(Text("CaptionCount", captions.ActiveCount));
            return;
        }

        if (args.Length != 2 || args[1] is not ("batch" or "long"))
        {
            caller.Reply("/soundcaptionsdiag captions [count | batch | long]");
            return;
        }

        if (!ModContent.GetInstance<ClientConfig>().ShowSoundCaptions)
        {
            caller.Reply(Text("CaptionsDisabled"));
            return;
        }

        if (args[1] == "long")
        {
            captions.RecordDiagnosticLong();
            return;
        }

        const int count = 30;
        captions.RecordDiagnosticBatch(count);
        caller.Reply(Text("CaptionBatch", count, captions.ActiveCount));
    }

    /// <summary>分派曲目选择、循环控制和测试倍率子命令。</summary>
    private static bool RunAudioProbe(CommandCaller caller, string[] args)
    {
        var music = ModContent.GetInstance<MusicProbeSystem>();
        var sound = ModContent.GetInstance<SoundVolumeProbeSystem>();
        switch (args[0])
        {
            case "music":
                if (args.Length == 1)
                    caller.Reply(music.Status);
                else if (args.Length == 2 && args[1] is "auto" or "cue" or "stream" or "none")
                    caller.Reply(music.Select(args[1]) ? Text("SelectedProbe", args[1]) : Text("ProbeUnavailable"));
                else
                    caller.Reply("/soundcaptionsdiag music [auto | cue | stream | none]");
                return true;
            case "loop":
                if (args.Length == 1 || (args.Length == 2 && args[1] == "status"))
                    caller.Reply(sound.LoopStatus);
                else if (args.Length == 2 && args[1] is "start" or "pair")
                    caller.Reply(Text(sound.StartLoop(spatialPair: args[1] == "pair") ? "LoopStarted" : "ProbeUnavailable"));
                else if (args.Length == 2 && args[1] == "stop")
                {
                    sound.StopLoop();
                    caller.Reply(Text("LoopStopped"));
                }
                else
                    caller.Reply("/soundcaptionsdiag loop [start | pair | stop | status]");
                return true;
            case "gain":
                if (args.Length != 3 || args[2] is not ("0" or "50" or "100"))
                {
                    caller.Reply("/soundcaptionsdiag gain [short | loop | cue | stream] [0 | 50 | 100]");
                    return true;
                }

                float gain = int.Parse(args[2]) / 100f;
                bool applied = args[1] switch
                {
                    "short" => sound.SetGain(looping: false, gain),
                    "loop" => sound.SetGain(looping: true, gain),
                    "cue" or "stream" => music.SetGain(args[1], gain),
                    _ => false
                };
                caller.Reply(applied ? Text("GainSet", args[1], gain) : Text("ProbeUnavailable"));
                return true;
            default:
                return false;
        }
    }

    /// <summary>通过原生播放接口发出测试音效，并报告捕获结果。</summary>
    private static SlotId PlayProbe(CommandCaller caller, SoundCaptureSystem capture,
        SoundStyle style, Vector2? position, bool nullableOverload = false)
    {
        int acceptedBefore = capture.Accepted;
        int rejectedBefore = capture.Rejected;
        SoundStyle? optionalStyle = style;
        SlotId slot = nullableOverload
            ? SoundEngine.PlaySound(in optionalStyle, position, null)
            : SoundEngine.PlaySound(in style, position, null);

        int accepted = capture.Accepted - acceptedBefore;
        int rejected = capture.Rejected - rejectedBefore;
        string result = slot.IsValid ? "ValidSlot" : SoundEngine.IsAudioSupported ? "InvalidSlot" : "NoAudio";
        caller.Reply(Text("ProbeResult", Text(result), accepted, rejected, DescribePosition(position)));
        if (accepted > 0 && capture.Latest is { } latest && latest.Playback.Slot.Equals(slot))
        {
            caller.Reply(Text("LastAccepted", latest.Playback.RawKey, DescribePosition(latest.Playback.Position)));
            caller.Reply(Text("Output", latest.OutputVolume, latest.Pan, latest.Pitch));
        }

        return slot;
    }

    /// <summary>触发两次同族声音验证原生实例上限，随后清理探针实例。</summary>
    private static void ProbeInstanceLimit(CommandCaller caller, SoundCaptureSystem capture, Vector2 center)
    {
        var style = SoundID.Item1 with
        {
            Identifier = "SoundCaptions/InstanceLimitProbe",
            MaxInstances = 1,
            LimitsArePerVariant = false,
            SoundLimitBehavior = SoundLimitBehavior.IgnoreNew,
            IsLooped = true
        };
        SlotId first = PlayProbe(caller, capture, style, center);
        try
        {
            PlayProbe(caller, capture, style, center);
        }
        finally
        {
            if (SoundEngine.TryGetActiveSound(first, out ActiveSound active))
                active.Stop();
        }
    }
}
