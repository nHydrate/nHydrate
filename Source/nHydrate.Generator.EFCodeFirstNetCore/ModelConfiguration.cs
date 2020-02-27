using nHydrate.Generator.Common.GeneratorFramework;

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
