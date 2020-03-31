#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using nHydrate.Generator.Common.Util;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using System.IO.Compression;

namespace nHydrate.Dsl.Custom
{
    public static class SQLFileManagement
    {
        private const string FOLDER_ET = "_Entities";
        private const string FOLDER_VW = "_Views";
        
        public static string GetModelFolder(string rootFolder, string modelName)
        {
            return Path.Combine(rootFolder, "_" + modelName);
        }

        public static void SaveToDisk(nHydrateModel modelRoot, string rootFolder, string modelName, nHydrateDiagram diagram)
        {
            modelRoot.IsSaving = true;
            try
            {
                var folderName = modelName.Replace(".nhydrate", ".model");
                var modelFolder = GetModelFolder(rootFolder, folderName);
                var generatedFileList = new List<string>();
                nHydrate.Dsl.Custom.SQLFileManagement.SaveToDisk(modelRoot, modelRoot.Views, modelFolder, diagram, generatedFileList); //must come before entities
                nHydrate.Dsl.Custom.SQLFileManagement.SaveToDisk(modelRoot, modelRoot.Entities, modelFolder, diagram, generatedFileList);
                nHydrate.Dsl.Custom.SQLFileManagement.SaveDiagramFiles(modelFolder, diagram, generatedFileList);
                RemoveOrphans(modelFolder, generatedFileList);

                try
                {
                    var compressedFile = Path.Combine(rootFolder, modelName + ".zip");
                    if (File.Exists(compressedFile))
                    {
                        File.Delete(compressedFile);
                        System.Threading.Thread.Sleep(300);
                    }

                    //Create ZIP file with entire model folder
                    System.IO.Compression.ZipFile.CreateFromDirectory(modelFolder, compressedFile, System.IO.Compression.CompressionLevel.Fastest, true);

                    //Now add the top level model artifacts
                    var artifacts = Directory.GetFiles(rootFolder, $"{modelName}.*").ToList();
                    artifacts.RemoveAll(x => x == compressedFile);
                    using (var zipToOpen = System.IO.Compression.ZipFile.Open(compressedFile, System.IO.Compression.ZipArchiveMode.Update))
                    {
                        foreach (var ff in artifacts)
                        {
                            var fi = new FileInfo(ff);
                            zipToOpen.CreateEntryFromFile(ff, fi.Name);
                        }
                    }

                }
                catch (Exception ex)
                {
                    //Do Nothing
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                modelRoot.IsSaving = false;
            }
        }

        /// <summary>
        /// Saves Stored Procedures to disk
        /// </summary>
        private static void SaveToDisk(nHydrateModel modelRoot, IEnumerable<Entity> list, string rootFolder, nHydrateDiagram diagram, List<string> generatedFileList)
        {
            var folder = Path.Combine(rootFolder, FOLDER_ET);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            #region Save other parameter/field information
            foreach (var item in list)
            {
                var document = new XmlDocument();
                document.LoadXml(@"<configuration type=""entity"" name=""" + item.Name + @"""></configuration>");

                XmlHelper.AddLineBreak(document.DocumentElement);
                XmlHelper.AddCData(document.DocumentElement, "summary", item.Summary);
                XmlHelper.AddLineBreak(document.DocumentElement);

                XmlHelper.AddAttribute(document.DocumentElement, "id", item.Id);
                XmlHelper.AddAttribute(document.DocumentElement, "allowcreateaudit", item.AllowCreateAudit);
                XmlHelper.AddAttribute(document.DocumentElement, "allowmodifyaudit", item.AllowModifyAudit);
                XmlHelper.AddAttribute(document.DocumentElement, "allowtimestamp", item.AllowTimestamp);
                XmlHelper.AddAttribute(document.DocumentElement, "codefacade", item.CodeFacade);
                XmlHelper.AddAttribute(document.DocumentElement, "immutable", item.Immutable);
                XmlHelper.AddAttribute(document.DocumentElement, "isassociative", item.IsAssociative);
                XmlHelper.AddAttribute(document.DocumentElement, "typedentity", item.TypedEntity.ToString());
                XmlHelper.AddAttribute(document.DocumentElement, "schema", item.Schema);
                XmlHelper.AddAttribute(document.DocumentElement, "generatesdoublederived", item.GeneratesDoubleDerived);
                XmlHelper.AddAttribute(document.DocumentElement, "isTenant", item.IsTenant);

                #region Fields
                {
                    var fieldsNodes = XmlHelper.AddElement(document.DocumentElement, "fieldset") as XmlElement;
                    XmlHelper.AddLineBreak((XmlElement)fieldsNodes);
                    foreach (var field in item.Fields.OrderBy(x => x.Name))
                    {
                        var fieldNode = XmlHelper.AddElement(fieldsNodes, "field");

                        XmlHelper.AddLineBreak((XmlElement)fieldNode);
                        XmlHelper.AddCData((XmlElement)fieldNode, "summary", field.Summary);
                        XmlHelper.AddLineBreak((XmlElement)fieldNode);

                        XmlHelper.AddAttribute(fieldNode, "id", field.Id);
                        XmlHelper.AddAttribute(fieldNode, "name", field.Name);
                        XmlHelper.AddAttribute(fieldNode, "nullable", field.Nullable);
                        XmlHelper.AddAttribute(fieldNode, "datatype", field.DataType.ToString());
                        XmlHelper.AddAttribute(fieldNode, "identity", field.Identity.ToString());
                        XmlHelper.AddAttribute(fieldNode, "codefacade", field.CodeFacade);
                        XmlHelper.AddAttribute(fieldNode, "dataformatstring", field.DataFormatString);
                        XmlHelper.AddAttribute(fieldNode, "default", field.Default);
                        XmlHelper.AddAttribute(fieldNode, "defaultisfunc", field.DefaultIsFunc);
                        XmlHelper.AddAttribute(fieldNode, "formula", field.Formula);
                        XmlHelper.AddAttribute(fieldNode, "isindexed", field.IsIndexed);
                        XmlHelper.AddAttribute(fieldNode, "isprimarykey", field.IsPrimaryKey);
                        XmlHelper.AddAttribute(fieldNode, "Iscalculated", field.IsCalculated);
                        XmlHelper.AddAttribute(fieldNode, "isunique", field.IsUnique);
                        XmlHelper.AddAttribute(fieldNode, "length", field.Length);
                        XmlHelper.AddAttribute(fieldNode, "scale", field.Scale);
                        XmlHelper.AddAttribute(fieldNode, "sortorder", field.SortOrder);
                        XmlHelper.AddAttribute(fieldNode, "isreadonly", field.IsReadOnly);
                        XmlHelper.AddAttribute(fieldNode, "obsolete", field.Obsolete);

                        XmlHelper.AddLineBreak((XmlElement)fieldsNodes);
                    }
                    XmlHelper.AddLineBreak((XmlElement)document.DocumentElement);
                }
                #endregion

                XmlHelper.AddLineBreak(document.DocumentElement);
                var f = Path.Combine(folder, item.Name + ".configuration.xml");
                WriteFileIfNeedBe(f, document.ToIndentedString(), generatedFileList);

                //Save other files
                SaveEntityIndexes(folder, item, generatedFileList);
                SaveRelations(folder, item, generatedFileList);
                SaveEntityStaticData(folder, item, generatedFileList);

            }
            #endregion

            WriteReadMeFile(folder, generatedFileList);
        }

        private static void SaveEntityIndexes(string folder, Entity item, List<string> generatedFileList)
        {
            var document = new XmlDocument();
            document.LoadXml(@"<configuration type=""entity.indexes"" id=""" + item.Id + @"""></configuration>");

            XmlHelper.AddLineBreak((XmlElement)document.DocumentElement);
            foreach (var index in item.Indexes)
            {
                var indexNode = XmlHelper.AddElement(document.DocumentElement, "index");

                XmlHelper.AddLineBreak((XmlElement)indexNode);
                XmlHelper.AddCData((XmlElement)indexNode, "summary", index.Summary);
                XmlHelper.AddLineBreak((XmlElement)indexNode);

                XmlHelper.AddAttribute(indexNode, "id", index.Id);
                XmlHelper.AddAttribute(indexNode, "clustered", index.Clustered);
                XmlHelper.AddAttribute(indexNode, "importedname", index.ImportedName);
                XmlHelper.AddAttribute(indexNode, "indextype", index.IndexType.ToString("d"));
                XmlHelper.AddAttribute(indexNode, "isunique", index.IsUnique);

                XmlHelper.AddLineBreak((XmlElement)document.DocumentElement);

                //Process the columns
                var indexColumnsNodes = XmlHelper.AddElement((XmlElement)indexNode, "indexcolumnset") as XmlElement;
                XmlHelper.AddLineBreak((XmlElement)indexColumnsNodes);
                foreach (var indexColumn in index.IndexColumns)
                {
                    var indexColumnNode = XmlHelper.AddElement(indexColumnsNodes, "column");
                    XmlHelper.AddAttribute(indexColumnNode, "id", indexColumn.Id);
                    XmlHelper.AddAttribute(indexColumnNode, "ascending", indexColumn.Ascending);
                    XmlHelper.AddAttribute(indexColumnNode, "fieldid", indexColumn.FieldID);
                    XmlHelper.AddAttribute(indexColumnNode, "sortorder", indexColumn.SortOrder);

                    XmlHelper.AddLineBreak((XmlElement)indexColumnsNodes);
                }
            }

            var f = Path.Combine(folder, item.Name + ".indexes.xml");
            WriteFileIfNeedBe(f, document.ToIndentedString(), generatedFileList);

        }

        private static void SaveEntityStaticData(string folder, Entity item, List<string> generatedFileList)
        {
            var f = Path.Combine(folder, item.Name + ".staticdata.xml");
            if (item.StaticDatum.Count == 0)
                return;

            var document = new XmlDocument();
            document.LoadXml(@"<configuration type=""entity.staticdata"" id=""" + item.Id + @"""></configuration>");

            XmlHelper.AddLineBreak((XmlElement)document.DocumentElement);
            foreach (var data in item.StaticDatum)
            {
                var dataNode = XmlHelper.AddElement(document.DocumentElement, "data");

                XmlHelper.AddAttribute(dataNode, "columnkey", data.ColumnKey);
                XmlHelper.AddAttribute(dataNode, "value", data.Value);
                XmlHelper.AddAttribute(dataNode, "orderkey", data.OrderKey);
                XmlHelper.AddLineBreak((XmlElement)document.DocumentElement);
            }

            WriteFileIfNeedBe(f, document.ToIndentedString(), generatedFileList);
        }

        private static void SaveRelations(string folder, Entity item, List<string> generatedFileList)
        {
            var document = new XmlDocument();
            document.LoadXml(@"<configuration type=""entity.relations"" id=""" + item.Id + @"""></configuration>");

            XmlHelper.AddLineBreak((XmlElement)document.DocumentElement);
            foreach (var relation in item.RelationshipList)
            {
                var relationNode = XmlHelper.AddElement(document.DocumentElement, "relation");

                XmlHelper.AddLineBreak((XmlElement)relationNode);
                XmlHelper.AddCData((XmlElement)relationNode, "summary", relation.Summary);
                XmlHelper.AddLineBreak((XmlElement)relationNode);

                XmlHelper.AddAttribute(relationNode, "id", relation.InternalId);
                XmlHelper.AddAttribute(relationNode, "childid", relation.ChildEntity.Id);
                XmlHelper.AddAttribute(relationNode, "isenforced", relation.IsEnforced);
                XmlHelper.AddAttribute(relationNode, "deleteaction", relation.DeleteAction.ToString());
                XmlHelper.AddAttribute(relationNode, "rolename", relation.RoleName);

                XmlHelper.AddLineBreak((XmlElement)document.DocumentElement);

                //Process the columns
                var relationColumnsNodes = XmlHelper.AddElement((XmlElement)relationNode, "relationfieldset") as XmlElement;
                XmlHelper.AddLineBreak((XmlElement)relationColumnsNodes);
                foreach (var relationField in relation.FieldMapList())
                {
                    var realtionFieldNode = XmlHelper.AddElement(relationColumnsNodes, "field");
                    XmlHelper.AddAttribute(realtionFieldNode, "id", relationField.Id);
                    XmlHelper.AddAttribute(realtionFieldNode, "sourcefieldid", relationField.SourceFieldId);
                    XmlHelper.AddAttribute(realtionFieldNode, "targetfieldid", relationField.TargetFieldId);

                    XmlHelper.AddLineBreak((XmlElement)relationColumnsNodes);
                }
            }

            var f = Path.Combine(folder, item.Name + ".relations.xml");
            WriteFileIfNeedBe(f, document.ToIndentedString(), generatedFileList);

        }

        private static void SaveDiagramFiles(string rootFolder, nHydrateDiagram diagram, List<string> generatedFileList)
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
            WriteFileIfNeedBe(fileName, document.ToIndentedString(), generatedFileList);
        }

        /// <summary>
        /// Saves Views to disk
        /// </summary>
        private static void SaveToDisk(nHydrateModel modelRoot, IEnumerable<View> list, string rootFolder, nHydrateDiagram diagram, List<string> generatedFileList)
        {
            var folder = Path.Combine(rootFolder, FOLDER_VW);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            foreach (var item in list)
            {
                var f = Path.Combine(folder, item.Name + ".sql");
                WriteFileIfNeedBe(f, item.SQL, generatedFileList);
            }

            #region Save other parameter/field information
            foreach (var item in list)
            {
                var document = new XmlDocument();
                document.LoadXml(@"<configuration type=""view"" name=""" + item.Name + @"""></configuration>");

                XmlHelper.AddLineBreak(document.DocumentElement);
                XmlHelper.AddCData(document.DocumentElement, "summary", item.Summary);
                XmlHelper.AddLineBreak(document.DocumentElement);

                XmlHelper.AddAttribute(document.DocumentElement, "id", item.Id);
                XmlHelper.AddAttribute(document.DocumentElement, "codefacade", item.CodeFacade);
                XmlHelper.AddAttribute(document.DocumentElement, "schema", item.Schema);
                XmlHelper.AddAttribute(document.DocumentElement, "generatesdoublederived", item.GeneratesDoubleDerived);

                var fieldsNodes = XmlHelper.AddElement(document.DocumentElement, "fieldset") as XmlElement;
                XmlHelper.AddLineBreak((XmlElement)fieldsNodes);

                foreach (var field in item.Fields.OrderBy(x => x.Name))
                {
                    var fieldNode = XmlHelper.AddElement(fieldsNodes, "field");

                    XmlHelper.AddLineBreak((XmlElement)fieldNode);
                    XmlHelper.AddCData((XmlElement)fieldNode, "summary", field.Summary);
                    XmlHelper.AddLineBreak((XmlElement)fieldNode);

                    XmlHelper.AddAttribute(fieldNode, "id", field.Id);
                    XmlHelper.AddAttribute(fieldNode, "name", field.Name);
                    XmlHelper.AddAttribute(fieldNode, "nullable", field.Nullable);
                    XmlHelper.AddAttribute(fieldNode, "datatype", field.DataType.ToString());
                    XmlHelper.AddAttribute(fieldNode, "codefacade", field.CodeFacade);
                    XmlHelper.AddAttribute(fieldNode, "default", field.Default);
                    XmlHelper.AddAttribute(fieldNode, "length", field.Length);
                    XmlHelper.AddAttribute(fieldNode, "scale", field.Scale);
                    XmlHelper.AddAttribute(fieldNode, "isprimarykey", field.IsPrimaryKey);

                    XmlHelper.AddLineBreak((XmlElement)fieldsNodes);
                }

                XmlHelper.AddLineBreak(document.DocumentElement);
                var f = Path.Combine(folder, item.Name + ".configuration.xml");
                WriteFileIfNeedBe(f, document.ToIndentedString(), generatedFileList);
            }
            #endregion

            WriteReadMeFile(folder, generatedFileList);
        }

        public static void LoadFromDisk(nHydrateModel model, string rootFolder, Microsoft.VisualStudio.Modeling.Store store, string modelName)
        {
            model.IsSaving = true;
            try
            {
                var modelFile = Path.Combine(rootFolder, modelName);
                var fi = new FileInfo(modelFile);
                var showError = (fi.Length > 10); //New file is small so show no error if creating new

                var folderName = modelName.Replace(".nhydrate", ".model");
                var modelFolder = GetModelFolder(rootFolder, folderName);

                //If the model folder does NOT exist
                if (showError && !Directory.Exists(modelFolder))
                {
                    //Try to use the ZIP file
                    var compressedFile = Path.Combine(rootFolder, modelName + ".zip");
                    if (!File.Exists(compressedFile))
                    {
                        MessageBox.Show("The model folder was not found and the ZIP file is missing. One of these must exist to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //Unzip the whole file
                    //System.IO.Compression.ZipFile.ExtractToDirectory(compressedFile, rootFolder);
                    ExtractToDirectory(compressedFile, rootFolder, false);
                }

                nHydrate.Dsl.Custom.SQLFileManagement.LoadFromDisk(model.Views, model, modelFolder, store); //must come before entities
                nHydrate.Dsl.Custom.SQLFileManagement.LoadFromDisk(model.Entities, model, modelFolder, store);
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

        private static void LoadFromDisk(IEnumerable<Entity> list, nHydrateModel model, string rootFolder, Microsoft.VisualStudio.Modeling.Store store)
        {
            var folder = Path.Combine(rootFolder, FOLDER_ET);
            if (!Directory.Exists(folder)) return;

            #region Load other parameter/field information
            var fList = Directory.GetFiles(folder, "*.configuration.xml");
            foreach (var f in fList)
            {
                var document = new XmlDocument();
                try
                {
                    document.Load(f);
                }
                catch (Exception ex)
                {
                    //Do Nothing
                    MessageBox.Show("The file '" + f + "' is not valid and could not be loaded!", "Load Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var fi = new FileInfo(f);
                var name = fi.Name.Substring(0, fi.Name.Length - ".configuration.xml".Length).ToLower();
                var itemID = XmlHelper.GetAttributeValue(document.DocumentElement, "id", Guid.Empty);
                var item = list.FirstOrDefault(x => x.Id == itemID);
                if (item == null)
                {
                    item = new Entity(model.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, XmlHelper.GetAttributeValue(document.DocumentElement, "id", Guid.NewGuid())) });
                    model.Entities.Add(item);
                }

                System.Windows.Forms.Application.DoEvents();

                #region Properties

                item.Name = XmlHelper.GetAttributeValue(document.DocumentElement, "name", item.Name);
                item.AllowCreateAudit = XmlHelper.GetAttributeValue(document.DocumentElement, "allowcreateaudit", item.AllowCreateAudit);
                item.AllowModifyAudit = XmlHelper.GetAttributeValue(document.DocumentElement, "allowmodifyaudit", item.AllowModifyAudit);
                item.AllowTimestamp = XmlHelper.GetAttributeValue(document.DocumentElement, "allowtimestamp", item.AllowTimestamp);
                item.CodeFacade = XmlHelper.GetAttributeValue(document.DocumentElement, "codefacade", item.CodeFacade);
                item.Immutable = XmlHelper.GetAttributeValue(document.DocumentElement, "immutable", item.Immutable);
                item.IsAssociative = XmlHelper.GetAttributeValue(document.DocumentElement, "isassociative", item.IsAssociative);
                item.GeneratesDoubleDerived = XmlHelper.GetAttributeValue(document.DocumentElement, "generatesdoublederived", item.GeneratesDoubleDerived);
                item.IsTenant = XmlHelper.GetAttributeValue(document.DocumentElement, "isTenant", item.IsTenant);

                var tev = XmlHelper.GetAttributeValue(document.DocumentElement, "typedentity", item.TypedEntity.ToString());
                if (Enum.TryParse<TypedEntityConstants>(tev, true, out var te))
                    item.TypedEntity = te;

                item.Schema = XmlHelper.GetAttributeValue(document.DocumentElement, "schema", item.Schema);
                item.Summary = XmlHelper.GetNodeValue(document.DocumentElement, "summary", item.Summary);

                #endregion

                #region Fields

                var fieldsNodes = document.DocumentElement.SelectSingleNode("//fieldset");
                if (fieldsNodes != null)
                {
                    var nameList = new List<string>();
                    foreach (XmlNode n in fieldsNodes.ChildNodes)
                    {
                        var subItemID = XmlHelper.GetAttributeValue(n, "id", Guid.Empty);
                        var field = item.Fields.FirstOrDefault(x => x.Id == subItemID);
                        if (field == null)
                        {
                            field = new Field(item.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, XmlHelper.GetAttributeValue(n, "id", Guid.NewGuid())) });
                            item.Fields.Add(field);
                        }
                        field.Name = XmlHelper.GetAttributeValue(n, "name", string.Empty);
                        field.CodeFacade = XmlHelper.GetAttributeValue(n, "codefacade", field.CodeFacade);
                        nameList.Add(field.Name.ToLower());
                        field.Nullable = XmlHelper.GetAttributeValue(n, "nullable", field.Nullable);

                        var dtv = XmlHelper.GetAttributeValue(n, "datatype", field.DataType.ToString());
                        if (Enum.TryParse<DataTypeConstants>(dtv, true, out var dt))
                            field.DataType = dt;

                        var itv = XmlHelper.GetAttributeValue(n, "identity", field.Identity.ToString());
                        if (Enum.TryParse<IdentityTypeConstants>(itv, true, out var it))
                            field.Identity = it;

                        field.DataFormatString = XmlHelper.GetNodeValue(n, "dataformatstring", field.DataFormatString);
                        field.Default = XmlHelper.GetAttributeValue(n, "default", field.Default);
                        field.DefaultIsFunc = XmlHelper.GetAttributeValue(n, "defaultisfunc", field.DefaultIsFunc);
                        field.Formula = XmlHelper.GetAttributeValue(n, "formula", field.Formula);
                        field.IsIndexed = XmlHelper.GetAttributeValue(n, "isindexed", field.IsIndexed);
                        field.IsPrimaryKey = XmlHelper.GetAttributeValue(n, "isprimarykey", field.IsPrimaryKey);
                        field.IsCalculated = XmlHelper.GetAttributeValue(n, "Iscalculated", field.IsCalculated);
                        field.IsUnique = XmlHelper.GetAttributeValue(n, "isunique", field.IsUnique);
                        field.Length = XmlHelper.GetAttributeValue(n, "length", field.Length);
                        field.Scale = XmlHelper.GetAttributeValue(n, "scale", field.Scale);
                        field.SortOrder = XmlHelper.GetAttributeValue(n, "sortorder", field.SortOrder);
                        field.IsReadOnly = XmlHelper.GetAttributeValue(n, "isreadonly", field.IsReadOnly);
                        field.Obsolete = XmlHelper.GetAttributeValue(n, "obsolete", field.Obsolete);
                        field.Summary = XmlHelper.GetNodeValue(n, "summary", field.Summary);
                    }
                    if (item.Fields.Remove(x => !nameList.Contains(x.Name.ToLower())) > 0)
                        item.nHydrateModel.IsDirty = true;
                }

                #endregion

                LoadEntityIndexes(folder, item);

                //Order fields (skip for model that did not have sort order when saved)
                var fc = new FieldOrderComparer();
                if (item.Fields.Count(x => x.SortOrder > 0) > 0)
                    item.Fields.Sort(fc.Compare);
            }

            //Must load relations AFTER ALL entities are loaded
            foreach (var item in model.Entities)
            {
                LoadEntityRelations(folder, item);
                LoadEntityStaticData(folder, item);
            }

            #endregion

        }

        private static void LoadEntityIndexes(string folder, Entity entity)
        {
            XmlDocument document = null;
            var fileName = Path.Combine(folder, entity.Name + ".indexes.xml");
            if (!File.Exists(fileName)) return;
            try
            {
                document = new XmlDocument();
                document.Load(fileName);
            }
            catch (Exception ex)
            {
                //Do Nothing
                MessageBox.Show("The file '" + fileName + "' is not valid and could not be loaded!", "Load Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (XmlNode n in document.DocumentElement)
            {
                var id = XmlHelper.GetAttributeValue(n, "id", Guid.NewGuid());
                var newIndex = entity.Indexes.FirstOrDefault(x => x.Id == id);
                if (newIndex == null)
                {
                    newIndex = new Index(entity.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, id) });
                    entity.Indexes.Add(newIndex);
                }
                newIndex.Clustered = XmlHelper.GetAttributeValue(n, "clustered", newIndex.Clustered);
                newIndex.ImportedName = XmlHelper.GetAttributeValue(n, "importedname", newIndex.ImportedName);
                newIndex.IndexType = (IndexTypeConstants)XmlHelper.GetAttributeValue(n, "indextype", int.Parse(newIndex.IndexType.ToString("d")));
                newIndex.IsUnique = XmlHelper.GetAttributeValue(n, "isunique", newIndex.IsUnique);

                var indexColumnsNode = n.SelectSingleNode("indexcolumnset");
                if (indexColumnsNode != null)
                {
                    foreach (XmlNode m in indexColumnsNode.ChildNodes)
                    {
                        id = XmlHelper.GetAttributeValue(m, "id", Guid.NewGuid());
                        var newIndexColumn = newIndex.IndexColumns.FirstOrDefault(x => x.Id == id);
                        if (newIndexColumn == null)
                        {
                            newIndexColumn = new IndexColumn(entity.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, id) });
                            newIndex.IndexColumns.Add(newIndexColumn);
                        }
                        newIndexColumn.Ascending = XmlHelper.GetAttributeValue(m, "ascending", newIndexColumn.Ascending);
                        newIndexColumn.FieldID = XmlHelper.GetAttributeValue(m, "fieldid", newIndexColumn.FieldID);
                        newIndexColumn.SortOrder = XmlHelper.GetAttributeValue(m, "sortorder", newIndexColumn.SortOrder);
                        newIndexColumn.IsInternal = true;
                    }
                }
            }

        }

        private static void LoadEntityRelations(string folder, Entity entity)
        {
            XmlDocument document = null;
            var fileName = Path.Combine(folder, entity.Name + ".relations.xml");
            if (!File.Exists(fileName)) return;
            try
            {
                document = new XmlDocument();
                document.Load(fileName);
            }
            catch (Exception ex)
            {
                //Do Nothing
                MessageBox.Show("The file '" + fileName + "' is not valid and could not be loaded!", "Load Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (XmlNode n in document.DocumentElement)
            {
                var childid = XmlHelper.GetAttributeValue(n, "childid", Guid.Empty);
                var child = entity.nHydrateModel.Entities.FirstOrDefault(x => x.Id == childid);
                if (child != null)
                {
                    entity.ChildEntities.Add(child);
                    var connection = entity.Store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.Last() as EntityHasEntities;
                    connection.InternalId = XmlHelper.GetAttributeValue(n, "id", Guid.Empty);
                    connection.IsEnforced = XmlHelper.GetAttributeValue(n, "isenforced", connection.IsEnforced);
                    connection.DeleteAction = (DeleteActionConstants) Enum.Parse(typeof(DeleteActionConstants), XmlHelper.GetAttributeValue(n, "deleteaction", connection.DeleteAction.ToString()));
                    connection.RoleName = XmlHelper.GetAttributeValue(n, "rolename", connection.RoleName);

                    var relationColumnsNode = n.SelectSingleNode("relationfieldset");
                    if (relationColumnsNode != null)
                    {
                        foreach (XmlNode m in relationColumnsNode.ChildNodes)
                        {
                            var sourceFieldID = XmlHelper.GetAttributeValue(m, "sourcefieldid", Guid.Empty);
                            var targetFieldID = XmlHelper.GetAttributeValue(m, "targetfieldid", Guid.Empty);
                            var sourceField = entity.Fields.FirstOrDefault(x => x.Id == sourceFieldID);
                            var targetField = entity.nHydrateModel.Entities.SelectMany(x => x.Fields).FirstOrDefault(x => x.Id == targetFieldID);
                            if ((sourceField != null) && (targetField != null))
                            {
                                var id = XmlHelper.GetAttributeValue(m, "id", Guid.NewGuid());
                                var newRelationField = new RelationField(entity.Partition, new PropertyAssignment[] {new PropertyAssignment(ElementFactory.IdPropertyAssignment, id)});
                                newRelationField.SourceFieldId = sourceFieldID;
                                newRelationField.TargetFieldId = targetFieldID;
                                newRelationField.RelationID = connection.Id;
                                entity.nHydrateModel.RelationFields.Add(newRelationField);
                            }
                        }
                    }
                }

            }

        }

        private static void LoadEntityStaticData(string folder, Entity entity)
        {
            XmlDocument document = null;
            var fileName = Path.Combine(folder, entity.Name + ".staticdata.xml");
            if (!File.Exists(fileName)) return;
            try
            {
                document = new XmlDocument();
                document.Load(fileName);
            }
            catch (Exception ex)
            {
                //Do Nothing
                MessageBox.Show("The file '" + fileName + "' is not valid and could not be loaded!", "Load Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (XmlNode n in document.DocumentElement)
            {
                var newData = new StaticData(entity.Partition);
                entity.StaticDatum.Add(newData);
                newData.OrderKey = XmlHelper.GetAttributeValue(n, "orderkey", newData.OrderKey);
                newData.Value = XmlHelper.GetAttributeValue(n, "value", newData.Value);
                newData.ColumnKey = XmlHelper.GetAttributeValue(n, "columnkey", newData.ColumnKey);
            }

        }

        private static void LoadFromDisk(IEnumerable<View> list, nHydrateModel model, string rootFolder, Microsoft.VisualStudio.Modeling.Store store)
        {
            var folder = Path.Combine(rootFolder, FOLDER_VW);
            if (!Directory.Exists(folder)) return;

            #region Load other parameter/field information
            var fList = Directory.GetFiles(folder, "*.configuration.xml");
            foreach (var f in fList)
            {
                var document = new XmlDocument();
                try
                {
                    document.Load(f);
                }
                catch (Exception ex)
                {
                    //Do Nothing
                    MessageBox.Show("The file '" + f + "' is not valid and could not be loaded!", "Load Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var fi = new FileInfo(f);
                var name = fi.Name.Substring(0, fi.Name.Length - ".configuration.xml".Length).ToLower();
                var itemID = XmlHelper.GetAttributeValue(document.DocumentElement, "id", Guid.Empty);
                var item = list.FirstOrDefault(x => x.Id == itemID);
                if (item == null)
                {
                    item = new View(model.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, XmlHelper.GetAttributeValue(document.DocumentElement, "id", Guid.NewGuid())) });
                    model.Views.Add(item);
                }

                System.Windows.Forms.Application.DoEvents();

                item.Name = XmlHelper.GetAttributeValue(document.DocumentElement, "name", item.Name);
                item.CodeFacade = XmlHelper.GetAttributeValue(document.DocumentElement, "codefacade", item.CodeFacade);
                item.Schema = XmlHelper.GetAttributeValue(document.DocumentElement, "schema", item.Schema);
                item.GeneratesDoubleDerived = XmlHelper.GetAttributeValue(document.DocumentElement, "generatesdoublederived", item.GeneratesDoubleDerived);
                item.Summary = XmlHelper.GetNodeValue(document.DocumentElement, "summary", item.Summary);

                //Fields
                var fieldsNodes = document.DocumentElement.SelectSingleNode("//fieldset");
                if (fieldsNodes != null)
                {
                    var nameList = new List<string>();
                    foreach (XmlNode n in fieldsNodes.ChildNodes)
                    {
                        var subItemID = XmlHelper.GetAttributeValue(n, "id", Guid.Empty);
                        var field = item.Fields.FirstOrDefault(x => x.Id == subItemID);
                        if (field == null)
                        {
                            field = new ViewField(item.Partition, new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, XmlHelper.GetAttributeValue(n, "id", Guid.NewGuid())) });
                            item.Fields.Add(field);
                        }

                        field.Name = XmlHelper.GetAttributeValue(n, "name", field.Name);
                        field.CodeFacade = XmlHelper.GetAttributeValue(n, "codefacade", field.CodeFacade);
                        nameList.Add(field.Name.ToLower());
                        field.Nullable = XmlHelper.GetAttributeValue(n, "nullable", field.Nullable);
                        var dtv = XmlHelper.GetAttributeValue(n, "datatype", field.DataType.ToString());
                        if (Enum.TryParse<DataTypeConstants>(dtv, true, out var dt))
                            field.DataType = dt;
                        field.Default = XmlHelper.GetAttributeValue(n, "default", field.Default);
                        field.Length = XmlHelper.GetAttributeValue(n, "length", field.Length);
                        field.Scale = XmlHelper.GetAttributeValue(n, "scale", field.Scale);
                        field.Summary = XmlHelper.GetNodeValue(n, "summary", field.Summary);
                        field.IsPrimaryKey = XmlHelper.GetAttributeValue(n, "isprimarykey", field.IsPrimaryKey);
                    }
                    if (item.Fields.Remove(x => !nameList.Contains(x.Name.ToLower())) > 0)
                        item.nHydrateModel.IsDirty = true;
                }

            }
            #endregion

            #region Load SQL
            fList = Directory.GetFiles(folder, "*.sql");
            foreach (var f in fList)
            {
                var fi = new FileInfo(f);
                if (fi.Name.ToLower().EndsWith(".sql"))
                {
                    var name = fi.Name.Substring(0, fi.Name.Length - 4).ToLower();
                    var item = list.FirstOrDefault(x => x.Name.ToLower() == name);
                    if (item != null)
                    {
                        item.SQL = File.ReadAllText(f);
                        System.Windows.Forms.Application.DoEvents();
                    }
                }
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

        #region Private Helpers

        private static void WriteFileIfNeedBe(string fileName, string contents, List<string> generatedFileList)
        {
            if (fileName.ToLower().EndsWith(".xml"))
            {
                generatedFileList.Add(fileName);
                try
                {
                    //Load formatted original XML
                    var origXML = string.Empty;
                    if (File.Exists(fileName))
                    {
                        var xmlText = File.ReadAllText(fileName);
                        if (!string.IsNullOrEmpty(xmlText))
                        {
                            var documentCheck = new XmlDocument();
                            documentCheck.LoadXml(xmlText);
                            origXML = documentCheck.ToIndentedString();
                        }
                    }

                    //Load formatted new XML
                    var newXML = string.Empty;
                    {
                        var documentCheck = new XmlDocument();
                        documentCheck.LoadXml(contents);
                        newXML = documentCheck.ToIndentedString();
                    }

                    if (origXML == newXML)
                        return;
                    else
                        contents = newXML;
                }
                catch (Exception ex)
                {
                    //If there is an error then process like a non-XML file
                    //Do Nothing
                }
            }
            else
            {
                //Check if this is the same content and if so do nothing
                generatedFileList.Add(fileName);
                if (File.Exists(fileName))
                {
                    var t = File.ReadAllText(fileName);
                    if (t == contents)
                        return;
                }
            }

            File.WriteAllText(fileName, contents);
            System.Windows.Forms.Application.DoEvents();
        }

        private static void WriteReadMeFile(string folder, List<string> generatedFileList)
        {
            var f = Path.Combine(folder, "ReadMe.nHydrate.txt");
            WriteFileIfNeedBe(f, "This is a managed folder of a nHydrate model. You may change '*.configuration.xml' and '*.sql' files in any text editor if desired but do not add or remove files from this folder. This is a distributed model and making changes can break the model load.", generatedFileList);
        }

        private static void RemoveOrphans(string rootFolder, List<string> generatedFiles)
        {
            //Only get these specific folder in case there is version control or some other third-party application running
            //Only touch the files we know about
            var files = new List<string>();
            files.AddRange(Directory.GetFiles(rootFolder, "*.*", SearchOption.TopDirectoryOnly));
            files.AddRange(Directory.GetFiles(Path.Combine(rootFolder, "_Entities"), "*.*", SearchOption.TopDirectoryOnly));
            files.AddRange(Directory.GetFiles(Path.Combine(rootFolder, "_Views"), "*.*", SearchOption.TopDirectoryOnly));
            files.ToList().ForEach(x => x = x.ToLower());
            generatedFiles.ToList().ForEach(x => x = x.ToLower());

            foreach (var f in files)
            {
                var fi = new FileInfo(f);
                if (fi.Name.ToLower().Contains("readme.nhydrate.txt"))
                {
                    //Skip
                }
                else if (generatedFiles.Contains(f))
                {
                    //Skip
                }
                else
                {
                    File.Delete(f);
                }

            }
        }

        private static void ExtractToDirectory(string compressedFile, string destinationDirectoryName, bool overwrite)
        {
            using (var archive = System.IO.Compression.ZipFile.Open(compressedFile, System.IO.Compression.ZipArchiveMode.Update))
            {
                var di = Directory.CreateDirectory(destinationDirectoryName);
                var destinationDirectoryFullPath = di.FullName;
                foreach (var file in archive.Entries)
                {
                    var completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.FullName));
                    if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new IOException("Trying to extract file outside of destination directory. See this link for more info: https://snyk.io/research/zip-slip-vulnerability");
                    }

                    if (file.Name == string.Empty)
                    {
                        // Assuming Empty for Directory
                        Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                        continue;
                    }

                    if (!File.Exists(completeFileName))
                    {
                        var folder = (new FileInfo(completeFileName)).DirectoryName;
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                            System.Threading.Thread.Sleep(200);
                        }
                        file.ExtractToFile(completeFileName, overwrite);
                    }
                }
            }
        }

        #endregion

    }
}