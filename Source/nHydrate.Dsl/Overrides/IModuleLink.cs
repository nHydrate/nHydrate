using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Dsl
{
	public interface IModuleLink
	{
		//Microsoft.VisualStudio.Modeling.LinkedElementCollection<Module> Modules { get; }
		IEnumerable<Module> Modules { get; }

		void AddModule(Module module);

		void RemoveModule(Module module);

	}
}

