#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using System;
using System.IO;
using nHydrate.Generator.Common.Logging;

namespace nHydrate.Generator.Common.Util
{
	public class FileSystemHelper
	{

		public FileSystemHelper()
		{
		}

		public static bool ValidDirectoryString(string directoryPath)
		{
			try
			{
				var currentFolder = new DirectoryInfo(directoryPath);
				return true;
			}
			catch (Exception ex)
			{
				nHydrateLog.LogWarning(ex);
				return false;
			}
		}

		public static string DirectoryFromFileName(string fileName)
		{
			var fi = new FileInfo(fileName);
			return fi.DirectoryName;
		}

		public static void CopyFolder(string sourceFolder, string destFolder)
		{
			CopyFolder(sourceFolder, destFolder, false);
		}

		public static void CopyFolder(string sourceFolder, string destFolder, bool overwrite)
		{
			if (!Directory.Exists(destFolder))
				Directory.CreateDirectory(destFolder);
			var files = Directory.GetFiles(sourceFolder);
			foreach (var file in files)
			{
				var name = Path.GetFileName(file);
				var dest = Path.Combine(destFolder, name);
				File.Copy(file, dest, overwrite);
			}
			var folders = Directory.GetDirectories(sourceFolder);
			foreach (var folder in folders)
			{
				var name = Path.GetFileName(folder);
				var dest = Path.Combine(destFolder, name);
				CopyFolder(folder, dest, overwrite);
			}
		}

	}
}
