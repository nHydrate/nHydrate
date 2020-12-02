#pragma warning disable 0168
using Microsoft.VisualStudio.Modeling;
using nHydrate.Generator.Common.Util;
using nHydrate.ModelManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;

namespace nHydrate.Dsl.Custom
{
    public static class SQLFileManagement
    {
        public static string GetModelFolder(string rootFolder, string modelName)
        {
            return Path.Combine(rootFolder, "_" + modelName);
        }

        public static void SaveToDisk(nHydrateModel model, string rootFolder, string modelName, nHydrateDiagram diagram)
        {
            var diskModel = new DiskModelYaml();
            model.IsSaving = true;
            try
            {
                diskModel.ModelProperties.CompanyName = model.CompanyName;
                diskModel.ModelProperties.EmitSafetyScripts = model.EmitSafetyScripts;
                diskModel.ModelProperties.DefaultNamespace = model.DefaultNamespace;
                diskModel.ModelProperties.ProjectName = model.ProjectName;
                diskModel.ModelProperties.UseUTCTime = model.UseUTCTime;
                diskModel.ModelProperties.Version = model.Version;
                diskModel.ModelProperties.Id = model.Id;
                diskModel.ModelProperties.TenantColumnName = model.TenantColumnName;
                diskModel.ModelProperties.CreatedByColumnName = model.CreatedByColumnName;
                diskModel.ModelProperties.CreatedDateColumnName = model.CreatedDateColumnName;
                diskModel.ModelProperties.ModifiedByColumnName = model.ModifiedByColumnName;
                diskModel.ModelProperties.ModifiedDateColumnName = model.ModifiedDateColumnName;
                diskModel.ModelProperties.ConcurrencyCheckColumnName = model.ConcurrencyCheckColumnName;
                diskModel.ModelProperties.GrantExecUser = model.GrantUser;

                var folderName = modelName.Replace(".nhydrate", ".model");
                var modelFolder = GetModelFolder(rootFolder, folderName);
                nHydrate.Dsl.Custom.SQLFileManagement.SaveToDisk(diskModel, model.Views);
                nHydrate.Dsl.Custom.SQLFileManagement.SaveToDisk(diskModel, model.Entities);
                FileManagement.Save2(rootFolder, modelName, diskModel);
                nHydrate.Dsl.Custom.SQLFileManagement.SaveDiagramFiles(modelFolder, diagram);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                model.IsSaving = false;
            }
        }

        /// <summary>
        /// Saves Stored Procedures to disk
        /// </summary>
        private static void SaveToDisk(DiskModelYaml model, IEnumerable<Entity> list)
        {
            #region Save other parameter/field information

            foreach (var item in list)
            {
                //var node = new nHydrate.ModelManagement.Entity.configuration();
                //model.Entities.Add(node);
                var node = model.Entities.AddNew();
                node.AllowCreateAudit = item.AllowCreateAudit;
                node.AllowModifyAudit = item.AllowModifyAudit;
                node.AllowTimestamp = item.AllowTimestamp;
                node.CodeFacade = item.CodeFacade;
                node.GeneratesDoubleDerived = item.GeneratesDoubleDerived;
                node.Id = item.Id;
                node.Immutable = item.Immutable;
                node.IsAssociative = item.IsAssociative;
                node.IsTenant = item.IsTenant;
                node.Name = item.Name;
                node.Schema = item.Schema;
                node.Summary = item.Summary;
                //node.type = "entity";
                node.TypedTable = item.TypedEntity.Convert<Generator.Common.TypedTableConstants>();

                #region Fields
                //var nodeFields = new List<ModelManagement.Entity.configurationField>();
                var loopIndex = 0;
                foreach (var field in item.Fields.OrderBy(x => x.SortOrder))
                {
                    //var nodeField = new ModelManagement.Entity.configurationField();
                    //nodeFields.Add(nodeField);
                    var nodeField = node.Fields.AddNew();
                    nodeField.CodeFacade = field.CodeFacade;
                    nodeField.DataFormatString = field.DataFormatString;
                    nodeField.Datatype = field.DataType.Convert<Generator.Common.DataTypeConstants>();
                    nodeField.Default = field.Default;
                    nodeField.DefaultIsFunc = field.DefaultIsFunc;
                    nodeField.Formula = field.Formula;
                    nodeField.Id = field.Id;
                    nodeField.Identity = field.Identity.Convert<Generator.Common.IdentityTypeConstants>();
                    nodeField.IsCalculated = field.IsCalculated;
                    nodeField.IsIndexed = field.IsIndexed;
                    nodeField.IsPrimaryKey = field.IsPrimaryKey;
                    nodeField.IsReadonly = field.IsReadOnly;
                    nodeField.IsUnique = field.IsUnique;
                    nodeField.Length = field.Length;
                    nodeField.Name = field.Name;
                    nodeField.Nullable = field.Nullable;
                    nodeField.Obsolete = field.Obsolete;
                    nodeField.Scale = (byte)field.Scale;
                    nodeField.Summary = field.Summary;
                    //node.fieldset = nodeFields.ToArray();
                }
                //node.fieldset = nodeFields.ToArray();
                #endregion

                //Save other files
                SaveEntityIndexes(model, item, node);
                SaveRelations(model, item, node);
                SaveEntityStaticData(model, item, node);

            }
            #endregion
        }

