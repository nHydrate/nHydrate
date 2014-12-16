#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDesign = global::Microsoft.VisualStudio.Modeling.Design;
using System.ComponentModel;

namespace nHydrate.Dsl
{
    partial class Entity : nHydrate.Dsl.IModuleLink, nHydrate.Dsl.IDatabaseEntity, nHydrate.Dsl.IFieldContainer
    {
        public string CamelName
        {
            get { return StringHelper.DatabaseNameToCamelCase(this.PascalName); }
        }

        public string DatabaseName
        {
            get { return this.Name; }
        }

        protected override void OnCopy(DslModeling.ModelElement sourceElement)
        {
            base.OnCopy(sourceElement);
        }

        public string PascalName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.CodeFacade))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                else
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
            }
        }

        /// <summary>
        /// Get the full hierarchy of tables starting with this table 
        /// and working back to the most base table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity> GetTableHierarchy()
        {
            var retval = new List<Entity>();
            var t = this;
            while (t != null)
            {
                retval.Add(t);
                t = t.ParentInheritedEntity;
            }
            retval.Reverse();
            return retval;
        }

        /// <summary>
        /// Returns the generated columns for this table only (not hierarchy)
        /// </summary>
        public IEnumerable<Field> GeneratedColumns
        {
            get { return this.Fields.Where(x => x.IsGenerated).OrderBy(x => x.Name); }
        }

        /// <summary>
        /// Determines the fields that constitute the table primary key.
        /// </summary>
        public IList<Field> PrimaryKeyFields
        {
            get { return this.Fields.Where(x => x.IsPrimaryKey).ToList(); }
        }

        public IList<Index> IndexList
        {
            get
            {
                return this.Store.ElementDirectory.AllElements
                    .Where(x => x is Index)
                    .ToList()
                    .Cast<Index>()
                    .Where(x => x.Entity == this)
                    .ToList();
            }
        }

        public IList<EntityHasEntities> RelationshipList
        {
            get
            {
                return this.Store.ElementDirectory.AllElements
                    .Where(x => x is EntityHasEntities)
                    .ToList()
                    .Cast<EntityHasEntities>()
                    .Where(x => x.SourceEntity == this)
                    .ToList();
            }
        }

        public IList<EntityInheritsEntity> ParentRelationshipList
        {
            get
            {
                return this.Store.ElementDirectory.AllElements
                    .Where(x => x is EntityInheritsEntity)
                    .ToList()
                    .Cast<EntityInheritsEntity>()
                    .Where(x => x.ChildDerivedEntities == this)
                    .ToList();
            }
        }

        public IList<EntityHasViews> RelationshipViewList
        {
            get
            {
                return this.Store.ElementDirectory.AllElements
                    .Where(x => x is EntityHasViews)
                    .ToList()
                    .Cast<EntityHasViews>()
                    .Where(x => x.ParentEntity == this)
                    .ToList();
            }
        }

        /// <summary>
        /// Returns generated relations for this table
        /// </summary>
        public IEnumerable<EntityHasEntities> GetRelationsWhereChild()
        {
            return GetRelationsWhereChild(false);
        }

        /// <summary>
        /// Returns generated relations for this table
        /// </summary>
        public IEnumerable<EntityHasEntities> GetRelationsWhereChild(bool fullHierarchy)
        {
            var retval = this.nHydrateModel.GetRelationsWhereChild(this, fullHierarchy);
            return retval.Where(x => x.TargetEntity.IsGenerated && x.SourceEntity.IsGenerated);
        }

        /// <summary>
        /// Determines if the specified table is an ancestor of the this table object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsInheritedFrom(Entity entity)
        {
            var t = this.ParentInheritedEntity;
            while (t != null)
            {
                if (t == entity) return true;
                t = t.ParentInheritedEntity;
            }
            return false;
        }

        /// <summary>
        /// Returns a list of all parent and child relations
        /// </summary>
        public IList<EntityHasEntities> AllRelationships
        {
            get
            {
                var retval = new List<EntityHasEntities>();
                foreach (var r in this.Store.ElementDirectory.AllElements.Where(x => x is EntityHasEntities).Cast<EntityHasEntities>())
                {
                    if ((r.SourceEntity != null) && (r.TargetEntity != null))
                    {
                        if ((r.SourceEntity == this) || (r.TargetEntity == this))
                        {
                            retval.Add(r);
                        }
                    }
                }
                return retval;
            }
        }

        /// <summary>
        /// This gets all columns from this and all base classes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Field> GetColumnsFullHierarchy()
        {
            return GetColumnsFullHierarchy(true);
        }

        /// <summary>
        /// This gets all columns from this and all base classes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Field> GetColumnsFullHierarchy(bool includeCurrent)
        {
            try
            {
                var nameList = new List<string>();
                var retval = new List<Field>();
                var t = this;
                if (!includeCurrent) t = t.ParentInheritedEntity;
                while (t != null)
                {
                    foreach (var c in t.Fields)
                    {
                        if (!nameList.Contains(c.Name.ToLower()))
                        {
                            nameList.Add(c.Name.ToLower());
                            retval.Add(c);
                        }
                    }
                    t = t.ParentInheritedEntity;
                }
                return retval;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Given a field from this table a search is performed to get the same column from all base tables down the inheritance hierarchy
        /// </summary>
        public IEnumerable<Field> GetBasePKColumnList(Field column)
        {
            if (column == null)
                throw new Exception("The column cannot be null.");
            if (this.PrimaryKeyFields.Count(x => x.Name == column.Name) == 0)
                throw new Exception("The column does not belong to this table.");

            var retval = new List<Field>();
            var tList = GetTableHierarchy();
            foreach (var table in tList)
            {
                column = table.PrimaryKeyFields.FirstOrDefault(x => x.Name == column.Name);
                if (column == null)
                    break;
                else
                    retval.Add(column);
            }
            return retval;
        }

        /// <summary>
        /// Given a field from this table a search is performed to get the same column from the base table if one exists
        /// </summary>
        public Field GetBasePKColumn(Field column)
        {
            if (column == null)
                throw new Exception("The column cannot be null.");
            if (this.PrimaryKeyFields.Count(x => x.Name == column.Name) == 0)
                throw new Exception("The column does not belong to this table.");

            var tList = new List<Entity>(GetTableHierarchy());
            tList.Add(this);
            return tList.First().PrimaryKeyFields.FirstOrDefault(x => x.Name == column.Name);
        }

        /// <summary>
        /// Returns all primary keys from the ultimate ancestor in the table hierarchy
        /// </summary>
        public IEnumerable<Field> GetBasePKColumnList()
        {
            var retval = new List<Field>();
            var tList = new List<Entity>(GetTableHierarchy());
            tList.Add(this);
            foreach (var column in this.PrimaryKeyFields)
            {
                retval.Add(tList.First().PrimaryKeyFields.First(x => x.Name == column.Name));
            }
            return retval;
        }

        public IEnumerable<EntityHasEntities> GetRelationsFullHierarchy()
        {
            try
            {
                var allRelations = new List<EntityHasEntities>();
                var allTables = this.GetTableHierarchy();
                foreach (var entity in allTables)
                {
                    foreach (var relation in entity.AllRelationships)
                    {
                        allRelations.Add(relation);
                    }
                }
                return allRelations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool IsValidInheritance
        {
            get
            {
                var inheritTables = new List<Entity>(this.GetTableHierarchy());
                var pkList = new Dictionary<string, Field>();
                foreach (var c in this.PrimaryKeyFields.OrderBy(x => x.Name))
                {
                    if (pkList.ContainsKey(c.Name)) return true;
                    pkList.Add(c.Name, c);
                }

                //Ensure that all tables have the same primary keys
                foreach (var t in inheritTables)
                {
                    if (t.PrimaryKeyFields.Count != this.PrimaryKeyFields.Count)
                    {
                        //Different number of pk columns so invalid
                        return false;
                    }
                    else
                    {
                        foreach (var c in t.PrimaryKeyFields.OrderBy(x => x.Name))
                        {
                            if (!pkList.ContainsKey(c.Name))
                                return false;
                            if (pkList[c.Name].DataType != c.DataType)
                                return false;
                        }
                    }
                }

                //Ensure that all tables in inheritance hierarchy
                //do not have duplicate column names except primary keys
                var columNames = new List<string>();
                foreach (var t in inheritTables)
                {
                    foreach (var c in t.Fields)
                    {
                        //Make sure this is not a PK
                        if (!pkList.ContainsKey(c.Name))
                        {
                            //If the column already exists then it is a duplicate
                            if (columNames.Contains(c.Name))
                                return false;

                            columNames.Add(c.Name);
                        }
                    }
                }

                return true;
            }
        }

        public override bool Immutable
        {
            get
            {
                if (this.TypedEntity != TypedEntityConstants.None) return true;
                return base.Immutable;
            }
            set
            {
                base.Immutable = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        #region IModuleLink

        IEnumerable<Module> IModuleLink.Modules
        {
            get { return this.Modules.AsEnumerable(); }
        }

        void IModuleLink.AddModule(Module module)
        {
            if (!this.Modules.Contains(module))
                this.Modules.Add(module);
        }

        void IModuleLink.RemoveModule(Module module)
        {
            if (this.Modules.Contains(module))
                this.Modules.Remove(module);
        }

        #endregion

        protected override void OnDeleting()
        {
            if (this.nHydrateModel != null)
                this.nHydrateModel.RemovedTables.Add(this.PascalName);

            foreach (var index in this.Indexes)
            {
                var count1 = this.nHydrateModel.IndexModules.Remove(x => x.IndexID == index.Id);
            }


            base.OnDeleting();
        }


        #region IFieldContainer Members

        public IEnumerable<IField> FieldList
        {
            get { return this.Fields; }
        }

        #endregion
    }

    partial class EntityBase
    {
        private bool CanMergeStaticData(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
        {
            return false;
        }

        private bool CanMergeSelectCommand(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
        {
            return false;
        }

        private bool CanMergeComposite(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
        {
            return false;
        }

        partial class NamePropertyHandler
        {
            protected override void OnValueChanged(EntityBase element, string oldValue, string newValue)
            {
                if (element.nHydrateModel != null && !element.nHydrateModel.IsLoading)
                {
                    if (string.IsNullOrEmpty(newValue))
                        throw new Exception("The name must have a value.");

                    var count = element.nHydrateModel.Entities.Count(x => x.Name.ToLower() == newValue.ToLower() && x.Id != element.Id);
                    if (count > 0)
                        throw new Exception("There is already an object with the specified name. The change has been cancelled.");
                }
                base.OnValueChanged(element, oldValue, newValue);
            }
        }

        string GetSecurityValue()
        {
            return (this.SecurityFunction != null) ? "(Has Security)" : "";
        }

        void SetSecurityValue(string v)
        {
        }

    }

    [RefreshProperties(RefreshProperties.All)]
    partial class SecurityFunction
    {
        public override string ToString()
        {
            return this.SQL;
        }
    }

    [RefreshProperties(RefreshProperties.All)]
    partial class SecurityFunctionParameter
    {
        public override string ToString()
        {
            return this.Name;
        }
    }
}