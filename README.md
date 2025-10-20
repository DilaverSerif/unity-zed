# Zed for Unity [![openupm](https://img.shields.io/npm/v/com.maligan.unity-zed?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.maligan.unity-zed/)

This is a homemade package to integrate [Zed](https://zed.dev) as Unity external script editor.

## Features

- **Cross-platform support**: Works on Windows, macOS, and Linux
- **Automatic discovery**: Finds your Zed installation automatically
- **Project generation**: Supports C# solution and project file generation
- **Line/column navigation**: Open files at specific lines and columns
- **Integration**: Seamlessly integrates with Unity's external editor workflow

## Roadmap

- [x] Discovery of Zed installations
- [x] Register as Unity external tools
- [x] Support C# sln/csproj generation
- [x] Windows compatibility
- [ ] Write Zed extension to deeper integration via IPC

## Installation

```sh
# 1. Via OpenUPM
openupm add com.maligan.unity-zed

# 2. Via PackageManger & GitHub URL
https://github.com/maligan/unity-zed.git

# 3. Via copy this repository content into Packages/ folder
```

## Platform Support

### Windows ✅
The package now supports Windows with automatic detection of Zed installations in these locations:
- `C:\Program Files\Zed\zed.exe`
- `C:\Program Files (x86)\Zed\zed.exe`
- `%LOCALAPPDATA%\Zed\zed.exe`
- `%APPDATA%\Zed\zed.exe`
- User installations via `C:\Users\%USERNAME%\AppData\Local\Programs\Zed\Zed.exe`
- Scoop installations
- Chocolatey installations
- winget installations
- PATH environment variable

### macOS ✅
- `/Applications/Zed.app/Contents/MacOS/cli`
- `/usr/local/bin/zed`

### Linux ✅
- Flatpak installations
- Repository installations
- NixOS
- Official website installations

## Usage

1. Install the package using one of the methods above
2. Go to **Edit → Preferences → External Tools**
3. Select **Zed Editor** from the **External Script Editor** dropdown
4. Unity will automatically detect your Zed installation
5. Double-click any C# script to open it in Zed

## AI Features and IntelliSense Support

This package now includes enhanced support for Zed's AI features and C# IntelliSense:

### Automatic LSP Configuration
The package automatically creates a `.zed/settings.json` file in your Unity project with:
- **C# Language Server**: OmniSharp configuration for Unity C# projects
- **IntelliSense**: Auto-completion, code navigation, and refactoring
- **Real-time Diagnostics**: Error detection and suggestions
- **Code Formatting**: Automatic C# code formatting

### Manual OmniSharp Installation
If AI features don't work automatically, install OmniSharp:

```bash
# Option 1: Install as .NET tool
dotnet tool install -g omnisharp

# Option 2: Use Visual Studio's OmniSharp
# Located in: Visual Studio 2022\MSBuild\Current\Bin\Roslyn\omnisharp.exe
```

### Enable AI Features in Zed
1. Open your Unity project in Zed
2. Press `Cmd/Ctrl + Shift + P` → "Settings"
3. Ensure Language Server Protocol is enabled for C#
4. Restart Zed after configuration changes

### Troubleshooting AI Features

**No IntelliSense or AI suggestions:**
- Check that OmniSharp is installed and accessible
- Verify `.zed/settings.json` exists in your Unity project
- Restart both Unity and Zed after making changes
- Check Zed's developer console for LSP connection errors

**Zed not detected on Windows:**
- Make sure Zed is installed in one of the supported locations
- Check that `zed.exe` is accessible in your PATH
- Restart Unity after installing Zed

**File opening issues:**
- Ensure your Unity project has solution/project file generation enabled
- Check Unity Console for any error messages

**LSP connection issues:**
- Verify OmniSharp path in `.zed/settings.json`
- Check that your project has `.sln` and `.csproj` files
- Ensure Unity's External Script Editor is set to Zed Editor
