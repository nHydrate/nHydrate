#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
        internal const string LOG_SWITCHNAME = "Default";
        private const string _configName = "nHydrate.config.xml";
        #endregion

        #region Member Variables
        private static nHydrateLog _instance;
        //private FileInfo _appConfigFile;
        //private FileSystemWatcher _appFileWatcher;
        private readonly string _exeName;
        private static ArrayList _logClasses = new ArrayList();
        private static TraceSwitch _currentSwitch;
        #endregion

        #region Setup and Initilize
        static nHydrateLog()
        {
            try
            {
                _instance = new nHydrateLog(Process.GetCurrentProcess().ProcessName);
            }
            catch (Exception ex)
            {
                LogLogFailure("public static void InitializeWSLog(string exeName:" + Process.GetCurrentProcess().ProcessName + ")" + Environment.NewLine + ex.ToString());
            }
        }

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

        //private void InitializeConfigFile()
        //{
        //  _appConfigFile = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "nHydrate\\" + _configName));
        //  if (!_appConfigFile.Directory.Exists)
        //    _appConfigFile.Directory.Create();
        //  if (!_appConfigFile.Exists)
        //    _appConfigFile.Create();
        //  _appFileWatcher = new FileSystemWatcher(_appConfigFile.DirectoryName, _appConfigFile.Name);
        //  _appFileWatcher.NotifyFilter = NotifyFilters.LastWrite;
        //  _appFileWatcher.Changed += new FileSystemEventHandler(mAppFileWatcher_Changed);
        //  _appFileWatcher.EnableRaisingEvents = true;
        //}
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

        public static void LogAlways(string message, object arg1)
        {
            try
            {
                Log(TraceLevel.Off, message, arg1);
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogAlways(string message, object arg1, object arg2)
        {
            try
            {
                Log(TraceLevel.Off, message, arg1, arg2);
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogAlways(string message, object arg1, object arg2, object arg3)
        {
            try
            {
                Log(TraceLevel.Off, message, arg1, arg2, arg3);
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogAlways(string message, object[] args)
        {
            try
            {
                Log(TraceLevel.Off, message, args);
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

        public static void LogError(string message, object arg1)
        {
            try
            {
                if (_currentSwitch.TraceError)
                {
                    Log(TraceLevel.Error, message, arg1);
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

        public static void LogError(string message, object arg1, object arg2, object arg3)
        {
            try
            {
                if (_currentSwitch.TraceError)
                {
                    Log(TraceLevel.Error, message, arg1, arg2, arg3);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogError(string message, object[] args)
        {
            try
            {
                if (_currentSwitch.TraceError)
                {
                    Log(TraceLevel.Error, message, args);
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

        public static void LogError(Exception ex, NameValueCollection error)
        {
            try
            {
                var errorMsg = new StringBuilder();
                foreach (string name in error.Keys)
                {
                    errorMsg.AppendFormat("{0} : {1}\n", name, error[name]);
                }
                errorMsg.Append(ex.ToString());
                LogError(errorMsg.ToString());
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

        public static void LogWarning(string message, object arg1)
        {
            try
            {
                if (_currentSwitch.TraceWarning)
                {
                    Log(TraceLevel.Warning, message, arg1);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogWarning(string message, object arg1, object arg2)
        {
            try
            {
                if (_currentSwitch.TraceWarning)
                {
                    Log(TraceLevel.Warning, message, arg1, arg2);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogWarning(string message, object arg1, object arg2, object arg3)
        {
            try
            {
                if (_currentSwitch.TraceWarning)
                {
                    Log(TraceLevel.Warning, message, arg1, arg2, arg3);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogWarning(string message, object[] args)
        {
            try
            {
                if (_currentSwitch.TraceWarning)
                {
                    Log(TraceLevel.Warning, message, args);
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

        public static void LogInfo(string message, object arg1, object arg2)
        {
            try
            {
                if (_currentSwitch.TraceInfo)
                {
                    Log(TraceLevel.Info, message, arg1, arg2);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogInfo(string message, object arg1, object arg2, object arg3)
        {
            try
            {
                if (_currentSwitch.TraceInfo)
                {
                    Log(TraceLevel.Info, message, arg1, arg2, arg3);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogInfo(string message, object[] args)
        {
            try
            {
                if (_currentSwitch.TraceInfo)
                {
                    Log(TraceLevel.Info, message, args);
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

        public static void LogVerbose(string message, object arg1)
        {
            try
            {
                if (_currentSwitch.TraceVerbose)
                {
                    Log(TraceLevel.Verbose, message, arg1);
                }
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public static void LogVerbose(string message, object arg1, object arg2)
        {
            try
            {
                if (_currentSwitch.TraceVerbose)
                {
                    Log(TraceLevel.Verbose, message, arg1, arg2);
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

        public static void LogVerbose(string message, object[] args)
        {
            try
            {
                if (_currentSwitch.TraceVerbose)
                {
                    Log(TraceLevel.Verbose, message, args);
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
        //private void mAppFileWatcher_Changed(object sender, FileSystemEventArgs e)
        //{
        //  try
        //  {
        //    WSLog.LogVerbose("Called: WSLog.mAppFileWatcher_Changed(object sender, FileSystemEventArgs e)");
        //    var oDoc = new XmlDocument();
        //    if (_appConfigFile.Exists)
        //    {
        //      if (File.Exists(_appConfigFile.FullName))
        //      {
        //        try
        //        {
        //          oDoc.Load(_appConfigFile.FullName);
        //          ResetProperties(oDoc);
        //          ResetSwitches(oDoc);
        //          ResetListeners(oDoc);
        //        }
        //        catch (Exception ex)
        //        {
        //          //Do Nothing - The file is invalid
        //        }
        //      }
        //    }
        //  }
        //  catch (Exception ex)
        //  {
        //    WSLog.LogLogFailure("Configuration File Cannot Be Parsed Change Was Invalid\n" + ex.ToString());
        //    WSLog.LogLogFailure(_appConfigFile.FullName);
        //    WSLog.LogLogFailure(File.ReadAllText(_appConfigFile.FullName));
        //  }
        //}

        private void ResetProperties(XmlDocument oDoc)
        {
            nHydrateLog.LogVerbose("Called: private void ResetProperties(XmlDocument oDoc)");

            var traceNode = oDoc.DocumentElement.SelectSingleNode("application[@name = \"" +
                _exeName + "\"]/system.diagnostics/trace");

            if (traceNode != null)
            {
                if (traceNode.Attributes["autoflush"] != null)
                {
                    try { Trace.AutoFlush = bool.Parse(traceNode.Attributes["autoflush"].Value); }
                    catch { Trace.AutoFlush = false; }
                } // end if

                if (traceNode.Attributes["indentsize"] != null)
                {
                    try { Trace.IndentSize = int.Parse(traceNode.Attributes["indentsize"].Value); }
                    catch { Trace.IndentSize = 0; }
                } // end if
            } // end if
        } // end ResetProperties

        private void ResetSwitches(XmlDocument oDoc)
        {
            nHydrateLog.LogVerbose("Called: private void ResetSwitches(XmlDocument oDoc)");
            var switchesNode = oDoc.DocumentElement.SelectSingleNode("application[@name = \"" +
                _exeName + "\"]/system.diagnostics/switches");

            if (switchesNode != null)
            {
                var switchNode = switchesNode.SelectSingleNode("add[@name = \"" + LOG_SWITCHNAME + "\"]");
                if (switchNode == null)
                    _currentSwitch.Level = TraceLevel.Off;
                else
                    _currentSwitch.Level = (TraceLevel)int.Parse(switchNode.Attributes["value"].Value);
            }

        }

        private void ResetListeners(XmlDocument oDoc)
        {
            nHydrateLog.LogVerbose("Called: private void ResetListeners(XmlDocument oDoc)");

            var listenersNode = oDoc.DocumentElement.SelectSingleNode("application[@name = \"" +
                _exeName + "\"]/system.diagnostics/trace/listeners");

            if (listenersNode != null)
            {
                nHydrateLog.LogInfo("Pre Listener Count : {0}", listenersNode.ChildNodes.Count);
                if (listenersNode != null)
                {
                    RemoveListeners(listenersNode);
                    AddListeners(listenersNode);
                }
            }
            else
            {
                RemoveListeners();
            }
            if (listenersNode != null)
            {
                nHydrateLog.LogInfo("Post Listener Count : {0}", listenersNode.ChildNodes.Count);
            }
            else
            {
                nHydrateLog.LogLogFailure("Post Listener Count : 0");
            }
        }

        private void RemoveListeners()
        {
            for (var i = Trace.Listeners.Count - 1; i >= 0; --i)
            {
                var listener = Trace.Listeners[i];
                listener.Flush();
                Trace.Listeners.Remove(Trace.Listeners[i]);
                listener.Dispose();
            }
        }

        private void RemoveListeners(XmlNode listenersNode)
        {
            for (var i = Trace.Listeners.Count - 1; i >= 0; --i)
            {
                var listener = Trace.Listeners[i];
                if (!ListenerExists(listener.Name, listenersNode))
                {
                    listener.Flush();
                    Trace.Listeners.Remove(Trace.Listeners[i]);
                    listener.Dispose();
                }
            }
        }

        private void AddListeners(XmlNode listenersNode)
        {
            foreach (XmlNode listenerNode in listenersNode.ChildNodes)
            {
                var listenerName = string.Empty;
                try
                {
                    listenerName = listenerNode.Attributes["name"].Value;
                    var typeString = listenerNode.Attributes["type"].Value;
                    var initializationDataAttribute = listenerNode.Attributes["initializeData"];
                    if (!ListenerExists(listenerName))
                    {
                        AddListener(listenerName, typeString, initializationDataAttribute);
                    }
                }
                catch (Exception ex)
                {
                    nHydrateLog.LogWarning(ex, "Cannot Add One Of The Listeners: " + listenerName);
                }
            }

        }

        private void AddListener(string listenerName, string typeString, XmlAttribute initializationDataAttribute)
        {
            if (initializationDataAttribute != null)
            {
                var initializationData = initializationDataAttribute.Value;
                AddListener(listenerName, typeString, initializationData);
            }
            else
            {
                AddListener(listenerName, typeString);
            }
        }

        private void AddListener(string listenerName, string typeString)
        {
            var newListener = CreateListener(listenerName, typeString);
            if (newListener != null)
                Trace.Listeners.Add(newListener);
        }

        private void AddListener(string listenerName, string typeString, string initializationData)
        {
            var newListener = CreateListener(listenerName, typeString, initializationData);
            if (newListener != null)
                Trace.Listeners.Add(newListener);
        }

        private bool ListenerExists(string listenerName, XmlNode listenersNode)
        {
            var testNode = listenersNode.SelectSingleNode(String.Format("add[@name = \"{0}\"]", listenerName));
            return !(testNode == null);
        }

        private bool ListenerExists(string listenerName)
        {
            var retVal = false;
            foreach (TraceListener tl in Trace.Listeners)
            {
                if (StringHelper.Match(listenerName, tl.Name))
                {
                    retVal = true;
                }
            }
            return retVal;
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


        private TraceListener CreateListener(string listenerName, string typeString)
        {
            TraceListener retVal = null;
            try
            {
                nHydrateLog.LogVerbose("CreateListener(string listenerName:{0}, string typeString:{1})", listenerName, typeString);

                if (typeString == ("System.Diagnostics.DefaultTraceListener"))
                {
                    retVal = new DefaultTraceListener();
                }
                else if (typeString == ("System.Diagnostics.EventLogTraceListener"))
                {
                    retVal = new EventLogTraceListener();
                }
                else
                {
                    retVal = (TraceListener)ReflectionHelper.CreateInstance(Type.GetType(typeString));
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

    #region class WSTraceSwitch
    public class WSTraceSwitch : TraceSwitch
    {
        internal WSTraceSwitch(string name, string description) : base(name, description) { }
    }
    #endregion

}