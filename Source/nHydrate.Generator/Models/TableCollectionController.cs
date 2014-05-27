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
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Forms;

namespace nHydrate.Generator.Models
{
	public class TableCollectionController : BaseModelObjectController
	{
		#region Member Variables

		private ContextMenu PopupMenu = null;
		private ListView ControllerListView = null;

		#endregion

		#region Constructor

		protected internal TableCollectionController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Entities";
			this.HeaderDescription = "This is a list of entities defined for this model";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Entities);
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if (_node == null)
				{
					_node = new TableCollectionNode(this);
				}
				return _node;
			}
		}

		public override ModelObjectUserInterface UIControl
		{
			get
			{
				if (this.UserInterface == null)
				{
					var ctrl = new PanelUIControl();
					ControllerListView = new ListView();
					ControllerListView.View = View.Details;
					ControllerListView.HideSelection = false;
					ControllerListView.FullRowSelect = true;
					ControllerListView.Dock = DockStyle.Fill;
					ControllerListView.Columns.Add("Name", 250, HorizontalAlignment.Left);
					ControllerListView.Columns.Add("Code Facade", 250, HorizontalAlignment.Left);
					ControllerListView.Columns.Add("Fields", 150, HorizontalAlignment.Left);
					ControllerListView.Columns.Add("Schema", 100, HorizontalAlignment.Left);
					var typeList = new System.Type[] { typeof(string), typeof(string), typeof(int) };
					var comparer = new nHydrate.Generator.Common.Forms.ListViewComparer(ControllerListView, typeList);
					ControllerListView.ListViewItemSorter = comparer;
					comparer.Column = 0;
					ControllerListView.Sorting = SortOrder.Ascending;
					ControllerListView.SmallImageList = nHydrate.Generator.ImageHelper.GetImageList();
					ctrl.MainPanel.Controls.Add(ControllerListView);
					ctrl.Dock = DockStyle.Fill;

					ControllerListView.KeyUp += new KeyEventHandler(listView_KeyUp);
					ControllerListView.DoubleClick += new EventHandler(listView_DoubleClick);

					var menuItems = new List<MenuCommand>();
					this.PopupMenu = new ContextMenu();
					ControllerListView.ContextMenu = this.PopupMenu;

					menuItems.AddMenuItem("Delete", new EventHandler(menuDelete_Click));
					menuItems.AddMenuItem("-");
					menuItems.AddMenuItem("Copy",new EventHandler(menuCopy_Click));
					menuItems.AddMenuItem("Paste", new EventHandler(menuPaste_Click));
					this.PopupMenu.MenuItems.AddRange(menuItems.ToArray());

					this.UserInterface = ctrl;
				}

				this.ReloadControl();

				return this.UserInterface;
			}
		}

		private void ReloadControl()
		{
			if (ControllerListView != null)
				ControllerListView.BeginUpdate();

			try
			{
				//Load the list
				ControllerListView.Items.Clear();
				var list = (TableCollection)this.Object;
				foreach (var table in list.OrderBy(x => x.Name))
				{
					var newItem = new ListViewItem(table.Name);
					newItem.Tag = table;
					newItem.Name = table.Key;

					if (!table.Generated)
						newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.TableNonGen);
					else if (table.AssociativeTable)
						newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.TableAssociative);
					else if (table.IsTypeTable)
						newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.TableType);
					else if (table.ParentTable != null)
						newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.TableDerived);
					else
						newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.Table);

					newItem.SubItems.Add(table.CodeFacade);
					newItem.SubItems.Add(table.Columns.Count.ToString());
					newItem.SubItems.Add(string.IsNullOrEmpty(table.DBSchema) ? "dbo" : table.DBSchema);
					ControllerListView.Items.Add(newItem);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				if (this.ControllerListView != null) 
					this.ControllerListView.EndUpdate();
			}
		}

		private void listView_DoubleClick(object sender, System.EventArgs e)
		{
			if (this.ControllerListView.SelectedItems.Count == 1)
			{
				var item = this.ControllerListView.SelectedItems[0];
				var nodeList = this.Node.Nodes.Find(((Table)item.Tag).Key, false);
				if (nodeList.Length > 0)
				{
					if (this.Node.TreeView != null)
						this.Node.TreeView.SelectedNode = nodeList[0];
				}
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
				//Select All
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

		private void menuCopy_Click(object sender, System.EventArgs e)
		{
			this.CopySelected();
		}

		private void menuPaste_Click(object sender, System.EventArgs e)
		{
			this.PasteClipboard();
		}

		private void menuDelete_Click(object sender, System.EventArgs e)
		{
			if (ControllerListView.SelectedItems.Count == 0)
			{
				MessageBox.Show("There are no items selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			else if (MessageBox.Show("Do you wish to delete the selected items?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
			{
				return;
			}

			foreach (ListViewItem item in ControllerListView.SelectedItems)
				((TableCollection)this.Object).Remove(((Table)item.Tag).Id);

			//Remove the item from the listview
			for (var ii = ControllerListView.SelectedItems.Count - 1; ii >= 0; ii--)
			{
				//Remove the item from the tree
				this.Node.Nodes.Remove(this.Node.Nodes[ControllerListView.SelectedItems[ii].Name]);
				//Remove Listitem
				ControllerListView.Items.Remove(ControllerListView.SelectedItems[ii]);
			}

			this.Object.Root.Dirty = true;

		}

		public override MenuCommand[] GetMenuCommands()
		{
			var mcAddTable = new DefaultMenuCommand();
			mcAddTable.Text = "New Entity";
			mcAddTable.Click += new EventHandler(AddTableMenuClick);

			var menuSep5 = new DefaultMenuCommand();
			menuSep5.Text = "-";

			var menuUpdateTableName = new DefaultMenuCommand();
			menuUpdateTableName.Text = "Update Entity Names...";
			menuUpdateTableName.Click += new EventHandler(UpdateTableNameMenuClick);

			var menuUpdateCodeFace = new DefaultMenuCommand();
			menuUpdateCodeFace.Text = "Update Entity Codefacades...";
			menuUpdateCodeFace.Click += new EventHandler(UpdateCodefacadeMenuClick);

			var menuSep4 = new DefaultMenuCommand();
			menuSep4.Text = "-";

			var menuUpdateColumnName = new DefaultMenuCommand();
			menuUpdateColumnName.Text = "Update Field Names...";
			menuUpdateColumnName.Click += new EventHandler(UpdateColumnNameColumnMenuClick);

			var menuUpdateColumnCodeFace = new DefaultMenuCommand();
			menuUpdateColumnCodeFace.Text = "Update Field Codefacades...";
			menuUpdateColumnCodeFace.Click += new EventHandler(UpdateCodefacadeColumnMenuClick);

			var menuDeleteColumns = new DefaultMenuCommand();
			menuDeleteColumns.Text = "Delete Fields...";
			menuDeleteColumns.Click += new EventHandler(DeleteColumnsMenuClick);

			//MenuCommand menuSep = new DefaultMenuCommand();
			//menuSep.Text = "-";

			//MenuCommand mcAddUnitTest = new DefaultMenuCommand();
			//mcAddUnitTest.Text = "Add Unit Tests";
			//mcAddUnitTest.Click += new EventHandler(AddUnitTestMenuClick);

			//MenuCommand mcClearUnitTest = new DefaultMenuCommand();
			//mcClearUnitTest.Text = "Clear Unit Tests";
			//mcClearUnitTest.Click += new EventHandler(ClearUnitTestMenuClick);

			var menuSep2 = new DefaultMenuCommand();
			menuSep2.Text = "-";

			var mcAuditRoot = new DefaultMenuCommand();
			mcAuditRoot.Text = "Audits";

			var mcAddCreateAudit = new DefaultMenuCommand();
			mcAddCreateAudit.Text = "Add Create Audits";
			mcAddCreateAudit.Click += new EventHandler(AddCreateAuditMenuClick);
			mcAuditRoot.MenuItems.Add(mcAddCreateAudit);

			var mcRemoveCreateAudit = new DefaultMenuCommand();
			mcRemoveCreateAudit.Text = "Remove Create Audits";
			mcRemoveCreateAudit.Click += new EventHandler(RemoveCreateAuditMenuClick);
			mcAuditRoot.MenuItems.Add(mcRemoveCreateAudit);

			var menuSep10 = new DefaultMenuCommand();
			menuSep10.Text = "-";
			mcAuditRoot.MenuItems.Add(menuSep10);

			var mcAddModifyAudit = new DefaultMenuCommand();
			mcAddModifyAudit.Text = "Add Modify Audits";
			mcAddModifyAudit.Click += new EventHandler(AddModifyAuditMenuClick);
			mcAuditRoot.MenuItems.Add(mcAddModifyAudit);

			var mcRemoveModifyAudit = new DefaultMenuCommand();
			mcRemoveModifyAudit.Text = "Remove Modify Audits";
			mcRemoveModifyAudit.Click += new EventHandler(RemoveModifyAuditMenuClick);
			mcAuditRoot.MenuItems.Add(mcRemoveModifyAudit);

			var menuSep11 = new DefaultMenuCommand();
			menuSep11.Text = "-";
			mcAuditRoot.MenuItems.Add(menuSep11);

			var mcAddTimestampAudit = new DefaultMenuCommand();
			mcAddTimestampAudit.Text = "Add Timestamp Audits";
			mcAddTimestampAudit.Click += new EventHandler(AddTimestampAuditMenuClick);
			mcAuditRoot.MenuItems.Add(mcAddTimestampAudit);

			var mcRemoveTimestampAudit = new DefaultMenuCommand();
			mcRemoveTimestampAudit.Text = "Remove Timestamp Audits";
			mcRemoveTimestampAudit.Click += new EventHandler(RemoveTimestampAuditMenuClick);
			mcAuditRoot.MenuItems.Add(mcRemoveTimestampAudit);

			var menuSep3 = new DefaultMenuCommand();
			menuSep3.Text = "-";

			var menuPaste = new DefaultMenuCommand();
			menuPaste.Text = "Paste";
			menuPaste.Click += new EventHandler(PasteMenuClick);

			return new MenuCommand[] { 
				mcAddTable, menuSep5,
				menuUpdateTableName, menuUpdateCodeFace, menuSep4, 
				menuUpdateColumnName, menuUpdateColumnCodeFace, menuDeleteColumns,
				//menuSep, mcAddUnitTest, mcClearUnitTest, 
				menuSep2, 
				mcAuditRoot,
				menuSep3, menuPaste };
		}

		private void PasteTable(TableCollection tableCollection, XmlNode tableNode, XmlNode columnListNode)
		{
			try
			{
				var table = tableCollection.Add();
				var id = table.Id;
				table.XmlLoad(tableNode);
				table.SetId(id);
				table.SetKey(Guid.NewGuid().ToString());
				table.Name = "[" + table.Name + "]";
				table.Columns.Clear();
				table.Relationships.Clear();
				table.CustomRetrieveRules.Clear();

				var columnMapper = new Dictionary<int, int>();
				foreach (XmlNode child in columnListNode)
				{
					var column = ((ModelRoot)this.Object.Root).Database.Columns.Add();
					id = column.Id;
					column.XmlLoad(child);
					columnMapper.Add(column.Id, id);
					column.SetId(id);
					column.SetKey(Guid.NewGuid().ToString());
					table.Columns.Add(column.CreateRef());
					column.ParentTableRef = table.CreateRef();
				}

				foreach (RowEntry r in table.StaticData)
				{
					foreach (CellEntry cell in r.CellEntries)
					{
						if (columnMapper.ContainsKey(cell.ColumnRef.Ref))
						{
							var newID = columnMapper[cell.ColumnRef.Ref];
							cell.ColumnRef.Ref = newID;
						}
					}
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void CopySelected()
		{
			var document = new XmlDocument();
			document.LoadXml("<a></a>");

			foreach (ListViewItem item in ControllerListView.SelectedItems)
			{
				var table = item.Tag as Table;

				var tableContainerNode = document.CreateElement("z");
				document.DocumentElement.AppendChild(tableContainerNode);

				//Add a table node
				var tableNode = document.CreateElement("table");
				table.XmlAppend(tableNode);
				tableContainerNode.AppendChild(tableNode);

				//Add the columns
				var columnListNode = document.CreateElement("columnList");
				tableContainerNode.AppendChild(columnListNode);
				foreach (Reference reference in table.Columns)
				{
					var column = reference.Object as Column;
					var columnNode = document.CreateElement("column");
					column.XmlAppend(columnNode);
					columnListNode.AppendChild(columnNode);
				}
			}

			Clipboard.SetData("ws.model.tablecollection", document.OuterXml);
		}

		private void PasteClipboard()
		{
			try
			{
				var tableCollection = (TableCollection)this.Object;
				if (Clipboard.ContainsData("ws.model.tablecollection"))
				{
					var document = new XmlDocument();
					document.LoadXml((string)Clipboard.GetData("ws.model.tablecollection"));
					foreach (XmlNode node in document.DocumentElement.SelectNodes("z"))
					{
						var tableNode = node.SelectSingleNode("table");
						var columnListNode = node.SelectSingleNode("columnList");
						this.PasteTable(tableCollection, tableNode, columnListNode);
					}
					this.OnItemChanged(this, new System.EventArgs());
				}
				else if (Clipboard.ContainsData("ws.model.table"))
				{
					var document = new XmlDocument();
					document.LoadXml((string)Clipboard.GetData("ws.model.table"));
					this.PasteTable(tableCollection,
						document.DocumentElement.SelectSingleNode("table"),
						document.DocumentElement.SelectSingleNode("columnList"));
					this.OnItemChanged(this, new System.EventArgs());
				}

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

				var tableCollection = (TableCollection)this.Object;

				#region Check for zero tables
				if (tableCollection.Count == 0)
				{
					retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextNoTables, this);
				}
				#endregion

				#region Check for duplicate names
				var nameList = new Hashtable();
				foreach (Table table in tableCollection)
				{
					if (table.Generated)
					{
						var name = table.Name.ToLower();
						if (nameList.ContainsKey(name))
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextDuplicateName, table.Name), table.Controller);
						else
							nameList.Add(name, string.Empty);
					}
				}
				#endregion

				#region Check for duplication relationship names

				var duplicateList = new Dictionary<string, RelationshipChecker>();
				foreach (Table table in tableCollection)
				{
					foreach (Reference reference in table.Relationships)
					{
						var relation = (Relation)reference.Object;
						var childTable = (Table)relation.ChildTableRef.Object;
						if (childTable != null)
						{
							var key = string.Empty;
							if (StringHelper.Match(table.Name, childTable.Name, true))
							{
								if (string.Compare(table.Name, childTable.Name, false) < 0)
									key = childTable.Name + "|" + relation.RoleName + "|" + table.Name;
								else
									key = table.Name + "|" + relation.RoleName + "|" + childTable.Name;
							}
							else
							{
								if (string.Compare(table.Name, childTable.Name, false) < 0)
									key = table.Name + "|" + relation.RoleName + "|" + childTable.Name;
								else
									key = childTable.Name + "|" + relation.RoleName + "|" + table.Name;
							}

							if (duplicateList.ContainsKey(key))
							{
								if (StringHelper.Match(table.Name, childTable.Name, true))
									duplicateList[key].TableList.Add(table);
								else duplicateList[key].TableList.Add(childTable);
							}
							else
							{
								var rc = new RelationshipChecker(relation);
								if (string.Compare(table.Name, childTable.Name, true) < 0)
									rc.TableList.Add(childTable);
								else rc.TableList.Add(table);
								duplicateList.Add(key, rc);
							}
						}

					}

				}

				foreach (var key in duplicateList.Keys)
				{
					if (duplicateList[key].TableList.Count > 1)
					{
						var t1 = ((Table)duplicateList[key].Relationship.ChildTableRef.Object).Name;
						var t2 = ((Table)duplicateList[key].Relationship.ParentTableRef.Object).Name;
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextConflictingRelationships, "'" + t1 + "' and '" + t2 + "'"), duplicateList[key].TableList[0].Controller);
					}
				}

				#endregion

				#region Check for duplicate codefacades
				if (retval.Count == 0)
				{
					nameList = new Hashtable();
					foreach (Table table in tableCollection)
					{
						if (table.Generated)
						{
							var name = table.PascalName.ToLower();
							if (nameList.ContainsKey(name))
								retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextDuplicateCodeFacade, table.Name), table.Controller);
							else
								nameList.Add(name, string.Empty);
						}
					}
				}
				#endregion

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

		private void UpdateCodefacadeColumnMenuClick(object sender, System.EventArgs e)
		{
			var model = (ModelRoot)((TableCollection)this.Object).Root;
			var F = new ColumnCodeFacadeUpdateForm(null, model, ColumnCodeFacadeUpdateForm.FieldSettingConstants.CodeFacade);
			F.ShowDialog();
		}

		private void DeleteColumnsMenuClick(object sender, System.EventArgs e)
		{
			try
			{
				var F = new DeleteColumnsForm((ModelRoot)((TableCollection)this.Object).Root);
				F.ShowDialog();

				if (F.Changed)
				{
					foreach (TableNode n in this.Node.Nodes)
					{
						if (n != null)
						{
							if (F.ChangedTables.Count(x => x.Name == ((Table)n.Object).Name) > 0)
								n.Refresh();
						}
					}

					this.Refresh();
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void UpdateColumnNameColumnMenuClick(object sender, System.EventArgs e)
		{
			var root = (ModelRoot)((TableCollection)this.Object).Root;
			var F = new ColumnCodeFacadeUpdateForm(null, root, ColumnCodeFacadeUpdateForm.FieldSettingConstants.Name);
			F.ShowDialog();
		}

		private void UpdateCodefacadeMenuClick(object sender, System.EventArgs e)
		{
			var list = (TableCollection)this.Object;
			var F = new TableCodeFacadeUpdateForm(list, TableCodeFacadeUpdateForm.FieldSettingConstants.CodeFacade);
			if (F.ShowDialog() == DialogResult.OK)
			{
				this.Refresh();
			}
		}

		private void UpdateTableNameMenuClick(object sender, System.EventArgs e)
		{
			try
			{
				var list = (TableCollection)this.Object;
				var F = new TableCodeFacadeUpdateForm(list, TableCodeFacadeUpdateForm.FieldSettingConstants.Name);
				if (F.ShowDialog() == DialogResult.OK)
				{
					this.Refresh();
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				throw;
			}
		}

		private void AddTableMenuClick(object sender, System.EventArgs e)
		{
			var table = ((TableCollection)this.Object).Add();
			this.OnItemChanged(this, new System.EventArgs());
		}

		//private void AddUnitTestMenuClick(object sender, System.EventArgs e)
		//{
		//  if (MessageBox.Show("This action will add unit tests to all tables. Do you wish to proceeed?", "Add Unit Tests?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		//  {
		//    TableCollection tableCollection = (TableCollection)this.Object;
		//    foreach (Table table in tableCollection)
		//    {
		//      if (!table.Immutable)
		//      {
		//        table.AddUnitTests();
		//      }
		//    }
		//    this.OnItemChanged(this, new System.EventArgs());
		//  }
		//}

		//private void ClearUnitTestMenuClick(object sender, System.EventArgs e)
		//{
		//  if (MessageBox.Show("This action will remove all unit tests from all tables. Do you wish to proceeed?", "Add Unit Tests?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		//  {
		//    TableCollection tableCollection = (TableCollection)this.Object;
		//    foreach (Table table in tableCollection)
		//    {
		//      table.UnitTestDependencies.Clear();
		//      table.AllowUnitTest = Table.UnitTestSettingsConstants.StubOnly;
		//    }
		//    this.OnItemChanged(this, new System.EventArgs());
		//  }
		//}

		private void AddCreateAuditMenuClick(object sender, System.EventArgs e)
		{
			var tableCollection = (TableCollection)this.Object;
			foreach (Table table in tableCollection)
			{
				table.AllowCreateAudit = true;
			}
		}

		private void RemoveCreateAuditMenuClick(object sender, System.EventArgs e)
		{
			var tableCollection = (TableCollection)this.Object;
			foreach (Table table in tableCollection)
			{
				table.AllowCreateAudit = false;
			}
			this.OnItemChanged(this, new System.EventArgs());
		}

		private void AddModifyAuditMenuClick(object sender, System.EventArgs e)
		{
			var tableCollection = (TableCollection)this.Object;
			foreach (Table table in tableCollection)
			{
				table.AllowModifiedAudit = true;
			}
			this.OnItemChanged(this, new System.EventArgs());
		}

		private void RemoveModifyAuditMenuClick(object sender, System.EventArgs e)
		{
			var tableCollection = (TableCollection)this.Object;
			foreach (Table table in tableCollection)
			{
				table.AllowModifiedAudit = false;
			}
			this.OnItemChanged(this, new System.EventArgs());
		}

		private void AddTimestampAuditMenuClick(object sender, System.EventArgs e)
		{
			var tableCollection = (TableCollection)this.Object;
			foreach (Table table in tableCollection)
			{
				table.AllowTimestamp = true;
			}
			this.OnItemChanged(this, new System.EventArgs());
		}

		private void RemoveTimestampAuditMenuClick(object sender, System.EventArgs e)
		{
			var tableCollection = (TableCollection)this.Object;
			foreach (Table table in tableCollection)
			{
				table.AllowTimestamp = false;
			}
			this.OnItemChanged(this, new System.EventArgs());
		}

		private void PasteMenuClick(object sender, System.EventArgs e)
		{
			this.PasteClipboard();
		}

		#endregion

		#region Helper Class

		private class RelationshipChecker
		{
			public RelationshipChecker(Relation relationship)
			{
				this.Relationship = relationship;
				this.TableList = new List<Table>();
			}

			public List<Table> TableList { get; set; }
			public Relation Relationship { get; set; }
		}

		#endregion

	}
}