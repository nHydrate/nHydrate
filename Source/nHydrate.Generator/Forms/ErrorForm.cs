using System;
using System.Windows.Forms;

namespace nHydrate.Generator.Forms
{
	public partial class ErrorForm : Form
	{
		public ErrorForm()
		{
			InitializeComponent();
			this.Size = this.MinimumSize;
		}

		public ErrorForm(string message, Exception exception)
			: this()
		{
			txtText.Text = message;
			if (exception == null)
			{
				cmdDetail.Visible = false;
			}
			else
			{
				txtText2.Text = exception.ToString();
			}
		}

		private void cmdDetail_Click(object sender, EventArgs e)
		{
			this.Size = this.MaximumSize;
			cmdDetail.Enabled = false;
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

	}
}
