namespace PROJECTNAMESPACE
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;

	internal partial class InstallSettingsUI : UserControl
	{
		public InstallSettingsUI()
		{
			InitializeComponent();
		}

		public void LoadUI(InstallSetup setup)
		{
			chkIgnoreWarnings.Checked = setup.AcceptVersionWarningsChangedScripts && setup.AcceptVersionWarningsNewScripts;
			chkSkipNormalize.Checked = setup.SkipNormalize;
			chkUseTransaction.Checked = setup.UseTransaction;
			chkUseHash.Checked = setup.UseHash;
		}

		public void SaveUI(InstallSetup setup)
		{
			setup.AcceptVersionWarningsChangedScripts = chkIgnoreWarnings.Checked;
			setup.AcceptVersionWarningsNewScripts = chkIgnoreWarnings.Checked;
			setup.SkipNormalize = chkSkipNormalize.Checked;
			setup.UseTransaction = chkUseTransaction.Checked;
			setup.UseHash = chkUseHash.Checked;
		}

		private void cmdHelp_Click(object sender, EventArgs e)
		{
			DatabaseInstaller.ShowHelp();
		}

	}
}
