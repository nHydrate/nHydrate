#pragma warning disable 0168
using System;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public enum ReferenceType
    {
        Column = 0,
        Table = 1,
        Relation = 2,
        CustomViewColumn = 3,
        CustomView = 8,
    }

    public class Reference : BaseModelObject
    {
        #region Member Variables

        #endregion

        #region Constructor

        public Reference(INHydrateModelObject root)
            : base(root)
        {
        }

        public Reference()
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        #region Property Implementations

        public int Ref { get; set; } = 1;

        public ReferenceType RefType { get; set; } = ReferenceType.Table;

        public INHydrateModelObject Object
        {
            get
            {
                try
                {
                    var modelRoot = (ModelRoot)this.Root;
                    INHydrateModelObject retVal = null;
                    switch (this.RefType)
                    {
                        case ReferenceType.Column:
                            retVal = modelRoot.Database.Columns.GetById(Ref).FirstOrDefault();
                            break;
                        case ReferenceType.Relation:
                            retVal = modelRoot.Database.Relations.GetById(Ref).FirstOrDefault();
                            break;
                        case ReferenceType.Table:
                            retVal = modelRoot.Database.Tables.GetById(Ref).FirstOrDefault();
                            break;
                        case ReferenceType.CustomView:
                            retVal = modelRoot.Database.CustomViews.GetById(Ref).FirstOrDefault();
                            break;
                        case ReferenceType.CustomViewColumn:
                            retVal = modelRoot.Database.CustomViewColumns.GetById(Ref).FirstOrDefault();
                            break;
                        default:
                            throw new Exception("Cannot Handle Reference Type");
                    }
                    return retVal;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                node.AddAttribute("key", this.Key);
                node.AddAttribute("ref", this.Ref);
                node.AddAttribute("refType", (int)this.RefType);
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
                Ref = XmlHelper.GetAttributeValue(node, "ref", Ref);

                var refTypeNode = XmlHelper.GetAttributeValue(node, "refType", -1);
                if (refTypeNode != -1)
                    RefType = (ReferenceType)refTypeNode;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Helpers

        public override string ToString()
        {
            var modelRoot = (ModelRoot)this.Root;
            var retval = base.ToString();
            switch (this.RefType)
            {
                case ReferenceType.Column:
                    retval = modelRoot.Database.Columns[Ref].ToString();
                    break;
                case ReferenceType.Relation:
                    retval = modelRoot.Database.Relations.GetById(Ref).ToString();
                    break;
                case ReferenceType.Table:
                    retval = modelRoot.Database.Tables[Ref].ToString();
                    break;
                case ReferenceType.CustomView:
                    retval = modelRoot.Database.CustomViews[Ref].ToString();
                    break;
                case ReferenceType.CustomViewColumn:
                    retval = modelRoot.Database.CustomViewColumns[Ref].ToString();
                    break;
            }
            return retval;
        }

        #endregion

    }

}