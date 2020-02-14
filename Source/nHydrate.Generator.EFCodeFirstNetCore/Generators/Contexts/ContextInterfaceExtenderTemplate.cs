#pragma warning disable 0168
using System;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Contexts
{
    public class ContextInterfaceExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public ContextInterfaceExtenderTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileName
        {
            get { return string.Format("I{0}Entities.cs", _model.ProjectName); }
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

        private void GenerateContent()
        {
            try
            {
                nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");
                sb.AppendLine("	partial interface I" + _model.ProjectName + "Entities");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine("}");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}