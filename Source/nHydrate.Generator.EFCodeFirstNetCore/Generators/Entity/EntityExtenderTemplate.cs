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

        #region BaseClassTemplate overrides

        public override string FileName
        {
            get { return string.Format("{0}.cs", _currentTable.PascalName); }
        }


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

        #endregion

        #region GenerateContent

        public void GenerateContent()
        {
            try
            {
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
                sb.AppendLine("{");
                sb.AppendLine("	partial class " + _currentTable.PascalName);
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine("}");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

    }
}