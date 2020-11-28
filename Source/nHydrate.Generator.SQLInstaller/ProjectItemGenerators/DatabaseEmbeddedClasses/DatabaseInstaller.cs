//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using Microsoft.Data.SqlClient;
using Serilog;

namespace PROJECTNAMESPACE
{
    /// <summary>
    /// The database installer class
    /// </summary>
    [RunInstaller(true)]
    public partial class DatabaseInstaller : Installer
    {
        #region Members
        private string PARAMKEYS_APPDB = "connectionstring";
        private string PARAMKEYS_HELP = "showhelp";
        private string PARAMKEYS_SCRIPT = "script";
        private string PARAMKEYS_SCRIPTFILE = "scriptfile";
        private string PARAMKEYS_SCRIPTFILEACTION = "scriptfileaction";
        private string PARAMKEYS_DBVERSION = "dbversion";
        private string PARAMKEYS_VERSIONWARN = "acceptwarnings";
        private string PARAMKEYS_TRAN = "transaction";
        private string PARAMKEYS_SKIPNORMALIZE = "skipnormalize";
        private string PARAMKEYS_HASH = "usehash";
        private string PARAMKEYS_CHECKONLY = "checkonly";
        #endregion

        #region Constructor
        /// <summary>
        /// The default constructor
        /// </summary>
        public DatabaseInstaller()
        {
            InitializeComponent();
        }
        #endregion

        private List<System.Type> GetDatabaseActions()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                  .Where(x => typeof(IDatabaseAction).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                  .ToList();
        }

        #region Install

        /// <summary>
        /// Performs an install of a database
        /// </summary>
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            //base.Install(stateSaver);
            var commandParams = stateSaver as Dictionary<string, string>;

            foreach (var tt in GetDatabaseActions())
            {
                var action = Activator.CreateInstance(tt) as IDatabaseAction;
                action.Execute(commandParams);
            }

