#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections;
using System.IO;
using EnvDTE;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Collections.Generic;

namespace nHydrate.Generator.Common.Util
{
    public class EnvDTEHelper
    {
        #region member variables

        private EnvDTE80.DTE2 _applicationObject;
        private static EnvDTEHelper _instance = null;
        private SolutionEvents _solutionEvents;

        #endregion

        public enum FileStateConstants
        {
            Success,
            Skipped,
            Failed,
        }

        private EnvDTEHelper()
        {
            BackgroundColor = System.Drawing.Color.White;
            ForegroundColor = System.Drawing.Color.Black;
            SelectedBackgroundColor = System.Drawing.Color.FromArgb(0x33, 0x99, 0xff);
        }

        public void ClearCache()
        {
            _projectItemCache = new Dictionary<string, ProjectItem>();
            _projectCache = new HashTable<Project, List<ProjectItemCacheItem>>();
            _projectItemFileNameCache.Clear();
        }

        public _DTE ApplicationObject => _applicationObject;

        public void SetDTE(_DTE applicationObject)
        {
            if (_applicationObject == null)
            {
                _applicationObject = applicationObject as EnvDTE80.DTE2;
                _solutionEvents = (EnvDTE.SolutionEvents)_applicationObject.Events.SolutionEvents;
                TextManagerEvents.Setup();
            }
        }

        public System.Drawing.Color BackgroundColor { get; internal set; }
        public System.Drawing.Color ForegroundColor { get; internal set; }
        public System.Drawing.Color SelectedBackgroundColor { get; internal set; }

