using System;
using System.ComponentModel;
using System.Windows.Forms;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.ModelUI
{
	public partial class ModelRootControllerUIControl : UserControl
	{
		private ModelRoot _model = null;

		public ModelRootControllerUIControl()
		{
			InitializeComponent();

			foreach (var v in Enum.GetNames(typeof(SQLServerTypeConstants)))
				cboSQLServerType.Items.Add(v);
		}

		public void Populate(ModelRoot model)
		{
			_model = model;
			_model.PropertyChanged += new PropertyChangedEventHandler(_model_PropertyChanged);
			this.LoadControls();

			#region Hook events
			txtCompanyName.Leave += new EventHandler(txtCompanyName_Leave);
			txtCompanyName.Enter += new EventHandler(txtCompanyName_Enter);
			txtCompanyName.KeyDown += new KeyEventHandler(txtCompanyName_KeyDown);

			txtCopyright.Leave += new EventHandler(txtCopyright_Leave);
			txtCopyright.Enter += new EventHandler(txtCopyright_Enter);
			txtCopyright.KeyDown += new KeyEventHandler(txtCopyright_KeyDown);

			txtDefaultNamespace.Leave += new EventHandler(txtDefaultNamespace_Leave);
			txtDefaultNamespace.Enter += new EventHandler(txtDefaultNamespace_Enter);
			txtDefaultNamespace.KeyDown += new KeyEventHandler(txtDefaultNamespace_KeyDown);

			txtProjectName.Leave += new EventHandler(txtProjectName_Leave);
			txtProjectName.Enter += new EventHandler(txtProjectName_Enter);
			txtProjectName.KeyDown += new KeyEventHandler(txtProjectName_KeyDown);

			txtStoredProcPrefix.Leave += new EventHandler(txtStoredProcPrefix_Leave);
			txtStoredProcPrefix.Enter += new EventHandler(txtStoredProcPrefix_Enter);
			txtStoredProcPrefix.KeyDown += new KeyEventHandler(txtStoredProcPrefix_KeyDown);

			txtVersion.Leave += new EventHandler(txtVersion_Leave);
			txtVersion.Enter += new EventHandler(txtVersion_Enter);
			txtVersion.KeyDown += new KeyEventHandler(txtVersion_KeyDown);

			chkCustomChangeEvents.CheckStateChanged += new EventHandler(chkCustomChangeEvents_CheckStateChanged);
			chkSupprtLegacySearch.CheckStateChanged += new EventHandler(chkSupprtLegacySearch_CheckStateChanged);
			chkTransformNames.CheckStateChanged += new EventHandler(chkTransformNames_CheckStateChanged);
			chkUseUTC.CheckStateChanged += new EventHandler(chkUseUTC_CheckStateChanged);
			#endregion

			#region Setup Tooltips
			toolTip1.SetToolTip(txtCompanyName, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "CompanyName"));
			toolTip1.SetToolTip(txtCopyright, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "Copyright"));
			toolTip1.SetToolTip(txtDefaultNamespace, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "DefaultNamespace"));
			toolTip1.SetToolTip(txtProjectName, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "ProjectName"));
			toolTip1.SetToolTip(txtStoredProcPrefix, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "StoredProcedurePrefix"));
			toolTip1.SetToolTip(txtVersion, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "Version"));
			toolTip1.SetToolTip(chkCustomChangeEvents, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "EnableCustomChangeEvents"));
			toolTip1.SetToolTip(chkSupprtLegacySearch, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "SupportLegacySearchObject"));
			toolTip1.SetToolTip(chkTransformNames, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "TransformNames"));
			toolTip1.SetToolTip(chkUseUTC, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "UseUTCTime"));
			toolTip1.SetToolTip(cboSQLServerType, ReflectionHelper.GetPropertyAttributeDescriptionValue(_model, "SQLServerType"));
			#endregion

		}

		private void LoadControls()
		{
			txtCompanyName.Text = _model.CompanyName;
			txtCopyright.Text = _model.Copyright;
			txtDefaultNamespace.Text = _model.DefaultNamespace;
			txtProjectName.Text = _model.ProjectName;
			txtStoredProcPrefix.Text = _model.StoredProcedurePrefix;
			txtVersion.Text = _model.Version;

			chkCustomChangeEvents.Checked = _model.EnableCustomChangeEvents;
			chkSupprtLegacySearch.Checked = _model.SupportLegacySearchObject;
			chkTransformNames.Checked = _model.TransformNames;
			chkUseUTC.Checked = _model.UseUTCTime;

			cboSQLServerType.SelectedItem = _model.SQLServerType.ToString();
		}

		private void _model_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadControls();
		}

		private void txtVersion_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
			//textBox1.Text += e.Alt + " / " + e.Control + " / " + e.KeyCode + " / " + e.KeyValue + " / " + e.Shift + " / " + e.SuppressKeyPress + "\r\n";
			//else
			//{
			//  if (e.KeyCode == Keys.Tab)
			//  {
			//    this.SelectNextControl(sender as Control, true, true, true, true);
			//    e.Handled = true;
			//  }
			//}
		}

		private void txtVersion_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _model.Version;
		}

		private void txtVersion_Leave(object sender, EventArgs e)
		{
			if (_model.Version != ((TextBox)sender).Text)
				_model.Version = ((TextBox)sender).Text;
		}

		private void txtStoredProcPrefix_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtStoredProcPrefix_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _model.StoredProcedurePrefix;
		}

		private void txtStoredProcPrefix_Leave(object sender, EventArgs e)
		{
			if (_model.StoredProcedurePrefix != ((TextBox)sender).Text)
				_model.StoredProcedurePrefix = ((TextBox)sender).Text;
		}

		private void txtProjectName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtProjectName_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _model.ProjectName;
		}

		private void txtProjectName_Leave(object sender, EventArgs e)
		{
			if (_model.ProjectName != ((TextBox)sender).Text)
				_model.ProjectName = ((TextBox)sender).Text;
		}

		private void txtDefaultNamespace_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtDefaultNamespace_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _model.DefaultNamespace;
		}

		private void txtDefaultNamespace_Leave(object sender, EventArgs e)
		{
			if (_model.DefaultNamespace != ((TextBox)sender).Text)
				_model.DefaultNamespace = ((TextBox)sender).Text;
		}

		private void txtCopyright_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtCopyright_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _model.Copyright;
		}

		private void txtCopyright_Leave(object sender, EventArgs e)
		{
			if (_model.Copyright != ((TextBox)sender).Text)
				_model.Copyright = ((TextBox)sender).Text;
		}

		private void txtCompanyName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtCompanyName_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _model.CompanyName;
		}

		private void txtCompanyName_Leave(object sender, EventArgs e)
		{
			if (_model.CompanyName != ((TextBox)sender).Text)
				_model.CompanyName = ((TextBox)sender).Text;
		}

		private void chkUseUTC_CheckStateChanged(object sender, EventArgs e)
		{
			_model.UseUTCTime = ((CheckBox)sender).Checked;
		}

		private void chkTransformNames_CheckStateChanged(object sender, EventArgs e)
		{
			_model.TransformNames = ((CheckBox)sender).Checked;
		}

		private void chkSupprtLegacySearch_CheckStateChanged(object sender, EventArgs e)
		{
			_model.SupportLegacySearchObject = ((CheckBox)sender).Checked;
		}

		private void chkCustomChangeEvents_CheckStateChanged(object sender, EventArgs e)
		{
			_model.EnableCustomChangeEvents = ((CheckBox)sender).Checked;
		}


	}
}