namespace nHydrate.Generator.ModelUI
{
	partial class DatabaseControllerUIControl
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
			this.txtCreatedByColumn = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtCreatedDateColumn = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtModifiedByColumn = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtModifiedDateColumn = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtTimestampColumn = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtGrantExec = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// txtCreatedByColumn
			// 
			this.txtCreatedByColumn.Location = new System.Drawing.Point(167, 4);
			this.txtCreatedByColumn.Name = "txtCreatedByColumn";
			this.txtCreatedByColumn.Size = new System.Drawing.Size(163, 20);
			this.txtCreatedByColumn.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(127, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Created by column name:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(137, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Created date column name:";
			// 
			// txtCreatedDateColumn
			// 
			this.txtCreatedDateColumn.Location = new System.Drawing.Point(167, 30);
			this.txtCreatedDateColumn.Name = "txtCreatedDateColumn";
			this.txtCreatedDateColumn.Size = new System.Drawing.Size(163, 20);
			this.txtCreatedDateColumn.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(130, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Modified by column name:";
			// 
			// txtModifiedByColumn
			// 
			this.txtModifiedByColumn.Location = new System.Drawing.Point(167, 56);
			this.txtModifiedByColumn.Name = "txtModifiedByColumn";
			this.txtModifiedByColumn.Size = new System.Drawing.Size(163, 20);
			this.txtModifiedByColumn.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 82);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(140, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Modified date column name:";
			// 
			// txtModifiedDateColumn
			// 
			this.txtModifiedDateColumn.Location = new System.Drawing.Point(167, 82);
			this.txtModifiedDateColumn.Name = "txtModifiedDateColumn";
			this.txtModifiedDateColumn.Size = new System.Drawing.Size(163, 20);
			this.txtModifiedDateColumn.TabIndex = 3;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(7, 108);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(127, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "Timestamp column name:";
			// 
			// txtTimestampColumn
			// 
			this.txtTimestampColumn.Location = new System.Drawing.Point(167, 108);
			this.txtTimestampColumn.Name = "txtTimestampColumn";
			this.txtTimestampColumn.Size = new System.Drawing.Size(163, 20);
			this.txtTimestampColumn.TabIndex = 4;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(7, 134);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(85, 13);
			this.label6.TabIndex = 2;
			this.label6.Text = "Grant exec user:";
			// 
			// txtGrantExec
			// 
			this.txtGrantExec.Location = new System.Drawing.Point(167, 134);
			this.txtGrantExec.Name = "txtGrantExec";
			this.txtGrantExec.Size = new System.Drawing.Size(163, 20);
			this.txtGrantExec.TabIndex = 5;
			// 
			// toolTip1
			// 
			this.toolTip1.IsBalloon = true;
			// 
			// DatabaseControllerUIControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtGrantExec);
			this.Controls.Add(this.txtModifiedDateColumn);
			this.Controls.Add(this.txtCreatedDateColumn);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtTimestampColumn);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtModifiedByColumn);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtCreatedByColumn);
			this.Controls.Add(this.label1);
			this.Name = "DatabaseControllerUIControl";
			this.Size = new System.Drawing.Size(541, 304);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtCreatedByColumn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtCreatedDateColumn;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtModifiedByColumn;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtModifiedDateColumn;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtTimestampColumn;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtGrantExec;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}
