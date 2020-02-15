#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace nHydrate.Dsl.Design.Editors
{
	internal class ModuleEditor : System.Drawing.Design.UITypeEditor
	{
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
		}

		private System.Windows.Forms.Design.IWindowsFormsEditorService edSvc = null;
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			var retval = Guid.Empty;
			try
			{
				edSvc = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

				var moduleRule = context.Instance as ModuleRule;
				var modulelist = moduleRule.Module.nHydrateModel.Modules.OrderBy(x => x.Name).Where(x => x.Id != moduleRule.Module.Id).ToList();
				var selected = modulelist.FirstOrDefault(x=>x.Id == moduleRule.DependentModule); 

				//Create the list box
				var newBox = new System.Windows.Forms.ListBox();
				newBox.Click += new EventHandler(newBox_Click);
				newBox.IntegralHeight = false;

				var values = new List<string>();
				values.Add(false.ToString());
				values.Add(true.ToString());

				newBox.Items.AddRange(modulelist.Select(x => x.Name).ToArray());
				if (selected != null)
					newBox.SelectedIndex = modulelist.IndexOf(selected);

				edSvc.DropDownControl(newBox);
				if ((moduleRule != null) && (newBox.SelectedIndex != -1))
					retval = modulelist[newBox.SelectedIndex].Id;

			}
			catch (Exception ex) { }
			return retval;
		}

		private void newBox_Click(object sender, System.EventArgs e)
		{
			if (edSvc != null)
				edSvc.CloseDropDown();
		}

	}
}

