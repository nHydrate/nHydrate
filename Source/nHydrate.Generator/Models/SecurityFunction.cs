#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
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

        public ReferenceCollection Parameters
        {
            get { return _parameters; }
        }

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
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.SQL= XmlHelper.GetAttributeValue(node, "sql", string.Empty);

                var parametersNode = node.SelectSingleNode("parameters");
                if (parametersNode != null)
                    this.Parameters.XmlLoad(parametersNode);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}
