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
            var diskModel = new DiskModel();
            model.IsSaving = true;
            try
            {
                diskModel.ModelProperties.CompanyName = model.CompanyName;
                diskModel.ModelProperties.EmitSafetyScripts = model.EmitSafetyScripts;
                diskModel.ModelProperties.DefaultNamespace = model.DefaultNamespace;
                diskModel.ModelProperties.ProjectName = model.ProjectName;
                diskModel.ModelProperties.UseUTCTime = model.UseUTCTime;
                diskModel.ModelProperties.Version = model.Version;
                diskModel.ModelProperties.Id = model.Id.ToString();
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
                FileManagement.Save(rootFolder, modelName, diskModel);
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
        private static void SaveToDisk(DiskModel model, IEnumerable<Entity> list)
        {
            #region Save other parameter/field information

            foreach (var item in list)
            {
                var node = new nHydrate.ModelManagement.Entity.configuration();
                model.Entities.Add(node);
                node.allowcreateaudit = item.AllowCreateAudit.ToByte();
                node.allowmodifyaudit = item.AllowModifyAudit.ToByte();
                node.allowtimestamp = item.AllowTimestamp.ToByte();
                node.codefacade = item.CodeFacade;
                node.generatesdoublederived = item.GeneratesDoubleDerived.ToByte();
                node.id = item.Id.ToString();
                node.immutable = item.Immutable.ToByte();
                node.isassociative = item.IsAssociative.ToByte();
                node.isTenant = item.IsTenant.ToByte();
                node.name = item.Name;
                node.schema = item.Schema;
                node.summary = item.Summary;
                node.type = "entity";
                node.typedentity = item.TypedEntity.ToString();

                #region Fields
                    var nodeFields = new List<ModelManagement.Entity.configurationField>();
                    foreach (var field in item.Fields.OrderBy(x => x.Name))
                    {
                        var nodeField = new ModelManagement.Entity.configurationField();
                        nodeFields.Add(nodeField);
                        nodeField.codefacade = field.CodeFacade;
                        nodeField.dataformatstring = field.DataFormatString;
                        nodeField.datatype = field.DataType.ToString();
                        nodeField.@default = field.Default;
                        nodeField.defaultisfunc = field.DefaultIsFunc.ToByte();
                        nodeField.formula = field.Formula;
                        nodeField.id = field.Id.ToString();
                        nodeField.identity = field.Identity.ToString();
                        nodeField.Iscalculated = field.IsCalculated.ToByte();
                        nodeField.isindexed = field.IsIndexed.ToByte();
                        nodeField.isprimarykey = field.IsPrimaryKey.ToByte();
                        nodeField.isreadonly = field.IsReadOnly.ToByte();
                        nodeField.isunique = field.IsUnique.ToByte();
                        nodeField.length = field.Length;
                        nodeField.name = field.Name;
                        nodeField.nullable = field.Nullable.ToByte();
                        nodeField.obsolete = field.Obsolete.ToByte();
                        nodeField.scale = (byte)field.Scale;
                        nodeField.sortorder = (byte)field.SortOrder;
                        nodeField.summary = field.Summary;
                        node.fieldset = nodeFields.ToArray();
                    }
                    node.fieldset = nodeFields.ToArray();
                #endregion

                //Save other files
                SaveEntityIndexes(model, item, node);
                SaveRelations(model, item, node);
                SaveEntityStaticData(model, item, node);

            }
            #endregion
        }

        private static void SaveEntityIndexes(DiskModel model, Entity item, ModelManagement.Entity.configuration node)
        {
            var root = new ModelManagement.Index.configuration();
            model.Indexes.Add(root);
            root.id = item.Id.ToString();
            root.type = "entity.indexes";

            var nodeFields = new List<ModelManagement.Index.configurationIndex>();
            foreach (var index in item.Indexes)
            {
                var nodeField = new ModelManagement.Index.configurationIndex();
                nodeFields.Add(nodeField);
                nodeField.clustered = index.Clustered.ToByte();
                nodeField.id = index.Id.ToString();
                nodeField.importedname = index.ImportedName;
                nodeField.indextype = (byte)index.IndexType;
                nodeField.isunique = index.IsUnique.ToByte();
                nodeField.summary = index.Summary;

                var nodeIndexColumns = new List<ModelManagement.Index.configurationIndexColumn>();
                foreach (var indexColumn in index.IndexColumns)
                {
                    var nodeIndexColumn = new ModelManagement.Index.configurationIndexColumn();
                    nodeIndexColumns.Add(nodeIndexColumn);
                    nodeIndexColumn.ascending= indexColumn.Ascending.ToByte();
                    nodeIndexColumn.fieldid= indexColumn.FieldID.ToString();
                    nodeIndexColumn.id= indexColumn.Id.ToString();
                    nodeIndexColumn.sortorder= indexColumn.SortOrder;
                }
                nodeField.indexcolumnset = nodeIndexColumns.ToArray();
                nodeFields.Add(nodeField);
            }
            root.index = nodeFields.ToArray();

        }

        private static void SaveEntityStaticData(DiskModel model, Entity item, ModelManagement.Entity.configuration node)
        {
            if (item.StaticDatum.Count == 0)
                return;

            var nodeData = new nHydrate.ModelManagement.StaticData.configuration();
            nodeData.id = item.Id.ToString();
            nodeData.type = "entity.staticdata";

            var nodeDataItems = new List<ModelManagement.StaticData.configurationData>();
            foreach (var data in item.StaticDatum)
            {
                var nodeDataItem = new ModelManagement.StaticData.configurationData();
                nodeDataItems.Add(nodeDataItem);
                nodeDataItem.columnkey = data.ColumnKey.ToString();
                nodeDataItem.orderkey = data.OrderKey;
                nodeDataItem.value = data.Value;
            }
            nodeData.data = nodeDataItems.ToArray();
            model.StaticData.Add(nodeData);
        }

        private static void SaveRelations(DiskModel model, Entity item, ModelManagement.Entity.configuration node)
        {
            var nodeData = new nHydrate.ModelManagement.Relation.configuration();
            nodeData.id = item.Id.ToString();
            nodeData.type = "entity.relations";

            var nodeRelations = new List<ModelManagement.Relation.configurationRelation>();
            foreach (var relation in item.RelationshipList)
            {

                var nodeRelationItem = new ModelManagement.Relation.configurationRelation();
                nodeRelations.Add(nodeRelationItem);
                nodeRelationItem.childid = relation.ChildEntity.Id.ToString();
                nodeRelationItem.deleteaction = relation.DeleteAction.ToString();
                nodeRelationItem.id = relation.InternalId.ToString();
                nodeRelationItem.isenforced = relation.IsEnforced.ToByte();
                nodeRelationItem.rolename = relation.RoleName;
                nodeRelationItem.summary = relation.Summary;

                var nodeRelationFields = new List<ModelManagement.Relation.configurationRelationField>();
                foreach (var relationField in relation.FieldMapList())
                {
                    var nodeRelationField = new ModelManagement.Relation.configurationRelationField();
                    nodeRelationFields.Add(nodeRelationField);
                    nodeRelationField.id = relationField.Id.ToString();
                    nodeRelationField.sourcefieldid = relationField.SourceFieldId.ToString();
                    nodeRelationField.targetfieldid = relationField.TargetFieldId.ToString();
                }
                nodeRelationItem.relationfieldset = nodeRelationFields.ToArray();
            }
            nodeData.relation = nodeRelations.ToArray();
            model.Relations.Add(nodeData);
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
        private static void SaveToDisk(DiskModel model, IEnumerable<View> list)
        {
            #region Save other parameter/field information
            foreach (var item in list)
            {
                var node = new nHydrate.ModelManagement.View.configuration();
                model.Views.Add(node);
                node.codefacade = item.CodeFacade;
                node.generatesdoublederived = item.GeneratesDoubleDerived.ToByte();
                node.id = item.Id.ToString();
                node.name = item.Name;
                node.schema = item.Schema;
                node.summary = item.Summary;
                node.type = "view";
                #endregion

                var nodeFields = new List<ModelManagement.View.configurationField>();
                foreach (var field in item.Fields.OrderBy(x => x.Name))
                {
                    var nodeField = new ModelManagement.View.configurationField();
                    nodeFields.Add(nodeField);
                    nodeField.codefacade = field.CodeFacade;
                    nodeField.datatype = field.DataType.ToString();
                    nodeField.@default = field.Default;
                    nodeField.id = field.Id.ToString();
                    nodeField.isprimarykey = field.IsPrimaryKey.ToByte();
                    nodeField.length = field.Length;
                    nodeField.name = field.Name;
                    nodeField.nullable = field.Nullable.ToByte();
                    nodeField.scale = (byte)field.Scale;
                    nodeField.summary = field.Summary;
                }
            }
        }

        public static void LoadFromDisk(nHydrateModel model, string rootFolder, Microsoft.VisualStudio.Modeling.Store store, string modelName)
        {
            model.IsSaving = true;
            try
            {
                bool wasLoaded;
                var diskModel = FileManagement.Load(rootFolder, modelName, out wasLoaded);
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

        private static void LoadFromDisk(IEnumerable<Entity> list, nHydrateModel model, DiskModel diskModel)
        {
            #region Load other parameter/field information
            foreach (var entity in diskModel.Entities)
            {
                var itemID = new Guid(entity.id);
                var item = list.FirstOrDefault(x => x.Id == itemID);
                if (item == null)
                {
                    item = new Entity(model.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, itemID) });
                    model.Entities.Add(item);
                }

                System.Windows.Forms.Application.DoEvents();

                #region Properties

                item.Name = entity.name;
                item.AllowCreateAudit = entity.allowcreateaudit != 0;
                item.AllowModifyAudit = entity.allowmodifyaudit != 0;
                item.AllowTimestamp = entity.allowtimestamp != 0;
                item.CodeFacade = entity.codefacade;
                item.Immutable = entity.immutable != 0;
                item.IsAssociative = entity.isassociative != 0;
                item.GeneratesDoubleDerived = entity.generatesdoublederived != 0;
                item.IsTenant = entity.isTenant != 0;

                if (Enum.TryParse<TypedEntityConstants>(entity.typedentity, true, out var te))
                    item.TypedEntity = te;

                item.Schema = entity.schema;
                item.Summary = entity.summary;

                #endregion

                #region Fields

                var nameList = new List<string>();
                foreach (var fieldNode in entity.fieldset)
                {
                    var subItemID = new Guid(fieldNode.id);
                    var field = item.Fields.FirstOrDefault(x => x.Id == subItemID);
                    if (field == null)
                    {
                        field = new Field(item.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, subItemID) });
                        item.Fields.Add(field);
                    }
                    field.Name = fieldNode.name;
                    field.CodeFacade = fieldNode.codefacade;
                    nameList.Add(field.Name.ToLower());
                    field.Nullable = fieldNode.nullable != 0;

                    if (Enum.TryParse<DataTypeConstants>(fieldNode.datatype, true, out var dt))
                        field.DataType = dt;

                    if (Enum.TryParse<IdentityTypeConstants>(fieldNode.identity, true, out var it))
                        field.Identity = it;

                    field.DataFormatString = fieldNode.dataformatstring;
                    field.Default = fieldNode.@default;
                    field.DefaultIsFunc = fieldNode.defaultisfunc != 0;
                    field.Formula = fieldNode.formula;
                    field.IsIndexed = fieldNode.isindexed != 0;
                    field.IsPrimaryKey = fieldNode.isprimarykey != 0;
                    field.IsCalculated = fieldNode.Iscalculated != 0;
                    field.IsUnique = fieldNode.isunique != 0;
                    field.Length = fieldNode.length;
                    field.Scale = fieldNode.scale;
                    field.SortOrder = fieldNode.sortorder;
                    field.IsReadOnly = fieldNode.isreadonly != 0;
                    field.Obsolete = fieldNode.obsolete != 0;
                    field.Summary = fieldNode.summary;
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

        private static void LoadEntityIndexes(Entity entity, DiskModel diskModel)
        {
            foreach (var indexNode in diskModel.Indexes.Where(x => new Guid(x.id) == entity.Id).SelectMany(x => x.index))
            {
                var id = new Guid(indexNode.id);
                var newIndex = entity.Indexes.FirstOrDefault(x => x.Id == id);
                if (newIndex == null)
                {
                    newIndex = new Index(entity.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, id) });
                    entity.Indexes.Add(newIndex);
                }
                newIndex.Clustered = indexNode.clustered != 0;
                newIndex.ImportedName = indexNode.importedname;
                newIndex.IndexType = (IndexTypeConstants)indexNode.indextype;
                newIndex.IsUnique = indexNode.isunique != 0;

                foreach (var indexColumnNode in indexNode.indexcolumnset)
                {
                    id = new Guid(indexColumnNode.id);
                    var newIndexColumn = newIndex.IndexColumns.FirstOrDefault(x => x.Id == id);
                    if (newIndexColumn == null)
                    {
                        newIndexColumn = new IndexColumn(entity.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, id) });
                        newIndex.IndexColumns.Add(newIndexColumn);
                    }
                    newIndexColumn.Ascending = indexColumnNode.ascending != 0;
                    newIndexColumn.FieldID = new Guid(indexColumnNode.fieldid);
                    newIndexColumn.SortOrder = indexColumnNode.sortorder;
                    newIndexColumn.IsInternal = true;
                }
            }

        }

        private static void LoadEntityRelations(Entity entity, DiskModel diskModel)
        {
            foreach (var relationNode in diskModel.Relations.Where(x => new Guid(x.id) == entity.Id).SelectMany(x => x.relation))
            {
                var childid = new Guid(relationNode.childid);
                var child = entity.nHydrateModel.Entities.FirstOrDefault(x => x.Id == childid);
                if (child != null)
                {
                    entity.ChildEntities.Add(child);
                    var connection = entity.Store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.Last() as EntityHasEntities;
                    connection.InternalId = new Guid(relationNode.id);
                    connection.IsEnforced = relationNode.isenforced != 0;
                    connection.DeleteAction = (DeleteActionConstants)Enum.Parse(typeof(DeleteActionConstants), relationNode.deleteaction);
                    connection.RoleName = relationNode.rolename;

                    foreach (var relationFieldNode in relationNode.relationfieldset)
                    {
                        var sourceFieldID = new Guid(relationFieldNode.sourcefieldid);
                        var targetFieldID = new Guid(relationFieldNode.targetfieldid);
                        var sourceField = entity.Fields.FirstOrDefault(x => x.Id == sourceFieldID);
                        var targetField = entity.nHydrateModel.Entities.SelectMany(x => x.Fields).FirstOrDefault(x => x.Id == targetFieldID);
                        if ((sourceField != null) && (targetField != null))
                        {
                            var id = new Guid(relationFieldNode.id);
                            var newRelationField = new RelationField(entity.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, id) });
                            newRelationField.SourceFieldId = sourceFieldID;
                            newRelationField.TargetFieldId = targetFieldID;
                            newRelationField.RelationID = connection.Id;
                            entity.nHydrateModel.RelationFields.Add(newRelationField);
                        }
                    }
                }
            }

        }

        private static void LoadEntityStaticData(Entity entity, DiskModel diskModel)
        {
            foreach (var dataNode in diskModel.StaticData.Where(x => new Guid(x.id) == entity.Id).SelectMany(x => x.data))
            {
                var newData = new StaticData(entity.Partition);
                entity.StaticDatum.Add(newData);
                newData.OrderKey = dataNode.orderkey;
                newData.Value = dataNode.value;
                newData.ColumnKey = new Guid(dataNode.columnkey);
            }

        }

        private static void LoadFromDisk(IEnumerable<View> list, nHydrateModel model, DiskModel diskModel)
        {
            #region Load other parameter/field information

            foreach (var view in diskModel.Views)
            {
                var item = list.FirstOrDefault(x => x.Id == new Guid(view.id));
                if (item == null)
                {
                    item = new View(model.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, new Guid(view.id)) });
                    model.Views.Add(item);
                }

                System.Windows.Forms.Application.DoEvents();

                item.Name = view.name;
                item.CodeFacade = view.codefacade;
                item.Schema = view.schema;
                item.GeneratesDoubleDerived = view.generatesdoublederived != 0;
                item.Summary = view.summary;
                item.SQL = view.sql;

                var nameList = new List<string>();
                foreach (var fieldNode in view.fieldset)
                {
                    var subItemID = new Guid(fieldNode.id);
                    var field = item.Fields.FirstOrDefault(x => x.Id == subItemID);
                    if (field == null)
                    {
                        field = new ViewField(item.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, subItemID) });
                        item.Fields.Add(field);
                    }

                    field.Name = fieldNode.name;
                    field.CodeFacade = fieldNode.codefacade;
                    nameList.Add(field.Name.ToLower());
                    field.Nullable = fieldNode.nullable != 0;
                    if (Enum.TryParse<DataTypeConstants>(fieldNode.datatype, true, out var dt))
                        field.DataType = dt;
                    field.Default = fieldNode.@default;
                    field.Length = fieldNode.length;
                    field.Scale = fieldNode.scale;
                    field.Summary = fieldNode.summary;
                    field.IsPrimaryKey = fieldNode.isprimarykey != 0;
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
