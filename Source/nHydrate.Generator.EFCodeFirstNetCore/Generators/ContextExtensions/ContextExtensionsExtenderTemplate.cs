#pragma warning disable 0168
using nHydrate.Generator.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextExtensions
{
    public class ContextExtensionsExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();

        public ContextExtensionsExtenderTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileName => _model.ProjectName + "EntitiesExtensions.cs";

        public override string FileContent
        {
            get
            {
                GenerateContent();
                return sb.ToString();
            }
        }

        public void GenerateContent()
        {
            sb.AppendLine("namespace " + this.GetLocalNamespace());
            sb.AppendLine("{");
            sb.AppendLine("	partial class " + _model.ProjectName + "EntitiesExtensions");
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine("}");
        }

    }
}