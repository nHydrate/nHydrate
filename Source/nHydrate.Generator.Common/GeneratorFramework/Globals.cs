using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public enum SQLServerTypeConstants
    {
        SQL2005,
        SQL2008,
        SQLAzure,
    }

    public delegate void StandardEventHandler(object sender, System.EventArgs e);
    public delegate void ItemChanagedEventHandler(object sender, System.EventArgs e);
    public delegate void BooleanDelegate(object sender, BooleanEventArgs e);
}
