namespace PROJECTNAMESPACE
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;

	internal partial class InstallSettingsUI : UserControl
	{
		public InstallSettingsUI()
		{
			InitializeComponent();
		}

		public void LoadUI(InstallSetup setup)
		{
			chkIgnoreWarnings.Checked = setup.AcceptVersionWarningsChangedScripts && setup.AcceptVersionWarningsNewScripts;
			chkSkipNormalize.Checked = setup.SkipNormalize;
			chkUseTransaction.Checked = setup.UseTransaction;
			chkUseHash.Checked = setup.UseHash;
		}

		public void SaveUI(InstallSetup setup)
		{
			setup.AcceptVersionWarningsChangedScripts = chkIgnoreWarnings.Checked;
			setup.AcceptVersionWarningsNewScripts = chkIgnoreWarnings.Checked;
            setup.SkipNormalize = chkSkipNormalize.Checked;
			setup.UseTransaction = chkUseTransaction.Checked;
			setup.UseHash = chkUseHash.Checked;
		}

		private void cmdHelp_Click(object sender, EventArgs e)
		{
			//Create Help dialog
			var sb = new StringBuilder();
			sb.AppendLine("Creates or updates a Sql Server database");
			sb.AppendLine();
			sb.AppendLine("InstallUtil.exe PROJECTNAMESPACE.dll [/upgrade] [/create] [/master:connectionstring] [/connectionstring:connectionstring] [/newdb:name] [/showsql:true|false] [/tranaction:true|false] [/skipnormalize] [/scriptfile:filename] [/scriptfileaction:append] [/checkonly] [/usehash] [/acceptwarnings:all|none|new|changed]");
			sb.AppendLine();
			sb.AppendLine("Providing no parameters will display the default UI.");
			sb.AppendLine();
			sb.AppendLine("/upgrade");
			sb.AppendLine("Specifies that this is an update database operation");
			sb.AppendLine();
			sb.AppendLine("/create");
			sb.AppendLine("Specifies that this is a create database operation");
			sb.AppendLine();
			sb.AppendLine("/master:\"connectionstring\"");
			sb.AppendLine("Specifies the master connection string. This is only required for create database.");
			sb.AppendLine();
			sb.AppendLine("/connectionstring:\"connectionstring\"");
			sb.AppendLine("/Specifies the connection string to the upgrade database");
			sb.AppendLine();
			sb.AppendLine("newdb:name");
			sb.AppendLine("When creating a new database, this is the name of the newly created database.");
			sb.AppendLine();
			sb.AppendLine("/showsql:[true|false]");
			sb.AppendLine("Displays each SQL statement in the console window as its executed. Default is false.");
			sb.AppendLine();
			sb.AppendLine("/tranaction:[true|false]");
			sb.AppendLine("Specifies whether to use a database transaction. Outside of a transaction there is no rollback functionality if an error occurs! Default is true.");
			sb.AppendLine();
			sb.AppendLine("/skipnormalize");
			sb.AppendLine("Specifies whether to skip the normalization script. The normalization script is used to ensure that the database has the correct schema.");
			sb.AppendLine();
			sb.AppendLine("/scriptfile:filename");
			sb.AppendLine("Specifies that a script be created and written to the specified file.");
			sb.AppendLine();
			sb.AppendLine("/scriptfileaction:append");
			sb.AppendLine("Optionally you can specify to append the script to an existing file. If this parameter is omitted, the file will first be deleted if it exists.");
			sb.AppendLine();
			sb.AppendLine("/usehash:[true|false]");
			sb.AppendLine("Specifies that only scripts that have changed will be applied to the database . Default is true.");
			sb.AppendLine();
			sb.AppendLine("/checkonly");
			sb.AppendLine("Specifies check mode and that no scripts will be run against the database. If any changes have occurred, an exception is thrown with the change list.");
			sb.AppendLine();

			MessageBox.Show(sb.ToString(), "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

	}
}
