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
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class TableNode : ModelObjectTreeNode
	{
		#region Member Variables

		protected ModelObjectTreeNode _columnCollectionNode = null;
		protected ModelObjectTreeNode _customRetrieveRulesNode = null;
		protected ModelObjectTreeNode _compositeCollectionNode = null;
		protected ModelObjectTreeNode _componentCollectionNode = null;
		protected ModelObjectTreeNode _relationshipCollectionNode = null;

		#endregion

		#region Constructor

		public TableNode(TableController controller)
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

				var table = ((Table)this.Object);

				this.Text = table.ToString();
				this.Name = table.Key;
				if (!table.Generated)
					this.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.TableNonGen);
				else if (table.AssociativeTable)
					this.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.TableAssociative);
				else if (table.IsTypeTable)
					this.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.TableType);
				else if (table.ParentTable != null)
					this.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.TableDerived);
				else
					this.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.Table);

				this.SelectedImageIndex = this.ImageIndex;
				this.ToolTipText = table.Description;

				//Columns
				if ((table.Columns != null) && (_columnCollectionNode == null))
				{
					var element = new ColumnCollectionController(table.Columns);
					_columnCollectionNode = element.Node;
					this.Nodes.Add(element.Node);
				}
				else _columnCollectionNode.Refresh();

				//Retrieve rules
				if ((table.CustomRetrieveRules != null) && (_customRetrieveRulesNode == null))
				{
					var element = new CustomRetrieveRuleCollectionController(table.CustomRetrieveRules);
					_customRetrieveRulesNode = element.Node;
					this.Nodes.Add(element.Node);
				}
				else _customRetrieveRulesNode.Refresh();

				//Composites
				//if (table.CompositeList != null)
				//{
				//  TableCompositeCollectionController element = null;
				//  if (mCompositeCollectionNode == null)
				//  {
				//    element = new TableCompositeCollectionController(((Table)_object).CompositeList);
				//    mCompositeCollectionNode = element.Node;
				//    this.Nodes.Add(element.Node);
				//  }
				//  else
				//  {
				//    element = (TableCompositeCollectionController)mCompositeCollectionNode.Controller;
				//  }
				//  mCompositeCollectionNode.Nodes.Clear();
				//  element.Node.Refresh();
				//}

				//Components
				if (table.ComponentList != null)
				{
					TableComponentCollectionController element = null;
					if (_componentCollectionNode == null)
					{
						element = new TableComponentCollectionController(((Table)_object).ComponentList);
						_componentCollectionNode = element.Node;
						this.Nodes.Add(element.Node);
					}
					else
					{
						element = (TableComponentCollectionController)_componentCollectionNode.Controller;
					}
					_componentCollectionNode.Nodes.Clear();
					element.Node.Refresh();
				}

				////Relations
				//if (table.Relationships != null)
				//{
				//  RelationCollectionController element = null;
				//  if (_relationshipCollectionNode == null)
				//  {
				//    element = new RelationCollectionController(((Table)_object).Relationships);
				//    _relationshipCollectionNode = element.Node;
				//    this.Nodes.Add(element.Node);
				//  }
				//  else
				//  {
				//    element = (RelationCollectionController)_relationshipCollectionNode.Controller;
				//  }
				//  _relationshipCollectionNode.Nodes.Clear();
				//  element.Node.Refresh();
				//}

				//Update the TableCollection list
				if (this.Parent != null)
					((TableCollectionNode)this.Parent).Refresh();
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		#endregion

	}
}
