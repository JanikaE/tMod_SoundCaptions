using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundCaptions.Common.Captions;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace SoundCaptions.Common.Config;

/// <summary>在原生客户端配置页管理并恢复已保存的音量调整。</summary>
public sealed class AdjustedVolumesElement : ConfigElement
{
    private const int PageSize = 5;
    private const int RowHeight = 36;
    private const int RowsTop = 95;
    private const int FooterTop = RowsTop + PageSize * RowHeight + 4;
    private static readonly Color ButtonColor = new(56, 79, 138);
    private static readonly Color SelectedColor = new(76, 110, 170);

    private ClientConfig Draft => (ClientConfig)Item;
    private UIElement rows;
    private UIElement footer;
    private UISearchBar search;
    private UIPanel soundTab;
    private UIPanel musicTab;
    private bool music;
    private bool refresh = true;
    private bool confirmReset;
    private int page;
    private string query = "";
    private string hoveredDetails;

    public override void OnBind()
    {
        base.OnBind();
        Height.Set(FooterTop + 68, 0f);
        TooltipFunction = () => hoveredDetails ?? Text("Help");

        soundTab = Button(this, Text("Sound"), 32, 0.02f, 0.47f, () => SelectGroup(false));
        musicTab = Button(this, Text("Music"), 32, 0.51f, 0.47f, () => SelectGroup(true));

        var searchFrame = new UIPanel { BackgroundColor = ButtonColor, BorderColor = ButtonColor };
        searchFrame.SetPadding(4);
        searchFrame.Left.Set(0, 0.02f);
        searchFrame.Width.Set(0, 0.96f);
        searchFrame.Top.Set(62, 0);
        searchFrame.Height.Set(29, 0);
        Append(searchFrame);
        search = new UISearchBar(Language.GetText("Mods.SoundCaptions.VolumeManager.Search"), 0.76f);
        search.Width.Set(0, 1f);
        search.Height.Set(0, 1f);
        search.OnLeftMouseDown += (_, _) => search.ToggleTakingText();
        search.OnContentsChanged += contents =>
        {
            query = contents ?? "";
            page = 0;
            refresh = true;
        };
        searchFrame.Append(search);

        rows = new UIElement();
        rows.Top.Set(RowsTop, 0);
        rows.Height.Set(PageSize * RowHeight, 0);
        rows.Width.Set(0, 1);
        Append(rows);

        footer = new UIElement();
        footer.Top.Set(FooterTop, 0);
        footer.Height.Set(64, 0);
        footer.Width.Set(0, 1);
        Append(footer);
    }

    public override void Update(GameTime gameTime)
    {
        if (refresh)
            Rebuild();

        base.Update(gameTime);
    }

    /// <summary>切换音效或音乐分组并回到第一页。</summary>
    private void SelectGroup(bool showMusic)
    {
        StopSearch();
        music = showMusic;
        page = 0;
        refresh = true;
    }

    /// <summary>按当前搜索、页码和确认状态刷新列表与操作区。</summary>
    private void Rebuild()
    {
        refresh = false;
        hoveredDetails = null;
        rows.RemoveAllChildren();
        footer.RemoveAllChildren();
        soundTab.BackgroundColor = music ? ButtonColor : SelectedColor;
        musicTab.BackgroundColor = music ? SelectedColor : ButtonColor;

        List<AdjustedVolumeEntry> matches = AdjustedVolumeEntry.Search(Draft, music, query);
        int pages = Math.Max(1, (matches.Count + PageSize - 1) / PageSize);
        page = Math.Clamp(page, 0, pages - 1);
        int first = page * PageSize;
        int last = Math.Min(matches.Count, first + PageSize);
        if (first == last)
            AddLabel(rows, Text("Empty"), 16, 0.03f, 0.94f);

        for (int i = first; i < last; i++)
            AddRow(matches[i], (i - first) * RowHeight);

        Button(footer, "‹", 0, 0.02f, 0.16f, () => MovePage(-1));
        AddLabel(footer, Language.GetTextValue("Mods.SoundCaptions.VolumeManager.Page", page + 1, pages, matches.Count),
            5, 0.2f, 0.60f);
        Button(footer, "›", 0, 0.82f, 0.16f, () => MovePage(1));

        if (confirmReset)
        {
            Button(footer, Text("ConfirmReset"), 31, 0.02f, 0.58f, ResetAll);
            Button(footer, Text("CancelReset"), 31, 0.62f, 0.36f, () =>
            {
                confirmReset = false;
                refresh = true;
            });
        }
        else
        {
            Button(footer, Text("ResetAll"), 31, 0.02f, 0.96f, () =>
            {
                StopSearch();
                confirmReset = true;
                refresh = true;
            });
        }

        Recalculate();
    }

