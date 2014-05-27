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
	public partial class TerritoryList : BasePage
	{
		#region Page Events

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			grdItem.RowDataBound += new GridViewRowEventHandler(grdItem_RowDataBound);
			this.PagingControl1.ObjectSingular = "Territory";
			this.PagingControl1.ObjectPlural = "Territories";
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
				IEnumerable<Territory> list = null;

				if (this.Request["regionid"] != null)
				{
					int regionId;
					int.TryParse(this.Request["regionid"], out regionId);
					list = context.Territory
					.GetPagedResults(x => x.RegionID == regionId,
					x => x.TerritoryID, 
					paging); 
					
					var region = Region.SelectByPK(regionId);
					lblHeader.Text = "This is a list of all territories for the <a href='/RegionItem.aspx?id=" + region.RegionID + "'>" + region.RegionDescription + "</a> region.";
				}
				else
				{
					list = context.Territory
					.GetPagedResults(x => x.TerritoryID,
					paging);
					lblHeader.Text = "This is a list of all territories.";
				}

				this.PagingControl1.ItemCount = paging.RecordCount;
				grdItem.DataSource = list;
				grdItem.DataBind();
				SessionHelper.LastTerritoryListSearch = this.Request.Url.AbsoluteUri;
			}
		}

		#endregion

		#region Event Handlers

		private void grdItem_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				Territory territory = (Territory)e.Row.DataItem;
				var linkEdit = (HyperLink)e.Row.FindControl("linkEdit");
				HyperLink lblRegion = (HyperLink)e.Row.FindControl("lblRegion");

				linkEdit.NavigateUrl = "/TerritoryItem.aspx?id=" + territory.TerritoryId;
				lblRegion.Text = territory.RegionItem.RegionDescription;
				lblRegion.NavigateUrl = "/RegionItem.aspx?id=" + territory.RegionId;

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
