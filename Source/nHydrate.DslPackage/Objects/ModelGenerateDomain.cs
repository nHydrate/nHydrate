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
using nHydrate.Dsl.Objects;
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

            //Verify registered version
            if (!nHydrate.Generator.Common.GeneratorFramework.AddinAppData.Instance.PremiumValidated)
            {
                //REGISTERED FEATURES:
                //Entity > 50
                //Use Modules
                //Use Functions
                //if (model.Modules.Count > 0 ||
                //  model.Entities.Count(x => x.IsGenerated) > 50 ||
                //  model.Functions.Count > 0)
                //{
                //  if (MessageBox.Show("You must register this product to use the following functionality:\n\n1. Use more than 50 Entities\n2. Use Modules\n3.Use Functions\n\nDo you wish to go to the nHydrate.org website and register this software?", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.Yes)
                //  {
                //    System.Diagnostics.Process.Start("http://www.nhydrate.org");
                //  }
                //  return;
                //}
            }

            try
            {
                #region Generation

                //Clean up delete tracking
                model.RemovedTables.Remove(x => model.Entities.Select(y => y.PascalName).Contains(x));
                model.RemovedViews.Remove(x => model.Views.Select(y => y.PascalName).Contains(x));
                model.RemovedStoredProcedures.Remove(x => model.StoredProcedures.Select(y => y.PascalName).Contains(x));
                model.RemovedFunctions.Remove(x => model.Functions.Select(y => y.PascalName).Contains(x));

                var g = new nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper();
                g.ProjectItemGenerated += new nHydrate.Generator.Common.GeneratorFramework.ProjectItemGeneratedEventHandler(g_ProjectItemGenerated);

                var genList = BuildModelList(model, diagram, docData);

                var excludeList = new List<Type>();
                var generatorTypeList = g.GetProjectGenerators(genList.First());
                if (generatorTypeList.Count == 0)
                    return; //add message box

                var generateModuleList = new List<string>();

                if (ChooseGenerators(model, genList, generatorTypeList, excludeList, g, generateModuleList))
                {
                    //Perform actual generation
                    if (genList.Count > 0)
                    {
                        PerformGeneration(model, genList, diagram.Store, docData, excludeList, g, generateModuleList);
                    }

                    model.RemovedTables.Clear();
                    model.RemovedViews.Clear();
                    model.RemovedStoredProcedures.Clear();
                    model.RemovedFunctions.Clear();
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
            nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper genHelper,
            List<string> generateModuleList)
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

            //Initalize all the model configuration objects
            var modelRoot = genList.First().Model as ModelRoot;
            modelRoot.ModelConfigurations = new Dictionary<string, IModelConfiguration>();
            foreach (var genType in generatorTypeList)
            {
                var generator = genHelper.GetProjectGenerator(genType);
                modelRoot.ModelConfigurations.Add(generator.GetType().Name, generator.ModelConfiguration);
            }

            //Show generator list
            var allModules = model.Modules.Select(x => x.Name).ToList();
            if (!model.UseModules) allModules.Clear();
            using (var F = new GenerateSettings(genList.First(), generatorTypeList, null, allModules))
            {
                if (F.ShowDialog() != DialogResult.OK) return false;
                excludeList.AddRange(F.ExcludeList);
                generateModuleList.AddRange(F.SelectedModules);
            }

            //If we are using modules then filter on the selected ones
            if (model.UseModules)
            {
                genList = genList.Where(x => generateModuleList.Contains((x.Model as nHydrate.Generator.Models.ModelRoot).ModuleName)).ToList();
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
            nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper genHelper,
            List<string> generateModuleList)
        {
            _totalFileCount = 0;
            _processedFileCount = 0;
            var pkey = string.Empty;
            try
            {
                var startTime = DateTime.Now;
                var isLicenseError = false;
                var fullModuleGenList = genList.Select(x => new ModuleVersionInfo() { ModuleName = (x.Model as nHydrate.Generator.Models.ModelRoot).ModuleName }).ToList();

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
                        if (model.UseModules)
                        {
                            if (generateModuleList.Contains((item.Model as nHydrate.Generator.Models.ModelRoot).ModuleName))
                                genHelper.GenerateAll(item, excludeList);
                        }
                        else
                        {
                            genHelper.GenerateAll(item, excludeList);
                        }
                    }

                    if (model.Refactorizations.Count > 0)
                    {
                        using (var transaction = store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                        {
                            model.Refactorizations.Clear();
                            model.UseUTCTime = !model.UseUTCTime; //Trigger a model change
                            model.UseUTCTime = !model.UseUTCTime; //Keep this line too
                            transaction.Commit();

                            //Save document
                            (docData as nHydrateDocData).Save(docData.FileName, 1, 0);

                        }
                    }

                    var modelKey = (genList.FirstOrDefault()?.Model as nHydrate.Generator.Models.ModelRoot)?.Key;

                    //Save model statistics
                    var eCount = model.Entities.Count();
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
                    MessageBox.Show("This product is not properly licensed", "License Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            if (model.UseModules)
            {
                foreach (var module in model.Modules)
                {
                    var genProject = new nHydrateGeneratorProject();
                    genList.Add(genProject);
                    var root = CreatePOCOModel(model, diagram, module);
                    root.SetKey(module.Id.ToString());
                    root.GeneratorProject = genProject;
                    genProject.RootController.Object = root;
                    var fi = new System.IO.FileInfo(docData.FileName);
                    genProject.FileName = docData.FileName + "." + module.Name + ".generating";
                    var document = new System.Xml.XmlDocument();
                    document.LoadXml("<modelRoot guid=\"" + module.Id + "\" type=\"nHydrate.Generator.nHydrateGeneratorProject\" assembly=\"nHydrate.Generator.dll\"><ModelRoot></ModelRoot></modelRoot>");
                    ((nHydrate.Generator.Common.GeneratorFramework.IXMLable)root).XmlAppend(document.DocumentElement.ChildNodes[0]);
                    System.IO.File.WriteAllText(genProject.FileName, document.ToIndentedString());

                    ProcessRenamed(genProject.FileName + ".sql.lastgen", root);

                    root.RemovedTables.AddRange(model.RemovedTables);
                    root.RemovedViews.AddRange(model.RemovedViews);
                    root.RemovedStoredProcedures.AddRange(model.RemovedStoredProcedures);
                    root.RemovedFunctions.AddRange(model.RemovedFunctions);

                    //Remove non-generated items from the project
                    root.RemovedTables.AddRange(model.Entities.Where(x => !x.IsGenerated).Select(x => x.Name));
                    root.RemovedTables.AddRange(model.Views.Where(x => !x.IsGenerated).Select(x => x.Name));
                    root.RemovedTables.AddRange(model.StoredProcedures.Where(x => !x.IsGenerated).Select(x => x.Name));
                    root.RemovedTables.AddRange(model.Functions.Where(x => !x.IsGenerated).Select(x => x.Name));
                    //Remove EnumOnly type-tables from the project
                    root.RemovedTables.AddRange(model.Entities.Where(x => x.TypedEntity == TypedEntityConstants.EnumOnly && x.IsGenerated).Select(x => x.Name));
                }
            }
            else
            {
                var genProject = new nHydrateGeneratorProject();
                genList.Add(genProject);
                var root = CreatePOCOModel(model, diagram, null);
                root.SetKey(model.Id.ToString());
                root.GeneratorProject = genProject;
                genProject.RootController.Object = root;
                var fi = new System.IO.FileInfo(docData.FileName);
                genProject.FileName = docData.FileName + ".generating";
                var document = new System.Xml.XmlDocument();
                document.LoadXml("<modelRoot guid=\"" + model.Id + "\" type=\"nHydrate.Generator.nHydrateGeneratorProject\" assembly=\"nHydrate.Generator.dll\"><ModelRoot></ModelRoot></modelRoot>");
                ((nHydrate.Generator.Common.GeneratorFramework.IXMLable)root).XmlAppend(document.DocumentElement.ChildNodes[0]);
                System.IO.File.WriteAllText(genProject.FileName, document.ToIndentedString());

                ProcessRenamed(genProject.FileName + ".sql.lastgen", root);

                root.RemovedTables.AddRange(model.RemovedTables);

                //NOTE: This caused diff scripts to be generated EVERY time so removed for now
                //Remove associative tables since they cause issues if they exist
                //root.RemovedTables.AddRange(model.Entities.Where(x => x.IsAssociative && x.IsGenerated).Select(x => x.Name));

                root.RemovedViews.AddRange(model.RemovedViews);
                root.RemovedStoredProcedures.AddRange(model.RemovedStoredProcedures);
                root.RemovedFunctions.AddRange(model.RemovedFunctions);
                root.RemovedTables.AddRange(model.Views.Where(x => !x.IsGenerated).Select(x => x.Name));
                root.RemovedTables.AddRange(model.StoredProcedures.Where(x => !x.IsGenerated).Select(x => x.Name));
                root.RemovedTables.AddRange(model.Functions.Where(x => !x.IsGenerated).Select(x => x.Name));
                //Remove EnumOnly type-tables from the project
                root.RemovedTables.AddRange(model.Entities.Where(x => x.TypedEntity == TypedEntityConstants.EnumOnly && x.IsGenerated).Select(x => x.Name));

            }

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

                //Find renamed stored procedures
                {
                    var renamedItem = oldRoot.Database.CustomStoredProcedures.FirstOrDefault(x => x.Key == t.Key && x.PascalName.ToLower() != t.PascalName.ToLower());
                    if (renamedItem != null)
                        root.RemovedStoredProcedures.Add(renamedItem.Name);
                }

                //Find renamed functions
                {
                    var renamedItem = oldRoot.Database.Functions.FirstOrDefault(x => x.Key == t.Key && x.PascalName.ToLower() != t.PascalName.ToLower());
                    if (renamedItem != null)
                        root.RemovedFunctions.Add(renamedItem.Name);
                }

                //Find tables that WERE generated last time but NOT generated this time, remove the tables
                {
                    var item1 = oldRoot.Database.Tables.FirstOrDefault(x => x.Key == t.Key && x.PascalName.ToLower() == t.PascalName.ToLower() && x.Generated);
                    var item2 = root.Database.Tables.FirstOrDefault(x => x.Key == t.Key && x.PascalName.ToLower() == t.PascalName.ToLower() && !x.Generated);
                    if (item1 != null && item2 != null)
                        root.RemovedTables.Add(item2.Name);
                }

            }

        }

        #endregion

        #region CreatePOCOModel
        private nHydrate.Generator.Models.ModelRoot CreatePOCOModel(nHydrateModel model, Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram, Module ownerModule)
        {
            try
            {
                var root = new nHydrate.Generator.Models.ModelRoot(null);
                root.TransformNames = model.TransformNames;
                root.EnableCustomChangeEvents = model.EmitChangeScripts;
                root.CompanyName = model.CompanyName;
                root.EmitSafetyScripts = model.EmitSafetyScripts;
                root.Copyright = model.Copyright;
                root.DefaultNamespace = model.DefaultNamespace;
                root.ProjectName = model.ProjectName;
                root.SQLServerType = (SQLServerTypeConstants)Enum.Parse(typeof(SQLServerTypeConstants), model.SQLServerType.ToString());
                root.SupportLegacySearchObject = false;
                root.UseUTCTime = model.UseUTCTime;
                root.Version = model.Version;
                root.Database.ResetKey(model.Id.ToString());
                root.OutputTarget = string.Empty; //model.OutputTarget;
                //These have the same mapping values flags so we need convert to int and then convert to the other enumeration
                root.TenantColumnName = model.TenantColumnName;
                root.TenantPrefix = model.TenantPrefix;

                foreach (var md in model.ModelMetadata)
                {
                    var newmd = new Generator.Models.MetadataItem();
                    newmd.Key = md.Key;
                    newmd.Value = md.Value;
                    root.MetaData.Add(newmd);
                }

                #region Set Refactorizations
                foreach (var r in model.Refactorizations)
                {
                    if (r is nHydrate.Dsl.Objects.RefactorTableSplit)
                    {
                        var newR = new nHydrate.Generator.Common.GeneratorFramework.RefactorTableSplit();
                        newR.EntityKey1 = (r as nHydrate.Dsl.Objects.RefactorTableSplit).EntityKey1;
                        newR.EntityKey2 = (r as nHydrate.Dsl.Objects.RefactorTableSplit).EntityKey2;
                        var flist = (r as nHydrate.Dsl.Objects.RefactorTableSplit).ReMappedFieldIDList;
                        foreach (var k in flist.Keys)
                            newR.ReMappedFieldIDList.Add(k, flist[k]);
                        root.Refactorizations.Add(newR);
                    }
                    else if (r is nHydrate.Dsl.Objects.RefactorTableCombine)
                    {
                        var newR = new nHydrate.Generator.Common.GeneratorFramework.RefactorTableCombine();
                        newR.EntityKey1 = (r as nHydrate.Dsl.Objects.RefactorTableCombine).EntityKey1;
                        newR.EntityKey2 = (r as nHydrate.Dsl.Objects.RefactorTableCombine).EntityKey2;
                        var flist = (r as nHydrate.Dsl.Objects.RefactorTableCombine).ReMappedFieldIDList;
                        foreach (var k in flist.Keys)
                            newR.ReMappedFieldIDList.Add(k, flist[k]);
                        root.Refactorizations.Add(newR);
                    }
                }
                #endregion

                if (ownerModule != null)
                    root.ModuleName = ownerModule.Name;

                root.Database.CreatedByColumnName = model.CreatedByColumnName;
                root.Database.CreatedDateColumnName = model.CreatedDateColumnName;
                root.Database.ModifiedByColumnName = model.ModifiedByColumnName;
                root.Database.ModifiedDateColumnName = model.ModifiedDateColumnName;
                root.Database.TimestampColumnName = model.TimestampColumnName;
                root.Database.GrantExecUser = model.GrantUser;
                root.Database.Collate = model.Collate;

                root.Database.PrecedenceOrderList = PrecedenceUtil.GetAllPrecedenceItems(model)
                    .Select(x => x.ID)
                    .ToList();

                #region Load the entities
                foreach (var entity in model.Entities.Where(x => x.IsGenerated).Where(x => ownerModule == null || x.Modules.Contains(ownerModule)))
                {
                    #region Table Info
                    var newTable = root.Database.Tables.Add();
                    newTable.ResetKey(entity.Id.ToString());
                    newTable.ResetId(HashString(newTable.Key));
                    newTable.AllowAuditTracking = entity.AllowAuditTracking;
                    newTable.AllowCreateAudit = entity.AllowCreateAudit;
                    newTable.AllowModifiedAudit = entity.AllowModifyAudit;
                    newTable.AllowTimestamp = entity.AllowTimestamp;
                    newTable.AssociativeTable = entity.IsAssociative;
                    newTable.CodeFacade = entity.CodeFacade;
                    newTable.DBSchema = entity.Schema;
                    newTable.Description = entity.Summary;
                    newTable.EnforcePrimaryKey = entity.EnforcePrimaryKey;
                    newTable.Generated = entity.IsGenerated;
                    newTable.Immutable = entity.Immutable;
                    newTable.TypedTable = (nHydrate.Generator.Models.TypedTableConstants)Enum.Parse(typeof(nHydrate.Generator.Models.TypedTableConstants), entity.TypedEntity.ToString(), true);
                    newTable.Name = entity.Name;
                    newTable.GeneratesDoubleDerived = entity.GeneratesDoubleDerived;
                    newTable.IsTenant = entity.IsTenant;

                    //Add metadata
                    foreach (var md in entity.EntityMetadata)
                    {
                        newTable.MetaData.Add(new nHydrate.Generator.Models.MetadataItem() { Key = md.Key, Value = md.Value });
                    }

                    if (entity.SecurityFunction != null)
                    {
                        newTable.Security.ResetKey(entity.SecurityFunction.Id.ToString());
                        newTable.Security.SQL = entity.SecurityFunction.SQL;

                        //Just in case these are ordered get all sort-ordered parameters first then take on all unordred alphabetized parmameters
                        var orderedParameters = entity.SecurityFunction.SecurityFunctionParameters.Where(x => x.SortOrder > 0).OrderBy(x => x.SortOrder).ToList();
                        orderedParameters.AddRange(entity.SecurityFunction.SecurityFunctionParameters.Where(x => x.SortOrder == 0).OrderBy(x => x.Name).ToList());
                        foreach (var parameter in orderedParameters)
                        {
                            var newParameter = root.Database.FunctionParameters.Add();
                            newParameter.ResetKey(parameter.Id.ToString());
                            newParameter.ResetId(HashString(newParameter.Key));
                            newParameter.ParentTableRef = newTable.CreateRef(newTable.Key);
                            newParameter.AllowNull = parameter.Nullable;
                            newParameter.CodeFacade = parameter.CodeFacade;
                            newParameter.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), parameter.DataType.ToString());
                            newParameter.Default = parameter.Default;
                            newParameter.Description = parameter.Summary;
                            newParameter.Generated = parameter.IsGenerated;
                            newParameter.Length = parameter.Length;
                            newParameter.Name = parameter.Name;
                            newParameter.Scale = parameter.Scale;
                            newParameter.SortOrder = parameter.SortOrder;

                            var r = newParameter.CreateRef(newParameter.Key);
                            r.RefType = nHydrate.Generator.Models.ReferenceType.FunctionParameter;
                            newTable.Security.Parameters.Add(r);
                        }

                    }

                    #endregion

                    #region Load the fields for this entity
                    var fieldList = entity.Fields.Where(x => x.IsGenerated).Where(x => ownerModule == null || x.Modules.Contains(ownerModule)).ToList();
                    foreach (var field in fieldList.OrderBy(x => x.SortOrder))
                    {
                        var newColumn = root.Database.Columns.Add();
                        newColumn.ResetKey(field.Id.ToString());
                        newColumn.ResetId(HashString(newColumn.Key));
                        newColumn.AllowNull = field.Nullable;
                        newColumn.Collate = field.Collate;
                        newColumn.CodeFacade = field.CodeFacade;
                        newColumn.ComputedColumn = field.IsCalculated;
                        newColumn.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.DataType.ToString());
                        newColumn.Default = field.Default;
                        newColumn.DefaultIsFunc = field.DefaultIsFunc;
                        newColumn.Description = field.Summary;
                        newColumn.Formula = field.Formula;
                        newColumn.FriendlyName = field.FriendlyName;
                        newColumn.Generated = field.IsGenerated;
                        newColumn.Identity = (nHydrate.Generator.Models.IdentityTypeConstants)Enum.Parse(typeof(nHydrate.Generator.Models.IdentityTypeConstants), field.Identity.ToString());
                        newColumn.IsIndexed = field.IsIndexed;
                        newColumn.IsReadOnly = field.IsReadOnly;
                        newColumn.IsUnique = field.IsUnique;
                        newColumn.Length = field.Length;
                        newColumn.Max = field.Max;
                        newColumn.Min = field.Min;
                        newColumn.Name = field.Name;
                        newColumn.ParentTableRef = newTable.CreateRef(newTable.Key);
                        newColumn.PrimaryKey = field.IsPrimaryKey;
                        newColumn.Scale = field.Scale;
                        newColumn.ValidationExpression = field.ValidationExpression;
                        newColumn.IsBrowsable = field.IsBrowsable;
                        newColumn.Category = field.Category;
                        newColumn.SortOrder = field.SortOrder;
                        newColumn.Mask = field.DataFormatString;
                        newColumn.UIDataType = (System.ComponentModel.DataAnnotations.DataType)Enum.Parse(typeof(System.ComponentModel.DataAnnotations.DataType), field.UIDataType.ToString(), true);
                        newColumn.Obsolete = field.Obsolete;
                        newTable.Columns.Add(newColumn.CreateRef(newColumn.Key));

                        //Add metadata
                        foreach (var md in field.FieldMetadata)
                        {
                            newColumn.MetaData.Add(new nHydrate.Generator.Models.MetadataItem() { Key = md.Key, Value = md.Value });
                        }

                    }
                    #endregion

                    #region Indexes

                    //Find all index for this entity in module
                    List<Index> indexList = null;
                    if (ownerModule != null)
                    {
                        indexList = new List<Index>();
                        indexList.AddRange(entity.Indexes.Where(x => x.IndexType == IndexTypeConstants.PrimaryKey).ToList());
                        foreach (var im in model.IndexModules.Where(x => x.ModuleId == ownerModule.Id))
                        {
                            indexList.AddRange(entity.Indexes.Where(x => x.IndexType != IndexTypeConstants.PrimaryKey && x.Id == im.IndexID));
                        }
                    }
                    else
                    {
                        indexList = entity.Indexes.ToList();
                    }

                    foreach (var index in indexList)
                    {
                        var indexColumns = index.IndexColumns.Where(x => x.GetField() != null && (ownerModule == null || x.GetField().Modules.Contains(ownerModule))).ToList();
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
                    //if (entity.TypedEntity != TypedEntityConstants.None)
                    //{
                    //Determine how many rows there are
                    var orderKeyList = entity.StaticDatum.Select(x => x.OrderKey).Distinct().ToList();
                    var rowCount = orderKeyList.Count();

                    //Create a OLD static data row for each one
                    for (var ii = 0; ii < rowCount; ii++)
                    {
                        //For each row create N cells one for each column
                        var rowEntry = new nHydrate.Generator.Models.RowEntry(newTable.Root);
                        var staticDataFieldList = fieldList.Where(x => !x.IsBinaryType() && x.DataType != DataTypeConstants.Timestamp).ToList();
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
                                    //cellEntry.ResetId(HashString(cellEntry.Key));
                                }

                                //Add the cell to the row
                                rowEntry.CellEntries.Add(cellEntry);
                            }

                        }
                        newTable.StaticData.Add(rowEntry);
                    }
                    //}
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
                                if (ownerModule != null)
                                {
                                    isValidRelation = ((IModuleLink)relation).Modules.Contains(ownerModule);
                                }

                                if (model.UseModules && isValidRelation)
                                {
                                    //If using modules then check that this relation's fields are in this module
                                    foreach (var columnSet in fieldList)
                                    {
                                        var field1 = parent.Fields.FirstOrDefault(x => x.Id == columnSet.SourceFieldId);
                                        var field2 = child.Fields.FirstOrDefault(x => x.Id == columnSet.TargetFieldId);
                                        if (!field1.Modules.Contains(ownerModule)) isValidRelation = false;
                                        if (!field2.Modules.Contains(ownerModule)) isValidRelation = false;
                                    }

                                }

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

                                    if (ownerModule == null)
                                    {
                                        newRelation.Enforce = relation.IsEnforced;
                                    }
                                    else
                                    {
                                        var relationModule = model.RelationModules.FirstOrDefault(x => x.RelationID == relation.Id && x.ModuleId == ownerModule.Id);
                                        if (relationModule == null)
                                        {
                                            //I do not think this should ever happen??
                                            newRelation.Enforce = false;
                                        }
                                        else
                                        {
                                            newRelation.Enforce = relationModule.IsEnforced;
                                        }
                                    }

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
                                else
                                {
                                    System.Diagnostics.Debug.Write(string.Empty);
                                }

                            }
                            else
                            {
                                System.Diagnostics.Debug.Write(string.Empty);
                            }

                        }
                    }

                } //inner block

                #endregion

                #region Views
                foreach (var view in model.Views.Where(x => x.IsGenerated).Where(x => ownerModule == null || x.Modules.Contains(ownerModule)))
                {
                    var newView = root.Database.CustomViews.Add();
                    newView.ResetKey(view.Id.ToString());
                    newView.ResetId(HashString(newView.Key));
                    newView.CodeFacade = view.CodeFacade;
                    newView.DBSchema = view.Schema;
                    newView.Description = view.Summary;
                    newView.Generated = view.IsGenerated;
                    newView.Name = view.Name;
                    newView.SQL = view.SQL;
                    newView.GeneratesDoubleDerived = view.GeneratesDoubleDerived;
                    newView.PrecedenceOrder = view.PrecedenceOrder;

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
                        newField.FriendlyName = field.FriendlyName;
                        newField.IsPrimaryKey = field.IsPrimaryKey;
                        newField.Generated = field.IsGenerated;
                        newField.Length = field.Length;
                        newField.Name = field.Name;
                        newField.Scale = field.Scale;
                        newView.Columns.Add(newField.CreateRef(newField.Key));
                        newField.ParentViewRef = newView.CreateRef(newView.Key);
                    }

                }
                #endregion

                #region View Relations
                {
                    var relationConnectors = diagram.NestedChildShapes.Where(x => x is EntityViewAssociationConnector).Cast<EntityViewAssociationConnector>().ToList();
                    foreach (var shape in relationConnectors)
                    {
                        if (shape is EntityViewAssociationConnector)
                        {
                            var connector = shape as EntityViewAssociationConnector;
                            var parent = connector.FromShape.ModelElement as Entity;
                            var child = connector.ToShape.ModelElement as nHydrate.Dsl.View;

                            var relation = connector.ModelElement as EntityHasViews;
                            var fieldList = model.RelationFields.Where(x => x.RelationID == relation.Id);

                            var parentTable = root.Database.Tables.FirstOrDefault(x => x.Name == parent.Name);
                            var childTable = root.Database.CustomViews.FirstOrDefault(x => x.Name == child.Name);

                            //If we found both parent and child tables...
                            if (parentTable != null && childTable != null)
                            {
                                var isValidRelation = true;
                                if (ownerModule != null)
                                {
                                    isValidRelation = ((IModuleLink)relation).Modules.Contains(ownerModule);
                                }

                                if (model.UseModules && isValidRelation)
                                {
                                    //If using modules then check that this relation's fields are in this module
                                    foreach (var columnSet in fieldList)
                                    {
                                        var field1 = parent.Fields.FirstOrDefault(x => x.Id == columnSet.SourceFieldId);
                                        var field2 = child.Fields.FirstOrDefault(x => x.Id == columnSet.TargetFieldId);
                                        if (!field1.Modules.Contains(ownerModule)) isValidRelation = false;
                                    }

                                }

                                if (isValidRelation)
                                {
                                    var newRelation = root.Database.ViewRelations.Add();
                                    newRelation.ParentTableRef = parentTable.CreateRef(parentTable.Key);
                                    newRelation.ChildViewRef = childTable.CreateRef(childTable.Key);
                                    newRelation.RoleName = ((EntityHasViews)connector.ModelElement).RoleName;

                                    //Create the column links
                                    foreach (var columnSet in fieldList)
                                    {
                                        var field1 = parent.Fields.FirstOrDefault(x => x.Id == columnSet.SourceFieldId);
                                        var field2 = child.Fields.FirstOrDefault(x => x.Id == columnSet.TargetFieldId);

                                        var column1 = parentTable.GetColumnsFullHierarchy().FirstOrDefault(x => x.Name == field1.Name);
                                        var column2 = childTable.GetColumns().FirstOrDefault(x => x.Name == field2.Name);

                                        newRelation.ColumnRelationships.Add(new nHydrate.Generator.Models.ViewColumnRelationship(root)
                                        {
                                            ParentColumnRef = column1.CreateRef(column1.Key),
                                            ChildColumnRef = column2.CreateRef(column2.Key),
                                        }
                                        );
                                    }

                                    //Actually add the relation to the collection
                                    if (newRelation.ColumnRelationships.Count > 0)
                                        parentTable.ViewRelationships.Add(newRelation.CreateRef(newRelation.Key));
                                }
                                else
                                {
                                    System.Diagnostics.Debug.Write(string.Empty);
                                }

                            }
                            else
                            {
                                System.Diagnostics.Debug.Write(string.Empty);
                            }

                        }
                    }

                } //inner block

                #endregion

                #region Stored Procedures
                foreach (var storedProc in model.StoredProcedures.Where(x => x.IsGenerated).Where(x => ownerModule == null || x.Modules.Contains(ownerModule)))
                {
                    var newStoredProc = root.Database.CustomStoredProcedures.Add();
                    newStoredProc.ResetKey(storedProc.Id.ToString());
                    newStoredProc.ResetId(HashString(newStoredProc.Key));
                    newStoredProc.CodeFacade = storedProc.CodeFacade;
                    newStoredProc.DatabaseObjectName = storedProc.DatabaseObjectName;
                    newStoredProc.DBSchema = storedProc.Schema;
                    newStoredProc.Description = storedProc.Summary;
                    newStoredProc.Generated = storedProc.IsGenerated;
                    newStoredProc.Name = storedProc.Name;
                    newStoredProc.SQL = storedProc.SQL;
                    newStoredProc.GeneratesDoubleDerived = storedProc.GeneratesDoubleDerived;
                    newStoredProc.PrecedenceOrder = storedProc.PrecedenceOrder;
                    newStoredProc.IsExisting = storedProc.IsExisting;

                    foreach (var field in storedProc.Fields)
                    {
                        var newField = root.Database.CustomStoredProcedureColumns.Add();
                        newField.ResetKey(field.Id.ToString());
                        newField.ResetId(HashString(newField.Key));
                        newField.AllowNull = field.Nullable;
                        newField.CodeFacade = field.CodeFacade;
                        newField.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.DataType.ToString());
                        newField.Default = field.Default;
                        newField.Description = field.Summary;
                        newField.FriendlyName = field.FriendlyName;
                        newField.Generated = field.IsGenerated;
                        newField.Length = field.Length;
                        newField.Name = field.Name;
                        newField.Scale = field.Scale;
                        newStoredProc.Columns.Add(newField.CreateRef(newField.Key));
                        newField.ParentRef = newStoredProc.CreateRef(newStoredProc.Key);
                    }

                    //Just in case these are ordered get all sort-ordered parameters first then take on all unordred alphabetized parmameters
                    var orderedParameters = storedProc.Parameters.Where(x => x.SortOrder > 0).OrderBy(x => x.SortOrder).ToList();
                    orderedParameters.AddRange(storedProc.Parameters.Where(x => x.SortOrder == 0).OrderBy(x => x.Name).ToList());
                    foreach (var parameter in orderedParameters)
                    {
                        var newParameter = root.Database.CustomRetrieveRuleParameters.Add();
                        newParameter.ResetKey(parameter.Id.ToString());
                        newParameter.ResetId(HashString(newParameter.Key));
                        newParameter.AllowNull = parameter.Nullable;
                        newParameter.CodeFacade = parameter.CodeFacade;
                        newParameter.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), parameter.DataType.ToString());
                        newParameter.Default = parameter.Default;
                        newParameter.Description = parameter.Summary;
                        newParameter.Generated = parameter.IsGenerated;
                        newParameter.IsOutputParameter = parameter.IsOutputParameter;
                        newParameter.Length = parameter.Length;
                        newParameter.Name = parameter.Name;
                        newParameter.Scale = parameter.Scale;
                        newParameter.SortOrder = parameter.SortOrder;
                        newStoredProc.Parameters.Add(newParameter.CreateRef(newParameter.Key));
                        newParameter.ParentTableRef = newStoredProc.CreateRef(newStoredProc.Key);
                    }

                }
                #endregion

                #region Functions
                foreach (var function in model.Functions.Where(x => x.IsGenerated).Where(x => ownerModule == null || x.Modules.Contains(ownerModule)))
                {
                    var newFunction = root.Database.Functions.Add();
                    newFunction.ResetKey(function.Id.ToString());
                    newFunction.ResetId(HashString(newFunction.Key));
                    newFunction.CodeFacade = function.CodeFacade;
                    newFunction.DBSchema = function.Schema;
                    newFunction.Description = function.Summary;
                    newFunction.Generated = function.IsGenerated;
                    newFunction.Name = function.Name;
                    newFunction.SQL = function.SQL;
                    newFunction.IsTable = function.IsTable;
                    newFunction.ReturnVariable = function.ReturnVariable;
                    newFunction.PrecedenceOrder = function.PrecedenceOrder;

                    foreach (var field in function.Fields)
                    {
                        var newField = root.Database.FunctionColumns.Add();
                        newField.ResetKey(field.Id.ToString());
                        newField.ResetId(HashString(newField.Key));
                        newField.AllowNull = field.Nullable;
                        newField.CodeFacade = field.CodeFacade;
                        newField.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.DataType.ToString());
                        newField.Default = field.Default;
                        newField.Description = field.Summary;
                        newField.FriendlyName = field.FriendlyName;
                        newField.Generated = field.IsGenerated;
                        newField.Length = field.Length;
                        newField.Name = field.Name;
                        newField.Scale = field.Scale;
                        newFunction.Columns.Add(newField.CreateRef(newField.Key));
                        newField.ParentRef = newFunction.CreateRef(newFunction.Key);
                    }

                    //Just in case these are ordered get all sort-ordered parameters first then take on all unordred alphabetized parmameters
                    var orderedParameters = function.Parameters.Where(x => x.SortOrder > 0).OrderBy(x => x.SortOrder).ToList();
                    orderedParameters.AddRange(function.Parameters.Where(x => x.SortOrder == 0).OrderBy(x => x.Name).ToList());
                    foreach (var parameter in orderedParameters)
                    {
                        var newParameter = root.Database.FunctionParameters.Add();
                        newParameter.ResetKey(parameter.Id.ToString());
                        newParameter.ResetId(HashString(newParameter.Key));
                        newParameter.AllowNull = parameter.Nullable;
                        newParameter.CodeFacade = parameter.CodeFacade;
                        newParameter.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), parameter.DataType.ToString());
                        newParameter.Default = parameter.Default;
                        newParameter.Description = parameter.Summary;
                        newParameter.Generated = parameter.IsGenerated;
                        newParameter.Length = parameter.Length;
                        newParameter.Name = parameter.Name;
                        newParameter.Scale = parameter.Scale;
                        newParameter.SortOrder = parameter.SortOrder;

                        var r = newParameter.CreateRef(newParameter.Key);
                        r.RefType = nHydrate.Generator.Models.ReferenceType.FunctionParameter;
                        newFunction.Parameters.Add(r);
                        newParameter.ParentTableRef = newFunction.CreateRef(newFunction.Key);
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

        private class ModuleVersionInfo
        {
            public string ModuleName { get; set; }
            public int ServerVersion { get; set; }
        }

    }
}