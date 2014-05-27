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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.ModelUI;

namespace nHydrate.Generator.Models
{
	public class ColumnController : BaseModelObjectController
	{
		#region Member Variables

		private FieldControllerUIControl _uiControl = null;

		#endregion

		#region Constructor

		protected internal ColumnController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Field";
			this.HeaderDescription = "This defines the settings for the selected field";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Field);
		}

		#endregion

		#region BaseModelObjectController Members

		public override bool IsEnabled
		{
			get { return (this.Node.Tag != null); }
		}

		public override ModelObjectTreeNode Node
		{
			get
			{
				if (_node == null)
				{
					_node = new ColumnNode(this);
				}
				return _node;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			if (this.Node.Parent.Parent is TableNode)
			{
				var node = this.Node.Parent.Parent as TableNode;
				var table = node.Object as Table;
				var table2 = ((Column)this.Object).ParentTableRef.Object as Table;

				var menuItems = new List<MenuCommand>();
				if (table == table2)
				{
					var menuDelete = new DefaultMenuCommand();
					menuDelete.Text = "Delete";
					menuDelete.Click += new EventHandler(DeleteMenuClick);
					menuItems.Add(menuDelete);

					var menuSep = new DefaultMenuCommand();
					menuSep.Text = "-";
					menuItems.Add(menuSep);
				}

				var menuCopy = new DefaultMenuCommand();
				menuCopy.Text = "Copy";
				menuCopy.Click += new EventHandler(CopyMenuClick);
				menuItems.Add(menuCopy);

				return menuItems.ToArray();
			}
			else
			{
				return new MenuCommand[] { };
			}
		}

		public override MessageCollection Verify()
		{
			try
			{
				var retval = new MessageCollection();
				retval.AddRange(base.Verify());

				var column = (Column)this.Object;

				#region Check valid name
				if (!ValidationHelper.ValidDatabaseIdenitifer(column.DatabaseName))
					retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, column.Name), column.Controller);
				if (!ValidationHelper.ValidCodeIdentifier(column.PascalName))
					retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, column.Name), column.Controller);
				if (!ValidationHelper.ValidFieldIdentifier(column.PascalName))
					retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, column.Name), column.Controller);

				#endregion

				#region Check valid name based on codefacade
				if ((!string.IsNullOrEmpty(column.CodeFacade)) && !ValidationHelper.ValidDatabaseIdenitifer(column.CodeFacade))
					retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextInvalidCodeFacade, column.Controller);

				if (ModelHelper.IsNumericType(column.DataType))
				{
					if ((column.Min >= double.MinValue) && (column.Max >= double.MinValue))
					{
						if (column.Min > column.Max)
							retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextMinMaxValueMismatch, column.Controller);
					}
				}
				else //Non-numeric
				{
					//Neither should be set
					if ((column.Min >= double.MinValue) || (column.Max >= double.MinValue))
					{
						retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextMinMaxValueInvalidType, column.Controller);
					}
				}
				#endregion

				#region Validate identity field
				if (column.Identity != IdentityTypeConstants.None)
				{
					//If this is an identity then must be of certain datatype
					switch (column.DataType)
					{
						//case System.Data.SqlDbType.TinyInt:
						case System.Data.SqlDbType.SmallInt:
						case System.Data.SqlDbType.Int:
						case System.Data.SqlDbType.BigInt:
						case System.Data.SqlDbType.UniqueIdentifier:
							break;
						default:
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentityColumn, column.Name), column.Controller);
							break;
					}
				}
				#endregion

				#region Varchar Max only supported in SQL 2008

				//if (((ModelRoot)column.Root).SQLServerType == SQLServerTypeConstants.SQL2005)
				//{
				//  if (ModelHelper.SupportsMax(column.DataType) && column.Length == 0)
				//  {
				//    retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextColumnMaxNotSupported, column.Name), column.Controller);
				//  }
				//}

				#endregion

				#region Columns cannot be 0 length

				if (!ModelHelper.SupportsMax(column.DataType) && column.Length == 0)
				{
					retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextColumnLengthNotZero, column.Name), column.Controller);
				}

				#endregion

				#region Validate Decimals

				if (column.DataType == System.Data.SqlDbType.Decimal)
				{
					if (column.Length < 1 || column.Length > 38)
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextColumnDecimalPrecision, column.Name), column.Controller);
					if (column.Scale < 0 || column.Scale > column.Length)
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextColumnDecimalScale, column.Name), column.Controller);
				}

				#endregion

				#region Verify Datatypes for SQL 2005/2008

				if (!Column.IsSupportedType(column.DataType, ((ModelRoot)this.Object.Root).SQLServerType))
				{
					retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextDataTypeNotSupported, column.Name), column.Controller);
				}

				#endregion

				#region Computed Column

				if (column.ComputedColumn)
				{
					if (column.Formula.Trim() == "")
					{
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextComputeColumnNoFormula, column.Name), column.Controller);
					}

					if (column.PrimaryKey)
					{
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextComputeColumnNoPK, column.Name), column.Controller);
					}

				}

				if (!column.ComputedColumn && !string.IsNullOrEmpty(column.Formula))
				{
					retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextComputeNonColumnHaveFormula, column.Name), column.Controller);
				}

				#endregion

				#region Validate Defaults

				if (!string.IsNullOrEmpty(column.Default))
				{
					if (!column.CanHaveDefault())
					{
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextColumnCannotHaveDefault, column.Name), column.Controller);
					}
					else if (!column.IsValidDefault())
					{
						retval.Add(MessageTypeConstants.Warning, string.Format(ValidationHelper.ErrorTextColumnInvalidDefault, column.Name), column.Controller);
					}
				}

				#endregion

				#region Check Decimals for common error

				if (column.DataType == System.Data.SqlDbType.Decimal)
				{
					if (column.Length == 1)
						retval.Add(MessageTypeConstants.Warning, string.Format(ValidationHelper.ErrorTextDecimalColumnTooSmall, column.Name, column.Length.ToString()), column.Controller);
				}

				#endregion

				return retval;

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public override bool DeleteObject()
		{
			try
			{
				//Is an inherited column? If so then skip out
				if (this.Node.Tag == null) return false;

				if (MessageBox.Show("Do you wish to delete the selected field?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					return false;

				var parentNode = (ModelObjectTreeNode)this.Node.Parent;
				var referenceCollection = (ReferenceCollection)parentNode.Object;
				var columnCollection = ((ModelRoot)this.Object.Root).Database.Columns;

				foreach (Reference reference in referenceCollection)
				{
					if (reference.Object == this.Object)
					{
						var column = reference.Object as Column;

						//Remove from any Table Components
						foreach (TableComponent tableComponent in ((Table)column.ParentTableRef.Object).ComponentList)
						{
							foreach (Reference r in tableComponent.Columns)
							{
								var c = (Column)r.Object;
								if (c == column)
								{
									tableComponent.Columns.Remove(r);
									break;
								}
							}
						}

						//Remove actual column
						columnCollection.Remove(column);

						//Remove column reference
						referenceCollection.Remove(reference);
						break;
					}
				}

				this.Node.Remove();
				this.Object.Root.Dirty = true;
				parentNode.Controller.Refresh();
				((ModelObjectTreeNode)parentNode.Parent).Refresh();

				//Refresh all column controllers on derived tables
				foreach (var table in ((Table)((Column)this.Object).ParentTableRef.Object).GetTablesInheritedFromHierarchy())
				{
					table.Controller.OnItemChanged(this, new System.EventArgs());
				}

				return true;
			}
			catch (Exception ex)
			{
				throw;
			}

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
		//      _uiControl = new FieldControllerUIControl();
		//      _uiControl.Populate(this.Object as Column);
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

		private void DeleteMenuClick(object sender, System.EventArgs e)
		{
			this.DeleteObject();
		}

		private void CopyMenuClick(object sender, System.EventArgs e)
		{
			var document = new XmlDocument();
			document.LoadXml("<a></a>");
			((Column)this.Object).XmlAppend(document.DocumentElement);
			Clipboard.SetData("ws.model.column", document.OuterXml);
		}

		#endregion

	}
}