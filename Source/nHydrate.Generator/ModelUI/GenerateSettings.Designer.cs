namespace nHydrate.Generator.ModelUI
{
	partial class GenerateSettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateSettings));
			this.wizard1 = new nHydrate.Wizard.Wizard();
			this.pageFinish = new nHydrate.Wizard.WizardPage();
			this.lblFinish = new System.Windows.Forms.Label();
			this.pageModules = new nHydrate.Wizard.WizardPage();
			this.chkModule = new System.Windows.Forms.CheckedListBox();
			this.pageTemplates = new nHydrate.Wizard.WizardPage();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lstDependency = new System.Windows.Forms.ListBox();
			this.tvwProjects = new System.Windows.Forms.TreeView();
			this.wizard1.SuspendLayout();
			this.pageFinish.SuspendLayout();
			this.pageModules.SuspendLayout();
			this.pageTemplates.SuspendLayout();
			this.SuspendLayout();
			// 
			// wizard1
			// 
			this.wizard1.ButtonFlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.wizard1.Controls.Add(this.pageTemplates);
			this.wizard1.Controls.Add(this.pageFinish);
			this.wizard1.Controls.Add(this.pageModules);
			this.wizard1.HeaderImage = ((System.Drawing.Image)(resources.GetObject("wizard1.HeaderImage")));
			this.wizard1.Location = new System.Drawing.Point(0, 0);
			this.wizard1.Name = "wizard1";
			this.wizard1.Size = new System.Drawing.Size(518, 424);
			this.wizard1.TabIndex = 0;
			this.wizard1.WizardPages.AddRange(new nHydrate.Wizard.WizardPage[] {
            this.pageTemplates,
            this.pageModules,
            this.pageFinish});
			// 
			// pageFinish
			// 
			this.pageFinish.Controls.Add(this.lblFinish);
			this.pageFinish.Description = "Generator selection complete!";
			this.pageFinish.Location = new System.Drawing.Point(0, 0);
			this.pageFinish.Name = "pageFinish";
			this.pageFinish.Size = new System.Drawing.Size(518, 376);
			this.pageFinish.TabIndex = 9;
			this.pageFinish.Title = "Finish";
			// 
			// lblFinish
			// 
			this.lblFinish.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFinish.Location = new System.Drawing.Point(12, 78);
			this.lblFinish.Name = "lblFinish";
			this.lblFinish.Size = new System.Drawing.Size(494, 113);
			this.lblFinish.TabIndex = 0;
			this.lblFinish.Text = "Press the Finish button to apply the selected generators and create code.";
			// 
			// pageModules
			// 
			this.pageModules.Controls.Add(this.chkModule);
			this.pageModules.Description = "Choose the modules to apply the generators";
			this.pageModules.Location = new System.Drawing.Point(0, 0);
			this.pageModules.Name = "pageModules";
			this.pageModules.Size = new System.Drawing.Size(518, 376);
			this.pageModules.TabIndex = 8;
			this.pageModules.Title = "Modules";
			// 
			// chkModule
			// 
			this.chkModule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkModule.CheckOnClick = true;
			this.chkModule.FormattingEnabled = true;
			this.chkModule.IntegralHeight = false;
			this.chkModule.Location = new System.Drawing.Point(12, 74);
			this.chkModule.Name = "chkModule";
			this.chkModule.Size = new System.Drawing.Size(494, 299);
			this.chkModule.TabIndex = 4;
			// 
			// pageTemplates
			// 
			this.pageTemplates.Controls.Add(this.txtDescription);
			this.pageTemplates.Controls.Add(this.label1);
			this.pageTemplates.Controls.Add(this.lstDependency);
			this.pageTemplates.Controls.Add(this.tvwProjects);
			this.pageTemplates.Description = "Choose the generators that will create your code";
			this.pageTemplates.Location = new System.Drawing.Point(0, 0);
			this.pageTemplates.Name = "pageTemplates";
			this.pageTemplates.Size = new System.Drawing.Size(518, 376);
			this.pageTemplates.TabIndex = 7;
			this.pageTemplates.Title = "Generators";
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Location = new System.Drawing.Point(331, 74);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ReadOnly = true;
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(175, 165);
			this.txtDescription.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(9, 242);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(497, 39);
			this.label1.TabIndex = 9;
			this.label1.Text = "The dependencies for the selected project are listed below. You do not need to re" +
    "generate all projects all the time, but the projects below MUST exist in order f" +
    "or the selected project to compile.";
			// 
			// lstDependency
			// 
			this.lstDependency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstDependency.FormattingEnabled = true;
			this.lstDependency.IntegralHeight = false;
			this.lstDependency.Location = new System.Drawing.Point(12, 284);
			this.lstDependency.Name = "lstDependency";
			this.lstDependency.Size = new System.Drawing.Size(494, 78);
			this.lstDependency.TabIndex = 8;
			// 
			// tvwProjects
			// 
			this.tvwProjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tvwProjects.CheckBoxes = true;
			this.tvwProjects.Location = new System.Drawing.Point(12, 74);
			this.tvwProjects.Name = "tvwProjects";
			this.tvwProjects.Size = new System.Drawing.Size(304, 165);
			this.tvwProjects.TabIndex = 6;
			// 
			// GenerateSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(518, 424);
			this.Controls.Add(this.wizard1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GenerateSettings";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "nHydrate Project Generators";
			this.wizard1.ResumeLayout(false);
			this.wizard1.PerformLayout();
			this.pageFinish.ResumeLayout(false);
			this.pageModules.ResumeLayout(false);
			this.pageTemplates.ResumeLayout(false);
			this.pageTemplates.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Wizard.Wizard wizard1;
		private Wizard.WizardPage pageFinish;
		private Wizard.WizardPage pageModules;
		private Wizard.WizardPage pageTemplates;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstDependency;
		private System.Windows.Forms.TreeView tvwProjects;
		private System.Windows.Forms.CheckedListBox chkModule;
		private System.Windows.Forms.Label lblFinish;
	}
}