using Microsoft.Xna.Framework.Input;
using SoundCaptions.Common.Captions;
using Terraria.Audio;
using Terraria;
using Terraria.ModLoader;

namespace SoundCaptions;

/// <summary>提供客户端模组入口、编辑快捷键和可选的兼容名称注册接口。</summary>
public sealed class SoundCaptions : Mod
{
    internal ModKeybind ToggleEditMode { get; private set; }

    public override void Load()
    {
        if (Main.dedServ)
            return;

        ToggleEditMode = KeybindLoader.RegisterKeybind(this, "ToggleEditMode", Keys.F9);
    }

    public override object Call(params object[] args)
    {
        if (Main.dedServ || args is not [string command, object target, object value] ||
            !NameOverride.TryCreate(value, out NameOverride title))
            return false;

        return (command, target) switch
        {
            ("RegisterSoundCaption", SoundStyle style) => CaptionNames.RegisterSound(style, title),
            ("RegisterMusicTitle", string key) => CaptionNames.RegisterMusic(key, title),
            _ => false
        };
    }

    public override void Unload()
    {
        // 标题覆盖由其他模组在本次加载时注册，清理后下一次可按新的模组组合重新登记。
        CaptionNames.Clear();
        ToggleEditMode = null;
    }
}
