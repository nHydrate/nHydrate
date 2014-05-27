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
using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class TableCollectionNode : ModelObjectTreeNode
	{
		#region Member Variables

		#endregion

		#region Constructor

		public TableCollectionNode(TableCollectionController controller)
			: base(controller)
		{

		}

		#endregion

		#region Refresh

		public override void Refresh()
		{
			try
			{
				if ((this.TreeView != null) && (this.TreeView.InvokeRequired))
				{
					this.TreeView.Invoke(new EmptyDelegate(this.Refresh));
					return;
				}

				if (this.TreeView != null)
					this.TreeView.BeginUpdate();

				this.Text = "Entities";
				this.Name = "Entities";
				this.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.Tables);
				this.SelectedImageIndex = this.ImageIndex;

				var tableCollection = (TableCollection)this.Object;

				//Add new nodes      
				foreach (Table table in tableCollection)
				{
					if (this.Nodes.Find(table.Key, false).Length == 0)
					{
						var tc = new TableController(table);
						tc.Node.Name = table.Key;
						tc.Node.Text = table.Name;
						this.Nodes.Add(tc.Node);
					}
				}

				//Use a hash table for speed
				var tableCache = new Hashtable();
				foreach (Table table in tableCollection)
					tableCache.Add(table.Key, table);

				//Rename nodes if name change
				foreach (TreeNode node in this.Nodes)
				{
					if (tableCache.ContainsKey(node.Name))
					{
						var item = (Table)tableCache[node.Name];
						//var item = tableCollection.FirstOrDefault(x => x.Key == node.Name);
						if ((item != null) && (node.Text != item.Name)) node.Text = item.Name;
					}
				}

				//Remove non-existing nodes
				for (var ii = this.Nodes.Count - 1; ii >= 0; ii--)
				{
					var node = (TableNode)this.Nodes[ii];
					if (!tableCollection.Contains(node.Text))
					{
						this.Nodes.RemoveAt(ii);
					}
				}

				this.Sort();
				this.Controller.UIControl.Refresh();
				this.Expand();
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

		#endregion

	}
}
