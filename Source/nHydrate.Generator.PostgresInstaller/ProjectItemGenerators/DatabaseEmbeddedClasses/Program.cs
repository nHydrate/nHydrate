using System;
using System.Collections.Generic;

namespace PROJECTNAMESPACE
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting Install...");
            var stateSaver = new Dictionary<object, object>();
            var installer = new DatabaseInstaller();
            installer.Install(stateSaver);
            System.Console.WriteLine("Install Complete");
        }
    }
}
