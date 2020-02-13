using System;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.Datasite
{
	public abstract class BaseScriptGenerator : BaseProjectItemGenerator
	{
		public override string LocalNamespaceExtension
		{
			get { return DatasiteProjectGenerator.NamespaceExtension; }
		}
	}
}

