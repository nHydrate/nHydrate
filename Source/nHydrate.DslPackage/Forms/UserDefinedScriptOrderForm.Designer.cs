namespace nHydrate.DslPackage.Forms
{
    partial class UserDefinedScriptOrderForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserDefinedScriptOrderForm));
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.lvwItem = new System.Windows.Forms.ListView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblLine100 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.cboInstaller = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.Location = new System.Drawing.Point(466, 478);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 3;
			this.cmdCancel.Text = "Close";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.Location = new System.Drawing.Point(305, 478);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(155, 23);
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "Add Precedence Order";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// lvwItem
			// 
			this.lvwItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvwItem.CheckBoxes = true;
			this.lvwItem.FullRowSelect = true;
			this.lvwItem.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvwItem.HideSelection = false;
			this.lvwItem.LabelWrap = false;
			this.lvwItem.Location = new System.Drawing.Point(15, 214);
			this.lvwItem.MultiSelect = false;
			this.lvwItem.Name = "lvwItem";
			this.lvwItem.ShowGroups = false;
			this.lvwItem.ShowItemToolTips = true;
			this.lvwItem.Size = new System.Drawing.Size(526, 258);
			this.lvwItem.SmallImageList = this.imageList1;
			this.lvwItem.TabIndex = 1;
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
			// panel1
			// 
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.cboInstaller);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(553, 197);
			this.panel1.TabIndex = 94;
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
			this.panel3.Size = new System.Drawing.Size(553, 154);
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
			this.lblLine100.Location = new System.Drawing.Point(0, 152);
			this.lblLine100.Name = "lblLine100";
			this.lblLine100.Size = new System.Drawing.Size(553, 2);
			this.lblLine100.TabIndex = 71;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(91, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(450, 133);
			this.label6.TabIndex = 68;
			this.label6.Text = resources.GetString("label6.Text");
			// 
			// cboInstaller
			// 
			this.cboInstaller.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboInstaller.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboInstaller.FormattingEnabled = true;
			this.cboInstaller.Location = new System.Drawing.Point(110, 163);
			this.cboInstaller.Name = "cboInstaller";
			this.cboInstaller.Size = new System.Drawing.Size(431, 21);
			this.cboInstaller.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 163);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Installer Project:";
			// 
			// UserDefinedScriptOrderForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(553, 513);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lvwItem);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 400);
			this.Name = "UserDefinedScriptOrderForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "User-Defined Scripts";
			this.ResizeEnd += new System.EventHandler(this.UserDefinedScriptOrderForm_ResizeEnd);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
				private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.ListView lvwItem;
				private System.Windows.Forms.ImageList imageList1;
				private System.Windows.Forms.Panel panel1;
				private System.Windows.Forms.Panel panel3;
				private System.Windows.Forms.PictureBox pictureBox1;
				private System.Windows.Forms.Label lblLine100;
				private System.Windows.Forms.Label label6;
				private System.Windows.Forms.ComboBox cboInstaller;
				private System.Windows.Forms.Label label1;
    }
}