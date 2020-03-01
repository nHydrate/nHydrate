using System;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators
{
	public abstract class BaseDbScriptGenerator : BaseProjectItemGenerator
	{
		public override string LocalNamespaceExtension => PostgresDatabaseProjectGenerator.NamespaceExtension;
    }
}

