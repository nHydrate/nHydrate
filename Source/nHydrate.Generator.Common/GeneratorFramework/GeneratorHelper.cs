using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.Logging;
using nHydrate.Generator.Common.ProjectItemGenerators;
using nHydrate.Generator.Common.Util;
using System;
using System.Collections.Generic;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public abstract class GeneratorHelper
    {
        #region Class Members

        protected List<string> _errorList = new List<string>();

        #endregion

        #region Events

        public event ProjectItemGeneratedEventHandler ProjectItemGenerated;
        protected event ProjectItemDeletedEventHandler ProjectItemDeleted;
        protected event ProjectItemGenerationCompleteEventHandler GenerationComplete;
        protected event ProjectItemGeneratedEventHandler ProjectItemGeneratedError;

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

        protected abstract string GetExtensionsFolder();

        protected abstract void LogError(string message);

        #region Public Count Methods

        public virtual int GetFileCount(IGenerator generator, List<Type> excludeList)
        {
            var retval = 0;
            try
            {
                var globalCacheFile = new GlobalCacheFile();
                _generator = generator;
                var projectGenerators = GetProjectGenerators(generator);
                var generatorTypes = ReflectionHelper.GetCreatableObjectImplementsInterface(typeof(IProjectItemGenerator), GetExtensionsFolder());
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
                        LogError(ex.ToString());
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

        protected IGenerator _generator;
        public virtual void GenerateAll(IGenerator generator, List<Type> excludeList)
        {
            try
            {
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
                        LogError(ex.ToString());
                    }
                }

                System.Diagnostics.Debug.Write(string.Empty);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        protected abstract void GenerateProject(IGenerator generator, Type projectGeneratorType);

        protected abstract IProjectGeneratorProjectCreator GetProjectGeneratorProjectCreator(string outputFolder);

        protected void CreateProject(IGenerator generator, Type projectGeneratorType, string outputFolder)
        {
            try
            {
                var projectGenerator = GetProjectGenerator(projectGeneratorType);

                //For the VSIX modeler, this is the object that will create project in Visual Studio
                (projectGenerator as IProjectGenerator).ProjectGeneratorProjectCreator = GetProjectGeneratorProjectCreator(outputFolder);

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

        protected IProjectItemGenerator GetProjectItemGenerator(Type projectItemGeneratorType)
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

        protected void GenerateProjectItems(IProjectGenerator projectGenerator)
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

        protected void GenerateSubItems(IProjectItemGenerator projectItemGenerator)
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

        protected abstract void projectItemGenerator_ProjectItemGenerated(object sender, ProjectItemGeneratedEventArgs e);
        protected abstract void projectItemGenerator_ProjectItemDeleted(object sender, ProjectItemDeletedEventArgs e);
        protected abstract void projectItemGenerator_ProjectItemGenerationError(object sender, ProjectItemGeneratedErrorEventArgs e);
        protected abstract void projectItemGenerator_ProjectItemExists(object sender, ProjectItemExistsEventArgs e);

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

        protected List<Type> GetProjectItemGenerators(IProjectGenerator projectGenerator)
        {
            return GetGeneratorsImpl(projectGenerator);
        }

        private List<Type> GetProjectItemGenerators(IProjectItemGenerator projectItemGenerator)
        {
            return GetGeneratorsImpl(projectItemGenerator);
        }
        #endregion

        #region Private Helpers

        protected readonly List<string> processedFiles = new List<string>();

        /// <summary>
        /// Gets the errors that have occured since object instantiation
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetErrorList()
        {
            return _errorList;
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
                    generatorTypes = ReflectionHelper.GetCreatableObjectImplementsInterface(typeof(IProjectGenerator), GetExtensionsFolder());
                }
                else
                {
                    generatorTypes = ReflectionHelper.GetCreatableObjectImplementsInterface(typeof(IProjectItemGenerator), GetExtensionsFolder());
                }

                if (generatorTypes.Length == 0)
                {
                    LogError($"There are no generators installed or there was an error loading the installed generators from the following path. '{GetExtensionsFolder()}'");
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
    }
}
