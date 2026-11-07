using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using SoundCaptions.Common.Audio;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SoundCaptions.Common.Captions;

/// <summary>管理兼容模组注册的显示名称及无专名时的资源文案。</summary>
internal static class CaptionNames
{
    private static readonly ConcurrentDictionary<VolumeTargetKey, NameOverride> Sound = new();
    private static readonly ConcurrentDictionary<string, NameOverride> Music = new(StringComparer.Ordinal);
    private static readonly Regex Separators = new(@"[_-]+|(?<=[a-z0-9])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])",
        RegexOptions.CultureInvariant);

    /// <summary>为精确匹配的音效目标注册首个显示标题。</summary>
    internal static bool RegisterSound(SoundStyle style, NameOverride title) =>
        !string.IsNullOrWhiteSpace(style.SoundPath) && Sound.TryAdd(VolumeTargetKey.FromStyle(style), title);

    /// <summary>为格式有效的稳定曲目路径注册首个显示标题。</summary>
    internal static bool RegisterMusic(string key, NameOverride title)
    {
        int slash = key.IndexOf('/');
        return slash > 0 && slash < key.Length - 1 && !key.Contains('\\') &&
            Music.TryAdd(key, title);
    }

    /// <summary>按稳定音效键查询已注册的标题。</summary>
    internal static bool TryGetSound(VolumeTargetKey key, out NameOverride title) => Sound.TryGetValue(key, out title);
    /// <summary>按稳定曲目键查询已注册的标题。</summary>
    internal static bool TryGetMusic(string key, out NameOverride title) => Music.TryGetValue(key, out title);

    /// <summary>卸载时清空兼容注册，避免下一次加载继承旧模组文本。</summary>
    internal static void Clear()
    {
        Sound.Clear();
        Music.Clear();
    }

    /// <summary>用来源模组和可读的资源文件名生成回退标题。</summary>
    internal static string FriendlyAssetTitle(string path)
    {
        string leaf = path[(path.LastIndexOf('/') + 1)..];
        string resource = Separators.Replace(leaf, " ").Trim();
        if (string.IsNullOrWhiteSpace(resource))
            return null;

        int separator = path.IndexOf('/');
        if (separator <= 0)
            return resource;

        string owner = path[..separator];
        string displayName = ModLoader.TryGetMod(owner, out Mod mod) ? mod.DisplayName : owner;
        return $"{displayName} · {resource}";
    }
}

/// <summary>保存兼容模组提供的直接文字或随语言变化的本地化文本。</summary>
internal readonly record struct NameOverride(string Literal, LocalizedText Localized)
{
    internal string Text => Localized?.Value ?? Literal;

    /// <summary>将非空字符串或有效本地化文本转换为显示覆盖项。</summary>
    internal static bool TryCreate(object value, out NameOverride title)
    {
        title = value switch
        {
            string literal => new NameOverride(literal.Trim(), null),
            LocalizedText localized => new NameOverride(null, localized),
            _ => default
        };

        return !string.IsNullOrWhiteSpace(title.Text);
    }
}
