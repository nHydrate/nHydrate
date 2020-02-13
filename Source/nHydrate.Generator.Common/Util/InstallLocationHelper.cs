using System.IO;

namespace nHydrate.Generator.Common.Util
{
	/// <summary>
	/// Summary description for InstallLocationHelper.
	/// </summary>
	public class InstallLocationHelper
	{
		public InstallLocationHelper()
		{
		}

		public static DirectoryInfo VisualStudio2003()
		{
			const string location = @"SOFTWARE\Microsoft\VisualStudio\7.1\Setup\VS";
			const string item = @"ProductDir";
			var installLocation = RegistryHelper.GetLocalMachineRegistryValue(location, item);
			return new DirectoryInfo(StringHelper.EnsureDirectorySeperatorAtEnd(installLocation));
		}

		public static DirectoryInfo VisualStudio2005()
		{
			const string location = @"SOFTWARE\Microsoft\VisualStudio\8.0\Setup\VS";
			const string item = @"ProductDir";
			var installLocation = RegistryHelper.GetLocalMachineRegistryValue(location, item);
			return new DirectoryInfo(StringHelper.EnsureDirectorySeperatorAtEnd(installLocation));
		}
		
		public static DirectoryInfo VisualCSharp()
		{
			const string location = @"SOFTWARE\Microsoft\VisualStudio\8.0\Setup\VC#";
			const string item = @"ProductDir";
			var installLocation = RegistryHelper.GetLocalMachineRegistryValue(location, item);
			return new DirectoryInfo(StringHelper.EnsureDirectorySeperatorAtEnd(installLocation));
		}

		public static DirectoryInfo DotNetFramework()
		{
			const string location = @"SOFTWARE\Microsoft\.NETFramework";
			const string item = @"InstallRoot";
			var installLocation = RegistryHelper.GetLocalMachineRegistryValue(location, item);
			return new DirectoryInfo(StringHelper.EnsureDirectorySeperatorAtEnd(installLocation));
		}

		public static DirectoryInfo DotNetFrameworkV1_1_4322()
		{
			var dotNetFramework = DotNetFramework();
			var retVal = new DirectoryInfo(Path.Combine(dotNetFramework.FullName, "v1.1.4322\\"));
			return retVal;
		}
	}
}

