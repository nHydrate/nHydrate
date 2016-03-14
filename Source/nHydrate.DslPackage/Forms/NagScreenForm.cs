#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System.Reflection;
using System.Windows.Forms;

namespace nHydrate.DslPackage.Forms
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

