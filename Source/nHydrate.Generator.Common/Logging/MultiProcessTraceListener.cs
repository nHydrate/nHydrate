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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace nHydrate.Generator.Common.Logging
{
	public sealed partial class MultiProcessFileTraceListener : TraceListener
	{
		private readonly Thread _writeThread;
		private readonly MultiProcessLogFileWriter _logFile;
		private const long _def_fileSize = 1024 * 1024 * 5;

		public MultiProcessFileTraceListener(string input)
		{
			var initializeData = input.Split('?');
			long fileSize;
			if (initializeData.Length > 1)
			{
				try
				{
					fileSize = long.Parse(initializeData[1]);
				}
				catch
				{
					fileSize = _def_fileSize;
				}
			}
			else
			{
				fileSize = _def_fileSize;
			}
			_logFile = new MultiProcessLogFileWriter(initializeData[0], fileSize);
			_writeThread = new Thread(new ThreadStart(_logFile.ThreadStartProc));
			_writeThread.IsBackground = true;
			_writeThread.Start();
		}

		override public void Write(string message)
		{
			_logFile.Write(message);
		}

		override public void WriteLine(string message)
		{
			_logFile.WriteLine(message);
		}

		private bool _disposed = false;
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (!_disposed)
				{
					_logFile.Dispose();
					Thread.Sleep(1000);
					if (_writeThread != null && _writeThread.ThreadState != System.Threading.ThreadState.Aborted)
						_writeThread.Abort();
					_disposed = true;
				}
			}
			else
			{
				if (!_disposed)
				{
					if (_logFile != null)
					{
						_logFile.Dispose();
						Thread.Sleep(1000);
					}
					if (_writeThread != null && _writeThread.ThreadState != System.Threading.ThreadState.Aborted)
						_writeThread.Abort();
					_disposed = true;

				}
			}
		}

		~MultiProcessFileTraceListener()
		{
			Dispose(false);
		}


		#region Class MultiProcessLogFileWriter
		private partial class MultiProcessLogFileWriter : IDisposable
		{
			private readonly long _fileSize;

			#region member variables
			private Mutex _mutex;
			private readonly FileInfo _file;
			private readonly ArrayList _logStatementBuffer;
			private readonly AutoResetEvent _addedToList;
			private bool _kill = false;
			#endregion

			#region constructors
			public MultiProcessLogFileWriter(string fileName, long fileSize)
			{
				_fileSize = fileSize;
				_addedToList = new AutoResetEvent(false);
				_logStatementBuffer = new ArrayList();
				_file = new FileInfo(fileName);
				InitializeMutex(fileName);
			}
			#endregion

			#region thread start
			public void ThreadStartProc()
			{
				try
				{
					while (!_kill)
					{
						_addedToList.WaitOne();
						FlushBuffer();
					}
				}
				catch (System.Threading.ThreadAbortException)
				{
					//eat it!
				}
				catch (Exception ex)
				{
					nHydrateLog.LogLogFailure(ex.ToString());
				}
			}
			#endregion

			#region write methods
			public void Write(string message)
			{
				lock (_logStatementBuffer)
				{
					var ls = new LogStatement(false, message);
					_logStatementBuffer.Add(ls);
				}
				_addedToList.Set();
			}

			public void WriteLine(string message)
			{
				lock (_logStatementBuffer)
				{
					var ls = new LogStatement(true, message);
					_logStatementBuffer.Add(ls);
				}
				_addedToList.Set();
			}
			#endregion

			#region private helpers
			private void InitializeMutex(string fileName)
			{
				var mutexId = fileName.Replace(":", "_");
				mutexId = mutexId.Replace(@"\", "_");
				mutexId = mutexId.Replace("/", "_");
				mutexId = mutexId.Replace(".", "_");
				_mutex = new Mutex(false, mutexId);
			}

			private ArrayList GetBufferCopy()
			{
				ArrayList copyOfBuffer = null;
				lock (_logStatementBuffer)
				{
					copyOfBuffer = new ArrayList(_logStatementBuffer);
					_logStatementBuffer.Clear();
				}
				return copyOfBuffer;
			}

			private void FlushBuffer()
			{
				_mutex.WaitOne();
				try
				{
					var logBufferCopy = GetBufferCopy();
					if (logBufferCopy != null)
					{
						WriteStatements(logBufferCopy);
						_file.Refresh();
						lock (_file)
						{
							if (_file.Exists && _file.Length > _fileSize)
							{
								var filePattern = "*" + _file.Name;
								var logDirectory = _file.Directory;
								var destFileName = DateTime.UtcNow.ToString("yyyyMMdd-hhmmss") + "_" + System.Guid.NewGuid() + "_" + _file.Name;
								destFileName = Path.Combine(logDirectory.FullName, destFileName);
								_file.CopyTo(destFileName, true);
								_file.Delete();
								DeleteOldFiles(filePattern, logDirectory, _file);
							}
						}
					}
				}
				catch (Exception ex)
				{
					nHydrateLog.LogLogFailure(ex.ToString());
				}
				finally
				{
					_mutex.ReleaseMutex();
				}
			}
			private static int CompareFiles(FileInfo f1, FileInfo f2)
			{
				return f1.LastWriteTime.CompareTo(f2.LastWriteTime);
			}

			private static void DeleteOldFiles(string filePattern, DirectoryInfo workingDirectory, FileInfo _file)
			{
				var cnt = workingDirectory.GetFiles(filePattern).Length;

				var smaxLogFiles = System.Configuration.ConfigurationManager.AppSettings["maxlogfiles"];

				var maxLogFiles = 5;

				if (!int.TryParse(smaxLogFiles, out maxLogFiles))
					maxLogFiles = 5;

				if (cnt > maxLogFiles)
				{
					var sl = new System.Collections.Generic.List<FileInfo>();
					foreach (var file in workingDirectory.GetFiles(filePattern))
						sl.Add(file);

					sl.Sort(new Comparison<FileInfo>(CompareFiles));


					for (var ii = 0; ii < sl.Count; ii++)
					{
						if (ii < sl.Count - maxLogFiles)
						{
							try
							{
								sl[ii].Delete();
								nHydrateLog.LogAlways(String.Format("Log File Deleted: {0} ", sl[ii].FullName));
							}
							catch (Exception ex)
							{
								nHydrateLog.LogLogFailure(ex.ToString());
							}
						}
					}
				}
				foreach (var file in workingDirectory.GetFiles(filePattern))
				{
					try
					{

						var fileage = DateTime.Now.Subtract(file.LastWriteTime);
						if (fileage.TotalDays > 7)
							file.Delete();
					}
					catch (Exception ex)
					{
						nHydrateLog.LogLogFailure(ex.ToString());
					}
				}
			}

			private void WriteStatements(ArrayList logStatements)
			{
				StreamWriter sw = null;
				if (!_file.Directory.Exists)
				{
					_file.Directory.Create();
				}

				if (!_file.Exists)
				{
					sw = _file.CreateText();
				}
				else
				{
					sw = _file.AppendText();
				}

				using (sw)
				{
					foreach (LogStatement ls in logStatements)
					{
						if (ls.WriteLine)
						{
							sw.WriteLine(ls.Message);
						}
						else
						{
							sw.Write(ls.Message);
						}
					}
				}
			}
			#endregion

			#region Struct LogStatement
			private struct LogStatement
			{
				public LogStatement(bool writeLine, string message)
				{
					WriteLine = writeLine;
					Message = message;
				}
				public readonly bool WriteLine;
				public readonly string Message;
			}
			#endregion

			#region IDisposable Members
			bool _disposed = false;
			protected virtual void Dispose(bool disposing)
			{
				if (!_disposed)
				{
					_kill = true;
					if (_addedToList != null)
						_addedToList.Set();
					_disposed = true;
				}
			}

			public void Dispose()
			{
				Dispose(true);
			}

			~MultiProcessLogFileWriter()
			{
				Dispose(false);
			}
			#endregion
		}

		#endregion

	}

}

