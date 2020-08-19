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

        /*
            /model:C:\code\nHydrateTestAug\ModelProject\Model1.nhydrate /output:C:\code\nHydrateTestAug
        */

        static int Main(string[] args)
        {
            var commandParams = GetCommandLineParameters();

            if (!commandParams.ContainsKey(ModelKey))
                return ShowError("The model is required.");
            if (!commandParams.ContainsKey(OutputKey))
                return ShowError("The output folder is required.");

            var modelFile = commandParams[ModelKey];
            var output = commandParams[OutputKey];

            try
            {
                var obj = ModelHelper.CreatePOCOModel(modelFile);
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
