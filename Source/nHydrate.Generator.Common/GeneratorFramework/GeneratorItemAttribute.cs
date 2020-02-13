using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class GeneratorItemAttribute : System.Attribute
	{
		protected string _name;
		protected Type _parentType;

		public GeneratorItemAttribute(string name, Type parentType)
		{
			_name = name;
			_parentType = parentType;
		}

		public Type ParentType
		{
			get { return _parentType; }
		}

		public string Name
		{
			get { return _name; }
		}

	}
}
