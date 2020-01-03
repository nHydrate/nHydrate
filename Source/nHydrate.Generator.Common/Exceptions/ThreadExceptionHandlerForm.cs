#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System.Text;
using System.Windows.Forms;

namespace nHydrate.Generator.Common.Exceptions
{
	/// <summary>
	/// Summary description for ThreadExceptionHandlerForm.
	/// </summary>
	public partial class ThreadExceptionHandlerForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox _tbExceptionText;
		private System.Windows.Forms.Button _btnContinue;
		private System.Windows.Forms.Button _btnExit;

		public ThreadExceptionHandlerForm(string message)
		{
			InitializeComponent();
			this._tbExceptionText.Visible = false;
			this.lnkPasteToClipboard.Visible = false;
			this.Size = new System.Drawing.Size(790, 125);
			this._btnDetails.Click += new System.EventHandler(this._btnDetails_Click);
			_tbExceptionText.Text = message;
		}    /// <summary>
		/// Required designer variable.
		/// </summary>
		private readonly System.ComponentModel.Container components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			this._tbExceptionText = new System.Windows.Forms.TextBox();
			this._btnContinue = new System.Windows.Forms.Button();
			this._btnExit = new System.Windows.Forms.Button();
			this._btnDetails = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lnkPasteToClipboard = new System.Windows.Forms.LinkLabel();
			this._pnlClickToPasteError = new System.Windows.Forms.Panel();
			this._pnlLabelButtons = new System.Windows.Forms.Panel();
			this._pnlButtons = new System.Windows.Forms.Panel();
			this._pnlContinue = new System.Windows.Forms.Panel();
			this._pnlDetails = new System.Windows.Forms.Panel();
			this._pnlExit = new System.Windows.Forms.Panel();
			this._pnlClickToPasteError.SuspendLayout();
			this._pnlLabelButtons.SuspendLayout();
			this._pnlButtons.SuspendLayout();
			this._pnlContinue.SuspendLayout();
			this._pnlDetails.SuspendLayout();
			this._pnlExit.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tbExceptionText
			// 
			this._tbExceptionText.AcceptsReturn = true;
			this._tbExceptionText.AcceptsTab = true;
			this._tbExceptionText.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tbExceptionText.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._tbExceptionText.Location = new System.Drawing.Point(0, 90);
			this._tbExceptionText.Multiline = true;
			this._tbExceptionText.Name = "_tbExceptionText";
			this._tbExceptionText.ReadOnly = true;
			this._tbExceptionText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this._tbExceptionText.Size = new System.Drawing.Size(798, 180);
			this._tbExceptionText.TabIndex = 0;
			this._tbExceptionText.WordWrap = false;
			// 
			// _btnContinue
			// 
			this._btnContinue.Dock = System.Windows.Forms.DockStyle.Fill;
			this._btnContinue.Location = new System.Drawing.Point(0, 2);
			this._btnContinue.Name = "_btnContinue";
			this._btnContinue.Size = new System.Drawing.Size(107, 26);
			this._btnContinue.TabIndex = 1;
			this._btnContinue.Text = "&Continue";
			this._btnContinue.Click += new System.EventHandler(this._btnContinue_Click);
			// 
			// _btnExit
			// 
			this._btnExit.Dock = System.Windows.Forms.DockStyle.Fill;
			this._btnExit.Location = new System.Drawing.Point(0, 2);
			this._btnExit.Name = "_btnExit";
			this._btnExit.Size = new System.Drawing.Size(107, 26);
			this._btnExit.TabIndex = 2;
			this._btnExit.Text = "&Exit";
			this._btnExit.Click += new System.EventHandler(this._btnExit_Click);
			// 
			// _btnDetails
			// 
			this._btnDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this._btnDetails.Location = new System.Drawing.Point(0, 2);
			this._btnDetails.Name = "_btnDetails";
			this._btnDetails.Size = new System.Drawing.Size(107, 26);
			this._btnDetails.TabIndex = 3;
			this._btnDetails.Text = "&Details>>";
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(691, 90);
			this.label1.TabIndex = 4;
			this.label1.Text = "An Error has occured that may be recoverable.  Press Continue to proceed or Exit " +
					"to end the program.";
			// 
			// lnkPasteToClipboard
			// 
			this.lnkPasteToClipboard.AutoSize = true;
			this.lnkPasteToClipboard.Dock = System.Windows.Forms.DockStyle.Right;
			this.lnkPasteToClipboard.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkPasteToClipboard.Location = new System.Drawing.Point(517, 0);
			this.lnkPasteToClipboard.Name = "lnkPasteToClipboard";
			this.lnkPasteToClipboard.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.lnkPasteToClipboard.Size = new System.Drawing.Size(281, 24);
			this.lnkPasteToClipboard.TabIndex = 5;
			this.lnkPasteToClipboard.TabStop = true;
			this.lnkPasteToClipboard.Text = "Click here to paste error to clipboard";
			this.lnkPasteToClipboard.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPasteToClipboard_LinkClicked);
			// 
			// _pnlClickToPasteError
			// 
			this._pnlClickToPasteError.Controls.Add(this.lnkPasteToClipboard);
			this._pnlClickToPasteError.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._pnlClickToPasteError.Location = new System.Drawing.Point(0, 270);
			this._pnlClickToPasteError.Name = "_pnlClickToPasteError";
			this._pnlClickToPasteError.Size = new System.Drawing.Size(798, 28);
			this._pnlClickToPasteError.TabIndex = 6;
			// 
			// _pnlLabelButtons
			// 
			this._pnlLabelButtons.Controls.Add(this.label1);
			this._pnlLabelButtons.Controls.Add(this._pnlButtons);
			this._pnlLabelButtons.Dock = System.Windows.Forms.DockStyle.Top;
			this._pnlLabelButtons.Location = new System.Drawing.Point(0, 0);
			this._pnlLabelButtons.Name = "_pnlLabelButtons";
			this._pnlLabelButtons.Size = new System.Drawing.Size(798, 90);
			this._pnlLabelButtons.TabIndex = 7;
			// 
			// _pnlButtons
			// 
			this._pnlButtons.Controls.Add(this._pnlExit);
			this._pnlButtons.Controls.Add(this._pnlDetails);
			this._pnlButtons.Controls.Add(this._pnlContinue);
			this._pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
			this._pnlButtons.Location = new System.Drawing.Point(691, 0);
			this._pnlButtons.Name = "_pnlButtons";
			this._pnlButtons.Size = new System.Drawing.Size(107, 90);
			this._pnlButtons.TabIndex = 5;
			// 
			// _pnlContinue
			// 
			this._pnlContinue.Controls.Add(this._btnContinue);
			this._pnlContinue.Dock = System.Windows.Forms.DockStyle.Top;
			this._pnlContinue.Location = new System.Drawing.Point(0, 0);
			this._pnlContinue.Name = "_pnlContinue";
			this._pnlContinue.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this._pnlContinue.Size = new System.Drawing.Size(107, 30);
			this._pnlContinue.TabIndex = 4;
			// 
			// _pnlDetails
			// 
			this._pnlDetails.Controls.Add(this._btnDetails);
			this._pnlDetails.Dock = System.Windows.Forms.DockStyle.Top;
			this._pnlDetails.Location = new System.Drawing.Point(0, 30);
			this._pnlDetails.Name = "_pnlDetails";
			this._pnlDetails.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this._pnlDetails.Size = new System.Drawing.Size(107, 30);
			this._pnlDetails.TabIndex = 5;
			// 
			// _pnlExit
			// 
			this._pnlExit.Controls.Add(this._btnExit);
			this._pnlExit.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pnlExit.Location = new System.Drawing.Point(0, 60);
			this._pnlExit.Name = "_pnlExit";
			this._pnlExit.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this._pnlExit.Size = new System.Drawing.Size(107, 30);
			this._pnlExit.TabIndex = 6;
			// 
			// ThreadExceptionHandlerForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
			this.ClientSize = new System.Drawing.Size(798, 298);
			this.Controls.Add(this._tbExceptionText);
			this.Controls.Add(this._pnlLabelButtons);
			this.Controls.Add(this._pnlClickToPasteError);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ThreadExceptionHandlerForm";
			this.Text = "Application Error";
			this._pnlClickToPasteError.ResumeLayout(false);
			this._pnlClickToPasteError.PerformLayout();
			this._pnlLabelButtons.ResumeLayout(false);
			this._pnlButtons.ResumeLayout(false);
			this._pnlContinue.ResumeLayout(false);
			this._pnlDetails.ResumeLayout(false);
			this._pnlExit.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void _btnContinue_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Retry;
		}

		private void _btnExit_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Abort;
		}

		private void _btnDetails_Click(object sender, System.EventArgs e)
		{
			if (_btnDetails.Text.Contains(">>"))
			{
				_btnDetails.Text = "<<&Less";
				this.Size = new System.Drawing.Size(790, 279);
				this._tbExceptionText.Visible = true;
				this.lnkPasteToClipboard.Visible = true;
			}
			else
			{
				_btnDetails.Text = "&More>>";
				this._tbExceptionText.Visible = false;
				this.lnkPasteToClipboard.Visible = false;
				this.Size = new System.Drawing.Size(790, 125);
			}
		}

		private void lnkPasteToClipboard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var sb = new StringBuilder();
			sb.Append("OS info: ");
			sb.AppendLine(System.Environment.OSVersion.ToString());
			sb.Append("Error Date: ");
			sb.AppendLine(System.DateTime.Now.ToString());
			sb.AppendLine();
			sb.Append(this._tbExceptionText.Text);

			Clipboard.SetText(sb.ToString());
		}

		private Button _btnDetails;
		private Label label1;
		private LinkLabel lnkPasteToClipboard;
		private Panel _pnlClickToPasteError;
		private Panel _pnlLabelButtons;
		private Panel _pnlButtons;
		private Panel _pnlExit;
		private Panel _pnlDetails;
		private Panel _pnlContinue;
	}
}

