namespace nHydrate.DslPackage.Forms
{
	partial class ModelUtilitiesForm
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
            this.cmdClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdAudit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdLegacy = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdPrecedense = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdCleanUp = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cmdBulkRename = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cmdExport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(514, 194);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(108, 23);
            this.cmdClose.TabIndex = 7;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(495, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "This allows you to view all entities in a list and change thier auditing properti" +
    "es.";
            // 
            // cmdAudit
            // 
            this.cmdAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAudit.Location = new System.Drawing.Point(514, 13);
            this.cmdAudit.Name = "cmdAudit";
            this.cmdAudit.Size = new System.Drawing.Size(108, 23);
            this.cmdAudit.TabIndex = 0;
            this.cmdAudit.Text = "Auditing";
            this.cmdAudit.UseVisualStyleBackColor = true;
            this.cmdAudit.Click += new System.EventHandler(this.cmdAudit_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(13, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(495, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "This allows you to import a legacy model from the nHydrate 4.2 version.";
            // 
            // cmdLegacy
            // 
            this.cmdLegacy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdLegacy.Location = new System.Drawing.Point(514, 42);
            this.cmdLegacy.Name = "cmdLegacy";
            this.cmdLegacy.Size = new System.Drawing.Size(108, 23);
            this.cmdLegacy.TabIndex = 1;
            this.cmdLegacy.Text = "Import Legacy";
            this.cmdLegacy.UseVisualStyleBackColor = true;
            this.cmdLegacy.Click += new System.EventHandler(this.cmdLegacy_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(13, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(495, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "This allows you to reset the scripting order of model objects by setting a preced" +
    "ense order.";
            // 
            // cmdPrecedense
            // 
            this.cmdPrecedense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPrecedense.Location = new System.Drawing.Point(514, 71);
            this.cmdPrecedense.Name = "cmdPrecedense";
            this.cmdPrecedense.Size = new System.Drawing.Size(108, 23);
            this.cmdPrecedense.TabIndex = 2;
            this.cmdPrecedense.Text = "Set Precedence";
            this.cmdPrecedense.UseVisualStyleBackColor = true;
            this.cmdPrecedense.Click += new System.EventHandler(this.cmdPrecedense_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(13, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(495, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "This allows you to clean up columns in mass after an import.";
            // 
            // cmdCleanUp
            // 
            this.cmdCleanUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCleanUp.Location = new System.Drawing.Point(514, 100);
            this.cmdCleanUp.Name = "cmdCleanUp";
            this.cmdCleanUp.Size = new System.Drawing.Size(108, 23);
            this.cmdCleanUp.TabIndex = 3;
            this.cmdCleanUp.Text = "Clean Up";
            this.cmdCleanUp.UseVisualStyleBackColor = true;
            this.cmdCleanUp.Click += new System.EventHandler(this.cmdCleanUp_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(13, 129);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(495, 23);
            this.label6.TabIndex = 1;
            this.label6.Text = "Change entity names/facades in bulk";
            // 
            // cmdBulkRename
            // 
            this.cmdBulkRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBulkRename.Location = new System.Drawing.Point(514, 129);
            this.cmdBulkRename.Name = "cmdBulkRename";
            this.cmdBulkRename.Size = new System.Drawing.Size(108, 23);
            this.cmdBulkRename.TabIndex = 5;
            this.cmdBulkRename.Text = "Bulk Rename";
            this.cmdBulkRename.UseVisualStyleBackColor = true;
            this.cmdBulkRename.Click += new System.EventHandler(this.cmdBulkRename_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(13, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(495, 23);
            this.label7.TabIndex = 1;
            this.label7.Text = "Export the model artifacts to a CSV file";
            // 
            // cmdExport
            // 
            this.cmdExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdExport.Location = new System.Drawing.Point(514, 158);
            this.cmdExport.Name = "cmdExport";
            this.cmdExport.Size = new System.Drawing.Size(108, 23);
            this.cmdExport.TabIndex = 6;
            this.cmdExport.Text = "Export";
            this.cmdExport.UseVisualStyleBackColor = true;
            this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
            // 
            // ModelUtilitiesForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(634, 229);
            this.Controls.Add(this.cmdExport);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmdBulkRename);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmdCleanUp);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdPrecedense);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdLegacy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdAudit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelUtilitiesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Utilities";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdAudit;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cmdLegacy;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button cmdPrecedense;
		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdCleanUp;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button cmdBulkRename;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button cmdExport;
	}
}