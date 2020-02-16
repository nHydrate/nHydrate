using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nHydrate.DslPackage.Objects;

namespace nHydrate.DslPackage.Forms
{
	public partial class FindWindowColumnSetupForm : Form
	{
		private List<FindWindowColumnItem> _columns = null;

		public FindWindowColumnSetupForm()
		{
			InitializeComponent();
		}

		public FindWindowColumnSetupForm(List<FindWindowColumnItem> columns)
			: this()
		{
			_columns = columns;

			foreach (var column in _columns)
			{
				lstItem.Items.Add(column);
				if (column.Visible)
					lstItem.SetItemChecked(lstItem.Items.Count - 1, true);
			}
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			_columns.ForEach(x => x.Visible = false);
			foreach (FindWindowColumnItem item in lstItem.CheckedItems)
			{
				item.Visible = true;
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
