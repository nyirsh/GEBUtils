namespace TR2Edit
{
    partial class TR2EditForm
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataTable = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tR2MergerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accentsCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noCompressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedRowControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnListBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.allToReplaceChecks = new System.Windows.Forms.CheckBox();
            this.replaceAllButton = new System.Windows.Forms.Button();
            this.search_replaceBox = new System.Windows.Forms.TextBox();
            this.findButton = new System.Windows.Forms.Button();
            this.search_valueBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataTable
            // 
            this.dataTable.AllowUserToAddRows = false;
            this.dataTable.AllowUserToDeleteRows = false;
            this.dataTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataTable.Location = new System.Drawing.Point(12, 38);
            this.dataTable.MultiSelect = false;
            this.dataTable.Name = "dataTable";
            this.dataTable.RowHeadersVisible = false;
            this.dataTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataTable.ShowEditingIcon = false;
            this.dataTable.Size = new System.Drawing.Size(918, 290);
            this.dataTable.TabIndex = 0;
            this.dataTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataTable_CellValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(942, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(111, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tR2MergerToolStripMenuItem,
            this.accentsCheckToolStripMenuItem});
            this.toolsToolStripMenuItem.Enabled = false;
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // tR2MergerToolStripMenuItem
            // 
            this.tR2MergerToolStripMenuItem.Name = "tR2MergerToolStripMenuItem";
            this.tR2MergerToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.tR2MergerToolStripMenuItem.Text = "TR2 Merger";
            this.tR2MergerToolStripMenuItem.Click += new System.EventHandler(this.tR2MergerToolStripMenuItem_Click);
            // 
            // accentsCheckToolStripMenuItem
            // 
            this.accentsCheckToolStripMenuItem.Name = "accentsCheckToolStripMenuItem";
            this.accentsCheckToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.accentsCheckToolStripMenuItem.Text = "Accents Replace";
            this.accentsCheckToolStripMenuItem.Click += new System.EventHandler(this.accentsCheckToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compressionToolStripMenuItem,
            this.advancedRowControlToolStripMenuItem});
            this.optionsToolStripMenuItem.Enabled = false;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // compressionToolStripMenuItem
            // 
            this.compressionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noCompressionToolStripMenuItem,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.compressionToolStripMenuItem.Name = "compressionToolStripMenuItem";
            this.compressionToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.compressionToolStripMenuItem.Text = "Compression Level";
            // 
            // noCompressionToolStripMenuItem
            // 
            this.noCompressionToolStripMenuItem.Checked = true;
            this.noCompressionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noCompressionToolStripMenuItem.Name = "noCompressionToolStripMenuItem";
            this.noCompressionToolStripMenuItem.Size = new System.Drawing.Size(80, 22);
            this.noCompressionToolStripMenuItem.Text = "0";
            this.noCompressionToolStripMenuItem.Click += new System.EventHandler(this.noCompressionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(80, 22);
            this.toolStripMenuItem3.Text = "1";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(80, 22);
            this.toolStripMenuItem4.Text = "2";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // advancedRowControlToolStripMenuItem
            // 
            this.advancedRowControlToolStripMenuItem.Name = "advancedRowControlToolStripMenuItem";
            this.advancedRowControlToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.advancedRowControlToolStripMenuItem.Text = "Advanced Row Control";
            this.advancedRowControlToolStripMenuItem.Click += new System.EventHandler(this.advancedRowControlToolStripMenuItem_Click);
            // 
            // columnListBox
            // 
            this.columnListBox.Enabled = false;
            this.columnListBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.columnListBox.FormattingEnabled = true;
            this.columnListBox.Location = new System.Drawing.Point(56, 4);
            this.columnListBox.Name = "columnListBox";
            this.columnListBox.Size = new System.Drawing.Size(181, 21);
            this.columnListBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Column:";
            // 
            // searchPanel
            // 
            this.searchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.searchPanel.Controls.Add(this.allToReplaceChecks);
            this.searchPanel.Controls.Add(this.replaceAllButton);
            this.searchPanel.Controls.Add(this.search_replaceBox);
            this.searchPanel.Controls.Add(this.findButton);
            this.searchPanel.Controls.Add(this.search_valueBox);
            this.searchPanel.Controls.Add(this.label2);
            this.searchPanel.Controls.Add(this.label1);
            this.searchPanel.Controls.Add(this.columnListBox);
            this.searchPanel.Location = new System.Drawing.Point(12, 328);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(918, 26);
            this.searchPanel.TabIndex = 4;
            // 
            // allToReplaceChecks
            // 
            this.allToReplaceChecks.AutoSize = true;
            this.allToReplaceChecks.Enabled = false;
            this.allToReplaceChecks.Location = new System.Drawing.Point(873, 7);
            this.allToReplaceChecks.Name = "allToReplaceChecks";
            this.allToReplaceChecks.Size = new System.Drawing.Size(37, 17);
            this.allToReplaceChecks.TabIndex = 9;
            this.allToReplaceChecks.Text = "All";
            this.allToReplaceChecks.UseVisualStyleBackColor = true;
            // 
            // replaceAllButton
            // 
            this.replaceAllButton.Enabled = false;
            this.replaceAllButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.replaceAllButton.Location = new System.Drawing.Point(810, 3);
            this.replaceAllButton.Name = "replaceAllButton";
            this.replaceAllButton.Size = new System.Drawing.Size(57, 23);
            this.replaceAllButton.TabIndex = 8;
            this.replaceAllButton.Text = "Replace";
            this.replaceAllButton.UseVisualStyleBackColor = true;
            this.replaceAllButton.Click += new System.EventHandler(this.replaceAllButton_Click);
            // 
            // search_replaceBox
            // 
            this.search_replaceBox.Enabled = false;
            this.search_replaceBox.Location = new System.Drawing.Point(594, 4);
            this.search_replaceBox.Name = "search_replaceBox";
            this.search_replaceBox.Size = new System.Drawing.Size(211, 20);
            this.search_replaceBox.TabIndex = 7;
            // 
            // findButton
            // 
            this.findButton.Enabled = false;
            this.findButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findButton.Location = new System.Drawing.Point(514, 3);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(49, 23);
            this.findButton.TabIndex = 6;
            this.findButton.Text = "Find";
            this.findButton.UseVisualStyleBackColor = true;
            this.findButton.Click += new System.EventHandler(this.findButton_Click);
            // 
            // search_valueBox
            // 
            this.search_valueBox.Enabled = false;
            this.search_valueBox.Location = new System.Drawing.Point(297, 4);
            this.search_valueBox.Name = "search_valueBox";
            this.search_valueBox.Size = new System.Drawing.Size(211, 20);
            this.search_valueBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(254, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Value:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(826, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "v1.3.1 by DarkVanth";
            // 
            // TR2EditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 357);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.searchPanel);
            this.Controls.Add(this.dataTable);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TR2EditForm";
            this.Text = "TR2 Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TR2EditForm_FormClosing);
            this.Resize += new System.EventHandler(this.TR2EditForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noCompressionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        public System.Windows.Forms.DataGridView dataTable;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tR2MergerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedRowControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accentsCheckToolStripMenuItem;
        private System.Windows.Forms.ComboBox columnListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.Button findButton;
        private System.Windows.Forms.TextBox search_valueBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button replaceAllButton;
        private System.Windows.Forms.TextBox search_replaceBox;
        private System.Windows.Forms.CheckBox allToReplaceChecks;
        private System.Windows.Forms.Label label3;
    }
}

