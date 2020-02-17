namespace nHydrate.DslPackage.Forms
{
	partial class DBObjectDifferenceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBObjectDifferenceForm));
            this.pnlFields = new System.Windows.Forms.Panel();
            this.lblFieldInfo = new System.Windows.Forms.Label();
            this.lstFields = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitterField = new System.Windows.Forms.Splitter();
            this.splitterParameter = new System.Windows.Forms.Splitter();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.txtText2 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.splitterVert = new System.Windows.Forms.Splitter();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.txtText1 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.pnlFields.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFields
            // 
            this.pnlFields.Controls.Add(this.lblFieldInfo);
            this.pnlFields.Controls.Add(this.lstFields);
            this.pnlFields.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFields.Location = new System.Drawing.Point(0, 444);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.Size = new System.Drawing.Size(685, 200);
            this.pnlFields.TabIndex = 4;
            // 
            // lblFieldInfo
            // 
            this.lblFieldInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFieldInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblFieldInfo.Location = new System.Drawing.Point(0, 177);
            this.lblFieldInfo.Name = "lblFieldInfo";
            this.lblFieldInfo.Size = new System.Drawing.Size(685, 23);
            this.lblFieldInfo.TabIndex = 2;
            this.lblFieldInfo.Text = "[INFO]";
            this.lblFieldInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFieldInfo.Visible = false;
            // 
            // lstFields
            // 
            this.lstFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFields.FullRowSelect = true;
            this.lstFields.HideSelection = false;
            this.lstFields.Location = new System.Drawing.Point(0, 0);
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(685, 200);
            this.lstFields.SmallImageList = this.imageList1;
            this.lstFields.TabIndex = 1;
            this.lstFields.UseCompatibleStateImageBehavior = false;
            this.lstFields.View = System.Windows.Forms.View.Details;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "add.jpg");
            this.imageList1.Images.SetKeyName(1, "delete.ico");
            this.imageList1.Images.SetKeyName(2, "notequal.jpg");
            // 
            // splitterField
            // 
            this.splitterField.BackColor = System.Drawing.SystemColors.Control;
            this.splitterField.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterField.Location = new System.Drawing.Point(0, 438);
            this.splitterField.Name = "splitterField";
            this.splitterField.Size = new System.Drawing.Size(685, 6);
            this.splitterField.TabIndex = 5;
            this.splitterField.TabStop = false;
            // 
            // splitterParameter
            // 
            this.splitterParameter.BackColor = System.Drawing.SystemColors.Control;
            this.splitterParameter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterParameter.Location = new System.Drawing.Point(0, 432);
            this.splitterParameter.Name = "splitterParameter";
            this.splitterParameter.Size = new System.Drawing.Size(685, 6);
            this.splitterParameter.TabIndex = 8;
            this.splitterParameter.TabStop = false;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlRight);
            this.pnlMain.Controls.Add(this.splitterVert);
            this.pnlMain.Controls.Add(this.pnlLeft);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(685, 432);
            this.pnlMain.TabIndex = 9;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.SystemColors.Control;
            this.pnlRight.Controls.Add(this.txtText2);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(206, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(479, 432);
            this.pnlRight.TabIndex = 2;
            // 
            // txtText2
            // 
            this.txtText2.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.txtText2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtText2.CommentPrefix = "--";
            this.txtText2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtText2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtText2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtText2.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtText2.LeftBracket = '(';
            this.txtText2.Location = new System.Drawing.Point(0, 0);
            this.txtText2.Name = "txtText2";
            this.txtText2.RightBracket = ')';
            this.txtText2.Size = new System.Drawing.Size(479, 432);
            this.txtText2.TabIndex = 1;
            // 
            // splitterVert
            // 
            this.splitterVert.BackColor = System.Drawing.SystemColors.Control;
            this.splitterVert.Location = new System.Drawing.Point(200, 0);
            this.splitterVert.Name = "splitterVert";
            this.splitterVert.Size = new System.Drawing.Size(6, 432);
            this.splitterVert.TabIndex = 1;
            this.splitterVert.TabStop = false;
            this.splitterVert.DoubleClick += new System.EventHandler(this.splitterVert_DoubleClick);
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.SystemColors.Control;
            this.pnlLeft.Controls.Add(this.txtText1);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(200, 432);
            this.pnlLeft.TabIndex = 0;
            // 
            // txtText1
            // 
            this.txtText1.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.txtText1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtText1.CommentPrefix = "--";
            this.txtText1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtText1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtText1.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtText1.LeftBracket = '(';
            this.txtText1.Location = new System.Drawing.Point(0, 0);
            this.txtText1.Name = "txtText1";
            this.txtText1.RightBracket = ')';
            this.txtText1.Size = new System.Drawing.Size(200, 432);
            this.txtText1.TabIndex = 1;
            // 
            // DBObjectDifferenceForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(685, 644);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.splitterParameter);
            this.Controls.Add(this.splitterField);
            this.Controls.Add(this.pnlFields);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(516, 379);
            this.Name = "DBObjectDifferenceForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Object Differences";
            this.pnlFields.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlFields;
		private System.Windows.Forms.ListView lstFields;
		private System.Windows.Forms.Splitter splitterField;
		private System.Windows.Forms.Splitter splitterParameter;
		private System.Windows.Forms.Panel pnlMain;
		private System.Windows.Forms.Panel pnlRight;
		private FastColoredTextBoxNS.FastColoredTextBox txtText2;
		private System.Windows.Forms.Splitter splitterVert;
		private System.Windows.Forms.Panel pnlLeft;
		private FastColoredTextBoxNS.FastColoredTextBox txtText1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Label lblFieldInfo;
	}
}