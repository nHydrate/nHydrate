using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Reflection;
using System.IO;

namespace nHydrate.Generator.Datasite
{
	internal static class Helpers
	{
		public static string GetFileContent(EmbeddedResourceName ern)
		{
			var retVal = string.Empty;
			var asm = Assembly.GetExecutingAssembly();
			var manifestStream = asm.GetManifestResourceStream(ern.FullName);
			try
			{
				using (var sr = new System.IO.StreamReader(manifestStream))
				{
					retVal = sr.ReadToEnd();
				}
			}
			catch { }
			finally
			{
				manifestStream.Close();
			}
			return retVal;
		}

		public static byte[] GetFileBinContent(EmbeddedResourceName ern)
		{
			var retVal = string.Empty;
			var asm = Assembly.GetExecutingAssembly();
			var manifestStream = asm.GetManifestResourceStream(ern.FullName);
			try
			{
				var memoryStream = new MemoryStream();
				manifestStream.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
			catch { }
			finally
			{
				manifestStream.Close();
			}
			return null;
		}

	}
}
