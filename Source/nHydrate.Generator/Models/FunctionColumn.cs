#pragma warning disable 0168
using System;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class FunctionColumn : ColumnBase, ICodeFacadeObject
    {
        #region Member Variables

        protected const int _def_sortOrder = 0;
        protected const bool _def_UIVisible = false;
        protected const string _def_codefacade = "";
        protected const string _def_friendlyName = "";
        protected const string _def_default = "";

        protected string _codeFacade = _def_codefacade;
        protected string _default = _def_default;
        protected Reference _parentRef = null;
        protected Reference _relationshipRef = null;
        protected string _friendlyName = _def_friendlyName;
        protected int _sortOrder = _def_sortOrder;
        protected bool _UIVisible = _def_UIVisible;

        #endregion

        #region Constructor

        public FunctionColumn(INHydrateModelObject root)
            : base(root)
        {
        }

        public FunctionColumn()
            : base(null)
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        #region Property Implementations

        public Reference RelationshipRef
        {
            get { return _relationshipRef; }
            set { _relationshipRef = value; }
        }

        public string Default
        {
            get { return _default; }
            set
            {
                _default = value;
            }
        }

        public Reference ParentRef
        {
            get { return _parentRef; }
            set { _parentRef = value; }
        }

        public string FriendlyName
        {
            get { return _friendlyName; }
            set
            {
                _friendlyName = value;
            }
        }

        public int SortOrder
        {
            get { return _sortOrder; }
            set
            {
                _sortOrder = value;
            }
        }

        public bool UIVisible
        {
            get { return _UIVisible; }
            set
            {
                _UIVisible = value;
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

                if (this.Generated != _def_generated)
                    XmlHelper.AddAttribute((XmlElement)node, "generated", this.Generated);

                node.AddAttribute("name", this.Name);

                if (this.CodeFacade != _def_codefacade)
                    node.AddAttribute("codeFacade", this.CodeFacade);

                if (this.Description != _def_description)
                    node.AddAttribute("description", this.Description);

                if (this.FriendlyName != _def_friendlyName)
                    node.AddAttribute("dataFieldFriendlyName", this.FriendlyName);

                if (this.UIVisible != _def_UIVisible)
                    node.AddAttribute("dataFieldVisibility", this.UIVisible);

                if (this.SortOrder != _def_sortOrder)
                    node.AddAttribute("dataFieldSortOrder", this.SortOrder);

                if (this.Default != _def_default)
                    node.AddAttribute("default", this.Default);

                if (this.Length != _def_length)
                    node.AddAttribute("length", this.Length);

                node.AddAttribute("scale", this.Scale);
                node.AddAttribute("id", this.Id);

                if (this.SortOrder != _def_sortOrder)
                    node.AddAttribute("sortOrder", this.SortOrder);

                node.AddAttribute("type", (int)this.DataType);

                if (this.AllowNull != _def_allowNull)
                    node.AddAttribute("allowNull", this.AllowNull);

                if (this.IsBrowsable != _def_isBrowsable)
                    node.AddAttribute("isBrowsable", this.IsBrowsable);

                if (RelationshipRef != null)
                {
                    var relationshipRefNode = oDoc.CreateElement("relationshipRef");
                    RelationshipRef.XmlAppend(relationshipRefNode);
                    node.AppendChild(relationshipRefNode);
                }

                var parentRefNode = oDoc.CreateElement("parentTableRef");
                ParentRef.XmlAppend(parentRefNode);
                node.AppendChild(parentRefNode);

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
                this.Generated = XmlHelper.GetAttributeValue(node, "generated", _def_generated);
                this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
                this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codefacade);
                this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
                this.FriendlyName = XmlHelper.GetAttributeValue(node, "dataFieldFriendlyName", _def_friendlyName);
                this.UIVisible = XmlHelper.GetAttributeValue(node, "dataFieldVisibility", _def_UIVisible);
                this.SortOrder = XmlHelper.GetAttributeValue(node, "dataFieldSortOrder", _def_sortOrder);
                var relationshipRefNode = node.SelectSingleNode("relationshipRef");
                if (relationshipRefNode != null)
                {
                    RelationshipRef = new Reference(this.Root);
                    RelationshipRef.XmlLoad(relationshipRefNode);
                }

                this.Default = XmlHelper.GetAttributeValue(node, "default", _def_default);
                this.Length = XmlHelper.GetAttributeValue(node, "length", _length);
                this.Scale = XmlHelper.GetAttributeValue(node, "scale", _scale);
                this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

                var parentRefNode = node.SelectSingleNode("parentTableRef");
                ParentRef = new Reference(this.Root);
                if (parentRefNode != null)
                    ParentRef.XmlLoad(parentRefNode);

                var typeString = XmlHelper.GetAttributeValue(node, "type", string.Empty);
                if (!string.IsNullOrEmpty(typeString))
                    _dataType = (System.Data.SqlDbType)int.Parse(typeString);

                this.AllowNull = XmlHelper.GetAttributeValue(node, "allowNull", _allowNull);
                this.IsBrowsable = XmlHelper.GetAttributeValue(node, "isBrowsable", _def_isBrowsable);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region Helpers

        public override Reference CreateRef()
        {
            return CreateRef(Guid.NewGuid().ToString());
        }

        public override Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.FunctionColumn;
            return returnVal;
        }

        public override string PascalName
        {
            get
            {
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot) this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                if ((this.CodeFacade == string.Empty) && (((ModelRoot) this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot) this.Root).TransformNames))
                    return this.CodeFacade;
                if ((this.CodeFacade == string.Empty) && !(((ModelRoot) this.Root).TransformNames))
                    return this.Name;

                return this.Name; //Default
            }
        }

        #endregion

        #region ICodeFacadeObject Members

        public string CodeFacade
        {
            get { return _codeFacade; }
            set
            {
                _codeFacade = value;
            }
        }

        public string GetCodeFacade()
        {
            if (this.CodeFacade == "")
                return this.Name;
            else
                return this.CodeFacade;
        }

        #endregion

    }
}
