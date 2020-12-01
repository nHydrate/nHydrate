#pragma warning disable 0168


using System;

namespace nHydrate.Generator.Common.Util
{
    public static class SecurityHelper
    {
        /// <summary>
        /// Gets a unique key from the processor
        /// </summary>
        /// <returns>The unique machine key</returns>
        public static string GetMachineID()
        {
#if NET5_0
            return "";
#else
            try
            {
                var cpuInfo = String.Empty;
                var mc = new System.Management.ManagementClass("Win32_Processor");
                var moc = mc.GetInstances();
                foreach (System.Management.ManagementObject mo in moc)
                {
                    if (cpuInfo == String.Empty)
                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                return cpuInfo;
            }
            catch (Exception ex)
            {
                return "";
            }
#endif
        }
    }
}
