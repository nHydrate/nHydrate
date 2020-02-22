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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
    [GeneratorItemAttribute("SQLStoredProcedureAllTableGenerator", typeof(DatabaseProjectGenerator))]
    public class SQLStoredProcedureAllTableGenerator : BaseDbScriptGenerator
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
                    var DEFAULT_PATH = @"Stored Procedures\Generated\Objects";
                    var eventArgs = new ProjectItemExistsEventArgs(ProjectName, DEFAULT_PATH, ProjectItemType.Folder);
                    OnProjectItemExists(this, eventArgs);
                    if (eventArgs.Exists)
                    {
                        return DEFAULT_PATH;
                    }
                    else
                    {
                        _useSingleFile = true;
                        return @"5_Programmability\Stored Procedures\Internal";
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
            get
            {
                if (this.UseSingleFile)
                    return 1;
                else
                    return _model.Database.Tables.Count(x => x.Generated);
            }
        }

        public override void Generate()
        {
            //if (_model.Database.AllowZeroTouch) return;
            try
            {
                if (this.UseSingleFile)
                {
                    //Process all views
                    var sb = new StringBuilder();
                    sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                    sb.AppendLine();

                    if (_model.EmitSafetyScripts)
                    {
                        sb.AppendLine("--##SECTION BEGIN [INTERNAL STORED PROCS]");
                        sb.AppendLine();

                        foreach (var table in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
                        {
                            var template = new SQLStoredProcedureTableAllTemplate(_model, table, true);
                            sb.Append(template.FileContent);
                        }

                        sb.AppendLine("--##SECTION END [INTERNAL STORED PROCS]");
                        sb.AppendLine();
                    }

                    var eventArgs = new ProjectItemGeneratedEventArgs("StoredProcedures.sql", sb.ToString(), ProjectName, this.ParentItemPath, ProjectItemType.Folder, this, true);
                    eventArgs.Properties.Add("BuildAction", 3);
                    OnProjectItemGenerated(this, eventArgs);
                }
                else
                {
                    if (_model.EmitSafetyScripts)
                    {
                        //Process all tables
                        foreach (var table in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
                        {
                            var template = new SQLStoredProcedureTableAllTemplate(_model, table, false);
                            var fullFileName = template.FileName;
                            var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this.ParentItemPath, ProjectItemType.Folder, this, true);
                            eventArgs.Properties.Add("BuildAction", 3);
                            OnProjectItemGenerated(this, eventArgs);
                        }
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