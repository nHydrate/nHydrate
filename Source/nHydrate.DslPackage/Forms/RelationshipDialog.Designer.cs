namespace nHydrate.DslPackage.Forms
{
	partial class RelationshipDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RelationshipDialog));
			this.txtRole = new nHydrate.DslPackage.Forms.CueTextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.chkEnforce = new System.Windows.Forms.CheckBox();
			this.cboChildField = new System.Windows.Forms.ComboBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.lblLine100 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblPrimaryTable = new System.Windows.Forms.Label();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cboParentField = new System.Windows.Forms.ComboBox();
			this.cmdAdd = new System.Windows.Forms.Button();
			this.lvwColumns = new System.Windows.Forms.ListView();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdOK = new System.Windows.Forms.Button();
			this.lblSecondaryTable = new System.Windows.Forms.Label();
			this.cboChildTable = new System.Windows.Forms.ComboBox();
			this.pnlCover = new System.Windows.Forms.Panel();
			this.lblCover = new System.Windows.Forms.Label();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.pnlCover.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtRole
			// 
			this.txtRole.Cue = "<Enter Role Name>";
			this.txtRole.Location = new System.Drawing.Point(269, 146);
			this.txtRole.Name = "txtRole";
			this.txtRole.Size = new System.Drawing.Size(226, 20);
			this.txtRole.TabIndex = 75;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(237, 16);
			this.label4.TabIndex = 65;
			this.label4.Text = "Primary key";
			// 
			// chkEnforce
			// 
			this.chkEnforce.AutoSize = true;
			this.chkEnforce.Location = new System.Drawing.Point(32, 147);
			this.chkEnforce.Name = "chkEnforce";
			this.chkEnforce.Size = new System.Drawing.Size(63, 17);
			this.chkEnforce.TabIndex = 74;
			this.chkEnforce.Text = "Enforce";
			this.chkEnforce.UseVisualStyleBackColor = true;
			// 
			// cboChildField
			// 
			this.cboChildField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChildField.Location = new System.Drawing.Point(259, 40);
			this.cboChildField.Name = "cboChildField";
			this.cboChildField.Size = new System.Drawing.Size(230, 21);
			this.cboChildField.Sorted = true;
			this.cboChildField.TabIndex = 5;
			this.cboChildField.SelectedIndexChanged += new System.EventHandler(this.cboChildField_SelectedIndexChanged);
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Window;
			this.panel2.Controls.Add(this.lblLine100);
			this.panel2.Controls.Add(this.label6);
			this.panel2.Controls.Add(this.pictureBox1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(517, 65);
			this.panel2.TabIndex = 83;
			// 
			// lblLine100
			// 
			this.lblLine100.BackColor = System.Drawing.Color.DarkGray;
			this.lblLine100.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblLine100.Location = new System.Drawing.Point(0, 63);
			this.lblLine100.Name = "lblLine100";
			this.lblLine100.Size = new System.Drawing.Size(517, 2);
			this.lblLine100.TabIndex = 71;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(91, 18);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(200, 13);
			this.label6.TabIndex = 68;
			this.label6.Text = "Specify the relationship properties";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 63;
			this.pictureBox1.TabStop = false;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(266, 130);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(61, 13);
			this.label5.TabIndex = 82;
			this.label5.Text = "Role name:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(259, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(230, 16);
			this.label3.TabIndex = 66;
			this.label3.Text = "Foreign key";
			// 
			// lblPrimaryTable
			// 
			this.lblPrimaryTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblPrimaryTable.Location = new System.Drawing.Point(26, 97);
			this.lblPrimaryTable.Name = "lblPrimaryTable";
			this.lblPrimaryTable.Size = new System.Drawing.Size(237, 21);
			this.lblPrimaryTable.TabIndex = 81;
			this.lblPrimaryTable.Text = "[PRIMARY]";
			this.lblPrimaryTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(425, 399);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(80, 24);
			this.cmdCancel.TabIndex = 77;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdDelete
			// 
			this.cmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdDelete.Location = new System.Drawing.Point(425, 67);
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size(64, 24);
			this.cmdDelete.TabIndex = 7;
			this.cmdDelete.Text = "Delete";
			this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.cboChildField);
			this.groupBox1.Controls.Add(this.cboParentField);
			this.groupBox1.Controls.Add(this.cmdDelete);
			this.groupBox1.Controls.Add(this.cmdAdd);
			this.groupBox1.Controls.Add(this.lvwColumns);
			this.groupBox1.Location = new System.Drawing.Point(10, 172);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(495, 220);
			this.groupBox1.TabIndex = 80;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Column Relations";
			// 
			// cboParentField
			// 
			this.cboParentField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboParentField.Location = new System.Drawing.Point(16, 40);
			this.cboParentField.Name = "cboParentField";
			this.cboParentField.Size = new System.Drawing.Size(237, 21);
			this.cboParentField.Sorted = true;
			this.cboParentField.TabIndex = 4;
			this.cboParentField.SelectedIndexChanged += new System.EventHandler(this.cboParentField_SelectedIndexChanged);
			// 
			// cmdAdd
			// 
			this.cmdAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdAdd.Location = new System.Drawing.Point(353, 67);
			this.cmdAdd.Name = "cmdAdd";
			this.cmdAdd.Size = new System.Drawing.Size(64, 24);
			this.cmdAdd.TabIndex = 6;
			this.cmdAdd.Text = "Add";
			this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
			// 
			// lvwColumns
			// 
			this.lvwColumns.FullRowSelect = true;
			this.lvwColumns.GridLines = true;
			this.lvwColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvwColumns.HideSelection = false;
			this.lvwColumns.Location = new System.Drawing.Point(16, 104);
			this.lvwColumns.Name = "lvwColumns";
			this.lvwColumns.Size = new System.Drawing.Size(473, 104);
			this.lvwColumns.TabIndex = 8;
			this.lvwColumns.UseCompatibleStateImageBehavior = false;
			this.lvwColumns.View = System.Windows.Forms.View.Details;
			this.lvwColumns.SelectedIndexChanged += new System.EventHandler(this.lvwColumns_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(269, 81);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(227, 16);
			this.label2.TabIndex = 79;
			this.label2.Text = "Foreign key entity";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(26, 81);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(237, 16);
			this.label1.TabIndex = 78;
			this.label1.Text = "Primary key entity";
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.Location = new System.Drawing.Point(337, 399);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(80, 24);
			this.cmdOK.TabIndex = 76;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// lblSecondaryTable
			// 
			this.lblSecondaryTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblSecondaryTable.Location = new System.Drawing.Point(272, 97);
			this.lblSecondaryTable.Name = "lblSecondaryTable";
			this.lblSecondaryTable.Size = new System.Drawing.Size(227, 21);
			this.lblSecondaryTable.TabIndex = 84;
			this.lblSecondaryTable.Text = "[SECONDARY]";
			this.lblSecondaryTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cboChildTable
			// 
			this.cboChildTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChildTable.FormattingEnabled = true;
			this.cboChildTable.IntegralHeight = false;
			this.cboChildTable.Location = new System.Drawing.Point(278, 106);
			this.cboChildTable.Name = "cboChildTable";
			this.cboChildTable.Size = new System.Drawing.Size(227, 21);
			this.cboChildTable.TabIndex = 85;
			this.cboChildTable.Visible = false;
			// 
			// pnlCover
			// 
			this.pnlCover.Controls.Add(this.lblCover);
			this.pnlCover.Location = new System.Drawing.Point(505, 310);
			this.pnlCover.Name = "pnlCover";
			this.pnlCover.Size = new System.Drawing.Size(489, 247);
			this.pnlCover.TabIndex = 86;
			this.pnlCover.Visible = false;
			// 
			// lblCover
			// 
			this.lblCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblCover.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblCover.Location = new System.Drawing.Point(0, 0);
			this.lblCover.Name = "lblCover";
			this.lblCover.Size = new System.Drawing.Size(489, 247);
			this.lblCover.TabIndex = 0;
			this.lblCover.Text = "You must choose a foreign key entity to configure this item.";
			this.lblCover.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// RelationshipDialog
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(517, 426);
			this.Controls.Add(this.pnlCover);
			this.Controls.Add(this.cboChildTable);
			this.Controls.Add(this.lblSecondaryTable);
			this.Controls.Add(this.txtRole);
			this.Controls.Add(this.chkEnforce);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lblPrimaryTable);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RelationshipDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Entity Relationship";
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.pnlCover.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private nHydrate.DslPackage.Forms.CueTextBox txtRole;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox chkEnforce;
		private System.Windows.Forms.ComboBox cboChildField;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblPrimaryTable;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdDelete;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cboParentField;
		private System.Windows.Forms.Button cmdAdd;
		private System.Windows.Forms.ListView lvwColumns;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Label lblSecondaryTable;
		private System.Windows.Forms.Label lblLine100;
		private System.Windows.Forms.ComboBox cboChildTable;
		private System.Windows.Forms.Panel pnlCover;
		private System.Windows.Forms.Label lblCover;
	}
}