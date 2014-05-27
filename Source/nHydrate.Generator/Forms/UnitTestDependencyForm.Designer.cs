namespace Widgetsphere.Generator.Forms
{
	partial class UnitTestDependencyForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnitTestDependencyForm));
			this.cmdDown = new System.Windows.Forms.Button();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.cmdUp = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.lstMembers = new System.Windows.Forms.ListBox();
			this.cboTable = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdAddTable = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdAddAll = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmdDown
			// 
			this.cmdDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdDown.Image = ((System.Drawing.Image)(resources.GetObject("cmdDown.Image")));
			this.cmdDown.Location = new System.Drawing.Point(454, 72);
			this.cmdDown.Name = "cmdDown";
			this.cmdDown.Size = new System.Drawing.Size(24, 24);
			this.cmdDown.TabIndex = 4;
			this.cmdDown.Click += new System.EventHandler(this.cmdDown_Click);
			// 
			// cmdDelete
			// 
			this.cmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdDelete.Location = new System.Drawing.Point(8, 344);
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size(64, 23);
			this.cmdDelete.TabIndex = 5;
			this.cmdDelete.Text = "Delete";
			this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
			// 
			// cmdUp
			// 
			this.cmdUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdUp.Image = ((System.Drawing.Image)(resources.GetObject("cmdUp.Image")));
			this.cmdUp.Location = new System.Drawing.Point(454, 40);
			this.cmdUp.Name = "cmdUp";
			this.cmdUp.Size = new System.Drawing.Size(24, 24);
			this.cmdUp.TabIndex = 3;
			this.cmdUp.Click += new System.EventHandler(this.cmdUp_Click);
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdOK.Location = new System.Drawing.Point(344, 344);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(64, 23);
			this.cmdOK.TabIndex = 7;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// lstMembers
			// 
			this.lstMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lstMembers.IntegralHeight = false;
			this.lstMembers.Location = new System.Drawing.Point(8, 40);
			this.lstMembers.Name = "lstMembers";
			this.lstMembers.Size = new System.Drawing.Size(440, 298);
			this.lstMembers.TabIndex = 2;
			this.lstMembers.SelectedIndexChanged += new System.EventHandler(this.lstMembers_SelectedIndexChanged);
			// 
			// cboTable
			// 
			this.cboTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.cboTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTable.FormattingEnabled = true;
			this.cboTable.Location = new System.Drawing.Point(72, 8);
			this.cboTable.Name = "cboTable";
			this.cboTable.Size = new System.Drawing.Size(326, 21);
			this.cboTable.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 56;
			this.label1.Text = "Table List:";
			// 
			// cmdAddTable
			// 
			this.cmdAddTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdAddTable.Location = new System.Drawing.Point(406, 8);
			this.cmdAddTable.Name = "cmdAddTable";
			this.cmdAddTable.Size = new System.Drawing.Size(75, 23);
			this.cmdAddTable.TabIndex = 1;
			this.cmdAddTable.Text = "Add Table";
			this.cmdAddTable.UseVisualStyleBackColor = true;
			this.cmdAddTable.Click += new System.EventHandler(this.cmdAddTable_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdCancel.Location = new System.Drawing.Point(416, 344);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(64, 23);
			this.cmdCancel.TabIndex = 8;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdAddAll
			// 
			this.cmdAddAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdAddAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cmdAddAll.Location = new System.Drawing.Point(78, 344);
			this.cmdAddAll.Name = "cmdAddAll";
			this.cmdAddAll.Size = new System.Drawing.Size(107, 23);
			this.cmdAddAll.TabIndex = 6;
			this.cmdAddAll.Text = "Add All Tables";
			this.cmdAddAll.Click += new System.EventHandler(this.cmdAddAll_Click);
			// 
			// UnitTestDependencyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(488, 374);
			this.Controls.Add(this.cmdAddAll);
			this.Controls.Add(this.cmdAddTable);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboTable);
			this.Controls.Add(this.cmdDown);
			this.Controls.Add(this.cmdDelete);
			this.Controls.Add(this.cmdUp);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.lstMembers);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(350, 250);
			this.Name = "UnitTestDependencyForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "UnitTest Dependencies";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.Button cmdDown;
		protected System.Windows.Forms.Button cmdDelete;
		protected System.Windows.Forms.Button cmdUp;
		protected System.Windows.Forms.Button cmdOK;
		protected System.Windows.Forms.ListBox lstMembers;
		private System.Windows.Forms.ComboBox cboTable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdAddTable;
		protected System.Windows.Forms.Button cmdCancel;
		protected System.Windows.Forms.Button cmdAddAll;

	}
}