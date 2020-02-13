using System;
using System.IO;
using System.Net;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.Common.Util
{
	public class HTTPHelper
	{
		#region Class Members

		// The stream of data retrieved from the web server
		private Stream _response;
		// The stream of data that we write to the harddrive
		private Stream _local;
		// The response from the web server containing information about the file
		//private HttpWebResponse _webResponse;

		#endregion

		#region Events

		public event EventHandler<ProgressEventArgs> Progress;

		protected virtual void OnProgress(ProgressEventArgs e)
		{
			if (this.Progress != null)
			{
				this.Progress(this, e);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Given a file URL, this method will download it and save it as the specified local file name
		/// </summary>
		/// <param name="url">A URL of a file to download</param>
		/// <param name="localFile">The local filename to use when saving the file locally</param>
		/// <returns>True if the action was successful</returns>
		public bool Download(string url, string localFile)
		{
			var f = Download(url);
			//remove the file if neccessary
			if (File.Exists(localFile)) File.Delete(localFile);
			//Move the temp file to the new file
			File.Move(f, localFile);
			return true;
		}

		/// <summary>
		/// Given a file URL, this method will download it and return the load disk file
		/// </summary>
		/// <param name="url">A URL of a file to download</param>
		/// <returns>A local file name where the downloaded URL was saved</returns>
		public string Download(string url)
		{
			var retval = System.IO.Path.GetTempFileName();
			using (var wcDownload = new WebClient())
			{
				//try
				//{
				//  // Create a request to the file we are downloading
				//  HttpWebRequest _webRequest = (HttpWebRequest)WebRequest.Create(url);
				//  // Set default authentication for retrieving the file
				//  _webRequest.Credentials = CredentialCache.DefaultCredentials;
				//  // Retrieve the response from the server
				//  _webResponse = (HttpWebResponse)_webRequest.GetResponse();
				//  // Ask the server for the file size and store it
				//  Int64 fileSize = _webResponse.ContentLength;
				//}
				//finally
				//{
				//  // When the above code has ended, close the streams
				//  _response.Close();
				//  _local.Close();
				//}

				try
				{
					// Open the URL for download
					_response = wcDownload.OpenRead(url);
					// Create a new file stream where we will be saving the data (local drive)
					_local = new FileStream(retval, FileMode.Create, FileAccess.Write, FileShare.None);

					// It will store the current number of bytes we retrieved from the server
					var bytesSize = 0;
					// A buffer for storing and writing the data retrieved from the server
					var downBuffer = new byte[2048];

					// Loop through the buffer until the buffer is empty
					while ((bytesSize = _response.Read(downBuffer, 0, downBuffer.Length)) > 0)
					{
						// Write the data from the buffer to the local hard drive
						_local.Write(downBuffer, 0, bytesSize);
					}
				}
				catch (WebException ex)
				{
					if (ex.Message.Contains("The remote server returned an error: (404) Not Found"))
						return string.Empty;
					else
						throw;
				}
				finally
				{
					// When the above code has ended, close the streams
					if (_response != null) _response.Close();
					if (_local != null) _local.Close();
				}
			}
			return retval;

		}

		#endregion

		#region Event Handlers

		private void UpdateProgress(Int64 BytesRead, Int64 TotalBytes)
		{
			// Calculate the download progress in percentages
			var percentProgress = Convert.ToInt32((BytesRead * 100) / TotalBytes);
			this.OnProgress(new ProgressEventArgs(percentProgress));
		}

		#endregion

	}

}

