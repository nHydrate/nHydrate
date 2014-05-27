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
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class CustomStoredProcedureCollectionController : BaseModelObjectController
	{
		#region Member Variables

		private ContextMenu PopupMenu = null;
		private ListView ControllerListView = null;

		#endregion

		#region Constructor

		protected internal CustomStoredProcedureCollectionController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Stored Procedures";
			this.HeaderDescription = "This is a list of stored procedures defined for this model";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.StoredProcs);
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if(_node == null)
				{
					_node = new CustomStoredProcedureCollectionNode(this);
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
					ControllerListView = new ListView();
					ControllerListView.View = View.Details;
					ControllerListView.HideSelection = false;
					ControllerListView.FullRowSelect = true;
					ControllerListView.Dock = DockStyle.Fill;
					ControllerListView.Columns.Add("Name", 250, HorizontalAlignment.Left);
					ControllerListView.Columns.Add("Code Facade", 250, HorizontalAlignment.Left);
					ControllerListView.Columns.Add("Fields", 250, HorizontalAlignment.Left);
					var typeList = new System.Type[] { typeof(string), typeof(string), typeof(int) };
					var comparer = new nHydrate.Generator.Common.Forms.ListViewComparer(ControllerListView, typeList);
					ControllerListView.ListViewItemSorter = comparer;
					comparer.Column = 0;
					ControllerListView.Sorting = SortOrder.Ascending;
					ControllerListView.SmallImageList = nHydrate.Generator.ImageHelper.GetImageList();
					ctrl.MainPanel.Controls.Add(ControllerListView);
					ctrl.Dock = DockStyle.Fill;

					ControllerListView.Sort();          
					ControllerListView.KeyUp += new KeyEventHandler(listView_KeyUp);
					ControllerListView.DoubleClick += new EventHandler(listView_DoubleClick);

					var menuItems = new List<MenuCommand>();
					this.PopupMenu = new ContextMenu();
					ControllerListView.ContextMenu = this.PopupMenu;

					menuItems.AddMenuItem("Delete", new EventHandler(menuDelete_Click));
					menuItems.AddMenuItem("-");
					menuItems.AddMenuItem("Copy", new EventHandler(menuCopy_Click));
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
			//Load the list
			ControllerListView.Items.Clear();
			foreach (CustomStoredProcedure customStoredProcedure in (CustomStoredProcedureCollection)this.Object)
			{
				var newItem = new ListViewItem(customStoredProcedure.Name);
				newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.CustomStoredProcedure);
				newItem.Tag = customStoredProcedure;
				newItem.Name = customStoredProcedure.Key;
				newItem.SubItems.Add(customStoredProcedure.CodeFacade);
				newItem.SubItems.Add(customStoredProcedure.Columns.Count.ToString());
				ControllerListView.Items.Add(newItem);
			}
		}

		private void listView_DoubleClick(object sender, System.EventArgs e)
		{
			if(this.ControllerListView.SelectedItems.Count == 1)
			{
				var item = this.ControllerListView.SelectedItems[0];
				var nodeList = this.Node.Nodes.Find(((CustomStoredProcedure)item.Tag).Key, false);
				if(nodeList.Length > 0)
				{
					if(this.Node.TreeView != null)
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
			try
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
					((CustomStoredProcedureCollection)this.Object).Remove(((CustomStoredProcedure)item.Tag).Id);

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
			catch (Exception ex)
			{
				throw;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			var mcAddCustomStoredProcedure = new DefaultMenuCommand();
			mcAddCustomStoredProcedure.Text = "New Stored Procedure";
			mcAddCustomStoredProcedure.Click += new EventHandler(AddCustomStoredProcedureMenuClick);

			var menuSep = new DefaultMenuCommand();
			menuSep.Text = "-";

			var menuPaste = new DefaultMenuCommand();
			menuPaste.Text = "Paste";
			menuPaste.Click += new EventHandler(PasteMenuClick);

			return new MenuCommand[] { mcAddCustomStoredProcedure, menuSep, menuPaste };
		}

		private void PasteTable(CustomStoredProcedureCollection customStoredProcedureCollection, 
			XmlNode customStoredProcedureNode,
			XmlNode columnListNode,
			XmlNode parameterListNode)
		{
			try
			{
				var customStoredProcedure = customStoredProcedureCollection.Add();
				var id = customStoredProcedure.Id;
				customStoredProcedure.XmlLoad(customStoredProcedureNode);
				customStoredProcedure.SetId(id);
				customStoredProcedure.SetKey(Guid.NewGuid().ToString());
				customStoredProcedure.Name = "[" + customStoredProcedure.Name + "]";
				customStoredProcedure.Columns.Clear();
				customStoredProcedure.Parameters.Clear();

				foreach (XmlNode child in columnListNode)
				{
					var column = ((ModelRoot)this.Object.Root).Database.CustomStoredProcedureColumns.Add();
					id = column.Id;
					column.XmlLoad(child);
					column.SetId(id);
					column.SetKey(Guid.NewGuid().ToString());
					customStoredProcedure.Columns.Add(column.CreateRef());
					column.ParentRef = customStoredProcedure.CreateRef();
				}

				//Parameters
				foreach (XmlNode node in parameterListNode)
				{
					var parameter = ((ModelRoot)this.Object.Root).Database.CustomRetrieveRuleParameters.Add();
					id = parameter.Id;
					parameter.XmlLoad(node);
					parameter.SetId(id);
					parameter.SetKey(Guid.NewGuid().ToString());
					customStoredProcedure.Parameters.Add(parameter.CreateRef());
					parameter.ParentTableRef = customStoredProcedure.CreateRef();
				}

			}
			catch (Exception ex)
			{
				throw;
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
					var customStoredProcedure = (CustomStoredProcedure)item.Tag;

					var tableContainerNode = document.CreateElement("z");
					document.DocumentElement.AppendChild(tableContainerNode);

					//Add a CustomStoredProcedure node
					var storedProcedureColumnNode = document.CreateElement("storedprocedure");
					customStoredProcedure.XmlAppend(storedProcedureColumnNode);
					tableContainerNode.AppendChild(storedProcedureColumnNode);

					//Add the columns
					var columnListNode = document.CreateElement("columnList");
					tableContainerNode.AppendChild(columnListNode);
					foreach (Reference reference in customStoredProcedure.Columns)
					{
						var column = (CustomStoredProcedureColumn)reference.Object;
						var columnNode = document.CreateElement("column");
						column.XmlAppend(columnNode);
						columnListNode.AppendChild(columnNode);
					}

					//Add the parameters
					var parameterListNode = document.CreateElement("parameterList");
					tableContainerNode.AppendChild(parameterListNode);
					foreach (Reference reference in customStoredProcedure.Parameters)
					{
						var parameterNode = document.CreateElement("parameter");
						((Parameter)reference.Object).XmlAppend(parameterNode);
						parameterListNode.AppendChild(parameterNode);
					}

				}

				Clipboard.SetData("ws.model.storedprocedurecollection", document.OuterXml);

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
				var customStoredProcedureCollection = (CustomStoredProcedureCollection)this.Object;
				if (Clipboard.ContainsData("ws.model.storedprocedurecollection"))
				{
					var document = new XmlDocument();
					document.LoadXml((string)Clipboard.GetData("ws.model.storedprocedurecollection"));
					foreach (XmlNode node in document.DocumentElement.SelectNodes("z"))
					{
						var tableNode = node.SelectSingleNode("storedprocedure");
						var columnListNode = node.SelectSingleNode("columnList");
						var parameterListNode = node.SelectSingleNode("parameterList");
						this.PasteTable(customStoredProcedureCollection, tableNode, columnListNode, parameterListNode);
					}
					this.OnItemChanged(this, new System.EventArgs());
				}
				else if (Clipboard.ContainsData("ws.model.storedprocedure"))
				{
					var document = new XmlDocument();
					document.LoadXml((string)Clipboard.GetData("ws.model.storedprocedure"));
					this.PasteTable(customStoredProcedureCollection,
						document.DocumentElement.SelectSingleNode("storedprocedure"),
						document.DocumentElement.SelectSingleNode("columnList"),
						document.DocumentElement.SelectSingleNode("parameterList"));
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

				//Check for duplicate names
				var nameList = new Hashtable();
				foreach(CustomStoredProcedure customStoredProcedure in (CustomStoredProcedureCollection)this.Object)
				{
					if(customStoredProcedure.Generated)
					{
						var name = customStoredProcedure.Name.ToLower();
						if(nameList.ContainsKey(name))
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextDuplicateName, name), customStoredProcedure.Controller);
						else
							nameList.Add(name, string.Empty);
					}
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

		private void AddCustomStoredProcedureMenuClick(object sender, System.EventArgs e)
		{
			var customStoredProcedure = ((CustomStoredProcedureCollection)this.Object).Add();
			this.OnItemChanged(this, new System.EventArgs());
		}

		private void PasteMenuClick(object sender, System.EventArgs e)
		{
			this.PasteClipboard();
			//try
			//{
			//  if (Clipboard.ContainsData("ws.model.storedprocedure"))
			//  {
			//    XmlDocument document = new XmlDocument();
			//    document.LoadXml((string)Clipboard.GetData("ws.model.storedprocedure"));

			//    CustomStoredProcedure newItem = ((CustomStoredProcedureCollection)this.Object).Add();
			//    int id = newItem.Id;
			//    newItem.XmlLoad(document.DocumentElement.SelectSingleNode("storedprocedure"));
			//    newItem.SetId(id);
			//    newItem.SetKey(Guid.NewGuid().ToString());
			//    newItem.Name = "[" + newItem.Name + "]";
			//    newItem.Columns.Clear();
			//    newItem.Parameters.Clear();

			//    //Columns
			//    foreach (XmlNode node in document.DocumentElement.SelectSingleNode("columnList").ChildNodes)
			//    {
			//      CustomStoredProcedureColumn newCustomStoredProcedureColumn = ((ModelRoot)newItem.Root).Database.CustomStoredProcedureColumns.Add();
			//      Reference columnReference = new Reference(newItem.Root);
			//      newItem.Columns.Add(columnReference);
			//      id = newCustomStoredProcedureColumn.Id;
			//      newCustomStoredProcedureColumn.XmlLoad(node);
			//      newCustomStoredProcedureColumn.SetId(id);
			//      newCustomStoredProcedureColumn.SetKey(Guid.NewGuid().ToString());
			//      columnReference.Ref = newCustomStoredProcedureColumn.Id;
			//      columnReference.RefType = ReferenceType.CustomStoredProcedureColumn;
			//    }

			//    //Parameters
			//    foreach (XmlNode node in document.DocumentElement.SelectSingleNode("parameterList").ChildNodes)
			//    {
			//      Parameter parameter = ((ModelRoot)this.Object.Root).Database.CustomRetrieveRuleParameters.Add();
			//      id = parameter.Id;
			//      parameter.XmlLoad(node);
			//      parameter.SetId(id);
			//      parameter.SetKey(Guid.NewGuid().ToString());
			//      ((ReferenceCollection)newItem.Parameters).Add(parameter.CreateRef());
			//      Reference tableRef = newItem.CreateRef();
			//      parameter.ParentTableRef = tableRef;
			//    }

			//    this.OnItemChanged(this, new System.EventArgs());
			//  }
			//}
			//catch (Exception ex)
			//{
			//  throw;
			//}
		}

		#endregion

	}
}