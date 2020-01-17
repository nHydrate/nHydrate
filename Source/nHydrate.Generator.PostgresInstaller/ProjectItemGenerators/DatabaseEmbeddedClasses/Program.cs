using System;
using System.Collections.Generic;

namespace PROJECTNAMESPACE
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                Example Connection String
                Server=localhost;Database=MyDatabase;UID=postgres;PWD=postgres;

                Example command line to create a database
                /create /newdb:"MyDatabase" /master:"Server=localhost;Database=postgres;UID=postgres;PWD=postgres;" /connectionstring:"Server=localhost;Database=MyDatabase;UID=postgres;PWD=postgres;"

                Example command line to update a database
                /update /connectionstring:"Server=localhost;Database=MyDatabase;UID=postgres;PWD=postgres;"

             */

            System.Console.WriteLine("Starting Install...");
            var stateSaver = new Dictionary<object, object>();
            var installer = new DatabaseInstaller();
            installer.Install(stateSaver);
            System.Console.WriteLine("Install Complete");
        }
    }
}
