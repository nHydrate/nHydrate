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
            var fList = Directory.GetFiles(folder, "*.configuration.xml");
            foreach (var f in fList)
            {
                results.Entities.Add(GetObject<Entity.configuration>(f));
            }

            //Load Indexes
            fList = Directory.GetFiles(folder, "*.indexes.xml");
            foreach (var f in fList)
            {
                results.Indexes.Add(GetObject<nHydrate.ModelManagement.Index.configuration>(f));
            }

            //Load Relations
            fList = Directory.GetFiles(folder, "*.relations.xml");
            foreach (var f in fList)
            {
                results.Relations.Add(GetObject<Relation.configuration>(f));
            }

            //Load Static Data
            fList = Directory.GetFiles(folder, "*.staticdata.xml");
            foreach (var f in fList)
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
                var fList = Directory.GetFiles(folder, "*.configuration.xml");
                foreach (var f in fList)
                {
                    results.Views.Add(GetObject<nHydrate.ModelManagement.View.configuration>(f));
                }

                fList = Directory.GetFiles(folder, "*.sql");
                foreach (var f in fList)
                {
                    var fi = new FileInfo(f);
                    if (fi.Name.ToLower().EndsWith(".sql"))
                    {
                        var name = fi.Name.Substring(0, fi.Name.Length - 4).ToLower();
                        var item = results.Views.FirstOrDefault(x => x.name.ToLower() == name);
                        if (item != null)
                        {
                            item.sql = File.ReadAllText(f);
                        }
                    }

                }
            }

        }

        private static T GetObject<T> (string fileName)
        {
            var reader =    new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var file = new System.IO.StreamReader(fileName))
            {
                return  (T)reader.Deserialize(file);
            }

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


            //YAMLYAMLYAMLYAMLYAMLYAMLYAMLYAMLYAMLYAMLYAMLYAML
            foreach (var obj in model.Entities)
            {
                var serializer = new YamlDotNet.Serialization.SerializerBuilder()
                        .WithTypeConverter(new SystemTypeTypeConverter())
                        .Build();

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
                    Type = obj.type,
                    Typedentity = obj.typedentity,
                };

                foreach (var ff in obj.fieldset)
                {
                    newEntity.Fields.Add(new EntityFieldYaml
                    {
                        CodeFacade = ff.codefacade,
                        DataFormatString = ff.dataformatstring,
                        Datatype =  ff.datatype.ToEnum<Utilities.DataTypeConstants>(),
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
                        SortOrder = ff.sortorder,
                        Summary = ff.summary,
                    });
                }

                foreach (var rr in model.Relations.Where(x=>x.id == obj.id))
                {
                    if (rr.relation.Any())
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
                            var newRelationField = new RelationFieldYaml {
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

                foreach (var ii in model.Indexes.Where(x => x.id == obj.id))
                {
                    var entity = model.Entities.FirstOrDefault(x => x.id == ii.id);
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

                        foreach (var i2 in ifield.indexcolumnset)
                        {
                            newIndex.Fields.AddItem(new IndexFieldYaml
                            {
                                Ascending = i2.ascending != 0,
                                FieldId = i2.fieldid,
                                Id = i2.id,
                                SortOrder = i2.sortorder,
                            });
                        }
                    }
                }

                var yaml = serializer.Serialize(newEntity);
                var f = Path.Combine(folder, obj.name + ".yaml");
                generatedFileList.Add(f);
                File.WriteAllText(f, yaml);
            }
            //YAMLYAMLYAMLYAMLYAMLYAMLYAMLYAMLYAMLYAMLYAMLYAML



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
