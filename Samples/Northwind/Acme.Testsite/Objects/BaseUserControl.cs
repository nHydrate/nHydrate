using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Northwind.TestSite.Objects
{
	public class BaseUserControl : System.Web.UI.UserControl
	{
		public new BasePage Page
		{
			get { return (BasePage)base.Page; }
			set { base.Page = value; }
		}
	}
}
