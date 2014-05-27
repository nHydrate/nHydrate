using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Dsl.Design.Editors
{
	internal class VersionHistoryCollectionEditor : System.Drawing.Design.UITypeEditor
	{
		public VersionHistoryCollectionEditor()
		{
		}

		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return System.Drawing.Design.UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			try
			{
				var edSvc = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
				var F = new Widgetsphere.Generator.Forms.VersionHistoryCollectionForm((VersionHistoryCollection)value);
				if (edSvc.ShowDialog(F) == System.Windows.Forms.DialogResult.OK)
				{
					context.OnComponentChanged();
				}
			}
			catch (Exception ex) { }
			return value;
		}

	}
}
