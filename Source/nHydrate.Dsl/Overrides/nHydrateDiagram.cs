#pragma warning disable 0168
using System;
using System.Linq;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Dsl
{
    partial class nHydrateDiagram
    {
        ///Determine if this diagram is loading from disk
        public bool IsLoading { get; set; } = false;

        #region Constructors

        //Constructors were not generated for this class because it had HasCustomConstructor
        //set to true. Please provide the constructors below in a partial class.
        public nHydrateDiagram(Microsoft.VisualStudio.Modeling.Store store, params Microsoft.VisualStudio.Modeling.PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
        }

        public nHydrateDiagram(Microsoft.VisualStudio.Modeling.Partition partition, params Microsoft.VisualStudio.Modeling.PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            this.Store.UndoManager.AddCanUndoRedoCallback(CanUndoRedoCallback);
            this.Store.TransactionManager.AddCanCommitCallback(CanCommitCallback);

            //Custom code so need to override the constructor
            this.ShowGrid = false;
            this.DisplayType = true;
            this.IsLoading = true;
            this.DiagramAdded += new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(nHydrateDiagram_DiagramAdded);
            TextManagerEvents.RegisterForTextManagerEvents();
        }

        private bool CanUndoRedoCallback(bool isUndo, Microsoft.VisualStudio.Modeling.TransactionItem transactionItem)
        {
            return true;
        }

        public Microsoft.VisualStudio.Modeling.CanCommitResult CanCommitCallback(Microsoft.VisualStudio.Modeling.Transaction transaction)
        {
            return Microsoft.VisualStudio.Modeling.CanCommitResult.Commit;
        }

        #endregion

        #region Events

        protected override void OnElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
        {
            var model = this.ModelElement as nHydrate.Dsl.nHydrateModel;
            if (!model.IsLoading)
            {
            }
            base.OnElementAdded(e);
        }

        public event EventHandler<ModelElementEventArgs> ShapeDoubleClick;

        protected void OnShapeDoubleClick(ModelElementEventArgs e)
        {
            if (this.ShapeDoubleClick != null)
                ShapeDoubleClick(this, e);
        }

        public event EventHandler<ModelElementEventArgs> ShapeConfiguring;

        protected void OnShapeConfiguring(ModelElementEventArgs e)
        {
            if (this.ShapeConfiguring != null)
                ShapeConfiguring(this, e);
        }

        #endregion

        #region Event Handlers

        private void FieldAdded(object sender, Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
        {
            var field = e.ModelElement as Field;
            if (field.Entity == null) return;
            if (field.Entity.nHydrateModel == null) return;

            if (!field.Entity.nHydrateModel.IsLoading && field.SortOrder == 0)
            {
                var maxSortOrder = 1;
                if (field.Entity.Fields.Count > 0)
                    maxSortOrder = field.Entity.Fields.Max(x => x.SortOrder);

                using (var transaction = this.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                {
                    field.SortOrder = ++maxSortOrder;
                    transaction.Commit();
                }
            }
        }

        private void IndexColumnAdded(object sender, Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
        {
            var ic = e.ModelElement as IndexColumn;
            if (ic.Index == null) return;
            if (ic.Index.Entity == null) return;
            if (ic.Index.Entity.nHydrateModel == null) return;

            if (!ic.IsInternal && !ic.Index.Entity.nHydrateModel.IsLoading && !this.Store.InUndo)
            {
                if (!ic.Index.Entity.nHydrateModel.IsLoading)
                {
                    if (ic.Index.IndexType != IndexTypeConstants.User)
                        throw new Exception("This is a managed index and cannot be modified.");
                }
            }

            if (!ic.Index.Entity.nHydrateModel.IsLoading)
            {
                if (ic.SortOrder == 0)
                {
                    using (var transaction = this.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                    {
                        var max = 0;
                        if (ic.Index.IndexColumns.Count > 0)
                            max = ic.Index.IndexColumns.Max(x => x.SortOrder);
                        ic.SortOrder = max + 1;
                        transaction.Commit();
                    }
                }
            }

        }

        private void FieldPropertyChanged(object sender, Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
        {
            var f = e.ModelElement as Field;
            f.CachedImage = null;
        }

        private void ViewFieldPropertyChanged(object sender, Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
        {
            var f = e.ModelElement as ViewField;
            f.CachedImage = null;
        }

        private bool _isLoaded = false;

        protected void nHydrateDiagram_DiagramAdded(object sender, Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
        {
            if (_isLoaded) return;
            _isLoaded = true;

            var model = this.Diagram.ModelElement as nHydrateModel;

            //Notify when field is changed so we can refresh icon
            this.Store.EventManagerDirectory.ElementPropertyChanged.Add(
                this.Store.DomainDataDirectory.FindDomainClass(typeof(Field)),
                new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(FieldPropertyChanged));

            this.Store.EventManagerDirectory.ElementPropertyChanged.Add(
                this.Store.DomainDataDirectory.FindDomainClass(typeof(ViewField)),
                new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ViewFieldPropertyChanged));

            //Notify when an index column is added
            this.Store.EventManagerDirectory.ElementAdded.Add(
                this.Store.DomainDataDirectory.FindDomainClass(typeof(IndexColumn)),
                new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(IndexColumnAdded));

            //Notify when a Field is added
            this.Store.EventManagerDirectory.ElementAdded.Add(
                this.Store.DomainDataDirectory.FindDomainClass(typeof(Field)),
                new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(FieldAdded));

            this.IsLoading = false;
            model.IsDirty = true;

        }

        #endregion

        protected override void OnChildConfiguring(Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement child, bool createdDuringViewFixup)
        {
            base.OnChildConfiguring(child, createdDuringViewFixup);

            try
            {
                if (!this.IsLoading)
                {
                    //Add a default field to entities
                    if (child.ModelElement is Entity)
                    {
                        var item = child.ModelElement as Entity;
                        var model = item.nHydrateModel;
                        if (item.Fields.Count == 0)
                        {
                            var field = new Field(item.Partition)
                                            {
                                                DataType = DataTypeConstants.Int,
                                                Identity = IdentityTypeConstants.Database,
                                                Name = "ID",
                                            };
                            item.Fields.Add(field); //Add then set PK so it will trigger index code
                            field.IsPrimaryKey = true;
                        }

                        #region Pasting
                        //If there are invalid indexes then try to remap them
                        foreach (var index in item.Indexes.Where(x => x.FieldList.Any(z => z == null)))
                        {
                            foreach (var c in index.IndexColumns.Where(x => x.Field == null && x.FieldID != Guid.Empty))
                            {
                                var f = model.Entities.SelectMany(x => x.Fields).FirstOrDefault(x => x.Id == c.FieldID);
                                if (f != null)
                                {
                                    var f2 = item.Fields.FirstOrDefault(x => x.Name == f.Name);
                                    if (f2 != null)
                                        c.FieldID = f2.Id;
                                }
                            }
                        }

                        //Add a PK index if not one
                        if (!item.Indexes.Any(x => x.IndexType == IndexTypeConstants.PrimaryKey) && item.PrimaryKeyFields.Count > 0)
                        {
                            var index = new Index(item.Partition) { IndexType = IndexTypeConstants.PrimaryKey };
                            item.Indexes.Add(index);
                            var loop = 0;
                            foreach (var field in item.PrimaryKeyFields)
                            {
                                var newIndexColumn = new IndexColumn(item.Partition);
                                index.IndexColumns.Add(newIndexColumn);
                                newIndexColumn.FieldID = field.Id;
                                newIndexColumn.SortOrder = loop;
                                loop++;
                            }
                        }
                        #endregion

                    }
                    else if (child.ModelElement is View)
                    {
                        var item = child.ModelElement as View;
                        if (item.Fields.Count == 0)
                        {
                            var field = new ViewField(item.Partition)
                                            {
                                                DataType = DataTypeConstants.Int,
                                                Name = "Field1",
                                            };
                            item.Fields.Add(field);
                        }
                    }

                    this.OnShapeConfiguring(new ModelElementEventArgs() { Shape = child });
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal void NotifyShapeDoubleClick(Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement shape)
        {
            this.OnShapeDoubleClick(new ModelElementEventArgs() { Shape = shape });
        }

        protected override void InitializeResources(Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet)
        {
            var dte = GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.SetDTE(dte);
        }

    }

}