#pragma warning disable 0168
using System;
using System.Management;

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
			try
			{
				var cpuInfo = String.Empty;
				var mc = new ManagementClass("Win32_Processor");
				var moc = mc.GetInstances();
				foreach (ManagementObject mo in moc)
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
		}
	}
}
