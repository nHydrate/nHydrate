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
	public partial class OrderList : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			grdItem.RowDataBound += new GridViewRowEventHandler(grdItem_RowDataBound);
			this.PagingControl1.ObjectSingular = "Order";
			this.PagingControl1.ObjectPlural = "Orders";
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
			OrderPaging paging = new OrderPaging(
				this.PagingControl1.PageIndex,
				this.PagingControl1.RecordsPerPage,
				Order.FieldNameConstants.OrderId,
				true);

			OrderCollection orderCollection = null;

			if (this.Request["customerid"] != null)
			{
				orderCollection = OrderCollection.RunSelect(x => x.CustomerId == this.Request["customerid"], paging);
				lblHeader.Text = "This is a list of all orders for customer '" + this.Request["customerid"] + "'.";
			}
			else
			{
				orderCollection = OrderCollection.RunSelect(x => true, paging);
				lblHeader.Text = "This is a list of all orders.";
			}

			this.PagingControl1.ItemCount = paging.RecordCount;
			grdItem.DataSource = orderCollection;
			grdItem.DataBind();
			SessionHelper.LastOrderListSearch = this.Request.Url.AbsoluteUri;
		}

		#endregion

		#region Event Handlers

		private void grdItem_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				Order order = (Order)e.Row.DataItem;
				var linkEdit = (HyperLink)e.Row.FindControl("linkEdit");
				var linkOrderDetails = (HyperLink)e.Row.FindControl("linkOrderDetails");

				linkEdit.NavigateUrl = "/OrderItem.aspx?id=" + order.OrderId;
				linkOrderDetails.NavigateUrl = "/OrderDetailList.aspx?orderid=" + order.OrderId;
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