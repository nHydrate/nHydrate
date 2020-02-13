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
