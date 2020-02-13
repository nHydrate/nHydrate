using System.Windows.Forms;

namespace nHydrate.Generator.Common.Forms
{
	public partial class ErrorForm : Form
	{
		public ErrorForm()
		{
			InitializeComponent();
		}

		public ErrorForm(string message, string error)
			: this()
		{
			lblMessage.Text = message;
			txtError.Text = error;
		}

		private void cmdClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	}
}

