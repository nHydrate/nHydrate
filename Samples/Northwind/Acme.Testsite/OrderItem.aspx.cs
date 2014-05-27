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
	public partial class OrderItem : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			cmdSave.Click += new EventHandler(cmdSave_Click);
			cmdCancel.Click += new EventHandler(cmdCancel_Click);
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
			int.TryParse(this.Request["id"], out orderId);
			Order order = Order.SelectUsingPK(orderId);
			if (order == null)
			{
				this.Response.Redirect(SessionHelper.LastOrderListSearch);
				return;
			}

			lblOrderId.Text = order.OrderId.ToString();
			txtAddress.Text = order.ShipAddress;
			txtCity.Text = order.ShipCity;
			txtZip.Text = order.ShipPostalCode;			
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
			int.TryParse(this.Request["id"], out orderId);
			Order order = Order.SelectUsingPK(orderId);

			order.ShipAddress = txtAddress.Text;
			order.ShipCity = txtCity.Text;
			order.ShipPostalCode = txtZip.Text;
			order.Persist();

			this.Response.Redirect(SessionHelper.LastOrderListSearch);
		}

		#endregion

	}
}
