using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.Util;
using System.Xml;

namespace nHydrate.Generator.Common
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

        public XmlNode XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;
            var ModelRootNode = oDoc.CreateElement("ModelRoot");
            this.Model.XmlAppend(ModelRootNode);
            node.AppendChild(ModelRootNode);
            return node;
        }

        public XmlNode XmlLoad(XmlNode node)
        {
            var ModelRootNode = node.SelectSingleNode("ModelRoot");
            this.Model.XmlLoad(ModelRootNode);
            ((ModelRoot)this.Model).CleanUp();
            return node;
        }

        public static string DomainProjectName(ModelRoot model)
        {
            var retval = $"{model.CompanyName}.{model.ProjectName}";
            return model.DefaultNamespace.IsEmpty() ? retval : model.DefaultNamespace;
        }

        public IModelObject Model { get; set; }

        public string FileName { get; set; } = string.Empty;
    }
}
