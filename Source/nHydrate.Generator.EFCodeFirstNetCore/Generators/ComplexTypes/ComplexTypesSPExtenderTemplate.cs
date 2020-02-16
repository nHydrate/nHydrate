#pragma warning disable 0168
using System;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ComplexTypes
{
    public class ComplexTypesSPExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();
        private readonly CustomStoredProcedure _item = null;

        public ComplexTypesSPExtenderTemplate(ModelRoot model, CustomStoredProcedure item)
            : base(model)
        {
            _item = item;
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return _item.PascalName + ".cs"; }
        }

        public override string FileContent
        {
            get
            {
                try
                {
                    this.GenerateContent();
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
                sb.AppendLine($"namespace {this.GetLocalNamespace()}.Entity");
                sb.AppendLine("{");
                sb.AppendLine("	partial class " + _item.PascalName);
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