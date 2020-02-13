using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator
{
	interface INHydrateGenerator : IGenerator
	{
		INHydrateModelObjectController RootController { get; set; }
		ImageList ImageList { get; }
		MenuCommand[] GetMenuCommands();
		void HandleCommand(string command);
		event VerifyDelegate VerifyComplete;
	}
}
