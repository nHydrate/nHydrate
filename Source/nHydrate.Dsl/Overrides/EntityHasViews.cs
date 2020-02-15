using System;
using System.Collections.Generic;
using System.Linq;
using nHydrate.Generator.Common.Util;
using DslModeling = global::Microsoft.VisualStudio.Modeling;

namespace nHydrate.Dsl
{
    partial class EntityHasViews
    {
        #region Constructors
        public EntityHasViews(Entity source, View target)
            : base((source != null ? source.Partition : null), new DslModeling::RoleAssignment[] { new DslModeling::RoleAssignment(EntityHasViews.ParentEntityDomainRoleId, source), new DslModeling::RoleAssignment(EntityHasViews.ChildViewDomainRoleId, target) }, null)
        {
            this.InternalId = Guid.NewGuid();
        }

        public EntityHasViews(DslModeling::Store store, params DslModeling::RoleAssignment[] roleAssignments)
            : base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, null)
        {
            this.InternalId = Guid.NewGuid();
        }

        public EntityHasViews(DslModeling::Store store, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
            : base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, propertyAssignments)
        {
            this.InternalId = Guid.NewGuid();
        }

        public EntityHasViews(DslModeling::Partition partition, params DslModeling::RoleAssignment[] roleAssignments)
            : base(partition, roleAssignments, null)
        {
            this.InternalId = Guid.NewGuid();
        }

        public EntityHasViews(DslModeling::Partition partition, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, roleAssignments, propertyAssignments)
        {
            this.InternalId = Guid.NewGuid();
        }
        #endregion

        /// <summary>
        /// Used when loading an relation since we cannot set the GUID so it is loaded from file (ModelToDisk) and the ID put here for later reference during the load
        /// </summary>
        public Guid InternalId { get; internal set; }

        public string PascalRoleName => StringHelper.FirstCharToUpper(this.RoleName);

        public override string ToString()
        {
            return this.DisplayName;
        }

        public string DisplayName
        {
            get
            {
                var retval = string.Empty;
                if (this.ParentEntity == null) return "(Unknown)";
                if (this.ChildView == null) return "(Unknown)";
                if (!this.FieldMapList().Any()) return "(Unknown)";

                retval = this.ParentEntity.Name + " -> " + this.ChildView.Name;

                if (!string.IsNullOrEmpty(this.RoleName))
                {
                    retval += " (Role: " + this.RoleName + ")";
                }

                var index = 0;
                var fields = this.FieldMapList().ToList();
                foreach (var cr in fields)
                {
                    retval += " [" + cr.GetSourceField(this).Name + ":" + cr.GetTargetField(this).Name + "]";
                    if (index < fields.Count - 1) retval += ",";
                    index++;
                }

                return retval;

            }
        }

        protected override void OnDeleting()
        {
            //Remove from relation mapped collections
            var count1 = this.ParentEntity.nHydrateModel.RelationFields.Remove(x => x.RelationID == this.Id);
            base.OnDeleting();
        }
    }

    partial class EntityHasViewsBase
    {
        protected EntityHasViewsBase(DslModeling::Partition partition, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, roleAssignments, propertyAssignments)
        {
        }

    }
}
