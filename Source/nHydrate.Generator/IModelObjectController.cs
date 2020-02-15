using System;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator
{
	public interface INHydrateModelObjectController : IDisposable
	{
		ModelObjectTreeNode Node { get; }
		INHydrateModelObject Object { get; set; }
		ModelObjectUserInterface UIControl { get; }
		string HeaderText { get; set; }
		string HeaderDescription { get; set; }
		System.Drawing.Bitmap HeaderImage { get; set; }

		MenuCommand[] GetMenuCommands();
		event ItemChanagedEventHandler ItemChanged;
		MessageCollection Verify();
		bool DeleteObject();
		void Refresh();
		bool IsEnabled { get; }
		bool IsPopupUI { get; set; }

		void OnItemChanged(object sender, System.EventArgs e);
	}
}
