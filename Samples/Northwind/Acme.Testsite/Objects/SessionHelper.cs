using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Northwind.TestSite.Objects
{
	internal static class SessionHelper
	{
		public static string LastCustomerListSearch
		{
			get
			{
				if (HttpContext.Current.Session["LastCustomerListSearch"] == null)
					return "/CustomerList.aspx";
				else
					return (string)HttpContext.Current.Session["LastCustomerListSearch"];
			}
			set { HttpContext.Current.Session["LastCustomerListSearch"] = value; }
		}

		public static string LastOrderListSearch
		{
			get
			{
				if (HttpContext.Current.Session["LastOrderListSearch"] == null)
					return "/OrderList.aspx";
				else
					return (string)HttpContext.Current.Session["LastOrderListSearch"];
			}
			set { HttpContext.Current.Session["LastOrderListSearch"] = value; }
		}

		public static string LastProductListSearch
		{
			get
			{
				if (HttpContext.Current.Session["LastProductListSearch"] == null)
					return "/ProductList.aspx";
				else
					return (string)HttpContext.Current.Session["LastProductListSearch"];
			}
			set { HttpContext.Current.Session["LastProductListSearch"] = value; }
		}

		public static string LastCategoryListSearch
		{
			get
			{
				if (HttpContext.Current.Session["LastCategoryListSearch"] == null)
					return "/CategoryList.aspx";
				else
					return (string)HttpContext.Current.Session["LastCategoryListSearch"];
			}
			set { HttpContext.Current.Session["LastCategoryListSearch"] = value; }
		}

		public static string LastSupplierListSearch
		{
			get
			{
				if (HttpContext.Current.Session["LastSupplierListSearch"] == null)
					return "/SupplierList.aspx";
				else
					return (string)HttpContext.Current.Session["LastSupplierListSearch"];
			}
			set { HttpContext.Current.Session["LastSupplierListSearch"] = value; }
		}

		public static string LastRegionListSearch
		{
			get
			{
				if (HttpContext.Current.Session["LastRegionListSearch"] == null)
					return "/RegionList.aspx";
				else
					return (string)HttpContext.Current.Session["LastRegionListSearch"];
			}
			set { HttpContext.Current.Session["LastRegionListSearch"] = value; }
		}

		public static string LastTerritoryListSearch
		{
			get
			{
				if (HttpContext.Current.Session["LastTerritoryListSearch"] == null)
					return "/RegionList.aspx";
				else
					return (string)HttpContext.Current.Session["LastTerritoryListSearch"];
			}
			set { HttpContext.Current.Session["LastTerritoryListSearch"] = value; }
		}		

		public static string LastOrderDetailListSearch
		{
			get
			{
				if (HttpContext.Current.Session["LastOrderDetailListSearch"] == null)
					return "/RegionList.aspx";
				else
					return (string)HttpContext.Current.Session["LastOrderDetailListSearch"];
			}
			set { HttpContext.Current.Session["LastOrderDetailListSearch"] = value; }
		}		
		
	}
}
