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
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class CustomViewColumnController : BaseModelObjectController
	{
		#region Member Variables

		#endregion

		#region Constructor

		protected internal CustomViewColumnController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Field";
			this.HeaderDescription = "This defines the settings for the selected field";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Field);
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if(_node == null)
				{
					_node = new CustomViewColumnNode(this);
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
			try
			{
				var retval = new MessageCollection();
				retval.AddRange(base.Verify());

				var customViewColumn = (CustomViewColumn)this.Object;

				//Check valid name
				if (!ValidationHelper.ValidDatabaseIdenitifer(customViewColumn.DatabaseName))
					retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, customViewColumn.Name), customViewColumn.Controller);
				if (!ValidationHelper.ValidCodeIdentifier(customViewColumn.PascalName))
					retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, customViewColumn.Name), customViewColumn.Controller);
				if (!ValidationHelper.ValidFieldIdentifier(customViewColumn.PascalName))
					retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, customViewColumn.Name), customViewColumn.Controller);

				//Check valid name
				if ((!string.IsNullOrEmpty(customViewColumn.CodeFacade)) && !ValidationHelper.ValidDatabaseIdenitifer(customViewColumn.CodeFacade))
					retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextInvalidCodeFacade, customViewColumn.Controller);

				return retval;

			}
			catch(Exception ex)
			{
				throw;
			}

		}

		public override bool DeleteObject()
		{
			try
			{
				if(MessageBox.Show("Do you wish to delete the selected customViewColumn?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					return false;

				var parentNode = (ModelObjectTreeNode)this.Node.Parent;
				var referenceCollection = (ReferenceCollection)parentNode.Object;
				var customViewColumnCollection = ((ModelRoot)this.Object.Root).Database.CustomViewColumns;

				foreach(Reference reference in referenceCollection)
				{
					if(reference.Object == this.Object)
					{
						parentNode.Controller.Refresh();
						customViewColumnCollection.Remove((CustomViewColumn)reference.Object);
						referenceCollection.Remove(reference);
						break;
					}
				}

				this.Node.Remove();
				this.Object.Root.Dirty = true;
				return true;
			}
			catch(Exception ex)
			{
				throw;
			}

		}

		public override void Refresh()
		{
		}

		#endregion

		#region Menu Handlers

		private void MenuClick(object sender, System.EventArgs e)
		{
		}

		#endregion

	}
}