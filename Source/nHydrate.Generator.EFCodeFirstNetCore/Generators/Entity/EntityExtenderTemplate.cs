#pragma warning disable 0168
using System;
using nHydrate.Generator.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Entity
{
    public class EntityExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private Table _currentTable = null;

        public EntityExtenderTemplate(ModelRoot model, Table table)
            : base(model)
        {
            _currentTable = table;
        }

        public override string FileName => $"{_currentTable.PascalName}.cs";

        public override string FileContent
        {
            get
            {
                try
                {
                    GenerateContent();
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void GenerateContent()
        {
            sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
            sb.AppendLine("{");
            sb.AppendLine("	partial class " + _currentTable.PascalName);
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine("}");
        }

    }
}