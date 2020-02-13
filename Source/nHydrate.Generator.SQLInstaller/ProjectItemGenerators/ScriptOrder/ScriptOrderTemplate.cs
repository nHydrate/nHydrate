#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator;
using nHydrate.Generator.Models;
using System.Collections;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;
using System.Xml;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.ScriptOrder
{
    class ScriptOrderTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();

        #region Constructors
        public ScriptOrderTemplate(ModelRoot model)
            : base(model)
        {
        }
        #endregion

        #region BaseClassTemplate overrides
        public override string FileContent
        {
            get
            {
                GenerateContent();
                return sb.ToString();
            }
        }

        public override string FileName
        {
            get
            {
                return string.Format("ScriptOrder.nOrder");
            }
        }
        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                var document = new System.Xml.XmlDocument();
                document.LoadXml("<root type=\"installer\"></root>");
                XmlHelper.AddLineBreak(document.DocumentElement);
                if (_model.Database.PrecedenceOrderList != null)
                {
                    foreach (var k in _model.Database.PrecedenceOrderList)
                    {
                        var n = document.CreateElement("key");
                        n.InnerText = k.ToString();
                        document.DocumentElement.AppendChild(n);
                        XmlHelper.AddLineBreak(document.DocumentElement);
                    }
                }
                sb.Append(document.OuterXml);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}