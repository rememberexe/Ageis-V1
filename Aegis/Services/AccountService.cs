using Aegis.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Aegis.Services
{
    public static class AccountService
    {
        private static readonly string BasePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Aegis");

        private static readonly string AccountsFile =
            Path.Combine(BasePath, "accounts.json");

        public static List<AegisAccount> LoadAccounts()
        {
            if (!File.Exists(AccountsFile))
                return new List<AegisAccount>();

            var json = File.ReadAllText(AccountsFile);
            return JsonSerializer.Deserialize<List<AegisAccount>>(json);
        }

        public static void SaveAccounts(List<AegisAccount> accounts)
        {
            Directory.CreateDirectory(BasePath);

            var json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(AccountsFile, json);
        }

        public static string GetAccountPath(string username)
        {
            return Path.Combine(BasePath, "Accounts", username);
        }
    }
}
