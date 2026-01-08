using Aegis.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Aegis.Security
{
    public static class VaultService
    {
        private static readonly string VaultFolder =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Aegis");

        private static readonly string VaultFile =
            Path.Combine(VaultFolder, "vault.json");

        public static List<VaultEntry> LoadVault(string masterPassword)
        {
            try
            {
                if (!File.Exists(VaultFile))
                    return new List<VaultEntry>();

                string json = File.ReadAllText(VaultFile);
                return JsonSerializer.Deserialize<List<VaultEntry>>(json)
                       ?? new List<VaultEntry>();
            }
            catch
            {
                // Bozuk dosya vs.
                return new List<VaultEntry>();
            }
        }

        public static void SaveVault(string masterPassword, List<VaultEntry> entries)
        {
            try
            {
                if (!Directory.Exists(VaultFolder))
                    Directory.CreateDirectory(VaultFolder);

                string json = JsonSerializer.Serialize(
                    entries,
                    new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(VaultFile, json);
            }
            catch
            {
                // Loglama sonra eklenir
            }
        }
    }
}
