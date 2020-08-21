using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Common.EventArgs
{
    public class ProjectItemExistsEventArgs : System.EventArgs
    {
        #region Constructors

        public ProjectItemExistsEventArgs(string projectName, string itemName, ProjectItemType itemType)
        {
            this.ProjectName = projectName;
            this.ProjectItemName = itemName;
            this.ItemType = itemType;
            this.Exists = false;
        }

        #endregion

        #region Property Implementations

        public string ProjectItemName { get; }
        public string ProjectName { get; }
        public ProjectItemType ItemType { get; }
        public bool Exists { get; set; }

        #endregion

    }
}

