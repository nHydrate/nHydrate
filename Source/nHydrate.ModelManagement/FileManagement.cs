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
        private const string FOLDER_ET = "_Entities";
        private const string FOLDER_VW = "_Views";

        public static DiskModel Load(string rootFolder, string modelName, out bool wasLoaded)
        {
            wasLoaded = false;
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

            //Yaml
            //var results2 = new DiskModelYaml();
            //LoadEntitiesYaml(modelFolder, results2);
            //LoadViewsYaml(modelFolder, results2);

            //Save the global model properties
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

            //Yaml
            //SaveViewsYaml(modelFolder, model, generatedFileList); //must come before entities
            //SaveEntitiesYaml(modelFolder, model, generatedFileList);

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

        private static void SaveEntitiesYaml(string rootFolder, DiskModel model, List<string> generatedFileList)
        {
            var folder = Path.Combine(rootFolder, FOLDER_ET);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            //Entities
            foreach (var obj in model.Entities)
            {
                var newEntity = new EntityYaml
                {
                    AllowCreateAudit = obj.allowcreateaudit != 0,
                    AllowModifyAudit = obj.allowmodifyaudit != 0,
                    AllowTimestamp = obj.allowtimestamp != 0,
                    CodeFacade = obj.codefacade,
                    GeneratesDoubleDerived = obj.generatesdoublederived != 0,
                    Id = obj.id,
                    Immutable = obj.immutable != 0,
                    IsAssociative = obj.isassociative != 0,
                    IsTenant = obj.isTenant != 0,
                    Name = obj.name,
                    Schema = obj.schema,
                    Summary = obj.summary,
                    Identity = obj.typedentity.ToEnum<Utilities.IdentityTypeConstants>(),
                };

                //Fields
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
                        Id = ff.id,
                        Identity = ff.identity,
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

                //Relations
                foreach (var rr in model.Relations.Where(x => x.id == obj.id))
                {
                    foreach (var relation in rr.relation)
                    {
                        var entity = model.Entities.FirstOrDefault(x => x.id == rr.id);
                        var entity2 = model.Entities.FirstOrDefault(x => x.id == rr.relation[0].childid);
                        var newRelation = new RelationYaml
                        {
                            ChildEntity = entity2.name,
                            ChildId = entity2.id,
                        };
                        newEntity.Relations.Add(newRelation);
                        newRelation.IsEnforced = rr.relation[0].isenforced != 0;
                        newRelation.DeleteAction = rr.relation[0].deleteaction;
                        newRelation.RoleName = rr.relation[0].rolename;
                        newRelation.Summary = rr.relation[0].summary;
                        foreach (var fsi in rr.relation[0].relationfieldset)
                        {
                            var newRelationField = new RelationFieldYaml
                            {
                                Id = fsi.id,
                                SourceFieldId = fsi.sourcefieldid,
                                SourceFieldName = entity.fieldset.FirstOrDefault(x => x.id == fsi.sourcefieldid)?.name,
                                TargetFieldId = fsi.targetfieldid,
                                TargetFieldName = entity2.fieldset.FirstOrDefault(x => x.id == fsi.targetfieldid)?.name,
                            };
                            newRelation.Fields.Add(newRelationField);
                        }
                    }
                }

                //Indexes
                foreach (var ii in model.Indexes.Where(x => x.id == obj.id))
                {
                    foreach (var ifield in ii.index)
                    {
                        var newIndex = newEntity.Indexes.AddItem(new IndexYaml
                        {
                            Clustered = ifield.clustered != 0,
                            Id = ifield.id,
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
                                FieldId = i2.fieldid,
                                Id = i2.id,
                            });
                        }
                    }
                }

                //Static data
                foreach (var sd in model.StaticData.Where(x => x.id == obj.id))
                {
                    foreach (var dd in sd.data.OrderBy(x => x.orderkey))
                    {
                        newEntity.StaticData.AddItem(new StaticDataYaml
                        {
                            Key = dd.columnkey,
                            Value = dd.value
                        });
                    }
                }

                SaveYamlObject(newEntity, Path.Combine(folder, obj.name + ".yaml"), generatedFileList);
            }
        }

        private static void SaveViewsYaml(string rootFolder, DiskModel model, List<string> generatedFileList)
        {
            var folder = Path.Combine(rootFolder, FOLDER_VW);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            //Views
            foreach (var obj in model.Views)
            {
                var newView = new ViewYaml
                {
                    CodeFacade = obj.codefacade,
                    GeneratesDoubleDerived = obj.generatesdoublederived != 0,
                    Id = obj.id,
                    Sql = obj.sql,
                    Name = obj.name,
                    Schema = obj.schema,
                    Summary = obj.summary,
                };

                //Fields
                foreach (var ff in obj.fieldset)
                {
                    newView.Fields.AddItem(new ViewFieldYaml
                    {
                        CodeFacade = ff.codefacade,
                        Datatype = ff.datatype.ToEnum<Utilities.DataTypeConstants>(),
                        Default = ff.@default,
                        Id = ff.id,
                        IsPrimaryKey = ff.isprimarykey != 0,
                        Length = ff.length,
                        Name = ff.name,
                        Nullable = ff.nullable != 0,
                        Scale = ff.scale,
                        Summary = ff.summary,
                    });
                }

                SaveYamlObject(newView, Path.Combine(folder, obj.name + ".yaml"), generatedFileList);
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
            generatedFileList.Add(fileName);
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
