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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace nHydrate.Dsl.Design.Forms
{
	/// <summary>
	/// The SQL entry form. See link below for source to textbox
	/// </summary>
	/// <remarks>
	/// http://www.codeproject.com/KB/edit/FastColoredTextBox_.aspx
	/// </remarks>
	public partial class SQLForm : Form
	{
		public SQLForm()
		{
			InitializeComponent();
			this.Size = new Size((int)(Screen.PrimaryScreen.WorkingArea.Width * 0.8), (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.8));
			txtSQL.CurrentLineColor = Color.FromArgb(200, 210, 210, 255);
			txtSQL.ChangedLineColor = Color.FromArgb(255, 230, 230, 255);
		}

		public string SQL
		{
			get { return txtSQL.Text; }
			set
			{
				txtSQL.Text = value;
				txtSQL.ClearUndo();
				txtSQL.ClearUndo();
			}
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void txtSQL_Load(object sender, EventArgs e)
		{

		}

	}
}
