namespace nHydrate.DslPackage.Forms
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
            this.grpConnectionStringPostgres = new System.Windows.Forms.GroupBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtConnectionStringPostgres = new nHydrate.Generator.Common.Forms.CueTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.optDatabaseTypePostgres = new System.Windows.Forms.RadioButton();
            this.optDatabaseTypeSQL = new System.Windows.Forms.RadioButton();
            this.cmdTestConnection = new System.Windows.Forms.Button();
            this.DatabaseConnectionControl1 = new nHydrate.DslPackage.Forms.DatabaseConnectionControl();
            this.pageSummary = new nHydrate.Wizard.WizardPage();
            this.txtSummary = new FastColoredTextBoxNS.FastColoredTextBox();
            this.pageEntities = new nHydrate.Wizard.WizardPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkIgnoreRelations = new System.Windows.Forms.CheckBox();
            this.chkInheritance = new System.Windows.Forms.CheckBox();
            this.chkSettingPK = new System.Windows.Forms.CheckBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.cmdViewDiff = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tvwAdd = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tvwRefresh = new System.Windows.Forms.TreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tvwDelete = new System.Windows.Forms.TreeView();
            this.wizard1.SuspendLayout();
            this.pageConnection.SuspendLayout();
            this.grpConnectionStringPostgres.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pageSummary.SuspendLayout();
            this.pageEntities.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            this.wizard1.Controls.Add(this.pageEntities);
            this.wizard1.Controls.Add(this.pageConnection);
            this.wizard1.Controls.Add(this.pageSummary);
            this.wizard1.HeaderImage = ((System.Drawing.Image)(resources.GetObject("wizard1.HeaderImage")));
            this.wizard1.Location = new System.Drawing.Point(0, 0);
            this.wizard1.Name = "wizard1";
            this.wizard1.Size = new System.Drawing.Size(653, 543);
            this.wizard1.TabIndex = 74;
            this.wizard1.WizardPages.AddRange(new nHydrate.Wizard.WizardPage[] {
            this.pageConnection,
            this.pageEntities,
            this.pageSummary});
            // 
            // pageConnection
            // 
            this.pageConnection.Controls.Add(this.grpConnectionStringPostgres);
            this.pageConnection.Controls.Add(this.panel1);
            this.pageConnection.Controls.Add(this.cmdTestConnection);
            this.pageConnection.Controls.Add(this.DatabaseConnectionControl1);
            this.pageConnection.Description = "Specify your database connection information.";
            this.pageConnection.Location = new System.Drawing.Point(0, 0);
            this.pageConnection.Name = "pageConnection";
            this.pageConnection.Size = new System.Drawing.Size(653, 495);
            this.pageConnection.TabIndex = 7;
            this.pageConnection.Title = "Database Connection";
            // 
            // grpConnectionStringPostgres
            // 
            this.grpConnectionStringPostgres.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConnectionStringPostgres.Controls.Add(this.lblConnectionString);
            this.grpConnectionStringPostgres.Controls.Add(this.txtConnectionStringPostgres);
            this.grpConnectionStringPostgres.Location = new System.Drawing.Point(12, 396);
            this.grpConnectionStringPostgres.Name = "grpConnectionStringPostgres";
            this.grpConnectionStringPostgres.Size = new System.Drawing.Size(619, 57);
            this.grpConnectionStringPostgres.TabIndex = 82;
            this.grpConnectionStringPostgres.TabStop = false;
            this.grpConnectionStringPostgres.Visible = false;
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(19, 27);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(112, 13);
            this.lblConnectionString.TabIndex = 5;
            this.lblConnectionString.Text = "Database connection:";
            // 
            // txtConnectionStringPostgres
            // 
            this.txtConnectionStringPostgres.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionStringPostgres.Cue = "<Enter Connectionstring>";
            this.txtConnectionStringPostgres.Location = new System.Drawing.Point(136, 24);
            this.txtConnectionStringPostgres.Name = "txtConnectionStringPostgres";
            this.txtConnectionStringPostgres.Size = new System.Drawing.Size(469, 20);
            this.txtConnectionStringPostgres.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.optDatabaseTypePostgres);
            this.panel1.Controls.Add(this.optDatabaseTypeSQL);
            this.panel1.Location = new System.Drawing.Point(12, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(619, 52);
            this.panel1.TabIndex = 81;
            // 
            // optDatabaseTypePostgres
            // 
            this.optDatabaseTypePostgres.AutoSize = true;
            this.optDatabaseTypePostgres.Location = new System.Drawing.Point(157, 13);
            this.optDatabaseTypePostgres.Name = "optDatabaseTypePostgres";
            this.optDatabaseTypePostgres.Size = new System.Drawing.Size(66, 17);
            this.optDatabaseTypePostgres.TabIndex = 0;
            this.optDatabaseTypePostgres.Text = "Postgres";
            this.optDatabaseTypePostgres.UseVisualStyleBackColor = true;
            // 
            // optDatabaseTypeSQL
            // 
            this.optDatabaseTypeSQL.AutoSize = true;
            this.optDatabaseTypeSQL.Checked = true;
            this.optDatabaseTypeSQL.Location = new System.Drawing.Point(17, 13);
            this.optDatabaseTypeSQL.Name = "optDatabaseTypeSQL";
            this.optDatabaseTypeSQL.Size = new System.Drawing.Size(80, 17);
            this.optDatabaseTypeSQL.TabIndex = 0;
            this.optDatabaseTypeSQL.TabStop = true;
            this.optDatabaseTypeSQL.Text = "SQL Server";
            this.optDatabaseTypeSQL.UseVisualStyleBackColor = true;
            // 
            // cmdTestConnection
            // 
            this.cmdTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTestConnection.Location = new System.Drawing.Point(501, 459);
            this.cmdTestConnection.Name = "cmdTestConnection";
            this.cmdTestConnection.Size = new System.Drawing.Size(130, 23);
            this.cmdTestConnection.TabIndex = 80;
            this.cmdTestConnection.Text = "Test Connection";
            this.cmdTestConnection.UseVisualStyleBackColor = true;
            // 
            // DatabaseConnectionControl1
            // 
            this.DatabaseConnectionControl1.AutoSize = true;
            this.DatabaseConnectionControl1.FileName = "";
            this.DatabaseConnectionControl1.Location = new System.Drawing.Point(12, 125);
            this.DatabaseConnectionControl1.Name = "DatabaseConnectionControl1";
            this.DatabaseConnectionControl1.Size = new System.Drawing.Size(619, 269);
            this.DatabaseConnectionControl1.TabIndex = 76;
            // 
            // pageSummary
            // 
            this.pageSummary.Controls.Add(this.txtSummary);
            this.pageSummary.Description = "This is a summary of the import. Please verify this information and press \'Finish" +
    "\' to import the new objects.";
            this.pageSummary.Location = new System.Drawing.Point(0, 0);
            this.pageSummary.Name = "pageSummary";
            this.pageSummary.Size = new System.Drawing.Size(653, 495);
            this.pageSummary.TabIndex = 9;
            this.pageSummary.Title = "Summary";
            // 
            // txtSummary
            // 
            this.txtSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSummary.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.txtSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtSummary.CommentPrefix = "--";
            this.txtSummary.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSummary.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.txtSummary.LeftBracket = '(';
            this.txtSummary.Location = new System.Drawing.Point(12, 76);
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.RightBracket = ')';
            this.txtSummary.ShowLineNumbers = false;
            this.txtSummary.Size = new System.Drawing.Size(629, 404);
            this.txtSummary.TabIndex = 2;
            // 
            // pageEntities
            // 
            this.pageEntities.Controls.Add(this.panel2);
            this.pageEntities.Controls.Add(this.pnlMain);
            this.pageEntities.Description = "Choose the objects to import.";
            this.pageEntities.Location = new System.Drawing.Point(0, 0);
            this.pageEntities.Name = "pageEntities";
            this.pageEntities.Size = new System.Drawing.Size(653, 495);
            this.pageEntities.TabIndex = 8;
            this.pageEntities.Title = "Choose Objects";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkIgnoreRelations);
            this.panel2.Controls.Add(this.chkInheritance);
            this.panel2.Controls.Add(this.chkSettingPK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 441);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(653, 54);
            this.panel2.TabIndex = 75;
            // 
            // chkIgnoreRelations
            // 
            this.chkIgnoreRelations.AutoSize = true;
            this.chkIgnoreRelations.Location = new System.Drawing.Point(151, 31);
            this.chkIgnoreRelations.Name = "chkIgnoreRelations";
            this.chkIgnoreRelations.Size = new System.Drawing.Size(103, 17);
            this.chkIgnoreRelations.TabIndex = 73;
            this.chkIgnoreRelations.Text = "Ignore Relations";
            this.chkIgnoreRelations.UseVisualStyleBackColor = true;
            // 
            // chkInheritance
            // 
            this.chkInheritance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkInheritance.AutoSize = true;
            this.chkInheritance.Location = new System.Drawing.Point(151, 8);
            this.chkInheritance.Name = "chkInheritance";
            this.chkInheritance.Size = new System.Drawing.Size(119, 17);
            this.chkInheritance.TabIndex = 72;
            this.chkInheritance.Text = "Assume Inheritance";
            this.chkInheritance.UseVisualStyleBackColor = true;
            // 
            // chkSettingPK
            // 
            this.chkSettingPK.AutoSize = true;
            this.chkSettingPK.Checked = true;
            this.chkSettingPK.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSettingPK.Location = new System.Drawing.Point(8, 8);
            this.chkSettingPK.Name = "chkSettingPK";
            this.chkSettingPK.Size = new System.Drawing.Size(124, 17);
            this.chkSettingPK.TabIndex = 70;
            this.chkSettingPK.Text = "Override Primary Key";
            this.chkSettingPK.UseVisualStyleBackColor = true;
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMain.Controls.Add(this.cmdViewDiff);
            this.pnlMain.Controls.Add(this.tabControl1);
            this.pnlMain.Location = new System.Drawing.Point(0, 78);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMain.Size = new System.Drawing.Size(653, 363);
            this.pnlMain.TabIndex = 73;
            // 
            // cmdViewDiff
            // 
            this.cmdViewDiff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdViewDiff.Enabled = false;
            this.cmdViewDiff.Location = new System.Drawing.Point(500, 345);
            this.cmdViewDiff.Name = "cmdViewDiff";
            this.cmdViewDiff.Size = new System.Drawing.Size(140, 23);
            this.cmdViewDiff.TabIndex = 73;
            this.cmdViewDiff.Text = "View Differences";
            this.cmdViewDiff.UseVisualStyleBackColor = true;
            this.cmdViewDiff.Click += new System.EventHandler(this.cmdViewDiff_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(10, 10);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(633, 329);
            this.tabControl1.TabIndex = 70;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tvwAdd);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(625, 303);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Add";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tvwAdd
            // 
            this.tvwAdd.CheckBoxes = true;
            this.tvwAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwAdd.HideSelection = false;
            this.tvwAdd.Location = new System.Drawing.Point(3, 3);
            this.tvwAdd.Name = "tvwAdd";
            this.tvwAdd.Size = new System.Drawing.Size(619, 297);
            this.tvwAdd.TabIndex = 68;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tvwRefresh);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(625, 303);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Refresh";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tvwRefresh
            // 
            this.tvwRefresh.CheckBoxes = true;
            this.tvwRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwRefresh.HideSelection = false;
            this.tvwRefresh.Location = new System.Drawing.Point(3, 3);
            this.tvwRefresh.Name = "tvwRefresh";
            this.tvwRefresh.Size = new System.Drawing.Size(619, 297);
            this.tvwRefresh.TabIndex = 69;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tvwDelete);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(625, 303);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Delete";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tvwDelete
            // 
            this.tvwDelete.CheckBoxes = true;
            this.tvwDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwDelete.HideSelection = false;
            this.tvwDelete.Location = new System.Drawing.Point(3, 3);
            this.tvwDelete.Name = "tvwDelete";
            this.tvwDelete.Size = new System.Drawing.Size(619, 297);
            this.tvwDelete.TabIndex = 69;
            // 
            // ImportDatabaseForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(653, 543);
            this.Controls.Add(this.wizard1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(659, 582);
            this.Name = "ImportDatabaseForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Model Wizard";
            this.wizard1.ResumeLayout(false);
            this.wizard1.PerformLayout();
            this.pageConnection.ResumeLayout(false);
            this.pageConnection.PerformLayout();
            this.grpConnectionStringPostgres.ResumeLayout(false);
            this.grpConnectionStringPostgres.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pageSummary.ResumeLayout(false);
            this.pageEntities.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ImageList imageList1;
		private nHydrate.Wizard.Wizard wizard1;
		private nHydrate.Wizard.WizardPage pageConnection;
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
		private nHydrate.DslPackage.Forms.DatabaseConnectionControl DatabaseConnectionControl1;
		private System.Windows.Forms.Button cmdTestConnection;
		private System.Windows.Forms.CheckBox chkInheritance;
		private System.Windows.Forms.CheckBox chkIgnoreRelations;
		private System.Windows.Forms.Button cmdViewDiff;
		private Wizard.WizardPage pageSummary;
		private FastColoredTextBoxNS.FastColoredTextBox txtSummary;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton optDatabaseTypePostgres;
        private System.Windows.Forms.RadioButton optDatabaseTypeSQL;
        private System.Windows.Forms.GroupBox grpConnectionStringPostgres;
        private System.Windows.Forms.Label lblConnectionString;
        private Generator.Common.Forms.CueTextBox txtConnectionStringPostgres;
    }
}