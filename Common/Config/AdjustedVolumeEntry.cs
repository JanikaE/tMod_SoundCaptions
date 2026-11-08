using System;
using System.Collections.Generic;
using SoundCaptions.Common.Audio;
using SoundCaptions.Common.Captions;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SoundCaptions.Common.Config;

/// <summary>表示集中管理列表中的一个非默认音量目标及其可用状态。</summary>
internal readonly record struct AdjustedVolumeEntry(string Title, string Owner, string StableKey,
    bool Available, int Percent, VolumeTargetKey? SoundKey, string MusicKey)
{
    /// <summary>生成指定分组的已调整音量列表，按查询筛选并稳定排序。</summary>
    internal static List<AdjustedVolumeEntry> Search(ClientConfig config, bool music, string query)
    {
        var entries = new List<AdjustedVolumeEntry>();
        if (music)
        {
            foreach (var pair in config.MusicVolumes)
            {
                if (string.IsNullOrWhiteSpace(pair.Key) || pair.Value == ClientConfig.DefaultVolumePercent)
                    continue;

                entries.Add(new AdjustedVolumeEntry(MusicTitleCatalog.GetTitle(pair.Key), OwnerOf(pair.Key),
                    pair.Key, MusicTitleCatalog.ResolveSlot(pair.Key) > 0, Math.Clamp(pair.Value, 0, ClientConfig.MaxVolumePercent), null, pair.Key));
            }
        }
        else
        {
            foreach (SoundVolumeOverride record in config.SoundVolumes)
            {
                VolumeTargetKey key = record.Target;
                if (string.IsNullOrWhiteSpace(key.BasePath) || record.Percent == ClientConfig.DefaultVolumePercent)
                    continue;

                entries.Add(new AdjustedVolumeEntry(CaptionCatalog.Resolve(key).Text, OwnerOf(key.BasePath),
                    key.ToString(), SoundSourceAvailable(key.BasePath), Math.Clamp(record.Percent, 0, ClientConfig.MaxVolumePercent), key, null));
            }
        }

        if (!string.IsNullOrWhiteSpace(query))
        {
            query = query.Trim();
            entries.RemoveAll(entry => !entry.Matches(query));
        }

        entries.Sort(static (left, right) =>
        {
            int title = StringComparer.CurrentCultureIgnoreCase.Compare(left.Title, right.Title);
            return title != 0 ? title : StringComparer.Ordinal.Compare(left.StableKey, right.StableKey);
        });
        return entries;
    }

    /// <summary>把该行的音量写入待保存的配置草稿。</summary>
    internal void SetPercent(ClientConfig draft, int percent)
    {
        if (SoundKey is VolumeTargetKey sound)
            draft.SetSoundVolume(sound, percent);
        else
            draft.SetMusicVolume(MusicKey, percent);
    }

    /// <summary>判断名称、来源或完整稳定键是否包含搜索文本。</summary>
    private bool Matches(string query) =>
        Title.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        Owner.Contains(query, StringComparison.CurrentCultureIgnoreCase) ||
        StableKey.Contains(query, StringComparison.OrdinalIgnoreCase);

    /// <summary>根据音效路径检查原版或所属模组当前是否可用。</summary>
    private static bool SoundSourceAvailable(string path)
    {
        int slash = path.IndexOf('/');
        return slash > 0 && (path[..slash] == "Terraria" || ModLoader.TryGetMod(path[..slash], out _));
    }

    /// <summary>从资源路径提取来源，并优先显示已加载模组的显示名。</summary>
    private static string OwnerOf(string path) =>
        CaptionNames.AssetOwner(path) ?? Language.GetTextValue("Mods.SoundCaptions.VolumeManager.UnknownSource");
}
