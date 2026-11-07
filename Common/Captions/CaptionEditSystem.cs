using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using SoundCaptions.Common.Audio;
using SoundCaptions.Common.Config;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace SoundCaptions.Common.Captions;

/// <summary>管理客户端字幕编辑模式、输入捕获、布局草稿和音量试听。</summary>
public sealed class CaptionEditSystem : ModSystem
{
    private const string LayerName = "Sound Captions: Editor";
    private static CaptionLayoutConfig Layout => ModContent.GetInstance<CaptionLayoutConfig>();
    private static SoundCaptionSystem Captions => ModContent.GetInstance<SoundCaptionSystem>();
    private static bool InputAvailable => Main.hasFocus && !Main.gameMenu && !Main.hideUI &&
        !Main.mapFullscreen && !Main.ingameOptionsWindow && Main.InGameUI?.CurrentState == null &&
        !Main.drawingPlayerChat && !Main.editSign && !Main.editChest && !Main.blockInput && !PlayerInput.WritingText &&
        Main.CurrentInputTextTakerOverride == null;

    private readonly List<CaptionHit> hits = new();
    private Rectangle lastDrawnWindow;
    private bool editing;
    private bool leftWasDown;
    private bool pointerCaptured;
    private bool escapeWasDown;
    private bool escapeCaptured;
    private int scroll;
    private int worldResetPending;
    private CaptionSelection? selection;
    private LayoutDrag drag;
    private VolumeDrag volumeDrag;
    private CaptionVolumeTarget? numberTarget;
    private string numberText;
    private bool replaceNumber;
    private bool enterCaptured;
    private KeyboardState previousKeyboard;

    internal bool Visible => editing && InputAvailable;

    public override void Load()
    {
        if (Main.dedServ)
            return;

        On_Main.DoUpdate_F8_ToggleNetDiagnostics += SuppressConflictingVanillaKey;
        On_Main.DoUpdate_Enter_ToggleChat += SuppressChatWhileTyping;
        if (Main.instance != null)
            Main.instance.Deactivated += OnDeactivated;
    }

    public override void Unload()
    {
        if (!Main.dedServ)
        {
            On_Main.DoUpdate_F8_ToggleNetDiagnostics -= SuppressConflictingVanillaKey;
            On_Main.DoUpdate_Enter_ToggleChat -= SuppressChatWhileTyping;
            if (Main.instance != null)
                Main.instance.Deactivated -= OnDeactivated;
        }

        Close();
    }

    // 将重置交给客户端输入或绘制路径，避免世界切换与拖动、试听状态并发修改。
    public override void OnWorldLoad() => Interlocked.Exchange(ref worldResetPending, 1);

    public override void OnWorldUnload() => Interlocked.Exchange(ref worldResetPending, 1);

    /// <summary>在客户端处理世界切换并释放编辑会话和按键捕获。</summary>
    private void ApplyWorldReset()
    {
        if (Interlocked.Exchange(ref worldResetPending, 0) == 0)
            return;

        Close();
        pointerCaptured = escapeCaptured = false;
        leftWasDown = escapeWasDown = true;
    }

    /// <summary>在音量数字输入处理 Enter 时暂缓游戏打开聊天框。</summary>
    private void SuppressChatWhileTyping(On_Main.orig_DoUpdate_Enter_ToggleChat orig)
    {
        if (numberTarget == null && !enterCaptured)
            orig();
    }

    /// <summary>当 F8 分配给字幕编辑且可接收输入时避免同时触发原版诊断。</summary>
    private void SuppressConflictingVanillaKey(On_Main.orig_DoUpdate_F8_ToggleNetDiagnostics orig)
    {
        var keybind = ((SoundCaptions)Mod).ToggleEditMode;
        if (InputAvailable && Main.keyState.IsKeyDown(Keys.F8) &&
            keybind?.GetAssignedKeys().Contains(nameof(Keys.F8)) == true)
            return;

        orig();
    }

