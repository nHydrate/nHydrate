namespace nHydrate.Generator.ModelUI
{
  partial class StatisticsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsForm));
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdDetails = new System.Windows.Forms.Button();
			this.txtStats = new System.Windows.Forms.TextBox();
			this.lblHeader = new System.Windows.Forms.Label();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.cmdUncheckAll = new System.Windows.Forms.Button();
			this.cmdCheckAll = new System.Windows.Forms.Button();
			this.lstFile = new System.Windows.Forms.CheckedListBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdOK.Location = new System.Drawing.Point(399, 218);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdDetails
			// 
			this.cmdDetails.Location = new System.Drawing.Point(7, 218);
			this.cmdDetails.Name = "cmdDetails";
			this.cmdDetails.Size = new System.Drawing.Size(75, 23);
			this.cmdDetails.TabIndex = 1;
			this.cmdDetails.Text = "Details >>";
			this.cmdDetails.UseVisualStyleBackColor = true;
			this.cmdDetails.Click += new System.EventHandler(this.cmdDetails_Click);
			// 
			// txtStats
			// 
			this.txtStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtStats.Location = new System.Drawing.Point(7, 98);
			this.txtStats.Multiline = true;
			this.txtStats.Name = "txtStats";
			this.txtStats.ReadOnly = true;
			this.txtStats.Size = new System.Drawing.Size(472, 112);
			this.txtStats.TabIndex = 0;
			// 
			// lblHeader
			// 
			this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblHeader.Location = new System.Drawing.Point(0, 0);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(472, 48);
			this.lblHeader.TabIndex = 4;
			// 
			// cmdDelete
			// 
			this.cmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdDelete.Location = new System.Drawing.Point(392, 252);
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size(75, 23);
			this.cmdDelete.TabIndex = 4;
			this.cmdDelete.Text = "Delete";
			this.cmdDelete.UseVisualStyleBackColor = true;
			this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.cmdUncheckAll);
			this.panel1.Controls.Add(this.cmdCheckAll);
			this.panel1.Controls.Add(this.lstFile);
			this.panel1.Controls.Add(this.lblHeader);
			this.panel1.Controls.Add(this.cmdDelete);
			this.panel1.Location = new System.Drawing.Point(8, 247);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(472, 284);
			this.panel1.TabIndex = 6;
			// 
			// cmdUncheckAll
			// 
			this.cmdUncheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdUncheckAll.Location = new System.Drawing.Point(80, 252);
			this.cmdUncheckAll.Name = "cmdUncheckAll";
			this.cmdUncheckAll.Size = new System.Drawing.Size(75, 23);
			this.cmdUncheckAll.TabIndex = 6;
			this.cmdUncheckAll.Text = "Uncheck All";
			this.cmdUncheckAll.UseVisualStyleBackColor = true;
			this.cmdUncheckAll.Click += new System.EventHandler(this.cmdUncheckAll_Click);
			// 
			// cmdCheckAll
			// 
			this.cmdCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdCheckAll.Location = new System.Drawing.Point(0, 252);
			this.cmdCheckAll.Name = "cmdCheckAll";
			this.cmdCheckAll.Size = new System.Drawing.Size(75, 23);
			this.cmdCheckAll.TabIndex = 5;
			this.cmdCheckAll.Text = "Check All";
			this.cmdCheckAll.UseVisualStyleBackColor = true;
			this.cmdCheckAll.Click += new System.EventHandler(this.cmdCheckAll_Click);
			// 
			// lstFile
			// 
			this.lstFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstFile.CheckOnClick = true;
			this.lstFile.FormattingEnabled = true;
			this.lstFile.IntegralHeight = false;
			this.lstFile.Location = new System.Drawing.Point(0, 51);
			this.lstFile.Name = "lstFile";
			this.lstFile.Size = new System.Drawing.Size(472, 194);
			this.lstFile.TabIndex = 3;
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
			this.pictureBox3.Location = new System.Drawing.Point(12, 12);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(249, 75);
			this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox3.TabIndex = 24;
			this.pictureBox3.TabStop = false;
			// 
			// StatisticsForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.CancelButton = this.cmdOK;
			this.ClientSize = new System.Drawing.Size(486, 532);
			this.Controls.Add(this.pictureBox3);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.txtStats);
			this.Controls.Add(this.cmdDetails);
			this.Controls.Add(this.cmdOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(492, 285);
			this.Name = "StatisticsForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Generation Complete";
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button cmdOK;
    private System.Windows.Forms.Button cmdDetails;
    private System.Windows.Forms.TextBox txtStats;
    private System.Windows.Forms.Label lblHeader;
    private System.Windows.Forms.Button cmdDelete;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.CheckedListBox lstFile;
    private System.Windows.Forms.Button cmdUncheckAll;
    private System.Windows.Forms.Button cmdCheckAll;
		private System.Windows.Forms.PictureBox pictureBox3;
  }
}