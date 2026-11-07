using SoundCaptions.Common.Config;

namespace SoundCaptions.Common.Captions;

/// <summary>跟踪背景音乐的当前曲目、短显时限和无曲目确认状态。</summary>
internal sealed class MusicCaptionState
{
    private const double MissingConfirmationSeconds = 0.25;
    private double? missingSince;
    private double visibleUntil;
    private bool wasEnabled;
    private MusicDisplayMode previousMode;

    internal string CurrentKey { get; private set; }
    internal string VisibleKey { get; private set; }
    internal int SelectionChanges { get; private set; }

    /// <summary>根据观测到的稳定曲目键和显示模式更新字幕与切曲计数。</summary>
    internal void Update(string observedKey, double now, bool enabled, MusicDisplayMode mode, double duration)
    {
        string previousKey = CurrentKey;
        if (observedKey != null)
        {
            missingSince = null;
            CurrentKey = observedKey;
        }
        else if (CurrentKey != null)
        {
            // 切曲和淡化可能出现短暂无曲目。等待确认可避免同一首歌的字幕被重新触发。
            missingSince ??= now;
            if (now >= missingSince.Value + MissingConfirmationSeconds)
                CurrentKey = null;
        }

        // 使用稳定曲目键判定切曲，因此同一曲目的循环或音量变化不会重复显示短字幕。
        bool changed = CurrentKey != previousKey;
        if (changed)
            SelectionChanges++;

        if (CurrentKey != null && (changed || (enabled && !wasEnabled) || mode != previousMode))
            visibleUntil = now + duration;

        VisibleKey = enabled && (mode == MusicDisplayMode.Always || now < visibleUntil) ? CurrentKey : null;
        wasEnabled = enabled;
        previousMode = mode;
    }

    /// <summary>返回当前音乐字幕的不透明度，持续模式保持全亮。</summary>
    internal float Opacity(double now)
    {
        if (VisibleKey == null)
            return 0f;

        return previousMode == MusicDisplayMode.Always ? 1f : CaptionRenderer.TimedOpacity(now, visibleUntil);
    }

    /// <summary>清除曲目、显示状态和切曲计数。</summary>
    internal void Clear()
    {
        CurrentKey = VisibleKey = null;
        SelectionChanges = 0;
        missingSince = null;
        visibleUntil = 0;
        wasEnabled = false;
        previousMode = MusicDisplayMode.OnTrackChange;
    }
}
