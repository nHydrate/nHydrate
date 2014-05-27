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

using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class DatabaseNode : ModelObjectTreeNode
	{
		#region Member Variables

		protected ModelObjectTreeNode mTableCollectionNode = null;
		protected ModelObjectTreeNode mCustomViewCollectionNode = null;
		protected ModelObjectTreeNode mCustomStoredProcedureCollectionNode = null;
		protected ModelObjectTreeNode mCustomAggregateCollectionNode = null;

		#endregion

		#region Constructor

		public DatabaseNode(DatabaseController controller)
			: base(controller)
		{
		}

		#endregion

		#region Refresh

		public override void Refresh()
		{
			if ((this.TreeView != null) && (this.TreeView.InvokeRequired))
			{
				this.TreeView.Invoke(new EmptyDelegate(this.Refresh));
				return;
			}

			var database = this.Object as Database;

			this.Text = "Database";
			this.Name = database.Key;
			this.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.Database);
			this.SelectedImageIndex = this.ImageIndex;

			if ((database.Tables != null) && (mTableCollectionNode == null))
			{
				var dc = new TableCollectionController(database.Tables);
				//dc.Object = database.Tables;
				mTableCollectionNode = dc.Node;
				this.Nodes.Add(dc.Node);
			}

			if((database.CustomViews != null) && (mCustomViewCollectionNode == null))
			{
				var dc = new CustomViewCollectionController(database.CustomViews);
				//dc.Object = database.CustomViews;
				mCustomViewCollectionNode = dc.Node;
				this.Nodes.Add(dc.Node);
			}

			if((database.CustomStoredProcedures != null) && (mCustomStoredProcedureCollectionNode == null))
			{
				var dc = new CustomStoredProcedureCollectionController(database.CustomStoredProcedures);
				//dc.Object = database.CustomStoredProcedures;
				mCustomStoredProcedureCollectionNode = dc.Node;
				this.Nodes.Add(dc.Node);
			}

			//if ((database.CustomAggregates != null) && (mCustomAggregateCollectionNode == null))
			//{
			//  var dc = new CustomAggregateCollectionController(database.CustomAggregates);
			//  //dc.Object = database.CustomAggregates;
			//  mCustomAggregateCollectionNode = dc.Node;
			//  this.Nodes.Add(dc.Node);
			//}

			this.Controller.UIControl.Refresh();
			this.Expand();

		}

		#endregion

	}
}