        private static void SaveEntityIndexes(DiskModelYaml model, Entity item, EntityYaml node)
        {
            //var root = new ModelManagement.Index.configuration();
            //model.Indexes.Add(root);
            //root.id = item.Id.ToString();
            //root.type = "entity.indexes";

            //var nodeFields = new List<ModelManagement.Index.configurationIndex>();
            foreach (var index in item.Indexes)
            {
                //var nodeField = new ModelManagement.Index.configurationIndex();
                //nodeFields.Add(nodeField);
                var nodeField = node.Indexes.AddNew();
                nodeField.Clustered = index.Clustered;
                nodeField.Id = index.Id;
                nodeField.ImportedName = index.ImportedName;
                nodeField.IndexType = index.IndexType.Convert<Generator.Common.IndexTypeConstants>();
                nodeField.IsUnique = index.IsUnique;
                nodeField.Summary = index.Summary;

                var nodeIndexColumns = new List<ModelManagement.Index.configurationIndexColumn>();
                foreach (var indexColumn in index.IndexColumns.OrderBy(x => x.SortOrder))
                {
                    //var nodeIndexColumn = new ModelManagement.Index.configurationIndexColumn();
                    //nodeIndexColumns.Add(nodeIndexColumn);
                    var nodeIndexColumn = nodeField.Fields.AddNew();
                    nodeIndexColumn.Ascending = indexColumn.Ascending;
                    nodeIndexColumn.FieldId = indexColumn.FieldID;
                    nodeIndexColumn.FieldName = item.Fields.FirstOrDefault(x => x.Id == indexColumn.FieldID)?.Name;
                    //nodeIndexColumn.Id = indexColumn.Id;
                }
                //nodeField.indexcolumnset = nodeIndexColumns.ToArray();
                //nodeFields.Add(nodeField);
            }
            //root.index = nodeFields.ToArray();

        }

        private static void SaveEntityStaticData(DiskModelYaml model, Entity item, EntityYaml node)
        {
            //if (item.StaticDatum.Count == 0)
            //    return;

            //var nodeData = new nHydrate.ModelManagement.StaticData.configuration();
            //nodeData.id = item.Id.ToString();
            //nodeData.type = "entity.staticdata";

            //var nodeDataItems = new List<ModelManagement.StaticData.configurationData>();
            foreach (var data in item.StaticDatum)
            {
                //var nodeDataItem = new ModelManagement.StaticData.configurationData();
                //nodeDataItems.Add(nodeDataItem);
                var nodeDataItem = node.StaticData.AddNew();
                nodeDataItem.ColumnId = data.ColumnKey;
                nodeDataItem.SortOrder = data.OrderKey;
                nodeDataItem.Value = data.Value;
            }
            //nodeData.data = nodeDataItems.ToArray();
            //model.StaticData.Add(nodeData);
        }