            var paramUICount = 0;
            var setup = new InstallSetup();
            if (commandParams.Count > 0)
            {
                if (commandParams.Any(x => PARAMKEYS_TRAN.Contains(x.Key)))
                {
                    setup.UseTransaction = GetSetting(commandParams, PARAMKEYS_TRAN, true);
                    paramUICount++;
                }

                if (commandParams.ContainsKey(PARAMKEYS_SKIPNORMALIZE))
                {
                    setup.SkipNormalize = true;
                    paramUICount++;
                }

                if (commandParams.ContainsKey(PARAMKEYS_HASH))
                {
                    if (commandParams[PARAMKEYS_HASH].ToLower() == "true" || commandParams[PARAMKEYS_HASH].ToLower() == "1" || commandParams[PARAMKEYS_HASH].ToLower() == string.Empty)
                        setup.UseHash = true;
                    else if (commandParams[PARAMKEYS_HASH].ToLower() == "false" || commandParams[PARAMKEYS_HASH].ToLower() == "0")
                        setup.UseHash = false;
                    else
                        throw new Exception("The /" + PARAMKEYS_HASH + " parameter must be set to 'true or false'.");
                    paramUICount++;
                }

                if (commandParams.ContainsKey(PARAMKEYS_CHECKONLY))
                {
                    setup.CheckOnly = true;
                    paramUICount++;
                }

                if (commandParams.ContainsKey(PARAMKEYS_VERSIONWARN))
                {
                    if (commandParams[PARAMKEYS_VERSIONWARN].ToLower() == "all")
                    {
                        setup.AcceptVersionWarningsChangedScripts = true;
                        setup.AcceptVersionWarningsNewScripts = true;
                    }
                    else if (commandParams[PARAMKEYS_VERSIONWARN].ToLower() == "none")
                    {
                        setup.AcceptVersionWarningsChangedScripts = false;
                        setup.AcceptVersionWarningsNewScripts = false;
                    }
                    else if (commandParams[PARAMKEYS_VERSIONWARN].ToLower() == "new")
                    {
                        setup.AcceptVersionWarningsNewScripts = true;
                    }
                    else if (commandParams[PARAMKEYS_VERSIONWARN].ToLower() == "changed")
                    {
                        setup.AcceptVersionWarningsChangedScripts = true;
                    }
                    else
                    {
                        throw new Exception("The /" + PARAMKEYS_VERSIONWARN + " parameter must be set to 'all, none, new, or changed'.");
                    }
                    paramUICount++;
                }

                if (GetSetting(commandParams, PARAMKEYS_HELP, false))
                {
                    ShowHelp();
                    return;
                }

                setup.ConnectionString = GetSetting(commandParams, PARAMKEYS_APPDB, string.Empty);
                if (commandParams.Count(x => PARAMKEYS_APPDB.Contains(x.Key)) > 1)
                    throw new Exception("The connection string was specified more than once.");

                //Determine if calling as a script generator
                if (commandParams.ContainsKey(PARAMKEYS_SCRIPT))
                {
                    var scriptAction = commandParams[PARAMKEYS_SCRIPT].ToLower();
                    switch (scriptAction)
                    {
                        case "versioned":
                        case "unversioned":
                        default:
                            throw new Exception("The script action must be 'versioned' or 'unversioned'.");
                    }

                    if (!commandParams.ContainsKey(PARAMKEYS_SCRIPTFILE))
                        throw new Exception("The '" + PARAMKEYS_SCRIPTFILE + "' parameter must be set for script generation.");

                    var dumpFile = commandParams[PARAMKEYS_SCRIPTFILE];
                    if (!IsValidFileName(dumpFile))
                        throw new Exception("The '" + PARAMKEYS_SCRIPTFILE + "' parameter is not valid.");

                    var fileCreate = true;
                    if (commandParams.ContainsKey(PARAMKEYS_SCRIPTFILEACTION) && (commandParams[PARAMKEYS_SCRIPTFILEACTION] + string.Empty) == "append")
                        fileCreate = false;

                    if (File.Exists(dumpFile) && fileCreate)
                    {
                        File.Delete(dumpFile);
                        System.Threading.Thread.Sleep(500);
                    }

                    switch (scriptAction)
                    {
                        case "versioned":
                            if (commandParams.ContainsKey(PARAMKEYS_DBVERSION))
                            {
                                if (!GeneratedVersion.IsValid(commandParams[PARAMKEYS_DBVERSION]))
                                    throw new Exception("The '" + PARAMKEYS_DBVERSION + "' parameter is not valid.");

                                setup.Version = new GeneratedVersion(commandParams[PARAMKEYS_DBVERSION]);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(setup.ConnectionString))
                                    throw new Exception("Generation of versioned scripts requires a '" + PARAMKEYS_DBVERSION + "' parameter or valid connection string.");
                                else
                                {
                                    var s = new nHydrateSetting();
                                    s.Load(setup.ConnectionString);
                                    setup.Version = new GeneratedVersion(s.dbVersion);
                                }
                            }

                            File.AppendAllText(dumpFile, UpgradeInstaller.GetScript(setup));
                            break;
                        case "unversioned":
                            setup.Version = UpgradeInstaller._def_Version;
                            File.AppendAllText(dumpFile, UpgradeInstaller.GetScript(setup));
                            break;
                    }

                    return;
                }

                if (paramUICount < commandParams.Count)
                {
                    Install(setup);
                    return;
                }
            }

