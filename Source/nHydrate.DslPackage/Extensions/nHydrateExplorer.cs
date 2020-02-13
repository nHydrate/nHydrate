#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DslPackage
{
	partial class nHydrateExplorer
	{
		public override Microsoft.VisualStudio.Modeling.Shell.ModelElementTreeNode CreateModelElementTreeNode(Microsoft.VisualStudio.Modeling.ModelElement modelElement)
		{
			var n = base.CreateModelElementTreeNode(modelElement);
			//n.ContextMenu = new System.Windows.Forms.ContextMenu();
			//n.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("qqqq"));
			return n;
		}

		public override Microsoft.VisualStudio.Modeling.Shell.ExplorerTreeNode FindNodeForElement(Microsoft.VisualStudio.Modeling.ModelElement element)
		{
			return base.FindNodeForElement(element);
		}

		public override void InsertTreeNode(System.Windows.Forms.TreeNodeCollection siblingNodes, Microsoft.VisualStudio.Modeling.Shell.ExplorerTreeNode node)
		{
			base.InsertTreeNode(siblingNodes, node);
			//ResetIcons(node.TreeView.ImageList);
		}

		private bool _haveResetIcons = false;
		private void ResetIcons(System.Windows.Forms.ImageList listview)
		{
			if (_haveResetIcons) return;
			_haveResetIcons = true;
			try
			{
				var a = System.Reflection.Assembly.GetExecutingAssembly();
				var file = a.GetManifestResourceStream("nHydrate.DslPackage.Resources.folder.png");
				listview.Images[1] = System.Drawing.Image.FromStream(file);
			}
			catch (Exception ex)
			{
				//Do nothing
			}
		}

		//public override System.Windows.Forms.ContextMenu ContextMenu
		//{
		//  get
		//  {
		//    var list = base.ContextMenu;
		//    System.Windows.Forms.MessageBox.Show("Count A:" + list.MenuItems.Count, "Count");
		//    list.MenuItems.Add(new System.Windows.Forms.MenuItem("QQQQ", new EventHandler(HighlightItem)));
		//    System.Windows.Forms.MessageBox.Show("Count B:" + list.MenuItems.Count, "Count");
		//    return list;
		//  }
		//  set { base.ContextMenu = value; }
		//}

		//public override void AddCommandHandlers(System.ComponentModel.Design.IMenuCommandService menuCommandService)
		//{
		//  base.AddCommandHandlers(menuCommandService);

		//  var command = new Microsoft.VisualStudio.Modeling.Shell.DynamicStatusMenuCommand(
		//    new EventHandler(HighlightItemStatus),
		//    new EventHandler(HighlightItemExecute), 
		//    new System.ComponentModel.Design.CommandID(new Guid("ABCDAA34-817C-44FC-8227-EC4C4C56BE3C"), 20));
		//  command.Visible = true;
		//  command.Enabled = true;
		//  menuCommandService.AddCommand(command);
		//  //Microsoft.VisualStudio.Modeling.Shell.CommonModelingCommands.
		//}

		//private void HighlightItemStatus(object sender, EventArgs e)
		//{
		//  //System.Windows.Forms.MessageBox.Show("Hello", "Hello");
		//  System.ComponentModel.Design.MenuCommand cmd = sender as System.ComponentModel.Design.MenuCommand;
		//  cmd.Enabled = cmd.Visible = true;
		//}

		//private void HighlightItemExecute(object sender, EventArgs e)
		//{
		//  System.Windows.Forms.MessageBox.Show("Hello", "Hello");
		//}

	}
}

