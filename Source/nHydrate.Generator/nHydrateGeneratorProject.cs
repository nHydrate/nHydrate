using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;

namespace nHydrate.Generator
{
    [Generator("{4B5CFCAF-C668-4d40-947C-83B2AAEBB2B5}", "nHydrate Model")]
    public class nHydrateGeneratorProject : IGenerator
    {
        public nHydrateGeneratorProject()
        {
            var root = new ModelRoot(null);
            root.GeneratorProject = this;
            this.RootController = new ModelRootController(root);
        }

        public void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;
            var ModelRootNode = oDoc.CreateElement("ModelRoot");
            RootController.Object.XmlAppend(ModelRootNode);
            node.AppendChild(ModelRootNode);
        }

        public void XmlLoad(XmlNode node)
        {
            var ModelRootNode = node.SelectSingleNode("ModelRoot");
            RootController.Object.XmlLoad(ModelRootNode);
            ((ModelRoot)RootController.Object).CleanUp();
        }

        public static string DomainProjectName(ModelRoot model)
        {
            var retval = model.CompanyName + "." + model.ProjectName;

            if (string.IsNullOrEmpty(model.DefaultNamespace))
                return retval;
            else
                return model.DefaultNamespace;
        }

        public IModelObject Model
        {
            get { return RootController.Object; }
        }

        public INHydrateModelObjectController RootController { get; set; }

        public string FileName { get; set; } = string.Empty;

    }

}
