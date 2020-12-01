#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Entity
{
    public class EntityExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private readonly Table _currentTable = null;

        public EntityExtenderTemplate(ModelRoot model, Table table)
            : base(model)
        {
            _currentTable = table;
        }

        public override string FileName => $"{_currentTable.PascalName}.cs";

        public override string FileContent { get => Generate(); }

        public override string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"namespace {this.GetLocalNamespace()}.Entity");
            sb.AppendLine("{");
            sb.AppendLine("	partial class " + _currentTable.PascalName);
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine("}");
            return sb.ToString();
        }

    }
}
