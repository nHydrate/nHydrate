namespace nHydrate.Generator.ModelUI
{
	partial class RelationshipControllerUIControl
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
			this.label1 = new System.Windows.Forms.Label();
			this.lblTable = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblField = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblRole = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Relationship link:";
			// 
			// lblTable
			// 
			this.lblTable.AutoSize = true;
			this.lblTable.Location = new System.Drawing.Point(102, 4);
			this.lblTable.Name = "lblTable";
			this.lblTable.Size = new System.Drawing.Size(42, 13);
			this.lblTable.TabIndex = 0;
			this.lblTable.Text = "[DATA]";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(4, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Field links:";
			// 
			// lblField
			// 
			this.lblField.AutoSize = true;
			this.lblField.Location = new System.Drawing.Point(102, 52);
			this.lblField.Name = "lblField";
			this.lblField.Size = new System.Drawing.Size(42, 13);
			this.lblField.TabIndex = 0;
			this.lblField.Text = "[DATA]";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Role name:";
			// 
			// lblRole
			// 
			this.lblRole.AutoSize = true;
			this.lblRole.Location = new System.Drawing.Point(102, 27);
			this.lblRole.Name = "lblRole";
			this.lblRole.Size = new System.Drawing.Size(42, 13);
			this.lblRole.TabIndex = 0;
			this.lblRole.Text = "[DATA]";
			// 
			// RelationshipControllerUIControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblField);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblRole);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblTable);
			this.Controls.Add(this.label1);
			this.Name = "RelationshipControllerUIControl";
			this.Size = new System.Drawing.Size(407, 150);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblTable;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblField;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblRole;
	}
}
