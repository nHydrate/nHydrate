namespace nHydrate.DslPackage.Forms
{
	partial class DocumentationWindowControl
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
			this.pnlSummary = new System.Windows.Forms.Panel();
			this.lblObjectName = new System.Windows.Forms.Label();
			this.txtSummary = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.pnlSummary.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlSummary
			// 
			this.pnlSummary.Controls.Add(this.lblObjectName);
			this.pnlSummary.Controls.Add(this.txtSummary);
			this.pnlSummary.Controls.Add(this.label2);
			this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSummary.Location = new System.Drawing.Point(0, 0);
			this.pnlSummary.Name = "pnlSummary";
			this.pnlSummary.Size = new System.Drawing.Size(428, 227);
			this.pnlSummary.TabIndex = 0;
			// 
			// lblObjectName
			// 
			this.lblObjectName.AutoSize = true;
			this.lblObjectName.Location = new System.Drawing.Point(114, 10);
			this.lblObjectName.Name = "lblObjectName";
			this.lblObjectName.Size = new System.Drawing.Size(54, 13);
			this.lblObjectName.TabIndex = 2;
			this.lblObjectName.Text = "[OBJECT]";
			// 
			// txtSummary
			// 
			this.txtSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSummary.Location = new System.Drawing.Point(8, 35);
			this.txtSummary.Multiline = true;
			this.txtSummary.Name = "txtSummary";
			this.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSummary.Size = new System.Drawing.Size(410, 180);
			this.txtSummary.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(5, 5);
			this.label2.Margin = new System.Windows.Forms.Padding(5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(116, 22);
			this.label2.TabIndex = 0;
			this.label2.Text = "Documentation for:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DocumentationWindowControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlSummary);
			this.Name = "DocumentationWindowControl";
			this.Size = new System.Drawing.Size(428, 227);
			this.pnlSummary.ResumeLayout(false);
			this.pnlSummary.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlSummary;
		private System.Windows.Forms.TextBox txtSummary;
		private System.Windows.Forms.Label lblObjectName;
		private System.Windows.Forms.Label label2;
	}
}