    public override void PostUpdateInput()
    {
        if (Main.dedServ)
            return;

        ApplyWorldReset();
        if (!InputAvailable)
        {
            SuspendInput();
            return;
        }

        var triggers = PlayerInput.Triggers;
        bool left = triggers.Current.MouseLeft;
        var keybind = ((SoundCaptions)Mod).ToggleEditMode;
        KeyboardState keyboard = Keyboard.GetState();
        if (!keyboard.IsKeyDown(Keys.Enter))
            enterCaptured = false;

        if (numberTarget != null)
            HandleNumberInput(keyboard, triggers);
        else
        {
            // 本机原版使用 Shift+F9 切换灯光，保留组合键，只用 F9 打开编辑器。
            bool lightingShortcut = NewKey(keyboard, Keys.F9) && Utils.PressingShift(keyboard) &&
                keybind?.GetAssignedKeys().Contains(nameof(Keys.F9)) == true;
            HandleKeyboard(keyboard.IsKeyDown(Keys.Escape), keybind?.JustPressed == true && !lightingShortcut, triggers);
        }

        previousKeyboard = keyboard;

        bool consumePointer = HandlePointer(MouseInUi(), ScreenInUi(), left, PlayerInput.ScrollWheelDeltaForUI);
        if (!consumePointer)
            return;

        Main.LocalPlayer.mouseInterface = true;
        Main.mouseLeft = Main.mouseRight = false;
        triggers.Current.MouseLeft = triggers.Current.MouseRight = false;
        triggers.JustPressed.MouseLeft = triggers.JustPressed.MouseRight = false;
        PlayerInput.LockVanillaMouseScroll(LayerName);
        PlayerInput.ScrollWheelDelta = PlayerInput.ScrollWheelDeltaForUI = 0;
    }

    /// <summary>处理编辑开关、Esc 分层退出和背包按键冲突。</summary>
    private void HandleKeyboard(bool escape, bool toggle, TriggersPack triggers)
    {
        bool newEscape = escape && !escapeWasDown;
        escapeWasDown = escape;
        if (!escape)
            escapeCaptured = false;

        if (editing && newEscape)
        {
            escapeCaptured = true;
            drag = null;
            CancelVolumeEdit();
            if (selection != null)
                selection = null;
            else
                Close();
        }
        else if (toggle)
        {
            if (editing)
                Close();
            else
                editing = true;

            escapeCaptured |= newEscape;
        }

        if (escapeCaptured)
        {
            triggers.Current.Inventory = false;
            triggers.JustPressed.Inventory = false;
        }
    }

    /// <summary>读取数字键并处理清除、确认或取消音量输入。</summary>
    private void HandleNumberInput(KeyboardState keyboard, TriggersPack triggers)
    {
        bool escape = keyboard.IsKeyDown(Keys.Escape);
        escapeWasDown = escape;
        for (int slot = 1; slot <= 10; slot++)
        {
            string key = "Hotbar" + slot;
            triggers.Current.KeyStatus[key] = triggers.JustPressed.KeyStatus[key] = false;
        }

        triggers.Current.Inventory = triggers.JustPressed.Inventory = false;
        if (escape)
        {
            escapeCaptured = true;
            numberTarget = null;
            return;
        }

        if (NewKey(keyboard, Keys.Enter))
        {
            enterCaptured = true;
            CaptionVolumeTarget target = numberTarget.Value;
            numberTarget = null;
            if (int.TryParse(numberText, NumberStyles.None, CultureInfo.InvariantCulture, out int percent))
                target.Commit(percent);
            return;
        }

        if (NewKey(keyboard, Keys.Back))
        {
            numberText = replaceNumber || numberText.Length == 0 ? "" : numberText[..^1];
            replaceNumber = false;
        }

        foreach (Keys key in keyboard.GetPressedKeys())
        {
            if (previousKeyboard.IsKeyDown(key))
                continue;

            int digit = key is >= Keys.D0 and <= Keys.D9 ? key - Keys.D0 :
                key is >= Keys.NumPad0 and <= Keys.NumPad9 ? key - Keys.NumPad0 : -1;
            if (digit < 0)
                continue;

            if (replaceNumber)
            {
                numberText = "";
                replaceNumber = false;
            }

            if (numberText.Length < 3)
                numberText += (char)('0' + digit);
        }
    }

