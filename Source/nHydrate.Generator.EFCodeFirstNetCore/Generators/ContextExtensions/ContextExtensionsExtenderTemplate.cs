#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextExtensions
{
    public class ContextExtensionsExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        public ContextExtensionsExtenderTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileName => _model.ProjectName + "EntitiesExtensions.cs";

        public override string FileContent { get => Generate(); }

        public string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"namespace {this.GetLocalNamespace()}");
            sb.AppendLine("{");
            sb.AppendLine($"	partial class {_model.ProjectName}EntitiesExtensions");
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine("}");
            return sb.ToString();
        }

    }
}
