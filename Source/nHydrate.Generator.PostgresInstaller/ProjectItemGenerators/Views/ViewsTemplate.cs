#pragma warning disable 0168
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.Views
{
    class ViewsTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private CustomView _view;

        public ViewsTemplate(ModelRoot model, CustomView view)
            : base(model)
        {
            _view = view;
        }

        public override string FileContent
        {
            get
            {
                this.GenerateContent();
                return sb.ToString();
            }
        }

        public override string FileName => "Views.sql";

        private void GenerateContent()
        {
            sb.AppendLine($"--This SQL is generated for the model defined view [{_view.DatabaseName}]");
            sb.AppendLine();
            sb.Append(SQLEmit.GetSqlCreateView(_view, true));
        }
    }
}