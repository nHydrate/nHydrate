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
    [GeneratorItem("SqlSelectDefinedStoredProcedure", typeof(DatabaseProjectGenerator))]
    public class SQLSelectStoredProcedureGenerator : BaseDbScriptGenerator
    {
        #region Properties

        private string ParentItemPath => @"5_Programmability\Stored Procedures\Model";

        #endregion

        #region Overrides

        public override int FileCount => 1;

        public override void Generate()
        {
            try
            {
                //Process all views
                var sb = new StringBuilder();
                sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine();

                var grantSB = new StringBuilder();
                foreach (var storedProcedure in _model.Database.CustomStoredProcedures.Where(x => x.Generated)
                    .OrderBy(x => x.Name))
                {
                    var template = new SQLSelectStoredProcedureTemplate(_model, storedProcedure, true, grantSB);
                    sb.Append(template.FileContent);
                }

                //Add Grants
                sb.Append(grantSB.ToString());

                var eventArgs = new ProjectItemGeneratedEventArgs("StoredProcedures.sql", sb.ToString(), ProjectName,
                    this.ParentItemPath, ProjectItemType.Folder, this, true);
                eventArgs.Properties.Add("BuildAction", 3);
                OnProjectItemGenerated(this, eventArgs);

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
