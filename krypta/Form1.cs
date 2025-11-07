using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Linq;
using System.Drawing;

namespace krypta
{
    public partial class Form1 : Form
    {
        private PasswordVault _vault = new PasswordVault();
        private BindingList<PasswordEntry> _entries;
        private string _currentFilePath = null;
        private bool _isDirty = false;
        private bool _isLocked = false;
        private bool _suppressAutoLock = false;
        private string _currentMasterPassword = null;

        public Form1()
        {
            InitializeComponent();

            //Auto-lock wired, regardless of the Designer
            this.Deactivate += Form1_Deactivate;

            dataGridView1.AutoGenerateColumns = true;
            ApplyDataGridViewStyle();
            StartNewVault();
        }
        private void ApplyDataGridViewStyle()
        {
            // --- General setup ---

            dataGridView1.MultiSelect = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.GridColor = Color.LightGray;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // --- Column header styling ---
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Headers shouldn't change color when selected
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

            // --- Default cell styling ---
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Strong selection color
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            // --- Light alternate rows for readability ---
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 248);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            dataGridView1.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // --- Row layout ---
            dataGridView1.RowTemplate.Height = 24;

        }

        private void StartNewVault()
        {
            _vault = new PasswordVault();
            BindVaultToGrid();
            _currentFilePath = null;
            _isDirty = false;
            _isLocked = false;
            _currentMasterPassword = null;
            UpdateTitle();
            UpdateStatusBar();
        }

        private void BindVaultToGrid()
        {
            _entries = new BindingList<PasswordEntry>(_vault.Entries);
            _entries.ListChanged += Entries_ListChanged;
            dataGridView1.DataSource = _entries;

            // --- Configure columns ---
            dataGridView1.Columns["Title"].HeaderText = "Title / Website";
            dataGridView1.Columns["Username"].HeaderText = "Username / Email";
            dataGridView1.Columns["Password"].HeaderText = "Password";
            dataGridView1.Columns["Notes"].HeaderText = "Notes";

            // Make password column show ••••• instead of text
            DataGridViewTextBoxColumn pwdColumn =
                (DataGridViewTextBoxColumn)dataGridView1.Columns["Password"];
            pwdColumn.DefaultCellStyle.Format = "password"; // just for label clarity
            pwdColumn.DefaultCellStyle.NullValue = null;

            // Replace the password column’s cell template
            var cellTemplate = new DataGridViewTextBoxCell();
            pwdColumn.CellTemplate = cellTemplate;

            // Autosize columns
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return; // skip header

            var columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "Password")
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var entry = row.DataBoundItem as PasswordEntry;
                string actualPassword = entry?.Password;

                // 1) Color based on strength if the strength meter is enabled
                if (checkBoxStrengthMeter.Checked && !string.IsNullOrEmpty(actualPassword))
                {
                    int score = GetPasswordScore(actualPassword);
                    e.CellStyle.BackColor = GetStrengthColor(score);
                }
                else
                {
                    // Strength meter off or empty password -> normal background
                    e.CellStyle.BackColor = SystemColors.Window;
                }

