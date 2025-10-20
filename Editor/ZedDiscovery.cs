using Unity.CodeEditor;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Text;
using System;
using System.Diagnostics;
using NiceIO;

namespace UnityZed
{
    public class ZedDiscovery
    {
        private static bool k_IsWindows => UnityEngine.Application.platform == UnityEngine.RuntimePlatform.WindowsEditor;

        public CodeEditor.Installation[] GetInstallations()
        {
            var results = new List<CodeEditor.Installation>();

            var candidates = new List<(NPath path, TryGetVersion tryGetVersion)>();

            // [Windows]
            if (k_IsWindows)
            {
                // [Windows] (Program Files)
                candidates.Add(("C:\\Program Files\\Zed\\zed.exe", null));
                candidates.Add(("C:\\Program Files (x86)\\Zed\\zed.exe", null));

                // [Windows] (User local installation - Standard installer location)
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                candidates.Add((new NPath(localAppData).Combine("Programs", "Zed", "Zed.exe"), null));
                candidates.Add((new NPath(localAppData).Combine("Zed", "zed.exe"), null));

                // [Windows] (User AppData)
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                candidates.Add((new NPath(appData).Combine("Zed", "zed.exe"), null));

                // [Windows] (Scoop installation)
                var scoopPath = NPath.HomeDirectory.Combine("scoop", "apps", "zed", "current", "zed.exe");
                candidates.Add((scoopPath, null));

                // [Windows] (Chocolatey installation)
                candidates.Add(("C:\\ProgramData\\chocolatey\\lib\\zed\\tools\\zed.exe", null));

                // [Windows] (winget installation)
                candidates.Add((NPath.HomeDirectory.Combine("AppData", "Local", "Microsoft", "WinGet", "Packages", "zed.Zed", "zed.exe"), null));

                // [Windows] (Check for Zed in PATH)
                candidates.Add(("zed.exe", TryGetVersionFromWindowsExe));
            }
            else
            {
                // [MacOS]
                candidates.Add(("/Applications/Zed.app/Contents/MacOS/cli", TryGetVersionFromPlist));
                candidates.Add(("/usr/local/bin/zed", null));

                // [Linux] (Flatpak)
                candidates.Add(("/var/lib/flatpak/app/dev.zed.Zed/current/active/files/bin/zed", null));

                // [Linux] (Repo)
                candidates.Add(("/usr/bin/zeditor", null));

                // [Linux] (NixOS)
                candidates.Add(("/run/current-system/sw/bin/zeditor", null));

                // [Linux] (NixOS HomeManager from Zed Flake)
                candidates.Add(($"/etc/profiles/per-user/{Environment.UserName}/bin/zed", null));

                // [Linux] (NixOS HomeManager from NixPkgs)
                candidates.Add(($"/etc/profiles/per-user/{Environment.UserName}/bin/zeditor", null));

                // [Linux] (Official Website)
                candidates.Add((NPath.HomeDirectory.Combine(".local/bin/zed"), null));
            }

            foreach (var candidate in candidates)
            {
                var candidatePath = candidate.path;
                var candidateTryGetVersion = candidate.tryGetVersion ?? TryGetVersionFallback;

                if (candidatePath.FileExists())
                {
                    var name = new StringBuilder("Zed");

                    if (candidateTryGetVersion(candidatePath, out var version))
                        name.Append($" [{version}]");

                    results.Add(new()
                    {
                        Name = name.ToString(),
                        Path = candidatePath.MakeAbsolute().ToString(),
                    });

                    break;
                }
            }

            return results.ToArray();
        }

        public bool TryGetInstallationForPath(string editorPath, out CodeEditor.Installation installation)
        {
            foreach (var installed in GetInstallations())
            {
                if (installed.Path == editorPath)
                {
                    installation = installed;
                    return true;
                }
            }

            installation = default;
            return false;
        }

        //
        // TryGetVersion implementations
        //
        private delegate bool TryGetVersion(NPath path, out string vertion);

        private static bool TryGetVersionFallback(NPath path, out string version)
        {
            version = null;
            return false;
        }

        private static bool TryGetVersionFromPlist(NPath path, out string version)
        {
            version = null;

            var plistPath = path.Combine("../../").Combine("Info.plist");
            if (plistPath.FileExists() == false)
                return false;

            var xPath = new XPathDocument(plistPath.ToString());
            var xNavigator = xPath.CreateNavigator().SelectSingleNode("/plist/dict/key[text()='CFBundleShortVersionString']/following-sibling::string[1]/text()");
            if (xNavigator == null)
                return false;

            version = xNavigator.Value;
            return true;
        }

        private static bool TryGetVersionFromWindowsExe(NPath path, out string version)
        {
            version = null;

            if (!k_IsWindows || !path.FileExists())
                return false;

            try
            {
                // Try to get version from the executable file
                var versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(path.ToString());
                if (!string.IsNullOrEmpty(versionInfo.FileVersion))
                {
                    version = versionInfo.FileVersion;
                    return true;
                }
            }
            catch
            {
                // If version detection fails, just return false
            }

            return false;
        }
    }
}
