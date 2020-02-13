using System;
using System.Threading;
using System.Windows.Forms;
using nHydrate.Generator.Common.Logging;

namespace nHydrate.Generator.Common.Exceptions
{
	public class ThreadExceptionHandler
	{

		public void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			nHydrateLog.LogVerbose("Call: Application_ThreadException(object sender, ThreadExceptionEventArgs e)");
			try
			{
				ShowThreadExceptionDialog(e.Exception);
			}
			catch
			{
				try
				{
					MessageBox.Show("Fatal Error", 
						"Fatal Error",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Stop);
				}
				finally
				{
					Application.Exit();
				}
			}
		}

		public void Application_ThreadException(object sender, UnhandledExceptionEventArgs e)
		{
			nHydrateLog.LogVerbose("Call: Application_ThreadException(object sender, UnhandledExceptionEventArgs e)");
			try
			{
				if (e.IsTerminating)
				{
					nHydrateLog.LogError(e.ExceptionObject.ToString());
				}
				else
				{
					ShowThreadExceptionDialog(e.ExceptionObject);
				}
			}
			catch
			{
				try
				{
					MessageBox.Show("Fatal Error",
						"Fatal Error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Stop);
				}
				finally
				{
					Application.Exit();
				}
			}
		}


		private void ShowThreadExceptionDialog(object ex)
		{
			var newLine = Environment.NewLine;
			var errorMessage = String.Empty;
			if (ex.GetType().IsAssignableFrom(typeof(System.Exception))) 
			{
				var systemException = (Exception)ex;
				errorMessage = "Unhandled Exception: " + systemException.Message + newLine +
				"Exception Type: " + systemException.GetType() + newLine +
				"Stack Trace:" + newLine +
				systemException.StackTrace;
			}
			else
			{
				errorMessage = ex.ToString();
			}

			var exceptionForm = new ThreadExceptionHandlerForm(errorMessage);
			if (exceptionForm.ShowDialog() == DialogResult.Abort)
			{
				Application.Exit();
			}
		}
	} 
}

