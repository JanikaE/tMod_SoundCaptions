# Sound Captions player guide

[简体中文](README.md)

Sound Captions is a client-side caption mod for Terraria and tModLoader. It captions local sound effects and displays the current background music on a separate line. The baseline is Terraria 1.4.4.9 and tModLoader 2026.09.3.1 stable.

## Captions and layout

- Toggle sound and music captions independently. Sound captions last two seconds by default and fade at the end. A repeated sound refreshes its caption. Arrows indicate left or right relative to your character. Global sounds and sounds within the horizontal threshold have no arrow.
- In Mod Configuration → Sound Captions → Caption Settings, adjust font sizes, text colors, background opacity, duration, and the direction threshold. By default, the current track stays visible in bright green. In track-change mode, the caption appears for two seconds by default.
- Press **F9** by default to enter edit mode. Drag the header to move the window, drag the corner handle to resize it, or scroll through active sound captions. You can rebind the shortcut in the game controls. The game keeps previously saved bindings. Shift+F9 still switches vanilla lighting modes. Press it again to exit or use Esc to close the details panel and then exit edit mode.

## Individual volume

- In edit mode, select a sound or Current Track. Use the slider, percentage field, mute button, or reset button to set its volume from 0% to 200%. Press Enter to save a typed value or Esc to cancel. At 0% the target is muted, 100% restores the default, and higher values boost it within the limits of the audio backend.
- Open Adjusted Volumes at the bottom of Caption Settings to search and edit saved sound and music settings. Click Save on the configuration page to apply those changes. Restoring all volumes requires confirmation.
- The separate Caption Layout configuration stores the layout. Volume settings are local and apply even when captions are hidden.

## Multiplayer and compatibility

- Install the mod on each client that needs captions. The server determines its mod requirements. Each player has their own caption and volume settings.
- The mod supports standard tModLoader playback. Third-party players that bypass it need dedicated integration. Music resource packs keep the original track titles by default. Compatibility mods can register replacement titles.
- The [vanilla audio catalog](docs/VANILLA_AUDIO_CATALOG.md) lists vanilla caption coverage. Developer diagnostics are off by default and can be enabled temporarily in Caption Settings.

## Documentation and publishing

The [design specification](DESIGN.md) is maintained in Chinese. This guide is kept in sync with the Chinese guide. The Workshop descriptions are available in [Chinese](description_workshop.txt) and [English](description_workshop_en.txt). tModLoader reads the default Chinese file when publishing. The English text can be used for the English Steam Workshop entry.
