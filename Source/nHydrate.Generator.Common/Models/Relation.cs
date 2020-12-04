#pragma warning disable 0168
using nHydrate.Generator.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class Relation : BaseModelObject
    {
        #region Member Variables

        protected const string _def_roleName = "";
        protected const string _def_constraintname = "";
        protected const bool _def_enforce = true;
        protected const string _def_description = "";
        protected const DeleteActionConstants _def_deleteAction = DeleteActionConstants.NoAction;

        #endregion

        #region Constructor

        public Relation(INHydrateModelObject root)
            : base(root)
        {
            this.Initialize();
        }

        public Relation()
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        private void Initialize() => ColumnRelationships = new ColumnRelationshipCollection(this.Root);

        protected override void OnRootReset(System.EventArgs e) => this.Initialize();

        #region Property Implementations

        /// <summary>
        /// EF only supports relations where the primary table is from the PK
        /// If the parent table is from a non-PK unique field, EF will NOT render it
        /// </summary>
        public bool IsValidEFRelation => this.ColumnRelationships.AsEnumerable().All(cr => cr.ParentColumn.PrimaryKey);

        public ColumnRelationshipCollection ColumnRelationships { get; protected set; } = null;

        public Reference ParentTableRef { get; set; }

        public Reference ChildTableRef { get; set; }

        public Table ParentTable => this.ParentTableRef.Object as Table;

        public Table ChildTable => this.ChildTableRef.Object as Table;

        public string RoleName { get; set; } = _def_roleName;

        public string ConstraintName { get; set; } = string.Empty;

        public bool IsRequired => this.ColumnRelationships.Any(x => !x.ChildColumn.AllowNull);

        public bool IsManyToMany
        {
            get
            {
                var otherTable = this.ChildTable.AssociativeTable ? this.ChildTable : this.ParentTable;
                if (otherTable.AssociativeTable)
                {
                    //The associative table must have exactly 2 relations
                    var relationList = otherTable.GetRelationsWhereChild();
                    if (relationList.Count() == 2) return true;
                }
                return false;
            }
        }

        public bool IsOneToOne
        {
            get
            {
                //If any of the columns are not unique then the relationship is NOT unique
                var retval = true;
                var childPKCount = 0; //Determine if any of the child columns are in the PK
                foreach (ColumnRelationship columnRelationship in this.ColumnRelationships)
                {
                    var column1 = columnRelationship.ParentColumn;
                    var column2 = columnRelationship.ChildColumn;
                    retval &= column1.IsUnique;
                    retval &= column2.IsUnique;
                    if (this.ChildTable.PrimaryKeyColumns.Contains(column2)) childPKCount++;
                }

                //If at least one column was a Child table PK, 
                //then all columns must be in there to be 1:1
                if ((childPKCount > 0) && (this.ColumnRelationships.Count != this.ChildTable.PrimaryKeyColumns.Count))
                {
                    return false;
                }

                return retval;
            }
        }

        public bool IsInherited => !this.IsOneToOne ? false : this.ChildTable.IsInheritedFrom(this.ParentTable);

        public bool Enforce { get; set; }

        public string Description { get; set; }

        public DeleteActionConstants DeleteAction { get; set; }

        #endregion

        #region Methods

        public bool IsInvalidRelation() => (this.ChildTable == null || this.ParentTable == null) ? true : false;

        public override bool Equals(object obj)
        {
            try
            {
                if (!(obj is Relation)) return false;
                var relationOther = (Relation)obj;

                if (this.IsInvalidRelation()) return false;
                if (relationOther.IsInvalidRelation()) return false;

                #region Check Parents
                var parentTableName1 = this.ParentTable.Name;
                var parentTableName2 = relationOther.ParentTable.Name;

                var list1 = new SortedDictionary<string, ColumnRelationship>();
                foreach (ColumnRelationship cr in this.ColumnRelationships)
                {
                    if (cr.ChildColumn != null)
                    {
                        var column = cr.ChildColumn;
                        if (!list1.ContainsKey(column.Name))
                            list1.Add(column.Name, cr);
                    }
                }

                var list2 = new SortedDictionary<string, ColumnRelationship>();
                foreach (ColumnRelationship cr in relationOther.ColumnRelationships)
                {
                    if (cr.ChildColumn != null)
                    {
                        var column = cr.ChildColumn;
                        if (!list2.ContainsKey(column.Name))
                            list2.Add(cr.ChildColumn.Name, cr);
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
                var childTableName1 = this.ChildTable.Name;
                var childTableName2 = relationOther.ChildTable.Name;

                var list3 = new SortedDictionary<string, ColumnRelationship>();
                foreach (ColumnRelationship cr in this.ColumnRelationships)
                {
                    if (cr.ParentColumn != null)
                    {
                        var column = cr.ParentColumn;
                        if (!list3.ContainsKey(column.Name))
                            list3.Add(column.Name, cr);
                    }
                }

                var list4 = new SortedDictionary<string, ColumnRelationship>();
                foreach (ColumnRelationship cr in relationOther.ColumnRelationships)
                {
                    if (cr.ParentColumn != null)
                    {
                        var column = cr.ParentColumn;
                        if (!list4.ContainsKey(column.Name))
                            list4.Add(column.Name, cr);
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

        public override int GetHashCode() => base.GetHashCode();

        #endregion

        public string CorePropertiesHash
        {
            get
            {
                var sb = new StringBuilder();
                this.ColumnRelationships.ToList().ForEach(x => sb.Append(x.CorePropertiesHash));

                var prehash =
                    this.RoleName + "|" +
                    sb.ToString();
                return prehash;
            }
        }

        #region IXMLable Members

        public override XmlNode XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            node.AddAttribute("key", this.Key);
            node.AddAttribute("enforce", this.Enforce);
            node.AddAttribute("description", this.Description, _def_description);
            node.AddAttribute("deleteAction", this.DeleteAction.ToString());
            ColumnRelationships.ResetKey(Guid.Empty, true); //no need to save this key
            node.AppendChild(ColumnRelationships.XmlAppend(oDoc.CreateElement("crl")));

            var childTableRefNode = oDoc.CreateElement("ct");
            if (this.ChildTableRef != null)
                this.ChildTableRef.XmlAppend(childTableRefNode);
            node.AppendChild(childTableRefNode);

            var parentTableRefNode = oDoc.CreateElement("pt");
            if (this.ParentTableRef != null)
                this.ParentTableRef.XmlAppend(parentTableRefNode);
            node.AppendChild(parentTableRefNode);

            node.AddAttribute("id", this.Id);
            node.AddAttribute("roleName", this.RoleName, _def_roleName);
            node.AddAttribute("constraintName", this.ConstraintName, _def_constraintname);

            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            this.Key = node.GetAttributeValue("key", string.Empty);
            this.Enforce = node.GetAttributeValue("enforce", _def_enforce);
            this.Description = node.GetAttributeValue("description", _def_description);
            this.DeleteAction = (DeleteActionConstants)Enum.Parse(typeof(DeleteActionConstants), XmlHelper.GetAttributeValue(node, "deleteAction", _def_deleteAction.ToString()));

            var columnRelationshipsNode = node.SelectSingleNode("crl");
            ColumnRelationships.XmlLoad(columnRelationshipsNode);

            var childTableRefNode = node.SelectSingleNode("ct");
            if (this.ChildTableRef == null) this.ChildTableRef = new Reference(this.Root);
            this.ChildTableRef.XmlLoad(childTableRefNode);

            var parentTableRefNode = node.SelectSingleNode("pt");
            if (this.ParentTableRef == null) this.ParentTableRef = new Reference(this.Root);
            this.ParentTableRef.XmlLoad(parentTableRefNode);

            this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

            var roleName = node.GetAttributeValue("roleName", _def_roleName);
            if (roleName == "fk") roleName = string.Empty; //Error correct from earlier versions
            this.RoleName = roleName;

            this.ConstraintName = node.GetAttributeValue("constraintName", _def_constraintname);

            return node;
        }

        #endregion

        #region Helpers

        public Reference CreateRef() => CreateRef(Guid.NewGuid().ToString());

        public Reference CreateRef(string key) => new Reference(this.Root, key) { Ref = this.Id, RefType = ReferenceType.Relation };

        public string PascalRoleName => StringHelper.FirstCharToUpper(this.RoleName);

        public string DatabaseRoleName => this.RoleName;

        public IEnumerable<Column> FkColumns
        {
            get
            {
                try
                {
                    var sorted = new SortedDictionary<string, Column>();
                    foreach (ColumnRelationship columnRel in this.ColumnRelationships)
                    {
                        sorted.Add($"{columnRel.ParentColumn.Name}|{columnRel.ChildColumn.Name}|{this.RoleName}|{columnRel.Key}", columnRel.ChildColumn);
                    }
                    return sorted.Select(x => x.Value).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public override string ToString()
        {
            var tableCollection = ((ModelRoot)this.Root).Database.Tables;
            Table[] parentList = { };
            Table[] childList = { };
            if (this.ParentTableRef != null)
                parentList = tableCollection.GetById(this.ParentTableRef.Ref);
            if (this.ChildTableRef != null)
                childList = tableCollection.GetById(this.ChildTableRef.Ref);

            var retval = string.Empty;
            retval = (this.RoleName == "" ? "" : this.RoleName + " ") + "[" + ((!parentList.Any()) ? "(Unknown)" : parentList[0].Name) + " -> " + ((!childList.Any()) ? "(Unknown)" : childList[0].Name) + "]";
            return retval;
        }

        #endregion

    }
}
