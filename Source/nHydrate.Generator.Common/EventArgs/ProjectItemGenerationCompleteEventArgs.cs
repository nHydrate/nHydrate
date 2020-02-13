using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Common.EventArgs
{
	public class ProjectItemGenerationCompleteEventArgs : System.EventArgs
	{
		public ProjectItemGenerationCompleteEventArgs(IProjectItemGenerator projectItemGenerator)
		{
			this.ProjectItemGenerator = projectItemGenerator;
		}

		public IProjectItemGenerator ProjectItemGenerator { get; }

	}
}

