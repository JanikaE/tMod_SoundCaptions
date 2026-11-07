using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using SoundCaptions.Common.Audio;
using Terraria.Audio;

namespace SoundCaptions.Common.Captions;

/// <summary>维护以声音族去重的有效字幕索引及按最近播放排列的顺序。</summary>
internal sealed class CaptionStore
{
    private readonly Dictionary<CaptionKey, LinkedListNode<CaptionEntry>> entries = new();
    private readonly LinkedList<CaptionEntry> order = new();

    internal int Count => entries.Count;
    internal LinkedListNode<CaptionEntry> Newest => order.Last;

    /// <summary>新增或刷新声音族的字幕，并更新位置、到期时间和排序。</summary>
    internal void Record(SoundPlayback playback, SoundPlayer player, double now, double duration)
    {
        // 以声音族键去重，语义文案相同的独立音量目标仍分别保留。
        if (!entries.TryGetValue(playback.CaptionKey, out var node))
        {
            node = new LinkedListNode<CaptionEntry>(new CaptionEntry(playback.CaptionKey, CaptionCatalog.Resolve(playback.VolumeKey)));
            entries.Add(playback.CaptionKey, node);
        }
        else
        {
            // 移动原节点更新最近播放顺序，重复触发时无需再分配一行。
            order.Remove(node);
        }

        CaptionEntry entry = node.Value;
        entry.Position = playback.Position;
        entry.ExpiresAt = now + duration;
        entry.FollowsLoop = playback.IsLooped;
        if (playback.IsLooped)
        {
            entry.Loops ??= new List<CaptionLoop>();
            entry.Loops.Add(new CaptionLoop(player, playback.Slot));
        }

        order.AddLast(node);
    }

    /// <summary>刷新循环声音的位置和生命周期，并移除已到期的字幕。</summary>
    internal void Update(double now, double duration)
    {
        for (var node = order.First; node != null;)
        {
            var next = node.Next;
            CaptionEntry entry = node.Value;
            if (entry.Loops != null)
                UpdateLoops(entry, now, duration);

            if (now >= entry.ExpiresAt)
            {
                entries.Remove(entry.Key);
                order.Remove(node);
            }

            node = next;
        }
    }

    /// <summary>保留仍在播放或暂停的循环实例，并按最近有效实例维持字幕。</summary>
    private static void UpdateLoops(CaptionEntry entry, double now, double duration)
    {
        ActiveSound newest = null;
        for (int i = entry.Loops.Count - 1; i >= 0; i--)
        {
            CaptionLoop loop = entry.Loops[i];
            if (loop.Player.TryGetActiveSound(loop.Slot, out ActiveSound active) && active.IsPlayingOrPaused)
                newest ??= active;
            else
                entry.Loops.RemoveAt(i);
        }

        if (newest != null)
        {
            // 同族短声较新时，先保留它的位置和显示期限。到期后再由仍存活的循环接管这一行。
            if (entry.FollowsLoop || now >= entry.ExpiresAt)
            {
                entry.Position = newest.Position;
                entry.ExpiresAt = now + duration;
                entry.FollowsLoop = true;
            }

            return;
        }

        entry.Loops = null;
        if (entry.FollowsLoop)
        {
            entry.FollowsLoop = false;
            entry.ExpiresAt = now + duration;
        }
    }

    /// <summary>清空字幕索引和显示顺序，释放关联循环实例的引用。</summary>
    internal void Clear()
    {
        entries.Clear();
        order.Clear();
    }
}

/// <summary>表示一个声音族的有效字幕及其位置、到期时间和循环实例。</summary>
internal sealed class CaptionEntry(CaptionKey key, CaptionLabel label)
{
    internal CaptionKey Key { get; } = key;
    internal CaptionLabel Label { get; } = label;
    internal Vector2? Position;
    internal double ExpiresAt;
    internal bool FollowsLoop;
    internal List<CaptionLoop> Loops;
}

/// <summary>将循环声音的播放器与实例句柄关联，供更新时检查。</summary>
internal readonly record struct CaptionLoop(SoundPlayer Player, SlotId Slot);

/// <summary>累计排除单人暂停的字幕播放时间。</summary>
internal sealed class CaptionClock
{
    private long? lastSample;
    private bool wasPaused;
    private long elapsed;

    internal double Now => elapsed / (double)Stopwatch.Frequency;

    /// <summary>采样单调时钟，按暂停状态决定是否累计相邻采样的时差。</summary>
    internal void Sample(long timestamp, bool paused)
    {
        if (!paused && !wasPaused && lastSample is long previous)
            elapsed += timestamp - previous;

        lastSample = timestamp;
        wasPaused = paused;
    }

    /// <summary>清空世界内字幕时钟及之前的暂停状态。</summary>
    internal void Reset()
    {
        lastSample = null;
        wasPaused = false;
        elapsed = 0;
    }
}
