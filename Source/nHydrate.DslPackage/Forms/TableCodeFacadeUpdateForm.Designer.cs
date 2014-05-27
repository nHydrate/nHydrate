namespace nHydrate.DslPackage.Forms
{
	partial class TableCodeFacadeUpdateForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableCodeFacadeUpdateForm));
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.optPrefix = new System.Windows.Forms.CheckBox();
			this.pnlPrefix = new System.Windows.Forms.Panel();
			this.txtPrefix = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.cboCasing = new System.Windows.Forms.ComboBox();
			this.pnlReplace = new System.Windows.Forms.Panel();
			this.txtReplaceTarget = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtReplaceSource = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.chkReplaceText = new System.Windows.Forms.CheckBox();
			this.cmdRemoveAll = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblLine100 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.cboMode = new System.Windows.Forms.ComboBox();
			this.lvwItem = new System.Windows.Forms.ListView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.cmdCheck = new System.Windows.Forms.Button();
			this.cmdUncheck = new System.Windows.Forms.Button();
			this.lblStatus = new System.Windows.Forms.Label();
			this.pnlPrefix.SuspendLayout();
			this.panel2.SuspendLayout();
			this.pnlReplace.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(192, 287);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 11;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(112, 287);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 10;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// optPrefix
			// 
			this.optPrefix.AutoSize = true;
			this.optPrefix.Checked = true;
			this.optPrefix.CheckState = System.Windows.Forms.CheckState.Checked;
			this.optPrefix.Location = new System.Drawing.Point(3, 3);
			this.optPrefix.Name = "optPrefix";
			this.optPrefix.Size = new System.Drawing.Size(95, 17);
			this.optPrefix.TabIndex = 1;
			this.optPrefix.Text = "Remove Prefix";
			this.optPrefix.UseVisualStyleBackColor = true;
			this.optPrefix.CheckedChanged += new System.EventHandler(this.optPrefix_CheckedChanged);
			// 
			// pnlPrefix
			// 
			this.pnlPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlPrefix.Controls.Add(this.txtPrefix);
			this.pnlPrefix.Controls.Add(this.label1);
			this.pnlPrefix.Location = new System.Drawing.Point(3, 26);
			this.pnlPrefix.Name = "pnlPrefix";
			this.pnlPrefix.Size = new System.Drawing.Size(257, 33);
			this.pnlPrefix.TabIndex = 6;
			// 
			// txtPrefix
			// 
			this.txtPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPrefix.Location = new System.Drawing.Point(59, 4);
			this.txtPrefix.Name = "txtPrefix";
			this.txtPrefix.Size = new System.Drawing.Size(193, 20);
			this.txtPrefix.TabIndex = 2;
			this.txtPrefix.Text = "tbl";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Prefix:";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label6);
			this.panel2.Controls.Add(this.cboCasing);
			this.panel2.Controls.Add(this.pnlReplace);
			this.panel2.Controls.Add(this.chkReplaceText);
			this.panel2.Controls.Add(this.optPrefix);
			this.panel2.Controls.Add(this.pnlPrefix);
			this.panel2.Location = new System.Drawing.Point(12, 94);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(260, 182);
			this.panel2.TabIndex = 10;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(15, 152);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(42, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "Casing:";
			// 
			// cboCasing
			// 
			this.cboCasing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCasing.FormattingEnabled = true;
			this.cboCasing.Location = new System.Drawing.Point(62, 149);
			this.cboCasing.Name = "cboCasing";
			this.cboCasing.Size = new System.Drawing.Size(193, 21);
			this.cboCasing.TabIndex = 6;
			this.cboCasing.SelectedIndexChanged += new System.EventHandler(this.cboCasing_SelectedIndexChanged);
			// 
			// pnlReplace
			// 
			this.pnlReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlReplace.Controls.Add(this.txtReplaceTarget);
			this.pnlReplace.Controls.Add(this.label4);
			this.pnlReplace.Controls.Add(this.txtReplaceSource);
			this.pnlReplace.Controls.Add(this.label3);
			this.pnlReplace.Location = new System.Drawing.Point(3, 88);
			this.pnlReplace.Name = "pnlReplace";
			this.pnlReplace.Size = new System.Drawing.Size(257, 59);
			this.pnlReplace.TabIndex = 8;
			// 
			// txtReplaceTarget
			// 
			this.txtReplaceTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtReplaceTarget.Location = new System.Drawing.Point(59, 30);
			this.txtReplaceTarget.Name = "txtReplaceTarget";
			this.txtReplaceTarget.Size = new System.Drawing.Size(193, 20);
			this.txtReplaceTarget.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(16, 33);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(41, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Target:";
			// 
			// txtReplaceSource
			// 
			this.txtReplaceSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtReplaceSource.Location = new System.Drawing.Point(59, 4);
			this.txtReplaceSource.Name = "txtReplaceSource";
			this.txtReplaceSource.Size = new System.Drawing.Size(193, 20);
			this.txtReplaceSource.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 7);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Source:";
			// 
			// chkReplaceText
			// 
			this.chkReplaceText.AutoSize = true;
			this.chkReplaceText.Location = new System.Drawing.Point(3, 65);
			this.chkReplaceText.Name = "chkReplaceText";
			this.chkReplaceText.Size = new System.Drawing.Size(90, 17);
			this.chkReplaceText.TabIndex = 3;
			this.chkReplaceText.Text = "Replace Text";
			this.chkReplaceText.UseVisualStyleBackColor = true;
			this.chkReplaceText.CheckedChanged += new System.EventHandler(this.chkReplaceText_CheckedChanged);
			// 
			// cmdRemoveAll
			// 
			this.cmdRemoveAll.Location = new System.Drawing.Point(12, 286);
			this.cmdRemoveAll.Name = "cmdRemoveAll";
			this.cmdRemoveAll.Size = new System.Drawing.Size(90, 23);
			this.cmdRemoveAll.TabIndex = 9;
			this.cmdRemoveAll.Text = "Remove All";
			this.cmdRemoveAll.UseVisualStyleBackColor = true;
			this.cmdRemoveAll.Visible = false;
			this.cmdRemoveAll.Click += new System.EventHandler(this.cmdRemoveAll_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 11;
			this.pictureBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Window;
			this.panel1.Controls.Add(this.lblLine100);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(895, 65);
			this.panel1.TabIndex = 71;
			// 
			// lblLine100
			// 
			this.lblLine100.BackColor = System.Drawing.Color.DarkGray;
			this.lblLine100.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblLine100.Location = new System.Drawing.Point(0, 63);
			this.lblLine100.Name = "lblLine100";
			this.lblLine100.Size = new System.Drawing.Size(895, 2);
			this.lblLine100.TabIndex = 70;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(91, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(792, 38);
			this.label2.TabIndex = 68;
			this.label2.Text = "Change the names or codefacades of model objects based on rules. The name is used" +
    " as a base and applied to the name or codefacade based on the specified mode.";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 69);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(37, 13);
			this.label5.TabIndex = 72;
			this.label5.Text = "Mode:";
			// 
			// cboMode
			// 
			this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMode.FormattingEnabled = true;
			this.cboMode.Location = new System.Drawing.Point(74, 69);
			this.cboMode.Name = "cboMode";
			this.cboMode.Size = new System.Drawing.Size(195, 21);
			this.cboMode.TabIndex = 0;
			this.cboMode.SelectedIndexChanged += new System.EventHandler(this.cboMode_SelectedIndexChanged);
			// 
			// lvwItem
			// 
			this.lvwItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvwItem.CheckBoxes = true;
			this.lvwItem.FullRowSelect = true;
			this.lvwItem.HideSelection = false;
			this.lvwItem.Location = new System.Drawing.Point(278, 71);
			this.lvwItem.Name = "lvwItem";
			this.lvwItem.Size = new System.Drawing.Size(605, 414);
			this.lvwItem.SmallImageList = this.imageList1;
			this.lvwItem.TabIndex = 12;
			this.lvwItem.UseCompatibleStateImageBehavior = false;
			this.lvwItem.View = System.Windows.Forms.View.Details;
			this.lvwItem.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwItem_ColumnClick);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "Entity.png");
			this.imageList1.Images.SetKeyName(1, "view.png");
			this.imageList1.Images.SetKeyName(2, "storedproc.png");
			this.imageList1.Images.SetKeyName(3, "function.png");
			// 
			// cmdCheck
			// 
			this.cmdCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCheck.Location = new System.Drawing.Point(663, 491);
			this.cmdCheck.Name = "cmdCheck";
			this.cmdCheck.Size = new System.Drawing.Size(107, 23);
			this.cmdCheck.TabIndex = 13;
			this.cmdCheck.Text = "Check All";
			this.cmdCheck.UseVisualStyleBackColor = true;
			this.cmdCheck.Click += new System.EventHandler(this.cmdCheck_Click);
			// 
			// cmdUncheck
			// 
			this.cmdUncheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdUncheck.Location = new System.Drawing.Point(776, 491);
			this.cmdUncheck.Name = "cmdUncheck";
			this.cmdUncheck.Size = new System.Drawing.Size(107, 23);
			this.cmdUncheck.TabIndex = 14;
			this.cmdUncheck.Text = "Uncheck All";
			this.cmdUncheck.UseVisualStyleBackColor = true;
			this.cmdUncheck.Click += new System.EventHandler(this.cmdUncheck_Click);
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblStatus.Location = new System.Drawing.Point(278, 492);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(379, 23);
			this.lblStatus.TabIndex = 73;
			this.lblStatus.Text = "[STATUS]";
			// 
			// TableCodeFacadeUpdateForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(895, 526);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.cmdUncheck);
			this.Controls.Add(this.cmdCheck);
			this.Controls.Add(this.lvwItem);
			this.Controls.Add(this.cboMode);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.cmdRemoveAll);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(678, 360);
			this.Name = "TableCodeFacadeUpdateForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Update CodeFacade";
			this.pnlPrefix.ResumeLayout(false);
			this.pnlPrefix.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.pnlReplace.ResumeLayout(false);
			this.pnlReplace.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.CheckBox optPrefix;
		private System.Windows.Forms.Panel pnlPrefix;
		private System.Windows.Forms.TextBox txtPrefix;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.CheckBox chkReplaceText;
		private System.Windows.Forms.Panel pnlReplace;
		private System.Windows.Forms.TextBox txtReplaceTarget;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtReplaceSource;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button cmdRemoveAll;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblLine100;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cboMode;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox cboCasing;
		private System.Windows.Forms.ListView lvwItem;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Button cmdCheck;
		private System.Windows.Forms.Button cmdUncheck;
		private System.Windows.Forms.Label lblStatus;
	}
}