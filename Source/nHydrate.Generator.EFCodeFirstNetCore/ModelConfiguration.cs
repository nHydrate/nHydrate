using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nHydrate.Generator.EFCodeFirstNetCore
{
    public enum DatabaseTypeConstants
    {
        SqlServer = 1,
        Postgress = 2,
        Sqlite = 3,
    }

    public class ModelConfiguration : IModelConfiguration
    {
        public DatabaseTypeConstants DatabaseType { get; set; } = DatabaseTypeConstants.SqlServer;
    }
}
