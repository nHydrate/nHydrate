using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public delegate void StandardEventHandler(object sender, System.EventArgs e);
    public delegate void ItemChanagedEventHandler(object sender, System.EventArgs e);
    public delegate void BooleanDelegate(object sender, BooleanEventArgs e);
}
