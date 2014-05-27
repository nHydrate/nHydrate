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
	public partial class ProductList : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			grdItem.RowDataBound += new GridViewRowEventHandler(grdItem_RowDataBound);
			this.PagingControl1.ObjectSingular = "Product";
			this.PagingControl1.ObjectPlural = "Products";
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
			ProductPaging paging = new ProductPaging(this.PagingControl1.PageIndex, this.PagingControl1.RecordsPerPage, Product.FieldNameConstants.ProductId, true);
			ProductCollection productCollection = null;

			if (this.Request["categoryId"] != null)
			{
				int categoryId;
				int.TryParse(this.Request["categoryId"], out categoryId);
				productCollection = ProductCollection.RunSelect(x => x.CategoryId == categoryId, paging);
				Category category = Category.SelectUsingPK(categoryId);
				lblHeader.Text = "This is a list of all products for category <a href='/CategoryItem.aspx?id=" + category.CategoryId + "'>" + category.CategoryName + "</a>.";
			}
			else if (this.Request["supplierId"] != null)
			{
				int supplierId;
				int.TryParse(this.Request["supplierId"], out supplierId);
				productCollection = ProductCollection.RunSelect(x => x.SupplierId == supplierId, paging);
				Supplier supplier = Supplier.SelectUsingPK(supplierId);
				lblHeader.Text = "This is a list of all products for supplier <a href='/SupplierItem.aspx?id=" + supplier.SupplierId + "'>" + supplier.CompanyName + "</a>.";
			}
			else
			{
				productCollection = ProductCollection.RunSelect(x => true, paging);
				lblHeader.Text = "This is a list of all products.";
			}

			this.PagingControl1.ItemCount = paging.RecordCount;
			grdItem.DataSource = productCollection;
			grdItem.DataBind();
			SessionHelper.LastOrderListSearch = this.Request.Url.AbsoluteUri;
		}

		#endregion

		#region Event Handlers

		private void grdItem_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				Product product = (Product)e.Row.DataItem;
				var linkEdit = (HyperLink)e.Row.FindControl("linkEdit");
				var linkOrderDetails = (HyperLink)e.Row.FindControl("linkOrderDetails");
				HyperLink lblSupplier = (HyperLink)e.Row.FindControl("lblSupplier");
				HyperLink lblCategory = (HyperLink)e.Row.FindControl("lblCategory");

				linkEdit.NavigateUrl = "/ProductItem.aspx?id=" + product.ProductId;
				linkOrderDetails.NavigateUrl = "/OrderDetailList.aspx?productid=" + product.ProductId;

				if (product.SupplierId != null)
				{
					lblSupplier.Text = product.SupplierItem.CompanyName;
					lblSupplier.NavigateUrl = "/SupplierItem.aspx?id=" + product.SupplierId.ToString();
				}

				if (product.CategoryId != null)
				{
					lblCategory.Text = product.CategoryItem.CategoryName;
					lblCategory.NavigateUrl = "/CategoryItem.aspx?id=" + product.CategoryId.ToString();
				}

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