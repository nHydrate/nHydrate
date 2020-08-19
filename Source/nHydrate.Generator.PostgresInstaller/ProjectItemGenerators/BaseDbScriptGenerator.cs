using nHydrate.Generator.Common.ProjectItemGenerators;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators
{
    public abstract class BaseDbScriptGenerator : BaseProjectItemGenerator
    {
        public override string LocalNamespaceExtension => PostgresDatabaseProjectGenerator.NamespaceExtension;
    }
}

