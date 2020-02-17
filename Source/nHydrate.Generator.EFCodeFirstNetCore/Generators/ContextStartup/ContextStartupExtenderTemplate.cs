#pragma warning disable 0168
using System;
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

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return "ContextStartup.cs"; }
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
                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");
                sb.AppendLine("	partial class ContextStartup");
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