using Microsoft.Extensions.Configuration;
using Serilog;
using System.Collections.Generic;

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

                Example command line to create a database
                /create /newdb:"MyDatabase" /master:"server=.;initial catalog=master;Integrated Security=SSPI;" /connectionstring:"server=.;initial catalog=MyDatabase;Integrated Security=SSPI;"

                Example command line to update a database
                /update /connectionstring:"server=.;initial catalog=MyDatabase;Integrated Security=SSPI;"

                NOTE: To run this installer from Visual Studio, add one of the lines above to 
                this project's properties sheet, Debug tab, Application Arguments

             */

            IConfiguration Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build();

            Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(Configuration)
             .CreateLogger();

            Log.Information("Starting Install...");
            var stateSaver = new Dictionary<object, object>();
            var installer = new DatabaseInstaller();
            installer.Install(stateSaver);
            Log.Information("Install Complete");
        }
    }
}
