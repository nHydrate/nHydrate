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
using System.Collections.Generic;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.ModelUI;

namespace nHydrate.Generator.Models
{
	public class DatabaseController : BaseModelObjectController
	{
		#region Member Variables

		private DatabaseControllerUIControl _uiControl = null;

		#endregion

		#region Constructor

		protected internal DatabaseController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Database";
			this.HeaderDescription = "This object defines settings for the physical database implementation";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Database);
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if (_node == null)
				{
					_node = new DatabaseNode(this);
				}
				return _node;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			return new MenuCommand[] { };
		}

		public override MessageCollection Verify()
		{
			try
			{
				var retval = new MessageCollection();
				retval.AddRange(base.Verify());

				var database = (Database)this.Object;

				//Check valid name
				//if (!ValidationHelper.ValidDatabaseIdenitifer(database.DatabaseName))
				//  retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextInvalidDatabase, this);

				//Check Relations
				var deleteList = new ArrayList();
				var checkList = new ArrayList();
				foreach (Relation relation in database.Relations)
				{
					var table1 = (Table)relation.ParentTableRef.Object;
					var table2 = (Table)relation.ChildTableRef.Object;
					if ((table1 != null) && (table2 != null))
					{
						var relationName = table1.Name + "." + table1.GetAbsoluteBaseTable().Name + "." + relation.PascalRoleName + "." + table2.Name;
						if (checkList.Contains(relationName))
						{
							INHydrateModelObjectController controller = this;
							var list = database.Relations.Find(relation.Key);
							if (list.Length > 0)
							{
								var key = ((Relation)list.GetValue(0)).Key;
								var nodeList = this.Node.Nodes.Find(key, true);
								if (nodeList.Length > 0)
									controller = ((ModelObjectTreeNode)nodeList[0]).Controller;
							}
							retval.Add(MessageTypeConstants.Error, String.Format(ValidationHelper.ErrorTextDuplicateRoleName, table1.Name, table2.Name), table1.Controller);
						}

						else
						{
							checkList.Add(relationName);
						}
					}
					else
					{
						deleteList.Add(relation);
						//TODO
						//retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextRelationMustHaveParentChild, null);
					}
				}

				//Remove errors
				foreach (Relation relation in deleteList)
					database.Relations.Remove(relation);

				//Validate audit fields
				var auditFieldList = new List<string>();
				auditFieldList.Add(database.CreatedByColumnName);
				if (!auditFieldList.Contains(database.CreatedDateColumnName))
					auditFieldList.Add(database.CreatedDateColumnName);
				if (!auditFieldList.Contains(database.ModifiedByColumnName))
					auditFieldList.Add(database.ModifiedByColumnName);
				if (!auditFieldList.Contains(database.ModifiedDateColumnName))
					auditFieldList.Add(database.ModifiedDateColumnName);
				if (!auditFieldList.Contains(database.TimestampColumnName))
					auditFieldList.Add(database.TimestampColumnName);

				if (auditFieldList.Count != 5)
				{
					retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextAuditFieldsNotUnique, database.Controller);
				}
				else
				{
					auditFieldList = new List<string>();
					auditFieldList.Add(database.CreatedByPascalName);
					if (!auditFieldList.Contains(database.CreatedDatePascalName))
						auditFieldList.Add(database.CreatedDatePascalName);
					if (!auditFieldList.Contains(database.ModifiedByPascalName))
						auditFieldList.Add(database.ModifiedByPascalName);
					if (!auditFieldList.Contains(database.ModifiedDatePascalName))
						auditFieldList.Add(database.ModifiedDatePascalName);
					if (!auditFieldList.Contains(database.TimestampPascalName))
						auditFieldList.Add(database.TimestampPascalName);

					if (auditFieldList.Count != 5)
						retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextAuditFieldsNotUnique, database.Controller);
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
			return false;
		}

		public override void Refresh()
		{
		}

		//public override ModelObjectUserInterface UIControl
		//{
		//  get
		//  {
		//    if (this._userInterface == null)
		//    {
		//      var ctrl = new PanelUIControl();
		//      _uiControl = new DatabaseControllerUIControl();
		//      _uiControl.Populate(this.Object as Database);
		//      _uiControl.Dock = System.Windows.Forms.DockStyle.Fill;
		//      ctrl.MainPanel.Controls.Add(_uiControl);
		//      ctrl.Dock = DockStyle.Fill;
		//      this._userInterface = ctrl;
		//    }
		//    this._userInterface.Enabled = this.IsEnabled;
		//    return this._userInterface;
		//  }
		//}

		#endregion

		#region Menu Handlers

		#endregion

	}
}