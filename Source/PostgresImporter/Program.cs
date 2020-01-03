using Npgsql;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PostgresImport
{
    class Program
    {
        private const string _connectionString = @"Host=localhost;Database=ClovisEF;Username=postgres;Password=postgres";

        static void Main(string[] args)
        {
            var a = nHydrate.Tools.PostgresImporter.ImportDomain.GetPk(_connectionString);
            var b = nHydrate.Tools.PostgresImporter.ImportDomain.GetTables(_connectionString);
            var c = nHydrate.Tools.PostgresImporter.ImportDomain.GetIndexes(_connectionString);
        }

    }

}
