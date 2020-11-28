using Microsoft.Extensions.Configuration;
using nHydrate.Generator.Common;
using System;
using System.Collections.Generic;
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
            var allValues = Configuration.GetSection("nHydrate").GetChildren().Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key.ToString(), x => x.Value?.ToString());
            if (allValues.ContainsKey(ModelKey))
                modelFile = allValues[ModelKey];
            if (allValues.ContainsKey(OutputKey))
                output = allValues[OutputKey];
            if (allValues.ContainsKey(GeneratorsKey))
                generators = allValues[GeneratorsKey].Split(",", StringSplitOptions.RemoveEmptyEntries);

            //Command line (overrides above)
            allValues = Configuration.GetChildren().Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key.ToString(), x => x.Value?.ToString());
            if (allValues.ContainsKey(ModelKey))
                modelFile = allValues[ModelKey];
            if (allValues.ContainsKey(OutputKey))
                output = allValues[OutputKey];
            if (allValues.ContainsKey(GeneratorsKey))
                generators = allValues[GeneratorsKey].Split(",", StringSplitOptions.RemoveEmptyEntries);

            if (string.IsNullOrEmpty(modelFile))
                return ShowError("The model is required.");
            if (string.IsNullOrEmpty(output))
                return ShowError("The output folder is required.");
            if (!generators.Any())
                return ShowError("The generators are required.");

            var timer = System.Diagnostics.Stopwatch.StartNew();

            nHydrate.Generator.Common.Models.ModelRoot model = null;
            try
            {
                Console.WriteLine("Loading model...");
                model = ModelHelper.CreatePOCOModel(modelFile);
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
            if (model != null)
            {
                Console.WriteLine("Loading generators...");
                var genHelper = new nHydrate.Command.Core.GeneratorHelper(output);

                var genList = new List<nHydrateGeneratorProject>();
                var genProject = new nHydrateGeneratorProject();
                genList.Add(genProject);
                model.SetKey(model.Id.ToString());
                model.GeneratorProject = genProject;
                genProject.Model = model;
                genProject.FileName = modelFile + ".generating";
                var document = new System.Xml.XmlDocument();
                document.LoadXml("<modelRoot guid=\"" + model.Id + "\" type=\"nHydrate.Generator.nHydrateGeneratorProject\" assembly=\"nHydrate.Generator.dll\"><ModelRoot></ModelRoot></modelRoot>");
                ((nHydrate.Generator.Common.GeneratorFramework.IXMLable)model).XmlAppend(document.DocumentElement.ChildNodes[0]);
                System.IO.File.WriteAllText(genProject.FileName, document.ToIndentedString());

                var allgenerators = genHelper.GetProjectGenerators(genProject);

                var excludeList = allgenerators.Where(x => !generators.Contains(x.FullName)).ToList();

                Console.WriteLine($"Generating code...");
                foreach (var item in genList)
                {
                    genHelper.GenerateAll(item, excludeList);
                }
            }
            else
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
