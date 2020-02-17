using System;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Models;
using nHydrate.Dsl;

namespace nHydrate.DslPackage.Forms
{
	public partial class TableExtendedPropertiesForm : Form
	{
		public TableExtendedPropertiesForm()
		{
			InitializeComponent();
		}

		public TableExtendedPropertiesForm(
			Entity entity,
			Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram)
			: this()
		{
			lblHeader.Text = entity.Name + " Entity";

			//Child tables
			foreach (var child in entity.ChildEntities)
			{
				//if (child.IsDerivedFrom(entity))
				lstChildTable.Items.Add(child.Name);
			}

			//Parent relationships
			var connectorList= diagram.NestedChildShapes.Where(x=>x is EntityAssociationConnector).AsEnumerable<EntityAssociationConnector>();
			foreach (var connector in connectorList)
			{
				var parentTable = connector.FromShape.ModelElement as Entity;
				var childTable = connector.ToShape.ModelElement as Entity;
				if (childTable == entity)
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

