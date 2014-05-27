namespace nHydrate.DslPackage.Forms
{
	partial class ModuleMappings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModuleMappings));
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblLine100 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdSave = new System.Windows.Forms.Button();
			this.cboModule = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.panel2 = new System.Windows.Forms.Panel();
			this.cmdCollapse = new System.Windows.Forms.Button();
			this.cmdExpand = new System.Windows.Forms.Button();
			this.cmdUncheck = new System.Windows.Forms.Button();
			this.cmdCheck = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.pageEntity = new System.Windows.Forms.TabPage();
			this.tvwEntity = new System.Windows.Forms.TreeView();
			this.pageView = new System.Windows.Forms.TabPage();
			this.tvwView = new System.Windows.Forms.TreeView();
			this.pageStoredProc = new System.Windows.Forms.TabPage();
			this.tvwStoredProc = new System.Windows.Forms.TreeView();
			this.pageFunction = new System.Windows.Forms.TabPage();
			this.tvwFunction = new System.Windows.Forms.TreeView();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.pageEntity.SuspendLayout();
			this.pageView.SuspendLayout();
			this.pageStoredProc.SuspendLayout();
			this.pageFunction.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.cmdCancel);
			this.panel1.Controls.Add(this.cmdSave);
			this.panel1.Controls.Add(this.cboModule);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(635, 119);
			this.panel1.TabIndex = 0;
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
			this.panel3.Size = new System.Drawing.Size(635, 65);
			this.panel3.TabIndex = 85;
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
			this.lblLine100.Size = new System.Drawing.Size(635, 2);
			this.lblLine100.TabIndex = 71;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(91, 18);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(532, 38);
			this.label6.TabIndex = 68;
			this.label6.Text = "Associate modules with other model objects";
			// 
			// cmdCancel
			// 
			this.cmdCancel.Enabled = false;
			this.cmdCancel.Location = new System.Drawing.Point(420, 80);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdSave
			// 
			this.cmdSave.Enabled = false;
			this.cmdSave.Location = new System.Drawing.Point(339, 80);
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(75, 23);
			this.cmdSave.TabIndex = 1;
			this.cmdSave.Text = "Save";
			this.cmdSave.UseVisualStyleBackColor = true;
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// cboModule
			// 
			this.cboModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboModule.FormattingEnabled = true;
			this.cboModule.Location = new System.Drawing.Point(82, 80);
			this.cboModule.Name = "cboModule";
			this.cboModule.Size = new System.Drawing.Size(249, 21);
			this.cboModule.TabIndex = 0;
			this.cboModule.SelectedIndexChanged += new System.EventHandler(this.cboModule_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Module:";
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "Entity.png");
			this.imageList1.Images.SetKeyName(1, "field.png");
			this.imageList1.Images.SetKeyName(2, "view.png");
			this.imageList1.Images.SetKeyName(3, "storedproc.png");
			this.imageList1.Images.SetKeyName(4, "function.png");
			this.imageList1.Images.SetKeyName(5, "relationship.png");
			this.imageList1.Images.SetKeyName(6, "index.png");
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.cmdCollapse);
			this.panel2.Controls.Add(this.cmdExpand);
			this.panel2.Controls.Add(this.cmdUncheck);
			this.panel2.Controls.Add(this.cmdCheck);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 491);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(635, 38);
			this.panel2.TabIndex = 3;
			// 
			// cmdCollapse
			// 
			this.cmdCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCollapse.Location = new System.Drawing.Point(548, 6);
			this.cmdCollapse.Name = "cmdCollapse";
			this.cmdCollapse.Size = new System.Drawing.Size(75, 23);
			this.cmdCollapse.TabIndex = 7;
			this.cmdCollapse.Text = "Collapse All";
			this.cmdCollapse.UseVisualStyleBackColor = true;
			this.cmdCollapse.Click += new System.EventHandler(this.cmdCollapse_Click);
			// 
			// cmdExpand
			// 
			this.cmdExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdExpand.Location = new System.Drawing.Point(467, 6);
			this.cmdExpand.Name = "cmdExpand";
			this.cmdExpand.Size = new System.Drawing.Size(75, 23);
			this.cmdExpand.TabIndex = 6;
			this.cmdExpand.Text = "Expand All";
			this.cmdExpand.UseVisualStyleBackColor = true;
			this.cmdExpand.Click += new System.EventHandler(this.cmdExpand_Click);
			// 
			// cmdUncheck
			// 
			this.cmdUncheck.Location = new System.Drawing.Point(93, 6);
			this.cmdUncheck.Name = "cmdUncheck";
			this.cmdUncheck.Size = new System.Drawing.Size(75, 23);
			this.cmdUncheck.TabIndex = 5;
			this.cmdUncheck.Text = "Uncheck All";
			this.cmdUncheck.UseVisualStyleBackColor = true;
			this.cmdUncheck.Click += new System.EventHandler(this.cmdUncheck_Click);
			// 
			// cmdCheck
			// 
			this.cmdCheck.Location = new System.Drawing.Point(12, 6);
			this.cmdCheck.Name = "cmdCheck";
			this.cmdCheck.Size = new System.Drawing.Size(75, 23);
			this.cmdCheck.TabIndex = 4;
			this.cmdCheck.Text = "Check All";
			this.cmdCheck.UseVisualStyleBackColor = true;
			this.cmdCheck.Click += new System.EventHandler(this.cmdCheck_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.pageEntity);
			this.tabControl1.Controls.Add(this.pageView);
			this.tabControl1.Controls.Add(this.pageStoredProc);
			this.tabControl1.Controls.Add(this.pageFunction);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 119);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(635, 372);
			this.tabControl1.TabIndex = 4;
			// 
			// pageEntity
			// 
			this.pageEntity.Controls.Add(this.tvwEntity);
			this.pageEntity.Location = new System.Drawing.Point(4, 22);
			this.pageEntity.Name = "pageEntity";
			this.pageEntity.Padding = new System.Windows.Forms.Padding(3);
			this.pageEntity.Size = new System.Drawing.Size(627, 346);
			this.pageEntity.TabIndex = 0;
			this.pageEntity.Text = "Entities";
			this.pageEntity.UseVisualStyleBackColor = true;
			// 
			// tvwEntity
			// 
			this.tvwEntity.CheckBoxes = true;
			this.tvwEntity.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwEntity.Enabled = false;
			this.tvwEntity.HideSelection = false;
			this.tvwEntity.ImageIndex = 0;
			this.tvwEntity.ImageList = this.imageList1;
			this.tvwEntity.Location = new System.Drawing.Point(3, 3);
			this.tvwEntity.Name = "tvwEntity";
			this.tvwEntity.SelectedImageIndex = 0;
			this.tvwEntity.Size = new System.Drawing.Size(621, 340);
			this.tvwEntity.TabIndex = 4;
			// 
			// pageView
			// 
			this.pageView.Controls.Add(this.tvwView);
			this.pageView.Location = new System.Drawing.Point(4, 22);
			this.pageView.Name = "pageView";
			this.pageView.Padding = new System.Windows.Forms.Padding(3);
			this.pageView.Size = new System.Drawing.Size(627, 346);
			this.pageView.TabIndex = 1;
			this.pageView.Text = "Views";
			this.pageView.UseVisualStyleBackColor = true;
			// 
			// tvwView
			// 
			this.tvwView.CheckBoxes = true;
			this.tvwView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwView.Enabled = false;
			this.tvwView.HideSelection = false;
			this.tvwView.ImageIndex = 0;
			this.tvwView.ImageList = this.imageList1;
			this.tvwView.Location = new System.Drawing.Point(3, 3);
			this.tvwView.Name = "tvwView";
			this.tvwView.SelectedImageIndex = 0;
			this.tvwView.Size = new System.Drawing.Size(621, 340);
			this.tvwView.TabIndex = 4;
			// 
			// pageStoredProc
			// 
			this.pageStoredProc.Controls.Add(this.tvwStoredProc);
			this.pageStoredProc.Location = new System.Drawing.Point(4, 22);
			this.pageStoredProc.Name = "pageStoredProc";
			this.pageStoredProc.Padding = new System.Windows.Forms.Padding(3);
			this.pageStoredProc.Size = new System.Drawing.Size(627, 346);
			this.pageStoredProc.TabIndex = 2;
			this.pageStoredProc.Text = "Stored Procedures";
			this.pageStoredProc.UseVisualStyleBackColor = true;
			// 
			// tvwStoredProc
			// 
			this.tvwStoredProc.CheckBoxes = true;
			this.tvwStoredProc.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwStoredProc.Enabled = false;
			this.tvwStoredProc.HideSelection = false;
			this.tvwStoredProc.ImageIndex = 0;
			this.tvwStoredProc.ImageList = this.imageList1;
			this.tvwStoredProc.Location = new System.Drawing.Point(3, 3);
			this.tvwStoredProc.Name = "tvwStoredProc";
			this.tvwStoredProc.SelectedImageIndex = 0;
			this.tvwStoredProc.Size = new System.Drawing.Size(621, 340);
			this.tvwStoredProc.TabIndex = 4;
			// 
			// pageFunction
			// 
			this.pageFunction.Controls.Add(this.tvwFunction);
			this.pageFunction.Location = new System.Drawing.Point(4, 22);
			this.pageFunction.Name = "pageFunction";
			this.pageFunction.Padding = new System.Windows.Forms.Padding(3);
			this.pageFunction.Size = new System.Drawing.Size(627, 346);
			this.pageFunction.TabIndex = 3;
			this.pageFunction.Text = "Functions";
			this.pageFunction.UseVisualStyleBackColor = true;
			// 
			// tvwFunction
			// 
			this.tvwFunction.CheckBoxes = true;
			this.tvwFunction.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwFunction.Enabled = false;
			this.tvwFunction.HideSelection = false;
			this.tvwFunction.ImageIndex = 0;
			this.tvwFunction.ImageList = this.imageList1;
			this.tvwFunction.Location = new System.Drawing.Point(3, 3);
			this.tvwFunction.Name = "tvwFunction";
			this.tvwFunction.SelectedImageIndex = 0;
			this.tvwFunction.Size = new System.Drawing.Size(621, 340);
			this.tvwFunction.TabIndex = 4;
			// 
			// ModuleMappings
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(635, 529);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(552, 380);
			this.Name = "ModuleMappings";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Module Associations";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel2.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.pageEntity.ResumeLayout(false);
			this.pageView.ResumeLayout(false);
			this.pageStoredProc.ResumeLayout(false);
			this.pageFunction.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ComboBox cboModule;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button cmdUncheck;
		private System.Windows.Forms.Button cmdCheck;
		private System.Windows.Forms.Button cmdCollapse;
		private System.Windows.Forms.Button cmdExpand;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblLine100;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage pageEntity;
		private System.Windows.Forms.TreeView tvwEntity;
		private System.Windows.Forms.TabPage pageView;
		private System.Windows.Forms.TreeView tvwView;
		private System.Windows.Forms.TabPage pageStoredProc;
		private System.Windows.Forms.TreeView tvwStoredProc;
		private System.Windows.Forms.TabPage pageFunction;
		private System.Windows.Forms.TreeView tvwFunction;
	}
}