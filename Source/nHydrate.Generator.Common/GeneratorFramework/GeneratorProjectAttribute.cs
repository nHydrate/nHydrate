using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GeneratorProjectAttribute : GeneratorItemAttribute
    {
        public GeneratorProjectAttribute(string name, string description, string generatorGuid, Type parentType, Type currentType, bool isMain, string[] dependencyList)
            : base(name, parentType)
        {
            DependencyList = dependencyList;
            CurrentType = currentType;
            this.Description = description;
            this.GeneratorGuid = generatorGuid;
            this.IsMain = isMain;
        }

        public GeneratorProjectAttribute(string name, string description, string generatorGuid, Type parentType, Type currentType, bool isMain, string[] dependencyList, string[] exclusionList)
            : this(name, description, generatorGuid, parentType, currentType, isMain, dependencyList)
        {
            ExclusionList = exclusionList;
        }

        public Type CurrentType { get; }

        public string[] DependencyList { get; } = new string[0];

        public string[] ExclusionList { get; } = new string[0];

        public string Description { get; set; }
        public string GeneratorGuid { get; set; }
        public bool IsMain { get; set; }
    }
}
