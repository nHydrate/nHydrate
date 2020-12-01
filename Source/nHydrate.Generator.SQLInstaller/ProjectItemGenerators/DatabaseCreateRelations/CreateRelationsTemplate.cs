using nHydrate.Core.SQLGeneration;
using nHydrate.Generator.Common.Models;
using System.Linq;
using System.Text;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.DatabaseCreateRelations
{
    public class CreateRelationsTemplate : BaseDbScriptTemplate
    {
        public CreateRelationsTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileContent { get => Generate(); }

        public override string FileName => "3_CreateRelations.sql";

        public override string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
            sb.AppendLine("--Relations");
            sb.AppendLine();
            this.AppendAll(sb);
            return sb.ToString();
        }

        private void AppendAll(StringBuilder sb)
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
