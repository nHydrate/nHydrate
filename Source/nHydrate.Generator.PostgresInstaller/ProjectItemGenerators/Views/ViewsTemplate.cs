#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using System.Text;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.Views
{
    class ViewsTemplate : BaseDbScriptTemplate
    {
        private CustomView _view;

        public ViewsTemplate(ModelRoot model, CustomView view)
            : base(model)
        {
            _view = view;
        }

        public override string FileContent { get => Generate(); }

        public override string FileName => "Views.pgsql";

        private string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"--This SQL is generated for the model defined view [{_view.DatabaseName}]");
            sb.AppendLine();
            sb.Append(SQLEmit.GetSqlCreateView(_view, true));
            return sb.ToString();
        }
    }
}
