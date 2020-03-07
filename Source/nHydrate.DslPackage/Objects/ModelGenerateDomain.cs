#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using nHydrate.Generator;
using nHydrate.Generator.Common.Util;
using System.Windows.Forms;
using nHydrate.Dsl;
using nHydrate.Generator.ModelUI;
using nHydrate.Generator.Common.GeneratorFramework;
using System.IO;
using System.Xml;
using nHydrate.Generator.Models;

namespace nHydrate.DslPackage.Objects
{
    internal class ModelGenerateDomain
    {
        private DateTime _startTime = DateTime.Now;
        private int _totalFileCount = 0;
        private int _processedFileCount = 0;
        private int FilesSkipped { get; set; }
        private int FilesSuccess { get; set; }
        private int FilesFailed { get; set; }
        public List<nHydrate.Generator.Common.EventArgs.ProjectItemGeneratedEventArgs> GeneratedFileList { get; private set; }
        public List<string> ErrorList { get; private set; }

        public void Generate(nHydrateModel model, Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram, Microsoft.VisualStudio.Modeling.Shell.ModelingDocData docData)
        {
            this.ErrorList = new List<string>();
            GeneratedFileList = new List<nHydrate.Generator.Common.EventArgs.ProjectItemGeneratedEventArgs>();
            try
            {
                #region Generation

                //Clean up delete tracking
                model.RemovedTables.Remove(x => model.Entities.Select(y => y.PascalName).Contains(x));
                model.RemovedViews.Remove(x => model.Views.Select(y => y.PascalName).Contains(x));

                var g = new nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper();
                g.ProjectItemGenerated += new nHydrate.Generator.Common.GeneratorFramework.ProjectItemGeneratedEventHandler(g_ProjectItemGenerated);

                var genList = BuildModelList(model, diagram, docData);

                var excludeList = new List<Type>();
                var generatorTypeList = g.GetProjectGenerators(genList.First());
                if (generatorTypeList.Count == 0)
                    return; //add message box

                if (ChooseGenerators(model, genList, generatorTypeList, excludeList, g))
                {
                    //Perform actual generation
                    if (genList.Count > 0)
                    {
                        PerformGeneration(model, genList, diagram.Store, docData, excludeList, g);
                    }

                    model.RemovedTables.Clear();
                    model.RemovedViews.Clear();
                }

                #endregion

                //Remove temp file
                try
                {
                    genList.ForEach(x => System.IO.File.Delete(x.FileName));
                }
                catch { }

#if DEBUG
                if (this.ErrorList.Count > 0)
                {
                    var F = new nHydrate.DslPackage.Forms.ErrorForm();
                    F.SetErrors(this.ErrorList);
                    F.ShowDialog();
                }
#endif

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region ChooseGenerators
        private bool ChooseGenerators(
            nHydrateModel model,
            List<nHydrateGeneratorProject> genList,
            List<Type> generatorTypeList,
            List<Type> excludeList,
            nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper genHelper)
        {
            if (!genList.Any())
            {
                MessageBox.Show("There are no generators defined", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var cacheFile = new nHydrate.Generator.Common.ModelCacheFile(genList.First());
            if (cacheFile.ModelerVersion > System.Reflection.Assembly.GetExecutingAssembly().GetName().Version)
            {
                if (MessageBox.Show($"This model schema was last generated with a newer modeler version ({cacheFile.ModelerVersion}). Your current version is {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}. Generating with an older modeler may cause many files to change unexpectedly. Do you wish to proceed with the generation?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return false;
            }

            //Initialize all the model configuration objects
            var modelRoot = genList.First().Model as ModelRoot;
            modelRoot.ModelConfigurations = new Dictionary<string, IModelConfiguration>();
            foreach (var genType in generatorTypeList)
            {
                var generator = genHelper.GetProjectGenerator(genType);
                modelRoot.ModelConfigurations.Add(generator.GetType().Name, generator.ModelConfiguration);
            }

            //Show generator list
            using (var F = new GenerateSettings(genList.First(), generatorTypeList, null))
            {
                if (F.ShowDialog() != DialogResult.OK) return false;
                excludeList.AddRange(F.ExcludeList);
            }

            return true;

        }
        #endregion

        #region PerformGeneration
        private void PerformGeneration(
            nHydrateModel model,
            List<nHydrateGeneratorProject> genList,
            Microsoft.VisualStudio.Modeling.Store store,
            Microsoft.VisualStudio.Modeling.Shell.ModelingDocData docData,
            List<Type> excludeList,
            nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper genHelper)
        {
            _totalFileCount = 0;
            _processedFileCount = 0;
            var pkey = string.Empty;
            try
            {
                var startTime = DateTime.Now;
                var isLicenseError = false;
                try
                {
                    //Get the last version we generated on this machine
                    //We will use this to determine if any other generations have been performed on other machines
                    var cacheFile = new nHydrate.Generator.Common.ModelCacheFile(genList.First());
                    var cachedGeneratedVersion = cacheFile.GeneratedVersion;

                    var generatedVersion = cachedGeneratedVersion + 1;

                    pkey = ProgressHelper.ProgressingStarted("Generating...", false, 240000); //Put a 4 minute timer on it
                    foreach (var generator in genList)
                    {
                        var modelRoot = (generator.Model as nHydrate.Generator.Models.ModelRoot);
                        modelRoot.GeneratedVersion = generatedVersion;
                        _totalFileCount += genHelper.GetFileCount(generator, excludeList);
                    }
                    System.Diagnostics.Debug.WriteLine($"File count: {_totalFileCount}");

                    //Save document
                    var isDirty = 0;
                    docData.IsDirty(out isDirty);
                    if (model.IsDirty || (isDirty != 0))
                        (docData as nHydrateDocData).Save(docData.FileName, 1, 0);

                    _startTime = DateTime.Now;
                    foreach (var item in genList)
                    {
                        genHelper.GenerateAll(item, excludeList);
                    }

                    var modelKey = (genList.FirstOrDefault()?.Model as nHydrate.Generator.Models.ModelRoot)?.Key;

                    //Save model statistics
                    var eCount = model.Entities.Count;
                    var fCount = model.Entities.SelectMany(x => x.FieldList).Count();
                    ModelStatsFile.Log(modelKey, eCount, fCount);

                    //Save local copy of last generated version
                    cacheFile.GeneratedVersion = generatedVersion;
                    cacheFile.ModelerVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    cacheFile.Save();

                    this.ErrorList = genHelper.GetErrorList().ToList();
                }
                catch (nHydrate.Generator.Common.Exceptions.LicenseException ex)
                {
                    ProgressHelper.ProgressingComplete(pkey);
                    MessageBox.Show("This product is not properly licensed.", "License Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isLicenseError = true;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    ProgressHelper.ProgressingComplete(pkey);

                }

                var endTime = DateTime.Now;
                var duration = endTime.Subtract(startTime);
                #region Show Generation Complete Dialog
                if (!isLicenseError)
                {
                    using (var F = new StatisticsForm())
                    {
                        var text = "The generation was successful.\r\n\r\n";
                        text += "Files generated: " + this.FilesSuccess + "\r\n";
                        text += "Files skipped: " + this.FilesSkipped + "\r\n";
                        text += "Files failed: " + this.FilesFailed + "\r\n";
                        text += "\r\n\r\n";
                        text += "Generation time: " + duration.Hours.ToString("00") + ":" +
                                duration.Minutes.ToString("00") + ":" +
                                duration.Seconds.ToString("00");
                        F.DisplayText = text;
                        F.GeneratedFileList = this.GeneratedFileList;
                        F.ShowDialog();
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                ProgressHelper.ProgressingComplete(pkey);
                GlobalHelper.ShowError(ex);
            }
        }
        #endregion

        #region BuildModelList

        private List<nHydrateGeneratorProject> BuildModelList(nHydrateModel model, Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram, Microsoft.VisualStudio.Modeling.Shell.ModelingDocData docData)
        {
            var genList = new List<nHydrateGeneratorProject>();

            var genProject = new nHydrateGeneratorProject();
            genList.Add(genProject);
            var root = CreatePOCOModel(model, diagram);
            root.SetKey(model.Id.ToString());
            root.GeneratorProject = genProject;
            genProject.RootController.Object = root;
            var fi = new System.IO.FileInfo(docData.FileName);
            genProject.FileName = docData.FileName + ".generating";
            var document = new System.Xml.XmlDocument();
            document.LoadXml("<modelRoot guid=\"" + model.Id + "\" type=\"nHydrate.Generator.nHydrateGeneratorProject\" assembly=\"nHydrate.Generator.dll\"><ModelRoot></ModelRoot></modelRoot>");
            ((nHydrate.Generator.Common.GeneratorFramework.IXMLable) root).XmlAppend(document.DocumentElement.ChildNodes[0]);
            System.IO.File.WriteAllText(genProject.FileName, document.ToIndentedString());

            ProcessRenamed(genProject.FileName + ".sql.lastgen", root);

            root.RemovedTables.AddRange(model.RemovedTables);

            //NOTE: This caused diff scripts to be generated EVERY time so removed for now
            //Remove associative tables since they cause issues if they exist
            //root.RemovedTables.AddRange(model.Entities.Where(x => x.IsAssociative && x.IsGenerated).Select(x => x.Name));

            root.RemovedViews.AddRange(model.RemovedViews);
            //Remove EnumOnly type-tables from the project
            root.RemovedTables.AddRange(model.Entities.Where(x => x.TypedEntity == TypedEntityConstants.EnumOnly).Select(x => x.Name));

            return genList;
        }

        private static void ProcessRenamed(string lastGenFile, ModelRoot root)
        {
            if (!File.Exists(lastGenFile)) return;

            var genProjectLast = new nHydrateGeneratorProject();
            var xDoc = new XmlDocument();
            xDoc.Load(lastGenFile);
            genProjectLast.XmlLoad(xDoc.DocumentElement);

            var oldRoot = (genProjectLast.RootController.Object as nHydrate.Generator.Models.ModelRoot);
            foreach (nHydrate.Generator.Models.Table t in root.Database.Tables)
            {
                //Find renamed tables
                {
                    var renamedItem = oldRoot.Database.Tables.FirstOrDefault(x => x.Key == t.Key && x.PascalName.ToLower() != t.PascalName.ToLower());
                    if (renamedItem != null)
                        root.RemovedTables.Add(renamedItem.Name);
                }

                //Find renamed views
                {
                    var renamedItem = oldRoot.Database.CustomViews.FirstOrDefault(x => x.Key == t.Key && x.PascalName.ToLower() != t.PascalName.ToLower());
                    if (renamedItem != null)
                        root.RemovedViews.Add(renamedItem.Name);
                }

                //Find tables that WERE generated last time but NOT generated this time, remove the tables
                {
                    var item1 = oldRoot.Database.Tables.FirstOrDefault(x => x.Key == t.Key && x.PascalName.ToLower() == t.PascalName.ToLower());
                    var item2 = root.Database.Tables.FirstOrDefault(x => x.Key == t.Key && x.PascalName.ToLower() != t.PascalName.ToLower());
                    if (item1 != null && item2 != null)
                        root.RemovedTables.Add(item2.Name);
                }

            }

        }

        #endregion

        #region CreatePOCOModel
        private nHydrate.Generator.Models.ModelRoot CreatePOCOModel(nHydrateModel model, Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram)
        {
            try
            {
                var root = new nHydrate.Generator.Models.ModelRoot(null);
                root.EnableCustomChangeEvents = model.EmitChangeScripts;
                root.CompanyName = model.CompanyName;
                root.EmitSafetyScripts = model.EmitSafetyScripts;
                root.DefaultNamespace = model.DefaultNamespace;
                root.ProjectName = model.ProjectName;
                root.SupportLegacySearchObject = false;
                root.UseUTCTime = model.UseUTCTime;
                root.Version = model.Version;
                root.Database.ResetKey(model.Id.ToString());
                root.OutputTarget = string.Empty; //model.OutputTarget;
                //These have the same mapping values flags so we need convert to int and then convert to the other enumeration
                root.TenantColumnName = model.TenantColumnName;
                root.TenantPrefix = model.TenantPrefix;
                root.Database.CreatedByColumnName = model.CreatedByColumnName;
                root.Database.CreatedDateColumnName = model.CreatedDateColumnName;
                root.Database.ModifiedByColumnName = model.ModifiedByColumnName;
                root.Database.ModifiedDateColumnName = model.ModifiedDateColumnName;
                root.Database.TimestampColumnName = model.TimestampColumnName;
                root.Database.GrantExecUser = model.GrantUser;

                #region Load the entities
                foreach (var entity in model.Entities)
                {
                    #region Table Info
                    var newTable = root.Database.Tables.Add();
                    newTable.ResetKey(entity.Id.ToString());
                    newTable.ResetId(HashString(newTable.Key));
                    newTable.AllowCreateAudit = entity.AllowCreateAudit;
                    newTable.AllowModifiedAudit = entity.AllowModifyAudit;
                    newTable.AllowTimestamp = entity.AllowTimestamp;
                    newTable.AssociativeTable = entity.IsAssociative;
                    newTable.CodeFacade = entity.CodeFacade;
                    newTable.DBSchema = entity.Schema;
                    newTable.Description = entity.Summary;
                    newTable.Immutable = entity.Immutable;
                    newTable.TypedTable = (nHydrate.Generator.Models.TypedTableConstants)Enum.Parse(typeof(nHydrate.Generator.Models.TypedTableConstants), entity.TypedEntity.ToString(), true);
                    newTable.Name = entity.Name;
                    newTable.GeneratesDoubleDerived = entity.GeneratesDoubleDerived;
                    newTable.IsTenant = entity.IsTenant;
                    #endregion

                    #region Load the fields for this entity
                    var fieldList = entity.Fields.ToList();
                    foreach (var field in fieldList.OrderBy(x => x.SortOrder))
                    {
                        var newColumn = root.Database.Columns.Add();
                        newColumn.ResetKey(field.Id.ToString());
                        newColumn.ResetId(HashString(newColumn.Key));
                        newColumn.AllowNull = field.Nullable;
                        newColumn.CodeFacade = field.CodeFacade;
                        newColumn.ComputedColumn = field.IsCalculated;
                        newColumn.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.DataType.ToString());
                        newColumn.Default = field.Default;
                        newColumn.DefaultIsFunc = field.DefaultIsFunc;
                        newColumn.Description = field.Summary;
                        newColumn.Formula = field.Formula;
                        newColumn.Identity = (nHydrate.Generator.Models.IdentityTypeConstants)Enum.Parse(typeof(nHydrate.Generator.Models.IdentityTypeConstants), field.Identity.ToString());
                        newColumn.IsIndexed = field.IsIndexed;
                        newColumn.IsReadOnly = field.IsReadOnly;
                        newColumn.IsUnique = field.IsUnique;
                        newColumn.Length = field.Length;
                        newColumn.Name = field.Name;
                        newColumn.ParentTableRef = newTable.CreateRef(newTable.Key);
                        newColumn.PrimaryKey = field.IsPrimaryKey;
                        newColumn.Scale = field.Scale;
                        newColumn.SortOrder = field.SortOrder;
                        newColumn.Obsolete = field.Obsolete;
                        newTable.Columns.Add(newColumn.CreateRef(newColumn.Key));
                    }
                    #endregion

                    #region Indexes

                    var indexList = entity.Indexes.ToList();
                    foreach (var index in indexList)
                    {
                        var indexColumns = index.IndexColumns.Where(x => x.GetField() != null).ToList();
                        if (indexColumns.Count > 0)
                        {
                            var newIndex = new nHydrate.Generator.Models.TableIndex(newTable.Root)
                            {
                                Description = index.Summary,
                                IsUnique = index.IsUnique,
                                Clustered = index.Clustered,
                                PrimaryKey = (index.IndexType == IndexTypeConstants.PrimaryKey)
                            };
                            newTable.TableIndexList.Add(newIndex);
                            newIndex.ResetKey(index.Id.ToString());
                            newIndex.ResetId(HashString(newIndex.Key));
                            newIndex.ImportedName = index.ImportedName;

                            //Add index columns
                            foreach (var ic in indexColumns.OrderBy(x => x.SortOrder).ThenBy(x => x.GetField().Name))
                            {
                                var field = ic.GetField();
                                var newColumn = new nHydrate.Generator.Models.TableIndexColumn(newTable.Root) { Ascending = ic.Ascending, FieldID = field.Id };
                                newIndex.IndexColumnList.Add(newColumn);
                            }
                        }
                    }

                    #endregion

                    #region Static Data
                    //Determine how many rows there are
                    var orderKeyList = entity.StaticDatum.Select(x => x.OrderKey).Distinct().ToList();
                    var rowCount = orderKeyList.Count;

                    //Create a OLD static data row for each one
                    for (var ii = 0; ii < rowCount; ii++)
                    {
                        //For each row create N cells one for each column
                        var rowEntry = new nHydrate.Generator.Models.RowEntry(newTable.Root);
                        var staticDataFieldList = fieldList.Where(x => !x.DataType.IsBinaryType() && x.DataType != DataTypeConstants.Timestamp).ToList();
                        for (var jj = 0; jj < staticDataFieldList.Count; jj++)
                        {
                            var cellEntry = new nHydrate.Generator.Models.CellEntry(newTable.Root);
                            var column = newTable.GetColumns().ToList()[jj];
                            cellEntry.ColumnRef = column.CreateRef(column.Key);

                            var currentColumn = fieldList.FirstOrDefault(x => x.Id == new Guid(column.Key));
                            if (currentColumn != null)
                            {
                                var dataum = entity.StaticDatum.FirstOrDefault(x =>
                                    x.ColumnKey == currentColumn.Id &&
                                    x.OrderKey == orderKeyList[ii]);

                                if (dataum != null)
                                {
                                    cellEntry.Value = dataum.Value;
                                    cellEntry.ResetKey(dataum.Id.ToString());
                                }

                                //Add the cell to the row
                                rowEntry.CellEntries.Add(cellEntry);
                            }
                        }
                        newTable.StaticData.Add(rowEntry);
                    }
                    #endregion
                }

                #endregion

                #region Relations
                {
                    var relationConnectors = diagram.NestedChildShapes.Where(x => x is EntityAssociationConnector).Cast<EntityAssociationConnector>().ToList();
                    foreach (var shape in relationConnectors)
                    {
                        if (shape is EntityAssociationConnector)
                        {
                            var connector = shape as EntityAssociationConnector;
                            var parent = connector.FromShape.ModelElement as Entity;
                            var child = connector.ToShape.ModelElement as Entity;

                            var relation = connector.ModelElement as EntityHasEntities;
                            var fieldList = model.RelationFields.Where(x => x.RelationID == relation.Id);

                            var parentTable = root.Database.Tables.FirstOrDefault(x => x.Name == parent.Name);
                            var childTable = root.Database.Tables.FirstOrDefault(x => x.Name == child.Name);

                            //If we found both parent and child tables...
                            if (parentTable != null && childTable != null && !childTable.IsInheritedFrom(parentTable))
                            {
                                var isValidRelation = true;
                                if (isValidRelation)
                                {
                                    var newRelation = root.Database.Relations.Add();
                                    newRelation.ResetKey((connector.ModelElement as nHydrate.Dsl.EntityHasEntities).InternalId.ToString());
                                    newRelation.ResetId(HashString(newRelation.Key));
                                    newRelation.ParentTableRef = parentTable.CreateRef(parentTable.Key);
                                    newRelation.ChildTableRef = childTable.CreateRef(childTable.Key);
                                    newRelation.RoleName = ((EntityHasEntities)connector.ModelElement).RoleName;
                                    switch(relation.DeleteAction)
                                    {
                                        case DeleteActionConstants.Cascade:
                                            newRelation.DeleteAction = Relation.DeleteActionConstants.Cascade;
                                            break;
                                        case DeleteActionConstants.NoAction:
                                            newRelation.DeleteAction = Relation.DeleteActionConstants.NoAction;
                                            break;
                                        case DeleteActionConstants.SetNull:
                                            newRelation.DeleteAction = Relation.DeleteActionConstants.SetNull;
                                            break;
                                    }

                                    newRelation.Enforce = relation.IsEnforced;

                                    //Create the column links
                                    foreach (var columnSet in fieldList)
                                    {
                                        var field1 = parent.Fields.FirstOrDefault(x => x.Id == columnSet.SourceFieldId);
                                        var field2 = child.Fields.FirstOrDefault(x => x.Id == columnSet.TargetFieldId);

                                        var column1 = parentTable.GetColumnsFullHierarchy().FirstOrDefault(x => x.Name == field1.Name);
                                        var column2 = childTable.GetColumnsFullHierarchy().FirstOrDefault(x => x.Name == field2.Name);

                                        newRelation.ColumnRelationships.Add(new nHydrate.Generator.Models.ColumnRelationship(root)
                                        {
                                            ParentColumnRef = column1.CreateRef(column1.Key),
                                            ChildColumnRef = column2.CreateRef(column2.Key),
                                        }
                                        );
                                    }

                                    //Actually add the relation to the collection
                                    if (newRelation.ColumnRelationships.Count > 0)
                                        parentTable.Relationships.Add(newRelation.CreateRef(newRelation.Key));
                                }
                            }
                        }
                    }

                } //inner block

                #endregion

                #region Views
                foreach (var view in model.Views)
                {
                    var newView = root.Database.CustomViews.Add();
                    newView.ResetKey(view.Id.ToString());
                    newView.ResetId(HashString(newView.Key));
                    newView.CodeFacade = view.CodeFacade;
                    newView.DBSchema = view.Schema;
                    newView.Description = view.Summary;
                    newView.Name = view.Name;
                    newView.SQL = view.SQL;
                    newView.GeneratesDoubleDerived = view.GeneratesDoubleDerived;

                    foreach (var field in view.Fields)
                    {
                        var newField = root.Database.CustomViewColumns.Add();
                        newField.ResetKey(field.Id.ToString());
                        newField.ResetId(HashString(newField.Key));
                        newField.AllowNull = field.Nullable;
                        newField.CodeFacade = field.CodeFacade;
                        newField.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.DataType.ToString());
                        newField.Default = field.Default;
                        newField.Description = field.Summary;
                        newField.IsPrimaryKey = field.IsPrimaryKey;
                        newField.Length = field.Length;
                        newField.Name = field.Name;
                        newField.Scale = field.Scale;
                        newView.Columns.Add(newField.CreateRef(newField.Key));
                        newField.ParentViewRef = newView.CreateRef(newView.Key);
                    }

                }
                #endregion

                return root;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Validate
        public static bool Validate(nHydrateDocData docData, Microsoft.VisualStudio.Modeling.Store store, nHydrateModel model)
        {
            var key = ProgressHelper.ProgressingStarted("Verifying Model...");
            try
            {
                var validationController = docData.ValidationController;
                validationController.Validate(model.Entities, Microsoft.VisualStudio.Modeling.Validation.ValidationCategories.Custom);
                return validationController.Validate(store, Microsoft.VisualStudio.Modeling.Validation.ValidationCategories.Custom |
                                                                                        Microsoft.VisualStudio.Modeling.Validation.ValidationCategories.Load |
                                                                                        Microsoft.VisualStudio.Modeling.Validation.ValidationCategories.Menu |
                                                                                        Microsoft.VisualStudio.Modeling.Validation.ValidationCategories.Open |
                                                                                        Microsoft.VisualStudio.Modeling.Validation.ValidationCategories.Save);
            }
            catch (Exception ex)
            {
                ProgressHelper.ProgressingComplete(key);
                throw;
            }
            finally
            {
                ProgressHelper.ProgressingComplete(key);
            }
        }
        #endregion

        #region ProjectItemGenerated
        private void g_ProjectItemGenerated(object sender, nHydrate.Generator.Common.EventArgs.ProjectItemGeneratedEventArgs e)
        {
            try
            {
                if (e.FileState == EnvDTEHelper.FileStateConstants.Skipped)
                    this.FilesSkipped++;
                if (e.FileState == EnvDTEHelper.FileStateConstants.Success)
                    this.FilesSuccess++;
                if (e.FileState == EnvDTEHelper.FileStateConstants.Failed)
                    this.FilesFailed++;

                this.GeneratedFileList.Add(e);

                _processedFileCount++;
                var progress = -1;
                var mainText = string.Empty;
                var timeDisplay = string.Empty;
                if (_totalFileCount > 0)
                {
                    progress = (_processedFileCount * 100) / _totalFileCount;
                    if (progress > 100) progress = 100;

                    var totalDisplay = Math.Max(_processedFileCount, _totalFileCount);
                    mainText = "Generating " + _processedFileCount.ToString("###,###,###,##0") + " of " + totalDisplay.ToString("###,###,###,##0") + "...";
                    if (progress > 0)
                    {
                        var elapsedTime = DateTime.Now.Subtract(_startTime).TotalSeconds;
                        var totalTime = elapsedTime * (1 / (progress / 100.0));
                        timeDisplay = "(" + Extensions.ToElapsedTimeString(elapsedTime) + " of " + Extensions.ToElapsedTimeString(totalTime) + ")";
                    }
                }

                ProgressHelper.UpdateSubText(mainText, e.FullName, progress, timeDisplay);
            }
            catch (Exception ex)
            {
                //TODO
            }
        }
        #endregion

        private int HashString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            uint hash = 0;
            foreach (var b in System.Text.Encoding.Unicode.GetBytes(s))
            {
                hash += b;
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }
            // final avalanche
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            return (int)(hash % int.MaxValue);
        }

    }
}