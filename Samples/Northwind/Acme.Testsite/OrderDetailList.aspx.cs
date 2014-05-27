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
	public partial class OrderDetailList : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			grdItem.RowDataBound += new GridViewRowEventHandler(grdItem_RowDataBound);
			this.PagingControl1.ObjectSingular = "Detail";
			this.PagingControl1.ObjectPlural = "Details";
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
			OrderDetailPaging paging = new OrderDetailPaging(this.PagingControl1.PageIndex, this.PagingControl1.RecordsPerPage, OrderDetail.FieldNameConstants.OrderId, true);
			OrderDetailCollection orderDetailCollection = null;

			if (this.Request["orderid"] != null)
			{
				int orderId;
				int.TryParse(this.Request["orderid"], out orderId);
				orderDetailCollection = OrderDetailCollection.RunSelect(x => x.OrderId == orderId, paging);
				lblHeader.Text = "This is a list of all orders details for order <a href='/OrderItem.aspx?id=" + orderId + "'>" + orderId + "</a>.";
			}
			else if (this.Request["productid"] != null)
			{
				int productId = int.Parse(this.Request["productid"]);
				orderDetailCollection = OrderDetailCollection.RunSelect(x => x.ProductId == productId, paging);
				Product product = Product.SelectUsingPK(productId);
				lblHeader.Text = "This is a list of all orders details for product <a href='/ProductItem.aspx?id=" + product.ProductId + "'>" + product.ProductName + "</a>.";
			}
			else
			{
				orderDetailCollection = OrderDetailCollection.RunSelect(x => true, paging);
				lblHeader.Text = "This is a list of all order details.";
			}

			this.PagingControl1.ItemCount = paging.RecordCount;
			grdItem.DataSource = orderDetailCollection;
			grdItem.DataBind();
			SessionHelper.LastOrderListSearch = this.Request.Url.AbsoluteUri;
		}

		#endregion

		#region Event Handlers

		private void grdItem_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				OrderDetail orderDetail = (OrderDetail)e.Row.DataItem;
				var linkEdit = (HyperLink)e.Row.FindControl("linkEdit");
				var linkProduct = (HyperLink)e.Row.FindControl("linkProduct");
				
				linkEdit.NavigateUrl = "/OrderDetailItem.aspx?orderid=" + orderDetail.OrderId + "&productid=" + orderDetail.ProductId;
				linkProduct.Text = orderDetail.ProductItem.ProductName;
				linkProduct.NavigateUrl = "/ProductItem.aspx?id=" + orderDetail.ProductId;
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
