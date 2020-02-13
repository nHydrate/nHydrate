using System.Collections;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.EventArgs
{
    public class ProjectItemDeletedEventArgs : System.EventArgs
    {
        #region Constructors

        private ProjectItemDeletedEventArgs()
        {
        }

        public ProjectItemDeletedEventArgs(string projectItemName, string projectName, IProjectItemGenerator baseGenerator)
            : this()
        {
            this.BaseGenerator = baseGenerator;
            this.ProjectItemName = projectItemName;
            this.ProjectName = projectName;
        }

        #endregion

        #region Property Implementations
        public bool DeleteFile { get; set; } = false;

        public EnvDTEHelper.FileStateConstants FileState { get; set; } = EnvDTEHelper.FileStateConstants.Success;

        public string FullName { get; set; } = string.Empty;

        public IProjectItemGenerator BaseGenerator { get; } = null;

        public ProjectItemType ParentItemType { get; } = ProjectItemType.File;

        public ProjectItemContentType ContentType { get; set; } = ProjectItemContentType.String;

        public string ParentItemName { get; } = string.Empty;

        public string ProjectName { get; } = string.Empty;

        public string ProjectItemName { get; } = string.Empty;

        public Hashtable Properties { get; set; } = new Hashtable();

        #endregion

    }
}