    /// <summary>判断指定键是否刚在当前采样中按下。</summary>
    private bool NewKey(KeyboardState state, Keys key) => state.IsKeyDown(key) && previousKeyboard.IsKeyUp(key);

    /// <summary>处理编辑区的点击、滚动、拖动和鼠标捕获。</summary>
    private bool HandlePointer(Point mouse, Point screen, bool left, int wheel)
    {
        bool pressed = left && !leftWasDown;
        leftWasDown = left;
        bool wasCaptured = pointerCaptured;
        if (!left)
            pointerCaptured = false;

        if (!editing)
            return wasCaptured;

        if (volumeDrag != null)
        {
            if (screen != volumeDrag.Screen)
                CancelVolumeEdit();
            else
            {
                int percent = VolumeControls.PercentAt(volumeDrag.Slider, mouse.X);
                volumeDrag.Target.Preview(percent);
                if (!left)
                {
                    CaptionVolumeTarget target = volumeDrag.Target;
                    volumeDrag = null;
                    target.Commit(percent);
                }
            }

            return true;
        }

        if (drag != null)
        {
            if (drag.Screen != screen)
                drag = null;
            else
            {
                CaptionLayout.ApplyDrag(drag.Preview, drag.Original, drag.Start, mouse - drag.Pointer, screen, drag.ResizeEdges);
                if (!left)
                    CommitDrag();
            }

            hits.Clear();
            return true;
        }

        EditorGeometry geometry = GetGeometry(screen);
        bool overDetail = geometry.Detail.Contains(mouse);
        bool overWindow = geometry.Window.Contains(mouse);
        if (wheel != 0 && geometry.Body.Contains(mouse) && !overDetail)
        {
            scroll = Math.Clamp(scroll + Math.Sign(wheel), 0, Math.Max(0, Captions.ActiveCount - 1));
            hits.Clear();
        }

        if (pressed && (overWindow || overDetail))
        {
            pointerCaptured = true;
            numberTarget = null;
            if (overDetail)
                HandleDetailPress(geometry, mouse, screen);
            else if (geometry.Grip.Contains(mouse) || geometry.Header.Contains(mouse))
            {
                Point resizeEdges = geometry.Grip.Contains(mouse) ? geometry.ResizeEdges : Point.Zero;
                drag = new LayoutDrag(Layout, geometry.Window, mouse, screen, resizeEdges);
                hits.Clear();
            }
            else if (geometry.CurrentTrack.Contains(mouse) && Captions.Music.CurrentKey is string key)
            {
                selection = CaptionSelection.ForMusic(key);
            }
            else if (geometry.Window == lastDrawnWindow && geometry.Body.Contains(mouse))
            {
                foreach (CaptionHit hit in hits)
                {
                    if (hit.Bounds.Contains(mouse))
                    {
                        selection = CaptionSelection.ForSound(hit.Entry);
                        break;
                    }
                }
            }
        }

        if (pressed && !overWindow && !overDetail)
            numberTarget = null;

        return overWindow || overDetail || pointerCaptured || wasCaptured;
    }

    /// <summary>根据点击位置开始音量试听、数字输入或执行详情按钮操作。</summary>
    private void HandleDetailPress(EditorGeometry geometry, Point mouse, Point screen)
    {
        if (geometry.DetailClose.Contains(mouse))
        {
            selection = null;
            return;
        }

        if (selection is not CaptionSelection selected || !selected.Volume.Available)
            return;

        CaptionVolumeTarget target = selected.Volume;
        VolumeControls controls = VolumeControls.Create(geometry.Detail);
        if (controls.Slider.Contains(mouse))
        {
            volumeDrag = new VolumeDrag(target, controls.Slider, screen);
            target.Preview(VolumeControls.PercentAt(controls.Slider, mouse.X));
        }
        else if (controls.Number.Contains(mouse))
        {
            numberTarget = target;
            numberText = target.Percent.ToString(CultureInfo.InvariantCulture);
            replaceNumber = true;
        }
        else if (controls.Mute.Contains(mouse))
        {
            target.Commit(0);
        }
        else if (controls.Reset.Contains(mouse))
        {
            target.Commit(ClientConfig.DefaultVolumePercent);
        }
    }

