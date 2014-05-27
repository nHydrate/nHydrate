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
using System.Reflection;
using System.Text;
using Widgetsphere.Generator.Common.GeneratorFramework;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;
using Widgetsphere.Generator.Common.Util;

namespace Widgetsphere.Generator.ProjectItemGenerators.DatabaseEmbeddedClasses
{
	[GeneratorItemAttribute("DatabaseEmbeddedClassGenerator", typeof(DatabaseProjectGenerator))]
	public class DatabaseEmbeddedClassGenerator : BaseDbScriptGenerator
	{
		private const string EMBEDDED_LOCATION = "Widgetsphere.Generator.ProjectItemGenerators.DatabaseEmbeddedClasses";

		#region Overrides
		public override int FileCount
		{
			get { return 6; }
		}


		public override void Generate()
		{
			try
			{
				GenerateDatabaseInstallerCs();
				GenerateDatabaseInstallerDesignerCs();
				GenerateIdentifyDatabaseFormCs();
				GenerateIdentifyDatabaseFormResx();
				GenerateSqlServersCs();
				GenerateArchiveReaderCs();
				GenerateUpgradeInstaller();
				ProjectItemGenerationCompleteEventArgs gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);

			}
			catch (Exception ex)
			{				
				throw;
			}
		}

		private void GenerateIdentifyDatabaseFormResx()
		{
			string fullParentName = "IdentifyDatabaseForm.cs";
			string fileName = "IdentifyDatabaseForm.resx";
			EmbeddedResourceName ern = new EmbeddedResourceName();
			ern.AsmLocation = EMBEDDED_LOCATION;
			ern.FileName = fileName + ".embed";
			ern.FullName = EMBEDDED_LOCATION + "." + ern.FileName;
			string fileContent = GetFileContent(ern);
      ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(fileName, fileContent, ProjectName, fullParentName, this, true);
			eventArgs.Properties.Add("BuildAction", 3);
			OnProjectItemGenerated(this, eventArgs);
		}

		private void GenerateUpgradeInstaller()
		{
			string fileName = "UpgradeInstaller.cs";
			string fileContent = GetFileContent(new EmbeddedResourceName(EMBEDDED_LOCATION + "." + fileName));
			//TODO - Change the version for _upgradeToVersion and _previousVersion
			ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(fileName, fileContent, ProjectName, this, true);
			OnProjectItemGenerated(this, eventArgs);
		}

		private void GenerateSqlServersCs()
		{
			string fileName = "SqlServers.cs";
			string fileContent = GetFileContent(new EmbeddedResourceName(EMBEDDED_LOCATION + "." + fileName));
			ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(fileName, fileContent, ProjectName, this, true);
			OnProjectItemGenerated(this, eventArgs);
		}

		private void GenerateArchiveReaderCs()
		{
			string fileName = "ArchiveReader.cs";
			string fileContent = GetFileContent(new EmbeddedResourceName(EMBEDDED_LOCATION + "." + fileName));
			ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(fileName, fileContent, ProjectName, this, true);
			OnProjectItemGenerated(this, eventArgs);
		}		

		private void GenerateIdentifyDatabaseFormCs()
		{
			string fileName = "IdentifyDatabaseForm.cs";
			string fileContent = GetFileContent(new EmbeddedResourceName(EMBEDDED_LOCATION + "." + fileName));
			ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(fileName, fileContent, ProjectName, this, true);
			OnProjectItemGenerated(this, eventArgs);
		}

		private void GenerateDatabaseInstallerDesignerCs()
		{
			string fullParentName = "DatabaseInstaller.cs";
			string fileName = "DatabaseInstaller.Designer.cs";
			EmbeddedResourceName ern = new EmbeddedResourceName();
			ern.AsmLocation = EMBEDDED_LOCATION;
			ern.FileName = "DatabaseInstaller.Designer.embed";
			ern.FullName = EMBEDDED_LOCATION + "." + ern.FileName;
			string fileContent = GetFileContent(ern);
      ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(fileName, fileContent, ProjectName, fullParentName, this, true);
			OnProjectItemGenerated(this, eventArgs);
		}

		private void GenerateDatabaseInstallerCs()
		{
			string fileName = "DatabaseInstaller.cs";
			string fileContent = GetFileContent(new EmbeddedResourceName(EMBEDDED_LOCATION + "." + fileName));
			ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(fileName, fileContent, ProjectName, this, true);
			OnProjectItemGenerated(this, eventArgs);
		}


		private string GetResource(EmbeddedResourceName ern)
		{
			string retVal = string.Empty;
			Assembly asm = Assembly.GetExecutingAssembly();
			System.IO.Stream manifestStream = asm.GetManifestResourceStream(ern.FullName);
			try
			{
				using (System.IO.StreamReader sr = new System.IO.StreamReader(manifestStream))
				{
					retVal = sr.ReadToEnd();
				}
			}
			catch { }
			finally
			{
				manifestStream.Close();
			}
			return retVal;
		}

		private string GetFileContent(EmbeddedResourceName ern)
		{
			try
			{
				string retVal = GetResource(ern);
				retVal = ReplaceWidgetsphereSpecifics(retVal);
				return retVal;
			}
			catch (Exception ex)
			{				
				throw;
			}
		}

		private string ReplaceWidgetsphereSpecifics(string input)
		{
			string retVal = input;
			retVal = retVal.Replace("Widgetsphere", _model.CompanyName);
			retVal = retVal.Replace("Widgetsphere", _model.CompanyName);
			retVal = retVal.Replace("WS", _model.CompanyAbbreviation);
			retVal = retVal.Replace("Acme", _model.CompanyName);
			retVal = retVal.Replace("ZZ", _model.CompanyAbbreviation);
			string[] versionNumbers = _model.Version.Split('.');
			int major = int.Parse(versionNumbers[0]);
			int minor = int.Parse(versionNumbers[1]);
			int revision = int.Parse(versionNumbers[2]);
			int build = int.Parse(versionNumbers[3]);
			retVal = retVal.Replace("\"UPGRADE_VERSION\"", major + ", " + minor + ", " + revision + ", " + build);
			retVal = retVal.Replace("DATABASENAME", _model.Database.DatabaseName);
			retVal = retVal.Replace("PROJECTNAME", _model.ProjectName);
			return retVal;
		}
		#endregion


	}
}
