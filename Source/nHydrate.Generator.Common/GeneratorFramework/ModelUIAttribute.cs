using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModelUIAttribute : System.Attribute
    {
        #region Class Members

        private readonly System.Guid _projectGuid;
        private readonly string _name = string.Empty;

        #endregion

        #region Constructor

        public ModelUIAttribute(string projectGuid, string name)
        {
            _projectGuid = new Guid(projectGuid);
            _name = name;
        }

        #endregion

        #region Property Implementations

        public System.Guid ProjectGuid
        {
            get { return _projectGuid; }
        }

        public string Name
        {
            get { return _name; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Name;
        }

        #endregion

    }
}
