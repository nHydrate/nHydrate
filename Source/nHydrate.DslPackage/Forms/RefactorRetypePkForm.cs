using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Dsl;

namespace nHydrate.DslPackage.Forms
{
	public partial class RefactorRetypePkForm : Form
	{
		private Microsoft.VisualStudio.Modeling.Store _store;
		private nHydrateModel _model;
		private Entity _entity;

		public RefactorRetypePkForm()
		{
			InitializeComponent();
		}

		public RefactorRetypePkForm(Microsoft.VisualStudio.Modeling.Store store, nHydrateModel model, Entity entity)
			: this()
		{
			_store = store;
			_model = model;
			_entity = entity;

			foreach (var s in Enum.GetNames(typeof(DataTypeConstants)))
			{
				cboType.Items.Add(s);
			}

			cboType.SelectedItem = entity.PrimaryKeyFields.First().DataType.ToString();

			var tree = new Dictionary<EntityHasEntities, Field>();
			BuildRelationTree(entity, tree);

			var used = new List<string>();
			foreach (var r in tree.Keys)
			{
				var otherField = tree[r];
				var c = r.ParentEntity.Name + "." + otherField.Name;
				var p = r.ChildEntity.Name + "." + otherField.Name;
				if (!used.Contains(p))
				{
					lstItem.Items.Add(new RelationDisplay() { Display = p, Relation = r });
					used.Add(p);
				}
				if (!used.Contains(c))
				{
					lstItem.Items.Add(new RelationDisplay() { Display = c, Relation = r });
					used.Add(c);
				}
			}
		}

		private void BuildRelationTree(Entity entity, Dictionary<EntityHasEntities, Field> tree)
		{
			//Find all out bound relations
			foreach (var r in entity.RelationshipList)
			{
				if (!tree.Keys.Contains(r))
				{
					var relationField = r.FieldMapList().FirstOrDefault();
					var field = relationField.GetTargetField(r);
					if (relationField != null && field != null)
					{
						tree.Add(r, field);
						if (field.IsPrimaryKey)
							BuildRelationTree(r.ChildEntity, tree);
					}
				}
			}

			foreach (var r in entity.GetRelationsWhereChild())
			{
				if (!tree.Keys.Contains(r))
				{
					var relationField = r.FieldMapList().FirstOrDefault();
					if (relationField != null && 
						relationField.GetSourceField(r) != null &&
						relationField.GetTargetField(r) == entity.PrimaryKeyFields.First())
					{
						tree.Add(r, relationField.GetSourceField(r));
						BuildRelationTree(r.ParentEntity, tree);
					}
				}
			}

		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			var dataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), (string)cboType.SelectedItem);
			if (_entity.PrimaryKeyFields.First().DataType == dataType)
			{
				MessageBox.Show("You must choose a different data type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				foreach (RelationDisplay rd in lstItem.Items)
				{
					var rf = rd.Relation.FieldMapList().First();
					rf.GetSourceField(rd.Relation).DataType = dataType;
					rf.GetTargetField(rd.Relation).DataType = dataType;
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

		private  class RelationDisplay
		{
			public string Display { get; set; }
			public EntityHasEntities Relation { get; set; }

			public override string ToString()
			{
				return this.Display;
			}
		}
	}
}