    /// <summary>添加包含来源、稳定键、百分比和恢复操作的单行控件。</summary>
    private void AddRow(AdjustedVolumeEntry entry, int top)
    {
        var identity = new EntryIdentity(entry);
        identity.Left.Set(0, 0.02f);
        identity.Width.Set(0, 0.45f);
        identity.Top.Set(top, 0);
        identity.Height.Set(32, 0);
        identity.OnLeftClick += (_, _) => StopSearch();
        rows.Append(identity);
        string details = $"{entry.Title}\n{entry.Owner}\n{entry.StableKey}";
        identity.OnMouseOver += (_, _) => hoveredDetails = details;
        identity.OnMouseOut += (_, _) => hoveredDetails = null;
        var slider = new VolumeDraftSlider(entry.Percent, percent => Change(entry, percent));
        slider.Left.Set(0, 0.49f);
        slider.Width.Set(0, 0.28f);
        slider.Top.Set(top, 0);
        slider.Height.Set(32, 0);
        slider.OnLeftMouseDown += (_, _) => StopSearch();
        rows.Append(slider);
        Button(rows, Text("Reset"), top, 0.79f, 0.19f, () => Change(entry, ClientConfig.DefaultVolumePercent), 32);
    }

    /// <summary>按指定方向切换页码，实际范围在重建时约束。</summary>
    private void MovePage(int delta)
    {
        StopSearch();
        page += delta;
        refresh = true;
    }

    /// <summary>将用户修改写入配置草稿并通知原生界面有待保存变更。</summary>
    private void Change(AdjustedVolumeEntry entry, int percent)
    {
        StopSearch();
        if (percent == entry.Percent)
            return;

        entry.SetPercent(Draft, percent);
        MarkPending();
    }

    /// <summary>在用户确认后清除草稿里的音效和音乐覆盖。</summary>
    private void ResetAll()
    {
        confirmReset = false;
        Draft.ClearVolumeOverrides();
        MarkPending();
    }

    /// <summary>通知原生配置界面启用保存，并安排重新生成列表。</summary>
    private void MarkPending()
    {
        // 通过 SetObject 通知原生界面草稿已变动，沿用它的保存与撤销流程。
        SetObject(Draft.MusicVolumes);
        refresh = true;
    }

    /// <summary>在处理其他控件操作前结束搜索框的文字输入。</summary>
    private void StopSearch()
    {
        if (search.IsWritingText)
            search.ToggleTakingText();
    }

    /// <summary>创建一个文字居中、点击可执行指定操作的面板按钮。</summary>
    private static UIPanel Button(UIElement parent, string text, float top, float left,
        float width, Action action, int height = 27)
    {
        var button = new UIPanel();
        button.SetPadding(1);
        button.Left.Set(0, left);
        button.Width.Set(0, width);
        button.Top.Set(top, 0);
        button.Height.Set(height, 0);
        button.BackgroundColor = ButtonColor;
        button.OverflowHidden = true;

        var label = new UIText(text, 0.72f)
        {
            HAlign = 0.5f,
            TextOriginY = 0.5f,
            IgnoresMouseInteraction = true
        };
        label.Width.Set(0, 1f);
        label.Height.Set(0, 1f);
        button.Append(label);

        button.OnLeftClick += (_, _) => action();
        parent.Append(button);
        return button;
    }

