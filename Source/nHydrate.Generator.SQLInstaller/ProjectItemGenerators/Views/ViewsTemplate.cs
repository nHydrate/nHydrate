#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using System;
using System.Linq;
using System.Text;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.Views
{
    public class ViewsTemplate : BaseDbScriptTemplate
    {
        #region Constructors
        public ViewsTemplate(ModelRoot model)
            : base(model)
        {
        }
        #endregion

        #region BaseClassTemplate overrides

        public override string FileContent { get => Generate(); }

        public override string FileName => "Views.sql";

        #endregion

        #region GenerateContent
        public override string Generate()
        {
            var sb = new StringBuilder();
            foreach (var view in _model.Database.CustomViews.OrderBy(x => x.Name))
            {
                sb.AppendLine($"--This SQL is generated for the model defined view [{view.DatabaseName}]");
                sb.AppendLine();
                sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlCreateView(view, true));
                if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
                {
                    sb.AppendFormat("GRANT ALL ON [" + view.GetSQLSchema() + "].[{0}] TO [{1}]", view.DatabaseName, _model.Database.GrantExecUser).AppendLine();
                    sb.AppendLine($"--MODELID: {sb}");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        #endregion

    }
}
