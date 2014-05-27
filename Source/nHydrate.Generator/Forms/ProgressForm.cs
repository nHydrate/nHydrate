using System.Windows.Forms;

namespace nHydrate.Generator.Forms
{
	public partial class ProgressForm : Form
	{
		#region Class Members

		private int _percentDone = 0;
		private string _windowTitle = "Processing...";

		#endregion

		#region Constructors

		public ProgressForm()
		{
			InitializeComponent();
			this.Text = _windowTitle;
		}

		#endregion

		#region Methods

		public void SetProgress(int percentDone, string text)
		{
			lblText.Text = text;
			progressBar1.Value = percentDone;
			_percentDone = percentDone;
			this.Text = _windowTitle + " " + percentDone.ToString() + "%";
		}

		public void SetWindowTitle(string windowTitle)
		{
			_windowTitle = windowTitle;
		}

		#endregion

		#region Event Handlers

		private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_percentDone < 100)
			{
				if (e.CloseReason == CloseReason.UserClosing)
					e.Cancel = true;
			}
		}

		#endregion

	}
}
