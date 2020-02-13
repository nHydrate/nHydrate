using System.Windows.Forms;

namespace nHydrate.Generator.Common.Forms
{
	public partial class SendErrorForm : Form
	{
		public SendErrorForm()
		{
			InitializeComponent();
			this.FormClosing += new FormClosingEventHandler(SendErrorForm_FormClosing);
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

		private void SendErrorForm_FormClosing(object sender, FormClosingEventArgs e)
		{

		}

		#endregion

	}
}

