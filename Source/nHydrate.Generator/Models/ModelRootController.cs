#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class ModelRootController : BaseModelObjectController
	{
		#region Member Variables

		//private ModelRootControllerUIControl _uiControl = null;

		#endregion

		#region Constructor

		public ModelRootController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Model Root";
			this.HeaderDescription = "This object defines all high level settings for the model";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Model);
		}

		#endregion

		#region Methods

		public void SelectNode(INHydrateModelObject modelObject)
		{
			if (modelObject == this.Node.Object)
				this.Node.TreeView.SelectedNode = this.Node;
			else
				this.SelectNode(modelObject, this.Node.Nodes);
		}

		private void SelectNode(INHydrateModelObject modelObject, TreeNodeCollection nodeList)
		{
			foreach (ModelObjectTreeNode node in nodeList)
			{
				if (modelObject == node.Object)
					this.Node.TreeView.SelectedNode = node;
				else
					this.SelectNode(modelObject, node.Nodes);
			}
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if (_node == null)
				{
					//_node = new ModelRootNode(this);
					Application.DoEvents();
				}
				return _node;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			var menuList = new List<MenuCommand>();
			menuList.AddMenuItem("Model Summary", new EventHandler(SummaryClick));
			return menuList.ToArray();
		}

		public override MessageCollection Verify()
		{
			try
			{
				var retval = new MessageCollection();
				retval.AddRange(base.Verify());

				var root = (ModelRoot)this.Object;

				//Check valid name
				if (!ValidationHelper.ValidDatabaseIdenitifer(root.CompanyName) || !ValidationHelper.ValidCodeIdentifier(root.CompanyName))
					retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextInvalidCompany, this);
				if (!ValidationHelper.ValidDatabaseIdenitifer(root.ProjectName) || !ValidationHelper.ValidCodeIdentifier(root.ProjectName))
					retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextInvalidProject, this);
				if (!ValidationHelper.ValidDatabaseIdenitifer(root.CompanyAbbreviation) || !ValidationHelper.ValidCodeIdentifier(root.CompanyAbbreviation))
					retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextInvalidCompanyAbbreviation, this);

				if (!string.IsNullOrEmpty(root.DefaultNamespace))
				{
					if (!ValidationHelper.IsValidNamespace(root.DefaultNamespace))
						retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextInvalidNamespace, this);
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

		public virtual void ClearTree()
		{
			if (_node != null)
			{
				var oldNode = _node;
				_node = null;
				var s = this.Node.Name; //Reset
				oldNode.Nodes.Clear();
				oldNode.Nodes.AddRange(this.Node.Nodes.ToArray());
				_node = oldNode;
			}
		}

		//public override ModelObjectUserInterface UIControl
		//{
		//  get
		//  {
		//    if (this._userInterface == null)
		//    {
		//      var ctrl = new PanelUIControl();
		//      _uiControl = new ModelRootControllerUIControl();
		//      _uiControl.Populate(this.Object as ModelRoot);
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

		#region Event Handlers

		private void SummaryClick(object sender, System.EventArgs e)
		{
			//var F = new ModelSummaryForm((ModelRoot)this.Object);
			//F.ShowDialog();
		}

		#endregion

	}

}
