namespace nHydrate.DslPackage.Forms
{
	partial class PrecedenseOrderForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrecedenseOrderForm));
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdImport = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.cmdMoveUp = new System.Windows.Forms.Button();
			this.cmdMoveDown = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblLine100 = new System.Windows.Forms.Label();
			this.lblHeader = new System.Windows.Forms.Label();
			this.lvwItem = new System.Windows.Forms.ListView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.txtFilter = new nHydrate.Generator.Common.Forms.CueTextBox();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.Location = new System.Drawing.Point(437, 477);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 4;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdImport
			// 
			this.cmdImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdImport.Location = new System.Drawing.Point(356, 477);
			this.cmdImport.Name = "cmdImport";
			this.cmdImport.Size = new System.Drawing.Size(75, 23);
			this.cmdImport.TabIndex = 3;
			this.cmdImport.Text = "Import";
			this.cmdImport.UseVisualStyleBackColor = true;
			this.cmdImport.Click += new System.EventHandler(this.cmdImport_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.Location = new System.Drawing.Point(518, 477);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 5;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(12, 436);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(338, 67);
			this.label2.TabIndex = 3;
			this.label2.Text = resources.GetString("label2.Text");
			// 
			// cmdMoveUp
			// 
			this.cmdMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdMoveUp.Location = new System.Drawing.Point(437, 426);
			this.cmdMoveUp.Name = "cmdMoveUp";
			this.cmdMoveUp.Size = new System.Drawing.Size(75, 23);
			this.cmdMoveUp.TabIndex = 1;
			this.cmdMoveUp.Text = "Move Up";
			this.cmdMoveUp.UseVisualStyleBackColor = true;
			this.cmdMoveUp.Click += new System.EventHandler(this.cmdMoveUp_Click);
			// 
			// cmdMoveDown
			// 
			this.cmdMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdMoveDown.Location = new System.Drawing.Point(518, 426);
			this.cmdMoveDown.Name = "cmdMoveDown";
			this.cmdMoveDown.Size = new System.Drawing.Size(75, 23);
			this.cmdMoveDown.TabIndex = 2;
			this.cmdMoveDown.Text = "Move Down";
			this.cmdMoveDown.UseVisualStyleBackColor = true;
			this.cmdMoveDown.Click += new System.EventHandler(this.cmdMoveDown_Click);
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Window;
			this.panel2.Controls.Add(this.pictureBox1);
			this.panel2.Controls.Add(this.lblLine100);
			this.panel2.Controls.Add(this.lblHeader);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(605, 65);
			this.panel2.TabIndex = 88;
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
			this.lblLine100.Size = new System.Drawing.Size(605, 2);
			this.lblLine100.TabIndex = 71;
			// 
			// lblHeader
			// 
			this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHeader.Location = new System.Drawing.Point(72, 9);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(521, 47);
			this.lblHeader.TabIndex = 68;
			this.lblHeader.Text = resources.GetString("lblHeader.Text");
			// 
			// lvwItem
			// 
			this.lvwItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvwItem.FullRowSelect = true;
			this.lvwItem.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvwItem.HideSelection = false;
			this.lvwItem.LabelWrap = false;
			this.lvwItem.Location = new System.Drawing.Point(12, 97);
			this.lvwItem.MultiSelect = false;
			this.lvwItem.Name = "lvwItem";
			this.lvwItem.ShowGroups = false;
			this.lvwItem.ShowItemToolTips = true;
			this.lvwItem.Size = new System.Drawing.Size(581, 323);
			this.lvwItem.SmallImageList = this.imageList1;
			this.lvwItem.TabIndex = 0;
			this.lvwItem.UseCompatibleStateImageBehavior = false;
			this.lvwItem.View = System.Windows.Forms.View.Details;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "view.png");
			this.imageList1.Images.SetKeyName(1, "storedproc.png");
			this.imageList1.Images.SetKeyName(2, "function.png");
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 71);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 13);
			this.label1.TabIndex = 95;
			this.label1.Text = "Filter:";
			// 
			// txtFilter
			// 
			this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilter.Cue = "<Search by Text Filter>";
			this.txtFilter.Location = new System.Drawing.Point(50, 71);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(543, 20);
			this.txtFilter.TabIndex = 6;
			// 
			// PrecedenseOrderForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(605, 512);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtFilter);
			this.Controls.Add(this.lvwItem);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.cmdMoveDown);
			this.Controls.Add(this.cmdMoveUp);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmdImport);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(524, 431);
			this.Name = "PrecedenseOrderForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Order of Precedense";
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdImport;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cmdMoveUp;
		private System.Windows.Forms.Button cmdMoveDown;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label lblLine100;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.ListView lvwItem;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Label label1;
		private Generator.Common.Forms.CueTextBox txtFilter;
	}
}