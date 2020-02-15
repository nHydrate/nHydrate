using System;
using System.Collections.Generic;
using System.Linq;
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace nHydrate.Dsl
{
    partial class Index : nHydrate.Generator.Common.GeneratorFramework.IDirtyable
    {
        #region Constructors
        // Constructors were not generated for this relationship because it had HasCustomConstructor
        // set to true. Please provide the constructors below in a partial class.
        public Index(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
        }

        public Index(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
        }
        #endregion

        /// <summary>
        /// Calculate if this was meets the criteria of being created by an IsIndex Field
        /// </summary>
        [Browsable(false)]
        public bool IsIndexedType
        {
            get
            {
                //Must have 1 valid column and be ascending
                if (this.IndexColumns.Count != 1) return false;
                var field = this.IndexColumns[0].GetField();
                if (field == null) return false;
                if (!this.IndexColumns[0].Ascending) return false;
                return true;
            }
        }

        public override string Definition
        {
            get
            {
                var retval = string.Empty;
                foreach (var c in this.IndexColumns.OrderBy(x => x.SortOrder))
                {
                    var field = this.Entity.Fields.FirstOrDefault(x => x.Id == c.FieldID);
                    if (field != null)
                        retval += field.Name + ",";
                }
                retval = retval.TrimEnd(new char[] { ',' });
                if (string.IsNullOrEmpty(retval))
                    retval = "(Not Defined)";
                else if (this.IndexType == IndexTypeConstants.PrimaryKey)
                    retval = "(PK) " + retval;

                return retval;
            }
        }

        [Browsable(false)]
        public ReadOnlyCollection<Field> FieldList
        {
            get { return this.IndexColumns.Where(x => x != null).Select(x => x.Field).ToList().AsReadOnly(); }
        }

        public override bool IsUnique
        {
            get { return base.IsUnique || (this.IndexType == IndexTypeConstants.PrimaryKey); }
            set { base.IsUnique = value; }
        }

        protected override void OnDeleting()
        {
            if (this.Entity != null)
            {
                if (!this.Entity.nHydrateModel.IsLoading && !this.Entity.IsDeleting)
                {
                    //If this is the primary key then CANCEL
                    if (this.IndexType == IndexTypeConstants.PrimaryKey)
                        throw new Exception("This is a managed index for the primary key and cannot be removed.");

                    if (this.IndexColumns.Count == 1)
                    {
                        var column = this.IndexColumns.First();
                        if (column.Ascending)
                        {
                            var field = this.Entity.Fields.FirstOrDefault(x => x.Id == column.FieldID);

                            using (var transaction = this.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                            {
                                //This is an IsIndex mapped index so reset field property
                                if (field != null) field.IsIndexed = false;
                                transaction.Commit();
                            }
                        }
                    }
                }
            }

            base.OnDeleting();
        }

        public override string ToString()
        {
            var retval = string.Empty;
            foreach (var ic in this.IndexColumns.OrderBy(x => x.SortOrder).ThenBy(x => x.Field.Name))
            {
                var f = this.Entity.Fields.FirstOrDefault(x => x.Id == ic.FieldID);
                if (retval.Length > 0)
                    retval += ",";
                if (f == null)
                    retval += "(Unknown)";
                else
                    retval += f;
            }
            return retval;
        }

    }

    partial class IndexBase
    {
        partial class IsUniquePropertyHandler
        {
            protected override void OnValueChanged(IndexBase element, bool oldValue, bool newValue)
            {
                if (element.Entity != null)
                {
                    if (!element.Entity.nHydrateModel.IsLoading && !element.Store.InUndo)
                    {
                        if (element.IndexType == IndexTypeConstants.PrimaryKey)
                            throw new Exception("This is a managed index and cannot be modified.");
                    }
                }
                base.OnValueChanged(element, oldValue, newValue);
            }
        }
    }

}
