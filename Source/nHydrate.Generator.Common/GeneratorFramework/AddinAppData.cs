#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
using System.IO;
using System.Xml.Serialization;
using nHydrate.Generator.Common.Logging;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public class AddinAppData
    {
        #region Class Members

        private const string DATA_FILE_NAME = "addInStore.xml";
        private const string COMPANY_FOLDER = "nHydrate";
        private const string REG_LOCATION = @"Software\nHydrate.org\nHydrate Modeler";
        private const string REG_INSTALLDIR_PROPERTY = "InstallDir";

        private static readonly AddinAppData _instance = null;
        private readonly FileInfo _addinDataStoreFile = null;
        private readonly AddinProperties _addInProperties = null;

        #endregion

        #region Constructors

        static AddinAppData()
        {
            try
            {
                _instance = new AddinAppData();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private AddinAppData()
        {
            var fullFileName = string.Empty;
            try
            {
                var appDataFolder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), COMPANY_FOLDER);
                appDataFolder = Path.Combine(appDataFolder, DATA_FILE_NAME);
                fullFileName = appDataFolder;

                _addinDataStoreFile = new FileInfo(fullFileName);
                _addInProperties = new AddinProperties();
                if (_addinDataStoreFile.Exists)
                {
                    var serializer = new XmlSerializer(typeof(AddinProperties));
                    var tr = new StreamReader(_addinDataStoreFile.FullName);
                    _addInProperties = (AddinProperties)serializer.Deserialize(tr);
                    tr.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Filename: " + fullFileName, ex);
            }
        }

        #endregion

        #region Properties

        private AddinProperties Properties
        {
            get { return _addInProperties; }
        }

        public static AddinAppData Instance
        {
            get { return _instance; }
        }

        public string ExtensionDirectory
        {
            get
            {
                if (EnvDTEHelper.Instance.Version == "10.0")
                {
                    //VS 2010
                    //Get current path if need be
                    var retval = RegistryHelper.GetLocalMachineRegistryValue(REG_LOCATION, REG_INSTALLDIR_PROPERTY);
                    if (string.IsNullOrEmpty(retval))
                        retval = (new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).DirectoryName;
                    return retval;
                }
                //else if (EnvDTEHelper.Instance.Version == "11.0")
                //{
                //    //VS 2012
                //    //Get current path if need be
                //    var retval = RegistryHelper.GetLocalMachineRegistryValue(REG_LOCATION, REG_INSTALLDIR_PROPERTY);
                //    if (string.IsNullOrEmpty(retval))
                //        retval = (new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).DirectoryName;
                //    return retval;
                //}
                else
                {
                    //VS 2012+
                    return (new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).DirectoryName;
                }
            }
        }

        public string Key
        {
            get { return Properties.Key; }
            set { Properties.Key = value; }
        }

        public string PremiumKey
        {
            get { return Properties.PremiumKey; }
            set { Properties.PremiumKey = value; }
        }

        public bool PremiumValidated
        {
            get { return Properties.PremiumValidated; }
            set { Properties.PremiumValidated = value; }
        }

        public DateTime LastNag
        {
            get { return Properties.LastNag; }
            set { Properties.LastNag = value; }
        }

        public DateTime LastUpdateCheck
        {
            get { return Properties.LastUpdateCheck; }
            set { Properties.LastUpdateCheck = value; }
        }

        public string LastVersionChecked
        {
            get { return Properties.LastVersionChecked; }
            set { Properties.LastVersionChecked = value; }
        }

        public bool AllowStats
        {
            get { return Properties.AllowStats; }
            set { Properties.AllowStats = value; }
        }

        public void Save()
        {
            try
            {
                if (!_addinDataStoreFile.Directory.Exists)
                    _addinDataStoreFile.Directory.Create();

                var serializer = new XmlSerializer(typeof(AddinProperties));
                var tw = new StreamWriter(_addinDataStoreFile.FullName);
                serializer.Serialize(tw, Properties);
                tw.Close();
            }
            catch (Exception ex)
            {
                nHydrateLog.LogError(ex);
            }
        }

        #endregion

    }
}
