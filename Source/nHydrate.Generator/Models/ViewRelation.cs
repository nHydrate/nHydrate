#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class ViewRelation : BaseModelObject
    {
        #region Member Variables

        protected const string _def_roleName = "";
        protected const string _def_constraintname = "";
        protected const string _def_description = "";

        protected Reference _parentTableRef = null;
        protected Reference _childViewRef = null;
        protected string _roleName = _def_roleName;
        protected string _constraintName = string.Empty;
        protected ViewColumnRelationshipCollection _columnRelationships = null;
        private string _description = _def_description;

        #endregion

        #region Constructor

        public ViewRelation(INHydrateModelObject root)
            : base(root)
        {
            this.Initialize();
        }

        public ViewRelation()
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        private void Initialize()
        {
            _columnRelationships = new ViewColumnRelationshipCollection(this.Root);
        }

        protected override void OnRootReset(System.EventArgs e)
        {
            this.Initialize();
        }

        #region Events

        public event System.EventHandler BeforeChildTableChange;
        public event System.EventHandler BeforeParentTableChange;
        public event System.EventHandler AfterChildTableChange;
        public event System.EventHandler AfterParentTableChange;

        protected virtual void OnBeforeChildTableChange(object sender, System.EventArgs e)
        {
            if (this.BeforeChildTableChange != null)
                this.BeforeChildTableChange(sender, e);
        }

        protected virtual void OnBeforeParentTableChange(object sender, System.EventArgs e)
        {
            if (this.BeforeParentTableChange != null)
                this.BeforeParentTableChange(sender, e);
        }

        protected virtual void OnAfterChildTableChange(object sender, System.EventArgs e)
        {
            if (this.AfterChildTableChange != null)
                this.AfterChildTableChange(sender, e);
        }

        protected virtual void OnAfterParentTableChange(object sender, System.EventArgs e)
        {
            if (this.AfterParentTableChange != null)
                this.AfterParentTableChange(sender, e);
        }

        #endregion

        #region Property Implementations

        public ViewColumnRelationshipCollection ColumnRelationships
        {
            get { return _columnRelationships; }
        }

        public Reference ParentTableRef
        {
            get { return _parentTableRef; }
            set
            {
                if (_parentTableRef != value)
                {
                    this.OnBeforeParentTableChange(this, new EventArgs());
                    _parentTableRef = value;
                    this.RefreshRoleName();
                    this.OnAfterParentTableChange(this, new EventArgs());
                }
            }
        }

        public Reference ChildViewRef
        {
            get { return _childViewRef; }
            set
            {
                if (_childViewRef != value)
                {
                    this.OnBeforeChildTableChange(this, new EventArgs());
                    _childViewRef = value;
                    this.RefreshRoleName();
                    this.OnAfterChildTableChange(this, new EventArgs());
                }
            }
        }

        public string RoleName
        {
            get { return _roleName; }
            set
            {
                if (_roleName != value)
                {
                    _roleName = value;
                }
            }
        }

        public string ConstraintName
        {
            get { return _constraintName; }
            set
            {
                if (_constraintName != value)
                {
                    _constraintName = value;
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }

        #endregion

        #region Methods

        public bool IsInvalidRelation()
        {
            if (this.ChildViewRef == null) return true;
            if (this.ChildViewRef.Object == null) return true;
            if (this.ParentTableRef == null) return true;
            if (this.ParentTableRef.Object == null) return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            try
            {
                if (!(obj is Relation)) return false;

                if (this.IsInvalidRelation()) return false;

                #region Check Parents
                var parentTableName1 = this.ParentTable.Name;
                var parentTableName2 = this.ChildView.Name;

                var list1 = new SortedDictionary<string, ViewColumnRelationship>();
                foreach (var cr in this.ColumnRelationships.AsEnumerable())
                {
                    if (cr.ChildColumn != null)
                    {
                        if (!list1.ContainsKey(cr.ChildColumn.Name))
                            list1.Add(cr.ChildColumn.Name, cr);
                    }
                }

                var list2 = new SortedDictionary<string, ViewColumnRelationship>();
                foreach (var cr in this.ColumnRelationships.AsEnumerable())
                {
                    if (cr.ParentColumn != null)
                    {
                        if (!list2.ContainsKey(cr.ParentColumn.Name))
                            list2.Add(cr.ParentColumn.Name, cr);
                    }
                }

                var parentColName1 = string.Empty;
                foreach (var key in list1.Keys)
                {
                    parentColName1 += key;
                }

                var parentColName2 = string.Empty;
                foreach (var key in list2.Keys)
                {
                    parentColName2 += key;
                }
                #endregion

                #region Check Children
                var childTableName1 = this.ChildView.Name;
                var childTableName2 = this.ParentTable.Name;

                var list3 = new SortedDictionary<string, ViewColumnRelationship>();
                foreach (var cr in this.ColumnRelationships.AsEnumerable())
                {
                    if (cr.ParentColumn != null)
                    {
                        if (!list3.ContainsKey(cr.ParentColumn.Name))
                            list3.Add(cr.ParentColumn.Name, cr);
                    }
                }

                var list4 = new SortedDictionary<string, ViewColumnRelationship>();
                foreach (var cr in this.ColumnRelationships.AsEnumerable())
                {
                    if (cr.ChildColumn != null)
                    {
                        if (!list4.ContainsKey(cr.ChildColumn.Name))
                            list4.Add(cr.ChildColumn.Name, cr);
                    }
                }

                var childColName1 = string.Empty;
                foreach (var key in list3.Keys)
                {
                    childColName1 += key;
                }

                var childColName2 = string.Empty;
                foreach (var key in list4.Keys)
                {
                    childColName2 += key;
                }
                #endregion

                //string parentCol
                return (parentTableName1 == parentTableName2) &&
                    (parentColName1 == parentColName2) &&
                    (childTableName1 == childTableName2) &&
                    (childColName1 == childColName2);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Table ParentTable
        {
            get
            {
                if (this.ParentTableRef == null) return null;
                if (this.ParentTableRef.Object == null) return null;
                return this.ParentTableRef.Object as Table;
            }
        }

        public CustomView ChildView
        {
            get
            {
                if (this.ChildViewRef == null) return null;
                if (this.ChildViewRef.Object == null) return null;
                return this.ChildViewRef.Object as CustomView;
            }
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            XmlHelper.AddAttribute(node, "key", this.Key);

            if (this.Description != _def_description)
                XmlHelper.AddAttribute(node, "description", this.Description);

            var columnRelationshipsNode = oDoc.CreateElement("crl");
            ColumnRelationships.XmlAppend(columnRelationshipsNode);
            node.AppendChild(columnRelationshipsNode);

            var childTableRefNode = oDoc.CreateElement("ct");
            if (this.ChildViewRef != null)
                this.ChildViewRef.XmlAppend(childTableRefNode);
            node.AppendChild(childTableRefNode);

            var parentTableRefNode = oDoc.CreateElement("pt");
            if (this.ParentTableRef != null)
                this.ParentTableRef.XmlAppend(parentTableRefNode);
            node.AppendChild(parentTableRefNode);

            XmlHelper.AddAttribute(node, "id", this.Id);
            if (this.RoleName != _def_roleName)
                XmlHelper.AddAttribute(node, "roleName", this.RoleName);
            if (this.ConstraintName != _def_constraintname)
                XmlHelper.AddAttribute(node, "constraintName", this.ConstraintName);
        }

        public override void XmlLoad(XmlNode node)
        {
            this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            _description = XmlHelper.GetAttributeValue(node, "description", _def_description);

            var columnRelationshipsNode = node.SelectSingleNode("columnRelationships"); //deprecated, use "crl"
            if (columnRelationshipsNode == null)
                columnRelationshipsNode = node.SelectSingleNode("crl");
            ColumnRelationships.XmlLoad(columnRelationshipsNode);

            var childTableRefNode = node.SelectSingleNode("childTableRef"); //deprecated, use "ct"
            if (childTableRefNode == null) childTableRefNode = node.SelectSingleNode("ct");
            if (this.ChildViewRef == null) _childViewRef = new Reference(this.Root);
            this.ChildViewRef.XmlLoad(childTableRefNode);

            var parentTableRefNode = node.SelectSingleNode("parentTableRef"); //deprecated, use "pt"
            if (parentTableRefNode == null) parentTableRefNode = node.SelectSingleNode("pt");
            if (this.ParentTableRef == null) _parentTableRef = new Reference(this.Root);
            this.ParentTableRef.XmlLoad(parentTableRefNode);

            this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

            var roleName = XmlHelper.GetAttributeValue(node, "roleName", _def_roleName);
            if (roleName == "fk") roleName = string.Empty; //Error correct from earlier versions
            this.RoleName = roleName;

            this.ConstraintName = XmlHelper.GetAttributeValue(node, "constraintName", _def_constraintname);

            this.Dirty = false;
        }

        #endregion

        #region Helpers

        public Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.ViewRelation;
            return returnVal;
        }

        public override string ToString()
        {
            var tableCollection = ((ModelRoot)this.Root).Database.Tables;
            Table[] parentList = { };
            Table[] childList = { };
            if (this.ParentTableRef != null)
                parentList = tableCollection.GetById(this.ParentTableRef.Ref);
            if (this.ChildViewRef != null)
                childList = tableCollection.GetById(this.ChildViewRef.Ref);

            var retval = string.Empty;
            retval = (this.RoleName == "" ? "" : this.RoleName + " ") + "[" + ((parentList.Length == 0) ? "(Unknown)" : parentList[0].Name) + " -> " + ((childList.Length == 0) ? "(Unknown)" : childList[0].Name) + "]";
            return retval;
        }

        private void RefreshRoleName()
        {
            //try
            //{
            //  string newRoleName = string.Empty;
            //  if ((this.ParentTableRef != null) && (this.ChildTableRef != null))
            //  {
            //    newRoleName = ((Table)this.ParentTableRef.Object).Name + "_" + ((Table)this.ChildTableRef.Object).Name;
            //    Database database = ((ModelRoot)this.Root).Database;
            //    if (database.RelationRoleExists(newRoleName, this))
            //    {
            //      //If we are in there then need to loop and find a new name
            //      int ii = 1;
            //      while (database.RelationRoleExists(newRoleName + ii.ToString(), this))
            //        ii++;
            //      newRoleName = newRoleName + ii.ToString();
            //    }
            //  }
            //  this.RoleName = newRoleName;
            //}
            //catch (Exception ex)
            //{
            //  throw;
            //}
        }

        #endregion

    }
}