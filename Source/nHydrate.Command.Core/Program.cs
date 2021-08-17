using Microsoft.Extensions.Configuration;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Util;
using nHydrate.ModelManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace nHydrate.Command.Core
{
    class Program
    {
        private const string ModelKey = "model";
        private const string OutputKey = "output";
        private const string GeneratorsKey = "generators";
        private static GenStats _stats = new GenStats();

        /*
            --model=C:\code\nHydrateTestAug\ConsoleApp1\Model1.nhydrate --output=C:\code\nHydrateTestAug --generators=nHydrate.Generator.EFCodeFirstNetCore.EFCodeFirstNetCoreProjectGenerator,nHydrate.Generator.PostgresInstaller.PostgresDatabaseProjectGenerator,nHydrate.Generator.SQLInstaller.Core.DatabaseProjectGenerator
        */

        static int Main(string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            var modelFile = string.Empty;
            var output = string.Empty;
            var generators = new string[0];

            //AppSettings
            var allValues = Configuration.GetChildren().Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key.ToString(), x => x.Value?.ToString());
            if (allValues.ContainsKey(ModelKey))
                modelFile = allValues[ModelKey];
            if (allValues.ContainsKey(OutputKey))
                output = allValues[OutputKey];
            if (allValues.ContainsKey(GeneratorsKey))
                generators = allValues[GeneratorsKey].Split(",", StringSplitOptions.RemoveEmptyEntries);

            if (modelFile.IsEmpty())
                return ShowError("The model is required.");
            if (output.IsEmpty())
                return ShowError("The output folder is required.");

            //If there are no generators specified on the command line then check for the file "nhydrate.generators"
            if (!generators.Any())
            {
                var folderName = (new FileInfo(modelFile)).DirectoryName;
                var genDefFile = Path.Combine(folderName, "nhydrate.generators");
                if (File.Exists(genDefFile))
                    generators = File.ReadAllLines(genDefFile).Where(x => x.Trim() != string.Empty).ToArray();
                if (!generators.Any())
                    return ShowError("The generators are required.");
            }

            Console.WriteLine($"modelFile='{modelFile}'");
            Console.WriteLine($"output='{output}'");
            Console.WriteLine($"generators='{string.Join(",", generators)}'");

            //NOTE: Yaml Model files must end with ".nhydrate.yaml"
            //Old Xml file ends with ".nhydrate"

            //Specified a folder so look for the file
            string actualFile = null;
            if (Directory.Exists(modelFile))
            {
                var folderName = modelFile;

                //Look for new Yaml file
                var f = Directory.GetFiles(folderName, "*" + FileManagement.ModelExtension).FirstOrDefault();
                if (File.Exists(f)) actualFile = f;

                //Look for old xml file
                if (actualFile.IsEmpty())
                {
                    f = Directory.GetFiles(folderName, "*" + FileManagement.OldModelExtension).FirstOrDefault();
                    if (File.Exists(f)) actualFile = f;
                }

                if (actualFile.IsEmpty())
                {
                    //Back 1 folder
                    folderName = (new DirectoryInfo(folderName)).Parent.FullName;

                    f = Directory.GetFiles(folderName, "*" + FileManagement.ModelExtension).FirstOrDefault();
                    if (File.Exists(f)) actualFile = f;

                    //Look for old xml file
                    if (actualFile.IsEmpty())
                    {
                        f = Directory.GetFiles(folderName, "*" + FileManagement.OldModelExtension).FirstOrDefault();
                        if (File.Exists(f)) actualFile = f;
                    }
                }
            }
            else
            {
                //Is this the Yaml model?
                if (modelFile.EndsWith(FileManagement.ModelExtension))
                    actualFile = modelFile;

                //Is this the Xml model?
                if (modelFile.EndsWith(FileManagement.OldModelExtension))
                    actualFile = modelFile;

                //Look one folder back for Yaml
                if (actualFile.IsEmpty())
                {
                    var folderName = (new FileInfo(modelFile)).Directory.Parent.FullName;
                    var f = Directory.GetFiles(folderName, "*" + FileManagement.ModelExtension).FirstOrDefault();
                    if (File.Exists(f)) actualFile = f;
                }
            }

            if (actualFile.IsEmpty()) return ShowError("Model file not found.");
            modelFile = actualFile;

            var timer = System.Diagnostics.Stopwatch.StartNew();
            var formatModel = (allValues.ContainsKey("formatmodel") && allValues["formatmodel"] == "true");

            //TODO: when model files missing ID, it generates all fields as first one
           
            nHydrate.Generator.Common.Models.ModelRoot model = null;
            try
            {
                Console.WriteLine();
                Console.WriteLine("Loading model...");
                model = ModelHelper.CreatePOCOModel(modelFile, formatModel);
            }
            catch (ModelException ex)
            {
                //All YAML validation errors will come here
                Console.WriteLine(ex.Message);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown error.");
                return 1;
            }

            //Generate
            if (model != null && !formatModel)
            {
                Console.WriteLine("Loading generators...");
                var genHelper = new nHydrate.Command.Core.GeneratorHelper(output);
                genHelper.ProjectItemGenerated += new nHydrate.Generator.Common.GeneratorFramework.ProjectItemGeneratedEventHandler(g_ProjectItemGenerated);

                var genList = new List<nHydrateGeneratorProject>();
                var genProject = new nHydrateGeneratorProject();
                genList.Add(genProject);
                model.ResetKey(model.Key);
                model.GeneratorProject = genProject;
                genProject.Model = model;
                genProject.FileName = $"{modelFile}.generating";
                var document = new System.Xml.XmlDocument();
                document.LoadXml($"<modelRoot guid=\"{model.Key}\" type=\"nHydrate.Generator.nHydrateGeneratorProject\" assembly=\"nHydrate.Generator.dll\"><ModelRoot></ModelRoot></modelRoot>");
                ((nHydrate.Generator.Common.GeneratorFramework.IXMLable)model).XmlAppend(document.DocumentElement.ChildNodes[0]);
                System.IO.File.WriteAllText(genProject.FileName, document.ToIndentedString());

                var allgenerators = genHelper.GetProjectGenerators(genProject);
                var excludeList = allgenerators.Where(x => !generators.Contains(x.FullName)).ToList();

                //Get the last version we generated on this machine
                //We will use this to determine if any other generations have been performed on other machines
                var cacheFile = new nHydrate.Generator.Common.ModelCacheFile(genList.First());
                var cachedGeneratedVersion = cacheFile.GeneratedVersion;
                var generatedVersion = cachedGeneratedVersion + 1;
                model.GeneratedVersion = generatedVersion;

                Console.WriteLine($"Generating code...");
                foreach (var item in genList)
                {
                    genHelper.GenerateAll(item, excludeList);
                }

                //Save local copy of last generated version
                cacheFile.GeneratedVersion = generatedVersion;
                cacheFile.ModelerVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                cacheFile.Save();

                if (File.Exists(genProject.FileName))
                    File.Delete(genProject.FileName);

                //Write stats
                Console.WriteLine();
                Console.WriteLine("Generation Summary");
                Console.WriteLine($"Total Files: {_stats.ProcessedFileCount}");
                Console.WriteLine($"Files Success: {_stats.FilesSuccess}");
                Console.WriteLine($"Files Skipped: {_stats.FilesSkipped}");
                Console.WriteLine($"Files Failed: {_stats.FilesFailed}");
                Console.WriteLine();
            }
            else if (!formatModel)
            {
                Console.WriteLine("The model could not be loaded.");
            }

            timer.Stop();
            Console.WriteLine($"Generation complete. Elapsed={timer.ElapsedMilliseconds}ms");
            return 0;
        }

        private static int ShowError(string message)
        {
            Console.WriteLine(message);
            return 1;
        }

        private static void g_ProjectItemGenerated(object sender, nHydrate.Generator.Common.EventArgs.ProjectItemGeneratedEventArgs e)
        {
            _stats.ProcessedFileCount++;
            if (e.FileState == FileStateConstants.Skipped)
                _stats.FilesSkipped++;
            if (e.FileState == FileStateConstants.Success)
                _stats.FilesSuccess++;
            if (e.FileState == FileStateConstants.Failed)
                _stats.FilesFailed++;

            _stats.GeneratedFileList.Add(e);
            Console.WriteLine($"Generated File: {e.FullName} ({e.FileState})");
        }

        private class GenStats
        {
            public int ProcessedFileCount { get; set; }
            public int FilesSkipped { get; set; }
            public int FilesSuccess { get; set; }
            public int FilesFailed { get; set; }
            public List<nHydrate.Generator.Common.EventArgs.ProjectItemGeneratedEventArgs> GeneratedFileList { get; private set; } = new List<Generator.Common.EventArgs.ProjectItemGeneratedEventArgs>();
        }
    }
}
