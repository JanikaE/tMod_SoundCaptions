using System;
using System.Diagnostics;
using System.Threading;
using SoundCaptions.Common.Audio;
using SoundCaptions.Common.Captions;
using SoundCaptions.Common.Config;
using Terraria;
using Terraria.ModLoader;
using static SoundCaptions.Common.Audio.SoundCaptureSystem;

namespace SoundCaptions.Common.Diagnostics;

/// <summary>在诊断模式下运行客户端基线与字幕压力测量。</summary>
[Autoload(Side = ModSide.Client)]
public sealed class CaptionStressSystem : ModSystem
{
    private const int DurationSeconds = 120;
    private const int RepeatedPerUpdate = 32;
    private const int BatchSize = 48;
    private const int BatchInterval = 15;
    private const double MiB = 1024 * 1024;

    private Measurement current;
    private string lastResult;
    private int worldResetPending;

    public override void OnWorldLoad() => Interlocked.Exchange(ref worldResetPending, 1);
    public override void OnWorldUnload() => Interlocked.Exchange(ref worldResetPending, 1);
    public override void Unload() => Reset();

    /// <summary>在满足运行条件时开始一轮基线或压力测量。</summary>
    internal string Start(bool load)
    {
        ApplyWorldReset();
        if (current != null)
            return Text("StressAlreadyRunning");

        if (Main.gameMenu)
            return Text("StressNeedsWorld");

        if (load && !ModContent.GetInstance<ClientConfig>().ShowSoundCaptions)
            return Text("CaptionsDisabled");

        current = new Measurement(load);
        lastResult = null;
        return Text(load ? "StressLoadStarted" : "StressBaselineStarted", DurationSeconds);
    }

    /// <summary>显示当前测量的指标，或返回最近一次结果。</summary>
    internal string Status()
    {
        ApplyWorldReset();
        return current?.Report() ?? lastResult ?? Text("StressIdle");
    }

    /// <summary>停止当前测量并返回统计结果。</summary>
    internal string Stop()
    {
        ApplyWorldReset();
        if (current == null)
            return Text("StressIdle");

        return Finish();
    }

    public override void PostUpdateEverything()
    {
        ApplyWorldReset();
        if (current is not { } measurement)
            return;

        if (Main.gameMenu || !DiagnosticsEnabled ||
            measurement.Load && !ModContent.GetInstance<ClientConfig>().ShowSoundCaptions)
        {
            Reset();
            return;
        }

        long now = Stopwatch.GetTimestamp();
        measurement.Sample(now);
        var captions = ModContent.GetInstance<SoundCaptionSystem>();
        if (measurement.Elapsed(now) >= DurationSeconds)
        {
            Main.NewText(Finish());
            return;
        }

        if (measurement.Load)
        {
            int batch = measurement.Samples % BatchInterval == 0 ? BatchSize : 0;
            captions.RecordDiagnosticStress(RepeatedPerUpdate, batch);
        }

        measurement.PeakCaptions = Math.Max(measurement.PeakCaptions, captions.ActiveCount);
    }

    /// <summary>生成并缓存本轮统计，然后释放运行状态。</summary>
    private string Finish()
    {
        lastResult = current.Report();
        current = null;
        return lastResult;
    }

    /// <summary>在客户端消费世界切换信号并清理旧测量。</summary>
    private void ApplyWorldReset()
    {
        if (Volatile.Read(ref worldResetPending) != 0 && Interlocked.Exchange(ref worldResetPending, 0) != 0)
            Reset();
    }

    /// <summary>清除当前测量和此前的结果。</summary>
    private void Reset()
    {
        current = null;
        lastResult = null;
    }

    /// <summary>汇总一轮测量的更新间隔、托管堆样本和活动字幕峰值。</summary>
    private sealed class Measurement(bool load)
    {
        private readonly long start = Stopwatch.GetTimestamp();
        private readonly long initialHeap = GC.GetTotalMemory(false);
        private long previous;
        private long totalInterval;
        private long maxInterval;

        internal bool Load { get; } = load;
        internal int Samples { get; private set; }
        internal int PeakCaptions { get; set; }

        /// <summary>将单调时钟的时差换算为测量已经运行的秒数。</summary>
        internal double Elapsed(long now) => (now - start) / (double)Stopwatch.Frequency;

        /// <summary>累加本次更新间隔，并更新记录到的最大间隔。</summary>
        internal void Sample(long now)
        {
            if (previous != 0)
            {
                long interval = now - previous;
                totalInterval += interval;
                maxInterval = Math.Max(maxInterval, interval);
            }

            previous = now;
            Samples++;
        }

        /// <summary>将测量指标格式化为当前语言的诊断报告。</summary>
        internal string Report()
        {
            double toMs = 1000d / Stopwatch.Frequency;
            double average = Samples > 1 ? totalInterval * toMs / (Samples - 1) : 0d;
            return Text("StressReport", Text(Load ? "StressLoad" : "StressBaseline"),
                Elapsed(Stopwatch.GetTimestamp()), Samples, average, maxInterval * toMs,
                initialHeap / MiB, GC.GetTotalMemory(false) / MiB, PeakCaptions);
        }
    }
}