            Log.Information("Invalid configuration");

        }

        private bool IsValidFileName(string fileName)
        {
            try
            {
                new System.IO.FileInfo(fileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Performs an install of a database
        /// </summary>
        public void Install(InstallSetup setup)
        {

            //The connection string must reference an existing database
            if (!SqlServers.TestConnectionString(setup.ConnectionString))
                throw new Exception("The connection string does not reference a valid database.");

            try
            {
                UpgradeInstaller.UpgradeDatabase(setup);
            }
            catch (InvalidSQLException ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine("BEGIN ERROR SQL");
                sb.AppendLine(ex.SQL);
                sb.AppendLine("END ERROR SQL");
                sb.AppendLine();
                Log.Verbose(sb.ToString());
                UpgradeInstaller.LogError(ex, sb.ToString());
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the upgrade script for the specified database
        /// </summary>
        public string GetScript(InstallSetup setup)
        {
            if (string.IsNullOrEmpty(setup.ConnectionString) && setup.Version == null)
                throw new Exception("The connection string must be set.");
            if (setup.SkipSections == null)
                setup.SkipSections = new List<string>();
            return UpgradeInstaller.GetScript(setup);
        }

        #endregion

        #region Uninstall

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedState"></param>
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

        #endregion

        #region NeedsUpdate

        /// <summary>
        /// Determines if the specified database needs to be upgraded
        /// </summary>
        public virtual bool NeedsUpdate(string connectionString)
        {
            return UpgradeInstaller.NeedsUpdate(connectionString);
        }

        /// <summary>
        /// Determines the current version of the specified database
        /// </summary>
        public virtual string VersionInstalled(string connectionString)
        {
            return UpgradeInstaller.VersionInstalled(connectionString);
        }

        /// <summary>
        /// The database version to which this installer will upgrade a database
        /// </summary>
        public virtual string VersionLatest()
        {
            return UpgradeInstaller.VersionLatest();
        }

        /// <summary>
        /// Determines if the specified database has ever been versioned by the framework
        /// </summary>
        /// <param name="connectionString"></param>
        public virtual bool IsVersioned(string connectionString)
        {
            return UpgradeInstaller.IsVersioned(connectionString);
        }

        #endregion

        #region Helpers
        internal static bool GetSetting(Dictionary<string, string> commandParams, string key, bool defaultValue)
        {
            if (commandParams.ContainsKey(key))
            {
                if (commandParams[key] == "true" || commandParams[key] == "1")
                    return true;
                else if (commandParams[key] == "false" || commandParams[key] == "0")
                    return false;
                bool v;
                if (bool.TryParse(commandParams[key], out v))
                    return v;
                return defaultValue;
            }
            return defaultValue;
        }

        internal static string GetSetting(Dictionary<string, string> commandParams, string key, string defaultValue)
        {
            if (commandParams.ContainsKey(key))
                return commandParams[key];
            return defaultValue;
        }
        #endregion

        #region ShowHelp

        /// <summary />
        public static void ShowHelp()
        {
            //Create Help dialog
            var sb = new StringBuilder();
            sb.AppendLine("Updates a Sql Server database");
            sb.AppendLine();
            sb.AppendLine("InstallUtil.exe PROJECTNAMESPACE.dll [/connectionstring:connectionstring] [/transaction:true|false] [/skipnormalize] [/scriptfile:filename] [/scriptfileaction:append] [/checkonly] [/usehash] [/acceptwarnings:all|none|new|changed]");
            sb.AppendLine();
            sb.AppendLine("Providing no parameters will display the default UI.");
            sb.AppendLine();
            sb.AppendLine("/connectionstring:\"connectionstring\"");
            sb.AppendLine("/Specifies the connection string to the upgrade database");
            sb.AppendLine();
            sb.AppendLine("/transaction:[true|false]");
            sb.AppendLine("Specifies whether to use a database transaction. Outside of a transaction there is no rollback functionality if an error occurs! Default is true.");
            sb.AppendLine();
            sb.AppendLine("/skipnormalize");
            sb.AppendLine("Specifies whether to skip the normalization script. The normalization script is used to ensure that the database has the correct schema.");
            sb.AppendLine();
            sb.AppendLine("/scriptfile:filename");
            sb.AppendLine("Specifies that a script be created and written to the specified file.");
            sb.AppendLine();
            sb.AppendLine("/scriptfileaction:append");
            sb.AppendLine("Optionally you can specify to append the script to an existing file. If this parameter is omitted, the file will first be deleted if it exists.");
            sb.AppendLine();
            sb.AppendLine("/usehash:[true|false]");
            sb.AppendLine("Specifies that only scripts that have changed will be applied to the database . Default is true.");
            sb.AppendLine();
            sb.AppendLine("/checkonly");
            sb.AppendLine("Specifies check mode and that no scripts will be run against the database. If any changes have occurred, an exception is thrown with the change list.");
            sb.AppendLine();

            Log.Information(sb.ToString());
        }

        #endregion

    }

    #region InstallSetup

    /// <summary />
    public class InstallSetup
    {
        /// <summary />
        public InstallSetup()
        {
            this.SkipSections = new List<string>();
            this.UseTransaction = true;
            this.UseHash = true;
            this.SkipNormalize = false;
        }

        /// <summary>
        /// The connection string to the newly created database
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary />
        public GeneratedVersion Version { get; set; }

        /// <summary />
        public bool UseHash { get; set; }

        /// <summary />
        public bool UseTransaction { get; set; }

        /// <summary />
        public bool SkipNormalize { get; set; }

        /// <summary>
        /// The transaction to use for this action. If null, one will be created.
        /// </summary>
        public SqlTransaction Transaction { get; set; }

        /// <summary />
        public List<string> SkipSections { get; set; }

        /// <summary />
        public bool CheckOnly { get; set; }

        /// <summary />
        /// <summary />
        public bool AcceptVersionWarningsChangedScripts { get; set; }

        /// <summary />
        public bool AcceptVersionWarningsNewScripts { get; set; }

        /// <summary />
        public string DiskPath { get; set; }

        internal string DebugScriptName { get; set; }

    }

    #endregion

    public interface IDatabaseAction
    {
        void Execute(Dictionary<string, string> input);
    }

}
#pragma warning restore 0168
