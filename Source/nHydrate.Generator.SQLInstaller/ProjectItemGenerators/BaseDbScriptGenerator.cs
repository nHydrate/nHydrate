using System;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators
{
	public abstract class BaseDbScriptGenerator : BaseProjectItemGenerator
	{
		public override string LocalNamespaceExtension
		{
			get { return DatabaseProjectGenerator.NamespaceExtension; }
		}
	}
}

