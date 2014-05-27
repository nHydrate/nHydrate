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
	public partial class SupplierList : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			grdItem.RowDataBound += new GridViewRowEventHandler(grdItem_RowDataBound);
			this.PagingControl1.ObjectSingular = "Supplier";
			this.PagingControl1.ObjectPlural = "Suppliers";
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
			using (var context = new NorthwindEntities())
			{
				var paging = new nHydrate.EFCore.DataAccess.Paging(this.PagingControl1.PageIndex, this.PagingControl1.RecordsPerPage);
				var list = context.Product
					.GetPagedResults(x => x.SupplierID, paging);

				this.PagingControl1.ItemCount = paging.RecordCount;
				grdItem.DataSource = list;
				grdItem.DataBind();
				SessionHelper.LastSupplierListSearch = this.Request.Url.AbsoluteUri;
			}
		}

		#endregion

		#region Event Handlers

		private void grdItem_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var supplier = (Supplier)e.Row.DataItem;
				var linkEdit = (HyperLink)e.Row.FindControl("linkEdit");
				var linkProduct = (HyperLink)e.Row.FindControl("linkProduct");

				linkEdit.NavigateUrl = "/SupplierItem.aspx?id=" + supplier.SupplierID;
				linkProduct.NavigateUrl = "/ProductList.aspx?supplierid=" + supplier.SupplierID;

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
