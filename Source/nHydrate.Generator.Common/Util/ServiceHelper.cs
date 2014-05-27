using System;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Common.Util
{
	public static class ServiceHelper
	{
		public static bool CanConnect()
		{
			nhydrateservice.MainService service = null;
			try
			{
				service = new nhydrateservice.MainService();
				return service.IsLive();
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static bool ShouldCheck()
		{
			try
			{
				return (DateTime.Now.Subtract(AddinAppData.Instance.LastUpdateCheck).TotalDays >= 7);
			}
			catch (Exception ex)
			{
				//Something bad happened, probably permission based
				//Do Nothing
				return false;
			}
		}

	}
}
