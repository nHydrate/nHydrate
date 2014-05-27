namespace nHydrate.DslPackage.Forms
{
	partial class RefactorSplitTableForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RefactorSplitTableForm));
			this.wizard1 = new nHydrate.Wizard.Wizard();
			this.pagePrimaryKey = new nHydrate.Wizard.WizardPage();
			this.label5 = new System.Windows.Forms.Label();
			this.chkMakeRelation = new System.Windows.Forms.CheckBox();
			this.chkPKIdentity = new System.Windows.Forms.CheckBox();
			this.chkUsePK = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.pageFirst = new nHydrate.Wizard.WizardPage();
			this.txtEntityName = new System.Windows.Forms.TextBox();
			this.lblSourceEntity = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblFirstHeader = new System.Windows.Forms.Label();
			this.pageFinish = new nHydrate.Wizard.WizardPage();
			this.label4 = new System.Windows.Forms.Label();
			this.pageFields = new nHydrate.Wizard.WizardPage();
			this.cmdFieldMoveLeft = new System.Windows.Forms.Button();
			this.cmdFieldMoveRight = new System.Windows.Forms.Button();
			this.lstField2 = new System.Windows.Forms.ListBox();
			this.lstField1 = new System.Windows.Forms.ListBox();
			this.wizard1.SuspendLayout();
			this.pagePrimaryKey.SuspendLayout();
			this.pageFirst.SuspendLayout();
			this.pageFinish.SuspendLayout();
			this.pageFields.SuspendLayout();
			this.SuspendLayout();
			// 
			// wizard1
			// 
			this.wizard1.ButtonFlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.wizard1.Controls.Add(this.pageFirst);
			this.wizard1.Controls.Add(this.pageFinish);
			this.wizard1.Controls.Add(this.pageFields);
			this.wizard1.Controls.Add(this.pagePrimaryKey);
			this.wizard1.HeaderImage = ((System.Drawing.Image)(resources.GetObject("wizard1.HeaderImage")));
			this.wizard1.Location = new System.Drawing.Point(0, 0);
			this.wizard1.Name = "wizard1";
			this.wizard1.Size = new System.Drawing.Size(513, 354);
			this.wizard1.TabIndex = 0;
			this.wizard1.WizardPages.AddRange(new nHydrate.Wizard.WizardPage[] {
            this.pageFirst,
            this.pagePrimaryKey,
            this.pageFields,
            this.pageFinish});
			this.wizard1.BeforeSwitchPages += new nHydrate.Wizard.Wizard.BeforeSwitchPagesEventHandler(this.wizard1_BeforeSwitchPages);
			this.wizard1.AfterSwitchPages += new nHydrate.Wizard.Wizard.AfterSwitchPagesEventHandler(this.wizard1_AfterSwitchPages);
			this.wizard1.Finish += new System.EventHandler(this.wizard1_Finish);
			// 
			// pagePrimaryKey
			// 
			this.pagePrimaryKey.Controls.Add(this.label5);
			this.pagePrimaryKey.Controls.Add(this.chkMakeRelation);
			this.pagePrimaryKey.Controls.Add(this.chkPKIdentity);
			this.pagePrimaryKey.Controls.Add(this.chkUsePK);
			this.pagePrimaryKey.Controls.Add(this.label3);
			this.pagePrimaryKey.Location = new System.Drawing.Point(0, 0);
			this.pagePrimaryKey.Name = "pagePrimaryKey";
			this.pagePrimaryKey.Size = new System.Drawing.Size(513, 306);
			this.pagePrimaryKey.TabIndex = 9;
			this.pagePrimaryKey.Title = "Choose to include the primary keys from the original Entity";
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(13, 173);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(488, 54);
			this.label5.TabIndex = 2;
			this.label5.Text = resources.GetString("label5.Text");
			// 
			// chkMakeRelation
			// 
			this.chkMakeRelation.Checked = true;
			this.chkMakeRelation.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMakeRelation.Location = new System.Drawing.Point(36, 242);
			this.chkMakeRelation.Name = "chkMakeRelation";
			this.chkMakeRelation.Size = new System.Drawing.Size(162, 17);
			this.chkMakeRelation.TabIndex = 3;
			this.chkMakeRelation.Text = "Create a relation";
			this.chkMakeRelation.UseVisualStyleBackColor = true;
			this.chkMakeRelation.CheckedChanged += new System.EventHandler(this.chkMakeRelation_CheckedChanged);
			// 
			// chkPKIdentity
			// 
			this.chkPKIdentity.AutoSize = true;
			this.chkPKIdentity.Location = new System.Drawing.Point(66, 143);
			this.chkPKIdentity.Name = "chkPKIdentity";
			this.chkPKIdentity.Size = new System.Drawing.Size(237, 17);
			this.chkPKIdentity.TabIndex = 2;
			this.chkPKIdentity.Text = "Make the new Entity\'s primary key an identity";
			this.chkPKIdentity.UseVisualStyleBackColor = true;
			this.chkPKIdentity.CheckedChanged += new System.EventHandler(this.chkPKIdentity_CheckedChanged);
			// 
			// chkUsePK
			// 
			this.chkUsePK.AutoSize = true;
			this.chkUsePK.Checked = true;
			this.chkUsePK.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkUsePK.Location = new System.Drawing.Point(36, 120);
			this.chkUsePK.Name = "chkUsePK";
			this.chkUsePK.Size = new System.Drawing.Size(159, 17);
			this.chkUsePK.TabIndex = 1;
			this.chkUsePK.Text = "Use primary key from source";
			this.chkUsePK.UseVisualStyleBackColor = true;
			this.chkUsePK.CheckedChanged += new System.EventHandler(this.chkUsePK_CheckedChanged);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(13, 70);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(488, 46);
			this.label3.TabIndex = 0;
			this.label3.Text = "You may choose to include the primary keys from the source Entity or you can setu" +
    "p the primary keys later after creation. Data will be copied between tables only" +
    " if the source primary key is used.";
			// 
			// pageFirst
			// 
			this.pageFirst.Controls.Add(this.txtEntityName);
			this.pageFirst.Controls.Add(this.lblSourceEntity);
			this.pageFirst.Controls.Add(this.label2);
			this.pageFirst.Controls.Add(this.label1);
			this.pageFirst.Controls.Add(this.lblFirstHeader);
			this.pageFirst.Location = new System.Drawing.Point(0, 0);
			this.pageFirst.Name = "pageFirst";
			this.pageFirst.Size = new System.Drawing.Size(513, 306);
			this.pageFirst.TabIndex = 7;
			this.pageFirst.Title = "Split an existing Entity into two Entities";
			// 
			// txtEntityName
			// 
			this.txtEntityName.Location = new System.Drawing.Point(116, 158);
			this.txtEntityName.Name = "txtEntityName";
			this.txtEntityName.Size = new System.Drawing.Size(201, 20);
			this.txtEntityName.TabIndex = 0;
			// 
			// lblSourceEntity
			// 
			this.lblSourceEntity.AutoSize = true;
			this.lblSourceEntity.Location = new System.Drawing.Point(113, 134);
			this.lblSourceEntity.Name = "lblSourceEntity";
			this.lblSourceEntity.Size = new System.Drawing.Size(46, 13);
			this.lblSourceEntity.TabIndex = 2;
			this.lblSourceEntity.Text = "ENTITY";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 165);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "New Entity:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 134);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Source Entity:";
			// 
			// lblFirstHeader
			// 
			this.lblFirstHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFirstHeader.Location = new System.Drawing.Point(13, 78);
			this.lblFirstHeader.Name = "lblFirstHeader";
			this.lblFirstHeader.Size = new System.Drawing.Size(488, 40);
			this.lblFirstHeader.TabIndex = 0;
			this.lblFirstHeader.Text = resources.GetString("lblFirstHeader.Text");
			// 
			// pageFinish
			// 
			this.pageFinish.Controls.Add(this.label4);
			this.pageFinish.Location = new System.Drawing.Point(0, 0);
			this.pageFinish.Name = "pageFinish";
			this.pageFinish.Size = new System.Drawing.Size(513, 306);
			this.pageFinish.TabIndex = 11;
			this.pageFinish.Title = "Complete Split";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(13, 74);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(274, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Press the Finish button to split the Entity into two Entities.";
			// 
			// pageFields
			// 
			this.pageFields.Controls.Add(this.cmdFieldMoveLeft);
			this.pageFields.Controls.Add(this.cmdFieldMoveRight);
			this.pageFields.Controls.Add(this.lstField2);
			this.pageFields.Controls.Add(this.lstField1);
			this.pageFields.Location = new System.Drawing.Point(0, 0);
			this.pageFields.Name = "pageFields";
			this.pageFields.Size = new System.Drawing.Size(513, 306);
			this.pageFields.TabIndex = 8;
			this.pageFields.Title = "Choose the fields to move to the new Entity";
			// 
			// cmdFieldMoveLeft
			// 
			this.cmdFieldMoveLeft.Location = new System.Drawing.Point(244, 169);
			this.cmdFieldMoveLeft.Name = "cmdFieldMoveLeft";
			this.cmdFieldMoveLeft.Size = new System.Drawing.Size(23, 23);
			this.cmdFieldMoveLeft.TabIndex = 6;
			this.cmdFieldMoveLeft.Text = "<";
			this.cmdFieldMoveLeft.UseVisualStyleBackColor = true;
			this.cmdFieldMoveLeft.Click += new System.EventHandler(this.cmdFieldMoveLeft_Click);
			// 
			// cmdFieldMoveRight
			// 
			this.cmdFieldMoveRight.Location = new System.Drawing.Point(244, 140);
			this.cmdFieldMoveRight.Name = "cmdFieldMoveRight";
			this.cmdFieldMoveRight.Size = new System.Drawing.Size(23, 23);
			this.cmdFieldMoveRight.TabIndex = 5;
			this.cmdFieldMoveRight.Text = ">";
			this.cmdFieldMoveRight.UseVisualStyleBackColor = true;
			this.cmdFieldMoveRight.Click += new System.EventHandler(this.cmdFieldMoveRight_Click);
			// 
			// lstField2
			// 
			this.lstField2.FormattingEnabled = true;
			this.lstField2.IntegralHeight = false;
			this.lstField2.Location = new System.Drawing.Point(273, 76);
			this.lstField2.Name = "lstField2";
			this.lstField2.Size = new System.Drawing.Size(225, 217);
			this.lstField2.TabIndex = 7;
			// 
			// lstField1
			// 
			this.lstField1.FormattingEnabled = true;
			this.lstField1.IntegralHeight = false;
			this.lstField1.Location = new System.Drawing.Point(12, 76);
			this.lstField1.Name = "lstField1";
			this.lstField1.Size = new System.Drawing.Size(225, 217);
			this.lstField1.TabIndex = 4;
			// 
			// RefactorSplitTableForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(513, 354);
			this.Controls.Add(this.wizard1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RefactorSplitTableForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Refactor - Split Entity";
			this.wizard1.ResumeLayout(false);
			this.wizard1.PerformLayout();
			this.pagePrimaryKey.ResumeLayout(false);
			this.pagePrimaryKey.PerformLayout();
			this.pageFirst.ResumeLayout(false);
			this.pageFirst.PerformLayout();
			this.pageFinish.ResumeLayout(false);
			this.pageFinish.PerformLayout();
			this.pageFields.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Wizard.Wizard wizard1;
		private Wizard.WizardPage pageFirst;
		private System.Windows.Forms.TextBox txtEntityName;
		private System.Windows.Forms.Label lblSourceEntity;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblFirstHeader;
		private Wizard.WizardPage pageFields;
		private System.Windows.Forms.ListBox lstField2;
		private System.Windows.Forms.ListBox lstField1;
		private System.Windows.Forms.Button cmdFieldMoveLeft;
		private System.Windows.Forms.Button cmdFieldMoveRight;
		private Wizard.WizardPage pagePrimaryKey;
		private System.Windows.Forms.CheckBox chkUsePK;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkMakeRelation;
		private System.Windows.Forms.CheckBox chkPKIdentity;
		private Wizard.WizardPage pageFinish;
		private System.Windows.Forms.Label label4;
	}
}