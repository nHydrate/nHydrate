using System;
using System.Windows.Forms;
using nHydrate.Generator.Common;

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

        public override ModelObjectTreeNode Node { get; } = null;

        public override MessageCollection Verify()
        {
            var retval = new MessageCollection();
            retval.AddRange(base.Verify());

            var root = (ModelRoot) this.Object;

            //Check valid name
            if (!ValidationHelper.ValidDatabaseIdenitifer(root.CompanyName) || !ValidationHelper.ValidCodeIdentifier(root.CompanyName))
                retval.Add(ValidationHelper.ErrorTextInvalidCompany, this);
            if (!ValidationHelper.ValidDatabaseIdenitifer(root.ProjectName) || !ValidationHelper.ValidCodeIdentifier(root.ProjectName))
                retval.Add(ValidationHelper.ErrorTextInvalidProject, this);

            if (!string.IsNullOrEmpty(root.DefaultNamespace))
            {
                if (!ValidationHelper.IsValidNamespace(root.DefaultNamespace))
                    retval.Add(ValidationHelper.ErrorTextInvalidNamespace, this);
            }

            return retval;
        }

    }

}
