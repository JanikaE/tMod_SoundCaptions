using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using SoundCaptions.Common.Audio;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace SoundCaptions.Common.Config;

/// <summary>定义背景音乐字幕的切曲短显与持续显示模式。</summary>
public enum MusicDisplayMode
{
    OnTrackChange,
    Always
}

/// <summary>保存本机字幕外观、显示开关、诊断选项和单项音量覆盖。</summary>
public sealed class ClientConfig : ModConfig
{
    internal const float DefaultCaptionDurationSeconds = 2f;

    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header("Sound")]
    [DefaultValue(true)]
    public bool ShowSoundCaptions { get; set; } = true;

    [DefaultValue(DefaultCaptionDurationSeconds)]
    [Range(0.5f, 15f)]
    [Increment(0.5f)]
    public float SoundDurationSeconds { get; set; } = DefaultCaptionDurationSeconds;

    [ColorNoAlpha]
    [DefaultValue(typeof(Color), "255, 255, 255, 255")]
    public Color SoundTextColor { get; set; } = Color.White;

    [ColorNoAlpha]
    [DefaultValue(typeof(Color), "0, 0, 0, 255")]
    public Color SoundBackgroundColor { get; set; } = Color.Black;

    [DefaultValue(55)]
    [Range(0, 100)]
    [Slider]
    public int SoundBackgroundOpacity { get; set; } = 55;

    [DefaultValue(100)]
    [Range(50, 200)]
    [Slider]
    public int SoundTextScale { get; set; } = 100;

    [DefaultValue(32)]
    [Range(0, 160)]
    [Slider]
    public int DirectionThreshold { get; set; } = 32;

    [Header("Music")]
    [DefaultValue(true)]
    public bool ShowMusicCaptions { get; set; } = true;

    [DefaultValue(MusicDisplayMode.Always)]
    [Dropdown]
    public MusicDisplayMode MusicMode { get; set; } = MusicDisplayMode.Always;

    [DefaultValue(DefaultCaptionDurationSeconds)]
    [Range(0.5f, 15f)]
    [Increment(0.5f)]
    public float MusicDurationSeconds { get; set; } = DefaultCaptionDurationSeconds;

    [ColorNoAlpha]
    [DefaultValue(typeof(Color), "0, 255, 0, 255")]
    public Color MusicTextColor { get; set; } = Color.Lime;

    [ColorNoAlpha]
    [DefaultValue(typeof(Color), "0, 0, 0, 255")]
    public Color MusicBackgroundColor { get; set; } = Color.Black;

    [DefaultValue(55)]
    [Range(0, 100)]
    [Slider]
    public int MusicBackgroundOpacity { get; set; } = 55;

    [DefaultValue(100)]
    [Range(50, 200)]
    [Slider]
    public int MusicTextScale { get; set; } = 100;

    [Header("Development")]
    [DefaultValue(false)]
    public bool EnableDiagnostics { get; set; }

    internal const int DefaultVolumePercent = 100;
    internal const int MaxVolumePercent = 200;

    private List<SoundVolumeOverride> soundVolumes = new();
    private Dictionary<VolumeTargetKey, int> soundVolumeIndex;

    [JsonProperty]
    internal List<SoundVolumeOverride> SoundVolumes
    {
        get => soundVolumes;
        private set
        {
            soundVolumes = value ?? new();
            soundVolumeIndex = null;
        }
    }

    [Header("Volume")]
    [CustomModConfigItem(typeof(AdjustedVolumesElement))]
    public Dictionary<string, int> MusicVolumes { get; set; } = new(StringComparer.Ordinal);

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    internal int VolumeSchemaVersion { get; private set; } = 1;

    /// <summary>按稳定曲目键读取已保存的非默认音量。</summary>
    internal int? GetMusicVolume(string key)
    {
        if (MusicVolumes.TryGetValue(key, out int percent) && percent != DefaultVolumePercent)
            return Math.Clamp(percent, 0, MaxVolumePercent);

        return null;
    }

    /// <summary>规范化并更新指定曲目的覆盖，恢复默认时移除记录。</summary>
    internal void SetMusicVolume(string key, int percent)
    {
        percent = Math.Clamp(percent, 0, MaxVolumePercent);
        if (percent != DefaultVolumePercent)
            MusicVolumes[key] = percent;
        else
            MusicVolumes.Remove(key);
    }

    /// <summary>保存世界内音量编辑产生的配置草稿，并在失败时给出提示。</summary>
    internal bool SaveVolumeChanges(ClientConfig pending)
    {
        try
        {
            if (SaveChanges(pending) == ConfigSaveResult.Success)
                return true;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            Mod.Logger.Warn("Could not save the volume override.", exception);
        }

        Main.NewText(Language.GetTextValue("Mods.SoundCaptions.Editor.VolumeSaveFailed"), Color.OrangeRed);
        return false;
    }

    // 主菜单保存会重读配置，但可能延迟触发 OnChanged。索引在首次读取时按新集合构建。
    private Dictionary<VolumeTargetKey, int> SoundVolumeIndex => soundVolumeIndex ??= BuildSoundVolumeIndex();

    internal bool HasSoundVolumes => SoundVolumeIndex.Count != 0;

    /// <summary>按稳定声音族键读取已保存的非默认音量。</summary>
    internal int? GetSoundVolume(VolumeTargetKey key) =>
        SoundVolumeIndex.TryGetValue(key, out int percent) ? percent : null;

    /// <summary>规范化并替换声音族覆盖，使索引在下次读取时重建。</summary>
    internal void SetSoundVolume(VolumeTargetKey key, int percent)
    {
        percent = Math.Clamp(percent, 0, MaxVolumePercent);
        soundVolumeIndex = null;
        SoundVolumes.RemoveAll(record => record.Target == key);
        if (percent != DefaultVolumePercent)
            SoundVolumes.Add(new SoundVolumeOverride(key, percent));
    }

    /// <summary>清除音效和音乐的所有音量覆盖。</summary>
    internal void ClearVolumeOverrides()
    {
        SoundVolumes = new();
        MusicVolumes.Clear();
    }

    public override ModConfig Clone()
    {
        // 原生配置界面以草稿编辑。复制两个集合，防止取消操作时把改动留在活动配置中。
        var copy = (ClientConfig)base.Clone();
        copy.SoundVolumes = new List<SoundVolumeOverride>(SoundVolumes);
        copy.MusicVolumes = new Dictionary<string, int>(MusicVolumes, StringComparer.Ordinal);
        return copy;
    }

    public override void OnChanged()
    {
        // 资源改名时按配置版本显式迁移键，防止旧音量被猜测匹配到别的音效。
        VolumeSchemaVersion = Math.Max(1, VolumeSchemaVersion);
        var index = BuildSoundVolumeIndex();
        soundVolumeIndex = index;
        SoundVolumes.Clear();
        foreach (var pair in index)
            SoundVolumes.Add(new SoundVolumeOverride(pair.Key, pair.Value));

        foreach (string key in new List<string>(MusicVolumes.Keys))
        {
            if (string.IsNullOrWhiteSpace(key) || MusicVolumes[key] == DefaultVolumePercent)
                MusicVolumes.Remove(key);
            else
                MusicVolumes[key] = Math.Clamp(MusicVolumes[key], 0, MaxVolumePercent);
        }
    }

    /// <summary>过滤无效记录并按稳定键构建非默认音效音量索引。</summary>
    private Dictionary<VolumeTargetKey, int> BuildSoundVolumeIndex()
    {
        var index = new Dictionary<VolumeTargetKey, int>();
        foreach (SoundVolumeOverride record in SoundVolumes)
        {
            if (string.IsNullOrWhiteSpace(record.Target.BasePath))
                continue;

            int percent = Math.Clamp(record.Percent, 0, MaxVolumePercent);
            if (percent == DefaultVolumePercent)
                index.Remove(record.Target);
            else
                index[record.Target] = percent;
        }

        return index;
    }
}

/// <summary>保存一个稳定声音族键与其独立音量百分比。</summary>
internal readonly record struct SoundVolumeOverride(VolumeTargetKey Target, int Percent);
