#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nHydrate.Dsl;
using nHydrate.Dsl.Objects;

namespace nHydrate.DslPackage.Forms
{
	public partial class RefactorSplitTableForm : Form
	{
		private Microsoft.VisualStudio.Modeling.Store _store = null;
		private nHydrateDiagram _diagram = null;
		private nHydrateModel _model = null;
		private Entity _entity = null;

		public RefactorSplitTableForm()
		{
			InitializeComponent();
		}

		public RefactorSplitTableForm(Microsoft.VisualStudio.Modeling.Store store, nHydrateDiagram diagram, nHydrateModel model, Entity entity)
			: this()
		{
			_store = store;
			_model = model;
			_diagram = diagram;
			_entity = entity;
			wizard1.FinishEnabled = false;
			lblSourceEntity.Text = entity.Name;

			foreach (var field in entity.Fields.Where(x => !x.IsPrimaryKey).OrderBy(x => x.Name))
			{
				lstField1.Items.Add(new DisplayFieldItem() { Field = field });
			}
		}

		private void cmdFieldMoveRight_Click(object sender, EventArgs e)
		{
			if (lstField1.SelectedIndex != -1)
			{
				var index = lstField1.SelectedIndex;
				var item = lstField1.Items[lstField1.SelectedIndex];
				lstField2.Items.Add(item);
				lstField1.Items.RemoveAt(lstField1.SelectedIndex);
				if (index >= lstField1.Items.Count) index = lstField1.Items.Count - 1;
				if (index >= 0 && lstField1.Items.Count > 0) lstField1.SelectedIndex = index;
			}
		}

		private void cmdFieldMoveLeft_Click(object sender, EventArgs e)
		{
			if (lstField2.SelectedIndex != -1)
			{
				var index = lstField2.SelectedIndex;
				var item = lstField2.Items[lstField2.SelectedIndex];
				lstField1.Items.Add(item);
				lstField2.Items.RemoveAt(lstField2.SelectedIndex);
				if (index >= lstField2.Items.Count) index = lstField2.Items.Count - 1;
				if (index >= 0 && lstField2.Items.Count > 0) lstField2.SelectedIndex = index;
			}
		}

