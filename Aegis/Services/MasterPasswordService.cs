using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace Aegis.Security
{
    public static class MasterPasswordService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        private static readonly string BasePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Aegis");

        private static readonly string MasterPath =
            Path.Combine(BasePath, "master.json");

        public static bool MasterExists()
            => File.Exists(MasterPath);

        public static void CreateMasterPassword(string password)
        {
            Directory.CreateDirectory(BasePath);

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, Iterations, HashAlgorithmName.SHA256);

            var data = new MasterData
            {
                Salt = Convert.ToBase64String(salt),
                Hash = Convert.ToBase64String(pbkdf2.GetBytes(KeySize)),
                Iterations = Iterations
            };

            File.WriteAllText(
                MasterPath,
                JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static bool VerifyMasterPassword(string password)
        {
            if (!MasterExists())
                return false;

            var data = JsonSerializer.Deserialize<MasterData>(
                File.ReadAllText(MasterPath))!;

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                Convert.FromBase64String(data.Salt),
                data.Iterations,
                HashAlgorithmName.SHA256);

            var hash = pbkdf2.GetBytes(KeySize);

            return CryptographicOperations.FixedTimeEquals(
                hash, Convert.FromBase64String(data.Hash));
        }

        public static byte[] DeriveEncryptionKey(string password)
        {
            var data = JsonSerializer.Deserialize<MasterData>(
                File.ReadAllText(MasterPath))!;

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                Convert.FromBase64String(data.Salt),
                data.Iterations,
                HashAlgorithmName.SHA256);

            return pbkdf2.GetBytes(KeySize);
        }
    }

    public class MasterData
    {
        public string Salt { get; set; } = "";
        public string Hash { get; set; } = "";
        public int Iterations { get; set; }
    }
}