        private static void SaveRelations(DiskModelYaml model, Entity item, EntityYaml node)
        {
            //var nodeData = new nHydrate.ModelManagement.Relation.configuration();
            //nodeData.id = item.Id.ToString();
            //nodeData.type = "entity.relations";

            //var nodeRelations = new List<ModelManagement.Relation.configurationRelation>();
            foreach (var relation in item.RelationshipList)
            {

                //var nodeRelationItem = new ModelManagement.Relation.configurationRelation();
                //nodeRelations.Add(nodeRelationItem);
                var nodeRelationItem = node.Relations.AddNew();
                nodeRelationItem.ForeignEntityId = relation.ChildEntity.Id;
                nodeRelationItem.DeleteAction = relation.DeleteAction.Convert<Generator.Common.DeleteActionConstants>();
                nodeRelationItem.Id = relation.InternalId;
                nodeRelationItem.IsEnforced = relation.IsEnforced;
                nodeRelationItem.RoleName = relation.RoleName;
                nodeRelationItem.Summary = relation.Summary;

                var nodeRelationFields = new List<ModelManagement.Relation.configurationRelationField>();
                foreach (var relationField in relation.FieldMapList())
                {
                    //var nodeRelationField = new ModelManagement.Relation.configurationRelationField();
                    //nodeRelationFields.Add(nodeRelationField);
                    var nodeRelationField = nodeRelationItem.Fields.AddNew();
                    //nodeRelationField.Id = relationField.Id;
                    nodeRelationField.PrimaryFieldId = relationField.SourceFieldId;
                    nodeRelationField.ForeignFieldId = relationField.TargetFieldId;
                }
                //nodeRelationItem.relationfieldset = nodeRelationFields.ToArray();
            }
            //nodeData.relation = nodeRelations.ToArray();
            //model.Relations.Add(nodeData);
        }

        private static void SaveDiagramFiles(string rootFolder, nHydrateDiagram diagram)
        {
            var fileName = Path.Combine(rootFolder, "diagram.xml");
            var document = new XmlDocument();
            document.LoadXml(@"<configuration type=""diagram""></configuration>");
            foreach (var shape in diagram.NestedChildShapes)
            {
                if (shape is EntityShape)
                {
                    var item = ((shape as EntityShape).ModelElement as Entity);
                    var node = XmlHelper.AddElement(document.DocumentElement, "element");
                    node.AddAttribute("id", shape.ModelElement.Id);
                    node.AddAttribute("bounds", shape.AbsoluteBoundingBox.ToXmlValue());
                }
                else if (shape is ViewShape)
                {
                    var item = ((shape as ViewShape).ModelElement as View);
                    var node = XmlHelper.AddElement(document.DocumentElement, "element");
                    node.AddAttribute("id", shape.ModelElement.Id);
                    node.AddAttribute("bounds", shape.AbsoluteBoundingBox.ToXmlValue());
                }
            }
            document.Save(fileName);
        }

        /// <summary>
        /// Saves Views to disk
        /// </summary>
        private static void SaveToDisk(DiskModelYaml model, IEnumerable<View> list)
        {
            #region Save other parameter/field information
            foreach (var item in list)
            {
                //var node = new nHydrate.ModelManagement.View.configuration();
                //model.Views.Add(node);
                var node = model.Views.AddNew();
                node.CodeFacade = item.CodeFacade;
                node.GeneratesDoubleDerived = item.GeneratesDoubleDerived;
                node.Id = item.Id;
                node.Name = item.Name;
                node.Schema = item.Schema;
                node.Summary = item.Summary;
                //node.type = "view";
                #endregion

                //var nodeFields = new List<ModelManagement.View.configurationField>();
                foreach (var field in item.Fields.OrderBy(x => x.Name))
                {
                    //var nodeField = new ModelManagement.View.configurationField();
                    //nodeFields.Add(nodeField);
                    var nodeField = node.Fields.AddNew();
                    nodeField.CodeFacade = field.CodeFacade;
                    nodeField.Datatype = field.DataType.Convert<Generator.Common.DataTypeConstants>();
                    nodeField.Default = field.Default;
                    nodeField.Id = field.Id;
                    nodeField.IsPrimaryKey = field.IsPrimaryKey;
                    nodeField.Length = field.Length;
                    nodeField.Name = field.Name;
                    nodeField.Nullable = field.Nullable;
                    nodeField.Scale = (byte)field.Scale;
                    nodeField.Summary = field.Summary;
                }
            }
        }

