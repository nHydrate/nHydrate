using System;
using System.Collections.Generic;
using System.Text;

using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators
{
	public abstract class BaseDbScriptTemplate : BaseClassTemplate
	{
		public BaseDbScriptTemplate(ModelRoot model)
			: base(model)
		{
		}

		public override string LocalNamespaceExtension
		{
			get { return "Install"; }
		}
	}
}
