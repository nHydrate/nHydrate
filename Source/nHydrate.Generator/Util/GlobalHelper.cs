using nHydrate.Generator.Common.Logging;
using System;

namespace nHydrate.Generator.Util
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
            var F = new nHydrate.Generator.Forms.ErrorForm("An error has occurred!", message);
            F.ShowDialog();
        }

    }

}

