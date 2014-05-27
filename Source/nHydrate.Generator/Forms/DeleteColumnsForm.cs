using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	public partial class DeleteColumnsForm : Form
	{
		private readonly ModelRoot _modelRoot = null;
		private readonly List<Table> _changedTables = new List<Table>();

		#region Constructors

		public DeleteColumnsForm()
		{
			InitializeComponent();

			listView1.Columns.Clear();
			listView1.Columns.Add("Entity", 200);
			listView1.Columns.Add("Field", 200);

			listView1.ColumnClick += new ColumnClickEventHandler(listView1_ColumnClick);

		}

		public DeleteColumnsForm(ModelRoot modelRoot)
			: this()
		{
			_modelRoot = modelRoot;
		}

		#endregion

		public bool Changed
		{
			get { return (_changedTables.Count > 0); }
		}

		public IEnumerable<Table> ChangedTables
		{
			get { return _changedTables.AsEnumerable(); }
		}

		#region Methods

		private void FindColumns()
		{
			listView1.Items.Clear();
			foreach (Table table in _modelRoot.Database.Tables)
			{
				foreach (var column in table.GetColumns())
				{

					if (column.Name.ToUpperInvariant().Contains(txtColumnName.Text.ToUpperInvariant()))
					{
						var li = new ListViewItem();
						li.Text = ((Table)column.ParentTableRef.Object).Name;
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
			foreach (var li in items.Where(x => x.Checked))
			{
				var column = li.Tag as Column;
				var table = column.ParentTableRef.Object as Table;
				_modelRoot.Database.Columns.Remove(column);
				if (!_changedTables.Contains(table))
					_changedTables.Add(table);
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