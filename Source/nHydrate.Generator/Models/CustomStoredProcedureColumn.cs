#pragma warning disable 0168
using System;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class CustomStoredProcedureColumn : ColumnBase, ICodeFacadeObject
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
        private string _enumType = string.Empty;

        #endregion

        #region Constructor

        public CustomStoredProcedureColumn(INHydrateModelObject root)
            : base(root)
        {
        }

        public CustomStoredProcedureColumn()
            :base(null)
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
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Default"));
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
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("friendlyName"));
            }
        }

        public int SortOrder
        {
            get { return _sortOrder; }
            set
            {
                _sortOrder = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("sortOrder"));
            }
        }

        public bool UIVisible
        {
            get { return _UIVisible; }
            set
            {
                _UIVisible = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("UIVisible"));
            }
        }

        internal string EnumType
        {
            get { return _enumType; }
            set { _enumType = value; }
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                if (this.Generated != _def_generated)
                    XmlHelper.AddAttribute((XmlElement)node, "generated", this.Generated);

                XmlHelper.AddAttribute(node, "name", this.Name);

                if (this.CodeFacade != _def_codefacade)
                    XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

                if (this.Description != _def_description)
                    XmlHelper.AddAttribute(node, "description", this.Description);

                if (this.FriendlyName != _def_friendlyName)
                    XmlHelper.AddAttribute(node, "dataFieldFriendlyName", this.FriendlyName);

                if (this.UIVisible != _def_UIVisible)
                    XmlHelper.AddAttribute(node, "dataFieldVisibility", this.UIVisible);

                if (this.SortOrder != _def_sortOrder)
                    XmlHelper.AddAttribute(node, "dataFieldSortOrder", this.SortOrder);

                if (this.Default != _def_default)
                    XmlHelper.AddAttribute(node, "default", this.Default);

                if (this.Length != _def_length)
                    XmlHelper.AddAttribute(node, "length", this.Length);

                XmlHelper.AddAttribute(node, "scale", this.Scale);
                XmlHelper.AddAttribute(node, "id", this.Id);

                if (this.SortOrder != _def_sortOrder)
                    XmlHelper.AddAttribute(node, "sortOrder", this.SortOrder);

                XmlHelper.AddAttribute(node, "type", (int)this.DataType);

                if (this.AllowNull != _def_allowNull)
                    XmlHelper.AddAttribute(node, "allowNull", this.AllowNull);

                if (this.IsBrowsable != _def_isBrowsable)
                    XmlHelper.AddAttribute(node, "isBrowsable", this.IsBrowsable);

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

                this.Dirty = false;
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
            returnVal.RefType = ReferenceType.CustomStoredProcedureColumn;
            return returnVal;
        }

        public override string PascalName
        {
            get
            {
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                else if ((this.CodeFacade == "") && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot)this.Root).TransformNames))
                    return this.CodeFacade;
                else if ((this.CodeFacade == "") && !(((ModelRoot)this.Root).TransformNames))
                    return this.Name;
                return this.Name; //Default
            }
        }

        public override string GetCodeType(bool allowNullable)
        {
            var retval = string.Empty;
            if (!string.IsNullOrEmpty(this.EnumType))
            {
                retval = this.EnumType;
                if (allowNullable && this.AllowNull)
                    retval += "?";
                return retval;
            }
            else
            {
                return base.GetCodeType(allowNullable);
            }
        }

        #endregion

        public override string CorePropertiesHash
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.AllowNull + "|" +
                    this.Default + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.DataType.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        #region ICodeFacadeObject Members

        public string CodeFacade
        {
            get { return _codeFacade; }
            set
            {
                _codeFacade = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("codeFacade"));
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