    /// <summary>撤销未提交的试听并清除数字输入状态。</summary>
    private void CancelVolumeEdit()
    {
        if (volumeDrag != null)
            volumeDrag.Target.CancelPreview();

        volumeDrag = null;
        numberTarget = null;
        enterCaptured = false;
    }

    /// <summary>在拖动完成且几何尺寸有变动时保存布局草稿。</summary>
    private void CommitDrag()
    {
        LayoutDrag finished = drag;
        drag = null;
        Point screen = finished.Screen;
        if (CaptionLayout.GetBounds(screen.X, screen.Y, finished.Preview) == finished.Start)
            return;

        try
        {
            var result = Layout.SaveChanges(finished.Preview);
            if (result != ConfigSaveResult.Success)
                Main.NewText(Text("SaveFailed"), Color.OrangeRed);
        }
        catch (System.IO.IOException exception)
        {
            Mod.Logger.Warn("Could not save the caption layout.", exception);
            Main.NewText(Text("SaveFailed"), Color.OrangeRed);
        }
        catch (UnauthorizedAccessException exception)
        {
            Mod.Logger.Warn("Could not save the caption layout.", exception);
            Main.NewText(Text("SaveFailed"), Color.OrangeRed);
        }
    }

    /// <summary>关闭编辑模式并清理当前选择、草稿和命中记录。</summary>
    private void Close()
    {
        CancelVolumeEdit();
        editing = false;
        selection = null;
        scroll = 0;
        drag = null;
        hits.Clear();
    }

    /// <summary>在窗口失去焦点时挂起编辑输入。</summary>
    private void OnDeactivated(object sender, EventArgs args) => SuspendInput();

    /// <summary>取消未完成的手势，并等待已按住的按键释放后恢复操作。</summary>
    private void SuspendInput()
    {
        CancelVolumeEdit();
        drag = null;
        pointerCaptured = escapeCaptured = false;
        // 恢复焦点或关闭模态界面时，先等按键释放，避免已按住的键被当作新手势。
        leftWasDown = escapeWasDown = true;
        hits.Clear();
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        ApplyWorldReset();
        if (!Visible)
            return;

        int index = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
        if (index >= 0)
            layers.Insert(index, new LegacyGameInterfaceLayer(LayerName, Draw, InterfaceScaleType.UI));
    }

    /// <summary>绘制字幕编辑窗口及当前选择的详情面板。</summary>
    private bool Draw()
    {
        if (!Visible)
            return true;

        var screen = new Point(Main.screenWidth, Main.screenHeight);
        EditorGeometry geometry = GetGeometry(screen);
        lastDrawnWindow = geometry.Window;
        hits.Clear();
        if (geometry.Window.Width <= 0 || geometry.Window.Height <= 0)
            return true;

        var canvas = new CaptionEditorCanvas(Main.spriteBatch, FontAssets.MouseText.Value, TextureAssets.MagicPixel.Value);
        canvas.Frame(geometry.Window);
        canvas.Button(geometry.Header, Text("Header"));
        DrawSoundRows(canvas, geometry.Body);
        if (geometry.CurrentTrack.Width > 0 && Captions.Music.CurrentKey is string key)
            canvas.Button(geometry.CurrentTrack, $"{Text("CurrentTrack")} · {MusicTitleCatalog.GetTitle(key)}");

        canvas.Grip(geometry.Grip);
        if (selection is CaptionSelection target && geometry.Detail.Width > 0)
            DrawDetails(canvas, geometry, target);

        return true;
    }

