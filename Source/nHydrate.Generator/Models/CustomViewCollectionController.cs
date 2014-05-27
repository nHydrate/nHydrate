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
	public class CustomViewCollectionController : BaseModelObjectController
	{
		#region Member Variables

		private ContextMenu PopupMenu = null;
		private ListView ControllerListView = null;

		#endregion

		#region Constructor

		protected internal CustomViewCollectionController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Views";
			this.HeaderDescription = "This is a list of views defined for this model";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Views);
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if(_node == null)
				{
					_node = new CustomViewCollectionNode(this);
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
			foreach (CustomView customView in (CustomViewCollection)this.Object)
			{
				var newItem = new ListViewItem(customView.Name);
				newItem.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.CustomView);
				newItem.Tag = customView;
				newItem.Name = customView.Key;
				newItem.SubItems.Add(customView.CodeFacade);
				newItem.SubItems.Add(customView.Columns.Count.ToString());
				ControllerListView.Items.Add(newItem);
			}
		}

		private void listView_DoubleClick(object sender, System.EventArgs e)
		{
			if(this.ControllerListView.SelectedItems.Count == 1)
			{
				var item = this.ControllerListView.SelectedItems[0];
				var nodeList = this.Node.Nodes.Find(((CustomView)item.Tag).Key, false);
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

			foreach(ListViewItem item in ControllerListView.SelectedItems)
				((CustomViewCollection)this.Object).Remove(((CustomView)item.Tag).Id);

			//Remove the item from the listview
			for(var ii = ControllerListView.SelectedItems.Count - 1 ; ii >= 0 ; ii--)
			{
				//Remove the item from the tree
				this.Node.Nodes.Remove(this.Node.Nodes[ControllerListView.SelectedItems[ii].Name]);
				//Remove Listitem
				ControllerListView.Items.Remove(ControllerListView.SelectedItems[ii]);
			}

			this.Object.Root.Dirty = true;

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
			var menuList = new List<MenuCommand>();
			menuList.AddMenuItem("New View", new EventHandler(AddCustomViewMenuClick));
			menuList.AddMenuItem("-");
			menuList.AddMenuItem("Paste", new EventHandler(menuPaste_Click));
			return menuList.ToArray();
		}

		private void PasteTable(CustomViewCollection customViewCollection, XmlNode tableNode, XmlNode columnListNode)
		{
			try
			{
				var customView = customViewCollection.Add();
				var id = customView.Id;
				customView.XmlLoad(tableNode);
				customView.SetId(id);
				customView.SetKey(Guid.NewGuid().ToString());
				customView.Name = "[" + customView.Name + "]";
				customView.Columns.Clear();

				foreach (XmlNode child in columnListNode)
				{
					var column = ((ModelRoot)this.Object.Root).Database.CustomViewColumns.Add();
					id = column.Id;
					column.XmlLoad(child);
					column.SetId(id);
					column.SetKey(Guid.NewGuid().ToString());
					customView.Columns.Add(column.CreateRef());
					column.ParentViewRef = customView.CreateRef();
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
				var customView = (CustomView)item.Tag;

				var containerNode = document.CreateElement("z");
				document.DocumentElement.AppendChild(containerNode);

				//Add a CustomView node
				var viewNode = document.CreateElement("view");
				customView.XmlAppend(viewNode);
				containerNode.AppendChild(viewNode);

				//Add the columns
				var columnListNode = document.CreateElement("columnList");
				containerNode.AppendChild(columnListNode);
				foreach (Reference reference in customView.Columns)
				{
					var column = (CustomViewColumn)reference.Object;
					var columnNode = document.CreateElement("column");
					column.XmlAppend(columnNode);
					columnListNode.AppendChild(columnNode);
				}
			}

			Clipboard.SetData("ws.model.viewcollection", document.OuterXml);
		}

		private void PasteClipboard()
		{
			try
			{
				var customViewCollection = (CustomViewCollection)this.Object;
				if (Clipboard.ContainsData("ws.model.viewcollection"))
				{
					var document = new XmlDocument();
					document.LoadXml((string)Clipboard.GetData("ws.model.viewcollection"));
					foreach (XmlNode node in document.DocumentElement.SelectNodes("z"))
					{
						var viewNode = node.SelectSingleNode("view");
						var columnListNode = node.SelectSingleNode("columnList");
						this.PasteTable(customViewCollection, viewNode, columnListNode);
					}
					this.OnItemChanged(this, new System.EventArgs());
				}
				else if (Clipboard.ContainsData("ws.model.view"))
				{
					var document = new XmlDocument();
					document.LoadXml((string)Clipboard.GetData("ws.model.view"));
					this.PasteTable(customViewCollection,
						document.DocumentElement.SelectSingleNode("view"),
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

				//Check for duplicate names
				var nameList = new Hashtable();
				foreach(CustomView customView in (CustomViewCollection)this.Object)
				{
					if(customView.Generated)
					{
						var name = customView.Name.ToLower();
						if(nameList.ContainsKey(name))
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextDuplicateName, name), this);
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

		private void AddCustomViewMenuClick(object sender, System.EventArgs e)
		{
			var customView = ((CustomViewCollection)this.Object).Add();
			this.OnItemChanged(this, new System.EventArgs());
		}

		#endregion

	}
}