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

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
    [GeneratorItem("SQLStoredProcedureAllTableGenerator", typeof(DatabaseProjectGenerator))]
    public class SQLStoredProcedureAllTableGenerator : BaseDbScriptGenerator
    {
        #region Properties

        private string ParentItemPath => @"5_Programmability\Stored Procedures\Internal";

        #endregion

        #region Overrides

        public override int FileCount => 1;

        public override void Generate()
        {
            //if (_model.Database.AllowZeroTouch) return;
            try
            {
                //Process all views
                var sb = new StringBuilder();
                sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine();

                if (_model.EmitSafetyScripts)
                {
                    sb.AppendLine("--##SECTION BEGIN [INTERNAL STORED PROCS]");
                    sb.AppendLine();

                    foreach (var table in _model.Database.Tables
                        .Where(x => x.Generated && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
                    {
                        var template = new SQLStoredProcedureTableAllTemplate(_model, table, true);
                        sb.Append(template.FileContent);
                    }

                    sb.AppendLine("--##SECTION END [INTERNAL STORED PROCS]");
                    sb.AppendLine();
                }

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