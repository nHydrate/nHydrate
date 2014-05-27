namespace nHydrate.DslPackage.Forms
{
	partial class EntityAuditMappings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityAuditMappings));
			this.tvwItem = new System.Windows.Forms.TreeView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCollapse = new System.Windows.Forms.Button();
			this.cmdExpand = new System.Windows.Forms.Button();
			this.cmdUncheck = new System.Windows.Forms.Button();
			this.cmdCheck = new System.Windows.Forms.Button();
			this.panel3 = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblLine100 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// tvwItem
			// 
			this.tvwItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tvwItem.CheckBoxes = true;
			this.tvwItem.HideSelection = false;
			this.tvwItem.Location = new System.Drawing.Point(12, 71);
			this.tvwItem.Name = "tvwItem";
			this.tvwItem.Size = new System.Drawing.Size(590, 490);
			this.tvwItem.TabIndex = 0;
			this.tvwItem.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvwItem_AfterCheck);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.cmdCancel);
			this.panel2.Controls.Add(this.cmdOK);
			this.panel2.Controls.Add(this.cmdCollapse);
			this.panel2.Controls.Add(this.cmdExpand);
			this.panel2.Controls.Add(this.cmdUncheck);
			this.panel2.Controls.Add(this.cmdCheck);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 567);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(609, 38);
			this.panel2.TabIndex = 3;
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.Location = new System.Drawing.Point(527, 6);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 6;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.Location = new System.Drawing.Point(446, 6);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 5;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCollapse
			// 
			this.cmdCollapse.Location = new System.Drawing.Point(256, 6);
			this.cmdCollapse.Name = "cmdCollapse";
			this.cmdCollapse.Size = new System.Drawing.Size(75, 23);
			this.cmdCollapse.TabIndex = 4;
			this.cmdCollapse.Text = "Collapse All";
			this.cmdCollapse.UseVisualStyleBackColor = true;
			this.cmdCollapse.Click += new System.EventHandler(this.cmdCollapse_Click);
			// 
			// cmdExpand
			// 
			this.cmdExpand.Location = new System.Drawing.Point(175, 6);
			this.cmdExpand.Name = "cmdExpand";
			this.cmdExpand.Size = new System.Drawing.Size(75, 23);
			this.cmdExpand.TabIndex = 3;
			this.cmdExpand.Text = "Expand All";
			this.cmdExpand.UseVisualStyleBackColor = true;
			this.cmdExpand.Click += new System.EventHandler(this.cmdExpand_Click);
			// 
			// cmdUncheck
			// 
			this.cmdUncheck.Location = new System.Drawing.Point(93, 6);
			this.cmdUncheck.Name = "cmdUncheck";
			this.cmdUncheck.Size = new System.Drawing.Size(75, 23);
			this.cmdUncheck.TabIndex = 2;
			this.cmdUncheck.Text = "Uncheck All";
			this.cmdUncheck.UseVisualStyleBackColor = true;
			this.cmdUncheck.Click += new System.EventHandler(this.cmdUncheck_Click);
			// 
			// cmdCheck
			// 
			this.cmdCheck.Location = new System.Drawing.Point(12, 6);
			this.cmdCheck.Name = "cmdCheck";
			this.cmdCheck.Size = new System.Drawing.Size(75, 23);
			this.cmdCheck.TabIndex = 1;
			this.cmdCheck.Text = "Check All";
			this.cmdCheck.UseVisualStyleBackColor = true;
			this.cmdCheck.Click += new System.EventHandler(this.cmdCheck_Click);
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.Window;
			this.panel3.Controls.Add(this.pictureBox1);
			this.panel3.Controls.Add(this.lblLine100);
			this.panel3.Controls.Add(this.label6);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(609, 65);
			this.panel3.TabIndex = 86;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 72;
			this.pictureBox1.TabStop = false;
			// 
			// lblLine100
			// 
			this.lblLine100.BackColor = System.Drawing.Color.DarkGray;
			this.lblLine100.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblLine100.Location = new System.Drawing.Point(0, 63);
			this.lblLine100.Name = "lblLine100";
			this.lblLine100.Size = new System.Drawing.Size(609, 2);
			this.lblLine100.TabIndex = 71;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(91, 18);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(506, 38);
			this.label6.TabIndex = 68;
			this.label6.Text = "Modify the audit settings of all entities";
			// 
			// EntityAuditMappings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(609, 605);
			this.Controls.Add(this.tvwItem);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(515, 356);
			this.Name = "EntityAuditMappings";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Entity Auditing";
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView tvwItem;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button cmdUncheck;
		private System.Windows.Forms.Button cmdCheck;
		private System.Windows.Forms.Button cmdCollapse;
		private System.Windows.Forms.Button cmdExpand;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblLine100;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
	}
}