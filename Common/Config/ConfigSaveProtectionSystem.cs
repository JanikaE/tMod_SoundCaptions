using System;
using System.IO;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SoundCaptions.Common.Config;

/// <summary>为原生配置界面处理本模组客户端配置的写盘失败。</summary>
public sealed class ConfigSaveProtectionSystem : ModSystem
{
    private Hook saveHook;

    public override void Load()
    {
        if (!Main.dedServ)
            saveHook = new Hook(typeof(ModConfig).GetMethod(nameof(ModConfig.SaveChanges)), SaveWithStatus);
    }

    public override void Unload()
    {
        saveHook?.Dispose();
        saveHook = null;
    }

    /// <summary>描述原生配置保存方法的接收者和参数，供运行时挂钩调用。</summary>
    private delegate ConfigSaveResult SaveOriginal(ModConfig config, ModConfig pending,
        Action<string, Color> status, bool silent, bool broadcast);

    /// <summary>执行原生保存，并在本模组的配置写盘失败时保留待保存草稿。</summary>
    private ConfigSaveResult SaveWithStatus(SaveOriginal orig, ModConfig config, ModConfig pending,
        Action<string, Color> status, bool silent, bool broadcast)
    {
        // 只接管带状态回调的本模组配置页。世界内编辑和其他模组继续走各自的错误处理。
        if (status == null || config is not (ClientConfig or CaptionLayoutConfig))
            return orig(config, pending, status, silent, broadcast);

        try
        {
            return orig(config, pending, status, silent, broadcast);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            Mod.Logger.Warn("Could not save the Sound Captions client config.", exception);
            status(Language.GetTextValue("Mods.SoundCaptions.VolumeManager.ConfigSaveFailed"), Color.OrangeRed);
            // 原生配置界面收到非成功结果会保留草稿，因此用户可直接重试写盘。
            return ConfigSaveResult.NeedsReload;
        }
    }
}
