#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using System.IO;
using System.Windows.Forms;

namespace nHydrate.Generator
{
	public partial class DatabaseConnectionControl : UserControl
	{
		private readonly DataImportOptions _importOptions = new DataImportOptions();

		public DatabaseConnectionControl()
		{
			InitializeComponent();
			this.RefreshEnabled();
		}

		#region Properties

		public DataImportOptions ImportOptions
		{
			get
			{
				this.RefreshOptions();
				return _importOptions;
			}
		}

		#endregion

		#region Event Handlers

		private void opt1_CheckedChanged(object sender, System.EventArgs e)
		{
			if (opt1.Checked)
				opt2.Checked = false;
			this.RefreshEnabled();
		}

		private void opt2_CheckedChanged(object sender, System.EventArgs e)
		{
			if (opt2.Checked)
				opt1.Checked = false;
			this.RefreshEnabled();
		}

		private void chkWinAuth_CheckedChanged(object sender, EventArgs e)
		{
			this.RefreshEnabled();
		}

		#endregion

		#region Methods

		private void RefreshEnabled()
		{
			lblServer.Enabled = opt1.Checked;
			lblDatabase.Enabled = opt1.Checked;
			lblUID.Enabled = opt1.Checked && !chkWinAuth.Checked;
			lblPWD.Enabled = opt1.Checked && !chkWinAuth.Checked;
			txtServer.Enabled = opt1.Checked;
			txtDatabase.Enabled = opt1.Checked;
			txtUID.Enabled = opt1.Checked && !chkWinAuth.Checked;
			txtPWD.Enabled = opt1.Checked && !chkWinAuth.Checked;

			lblConnectionString.Enabled = opt2.Checked;
			txtConnectionString.Enabled = opt2.Checked;
		}

		public	void RefreshOptions()
		{
			_importOptions.UseConnectionString = !opt1.Checked;
			_importOptions.ConnectionString = txtConnectionString.Text;
			_importOptions.Database = txtDatabase.Text;
			_importOptions.PWD = txtPWD.Text;
			_importOptions.Server = txtServer.Text;
			_importOptions.UID = txtUID.Text;
			_importOptions.UseWinAuth = chkWinAuth.Checked;
		}

		private string FileName
		{
			get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"nHydrate\nHydrate.ConnectionDialog.config.xml"); }
		}

		public void LoadSettings()
		{
			var fi = new FileInfo(this.FileName);
			if (!fi.Directory.Exists)
				fi.Directory.Create();

			_importOptions.LoadXML(this.FileName);

			opt1.Checked = !_importOptions.UseConnectionString;
			opt2.Checked = _importOptions.UseConnectionString;
			txtConnectionString.Text = _importOptions.ConnectionString;
			txtDatabase.Text = _importOptions.Database;
			txtPWD.Text = _importOptions.PWD;
			txtServer.Text = _importOptions.Server;
			txtUID.Text = _importOptions.UID;
			chkWinAuth.Checked = _importOptions.UseWinAuth;

		}

		public void PersistSettings()
		{
			this.RefreshOptions();

			var fi = new FileInfo(this.FileName);
			if (!fi.Directory.Exists)
				fi.Directory.Create();

			_importOptions.SaveXML(this.FileName);
		}

		#endregion

	}
}

