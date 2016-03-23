#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using System.IO;

namespace nHydrate.DslPackage.Objects
{
    internal static class VersionHelper
    {
        public const string SERVICE_URL = "http://www.nhydrate.org/Webservice/MainService.asmx";

        public static bool CanConnect()
        {
            nHydrate.Generator.Common.nhydrateservice.MainService service = null;
            try
            {
                service = new nHydrate.Generator.Common.nhydrateservice.MainService();
                service.Url = SERVICE_URL;
                return service.IsLive();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string GetLatestVersion()
        {
            nHydrate.Generator.Common.nhydrateservice.MainService service = null;
            try
            {
                service = new nHydrate.Generator.Common.nhydrateservice.MainService();
                service.Url = SERVICE_URL;
                var version = service.GetLatestVersion3(AddinAppData.Instance.Key, GetCurrentVersion());
                //var version = service.GetLatestVersion();
                return version.Version;
            }
            catch (Exception ex)
            {
                return "(Unknown)";
            }
        }

        public static string GetCurrentVersion()
        {
            var a = System.Reflection.Assembly.GetExecutingAssembly();
            var version = a.GetName().Version;
            return version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
        }

        public static bool ShouldVersionCheck()
        {
            try
            {
                return (DateTime.Now.Subtract(AddinAppData.Instance.LastUpdateCheck).TotalDays >= 7);
            }
            catch (Exception ex)
            {
                //Something bad happened, probably permission based
                //Do Nothing
                return false;
            }
        }

        public static void DidVersionCheck()
        {
            try
            {
                AddinAppData.Instance.LastUpdateCheck = DateTime.Now;
                AddinAppData.Instance.Save();
            }
            catch (Exception ex)
            {
                //Something bad happened, probably permission based
                //Do Nothing
            }
        }

        public static bool ShouldNag()
        {
            try
            {
                return (string.IsNullOrEmpty(nHydrate.Generator.Common.GeneratorFramework.AddinAppData.Instance.Key) &&
                    (DateTime.Now.Subtract(AddinAppData.Instance.LastNag).TotalDays >= 7));
            }
            catch (Exception ex)
            {
                //Something bad happened, probably permission based
                //Do Nothing
                return false;
            }
        }

        public static void DidNag()
        {
            try
            {
                AddinAppData.Instance.LastNag = DateTime.Now;
                AddinAppData.Instance.Save();
            }
            catch (Exception ex)
            {
                //Something bad happened, probably permission based
                //Do Nothing
            }
        }

        public static bool NeedUpdate(string newVersion)
        {
            var currentVersion = GetCurrentVersion();
            try
            {
                var versionNew = new Version(newVersion);
                var versionNow = new Version(currentVersion);
                return (versionNow < versionNew);
            }
            catch (Exception ex)
            {
                return (newVersion != currentVersion);
            }

        }

    }
}