#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using nHydrate.ServerObjects;

namespace nHydrate.DslPackage.Objects
{
    internal static class VersionHelper
    {
#if DEBUG
        public const string SERVICE_URL = "http://localhost:58323/";
#else
        public const string SERVICE_URL = "http://api.nhydrate.com/";
#endif

        public static bool CanConnect()
        {
            try
            {
                return Post<object, bool>("", null);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string GetLatestVersion()
        {
            try
            {
                return Post<object, ResultModel>("version", null)?.Text;
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
                    (DateTime.Now.Subtract(AddinAppData.Instance.LastNag).TotalDays >= 3));
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

        public static ResultModel AuthenticateUser(LoginModel model)
        {
            var result = Post<LoginModel, ResultModel>("login", model);
            if (result == null) result = new ResultModel { Success = false, Text = "An error occurred." };
            return result;
        }

        public static void ResetStatistics(string key, bool allow)
        {
            //TODO
        }

        public static ResultModel RegisterUser(UserAccount model)
        {
            return Post<UserAccount, ResultModel>("register", model);
        }

        public static List<IdTextModel> GetCountries()
        {
            return Get<List<IdTextModel>>("countries");
        }

        public static void LogStats(GenStatModel model)
        {
            try
            {
                var result = Post<GenStatModel, ResultModel>("log-stats", model);
            }
            catch (Exception ex)
            {
                //Do nothing
            }
        }

        private static R Post<T, R>(string path, T model)
            where T : new()
            where R : new()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(SERVICE_URL);

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // List data response.
                    var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(path, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response body.
                        var result = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                        return JsonConvert.DeserializeObject<R>(result);
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    }
                    return default(R);
                }
            }
            catch (Exception ex)
            {
                //throw;
                return default(R);
            }
        }

        private static R Get<R>(string path)
            where R : new()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(SERVICE_URL);

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // List data response.
                    var response = client.GetAsync(path).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response body.
                        var result = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                        return JsonConvert.DeserializeObject<R>(result);
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    }
                    return default(R);
                }
            }
            catch (Exception ex)
            {
                //throw;
                return default(R);
            }
        }
    }
}