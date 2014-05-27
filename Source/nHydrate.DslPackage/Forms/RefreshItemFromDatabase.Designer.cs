namespace nHydrate.DslPackage.Forms
{
	partial class RefreshItemFromDatabase
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RefreshItemFromDatabase));
			this.wizard1 = new nHydrate.Wizard.Wizard();
			this.pageDatabase = new nHydrate.Wizard.WizardPage();
			this.DatabaseConnectionControl1 = new nHydrate.DslPackage.Forms.DatabaseConnectionControl();
			this.pageLast = new nHydrate.Wizard.WizardPage();
			this.cmdViewDiff = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.pageChoose = new nHydrate.Wizard.WizardPage();
			this.cboItem = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lblError = new System.Windows.Forms.Label();
			this.wizard1.SuspendLayout();
			this.pageDatabase.SuspendLayout();
			this.pageLast.SuspendLayout();
			this.pageChoose.SuspendLayout();
			this.SuspendLayout();
			// 
			// wizard1
			// 
			this.wizard1.ButtonFlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.wizard1.Controls.Add(this.pageLast);
			this.wizard1.Controls.Add(this.pageChoose);
			this.wizard1.Controls.Add(this.pageDatabase);
			this.wizard1.HeaderImage = ((System.Drawing.Image)(resources.GetObject("wizard1.HeaderImage")));
			this.wizard1.Location = new System.Drawing.Point(0, 0);
			this.wizard1.Name = "wizard1";
			this.wizard1.Size = new System.Drawing.Size(597, 369);
			this.wizard1.TabIndex = 0;
			this.wizard1.WizardPages.AddRange(new nHydrate.Wizard.WizardPage[] {
            this.pageDatabase,
            this.pageChoose,
            this.pageLast});
			// 
			// pageDatabase
			// 
			this.pageDatabase.Controls.Add(this.DatabaseConnectionControl1);
			this.pageDatabase.Description = "Specify the source database from which to refresh this object.";
			this.pageDatabase.Location = new System.Drawing.Point(0, 0);
			this.pageDatabase.Name = "pageDatabase";
			this.pageDatabase.Size = new System.Drawing.Size(597, 321);
			this.pageDatabase.TabIndex = 7;
			this.pageDatabase.Title = "Database Connection";
			// 
			// DatabaseConnectionControl1
			// 
			this.DatabaseConnectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DatabaseConnectionControl1.AutoSize = true;
			this.DatabaseConnectionControl1.FileName = "";
			this.DatabaseConnectionControl1.Location = new System.Drawing.Point(12, 73);
			this.DatabaseConnectionControl1.Name = "DatabaseConnectionControl1";
			this.DatabaseConnectionControl1.Size = new System.Drawing.Size(573, 260);
			this.DatabaseConnectionControl1.TabIndex = 83;
			// 
			// pageLast
			// 
			this.pageLast.Controls.Add(this.lblError);
			this.pageLast.Controls.Add(this.cmdViewDiff);
			this.pageLast.Controls.Add(this.label1);
			this.pageLast.Description = "Press the \'Finish\' button to complete the import process.";
			this.pageLast.Location = new System.Drawing.Point(0, 0);
			this.pageLast.Name = "pageLast";
			this.pageLast.Size = new System.Drawing.Size(597, 321);
			this.pageLast.TabIndex = 9;
			this.pageLast.Title = "Verify and Complete";
			// 
			// cmdViewDiff
			// 
			this.cmdViewDiff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdViewDiff.Location = new System.Drawing.Point(445, 143);
			this.cmdViewDiff.Name = "cmdViewDiff";
			this.cmdViewDiff.Size = new System.Drawing.Size(140, 23);
			this.cmdViewDiff.TabIndex = 1;
			this.cmdViewDiff.Text = "View Differences";
			this.cmdViewDiff.UseVisualStyleBackColor = true;
			this.cmdViewDiff.Click += new System.EventHandler(this.cmdViewDiff_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(13, 82);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(572, 48);
			this.label1.TabIndex = 0;
			this.label1.Text = "You may view differences between the existing and imported objects by pressing th" +
    "e button below. When complete you can apply the new changes by pressing the \'Fin" +
    "ish\' button.";
			// 
			// pageChoose
			// 
			this.pageChoose.Controls.Add(this.cboItem);
			this.pageChoose.Controls.Add(this.label2);
			this.pageChoose.Description = "Choose the database object to use as the source of the refresh.";
			this.pageChoose.Location = new System.Drawing.Point(0, 0);
			this.pageChoose.Name = "pageChoose";
			this.pageChoose.Size = new System.Drawing.Size(597, 321);
			this.pageChoose.TabIndex = 8;
			this.pageChoose.Title = "Choose Object";
			// 
			// cboItem
			// 
			this.cboItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboItem.FormattingEnabled = true;
			this.cboItem.Location = new System.Drawing.Point(140, 79);
			this.cboItem.Name = "cboItem";
			this.cboItem.Size = new System.Drawing.Size(445, 21);
			this.cboItem.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(17, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Database object:";
			// 
			// lblError
			// 
			this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblError.ForeColor = System.Drawing.Color.Red;
			this.lblError.Location = new System.Drawing.Point(12, 179);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(573, 48);
			this.lblError.TabIndex = 2;
			// 
			// RefreshItemFromDatabase
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(597, 369);
			this.Controls.Add(this.wizard1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RefreshItemFromDatabase";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Refresh item from Database";
			this.wizard1.ResumeLayout(false);
			this.wizard1.PerformLayout();
			this.pageDatabase.ResumeLayout(false);
			this.pageDatabase.PerformLayout();
			this.pageLast.ResumeLayout(false);
			this.pageChoose.ResumeLayout(false);
			this.pageChoose.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Wizard.Wizard wizard1;
		private Wizard.WizardPage pageChoose;
		private Wizard.WizardPage pageDatabase;
		private DatabaseConnectionControl DatabaseConnectionControl1;
		private System.Windows.Forms.ComboBox cboItem;
		private System.Windows.Forms.Label label2;
		private Wizard.WizardPage pageLast;
		private System.Windows.Forms.Button cmdViewDiff;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblError;

	}
}