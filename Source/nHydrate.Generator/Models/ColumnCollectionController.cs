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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Forms;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Forms;

namespace nHydrate.Generator.Models
{
	public class ColumnCollectionController : BaseModelObjectController
	{
		#region Member Variables

		private ContextMenu PopupMenu = null;
		private ListView ControllerListView = null;

		#endregion

		#region Constructor

		protected internal ColumnCollectionController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Fields";
			this.HeaderDescription = "This is a list of fields for the parent entity";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Fields);
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if (_node == null)
				{
					_node = new ColumnCollectionNode(this);
				}
				return _node;
			}
		}

		public override ModelObjectUserInterface UIControl
		{
			get
			{
				try
				{
					if (this.UserInterface == null)
					{
						var ctrl = new PanelUIControl();
						ControllerListView = new ListView();
						ControllerListView.View = View.Details;
						ControllerListView.HideSelection = false;
						ControllerListView.FullRowSelect = true;
						ControllerListView.Dock = DockStyle.Fill;
						ControllerListView.Columns.Add("Name", 100, HorizontalAlignment.Left);
						ControllerListView.Columns.Add("Code Facade", 100, HorizontalAlignment.Left);
						ControllerListView.Columns.Add("Primary Key", 100, HorizontalAlignment.Left);
						ControllerListView.Columns.Add("Allow Null", 100, HorizontalAlignment.Left);
						ControllerListView.Columns.Add("Type", 100, HorizontalAlignment.Left);
						ControllerListView.Columns.Add("Length", 50, HorizontalAlignment.Right);
						var typeList = new System.Type[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(int) };
						var comparer = new ListViewComparer(ControllerListView, typeList);
						ControllerListView.ListViewItemSorter = comparer;
						comparer.Column = 0;
						ControllerListView.Sorting = SortOrder.Ascending;
						ControllerListView.SmallImageList = nHydrate.Generator.ImageHelper.GetImageList();
						ctrl.MainPanel.Controls.Add(ControllerListView);
						ctrl.Dock = DockStyle.Fill;

						ControllerListView.KeyUp += new KeyEventHandler(listView_KeyUp);
						ControllerListView.DoubleClick += new EventHandler(listView_DoubleClick);

						try
						{
							if (((ReferenceCollection)this.Object).Parent is Table)
							{
								var menuItems = new List<MenuItem>();
								this.PopupMenu = new ContextMenu();
								ControllerListView.ContextMenu = this.PopupMenu;

								var newMenu = new MenuItem("Delete");
								newMenu.Click += new EventHandler(menuDelete_Click);
								menuItems.Add(newMenu);

								newMenu = new MenuItem("-");
								menuItems.Add(newMenu);

								newMenu = new MenuItem("Copy");
								newMenu.Click += new EventHandler(menuCopy_Click);
								menuItems.Add(newMenu);

								newMenu = new MenuItem("Paste");
								newMenu.Click += new EventHandler(menuPaste_Click);
								menuItems.Add(newMenu);

								this.PopupMenu.MenuItems.AddRange(menuItems.ToArray());

							}
						}
						catch (Exception ex)
						{
							throw;
						}

						this.UserInterface = ctrl;
					}

					this.ReloadControl();
					return this.UserInterface;

				}
				catch (Exception ex)
				{
					throw;
				}

			}
		}

		private void ReloadControl()
		{
			if (ControllerListView != null)
				ControllerListView.BeginUpdate();

			try
			{
				var referenceCollection = (ReferenceCollection)this.Object;
				Table parentTable = null;
				if (referenceCollection.Parent is Table)
					parentTable = (Table)referenceCollection.Parent;

				//Load the list
				var shallowColumnNames = new List<string>();
				ControllerListView.Items.Clear();
				foreach (Reference reference in referenceCollection)
				{
					var column = ((Column)reference.Object);
					shallowColumnNames.Add(column.Name.ToLower());
					this.AddColumnNode(reference, column);
				}

				//Load the inherited columns
				if (parentTable != null)
				{
					var allTables = new List<Table>(parentTable.GetTableHierarchy());
					allTables.Remove(parentTable);

					//Loop through all base tables and list columns that are NOT included in this table (the PK/audit columns)
					foreach (var table in allTables)
					{
						foreach (var column in table.GeneratedColumns)
						{
							if (!shallowColumnNames.Contains(column.Name.ToLower()))
							{
								shallowColumnNames.Add(column.Name.ToLower()); //just in case, no duplicates
								this.AddColumnNode(column);
							}
						}
					}
				}

			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				if (ControllerListView != null)
					ControllerListView.EndUpdate();
			}
		}

		private void AddColumnNode(Column column)
		{
			this.AddColumnNode(null, column);
		}

		private void AddColumnNode(Reference reference, Column column)
		{
			var newItem = new ListViewItem(column.Name);

			var inRelation = false;
			var parentRelations = ((ModelRoot)this.Object.Root).Database.Relations.FindByChildTable((Table)column.ParentTableRef.Object);
			foreach (var relation in parentRelations)
			{
				foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
				{
					if (columnRelationship.ChildColumnRef.Object == column)
						inRelation = true;
				}
			}

			if (column.PrimaryKey)
				newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.ColumnPrimaryKey);
			else if (inRelation)
				newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.ColumnForeignKey);
			else
				newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.Column);

			//There is not a reference for inherited columns
			if (reference != null)
			{
				newItem.Tag = reference;
				newItem.Name = reference.Key;
			}
			else
			{
				newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.ColumnInherit);
				newItem.ForeColor = SystemColors.GrayText;
			}

			newItem.SubItems.Add(column.CodeFacade);
			newItem.SubItems.Add(column.PrimaryKey.ToString());
			newItem.SubItems.Add(column.AllowNull.ToString());
			newItem.SubItems.Add(column.DataType.ToString().ToLower());
			newItem.SubItems.Add(column.Length.ToString());
			ControllerListView.Items.Add(newItem);
		}

		private void listView_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				if (this.ControllerListView.SelectedItems.Count == 1)
				{
					var item = this.ControllerListView.SelectedItems[0];
					var key = string.Empty;
					if (item.Tag != null) key = ((INHydrateModelObject)item.Tag).Key;
					var nodeList = this.Node.Nodes.Find(key, false);
					if (nodeList.Length > 0)
					{
						if (this.Node.TreeView != null)
							this.Node.TreeView.SelectedNode = nodeList[0];
					}
					else
					{
						var node = this.Node.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Text == item.Text);
						if (node != null) this.Node.TreeView.SelectedNode = node;
					}

				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void listView_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				menuDelete_Click(null, null);
			}
			else if ((e.KeyCode == Keys.A) && (e.Control))
			{
				this.ControllerListView.SelectAll();
			}
			else if ((e.KeyCode == Keys.C) && (e.Control))
			{
				//Copy
				this.CopySelected();
			}
			else if ((e.KeyCode == Keys.V) && (e.Control))
			{
				//Paste
				this.PasteClipboard();
			}
		}

		private void menuDelete_Click(object sender, System.EventArgs e)
		{
			var selecetdItems = new List<ListViewItem>();
			selecetdItems.AddRange(ControllerListView.SelectedList());

			if (selecetdItems.Count(x => x.Tag != null) == 0)
			{
				MessageBox.Show("There are no items selected that can be deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			else if (ControllerListView.SelectedItems.Count == 0)
			{
				MessageBox.Show("There are no items selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			else if (MessageBox.Show("Do you wish to delete the selected items?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
			{
				return;
			}

			var deletedList = new List<ListViewItem>();
			foreach (ListViewItem item in ControllerListView.SelectedItems)
			{
				if (item.Tag != null)
				{
					((ReferenceCollection)this.Object).Remove(((Reference)item.Tag));
					deletedList.Add(item);
				}
			}

			//Remove the item from the listview
			foreach (var item in deletedList)
			{
				//Remove the item from the tree
				this.Node.Nodes.Remove(this.Node.Nodes[item.Name]);
				//Remove Listitem
				ControllerListView.Items.Remove(item);
			}

			if (deletedList.Count == 0)
			{
				MessageBox.Show("There were no columns from the current table selected to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				//Refresh all column controllers on derived tables
				foreach (var table in ((Table)((Column)((Reference)deletedList[0].Tag).Object).ParentTableRef.Object).GetTablesInheritedFromHierarchy())
				{
					table.Controller.OnItemChanged(this, new System.EventArgs());
				}

				this.Object.Root.Dirty = true;
			}

		}

		private void menuCopy_Click(object sender, System.EventArgs e)
		{
			this.CopySelected();
		}

		private void menuPaste_Click(object sender, System.EventArgs e)
		{
			this.PasteClipboard();
		}

		public override MenuCommand[] GetMenuCommands()
		{
			if (this.Node.Parent is TableNode)
			{
				var mcAddColumn = new DefaultMenuCommand();
				mcAddColumn.Text = "New Field";
				mcAddColumn.Click += new EventHandler(AddColumnMenuClick);

				var mcImport = new DefaultMenuCommand();
				mcImport.Text = "Import Bulk...";
				mcImport.Click += new EventHandler(ImportBulkColumnMenuClick);

				var menuUpdateColumnName = new DefaultMenuCommand();
				menuUpdateColumnName.Text = "Update Field Names...";
				menuUpdateColumnName.Click += new EventHandler(UpdateColumnNameMenuClick);

				var menuUpdateCodeFace = new DefaultMenuCommand();
				menuUpdateCodeFace.Text = "Update Field Codefacades...";
				menuUpdateCodeFace.Click += new EventHandler(UpdateCodefacadeMenuClick);

				var menuSep = new DefaultMenuCommand();
				menuSep.Text = "-";

				var menuPaste = new DefaultMenuCommand();
				menuPaste.Text = "Paste";
				menuPaste.Click += new EventHandler(PasteMenuClick);

				return new MenuCommand[] { mcAddColumn, mcImport, menuUpdateColumnName, menuUpdateCodeFace, menuSep, menuPaste };
			}
			else
			{
				return new MenuCommand[] { };
			}
		}

		private void CopySelected()
		{
			try
			{
				var document = new XmlDocument();
				document.LoadXml("<a></a>");
				foreach (ListViewItem item in ControllerListView.SelectedItems)
				{
					if (item.Tag != null)
					{
						//Add a column node
						var column = ((Column)((Reference)item.Tag).Object);
						var columnNode = document.CreateElement("column");
						column.XmlAppend(columnNode);
						document.DocumentElement.AppendChild(columnNode);
					}
				}
				Clipboard.SetData("ws.model.columncollection", document.OuterXml);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void PasteClipboard()
		{
			try
			{
				if (Clipboard.ContainsData("ws.model.columncollection"))
				{
					var document = new XmlDocument();
					document.LoadXml((string)Clipboard.GetData("ws.model.columncollection"));
					foreach (XmlNode node in document.DocumentElement.SelectNodes("column"))
					{
						this.PasteColumn(node);
					}
					this.OnItemChanged(this, new System.EventArgs());
				}
				else if (Clipboard.ContainsData("ws.model.column"))
				{
					var document = new XmlDocument();
					document.LoadXml((string)Clipboard.GetData("ws.model.column"));
					this.PasteColumn(document.DocumentElement);
					this.OnItemChanged(this, new System.EventArgs());
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void PasteColumn(XmlNode columnNode)
		{
			try
			{
				var column = ((ModelRoot)this.Object.Root).Database.Columns.Add();
				var id = column.Id;
				column.XmlLoad(columnNode);
				column.SetId(id);
				((ReferenceCollection)this.Object).Add(column.CreateRef());
				var tableRef = ((Table)((ModelObjectTreeNode)this.Node.Parent).Controller.Object).CreateRef();
				column.ParentTableRef = tableRef;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public override MessageCollection Verify()
		{
			try
			{
				var retval = new MessageCollection();
				retval.AddRange(base.Verify());

				if (!(((ReferenceCollection)this.Object).Parent is Table))
					return retval;

				var referenceCollection = (ReferenceCollection)this.Object;
				var columnList = new List<Column>();
				foreach (Reference reference in referenceCollection)
				{
					var column = (Column)reference.Object;
					columnList.Add(column);
				}

				//Check for duplicate names
				var nameList = new Hashtable();
				foreach (var column in columnList)
				{
					var name = column.Name.ToLower();
					if (nameList.ContainsKey(name))
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextDuplicateName, column.Name), column.Controller);
					else
						nameList.Add(name, string.Empty);
				}

				if (retval.Count == 0)
				{
					//Check for duplicate codefacades
					nameList = new Hashtable();
					foreach (var column in columnList)
					{
						var name = column.PascalName.ToLower();
						if (nameList.ContainsKey(name))
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextDuplicateCodeFacade, column.Name), column.Controller);
						else
							nameList.Add(name, string.Empty);
					}
				}

				//Check for a primary key        
				var isPrimaryNull = false;
				foreach (var column in columnList)
				{
					if (column.PrimaryKey)
					{
						//hasPrimary = true;
						isPrimaryNull |= column.AllowNull;
					}
				}

				//Check for field named created,modfied,timestamp as these are taken
				foreach (var column in columnList)
				{
					var name = column.Name.ToLower().Replace("_", string.Empty);
					var t = (Table)column.ParentTableRef.Object;
					if (t.AllowCreateAudit)
					{
						//If there is a CreateAudit then no fields can be named the predined values
						if (string.Compare(name, ((ModelRoot)column.Root).Database.CreatedByColumnName.Replace("_", ""), true) == 0)
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextPreDefinedNameField, name), column.Controller);
						else if (string.Compare(name, ((ModelRoot)column.Root).Database.CreatedDateColumnName.Replace("_", ""), true) == 0)
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextPreDefinedNameField, name), column.Controller);
					}
					if (t.AllowModifiedAudit)
					{
						if (string.Compare(name, ((ModelRoot)column.Root).Database.ModifiedByColumnName.Replace("_", ""), true) == 0)
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextPreDefinedNameField, name), column.Controller);
						else if (string.Compare(name, ((ModelRoot)column.Root).Database.ModifiedDateColumnName.Replace("_", ""), true) == 0)
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextPreDefinedNameField, name), column.Controller);
					}
					if (t.AllowTimestamp)
					{
						if (string.Compare(name, ((ModelRoot)column.Root).Database.TimestampColumnName.Replace("_", ""), true) == 0)
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextPreDefinedNameField, name), column.Controller);
					}
				}

				if (columnList.Count != 0)
				{
					var parentTable = (Table)columnList[0].ParentTableRef.Object;
					if (parentTable.Generated)
					{
						//Make sure all PK are generated
						if (parentTable.PrimaryKeyColumns.Count != columnList.Count(x => x.Generated && x.PrimaryKey == true))
							retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextNoPrimaryKey, parentTable.Controller);
						else if (parentTable.PrimaryKeyColumns.Count == 0)
							retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextNoPrimaryKey, parentTable.Controller);
						else if (isPrimaryNull)
							retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextPrimaryKeyNull, parentTable.Controller);
					}
				}
				return retval;

			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				Application.DoEvents();
			}
		}

		public override bool DeleteObject()
		{
			return false;
		}

		public override void Refresh()
		{
			//this.UserInterface = null;
			this.ReloadControl();
			this.Node.Refresh();
		}

		#endregion

		#region Menu Handlers

		private void UpdateCodefacadeMenuClick(object sender, System.EventArgs e)
		{
			var list = (ReferenceCollection)this.Object;
			var F = new ColumnCodeFacadeUpdateForm(list, null, ColumnCodeFacadeUpdateForm.FieldSettingConstants.CodeFacade);
			F.ShowDialog();
		}

		private void UpdateColumnNameMenuClick(object sender, System.EventArgs e)
		{
			var F = new ColumnCodeFacadeUpdateForm((ReferenceCollection)this.Object, null, ColumnCodeFacadeUpdateForm.FieldSettingConstants.Name);
			F.ShowDialog();
		}

		private void ImportBulkColumnMenuClick(object sender, System.EventArgs e)
		{
			var table = (Table)((ModelObjectTreeNode)this.Node.Parent).Controller.Object;
			var columnList = (ReferenceCollection)this.Object;
			var F = new ImportColumns(table, columnList);
			if (F.ShowDialog() == DialogResult.OK)
			{
				this.OnItemChanged(this, new System.EventArgs());
				table.Controller.OnItemChanged(this, new System.EventArgs());
			}
		}

		private void AddColumnMenuClick(object sender, System.EventArgs e)
		{
			var column = ((ModelRoot)this.Object.Root).Database.Columns.Add();
			((ReferenceCollection)this.Object).Add(column.CreateRef());
			var tableRef = ((Table)((ModelObjectTreeNode)this.Node.Parent).Controller.Object).CreateRef();
			column.ParentTableRef = tableRef;
			this.OnItemChanged(this, new System.EventArgs());

			//Refresh all column controllers on derived tables
			foreach (var table in ((Table)column.ParentTableRef.Object).GetTablesInheritedFromHierarchy())
			{
				table.Controller.OnItemChanged(this, new System.EventArgs());
			}

		}

		private void PasteMenuClick(object sender, System.EventArgs e)
		{
			this.PasteClipboard();
		}

		#endregion

	}
}