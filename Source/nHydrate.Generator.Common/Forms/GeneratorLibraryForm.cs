using System;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.Forms
{
	public partial class GeneratorLibraryForm : Form
	{
		public GeneratorLibraryForm()
		{
			InitializeComponent();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Escape)
				this.Close();
		}

		public bool Populate()
		{
			if (!GeneratorStoreHelper.IsStoreInstalled())
				return false;

			try
			{
				//Ensure there is something creatable and if so add the control to this form
				var typeList = nHydrate.Generator.Common.Util.ReflectionHelper.GetCreatableObjectImplementsInterface(
					typeof(nHydrate.Generator.Common.GeneratorFramework.IGeneratorStore),
					nHydrate.Generator.Common.GeneratorFramework.AddinAppData.Instance.ExtensionDirectory);

				var t = typeList.FirstOrDefault();
				if (t == null)
				{
					return false;
				}

				var ctrl = (nHydrate.Generator.Common.GeneratorFramework.IGeneratorStore)nHydrate.Generator.Common.Util.ReflectionHelper.CreateInstance(t) as UserControl;
				if (ctrl == null)
				{
					return false;
				}

				this.Controls.Add(ctrl);
				((nHydrate.Generator.Common.GeneratorFramework.IGeneratorStore)ctrl).OnInstallComplete += new EventHandler(LibraryInstallComplete);
				((nHydrate.Generator.Common.GeneratorFramework.IGeneratorStore)ctrl).SetDTE(EnvDTEHelper.Instance.ApplicationObject);
				((nHydrate.Generator.Common.GeneratorFramework.IGeneratorStore)ctrl).LoadUI();
				ctrl.Dock = DockStyle.Fill;
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		private void LibraryInstallComplete(object sender, System.EventArgs e)
		{
			this.Close();
		}

	}
}
