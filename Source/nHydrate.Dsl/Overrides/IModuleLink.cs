using System.Collections.Generic;

namespace nHydrate.Dsl
{
	public interface IModuleLink
	{
		IEnumerable<Module> Modules { get; }
		void AddModule(Module module);
		void RemoveModule(Module module);
	}
}

