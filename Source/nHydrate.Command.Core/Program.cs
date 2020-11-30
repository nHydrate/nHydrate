using Microsoft.Extensions.Configuration;
using nHydrate.Generator.Common;
using nHydrate.ModelManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace nHydrate.Command.Core
{
    class Program
    {
        private const string ModelKey = "model";
        private const string OutputKey = "output";
        private const string GeneratorsKey = "generators";

        /*
            /model=C:\code\nHydrateTestAug\ConsoleApp1\Model1.nhydrate /output=C:\code\nHydrateTestAug /generators=nHydrate.Generator.EFCodeFirstNetCore.EFCodeFirstNetCoreProjectGenerator,nHydrate.Generator.PostgresInstaller.PostgresDatabaseProjectGenerator,nHydrate.Generator.SQLInstaller.Core.DatabaseProjectGenerator
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

            //Command line (overrides above)
            //allValues = Configuration.GetChildren().Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key.ToString(), x => x.Value?.ToString());
            //if (allValues.ContainsKey(ModelKey))
            //    modelFile = allValues[ModelKey];
            //if (allValues.ContainsKey(OutputKey))
            //    output = allValues[OutputKey];
            //if (allValues.ContainsKey(GeneratorsKey))
            //    generators = allValues[GeneratorsKey].Split(",", StringSplitOptions.RemoveEmptyEntries);

            if (string.IsNullOrEmpty(modelFile))
                return ShowError("The model is required.");
            if (string.IsNullOrEmpty(output))
                return ShowError("The output folder is required.");
            if (!generators.Any())
                return ShowError("The generators are required.");

            Console.WriteLine($"modelFile='{modelFile}'");
            Console.WriteLine($"output='{output}'");
            Console.WriteLine($"generators='{allValues[GeneratorsKey]}'");

            //Console.WriteLine();
            //Console.WriteLine("ALL KEYS");
            //foreach(var key in allValues.Keys)
            //    Console.WriteLine($"Key={key}, Value={allValues[key]}");
            //Console.WriteLine();

            //NOTE: Yaml Model files must end with ".nhydrate.yaml"

            //If a yaml file then check name and if not match look in parent folder
            if (modelFile.EndsWith(FileManagement.OldModelExtension))
            {
                //Do Nothing
            }
            else if (!modelFile.EndsWith(FileManagement.ModelExtension) && File.Exists(modelFile))
            {
                //Look in parent folder for model file
                var fi = new FileInfo(modelFile);
                var parentFolder = fi.Directory.Parent.FullName;
                var f = Directory.GetFiles(parentFolder, "*" + FileManagement.ModelExtension).FirstOrDefault();
                if (File.Exists(f)) modelFile = f;
                else return ShowError("Model file not found.");
            }

            //If a folder then find the model in the folder or parent folder
            if (Directory.Exists(modelFile))
            {
                var f = Directory.GetFiles(modelFile, "*" + FileManagement.ModelExtension).FirstOrDefault();
                if (File.Exists(f)) modelFile = f;
                else
                {
                    var parentFolder = (new DirectoryInfo(modelFile)).Parent.FullName;
                    f = Directory.GetFiles(parentFolder, "*" + FileManagement.ModelExtension).FirstOrDefault();
                    if (File.Exists(f)) modelFile = f;
                    else return ShowError("Model file not found.");
                }
            }

            var timer = System.Diagnostics.Stopwatch.StartNew();

            var buildModel = (allValues.ContainsKey("buildmodel") && allValues["buildmodel"] == "true");

            nHydrate.Generator.Common.Models.ModelRoot model = null;
            try
            {
                Console.WriteLine("Loading model...");
                model = ModelHelper.CreatePOCOModel(modelFile, buildModel);
            }
            catch (ModelException ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown error.");
                return 1;
            }

            //Generate
            if (model != null && !buildModel)
            {
                Console.WriteLine("Loading generators...");
                var genHelper = new nHydrate.Command.Core.GeneratorHelper(output);

                var genList = new List<nHydrateGeneratorProject>();
                var genProject = new nHydrateGeneratorProject();
                genList.Add(genProject);
                model.ResetKey(model.Key);
                model.GeneratorProject = genProject;
                genProject.Model = model;
                genProject.FileName = modelFile + ".generating";
                var document = new System.Xml.XmlDocument();
                document.LoadXml("<modelRoot guid=\"" + model.Id + "\" type=\"nHydrate.Generator.nHydrateGeneratorProject\" assembly=\"nHydrate.Generator.dll\"><ModelRoot></ModelRoot></modelRoot>");
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
            }
            else if (!buildModel)
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

        private static Dictionary<string, string> GetCommandLineParameters()
        {
            var retVal = new Dictionary<string, string>();
            var args = Environment.GetCommandLineArgs();
            args = args.Skip(1).ToArray();

            var loopcount = 0;
            foreach (var arg in args)
            {
                var regEx = new Regex(@"\s?[/](\w+)(:(.*))?");
                var regExMatch = regEx.Match(arg);
                if (regExMatch.Success)
                    retVal.Add(regExMatch.Groups[1].Value.ToLower(), regExMatch.Groups[3].Value);
                loopcount++;
            }

            return retVal;
        }

    }
}
