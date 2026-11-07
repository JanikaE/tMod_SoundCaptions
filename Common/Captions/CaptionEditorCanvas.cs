using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SoundCaptions.Common.Config;

namespace SoundCaptions.Common.Captions;

/// <summary>保存编辑窗口、标题、手柄、正文和详情面板的统一命中区域。</summary>
internal readonly record struct EditorGeometry(Rectangle Window, Rectangle Header, Rectangle Grip,
    Rectangle Body, Rectangle CurrentTrack, Rectangle Detail, Rectangle DetailClose, Point ResizeEdges)
{
    /// <summary>根据屏幕和字幕窗口确定编辑控件的布局与缩放方向。</summary>
    internal static EditorGeometry Create(Point screen, Rectangle window, bool trackAvailable, int detailHeight, Point? lockedEdges)
    {
        const int headerSize = 25;
        const int gripSize = 15;
        int headerHeight = Math.Min(headerSize, window.Height);
        int footerHeight = Math.Min(headerSize, window.Height - headerHeight);
        int gripWidth = Math.Min(gripSize, window.Width);
        // 手柄放在有扩展空间的一侧，窗口贴近边缘时仍能向屏幕内放大。
        Point edges = lockedEdges ?? new Point(window.Center.X >= screen.X / 2 ? -1 : 1,
            window.Center.Y >= screen.Y / 2 ? -1 : 1);
        bool gripAtTop = edges.Y < 0;
        bool gripAtLeft = edges.X < 0;
        int footerTop = window.Bottom - footerHeight;
        int gripX = gripAtLeft ? window.Left : window.Right - gripWidth;
        var grip = new Rectangle(gripX, gripAtTop ? window.Top : footerTop,
            gripWidth, gripAtTop ? headerHeight : footerHeight);
        var header = new Rectangle(window.X + (gripAtTop && gripAtLeft ? gripWidth : 0),
            window.Y, window.Width - (gripAtTop ? gripWidth : 0), headerHeight);
        var current = trackAvailable
            ? new Rectangle(window.X + (!gripAtTop && gripAtLeft ? gripWidth : 0),
                footerTop, window.Width - (gripAtTop ? 0 : gripWidth), footerHeight)
            : Rectangle.Empty;
        var body = new Rectangle(window.X + 2, header.Bottom + 2, Math.Max(0, window.Width - 4),
            Math.Max(0, footerTop - header.Bottom - 4));
        Rectangle detail = detailHeight > 0 ? DetailBounds(screen, window, detailHeight) : Rectangle.Empty;
        int closeSize = Math.Min(headerSize, Math.Min(detail.Width, detail.Height));
        var close = new Rectangle(detail.Right - closeSize, detail.Y, closeSize, closeSize);
        return new EditorGeometry(window, header, grip, body, current, detail, close, edges);
    }

    /// <summary>在字幕窗旁选择能放入当前屏幕的详情面板位置。</summary>
    private static Rectangle DetailBounds(Point screen, Rectangle window, int desiredHeight)
    {
        const int gap = 4;
        int width = Math.Min(300, screen.X);
        int height = Math.Min(desiredHeight, screen.Y);
        int y = Math.Clamp(window.Top, 0, screen.Y - height);
        if (window.Left >= width + gap)
            return new Rectangle(window.Left - width - gap, y, width, height);
        if (screen.X - window.Right >= width + gap)
            return new Rectangle(window.Right + gap, y, width, height);

        int x = Math.Clamp(window.Left, 0, screen.X - width);
        if (window.Top >= height + gap)
            return new Rectangle(x, window.Top - height - gap, width, height);
        if (screen.Y - window.Bottom >= height + gap)
            return new Rectangle(x, window.Bottom + gap, width, height);

        return new Rectangle(x, y, width, height);
    }
}

