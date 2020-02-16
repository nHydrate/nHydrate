namespace nHydrate.Generator.Models
{
    public class ParameterCollection : BaseModelCollection<Parameter>
    {
        public ParameterCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "parameter";
    }
}