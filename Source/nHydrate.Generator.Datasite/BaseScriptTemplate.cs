using System;
using System.Collections.Generic;
using System.Text;

using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.Datasite
{
	public abstract class BaseScriptTemplate : BaseClassTemplate
	{
		public BaseScriptTemplate(ModelRoot model)
			: base(model)
		{
		}

		public override string LocalNamespaceExtension
		{
			get { return "Datasite"; }
		}
	}
}

