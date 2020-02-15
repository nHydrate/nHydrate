#pragma warning disable 0168
using System;
using System.Linq;
using System.ComponentModel;

namespace nHydrate.Dsl.Design.Editors
{
	internal class EntityFieldEditor : System.Drawing.Design.UITypeEditor
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

				var indexColumn = context.Instance as IndexColumn;
				var field = indexColumn.Index.Entity.Fields.FirstOrDefault(x => x.Id == indexColumn.FieldID);
				var fieldList = indexColumn.Index.Entity.Fields.OrderBy(x => x.Name).ToList();

				//Create the list box
				var newBox = new System.Windows.Forms.ListBox();
				newBox.Click += new EventHandler(newBox_Click);
				newBox.IntegralHeight = false;

				newBox.Items.AddRange(fieldList.Select(x => x.Name).ToArray());
				if (field != null)
					newBox.SelectedIndex = fieldList.IndexOf(field);

				edSvc.DropDownControl(newBox);
				if ((indexColumn != null) && (newBox.SelectedIndex != -1))
					retval = fieldList[newBox.SelectedIndex].Id;

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

