namespace nHydrate.Dsl.Design.Forms
{
    partial class SecurityFunctionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecurityFunctionForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblLine100 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtSQL = new FastColoredTextBoxNS.FastColoredTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lstParameter = new System.Windows.Forms.ListBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmdParameterDelete = new System.Windows.Forms.Button();
            this.cmdParameterAdd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdDelete);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 561);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 36);
            this.panel1.TabIndex = 1;
            // 
            // cmdDelete
            // 
            this.cmdDelete.Location = new System.Drawing.Point(15, 8);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(75, 23);
            this.cmdDelete.TabIndex = 3;
            this.cmdDelete.Text = "Remove";
            this.cmdDelete.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(450, 8);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(370, 8);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.lblLine100);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(534, 65);
            this.panel2.TabIndex = 72;
            // 
            // lblLine100
            // 
            this.lblLine100.BackColor = System.Drawing.Color.DarkGray;
            this.lblLine100.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLine100.Location = new System.Drawing.Point(0, 63);
            this.lblLine100.Name = "lblLine100";
            this.lblLine100.Size = new System.Drawing.Size(534, 2);
            this.lblLine100.TabIndex = 72;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(91, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(431, 38);
            this.label1.TabIndex = 68;
            this.label1.Text = "Enter the function information behind which to secure an entity.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // txtSQL
            // 
            this.txtSQL.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.txtSQL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSQL.CommentPrefix = "--";
            this.txtSQL.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQL.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSQL.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtSQL.LeftBracket = '(';
            this.txtSQL.Location = new System.Drawing.Point(0, 25);
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.RightBracket = ')';
            this.txtSQL.Size = new System.Drawing.Size(507, 203);
            this.txtSQL.TabIndex = 73;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 74;
            this.label2.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(56, 72);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(466, 20);
            this.txtName.TabIndex = 75;
            // 
            // lstParameter
            // 
            this.lstParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstParameter.FormattingEnabled = true;
            this.lstParameter.Location = new System.Drawing.Point(0, 21);
            this.lstParameter.Name = "lstParameter";
            this.lstParameter.Size = new System.Drawing.Size(233, 171);
            this.lstParameter.TabIndex = 76;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(270, 225);
            this.propertyGrid1.TabIndex = 77;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstParameter);
            this.splitContainer1.Panel1.Controls.Add(this.panel4);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(507, 225);
            this.splitContainer1.SplitterDistance = 233;
            this.splitContainer1.TabIndex = 78;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cmdParameterDelete);
            this.panel4.Controls.Add(this.cmdParameterAdd);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 192);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(233, 33);
            this.panel4.TabIndex = 78;
            // 
            // cmdParameterDelete
            // 
            this.cmdParameterDelete.Location = new System.Drawing.Point(155, 7);
            this.cmdParameterDelete.Name = "cmdParameterDelete";
            this.cmdParameterDelete.Size = new System.Drawing.Size(75, 23);
            this.cmdParameterDelete.TabIndex = 0;
            this.cmdParameterDelete.Text = "Remove";
            this.cmdParameterDelete.UseVisualStyleBackColor = true;
            // 
            // cmdParameterAdd
            // 
            this.cmdParameterAdd.Location = new System.Drawing.Point(74, 7);
            this.cmdParameterAdd.Name = "cmdParameterAdd";
            this.cmdParameterAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdParameterAdd.TabIndex = 0;
            this.cmdParameterAdd.Text = "Add";
            this.cmdParameterAdd.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 21);
            this.label3.TabIndex = 77;
            this.label3.Text = "Parameters";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtSQL);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(507, 457);
            this.splitContainer2.SplitterDistance = 228;
            this.splitContainer2.TabIndex = 79;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.splitContainer2);
            this.panel3.Location = new System.Drawing.Point(15, 98);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(507, 457);
            this.panel3.TabIndex = 80;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(507, 25);
            this.label4.TabIndex = 74;
            this.label4.Text = "SQL: This must result in a set of rows from the parent entity";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SecurityFunctionForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(534, 597);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(550, 538);
            this.Name = "SecurityFunctionForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Entity Security";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lblLine100;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
        private FastColoredTextBoxNS.FastColoredTextBox txtSQL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ListBox lstParameter;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button cmdParameterDelete;
        private System.Windows.Forms.Button cmdParameterAdd;
        private System.Windows.Forms.Label label4;
	}
}