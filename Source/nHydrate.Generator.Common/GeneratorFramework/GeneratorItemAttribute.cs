using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class GeneratorItemAttribute : System.Attribute
	{
        public GeneratorItemAttribute(string name, Type parentType)
		{
			Name = name;
			ParentType = parentType;
		}

		public Type ParentType { get; }

        public string Name { get; }
    }
}
