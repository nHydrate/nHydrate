using System;
using nHydrate.Generator.Common.Logging;

namespace nHydrate.Generator.Common.Util
{
	public static class GlobalHelper
	{
		public static void ShowError(Exception ex)
		{
			ShowError(ex.ToString());
		}

		public static void ShowError(string message)
		{
			nHydrateLog.LogError(new Exception(message));
			var F = new nHydrate.Generator.Common.Forms.ErrorForm("An error has occurred!", message);
			F.ShowDialog();
		}

	}

}

