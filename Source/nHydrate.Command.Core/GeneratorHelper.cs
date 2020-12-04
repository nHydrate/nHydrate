using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.ProjectItemGenerators;
using nHydrate.Generator.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace nHydrate.Command.Core
{
    internal class GeneratorHelper : nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper
    {
        private readonly string _outputFolder;

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
            if (name.StartsWith(Path.DirectorySeparatorChar)) name = name.Substring(1, name.Length - 1);
            e.FullName = System.IO.Path.Combine(_outputFolder, e.ProjectName, name);
            if (File.Exists(e.FullName)) File.Delete(e.FullName);
            e.FileState = Generator.Common.Util.FileStateConstants.Success;
        }

        protected override void projectItemGenerator_ProjectItemExists(object sender, ProjectItemExistsEventArgs e)
        {
            var name = e.ProjectItemName;
            if (name.StartsWith(Path.DirectorySeparatorChar)) name = name.Substring(1, name.Length - 1);
            var fileName = System.IO.Path.Combine(_outputFolder, e.ProjectName, name);
            e.Exists = File.Exists(fileName);
        }

        protected override void projectItemGenerator_ProjectItemGenerated(object sender, ProjectItemGeneratedEventArgs e)
        {
            var name = e.ProjectItemName;
            if (name.StartsWith(Path.DirectorySeparatorChar)) name = name.Substring(1, name.Length - 1);
            var pathsRelative = new List<string>();
            var paths = new List<string>();

            paths.AddRange(_outputFolder.Split(Path.DirectorySeparatorChar));
            paths.AddRange(e.ProjectName.Split(Path.DirectorySeparatorChar));
            if (!e.ParentItemName.IsEmpty() && !e.ParentItemName.Contains("."))
            {
                paths.AddRange(e.ParentItemName.Split(Path.DirectorySeparatorChar));
                pathsRelative.AddRange(e.ParentItemName.Split(Path.DirectorySeparatorChar));
            }
            else if (!e.ParentItemName.IsEmpty() && e.ParentItemName.Contains("."))
            {
                var arr = e.ParentItemName.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (arr.Count > 1)
                {
                    arr.RemoveAt(arr.Count - 1);
                    paths.AddRange(arr);
                    pathsRelative.AddRange(arr);
                }
            }
            paths.AddRange(name.Split(Path.DirectorySeparatorChar));
            pathsRelative.AddRange(name.Split(Path.DirectorySeparatorChar));
            var fileName = System.IO.Path.Combine(paths.ToArray());

            var fileStateInfo = new Generator.Common.Util.FileStateInfo { FileName = fileName };
            if (!File.Exists(fileName) || e.Overwrite)
            {
                var folderName = new FileInfo(fileName).DirectoryName;
                //In case this is a parent nested file, do not try to create
                if (!File.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                    File.WriteAllText(fileName, e.ProjectItemContent);
                }
                else
                {
                    fileName = System.IO.Path.Combine(_outputFolder, e.ProjectName, name);
                    File.WriteAllText(fileName, e.ProjectItemContent);
                }
            }
            else
                fileStateInfo.FileState = Generator.Common.Util.FileStateConstants.Skipped;

            //Embed the file in the project if need be
            if (e.Properties.ContainsKey("BuildAction") && (int)e.Properties["BuildAction"] == 3)
            {
                var projectFileName = System.IO.Path.Combine(_outputFolder, e.ProjectName, $"{e.ProjectName}.csproj");
                if (File.Exists(projectFileName))
                {
                    var includeFile = System.IO.Path.Combine(pathsRelative.ToArray());

                    var document = new XmlDocument();
                    document.PreserveWhitespace = true;
                    document.Load(projectFileName);
                    var groups = document.DocumentElement.SelectNodes("ItemGroup");

                    //Ensure file is not already embedded
                    if (document.DocumentElement.SelectSingleNode($"ItemGroup/EmbeddedResource[@Include='{includeFile}']") == null)
                    {
                        var whiteSpace = "  ";
                        XmlNode targetGroup = null;
                        foreach (XmlElement g in groups)
                        {
                            //Find the first "ItemGroup" with a "EmbeddedResource" item
                            //This will be the one to which we add the new embedded file
                            if (g.SelectSingleNode("EmbeddedResource") != null)
                            {
                                if (g.FirstChild.NodeType == XmlNodeType.Whitespace)
                                {
                                    if (g.FirstChild.Value.Replace("\r", "").Replace("\n", "").Contains('\t'))
                                        whiteSpace = "\t";
                                }
                                targetGroup = g;
                                break;
                            }
                        }

                        //If there is no group create a new one
                        if (targetGroup == null)
                            targetGroup = document.DocumentElement.AppendChild(document.CreateElement("ItemGroup"));

                        //Add whitespace and the item
                        targetGroup.AppendChild(document.CreateSignificantWhitespace(whiteSpace));
                        var node = targetGroup.AppendChild(document.CreateElement("EmbeddedResource"));
                        var attr = node.Attributes.Append(document.CreateAttribute("Include"));
                        attr.Value = includeFile;
                        targetGroup.AppendChild(document.CreateSignificantWhitespace("\r\n" + whiteSpace));

                        document.Save(projectFileName);
                    }
                }
            }

            //Write Log
            Generator.Common.Logging.nHydrateLog.LogInfo("Project Item Generated: {0}", e.ProjectItemName);
            e.FileState = fileStateInfo.FileState;
            e.FullName = fileStateInfo.FileName;
            this.OnProjectItemGenerated(sender, e);
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
