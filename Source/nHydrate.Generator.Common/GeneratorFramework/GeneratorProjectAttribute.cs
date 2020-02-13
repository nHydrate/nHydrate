using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GeneratorProjectAttribute : GeneratorItemAttribute
    {
        protected Type _currentType;
        protected string[] _dependencyList = new string[0];
        protected string[] _exclusionList = new string[0];

        public GeneratorProjectAttribute(string name, string description, string generatorGuid, Type parentType, Type currentType, bool isMain, string[] dependencyList)
            : base(name, parentType)
        {
            _dependencyList = dependencyList;
            _currentType = currentType;
            this.Description = description;
            this.GeneratorGuid = generatorGuid;
            this.IsMain = isMain;
        }

        public GeneratorProjectAttribute(string name, string description, string generatorGuid, Type parentType, Type currentType, bool isMain, string[] dependencyList, string[] exclusionList)
            : this(name, description, generatorGuid, parentType, currentType, isMain, dependencyList)
        {
            _exclusionList = exclusionList;
        }

        public Type CurrentType
        {
            get { return _currentType; }
        }

        public string[] DependencyList
        {
            get { return _dependencyList; }
        }

        public string[] ExclusionList
        {
            get { return _exclusionList; }
        }

        public string Description { get; set; }
        public string GeneratorGuid { get; set; }
        public bool IsMain { get; set; }
    }
}
