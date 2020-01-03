#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.SQLSelectStoredProcedure
{
	[GeneratorItemAttribute("SqlSelectDefinedStoredProcedure", typeof(PostgresDatabaseProjectGenerator))]
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
