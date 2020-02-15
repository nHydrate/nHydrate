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
        Parameter = 5,
        CustomStoredProcedureColumn = 6,
        CustomAggregateColumn = 7,
        CustomView = 8,
        FunctionColumn = 9,
        FunctionParameter = 10,
        ViewRelation = 11,
    }

    public class Reference : BaseModelObject
    {
        #region Member Variables

        protected int _ref = 1;
        protected ReferenceType _refType = ReferenceType.Table;

        #endregion

        #region Constructor

        public Reference(INHydrateModelObject root)
            : base(root)
        {
        }

        #endregion

        #region Property Implementations

        public int Ref
        {
            get { return _ref; }
            set { _ref = value; }
        }

        public ReferenceType RefType
        {
            get { return _refType; }
            set { _refType = value; }
        }

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
                        case ReferenceType.ViewRelation:
                            retVal = modelRoot.Database.ViewRelations.GetById(Ref).FirstOrDefault();
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
                        case ReferenceType.Parameter:
                            retVal = modelRoot.Database.CustomRetrieveRuleParameters.GetById(Ref).FirstOrDefault();
                            break;
                        case ReferenceType.CustomStoredProcedureColumn:
                            retVal = modelRoot.Database.CustomStoredProcedureColumns.GetById(Ref).FirstOrDefault();
                            break;
                        case ReferenceType.FunctionColumn:
                            retVal = modelRoot.Database.FunctionColumns.GetById(Ref).FirstOrDefault();
                            break;
                        case ReferenceType.FunctionParameter:
                            retVal = modelRoot.Database.FunctionParameters.GetById(Ref).FirstOrDefault();
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

                XmlHelper.AddAttribute(node, "key", this.Key);
                XmlHelper.AddAttribute(node, "ref", this.Ref);
                XmlHelper.AddAttribute(node, "refType", (int)this.RefType);
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
                _ref = XmlHelper.GetAttributeValue(node, "ref", _ref);

                var refTypeNode = XmlHelper.GetAttributeValue(node, "refType", -1);
                if (refTypeNode != -1)
                    _refType = (ReferenceType)refTypeNode;

                this.Dirty = false;
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
                case ReferenceType.Parameter:
                    retval = modelRoot.Database.CustomRetrieveRuleParameters[Ref].ToString();
                    break;
                case ReferenceType.FunctionColumn:
                    retval = modelRoot.Database.FunctionColumns[Ref].ToString();
                    break;
                case ReferenceType.FunctionParameter:
                    retval = modelRoot.Database.FunctionParameters[Ref].ToString();
                    break;
            }
            return retval;
        }

        #endregion

    }

}