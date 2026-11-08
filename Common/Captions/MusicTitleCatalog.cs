using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SoundCaptions.Common.Captions;

/// <summary>在曲目槽位、稳定音乐键和显示名称之间转换。</summary>
internal static class MusicTitleCatalog
{
    private const string VanillaPrefix = "Terraria/Music/";
    private const string TitlePrefix = "Mods.SoundCaptions.MusicTitles.";
    private static readonly FieldInfo MusicPaths = typeof(MusicLoader).GetField("musicByPath",
        BindingFlags.Static | BindingFlags.NonPublic);

    /// <summary>将有效背景音乐槽位转换为可保存的稳定曲目键。</summary>
    internal static string ResolveKey(int slot)
    {
        if (slot <= 0 || slot == MusicID.RainSoundEffect)
            return null;

        if (MusicID.Search.TryGetName(slot, out string name))
            return slot < MusicID.Count ? VanillaPrefix + name : name;

        // 加载音乐后重建 MusicID 的静态成员会清掉搜索表里的模组条目，注册表仍保留完整资源路径。
        if (slot >= MusicID.Count && MusicPaths?.GetValue(null) is Dictionary<string, int> paths)
        {
            foreach (var pair in paths)
            {
                if (pair.Value == slot)
                    return pair.Key;
            }
        }

        return null;
    }

    /// <summary>用稳定曲目键查询当前加载会话中的音乐槽位。</summary>
    internal static int ResolveSlot(string key)
    {
        bool vanilla = key.StartsWith(VanillaPrefix, StringComparison.Ordinal);
        if (!vanilla && MusicLoader.GetMusicSlot(key) is > 0 and int modSlot)
            return modSlot;

        string name = vanilla ? key[VanillaPrefix.Length..] : key;
        return MusicID.Search.TryGetId(name, out int slot) && slot != MusicID.RainSoundEffect ? slot : 0;
    }

    /// <summary>按兼容覆盖、本地化原版曲名和可读资源名解析显示标题。</summary>
    internal static string GetTitle(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Language.GetTextValue(TitlePrefix + "Unknown");

        if (CaptionNames.TryGetMusic(key, out NameOverride custom))
            return custom.Text;

        if (key.StartsWith(VanillaPrefix, StringComparison.Ordinal))
        {
            string localizedKey = TitlePrefix + key[VanillaPrefix.Length..];
            return Language.GetTextValue(Language.Exists(localizedKey) ? localizedKey : TitlePrefix + "UnknownVanilla");
        }

        if (key == "SoundCaptions/Music/DiagnosticLoop")
            return Language.GetTextValue(TitlePrefix + "DiagnosticLoop");

        return CaptionNames.FriendlyAssetTitle(key) ?? Language.GetTextValue(TitlePrefix + "Unknown");
    }
}
