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

        private readonly FileInfo _addinDataStoreFile = null;

        #endregion

        #region Constructors

        static AddinAppData()
        {
            try
            {
                Instance = new AddinAppData();
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
                Properties = new AddinProperties();
                if (_addinDataStoreFile.Exists)
                {
                    var serializer = new XmlSerializer(typeof(AddinProperties));
                    var tr = new StreamReader(_addinDataStoreFile.FullName);
                    Properties = (AddinProperties)serializer.Deserialize(tr);
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

        private AddinProperties Properties { get; } = null;

        public static AddinAppData Instance { get; } = null;

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

                //VS 2012+
                return (new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).DirectoryName;
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
