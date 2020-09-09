using System;
using System.IO;
using System.Text;
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
                CreateProject(generator, projectGeneratorType, _outputFolder);
                GenerateProjectItems(projectGenerator);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override string GetExtensionsFolder()
        {
            return new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
        }

        protected override IProjectGeneratorProjectCreator GetProjectGeneratorProjectCreator(string outputFolder)
        {
            return new ProjectGeneratorProjectCreator(outputFolder);
        }

        protected override void LogError(string message)
        {
            //TODO: log to file
        }

        protected override void projectItemGenerator_ProjectItemDeleted(object sender, ProjectItemDeletedEventArgs e)
        {
            var name = e.ProjectItemName;
            if (name.StartsWith(@"\")) name = name.Substring(1, name.Length - 1);
            var fileName = System.IO.Path.Combine(_outputFolder, e.ProjectName, name);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            e.FileState = Generator.Common.Util.FileStateConstants.Success;
            e.FullName = fileName;
        }

        protected override void projectItemGenerator_ProjectItemExists(object sender, ProjectItemExistsEventArgs e)
        {
            var name = e.ProjectItemName;
            if (name.StartsWith(@"\")) name = name.Substring(1, name.Length - 1);
            var fileName = System.IO.Path.Combine(_outputFolder, e.ProjectName, name);
            e.Exists = File.Exists(fileName);
        }

        protected override void projectItemGenerator_ProjectItemGenerated(object sender, ProjectItemGeneratedEventArgs e)
        {
            var name = e.ProjectItemName;
            if (name.StartsWith(@"\")) name = name.Substring(1, name.Length - 1);
            var fileName = System.IO.Path.Combine(_outputFolder, e.ProjectName, name);

            if (!File.Exists(fileName) || e.Overwrite)
            {
                File.WriteAllText(fileName, e.ProjectItemContent);
            }
        }

        protected override void projectItemGenerator_ProjectItemGenerationError(object sender, ProjectItemGeneratedErrorEventArgs e)
        {
            if (e.ShowError)
                LogError(e.Text);
        }
    }

    internal class ProjectGeneratorProjectCreator : IProjectGeneratorProjectCreator
    {
        private string _outputFolder = "";

        public ProjectGeneratorProjectCreator(string outputFolder)
        {
            _outputFolder = outputFolder;
        }

        public void CreateProject(IProjectGenerator projectGenerator)
        {
            var folder = Path.Combine(_outputFolder, projectGenerator.ProjectName);
            var csProjFile = Path.Combine(folder, projectGenerator.ProjectName + ".csproj");
            
            //Do not overgen the project file
            if (File.Exists(csProjFile))
                return;

            //If the project file does not exist then create it
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var projectFileName = projectGenerator.ProjectTemplate.Replace(".vstemplate", ".csproj");
            var embedPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + $".Resources.{projectFileName}";
            var content = GetResource(embedPath);

            //Copy the template and project file to a temp folder and perform replacements
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            File.WriteAllText(csProjFile, content);

            projectGenerator.OnAfterGenerate();
        }

        private string GetResource(string name)
        {

            var retVal = string.Empty;
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var manifestStream = asm.GetManifestResourceStream(name);
            try
            {
                using (var sr = new System.IO.StreamReader(manifestStream))
                {
                    retVal = sr.ReadToEnd();
                }
            }
            catch
            {
            }
            finally
            {
                manifestStream.Close();
            }
            return retVal;
        }
    }
}
