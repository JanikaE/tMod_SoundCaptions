using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Audio;

namespace SoundCaptions.Common.Audio;

/// <summary>表示本次播放实际选中的音频资源路径。</summary>
internal readonly record struct RawSoundKey(string AssetPath)
{
    public override string ToString() => AssetPath;
}

/// <summary>以基础路径及标识符或变体集合表示稳定的音量目标。</summary>
internal readonly record struct VolumeTargetKey(string BasePath, string Identifier, string VariantFamily)
{
    /// <summary>从声音样式生成与播放参数及随机变体选择无关的目标键。</summary>
    internal static VolumeTargetKey FromStyle(in SoundStyle style)
    {
        string family = null;
        if (style.Identifier == null && !style.Variants.IsEmpty)
        {
            // 变体顺序与权重只影响抽样。排序后使用后缀集合，已有音量设置就不会因抽样参数改变而失效。
            int[] suffixes = style.Variants.ToArray();
            Array.Sort(suffixes);
            family = string.Join(",", Array.ConvertAll(suffixes, suffix => suffix.ToString(CultureInfo.InvariantCulture)));
        }

        return new VolumeTargetKey(style.SoundPath, style.Identifier, family);
    }
    public override string ToString()
    {
        string path = VariantFamily == null ? BasePath : $"{BasePath}[{VariantFamily}]";
        return Identifier == null ? path : $"{path} ({Identifier})";
    }
}

/// <summary>表示用于合并和排序字幕的声音族身份。</summary>
internal readonly record struct CaptionKey(VolumeTargetKey Family)
{
    public override string ToString() => Family.ToString();
}

/// <summary>保存已接受播放的实例句柄、声音身份、位置和循环状态。</summary>
internal readonly record struct SoundPlayback(
    SlotId Slot, RawSoundKey RawKey, CaptionKey CaptionKey, VolumeTargetKey VolumeKey,
    Vector2? Position, bool IsLooped)
{
    /// <summary>根据实际播放样式提取资源键、字幕键和音量目标。</summary>
    public static SoundPlayback Create(SlotId slot, SoundStyle style, Vector2? position)
    {
        string assetPath = style.SoundPath;
        ReadOnlySpan<int> variants = style.Variants;
        if (!variants.IsEmpty)
        {
            // 引擎保存的是变体索引，文件名使用对应后缀。具体文件只用于诊断，字幕与音量仍归于原声音族。
            if (GetSelectedVariant(ref style) is int index && (uint)index < (uint)variants.Length)
                assetPath += variants[index].ToString(CultureInfo.InvariantCulture);
        }

        var target = VolumeTargetKey.FromStyle(in style);
        return new SoundPlayback(slot, new RawSoundKey(assetPath), new CaptionKey(target), target, position, style.IsLooped);
    }

    /// <summary>读取引擎保存在样式中的已选变体索引，供诊断还原具体资源。</summary>
    // 私有访问器按支持的 tModLoader 版本核对。读取的是引擎已有的选择，避免再次抽样改变诊断结果。
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_SelectedVariant")]
    private static extern int? GetSelectedVariant(ref SoundStyle style);
}
