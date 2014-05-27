using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Northwind.TestSite.Objects;
using Acme.Northwind.EFDAL;
using Acme.Northwind.EFDAL.Entity;

namespace Northwind.TestSite
{
	public partial class OrderDetailItem : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			cmdSave.Click += new EventHandler(cmdSave_Click);
			cmdCancel.Click += new EventHandler(cmdCancel_Click);

			if (!this.IsPostBack)
			{
				//Load the products
				ProductCollection productCollection = ProductCollection.RunSelect();
				cboProduct.DataTextField = Product.FieldNameConstants.ProductName.ToString();
				cboProduct.DataValueField = Product.FieldNameConstants.ProductId.ToString();
				cboProduct.DataSource = (from x in productCollection orderby x.ProductName select x);
				cboProduct.DataBind();
			}
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
			int orderId;
			int productId;
			int.TryParse(this.Request["orderid"], out orderId);
			int.TryParse(this.Request["productid"], out productId);
			OrderDetail orderDetail = OrderDetail.SelectUsingPK(orderId, productId);
			if (orderDetail == null)
			{
				this.Response.Redirect(SessionHelper.LastOrderDetailListSearch);
				return;
			}

			lblOrderId.Text = orderDetail.OrderId.ToString();
			cboProduct.SelectedValue = orderDetail.ProductId.ToString();
			txtDiscount.Text = orderDetail.Discount.ToString();
			txtPrice.Text = orderDetail.UnitPrice.ToString();
			txtQuantity.Text = orderDetail.Quantity.ToString();

		}

		#endregion

		#region Event Handlers

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Response.Redirect(SessionHelper.LastOrderListSearch);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			int orderId;
			int productId;
			int.TryParse(this.Request["orderid"], out orderId);
			int.TryParse(this.Request["productid"], out productId);
			OrderDetail orderDetail = OrderDetail.SelectUsingPK(orderId, productId);

			//orderDetail.ProductId = int.Parse(cboProduct.SelectedValue );
			orderDetail.Discount = float.Parse(txtDiscount.Text);
			orderDetail.UnitPrice = decimal.Parse(txtPrice.Text);
			orderDetail.Quantity = short.Parse(txtQuantity.Text);
			orderDetail.Persist();

			this.Response.Redirect(SessionHelper.LastOrderListSearch);
		}

		#endregion

	}
}
