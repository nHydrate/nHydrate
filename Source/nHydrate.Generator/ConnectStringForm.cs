using System.Windows.Forms;

namespace nHydrate.Generator
{
	public partial class ConnectStringForm : Form
	{
		public ConnectStringForm()
		{
			InitializeComponent();
			DatabaseConnectionControl1.LoadSettings();
		}

		#region Property Implementations

		public string ConnectString
		{
			get { return this.DatabaseConnectionControl1.ImportOptions.GetConnectionString(); }
		}

		public bool AssumeInheritance
		{
			get { return chkInheritance.Checked; }
		}

		#endregion

		#region Event Handlers

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			DatabaseConnectionControl1.PersistSettings();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		#endregion

	}

}
