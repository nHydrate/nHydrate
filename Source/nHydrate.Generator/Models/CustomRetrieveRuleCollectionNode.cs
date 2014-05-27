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
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class CustomRetrieveRuleCollectionNode : ModelObjectTreeNode
	{
		#region Member Variables

		#endregion

		#region Constructor

		public CustomRetrieveRuleCollectionNode(CustomRetrieveRuleCollectionController controller)
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

				this.Text = "Select Commands";
				this.Name = "Select Commands";
				this.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
				this.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

				//Add new nodes
				var referenceCollection = (ReferenceCollection)this.Object;

				//Save the selected item
				var selectedKey = string.Empty;
				if ((this.TreeView != null) && (this.TreeView.SelectedNode != null) && (this.Nodes.Contains(this.TreeView.SelectedNode)))
				{
					var reference = referenceCollection.GetByKey(this.TreeView.SelectedNode.Name);
					if (reference != null)
						selectedKey = ((CustomRetrieveRule)reference.Object).Key;
				}

				foreach (Reference reference in referenceCollection)
				{
					if (this.Nodes.Find(reference.Key, false).Length == 0)
					{
						var tc = new CustomRetrieveRuleController(reference.Object);
						tc.Node.Name = reference.Key;
						tc.Node.Text = ((CustomRetrieveRule)reference.Object).Name;
						this.Nodes.Add(tc.Node);
					}
				}

				//Rename nodes if name change
				foreach (TreeNode node in this.Nodes)
				{
					var item = referenceCollection.FirstOrDefault(x => x.Key == node.Name);
					if (item != null) node.Text = ((CustomRetrieveRule)item.Object).Name;
				}

				//Remove non-existing nodes
				for (var ii = this.Nodes.Count - 1; ii >= 0; ii--)
				{
					var node = (CustomRetrieveRuleNode)this.Nodes[ii];
					if (!referenceCollection.Contains(node.Name))
					{
						this.Nodes.RemoveAt(ii);
					}
				}

				//Save the selected item      
				if ((this.TreeView != null) && (!string.IsNullOrEmpty(selectedKey)))
				{
					foreach (TreeNode node in this.Nodes)
					{
						var reference = referenceCollection.GetByKey(node.Name);
						if ((reference != null) && (selectedKey == ((CustomRetrieveRule)reference.Object).Key))
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

		}

		#endregion

	}
}
