using System.Linq;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Core.SQLGeneration;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.DatabaseCreateRelations
{
    public class CreateRelationsTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();

        public CreateRelationsTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileContent
        {
            get
            {
                GenerateContent();
                return sb.ToString();
            }
        }

        public override string FileName => "3_CreateRelations.sql";

        private void GenerateContent()
        {
            sb = new StringBuilder();
            sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
            sb.AppendLine("--Relations");
            sb.AppendLine();
            this.AppendAll();
        }

        private void AppendAll()
        {
            sb.AppendLine("--##SECTION BEGIN [RELATIONS]");
            sb.AppendLine();

            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                var tableName = Globals.GetTableDatabaseName(_model, table);
                var childRoleRelations = table.ChildRoleRelations;
                if (childRoleRelations.Count > 0)
                {
                    foreach (var relation in childRoleRelations.Where(x => x.Enforce))
                    {
                        sb.Append(SQLEmit.GetSqlAddFK(relation));
                        sb.AppendLine("GO");
                        sb.AppendLine();
                    }
                }
            }

            sb.AppendLine("--##SECTION END [RELATIONS]");
            sb.AppendLine();

        }

    }
}