using System;
using System.Linq;
using System.Text;
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslValidation = global::Microsoft.VisualStudio.Modeling.Validation;
using System.IO;
using System.Xml;

namespace nHydrate.Dsl
{
    partial class nHydrateSerializationHelper
    {
        private const string LAST_MODEL_MODEL_COMPATIBLE = "5.1.2.115";

        public override void SaveModelAndDiagram(Microsoft.VisualStudio.Modeling.SerializationResult serializationResult, nHydrateModel modelRoot, string modelFileName, nHydrateDiagram diagram, string diagramFileName, Encoding encoding,
            bool writeOptionalPropertiesWithDefaultValue)
        {
            var mainInfo = new FileInfo(modelFileName);
            modelRoot.ModelFileName = modelFileName;
            nHydrate.Dsl.Custom.SQLFileManagement.SaveToDisk(modelRoot, mainInfo.DirectoryName, mainInfo.Name, diagram);
            base.SaveModelAndDiagram(serializationResult, modelRoot, modelFileName, diagram, diagramFileName, encoding, writeOptionalPropertiesWithDefaultValue);

            //Model File
            if (modelRoot.ModelToDisk)
            {
                var document = new XmlDocument();
                document.Load(modelFileName);

                //Remove entire node for Views, Stored Procedures, and Functions
                for (var ii = document.DocumentElement.ChildNodes.Count - 1; ii >= 0; ii--)
                {
                    var n = document.DocumentElement.ChildNodes[ii];
                    document.DocumentElement.RemoveChild(n);
                }

                document.Save(modelFileName);
            }

            //Diagram File
            //Now gut the diagram file
            var diagramFile = modelFileName + ".diagram";
            if (modelRoot.ModelToDisk)
            {
                if (File.Exists(diagramFile))
                {
                    var document = new XmlDocument();
                    document.Load(diagramFile);

                    //Remove all child nodes
                    var n = document.DocumentElement.SelectSingleNode("nestedChildShapes");
                    if (n != null)
                    {
                        document.DocumentElement.RemoveChild(n);
                        document.Save(diagramFile);
                    }

                }
            }
            else
            {
                //strip out all the colors from the diagram file
                if (File.Exists(diagramFile))
                {
                    var document = new XmlDocument();
                    document.Load(diagramFile);
                    var list = document.DocumentElement.SelectNodes("//elementListCompartment");
                    foreach (XmlNode n in list)
                    {
                        n.Attributes.RemoveNamedItem("fillColor");
                        n.Attributes.RemoveNamedItem("outlineColor");
                        n.Attributes.RemoveNamedItem("textColor");
                        n.Attributes.RemoveNamedItem("titleTextColor");
                        n.Attributes.RemoveNamedItem("itemTextColor");
                    }

                    document.Save(diagramFile);
                }
            }

        }

        private nHydrateModel _model = null;

        public override nHydrateModel LoadModelAndDiagram(DslModeling::SerializationResult serializationResult, DslModeling::Partition modelPartition, string modelFileName, DslModeling::Partition diagramPartition, string diagramFileName,
            DslModeling::ISchemaResolver schemaResolver, DslValidation::ValidationController validationController, DslModeling::ISerializerLocator serializerLocator)
        {
            var modelRoot = base.LoadModelAndDiagram(serializationResult, modelPartition, modelFileName, diagramPartition, diagramFileName, schemaResolver, validationController, serializerLocator);
            _model = modelRoot;

            //Verify that we can open the model
            var thisAssem = System.Reflection.Assembly.GetExecutingAssembly();
            var thisAssemName = thisAssem.GetName();
            var toolVersion = thisAssemName.Version;
            var modelVersion = new Version(0, 0);
            var dslVersion = new Version(0, 0);

            if (!string.IsNullOrEmpty(modelRoot.ModelVersion))
                modelVersion = new Version(modelRoot.ModelVersion);

            if (toolVersion < modelVersion)
                throw new Exception("This model was created with newer version of the modeler. Please install version '" + modelVersion.ToString() + "' or higher.");

            try
            {
                var document = new XmlDocument();
                document.LoadXml(File.ReadAllText(modelFileName));
                var attr = document.DocumentElement.Attributes["dslVersion"];
                if (attr != null)
                {
                    dslVersion = new Version(attr.Value);
                }
            }
            catch
            {
            }

            //When saved the new version will be this tool version
            modelRoot.ModelVersion = LAST_MODEL_MODEL_COMPATIBLE;
            modelRoot.ModelFileName = modelFileName;

            modelRoot.IsDirty = false;

            var mainInfo = new FileInfo(modelFileName);
            nHydrate.Dsl.Custom.SQLFileManagement.LoadFromDisk(modelRoot, mainInfo.DirectoryName, modelRoot.Partition.Store, mainInfo.Name);
            modelRoot.IsDirty = false;

            #region Load Indexes

            //For now load the indexes into the REAL indexes collection
            //This should only happens the first time
            using (var transaction = modelRoot.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                LoadInitialIndexes(modelRoot);
                transaction.Commit();
            }

            #endregion

            return modelRoot;
        }

        public static void LoadInitialIndexes(nHydrateModel modelRoot)
        {
            //Setup primary keys
            foreach (var entity in modelRoot.Entities)
            {
                if (entity.Indexes.Count(x => x.IndexType == IndexTypeConstants.PrimaryKey) == 0 && entity.PrimaryKeyFields.Count > 0)
                {
                    var newIndex = new Index(entity.Partition);
                    newIndex.ParentEntityID = entity.Id;
                    newIndex.IndexType = IndexTypeConstants.PrimaryKey;
                    newIndex.Clustered = true;
                    entity.Indexes.Add(newIndex);

                    foreach (var field in entity.PrimaryKeyFields)
                    {
                        var newColumn = new IndexColumn(field.Partition);
                        newColumn.FieldID = field.Id;
                        newColumn.IsInternal = true;
                        newIndex.IndexColumns.Add(newColumn);
                    }
                }
            }

            var allIndexedField = modelRoot.Entities.SelectMany(x => x.Fields).Where(x => x.IsIndexed && !x.IsPrimaryKey);
            var allIndexes = modelRoot.Entities.SelectMany(x => x.Indexes);
            foreach (var field in allIndexedField)
            {
                var index = allIndexes.FirstOrDefault(x =>
                    x.IndexColumns.Count == 1 &&
                    x.IndexColumns.First().FieldID == field.Id &&
                    x.IndexColumns.First().Ascending);

                if (index == null)
                {
                    var newIndex = new Index(modelRoot.Partition);
                    newIndex.ParentEntityID = field.Entity.Id;
                    newIndex.IndexType = IndexTypeConstants.IsIndexed;
                    field.Entity.Indexes.Add(newIndex);

                    var newColumn = new IndexColumn(modelRoot.Partition);
                    newColumn.FieldID = field.Id;
                    newColumn.IsInternal = true;
                    newIndex.IndexColumns.Add(newColumn);
                }
            }
        }

    }

    partial class nHydrateSerializationHelperBase
    {
        private void OnPostLoadModel(DslModeling::SerializationResult serializationResult, DslModeling::Partition partition, string fileName, nHydrateModel modelRoot)
        {
        }

        private void OnPostLoadModelAndDiagram(DslModeling::SerializationResult serializationResult, DslModeling::Partition modelPartition, string modelFileName, DslModeling::Partition diagramPartition, string diagramFileName, nHydrateModel modelRoot, nHydrateDiagram diagram)
        {
        }
    }

}