using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace CryptoTerminalLauncher
{
    /// <summary>
    /// Application settings persisted to JSON.
    /// </summary>
    public class ConfigManager
    {
        public string InstallPath { get; set; } = string.Empty;
        public List<string> AddedTerminals { get; set; } = new List<string>();
        public bool AutoCheckUpdates { get; set; } = false;

        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CryptoTerminalLauncher",
            "config.json");

        /// <summary>
        /// Loads configuration from disk. Returns default values if file does not exist.
        /// </summary>
        public static ConfigManager Load()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return new ConfigManager();

                var json = File.ReadAllText(ConfigPath);
                return JsonSerializer.Deserialize<ConfigManager>(json) ?? new ConfigManager();
            }
            catch
            {
                return new ConfigManager();
            }
        }

        /// <summary>
        /// Saves configuration to disk.
        /// </summary>
        public void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(ConfigPath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch
            {
                // Silently ignore save failures
            }
        }
    }
}