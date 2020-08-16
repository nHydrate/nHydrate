using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using System.Xml;

namespace nHydrate.Generator
{
    [Generator("{4B5CFCAF-C668-4d40-947C-83B2AAEBB2B5}", "nHydrate Model")]
    public class nHydrateGeneratorProject : IGenerator
    {
        public nHydrateGeneratorProject()
        {
            var root = new ModelRoot(null);
            root.GeneratorProject = this;
            this.Model = root;
        }

        public void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;
            var ModelRootNode = oDoc.CreateElement("ModelRoot");
            this.Model.XmlAppend(ModelRootNode);
            node.AppendChild(ModelRootNode);
        }

        public void XmlLoad(XmlNode node)
        {
            var ModelRootNode = node.SelectSingleNode("ModelRoot");
            this.Model.XmlLoad(ModelRootNode);
            ((ModelRoot)this.Model).CleanUp();
        }

        public static string DomainProjectName(ModelRoot model)
        {
            var retval = model.CompanyName + "." + model.ProjectName;

            if (string.IsNullOrEmpty(model.DefaultNamespace))
                return retval;
            else
                return model.DefaultNamespace;
        }

        public IModelObject Model { get; set; }

        public string FileName { get; set; } = string.Empty;

    }

}
