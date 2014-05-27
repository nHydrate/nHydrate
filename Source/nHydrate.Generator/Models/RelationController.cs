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
using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.ModelUI;

namespace nHydrate.Generator.Models
{
	public class RelationController : BaseModelObjectController
	{
		#region Member Variables

		private RelationshipControllerUIControl _uiControl = null;

		#endregion

		#region Constructor

		protected internal RelationController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Relationship";
			this.HeaderDescription = "A single relationship for the selected entity";
			//this.HeaderImage = ImageHelper.GetImage(ImageConstants.Entity);

			((Relation)modelObject).BeforeChildTableChange += new EventHandler(RelationController_BeforeChildTableChange);
			((Relation)modelObject).BeforeParentTableChange += new EventHandler(RelationController_BeforeParentTableChange);
			((Relation)modelObject).AfterChildTableChange += new EventHandler(RelationController_AfterChildTableChange);
			((Relation)modelObject).AfterParentTableChange += new EventHandler(RelationController_AfterParentTableChange);
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if(_node == null)
				{
					_node = new RelationNode(this);
				}
				return _node;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			//MenuCommand mc = new DefaultMenuCommand();
			//mc.Text = "Text";
			//mc.Click += new EventHandler(MenuClick);
			//return new MenuCommand[]{mc};
			return new MenuCommand[] { };
		}

		public override MessageCollection Verify()
		{
			var retval = new MessageCollection();
			retval.AddRange(base.Verify());

			var relation = (Relation)this.Object;


			return retval;
		}

		public override bool DeleteObject()
		{
			this.Node.Remove();
			this.Object.Root.Dirty = true;
			return true;
		}

		public override void Refresh()
		{
		}

		public override ModelObjectUserInterface UIControl
		{
			get
			{
				if (this._userInterface == null)
				{
					var ctrl = new PanelUIControl();
					_uiControl = new RelationshipControllerUIControl();
					_uiControl.Populate(this.Object as Relation);
					_uiControl.Dock = System.Windows.Forms.DockStyle.Fill;
					ctrl.MainPanel.Controls.Add(_uiControl);
					ctrl.Dock = DockStyle.Fill;
					this._userInterface = ctrl;
				}
				this._userInterface.Enabled = this.IsEnabled;
				return this._userInterface;
			}
		}

		private void RelationController_BeforeParentTableChange(object sender, EventArgs e)
		{
			//Find root node
			var rootNode = this.Node;
			while (rootNode.Parent != null)
				rootNode = (ModelObjectTreeNode)rootNode.Parent;

			var relation = (Relation)this.Object;
			var modelRoot = ((ModelRoot)this.Object.Root);

			//Process parent table
			var parentTable = (Table)relation.ParentTableRef.Object;
			var nodeList = rootNode.Nodes.Find(parentTable.Key, true);
			foreach (TableNode tableNode in nodeList)
			{
				foreach (var node in tableNode.Nodes.Find(this.Object.Key, true))
				{
					node.Remove();
				}
			}
		}

		private void RelationController_BeforeChildTableChange(object sender, EventArgs e)
		{
			//Find root node
			var rootNode = this.Node;
			while (rootNode.Parent != null)
				rootNode = (ModelObjectTreeNode)rootNode.Parent;

			var relation = (Relation)this.Object;
			var modelRoot = ((ModelRoot)this.Object.Root);

			//Process child table
			var childTable = (Table)relation.ChildTableRef.Object;
			var nodeList = rootNode.Nodes.Find(childTable.Key, true);
			foreach (TableNode tableNode in nodeList)
			{
				foreach (var node in tableNode.Nodes.Find(this.Object.Key, true))
				{
					node.Remove();
				}
			}
		}

		private void RelationController_AfterParentTableChange(object sender, EventArgs e)
		{
			//Find root node
			var rootNode = this.Node;
			while (rootNode.Parent != null)
				rootNode = (ModelObjectTreeNode)rootNode.Parent;

			var relation = (Relation)this.Object;
			var modelRoot = ((ModelRoot)this.Object.Root);

			//Process parent table
			var parentTable = (Table)relation.ParentTableRef.Object;
			//parentTable.Relationships.Add(relation.CreateRef());
			var nodeList = rootNode.Nodes.Find(parentTable.Key, true);
			foreach (TableNode tableNode in nodeList)
				tableNode.Refresh();
		}

		private void RelationController_AfterChildTableChange(object sender, EventArgs e)
		{
			//Find root node
			var rootNode = this.Node;
			while (rootNode.Parent != null)
				rootNode = (ModelObjectTreeNode)rootNode.Parent;

			var relation = (Relation)this.Object;
			var modelRoot = ((ModelRoot)this.Object.Root);

			//Process child table
			var childTable = (Table)relation.ChildTableRef.Object;
			//childTable.Relationships.Add(relation.CreateRef());
			var nodeList = rootNode.Nodes.Find(childTable.Key, true);
			foreach (TableNode tableNode in nodeList)
				tableNode.Refresh();
		}

		#endregion

		#region Menu Handlers

		private void MenuClick(object sender, System.EventArgs e)
		{
		}

		#endregion

	}
}