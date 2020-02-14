#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
	public class ModelRootController : BaseModelObjectController
	{
		public ModelRootController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Model Root";
			this.HeaderDescription = "This object defines all high level settings for the model";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Model);
		}

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

		private void SummaryClick(object sender, System.EventArgs e)
		{
			//var F = new ModelSummaryForm((ModelRoot)this.Object);
			//F.ShowDialog();
		}

	}

}
