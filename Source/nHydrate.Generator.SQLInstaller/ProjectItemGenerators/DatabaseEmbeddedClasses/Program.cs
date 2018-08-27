using System;
using System.Collections.Generic;

namespace PROJECTNAMESPACE
{
    class Program
    {
        static void Main(string[] args)
        {
            //This class is added for convenience. If you wish to convert the installer to a console application
            //set its output type to "Console Application" and startup object to this class

            System.Console.WriteLine("Starting Install...");
            var stateSaver = new Dictionary<object, object>();
            var installer = new DatabaseInstaller();
            installer.Install(stateSaver);
            System.Console.WriteLine("Install Complete");
        }
    }
}
