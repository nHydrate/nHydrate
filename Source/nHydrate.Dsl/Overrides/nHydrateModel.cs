#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using nHydrate.Generator.Common.Util;
using DslModeling = global::Microsoft.VisualStudio.Modeling;

namespace nHydrate.Dsl
{
    partial class nHydrateModel
    {
        //Constructors were not generated for this class because it had HasCustomConstructor
        //set to true. Please provide the constructors below in a partial class.
        public nHydrateModel(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
        }

        public nHydrateModel(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            this.SyncServerToken = Guid.Empty;
            this.IsLoading = true;
            this.RemovedTables = new List<string>();
            this.RemovedViews = new List<string>();
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

        public string CreatedByPascalName => StringHelper.DatabaseNameToPascalCase(this.CreatedByColumnName);

        public string CreatedDatePascalName => StringHelper.DatabaseNameToPascalCase(this.CreatedDateColumnName);

        public string ModifiedByPascalName => StringHelper.DatabaseNameToPascalCase(this.ModifiedByColumnName);

        public string ModifiedDatePascalName => StringHelper.DatabaseNameToPascalCase(this.ModifiedDateColumnName);

        public string TimestampPascalName => StringHelper.DatabaseNameToPascalCase(this.TimestampColumnName);

        public List<string> RemovedTables { get; }
        public List<string> RemovedViews { get; }

        public Guid SyncServerToken { get; set; }
        public string ModelFileName { get; set; }
    }

    partial class nHydrateModelBase
    {
        private bool CanMergeRelationField(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
        {
            return false;
        }
    }

}
