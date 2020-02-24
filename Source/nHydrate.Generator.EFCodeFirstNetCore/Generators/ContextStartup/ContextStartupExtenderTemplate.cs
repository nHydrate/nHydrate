#pragma warning disable 0168
using nHydrate.Generator.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextStartup
{
    public class ContextStartupExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();

        public ContextStartupExtenderTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileName => "ContextStartup.cs";

        public override string FileContent
        {
            get
            {
                GenerateContent();
                return sb.ToString();
            }
        }

        private void GenerateContent()
        {
            sb.AppendLine("namespace " + this.GetLocalNamespace());
            sb.AppendLine("{");
            sb.AppendLine("	partial class ContextStartup");
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine("}");
        }

    }
}