using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class CustomViewCollection : BaseModelCollection<CustomView>
    {

        public CustomViewCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeOldName => "";
        protected override string NodeName => "customview";

        public CustomView this[string name]
        {
            get
            {
                foreach (CustomView element in this)
                {
                    if (string.Compare(name, element.Name, true) == 0)
                        return element;
                }
                return null;
            }
        }

        private string GetUniqueName()
        {
            return "[NEW VIEW]";
        }

    }
}