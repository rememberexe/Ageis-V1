using Aegis.Models;
using System;
using System.IO;
using System.Text.Json;

namespace Aegis.Services
{
    public static class SettingsService
    {
        private static readonly string SettingsPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Aegis",
                "settings.json");

        public static AppSettings Current { get; private set; }

        static SettingsService()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    Current = JsonSerializer.Deserialize<AppSettings>(json);
                }
                else
                {
                    Current = new AppSettings();
                    Save();
                }
            }
            catch
            {
                Current = new AppSettings();
            }
        }

        public static void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);

            var json = JsonSerializer.Serialize(Current, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(SettingsPath, json);
        }
    }
}
