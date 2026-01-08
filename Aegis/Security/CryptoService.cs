using System;
using System.Security.Cryptography;
using System.Text;

namespace Aegis.Security
{
    public static class CryptoService
    {
        private const int KeySize = 32; // 256 bit
        private const int IvSize = 16;

        // ==============================
        // KEY TÜRET (MASTER PASSWORD → KEY)
        // ==============================
        public static byte[] DeriveKey(
            string masterPassword,
            byte[] salt,
            int iterations)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                masterPassword,
                salt,
                iterations,
                HashAlgorithmName.SHA256
            );

            return pbkdf2.GetBytes(KeySize);
        }

        // ==============================
        // AES ŞİFRELE
        // ==============================
        public static (byte[] cipherText, byte[] iv) Encrypt(
            string plainText,
            byte[] key)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateIV();
            aes.Key = key;

            using var encryptor = aes.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherText = encryptor.TransformFinalBlock(
                plainBytes, 0, plainBytes.Length);

            return (cipherText, aes.IV);
        }

        // ==============================
        // AES ÇÖZ
        // ==============================
        public static string Decrypt(
            byte[] cipherText,
            byte[] iv,
            byte[] key)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.IV = iv;
            aes.Key = key;

            using var decryptor = aes.CreateDecryptor();
            byte[] plainBytes = decryptor.TransformFinalBlock(
                cipherText, 0, cipherText.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
