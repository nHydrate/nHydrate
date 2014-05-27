using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Acme.Northwind.EFDAL;
using Acme.Northwind.EFDAL.Entity;
using Northwind.TestSite.Objects;

namespace Northwind.TestSite
{
	public partial class CustomerList : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			grdItem.RowDataBound += new GridViewRowEventHandler(grdItem_RowDataBound);
			this.PagingControl1.ObjectSingular = "Customer";
			this.PagingControl1.ObjectPlural = "Customers";
			this.PagingControl1.RecordsPerPageChanged += new EventHandler(PagingControl1_RecordsPerPageChanged);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!this.IsPostBack)
			{
				this.Populate();
			}
		}

		#endregion

		#region Methods

		private void Populate()
		{
			CustomerPaging paging = new CustomerPaging(this.PagingControl1.PageIndex, this.PagingControl1.RecordsPerPage, Customer.FieldNameConstants.CustomerId, true);
			CustomerCollection customerCollection = CustomerCollection.RunSelect(x => true, paging);
			this.PagingControl1.ItemCount = paging.RecordCount;
			grdItem.DataSource = customerCollection;
			grdItem.DataBind();
			SessionHelper.LastCustomerListSearch = this.Request.Url.AbsoluteUri;
		}

		#endregion

		#region Event Handlers

		private void grdItem_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				Customer customer = (Customer)e.Row.DataItem;
				var linkEdit = (HyperLink)e.Row.FindControl("linkEdit");
				var linkOrders = (HyperLink)e.Row.FindControl("linkOrders");
				
				linkEdit.NavigateUrl = "/CustomerItem.aspx?id=" + customer.CustomerId;
				linkOrders.NavigateUrl = "/OrderList.aspx?customerid=" + customer.CustomerId;
			}
		}

		private void PagingControl1_RecordsPerPageChanged(object sender, EventArgs e)
		{
			var query = new URL(this.Request.Url.AbsoluteUri);
			query.Parameters.SetValue("rpp", PagingControl1.RecordsPerPage.ToString());
			query.Parameters.SetValue("po", "1");
			this.Response.Redirect(query.ToString());
		}

		#endregion

	}
}
