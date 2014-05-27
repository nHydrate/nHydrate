using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Widgetsphere.Generator.Common.Forms
{
  public partial class GenerateSpecifyForm : Form
  {
    public GenerateSpecifyForm()
    {
      InitializeComponent();
    }

    private void cmdOK_Click(object sender, System.EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void cmdCancel_Click(object sender, System.EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

  }
}