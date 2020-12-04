using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.Util;
using System.Linq;
using System.Text;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.DatabaseCreateData
{
    class CreateDataTemplate : BaseDbScriptTemplate
    {
        public CreateDataTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileContent { get => Generate(); }

        public override string FileName => "2_CreateData.sql";

        internal string OldFileName => "CreateData.sql";

        public override string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
            sb.AppendLine("--Static Data");
            sb.AppendLine();

            //Turn OFF CONSTRAINTS
            //sb.AppendLine("if (SERVERPROPERTY('EngineEdition') <> 5) --NOT AZURE");
            //sb.AppendLine("exec sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            //sb.AppendLine();

            #region Add Static Data

            foreach (var table in _model.Database.Tables.Where(x => !x.IsEnumOnly()).OrderBy(x => x.Name))
            {
                sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlInsertStaticData(table));
            }

            #endregion

            //Turn ON CONSTRAINTS
            //sb.AppendLine("if (SERVERPROPERTY('EngineEdition') <> 5) --NOT AZURE");
            //sb.AppendLine("exec sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");
            //sb.AppendLine();
            return sb.ToString();
        }

    }
}
