#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class SecurityFunction : BaseModelObject
    {
        #region Member Variables

        private readonly Table _parent = null;
        protected ReferenceCollection _parameters = null;

        #endregion

        #region Constructors

        public SecurityFunction(INHydrateModelObject root, Table parent)
            : base(root)
        {
            _parent = parent;

            _parameters = new ReferenceCollection(this.Root, this, ReferenceType.Parameter);
            _parameters.ResetKey(Guid.Empty.ToString());
            _parameters.ObjectPlural = "Parameters";
            _parameters.ObjectSingular = "Parameter";
            _parameters.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
            _parameters.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);
        }

        #endregion

        #region Properties

        public string SQL { get; set; }

        [
        Browsable(false),
        Description("Determines the parameters that are associated with this rule."),
        Category("Data"),
        ]
        public ReferenceCollection Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Returns the parameters for this object
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parameter> GetParameters()
        {
            var retval = new List<Parameter>();
            foreach (Reference reference in this.Parameters)
            {
                retval.Add((Parameter)reference.Object);
            }
            retval.RemoveAll(x => x == null);
            return retval.OrderBy(x => x.Name);
        }

        [Browsable(false)]
        public IEnumerable<Parameter> GeneratedParameters
        {
            get { return this.GetParameters().Where(x => x.Generated); }
        }

        #endregion

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(this.SQL));
        }

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);
                XmlHelper.AddAttribute(node, "sql", this.SQL);

                var parametersNode = oDoc.CreateElement("parameters");
                this.Parameters.XmlAppend(parametersNode);
                node.AppendChild(parametersNode);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public override void XmlLoad(XmlNode node)
        {
            try
            {
                _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.SQL= XmlHelper.GetAttributeValue(node, "sql", string.Empty);

                var parametersNode = node.SelectSingleNode("parameters");
                if (parametersNode != null)
                    this.Parameters.XmlLoad(parametersNode);

                this.Dirty = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}
