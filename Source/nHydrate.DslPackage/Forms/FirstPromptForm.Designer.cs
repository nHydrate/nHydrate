namespace nHydrate.DslPackage.Forms
{
	partial class FirstPromptForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtCompany = new nHydrate.Generator.Common.Forms.CueTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.lblNamespace = new System.Windows.Forms.Label();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdApply = new System.Windows.Forms.Button();
			this.lstPlatforms = new System.Windows.Forms.CheckedListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtProject = new nHydrate.Generator.Common.Forms.CueTextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 53);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Company name:";
			// 
			// txtCompany
			// 
			this.txtCompany.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCompany.Cue = "<Enter Company Name>";
			this.txtCompany.Location = new System.Drawing.Point(153, 53);
			this.txtCompany.Name = "txtCompany";
			this.txtCompany.Size = new System.Drawing.Size(311, 20);
			this.txtCompany.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(13, 105);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Root namespace:";
			// 
			// lblNamespace
			// 
			this.lblNamespace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblNamespace.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblNamespace.Location = new System.Drawing.Point(153, 105);
			this.lblNamespace.Name = "lblNamespace";
			this.lblNamespace.Size = new System.Drawing.Size(311, 20);
			this.lblNamespace.TabIndex = 2;
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.Location = new System.Drawing.Point(389, 210);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 4;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdApply
			// 
			this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdApply.Location = new System.Drawing.Point(308, 210);
			this.cmdApply.Name = "cmdApply";
			this.cmdApply.Size = new System.Drawing.Size(75, 23);
			this.cmdApply.TabIndex = 3;
			this.cmdApply.Text = "Apply";
			this.cmdApply.UseVisualStyleBackColor = true;
			this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
			// 
			// lstPlatforms
			// 
			this.lstPlatforms.CheckOnClick = true;
			this.lstPlatforms.FormattingEnabled = true;
			this.lstPlatforms.IntegralHeight = false;
			this.lstPlatforms.Location = new System.Drawing.Point(153, 132);
			this.lstPlatforms.Name = "lstPlatforms";
			this.lstPlatforms.Size = new System.Drawing.Size(311, 70);
			this.lstPlatforms.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 79);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Project name:";
			// 
			// txtProject
			// 
			this.txtProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtProject.Cue = "<Enter Project Name>";
			this.txtProject.Location = new System.Drawing.Point(153, 79);
			this.txtProject.Name = "txtProject";
			this.txtProject.Size = new System.Drawing.Size(311, 20);
			this.txtProject.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(13, 132);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(105, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Supported Platforms:";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(13, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(451, 38);
			this.label4.TabIndex = 5;
			this.label4.Text = "The company and project names are required. They are used to create the root name" +
    "space of generated projects. A model cannot be generated until these properties " +
    "have valid settings.";
			// 
			// FirstPromptForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(476, 245);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lstPlatforms);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdApply);
			this.Controls.Add(this.lblNamespace);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtProject);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtCompany);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FirstPromptForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Model Properties";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private nHydrate.Generator.Common.Forms.CueTextBox txtCompany;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblNamespace;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdApply;
		private System.Windows.Forms.CheckedListBox lstPlatforms;
		private System.Windows.Forms.Label label2;
		private nHydrate.Generator.Common.Forms.CueTextBox txtProject;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
	}
}