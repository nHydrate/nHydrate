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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class ColumnCollectionNode : ModelObjectTreeNode
	{
		#region Member Variables

		#endregion

		#region Constructor

		public ColumnCollectionNode(ColumnCollectionController controller)
			: base(controller)
		{
		}

		#endregion

		#region Refresh

		public override void Refresh()
		{
			try
			{
				//BaseModelObject parent = (BaseModelObject)((nHydrate.Generator.Models.ReferenceCollection)(((nHydrate.Generator.Models.ColumnCollectionController)((nHydrate.Generator.Common.GeneratorFramework.ModelObjectTreeNode)this).Controller).Object)).Parent;
				if ((this.TreeView != null) && (this.TreeView.InvokeRequired))
				{
					this.TreeView.Invoke(new EmptyDelegate(this.Refresh));
					return;
				}

				if (this.TreeView != null)
					this.TreeView.BeginUpdate();

				this.Text = "Fields";
				this.Name = "Fields";
				this.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
				this.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

				//Add new nodes
				var referenceCollection = (ReferenceCollection)this.Object;
				Table parentTable = null;
				if (referenceCollection.Parent is Table)
					parentTable = (Table)referenceCollection.Parent;

				//Save the selected item
				var selectedKey = string.Empty;
				if ((this.TreeView != null) && (this.TreeView.SelectedNode != null) && (this.Nodes.Contains(this.TreeView.SelectedNode)))
				{
					var reference = referenceCollection.GetByKey(this.TreeView.SelectedNode.Name);
					if (reference != null)
						selectedKey = ((Column)reference.Object).Key;
				}

				var shallowColumnNames = new List<string>();
				var delList = new List<Reference> ();
				foreach (Reference reference in referenceCollection)
				{
					if (this.Nodes.Find(reference.Key, false).Length == 0)
					{
						if (reference.Object != null)
						{
							var column = (Column)reference.Object;
							shallowColumnNames.Add(column.Name.ToLower());
							this.AddColumnNode(reference, column);
						}
						else
						{
							delList.Add(reference);
						}
					}
				}
				referenceCollection.RemoveRange(delList);

				//Rename nodes if name change
				foreach (ColumnNode node in this.Nodes)
				{
					var item = referenceCollection.FirstOrDefault(x => x.Key == node.Name);
					if (item != null) node.Text = ((Column)item.Object).Name;
				}

				//Remove non-existing nodes
				for (var ii = this.Nodes.Count - 1; ii >= 0; ii--)
				{
					var node = (ColumnNode)this.Nodes[ii];
					if (!referenceCollection.Contains(node.Name))
					{
						this.Nodes.RemoveAt(ii);
					}
				}

				//Load the inherited columns
				if (parentTable != null)
				{
					var allTables = new List<Table>(parentTable.GetTableHierarchy());
					allTables.Remove(parentTable);

					//Loop through all base tables and list columns that are NOT included in this table (the PK/audit columns)
					foreach (var table in allTables)
					{
						foreach (var column in table.GeneratedColumns)
						{
							if (!shallowColumnNames.Contains(column.Name.ToLower()))
							{
								shallowColumnNames.Add(column.Name.ToLower()); //just in case, no duplicates
								this.AddColumnNode(column);
							}
						}
					}
				}

				//Save the selected item      
				if ((this.TreeView != null) && (!string.IsNullOrEmpty(selectedKey)))
				{
					foreach (TreeNode node in this.Nodes)
					{
						var reference = referenceCollection.GetByKey(node.Name);
						if ((reference != null) && (selectedKey == ((Column)reference.Object).Key))
							this.TreeView.SelectedNode = node;
					}
				}

				this.Sort();

				this.Controller.UIControl.Refresh();

			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				if (this.TreeView != null)
					this.TreeView.EndUpdate();
			}

		}

		private void AddColumnNode(Column column)
		{
			this.AddColumnNode(null, column);
		}

		private void AddColumnNode(Reference reference, Column column)
		{
			var tc = new ColumnController(column);
			var isBaseTablePK = false;
			if (reference != null)
			{
				tc.Node.Tag = reference;
				tc.Node.Name = reference.Key;
			}
			else
			{
				isBaseTablePK = column.PrimaryKey;
				tc.Node.Name = column.Key;
				tc.Node.ForeColor = SystemColors.GrayText;
			}
			tc.Node.Refresh();
			tc.Node.Text = column.Name;

			if ((this.Nodes.Find(tc.Node.Name, false).Length == 0) && !isBaseTablePK)
			{
				this.Nodes.Add(tc.Node);
			}
		}

		#endregion

	}
}
