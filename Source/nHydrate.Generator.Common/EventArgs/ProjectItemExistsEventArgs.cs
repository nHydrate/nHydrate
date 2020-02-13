using System.Collections;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.EventArgs
{
	public class ProjectItemExistsEventArgs : System.EventArgs
	{
		#region Constructors

		public ProjectItemExistsEventArgs(string projectName, string itemName, ProjectItemType itemType)
		{
			this.ProjectName = projectName;
			this.ItemName = itemName;
			this.ItemType = itemType;
			this.Exists = false;
		}

		#endregion

		#region Property Implementations

		public string ItemName { get; private set; }
		public string ProjectName { get; private set; }
		public ProjectItemType ItemType { get; private set; }
		public bool Exists { get; set; }

		#endregion

	}
}