                // 2) Mask or show password text based on Show Passwords checkbox
                if (!checkBoxShowPasswords.Checked &&
                    e.Value is string val &&
                    !string.IsNullOrEmpty(val))
                {
                    // Mask it
                    e.Value = new string('•', val.Length);
                    e.FormattingApplied = true;
                }
            }
        }

        private void Entries_ListChanged(object sender, ListChangedEventArgs e)
        {
            MarkDirty();
            UpdateStatusBar();
        }

        private void LoadVault(string path)
        {
            string password = PromptForPassword("Enter vault password:");
            if (password == null)
                return;

            var vault = VaultFileService.Load(path, password);
            if (vault == null)
            {
                MessageBox.Show("Incorrect password or invalid vault file.",
                                "Krypta",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _vault = vault;
            BindVaultToGrid();
            _currentFilePath = path;
            _isDirty = false;
            _isLocked = false;

            _currentMasterPassword = password;   // <-- remember the correct master

            UpdateTitle();
            UpdateStatusBar();
        }

        private void SaveVault(string path)
        {
            // If we don't yet have a master password (brand new vault), ask to set one
            if (string.IsNullOrEmpty(_currentMasterPassword))
            {
                string newPassword = PromptForNewMasterPassword();
                if (newPassword == null)
                    return; // user cancelled

                _currentMasterPassword = newPassword;
            }

            // Use the current master password to encrypt and save
            VaultFileService.Save(path, _vault, _currentMasterPassword);
            _currentFilePath = path;
            _isDirty = false;

            UpdateTitle();
            UpdateStatusBar();
        }

        private bool SaveCurrentVault()
        {
            // If we already have a file name, just save to it
            if (!string.IsNullOrEmpty(_currentFilePath))
            {
                SaveVault(_currentFilePath);
                return true;
            }

            // Otherwise, act like Save As
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Krypta vault (*.kry)|*.kry|All files (*.*)|*.*";
                sfd.DefaultExt = "kry";
                sfd.AddExtension = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SaveVault(sfd.FileName);
                    return true;
                }
            }

            // User cancelled Save As
            return false;
        }

        private bool ConfirmSaveIfDirty()
        {
            if (!_isDirty)
                return true; // nothing to save

            DialogResult result;

            _suppressAutoLock = true;
            try
            {
                result = MessageBox.Show(this,
                    "You have unsaved changes. Do you want to save them?",
                    "Krypta",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                _suppressAutoLock = false;
            }

            if (result == DialogResult.Cancel)
                return false; // stop the action

            if (result == DialogResult.No)
                return true; // discard and continue

            // YES -> save
            bool saved = SaveCurrentVault();
            return saved; // if user cancels during Save As, saved will be false
        }

        private void UpdateTitle()
        {
            string fileName = _currentFilePath == null ? "Untitled" : Path.GetFileName(_currentFilePath);
            string dirtyMark = _isDirty ? "*" : "";
            Text = $"Krypta - {fileName}{dirtyMark}";
        }

        private void LockVault()
        {
            // Nothing to do
            if (_isLocked)
                return;

            // IMPORTANT:
            // Do NOT auto-lock if there are unsaved changes.
            // This avoids silently discarding edits when the user alt-tabs.
            if (_isDirty)
                return;

            // No vault loaded or nothing to lock
            if (string.IsNullOrEmpty(_currentFilePath) ||
                _vault == null ||
                _vault.Entries == null ||
                _vault.Entries.Count == 0)
                return;

            _isLocked = true;

            // Clear sensitive data from memory
            _vault = new PasswordVault();
            BindVaultToGrid();
            textBoxSearch.Text = string.Empty;
            checkBoxShowPasswords.Checked = false;
            _isDirty = false;

            _currentMasterPassword = null;

            UpdateTitle();
            UpdateStatusBar();
        }

        private void UnlockVault()
        {
            if (!_isLocked)
                return;

            if (string.IsNullOrEmpty(_currentFilePath))
            {
                MessageBox.Show("No vault file to reopen. Use File -> Open.", "Krypta");
                return;
            }

            // LoadVault will set _isLocked = false and update the status bar
            LoadVault(_currentFilePath);
        }

        private void MarkDirty()
        {
            if (!_isDirty)
            {
                _isDirty = true;
                UpdateTitle();
            }
        }

        private string PromptForNewMasterPassword()
        {
            using (var form = new Form())
            {
                form.Text = "Krypta - Set Master Password";
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.ClientSize = new Size(320, 160);

                var label1 = new Label
                {
                    Left = 10,
                    Top = 10,
                    Text = "Enter new master password:",
                    AutoSize = true
                };

                var textBox1 = new TextBox
                {
                    Left = 10,
                    Top = 30,
                    Width = 290,
                    PasswordChar = '•'
                };

                var label2 = new Label
                {
                    Left = 10,
                    Top = 60,
                    Text = "Confirm master password:",
                    AutoSize = true
                };

                var textBox2 = new TextBox
                {
                    Left = 10,
                    Top = 80,
                    Width = 290,
                    PasswordChar = '•'
                };

                var okButton = new Button
                {
                    Text = "OK",
                    Left = 145,
                    Width = 75,
                    Top = 115
                };

                var cancelButton = new Button
                {
                    Text = "Cancel",
                    Left = 230,
                    Width = 75,
                    Top = 115,
                    DialogResult = DialogResult.Cancel
                };

                // Validation logic for OK
                okButton.Click += (s, e) =>
                {
                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        MessageBox.Show(form,
                            "Master password cannot be empty.",
                            "Krypta",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    if (textBox1.Text != textBox2.Text)
                    {
                        MessageBox.Show(form,
                            "Passwords do not match.",
                            "Krypta",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };

                form.Controls.AddRange(new Control[] { label1, textBox1, label2, textBox2, okButton, cancelButton });
                form.AcceptButton = okButton;
                form.CancelButton = cancelButton;

                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    return textBox1.Text;
                }

                return null; // user cancelled
            }
        }


        private string GeneratePassword(int length = 16)
        {
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string symbols = "!@#$%^&*()-_=+[]{};:,.<>/?";

            string allChars = lower + upper + digits + symbols;

            if (length <= 0)
                throw new ArgumentException("Password length must be positive.", nameof(length));

            // Use a secure random number generator
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);

            var chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = allChars[bytes[i] % allChars.Length];
            }

            return new string(chars);
        }

        private string PromptForPassword(string message)
        {
            using (var form = new Form())
            {
                form.Text = "Krypta - Password";
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MinimizeBox = false;
                form.MaximizeBox = false;

                var label = new Label
                {
                    Left = 10,
                    Top = 10,
                    Text = message,
                    AutoSize = true
                };

                var textBox = new TextBox
                {
                    Left = 10,
                    Top = 35,
                    Width = 260,
                    PasswordChar = '•'
                };

                var okButton = new Button
                {
                    Text = "OK",
                    Left = 110,
                    Width = 75,
                    Top = 70,
                    DialogResult = DialogResult.OK
                };

                var cancelButton = new Button
                {
                    Text = "Cancel",
                    Left = 195,
                    Width = 75,
                    Top = 70,
                    DialogResult = DialogResult.Cancel
                };

                form.ClientSize = new System.Drawing.Size(280, 110);
                form.Controls.AddRange(new Control[] { label, textBox, okButton, cancelButton });
                form.AcceptButton = okButton;
                form.CancelButton = cancelButton;

                _suppressAutoLock = true;
                try
                {
                    var result = form.ShowDialog(this);
                    return result == DialogResult.OK ? textBox.Text : null;
                }
                finally
                {
                    _suppressAutoLock = false;
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmSaveIfDirty())
                return;

            StartNewVault();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmSaveIfDirty())
                return;

            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Krypta vault (*.kry)|*.kry|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadVault(ofd.FileName);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                SaveVault(_currentFilePath);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Krypta vault (*.kry)|*.kry|All files (*.*)|*.*";
                sfd.DefaultExt = "kry";
                sfd.AddExtension = true;

                if (!string.IsNullOrEmpty(_currentFilePath))
                {
                    // Suggest current name but with .kry extension
                    var name = Path.GetFileNameWithoutExtension(_currentFilePath);
                    sfd.FileName = name + ".kry";
                }

                if (sfd.ShowDialog() == DialogResult.OK)
                    SaveVault(sfd.FileName);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmSaveIfDirty())
                return;

            StartNewVault();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string query = textBoxSearch.Text.Trim().ToLower();

            // Get the CurrencyManager for the grid's data source
            var cm = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            cm.SuspendBinding();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                var entry = row.DataBoundItem as PasswordEntry;
                if (entry == null)
                    continue;

                bool matches =
                    string.IsNullOrEmpty(query) ||
                    (entry.Title?.ToLower().Contains(query) ?? false) ||
                    (entry.Username?.ToLower().Contains(query) ?? false) ||
                    (entry.Notes?.ToLower().Contains(query) ?? false);

                row.Visible = matches;
            }

            cm.ResumeBinding();

            // if current row got hidden, clear selection
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.Visible)
            {
                dataGridView1.CurrentCell = null;
            }
        }

        private void checkBoxShowPasswords_CheckedChanged_1(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            // Do not auto-lock during our own dialogs
            if (_suppressAutoLock)
                return;

            // Just try to lock; LockVault itself will decide
            LockVault();
        }

        private void unlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnlockVault();
        }

        private void ShowInfo(string text)
        {
            _suppressAutoLock = true;
            try
            {
                MessageBox.Show(this, text, "Krypta",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            finally
            {
                _suppressAutoLock = false;
            }
        }

        private void buttonGeneratePassword_Click(object sender, EventArgs e)
        {
            GeneratePasswordForSelectedEntry();
        }

        private void UpdateStatusBar()
        {
            // Shows current file or "Untitled"
            string fileName = string.IsNullOrEmpty(_currentFilePath)
                ? "File: [Unsaved / New]"
                : $"File: {Path.GetFileName(_currentFilePath)}";

            toolStripStatusLabelFile.Text = fileName;

            // Entry count
            int total = _vault?.Entries?.Count ?? 0;
            toolStripStatusLabelEntries.Text = $"Entries: {total}";

            // Lock status
            if (_isLocked)
            {
                toolStripStatusLabelLock.Text = "Status: Locked";
                toolStripStatusLabelLock.BackColor = Color.FromArgb(255, 128, 128); // soft red
                toolStripStatusLabelLock.ForeColor = Color.Black;
            }
            else
            {
                toolStripStatusLabelLock.Text = "Status: Unlocked";
                toolStripStatusLabelLock.BackColor = SystemColors.Control;
                toolStripStatusLabelLock.ForeColor = Color.Black;
            }
        }

        private int GetPasswordScore(string password)
        {
            if (string.IsNullOrEmpty(password))
                return 0;

            int score = 0;
            int length = password.Length;

            // Length contribution
            if (length >= 16)
                score += 50;
            else if (length >= 12)
                score += 40;
            else if (length >= 8)
                score += 25;
            else if (length >= 4)
                score += 10;
            // shorter than 4 -> no length bonus

            // Character variety
            bool hasLower = password.Any(char.IsLower);
            bool hasUpper = password.Any(char.IsUpper);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(ch => !char.IsLetterOrDigit(ch));

            int varietyCount = 0;
            if (hasLower) varietyCount++;
            if (hasUpper) varietyCount++;
            if (hasDigit) varietyCount++;
            if (hasSymbol) varietyCount++;

            score += varietyCount * 12; // up to +48

            // Cap at 100
            if (score > 100)
                score = 100;

            return score;
        }

        private Color GetStrengthColor(int score)
        {
            // Weak: red-ish
            if (score < 30)
                return Color.LightCoral;

            // Medium: yellow-ish
            if (score < 60)
                return Color.Khaki;

            // Strong: green-ish
            return Color.LightGreen;
        }

        private void checkBoxStrengthMeter_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmSaveIfDirty())
            {
                e.Cancel = true;
                return;
            }

            _currentMasterPassword = null;
        }

        private void toolStripStatusLabelLock_Click(object sender, EventArgs e)
        {
            // Only do something if actually locked
            if (_isLocked)
            {
                UnlockVault();
            }
        }

        private void GeneratePasswordForSelectedEntry()
        {
            var entry = GetSelectedEntry();
            if (entry == null)
                return;

            bool hasExistingPassword = !string.IsNullOrEmpty(entry.Password);

            // If there's already a password, ask before replacing
            if (hasExistingPassword)
            {
                _suppressAutoLock = true;
                try
                {
                    var result = MessageBox.Show(this,
                        "This entry already has a password.\nDo you want to replace it?",
                        "Krypta",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                        return;
                }
                finally
                {
                    _suppressAutoLock = false;
                }
            }

            // Generate a new password
            string password = GeneratePassword(16); // or whatever length you prefer
            entry.Password = password;
            MarkDirty();
            dataGridView1.Refresh();

            // Try copying to clipboard
            try
            {
                Clipboard.SetText(password);
            }
            catch
            {
                // ignore clipboard errors
            }

            // Show a temporary status message in green
            toolStripStatusLabelLock.Text = "Generated (copied)";
            toolStripStatusLabelLock.BackColor = Color.LightGreen;
            toolStripStatusLabelLock.ForeColor = Color.Black;

            // Set up a timer to reset the status after a few seconds
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 2500; // 2.5 seconds, adjust to taste
            timer.Tick += (s, ev) =>
            {
                timer.Stop();
                timer.Dispose();
                // Restore normal status (Locked/Unlocked + normal colors)
                UpdateStatusBar();
            };
            timer.Start();
        }

        private void DeleteSelectedEntry()
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.IsNewRow)
                return;

            var entry = dataGridView1.CurrentRow.DataBoundItem as PasswordEntry;
            if (entry == null)
                return;

            // Confirm delete
            _suppressAutoLock = true;
            try
            {
                var result = MessageBox.Show(this,
                    "Delete this entry?",
                    "Krypta",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;
            }
            finally
            {
                _suppressAutoLock = false;
            }

            // Remove from BindingList (this will update the grid and mark dirty via Entries_ListChanged)
            _entries.Remove(entry);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedEntry();
                e.Handled = true;
            }
        }

        private void deleteSelectedEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedEntry();
        }

        private void lockVaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If already locked, nothing to do
            if (_isLocked)
                return;

            // If there are unsaved changes, ask the user what to do
            if (_isDirty)
            {
                DialogResult result;

                _suppressAutoLock = true;
                try
                {
                    result = MessageBox.Show(this,
                        "You have unsaved changes. Save before locking the vault?",
                        "Krypta",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning);
                }
                finally
                {
                    _suppressAutoLock = false;
                }

                if (result == DialogResult.Cancel)
                {
                    // User changed their mind
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    // Try to save. If user cancels Save As, don't lock.
                    if (!SaveCurrentVault())
                        return; // save aborted -> do not lock

                    // SaveCurrentVault sets _isDirty = false on success
                }
                else if (result == DialogResult.No)
                {
                    // Discard changes explicitly
                    _isDirty = false;
                    UpdateTitle();
                }
            }

            // Now it's safe to lock: either there were no changes, or user
            // chose Yes (and we saved), or No (and we discarded)
            LockVault();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxSearch.Focus();
            textBoxSearch.SelectAll();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string aboutText =
        "Krypta - Encrypted Password Manager\n\n" +
        "Version: 1.0\n\n" +
        "Krypta is a Windows Forms application designed to securely\n" +
        "store, generate, and manage passwords using AES encryption.\n\n" +
        "Features:\n" +
        "- Master password and encrypted vaults (.kry)\n" +
        "- Auto-lock on inactivity\n" +
        "- Password generator and strength meter\n" +
        "- Search, show/hide, and data editing\n" +
        "- Among others\n\n" +
        "Disclaimer:\n" +
        "This program is provided for educational use only.\n" +
        "Do not use Krypta to store real-world sensitive credentials.";

            _suppressAutoLock = true;
            try
            {
                MessageBox.Show(this,
                    aboutText,
                    "About Krypta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            finally
            {
                _suppressAutoLock = false;
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Only respond to right-clicks on valid rows
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridView1.ClearSelection();
                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex >= 0 ? e.ColumnIndex : 0];
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        private PasswordEntry GetSelectedEntry()
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.IsNewRow)
                return null;

            return dataGridView1.CurrentRow.DataBoundItem as PasswordEntry;
        }

        private void copyUsernameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var entry = GetSelectedEntry();
            if (entry == null || string.IsNullOrEmpty(entry.Username))
                return;

            try
            {
                Clipboard.SetText(entry.Username);
                toolStripStatusLabelLock.Text = "Username copied";
            }
            catch
            {
                // ignore clipboard issues
            }
        }

        private void copyPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var entry = GetSelectedEntry();
            if (entry == null || string.IsNullOrEmpty(entry.Password))
                return;

            try
            {
                Clipboard.SetText(entry.Password);
                toolStripStatusLabelLock.Text = "Password copied";
            }
            catch
            {
                // ignore clipboard issues
            }
        }

        private void generatePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeneratePasswordForSelectedEntry();
        }

        private void deleteEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedEntry();
        }
    }
}
