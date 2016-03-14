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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nHydrate.Dsl;

namespace nHydrate.DslPackage.Forms
{
	public partial class RefactorPreviewCreateAssociativeForm : Form
	{
		private Microsoft.VisualStudio.Modeling.Store _store = null;
		private nHydrateDiagram _diagram = null;
		private nHydrateModel _model = null;
		private Entity _entity1 = null;
		private Entity _entity2 = null;

		public RefactorPreviewCreateAssociativeForm()
		{
			InitializeComponent();
		}

		public RefactorPreviewCreateAssociativeForm(Microsoft.VisualStudio.Modeling.Store store, nHydrateDiagram diagram, nHydrateModel model, Entity entity1, Entity entity2)
			: this()
		{
			_store = store;
			_model = model;
			_diagram = diagram;
			_entity1 = entity1;
			_entity2 = entity2;

			lblEntity1.Text = entity1.Name;
			lblEntity2.Text = entity2.Name;
			txtName.Text = _entity1.Name + _entity2.Name;
		}

		private void cmdApply_Click(object sender, EventArgs e)
		{
			if (!ValidationHelper.ValidCodeIdentifier(txtName.Text))
			{
				MessageBox.Show("The specified name is not a valid identifier.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				_diagram.IsLoading = true;

				//Add entity
				var newEntity = new Entity(_model.Partition) { Name = txtName.Text, IsAssociative = true, AllowAuditTracking = false, AllowCreateAudit = false, AllowModifyAudit = false, AllowTimestamp = false };
				_model.Entities.Add(newEntity);

				//Add relationships
				var currentList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
				_entity1.ChildEntities.Add(newEntity);
				var updatedList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
				updatedList.RemoveAll(x => currentList.Contains(x));
				var connection1 = updatedList.First() as EntityHasEntities;

				currentList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
				_entity2.ChildEntities.Add(newEntity);
				updatedList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
				updatedList.RemoveAll(x => currentList.Contains(x));
				var connection2 = updatedList.First() as EntityHasEntities;

				//Add primary keys from both entities
				foreach (var f in _entity1.PrimaryKeyFields)
				{
					var newF = new Field(_model.Partition) { DataType = f.DataType, IsPrimaryKey = true, Length = f.Length, Name = _entity1.Name + f.Name, };
					newEntity.Fields.Add(newF);

					_model.RelationFields.Add(
						new RelationField(_model.Partition)
						{
							SourceFieldId = f.Id,
							TargetFieldId = newF.Id,
							RelationID = connection1.Id
						});
				}

				foreach (var f in _entity2.PrimaryKeyFields)
				{
					var newF = new Field(_model.Partition) { DataType = f.DataType, IsPrimaryKey = true, Length = f.Length, Name = _entity2.Name + f.Name, };
					newEntity.Fields.Add(newF);

					_model.RelationFields.Add(
						new RelationField(_model.Partition)
						{
							SourceFieldId = f.Id,
							TargetFieldId = newF.Id,
							RelationID = connection2.Id
						});
				}

				transaction.Commit();
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

