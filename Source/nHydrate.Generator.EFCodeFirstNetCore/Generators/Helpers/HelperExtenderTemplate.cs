#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using System;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Helpers
{
    public class HelperExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        public HelperExtenderTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileName => "Globals.cs";

        public override string FileContent { get => Generate(); }

        private string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine("namespace " + this.GetLocalNamespace());
            sb.AppendLine("{");
            sb.AppendLine("}");
            return sb.ToString();
        }
 
    }
}
