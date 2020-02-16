#pragma warning disable 0168
using System;
using System.Linq;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using nHydrate.Generator.EFCodeFirstNetCore;
using nHydrate.Generator.Common;

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

        #region BaseClassTemplate overrides

        public override string FileName
        {
            get { return string.Format("{0}.cs", _currentView.PascalName); }
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
                sb.AppendLine("	partial class " + _currentView.PascalName);
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