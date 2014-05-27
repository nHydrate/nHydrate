using System;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	public partial class TableExtendedPropertiesForm : Form
	{
		private Table _table = null;

		public TableExtendedPropertiesForm()
		{
			InitializeComponent();
		}

		public TableExtendedPropertiesForm(Table table)
			: this()
		{
			_table = table;
			lblHeader.Text = table.Name + " Entity";

			var root = table.Root as ModelRoot;

			//Child tables
			foreach (var t in root.Database.Tables.OrderBy(x => x.Name))
			{
				if (t.IsInheritedFrom(table))
					lstChildTable.Items.Add(t.Name);
			}

			//Parent relationships
			foreach (Relation r in root.Database.Relations)
			{
				var parentTable = r.ParentTableRef.Object as Table;
				var childTable = r.ChildTableRef.Object as Table;
				if (childTable == table)
				{
					lstParentTable.Items.Add(parentTable.Name);
				}
			}

		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
