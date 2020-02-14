namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface INHydrateModelObject : IXMLable, IModelObject
    {
        INHydrateModelObject Root { get; }
        string Key { get; }
        bool Dirty { get; set; }
        event System.EventHandler DirtyChanged;
        INHydrateModelObjectController Controller { get; set; }
    }
}
