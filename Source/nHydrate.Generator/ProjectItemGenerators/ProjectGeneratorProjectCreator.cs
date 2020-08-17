using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.ProjectItemGenerators;
using nHydrate.Generator.GeneratorFramework;
using nHydrate.Generator.Util;
using System;
using System.IO;

namespace nHydrate.Generator.ProjectItemGenerators
{

    public partial class ProjectGeneratorProjectCreator : IProjectGeneratorProjectCreator
    {
        void IProjectGeneratorProjectCreator.CreateProject(IProjectGenerator projectGenerator)
        {
            try
            {
                //If there is no project defined then do nothing
                if (string.IsNullOrEmpty(projectGenerator.ProjectTemplate))
                    return;

                var newProject = EnvDTEHelper.Instance.GetProject(projectGenerator.ProjectName);
                if (newProject != null)
                    newProject.Delete();

                var templateFullName = Path.Combine(AddinAppData.Instance.ExtensionDirectory, projectGenerator.ProjectTemplate);

                //Copy the template and project file to a temp folder and perform replacements
                var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempPath);

                //Copy template
                var fi = new FileInfo(templateFullName);
                var targetFile = Path.Combine(tempPath, fi.Name);
                File.Copy(templateFullName, targetFile, true);

                //Copy project
                var sourceFile = templateFullName.Replace(".vstemplate", ".csproj");
                fi = new FileInfo(sourceFile);
                if (File.Exists(sourceFile))
                {
                    targetFile = Path.Combine(tempPath, fi.Name);
                    File.Copy(sourceFile, targetFile, true);
                    fi = new FileInfo(targetFile);
                    projectGenerator.GenerateCompanySpecificFile(tempPath, fi.Name);
                }

                //Copy the assembly file over
                sourceFile = Path.Combine(AddinAppData.Instance.ExtensionDirectory, "AssemblyInfo.cs");
                if (File.Exists(sourceFile))
                {
                    var propertyPath = Path.Combine(tempPath, "Properties");
                    Directory.CreateDirectory(propertyPath);
                    var t = Path.Combine(propertyPath, "AssemblyInfo.cs");
                    File.Copy(sourceFile, t, true);
                    fi = new FileInfo(t);
                    projectGenerator.GenerateCompanySpecificFile(propertyPath, fi.Name);
                }

                newProject = EnvDTEHelper.Instance.CreateProjectFromTemplate(targetFile, projectGenerator.ProjectName, projectGenerator.Model.OutputTarget);
                Directory.Delete(tempPath, true);
                projectGenerator.OnAfterGenerate();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
