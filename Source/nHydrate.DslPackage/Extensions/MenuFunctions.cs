using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DslPackage
{
	internal static class MenuFunctions
	{
		public static bool IsCurrentlyRunningAsAdmin()
		{
			var isAdmin = false;
			var currentIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
			if (currentIdentity != null)
			{
				var pricipal = new System.Security.Principal.WindowsPrincipal(currentIdentity);
				isAdmin = pricipal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
				pricipal = null;
			}
			return isAdmin;
		}

	}

}

