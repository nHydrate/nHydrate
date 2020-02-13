using System.Collections;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.EventArgs
{
	public class ProjectItemGeneratedEventArgs : System.EventArgs
	{
		#region Constructors

		private ProjectItemGeneratedEventArgs()
		{
			this.ParentItemType = ProjectItemType.File;
			this.Properties = new Hashtable();
			this.ProjectItemName = string.Empty;
			this.ProjectItemContent = string.Empty;
			this.ProjectName = string.Empty;
			this.ParentItemName = string.Empty;
			this.ContentType = ProjectItemContentType.String;
			this.Overwrite = true;
			this.FileState = EnvDTEHelper.FileStateConstants.Success;
			this.FullName = string.Empty;
			this.BaseGenerator = null;
			this.RunCustomTool = false;
		}

		public ProjectItemGeneratedEventArgs(string projectItemName, string projectItemContent, string projectName, IProjectItemGenerator baseGenerator, bool overwrite)
			: this()
		{
			this.BaseGenerator = baseGenerator;
			this.ProjectItemName = projectItemName;
			this.ProjectItemContent = projectItemContent;
			this.ProjectName = projectName;
			this.Overwrite = overwrite;
		}

		public ProjectItemGeneratedEventArgs(string projectItemName, string projectItemContent, string projectName, string parentItemName, IProjectItemGenerator baseGenerator, bool overwrite)
			: this()
		{
			this.BaseGenerator = baseGenerator;
			this.ProjectItemName = projectItemName;
			this.ProjectItemContent = projectItemContent;
			this.ProjectName = projectName;
			this.ParentItemName = parentItemName;
			this.Overwrite = overwrite;
		}

		public ProjectItemGeneratedEventArgs(string projectItemName, string projectItemContent, string projectName, string parentItemName, ProjectItemType parentItemType, IProjectItemGenerator baseGenerator, bool overwrite)
			: this()
		{
			this.BaseGenerator = baseGenerator;
			this.ProjectItemName = projectItemName;
			this.ProjectItemContent = projectItemContent;
			this.ProjectName = projectName;
			this.ParentItemName = parentItemName;
			this.ParentItemType = parentItemType;
			this.Overwrite = overwrite;
		}

		#endregion

		#region Property Implementations

		public EnvDTEHelper.FileStateConstants FileState { get; set; }

		public string FullName { get; set; }

		public IProjectItemGenerator BaseGenerator { get; }

		public ProjectItemType ParentItemType { get; }

		public ProjectItemContentType ContentType { get; set; }

		public string ParentItemName { get; }

		public string ProjectName { get; }

		public string ProjectItemName { get; internal set; }

		public string ProjectItemContent { get; }

		public byte[] ProjectItemBinaryContent { get; set; }

		public Hashtable Properties { get; set; }

		public bool Overwrite { get; }

		public bool RunCustomTool { get; set; }

		public string CustomToolName { get; set; }

		#endregion

	}
}

