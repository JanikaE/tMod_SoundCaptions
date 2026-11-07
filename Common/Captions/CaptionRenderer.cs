using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Localization;

namespace SoundCaptions.Common.Captions;

/// <summary>保存用于单次绘制的文字颜色、背景颜色和字号倍率。</summary>
internal readonly record struct CaptionAppearance(Color Text, Color Background, float Scale)
{
    /// <summary>将配置值限制在可用范围并生成绘制外观。</summary>
    internal static CaptionAppearance Create(Color text, Color background, int opacity, int scale)
    {
        float alpha = Math.Clamp(opacity, 0, 100) / 100f;
        return new CaptionAppearance(
            new Color(text.R, text.G, text.B),
            new Color(background.R, background.G, background.B) * alpha,
            Math.Clamp(scale, 50, 200) / 100f);
    }
}

/// <summary>保存排版后的行文本、尺寸和指示符占宽。</summary>
internal readonly record struct CaptionRow(string Text, int Width, int Height, float Indicator)
{
    /// <summary>将已测量的字幕行在可用区域内水平居中并沿底部排列。</summary>
    internal Rectangle GetBounds(Rectangle available) =>
        new(available.Left + (available.Width - Width) / 2, available.Bottom - Height, Width, Height);
}

/// <summary>测量和绘制字幕行、背景及音效方向或音乐标记。</summary>
internal readonly struct CaptionRenderer(SpriteBatch spriteBatch, DynamicSpriteFont font, Texture2D pixel)
{
    private const int PaddingX = 6;
    private const int PaddingY = 2;
    private const int IndicatorWidth = 17;
    internal const int RowGap = 2;
    private const double FadeSeconds = 0.35;

    /// <summary>根据剩余显示时间计算线性渐隐的不透明度。</summary>
    internal static float TimedOpacity(double now, double expiresAt) =>
        (float)Math.Clamp((expiresAt - now) / FadeSeconds, 0d, 1d);

    /// <summary>按音乐字号和上下留白计算预留行高。</summary>
    internal int MusicRowHeight(CaptionAppearance appearance) =>
        (int)MathF.Ceiling(font.LineSpacing * appearance.Scale) + 2 * PaddingY;

    /// <summary>测量可用区域内的字幕行，并按需要换行或省略。</summary>
    internal CaptionRow? MeasureRow(Rectangle available, string label, CaptionAppearance appearance,
        int direction = 0, bool music = false)
    {
        float scale = appearance.Scale;
        float indicator = music || direction != 0 ? IndicatorWidth * scale : 0f;
        float maxWidth = (available.Width - 2 * PaddingX - indicator) / scale;
        float maxHeight = (available.Height - 2 * PaddingY) / scale;
        if (maxWidth <= 0 || maxHeight < font.LineSpacing)
            return null;

        string text = label.Replace('\r', ' ').Replace('\n', ' ');
        Vector2 size = font.MeasureString(text);
        if (size.X > maxWidth)
        {
            text = music ? Ellipsize(text, maxWidth) : Wrap(text, maxWidth);
            size = font.MeasureString(text);
        }

        // 只在可用高度内排入完整文字行。显示空间不足时，较旧字幕仍保留到各自到期。
        int length = text.Length;
        while (size.Y > maxHeight)
        {
            length = text.LastIndexOf('\n', length - 1);
            if (length < 0)
                return null;

            size = font.MeasureString(text[..length]);
        }

        if (length < text.Length)
        {
            int lastLine = text.LastIndexOf('\n', length - 1) + 1;
            text = text[..lastLine] + Ellipsize(text[lastLine..length], maxWidth);
            size = font.MeasureString(text);
        }

        size *= scale;
        int rowHeight = (int)MathF.Ceiling(Math.Max(font.LineSpacing * scale, size.Y)) + 2 * PaddingY;
        if (rowHeight > available.Height)
            return null;

        int rowWidth = (int)MathF.Ceiling(size.X + indicator) + 2 * PaddingX;
        return new CaptionRow(text, rowWidth, rowHeight, indicator);
    }

    /// <summary>排版并绘制字幕，返回包含行间距的占用高度。</summary>
    internal int DrawRow(Rectangle available, string label, CaptionAppearance appearance,
        int direction = 0, bool music = false, float opacity = 1f)
    {
        if (MeasureRow(available, label, appearance, direction, music) is not CaptionRow row)
            return 0;

        DrawMeasuredRow(available, row, appearance, direction, music, opacity);
        return row.Height + RowGap;
    }

    /// <summary>用已有排版结果绘制字幕、背景和指示符。</summary>
    internal void DrawMeasuredRow(Rectangle available, CaptionRow row, CaptionAppearance appearance,
        int direction = 0, bool music = false, float opacity = 1f)
    {
        if (opacity <= 0f)
            return;

        float scale = appearance.Scale;
        float indicator = row.Indicator;
        Rectangle bounds = row.GetBounds(available);
        int left = bounds.Left;
        int top = bounds.Top;
        Color background = appearance.Background * opacity;
        if (background.A != 0)
            spriteBatch.Draw(pixel, bounds, background);

        bool indicatorOnLeft = music || direction < 0;
        float textCenter = left + (bounds.Width + (indicatorOnLeft ? indicator : -indicator)) / 2f;
        Color textColor = appearance.Text * opacity;
        float y = top + PaddingY;
        if (!row.Text.Contains('\n'))
        {
            DrawLine(row.Text, textCenter, y, scale, textColor);
        }
        else
        {
            foreach (string line in row.Text.Split('\n'))
            {
                DrawLine(line, textCenter, y, scale, textColor);
                y += font.LineSpacing * scale;
            }
        }

        if (indicator > 0f)
        {
            float indicatorX = indicatorOnLeft ? left + PaddingX : bounds.Right - PaddingX - indicator;
            var center = new Vector2(indicatorX + indicator / 2f, top + bounds.Height / 2f);
            DrawIndicator(center, scale, textColor, direction, music);
        }
    }

    /// <summary>将一行文字按指定中心位置绘制。</summary>
    private void DrawLine(string line, float center, float y, float scale, Color color)
    {
        float x = center - font.MeasureString(line).X * scale / 2f;
        spriteBatch.DrawString(font, line, new Vector2(x, y), color, 0f,
            Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    /// <summary>在可用宽度内折行，并拆分引擎未能折开的超长词。</summary>
    private string Wrap(string text, float width)
    {
        CultureInfo culture = Language.ActiveCulture?.CultureInfo ?? CultureInfo.CurrentCulture;
        string wrapped = font.CreateWrappedText(text, width, culture);
        // 引擎的换行可能保留超宽词。按文本元素拆分可在适配宽度时保留组合字符。
        var lines = new List<string>();
        foreach (string line in wrapped.Split('\n'))
        {
            string remaining = line.TrimEnd();
            while (font.MeasureString(remaining).X > width)
            {
                int prefix = FittingPrefix(remaining, width);
                if (prefix == 0)
                {
                    remaining = Ellipsize(remaining, width);
                    break;
                }

                lines.Add(remaining[..prefix].TrimEnd());
                remaining = remaining[prefix..].TrimStart();
            }

            if (remaining.Length > 0)
                lines.Add(remaining);
        }

        return string.Join('\n', lines);
    }

    /// <summary>将文本裁剪为指定宽度内可显示的带省略号内容。</summary>
    private string Ellipsize(string text, float width)
    {
        const string ellipsis = "…";
        if (font.MeasureString(ellipsis).X > width)
            return string.Empty;

        int length = FittingPrefix(text, width, ellipsis);
        return text[..length].TrimEnd() + ellipsis;
    }

    /// <summary>按文本元素边界寻找能容纳后缀的最长前缀。</summary>
    private int FittingPrefix(string text, float width, string suffix = "")
    {
        int[] offsets = StringInfo.ParseCombiningCharacters(text);
        int low = 0;
        int high = offsets.Length;
        while (low < high)
        {
            int mid = (low + high + 1) / 2;
            int length = mid == offsets.Length ? text.Length : offsets[mid];
            if (font.MeasureString(text[..length] + suffix).X <= width)
                low = mid;
            else
                high = mid - 1;
        }

        return low == offsets.Length ? text.Length : offsets[low];
    }

    /// <summary>绘制音乐标记或指向声源的左右箭头。</summary>
    private void DrawIndicator(Vector2 center, float scale, Color color, int direction, bool music)
    {
        if (music)
        {
            Stroke(center, new Vector2(0f, -5f), new Vector2(0f, 4f), scale, color);
            Stroke(center, new Vector2(0f, -5f), new Vector2(4f, -2f), scale, color);
            spriteBatch.Draw(pixel, center + new Vector2(-4f, 2f) * scale, new Rectangle(0, 0, 1, 1),
                color, 0f, Vector2.Zero, new Vector2(5f, 4f) * scale, SpriteEffects.None, 0f);
            return;
        }

        var tip = new Vector2(5f * direction, 0f);
        Stroke(center, tip, -tip, scale, color);
        Stroke(center, tip, new Vector2(direction, -4f), scale, color);
        Stroke(center, tip, new Vector2(direction, 4f), scale, color);
    }

    /// <summary>按指定缩放、颜色和端点绘制一段指示符线条。</summary>
    private void Stroke(Vector2 origin, Vector2 start, Vector2 end, float scale, Color color)
    {
        Vector2 offset = end - start;
        spriteBatch.Draw(pixel, origin + start * scale, new Rectangle(0, 0, 1, 1), color,
            MathF.Atan2(offset.Y, offset.X), new Vector2(0f, 0.5f),
            new Vector2(offset.Length(), 1.5f) * scale, SpriteEffects.None, 0f);
    }
}