        public static EnvDTEHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EnvDTEHelper();
                }
                return _instance;
            }
        }

        public static ProjectItem AddFolder(Project project, string folderName)
        {
            return project.ProjectItems.AddFolder(folderName, Constants.vsProjectItemKindPhysicalFolder);
        }

        public static ProjectItem AddFolder(ProjectItem projectItem, string folderName)
        {
            return projectItem.ProjectItems.AddFolder(folderName, Constants.vsProjectItemKindPhysicalFolder);
        }

        #region AddProjectItem

        /// <summary>
        /// Just gen file with no project
        /// </summary>
        public void AddProjectItem(string fileContent, byte[] fileContentBinary, ProjectItemContentType contentType, string relativePathAndName, bool overwrite, out FileStateInfo fileStateInfo)
        {
            try
            {
                fileStateInfo = new FileStateInfo();
                fileStateInfo.FileState = FileStateConstants.Failed;
                FileInfo newFile = null;

                var fullName = relativePathAndName;
                fullName = fullName.Replace(@"\\", @"\");
                fileStateInfo.FileName = fullName;
                newFile = new FileInfo(fullName);
                if (newFile.Exists && !overwrite)
                {
                    fileStateInfo.FileState = FileStateConstants.Skipped;
                }
                else
                {
                    if (!newFile.Directory.Exists)
                        newFile.Directory.Create();

                    if (contentType == ProjectItemContentType.String)
                    {
                        using (var sw = newFile.CreateText())
                        {
                            sw.Write(fileContent);
                        }
                    }
                    else
                    {
                        File.WriteAllBytes(newFile.FullName, fileContentBinary);
                    }

                    fileStateInfo.FileState = FileStateConstants.Success;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ProjectItem AddProjectItem(Project project, string fileContent, byte[] fileContentBinary, ProjectItemContentType contentType, string relativePathAndName, bool overwrite, out FileStateInfo fileStateInfo)
        {
            try
            {
                BuildCache(project);

                fileStateInfo = new FileStateInfo();
                fileStateInfo.FileState = FileStateConstants.Failed;
                FileInfo newFile = null;
                if (project.Kind == Constants.vsProjectKindUnmodeled)
                {
                    var applicationObject = (DTE)EnvDTEHelper.Instance._applicationObject;
                    //Handle on Solution Explorer window
                    var slnExplorer = applicationObject.Windows.Item(Constants.vsWindowKindSolutionExplorer);
                    var slnHierarchy = (UIHierarchy)slnExplorer.Object;
                    slnExplorer.Activate();
                    var dbProject = EnvDTEHelper.Instance.Find(project);
                    dbProject.Select(vsUISelectionType.vsUISelectionTypeSelect);
                }
                else
                {
                    var currentProjectFile = new FileInfo(project.FileName);
                    var fullName = StringHelper.EnsureDirectorySeparatorAtEnd(currentProjectFile.Directory.FullName) + relativePathAndName;
                    fullName = fullName.Replace(@"\\", @"\");
                    fileStateInfo.FileName = fullName;
                    newFile = new FileInfo(fullName);

                    if (newFile.Exists && !overwrite)
                    {
                        fileStateInfo.FileState = FileStateConstants.Skipped;

                        if (_projectCache[project].Select(x => x.Name).Contains(fullName.ToLower()))
                        {
                            //Do Nothing
                            return null;
                        }
                        else
                        {
                            return project.ProjectItems.AddFromFile(newFile.FullName);
                        }
                    }
                    else if (newFile.Exists && (newFile.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        fileStateInfo.FileState = FileStateConstants.Failed;
                        return null;
                    }
                    if (!newFile.Directory.Exists)
                        newFile.Directory.Create();

                    if (contentType == ProjectItemContentType.String)
                    {
                        using (var sw = newFile.CreateText())
                        {
                            sw.Write(fileContent);
                        }
                    }
                    else
                    {
                        File.WriteAllBytes(newFile.FullName, fileContentBinary);
                    }

                }

                try
                {
                    fileStateInfo.FileState = FileStateConstants.Success;
                    if (_projectCache[project].Select(x => x.Name).Contains(newFile.FullName.ToLower()))
                    {
                        //Do Nothing
                        return null;
                    }
                    else
                    {
                        return project.ProjectItems.AddFromFile(newFile.FullName);
                    }
                }
                catch (Exception ex)
                {
                    fileStateInfo.FileState = FileStateConstants.Failed;
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ProjectItem AddProjectItem(ProjectItem parent, string fileName, out FileStateInfo fileStateInfo)
        {
            fileStateInfo = new FileStateInfo();
            try
            {
                fileStateInfo.FileName = fileName;
                var newItem = parent.ProjectItems.AddFromFileCopy(fileName);
                fileStateInfo.FileState = FileStateConstants.Success;
                return newItem;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                if (ex.Message.ToLower().Contains("already exists"))
                    fileStateInfo.FileState = FileStateConstants.Skipped;
                else
                    fileStateInfo.FileState = FileStateConstants.Failed;
                return null;
            }
            catch (Exception ex)
            {
                fileStateInfo.FileState = FileStateConstants.Failed;
                throw;
            }
        }

        private static HashTable<Project, List<ProjectItemCacheItem>> _projectCache = new HashTable<Project, List<ProjectItemCacheItem>>();
        private Dictionary<string, ProjectItem> _projectItemCache = new Dictionary<string, ProjectItem>();
        private HashTable<ProjectItem, string> _projectItemFileNameCache = new HashTable<ProjectItem, string>();

        public ProjectItem AddProjectItem(Project project, ProjectItem parent, string fileName, string content, bool overwrite, out FileStateInfo fileStateInfo)
        {
            try
            {
                BuildCache(project);

                fileStateInfo = new FileStateInfo();
                var fi = new FileInfo(this.GetFileName(parent));
                var fullName = Path.Combine(fi.DirectoryName, fileName);
                fileStateInfo.FileName = fullName;
                try
                {
                    var fi2 = new FileInfo(fullName);
                    if (File.Exists(fullName) && !overwrite)
                    {
                        fileStateInfo.FileState = FileStateConstants.Skipped;
                        return null;
                    }
                    else if (fi2.Exists && (fi2.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        fileStateInfo.FileState = FileStateConstants.Failed;
                        return null;
                    }
                    else
                    {
                        File.WriteAllText(fullName, content);
                        fileStateInfo.FileState = FileStateConstants.Success;
                    }
                }
                catch (Exception ex)
                {
                    fileStateInfo.FileState = FileStateConstants.Failed;
                    //GlobalHelper.ShowError(ex);
                }

                ProjectItem newItem = null;
                if (_projectCache[project].Select(x => x.Name).Contains(fullName.ToLower()))
                {
                    //Do Nothing
                }
                else
                {
                    if (_projectItemCache.ContainsKey(fullName))
                        newItem = _projectItemCache[fullName];
                    else
                        newItem = parent.ProjectItems.AddFromFile(fullName);
                }

                return newItem;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        private void BuildCache(Project project)
        {
            try
            {
                if (_projectCache.ContainsKey(project)) return;
                _projectCache.Add(project, null);

                foreach (ProjectItem pItem in project.ProjectItems)
                {
                    BuildCacheSub(pItem);
                }

                #region Cache the existing items in the project file

                var cache = new List<ProjectItemCacheItem>();
                CacheProjectItems(project.ProjectItems, cache);
                _projectCache[project] = cache;

                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CacheProjectItems(ProjectItems projectItems, List<ProjectItemCacheItem> cache)
        {
            try
            {
                foreach (ProjectItem item in projectItems)
                {
                    if (item.Kind == Constants.vsProjectItemKindPhysicalFile)
                        cache.Add(new ProjectItemCacheItem()
                        {
                            Name = item.get_FileNames(1).ToLower()
                        });
                    CacheProjectItems(item.ProjectItems, cache);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void BuildCacheSub(ProjectItem projectItem)
        {
            try
            {
                var k = projectItem.get_FileNames(1);
                if (!_projectItemCache.ContainsKey(k))
                    _projectItemCache.Add(k, projectItem);
                else
                    System.Diagnostics.Debug.Write(string.Empty);

                foreach (ProjectItem pItem in projectItem.ProjectItems)
                {
                    BuildCacheSub(pItem);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ProjectItem AddFileAsProjectItem(Project project, string fileName, string relativePathAndName, bool overwrite, out FileStateInfo fileStateInfo)
        {
            try
            {
                fileStateInfo = new FileStateInfo();
                fileStateInfo.FileState = FileStateConstants.Failed;
                FileInfo newFile = null;
                if (project.Kind == Constants.vsProjectKindUnmodeled)
                {
                    var applicationObject = (DTE)EnvDTEHelper.Instance._applicationObject;
                    //Handle on Solution Explorer window
                    var slnExplorer = applicationObject.Windows.Item(Constants.vsWindowKindSolutionExplorer);
                    var slnHierarchy = (UIHierarchy)slnExplorer.Object;
                    slnExplorer.Activate();
                    var dbProject = EnvDTEHelper.Instance.Find(project);
                    dbProject.Select(vsUISelectionType.vsUISelectionTypeSelect);
                }
                else
                {
                    var currentProjectFile = new FileInfo(project.FileName);
                    var fullName = StringHelper.EnsureDirectorySeparatorAtEnd(currentProjectFile.Directory.FullName) + relativePathAndName;
                    fullName = fullName.Replace(@"\\", @"\");
                    fileStateInfo.FileName = fullName;
                    newFile = new FileInfo(fullName);
                    if (newFile.Exists && !overwrite)
                    {
                        fileStateInfo.FileState = FileStateConstants.Skipped;
                        return null;
                    }
                    else if (newFile.Exists && (newFile.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        fileStateInfo.FileState = FileStateConstants.Failed;
                        return null;
                    }
                    if (!newFile.Directory.Exists)
                        newFile.Directory.Create();

                    File.Copy(fileName, newFile.FullName);
                }

                try
                {
                    fileStateInfo.FileState = FileStateConstants.Success;
                    return project.ProjectItems.AddFromFile(newFile.FullName);
                }
                catch (Exception ex)
                {
                    fileStateInfo.FileState = FileStateConstants.Failed;
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void DeleteProjectItem(Project project, string relativePathAndName, bool deleteFile, out FileStateInfo fileStateInfo)
        {
            fileStateInfo = new FileStateInfo();
            fileStateInfo.FileState = FileStateConstants.Failed;
            var currentProjectFile = new FileInfo(project.FileName);
            relativePathAndName = relativePathAndName.TrimStart(new char[] { '\\' });
            var fullName = Path.Combine(currentProjectFile.Directory.FullName, relativePathAndName);
            fullName = fullName.Replace(@"\\", @"\");
            fileStateInfo.FileName = fullName;
            var newFile = new FileInfo(fullName);
            fileStateInfo.FileState = FileStateConstants.Failed;
            if (newFile.Exists)
            {
                var p = FindProjectItemByFileName(project.ProjectItems, fullName);
                if (p != null) p.Remove();
                if (deleteFile)
                {
                    try
                    {
                        File.Delete(fullName);
                    }
                    catch { }
                }
            }
        }

        public static void SetProperties(ProjectItem projectItem, Hashtable properties)
        {
            if (properties == null) return;
            foreach (DictionaryEntry de in properties)
            {
                EnvDTEHelper.SetProperty(projectItem, (string)de.Key, de.Value);
            }
        }

        public static void SetProperty(ProjectItem projectItem, string propertyName, object propertyValue)
        {
            if (projectItem == null) return;
            if (propertyValue == null) return;
            foreach (Property property in projectItem.Properties)
            {
                if (StringHelper.Match(property.Name, propertyName))
                {
                    property.Value = propertyValue;
                }
            }
        }

        public void DeleteProjectItem(ProjectItem parent, string fileName, out FileStateInfo fileStateInfo)
        {
            fileStateInfo = new FileStateInfo();
            var fi = new FileInfo(this.GetFileName(parent));
            var fullName = Path.Combine(fi.DirectoryName, fileName);
            fullName = fullName.Replace(@"\\", @"\");
            fileStateInfo.FileName = fullName;
            var p = parent.ProjectItems.Item(0);
            //var newItem = parent.ProjectItems.AddFromFile(fullName);
            fileStateInfo.FileState = FileStateConstants.Success;
        }

        public ProjectItem GetProjectItem(string projectName, string parentRelativeName, ProjectItemType parentItemType)
        {
            var relativeFolder = string.Empty;
            var parentFileName = string.Empty;
            if (parentItemType == ProjectItemType.File)
            {
                var folders = parentRelativeName.Split(new char[] { '\\' });
                for (var ii = 0; ii < folders.Length - 1; ii++)
                {
                    relativeFolder = relativeFolder + @"\" + folders[ii];
                }
                parentFileName = folders[folders.Length - 1];
            }
            else
            {
                relativeFolder = parentRelativeName;
            }

            if (relativeFolder != string.Empty && relativeFolder != "\\")
            {
                var folder = this.GetProjectItem(GetProject(projectName), relativeFolder);
                if (parentItemType == ProjectItemType.File)
                {
                    foreach (ProjectItem subItem in folder.ProjectItems)
                    {
                        if (subItem.Kind == Constants.vsProjectItemKindPhysicalFile && StringHelper.Match(subItem.Name, parentFileName, true))
                        {
                            return subItem;
                        }
                    }
                    return null;
                }
                return folder;
            }
            else
            {
                var projectToAddTo = this.GetProject(projectName);
                foreach (ProjectItem projectItem in projectToAddTo.ProjectItems)
                {
                    if (projectItem.Kind == Constants.vsProjectItemKindPhysicalFile && StringHelper.Match(projectItem.Name, parentFileName, true))
                    {
                        return projectItem;
                    }
                }
                return null;
            }
        }

        public FileInfo[] Find(string fileExtension)
        {
            var fileInfos = new ArrayList();
            foreach (UIHierarchyItem hierarchyItem in SolutionHierarchy.UIHierarchyItems.Item(1).UIHierarchyItems)
            {
                if (hierarchyItem.Name == "Solution Items")
                {
                    foreach (UIHierarchyItem subHierarchyItem in hierarchyItem.UIHierarchyItems)
                    {
                        if (subHierarchyItem.Name.EndsWith(".xml"))
                        {
                            var fi = new FileInfo(StringHelper.EnsureDirectorySeparatorAtEnd(this.SolutionDirectory.FullName) + "doc/" + subHierarchyItem.Name);
                            if (fi.Extension == fileExtension)
                            {
                                fileInfos.Add(fi);
                            }
                        }
                    }
                }
            }
            return (FileInfo[])fileInfos.ToArray(typeof(FileInfo));
        }

        public UIHierarchyItem Find(Project project)
        {
            UIHierarchyItem retVal = null;
            foreach (UIHierarchyItem hierarchyItem in SolutionHierarchy.UIHierarchyItems)
            {
                if (hierarchyItem.Object == project)
                    retVal = hierarchyItem;
                else
                    retVal = Find(hierarchyItem, project);
                if (retVal != null)
                    break;
            }
            return retVal;
        }

        private UIHierarchyItem Find(UIHierarchyItem hierarchyItem, Project project)
        {
            UIHierarchyItem retVal = null;
            foreach (UIHierarchyItem childItem in hierarchyItem.UIHierarchyItems)
            {
                if (childItem.Name == project.Name)
                {
                    if (ReflectionHelper.ImplementsInterface(hierarchyItem.Object, typeof(Solution)))
                        retVal = childItem;
                }
                else
                    retVal = Find(childItem, project);
                if (retVal != null)
                    break;
            }
            return retVal;
        }

        public static ProjectItem FindProjectItemByFileName(ProjectItems projectItems, string relativePathAndName)
        {
            for (var ii = 1; ii <= projectItems.Count; ii++)
            {
                var projectItem = projectItems.Item(ii);
                if (projectItem != null)
                {
                    if (projectItem.Kind == Constants.vsProjectItemKindPhysicalFolder)
                    {
                        var p = FindProjectItemByFileName(projectItem.ProjectItems, relativePathAndName);
                        if (p != null) return p;
                    }
                    else
                    {
                        var fileName = projectItem.FileNames[0];
                        if (fileName.ToLower() == relativePathAndName.ToLower())
                        {
                            return projectItem;
                        }
                    }
                }
            }
            return null;
        }

        public UIHierarchyItem Find(ProjectItem pi)
        {
            UIHierarchyItem retVal = null;
            foreach (UIHierarchyItem hierarchyItem in SolutionHierarchy.UIHierarchyItems)
            {
                if (hierarchyItem.Object == pi)
                    retVal = hierarchyItem;
                else
                    retVal = Find(hierarchyItem, pi);
                if (retVal != null)
                    break;
            }
            return retVal;
        }

        private UIHierarchyItem Find(UIHierarchyItem hierarchyItem, ProjectItem pi)
        {
            UIHierarchyItem retVal = null;
            foreach (UIHierarchyItem childItem in hierarchyItem.UIHierarchyItems)
            {
                if (childItem.Object == pi)
                    retVal = hierarchyItem;
                else
                    retVal = Find(childItem, pi);
                if (retVal != null)
                    break;
            }
            return retVal;
        }

        public UIHierarchy SolutionHierarchy
        {
            get
            {
                var solutionWindow = (Window)_applicationObject.Windows.Item(Constants.vsWindowKindSolutionExplorer);
                var solutionHierarchy = (UIHierarchy)solutionWindow.Object;
                return solutionHierarchy;
            }
        }

        public void ActivateSolutionHierarchy()
        {
            var solutionWindow = (Window)_applicationObject.Windows.Item(Constants.vsWindowKindSolutionExplorer);
            solutionWindow.Activate();
        }

        public string GetFileName(ProjectItem pi)
        {
            try
            {
                if (!_projectItemFileNameCache.ContainsKey(pi))
                {
                    _projectItemFileNameCache.Add(pi, pi.get_FileNames(1));
                }
                return _projectItemFileNameCache[pi];
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ProjectItem CurrentProjectItem
        {
            get
            {
                var selectedItems = _applicationObject.SelectedItems;
                var selectedItem = selectedItems.Item(1);
                if (selectedItem != null)
                {
                    return selectedItem.ProjectItem;
                }
                return null;
            }
        }

        public EnvDTE.Project CreateSolutionFolder(string relativePath)
        {
            return CreateSolutionFolder(relativePath, null);
        }

        public EnvDTE.Project CreateSolutionFolder(string relativePath, EnvDTE.Project parentFolder)
        {
            if (string.IsNullOrEmpty(relativePath)) return null;

            var arr = relativePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (parentFolder == null)
            {
                var folders = CurrentSolution.GetFolders();
                var selected = folders.FirstOrDefault(x => x.Name.ToLower() == arr.First().ToLower());
                if (selected == null)
                    selected = (CurrentSolution as EnvDTE80.Solution2).AddSolutionFolder(arr.First());

                if (arr.Length == 1)
                    return selected;
                else
                    return CreateSolutionFolder(string.Join(@"\", arr.Skip(1).Take(arr.Length - 1)), selected);
            }
            else
            {
                var folders = parentFolder.GetFolders();
                var selected = folders.FirstOrDefault(x => x.Name.ToLower() == arr.First().ToLower());
                if (selected == null)
                    selected = (parentFolder.Object as EnvDTE80.SolutionFolder).AddSolutionFolder(arr.First());

                if (arr.Length == 1)
                    return selected;
                else
                    return CreateSolutionFolder(string.Join(@"\", arr.Skip(1).Take(arr.Length - 1)), selected);
            }
        }

        public Project CreateProjectFromTemplate(string template, string projectName, string outputTarget)
        {
            try
            {
                var currentSolutionDirectory = (new FileInfo(CurrentSolution.FullName)).Directory;
                var targetRelativeFolder = Path.Combine(outputTarget, projectName);
                var targetFolder = Path.Combine(currentSolutionDirectory.FullName, targetRelativeFolder);
                var targetDirectory = (new DirectoryInfo(targetFolder));
                var projectFullName = Path.Combine(targetDirectory.FullName, projectName + ".csproj");

                if (targetDirectory.Exists && File.Exists(projectFullName))
                {
                    //If the project already exists then add it
                    Project retval = null;
                    try
                    {
                        //Get it if exists
                        retval = CurrentSolution.GetProjects().FirstOrDefault(x => x.Name == projectName);

                        //If cannot add then do not show error...might be in solution folder
                        retval = CurrentSolution.AddFromFile(projectFullName, false);
                    }
                    catch (Exception ex)
                    {
                        //Do NOthing
                    }
                    return retval;
                }

                //If not exists then create it
                if (string.IsNullOrEmpty(outputTarget))
                {
                    return CurrentSolution.AddFromTemplate(template, targetFolder, projectName);
                }
                else
                {
                    var folder = CreateSolutionFolder(outputTarget);
                    if (!targetDirectory.Exists)
                        Directory.CreateDirectory(targetFolder);
                    return (folder.Object as EnvDTE80.SolutionFolder).AddFromTemplate(template, targetFolder, projectName);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DirectoryInfo SolutionDirectory
        {
            get
            {
                try
                {
                    var fi = new FileInfo(CurrentSolution.FullName);
                    return fi.Directory;
                }
                catch (Exception ex)
                {
                    throw new Exception("Invalid file: '" + CurrentSolution.FullName + "'", ex);
                }
            }
        }

        private EnvDTE80.Solution2 CurrentSolution => _applicationObject.Solution as EnvDTE80.Solution2;

        public static bool ProjectExists(string projectName)
        {
            return (EnvDTEHelper.Instance.GetProject(projectName) != null);
        }

        public static bool ProjectLoaded(string projectName)
        {
            var p = EnvDTEHelper.Instance.GetProject(projectName);
            return ProjectExists(projectName) && (p != null && p.Object != null);
        }

        public Project GetProject(string projectName)
        {
            foreach (var proj in CurrentSolution.GetProjects())
            {
                try
                {
                    if (proj.Name.Match(projectName))
                        return proj;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }

            return null;
        }

        public ProjectItem GetProjectItem(ProjectItem parentItem, string projectItem, bool createPathIfNotExists)
        {
            if (projectItem != string.Empty)
            {
                ProjectItem currentProjectItem = null;
                foreach (ProjectItem pi in parentItem.ProjectItems)
                {
                    if (pi.Name == projectItem)
                    {
                        currentProjectItem = pi;
                    }
                }

                if (currentProjectItem == null && createPathIfNotExists)
                {
                    currentProjectItem = AddFolder(parentItem, projectItem);
                }
                return currentProjectItem;
            }
            else
            {
                return parentItem;
            }
        }

        public ProjectItem GetProjectItem(Project project, string projectItemString)
        {
            return GetProjectItem(project, projectItemString, true);
        }

        private ProjectItem GetProjectItem(Project project, string projectItemString, bool createPathIfNotExists)
        {
            try
            {
                ProjectItem currentProjectItem = null;
                var currentFolder = string.Empty;
                projectItemString = projectItemString.TrimStart(new char[] { '\\' });
                var folders = projectItemString.Split(new char[] { '\\' });
                var folder = folders.FirstOrDefault();
                if (folder != null)
                {
                    foreach (ProjectItem pi in project.ProjectItems)
                    {
                        if (pi.Name == folder)
                        {
                            currentProjectItem = pi;
                        }
                    }

                    if (currentProjectItem == null & createPathIfNotExists)
                    {
                        currentProjectItem = AddFolder(project, folder);
                    }
                }

                for (var ii = 1; ii < folders.Length; ii++)
                {
                    if (currentProjectItem != null)
                        currentProjectItem = GetProjectItem(currentProjectItem, folders[ii], createPathIfNotExists);
                }

                return currentProjectItem;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool GetProjectItemExists(string projectName, string parentRelativeName, ProjectItemType parentItemType)
        {
            var relativeFolder = string.Empty;
            var parentFileName = string.Empty;
            if (parentItemType == ProjectItemType.File)
            {
                var folders = parentRelativeName.Split(new char[] { '\\' });
                for (var ii = 0; ii < folders.Length - 1; ii++)
                {
                    relativeFolder = relativeFolder + @"\" + folders[ii];
                }
                parentFileName = folders[folders.Length - 1];
            }
            else
            {
                relativeFolder = parentRelativeName;
            }
            if (relativeFolder != string.Empty && relativeFolder != "\\")
            {
                var folder = this.GetProjectItem(GetProject(projectName), relativeFolder, false);
                if (parentItemType == ProjectItemType.File)
                {
                    foreach (ProjectItem subItem in folder.ProjectItems)
                    {
                        if (subItem.Kind == Constants.vsProjectItemKindPhysicalFile && StringHelper.Match(subItem.Name, parentFileName))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return (folder != null);
            }
            else
            {
                var projectToAddTo = this.GetProject(projectName);
                foreach (ProjectItem projectItem in projectToAddTo.ProjectItems)
                {
                    if (projectItem.Kind == Constants.vsProjectItemKindPhysicalFile && projectItem.Name == parentFileName)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public string Version => _applicationObject.Version;
    }
}
