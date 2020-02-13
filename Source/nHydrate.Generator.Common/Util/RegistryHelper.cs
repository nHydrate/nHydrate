using Microsoft.Win32;

namespace nHydrate.Generator.Common.Util
{
	/// <summary>
	/// Summary description for RegistryHelper.
	/// </summary>
	public class RegistryHelper
	{
		private RegistryHelper()
		{
		}
		
		public static string SetLocalMachineRegistryValue(string path, string item, string newValue)
		{
            var returnVal = string.Empty;
			try
            {
                var key = Registry.LocalMachine.OpenSubKey(path);
                if(key != null)
				{
					returnVal = (string)key.GetValue(item);
				}
            }
			catch
			{
			}
			return returnVal;
		}

		public static string GetLocalMachineRegistryValue(string path, string item)
		{
			RegistryKey key = null;
			var returnVal = string.Empty;
			try
			{
				key = Registry.LocalMachine.OpenSubKey(path);
				if(key != null)
				{
					returnVal = (string)key.GetValue(item);
				}
			}
			catch
			{
			}
			finally
			{
				if (key != null) key.Close();
			}
			return returnVal;
		}
	}
}

