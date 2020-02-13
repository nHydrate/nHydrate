#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
        protected const bool _def_enforce = true;
        protected const string _def_description = "";

        protected int _id = 1;
        protected Reference _parentTableRef = null;
        protected Reference _childViewRef = null;
        protected string _roleName = _def_roleName;
        protected string _constraintName = string.Empty;
        protected ViewColumnRelationshipCollection _columnRelationships = null;
        //private DateTime _createdDate = DateTime.Now;
        private string _description = _def_description;

        #endregion

        #region Constructor

        public ViewRelation(INHydrateModelObject root)
            : base(root)
        {
            _columnRelationships = new ViewColumnRelationshipCollection(this.Root);
        }

        #endregion

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

        /// <summary>
        /// EF only supports relations where the primary table is from the PK
        /// If the parent table is from a non-PK unique field, EF will NOT render it
        /// </summary>
        public bool IsValidEFRelation
        {
            get { return this.ColumnRelationships.AsEnumerable().All(cr => cr.ParentColumn.PrimaryKey); }
        }

        /// <summary>
        /// Determines the field mappings of this relationship.
        /// </summary>
        [Description("Determines the field mappings of this relationship.")]
        [Category("Data")]
        public ViewColumnRelationshipCollection ColumnRelationships
        {
            get { return _columnRelationships; }
        }

        /// <summary>
        /// Determines the parent table in the relationship.
        /// </summary>
        [Browsable(false)]
        [Description("Determines the parent table in the relationship.")]
        [Category("Data")]
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
                    this.OnPropertyChanged(this, new PropertyChangedEventArgs("parentTableRef"));
                }
            }
        }

        /// <summary>
        /// Determines the child table in the relationship.
        /// </summary>
        [Description("Determines the child view in the relationship.")]
        [Category("Data")]
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
                    this.OnPropertyChanged(this, new PropertyChangedEventArgs("childTableRef"));
                }
            }
        }

        /// <summary>
        /// Determines the unique id of this object.
        /// </summary>
        [Browsable(false)]
        public int Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Determines the database role name of this relation.
        /// </summary>
        [Description("Determines the database role name of this relation.")]
        [Category("Data")]
        [DefaultValue(_def_roleName)]
        public string RoleName
        {
            get { return _roleName; }
            set
            {
                if (_roleName != value)
                {
                    _roleName = value;
                    this.OnPropertyChanged(this, new PropertyChangedEventArgs("RoleName"));
                }
            }
        }

        [Browsable(false)]
        public string ConstraintName
        {
            get { return _constraintName; }
            set
            {
                if (_constraintName != value)
                {
                    _constraintName = value;
                    this.OnPropertyChanged(this, new PropertyChangedEventArgs("ConstraintName"));
                }
            }
        }

        /// <summary>
        /// Determines if this is a 1:1 relationship
        /// </summary>
        [Browsable(false)]
        public bool IsOneToOne
        {
            get
            {
                //If any of the columns are not unique then the relationship is NOT unique
                var retval = true;
                var childPKCount = 0; //Determine if any of the child columns are in the PK
                foreach (var columnRelationship in this.ColumnRelationships.AsEnumerable())
                {
                    var column1 = columnRelationship.ParentColumn;
                    var column2 = columnRelationship.ChildColumn;
                    if (this.ChildView.PrimaryKeyColumns.Contains(column2)) childPKCount++;
                }

                //If at least one column was a Child table PK, 
                //then all columns must be in there to be 1:1
                if ((childPKCount > 0) && (this.ColumnRelationships.Count != this.ChildView.PrimaryKeyColumns.Count))
                {
                    return false;
                }

                return retval;
            }
        }

        [Browsable(false)]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        /// <summary>
        /// A hash of the table/columns of this relationship with no role information
        /// </summary>
        [Browsable(false)]
        public string LinkHash
        {
            get
            {
                var retval = string.Empty;
                if (this.ParentTable != null) retval += this.ParentTable.Name.ToLower() + "|";
                if (this.ChildView != null) retval += this.ChildView.Name.ToLower() + "|";
                foreach (var cr in this.ColumnRelationships.AsEnumerable())
                {
                    if (cr.ParentColumn != null) retval += cr.ParentColumn.Name.ToLower() + "|";
                    if (cr.ChildColumn != null) retval += cr.ChildColumn.Name.ToLower() + "|";
                }
                return retval;
            }
        }

        public int UniqueHash
        {
            get { return (this.LinkHash + "|" + this.RoleName).GetHashCode(); }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Determines that all columns in the relationship are generated
        /// </summary>
        public bool IsGenerated
        {
            get
            {
                var retval = true;
                foreach (var columnRelationship in this.ColumnRelationships.AsEnumerable())
                {
                    var childColumn = columnRelationship.ChildColumn;
                    var parentColumn = columnRelationship.ParentColumn;
                    retval &= childColumn.Generated;
                    retval &= parentColumn.Generated;
                }
                return retval;
            }
        }

        public string ToLongString()
        {
            try
            {
                var col1 = this.ColumnRelationships.First().ParentColumn;
                var col2 = this.ColumnRelationships.First().ChildColumn;
                var retval = (this.RoleName == "" ? "" : this.RoleName + " ");
                retval += col1.ParentTable.Name + ".";
                retval += col1.ToString();
                retval += "->";
                retval += ((CustomView)col2.ParentViewRef.Object).Name + ".";
                retval += col2.ToString();
                return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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

        /// <summary>
        /// Get the parent table of this relation
        /// </summary>
        /// <returns></returns>
        public Table ParentTable
        {
            get
            {
                if (this.ParentTableRef == null) return null;
                if (this.ParentTableRef.Object == null) return null;
                return this.ParentTableRef.Object as Table;
            }
        }

        /// <summary>
        /// Get the child table of this relation
        /// </summary>
        /// <returns></returns>
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
            _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
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

            this.ResetId(XmlHelper.GetAttributeValue(node, "id", _id));

            var roleName = XmlHelper.GetAttributeValue(node, "roleName", _def_roleName);
            if (roleName == "fk") roleName = string.Empty; //Error correct from earlier versions
            this.RoleName = roleName;

            this.ConstraintName = XmlHelper.GetAttributeValue(node, "constraintName", _def_constraintname);

            this.Dirty = false;
        }

        #endregion

        #region Helpers

        public Reference CreateRef()
        {
            return CreateRef(Guid.NewGuid().ToString());
        }

        public Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.ViewRelation;
            return returnVal;
        }

        [Browsable(false)]
        public string PascalRoleName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(RoleName);
                else
                    return StringHelper.FirstCharToUpper(this.RoleName);
            }
        }

        [Browsable(false)]
        public string CamelRoleName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToCamelCase(RoleName);
                else
                    return StringHelper.FirstCharToLower(this.RoleName);
            }
        }

        [Browsable(false)]
        public string DatabaseRoleName
        {
            get { return this.RoleName; }
        }

        [Browsable(false)]
        public IEnumerable<CustomViewColumn> FkColumns
        {
            get
            {
                try
                {
                    var sorted = new SortedDictionary<string, CustomViewColumn>();
                    foreach (var columnRel in this.ColumnRelationships.AsEnumerable())
                    {
                        var parentColumn = columnRel.ParentColumn;
                        var childColumn = columnRel.ChildColumn;
                        sorted.Add(parentColumn.Name + "|" + childColumn.Name + "|" + this.RoleName + "|" + columnRel.Key, childColumn);
                    }

                    var fkColumns = new List<CustomViewColumn>();
                    foreach (var kvp in sorted)
                    {
                        fkColumns.Add(kvp.Value);
                    }
                    return fkColumns;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public void ResetId(int newId)
        {
            _id = newId;
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