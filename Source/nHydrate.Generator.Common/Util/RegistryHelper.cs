namespace nHydrate.Generator.Common.Util
{
    public class RegistryHelper
    {
        private RegistryHelper()
        {
        }

        public static string GetLocalMachineRegistryValue(string path, string item)
        {
#if NETSTANDARD
            return string.Empty;
#else
            Microsoft.Win32.RegistryKey key = null;
            var returnVal = string.Empty;
            try
            {
                key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);
                if (key != null)
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
#endif
        }
    }
}

