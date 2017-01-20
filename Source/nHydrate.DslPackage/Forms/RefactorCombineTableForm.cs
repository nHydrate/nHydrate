#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
using nHydrate.Dsl.Objects;

namespace nHydrate.DslPackage.Forms
{
	public partial class RefactorCombineTableForm : Form
	{
		private Microsoft.VisualStudio.Modeling.Store _store = null;
		private nHydrateDiagram _diagram = null;
		private nHydrateModel _model = null;
		private Entity _entity1 = null;
		private Entity _entity2 = null;

		public RefactorCombineTableForm()
		{
			InitializeComponent();
		}

		public RefactorCombineTableForm(Microsoft.VisualStudio.Modeling.Store store, nHydrateDiagram diagram, nHydrateModel model, Entity entity1, Entity entity2)
			: this()
		{
			_store = store;
			_model = model;
			_diagram = diagram;
			_entity1 = entity1;
			_entity2 = entity2;

			lblEntity1.Text = entity1.Name;
			lblEntity2.Text = entity2.Name;

			var fieldList = new List<Field>();
			fieldList.AddRange(entity1.Fields);
			fieldList.AddRange(entity2.Fields);
			fieldList.Remove(x => entity2.PrimaryKeyFields.Contains(x));

			if (fieldList.Select(x => x.Name.ToLower()).Count() != fieldList.Select(x => x.Name.ToLower()).Distinct().Count())
			{
				cmdApply.Enabled = false;
				lblError.Text = "Duplicate field names are not allowed.";
				lblError.Visible = true;
			}

			fieldList.ForEach(x => lstField.Items.Add(x.Name));
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			_diagram.IsLoading = true;
			_model.IsLoading = true;
			try
			{
				using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
				{
					//Save the refactor for later generation
					var refactor = new RefactorTableCombine() { Id = transaction.Id };
					refactor.EntityKey1 = _entity1.Id;
					refactor.EntityKey2 = _entity2.Id;
					_model.Refactorizations.Add(refactor);

					var fieldList = _entity2.Fields.Where(x => !x.IsPrimaryKey).ToList();
					foreach (var f in fieldList)
					{
						var newField = f.CloneFake();
						_entity1.Fields.Add(newField);
						refactor.ReMappedFieldIDList.Add(f.Id, newField.Id);
					}

					#region Add inbound relations to new entity from those moved fields
					var relations = _entity2.GetRelationsWhereChild().ToList();

					//Map to the new entity re-creating the relation
					foreach (var relation in relations)
					{
						var fieldMap = relation.FieldMapList();
						if (relation.SourceEntity != _entity1) //Make sure we do not add self-ref
						{
							var currentList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
							relation.SourceEntity.ChildEntities.Add(_entity1);
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
										TargetFieldId = _entity1.Fields.First(x => x.Id == refactor.ReMappedFieldIDList[relationField.GetTargetField(relation).Id]).Id,
										RelationID = connection.Id
									});
							}

						}
					}
					#endregion

					#region Add outbound relations to new entity from those moved fields
					relations = _entity2.RelationshipList.ToList();

					//Map to the new entity re-creating the relation
					foreach (var relation in relations)
					{
						var fieldMap = relation.FieldMapList();
						if (relation.TargetEntity != _entity1) //Make sure we do not add self-ref
						{
							var currentList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
							_entity1.ChildEntities.Add(relation.TargetEntity);
							var updatedList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
							updatedList.RemoveAll(x => currentList.Contains(x));
							var connection = updatedList.First() as EntityHasEntities;
							connection.RoleName = relation.RoleName;

							foreach (var relationField in fieldMap)
							{
								_model.RelationFields.Add(
									new RelationField(_model.Partition)
									{
										SourceFieldId = _entity1.PrimaryKeyFields.First().Id,
										TargetFieldId = relationField.GetTargetField(relation).Id,
										RelationID = connection.Id
									});
							}
						}
					}
					#endregion

					_entity2.nHydrateModel.Entities.Remove(_entity2);
					transaction.Commit();
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				_model.IsLoading = false;
				_diagram.IsLoading = false;
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
