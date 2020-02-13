using System.Collections;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.EventArgs
{
	public class ProjectItemGeneratedErrorEventArgs : System.EventArgs
	{
		#region Constructors

		#endregion

		#region Property Implementations

		public string Text { get; set; }

		public bool ShowError { get; set; }

		#endregion

	}
}

