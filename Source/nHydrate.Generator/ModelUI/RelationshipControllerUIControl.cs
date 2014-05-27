using System.Windows.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.ModelUI
{
	public partial class RelationshipControllerUIControl : UserControl
	{
		public RelationshipControllerUIControl()
		{
			InitializeComponent();
		}

		public void Populate(Relation relation)
		{
			lblTable.Text = relation.ParentTable.Name + " -> " + relation.ChildTable.Name;
			lblRole.Text = (string.IsNullOrEmpty(relation.RoleName) ? "---" : relation.RoleName);

			var text = string.Empty;

			foreach (ColumnRelationship r in relation.ColumnRelationships)
			{
				text += r.ParentColumn.Name + " = " + r.ChildColumn.Name + "\r\n";
			}

			lblField.Text = text;
		}

	}
}
