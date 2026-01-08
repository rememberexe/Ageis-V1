using Aegis.Models;
using System.IO;
using System.Text.Json;

namespace Aegis.Services.Persistence
{
    public static class VaultPersistenceService
    {
        private static readonly string VaultFilePath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Aegis",
                "vault.json");

        public static void Save(IEnumerable<VaultEntry> entries)
        {
            Directory.CreateDirectory(
                Path.GetDirectoryName(VaultFilePath)!);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(entries, options);
            File.WriteAllText(VaultFilePath, json);
        }

        public static List<VaultEntry> Load()
        {
            if (!File.Exists(VaultFilePath))
                return new List<VaultEntry>();

            string json = File.ReadAllText(VaultFilePath);
            return JsonSerializer.Deserialize<List<VaultEntry>>(json)
                   ?? new List<VaultEntry>();
        }
    }
}
