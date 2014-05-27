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

namespace nHydrate.Generator.Models
{
	public class CellEntryCollectionController : BaseModelObjectController
	{
		#region Member Variables

		#endregion

		#region Constructor

		protected internal CellEntryCollectionController(INHydrateModelObject modelObject)
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
					_node = new CellEntryCollectionNode(this);
				}
				return _node;
			}
		}

		public override ModelObjectUserInterface UIControl
		{
			get
			{
				if(this.UserInterface == null)
				{
					var ctrl = new PanelUIControl();
					var listView = new ListView();
					listView.View = View.Details;
					listView.Dock = DockStyle.Fill;
					listView.Columns.Add("Name", 100, HorizontalAlignment.Left);
					listView.Columns.Add("# Columns", 100, HorizontalAlignment.Left);
					ctrl.MainPanel.Controls.Add(listView);
					ctrl.Dock = DockStyle.Fill;
					this.UserInterface = ctrl;
				}
				return this.UserInterface;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			var mcAddCellEntry = new DefaultMenuCommand();
			mcAddCellEntry.Text = "New CellEntry";
			mcAddCellEntry.Click += new EventHandler(AddCellEntryMenuClick);

			return new MenuCommand[]{mcAddCellEntry};
		}

		public override MessageCollection Verify()
		{
			var retval = new MessageCollection();
			retval.AddRange(base.Verify());
			return retval;
		}

		public override bool DeleteObject()
		{
			return false;
		}

		public override void Refresh()
		{
		}

		#endregion

		#region Menu Handlers

		private void AddCellEntryMenuClick(object sender, System.EventArgs e)
		{
			var CellEntry = ((CellEntryCollection)this.Object).Add();
			this.OnItemChanged(this, new System.EventArgs());
		}

		#endregion

	}
}