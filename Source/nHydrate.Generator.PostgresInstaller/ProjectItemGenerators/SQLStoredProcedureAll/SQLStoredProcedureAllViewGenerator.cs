#pragma warning disable 0168
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
    [GeneratorItemAttribute("SQLStoredProcedureAllViewGenerator", typeof(PostgresDatabaseProjectGenerator))]
    public class SQLStoredProcedureAllViewGenerator : BaseDbScriptGenerator
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
                    var DEFAULT_PATH = @"Stored Procedures\Generated\CustomViews";
                    var eventArgs = new ProjectItemExistsEventArgs(ProjectName, DEFAULT_PATH, ProjectItemType.Folder);
                    OnProjectItemExists(this, eventArgs);
                    if (eventArgs.Exists)
                    {
                        _parentItemPath = DEFAULT_PATH;
                    }
                    else
                    {
                        _useSingleFile = true;
                        _parentItemPath = @"5_Programmability\Views\Model";
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
                    return _model.Database.CustomViews.Count;
            }
        }

        public override void Generate()
        {
            try
            {
                if (this.UseSingleFile)
                {
                    //Process views
                    var sb = new StringBuilder();
                    sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                    sb.AppendLine();

                    //Defined views
                    var grantSB = new StringBuilder();
                    foreach (var view in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
                    {
                        var template = new SQLStoredProcedureViewAllTemplate(_model, view, true, grantSB);
                        sb.Append(template.FileContent);
                    }

                    //Add grants
                    sb.Append(grantSB.ToString());

                    var eventArgs = new ProjectItemGeneratedEventArgs("Views.sql", sb.ToString(), ProjectName, this.ParentItemPath, ProjectItemType.Folder, this, true);
                    eventArgs.Properties.Add("BuildAction", 3);
                    OnProjectItemGenerated(this, eventArgs);
                }
                else
                {
                    //Process all views
                    foreach (var view in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
                    {
                        var grantSB = new StringBuilder();
                        var template = new SQLStoredProcedureViewAllTemplate(_model, view, false, grantSB);
                        var fullFileName = template.FileName;

                        //Add grants
                        var sb = new StringBuilder();
                        sb.Append(template.FileContent);
                        sb.Append(grantSB.ToString());

                        var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, sb.ToString(), ProjectName, this.ParentItemPath, ProjectItemType.Folder, this, true);
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