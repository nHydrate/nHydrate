using Microsoft.Win32;

namespace nHydrate.Generator.Common.Util
{
	public class RegistryHelper
	{
		private RegistryHelper()
		{
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
                key?.Close();
            }
			return returnVal;
		}
	}
}

