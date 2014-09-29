#pragma warning disable 0168
namespace PROJECTNAMESPACE
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;
	using System.IO;

	internal partial class SqlErrorForm : Form
	{
		public SqlErrorForm()
		{
			InitializeComponent();
		}

		public void Setup(InvalidSQLException exception, bool allowSkip)
		{
			UpgradeInstaller.LogError(exception.InnerException, "[ERROR SCRIPT]\r\n" + exception.SQL);
			txtError.Text = exception.InnerException.ToString();
			txtSql.Text = exception.SQL;
			cmdSkip.Visible = allowSkip;
		}

		public void SetupGeneric(Exception exception)
		{
			try
			{
				this.Text = "Error";
				this.splitter1.Visible = false;
				this.panel3.Dock = DockStyle.None;
				this.panel3.Visible = false;
				this.panel1.Dock = DockStyle.Fill;
				this.panel1.BringToFront();
				UpgradeInstaller.LogError(exception.InnerException, "[ERROR]\r\n" + exception.ToString());
				if (exception.InnerException != null)
					txtError.Text = exception.InnerException.ToString();
				else
					txtError.Text = exception.ToString();
				cmdSkip.Visible = false;
			}
			catch (Exception ex)
			{
				//Do Nothing
			}
		}
	}
}
#pragma warning restore 0168