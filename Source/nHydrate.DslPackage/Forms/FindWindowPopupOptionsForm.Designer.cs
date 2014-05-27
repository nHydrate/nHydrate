namespace nHydrate.DslPackage.Forms
{
	partial class FindWindowPopupOptionsForm
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
			this.chkEntity = new System.Windows.Forms.CheckBox();
			this.chkView = new System.Windows.Forms.CheckBox();
			this.chkStoredProc = new System.Windows.Forms.CheckBox();
			this.chkFunction = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chkField = new System.Windows.Forms.CheckBox();
			this.chkParameter = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// chkEntity
			// 
			this.chkEntity.AutoSize = true;
			this.chkEntity.Checked = true;
			this.chkEntity.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEntity.Location = new System.Drawing.Point(12, 36);
			this.chkEntity.Name = "chkEntity";
			this.chkEntity.Size = new System.Drawing.Size(52, 17);
			this.chkEntity.TabIndex = 0;
			this.chkEntity.Text = "Entity";
			this.chkEntity.UseVisualStyleBackColor = true;
			// 
			// chkView
			// 
			this.chkView.AutoSize = true;
			this.chkView.Checked = true;
			this.chkView.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkView.Location = new System.Drawing.Point(12, 59);
			this.chkView.Name = "chkView";
			this.chkView.Size = new System.Drawing.Size(49, 17);
			this.chkView.TabIndex = 1;
			this.chkView.Text = "View";
			this.chkView.UseVisualStyleBackColor = true;
			// 
			// chkStoredProc
			// 
			this.chkStoredProc.AutoSize = true;
			this.chkStoredProc.Checked = true;
			this.chkStoredProc.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkStoredProc.Location = new System.Drawing.Point(12, 82);
			this.chkStoredProc.Name = "chkStoredProc";
			this.chkStoredProc.Size = new System.Drawing.Size(109, 17);
			this.chkStoredProc.TabIndex = 2;
			this.chkStoredProc.Text = "Stored Procedure";
			this.chkStoredProc.UseVisualStyleBackColor = true;
			// 
			// chkFunction
			// 
			this.chkFunction.AutoSize = true;
			this.chkFunction.Checked = true;
			this.chkFunction.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFunction.Location = new System.Drawing.Point(12, 105);
			this.chkFunction.Name = "chkFunction";
			this.chkFunction.Size = new System.Drawing.Size(67, 17);
			this.chkFunction.TabIndex = 3;
			this.chkFunction.Text = "Function";
			this.chkFunction.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label1.Location = new System.Drawing.Point(12, 131);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(201, 1);
			this.label1.TabIndex = 2;
			// 
			// chkField
			// 
			this.chkField.AutoSize = true;
			this.chkField.Location = new System.Drawing.Point(12, 140);
			this.chkField.Name = "chkField";
			this.chkField.Size = new System.Drawing.Size(48, 17);
			this.chkField.TabIndex = 4;
			this.chkField.Text = "Field";
			this.chkField.UseVisualStyleBackColor = true;
			// 
			// chkParameter
			// 
			this.chkParameter.AutoSize = true;
			this.chkParameter.Location = new System.Drawing.Point(12, 163);
			this.chkParameter.Name = "chkParameter";
			this.chkParameter.Size = new System.Drawing.Size(74, 17);
			this.chkParameter.TabIndex = 5;
			this.chkParameter.Text = "Parameter";
			this.chkParameter.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label2.Location = new System.Drawing.Point(12, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(201, 20);
			this.label2.TabIndex = 6;
			this.label2.Text = "Search Options";
			// 
			// FindWindowPopupOptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(225, 188);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.chkParameter);
			this.Controls.Add(this.chkField);
			this.Controls.Add(this.chkFunction);
			this.Controls.Add(this.chkStoredProc);
			this.Controls.Add(this.chkView);
			this.Controls.Add(this.chkEntity);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindWindowPopupOptionsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkEntity;
		private System.Windows.Forms.CheckBox chkView;
		private System.Windows.Forms.CheckBox chkStoredProc;
		private System.Windows.Forms.CheckBox chkFunction;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkField;
		private System.Windows.Forms.CheckBox chkParameter;
		private System.Windows.Forms.Label label2;
	}
}