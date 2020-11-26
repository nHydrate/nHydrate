#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextStartup
{
    public class ContextStartupExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        public ContextStartupExtenderTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileName => "ContextStartup.cs";

        public override string FileContent { get => Generate(); }

        private string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine("namespace " + this.GetLocalNamespace());
            sb.AppendLine("{");
            sb.AppendLine("	partial class ContextStartup");
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine();

            sb.AppendLine("	partial class TenantContextStartup");
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine();

            sb.AppendLine("}");

            return sb.ToString();
        }

    }
}
