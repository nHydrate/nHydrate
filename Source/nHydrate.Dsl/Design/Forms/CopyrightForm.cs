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
	public partial class CopyrightForm : Form
	{
		public CopyrightForm()
		{
			InitializeComponent();
			txtText.KeyDown += txtText_KeyDown;
		}

		private void txtText_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A && e.Control)
			{
				txtText.SelectAll();
			}
		}

		public string Copyright
		{
			get { return txtText.Text; }
			set { txtText.Text = value; }
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}

