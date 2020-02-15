using System;
using System.Drawing;
using System.Windows.Forms;

namespace nHydrate.Dsl.Design.Forms
{
	/// <summary>
	/// The SQL entry form. See link below for source to textbox
	/// </summary>
	/// <remarks>
	/// http://www.codeproject.com/KB/edit/FastColoredTextBox_.aspx
	/// </remarks>
	public partial class SQLForm : Form
	{
		public SQLForm()
		{
			InitializeComponent();
			this.Size = new Size((int)(Screen.PrimaryScreen.WorkingArea.Width * 0.8), (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.8));
			txtSQL.CurrentLineColor = Color.FromArgb(200, 210, 210, 255);
			txtSQL.ChangedLineColor = Color.FromArgb(255, 230, 230, 255);
		}

		public string SQL
		{
			get { return txtSQL.Text; }
			set
			{
				txtSQL.Text = value;
				txtSQL.ClearUndo();
				txtSQL.ClearUndo();
			}
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

		private void txtSQL_Load(object sender, EventArgs e)
		{

		}

	}
}
