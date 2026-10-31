# Developer Notes

## Overview

Crypto Terminal Launcher is a launcher application for open-source crypto terminals such as Depth, HypeTerminal, and Tradoor. It allows users to browse available terminals, download releases from GitHub, and launch them locally.

## Important

This launcher does not modify or circumvent paid software. It only aggregates open-source crypto terminals and provides a convenient way to download and run them. All terminals included are open-source projects.

## Adding New Terminals

To add a new terminal, update the `GetAvailableTerminals` method in `TerminalManager.cs` with the terminal name, repository URL, release URL, description, and open-source status.

Update URLs in `TerminalManager.cs` where marked with `// TODO: replace with actual GitHub release URLs`.

## Known Limitations

- Crypto terminals typically require API keys for trading features. The launcher itself only provides read-only access without keys.
- Downloaded ZIP files must be valid GitHub release assets.
- The launcher does not verify terminal code beyond basic safety checks.