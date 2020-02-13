using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nHydrate.Dsl.Design.Forms
{
	public partial class OutputTargetForm : Form
	{
		public OutputTargetForm()
		{
			InitializeComponent();
		}

		public string TargetLocation
		{
			get { return txtLocation.Text; }
			set { txtLocation.Text = value; }
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

