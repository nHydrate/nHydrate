using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel.Design;
using System.Collections.Generic;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.Design.Editors
{
	internal class UnitTestDependencyEditor : System.Drawing.Design.UITypeEditor
	{
		public UnitTestDependencyEditor()
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
				Table table = (Table)context.Instance;
				Widgetsphere.Generator.Forms.UnitTestDependencyForm F = new Widgetsphere.Generator.Forms.UnitTestDependencyForm(table);
        if(edSvc.ShowDialog(F) == System.Windows.Forms.DialogResult.OK)
        {
					table.UnitTestDependencies.Clear();
					table.UnitTestDependencies.AddRange(F.GetSelectedList());
					table.Name = table.Name; //Make Dirty
          context.OnComponentChanged();
        }
      }
      catch(Exception ex) { }
      return value;
    }

	}
}
