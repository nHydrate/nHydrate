using System;

namespace nHydrate.Generator
{
	public interface INHydrateModelObjectController : IDisposable
	{
		ModelObjectTreeNode Node { get; }
		INHydrateModelObject Object { get; set; }
		string HeaderText { get; set; }
		string HeaderDescription { get; set; }
		System.Drawing.Bitmap HeaderImage { get; set; }

		MenuCommand[] GetMenuCommands();
		MessageCollection Verify();
		bool DeleteObject();
		void Refresh();
		bool IsEnabled { get; }
	}
}
