namespace nHydrate.Generator.Forms
{
	partial class NewPackageForm
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
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this._lblPackageDescription = new System.Windows.Forms.Label();
			this._lblPackageName = new System.Windows.Forms.Label();
			this._tbPackagename = new System.Windows.Forms.TextBox();
			this._tbPackageDescription = new System.Windows.Forms.TextBox();
			this._lblDescriptionText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Enabled = false;
			this.cmdOK.Location = new System.Drawing.Point(152, 80);
			this.cmdOK.Margin = new System.Windows.Forms.Padding(2);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(72, 24);
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "&OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this._btnOk_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(232, 80);
			this.cmdCancel.Margin = new System.Windows.Forms.Padding(2);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(72, 24);
			this.cmdCancel.TabIndex = 3;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this._btnCancel_Click);
			// 
			// _lblPackageDescription
			// 
			this._lblPackageDescription.Location = new System.Drawing.Point(5, 57);
			this._lblPackageDescription.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this._lblPackageDescription.Name = "_lblPackageDescription";
			this._lblPackageDescription.Size = new System.Drawing.Size(110, 14);
			this._lblPackageDescription.TabIndex = 2;
			this._lblPackageDescription.Text = "Package Description:";
			this._lblPackageDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _lblPackageName
			// 
			this._lblPackageName.Location = new System.Drawing.Point(5, 34);
			this._lblPackageName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this._lblPackageName.Name = "_lblPackageName";
			this._lblPackageName.Size = new System.Drawing.Size(110, 14);
			this._lblPackageName.TabIndex = 3;
			this._lblPackageName.Text = "Package Name:";
			this._lblPackageName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _tbPackagename
			// 
			this._tbPackagename.Location = new System.Drawing.Point(115, 32);
			this._tbPackagename.Margin = new System.Windows.Forms.Padding(2);
			this._tbPackagename.Name = "_tbPackagename";
			this._tbPackagename.Size = new System.Drawing.Size(189, 20);
			this._tbPackagename.TabIndex = 0;
			this._tbPackagename.TextChanged += new System.EventHandler(this._tbPackagename_TextChanged);
			// 
			// _tbPackageDescription
			// 
			this._tbPackageDescription.Location = new System.Drawing.Point(115, 54);
			this._tbPackageDescription.Margin = new System.Windows.Forms.Padding(2);
			this._tbPackageDescription.Name = "_tbPackageDescription";
			this._tbPackageDescription.Size = new System.Drawing.Size(189, 20);
			this._tbPackageDescription.TabIndex = 1;
			this._tbPackageDescription.TextChanged += new System.EventHandler(this._tbPackageDescription_TextChanged);
			// 
			// _lblDescriptionText
			// 
			this._lblDescriptionText.Location = new System.Drawing.Point(5, 7);
			this._lblDescriptionText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this._lblDescriptionText.Name = "_lblDescriptionText";
			this._lblDescriptionText.Size = new System.Drawing.Size(299, 19);
			this._lblDescriptionText.TabIndex = 6;
			this._lblDescriptionText.Text = "Enter the name and discription of the new package.";
			// 
			// NewPackageForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(315, 114);
			this.Controls.Add(this._lblDescriptionText);
			this.Controls.Add(this._tbPackageDescription);
			this.Controls.Add(this._tbPackagename);
			this.Controls.Add(this._lblPackageName);
			this.Controls.Add(this._lblPackageDescription);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Create New Package";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label _lblPackageDescription;
		private System.Windows.Forms.Label _lblPackageName;
		private System.Windows.Forms.TextBox _tbPackagename;
		private System.Windows.Forms.TextBox _tbPackageDescription;
    private System.Windows.Forms.Label _lblDescriptionText;
	}
}