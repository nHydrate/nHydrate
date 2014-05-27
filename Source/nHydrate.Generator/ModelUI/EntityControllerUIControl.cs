using System;
using System.ComponentModel;
using System.Windows.Forms;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.ModelUI
{
	public partial class EntityControllerUIControl : UserControl
	{

		private nHydrate.Generator.Models.Table _table = null;

		public EntityControllerUIControl()
		{
			InitializeComponent();
		}

		public void Populate(nHydrate.Generator.Models.Table table)
		{
			_table = table;
			_table.PropertyChanged += new PropertyChangedEventHandler(_table_PropertyChanged);
			this.LoadControls();

			#region Hook events
			txtCodeFacade.Leave += new EventHandler(txtCodeFacade_Leave);
			txtCodeFacade.Enter += new EventHandler(txtCodeFacade_Enter);
			txtCodeFacade.KeyDown += new KeyEventHandler(txtCodeFacade_KeyDown);

			txtDatabaseSchema.Leave += new EventHandler(txtDatabaseSchema_Leave);
			txtDatabaseSchema.Enter += new EventHandler(txtDatabaseSchema_Enter);
			txtDatabaseSchema.KeyDown += new KeyEventHandler(txtDatabaseSchema_KeyDown);

			txtDescription.Leave += new EventHandler(txtDescription_Leave);
			txtDescription.Enter += new EventHandler(txtDescription_Enter);
			txtDescription.KeyDown += new KeyEventHandler(txtDescription_KeyDown);

			txtName.Leave += new EventHandler(txtName_Leave);
			txtName.Enter += new EventHandler(txtName_Enter);
			txtName.KeyDown += new KeyEventHandler(txtName_KeyDown);

			chkAllowAuditTracking.CheckedChanged += new EventHandler(chkAllowAuditTracking_CheckedChanged);
			chkAllowCreateAudit.CheckedChanged += new EventHandler(chkAllowCreateAudit_CheckedChanged);
			chkAllowModifyAudit.CheckedChanged += new EventHandler(chkAllowModifyAudit_CheckedChanged);
			chkAllowTimestamp.CheckedChanged += new EventHandler(chkAllowTimestamp_CheckedChanged);
			chkEnforcePrimaryKey.CheckedChanged += new EventHandler(chkEnforcePrimaryKey_CheckedChanged);
			chkGenerated.CheckedChanged += new EventHandler(chkGenerated_CheckedChanged);
			chkImmutable.CheckedChanged += new EventHandler(chkImmutable_CheckedChanged);
			chkIsAssociative.CheckedChanged += new EventHandler(chkIsAssociative_CheckedChanged);
			chkIsTypeTable.CheckedChanged += new EventHandler(chkIsTypeTable_CheckedChanged);

			#endregion

			#region Setup Tooltips
			toolTip1.SetToolTip(txtName, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "Name"));
			toolTip1.SetToolTip(txtDescription, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "Description"));
			toolTip1.SetToolTip(txtCodeFacade, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "CodeFacade"));
			toolTip1.SetToolTip(txtDatabaseSchema, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "DBSchema"));
			toolTip1.SetToolTip(chkAllowAuditTracking, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "AllowAuditTracking"));
			toolTip1.SetToolTip(chkAllowCreateAudit, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "AllowCreateAudit"));
			toolTip1.SetToolTip(chkAllowModifyAudit, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "AllowModifiedAudit"));
			toolTip1.SetToolTip(chkAllowTimestamp, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "AllowTimestamp"));
			toolTip1.SetToolTip(chkEnforcePrimaryKey, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "EnforcePrimaryKey"));
			toolTip1.SetToolTip(chkGenerated, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "Generated"));
			toolTip1.SetToolTip(chkImmutable, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "Immutable"));
			toolTip1.SetToolTip(chkIsAssociative, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "AssociativeTable"));
			toolTip1.SetToolTip(chkIsTypeTable, ReflectionHelper.GetPropertyAttributeDescriptionValue(_table, "IsTypeTable"));
			#endregion

		}

		private void LoadControls()
		{
			txtName.Text = _table.Name;
			txtDescription.Text = _table.Description;
			txtCodeFacade.Text = _table.CodeFacade;
			txtDatabaseSchema.Text = _table.DBSchema;

			chkAllowAuditTracking.Checked = _table.AllowAuditTracking;
			chkAllowCreateAudit.Checked = _table.AllowCreateAudit;
			chkAllowModifyAudit.Checked = _table.AllowModifiedAudit;
			chkAllowTimestamp.Checked = _table.AllowTimestamp;
			chkEnforcePrimaryKey.Checked = _table.EnforcePrimaryKey;
			chkGenerated.Checked = _table.Generated;
			chkImmutable.Checked = _table.Immutable;
			chkIsAssociative.Checked = _table.AssociativeTable;
			chkIsTypeTable.Checked = _table.IsTypeTable;
		}

		private void _table_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadControls();
		}

		private void txtName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtName_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _table.Name;
		}

		private void txtName_Leave(object sender, EventArgs e)
		{
			if (_table.Name != ((TextBox)sender).Text)
				_table.Name = ((TextBox)sender).Text;
		}

		private void txtDescription_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtDescription_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _table.Description;
		}

		private void txtDescription_Leave(object sender, EventArgs e)
		{
			if (_table.Description != ((TextBox)sender).Text)
				_table.Description = ((TextBox)sender).Text;
		}

		private void txtDatabaseSchema_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtDatabaseSchema_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _table.DBSchema;
		}

		private void txtDatabaseSchema_Leave(object sender, EventArgs e)
		{
			if (_table.DBSchema != ((TextBox)sender).Text)
				_table.DBSchema = ((TextBox)sender).Text;
		}

		private void txtCodeFacade_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtCodeFacade_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _table.CodeFacade;
		}

		private void txtCodeFacade_Leave(object sender, EventArgs e)
		{
			if (_table.CodeFacade != ((TextBox)sender).Text)
				_table.CodeFacade = ((TextBox)sender).Text;

		}

		private void chkIsTypeTable_CheckedChanged(object sender, EventArgs e)
		{
			if (_table.IsTypeTable != ((CheckBox)sender).Checked)
				_table.IsTypeTable = ((CheckBox)sender).Checked;
		}

		private void chkIsAssociative_CheckedChanged(object sender, EventArgs e)
		{
			if (_table.AssociativeTable != ((CheckBox)sender).Checked)
				_table.AssociativeTable = ((CheckBox)sender).Checked;
		}

		private void chkImmutable_CheckedChanged(object sender, EventArgs e)
		{
			if (_table.Immutable != ((CheckBox)sender).Checked)
				_table.Immutable = ((CheckBox)sender).Checked;
		}

		private void chkGenerated_CheckedChanged(object sender, EventArgs e)
		{
			if (_table.Generated != ((CheckBox)sender).Checked)
				_table.Generated = ((CheckBox)sender).Checked;
		}

		private void chkEnforcePrimaryKey_CheckedChanged(object sender, EventArgs e)
		{
			if (_table.EnforcePrimaryKey != ((CheckBox)sender).Checked)
				_table.EnforcePrimaryKey = ((CheckBox)sender).Checked;
		}

		private void chkAllowTimestamp_CheckedChanged(object sender, EventArgs e)
		{
			if (_table.AllowTimestamp != ((CheckBox)sender).Checked)
				_table.AllowTimestamp = ((CheckBox)sender).Checked;
		}

		private void chkAllowModifyAudit_CheckedChanged(object sender, EventArgs e)
		{
			if (_table.AllowModifiedAudit != ((CheckBox)sender).Checked)
				_table.AllowModifiedAudit = ((CheckBox)sender).Checked;
		}

		private void chkAllowCreateAudit_CheckedChanged(object sender, EventArgs e)
		{
			if (_table.AllowCreateAudit != ((CheckBox)sender).Checked)
				_table.AllowCreateAudit = ((CheckBox)sender).Checked;
		}

		private void chkAllowAuditTracking_CheckedChanged(object sender, EventArgs e)
		{
			if (_table.AllowAuditTracking != ((CheckBox)sender).Checked)
				_table.AllowAuditTracking = ((CheckBox)sender).Checked;
		}

	}
}