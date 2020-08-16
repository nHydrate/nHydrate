#pragma warning disable 0168
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.PostgresInstaller
{
    [GeneratorProjectAttribute(
        "Postgres Database Installer (beta)",
        "Creates a project to maintain a Postgres database, used in conjunction with the generated Entity Framework data access layer.",
        "c7153425-b9f2-4291-82e8-88e1295eef09",
        typeof(nHydrateGeneratorProject),
        typeof(PostgresDatabaseProjectGenerator),
        true,
        new string[] { }
        )]
    public class PostgresDatabaseProjectGenerator : BaseProjectGenerator
    {
        protected override string ProjectTemplate => "efcorepostgresinstaller.vstemplate";

        public override string LocalNamespaceExtension => PostgresDatabaseProjectGenerator.NamespaceExtension;

        public static string NamespaceExtension => "PostgresInstall";

        public override IModelConfiguration ModelConfiguration { get; set; }
    }
}
