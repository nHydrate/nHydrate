using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GeneratorAttribute : System.Attribute
    {
        public GeneratorAttribute(string projectGuid, string modelName)
        {
            ProjectGuid = new Guid(projectGuid);
            ModelName = modelName;
        }

        public System.Guid ProjectGuid { get; }

        public string ModelName { get; } = string.Empty;

        public override string ToString()
        {
            return this.ModelName;
        }

    }
}

