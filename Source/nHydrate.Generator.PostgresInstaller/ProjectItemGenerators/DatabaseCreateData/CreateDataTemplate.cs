#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using System;
using System.Linq;
using System.Text;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.DatabaseCreateData
{
    class CreateDataTemplate : BaseDbScriptTemplate
    {
        #region Constructors
        public CreateDataTemplate(ModelRoot model)
            : base(model)
        {
        }
        #endregion

        #region BaseClassTemplate overrides

        public override string FileContent { get => Generate(); }

        public override string FileName => "2_CreateData.pgsql";

        internal string OldFileName => "CreateData.pgsql";

        #endregion

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
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                sb.Append(SQLEmit.GetSqlInsertStaticData(table));
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
