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
	public partial class SendErrorForm : Form
	{
		public SendErrorForm()
		{
			InitializeComponent();
		}

		#region Event Handlers

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			cmdOK.Enabled = false;
			cmdCancel.Enabled = false;

			this.Close();
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		#endregion
	}
}

