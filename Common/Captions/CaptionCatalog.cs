using System;
using SoundCaptions.Common.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace SoundCaptions.Common.Captions;

/// <summary>按显式覆盖、原版映射和资源回退的优先级解析音效字幕。</summary>
internal static class CaptionCatalog
{
    private static readonly VolumeTargetKey DiagnosticLoop =
        VolumeTargetKey.FromStyle(SoundID.Item1 with { Identifier = SoundVolumeProbeSystem.LoopIdentifier });

    /// <summary>为稳定音效目标选择字幕文案，并标记是否使用回退。</summary>
    internal static CaptionLabel Resolve(VolumeTargetKey target)
    {
        if (CaptionNames.TryGetSound(target, out NameOverride custom))
            return new CaptionLabel(null, null, Override: custom);

        if (VanillaSoundNames.TryGetName(target, out string name))
            return new CaptionLabel(name, null);

        if (target == DiagnosticLoop)
            return new CaptionLabel("DiagnosticLoop", null);

        string path = target.BasePath;
        if (string.IsNullOrWhiteSpace(path))
            return new CaptionLabel("Unknown", null, IsFallback: true);

        if (path == "SoundCaptions/Diagnostic/Stress")
            return new CaptionLabel("DiagnosticStress", null);

        if (path == "SoundCaptions/Diagnostic/Long")
            return new CaptionLabel("DiagnosticLong", null);

        if (path.StartsWith("SoundCaptions/Diagnostic/", StringComparison.Ordinal))
            return new CaptionLabel("DiagnosticEntry", path[(path.LastIndexOf('/') + 1)..]);

        if (path.StartsWith("Terraria/Sounds/", StringComparison.Ordinal))
            return new CaptionLabel("UnmappedVanilla", null, IsFallback: true);

        string title = CaptionNames.FriendlyAssetTitle(path);
        return new CaptionLabel(title == null ? "Unknown" : null, title, IsFallback: true);
    }
}

/// <summary>表示本地化字幕或直接显示文本，并保留回退状态。</summary>
internal readonly record struct CaptionLabel(string LocalizationKey, string Value, bool IsFallback = false,
    NameOverride? Override = null)
{
    internal string Text => Override?.Text ?? (LocalizationKey == null
        ? Value
        : Language.GetTextValue($"Mods.SoundCaptions.Captions.{LocalizationKey}", Value));
}
