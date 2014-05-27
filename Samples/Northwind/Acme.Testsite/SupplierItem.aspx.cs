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
	public partial class SupplierItem : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			cmdSave.Click += new EventHandler(cmdSave_Click);
			cmdCancel.Click += new EventHandler(cmdCancel_Click);

			if (!this.IsPostBack)
			{
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
			int supplierId;
			int.TryParse(this.Request["id"], out supplierId);
			Supplier supplier = Supplier.SelectUsingPK(supplierId);
			if (supplier == null)
			{
				this.Response.Redirect(SessionHelper.LastSupplierListSearch);
				return;
			}

			lblSupplierId.Text = supplier.SupplierId.ToString();
			txtAddress.Text = supplier.Address;
			txtCity.Text = supplier.City;
			txtCompany.Text = supplier.CompanyName;
			txtContact.Text = supplier.ContactName;
			txtHomePage.Text = supplier.HomePage;
			txtPhone.Text = supplier.Phone;
			txtZip.Text = supplier.PostalCode;

		}

		#endregion

		#region Event Handlers

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Response.Redirect(SessionHelper.LastSupplierListSearch);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			int supplierId;
			int.TryParse(this.Request["id"], out supplierId);
			Supplier supplier = Supplier.SelectUsingPK(supplierId);

			supplier.Address = txtAddress.Text;
			supplier.City = txtCity.Text;
			supplier.CompanyName = txtCompany.Text;
			supplier.ContactName = txtContact.Text;
			supplier.HomePage = txtHomePage.Text;
			supplier.Phone = txtPhone.Text;
			supplier.PostalCode = txtZip.Text;
			supplier.Persist();

			this.Response.Redirect(SessionHelper.LastCustomerListSearch);
		}

		#endregion

	}
}
