#region Copyright (c) 2006-2009 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2009 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Windows.Forms;

namespace Acme.PROJECTNAME.Install
{
	[RunInstaller(true)]
	public partial class DatabaseInstaller : Installer
	{
		private string _connectionString = string.Empty;
		private bool _newInstall = false;

		public DatabaseInstaller()
		{
			InitializeComponent();
		}

		public override void Install(System.Collections.IDictionary stateSaver)
		{
			base.Install(stateSaver);
			if (IdentifyDatabaseConnectionString())
				UpgradeInstaller.UpgradeDatabase(_connectionString, _newInstall);
		}

    public void Install(string masterConnectionString, string newDatabaseName, string newDatabaseConnectionString)
    {
      SqlServers.CreateDatabase(masterConnectionString, newDatabaseName);
      DateTime startTime = DateTime.Now;
      while(DateTime.Now.Subtract(startTime).TotalSeconds < 9)
      {
        Application.DoEvents();
      }

      try
      {
        UpgradeInstaller.UpgradeDatabase(newDatabaseConnectionString, true);
      }
      catch(Exception ex)
      {
        System.Threading.Thread.Sleep(8000);
        try
        {
          UpgradeInstaller.UpgradeDatabase(newDatabaseConnectionString, true);
        }
        catch
        {
          throw;
        }
      }
    }

		public void Install()
		{
			if (IdentifyDatabaseConnectionString())
				UpgradeInstaller.UpgradeDatabase(_connectionString, _newInstall);
		}
		
		public override void Uninstall(System.Collections.IDictionary savedState)
		{
			base.Uninstall(savedState);
		}

		public bool IdentifyDatabaseConnectionString()
		{
			IdentifyDatabaseForm idf = new IdentifyDatabaseForm();
			if (idf.ShowDialog() == DialogResult.OK)
			{
				_connectionString = idf.ConnectionString;
				_newInstall = idf.CreatedDatabase;
				return true;
			}
			return false;
		}

	}
}