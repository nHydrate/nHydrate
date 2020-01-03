using nHydrate.DslPackage.Objects.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nHydrate.Tools.PostgresImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Host=localhost;Database=Test44;Username=postgres;Password=postgres";
            //var b = ImportDomain.TestConnection(connectionString);
            var rr = ImportDomain.GetRelations(connectionString);
        }
    }
}
