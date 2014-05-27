using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Widgetsphere.Generator.Common.Util
{
	public class ArchiveReader
	{
		/// <summary>
		/// Extracts a resource and returns the extraction folder
		/// </summary>
		/// <param name="resourceFileName"></param>
		/// <returns></returns>
		public static string ExtractArchive(string resourceFileName)
		{
			try
			{
				if (!File.Exists(resourceFileName))
					return null;

				var extractDir = System.IO.Path.GetTempPath();
				extractDir = Path.Combine(extractDir, Guid.NewGuid().ToString());
				Directory.CreateDirectory(extractDir);

				ZipInputStream MyZipInputStream = null;
				FileStream MyFileStream = null;
				MyZipInputStream = new ZipInputStream(new FileStream(resourceFileName, FileMode.Open, FileAccess.Read));
				var MyZipEntry = MyZipInputStream.GetNextEntry();
				Directory.CreateDirectory(extractDir);
				while (MyZipEntry != null)
				{
					if (MyZipEntry.IsDirectory)
					{
						Directory.CreateDirectory(extractDir + @"\" + MyZipEntry.Name);
					}
					else
					{
						if (!Directory.Exists(extractDir + @"\" + Path.GetDirectoryName(MyZipEntry.Name)))
						{
							Directory.CreateDirectory(extractDir + @"\" + Path.GetDirectoryName(MyZipEntry.Name));
						}
						MyFileStream = new FileStream(extractDir + @"\" + MyZipEntry.Name, FileMode.OpenOrCreate, FileAccess.Write);
						int count;
						var buffer = new byte[4096];
						count = MyZipInputStream.Read(buffer, 0, 4096);
						while (count > 0)
						{
							MyFileStream.Write(buffer, 0, count);
							count = MyZipInputStream.Read(buffer, 0, 4096);
						}
						MyFileStream.Close();
					}
					try
					{
						MyZipEntry = MyZipInputStream.GetNextEntry();
					}
					catch
					{
						MyZipEntry = null;
					}
				}

				if (MyZipInputStream != null)
					MyZipInputStream.Close();

				if (MyFileStream != null)
					MyFileStream.Close();

				return extractDir;
			}
			catch (Exception ex)
			{
				throw;
			}

		}

	}
}
