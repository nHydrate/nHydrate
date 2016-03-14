#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using nHydrate.Generator.Models;
using System.Collections;
using nHydrate.Dsl;

namespace nHydrate.DslPackage.Forms
{
	public partial class DeleteColumnsForm : Form
	{
		private nHydrateModel _model = null;
		private Microsoft.VisualStudio.Modeling.Store _store = null;

		#region Constructors

		public DeleteColumnsForm()
		{
			InitializeComponent();

			listView1.Columns.Clear();
			listView1.Columns.Add("Entity", 200);
			listView1.Columns.Add("Field", 200);

			listView1.ColumnClick += new ColumnClickEventHandler(listView1_ColumnClick);

		}

		public DeleteColumnsForm(nHydrateModel model, Microsoft.VisualStudio.Modeling.Store store)
			: this()
		{
			_model = model;
			_store = store;
		}

		#endregion

		#region Methods

		private void FindColumns()
		{
			listView1.Items.Clear();
			foreach (var entity in _model.Entities)
			{
				foreach (var column in entity.Fields)
				{
					if (column.Name.ToUpperInvariant().Contains(txtColumnName.Text.ToUpperInvariant()))
					{
						var li = new ListViewItem();
						li.Text = column.Entity.Name;
						li.SubItems.Add(column.Name);
						li.Checked = true;
						li.Tag = column;
						listView1.Items.Add(li);
					}
				}
			}

			listView1.ListViewItemSorter = new nHydrate.Generator.Common.Forms.CommonLibrary.ListViewItemComparer(sortColumn, listView1.Sorting);
			listView1.Sort();

		}

		#endregion

		#region Event Handlers

		private void cmdRemove_Click(object sender, EventArgs e)
		{
			var items = new List<ListViewItem>();
			foreach (ListViewItem li in listView1.Items)
			{
				items.Add(li);
			}

			if (items.Count(x => x.Checked) == 0)
			{
				MessageBox.Show("There are no items selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (MessageBox.Show("Do you wish to remove " + items.Count(x => x.Checked) + " selected items?", "Remove Fields?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
			{
				return;
			}

			//Remove the columns
			using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				foreach (var li in items.Where(x => x.Checked))
				{
					var field = li.Tag as Field;
					var entity = field.Entity;
					entity.Fields.Remove(field);
				}
				transaction.Commit();
			}

			this.FindColumns();

		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void cmdFind_Click(object sender, EventArgs e)
		{
			this.FindColumns();
		}

		private void cmdCheckAll_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem li in listView1.Items)
			{
				li.Checked = true;
			}
		}

		private void cmdUncheckAll_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem li in listView1.Items)
			{
				li.Checked = false;
			}
		}

		private int sortColumn = -1;
		private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// Determine whether the column is the same as the last column clicked.
			if (e.Column != sortColumn)
			{
				// Set the sort column to the new column.
				sortColumn = e.Column;
				// Set the sort order to ascending by default.
				listView1.Sorting = SortOrder.Ascending;
			}
			else
			{
				// Determine what the last sort order was and change it.
				if (listView1.Sorting == SortOrder.Ascending)
					listView1.Sorting = SortOrder.Descending;
				else
					listView1.Sorting = SortOrder.Ascending;
			}

			// Call the sort method to manually sort.
			listView1.Sort();
			this.listView1.ListViewItemSorter = new nHydrate.Generator.Common.Forms.CommonLibrary.ListViewItemComparer(e.Column, listView1.Sorting);
		}

		#endregion

	}
}