		private void wizard1_BeforeSwitchPages(object sender, Wizard.Wizard.BeforeSwitchPagesEventArgs e)
		{
			if ((e.OldIndex == 0) && (e.NewIndex == e.OldIndex + 1))
			{
				txtEntityName.Text = txtEntityName.Text.Trim();

				//Verify new entity name
				if (!ValidationHelper.ValidCodeIdentifier(txtEntityName.Text))
				{
					MessageBox.Show("The specified name is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					e.Cancel = true;
					return;
				}

				//Verify it does not exist
				if (_model.Entities.Count(x => x.Name.ToLower() == txtEntityName.Text.ToLower()) > 0 ||
					_model.Entities.Count(x => x.PascalName.ToLower() == txtEntityName.Text.ToLower()) > 0)
				{
					MessageBox.Show("The specified name must be unqiue as a model entity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					e.Cancel = true;
					return;
				}
			}
			else if ((e.OldIndex == 1) && (e.NewIndex == e.OldIndex + 1))
			{

			}
			else if ((e.OldIndex == 2) && (e.NewIndex == e.OldIndex + 1))
			{
				//Ensure some fields checked
				if (lstField2.Items.Count == 0)
				{
					MessageBox.Show("There must be at least one field selected for the new Entity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					e.Cancel = true;
					return;
				}
			}

		}

		private void wizard1_AfterSwitchPages(object sender, Wizard.Wizard.AfterSwitchPagesEventArgs e)
		{
			wizard1.FinishEnabled = (e.NewIndex == wizard1.WizardPages.Count - 1);
		}

		private void chkUsePK_CheckedChanged(object sender, EventArgs e)
		{
			if (!chkUsePK.Checked)
			{
				//When NOT checked the rest are disabled
				chkMakeRelation.Checked = false;
				chkMakeRelation.Enabled = false;
				chkPKIdentity.Enabled = false;
			}
			else
			{
				chkMakeRelation.Enabled = true;
				chkPKIdentity.Enabled = true;
			}
		}

		private void chkMakeRelation_CheckedChanged(object sender, EventArgs e)
		{
			if (chkMakeRelation.Checked)
				chkPKIdentity.Checked = false;
			chkPKIdentity.Enabled = !chkMakeRelation.Checked;
		}

		private void chkPKIdentity_CheckedChanged(object sender, EventArgs e)
		{
			if (chkPKIdentity.Checked)
				chkMakeRelation.Checked = false;
			chkMakeRelation.Enabled = !chkPKIdentity.Checked;
		}

		private class DisplayFieldItem
		{
			public Field Field { get; set; }

			public override string ToString()
			{
				return this.Field.Name;
			}
		}

		private void wizard1_Finish(object sender, EventArgs e)
		{
			_diagram.IsLoading = true;
			_model.IsLoading = true;
			try
			{
				using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
				{
					//Save the refactor for later generation
					var refactor = new RefactorTableSplit() { Id = transaction.Id };
					refactor.EntityKey1 = _entity.Id;
					_model.Refactorizations.Add(refactor);

					//Add entity
					var newEntity = new Entity(_model.Partition) { Name = txtEntityName.Text };
					_model.Entities.Add(newEntity);
					refactor.EntityKey2 = newEntity.Id;

					//Add fields to new entity
					var movedFields = new List<Field>();
					foreach (DisplayFieldItem di in lstField2.Items)
					{
						var newField = di.Field.CloneFake();
						newEntity.Fields.Add(newField);
						refactor.ReMappedFieldIDList.Add(di.Field.Id, newField.Id);
						movedFields.Add(di.Field);
					}

					//Add inbound relations to new entity from those moved fields
					var relations = _entity.GetRelationsWhereChild().ToList();

					//If a relation has all fields mapped to the new entity then re-create the relation
					foreach (var relation in relations)
					{
						var count = 0;
						var fieldMap = relation.FieldMapList();
						foreach (var relationField in fieldMap)
						{
							if (movedFields.Contains(relationField.GetTargetField(relation))) count++;
						}

						//All fields are in target table so re-create relation
						if (fieldMap.Count() == count)
						{
							var currentList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
							relation.SourceEntity.ChildEntities.Add(newEntity);
							var updatedList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
							updatedList.RemoveAll(x => currentList.Contains(x));
							var connection = updatedList.First() as EntityHasEntities;
							connection.RoleName = relation.RoleName;

							foreach (var relationField in fieldMap)
							{
								_model.RelationFields.Add(
									new RelationField(_model.Partition)
									{
										SourceFieldId = relationField.GetSourceField(relation).Id,
										TargetFieldId = newEntity.Fields.First(x => x.Id == refactor.ReMappedFieldIDList[relationField.GetTargetField(relation).Id]).Id,
										RelationID = connection.Id
									});
							}

						}

						relation.Delete();

					}

					//Remove fields from original entity
					foreach (DisplayFieldItem di in lstField2.Items)
					{
						_entity.Fields.Remove(di.Field);
					}

					//Add primary key if needed
					if (chkUsePK.Checked)
					{
						foreach (var f in _entity.PrimaryKeyFields)
						{
							var newField = f.CloneFake();
							if (!chkPKIdentity.Checked)
								newField.Identity = IdentityTypeConstants.None;
							newEntity.Fields.Add(newField);

							refactor.ReMappedFieldIDList.Add(f.Id, newField.Id);
						}
					}

					//Create a relation if necessary
					if (chkMakeRelation.Checked)
					{
						var currentList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
						_entity.ChildEntities.Add(newEntity);
						var updatedList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
						updatedList.RemoveAll(x => currentList.Contains(x));
						var connection = updatedList.First() as EntityHasEntities;

						//Add primary keys from both entities
						foreach (var f in _entity.PrimaryKeyFields)
						{
							var newF = newEntity.Fields.FirstOrDefault(x => x.Name == f.Name);
							_model.RelationFields.Add(
								new RelationField(_model.Partition)
								{
									SourceFieldId = f.Id,
									TargetFieldId = newF.Id,
									RelationID = connection.Id
								});
						}

					}

					transaction.Commit();
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				_diagram.IsLoading = false;
				_model.IsLoading = false;
			}

		}

	}
}
