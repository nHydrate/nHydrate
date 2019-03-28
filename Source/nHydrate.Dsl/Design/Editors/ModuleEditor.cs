#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