    /// <summary>绘制可见的有效字幕，并记录可交互行的命中区域。</summary>
    private void DrawSoundRows(CaptionEditorCanvas canvas, Rectangle body)
    {
        ClientConfig config = ModContent.GetInstance<ClientConfig>();
        var appearance = CaptionAppearance.Create(config.SoundTextColor, config.SoundBackgroundColor,
            config.SoundBackgroundOpacity, config.SoundTextScale);
        var node = config.ShowSoundCaptions ? Captions.Newest : null;
        scroll = Math.Min(scroll, Math.Max(0, Captions.ActiveCount - 1));
        for (int i = 0; i < scroll && node != null; i++)
            node = node.Previous;

        if (node == null)
        {
            canvas.Renderer.DrawRow(body, Text("Preview"), appearance);
            return;
        }

        float? playerX = Main.LocalPlayer is { active: true } player ? player.Center.X : null;
        int threshold = Math.Clamp(config.DirectionThreshold, 0, 160);
        for (; node != null && body.Height > 0; node = node.Previous)
        {
            CaptionEntry entry = node.Value;
            int direction = SoundCaptionSystem.GetDirection(entry.Position, playerX, threshold);
            if (canvas.Renderer.MeasureRow(body, entry.Label.Text, appearance, direction) is not CaptionRow row)
                break;

            Rectangle rowBounds = row.GetBounds(body);
            VolumeTargetKey target = entry.Key.Family;
            if (!target.BasePath.StartsWith("SoundCaptions/Diagnostic/", StringComparison.Ordinal))
            {
                hits.Add(new CaptionHit(rowBounds, entry));
                if (selection?.SoundKey == target)
                    canvas.Highlight(rowBounds);
            }

            canvas.Renderer.DrawMeasuredRow(body, row, appearance, direction,
                opacity: CaptionRenderer.TimedOpacity(Captions.Now, entry.ExpiresAt));
            body.Height -= row.Height + CaptionRenderer.RowGap;
        }
    }

    /// <summary>显示选中条目的名称、完整目标键和音量操作区。</summary>
    private void DrawDetails(CaptionEditorCanvas canvas, EditorGeometry geometry, CaptionSelection target)
    {
        Rectangle detail = geometry.Detail;
        canvas.Frame(detail);
        canvas.Button(new Rectangle(detail.X, detail.Y, Math.Max(0, detail.Width - geometry.DetailClose.Width),
            geometry.DetailClose.Height), Text("Details"));
        canvas.Button(geometry.DetailClose, "×");

        int y = geometry.DetailClose.Bottom + 2;
        canvas.DetailText(detail, y, 44, target.Name);
        canvas.DetailText(detail, y + 44, 36, target.Source);
        canvas.DetailText(detail, y + 80, 19, Text(target.MusicKey != null ? "MusicScope" : "SoundScope"));
        if (target.Volume.Available)
            DrawVolumeControls(canvas, detail, target.Volume);
        else
            canvas.DetailText(detail, y + 99, 19, Text("VolumeUnavailable"));
        int identityHeight = Math.Max(0, Math.Min(80, detail.Bottom - y));
        if (new Rectangle(detail.Left, y, detail.Width, identityHeight).Contains(Main.mouseX, Main.mouseY))
            Main.instance.MouseText($"{target.Name}\n{target.Source}");
    }

    /// <summary>绘制当前音量和用于试听、输入、静音及恢复的控件。</summary>
    private void DrawVolumeControls(CaptionEditorCanvas canvas, Rectangle detail, CaptionVolumeTarget target)
    {
        VolumeControls controls = VolumeControls.Create(detail);
        if (controls.Slider.IsEmpty)
            return;

        int percent = target.Percent;
        canvas.Slider(controls.Slider, percent);
        canvas.Button(controls.Number, numberTarget == target ? numberText + "|" : $"{percent}%");
        if (numberTarget == target)
            canvas.Highlight(controls.Number);

        canvas.Button(controls.Mute, Text("Mute"));
        canvas.Button(controls.Reset, Text("ResetVolume"));
    }

    /// <summary>按当前布局或拖动草稿计算编辑器的绘制与命中区域。</summary>
    private EditorGeometry GetGeometry(Point screen)
    {
        Rectangle window = CaptionLayout.GetBounds(screen.X, screen.Y, drag?.Preview ?? Layout);
        Point? resizeEdges = drag != null && drag.ResizeEdges != Point.Zero ? drag.ResizeEdges : null;
        int detailHeight = selection == null ? 0 : selection.Value.Volume.Available ? 186 : 150;
        return EditorGeometry.Create(screen, window, Captions.Music.CurrentKey != null, detailHeight, resizeEdges);
    }

