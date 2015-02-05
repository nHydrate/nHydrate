#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nHydrate.DslPackage.Forms
{
	public partial class FirstPromptForm : Form
	{
		private nHydrate.Dsl.nHydrateModel _model = null;

		public FirstPromptForm()
		{
			InitializeComponent();
			txtCompany.TextChanged += new EventHandler(TextBox_TextChanged);
			txtProject.TextChanged += new EventHandler(TextBox_TextChanged);
		}

		public FirstPromptForm(nHydrate.Dsl.nHydrateModel model)
			: this()
		{
			_model = model;

			//Load the platoforms
			foreach (var s in Enum.GetNames(typeof(nHydrate.Dsl.DatabasePlatformConstants)))
			{
				lstPlatforms.Items.Add(s);
			}

			txtCompany.Text = _model.CompanyName;
			txtProject.Text = _model.ProjectName;

			for (var ii = 0; ii < lstPlatforms.Items.Count; ii++)
			{
				var v = (Dsl.DatabasePlatformConstants)Enum.Parse(typeof(Dsl.DatabasePlatformConstants), (string)lstPlatforms.Items[ii]);
				if ((_model.SupportedPlatforms & v) == v)
					lstPlatforms.SetItemChecked(ii, true);
			}

			TextBox_TextChanged(null, null);
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			txtCompany.Text=txtCompany.Text.Trim();
			txtProject.Text = txtProject.Text.Trim();

			if (!nHydrate.Dsl.ValidationHelper.ValidDatabaseIdenitifer(txtCompany.Text) || !nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(txtCompany.Text))
			{
				MessageBox.Show(nHydrate.Dsl.ValidationHelper.ErrorTextInvalidCompany, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else if (!nHydrate.Dsl.ValidationHelper.ValidDatabaseIdenitifer(txtProject.Text) || !nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(txtProject.Text))
			{
				MessageBox.Show(nHydrate.Dsl.ValidationHelper.ErrorTextInvalidProject, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (var transaction = _model.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				_model.CompanyName = txtCompany.Text.Trim();
				_model.ProjectName = txtProject.Text.Trim();

				_model.SupportedPlatforms = 0;
				foreach (string s in lstPlatforms.CheckedItems)
				{
					var v = (Dsl.DatabasePlatformConstants)Enum.Parse(typeof(Dsl.DatabasePlatformConstants), s);
					_model.SupportedPlatforms = _model.SupportedPlatforms | v;
				}

				transaction.Commit();
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		private void TextBox_TextChanged(object sender, EventArgs e)
		{
			var v = string.Empty;
			if (string.IsNullOrEmpty(txtCompany.Text.Trim()))
				v += "[NOT SET]";
			else
				v += txtCompany.Text.Trim();

			v += ".";

			if (string.IsNullOrEmpty(txtProject.Text.Trim()))
				v += "[NOT SET]";
			else
				v += txtProject.Text.Trim();

			lblNamespace.Text = v;
		}

	}
}

