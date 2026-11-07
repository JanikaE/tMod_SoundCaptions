using System;
using Microsoft.Xna.Framework;
using SoundCaptions.Common.Config;

namespace SoundCaptions.Common.Captions;

/// <summary>在屏幕尺寸、边距和配置之间换算字幕窗口的几何位置。</summary>
internal static class CaptionLayout
{
    private const int Margin = 24;

    /// <summary>根据屏幕尺寸及归一化锚点计算可见字幕窗口。</summary>
    internal static Rectangle GetBounds(int screenWidth, int screenHeight, CaptionLayoutConfig config)
    {
        (int x, int width) = ResolveAxis(screenWidth, config.WindowWidth, config.HorizontalPosition,
            CaptionLayoutConfig.MinimumWidth, CaptionLayoutConfig.MaximumWidth);
        (int y, int height) = ResolveAxis(screenHeight, config.WindowHeight, config.VerticalPosition,
            CaptionLayoutConfig.MinimumHeight, CaptionLayoutConfig.MaximumHeight);
        return new Rectangle(x, y, width, height);
    }

    /// <summary>根据拖动距离和操作边将布局变动写入草稿。</summary>
    internal static void ApplyDrag(CaptionLayoutConfig preview, CaptionLayoutConfig original, Rectangle start, Point delta,
        Point screen, Point resizeEdges)
    {
        (preview.HorizontalPosition, preview.WindowWidth) = EditAxis(screen.X, start.X, start.Width, delta.X,
            original.HorizontalPosition, original.WindowWidth, CaptionLayoutConfig.MinimumWidth,
            CaptionLayoutConfig.MaximumWidth, resizeEdges.X);
        (preview.VerticalPosition, preview.WindowHeight) = EditAxis(screen.Y, start.Y, start.Height, delta.Y,
            original.VerticalPosition, original.WindowHeight, CaptionLayoutConfig.MinimumHeight,
            CaptionLayoutConfig.MaximumHeight, resizeEdges.Y);
    }

    /// <summary>在单一坐标轴上计算移动或缩放后的锚点和尺寸。</summary>
    private static (float Position, int Size) EditAxis(int screen, int origin, int extent, int delta,
        float position, int requested, int minimum, int maximum, int resizeEdge)
    {
        if (delta == 0)
            return (position, requested);

        int margin = GetMargin(screen, minimum);
        int available = Math.Max(0, screen - 2 * margin);
        if (resizeEdge != 0)
        {
            int fixedEdge = resizeEdge < 0 ? origin + extent : origin;
            int space = resizeEdge < 0 ? fixedEdge - margin : screen - margin - fixedEdge;
            int upper = Math.Min(maximum, Math.Max(0, space));
            int newExtent = Math.Clamp(extent + delta * resizeEdge, Math.Min(minimum, upper), upper);
            if (newExtent != extent)
                requested = newExtent;
            if (resizeEdge < 0)
                origin = fixedEdge - newExtent;
            extent = newExtent;
        }
        else
        {
            origin += delta;
        }

        int travel = available - extent;
        return (travel > 0 ? Math.Clamp((origin - margin) / (float)travel, 0f, 1f) : position, requested);
    }

    /// <summary>在屏幕空间允许的范围内保留安全边距。</summary>
    private static int GetMargin(int screen, int minimum) => Math.Clamp((screen - minimum) / 2, 0, Margin);

    /// <summary>在单一坐标轴上约束请求尺寸和锚点，得到实际区域。</summary>
    private static (int Origin, int Extent) ResolveAxis(int screen, int requested, float position, int minimum, int maximum)
    {
        screen = Math.Max(0, screen);
        int margin = GetMargin(screen, minimum);
        int available = screen - 2 * margin;
        int extent = Math.Min(Math.Clamp(requested, minimum, maximum), available);
        float anchor = float.IsFinite(position) ? Math.Clamp(position, 0f, 1f) : 1f;
        int origin = margin + (int)MathF.Round((available - extent) * anchor);
        return (origin, extent);
    }
}