    /// <summary>将当前视口大小换算为界面坐标。</summary>
    private static Point ScreenInUi() => (PlayerInput.OriginalScreenSize * (1f / Main.UIScale)).ToPoint();

    /// <summary>将鼠标位置换算为与编辑器布局一致的界面坐标。</summary>
    private static Point MouseInUi() =>
        (new Vector2(PlayerInput.MouseX, PlayerInput.MouseY) * (1f / Main.UIScale)).ToPoint();

    /// <summary>获取字幕编辑器的本地化文本。</summary>
    private static string Text(string key) => Language.GetTextValue($"Mods.SoundCaptions.Editor.{key}");

    /// <summary>保存音量拖动的目标、滑轨区域和开始时的屏幕尺寸。</summary>
    private sealed record VolumeDrag(CaptionVolumeTarget Target, Rectangle Slider, Point Screen);

    /// <summary>将可点击的字幕行区域关联到实际声音条目。</summary>
    private readonly record struct CaptionHit(Rectangle Bounds, CaptionEntry Entry);

    /// <summary>保存布局手势的初始位置、拖动草稿和调整边。</summary>
    private sealed class LayoutDrag(CaptionLayoutConfig original, Rectangle start, Point pointer, Point screen, Point resizeEdges)
    {
        internal CaptionLayoutConfig Original { get; } = (CaptionLayoutConfig)original.Clone();
        internal CaptionLayoutConfig Preview { get; } = (CaptionLayoutConfig)original.Clone();
        internal Rectangle Start { get; } = start;
        internal Point Pointer { get; } = pointer;
        internal Point Screen { get; } = screen;
        internal Point ResizeEdges { get; } = resizeEdges;
    }
}

/// <summary>保存编辑详情的标题、来源和稳定音量目标，使条目到期后仍可调整。</summary>
internal readonly record struct CaptionSelection(string Name, string Source, VolumeTargetKey? SoundKey, string MusicKey)
{
    internal CaptionVolumeTarget Volume => new(SoundKey, MusicKey);

    /// <summary>为选中的音效生成带完整声音键的详情快照。</summary>
    internal static CaptionSelection ForSound(CaptionEntry entry) =>
        new(entry.Label.Text, entry.Key.Family.ToString(), entry.Key.Family, null);

    /// <summary>为选中的曲目生成带稳定曲目键的详情快照。</summary>
    internal static CaptionSelection ForMusic(string key) => new(MusicTitleCatalog.GetTitle(key), key, null, key);
}

/// <summary>统一编辑器对音效和音乐目标的试听与保存操作。</summary>
internal readonly record struct CaptionVolumeTarget(VolumeTargetKey? SoundKey, string MusicKey)
{
    private static SoundVolumeSystem Sound => ModContent.GetInstance<SoundVolumeSystem>();
    private static MusicVolumeSystem Music => ModContent.GetInstance<MusicVolumeSystem>();

    internal bool Available => SoundKey.HasValue ? Sound?.Ready == true :
        MusicKey != null && Music?.Supports(MusicKey) == true;

    internal int Percent => SoundKey is VolumeTargetKey key ? Sound.PercentFor(key) : Music.PercentFor(MusicKey);

    /// <summary>按目标类别开始临时音量试听。</summary>
    internal void Preview(int percent)
    {
        if (SoundKey is VolumeTargetKey key)
            Sound.Preview(key, percent);
        else
            Music.Preview(MusicKey, percent);
    }

    /// <summary>按目标类别保存最终音量百分比。</summary>
    internal void Commit(int percent)
    {
        if (SoundKey is VolumeTargetKey key)
            Sound.Commit(key, percent);
        else
            Music.Commit(MusicKey, percent);
    }

    /// <summary>取消目标所属音量系统中的临时试听。</summary>
    internal void CancelPreview()
    {
        if (SoundKey.HasValue)
            Sound?.CancelPreview();
        else
            Music?.CancelPreview();
    }
}
