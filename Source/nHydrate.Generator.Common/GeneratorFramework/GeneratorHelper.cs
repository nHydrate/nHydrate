using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using EnvDTE;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.Forms;
using nHydrate.Generator.Common.Logging;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public class GeneratorHelper
    {
        #region Class Members

        private static bool? _canGenerate = null;
        private List<string> _errorList = new List<string>();

        #endregion

        #region Events

        public event ProjectItemGeneratedEventHandler ProjectItemGenerated;
        public event ProjectItemDeletedEventHandler ProjectItemDeleted;
        public event ProjectItemGenerationCompleteEventHandler GenerationComplete;
        public event ProjectItemGeneratedEventHandler ProjectItemGeneratedError;

        protected virtual void OnProjectItemGenerated(object sender, ProjectItemGeneratedEventArgs pigArgs)
        {
            ProjectItemGenerated?.Invoke(sender, pigArgs);
        }

        protected virtual void OnProjectItemDeleted(object sender, ProjectItemDeletedEventArgs pigArgs)
        {
            if (ProjectItemDeleted != null)
            {
                this.ProjectItemDeleted(sender, pigArgs);
            }
        }

        protected virtual void OnGenerationComplete(object sender, ProjectItemGenerationCompleteEventArgs args)
        {
            if (GenerationComplete != null)
            {
                this.GenerationComplete(sender, args);
            }
        }

        protected virtual void OnProjectItemGeneratedError(object sender, ProjectItemGeneratedEventArgs pigArgs)
        {
            if (ProjectItemGeneratedError != null)
            {
                this.ProjectItemGeneratedError(sender, pigArgs);
            }
        }

        #endregion

        #region Model Methods
        private const string ROOT_NODE = "model";

        public static IGenerator OpenModel(string filePath)
        {
            IGenerator retVal = null;
            var file = new FileInfo(filePath);
            var xmlAttributeAssembleValue = string.Empty;
            if (file.Exists)
            {
                var processKey = string.Empty;
                try
                {
                    var xDoc = new XmlDocument();
                    string type = null;
                    FileInfo assemblyName = null;
                    xDoc.Load(file.FullName);
                    type = Common.Util.XmlHelper.GetAttributeValue(xDoc.DocumentElement, "type", string.Empty);
                    xmlAttributeAssembleValue = Common.Util.XmlHelper.GetAttributeValue(xDoc.DocumentElement, "assembly", string.Empty);
                    Uri assemblyUri = null;
                    try { assemblyUri = new Uri(xmlAttributeAssembleValue); }
                    catch { }

                    if (assemblyUri != null) assemblyName = new FileInfo(assemblyUri.AbsolutePath);
                    else assemblyName = new FileInfo(xmlAttributeAssembleValue);

                    var assemblyFile = Path.Combine(AddinAppData.Instance.ExtensionDirectory, assemblyName.Name);
                    var currentAssemblyFile = new FileInfo(assemblyFile);
                    if (currentAssemblyFile.Exists)
                    {
                        retVal = (IGenerator)ReflectionHelper.CreateInstance(currentAssemblyFile.FullName, type);
                        retVal.XmlLoad(xDoc.DocumentElement);
                        retVal.FileName = filePath;
                    }
                    else
                    {
                        GlobalHelper.ShowError("The model cannot be opened. You do not have the appropriate assembly. " + currentAssemblyFile.FullName);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(file.FullName + " does not have the correct format.", ex);
                }
            }
            else
            {
                throw new Exception("File does not exist:");
            }
            return retVal;
        }

        //private static void CreateNewModelFile(string loadingFile)
        //{
        //  var nmf = new NewModelForm(loadingFile);
        //  if (string.IsNullOrEmpty(nmf.FileContent))
        //    nmf.ShowDialog();
        //  var fi = new FileInfo(loadingFile);
        //  using (var writer = fi.AppendText())
        //  {
        //    writer.Write(nmf.FileContent);
        //    writer.Close();
        //  }
        //}

        public static void SaveModelFile(IGenerator generatorProject, string fullFileName)
        {
            var att = (GeneratorAttribute)ReflectionHelper.GetSingleAttribute(typeof(GeneratorAttribute), generatorProject);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(string.Format("<{0}></{0}>", ROOT_NODE));

            var xmlAttr = xmlDoc.CreateAttribute("guid");
            xmlAttr.Value = att.ProjectGuid.ToString();
            xmlDoc.DocumentElement.Attributes.Append(xmlAttr);

            var typeAttribute = xmlDoc.CreateAttribute("type");
            typeAttribute.Value = generatorProject.GetType().FullName;
            xmlDoc.DocumentElement.Attributes.Append(typeAttribute);

            var assemblyAttribute = xmlDoc.CreateAttribute("assembly");
            assemblyAttribute.Value = new FileInfo(generatorProject.GetType().Assembly.Location).Name;
            xmlDoc.DocumentElement.Attributes.Append(assemblyAttribute);

            generatorProject.XmlAppend(xmlDoc.DocumentElement);
            xmlDoc.Save(fullFileName);
        }

        public static ArrayList GetModels(ProjectItem projectItem)
        {
            var al = new ArrayList();
            foreach (ProjectItem pi in projectItem.ProjectItems)
            {
                if (IsModelFile(pi))
                {
                    al.Add(pi);
                }
                al.AddRange(GetModels(pi));
            }
            return al;
        }

        public static bool IsModelFile(ProjectItem projectItem)
        {
            var fullName = projectItem.get_FileNames(1);
            return IsModelFile(fullName);
        }

        public static bool IsModelFile(string fileName)
        {
            var ext = System.IO.Path.GetExtension(fileName);
            if ((string.Compare(ext, ".wsgen", true) != 0))
                return false;
            else
                return true;
        }

        private static Guid ModelFileGuid(string fileName)
        {
            var retVal = Guid.Empty;
            var textReader = new XmlTextReader(fileName);
            // If the node has value
            while (textReader.Read())
            {
                if (textReader.Name.ToLower() == ROOT_NODE)
                {
                    textReader.MoveToAttribute("guid");
                    var valid = StringHelper.GuidTryParse(textReader.Value, out retVal);
                    if (valid)
                        break;
                }
            }
            textReader.Close();
            return retVal;
        }

        /// <summary>
        /// In Vista if UAC is on the user does not have permissions to run this application
        /// </summary>
        /// <returns></returns>		
        public static bool CanGenerate()
        {

            if (_canGenerate != null) return _canGenerate.Value;

            try
            {
                //try to access the appdata path
                object o = AddinAppData.Instance;

                //try to access the install folder
                var files = Directory.GetFiles(AddinAppData.Instance.ExtensionDirectory, "*.csproj");
                if (files.Length > 0)
                {
                    var fileData = string.Empty;
                    using (var sr = File.OpenText(files[0]))
                    {
                        fileData = sr.ReadToEnd();
                    }
                }

                //try to create a file in the install folder
                var newFileName = Path.Combine(AddinAppData.Instance.ExtensionDirectory, Guid.NewGuid().ToString());
                File.WriteAllText(newFileName, "test");
                System.Threading.Thread.Sleep(500);
                File.Delete(newFileName);

                _canGenerate = true;
                return true;
            }
            catch (Exception ex)
            {
                _canGenerate = false;
                return false;
            }
        }

        #endregion

        #region Generation Methods

        #region Public Count Methods

        public int GetFileCount(IGenerator generator, List<Type> excludeList)
        {
            var retval = 0;
            try
            {
                EnvDTEHelper.Instance.ClearCache();
                var globalCacheFile = new GlobalCacheFile();
                _generator = generator;
                var projectGenerators = GetProjectGenerators(generator);
                var generatorTypes = ReflectionHelper.GetCreatableObjectImplementsInterface(typeof(IProjectItemGenerator), AddinAppData.Instance.ExtensionDirectory);
                foreach (var projectGeneratorType in projectGenerators)
                {
                    try
                    {
                        var exclude = false;
                        foreach (var key in globalCacheFile.ExcludeList)
                        {
                            if (key == projectGeneratorType.FullName)
                                exclude = true;
                        }

                        //Check the passed in exclude list
                        if (excludeList.Contains(projectGeneratorType))
                            exclude = true;

                        if (!exclude)
                        {
                            retval += GetFileCount(generator, projectGeneratorType, generatorTypes);
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                        MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                System.Diagnostics.Debug.Write(string.Empty);
                return retval;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private int GetFileCount(IGenerator generator, Type projectGeneratorType, System.Type[] generatorTypes)
        {
            var retval = 0;
            try
            {
                var projectGenerator = GetProjectGenerator(projectGeneratorType);
                projectGenerator.Initialize(generator.Model);
                retval += GetFileCount(projectGenerator, generatorTypes);
                retval += 1; // The actual project file it self
                return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int GetFileCount(IProjectGenerator projectGenerator, System.Type[] generatorTypes)
        {
            var retval = 0;
            try
            {
                var projectItemGenerators = GetProjectItemGenerators(projectGenerator);
                foreach (var projectItemGeneratorType in projectItemGenerators)
                {
                    try
                    {
                        retval += GetFileCount(projectItemGeneratorType, generatorTypes);
                    }
                    catch (Exception ex)
                    {
                        nHydrateLog.LogWarning(ex);
                    }
                }
                return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int GetFileCount(Type projectItemGeneratorType, System.Type[] generatorTypes)
        {
            var retval = 0;
            try
            {
                var projectItemGenerator = GetProjectItemGenerator(projectItemGeneratorType);
                projectItemGenerator.Initialize(_generator.Model);
                retval += GetFileCount(projectItemGenerator, generatorTypes);
                return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int GetFileCount(IProjectItemGenerator projectItemGenerator, System.Type[] generatorTypes)
        {
            var retval = 0;
            try
            {
                retval += projectItemGenerator.FileCount;

                foreach (var type in generatorTypes)
                {
                    var subProjectItemGenerator = GetProjectItemGenerator(type);
                    var genItemAttribute = (GeneratorItemAttribute)ReflectionHelper.GetSingleAttribute(typeof(GeneratorItemAttribute), subProjectItemGenerator);
                    if (genItemAttribute != null && genItemAttribute.ParentType == projectItemGenerator.GetType())
                    {
                        subProjectItemGenerator.Initialize(_generator.Model);
                        retval += GetFileCount(subProjectItemGenerator, generatorTypes);
                    }
                }

                return retval;
            }
            catch (Exception ex)
            {
                _errorList.Add(ex.ToString());
                nHydrateLog.LogError(ex);
                throw;
            }
        }

        #endregion

        #region Public Generate Methods

        public IGenerator _generator;
        public void GenerateAll(IGenerator generator, List<Type> excludeList)
        {
            try
            {
                EnvDTEHelper.Instance.ClearCache();
                var globalCacheFile = new GlobalCacheFile();
                _generator = generator;
                var projectGenerators = GetProjectGenerators(generator);
                foreach (var projectGeneratorType in projectGenerators)
                {
                    try
                    {
                        var exclude = false;
                        foreach (var key in globalCacheFile.ExcludeList)
                        {
                            if (key == projectGeneratorType.FullName)
                                exclude = true;
                        }

                        //Check the passed in exclude list
                        if (excludeList.Contains(projectGeneratorType))
                            exclude = true;

                        if (!exclude)
                        {
                            GenerateProject(generator, projectGeneratorType);
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                        MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                System.Diagnostics.Debug.Write(string.Empty);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void GenerateProject(IGenerator generator, Type projectGeneratorType)
        {
            try
            {
                var projectGenerator = GetProjectGenerator(projectGeneratorType);
                projectGenerator.Initialize(generator.Model);
                if (!EnvDTEHelper.ProjectExists(projectGenerator.ProjectName))
                {
                    CreateProject(generator, projectGeneratorType);
                }
                else if (!EnvDTEHelper.ProjectLoaded(projectGenerator.ProjectName))
                {
                    //LoadProject(generator, projectGeneratorType);
                    //throw new Exception("The project '" + projectGenerator.ProjectName + "' is unloaded and cannot be generated.");
                    _errorList.Add("The project '" + projectGenerator.ProjectName + "' is unloaded and cannot be generated.");
                    return;
                }
                GenerateProjectItems(projectGenerator);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CreateProject(IGenerator generator, Type projectGeneratorType)
        {
            try
            {
                var projectGenerator = GetProjectGenerator(projectGeneratorType);
                projectGenerator.Initialize(generator.Model);
                projectGenerator.CreateProject();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IProjectGenerator GetProjectGenerator(Type projectGeneratorType)
        {
            try
            {
                var projectGenerator = (IProjectGenerator)ReflectionHelper.CreateInstance(projectGeneratorType);
                return projectGenerator;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private IProjectItemGenerator GetProjectItemGenerator(Type projectItemGeneratorType)
        {
            try
            {
                var projectGenerator = (IProjectItemGenerator)ReflectionHelper.CreateInstance(projectItemGeneratorType);
                return projectGenerator;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GenerateProjectItems(IProjectGenerator projectGenerator)
        {
            try
            {
                var projectItemGenerators = GetProjectItemGenerators(projectGenerator);
                foreach (var projectItemGeneratorType in projectItemGenerators)
                {
                    try
                    {
                        GenerateProjectItems(projectItemGeneratorType);
                    }
                    catch (Exception ex)
                    {
                        nHydrateLog.LogWarning(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GenerateSubItems(IProjectItemGenerator projectItemGenerator)
        {
            try
            {
                var projectItemGenerators = GetProjectItemGenerators(projectItemGenerator);
                foreach (var projectItemGeneratorType in projectItemGenerators)
                {
                    try
                    {
                        GenerateProjectItems(projectItemGeneratorType);
                    }
                    catch (Exception ex)
                    {
                        nHydrateLog.LogWarning(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GenerateProjectItems(Type projectItemGeneratorType)
        {
            try
            {
                var projectItemGenerator = GetProjectItemGenerator(projectItemGeneratorType);
                projectItemGenerator.Initialize(_generator.Model);
                GenerateProjectItems(projectItemGenerator);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GenerateProjectItems(IProjectItemGenerator projectItemGenerator)
        {
            try
            {
                projectItemGenerator.GenerationComplete += new ProjectItemGenerationCompleteEventHandler(projectItemGenerator_GenerationComplete);
                projectItemGenerator.ProjectItemGenerated += new ProjectItemGeneratedEventHandler(projectItemGenerator_ProjectItemGenerated);
                projectItemGenerator.ProjectItemDeleted += new ProjectItemDeletedEventHandler(projectItemGenerator_ProjectItemDeleted);
                projectItemGenerator.ProjectItemExists += new ProjectItemExistsEventHandler(projectItemGenerator_ProjectItemExists);
                projectItemGenerator.ProjectItemGenerationError += new ProjectItemGeneratedErrorEventHandler(projectItemGenerator_ProjectItemGenerationError);
                projectItemGenerator.Generate();
            }
            catch (Exception ex)
            {
                _errorList.Add(ex.ToString());
                nHydrateLog.LogError(ex);
                throw;
            }
        }

        public List<Type> GetProjectGenerators(IGenerator generator)
        {
            return GetGeneratorsImpl(generator);
        }

        private List<Type> GetProjectItemGenerators(IProjectGenerator projectGenerator)
        {
            return GetGeneratorsImpl(projectGenerator);
        }

        private List<Type> GetProjectItemGenerators(IProjectItemGenerator projectItemGenerator)
        {
            return GetGeneratorsImpl(projectItemGenerator);
        }
        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the errors that have occured since object instanciation
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetErrorList()
        {
            return _errorList;
        }

        private readonly List<string> processedFiles = new List<string>();
        private static Dictionary<string, Project> projectCache = new Dictionary<string, Project>();
        private int _doEventCount = 0;
        private void projectItemGenerator_ProjectItemGenerated(object sender, ProjectItemGeneratedEventArgs e)
        {
            //.NET is a single-threaded application so we much release to let the UI refresh sometimes.
            if (_doEventCount % 10 == 0)
                Application.DoEvents();
            _doEventCount++;

            try
            {
                const string solutionDirReplaceText = "$(solutiondir)";
                if (e.FullName != null && e.FullName.ToLower().Contains(solutionDirReplaceText))
                {
                    var ti = e.FullName.ToLower().IndexOf(solutionDirReplaceText);
                    e.FullName = e.FullName.Remove(ti, solutionDirReplaceText.Length).Insert(ti, EnvDTEHelper.Instance.SolutionDirectory.FullName);
                }
                if (e.ProjectItemName != null && e.ProjectItemName.ToLower().Contains(solutionDirReplaceText))
                {
                    var ti = e.ProjectItemName.ToLower().IndexOf(solutionDirReplaceText);
                    e.ProjectItemName = e.ProjectItemName.Remove(ti, solutionDirReplaceText.Length).Insert(ti, EnvDTEHelper.Instance.SolutionDirectory.FullName);
                }

                //Get the parent project if one exists
                Project project = null;
                ProjectItem parent = null;
                if (!string.IsNullOrEmpty(e.ProjectName))
                {
                    if (projectCache.ContainsKey(e.ProjectName))
                    {
                        var p = projectCache[e.ProjectName];
                        //Test this COM object to ensure it has not expired. 
                        //If error, do nothing and the project will be requeried
                        try
                        {
                            if (p != null)
                            {
                                var s = p.Name;
                                project = p;
                            }
                            else
                                projectCache.Remove(e.ProjectName);
                        }
                        catch (Exception)
                        {
                            //Do Nothing
                            projectCache.Remove(e.ProjectName);
                        }
                    }

                    var fromCache = true;
                    if (project == null)
                    {
                        fromCache = false;
                        project = EnvDTEHelper.Instance.GetProject(e.ProjectName);
                        projectCache.Add(e.ProjectName, project);
                    }

                    if (!string.IsNullOrEmpty(e.ParentItemName))
                    {
                        parent = EnvDTEHelper.Instance.GetProjectItem(e.ProjectName, e.ParentItemName, e.ParentItemType);

                        //This should not happen. If do dump the cache project and requery
                        if (parent == null && fromCache)
                        {
                            if (projectCache.ContainsKey(e.ProjectName))
                                projectCache.Remove(e.ProjectName);
                            project = EnvDTEHelper.Instance.GetProject(e.ProjectName);
                            projectCache.Add(e.ProjectName, project);
                            parent = EnvDTEHelper.Instance.GetProjectItem(e.ProjectName, e.ParentItemName, e.ParentItemType);
                        }
                    }

                }

                var fileStateInfo = new FileStateInfo();
                ProjectItem projectItem = null;
                if (!string.IsNullOrEmpty(e.ParentItemName))
                {
                    if (e.ContentType == ProjectItemContentType.String)
                        projectItem = EnvDTEHelper.Instance.AddProjectItem(project, parent, e.ProjectItemName, e.ProjectItemContent, e.Overwrite, out fileStateInfo);
                    else
                    {
                        projectItem = EnvDTEHelper.Instance.AddProjectItem(parent, e.ProjectItemContent, out fileStateInfo);
                    }

                    if (fileStateInfo.FileState == EnvDTEHelper.FileStateConstants.Success)
                        EnvDTEHelper.SetProperties(projectItem, e.Properties);
                }
                else
                {
                    if (e.ContentType == ProjectItemContentType.String || e.ContentType == ProjectItemContentType.Binary)
                    {
                        if (project == null)
                            EnvDTEHelper.Instance.AddProjectItem(e.ProjectItemContent, e.ProjectItemBinaryContent, e.ContentType, e.ProjectItemName, e.Overwrite, out fileStateInfo);
                        else
                            projectItem = EnvDTEHelper.Instance.AddProjectItem(project, e.ProjectItemContent, e.ProjectItemBinaryContent, e.ContentType, e.ProjectItemName, e.Overwrite, out fileStateInfo);
                    }
                    else
                    {
                        projectItem = EnvDTEHelper.Instance.AddFileAsProjectItem(project, e.ProjectItemContent, e.ProjectItemName, e.Overwrite, out fileStateInfo);
                    }

                    if (fileStateInfo.FileState == EnvDTEHelper.FileStateConstants.Success)
                        EnvDTEHelper.SetProperties(projectItem, e.Properties);
                }

                //TEMP
                processedFiles.Add(fileStateInfo.FileName);
                //TEMP

                //Custom Tool Functionality
                if (e.RunCustomTool && projectItem != null)
                {
                    if (!string.IsNullOrEmpty(e.CustomToolName))
                    {
                        EnvDTEHelper.SetProperty(projectItem, "Generator", e.CustomToolName);
                        EnvDTEHelper.SetProperty(projectItem, "CustomTool", e.CustomToolName);
                    }

                    //Try to run the custom tool
                    try
                    {
                        var vsProjectItem = projectItem.Object as VSLangProj.VSProjectItem;
                        if (vsProjectItem != null)
                            vsProjectItem.RunCustomTool();
                    }
                    catch
                    {
                        //Do Nothing
                    }
                }

                if (fileStateInfo.FileName == string.Empty)
                {
                    System.Diagnostics.Debug.Write(string.Empty);
                }
                if (fileStateInfo.FileState == EnvDTEHelper.FileStateConstants.Failed)
                {
                    System.Diagnostics.Debug.Write(string.Empty);
                }

                //Write Log
                nHydrateLog.LogInfo("Project Item Generated: {0}", e.ProjectItemName);
                e.FileState = fileStateInfo.FileState;
                e.FullName = fileStateInfo.FileName;
                this.OnProjectItemGenerated(sender, e);

            }
            catch (Exception ex)
            {
                this.OnProjectItemGeneratedError(this, e);
                nHydrateLog.LogWarning(ex);
            }
        }

        private void projectItemGenerator_ProjectItemDeleted(object sender, ProjectItemDeletedEventArgs e)
        {
            try
            {
                var project = EnvDTEHelper.Instance.GetProject(e.ProjectName);
                var parent = EnvDTEHelper.Instance.GetProjectItem(e.ProjectName, e.ParentItemName, e.ParentItemType);

                var fileStateInfo = new FileStateInfo();
                if (e.ParentItemName != string.Empty)
                {
                    EnvDTEHelper.Instance.DeleteProjectItem(parent, e.ProjectItemName, out fileStateInfo);
                }
                else
                {
                    EnvDTEHelper.DeleteProjectItem(project, e.ProjectItemName, e.DeleteFile, out fileStateInfo);
                }
                Application.DoEvents();

                //Write Log
                nHydrateLog.LogInfo("Project Item Deleted: {0}", e.ProjectItemName);
                e.FileState = fileStateInfo.FileState;
                e.FullName = fileStateInfo.FileName;
                //this.OnProjectItemGenerated(sender, e);
            }
            catch (Exception ex)
            {
                //this.OnProjectItemGeneratedError(this, e);
                nHydrateLog.LogWarning(ex);
            }
        }

        private void projectItemGenerator_ProjectItemGenerationError(object sender, ProjectItemGeneratedErrorEventArgs e)
        {
            if (e.ShowError)
            {
                MessageBox.Show(e.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void projectItemGenerator_ProjectItemExists(object sender, ProjectItemExistsEventArgs e)
        {
            try
            {
                e.Exists = EnvDTEHelper.Instance.GetProjectItemExists(e.ProjectName, e.ItemName, e.ItemType);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void projectItemGenerator_GenerationComplete(object sender, ProjectItemGenerationCompleteEventArgs e)
        {
            nHydrateLog.LogInfo("Project Item Generation Complete: {0}", e.ProjectItemGenerator);
            try
            {
                GenerateSubItems(e.ProjectItemGenerator);
                this.OnGenerationComplete(sender, e);
            }
            catch (Exception ex)
            {
                nHydrateLog.LogWarning(ex);
            }
        }

        private readonly Dictionary<object, List<Type>> _detGeneratorsImplCache = new Dictionary<object, List<Type>>();
        private List<Type> GetGeneratorsImpl(object parent)
        {
            try
            {
                if (_detGeneratorsImplCache.ContainsKey(parent))
                {
                    return _detGeneratorsImplCache[parent];
                }

                var returnVal = new List<Type>();
                Type[] generatorTypes = null;
                if (ReflectionHelper.ImplementsInterface(parent, typeof(IGenerator)))
                {
                    generatorTypes = ReflectionHelper.GetCreatableObjectImplementsInterface(typeof(IProjectGenerator), AddinAppData.Instance.ExtensionDirectory);
                }
                else
                {
                    generatorTypes = ReflectionHelper.GetCreatableObjectImplementsInterface(typeof(IProjectItemGenerator), AddinAppData.Instance.ExtensionDirectory);
                }

                if (generatorTypes.Length == 0)
                {
                    MessageBox.Show($"There are no generators installed or there was an error loading the installed generators from the following path. '{AddinAppData.Instance.ExtensionDirectory}'", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                foreach (var type in generatorTypes)
                {
                    var genItemAttribute = (GeneratorItemAttribute)ReflectionHelper.GetSingleAttribute(typeof(GeneratorItemAttribute), type);
                    if (genItemAttribute != null && genItemAttribute.ParentType == parent.GetType())
                    {
                        returnVal.Add(type);
                    }
                }

                //Cache this for next time
                _detGeneratorsImplCache.Add(parent, returnVal);
                return returnVal;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #endregion
    }
}
