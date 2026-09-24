# Screensaver: Time edition - installation

This covers the **compiled** screensaver (a folder containing
`Screensaver.scr` plus its supporting files) - not the source code. To
build from source, see [`../pre-built/readme.md`](../pre-built/readme.md).

## What's in this folder

Once built (see `pre-built/build.ps1` / `pre-built/readme.md`), you get a
folder that looks roughly like this:

```
Screensaver/
  Screensaver.scr        an ordinary .exe, renamed to .scr
  *.dll                  the .NET runtime (self-contained) + WebView2
  web/
    index.html
    config.json
    i18n/
    fonts/
    vendor/
```

**All of these files must stay together in one folder.** `Screensaver.scr`
is not standalone - it loads the DLLs sitting next to it and the `web/`
folder (which holds the clock's entire content and logic). Don't move the
`.scr` file on its own, separated from the rest.

## Requirements

- Windows 10/11 x64.
- Microsoft Edge WebView2 Runtime - in practice this is almost certainly
  already installed (it ships with Windows Update / Edge on most Windows 10
  21H2+ machines and on every Windows 11 machine). If it's missing, you'll
  see it plainly: a "WebView2 initialization error" dialog on startup,
  rather than a silent crash or a black screen. If needed, get the runtime
  from Microsoft (search "WebView2 Runtime Evergreen Bootstrapper").

## Installing as your Windows screensaver

1. Move the whole folder to a permanent location - **not** Downloads/Temp,
   since those tend to get cleaned up by Windows or antivirus tools. Good
   locations: `C:\Program Files\Screensaver\` (needs administrator rights)
   or `%LOCALAPPDATA%\Programs\Screensaver\` (no admin needed).
2. Right-click `Screensaver.scr` -> **Install**. This sets it as your
   active Windows screensaver and immediately opens the Screen Saver
   Settings dialog with it already selected.
3. In that dialog you can set the idle time before the screensaver kicks
   in - a standard Windows option, unrelated to this project's own code.

**Note on the settings dropdown:** the classic Screen Saver Settings dialog
only lists `.scr` files that live in `C:\Windows\System32`. The
right-click **Install** action bypasses that requirement entirely - it
works from any location, because it sets the relevant registry entry
directly. If you specifically want it to also show up in that dropdown
list, you'd need to manually copy the whole folder into
`C:\Windows\System32\` (needs admin), but that's not required for normal
use.

## Configuration (language / time format)

Right-click `Screensaver.scr` -> **Configure** (or click "Settings..." in
the Screen Saver Settings dialog). This opens `web\config.json` in
Notepad:

```json
{ "language": "en", "timeFormat": "24" }
```

- `language`: any code listed in `web\i18n\languages.json` (currently `en`,
  `pl`).
- `timeFormat`: `"24"` or `"12"`.

Save the file - the change takes effect the next time the screensaver
starts.

## Quick test without installing

Double-clicking `Screensaver.scr` runs it immediately, fullscreen, on every
connected monitor (exactly as if it had just kicked in) - moving the mouse
or pressing a key closes it. Handy for a quick check before installing it
permanently.

## Known limitation

The small preview thumbnail in the Screen Saver Settings dialog currently
renders nothing - it stays empty/black. This is a deliberate simplification
in the code (the `/p` mode in `Program.cs` is a no-op) - the real
fullscreen screensaver works normally; only that small settings-dialog
thumbnail is affected.

## Uninstalling

Pick a different screensaver (or "None") in the Screen Saver Settings
dialog, then simply delete the folder.
