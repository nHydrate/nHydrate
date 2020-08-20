using EnvDTE;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Logging;
using nHydrate.Generator.Common.ProjectItemGenerators;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Util;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace nHydrate.Generator.GeneratorFramework
{
    public class GeneratorHelper : nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper
    {
        protected override string GetExtensionsFolder()
        {
            return AddinAppData.Instance.ExtensionDirectory;
        }

        #region Generation Methods

        #region Public Count Methods

        public override int GetFileCount(IGenerator generator, List<Type> excludeList)
        {
            EnvDTEHelper.Instance.ClearCache();
            return base.GetFileCount(generator, excludeList);
        }

        #endregion

        #region Public Generate Methods

        public override void GenerateAll(IGenerator generator, List<Type> excludeList)
        {
            EnvDTEHelper.Instance.ClearCache();
            base.GenerateAll(generator, excludeList);
        }

        protected override void GenerateProject(IGenerator generator, Type projectGeneratorType)
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

        protected override IProjectGeneratorProjectCreator GetProjectGeneratorProjectCreator()
        {
            return new Generator.ProjectItemGenerators.ProjectGeneratorProjectCreator();
        }

        #endregion

        protected override void LogError(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        #region Private Helpers

        private static Dictionary<string, Project> projectCache = new Dictionary<string, Project>();
        private int _doEventCount = 0;
        protected override void projectItemGenerator_ProjectItemGenerated(object sender, ProjectItemGeneratedEventArgs e)
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

                    if (fileStateInfo.FileState == FileStateConstants.Success)
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

                    if (fileStateInfo.FileState == FileStateConstants.Success)
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
                if (fileStateInfo.FileState == FileStateConstants.Failed)
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

        protected override void projectItemGenerator_ProjectItemDeleted(object sender, ProjectItemDeletedEventArgs e)
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

        protected override void projectItemGenerator_ProjectItemGenerationError(object sender, ProjectItemGeneratedErrorEventArgs e)
        {
            if (e.ShowError)
            {
                MessageBox.Show(e.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void projectItemGenerator_ProjectItemExists(object sender, ProjectItemExistsEventArgs e)
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

        #endregion

        #endregion
    }
}
