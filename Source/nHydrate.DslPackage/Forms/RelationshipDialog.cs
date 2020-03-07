#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Dsl;
using nHydrate.Generator.Common.Util;

namespace nHydrate.DslPackage.Forms
{
	public partial class RelationshipDialog : Form
	{
		public RelationshipDialog()
		{
			InitializeComponent();

			lvwColumns.Columns.Clear();
			lvwColumns.Columns.Add(string.Empty, lblSecondaryTable.Width, HorizontalAlignment.Left);
			lvwColumns.Columns.Add(string.Empty, lblSecondaryTable.Width, HorizontalAlignment.Left);
			cboChildTable.Location = lblSecondaryTable.Location;
			pnlCover.Location = new Point(16, 133);
			cboChildTable.SelectedValueChanged += new EventHandler(cboChildTable_SelectedValueChanged);
		}

		private EntityHasEntities _connector = null;
		private nHydrateModel _model = null;
		private Microsoft.VisualStudio.Modeling.Store _store = null;
		private bool _allowConfigure = false;

		public RelationshipDialog(nHydrateModel model, Microsoft.VisualStudio.Modeling.Store store, EntityHasEntities connector)
			: this(model, store, connector, false)
		{
		}

		public RelationshipDialog(nHydrateModel model, Microsoft.VisualStudio.Modeling.Store store, EntityHasEntities connector, bool allowConfigure)
			: this()
		{
			try
			{
				_connector = connector;
				_model = model;
				_store = store;
				_allowConfigure = allowConfigure;

				//Load the Form
				var parent = connector.ParentEntity;
				lblPrimaryTable.Text = parent.Name;
				
				if (!allowConfigure)
					lblSecondaryTable.Text = connector.ChildEntity.Name;

				LoadRelation();

				if (_allowConfigure)
				{
					cboChildTable.Items.Clear();
					foreach (var entity in _model.Entities.OrderBy(x => x.Name))
					{
						cboChildTable.Items.Add(entity.Name);
					}

					lblSecondaryTable.Visible = false;
					cboChildTable.Visible = true;
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#region Child Control Event Handlers

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			if (this.lvwColumns.Items.Count == 0)
			{
				MessageBox.Show("You must specify at least one set of key mappings.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var parent = _connector.ParentEntity;
			var child = _connector.ChildEntity;
			if (_allowConfigure)
			{
				child = _model.Entities.FirstOrDefault(x => x.Name == (string)cboChildTable.SelectedItem);
			}

			var relationId = Guid.Empty;
			relationId = _connector.Id;

			//Verify that they did not link the same two columns more than once
			var inError = false;
			var colList1 = new List<Guid>();
			var colList2 = new List<Guid>();
			foreach (ListViewItem item in this.lvwColumns.Items)
			{
				var parentField = parent.Fields.FirstOrDefault(x => x.Name == item.SubItems[0].Text);
				var childField = child.Fields.FirstOrDefault(x => x.Name == item.SubItems[1].Text);

				if (colList1.Contains(parentField.Id)) inError = true;
				else colList1.Add(parentField.Id);

				if (colList2.Contains(childField.Id)) inError = true;
				else colList2.Add(childField.Id);
			}

			if (inError)
			{
				MessageBox.Show("The relation is invalid. All source and target fields within a relation must be unqiue.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			//Actual make the change to the model
			using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				//Save
				_connector.IsEnforced = chkEnforce.Checked;
				_connector.RoleName = txtRole.Text;

				if (_allowConfigure)
				{
					_connector.ChildEntity = child;
				}

				////Remove all fields
				//var fieldList = _model.RelationFields.Where(x => x.RelationID == relationId).ToList();
				//foreach (var columnSet in fieldList)
				//{
				//  _model.RelationFields.Remove(columnSet);
				//}
				_model.RelationFields.RemoveAll(_model.RelationFields.Where(x => x.RelationID == relationId).ToList());

				foreach (ListViewItem item in this.lvwColumns.Items)
				{
					var parentField = parent.Fields.FirstOrDefault(x => x.Name == item.SubItems[0].Text);
					var childField = child.Fields.FirstOrDefault(x => x.Name == item.SubItems[1].Text);
					_model.RelationFields.Add(
						new RelationField(_model.Partition)
						{
							SourceFieldId = parentField.Id,
							TargetFieldId = childField.Id,
							RelationID = relationId,
						}
						);
				}

				transaction.Commit();
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void cmdAdd_Click(object sender, System.EventArgs e)
		{
			if ((!string.IsNullOrEmpty(cboParentField.Text)) && (!string.IsNullOrEmpty(cboChildField.Text)))
			{
				this.AddColumnMap(cboParentField.Text, cboChildField.Text);
			}
		}

		private void cmdDelete_Click(object sender, System.EventArgs e)
		{
			if (this.lvwColumns.SelectedItems.Count > 0)
			{
				for (var ii = this.lvwColumns.SelectedItems.Count - 1; ii >= 0; ii--)
				{
					this.lvwColumns.Items.Remove(this.lvwColumns.SelectedItems[ii]);
				}
			}
		}

		private void cboChildField_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.EnableButtons();
		}

		private void cboParentField_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//Default the child box to the same field name if not set and if exists
			if ((cboParentField.SelectedIndex != -1) && (cboChildField.SelectedIndex == -1))
			{
				foreach (string s in cboChildField.Items)
				{
					if (s.Match(cboParentField.SelectedItem.ToString()))
						cboChildField.SelectedItem = s;
				}
			}

			this.EnableButtons();
		}

		private void lvwColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.EnableButtons();
		}

		#endregion

		#region Methods

		private void LoadFields(string tableName, ComboBox cboField)
		{
			cboField.Items.Clear();
			var entity = _model.Entities.FirstOrDefault(x => x.Name == tableName);
			if (entity != null)
			{
				foreach (var field in entity.Fields)
				{
					cboField.Items.Add(field.Name);
				}
			}
			cboField.Enabled = (cboField.Items.Count > 0);
		}

		private void AddColumnMap(string field1, string field2)
		{
			var newItem = new ListViewItem();
			newItem.Text = field1;
			newItem.SubItems.Add(field2);
			this.lvwColumns.Items.Add(newItem);
		}

		private void EnableButtons()
		{
			cmdAdd.Enabled = ((!string.IsNullOrEmpty(cboParentField.Text)) && (!string.IsNullOrEmpty(cboChildField.Text)));
			cmdDelete.Enabled = (this.lvwColumns.SelectedItems.Count > 0);
		}

		private void LoadRelation()
		{
			var parent = _connector.ParentEntity;
			var child = _model.Entities.FirstOrDefault(x => x.Name == lblSecondaryTable.Text);

			this.LoadFields(lblPrimaryTable.Text, cboParentField);
			this.LoadFields(lblSecondaryTable.Text, cboChildField);

			//Load fields that are set
			var relationId = Guid.Empty;
			relationId = _connector.Id;

			var relation = _model.AllRelations.FirstOrDefault(x => x.Id == relationId);
			var fieldList = _model.RelationFields.Where(x => x.RelationID == relationId);
			foreach (var columnSet in fieldList)
			{
				var field1 = parent.Fields.FirstOrDefault(x => x.Id == columnSet.SourceFieldId);
				Field field2 = null;
				if (child != null)
					field2 = child.Fields.FirstOrDefault(x => x.Id == columnSet.TargetFieldId);

				if (field1 != null && field2 != null)
				{
					this.AddColumnMap(field1.Name, field2.Name);
					foreach (string s in cboParentField.Items)
					{
						if (s == field1.Name) cboParentField.SelectedItem = s;
					}

					foreach (string s in cboChildField.Items)
					{
						if (s == field2.Name) cboChildField.SelectedItem = s;
					}
				}
			}

			//New relation, nothing selected, so default to PK
			if (cboParentField.SelectedIndex == -1)
			{
				var pk = parent.PrimaryKeyFields.FirstOrDefault();
				if (pk != null)
				{
					cboParentField.SelectedItem = pk.Name;
				}
			}

			chkEnforce.Checked = _connector.IsEnforced;
			txtRole.Text = _connector.RoleName;
		}

		private void cboChildTable_SelectedValueChanged(object sender, EventArgs e)
		{
			this.lvwColumns.Items.Clear();
			lblSecondaryTable.Text = (string)cboChildTable.SelectedItem;
			this.LoadRelation();
		}

		#endregion

	}
}
