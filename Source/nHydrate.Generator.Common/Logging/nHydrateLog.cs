using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.Logging
{
    public class nHydrateLog
    {
        #region Constants
        internal const string LOG_ERROR_SOURCE = "nHydrateLogger";
        #endregion

        #region Member Variables
        private readonly string _exeName;
        private static TraceSwitch _currentSwitch;
        #endregion

        #region Setup and Initilize

        private nHydrateLog(string exeName)
        {
            _exeName = exeName;
            _currentSwitch = new TraceSwitch("nHydrate", "nHydrate trace switch for " + exeName);
            try
            {
                SetDefaults();
                //InitializeConfigFile();
                //mAppFileWatcher_Changed(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, _appConfigFile.DirectoryName, _appConfigFile.Name));
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        private void SetDefaults()
        {
            _currentSwitch.Level = TraceLevel.Error;
            var logFileFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "nHydrate\\" + _exeName + "nHydrate.log");
            var logFile = new FileInfo(logFileFullPath);
            AddListener("ExeDefaultListener", "System.Diagnostics.DefaultTraceListener", logFile.FullName);
        }

        #endregion

        #region TraceStatements
        private static void Log(TraceLevel level, string message)
        {
            string traceLevelString;
            if (level == TraceLevel.Off)
            {
                traceLevelString = "Always";
                traceLevelString = traceLevelString.PadRight(7);
            }
            else
            {
                traceLevelString = level.ToString();
                traceLevelString = traceLevelString.PadRight(7);
            }
            var logString = String.Format("{0}:{1}: {2}", traceLevelString, DateTime.UtcNow.ToString("yyyyMMdd.HHmmss"), message);
            Trace.WriteLine(logString);
        }

        private static void Log(TraceLevel level, string message, object arg1)
        {
            var logString = String.Format(message, arg1);
            Log(level, logString);
        }

        private static void Log(TraceLevel level, string message, object arg1, object arg2)
        {
            var logString = String.Format(message, arg1, arg2);
            Log(level, logString);
        }

        private static void Log(TraceLevel level, string message, object arg1, object arg2, object arg3)
        {
            var logString = String.Format(message, arg1, arg2, arg3);
            Log(level, logString);
        }

        private static void Log(TraceLevel level, string message, object[] args)
        {
            var logString = String.Format(message, args);
            Log(level, logString);
        }
        #endregion

        #region LogAlways
        public static void LogAlways(string message)
        {
            try
            {
                Log(TraceLevel.Off, message);
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        #endregion

        #region LogError
        public static void LogError(string message)
        {
            try
            {
                if (_currentSwitch.TraceError)
                {
                    Log(TraceLevel.Error, message);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogError(string message, object arg1, object arg2)
        {
            try
            {
                if (_currentSwitch.TraceError)
                {
                    Log(TraceLevel.Error, message, arg1, arg2);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogError(Exception ex)
        {
            try
            {
                LogError(ex.ToString());
            }
            catch (Exception ex2)
            {
                LogLogFailure(ex2.ToString());
            }
        }

        public static void LogError(Exception ex, string error)
        {
            try
            {
                LogError("{0}\n{1}", error, ex.ToString());
            }
            catch (Exception ex2)
            {
                LogLogFailure(ex2.ToString());
            }
        }
        #endregion

        #region LogWarning
        public static void LogWarning(string message)
        {
            try
            {
                if (_currentSwitch.TraceWarning)
                {
                    Log(TraceLevel.Warning, message);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogWarning(Exception ex)
        {
            try
            {
                LogError(ex.ToString());
            }
            catch (Exception ex2)
            {
                LogLogFailure(ex2.ToString());
            }
        }

        public static void LogWarning(Exception ex, string error)
        {
            try
            {
                LogError("{0}\n{1}", error, ex.ToString());
            }
            catch (Exception ex2)
            {
                LogLogFailure(ex2.ToString());
            }
        }
        #endregion

        #region LogInfo
        public static void LogInfo(string message)
        {
            try
            {
                if (_currentSwitch.TraceInfo)
                {
                    Log(TraceLevel.Info, message);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogInfo(string message, object arg1)
        {
            try
            {
                if (_currentSwitch.TraceInfo)
                {
                    Log(TraceLevel.Info, message, arg1);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        #endregion

        #region LogVerbose
        public static void LogVerbose(string message)
        {
            try
            {
                if (_currentSwitch.TraceVerbose)
                {
                    Log(TraceLevel.Verbose, message);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogVerbose(string message, object arg1, object arg2, object arg3)
        {
            try
            {
                if (_currentSwitch.TraceVerbose)
                {
                    Log(TraceLevel.Verbose, message, arg1, arg2, arg3);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        #endregion

        #region Log Log Failure
        public static void LogLogFailure(string message)
        {
            try
            {
                var logName = "Application";
                if (!EventLog.SourceExists(LOG_ERROR_SOURCE))
                {
                    EventLog.CreateEventSource(LOG_ERROR_SOURCE, logName);
                }
                else
                {
                    logName = EventLog.LogNameFromSourceName(LOG_ERROR_SOURCE, System.Environment.MachineName);
                }
                var applicationLog = new EventLog(logName);
                applicationLog.Source = LOG_ERROR_SOURCE;
                applicationLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }
        #endregion

        #region File System Watcher

        private void AddListener(string listenerName, string typeString, string initializationData)
        {
            var newListener = CreateListener(listenerName, typeString, initializationData);
            if (newListener != null)
                Trace.Listeners.Add(newListener);
        }

        private TraceListener CreateListener(string listenerName, string typeString, string initializationData)
        {

            TraceListener retVal = null;
            try
            {
                nHydrateLog.LogVerbose("CreateListener(string listenerName:{0}, string typeString:{1}, string initializationData:{2})", listenerName, typeString, initializationData);
                if (typeString == ("System.Diagnostics.TextWriterTraceListener"))
                {
                    retVal = new TextWriterTraceListener(initializationData);
                }
                else if (typeString == ("System.Diagnostics.EventLogTraceListener"))
                {
                    retVal = new EventLogTraceListener(initializationData);
                }
                else if (typeString == "System.Diagnostics.DefaultTraceListener")
                {
                    retVal = new System.Diagnostics.DefaultTraceListener();
                }
                else
                {
                    var obj = Type.GetType(typeString);
                    if (obj != null)
                        retVal = (TraceListener)ReflectionHelper.CreateInstance(obj, new object[] { initializationData });
                }
                if (retVal != null)
                {
                    retVal.Name = listenerName;
                }
                else
                {
                    throw new nHydrate.Generator.Common.Exceptions.nHydrateException(String.Format("Could not create listener - listenerName:{0}- typeString:{1})", listenerName, typeString));
                }
            }
            catch { }
            return retVal;

        }

        #endregion

    }

}
