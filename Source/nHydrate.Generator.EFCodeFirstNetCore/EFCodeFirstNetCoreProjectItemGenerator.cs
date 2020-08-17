using nHydrate.Generator.Common.ProjectItemGenerators;

namespace nHydrate.Generator.EFCodeFirstNetCore
{
    public abstract class EFCodeFirstNetCoreProjectItemGenerator : BaseClassGenerator
    {
        public override string LocalNamespaceExtension => EFCodeFirstNetCoreProjectGenerator.NamespaceExtension;
    }
}
