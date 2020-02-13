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
	public partial class ErrorForm : Form
	{
		public ErrorForm()
		{
			InitializeComponent();
			this.KeyDown += new KeyEventHandler(ErrorForm_KeyDown);
		}

		private void ErrorForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		public void SetErrors(IList<string> errorList)
		{
			foreach (var t in errorList)
			{
				txtBox.Text += t + "\r\n\r\n";
			}
		}
	}
}

