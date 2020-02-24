using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using nHydrate.Dsl;

namespace nHydrate.DslPackage.Forms
{
	public partial class IndexColumnOrder : Form
	{
		private nHydrateModel _model = null;
		private Store _store = null;
		private Index _index = null;

		public IndexColumnOrder()
		{
			InitializeComponent();
		}

		public IndexColumnOrder(Index index, nHydrateModel model, Store store)
			: this()
		{
			_index = index;
			_model = model;
			_store = store;

			foreach (var indexColumn in index.IndexColumns.OrderBy(x => x.SortOrder))
			{
				lstItem.Items.Add(indexColumn);
			}
		}

		private void cmdMoveUp_Click(object sender, EventArgs e)
		{
			if (lstItem.SelectedItem != null)
			{
				var selectedIndex = lstItem.SelectedIndex;
				if (selectedIndex > 0)
				{
					var newIndex = selectedIndex - 1;
					lstItem.Items.Insert(newIndex, lstItem.Items[selectedIndex]);
					lstItem.Items.RemoveAt(newIndex + 2);
					lstItem.SelectedIndex = newIndex;
				}
			}
		}

		private void cmdMoveDown_Click(object sender, EventArgs e)
		{
			if (lstItem.SelectedItem!=null)
			{
				var selectedIndex = lstItem.SelectedIndex;
				if (selectedIndex < lstItem.Items.Count - 1)
				{
					var newIndex = selectedIndex + 2;
					lstItem.Items.Insert(newIndex, lstItem.Items[selectedIndex]);
					lstItem.Items.RemoveAt(newIndex - 2);
					lstItem.SelectedIndex = newIndex - 1;
				}
			}

		}

		private void cmdOK_Click(object sender, EventArgs e)
		{

			using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				var it = _index.IndexType;
				_index.IndexType = IndexTypeConstants.User;
				var index = 1;
				foreach (IndexColumn ic in lstItem.Items)
				{
					ic.SortOrder = index++;
				}
				_index.IndexColumns.Sort(delegate(IndexColumn left, IndexColumn right)
				{
					return left.SortOrder > right.SortOrder ? 1 : 0;
				});
				_index.IndexType = it;
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

		
	}
}

