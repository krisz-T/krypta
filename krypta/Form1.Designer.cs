namespace krypta
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            vaultToolStripMenuItem = new ToolStripMenuItem();
            deleteSelectedEntryToolStripMenuItem = new ToolStripMenuItem();
            lockVaultToolStripMenuItem = new ToolStripMenuItem();
            unlockToolStripMenuItem = new ToolStripMenuItem();
            findToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            checkBoxShowPasswords = new CheckBox();
            textBoxSearch = new TextBox();
            dataGridView1 = new DataGridView();
            tableLayoutPanel1 = new TableLayoutPanel();
            buttonGeneratePassword = new Button();
            checkBoxStrengthMeter = new CheckBox();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabelFile = new ToolStripStatusLabel();
            toolStripStatusLabelEntries = new ToolStripStatusLabel();
            toolStripStatusLabelLock = new ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, vaultToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, closeToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newToolStripMenuItem.Size = new Size(186, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(186, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(186, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            saveAsToolStripMenuItem.Size = new Size(186, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.W;
            closeToolStripMenuItem.Size = new Size(186, 22);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(183, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Q;
            exitToolStripMenuItem.Size = new Size(186, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // vaultToolStripMenuItem
            // 
            vaultToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { deleteSelectedEntryToolStripMenuItem, lockVaultToolStripMenuItem, unlockToolStripMenuItem, findToolStripMenuItem });
            vaultToolStripMenuItem.Name = "vaultToolStripMenuItem";
            vaultToolStripMenuItem.Size = new Size(45, 20);
            vaultToolStripMenuItem.Text = "Vault";
            // 
            // deleteSelectedEntryToolStripMenuItem
            // 
            deleteSelectedEntryToolStripMenuItem.Name = "deleteSelectedEntryToolStripMenuItem";
            deleteSelectedEntryToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
            deleteSelectedEntryToolStripMenuItem.Size = new Size(225, 22);
            deleteSelectedEntryToolStripMenuItem.Text = "Delete selected entry";
            deleteSelectedEntryToolStripMenuItem.Click += deleteSelectedEntryToolStripMenuItem_Click;
            // 
            // lockVaultToolStripMenuItem
            // 
            lockVaultToolStripMenuItem.Name = "lockVaultToolStripMenuItem";
            lockVaultToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.L;
            lockVaultToolStripMenuItem.Size = new Size(225, 22);
            lockVaultToolStripMenuItem.Text = "Lock vault";
            lockVaultToolStripMenuItem.Click += lockVaultToolStripMenuItem_Click;
            // 
            // unlockToolStripMenuItem
            // 
            unlockToolStripMenuItem.Name = "unlockToolStripMenuItem";
            unlockToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.U;
            unlockToolStripMenuItem.Size = new Size(225, 22);
            unlockToolStripMenuItem.Text = "Unlock vault";
            unlockToolStripMenuItem.Click += unlockToolStripMenuItem_Click;
            // 
            // findToolStripMenuItem
            // 
            findToolStripMenuItem.Name = "findToolStripMenuItem";
            findToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            findToolStripMenuItem.Size = new Size(225, 22);
            findToolStripMenuItem.Text = "Find";
            findToolStripMenuItem.Visible = false;
            findToolStripMenuItem.Click += findToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.ShortcutKeys = Keys.F1;
            aboutToolStripMenuItem.Size = new Size(126, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // checkBoxShowPasswords
            // 
            checkBoxShowPasswords.AutoSize = true;
            checkBoxShowPasswords.Dock = DockStyle.Fill;
            checkBoxShowPasswords.Location = new Point(324, 8);
            checkBoxShowPasswords.Name = "checkBoxShowPasswords";
            checkBoxShowPasswords.Size = new Size(112, 23);
            checkBoxShowPasswords.TabIndex = 3;
            checkBoxShowPasswords.Text = "Show passwords";
            checkBoxShowPasswords.UseVisualStyleBackColor = true;
            checkBoxShowPasswords.CheckedChanged += checkBoxShowPasswords_CheckedChanged_1;
            // 
            // textBoxSearch
            // 
            textBoxSearch.Dock = DockStyle.Fill;
            textBoxSearch.Location = new Point(8, 8);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.PlaceholderText = "Search...";
            textBoxSearch.Size = new Size(310, 23);
            textBoxSearch.TabIndex = 5;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanel1.SetColumnSpan(dataGridView1, 4);
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(8, 37);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(784, 381);
            dataGridView1.TabIndex = 4;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.KeyDown += dataGridView1_KeyDown;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CausesValidation = false;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Controls.Add(dataGridView1, 1, 1);
            tableLayoutPanel1.Controls.Add(textBoxSearch, 0, 0);
            tableLayoutPanel1.Controls.Add(checkBoxShowPasswords, 1, 0);
            tableLayoutPanel1.Controls.Add(buttonGeneratePassword, 3, 0);
            tableLayoutPanel1.Controls.Add(checkBoxStrengthMeter, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 24);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(5);
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(800, 426);
            tableLayoutPanel1.TabIndex = 6;
            // 
            // buttonGeneratePassword
            // 
            buttonGeneratePassword.Dock = DockStyle.Fill;
            buttonGeneratePassword.FlatStyle = FlatStyle.Flat;
            buttonGeneratePassword.Location = new Point(560, 8);
            buttonGeneratePassword.Name = "buttonGeneratePassword";
            buttonGeneratePassword.Size = new Size(232, 23);
            buttonGeneratePassword.TabIndex = 6;
            buttonGeneratePassword.Text = "Generate";
            buttonGeneratePassword.UseVisualStyleBackColor = true;
            buttonGeneratePassword.Click += buttonGeneratePassword_Click;
            // 
            // checkBoxStrengthMeter
            // 
            checkBoxStrengthMeter.AutoSize = true;
            checkBoxStrengthMeter.Dock = DockStyle.Fill;
            checkBoxStrengthMeter.Location = new Point(442, 8);
            checkBoxStrengthMeter.Name = "checkBoxStrengthMeter";
            checkBoxStrengthMeter.Size = new Size(112, 23);
            checkBoxStrengthMeter.TabIndex = 7;
            checkBoxStrengthMeter.Text = "Strength meter";
            checkBoxStrengthMeter.UseVisualStyleBackColor = true;
            checkBoxStrengthMeter.CheckedChanged += checkBoxStrengthMeter_CheckedChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = Color.FromArgb(240, 240, 245);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabelFile, toolStripStatusLabelEntries, toolStripStatusLabelLock });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 7;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelFile
            // 
            toolStripStatusLabelFile.Name = "toolStripStatusLabelFile";
            toolStripStatusLabelFile.Size = new Size(28, 17);
            toolStripStatusLabelFile.Text = "File:";
            toolStripStatusLabelFile.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelEntries
            // 
            toolStripStatusLabelEntries.AutoSize = false;
            toolStripStatusLabelEntries.Name = "toolStripStatusLabelEntries";
            toolStripStatusLabelEntries.Size = new Size(662, 17);
            toolStripStatusLabelEntries.Spring = true;
            toolStripStatusLabelEntries.Text = "Entries: 0";
            toolStripStatusLabelEntries.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelLock
            // 
            toolStripStatusLabelLock.AutoSize = false;
            toolStripStatusLabelLock.Name = "toolStripStatusLabelLock";
            toolStripStatusLabelLock.Size = new Size(95, 17);
            toolStripStatusLabelLock.Text = "Status: Unlocked";
            toolStripStatusLabelLock.TextAlign = ContentAlignment.MiddleRight;
            toolStripStatusLabelLock.Click += toolStripStatusLabelLock_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 250);
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip1);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "KryPass";
            Deactivate += Form1_Deactivate;
            FormClosing += Form1_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private DataGridView dataGridView1;
        private CheckBox checkBoxShowPasswords;
        private TextBox textBoxSearch;
        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonGeneratePassword;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelEntries;
        private ToolStripStatusLabel toolStripStatusLabelLock;
        private CheckBox checkBoxStrengthMeter;
        private ToolStripStatusLabel toolStripStatusLabelFile;
        private ToolStripMenuItem vaultToolStripMenuItem;
        private ToolStripMenuItem deleteSelectedEntryToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem lockVaultToolStripMenuItem;
        private ToolStripMenuItem unlockToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem findToolStripMenuItem;
    }
}
