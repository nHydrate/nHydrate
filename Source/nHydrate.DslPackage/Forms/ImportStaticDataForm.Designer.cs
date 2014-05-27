namespace nHydrate.DslPackage.Forms
{
	partial class ImportStaticDataForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportStaticDataForm));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.wizard1 = new nHydrate.Wizard.Wizard();
			this.pageSummary = new nHydrate.Wizard.WizardPage();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pageChooseTable = new nHydrate.Wizard.WizardPage();
			this.label1 = new System.Windows.Forms.Label();
			this.cboTable = new System.Windows.Forms.ComboBox();
			this.pageImport = new nHydrate.Wizard.WizardPage();
			this.DatabaseConnectionControl1 = new nHydrate.DslPackage.Forms.DatabaseConnectionControl();
			this.pageWelcome = new nHydrate.Wizard.WizardPage();
			this.lblWelcome = new System.Windows.Forms.Label();
			this.wizard1.SuspendLayout();
			this.pageSummary.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.pageChooseTable.SuspendLayout();
			this.pageImport.SuspendLayout();
			this.pageWelcome.SuspendLayout();
			this.SuspendLayout();
			// 
			// wizard1
			// 
			this.wizard1.BackColor = System.Drawing.SystemColors.Control;
			this.wizard1.ButtonFlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.wizard1.Controls.Add(this.pageWelcome);
			this.wizard1.Controls.Add(this.pageSummary);
			this.wizard1.Controls.Add(this.pageChooseTable);
			this.wizard1.Controls.Add(this.pageImport);
			this.wizard1.HeaderImage = ((System.Drawing.Image)(resources.GetObject("wizard1.HeaderImage")));
			this.wizard1.Location = new System.Drawing.Point(0, 0);
			this.wizard1.Name = "wizard1";
			this.wizard1.Size = new System.Drawing.Size(418, 387);
			this.wizard1.TabIndex = 0;
			this.wizard1.WizardPages.AddRange(new nHydrate.Wizard.WizardPage[] {
            this.pageWelcome,
            this.pageImport,
            this.pageChooseTable,
            this.pageSummary});
			// 
			// pageSummary
			// 
			this.pageSummary.Controls.Add(this.dataGridView1);
			this.pageSummary.Description = "This is a summary of the new modelRoot.";
			this.pageSummary.Location = new System.Drawing.Point(0, 0);
			this.pageSummary.Name = "pageSummary";
			this.pageSummary.Size = new System.Drawing.Size(418, 339);
			this.pageSummary.TabIndex = 9;
			this.pageSummary.Title = "Summary";
			// 
			// dataGridView1
			// 
			dataGridViewCellStyle2.NullValue = "null";
			this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
			this.dataGridView1.Location = new System.Drawing.Point(12, 71);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 20;
			this.dataGridView1.Size = new System.Drawing.Size(394, 254);
			this.dataGridView1.TabIndex = 1;
			// 
			// Column1
			// 
			this.Column1.HeaderText = "Column1";
			this.Column1.Name = "Column1";
			// 
			// Column2
			// 
			this.Column2.HeaderText = "Column2";
			this.Column2.Name = "Column2";
			// 
			// pageChooseTable
			// 
			this.pageChooseTable.Controls.Add(this.label1);
			this.pageChooseTable.Controls.Add(this.cboTable);
			this.pageChooseTable.Description = "This is a list of imported itemCache. Select the ones you wish to be in the model" +
    "Root.";
			this.pageChooseTable.Location = new System.Drawing.Point(0, 0);
			this.pageChooseTable.Name = "pageChooseTable";
			this.pageChooseTable.Size = new System.Drawing.Size(418, 339);
			this.pageChooseTable.TabIndex = 8;
			this.pageChooseTable.Title = "Imported itemCache";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 77);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Choose entity:";
			// 
			// cboTable
			// 
			this.cboTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTable.FormattingEnabled = true;
			this.cboTable.Location = new System.Drawing.Point(110, 74);
			this.cboTable.Name = "cboTable";
			this.cboTable.Size = new System.Drawing.Size(296, 21);
			this.cboTable.TabIndex = 0;
			// 
			// pageImport
			// 
			this.pageImport.Controls.Add(this.DatabaseConnectionControl1);
			this.pageImport.Description = "Import a modelRoot from a database or skip this step";
			this.pageImport.Location = new System.Drawing.Point(0, 0);
			this.pageImport.Name = "pageImport";
			this.pageImport.Size = new System.Drawing.Size(418, 339);
			this.pageImport.TabIndex = 10;
			this.pageImport.Title = "Import Model";
			// 
			// DatabaseConnectionControl1
			// 
			this.DatabaseConnectionControl1.AutoSize = true;
			this.DatabaseConnectionControl1.FileName = "C:\\Users\\chrisd\\AppData\\Roaming\\nHydrate\\nHydrate.ConnectionDialog.config.xml";
			this.DatabaseConnectionControl1.Location = new System.Drawing.Point(12, 72);
			this.DatabaseConnectionControl1.Name = "DatabaseConnectionControl1";
			this.DatabaseConnectionControl1.Size = new System.Drawing.Size(403, 260);
			this.DatabaseConnectionControl1.TabIndex = 3;
			// 
			// pageWelcome
			// 
			this.pageWelcome.Controls.Add(this.lblWelcome);
			this.pageWelcome.Description = "Welcome to the modelRoot setup wizard";
			this.pageWelcome.Location = new System.Drawing.Point(0, 0);
			this.pageWelcome.Name = "pageWelcome";
			this.pageWelcome.Size = new System.Drawing.Size(418, 339);
			this.pageWelcome.TabIndex = 11;
			this.pageWelcome.Title = "Welcome";
			// 
			// lblWelcome
			// 
			this.lblWelcome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblWelcome.Location = new System.Drawing.Point(12, 73);
			this.lblWelcome.Name = "lblWelcome";
			this.lblWelcome.Size = new System.Drawing.Size(394, 113);
			this.lblWelcome.TabIndex = 1;
			this.lblWelcome.Text = "WELCOME";
			// 
			// ImportStaticDataForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(418, 387);
			this.Controls.Add(this.wizard1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ImportStaticDataForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Import Static Data";
			this.wizard1.ResumeLayout(false);
			this.wizard1.PerformLayout();
			this.pageSummary.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.pageChooseTable.ResumeLayout(false);
			this.pageChooseTable.PerformLayout();
			this.pageImport.ResumeLayout(false);
			this.pageImport.PerformLayout();
			this.pageWelcome.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private nHydrate.Wizard.Wizard wizard1;
		private nHydrate.Wizard.WizardPage pageWelcome;
		private System.Windows.Forms.Label lblWelcome;
		private nHydrate.Wizard.WizardPage pageSummary;
		private nHydrate.Wizard.WizardPage pageChooseTable;
		private nHydrate.Wizard.WizardPage pageImport;
		private DatabaseConnectionControl DatabaseConnectionControl1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboTable;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
	}
}