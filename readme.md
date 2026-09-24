# Screensaver: Time edition

A Windows screensaver: a 3D flying digital clock (WinForms + WebView2 +
Three.js) that bounces DVD-logo style across every connected monitor, with
a hidden author signature quietly revealing itself in whichever corner it
last bounced off of.

## Features

- A real 3D clock, not a flat image - per-character extruded text rendered
  in Three.js under a perspective camera, hosted inside a native WinForms
  window via WebView2.
- DVD-style bounce, running independently on every connected monitor (one
  screensaver window per display).
- 4 hidden corner signatures, revealed for a few seconds whenever the clock
  passes over that corner and leaves again.
- Configurable language (English / Polski) and time format (24h / 12h),
  edited via a plain `config.json` - reachable straight from Windows'
  right-click **Configure** on the `.scr` file.
- Self-contained build: bundles the full .NET 8 runtime, so the compiled
  screensaver runs even on a machine with no .NET installed.
- Installs exactly like any other Windows screensaver - right-click
  `Screensaver.scr` -> **Install**.

## Repo structure

- **[`pre-built/`](pre-built/readme.md)** - the C# project plus the Three.js
  web content it loads, and build instructions.
- **[`after-built/`](after-built/install.md)** - the ready-to-install build
  (`Screensaver.scr` + its supporting files) and installation instructions.

## Requirements

- Windows 10/11 x64 - WinForms + WebView2 are Windows-only, this doesn't
  build or run anywhere else.
- To build from source: .NET 8 SDK.
- To run the compiled build: the WebView2 Runtime, which is already present
  on almost every Windows 10 (21H2+) and Windows 11 machine.

## Known limitation

The small live-preview thumbnail in Windows' Screen Saver Settings dialog
stays empty/black - a deliberate simplification (see `pre-built/readme.md`
for details). The real fullscreen screensaver is unaffected.

## License

[PolyForm Noncommercial 1.0.0](LICENSE) - free to use, modify, and
redistribute for any noncommercial purpose. Commercial use (including
selling it, or a modified version of it) is not permitted.