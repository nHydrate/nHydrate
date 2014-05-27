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
using nHydrate.Generator.Forms;

namespace nHydrate.Generator.Models
{
	public class TableCompositeController : BaseModelObjectController
	{
		#region Member Variables

		#endregion

		#region Constructor

		protected internal TableCompositeController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if(_node == null)
				{
					_node = new TableCompositeNode(this);
				}
				return _node;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			var menuEdit = new DefaultMenuCommand();
			menuEdit.Text = "Edit";
			menuEdit.Click += new EventHandler(EditMenuClick);

			var menuSep1 = new DefaultMenuCommand();
			menuSep1.Text = "-";

			var menuDelete = new DefaultMenuCommand();
			menuDelete.Text = "Delete";
			menuDelete.Click += new EventHandler(DeleteMenuClick);

			return new MenuCommand[] { menuEdit, menuSep1, menuDelete };
		}

		public override MessageCollection Verify()
		{
			try
			{
				var retval = new MessageCollection();
				retval.AddRange(base.Verify());
				return retval;

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public override bool DeleteObject()
		{
			if (MessageBox.Show("Do you wish to delete this component?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			 {
				((TableComposite)this.Object).Parent.CompositeList.Remove((TableComposite)this.Object);
				this.Node.Remove();
				this.Object.Root.Dirty = true;
				this.Refresh();
				return true;
			}
			return false;
		}

		public override void Refresh()
		{
		}

		#endregion

		#region Menu Handlers

		private void DeleteMenuClick(object sender, System.EventArgs e)
		{
			this.DeleteObject();
		}

		private void EditMenuClick(object sender, System.EventArgs e)
		{
			var F = new TableCompositeColumnForm((TableComposite)this.Object);
			if (F.ShowDialog() == DialogResult.OK)
			{
				this.Object.Root.Dirty = true;
				this.OnItemChanged(this, new System.EventArgs());
				this.Refresh();
			}

		}

		#endregion

	}
}