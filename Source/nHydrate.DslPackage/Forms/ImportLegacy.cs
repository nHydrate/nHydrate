using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nHydrate.DslPackage.Forms
{
	public partial class ImportLegacy : Form
	{
		public ImportLegacy()
		{
			InitializeComponent();
			lblHeader.Text = "Import a legacy modelRoot from a previous version of nHydrate with this tool.";
		}

		public string ModelFileName
		{
			get { return txtFile.Text; }
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

		private void cmdFile_Click(object sender, EventArgs e)
		{
			var d = new OpenFileDialog();
			d.FileName = "*.wsgen";
			d.Filter = "nHydrate Legacy Models (*.wsgen)|*.wsgen";
			if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				txtFile.Text = d.FileName;
			}

		}
	}
}

