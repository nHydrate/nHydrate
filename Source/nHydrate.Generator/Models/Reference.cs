#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public enum ReferenceType
    {
        Column = 0,
        Table = 1,
        Relation = 2,
        CustomViewColumn = 3,
        CustomRetrieveRule = 4,
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

        [Browsable(false)]
        public int Ref
        {
            get { return _ref; }
            set { _ref = value; }
        }

        [Browsable(false)]
        public ReferenceType RefType
        {
            get { return _refType; }
            set { _refType = value; }
        }

        [Browsable(false)]
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
                            retVal = modelRoot.Database.Columns[Ref];
                            break;
                        case ReferenceType.Relation:
                            retVal = modelRoot.Database.Relations.GetById(Ref);
                            break;
                        case ReferenceType.ViewRelation:
                            retVal = modelRoot.Database.ViewRelations.GetById(Ref);
                            break;
                        case ReferenceType.Table:
                            retVal = modelRoot.Database.Tables[Ref];
                            break;
                        case ReferenceType.CustomView:
                            retVal = modelRoot.Database.CustomViews[Ref];
                            break;
                        case ReferenceType.CustomViewColumn:
                            retVal = modelRoot.Database.CustomViewColumns[Ref];
                            break;
                        case ReferenceType.CustomRetrieveRule:
                            retVal = modelRoot.Database.CustomRetrieveRules[Ref];
                            break;
                        case ReferenceType.Parameter:
                            retVal = modelRoot.Database.CustomRetrieveRuleParameters[Ref];
                            break;
                        case ReferenceType.CustomStoredProcedureColumn:
                            retVal = modelRoot.Database.CustomStoredProcedureColumns[Ref];
                            break;
                        case ReferenceType.FunctionColumn:
                            retVal = modelRoot.Database.FunctionColumns[Ref];
                            break;
                        case ReferenceType.FunctionParameter:
                            retVal = modelRoot.Database.FunctionParameters[Ref];
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
                _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
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
                case ReferenceType.CustomRetrieveRule:
                    retval = modelRoot.Database.CustomRetrieveRules[Ref].ToString();
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