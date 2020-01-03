#region Copyright (c) 2006-2012 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2012 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Reflection;

namespace PROJECTNAMESPACE
{
	internal class ArchiveReader
	{
		public static string ExtractArchive(string resourceFileName)
		{
			try
			{
				string extractDir = System.IO.Path.GetTempPath();
				extractDir = Path.Combine(extractDir, Guid.NewGuid().ToString());
				Directory.CreateDirectory(extractDir);

				string zipFilename = Path.Combine(extractDir, Guid.NewGuid().ToString());
				CreateFileFromResource(resourceFileName, zipFilename);

				ZipInputStream MyZipInputStream = null;
				FileStream MyFileStream = null;
				MyZipInputStream = new ZipInputStream(new FileStream(zipFilename, FileMode.Open, FileAccess.Read));
				ZipEntry MyZipEntry = MyZipInputStream.GetNextEntry();
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
						byte[] buffer = new byte[4096];
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
			catch { throw; }

		}

		private static void CreateFileFromResource(string resourceFileName, string diskFileName)
		{
			try
			{
				var sb = new StringBuilder();
				Assembly asm = Assembly.GetExecutingAssembly();
				System.IO.Stream manifestStream = asm.GetManifestResourceStream(resourceFileName);
				try
				{
					BinaryReader theReader = new BinaryReader(manifestStream);
					FileStream outputStream = new FileStream(diskFileName, FileMode.Create);
					BinaryWriter theWriter = new BinaryWriter(outputStream); byte[] theFileRead = new byte[manifestStream.Length];
					manifestStream.Read(theFileRead, 0, theFileRead.Length);
					theWriter.Write(theFileRead);
					theReader.Close();
					theWriter.Close();
				}
				catch { }
				finally
				{
					manifestStream.Close();
				}
			}
			catch { throw; }
		}

	}

}