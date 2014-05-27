using System.Windows.Forms;

namespace nHydrate.Generator.Common.Forms
{
	public partial class ProgressingForm : Form
	{
		#region Class Members
		
		private int _percentDone = 0;
		private string _windowTitle = "Processing...";

		#endregion

		#region Constructors

		public ProgressingForm()
		{
			InitializeComponent();			
		}

		#endregion

		#region Event Handlers

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			Application.DoEvents();
		}

		#endregion

	}
}