/// <summary>保存详情面板中音量滑块、数字输入和操作按钮的区域。</summary>
internal readonly record struct VolumeControls(Rectangle Slider, Rectangle Number, Rectangle Mute, Rectangle Reset)
{
    /// <summary>按详情面板的可用尺寸计算音量控件区域。</summary>
    internal static VolumeControls Create(Rectangle detail)
    {
        const int inset = 6;
        const int numberWidth = 50;
        const int gap = 4;
        const int y = 127;
        const int height = 24;
        if (detail.Width < 120 || detail.Height < y + 2 * height + gap)
            return default;

        int width = detail.Width - 2 * inset;
        var slider = new Rectangle(detail.Left + inset, detail.Top + y, width - numberWidth - gap, height);
        var number = new Rectangle(slider.Right + gap, slider.Y, numberWidth, height);
        int buttonWidth = (width - gap) / 2;
        var mute = new Rectangle(slider.X, slider.Bottom + gap, buttonWidth, height);
        var reset = new Rectangle(mute.Right + gap, mute.Y, width - buttonWidth - gap, height);
        return new VolumeControls(slider, number, mute, reset);
    }

    /// <summary>将鼠标横坐标换算为滑块上的整数音量百分比。</summary>
    internal static int PercentAt(Rectangle slider, int mouseX) =>
        (int)MathF.Round(Math.Clamp((mouseX - slider.Left - 7) / (float)(slider.Width - 14), 0f, 1f) * ClientConfig.MaxVolumePercent);

    /// <summary>将音量百分比换算为滑块柄的横坐标。</summary>
    internal static int ThumbX(Rectangle slider, int percent) =>
        slider.Left + 7 + (int)MathF.Round((slider.Width - 14) * (percent / (float)ClientConfig.MaxVolumePercent));
}

/// <summary>绘制编辑器的边框、按钮、滑块和详情文字。</summary>
internal readonly struct CaptionEditorCanvas(SpriteBatch batch, DynamicSpriteFont font, Texture2D pixel)
{
    private static readonly Color Outline = new(130, 174, 227);
    private static readonly Color Panel = new(24, 37, 62);
    private static readonly Color Control = new(48, 73, 112);
    private static readonly CaptionAppearance DetailAppearance = new(Color.White, Color.Transparent, 0.7f);

    internal CaptionRenderer Renderer => new(batch, font, pixel);

    /// <summary>绘制带边框的编辑面板背景。</summary>
    internal void Frame(Rectangle bounds)
    {
        Fill(bounds, Panel * 0.93f);
        Border(bounds, Outline);
    }

    /// <summary>绘制一个有背景和居中文案的按钮。</summary>
    internal void Button(Rectangle bounds, string label)
    {
        Fill(bounds, Control);
        Renderer.DrawRow(bounds, label, DetailAppearance, music: false);
    }

    /// <summary>绘制选中状态的轮廓。</summary>
    internal void Highlight(Rectangle bounds) => Border(bounds, Color.Gold);

    /// <summary>绘制滑轨及当前百分比对应的滑块柄。</summary>
    internal void Slider(Rectangle bounds, int percent)
    {
        Fill(bounds, Control);
        int x = VolumeControls.ThumbX(bounds, percent);
        Fill(new Rectangle(bounds.Left + 7, bounds.Center.Y - 1, bounds.Width - 14, 3), Outline);
        Fill(new Rectangle(x - 3, bounds.Center.Y - 7, 7, 14), Color.White);
    }

    /// <summary>绘制可用于改变窗口尺寸的角落手柄。</summary>
    internal void Grip(Rectangle bounds)
    {
        Fill(bounds, Control);
        for (int i = 0; i < 3; i++)
        {
            int width = 4 + i * 3;
            if (bounds.Width > width + 2 && bounds.Height > 4 + i * 3)
                Fill(new Rectangle(bounds.Right - width - 2, bounds.Bottom - 4 - (2 - i) * 3, width, 1), Outline);
        }
    }

    /// <summary>在详情面板的可用空间内绘制文本。</summary>
    internal void DetailText(Rectangle panel, int y, int height, string text)
    {
        int available = Math.Min(height, panel.Bottom - y);
        if (available > 0)
            Renderer.DrawRow(new Rectangle(panel.Left, y, panel.Width, available), text, DetailAppearance);
    }

    /// <summary>沿区域四边绘制边框。</summary>
    private void Border(Rectangle bounds, Color color)
    {
        Fill(new Rectangle(bounds.Left, bounds.Top, bounds.Width, 1), color);
        Fill(new Rectangle(bounds.Left, bounds.Bottom - 1, bounds.Width, 1), color);
        Fill(new Rectangle(bounds.Left, bounds.Top, 1, bounds.Height), color);
        Fill(new Rectangle(bounds.Right - 1, bounds.Top, 1, bounds.Height), color);
    }

    /// <summary>填充一个有效的编辑器区域。</summary>
    private void Fill(Rectangle bounds, Color color)
    {
        if (bounds.Width > 0 && bounds.Height > 0)
            batch.Draw(pixel, bounds, color);
    }
}
