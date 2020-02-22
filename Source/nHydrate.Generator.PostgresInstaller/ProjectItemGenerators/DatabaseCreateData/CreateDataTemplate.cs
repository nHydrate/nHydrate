#pragma warning disable 0168
using System;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.DatabaseCreateData
{
    class CreateDataTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();

        #region Constructors
        public CreateDataTemplate(ModelRoot model)
            : base(model)
        {
        }
        #endregion

        #region BaseClassTemplate overrides

        public override string FileContent
        {
            get
            {
                GenerateContent();
                return sb.ToString();
            }
        }

        public override string FileName => "2_CreateData.sql";

        internal string OldFileName => "CreateData.sql";

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}