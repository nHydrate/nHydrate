using System;

namespace nHydrate.Generator
{
	public interface INHydrateModelObjectController : IDisposable
	{
		ModelObjectTreeNode Node { get; }
		INHydrateModelObject Object { get; set; }
		string HeaderText { get; set; }
		MessageCollection Verify();
	}
}
