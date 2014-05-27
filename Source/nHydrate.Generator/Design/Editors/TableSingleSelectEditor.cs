#region Copyright (c) 2006-2012 nHydrate.org, All Rights Reserved
//--------------------------------------------------------------------- *
//                          NHYDRATE.ORG                                *
//             Copyright (c) 2006-2012 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF THE NHYDRATE GROUP *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS THIS PRODUCT                 *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF NHYDRATE,          *
//THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO             *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Design.Editors
{
	internal class ParentTableSelectEditor : System.Drawing.Design.UITypeEditor
	{
		public ParentTableSelectEditor()
		{
		}

		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
		}

		System.Windows.Forms.Design.IWindowsFormsEditorService edSvc = null;
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			try
			{
				edSvc = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

				var selectedItemKey = string.Empty;
				var root = (ModelRoot)((Table)context.Instance).Root;
				var sourceTable = (Table)context.Instance;
				var retval = sourceTable.ParentTable;
				selectedItemKey = sourceTable.ParentTableKey;

				var tableCollection = new List<Table>();
				foreach (Table t in root.Database.Tables)
				{
					tableCollection.Add(t);
				}

				//Remove the current table
				tableCollection.Remove(sourceTable);

				//Remove all parent tables
				var tList = sourceTable.GetTableHierarchy();
				foreach (var t in tList)
				{
					if (tableCollection.Contains(t))
						tableCollection.Remove(t);
				}

				//Remove all child tables
				tList = sourceTable.GetTablesInheritedFromHierarchy();
				foreach (var t in tList)
				{
					if (tableCollection.Contains(t))
						tableCollection.Remove(t);
				}

				//Add the current parent if one exists
				if (sourceTable.ParentTable != null)
					tableCollection.Add(sourceTable.ParentTable);

				//Create the list box 
				var newBox = new System.Windows.Forms.ListBox();
				newBox.Click += new EventHandler(newBox_Click);
				newBox.IntegralHeight = false;

				var sortedList = new SortedList();
				foreach (var table in tableCollection)
				{
					//Ensure key is unique to avoid error
					var text = table.Name.ToLower();
					var key = text;
					var ii = 0;
					while (sortedList.ContainsKey(key))
					{
						key = text + ii.ToString();
						ii++;
					}
					sortedList.Add(key, table);
				}

				//Re-add them in order
				newBox.Items.Add("(None)");
				newBox.SelectedIndex = 0;
				foreach (DictionaryEntry di in sortedList)
				{
					var t = (Table)di.Value;
					newBox.Items.Add(t);
					if (t.Key == selectedItemKey)
						newBox.SelectedIndex = newBox.Items.Count - 1;
				}

				edSvc.DropDownControl(newBox);
				if ((newBox.SelectedIndex > 0) && (newBox.SelectedItem != null))
					retval = ((Table)newBox.SelectedItem);
				else if (newBox.SelectedIndex == 0)
					retval = null;

				return retval;
			}
			catch (Exception ex) { }
			return null;
		}

		private void newBox_Click(object sender, System.EventArgs e) 
		{ 
			if (edSvc != null)
				edSvc.CloseDropDown();
		}

	}
}
