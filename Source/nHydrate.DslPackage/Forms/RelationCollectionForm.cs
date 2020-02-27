#pragma warning disable 0168
using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using nHydrate.Dsl;

namespace nHydrate.DslPackage.Forms
{
	internal partial class RelationCollectionForm : System.Windows.Forms.Form
	{
		#region Class Members

		private nHydrateModel _model = null;
		private Microsoft.VisualStudio.Modeling.Store _store = null;
		private Microsoft.VisualStudio.Modeling.Diagrams.Diagram _diagram = null;
		private EntityShape _entityShape = null;
		private nHydrateDocData _docData = null;
		
		#endregion

		public RelationCollectionForm()
		{
			InitializeComponent();
		}

		public RelationCollectionForm(
			nHydrateModel model, 
			EntityShape entityShape,
			Microsoft.VisualStudio.Modeling.Store store, 
			Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram,
			nHydrateDocData docData)
			: this()
		{
			_model = model;
			_store = store;
			_diagram = diagram;
			_docData = docData;
			_entityShape = entityShape;

			lvwMembers.Columns.Clear();
			lvwMembers.Columns.Add(new ColumnHeader() { Text = "Parent", Width = 200 });
			lvwMembers.Columns.Add(new ColumnHeader() { Text = "Child", Width = 200 });
			lvwMembers.Columns.Add(new ColumnHeader() { Text = "Role", Width = 200 });

			lvwMembers.ListViewItemSorter = new nHydrate.Generator.Common.ListViewItemComparer(0, lvwMembers.Sorting);
			lvwMembers.Sort();

			this.LoadList();
		}

		#region Form Events

		#endregion

		#region Methods

		private void AddItem(EntityAssociationConnector connector)
		{
			var li = new ListViewItem();
			li.Tag = connector;
			li.Text = (connector.FromShape.ModelElement as Entity).Name;

			if (connector.ToShape == null) li.SubItems.Add(string.Empty);
			else li.SubItems.Add((connector.ToShape.ModelElement as Entity).Name);

			li.SubItems.Add(((EntityHasEntities)connector.ModelElement).RoleName);
			lvwMembers.Items.Add(li);
		}

		private void EnableButtons()
		{
			cmdEdit.Enabled = (lvwMembers.SelectedItems.Count >= 0);
			cmdDelete.Enabled = (lvwMembers.SelectedItems.Count >= 0);
		}

		private void LoadList()
		{
			try
			{
				var index = -1;
				if (lvwMembers.SelectedItems.Count > 0)
					index = lvwMembers.SelectedItems[0].Index;

				//Load the relations into the list
				lvwMembers.Items.Clear();

				//Get a list of relations for the current entity only
				var relationshipList = new List<EntityAssociationConnector>();
				foreach (var connector in _diagram.NestedChildShapes.Where(x => x is EntityAssociationConnector).Cast<EntityAssociationConnector>())
				{
					if (connector.FromShape == _entityShape)
					{
						relationshipList.Add(connector);
					}
				}

				foreach (var connector in relationshipList)
				{
					this.AddItem(connector);
				}

				lvwMembers.SelectedItems.Clear();
				if ((0 <= index) && (index < lvwMembers.Items.Count))
					lvwMembers.Items[index].Selected = true;
				else if (index >= 0)
					lvwMembers.Items[lvwMembers.Items.Count - 1].Selected = true;
				else if (lvwMembers.Items.Count != 0)
					lvwMembers.Items[0].Selected = true;
			}
			catch (Exception ex)
			{
				throw;
			}
			this.EnableButtons();
		}

		private bool EditItem()
		{
			if (lvwMembers.SelectedItems.Count == 0) return false;
			var connector = lvwMembers.SelectedItems.Cast<ListViewItem>().FirstOrDefault().Tag as EntityAssociationConnector;
			var F = new nHydrate.DslPackage.Forms.RelationshipDialog(_model, _store, connector.ModelElement as EntityHasEntities);
			if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.LoadList();
				return true;
			}
			return false;
		}

		#endregion

		#region RelationPropertyChanged

		private void RelationPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadList();
		}

		#endregion

		#region Event Handlers

		private void cmdAdd_Click(object sender, EventArgs e)
		{
			using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				_docData.IsImporting = true;
				try
				{
					var parent = _entityShape.ModelElement as Entity;
					var currentList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
					parent.ChildEntities.Add(parent);
					var updatedList = _store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
					updatedList.RemoveAll(x => currentList.Contains(x));

					var connection = updatedList.First() as EntityHasEntities;

					var F = new nHydrate.DslPackage.Forms.RelationshipDialog(_model, _store, connection, true);
					if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
						transaction.Commit();
					}

				}
				catch (Exception ex)
				{
					throw;
				}
				finally
				{
					_docData.IsImporting = false;
				}

				this.LoadList();
			}
		}

		private void cmdEdit_Click(object sender, EventArgs e)
		{
			this.EditItem();
		}

		private void cmdDelete_Click(object sender, EventArgs e)
		{
			var list = new List<ListViewItem>();
			list.AddRange(lvwMembers.SelectedItems.Cast<ListViewItem>());
			foreach (var li in list)
			{
				var connector = li.Tag as EntityAssociationConnector;
				using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
				{
					connector.ModelElement.Delete();
					_diagram.NestedChildShapes.Remove(connector);
					transaction.Commit();
				}
				lvwMembers.Items.Remove(li);
			}
			this.LoadList();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void lvwMembers_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.EnableButtons();
		}

		private void lvwMembers_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13)
			{
				this.EditItem();
			}
		}

		private void lvwMembers_DoubleClick(object sender, EventArgs e)
		{
			this.EditItem();
		}

		#endregion

		private int sortColumn = -1;
		private void lvwMembers_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// Determine whether the column is the same as the last column clicked.
			if (e.Column != sortColumn)
			{
				// Set the sort column to the new column.
				sortColumn = e.Column;
				// Set the sort order to ascending by default.
				lvwMembers.Sorting = SortOrder.Ascending;
			}
			else
			{
				// Determine what the last sort order was and change it.
				if (lvwMembers.Sorting == SortOrder.Ascending)
					lvwMembers.Sorting = SortOrder.Descending;
				else
					lvwMembers.Sorting = SortOrder.Ascending;
			}

			// Call the sort method to manually sort.
			lvwMembers.Sort();
			this.lvwMembers.ListViewItemSorter = new nHydrate.Generator.Common.ListViewItemComparer(e.Column, lvwMembers.Sorting);
		}

	}
}
