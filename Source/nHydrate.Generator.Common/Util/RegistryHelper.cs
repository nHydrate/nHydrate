#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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

