using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ConvertCode
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void cmdParse_Click(object sender, EventArgs e)
		{
			var text = txtInput.Text;
			text = text.Replace("\r\n", "\n");
			text = text.Replace("\n", "\r\n");
			txtOutput.Text = AspStringBlock.ConvertCode(text);
		}

		private void cmdCopy_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(txtOutput.Text))
				Clipboard.SetText(txtOutput.Text);
		}

	}
}
