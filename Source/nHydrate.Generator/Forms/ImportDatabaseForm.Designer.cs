namespace nHydrate.Generator.Forms
{
	partial class ImportDatabaseForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportDatabaseForm));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.wizard1 = new nHydrate.Wizard.Wizard();
			this.pageConnection = new nHydrate.Wizard.WizardPage();
			this.cmdTestConnection = new System.Windows.Forms.Button();
			this.txtConnectionString = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkInheritance = new System.Windows.Forms.CheckBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.DatabaseConnectionControl1 = new nHydrate.Generator.DatabaseConnectionControl();
			this.pageEntities = new nHydrate.Wizard.WizardPage();
			this.pnlMain = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tvwAdd = new System.Windows.Forms.TreeView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tvwRefresh = new System.Windows.Forms.TreeView();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tvwDelete = new System.Windows.Forms.TreeView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.chkSettingPK = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.lblLine100 = new System.Windows.Forms.Label();
			this.wizard1.SuspendLayout();
			this.pageConnection.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.pageEntities.SuspendLayout();
			this.pnlMain.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "import_table_header.png");
			this.imageList1.Images.SetKeyName(1, "import_view_header.png");
			this.imageList1.Images.SetKeyName(2, "import_storedprocedure_header.png");
			this.imageList1.Images.SetKeyName(3, "import_table.png");
			this.imageList1.Images.SetKeyName(4, "import_view.png");
			this.imageList1.Images.SetKeyName(5, "import_storedprocedure.png");
			// 
			// wizard1
			// 
			this.wizard1.ButtonFlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.wizard1.Controls.Add(this.pageConnection);
			this.wizard1.Controls.Add(this.pageEntities);
			this.wizard1.Location = new System.Drawing.Point(0, 0);
			this.wizard1.Name = "wizard1";
			this.wizard1.Size = new System.Drawing.Size(643, 544);
			this.wizard1.TabIndex = 74;
			this.wizard1.WizardPages.AddRange(new nHydrate.Wizard.WizardPage[] {
            this.pageConnection,
            this.pageEntities});
			// 
			// pageConnection
			// 
			this.pageConnection.Controls.Add(this.cmdTestConnection);
			this.pageConnection.Controls.Add(this.txtConnectionString);
			this.pageConnection.Controls.Add(this.label2);
			this.pageConnection.Controls.Add(this.chkInheritance);
			this.pageConnection.Controls.Add(this.panel3);
			this.pageConnection.Controls.Add(this.DatabaseConnectionControl1);
			this.pageConnection.Location = new System.Drawing.Point(0, 0);
			this.pageConnection.Name = "pageConnection";
			this.pageConnection.Size = new System.Drawing.Size(643, 496);
			this.pageConnection.Style = nHydrate.Wizard.WizardPageStyle.Custom;
			this.pageConnection.TabIndex = 7;
			// 
			// cmdTestConnection
			// 
			this.cmdTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdTestConnection.Location = new System.Drawing.Point(501, 467);
			this.cmdTestConnection.Name = "cmdTestConnection";
			this.cmdTestConnection.Size = new System.Drawing.Size(130, 23);
			this.cmdTestConnection.TabIndex = 80;
			this.cmdTestConnection.Text = "Test Connection";
			this.cmdTestConnection.UseVisualStyleBackColor = true;
			// 
			// txtConnectionString
			// 
			this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtConnectionString.Location = new System.Drawing.Point(15, 347);
			this.txtConnectionString.Multiline = true;
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.ReadOnly = true;
			this.txtConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtConnectionString.Size = new System.Drawing.Size(616, 114);
			this.txtConnectionString.TabIndex = 79;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 319);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(125, 13);
			this.label2.TabIndex = 78;
			this.label2.Text = "Entity connection strings:";
			// 
			// chkInheritance
			// 
			this.chkInheritance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkInheritance.AutoSize = true;
			this.chkInheritance.Location = new System.Drawing.Point(12, 467);
			this.chkInheritance.Name = "chkInheritance";
			this.chkInheritance.Size = new System.Drawing.Size(119, 17);
			this.chkInheritance.TabIndex = 77;
			this.chkInheritance.Text = "Assume Inheritance";
			this.chkInheritance.UseVisualStyleBackColor = true;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.Window;
			this.panel3.Controls.Add(this.lblLine100);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Controls.Add(this.pictureBox2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.MinimumSize = new System.Drawing.Size(567, 65);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(643, 78);
			this.panel3.TabIndex = 75;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(91, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(178, 13);
			this.label1.TabIndex = 68;
			this.label1.Text = "Choose Your Data Connection";
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
			this.pictureBox2.Location = new System.Drawing.Point(8, 8);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(62, 56);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox2.TabIndex = 65;
			this.pictureBox2.TabStop = false;
			// 
			// DatabaseConnectionControl1
			// 
			this.DatabaseConnectionControl1.AutoSize = true;
			this.DatabaseConnectionControl1.Location = new System.Drawing.Point(12, 84);
			this.DatabaseConnectionControl1.Name = "DatabaseConnectionControl1";
			this.DatabaseConnectionControl1.Size = new System.Drawing.Size(619, 232);
			this.DatabaseConnectionControl1.TabIndex = 76;
			// 
			// pageEntities
			// 
			this.pageEntities.Controls.Add(this.pnlMain);
			this.pageEntities.Controls.Add(this.panel2);
			this.pageEntities.Controls.Add(this.panel1);
			this.pageEntities.Location = new System.Drawing.Point(0, 0);
			this.pageEntities.Name = "pageEntities";
			this.pageEntities.Size = new System.Drawing.Size(643, 496);
			this.pageEntities.Style = nHydrate.Wizard.WizardPageStyle.Custom;
			this.pageEntities.TabIndex = 8;
			// 
			// pnlMain
			// 
			this.pnlMain.Controls.Add(this.tabControl1);
			this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMain.Location = new System.Drawing.Point(0, 78);
			this.pnlMain.Margin = new System.Windows.Forms.Padding(0);
			this.pnlMain.Name = "pnlMain";
			this.pnlMain.Padding = new System.Windows.Forms.Padding(10);
			this.pnlMain.Size = new System.Drawing.Size(643, 382);
			this.pnlMain.TabIndex = 73;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(10, 10);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(623, 362);
			this.tabControl1.TabIndex = 70;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.tvwAdd);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(615, 336);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Add";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tvwAdd
			// 
			this.tvwAdd.CheckBoxes = true;
			this.tvwAdd.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwAdd.Location = new System.Drawing.Point(3, 3);
			this.tvwAdd.Name = "tvwAdd";
			this.tvwAdd.Size = new System.Drawing.Size(609, 330);
			this.tvwAdd.TabIndex = 68;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.tvwRefresh);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(615, 336);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Refresh";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tvwRefresh
			// 
			this.tvwRefresh.CheckBoxes = true;
			this.tvwRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwRefresh.Location = new System.Drawing.Point(3, 3);
			this.tvwRefresh.Name = "tvwRefresh";
			this.tvwRefresh.Size = new System.Drawing.Size(629, 363);
			this.tvwRefresh.TabIndex = 69;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.tvwDelete);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(615, 336);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Delete";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// tvwDelete
			// 
			this.tvwDelete.CheckBoxes = true;
			this.tvwDelete.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwDelete.Location = new System.Drawing.Point(3, 3);
			this.tvwDelete.Name = "tvwDelete";
			this.tvwDelete.Size = new System.Drawing.Size(629, 363);
			this.tvwDelete.TabIndex = 69;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.chkSettingPK);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 460);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(643, 36);
			this.panel2.TabIndex = 75;
			// 
			// chkSettingPK
			// 
			this.chkSettingPK.AutoSize = true;
			this.chkSettingPK.Checked = true;
			this.chkSettingPK.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSettingPK.Location = new System.Drawing.Point(8, 8);
			this.chkSettingPK.Name = "chkSettingPK";
			this.chkSettingPK.Size = new System.Drawing.Size(124, 17);
			this.chkSettingPK.TabIndex = 72;
			this.chkSettingPK.Text = "Override Primary Key";
			this.chkSettingPK.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Window;
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.MinimumSize = new System.Drawing.Size(567, 65);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(643, 78);
			this.panel1.TabIndex = 74;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(62, 56);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 69;
			this.pictureBox1.TabStop = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(91, 18);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(126, 13);
			this.label3.TabIndex = 68;
			this.label3.Text = "Choose Your Objects";
			// 
			// lblLine100
			// 
			this.lblLine100.BackColor = System.Drawing.Color.DarkGray;
			this.lblLine100.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblLine100.Location = new System.Drawing.Point(0, 76);
			this.lblLine100.Name = "lblLine100";
			this.lblLine100.Size = new System.Drawing.Size(643, 2);
			this.lblLine100.TabIndex = 70;
			// 
			// ImportDatabaseForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(643, 544);
			this.Controls.Add(this.wizard1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(659, 582);
			this.Name = "ImportDatabaseForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Import Model Wizard";
			this.wizard1.ResumeLayout(false);
			this.wizard1.PerformLayout();
			this.pageConnection.ResumeLayout(false);
			this.pageConnection.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.pageEntities.ResumeLayout(false);
			this.pnlMain.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ImageList imageList1;
		private nHydrate.Wizard.Wizard wizard1;
		private nHydrate.Wizard.WizardPage pageConnection;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private nHydrate.Wizard.WizardPage pageEntities;
		private System.Windows.Forms.Panel pnlMain;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TreeView tvwAdd;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TreeView tvwRefresh;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TreeView tvwDelete;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.CheckBox chkSettingPK;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chkInheritance;
		private DatabaseConnectionControl DatabaseConnectionControl1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtConnectionString;
		private System.Windows.Forms.Button cmdTestConnection;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblLine100;
	}
}