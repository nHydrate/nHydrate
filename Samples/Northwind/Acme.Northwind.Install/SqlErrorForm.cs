using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Acme.Northwind.Install
{
	internal partial class SqlErrorForm : Form
	{
		public SqlErrorForm()
		{
			InitializeComponent();
		}

		public void Setup(InvalidSQLException exception)
		{
			txtError.Text = exception.InnerException.ToString();
			txtSql.Text = exception.SQL;
		}
	}
}
