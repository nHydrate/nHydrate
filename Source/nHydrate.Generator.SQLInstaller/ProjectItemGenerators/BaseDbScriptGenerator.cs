using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators
{
	public abstract class BaseDbScriptGenerator : BaseProjectItemGenerator
	{
		public override string LocalNamespaceExtension => DatabaseProjectGenerator.NamespaceExtension;
    }
}

