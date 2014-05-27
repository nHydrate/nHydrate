namespace nHydrate.DslPackage.Forms
{
	partial class TableExtendedPropertiesForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableExtendedPropertiesForm));
			this.cmdClose = new System.Windows.Forms.Button();
			this.lstChildTable = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lstParentTable = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.lblLine100 = new System.Windows.Forms.Label();
			this.lblHeader = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdClose
			// 
			this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdClose.Location = new System.Drawing.Point(302, 313);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(75, 23);
			this.cmdClose.TabIndex = 2;
			this.cmdClose.Text = "Close";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// lstChildTable
			// 
			this.lstChildTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstChildTable.FormattingEnabled = true;
			this.lstChildTable.Location = new System.Drawing.Point(12, 83);
			this.lstChildTable.Name = "lstChildTable";
			this.lstChildTable.Size = new System.Drawing.Size(362, 95);
			this.lstChildTable.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 67);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(182, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "I am a parent entity in these relations:";
			// 
			// lstParentTable
			// 
			this.lstParentTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstParentTable.FormattingEnabled = true;
			this.lstParentTable.Location = new System.Drawing.Point(12, 204);
			this.lstParentTable.Name = "lstParentTable";
			this.lstParentTable.Size = new System.Drawing.Size(362, 95);
			this.lstParentTable.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 188);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(174, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "I am a child entity in these relations:";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Window;
			this.panel2.Controls.Add(this.lblLine100);
			this.panel2.Controls.Add(this.lblHeader);
			this.panel2.Controls.Add(this.pictureBox1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(389, 65);
			this.panel2.TabIndex = 71;
			// 
			// lblLine100
			// 
			this.lblLine100.BackColor = System.Drawing.Color.DarkGray;
			this.lblLine100.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblLine100.Location = new System.Drawing.Point(0, 63);
			this.lblLine100.Name = "lblLine100";
			this.lblLine100.Size = new System.Drawing.Size(389, 2);
			this.lblLine100.TabIndex = 71;
			// 
			// lblHeader
			// 
			this.lblHeader.AutoSize = true;
			this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHeader.Location = new System.Drawing.Point(91, 18);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(120, 13);
			this.lblHeader.TabIndex = 68;
			this.lblHeader.Text = "Relationship Details";
			// 
			// TableExtendedPropertiesForm
			// 
			this.AcceptButton = this.cmdClose;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.cmdClose;
			this.ClientSize = new System.Drawing.Size(389, 348);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstParentTable);
			this.Controls.Add(this.lstChildTable);
			this.Controls.Add(this.cmdClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(395, 316);
			this.Name = "TableExtendedPropertiesForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Relationship";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.ListBox lstChildTable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstParentTable;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.Label lblLine100;
	}
}