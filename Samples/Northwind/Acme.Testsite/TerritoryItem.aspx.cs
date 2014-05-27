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
	public partial class TerritoryItem : BasePage
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
				RegionCollection regionCollection = RegionCollection.RunSelect();
				cboRegion.DataTextField = Region.FieldNameConstants.RegionDescription.ToString();
				cboRegion.DataValueField = Region.FieldNameConstants.RegionId.ToString();
				cboRegion.DataSource = (from x in regionCollection orderby x.RegionDescription select x);
				cboRegion.DataBind();
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
			Territory territory = Territory.SelectUsingPK(this.Request["id"] + "");
			if (territory == null)
			{
				this.Response.Redirect(SessionHelper.LastTerritoryListSearch);
				return;
			}

			lblTerritoryId.Text = territory.TerritoryId.ToString();
			txtDescription.Text = territory.TerritoryDescription;
			cboRegion.SelectedValue = territory.RegionId.ToString();
		}

		#endregion

		#region Event Handlers

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Response.Redirect(SessionHelper.LastTerritoryListSearch);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			Territory territory = Territory.SelectUsingPK(this.Request["id"] + "");

			territory.TerritoryDescription = txtDescription.Text;
			territory.RegionId = int.Parse(cboRegion.SelectedValue);
			territory.Persist();

			this.Response.Redirect(SessionHelper.LastTerritoryListSearch);
		}

		#endregion

	}
}
