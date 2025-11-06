using System.IO;
using System.Text.Json;

namespace krypta
{
    public static class VaultFileService
    {
        public static PasswordVault Load(string path, string password)
        {
            if (!File.Exists(path))
                return new PasswordVault();

            string encrypted = File.ReadAllText(path);
            string json = CryptoService.Decrypt(encrypted, password);

            if (json == null)
                return null; // wrong password or invalid data

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var vault = JsonSerializer.Deserialize<PasswordVault>(json, options);
            return vault ?? new PasswordVault();
        }

        public static void Save(string path, PasswordVault vault, string password)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(vault, options);
            string encrypted = CryptoService.Encrypt(json, password);

            File.WriteAllText(path, encrypted);
        }
    }
}
