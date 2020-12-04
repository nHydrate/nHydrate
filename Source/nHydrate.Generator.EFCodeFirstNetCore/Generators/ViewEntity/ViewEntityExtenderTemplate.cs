using nHydrate.Generator.Common.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ViewEntity
{
    public class ViewEntityExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private readonly CustomView _currentView = null;

        public ViewEntityExtenderTemplate(ModelRoot model, CustomView table)
            : base(model)
        {
            _currentView = table;
        }

        public override string FileName => $"{_currentView.PascalName}.cs";

        public override string FileContent { get => Generate(); }

        public override string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"namespace {this.GetLocalNamespace()}.Entity");
            sb.AppendLine("{");
            sb.AppendLine("	partial class " + _currentView.PascalName);
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine("}");
            return sb.ToString();
        }

    }
}
