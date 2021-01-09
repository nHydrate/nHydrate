#pragma warning disable 0168
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.Util;
using System.Linq;
using System.Text;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.DatabaseCreateRelations
{
    public class CreateRelationsTemplate : BaseDbScriptTemplate
    {
        #region Constructors
        public CreateRelationsTemplate(ModelRoot model)
            : base(model)
        {
        }
        #endregion

        public override string FileContent { get => Generate(); }

        public override string FileName => "3_CreateRelations.pgsql";

        public override string Generate()
        {
            var sb = new StringBuilder();
            sb = new StringBuilder();
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

            foreach (var table in _model.Database.Tables.Where(x => !x.IsEnumOnly()).OrderBy(x => x.Name))
            {
                var childRoleRelations = table.ChildRoleRelations;
                if (childRoleRelations.Count > 0)
                {
                    foreach (var relation in childRoleRelations.Where(x => x.Enforce).OrderBy(x => x.ParentTable.Name).ThenBy(x => x.ChildTable.Name).ThenBy(x => x.RoleName))
                    {
                        sb.Append(SQLEmit.GetSqlAddFK(relation));
                        sb.AppendLine("--GO");
                        sb.AppendLine();
                    }
                }
            }

            sb.AppendLine("--##SECTION END [RELATIONS]");
            sb.AppendLine();

        }

    }
}
