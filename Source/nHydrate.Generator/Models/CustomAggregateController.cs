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
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class CustomAggregateController : BaseModelObjectController
	{
		#region Member Variables

		#endregion

		#region Constructor

		protected internal CustomAggregateController(INHydrateModelObject modelObject)
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
					_node = new CustomAggregateNode(this);
				}
				return _node;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			var mc = new DefaultMenuCommand();
			mc.Text = "Delete";
			mc.Click += new EventHandler(DeleteMenuClick);
			return new MenuCommand[] { mc };
		}

		public override MessageCollection Verify()
		{
			try
			{
				var retval = new MessageCollection();
				retval.AddRange(base.Verify());

				var customAggregate = (CustomAggregate)this.Object;
				if(customAggregate.Generated)
				{
					//Check valid name
					if (!ValidationHelper.ValidDatabaseIdenitifer(customAggregate.DatabaseName))
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, customAggregate.Name), this);
					if (!ValidationHelper.ValidCodeIdentifier(customAggregate.PascalName))
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, customAggregate.Name), this);

					//Check Aggregate SQL
					if (string.IsNullOrEmpty(customAggregate.SQL))
						retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextSQLRequired, this);

					//Check that object has at least one generated column
					if(customAggregate.GeneratedColumns.Count() == 0)
						retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextColumnsRequired, this);

				}

				return retval;

			}
			catch(Exception ex)
			{
				throw;
			}

		}

		public override bool DeleteObject()
		{
			if (MessageBox.Show("Do you wish to delete this custom aggregate?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				((ModelRoot)this.Object.Root).Database.CustomAggregates.Remove(((CustomAggregate)this.Object).Id);
				this.Node.Remove();
				this.Object.Root.Dirty = true;
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

		#endregion

	}
}