using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class GeneratorAttribute : System.Attribute
	{
		#region Class Members

		private readonly System.Guid _projectGuid;
		private readonly string _modelName = string.Empty;

		#endregion

		#region Constructor

		public GeneratorAttribute(string projectGuid, string modelName)
		{
			_projectGuid = new Guid(projectGuid);
			_modelName = modelName;
		}

		#endregion

		#region Property Implementations

		public System.Guid ProjectGuid
		{
			get { return _projectGuid; }
		}

		public string ModelName
		{
			get { return _modelName; }
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			return this.ModelName;
		}

		#endregion

	}
}