        public static void LoadFromDisk(nHydrateModel model, string rootFolder, Microsoft.VisualStudio.Modeling.Store store, string modelName)
        {
            model.IsSaving = true;
            try
            {
                bool wasLoaded;
                var diskModel = FileManagement.Load2(rootFolder, modelName, out wasLoaded);
                if (wasLoaded)
                {
                    model.CompanyName = diskModel.ModelProperties.CompanyName;
                    model.EmitSafetyScripts = diskModel.ModelProperties.EmitSafetyScripts;
                    model.DefaultNamespace = diskModel.ModelProperties.DefaultNamespace;
                    model.ProjectName = diskModel.ModelProperties.ProjectName;
                    model.UseUTCTime = diskModel.ModelProperties.UseUTCTime;
                    model.Version = diskModel.ModelProperties.Version;
                    //model.Id = new Guid(diskModel.ModelProperties.Id);
                    model.TenantColumnName = diskModel.ModelProperties.TenantColumnName;
                    model.CreatedByColumnName = diskModel.ModelProperties.CreatedByColumnName;
                    model.CreatedDateColumnName = diskModel.ModelProperties.CreatedDateColumnName;
                    model.ModifiedByColumnName = diskModel.ModelProperties.ModifiedByColumnName;
                    model.ModifiedDateColumnName = diskModel.ModelProperties.ModifiedDateColumnName;
                    model.ConcurrencyCheckColumnName = diskModel.ModelProperties.ConcurrencyCheckColumnName;
                    model.GrantUser = diskModel.ModelProperties.GrantExecUser;

                    nHydrate.Dsl.Custom.SQLFileManagement.LoadFromDisk(model.Views, model, diskModel); //must come before entities
                    nHydrate.Dsl.Custom.SQLFileManagement.LoadFromDisk(model.Entities, model, diskModel);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                model.IsSaving = false;
            }
        }

        private static void LoadFromDisk(IEnumerable<Entity> list, nHydrateModel model, DiskModelYaml diskModel)
        {
            #region Load other parameter/field information
            foreach (var entity in diskModel.Entities)
            {
                var item = list.FirstOrDefault(x => x.Id == entity.Id);
                if (item == null)
                {
                    item = new Entity(model.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, entity.Id) });
                    model.Entities.Add(item);
                }

                System.Windows.Forms.Application.DoEvents();

                #region Properties

                item.Name = entity.Name;
                item.AllowCreateAudit = entity.AllowCreateAudit;
                item.AllowModifyAudit = entity.AllowModifyAudit;
                item.AllowTimestamp = entity.AllowTimestamp;
                item.CodeFacade = entity.CodeFacade;
                item.Immutable = entity.Immutable;
                item.IsAssociative = entity.IsAssociative;
                item.GeneratesDoubleDerived = entity.GeneratesDoubleDerived;
                item.IsTenant = entity.IsTenant;
                item.TypedEntity = entity.TypedTable.Convert<TypedEntityConstants>();
                item.Schema = entity.Schema;
                item.Summary = entity.Summary;

                #endregion

                #region Fields

                var nameList = new List<string>();
                var loopIndex = 0;
                foreach (var fieldNode in entity.Fields)
                {
                    var field = item.Fields.FirstOrDefault(x => x.Id == fieldNode.Id);
                    if (field == null)
                    {
                        field = new Field(item.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, fieldNode.Id) });
                        item.Fields.Add(field);
                    }
                    field.Name = fieldNode.Name;
                    field.CodeFacade = fieldNode.CodeFacade;
                    nameList.Add(field.Name.ToLower());
                    field.Nullable = fieldNode.Nullable;
                    field.DataType = fieldNode.Datatype.Convert<DataTypeConstants>();
                    field.Identity = fieldNode.Identity.Convert<IdentityTypeConstants>();
                    field.DataFormatString = fieldNode.DataFormatString;
                    field.Default = fieldNode.Default;
                    field.DefaultIsFunc = fieldNode.DefaultIsFunc;
                    field.Formula = fieldNode.Formula;
                    field.IsIndexed = fieldNode.IsIndexed;
                    field.IsPrimaryKey = fieldNode.IsPrimaryKey;
                    field.IsCalculated = fieldNode.IsCalculated;
                    field.IsUnique = fieldNode.IsUnique;
                    field.Length = fieldNode.Length;
                    field.Scale = fieldNode.Scale;
                    field.SortOrder = loopIndex++;
                    field.IsReadOnly = fieldNode.IsReadonly;
                    field.Obsolete = fieldNode.Obsolete;
                    field.Summary = fieldNode.Summary;
                }
                if (item.Fields.Remove(x => !nameList.Contains(x.Name.ToLower())) > 0)
                    item.nHydrateModel.IsDirty = true;

