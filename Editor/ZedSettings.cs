using UnityEngine;
using NiceIO;
using SimpleJSON;
using System;

namespace UnityZed
{
    public class ZedSettings
    {
        private static readonly ILogger sLogger = ZedLogger.Create();

        private readonly NPath m_SettingsPath;

        public ZedSettings()
        {
            m_SettingsPath = new NPath(Application.dataPath).Parent.Combine(".zed/settings.json");
        }

        public void Sync()
        {
            if (m_SettingsPath.FileExists() == false)
            {
                sLogger.Log("Zed settings file not found, creating default settings file.");
                CreateDefaultSettings();
            }
            else
            {
                // Update existing settings with OmniSharp path if needed
                UpdateOmniSharpPath();
            }
        }

        private void CreateDefaultSettings()
        {
            m_SettingsPath.CreateFile();
            var settings = JSON.Parse(kDefaultSettings);

            // Try to find and configure OmniSharp path
            var omnisharpPath = FindOmniSharpPath();
            if (!string.IsNullOrEmpty(omnisharpPath))
            {
                settings["lsp"]["omnisharp"]["binary"]["path"] = omnisharpPath;
                sLogger.Log($"Found OmniSharp at: {omnisharpPath}");
            }

            m_SettingsPath.WriteAllText(settings.ToString());
        }

        private void UpdateOmniSharpPath()
        {
            try
            {
                var settings = JSON.Parse(m_SettingsPath.ReadAllText());
                var currentPath = settings["lsp"]["omnisharp"]["binary"]["path"].Value;

                if (string.IsNullOrEmpty(currentPath))
                {
                    var omnisharpPath = FindOmniSharpPath();
                    if (!string.IsNullOrEmpty(omnisharpPath))
                    {
                        settings["lsp"]["omnisharp"]["binary"]["path"] = omnisharpPath;
                        m_SettingsPath.WriteAllText(settings.ToString());
                        sLogger.Log($"Updated OmniSharp path to: {omnisharpPath}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                sLogger.Log($"Failed to update OmniSharp path: {ex.Message}");
            }
        }

        private string FindOmniSharpPath()
        {
            var candidates = new[]
            {
                // Check in common installation locations
                System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Programs", "Microsoft VS Code", "bin", "omnisharp.cmd"),
                System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles), "Microsoft Visual Studio", "2022", "Professional", "MSBuild", "Current", "Bin", "Roslyn", "omnisharp.exe"),
                System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles), "Microsoft Visual Studio", "2022", "Community", "MSBuild", "Current", "Bin", "Roslyn", "omnisharp.exe"),
                System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles), "Microsoft Visual Studio", "2022", "Enterprise", "MSBuild", "Current", "Bin", "Roslyn", "omnisharp.exe"),
                "omnisharp.exe", // Try in PATH
            };

            foreach (var candidate in candidates)
            {
                if (System.IO.File.Exists(candidate))
                {
                    return candidate;
                }
            }

            // Try to find OmniSharp via dotnet tool
            try
            {
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "tool list -g | findstr omnisharp",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(startInfo))
                {
                    var output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (output.Contains("omnisharp"))
                    {
                        // Extract path from dotnet tool list output
                        var lines = output.Split('\n');
                        foreach (var line in lines)
                        {
                            if (line.Contains("omnisharp"))
                            {
                                // This would need proper parsing of the tool list output
                                return "omnisharp"; // Fallback to PATH
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore tool detection errors
            }

            return null;
        }

        private const string kDefaultSettings = @"{
            ""languages"": {
                ""C#"": {
                    ""path"": ""csharp"",
                    ""language_servers"": [
                        ""omnisharp""
                    ],
                    ""formatter"": {
                        ""language_server"": {
                            ""name"": ""omnisharp""
                        }
                    }
                }
            },
            ""lsp"": {
                ""omnisharp"": {
                    ""binary"": {
                        ""path"": """",
                        ""arguments"": [
                            ""-lsp"",
                            ""--encoding"",
                            ""utf-8""
                        ]
                    },
                    ""initialization_options"": {
                        ""RoslynExtensionsOptions"": {
                            ""EnableImportCompletion"": true,
                            ""AnalyzeOpenDocumentsOnly"": false,
                            ""EnableAnalyzersSupport"": true,
                            ""Diagnostics"": {
                                ""EnableAnalyzers"": true,
                                ""EnableCodeActions"": true
                            }
                        },
                        ""Sdk"": {
                            ""IncludePrereleases"": true
                        }
                    }
                }
            },
            ""file_scan_exclusions"": [
                ""**/.*"",
                ""**/*~"",

                ""**/*.meta"",
                ""**/*.booproj"",
                ""**/*.pibd"",
                ""**/*.suo"",
                ""**/*.user"",
                ""**/*.userprefs"",
                ""**/*.unityproj"",
                ""**/*.dll"",
                ""**/*.exe"",
                ""**/*.pdf"",
                ""**/*.mid"",
                ""**/*.midi"",
                ""**/*.wav"",
                ""**/*.gif"",
                ""**/*.ico"",
                ""**/*.jpg"",
                ""**/*.jpeg"",
                ""**/*.png"",
                ""**/*.psd"",
                ""**/*.tga"",
                ""**/*.tif"",
                ""**/*.tiff"",
                ""**/*.3ds"",
                ""**/*.3DS"",
                ""**/*.fbx"",
                ""**/*.FBX"",
                ""**/*.lxo"",
                ""**/*.LXO"",
                ""**/*.ma"",
                ""**/*.MA"",
                ""**/*.obj"",
                ""**/*.OBJ"",
                ""**/*.asset"",
                ""**/*.cubemap"",
                ""**/*.flare"",
                ""**/*.mat"",
                ""**/*.meta"",
                ""**/*.prefab"",
                ""**/*.unity"",

                ""build/"",
                ""Build/"",
                ""library/"",
                ""Library/"",
                ""obj/"",
                ""Obj/"",
                ""ProjectSettings/"",
                ""UserSettings/"",
                ""temp/"",
                ""Temp/"",
                ""logs"",
                ""Logs""
            ],
            ""project_panel"": {
                ""file_types"": {
                    ""C#"": {
                        ""icon"": ""file_type_csharp""
                    }
                }
            }
        }";
    }
}
