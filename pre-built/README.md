# Screensaver: Time edition

A Windows screensaver built with WinForms + WebView2, rendering a 3D flying
digital clock (Three.js, extruded per-character text under a real
perspective camera) that bounces around the screen DVD-logo style. A small
author signature quietly reveals itself in whichever corner the clock
bounces off of, then fades away.

This folder (`pre-built/`) contains the raw source - C# project plus the
Three.js web content it loads through WebView2 - not a compiled binary. For
the ready-to-install build, see
[`../after-built/install.md`](../after-built/install.md).

## Requirements

- Windows 10/11 x64. WinForms + WebView2 are Windows-only technologies;
  this project will not build or run anywhere else.
- .NET 8 SDK - https://dotnet.microsoft.com/download/dotnet/8.0

## Project structure

```
Screensaver.csproj                 project definition (.NET 8, WinForms,
                                    self-contained win-x64, references
                                    Microsoft.Web.WebView2)
Program.cs                         entry point; handles the screensaver
                                    command-line contract: /s (show),
                                    /c (configure), /p (preview)
MultiScreenApplicationContext.cs   creates one MainForm per connected
                                    monitor; exits the whole app once
                                    user activity is detected
MainForm.cs                        a single screensaver window - hosts a
                                    WebView2 control, loads web/index.html
LowLevelInputHook.cs               global mouse/keyboard hook (Win32 API)
                                    that detects user activity regardless
                                    of which window currently has focus
web/                                everything the WebView2 control loads:
                                    HTML/CSS/JS (Three.js), i18n strings,
                                    fonts, config.json
build.ps1                          one-shot publish + package script,
                                    see "Building" below
```

`web/**/*` is copied into the build output automatically (see
`<Content Include="web\**\*.*">` in `Screensaver.csproj`) - nothing under
`web/` needs to be copied by hand.

## Building

Simplest: run `build.ps1` from this folder in PowerShell. It runs
`dotnet publish` (self-contained, win-x64) and renames `Screensaver.exe` to
`Screensaver.scr` in `publish/Screensaver/`.

Equivalent by hand:

```powershell
dotnet publish Screensaver.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o publish\Screensaver
Rename-Item publish\Screensaver\Screensaver.exe Screensaver.scr
```

`--self-contained true` bundles the full .NET runtime into the output, so
it runs on a machine with no .NET installed, at the cost of a larger output
folder (already set in `.csproj` too; the CLI flag is just an explicit
repeat).

## Quick testing without publishing or installing

Plain `dotnet run` works too (runs from `bin/Debug/...`, not
self-contained, but functionally identical):

```powershell
dotnet run                  # fullscreen on every monitor, like the real screensaver
dotnet run -- /c             # opens web\config.json in Notepad
dotnet run -- /p             # preview mode - currently a no-op, see below
```

Moving the mouse or pressing a key closes the screensaver window (mirroring
real Windows behavior) - `LowLevelInputHook` ignores the first 2 seconds
after startup so an accidental nudge right after launch doesn't immediately
kick you out.

## Editing the clock's look/behavior

All of the 3D clock logic (Three.js, extruded text, perspective camera,
DVD-style bounce, corner hitboxes for the author label) lives in
`web/index.html` - an ordinary page loaded by WebView2. Edit it and run
`dotnet run` to see the result immediately, no C# rebuild needed. Text/
language changes live in `web/i18n/*.json` and `web/i18n/languages.json`.

## Known limitation: `/p` (preview) mode

Windows invokes a screensaver with `/p <window handle>` when it needs to
draw a live thumbnail inside the small preview monitor in the Screen Saver
Settings dialog. In this project, the `/p` branch in `Program.cs` simply
returns immediately - no thumbnail is rendered (the preview stays empty/
black). This doesn't affect the real fullscreen screensaver (`/s`, the
default) or configuration (`/c`) - it's purely a cosmetic gap in the
settings dialog itself. It could be implemented (embedding WebView2 directly
into the handed-in window handle) if needed later.