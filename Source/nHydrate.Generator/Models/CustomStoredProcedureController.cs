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
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class CustomStoredProcedureController : BaseModelObjectController
	{
		#region Member Variables

		#endregion

		#region Constructor

		protected internal CustomStoredProcedureController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Stored Procedure";
			this.HeaderDescription = "Defines settings for the selected stored procedure";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.StoredProc);
		}

		#endregion

		#region BaseModelObjectController Members
		public override ModelObjectTreeNode Node
		{
			get
			{
				if (_node == null)
				{
					_node = new CustomStoredProcedureNode(this);
				}
				return _node;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			var menuDelete = new DefaultMenuCommand();
			menuDelete.Text = "Delete";
			menuDelete.Click += new EventHandler(DeleteMenuClick);

			var menuSep = new DefaultMenuCommand();
			menuSep.Text = "-";

			var menuCopy = new DefaultMenuCommand();
			menuCopy.Text = "Copy";
			menuCopy.Click += new EventHandler(CopyMenuClick);

			return new MenuCommand[] { menuDelete, menuSep, menuCopy };
		}

		public override MessageCollection Verify()
		{
			try
			{
				var retval = new MessageCollection();
				retval.AddRange(base.Verify());

				var customStoredProcedure = (CustomStoredProcedure)this.Object;
				if (customStoredProcedure.Generated)
				{
					//Check valid name
					if (!ValidationHelper.ValidDatabaseIdenitifer(customStoredProcedure.DatabaseName))
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, customStoredProcedure.Name), this);
					if (!ValidationHelper.ValidCodeIdentifier(customStoredProcedure.PascalName))
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, customStoredProcedure.Name), this);

					//Check StoredProcedure SQL
					if (!customStoredProcedure.IsExisting && string.IsNullOrEmpty(customStoredProcedure.SQL))
					{
						retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextSQLRequired, this);
					}

					if (customStoredProcedure.IsExisting && string.IsNullOrEmpty(customStoredProcedure.DatabaseObjectName))
					{
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextExistingSPNeedsDBName, customStoredProcedure.Name), this);
					}

					//Check that object has at least one generated column
					//if (customStoredProcedure.GetColumns().Count(x => x.Generated) == 0)
					//	retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextColumnsRequired, this);

				}

				return retval;

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public override bool DeleteObject()
		{
			if (MessageBox.Show("Do you wish to delete this custom stored procedure?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				((ModelRoot)this.Object.Root).Database.CustomStoredProcedures.Remove(((CustomStoredProcedure)this.Object).Id);
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

		private void CopyMenuClick(object sender, System.EventArgs e)
		{
			try
			{
				var document = new XmlDocument();
				document.LoadXml("<a></a>");

				//Add a node
				var parentNode = document.CreateElement("storedprocedure");
				((CustomStoredProcedure)this.Object).XmlAppend(parentNode);
				document.DocumentElement.AppendChild(parentNode);

				//Add the columns
				var columnListNode = document.CreateElement("columnList");
				document.DocumentElement.AppendChild(columnListNode);
				foreach (Reference reference in ((CustomStoredProcedure)this.Object).Columns)
				{
					var columnNode = document.CreateElement("column");
					((CustomStoredProcedureColumn)reference.Object).XmlAppend(columnNode);
					columnListNode.AppendChild(columnNode);
				}

				//Add the parameters
				var parameterListNode = document.CreateElement("parameterList");
				document.DocumentElement.AppendChild(parameterListNode);
				foreach (Reference reference in ((CustomStoredProcedure)this.Object).Parameters)
				{
					var parameterNode = document.CreateElement("parameter");
					((Parameter)reference.Object).XmlAppend(parameterNode);
					parameterListNode.AppendChild(parameterNode);
				}

				Clipboard.SetData("ws.model.storedprocedure", document.OuterXml);

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

	}
}