                #endregion

                LoadEntityIndexes(item, diskModel);

                //Order fields (skip for model that did not have sort order when saved)
                var fc = new FieldOrderComparer();
                if (item.Fields.Count(x => x.SortOrder > 0) > 0)
                    item.Fields.Sort(fc.Compare);
            }

            //Must load relations AFTER ALL entities are loaded
            foreach (var item in model.Entities)
            {
                LoadEntityRelations(item, diskModel);
                LoadEntityStaticData(item, diskModel);
            }

            #endregion

        }

        private static void LoadEntityIndexes(Entity entity, DiskModelYaml diskModel)
        {
            var entityYaml = diskModel.Entities.FirstOrDefault(x => x.Id == entity.Id);
            if (entityYaml == null) return;
            foreach (var indexNode in entityYaml.Indexes)
            {
                var newId = indexNode.Id; //Guid.NewGuid();
                var newIndex = entity.Indexes.FirstOrDefault(x => x.Id == newId);
                if (newIndex == null)
                {
                    newIndex = new Index(entity.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, newId) });
                    entity.Indexes.Add(newIndex);
                }
                newIndex.Clustered = indexNode.Clustered;
                newIndex.ImportedName = indexNode.ImportedName;
                newIndex.IndexType = (IndexTypeConstants)indexNode.IndexType;
                newIndex.IsUnique = indexNode.IsUnique;

                var loopIndex = 0;
                foreach (var indexColumnNode in indexNode.Fields)
                {
                    var newId2 = Guid.NewGuid(); //indexColumnNode.Id
                    var newIndexColumn = newIndex.IndexColumns.FirstOrDefault(x => x.Id == newId2);
                    if (newIndexColumn == null)
                    {
                        newIndexColumn = new IndexColumn(entity.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, newId2) });
                        newIndex.IndexColumns.Add(newIndexColumn);
                    }
                    newIndexColumn.Ascending = indexColumnNode.Ascending;
                    newIndexColumn.FieldID = indexColumnNode.FieldId;
                    newIndexColumn.SortOrder = loopIndex++;
                    newIndexColumn.IsInternal = true;
                }
            }

        }

        private static void LoadEntityRelations(Entity entity, DiskModelYaml diskModel)
        {
            var entityYaml = diskModel.Entities.FirstOrDefault(x => x.Id == entity.Id);
            if (entityYaml == null) return;
            foreach (var relationNode in entityYaml.Relations)
            {
                var child = entity.nHydrateModel.Entities.FirstOrDefault(x => x.Id == relationNode.ForeignEntityId);
                if (child != null)
                {
                    entity.ChildEntities.Add(child);
                    var connection = entity.Store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.Last() as EntityHasEntities;
                    connection.InternalId = relationNode.Id; //Guid.NewGuid();
                    connection.IsEnforced = relationNode.IsEnforced;
                    connection.DeleteAction = relationNode.DeleteAction.Convert<DeleteActionConstants>();
                    connection.RoleName = relationNode.RoleName;

                    foreach (var relationFieldNode in relationNode.Fields)
                    {
                        var sourceFieldID = relationFieldNode.PrimaryFieldId;
                        var targetFieldID = relationFieldNode.ForeignFieldId;
                        var sourceField = entity.Fields.FirstOrDefault(x => x.Id == sourceFieldID);
                        var targetField = entity.nHydrateModel.Entities.SelectMany(x => x.Fields).FirstOrDefault(x => x.Id == targetFieldID);
                        if ((sourceField != null) && (targetField != null))
                        {
                            var newRelationField = new RelationField(entity.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, Guid.NewGuid()) });
                            newRelationField.SourceFieldId = sourceFieldID;
                            newRelationField.TargetFieldId = targetFieldID;
                            newRelationField.RelationID = connection.Id;
                            entity.nHydrateModel.RelationFields.Add(newRelationField);
                        }
                    }
                }
            }

        }

        private static void LoadEntityStaticData(Entity entity, DiskModelYaml diskModel)
        {
            var entityYaml = diskModel.Entities.FirstOrDefault(x => x.Id == entity.Id);
            if (entityYaml == null) return;
            foreach (var dataNode in entityYaml.StaticData)
            {
                var newData = new StaticData(entity.Partition);
                entity.StaticDatum.Add(newData);
                newData.OrderKey = dataNode.SortOrder;
                newData.Value = dataNode.Value;
                newData.ColumnKey = dataNode.ColumnId;
            }

        }

        private static void LoadFromDisk(IEnumerable<View> list, nHydrateModel model, DiskModelYaml diskModel)
        {
            #region Load other parameter/field information

            foreach (var view in diskModel.Views)
            {
                var item = list.FirstOrDefault(x => x.Id == view.Id);
                if (item == null)
                {
                    item = new View(model.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, view.Id) });
                    model.Views.Add(item);
                }

                System.Windows.Forms.Application.DoEvents();

                item.Name = view.Name;
                item.CodeFacade = view.CodeFacade;
                item.Schema = view.Schema;
                item.GeneratesDoubleDerived = view.GeneratesDoubleDerived;
                item.Summary = view.Summary;
                item.SQL = view.Sql;

                var nameList = new List<string>();
                foreach (var fieldNode in view.Fields)
                {
                    var field = item.Fields.FirstOrDefault(x => x.Id == fieldNode.Id);
                    if (field == null)
                    {
                        field = new ViewField(item.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, fieldNode.Id) });
                        item.Fields.Add(field);
                    }

                    field.Name = fieldNode.Name;
                    field.CodeFacade = fieldNode.CodeFacade;
                    nameList.Add(field.Name.ToLower());
                    field.Nullable = fieldNode.Nullable;
                    field.DataType = fieldNode.Datatype.Convert<DataTypeConstants>();
                    field.Default = fieldNode.Default;
                    field.Length = fieldNode.Length;
                    field.Scale = fieldNode.Scale;
                    field.Summary = fieldNode.Summary;
                    field.IsPrimaryKey = fieldNode.IsPrimaryKey;
                }
                if (item.Fields.Remove(x => !nameList.Contains(x.Name.ToLower())) > 0)
                    item.nHydrateModel.IsDirty = true;
            }

            #endregion

        }

        public static void LoadDiagramFiles(nHydrateModel model, string rootFolder, string modelName, nHydrateDiagram diagram)
        {
            if (!model.ModelToDisk)
                return;

            var fileName = Path.Combine(GetModelFolder(rootFolder, modelName), "diagram.xml");
            if (!File.Exists(fileName)) return;
            using (var transaction = model.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                var document = new XmlDocument();
                var id = Guid.Empty;
                try
                {
                    document.Load(fileName);
                    if (document.DocumentElement == null) throw new Exception("No Root"); //this is thrown away

                    foreach (XmlNode node in document.DocumentElement.ChildNodes)
                    {
                        id = XmlHelper.GetAttributeValue(node, "id", Guid.Empty);
                        var shape = diagram.NestedChildShapes.FirstOrDefault(x => x.ModelElement.Id == id) as Microsoft.VisualStudio.Modeling.Diagrams.NodeShape;
                        if (shape != null)
                            shape.Bounds = Extensions.ConvertRectangleDFromXmlValue(XmlHelper.GetAttributeValue(node, "bounds", string.Empty));
                    }
                }
                catch (Exception ex) { return; }
                transaction.Commit();
            }

        }

        private static byte ToByte(this bool v)
        {
            return (byte)(v ? 1 : 0);
        }

    }
}
