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
            this.DiagramVisibility = VisibilityTypeConstants.Function | VisibilityTypeConstants.StoredProcedure | VisibilityTypeConstants.View;

            this.RemovedTables = new List<string>();
            this.RemovedViews = new List<string>();
            this.RemovedStoredProcedures = new List<string>();
            this.RemovedFunctions = new List<string>();
        }

        public override bool ModelToDisk { get => true; }

        public bool IsLoading { get; set; } = false;

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

        public List<string> RemovedTables { get; }
        public List<string> RemovedViews { get; }
        public List<string> RemovedStoredProcedures { get; }
        public List<string> RemovedFunctions { get; }

        public Guid SyncServerToken { get; set; }
        public string ModelFileName { get; set; }

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
