using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

namespace nHydrate.Generator.Common.Util
{
	public class ResourceHelper
	{
		#region static hashtable for holding resource libraries
		private static readonly Hashtable mResourceLibraries = new Hashtable();
		private static Hashtable ResourceLibraries
		{
			get
			{
				return mResourceLibraries;
			}
		}
		#endregion

		#region resource Helper creation
		private readonly ResourceManager rm;
		private ResourceHelper(string resourceFullName, Assembly assembly)
		{
			rm = new ResourceManager(resourceFullName, assembly);
		}
		#endregion

		#region public methods used to get information from the resource Helper
		public int GetInt(string key)
		{
            var resourceVal = rm.GetString(key);
			try
            {
                var returnVal = int.Parse(resourceVal, CultureInfo.CurrentCulture.NumberFormat);
                return returnVal;
            }
			catch(Exception ex)
			{
				throw new Exception(String.Format("Resource cannot be cast to the suggested type. type: {0} key: {1}", "int", key), ex);
			}
		}

		public float GetFloat(string key)
		{
            var resourceVal = rm.GetString(key);
			try
            {
                var returnVal = float.Parse(resourceVal, CultureInfo.CurrentCulture.NumberFormat);
                return returnVal;
            }
			catch(Exception ex)
			{
				throw new Exception(String.Format("Resource cannot be cast to the suggested type. type: {0} key: {1}", "float", key), ex);
			}
		}


		public string GetString(string key)
		{
			try
			{
				var resourceVal = rm.GetString(key);
				if(!StringHelper.Match(resourceVal, string.Empty, true))
				{
					return resourceVal;
				}
				else
				{
					throw new Exception(String.Format("Resource cannot be found or is empty. key: {0}", key));
				}
			}
			catch(Exception ex)
			{
				throw new Exception(String.Format("Resource cannot be found or is empty. key: {0}", key), ex);
			}
		}
		#endregion

		#region resource Helper factory
		public static ResourceHelper GetResourceHelper(string resourceFullName, Type callingObjectType)
		{
			var uniqueName = resourceFullName + callingObjectType.Assembly.FullName;
			lock(ResourceLibraries.SyncRoot)
			{
				if(!ResourceLibraries.Contains(uniqueName))
				{
					var rl = new ResourceHelper(resourceFullName, callingObjectType.Assembly);
					ResourceLibraries.Add(uniqueName, rl);
				}
			}
			return (ResourceHelper)ResourceLibraries[uniqueName];
		}
		#endregion

		#region get resources as stream
		public static string GetResourceFileAsString(System.Reflection.Assembly assembly, string resourcePath)
		{
			
			var returnVal = string.Empty;
			using(var resourceStream = GetResourceFileAsStream(assembly, resourcePath))
			{
				var sr = new StreamReader(resourceStream, System.Text.Encoding.UTF8);
				returnVal = sr.ReadToEnd();
			}
			return returnVal;
		}

		public static Stream GetResourceFileAsStream(System.Reflection.Assembly assembly, string resourcePath)
		{
			//Assembly asbly = callingObjectType.Assembly;
			var stream = assembly.GetManifestResourceStream(resourcePath);
			var sr = new StreamReader(stream, System.Text.Encoding.UTF8);
			return sr.BaseStream;
		}

		public static Image GetResourceFileAsImage(System.Reflection.Assembly assembly, string resourcePath)
		{
			Bitmap returnImage = null;
			using(var stream = GetResourceFileAsStream(assembly, resourcePath))
			{
				returnImage = new Bitmap(stream);
			}
			return returnImage;
		}

		public static Icon GetResourceFileAsIcon(System.Reflection.Assembly assembly, string resourcePath)
		{
			Icon returnImage = null;
			using(var stream = GetResourceFileAsStream(assembly, resourcePath))
			{
				returnImage = new Icon(stream);
			}
			return returnImage;
		}
		#endregion
	
	}
}

