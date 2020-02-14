using System.Collections.Generic;
using System.Windows.Forms;

namespace nHydrate.DslPackage.Forms
{
	public partial class ErrorForm : Form
	{
		public ErrorForm()
		{
			InitializeComponent();
			this.KeyDown += ErrorForm_KeyDown;
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

