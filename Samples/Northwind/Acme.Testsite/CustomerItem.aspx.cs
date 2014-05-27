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
	public partial class CustomerItem : BasePage
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
			Customer customer = Customer.SelectUsingPK(this.Request["id"] + "");
			if (customer == null)
			{
				this.Response.Redirect(SessionHelper.LastCustomerListSearch);
				return;
			}

			lblCustomerId.Text = customer.CustomerId;
			txtAddress.Text = customer.Address;
			txtCity.Text = customer.City;
			txtCompany.Text = customer.CompanyName;
			txtContact.Text = customer.ContactName;
			txtPhone.Text = customer.Phone;
			txtZip.Text = customer.PostalCode;

		}

		#endregion

		#region Event Handlers

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Response.Redirect(SessionHelper.LastCustomerListSearch);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			Customer customer = Customer.SelectUsingPK(this.Request["id"]);

			customer.Address = txtAddress.Text;
			customer.City = txtCity.Text;
			customer.CompanyName = txtCompany.Text;
			customer.ContactName = txtContact.Text;
			customer.Phone = txtPhone.Text;
			customer.PostalCode = txtZip.Text;
			customer.Persist();

			this.Response.Redirect(SessionHelper.LastCustomerListSearch);
		}

		#endregion

	}
}