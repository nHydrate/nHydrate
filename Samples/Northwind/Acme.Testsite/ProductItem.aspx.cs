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
	public partial class ProductItem : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			cmdSave.Click += new EventHandler(cmdSave_Click);
			cmdCancel.Click += new EventHandler(cmdCancel_Click);

			if (!this.IsPostBack)
			{
				//Load the suppliers
				SupplierCollection supplierCollection = SupplierCollection.RunSelect();
				cboSupplier.DataTextField = Supplier.FieldNameConstants.CompanyName.ToString();
				cboSupplier.DataValueField = Supplier.FieldNameConstants.SupplierId.ToString();
				cboSupplier.DataSource = (from x in supplierCollection orderby x.CompanyName select x);
				cboSupplier.DataBind();

				//Load the cateogries
				CategoryCollection categoryCollection = CategoryCollection.RunSelect();
				cboCategory.DataTextField = Category.FieldNameConstants.CategoryName.ToString();
				cboCategory.DataValueField = Category.FieldNameConstants.CategoryId.ToString();
				cboCategory.DataSource = (from x in categoryCollection orderby x.CategoryName select x);
				cboCategory.DataBind();
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
			int productId;
			int.TryParse(this.Request["id"], out productId);
			Product product = Product.SelectUsingPK(productId);
			if (product == null)
			{
				this.Response.Redirect(SessionHelper.LastProductListSearch);
				return;
			}

			lblProductId.Text = product.ProductId.ToString();
			txtInStock.Text = product.UnitsInStock.ToString();
			txtName.Text = product.ProductName;
			txtOnOrder.Text = product.UnitsOnOrder.ToString();
			txtPrice.Text = product.UnitPrice.ToString();
			txtQuantityPerUnit.Text = product.QuantityPerUnit;
			cboCategory.SelectedValue = product.CategoryId.ToString();
			cboSupplier.SelectedValue = product.SupplierId.ToString();

		}

		#endregion

		#region Event Handlers

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Response.Redirect(SessionHelper.LastProductListSearch);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			int productId;
			int.TryParse(this.Request["id"], out productId);
			Product product = Product.SelectUsingPK(productId);

			product.UnitsInStock = short.Parse(txtInStock.Text);
			product.ProductName = txtName.Text;
			product.UnitsOnOrder = short.Parse(txtOnOrder.Text);
			product.UnitPrice = decimal.Parse(txtPrice.Text);
			product.QuantityPerUnit = txtQuantityPerUnit.Text;
			product.CategoryId = int.Parse(cboCategory.SelectedValue);
			product.SupplierId = int.Parse(cboSupplier.SelectedValue);
			product.Persist();

			this.Response.Redirect(SessionHelper.LastCustomerListSearch);
		}

		#endregion

	}
}
