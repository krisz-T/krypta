using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace krypta
{
    public static class CryptoService
    {
        // Fixed salt for demo purposes (educational, not production secure)
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("KryptaSalt1234");

        public static string Encrypt(string plainText, string password)
        {
            using var aes = Aes.Create();
            var key = new Rfc2898DeriveBytes(password, Salt, 10000);
            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText, string password)
        {
            try
            {
                var bytes = Convert.FromBase64String(cipherText);
                using var aes = Aes.Create();
                var key = new Rfc2898DeriveBytes(password, Salt, 10000);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using var ms = new MemoryStream(bytes);
                using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch
            {
                // Wrong password or corrupt file
                return null;
            }
        }
    }
}
