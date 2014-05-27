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
	public partial class CategoryList : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			grdItem.RowDataBound += new GridViewRowEventHandler(grdItem_RowDataBound);
			this.PagingControl1.ObjectSingular = "Category";
			this.PagingControl1.ObjectPlural = "Categories";
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
			var paging = new CategoryPaging(
				this.PagingControl1.PageIndex,
				this.PagingControl1.RecordsPerPage,
				Category.FieldNameConstants.CategoryID,
				true);

			CategoryCollection categoryCollection =
				CategoryCollection.RunSelect(x => true, paging);

			this.PagingControl1.ItemCount = paging.RecordCount;
			grdItem.DataSource = categoryCollection;
			grdItem.DataBind();
			SessionHelper.LastCategoryListSearch = this.Request.Url.AbsoluteUri;

			grdItem.DataSource = CategoryCollection.RunSelect();
			grdItem.DataBind();


		}

		#endregion

		#region Event Handlers

		private void grdItem_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var category = (Category)e.Row.DataItem;
				var linkEdit = (HyperLink)e.Row.FindControl("linkEdit");
				var linkProducts = (HyperLink)e.Row.FindControl("linkProducts");

				linkEdit.NavigateUrl = "/CategoryItem.aspx?id=" + category.CategoryId;
				linkProducts.NavigateUrl = "/ProductList.aspx?categoryid=" + category.CategoryId;
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