    /// <summary>向指定区域添加不接收鼠标交互的静态标签。</summary>
    private static void AddLabel(UIElement parent, string text, float top, float left, float width)
    {
        var label = new UIText(text, 0.72f);
        label.Left.Set(0, left);
        label.Width.Set(0, width);
        label.Top.Set(top, 0);
        label.IgnoresMouseInteraction = true;
        parent.Append(label);
    }

    /// <summary>获取音量管理界面的本地化文字。</summary>
    private static string Text(string key) => Language.GetTextValue($"Mods.SoundCaptions.VolumeManager.{key}");

    /// <summary>绘制音量目标的名称和来源，并区分当前不可用的记录。</summary>
    private sealed class EntryIdentity(AdjustedVolumeEntry entry) : UIElement
    {
        protected override void DrawSelf(SpriteBatch batch)
        {
            Rectangle bounds = GetDimensions().ToRectangle();
            if (bounds.Width <= 8)
                return;

            batch.Draw(TextureAssets.MagicPixel.Value, bounds, ButtonColor);
            string owner = entry.Available ? entry.Owner : $"{Text("Unavailable")} · {entry.Owner}";
            DrawLine(batch, bounds, entry.Title, 0, Color.White);
            DrawLine(batch, bounds, owner, 16, entry.Available ? Color.LightGray : Color.Orange);
        }

        /// <summary>绘制一行适配当前可用宽度的名称或来源。</summary>
        private static void DrawLine(SpriteBatch batch, Rectangle bounds, string text, int y, Color color)
        {
            const float scale = 0.65f;
            var font = FontAssets.MouseText.Value;
            float width = (bounds.Width - 8) / scale;
            if (font.MeasureString(text).X > width)
                text = font.CreateCroppedText(text, width);

            Utils.DrawBorderString(batch, text, new Vector2(bounds.Left + 4, bounds.Top + y), color, scale);
        }
    }

    /// <summary>显示音量百分比并在拖动完成后提交到配置草稿。</summary>
    private sealed class VolumeDraftSlider : UIElement
    {
        private readonly int initial;
        private readonly Action<int> commit;
        private int percent;
        private bool dragging;

        /// <summary>以现有百分比和草稿提交回调初始化滑块。</summary>
        internal VolumeDraftSlider(int initial, Action<int> commit)
        {
            this.initial = percent = initial;
            this.commit = commit;
        }

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            dragging = true;
            ReadPointer(evt.MousePosition.X);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!dragging)
                return;

            if (!Main.hasFocus)
            {
                dragging = false;
                percent = initial;
                return;
            }

            ReadPointer(Main.MouseScreen.X);
            if (!Main.mouseLeft)
            {
                dragging = false;
                commit(percent);
            }
        }

        /// <summary>按鼠标横坐标更新当前滑块百分比。</summary>
        private void ReadPointer(float x) => percent = VolumeControls.PercentAt(GetDimensions().ToRectangle(), (int)x);

        protected override void DrawSelf(SpriteBatch batch)
        {
            Rectangle bounds = GetDimensions().ToRectangle();
            if (bounds.Width <= 14)
                return;

            var canvas = new CaptionEditorCanvas(batch, FontAssets.MouseText.Value, TextureAssets.MagicPixel.Value);
            canvas.Slider(new Rectangle(bounds.X, bounds.Y + 13, bounds.Width, 19), percent);
            Utils.DrawBorderString(batch, $"{percent}%", new Vector2(bounds.Center.X, bounds.Y), Color.White, 0.65f, 0.5f);
        }
    }
}
