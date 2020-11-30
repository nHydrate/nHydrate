using nHydrate.ModelManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace nHydrate.ModelManagement
{
    public static class FileManagement
    {
        private const string FOLDER_ET = "_entities";
        private const string FOLDER_VW = "_views";
        public const string ModelExtension = ".nhydrate.yaml";
        public const string OldModelExtension = ".nhydrate";

        public static DiskModel Load(string rootFolder, string modelName, out bool wasLoaded)
        {
            wasLoaded = false;
            if (modelName.EndsWith(".yaml")) return null;
            var modelFile = Path.Combine(rootFolder, modelName);
            var fi = new FileInfo(modelFile);
            var showError = (fi.Length > 10); //New file is small so show no error if creating new

            var folderName = modelName.Replace(".nhydrate", ".model");
            var modelFolder = GetModelFolder(rootFolder, folderName);

            //If the model folder does NOT exist
            if (!Directory.Exists(modelFolder))
            {
                if (showError)
                {
                    throw new Exception("The model folder was not found.");
                }
            }
            else wasLoaded = true;

            //Remove old ZIP file. It is no longer used
            try
            {
                var compressedFile = Path.Combine(rootFolder, modelName + ".zip");
                if (File.Exists(compressedFile))
                    File.Delete(compressedFile);
            }
            catch { }

            var results = new DiskModel();
            LoadEntities(modelFolder, results);
            LoadViews(modelFolder, results);

            //Load the global model properties
            var globalFile = Path.Combine(modelFolder, "model.xml");
            if (File.Exists(globalFile))
                results.ModelProperties = GetObject<ModelProperties>(globalFile);

            RemoveNullStrings(results.ModelProperties);

            #region Clean up
            //Ensure all arrays are not null
            foreach (var obj in results.Entities)
            {
                if (obj.fieldset == null) obj.fieldset = new Entity.configurationField[0];
            }
            foreach (var obj in results.Indexes)
            {
                if (obj.index == null) obj.index = new Index.configurationIndex[0];
                foreach (var obj2 in obj.index)
                {
                    if (obj2.indexcolumnset == null) obj2.indexcolumnset = new Index.configurationIndexColumn[0];
                }
            }
            foreach (var obj in results.Relations)
            {
                if (obj.relation == null) obj.relation = new Relation.configurationRelation[0];
                foreach (var obj2 in obj.relation)
                {
                    if (obj2.relationfieldset == null) obj2.relationfieldset = new Relation.configurationRelationField[0];
                }
            }
            foreach (var obj in results.StaticData)
            {
                if (obj.data == null) obj.data = new StaticData.configurationData[0];
            }
            foreach (var obj in results.Views)
            {
                if (obj.fieldset == null) obj.fieldset = new View.configurationField[0];
            }
            #endregion

            return results;
        }

        private static void LoadEntities(string rootFolder, DiskModel results)
        {
            var folder = Path.Combine(rootFolder, FOLDER_ET);
            if (!Directory.Exists(folder)) return;

            //Load Entities
            foreach (var f in Directory.GetFiles(folder, "*.configuration.xml"))
            {
                results.Entities.Add(GetObject<Entity.configuration>(f));
            }

            //Load Indexes
            foreach (var f in Directory.GetFiles(folder, "*.indexes.xml"))
            {
                results.Indexes.Add(GetObject<nHydrate.ModelManagement.Index.configuration>(f));
            }

            //Clean up indexes
            var indexIdList = new List<string>();
            foreach(var i1 in results.Indexes)
            {
                var newList = new List<Index.configurationIndex>();
                foreach (var i2 in i1.index)
                {
                    if (!indexIdList.Contains(i2.id))
                    {
                        indexIdList.Add(i2.id);
                        newList.Add(i2);
                    }
                }
                i1.index = newList.ToArray();
            }

            //Load Relations
            foreach (var f in Directory.GetFiles(folder, "*.relations.xml"))
            {
                results.Relations.Add(GetObject<Relation.configuration>(f));
            }

            //Load Static Data
            foreach (var f in Directory.GetFiles(folder, "*.staticdata.xml"))
            {
                results.StaticData.Add(GetObject<StaticData.configuration>(f));
            }
        }

        private static void LoadViews(string rootFolder, DiskModel results)
        {
            var folder = Path.Combine(rootFolder, FOLDER_VW);

            //Load Views
            if (Directory.Exists(folder))
            {
                foreach (var f in Directory.GetFiles(folder, "*.configuration.xml"))
                {
                    results.Views.Add(GetObject<nHydrate.ModelManagement.View.configuration>(f));
                }

                foreach (var f in Directory.GetFiles(folder, "*.sql"))
                {
                    var fi = new FileInfo(f);
                    var name = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length).ToLower();
                    var item = results.Views.FirstOrDefault(x => x.name.ToLower() == name);
                    if (item != null)
                        item.sql = File.ReadAllText(f);
                }
            }
        }

        public static DiskModelYaml Load2(string rootFolder, string modelName, out bool wasLoaded)
        {
            wasLoaded = false;
            var modelFile = Path.Combine(rootFolder, modelName);
            var fi = new FileInfo(modelFile);
            var showError = (fi.Length > 10); //New file is small so show no error if creating new

            var modelFolder = GetModelFolder(rootFolder, modelName.Replace(".nhydrate", ".model"));
            if (modelName.EndsWith(".yaml"))
                modelFolder = rootFolder;

            //If the model file is empty and folder has 1 file
            if (fi.Length == 0 && Directory.EnumerateFileSystemEntries(modelFolder).Count() == 1)
            {
                //The model file is 0 bytes and there is no folder, so this is a new model
                wasLoaded = true;
            }
            else if (!Directory.Exists(modelFolder))
            {
                if (showError)
                {
                    throw new Exception("The model folder was not found.");
                }
            }
            else wasLoaded = true;

            //Remove old ZIP file. It is no longer used
            try
            {
                var compressedFile = Path.Combine(rootFolder, modelName + ".zip");
                if (File.Exists(compressedFile))
                    File.Delete(compressedFile);
            }
            catch { }

            var results = new DiskModelYaml();

            //Determine if model is old XML style and convert
            var oldModel = Load(rootFolder, modelName, out bool wasOldLoaded);
            if (wasOldLoaded && oldModel.ModelProperties.Id != Guid.Empty)
            {
                ConvertEntitiesOld2Yaml(rootFolder, oldModel, results);
                ConvertViewsOld2Yaml(rootFolder, oldModel, results);
                results.ModelProperties = oldModel.ModelProperties;
            }
            else
            {
                LoadEntitiesYaml(modelFolder, results);
                LoadViewsYaml(modelFolder, results);

                //Save the global model properties
                var globalFile = Path.Combine(modelFolder, "model.yaml");
                if (File.Exists(globalFile))
                    results.ModelProperties = GetYamlObject<ModelProperties>(globalFile) ?? new ModelProperties { Id = Guid.NewGuid() };
            }

            return results;
        }

        private static void LoadEntitiesYaml(string rootFolder, DiskModelYaml results)
        {
            var folder = Path.Combine(rootFolder, FOLDER_ET);
            if (!Directory.Exists(folder)) return;
            var fList = Directory.GetFiles(folder, "*.yaml");
            //Only use Yaml if there are actual files
            if (!fList.Any()) return;

            results.Entities.Clear();
            foreach (var f in fList)
                results.Entities.Add(GetYamlObject<EntityYaml>(f));

            //Fill in field IDs if need be
            results.Entities
                .Where(x => x.Id == Guid.Empty)
                .ToList()
                .ForEach(x => x.Id = Guid.NewGuid());

            //Validate model. Ensure all Guids match up, etc
            var allEntities = results.Entities.Select(x => x.Id).ToList();
            if (allEntities.Count != allEntities.Distinct().Count())
                throw new ModelException("Entities must have unique ID values.");

            foreach (var entity in results.Entities)
            {
                //Fill in field IDs if need be
                entity.Fields
                    .Where(x => x.Id == Guid.Empty)
                    .ToList()
                    .ForEach(x => x.Id = Guid.NewGuid());

                if (entity.Fields.Count != entity.Fields.Select(x => x.Id).Count())
                    throw new ModelException($"Entity: '{entity.Name}': All fields must have a unique ID.");

                //Indexes
                if (entity.Indexes.Count(x => x.Clustered) > 1)
                    throw new ModelException($"Entity: '{entity.Name}': An entity can have only one clustered index.");

                foreach (var index in entity.Indexes)
                {
                    foreach (var field in index.Fields)
                    {
                        var targetField = entity.Fields.FirstOrDefault(x => x.Id == field.FieldId);
                        if (field.FieldId == Guid.Empty)
                            targetField = entity.Fields.FirstOrDefault(x => x.Name?.ToLower() == field.FieldName?.ToLower());

                        if (targetField == null)
                            throw new ModelException($"Entity: '{entity.Name}': The index must map to an existing field.");

                        field.FieldId = targetField.Id;
                        field.FieldName = targetField.Name;
                    }
                }

                //Relations
                foreach (var relation in entity.Relations)
                {
                    if (!relation.Fields.Any())
                        throw new ModelException($"Entity: '{entity.Name}': The relation must have at least one field.");

                    var foreignEntity = results.Entities.FirstOrDefault(x => x.Id == relation.ForeignEntityId);
                    if (relation.ForeignEntityId == Guid.Empty)
                        foreignEntity = results.Entities.FirstOrDefault(x => x.Name?.ToLower() == relation.ForeignEntityName?.ToLower());

                    if (foreignEntity == null)
                        throw new ModelException($"Entity: '{entity.Name}': The relation must map to an existing entity.");

                    relation.ForeignEntityName = foreignEntity.Name;
                    relation.ForeignEntityId = foreignEntity.Id;
                    foreach (var field in relation.Fields)
                    {
                        var primaryField = entity.Fields.FirstOrDefault(x => x.Id == field.PrimaryFieldId);
                        if (field.PrimaryFieldId == Guid.Empty)
                            primaryField = entity.Fields.FirstOrDefault(x => x.Name?.ToLower() == field.PrimaryFieldName?.ToLower());

                        var foreignField = foreignEntity.Fields.FirstOrDefault(x => x.Id == field.ForeignFieldId);
                        if(field.ForeignFieldId == Guid.Empty)
                            foreignField = foreignEntity.Fields.FirstOrDefault(x => x.Name?.ToLower() == field.ForeignFieldName?.ToLower());

                        if (primaryField == null)
                            throw new ModelException($"Entity: '{entity.Name}': The relation primary field must map to an existing field.");

                        if (foreignField == null)
                            throw new ModelException($"Entity: '{entity.Name}': The relation foreign field must map to an existing field.");

                        field.PrimaryFieldId = primaryField.Id;
                        field.PrimaryFieldName = primaryField.Name;

                        field.ForeignFieldId = foreignField.Id;
                        field.ForeignFieldName = foreignField.Name;
                    }
                }
            }

        }

        private static void LoadViewsYaml(string rootFolder, DiskModelYaml results)
        {
            var folder = Path.Combine(rootFolder, FOLDER_VW);
            if (!Directory.Exists(folder)) return;
            var fList = Directory.GetFiles(folder, "*.yaml");
            //Only use Yaml if there are actual files
            if (!fList.Any()) return;

            results.Views.Clear();
            foreach (var f in fList)
                results.Views.Add(GetYamlObject<ViewYaml>(f));

            foreach (var f in Directory.GetFiles(folder, "*.sql"))
            {
                var fi = new FileInfo(f);
                var name = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length).ToLower();
                var item = results.Views.FirstOrDefault(x => x.Name.ToLower() == name);
                if (item != null)
                    item.Sql = File.ReadAllText(f);
            }
        }

        private static T GetObject<T>(string fileName)
        {
            var reader = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var file = new System.IO.StreamReader(fileName))
            {
                return (T)reader.Deserialize(file);
            }
        }

        private static T GetYamlObject<T>(string fileName)
        {
            var serializer = new YamlDotNet.Serialization.DeserializerBuilder()
                   .WithTypeConverter(new SystemTypeTypeConverter())
                   .Build();
            var yaml = File.ReadAllText(fileName);
            return serializer.Deserialize<T>(yaml);
        }

        public static void Save(string rootFolder, string modelName, DiskModel model)
        {
            var folderName = modelName.Replace(".nhydrate", ".model");
            var modelFolder = GetModelFolder(rootFolder, folderName);

            var generatedFileList = new List<string>();
            SaveViews(modelFolder, model, generatedFileList); //must come before entities
            SaveEntities(modelFolder, model, generatedFileList);

            //Save the global model properties
            RemoveNullStrings(model.ModelProperties);
            SaveObject(model.ModelProperties, Path.Combine(modelFolder, "model.xml"), generatedFileList);

            //Do not remove diagram file
            generatedFileList.Add(Path.Combine(modelFolder, "diagram.xml"));

            RemoveOrphans(modelFolder, generatedFileList);
        }

        private static void SaveEntities(string rootFolder, DiskModel model, List<string> generatedFileList)
        {
            var folder = Path.Combine(rootFolder, FOLDER_ET);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            //Save Entities
            foreach (var obj in model.Entities)
            {
                var f = Path.Combine(folder, obj.name + ".configuration.xml");
                SaveObject(obj, f, generatedFileList);
            }

            //Save Indexes
            foreach (var obj in model.Indexes)
            {
                var entity = model.Entities.FirstOrDefault(x => x.id == obj.id);
                var f = Path.Combine(folder, entity.name + ".indexes.xml");
                SaveObject(obj, f, generatedFileList);
            }

            //Save Relations
            foreach (var obj in model.Relations)
            {
                var entity = model.Entities.FirstOrDefault(x => x.id == obj.id);
                var f = Path.Combine(folder, entity.name + ".relations.xml");
                SaveObject(obj, f, generatedFileList);
            }

            //Save Static Data
            foreach (var obj in model.StaticData)
            {
                var entity = model.Entities.FirstOrDefault(x => x.id == obj.id);
                var f = Path.Combine(folder, entity.name + ".staticdata.xml");
                SaveObject(obj, f, generatedFileList);
            }

            WriteReadMeFile(folder, generatedFileList);
        }

        private static void SaveViews(string rootFolder, DiskModel model, List<string> generatedFileList)
        {
            var folder = Path.Combine(rootFolder, FOLDER_VW);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            //Save Views
            foreach (var obj in model.Views)
            {
                var f = Path.Combine(folder, obj.name + ".configuration.xml");
                SaveObject(obj, f, generatedFileList);

                var f1 = Path.Combine(folder, obj.name + ".sql");
                WriteFileIfNeedBe(f1, obj.sql, new List<string>());
            }

            WriteReadMeFile(folder, generatedFileList);
        }

        public static void Save2(string rootFolder, string modelName, DiskModelYaml model)
        {
            var modelFolder = GetModelFolder(rootFolder, modelName.Replace(".nhydrate", ".model"));
            if (modelName.EndsWith(ModelExtension))
                modelFolder = rootFolder;

            var generatedFileList = new List<string>();
            SaveViewsYaml(modelFolder, model, generatedFileList); //must come before entities
            SaveEntitiesYaml(modelFolder, model, generatedFileList);

            //Save the global model properties
            SaveYamlObject(model.ModelProperties, Path.Combine(modelFolder, "model" + ModelExtension), generatedFileList);

            //Do not remove diagram file
            generatedFileList.Add(Path.Combine(modelFolder, "diagram.xml"));

            RemoveOrphans(modelFolder, generatedFileList);
        }

        private static void SaveEntitiesYaml(string rootFolder, DiskModelYaml model, List<string> generatedFileList)
        {
            var folder = Path.Combine(rootFolder, FOLDER_ET);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            //Save Entities
            foreach (var obj in model.Entities)
            {
                var f = Path.Combine(folder, $"{obj.Name}.yaml".ToLower());
                SaveYamlObject(obj, f, generatedFileList);
            }
        }

        private static void SaveViewsYaml(string rootFolder, DiskModelYaml model, List<string> generatedFileList)
        {
            var folder = Path.Combine(rootFolder, FOLDER_VW);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            //Save Views
            foreach (var obj in model.Views)
            {
                var f = Path.Combine(folder, $"{obj.Name}.yaml".ToLower());
                SaveYamlObject(obj, f, generatedFileList);
            }
        }

        private static void ConvertEntitiesOld2Yaml(string rootFolder, DiskModel model, DiskModelYaml model2)
        {
            var folder = Path.Combine(rootFolder, FOLDER_ET);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            foreach (var obj in model.Entities)
            {
                var newEntity = model2.Entities.AddItem(new EntityYaml
                {
                    AllowCreateAudit = obj.allowcreateaudit != 0,
                    AllowModifyAudit = obj.allowmodifyaudit != 0,
                    AllowTimestamp = obj.allowtimestamp != 0,
                    CodeFacade = obj.codefacade,
                    GeneratesDoubleDerived = obj.generatesdoublederived != 0,
                    Id = obj.id.ToGuid(),
                    Immutable = obj.immutable != 0,
                    TypedTable = obj.typedentity.ToEnum<Utilities.TypedTableConstants>(),
                    IsAssociative = obj.isassociative != 0,
                    IsTenant = obj.isTenant != 0,
                    Name = obj.name,
                    Schema = obj.schema,
                    Summary = obj.summary,
                });

                #region Fields
                foreach (var ff in obj.fieldset.OrderBy(x => x.sortorder))
                {
                    newEntity.Fields.AddItem(new EntityFieldYaml
                    {
                        CodeFacade = ff.codefacade,
                        DataFormatString = ff.dataformatstring,
                        Datatype = ff.datatype.ToEnum<Utilities.DataTypeConstants>(),
                        Default = ff.@default,
                        DefaultIsFunc = ff.defaultisfunc != 0,
                        Formula = ff.formula,
                        Id = ff.id.ToGuid(),
                        Identity = ff.identity.ToEnum<Utilities.IdentityTypeConstants>(),
                        IsCalculated = ff.Iscalculated != 0,
                        IsIndexed = ff.isindexed != 0,
                        IsPrimaryKey = ff.isprimarykey != 0,
                        IsReadonly = ff.isreadonly != 0,
                        IsUnique = ff.isunique != 0,
                        Length = ff.length,
                        Name = ff.name,
                        Nullable = ff.nullable != 0,
                        Obsolete = ff.obsolete != 0,
                        Scale = ff.scale,
                        Summary = ff.summary,
                    });
                }
                #endregion

                #region Indexes
                foreach (var ii in model.Indexes.Where(x => x.id == obj.id))
                {
                    foreach (var ifield in ii.index)
                    {
                        var newIndex = newEntity.Indexes.AddItem(new IndexYaml
                        {
                            Clustered = ifield.clustered != 0,
                            //Id = ifield.id.ToGuid(),
                            ImportedName = ifield.importedname,
                            IndexType = (Utilities.IndexTypeConstants)ifield.indextype,
                            IsUnique = ifield.isunique != 0,
                            Summary = ifield.summary,
                        });

                        foreach (var i2 in ifield.indexcolumnset.OrderBy(x => x.sortorder))
                        {
                            newIndex.Fields.AddItem(new IndexFieldYaml
                            {
                                Ascending = i2.ascending != 0,
                                FieldId = i2.fieldid.ToGuid(),
                                FieldName = newEntity.Fields.FirstOrDefault(x => x.Id == i2.fieldid.ToGuid())?.Name,
                                //Id = i2.id.ToGuid(),
                            });
                        }
                    }
                }
                #endregion

                #region Static data
                foreach (var sd in model.StaticData.Where(x => x.id == obj.id))
                {
                    foreach (var dd in sd.data.OrderBy(x => x.orderkey))
                    {
                        newEntity.StaticData.AddItem(new StaticDataYaml
                        {
                            ColumnId = dd.columnkey.ToGuid(),
                            Value = dd.value,
                            SortOrder = dd.orderkey,
                        });
                    }
                }
                #endregion
            }

            #region Relations (After all entities are loaded)

            foreach (var obj in model.Entities)
            {
                var newEntity = model2.Entities.First(x => x.Id == new Guid(obj.id));
                foreach (var rr in model.Relations.Where(x => x.id == obj.id))
                {
                    foreach (var relation in rr.relation)
                    {
                        var entity = model.Entities.FirstOrDefault(x => x.id == rr.id);
                        var entity2 = model.Entities.FirstOrDefault(x => x.id == relation.childid);
                        var newRelation = newEntity.Relations.AddItem(new RelationYaml
                        {
                            //Id = relation.id.ToGuid(),
                            ForeignEntityName = entity2.name,
                            ForeignEntityId = entity2.id.ToGuid(),
                        });
                        newRelation.IsEnforced = relation.isenforced != 0;
                        newRelation.DeleteAction = relation.deleteaction.ToEnum<Utilities.DeleteActionConstants>();
                        newRelation.RoleName = relation.rolename;
                        newRelation.Summary = relation.summary;
                        foreach (var fsi in relation.relationfieldset)
                        {
                            var newRelationField = new RelationFieldYaml
                            {
                                //Id = fsi.id.ToGuid(),
                                PrimaryFieldId = fsi.sourcefieldid.ToGuid(),
                                PrimaryFieldName = entity.fieldset.FirstOrDefault(x => x.id == fsi.sourcefieldid)?.name,
                                ForeignFieldId = fsi.targetfieldid.ToGuid(),
                                ForeignFieldName = entity2.fieldset.FirstOrDefault(x => x.id == fsi.targetfieldid)?.name,
                            };
                            newRelation.Fields.AddItem(newRelationField);
                        }
                    }
                }
            }
            #endregion
        }

        private static void ConvertViewsOld2Yaml(string rootFolder, DiskModel model, DiskModelYaml model2)
        {
            var folder = Path.Combine(rootFolder, FOLDER_VW);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            //Views
            foreach (var obj in model.Views)
            {
                var newView = model2.Views.AddItem(new ViewYaml
                {
                    CodeFacade = obj.codefacade,
                    GeneratesDoubleDerived = obj.generatesdoublederived != 0,
                    Id = obj.id.ToGuid(),
                    Sql = obj.sql,
                    Name = obj.name,
                    Schema = obj.schema,
                    Summary = obj.summary,
                });

                //Fields
                foreach (var ff in obj.fieldset)
                {
                    newView.Fields.AddItem(new ViewFieldYaml
                    {
                        CodeFacade = ff.codefacade,
                        Datatype = ff.datatype.ToEnum<Utilities.DataTypeConstants>(),
                        Default = ff.@default,
                        Id = ff.id.ToGuid(),
                        IsPrimaryKey = ff.isprimarykey != 0,
                        Length = ff.length,
                        Name = ff.name,
                        Nullable = ff.nullable != 0,
                        Scale = ff.scale,
                        Summary = ff.summary,
                    });
                }
            }
        }

        private static void SaveObject<T>(T obj, string fileName, List<string> generatedFileList)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var wfile = new System.IO.StreamWriter(fileName))
            {
                writer.Serialize(wfile, obj, ns);
            }
            generatedFileList.Add(fileName);
        }

        private static void SaveYamlObject<T>(T obj, string fileName, List<string> generatedFileList)
        {
            var serializer = new YamlDotNet.Serialization.SerializerBuilder()
                    .WithTypeConverter(new SystemTypeTypeConverter())
                    .Build();

            var yaml = serializer.Serialize(obj);
            File.WriteAllText(fileName, yaml);
            generatedFileList.Add(fileName);
        }

        #region Private Helpers

        private static string GetModelFolder(string rootFolder, string modelName)
        {
            return Path.Combine(rootFolder, "_" + modelName);
        }

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
            files.AddRange(Directory.GetFiles(Path.Combine(rootFolder, "_entities"), "*.*", SearchOption.TopDirectoryOnly));
            files.AddRange(Directory.GetFiles(Path.Combine(rootFolder, "_views"), "*.*", SearchOption.TopDirectoryOnly));
            files.ToList().ForEach(x => x = x.ToLower());
            generatedFiles.ToList().ForEach(x => x = x.ToLower());

            foreach (var f in files)
            {
                var fi = new FileInfo(f);
                if (generatedFiles.Contains(f))
                {
                    //Skip
                }
                else
                {
                    File.Delete(f);
                }
            }
        }

        private static string ToIndentedString(this XmlDocument doc)
        {
            var stringWriter = new StringWriter(new StringBuilder());
            var xmlTextWriter = new XmlTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented,
                IndentChar = '\t'
            };
            doc.Save(xmlTextWriter);
            var t = stringWriter.ToString();
            t = t.Replace(@" encoding=""utf-16""", string.Empty);
            return t;
        }

        private static void RemoveNullStrings(object obj)
        {
            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    var v = propertyInfo.GetValue(obj);
                    if (v == null)
                        propertyInfo.SetValue(obj, string.Empty);
                }
            }

        }
        #endregion

    }
}
