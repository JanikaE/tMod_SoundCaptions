using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SoundCaptions.Common.Config;

/// <summary>单独保存客户端字幕窗口的大小与归一化位置。</summary>
public sealed class CaptionLayoutConfig : ModConfig
{
    internal const int DefaultWidth = 360;
    internal const int DefaultHeight = 240;
    internal const int MinimumWidth = 96;
    internal const int MinimumHeight = 64;
    internal const int MaximumWidth = 1600;
    internal const int MaximumHeight = 1200;

    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(1f)]
    [Range(0f, 1f)]
    [Increment(0.01f)]
    public float HorizontalPosition { get; set; } = 1f;

    [DefaultValue(1f)]
    [Range(0f, 1f)]
    [Increment(0.01f)]
    public float VerticalPosition { get; set; } = 1f;

    [DefaultValue(DefaultWidth)]
    [Range(MinimumWidth, MaximumWidth)]
    public int WindowWidth { get; set; } = DefaultWidth;

    [DefaultValue(DefaultHeight)]
    [Range(MinimumHeight, MaximumHeight)]
    public int WindowHeight { get; set; } = DefaultHeight;
}
