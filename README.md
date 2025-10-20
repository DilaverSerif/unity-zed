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

## Troubleshooting

**Zed not detected on Windows:**
- Make sure Zed is installed in one of the supported locations
- Check that `zed.exe` is accessible in your PATH
- Restart Unity after installing Zed

**File opening issues:**
- Ensure your Unity project has solution/project file generation enabled
- Check Unity Console for any error messages
