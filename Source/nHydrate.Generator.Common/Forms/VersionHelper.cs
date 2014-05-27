using System;
using System.IO;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Common.Forms
{
	internal static class VersionHelper
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

		public static string GetLatestVersion()
		{
			nhydrateservice.MainService service = null;
			try
			{
				service = new nhydrateservice.MainService();
				var version = service.GetLatestVersion3(AddinAppData.Instance.Key, GetCurrentVersion());
				return version.Version;
			}
			catch (Exception ex)
			{
				return "(Unknown)";
			}
		}

		public static string GetCurrentVersion()
		{
			var fileName = Path.Combine(AddinAppData.Instance.ExtensionDirectory, "VSCodeTools2008.dll");
			if (File.Exists(fileName))
			{
				var a = System.Reflection.Assembly.LoadFrom(fileName);
				var version = a.GetName().Version;
				return version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
			}
			return "(Unknown)";
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

		public static bool NeedUpdate(string newVersion)
		{
			var currentVersion = GetCurrentVersion();
			try
			{
				var versionNew = new Version(newVersion);
				var versionNow = new Version(currentVersion);
				return (versionNow < versionNew);
			}
			catch (Exception ex)
			{
				return (newVersion != currentVersion);
			}

		}

	}
}
