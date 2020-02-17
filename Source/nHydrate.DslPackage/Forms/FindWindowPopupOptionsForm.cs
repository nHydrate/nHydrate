using System;
using System.Windows.Forms;

namespace nHydrate.DslPackage.Forms
{
	public partial class FindWindowPopupOptionsForm : Form
	{
		private nHydrate.DslPackage.Forms.FindWindowControl.RefreshDelegate _myMethod;
		private nHydrate.DslPackage.Forms.FindWindowControl.Settings _settings;
		private bool _canUnload = true;

		public FindWindowPopupOptionsForm()
		{
			InitializeComponent();
		}

		public FindWindowPopupOptionsForm(nHydrate.DslPackage.Forms.FindWindowControl.Settings settings, nHydrate.DslPackage.Forms.FindWindowControl.RefreshDelegate myMethod)
			: this()
		{
			_myMethod = myMethod;
			_settings = settings;

			SetupForm();

			this.KeyUp += FindWindowPopupOptionsForm_KeyUp;
			this.Deactivate += FindWindowPopupOptionsForm_Deactivate;
			chkEntity.CheckedChanged += CheckedChanged;
			chkField.CheckedChanged += CheckedChanged;
			chkView.CheckedChanged += CheckedChanged;
		}

		private void SetupForm()
		{
			chkEntity.Checked = _settings.AllowEntity;
			chkField.Checked = _settings.AllowField;
			chkView.Checked = _settings.AllowView;
		}

		private void CheckedChanged(object sender, EventArgs e)
		{
			if (!(chkEntity.Checked || chkView.Checked))
			{
				_canUnload = false;
				MessageBox.Show("At least one object type must be selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				_canUnload = true;
				SetupForm();
				return;
			}

			_settings.AllowEntity = chkEntity.Checked;
			_settings.AllowView = chkView.Checked;
			_settings.AllowField = chkField.Checked;
			_myMethod();
		}

		private void FindWindowPopupOptionsForm_Deactivate(object sender, EventArgs e)
		{
			if (_canUnload)
				this.Close();
		}

		private void FindWindowPopupOptionsForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

	}
}