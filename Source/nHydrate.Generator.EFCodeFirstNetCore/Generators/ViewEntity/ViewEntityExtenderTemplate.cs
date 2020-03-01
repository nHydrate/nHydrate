using System;
using nHydrate.Generator.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ViewEntity
{
    public class ViewEntityExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private CustomView _currentView = null;

        public ViewEntityExtenderTemplate(ModelRoot model, CustomView table)
            : base(model)
        {
            _currentView = table;
        }

        public override string FileName => $"{_currentView.PascalName}.cs";

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
            sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
            sb.AppendLine("{");
            sb.AppendLine("	partial class " + _currentView.PascalName);
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine("}");
        }

    }
}