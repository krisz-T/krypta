using System.Collections.Generic;

namespace krypta
{
    public class PasswordEntry
    {
        public string Title { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class PasswordVault
    {
        public List<PasswordEntry> Entries { get; set; } = new List<PasswordEntry>();
    }
}
