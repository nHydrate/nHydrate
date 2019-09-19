#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
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

