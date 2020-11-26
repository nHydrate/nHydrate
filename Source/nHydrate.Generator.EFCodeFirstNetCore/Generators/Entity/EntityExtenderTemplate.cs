#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using System;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Entity
{
    public class EntityExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private Table _currentTable = null;

        public EntityExtenderTemplate(ModelRoot model, Table table)
            : base(model)
        {
            _currentTable = table;
        }

        public override string FileName => $"{_currentTable.PascalName}.cs";

        public override string FileContent { get => Generate(); }

        private string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
            sb.AppendLine("{");
            sb.AppendLine("	partial class " + _currentTable.PascalName);
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine("}");
            return sb.ToString();
        }

    }
}
