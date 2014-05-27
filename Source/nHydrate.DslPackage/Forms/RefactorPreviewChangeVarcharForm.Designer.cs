namespace nHydrate.DslPackage.Forms
{
    partial class RefactorPreviewChangeVarcharForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RefactorPreviewChangeVarcharForm));
            this.cmdApply = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblLine100 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tvwItem = new System.Windows.Forms.TreeView();
            this.cmdUncheckAll = new System.Windows.Forms.Button();
            this.cmdCheckAllText = new System.Windows.Forms.Button();
            this.cmdExpandAll = new System.Windows.Forms.Button();
            this.cmdCollapseAll = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboDirection = new System.Windows.Forms.ComboBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdApply
            // 
            this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdApply.Location = new System.Drawing.Point(479, 442);
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size(75, 23);
            this.cmdApply.TabIndex = 7;
            this.cmdApply.Text = "Apply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(560, 442);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.lblLine100);
            this.panel2.Controls.Add(this.lblHeader);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(647, 65);
            this.panel2.TabIndex = 86;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 72;
            this.pictureBox1.TabStop = false;
            // 
            // lblLine100
            // 
            this.lblLine100.BackColor = System.Drawing.Color.DarkGray;
            this.lblLine100.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLine100.Location = new System.Drawing.Point(0, 63);
            this.lblLine100.Name = "lblLine100";
            this.lblLine100.Size = new System.Drawing.Size(647, 2);
            this.lblLine100.TabIndex = 71;
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(91, 18);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(544, 38);
            this.lblHeader.TabIndex = 68;
            this.lblHeader.Text = "Convert field types of Varchar to NVarchar OR NVarchar to Varchar";
            // 
            // tvwItem
            // 
            this.tvwItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvwItem.CheckBoxes = true;
            this.tvwItem.HideSelection = false;
            this.tvwItem.Location = new System.Drawing.Point(12, 71);
            this.tvwItem.Name = "tvwItem";
            this.tvwItem.Size = new System.Drawing.Size(623, 333);
            this.tvwItem.TabIndex = 0;
            // 
            // cmdUncheckAll
            // 
            this.cmdUncheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdUncheckAll.Location = new System.Drawing.Point(13, 442);
            this.cmdUncheckAll.Name = "cmdUncheckAll";
            this.cmdUncheckAll.Size = new System.Drawing.Size(109, 23);
            this.cmdUncheckAll.TabIndex = 3;
            this.cmdUncheckAll.Text = "Uncheck All";
            this.cmdUncheckAll.UseVisualStyleBackColor = true;
            this.cmdUncheckAll.Click += new System.EventHandler(this.cmdUncheckAll_Click);
            // 
            // cmdCheckAllText
            // 
            this.cmdCheckAllText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdCheckAllText.Location = new System.Drawing.Point(128, 442);
            this.cmdCheckAllText.Name = "cmdCheckAllText";
            this.cmdCheckAllText.Size = new System.Drawing.Size(109, 23);
            this.cmdCheckAllText.TabIndex = 4;
            this.cmdCheckAllText.Text = "Check All";
            this.cmdCheckAllText.UseVisualStyleBackColor = true;
            this.cmdCheckAllText.Click += new System.EventHandler(this.cmdCheckAllText_Click);
            // 
            // cmdExpandAll
            // 
            this.cmdExpandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdExpandAll.Location = new System.Drawing.Point(13, 413);
            this.cmdExpandAll.Name = "cmdExpandAll";
            this.cmdExpandAll.Size = new System.Drawing.Size(109, 23);
            this.cmdExpandAll.TabIndex = 1;
            this.cmdExpandAll.Text = "Expand All";
            this.cmdExpandAll.UseVisualStyleBackColor = true;
            this.cmdExpandAll.Click += new System.EventHandler(this.cmdExpandAll_Click);
            // 
            // cmdCollapseAll
            // 
            this.cmdCollapseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdCollapseAll.Location = new System.Drawing.Point(128, 413);
            this.cmdCollapseAll.Name = "cmdCollapseAll";
            this.cmdCollapseAll.Size = new System.Drawing.Size(109, 23);
            this.cmdCollapseAll.TabIndex = 2;
            this.cmdCollapseAll.Text = "Collapse All";
            this.cmdCollapseAll.UseVisualStyleBackColor = true;
            this.cmdCollapseAll.Click += new System.EventHandler(this.cmdCollapseAll_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(243, 411);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(392, 23);
            this.lblStatus.TabIndex = 87;
            this.lblStatus.Text = "[STATUS]";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboDirection
            // 
            this.cboDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDirection.FormattingEnabled = true;
            this.cboDirection.Location = new System.Drawing.Point(246, 442);
            this.cboDirection.Name = "cboDirection";
            this.cboDirection.Size = new System.Drawing.Size(161, 21);
            this.cboDirection.TabIndex = 88;
            // 
            // RefactorPreviewChangeVarcharForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(647, 477);
            this.Controls.Add(this.cboDirection);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cmdCollapseAll);
            this.Controls.Add(this.cmdExpandAll);
            this.Controls.Add(this.cmdCheckAllText);
            this.Controls.Add(this.cmdUncheckAll);
            this.Controls.Add(this.tvwItem);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdApply);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(663, 424);
            this.Name = "RefactorPreviewChangeVarcharForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preview Changes - Convert Datatypes";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdApply;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblLine100;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.TreeView tvwItem;
		private System.Windows.Forms.Button cmdUncheckAll;
        private System.Windows.Forms.Button cmdCheckAllText;
				private System.Windows.Forms.Button cmdExpandAll;
				private System.Windows.Forms.Button cmdCollapseAll;
				private System.Windows.Forms.Label lblStatus;
                private System.Windows.Forms.ComboBox cboDirection;
	}
}