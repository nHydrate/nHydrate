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
	public partial class OptionsForm : Form
	{
		private ModelerOptions options = new ModelerOptions();

		public OptionsForm()
		{
			InitializeComponent();
			options.Load();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			options.Save();
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

