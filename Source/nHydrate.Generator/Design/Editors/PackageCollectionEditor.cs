using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel.Design;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.Design.Editors
{
	internal class PackageCollectionEditor : System.Drawing.Design.UITypeEditor
	{
		public PackageCollectionEditor()
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
        System.Windows.Forms.Design.IWindowsFormsEditorService edSvc = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
				Widgetsphere.Generator.Forms.PackageCollectionForm F = new Widgetsphere.Generator.Forms.PackageCollectionForm((PackageCollection)value);
        if(edSvc.ShowDialog(F) == System.Windows.Forms.DialogResult.OK)
        {
          context.OnComponentChanged();
        }
      }
      catch(Exception ex) { }
      return value;
    }

	}
}
