#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
using nHydrate.Dsl;

namespace nHydrate.DslPackage.Forms
{
	public partial class TableCompositeColumnForm : Form
	{
		#region Class Members

		private nHydrateModel _model = null;
		private Composite _composite = null;
		private Microsoft.VisualStudio.Modeling.Store _store = null;

		#endregion

		#region Constructors

		public TableCompositeColumnForm()
		{
			InitializeComponent();
		}

		public TableCompositeColumnForm(Composite composite, nHydrateModel model, Microsoft.VisualStudio.Modeling.Store store)
			: this()
		{
			_composite = composite;
			_model = model;
			_store = store;

			this.RefreshForm();
		}

		#endregion

		#region Methods

		private void RefreshForm()
		{
			var columnList = _composite.Entity.GetColumnsFullHierarchy();
			foreach (var column in (from x in columnList orderby x.Name select x))
			{
				lstMembers.Items.Add(column);
			}

			foreach (var field in _composite.GetFields())
			{
				if (lstMembers.Items.Contains(field))
				{
					lstMembers.SetItemChecked(lstMembers.Items.IndexOf(field), true);
				}
			}

		}

		#endregion

		#region Event Handlers

		private void cmdOK_Click(object sender, EventArgs e)
		{
			using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				_composite.Fields.Clear();
				foreach (Field column in lstMembers.CheckedItems)
				{
					var newField = new CompositeField(_model.Partition);
					newField.FieldId = column.Id;
					_composite.Fields.Add(newField);
				}
				transaction.Commit();
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		#endregion

	}
}

