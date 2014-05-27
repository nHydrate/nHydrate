using System;
using System.ComponentModel;
using System.Windows.Forms;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.ModelUI
{
	public partial class FieldControllerUIControl : UserControl
	{
		private nHydrate.Generator.Models.Column _column = null;

		public FieldControllerUIControl()
		{
			InitializeComponent();

			foreach (var v in Enum.GetNames(typeof(System.Data.SqlDbType)))
				cboDataType.Items.Add(v);

			foreach (var v in Enum.GetNames(typeof(nHydrate.Generator.Models.IdentityTypeConstants)))
				cboIdentityType.Items.Add(v);

		}

		public void Populate(nHydrate.Generator.Models.Column column)
		{
			_column = column;
			_column.PropertyChanged += new PropertyChangedEventHandler(_column_PropertyChanged);
			this.LoadControls();

			#region Hook events
			txtCodeFacade.Leave += new EventHandler(txtCodeFacade_Leave);
			txtCodeFacade.Enter += new EventHandler(txtCodeFacade_Enter);
			txtCodeFacade.KeyDown += new KeyEventHandler(txtCodeFacade_KeyDown);

			txtCollate.Leave += new EventHandler(txtCollate_Leave);
			txtCollate.Enter += new EventHandler(txtCollate_Enter);
			txtCollate.KeyDown += new KeyEventHandler(txtCollate_KeyDown);

			txtDefault.Leave += new EventHandler(txtDefault_Leave);
			txtDefault.Enter += new EventHandler(txtDefault_Enter);
			txtDefault.KeyDown += new KeyEventHandler(txtDefault_KeyDown);

			txtDescription.Leave += new EventHandler(txtDescription_Leave);
			txtDescription.Enter += new EventHandler(txtDescription_Enter);
			txtDescription.KeyDown += new KeyEventHandler(txtDescription_KeyDown);

			txtFormula.Leave += new EventHandler(txtFormula_Leave);
			txtFormula.Enter += new EventHandler(txtFormula_Enter);
			txtFormula.KeyDown += new KeyEventHandler(txtFormula_KeyDown);

			txtName.Leave += new EventHandler(txtName_Leave);
			txtName.Enter += new EventHandler(txtName_Enter);
			txtName.KeyDown += new KeyEventHandler(txtName_KeyDown);

			txtMax.Leave += new EventHandler(txtMax_Leave);
			txtMax.Enter += new EventHandler(txtMax_Enter);
			txtMax.KeyDown += new KeyEventHandler(txtMax_KeyDown);

			txtMin.Leave += new EventHandler(txtMin_Leave);
			txtMin.Enter += new EventHandler(txtMin_Enter);
			txtMin.KeyDown += new KeyEventHandler(txtMin_KeyDown);

			chkComputedColumn.CheckedChanged += new EventHandler(chkComputedColumn_CheckedChanged);
			chkGenerated.CheckedChanged += new EventHandler(chkGenerated_CheckedChanged);
			chkIsIndexed.CheckedChanged += new EventHandler(chkIsIndexed_CheckedChanged);
			chkIsNull.CheckedChanged += new EventHandler(chkIsNull_CheckedChanged);
			chkIsPrimaryKey.CheckedChanged += new EventHandler(chkIsPrimaryKey_CheckedChanged);
			chkIsSearchable.CheckedChanged += new EventHandler(chkIsSearchable_CheckedChanged);
			chkIsUnique.CheckedChanged += new EventHandler(chkIsUnique_CheckedChanged);

			#endregion

			#region Setup Tooltips
			toolTip1.SetToolTip(txtCodeFacade, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "CodeFacade"));
			toolTip1.SetToolTip(txtCollate, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "Collate"));
			toolTip1.SetToolTip(txtDefault, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "Default"));
			toolTip1.SetToolTip(txtDescription, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "Description"));
			toolTip1.SetToolTip(txtFormula, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "Formula"));
			toolTip1.SetToolTip(txtMax, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "Min"));
			toolTip1.SetToolTip(txtMin, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "Max"));
			toolTip1.SetToolTip(txtName, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "Name"));
			toolTip1.SetToolTip(chkComputedColumn, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "ComputedColumn"));
			toolTip1.SetToolTip(chkGenerated, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "Generated"));
			toolTip1.SetToolTip(chkIsIndexed, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "IsIndexed"));
			toolTip1.SetToolTip(chkIsNull, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "AllowNull"));
			toolTip1.SetToolTip(chkIsPrimaryKey, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "PrimaryKey"));
			toolTip1.SetToolTip(chkIsSearchable, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "IsSearchable"));
			toolTip1.SetToolTip(chkIsUnique, ReflectionHelper.GetPropertyAttributeDescriptionValue(_column, "IsUnique"));
			#endregion

		}

		private void chkIsUnique_CheckedChanged(object sender, EventArgs e)
		{
			if (_column.IsUnique != ((CheckBox)sender).Checked)
			_column.IsUnique = ((CheckBox)sender).Checked;
		}

		private void chkIsSearchable_CheckedChanged(object sender, EventArgs e)
		{
			if (_column.IsSearchable != ((CheckBox)sender).Checked)
			_column.IsSearchable = ((CheckBox)sender).Checked;
		}

		private void chkIsPrimaryKey_CheckedChanged(object sender, EventArgs e)
		{
			if (_column.PrimaryKey != ((CheckBox)sender).Checked)
			_column.PrimaryKey = ((CheckBox)sender).Checked;
		}

		private void chkIsNull_CheckedChanged(object sender, EventArgs e)
		{
			if (_column.AllowNull != ((CheckBox)sender).Checked)
			_column.AllowNull = ((CheckBox)sender).Checked;
		}

		private void chkIsIndexed_CheckedChanged(object sender, EventArgs e)
		{
			if (_column.IsIndexed != ((CheckBox)sender).Checked)
			_column.IsIndexed = ((CheckBox)sender).Checked;
		}

		private void chkGenerated_CheckedChanged(object sender, EventArgs e)
		{
			if (_column.Generated != ((CheckBox)sender).Checked)
			_column.Generated = ((CheckBox)sender).Checked;
		}

		private void chkComputedColumn_CheckedChanged(object sender, EventArgs e)
		{
			if (_column.ComputedColumn != ((CheckBox)sender).Checked)
			_column.ComputedColumn = ((CheckBox)sender).Checked;
		}

		private void txtMin_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtMin_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _column.Min;
		}

		private void txtMin_Leave(object sender, EventArgs e)
		{
			int i;
			if (int.TryParse(((TextBox)sender).Text, out i))
			{
				if (_column.Min != i)
					_column.Min = i;
			}
			else
			{
				((TextBox)sender).Tag = _column.Min;
			}
		}

		private void txtMax_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtMax_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _column.Max;
		}

		private void txtMax_Leave(object sender, EventArgs e)
		{
			int i;
			if (int.TryParse(((TextBox)sender).Text, out i))
			{
				if (_column.Max != i)
					_column.Max = i;
			}
			else
			{
				((TextBox)sender).Tag = _column.Max;
			}
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
			((TextBox)sender).Tag = _column.Name;
		}

		private void txtName_Leave(object sender, EventArgs e)
		{
			if (_column.Name != ((TextBox)sender).Text)
				_column.Name = ((TextBox)sender).Text;
		}

		private void txtFormula_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtFormula_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _column.Formula;
		}

		private void txtFormula_Leave(object sender, EventArgs e)
		{
			if (_column.Formula != ((TextBox)sender).Text)
				_column.Formula = ((TextBox)sender).Text;
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
			((TextBox)sender).Tag = _column.Description;
		}

		private void txtDescription_Leave(object sender, EventArgs e)
		{
			if (_column.Description != ((TextBox)sender).Text)
				_column.Description = ((TextBox)sender).Text;
		}

		private void txtDefault_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtDefault_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _column.Default;
		}

		private void txtDefault_Leave(object sender, EventArgs e)
		{
			if (_column.Default != ((TextBox)sender).Text)
				_column.Default = ((TextBox)sender).Text;
		}

		private void txtCollate_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtCollate_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _column.Collate;
		}

		private void txtCollate_Leave(object sender, EventArgs e)
		{
			if (_column.Collate != ((TextBox)sender).Text)
				_column.Collate = ((TextBox)sender).Text;
		}

		private void LoadControls()
		{
			txtCodeFacade.Text = _column.CodeFacade;
			txtDescription.Text = _column.Description;
			txtFormula.Text = _column.Formula;
			txtMax.Text = _column.Max.ToString();
			txtMin.Text = _column.Min.ToString();
			txtName.Text = _column.Name;
			txtDefault.Text = _column.Default;
			txtCollate.Text = _column.Collate;

			chkComputedColumn.Checked = _column.ComputedColumn;
			chkGenerated.Checked = _column.Generated;
			chkIsIndexed.Checked = _column.IsIndexed;
			chkIsNull.Checked = _column.AllowNull;
			chkIsPrimaryKey.Checked = _column.PrimaryKey;
			chkIsSearchable.Checked = _column.IsSearchable;
			chkIsUnique.Checked = _column.IsUnique;

			cboDataType.SelectedItem = _column.DataType.ToString();
			cboIdentityType.SelectedItem = _column.Identity.ToString();

			udLength.Value = _column.Length;
			udScale.Value = _column.Scale;
		}

		private void _column_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadControls();
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
			((TextBox)sender).Tag = _column.CodeFacade;
		}

		private void txtCodeFacade_Leave(object sender, EventArgs e)
		{
			if (_column.CodeFacade != ((TextBox)sender).Text)
				_column.CodeFacade = ((TextBox)sender).Text;

		}

	}
}