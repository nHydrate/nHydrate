#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;
using DslModeling = global::Microsoft.VisualStudio.Modeling;

namespace nHydrate.Dsl
{
    partial class nHydrateModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public nHydrateModel(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public nHydrateModel(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            this.SyncServerToken = Guid.Empty;
            this.IsLoading = true;
            this.Refactorizations = new List<Objects.IRefactor>();
            this.DiagramVisibility = VisibilityTypeConstants.Function | VisibilityTypeConstants.StoredProcedure | VisibilityTypeConstants.View;

            this.RemovedTables = new List<string>();
            this.RemovedViews = new List<string>();
            this.RemovedStoredProcedures = new List<string>();
            this.RemovedFunctions = new List<string>();
        }

        public override bool ModelToDisk { get => true; }

        public List<nHydrate.Dsl.Objects.IRefactor> Refactorizations { get; set; }

        //protected internal bool IsLoading { get; set; }
        private bool _IsLoading = false;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; }
        }

        public bool IsSaving { get; set; }

        public IEnumerable<EntityHasEntities> GetRelationsWhereChild(Entity entity)
        {
            var retval = new List<EntityHasEntities>();
            foreach (var relation in this.AllRelations)
            {
                var childTable = relation.TargetEntity;
                if (childTable == entity)
                    retval.Add(relation);
            }
            return retval;
        }

        public IList<EntityHasEntities> AllRelations
        {
            get
            {
                return this.Store.ElementDirectory.AllElements
                .Where(x => x is EntityHasEntities)
                .ToList()
                .Cast<EntityHasEntities>()
                .ToList();
            }
        }

        public IList<EntityHasEntities> FindByParentTable(Entity entity)
        {
            return FindByParentTable(entity, false);
        }

        /// <summary>
        /// Find all relationships that have a specific parent table
        /// </summary>
        /// <param name="entity">The table from which all relations begin</param>
        /// <param name="includeHierarchy">Determines if the return includes all tables in the inheritence tree</param>
        /// <returns></returns>
        public IList<EntityHasEntities> FindByParentTable(Entity entity, bool includeHierarchy)
        {
            var tableList = new List<Entity>();
            var columnList = new List<Field>();
            if (includeHierarchy)
            {
                tableList.AddRange(new List<Entity>(entity.GetTableHierarchy()));
                foreach (var t in tableList)
                {
                    foreach (var column in (from x in t.GetColumnsFullHierarchy() select x))
                    {
                        columnList.Add(column);
                    }
                }
            }
            else
            {
                tableList = new List<Entity>();
                tableList.Add(entity);
                columnList.AddRange(entity.Fields);
            }

            var retval = new List<EntityHasEntities>();
            foreach (var relation in this.Store.ElementDirectory.AllElements.Where(x => x is EntityHasEntities).Cast<EntityHasEntities>())
            {
                var parentTable = relation.SourceEntity;
                foreach (var columnRelationship in relation.FieldMapList())
                {
                    foreach (var column in columnList)
                    {
                        if (tableList.Contains(parentTable))
                        {
                            if (!retval.Contains(relation))
                                retval.Add(relation);
                        }
                    }
                }
            }

            return retval.AsReadOnly();
        }

        public IList<EntityHasEntities> FindByChildTable(Entity entity)
        {
            return FindByChildTable(entity, false);
        }

        /// <summary>
        /// Find all relationships that have a specific child table
        /// </summary>
        /// <param name="entity">The table from which all relations begin</param>
        /// <param name="includeHierarchy">Determines if the return includes all tables in the inheritence tree</param>
        /// <returns></returns>
        public IList<EntityHasEntities> FindByChildTable(Entity entity, bool includeHierarchy)
        {
            try
            {
                var retval = new List<EntityHasEntities>();
                var tableList = new List<Entity>();
                var columnList = new List<Field>();
                if (includeHierarchy)
                {
                    tableList.AddRange(entity.GetTableHierarchy());
                    foreach (var t in tableList)
                    {
                        foreach (var column in (from x in t.GetColumnsFullHierarchy() select x))
                        {
                            columnList.Add(column);
                        }
                    }
                }
                else
                {
                    tableList = new List<Entity>();
                    tableList.Add(entity);
                    columnList.AddRange(entity.Fields);
                }

                foreach (var relation in this.Store.ElementDirectory.AllElements.Where(x => x is EntityHasEntities).Cast<EntityHasEntities>())
                {
                    var childTable = relation.TargetEntity;
                    foreach (var columnRelationship in relation.FieldMapList())
                    {
                        foreach (var column in columnList)
                        {
                            if (tableList.Contains(childTable))
                            {
                                if (!retval.Contains(relation))
                                    retval.Add(relation);
                            }
                        }
                    }
                }

                return retval.AsReadOnly();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string CreatedByPascalName
        {
            get { return StringHelper.DatabaseNameToPascalCase(this.CreatedByColumnName); }
        }

        public string CreatedDatePascalName
        {
            get { return StringHelper.DatabaseNameToPascalCase(this.CreatedDateColumnName); }
        }

        public string ModifiedByPascalName
        {
            get { return StringHelper.DatabaseNameToPascalCase(this.ModifiedByColumnName); }
        }

        public string ModifiedDatePascalName
        {
            get { return StringHelper.DatabaseNameToPascalCase(this.ModifiedDateColumnName); }
        }

        public string TimestampPascalName
        {
            get { return StringHelper.DatabaseNameToPascalCase(this.TimestampColumnName); }
        }

        public List<string> RemovedTables { get; private set; }
        public List<string> RemovedViews { get; private set; }
        public List<string> RemovedStoredProcedures { get; private set; }
        public List<string> RemovedFunctions { get; private set; }

        /// <summary>
        /// The URL to the SyncServer service
        /// </summary>
        public string SyncServerURL { get; set; }
        public Guid SyncServerToken { get; set; }
        public string ModelFileName { get; set; }

        /// <summary>
        /// This willbe used to track version with an nHydrate Server
        /// </summary>
        public long ServerVersion { get; set; }

    }

    partial class nHydrateModelBase
    {
        private bool CanMergeRelationField(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
        {
            return false;
        }

        private bool CanMergeRelationModule(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
        {
            return false;
        }

        private bool CanMergeIndexModule(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
        {
            return false;
        }

    }

}
