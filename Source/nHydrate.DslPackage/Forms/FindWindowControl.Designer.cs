namespace nHydrate.DslPackage.Forms
{
	partial class FindWindowControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindWindowControl));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlType = new System.Windows.Forms.Panel();
            this.txtSearch = new nHydrate.Generator.Common.Forms.CueTextBox();
            this.cmdSettings = new System.Windows.Forms.Button();
            this.lvwMain = new System.Windows.Forms.ListView();
            this.contextMenuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemMainSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMainDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMainSetupColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMainSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemMainRelationships = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMainRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMainShowRelatedEntities = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMainStaticData = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMainViewIndexes = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.lvwSubItem = new System.Windows.Forms.ListView();
            this.contextMenuSub = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemSubSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSubDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSubSetupColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlType.SuspendLayout();
            this.contextMenuMain.SuspendLayout();
            this.contextMenuSub.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Entity.png");
            this.imageList1.Images.SetKeyName(1, "view.png");
            this.imageList1.Images.SetKeyName(2, "storedproc.png");
            this.imageList1.Images.SetKeyName(3, "function.png");
            this.imageList1.Images.SetKeyName(4, "field.png");
            this.imageList1.Images.SetKeyName(5, "parameter.png");
            // 
            // pnlType
            // 
            this.pnlType.Controls.Add(this.txtSearch);
            this.pnlType.Controls.Add(this.cmdSettings);
            this.pnlType.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlType.Location = new System.Drawing.Point(0, 0);
            this.pnlType.Name = "pnlType";
            this.pnlType.Size = new System.Drawing.Size(393, 20);
            this.pnlType.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Cue = "<Search Object View>";
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(367, 20);
            this.txtSearch.TabIndex = 0;
            // 
            // cmdSettings
            // 
            this.cmdSettings.BackColor = System.Drawing.SystemColors.Window;
            this.cmdSettings.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmdSettings.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmdSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdSettings.Image = ((System.Drawing.Image)(resources.GetObject("cmdSettings.Image")));
            this.cmdSettings.Location = new System.Drawing.Point(367, 0);
            this.cmdSettings.Name = "cmdSettings";
            this.cmdSettings.Size = new System.Drawing.Size(26, 20);
            this.cmdSettings.TabIndex = 1;
            this.cmdSettings.UseVisualStyleBackColor = false;
            // 
            // lvwMain
            // 
            this.lvwMain.ContextMenuStrip = this.contextMenuMain;
            this.lvwMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.lvwMain.FullRowSelect = true;
            this.lvwMain.HideSelection = false;
            this.lvwMain.LabelEdit = true;
            this.lvwMain.Location = new System.Drawing.Point(0, 20);
            this.lvwMain.Name = "lvwMain";
            this.lvwMain.Size = new System.Drawing.Size(393, 100);
            this.lvwMain.SmallImageList = this.imageList1;
            this.lvwMain.TabIndex = 2;
            this.lvwMain.UseCompatibleStateImageBehavior = false;
            this.lvwMain.View = System.Windows.Forms.View.Details;
            // 
            // contextMenuMain
            // 
            this.contextMenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemMainSelect,
            this.menuItemMainDelete,
            this.menuItemMainSetupColumns,
            this.menuItemMainSep1,
            this.menuItemMainRelationships,
            this.menuItemMainRefresh,
            this.menuItemMainShowRelatedEntities,
            this.menuItemMainStaticData,
            this.menuItemMainViewIndexes});
            this.contextMenuMain.Name = "contextMenuMain";
            this.contextMenuMain.Size = new System.Drawing.Size(203, 208);
            // 
            // menuItemMainSelect
            // 
            this.menuItemMainSelect.Name = "menuItemMainSelect";
            this.menuItemMainSelect.Size = new System.Drawing.Size(202, 22);
            this.menuItemMainSelect.Text = "Select";
            // 
            // menuItemMainDelete
            // 
            this.menuItemMainDelete.Name = "menuItemMainDelete";
            this.menuItemMainDelete.Size = new System.Drawing.Size(202, 22);
            this.menuItemMainDelete.Text = "Delete";
            // 
            // menuItemMainSetupColumns
            // 
            this.menuItemMainSetupColumns.Name = "menuItemMainSetupColumns";
            this.menuItemMainSetupColumns.Size = new System.Drawing.Size(202, 22);
            this.menuItemMainSetupColumns.Text = "Setup Columns...";
            // 
            // menuItemMainSep1
            // 
            this.menuItemMainSep1.Name = "menuItemMainSep1";
            this.menuItemMainSep1.Size = new System.Drawing.Size(199, 6);
            // 
            // menuItemMainRelationships
            // 
            this.menuItemMainRelationships.Name = "menuItemMainRelationships";
            this.menuItemMainRelationships.Size = new System.Drawing.Size(202, 22);
            this.menuItemMainRelationships.Text = "Relationships...";
            // 
            // menuItemMainRefresh
            // 
            this.menuItemMainRefresh.Name = "menuItemMainRefresh";
            this.menuItemMainRefresh.Size = new System.Drawing.Size(202, 22);
            this.menuItemMainRefresh.Text = "Refresh from Database...";
            // 
            // menuItemMainShowRelatedEntities
            // 
            this.menuItemMainShowRelatedEntities.Name = "menuItemMainShowRelatedEntities";
            this.menuItemMainShowRelatedEntities.Size = new System.Drawing.Size(202, 22);
            this.menuItemMainShowRelatedEntities.Text = "Show Related Entities...";
            // 
            // menuItemMainStaticData
            // 
            this.menuItemMainStaticData.Name = "menuItemMainStaticData";
            this.menuItemMainStaticData.Size = new System.Drawing.Size(202, 22);
            this.menuItemMainStaticData.Text = "Static Data...";
            // 
            // menuItemMainViewIndexes
            // 
            this.menuItemMainViewIndexes.Name = "menuItemMainViewIndexes";
            this.menuItemMainViewIndexes.Size = new System.Drawing.Size(202, 22);
            this.menuItemMainViewIndexes.Text = "View Indexes...";
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.Desktop;
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 120);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(393, 4);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // lvwSubItem
            // 
            this.lvwSubItem.ContextMenuStrip = this.contextMenuSub;
            this.lvwSubItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwSubItem.FullRowSelect = true;
            this.lvwSubItem.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwSubItem.HideSelection = false;
            this.lvwSubItem.LabelEdit = true;
            this.lvwSubItem.Location = new System.Drawing.Point(0, 124);
            this.lvwSubItem.Name = "lvwSubItem";
            this.lvwSubItem.Size = new System.Drawing.Size(393, 99);
            this.lvwSubItem.SmallImageList = this.imageList1;
            this.lvwSubItem.TabIndex = 4;
            this.lvwSubItem.UseCompatibleStateImageBehavior = false;
            this.lvwSubItem.View = System.Windows.Forms.View.Details;
            // 
            // contextMenuSub
            // 
            this.contextMenuSub.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSubSelect,
            this.menuItemSubDelete,
            this.menuItemSubSetupColumns});
            this.contextMenuSub.Name = "contextMenuSub";
            this.contextMenuSub.Size = new System.Drawing.Size(165, 70);
            // 
            // menuItemSubSelect
            // 
            this.menuItemSubSelect.Name = "menuItemSubSelect";
            this.menuItemSubSelect.Size = new System.Drawing.Size(164, 22);
            this.menuItemSubSelect.Text = "Select";
            // 
            // menuItemSubDelete
            // 
            this.menuItemSubDelete.Name = "menuItemSubDelete";
            this.menuItemSubDelete.Size = new System.Drawing.Size(164, 22);
            this.menuItemSubDelete.Text = "Delete";
            // 
            // menuItemSubSetupColumns
            // 
            this.menuItemSubSetupColumns.Name = "menuItemSubSetupColumns";
            this.menuItemSubSetupColumns.Size = new System.Drawing.Size(164, 22);
            this.menuItemSubSetupColumns.Text = "Setup Columns...";
            // 
            // FindWindowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvwSubItem);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.lvwMain);
            this.Controls.Add(this.pnlType);
            this.Name = "FindWindowControl";
            this.Size = new System.Drawing.Size(393, 223);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FindWindowControl_KeyUp);
            this.pnlType.ResumeLayout(false);
            this.pnlType.PerformLayout();
            this.contextMenuMain.ResumeLayout(false);
            this.contextMenuSub.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Panel pnlType;
		private nHydrate.Generator.Common.Forms.CueTextBox txtSearch;
		private System.Windows.Forms.ListView lvwMain;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ListView lvwSubItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuMain;
		private System.Windows.Forms.ContextMenuStrip contextMenuSub;
		private System.Windows.Forms.ToolStripMenuItem menuItemMainSelect;
		private System.Windows.Forms.ToolStripMenuItem menuItemMainRefresh;
		private System.Windows.Forms.ToolStripMenuItem menuItemMainDelete;
		private System.Windows.Forms.ToolStripMenuItem menuItemSubSelect;
		private System.Windows.Forms.ToolStripMenuItem menuItemSubDelete;
		private System.Windows.Forms.ToolStripSeparator menuItemMainSep1;
		private System.Windows.Forms.ToolStripMenuItem menuItemMainRelationships;
		private System.Windows.Forms.ToolStripMenuItem menuItemMainShowRelatedEntities;
		private System.Windows.Forms.ToolStripMenuItem menuItemMainStaticData;
		private System.Windows.Forms.ToolStripMenuItem menuItemMainViewIndexes;
		private System.Windows.Forms.ToolStripMenuItem menuItemMainSetupColumns;
		private System.Windows.Forms.ToolStripMenuItem menuItemSubSetupColumns;
		private System.Windows.Forms.Button cmdSettings;


	}
}
