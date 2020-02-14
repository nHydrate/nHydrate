using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace nHydrate.DslPackage.Forms
{
	public partial class DeleteModeObjectPrompt : Form
	{
		public DeleteModeObjectPrompt()
		{
			InitializeComponent();
		}

		public DeleteModeObjectPrompt(List<string> list)
			: this()
		{
			foreach (var s in list)
				lstItem.Items.Add(s);
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
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

