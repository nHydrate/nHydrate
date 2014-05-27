using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Northwind.TestSite.Objects
{
	public class URL
	{
		private string _page = "";
		private URLParameterCollection _parameters = new URLParameterCollection();

		public URL()
		{
		}

		public URL(string url)
			: this()
		{
			if (url.Contains("%")) url = HttpUtility.UrlDecode(url);
			string[] arr = url.Split('?');
			_page = arr[0];

			if (arr.Length == 2)
			{
				string[] arr2 = arr[1].Split('&');
				foreach (string raw in arr2)
				{
					string[] arr3 = raw.Split('=');
					if (arr3.Length == 2)
					{
						this.Parameters.Add(new URLParameter(arr3[0], arr3[1]));
					}
				}
			}

		}

		public string Page
		{
			get { return _page; }
			set { _page = value; }
		}

		public URLParameterCollection Parameters
		{
			get { return _parameters; }
		}

		public string GetURL()
		{
			string retval = "";
			return retval;
		}

		public int PageOffset
		{
			get
			{
				int retval;
				if (int.TryParse(this.Parameters.GetValue("po"), out retval))
					return retval;
				else
					return 1;
			}
			set { this.Parameters.SetValue("po", value.ToString()); }
		}

		public int RecordsPerPage
		{
			get
			{
				int retval;
				if (int.TryParse(this.Parameters.GetValue("rpp"), out retval))
					return retval;
				else
					return 1;
			}
			set { this.Parameters.SetValue("rpp", value.ToString()); }
		}

		public override string ToString()
		{
			string retval = this.Page;
			if (this.Parameters.Count != 0)
			{
				retval += "?";
				foreach (URLParameter item in this.Parameters)
				{
					retval += item.Name + "=" + item.Value + "&";
				}
			}

			if (retval.EndsWith("&"))
				retval = retval.Substring(0, retval.Length - 1);

			return retval;
		}
	
	}
}