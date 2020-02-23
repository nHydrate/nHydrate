#pragma warning disable 0168
using System;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Helpers
{
    public class HelperExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();

        public HelperExtenderTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides

        public override string FileName => "Globals.cs";

        public override string FileContent
        {
            get
            {
                try
                {
                    sb = new StringBuilder();
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
                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");
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