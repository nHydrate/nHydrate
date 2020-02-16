using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public class AddinProperties
    {
        internal AddinProperties()
        {
        }

        public string ExtensionDirectory { get; set; }
        public string Key { get; set; }
        public bool AllowStats { get; set; }
        public DateTime LastUpdateCheck { get; set; }
        public DateTime LastNag { get; set; }
        public string PremiumKey { get; set; }
        public bool PremiumValidated { get; set; }
    }
}
