#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLSelectStoredProcedure
{
	[GeneratorItemAttribute("SqlSelectDefinedStoredProcedure", typeof(DatabaseProjectGenerator))]
	public class SQLSelectStoredProcedureGenerator : BaseDbScriptGenerator
	{
		#region Properties

		private string _parentItemPath = string.Empty;
		private bool _useSingleFile = false;
		private string ParentItemPath
		{
			get
			{
				if (string.IsNullOrEmpty(_parentItemPath))
				{
					//Feb 7, 2012 - Rearranged the Installer output folders. This code ensures old projects do not break
					//If the old folder structure exists then continue to use it.
					var DEFAULT_PATH = @"Stored Procedures\Generated\CustomStoredProcedures";
					var eventArgs = new ProjectItemExistsEventArgs(ProjectName, DEFAULT_PATH, ProjectItemType.Folder);
					OnProjectItemExists(this, eventArgs);
					if (eventArgs.Exists)
					{
						return DEFAULT_PATH;
					}
					else
					{
						_useSingleFile = true;
						return @"5_Programmability\Stored Procedures\Model";
					}
				}
				return _parentItemPath;
			}
		}

		private bool UseSingleFile
		{
			get
			{
				var s = this.ParentItemPath; //forces a refresh of this private variable
				return _useSingleFile;
			}
		}

		#endregion

		#region Overrides

		public override int FileCount
		{
			get { return 1; }
		}

		public override void Generate()
		{
			try
			{
				if (this.UseSingleFile)
				{
					//Process all views
					var sb = new StringBuilder();
					sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
					sb.AppendLine();

					var grantSB = new StringBuilder();
					foreach (var storedProcedure in _model.Database.CustomStoredProcedures.Where(x => x.Generated).OrderBy(x => x.Name))
					{
						var template = new SQLSelectStoredProcedureTemplate(_model, storedProcedure, true, grantSB);
						sb.Append(template.FileContent);
					}

					//Add Grants
					sb.Append(grantSB.ToString());

					var eventArgs = new ProjectItemGeneratedEventArgs("StoredProcedures.sql", sb.ToString(), ProjectName, this.ParentItemPath, ProjectItemType.Folder, this, true);
					eventArgs.Properties.Add("BuildAction", 3);
					OnProjectItemGenerated(this, eventArgs);
				}
				else
				{
					foreach (var storedProcedure in _model.Database.CustomStoredProcedures.Where(x => x.Generated).OrderBy(x => x.Name))
					{
						var grantSB = new StringBuilder();
						var template = new SQLSelectStoredProcedureTemplate(_model, storedProcedure, false, grantSB);

						//Add grants
						var sb = new StringBuilder();
						sb.Append(template.FileContent);
						sb.Append(grantSB.ToString());

						var fullFileName = template.FileName;
						var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this.ParentItemPath, ProjectItemType.Folder, this, true);
						eventArgs.Properties.Add("BuildAction", 3);
						OnProjectItemGenerated(this, eventArgs);
					}
				}

				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

	}
}
