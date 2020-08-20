using System;
using System.IO;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.ProjectItemGenerators;

namespace nHydrate.Command.Core
{
    internal class GeneratorHelper : nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper
    {
        private string _outputFolder;

        public GeneratorHelper(string outputFolder)
        {
            _outputFolder = outputFolder;
        }

        protected override void GenerateProject(IGenerator generator, Type projectGeneratorType)
        {
            try
            {
                var projectGenerator = GetProjectGenerator(projectGeneratorType);
                projectGenerator.Initialize(generator.Model);

                //TODO
                //if (DOES NOT EXIST: projectGenerator.ProjectName)
                //{
                //    //TODO: CreateProject
                //}

                GenerateProjectItems(projectGenerator);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override string GetExtensionsFolder()
        {
            return System.IO.Directory.GetCurrentDirectory();
        }

        protected override IProjectGeneratorProjectCreator GetProjectGeneratorProjectCreator()
        {
            return new ProjectGeneratorProjectCreator();
        }

        protected override void LogError(string message)
        {
            //TODO: log to file
        }

        protected override void projectItemGenerator_ProjectItemDeleted(object sender, ProjectItemDeletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override void projectItemGenerator_ProjectItemExists(object sender, ProjectItemExistsEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override void projectItemGenerator_ProjectItemGenerated(object sender, ProjectItemGeneratedEventArgs e)
        {
            var name = e.ProjectItemName;
            if (name.StartsWith(@"\"))
                name = name.Substring(1, name.Length - 1);
            var fileName = System.IO.Path.Combine(_outputFolder, e.ProjectName, name);

            if (!File.Exists(fileName) || e.Overwrite)
            {
                File.WriteAllText(fileName, e.ProjectItemContent);
            }
        }

        protected override void projectItemGenerator_ProjectItemGenerationError(object sender, ProjectItemGeneratedErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }

    internal class ProjectGeneratorProjectCreator : IProjectGeneratorProjectCreator
    {
        public void CreateProject(IProjectGenerator projectGenerator)
        {
            throw new NotImplementedException();
        }
    }
}
