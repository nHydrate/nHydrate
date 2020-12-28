using Microsoft.Extensions.Configuration;
using Serilog;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PROJECTNAMESPACE
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                Example Connection String
                server=.;initial catalog=MyDatabase;Integrated Security=SSPI;
                server=.;initial catalog=MyDatabase;user id=sa;password=Password1;

                Example command line to update a database
                --update=true --connectionstring="server=.;initial catalog=MyDatabase;Integrated Security=SSPI;"

                NOTE: To run this installer from Visual Studio, add one of the lines above to 
                this project's properties sheet, Debug tab, Application Arguments

             */

            IConfiguration Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                    .AddCommandLine(args)
                    .AddEnvironmentVariables()
                    .Build();

            var allValues = Configuration.GetChildren().Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key.ToString(), x => x.Value?.ToString());

            Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(Configuration)
             .CreateLogger();

            Log.Information("Starting Install...");
            try
            {
                var installer = new DatabaseInstaller();
                installer.Install(allValues);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Install Exception");
            }
            Log.Information("Install Complete");
        }
    }
}
