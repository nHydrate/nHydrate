namespace nHydrate.Generator.Common.Forms
{
	partial class ModelTree
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelTree));
			this.mMenuItemNew = new System.Windows.Forms.MenuItem();
			this.mMenuItemOpen = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mMenuItemFile = new System.Windows.Forms.MenuItem();
			this.mMenuItemSave = new System.Windows.Forms.MenuItem();
			this.mMenuItemClose = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mMenuItemExit = new System.Windows.Forms.MenuItem();
			this.mControlPanel = new System.Windows.Forms.Panel();
			this.pnlChild = new System.Windows.Forms.Panel();
			this.pnlContentUser = new System.Windows.Forms.Panel();
			this.lblNoDesignerMessage = new System.Windows.Forms.Label();
			this.pnlContentHeader = new System.Windows.Forms.Panel();
			this.imgContentHeader = new System.Windows.Forms.PictureBox();
			this.lblContentHeaderText2 = new System.Windows.Forms.Label();
			this.lblContentHeaderText = new System.Windows.Forms.Label();
			this.mHorizontalSplitter = new System.Windows.Forms.Splitter();
			this.tvwModel = new System.Windows.Forms.TreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.lvwError = new nHydrate.Generator.Common.Forms.ErrorControl();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.lvwOutput = new System.Windows.Forms.ListView();
			this.menuTools = new System.Windows.Forms.MenuItem();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.mControlPanel.SuspendLayout();
			this.pnlChild.SuspendLayout();
			this.pnlContentUser.SuspendLayout();
			this.pnlContentHeader.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgContentHeader)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// mMenuItemNew
			// 
			this.mMenuItemNew.Index = 0;
			this.mMenuItemNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.mMenuItemNew.Text = "&New";
			// 
			// mMenuItemOpen
			// 
			this.mMenuItemOpen.Index = 1;
			this.mMenuItemOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mMenuItemOpen.Text = "&Open";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 3;
			this.menuItem2.Text = "-";
			// 
			// mMenuItemFile
			// 
			this.mMenuItemFile.Index = -1;
			this.mMenuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mMenuItemNew,
            this.mMenuItemOpen,
            this.mMenuItemSave,
            this.menuItem2,
            this.mMenuItemClose,
            this.menuItem1,
            this.mMenuItemExit});
			this.mMenuItemFile.Text = "&File";
			// 
			// mMenuItemSave
			// 
			this.mMenuItemSave.Index = 2;
			this.mMenuItemSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.mMenuItemSave.Text = "&Save";
			// 
			// mMenuItemClose
			// 
			this.mMenuItemClose.Index = 4;
			this.mMenuItemClose.Text = "&Close";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 5;
			this.menuItem1.Text = "-";
			// 
			// mMenuItemExit
			// 
			this.mMenuItemExit.Index = 6;
			this.mMenuItemExit.Text = "E&xit";
			// 
			// mControlPanel
			// 
			this.mControlPanel.Controls.Add(this.pnlChild);
			this.mControlPanel.Controls.Add(this.mHorizontalSplitter);
			this.mControlPanel.Controls.Add(this.tvwModel);
			this.mControlPanel.Controls.Add(this.splitter1);
			this.mControlPanel.Controls.Add(this.tabControl1);
			this.mControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mControlPanel.Location = new System.Drawing.Point(0, 0);
			this.mControlPanel.Name = "mControlPanel";
			this.mControlPanel.Size = new System.Drawing.Size(730, 428);
			this.mControlPanel.TabIndex = 7;
			// 
			// pnlChild
			// 
			this.pnlChild.Controls.Add(this.pnlContentUser);
			this.pnlChild.Controls.Add(this.pnlContentHeader);
			this.pnlChild.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlChild.Location = new System.Drawing.Point(306, 0);
			this.pnlChild.Name = "pnlChild";
			this.pnlChild.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.pnlChild.Size = new System.Drawing.Size(424, 285);
			this.pnlChild.TabIndex = 6;
			// 
			// pnlContentUser
			// 
			this.pnlContentUser.Controls.Add(this.lblNoDesignerMessage);
			this.pnlContentUser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlContentUser.Location = new System.Drawing.Point(0, 65);
			this.pnlContentUser.Name = "pnlContentUser";
			this.pnlContentUser.Size = new System.Drawing.Size(419, 220);
			this.pnlContentUser.TabIndex = 1;
			// 
			// lblNoDesignerMessage
			// 
			this.lblNoDesignerMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblNoDesignerMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblNoDesignerMessage.Location = new System.Drawing.Point(0, 0);
			this.lblNoDesignerMessage.Name = "lblNoDesignerMessage";
			this.lblNoDesignerMessage.Size = new System.Drawing.Size(419, 220);
			this.lblNoDesignerMessage.TabIndex = 2;
			this.lblNoDesignerMessage.Text = "There are no designers visible at this time.";
			this.lblNoDesignerMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlContentHeader
			// 
			this.pnlContentHeader.BackColor = System.Drawing.SystemColors.Window;
			this.pnlContentHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlContentHeader.Controls.Add(this.imgContentHeader);
			this.pnlContentHeader.Controls.Add(this.lblContentHeaderText2);
			this.pnlContentHeader.Controls.Add(this.lblContentHeaderText);
			this.pnlContentHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlContentHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlContentHeader.Name = "pnlContentHeader";
			this.pnlContentHeader.Size = new System.Drawing.Size(419, 65);
			this.pnlContentHeader.TabIndex = 0;
			// 
			// imgContentHeader
			// 
			this.imgContentHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.imgContentHeader.Image = global::nHydrate.Generator.Properties.Resources.components;
			this.imgContentHeader.Location = new System.Drawing.Point(362, 4);
			this.imgContentHeader.Name = "imgContentHeader";
			this.imgContentHeader.Size = new System.Drawing.Size(48, 48);
			this.imgContentHeader.TabIndex = 1;
			this.imgContentHeader.TabStop = false;
			// 
			// lblContentHeaderText2
			// 
			this.lblContentHeaderText2.Location = new System.Drawing.Point(18, 27);
			this.lblContentHeaderText2.Name = "lblContentHeaderText2";
			this.lblContentHeaderText2.Size = new System.Drawing.Size(345, 25);
			this.lblContentHeaderText2.TabIndex = 2;
			this.lblContentHeaderText2.Text = "Some Text";
			// 
			// lblContentHeaderText
			// 
			this.lblContentHeaderText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lblContentHeaderText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblContentHeaderText.Location = new System.Drawing.Point(7, 4);
			this.lblContentHeaderText.Name = "lblContentHeaderText";
			this.lblContentHeaderText.Size = new System.Drawing.Size(349, 23);
			this.lblContentHeaderText.TabIndex = 0;
			this.lblContentHeaderText.Text = "Header";
			// 
			// mHorizontalSplitter
			// 
			this.mHorizontalSplitter.Location = new System.Drawing.Point(303, 0);
			this.mHorizontalSplitter.Name = "mHorizontalSplitter";
			this.mHorizontalSplitter.Size = new System.Drawing.Size(3, 285);
			this.mHorizontalSplitter.TabIndex = 5;
			this.mHorizontalSplitter.TabStop = false;
			// 
			// tvwModel
			// 
			this.tvwModel.Dock = System.Windows.Forms.DockStyle.Left;
			this.tvwModel.HideSelection = false;
			this.tvwModel.Location = new System.Drawing.Point(0, 0);
			this.tvwModel.Name = "tvwModel";
			this.tvwModel.Size = new System.Drawing.Size(303, 285);
			this.tvwModel.TabIndex = 4;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 285);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(730, 3);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tabControl1.Location = new System.Drawing.Point(0, 288);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(730, 140);
			this.tabControl1.TabIndex = 7;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.lvwError);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(722, 114);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Messages";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// lvwError
			// 
			this.lvwError.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvwError.FullRowSelect = true;
			this.lvwError.HideSelection = false;
			this.lvwError.Location = new System.Drawing.Point(3, 3);
			this.lvwError.Name = "lvwError";
			this.lvwError.SelectedMessage = null;
			this.lvwError.Size = new System.Drawing.Size(716, 108);
			this.lvwError.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvwError.TabIndex = 1;
			this.lvwError.UseCompatibleStateImageBehavior = false;
			this.lvwError.View = System.Windows.Forms.View.Details;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.lvwOutput);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(722, 114);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Output";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// lvwOutput
			// 
			this.lvwOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvwOutput.Location = new System.Drawing.Point(3, 3);
			this.lvwOutput.Name = "lvwOutput";
			this.lvwOutput.Size = new System.Drawing.Size(561, 108);
			this.lvwOutput.TabIndex = 0;
			this.lvwOutput.UseCompatibleStateImageBehavior = false;
			this.lvwOutput.View = System.Windows.Forms.View.Details;
			// 
			// menuTools
			// 
			this.menuTools.Enabled = false;
			this.menuTools.Index = -1;
			this.menuTools.Text = "&Tools";
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "warning.ico");
			this.imageList1.Images.SetKeyName(1, "error.ico");
			// 
			// ModelTree
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mControlPanel);
			this.Name = "ModelTree";
			this.Size = new System.Drawing.Size(730, 428);
			this.mControlPanel.ResumeLayout(false);
			this.pnlChild.ResumeLayout(false);
			this.pnlContentUser.ResumeLayout(false);
			this.pnlContentHeader.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.imgContentHeader)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.MenuItem mMenuItemNew;
		private System.Windows.Forms.MenuItem mMenuItemOpen;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mMenuItemFile;
		private System.Windows.Forms.MenuItem mMenuItemSave;
		private System.Windows.Forms.MenuItem mMenuItemClose;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mMenuItemExit;
		private System.Windows.Forms.Panel mControlPanel;
		private System.Windows.Forms.Splitter mHorizontalSplitter;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.MenuItem menuTools;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		protected System.Windows.Forms.ListView lvwOutput;
		protected System.Windows.Forms.Panel pnlChild;
		protected System.Windows.Forms.TreeView tvwModel;
		protected System.Windows.Forms.TabControl tabControl1;
		protected ErrorControl lvwError;
		private System.Windows.Forms.Panel pnlContentHeader;
		private System.Windows.Forms.PictureBox imgContentHeader;
		private System.Windows.Forms.Label lblContentHeaderText;
		private System.Windows.Forms.Label lblContentHeaderText2;
		private System.Windows.Forms.Panel pnlContentUser;
		private System.Windows.Forms.Label lblNoDesignerMessage;
	}
}
