namespace nHydrate.DslPackage.Forms
{
	partial class ErrorForm
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
			this.txtBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtBox
			// 
			this.txtBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtBox.Location = new System.Drawing.Point(0, 0);
			this.txtBox.Multiline = true;
			this.txtBox.Name = "txtBox";
			this.txtBox.ReadOnly = true;
			this.txtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtBox.Size = new System.Drawing.Size(668, 511);
			this.txtBox.TabIndex = 0;
			// 
			// ErrorForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(668, 511);
			this.Controls.Add(this.txtBox);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ErrorForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Errors";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtBox;
	}
}