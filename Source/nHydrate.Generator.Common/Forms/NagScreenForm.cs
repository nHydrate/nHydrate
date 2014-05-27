using System.Reflection;
using System.Windows.Forms;

namespace nHydrate.Generator.Common.Forms
{
	internal partial class NagScreenForm : Form
	{
		public NagScreenForm()
		{
			InitializeComponent();

			linkLabel1.Links.Add(new LinkLabel.Link(0, linkLabel1.Text.Length, linkLabel1.Text));
			var asm = Assembly.GetExecutingAssembly();

			lblText.Text = "This product has not been registered. This is a FREE product, but we would like to keep track of its use. We will be developing the collaboration functionality on the main site to improve the tool over time. Please register at the URL below to get your FREE key.";
			lblTrademark.Text = ((AssemblyTrademarkAttribute)asm.GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false)[0]).Trademark;
		}

		private void cmdClose_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			linkLabel1.Links[linkLabel1.Links.IndexOf(e.Link)].Visited = true;
			System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
		}

		private void cmdRegister_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();

			var F = new RegistrationForm();
			F.ShowDialog();
		}

	}
}
