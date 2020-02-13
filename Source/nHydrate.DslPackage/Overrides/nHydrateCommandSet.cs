#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using nHydrate.Dsl;

namespace nHydrate.DslPackage
{
    partial class nHydrateClipboardCommandSet
    {
        protected override void OnMenuCopy(object sender, EventArgs args)
        {
            base.OnMenuCopy(sender, args);
        }

        protected override void OnStatusPaste(object sender, EventArgs args)
        {
            base.OnStatusPaste(sender, args);
        }

        protected override void OnMenuPaste(object sender, global::System.EventArgs args)
        {
            nHydrateModel model = null;
            try
            {
                nHydrateDiagram diagram = null;
                nHydrate.Dsl.Entity selectedEntity = null;
                foreach (var item in this.CurrentSelection)
                {
                    if (diagram == null && item is nHydrateDiagram)
                        diagram = item as nHydrateDiagram;
                    if (diagram == null && item is Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement)
                    {
                        diagram = (item as Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement).Diagram as nHydrateDiagram;
                        if (item is EntityShape)
                            selectedEntity = (item as EntityShape).ModelElement as nHydrate.Dsl.Entity;
                    }
                }

                if (diagram != null)
                {
                    model = diagram.ModelElement as nHydrateModel;
                    model.IsLoading = true;
                }

                var beforeList = model.Entities.ToList();
                base.OnMenuPaste(sender, args);
                var afterList = model.Entities.ToList().Except(beforeList).ToList();

                #region Check indexes after Entity paste to make sure they are setup
                foreach (var item in afterList)
                {
                    try
                    {
                        var settings = Extensions.FromXml<CopyStateSettings>(item.CopyStateInfo);
                        using (var transaction = item.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                        {
                            //Now reset all indexes
                            item.Indexes.Clear();

                            foreach (var indexDef in settings.Indexes)
                            {
                                var newIndex = item.Indexes.AddNew() as nHydrate.Dsl.Index;
                                newIndex.Clustered = indexDef.Clustered;
                                newIndex.IsUnique = indexDef.IsUnique;
                                newIndex.Summary = indexDef.Summary;
                                newIndex.IndexType = indexDef.IndexType;

                                foreach (var columnDef in indexDef.Columns)
                                {
                                    var newColumn = newIndex.IndexColumns.AddNew() as IndexColumn;
                                    newColumn.Ascending = columnDef.Acending;
                                    var fieldRef = item.FieldList.FirstOrDefault(x => x.Name == columnDef.Name);
                                    if (fieldRef != null)
                                        newColumn.FieldID = (fieldRef as Microsoft.VisualStudio.Modeling.ModelElement).Id;
                                }
                            }
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                #endregion

                #region We have pasted some fields so verify indexes
                //THIS DOES NOT WORK. NEED TO SAVE FIELDS BEFORE/AFTER AND COMPARE
                //if (afterList.Count == 0 && this.CurrentSelection.Count == 1 && selectedEntity != null)
                //{
                //    var item = selectedEntity;
                //    using (var transaction = item.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                //    {
                //        foreach (Field field in item.FieldList)
                //        {
                //            if (field.IsIndexed)
                //            {
                //                if (!item.Indexes.Any(x => x.FieldList.Any(z => z.Id == field.Id) && x.IndexType == IndexTypeConstants.IsIndexed))
                //                {
                //                    var newIndex = item.Indexes.AddNew() as nHydrate.Dsl.Index;
                //                    newIndex.Clustered = false;
                //                    newIndex.IsUnique = false;
                //                    newIndex.IndexType = IndexTypeConstants.IsIndexed;

                //                    var newColumn = newIndex.IndexColumns.AddNew() as IndexColumn;
                //                    newColumn.Ascending = true;
                //                    newColumn.FieldID = field.Id;
                //                }
                //            }
                //        }

                //        transaction.Commit();
                //    }

                //}
                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (model != null)
                    model.IsLoading = false;
            }

        }

        protected override void ProcessOnMenuPasteCommand()
        {
            base.ProcessOnMenuPasteCommand();
        }

        protected override void ProcessOnMenuCopyCommand()
        {
            try
            {
                var diagram = (this.CurrentModelingDocView as Microsoft.VisualStudio.Modeling.Shell.SingleDiagramDocView).Diagram;
                foreach (EntityShape shape in this.CurrentDocumentSelection.ToList<object>().Where(x => x is EntityShape).ToList())
                {
                    using (var transaction = shape.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                    {
                        var entity = shape.ModelElement as Entity;
                        var cache = new CopyStateSettings();

                        foreach (var item in entity.Indexes)
                        {
                            var indexCache = new CopyStateSettings.CopyStateIndex
                            {
                                Clustered = item.Clustered,
                                IsUnique = item.IsUnique,
                                Summary = item.Summary,
                                IndexType = item.IndexType,
                            };
                            cache.Indexes.Add(indexCache);

                            foreach (var col in item.IndexColumns)
                            {
                                indexCache.Columns.Add(new CopyStateSettings.CopyStateIndexColumn {
                                    Acending = col.Ascending,
                                    Name = col.Field.Name,
                                });
                            }
                        }

                        var xml = Extensions.ToXml(cache);
                        entity.CopyStateInfo = xml;
                        transaction.Commit();
                    }
                }

                base.ProcessOnMenuCopyCommand();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    [Serializable]
    public class CopyStateSettings
    {
        public CopyStateSettings()
        {
            this.Indexes = new List<CopyStateIndex>();
        }

        public List<CopyStateIndex> Indexes { get; set; }

        [Serializable]
        public class CopyStateIndex
        {
            public CopyStateIndex()
            {
                this.Columns = new List<CopyStateIndexColumn>();
            }

            public bool Clustered { get; set; }
            public bool IsUnique { get; set; }
            public string Summary { get; set; }
            public IndexTypeConstants IndexType { get; set; }

            public List<CopyStateIndexColumn> Columns { get; set; }
        }

        [Serializable]
        public class CopyStateIndexColumn
        {
            public bool Acending { get; set; }
            public string Name { get; set; }
        }

    }

}