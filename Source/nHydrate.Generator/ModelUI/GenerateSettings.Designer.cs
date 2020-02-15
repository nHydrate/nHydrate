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
            this.pageTemplates = new nHydrate.Wizard.WizardPage();
            this.label2 = new System.Windows.Forms.Label();
            this.linkShowAll = new System.Windows.Forms.LinkLabel();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstDependency = new System.Windows.Forms.ListBox();
            this.tvwProjects = new System.Windows.Forms.TreeView();
            this.pageFinish = new nHydrate.Wizard.WizardPage();
            this.lblFinish = new System.Windows.Forms.Label();
            this.wizard1.SuspendLayout();
            this.pageTemplates.SuspendLayout();
            this.pageFinish.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizard1
            // 
            this.wizard1.ButtonFlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.wizard1.Controls.Add(this.pageFinish);
            this.wizard1.Controls.Add(this.pageTemplates);
            this.wizard1.HeaderImage = ((System.Drawing.Image)(resources.GetObject("wizard1.HeaderImage")));
            this.wizard1.Location = new System.Drawing.Point(0, 0);
            this.wizard1.Name = "wizard1";
            this.wizard1.Size = new System.Drawing.Size(518, 364);
            this.wizard1.TabIndex = 0;
            this.wizard1.WizardPages.AddRange(new nHydrate.Wizard.WizardPage[] {
            this.pageTemplates,
            this.pageFinish});
            // 
            // pageTemplates
            // 
            this.pageTemplates.Controls.Add(this.label2);
            this.pageTemplates.Controls.Add(this.linkShowAll);
            this.pageTemplates.Controls.Add(this.txtDescription);
            this.pageTemplates.Controls.Add(this.label1);
            this.pageTemplates.Controls.Add(this.lstDependency);
            this.pageTemplates.Controls.Add(this.tvwProjects);
            this.pageTemplates.Description = "Choose the generators that will create your code";
            this.pageTemplates.Location = new System.Drawing.Point(0, 0);
            this.pageTemplates.Name = "pageTemplates";
            this.pageTemplates.Size = new System.Drawing.Size(518, 316);
            this.pageTemplates.TabIndex = 7;
            this.pageTemplates.Title = "Generators";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.PapayaWhip;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(494, 40);
            this.label2.TabIndex = 11;
            this.label2.Text = "NOTE: This version of nHydrate only supports .NET Core. If you are using Entity F" +
    "ramework 4.x - 6.x, do not generate with this modeler. The older modeler is avai" +
    "lable on the nHydrate.com website.";
            // 
            // linkShowAll
            // 
            this.linkShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkShowAll.AutoSize = true;
            this.linkShowAll.Location = new System.Drawing.Point(12, 272);
            this.linkShowAll.Name = "linkShowAll";
            this.linkShowAll.Size = new System.Drawing.Size(48, 13);
            this.linkShowAll.TabIndex = 10;
            this.linkShowAll.TabStop = true;
            this.linkShowAll.Text = "Show All";
            this.linkShowAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkShowAll_LinkClicked);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(331, 126);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(175, 136);
            this.txtDescription.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(128, 272);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(423, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "The dependencies for the selected project are listed below. You do not need to re" +
    "generate all projects all the time, but the projects below MUST exist in order f" +
    "or the selected project to compile.";
            this.label1.Visible = false;
            // 
            // lstDependency
            // 
            this.lstDependency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDependency.FormattingEnabled = true;
            this.lstDependency.IntegralHeight = false;
            this.lstDependency.Location = new System.Drawing.Point(12, 289);
            this.lstDependency.Name = "lstDependency";
            this.lstDependency.Size = new System.Drawing.Size(494, 62);
            this.lstDependency.TabIndex = 8;
            this.lstDependency.Visible = false;
            // 
            // tvwProjects
            // 
            this.tvwProjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvwProjects.CheckBoxes = true;
            this.tvwProjects.HideSelection = false;
            this.tvwProjects.Location = new System.Drawing.Point(12, 126);
            this.tvwProjects.Name = "tvwProjects";
            this.tvwProjects.Size = new System.Drawing.Size(304, 136);
            this.tvwProjects.TabIndex = 6;
            // 
            // pageFinish
            // 
            this.pageFinish.Controls.Add(this.lblFinish);
            this.pageFinish.Description = "Generator selection complete!";
            this.pageFinish.Location = new System.Drawing.Point(0, 0);
            this.pageFinish.Name = "pageFinish";
            this.pageFinish.Size = new System.Drawing.Size(518, 316);
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
            // GenerateSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 364);
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
            this.pageTemplates.ResumeLayout(false);
            this.pageTemplates.PerformLayout();
            this.pageFinish.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private Wizard.Wizard wizard1;
		private Wizard.WizardPage pageFinish;
		private Wizard.WizardPage pageTemplates;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstDependency;
		private System.Windows.Forms.TreeView tvwProjects;
		private System.Windows.Forms.Label lblFinish;
        private System.Windows.Forms.LinkLabel linkShowAll;
        private System.Windows.Forms.Label label2;
    }
}