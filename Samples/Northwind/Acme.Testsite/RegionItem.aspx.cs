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
	public partial class RegionItem : BasePage
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
			int regionId;
			int.TryParse(this.Request["id"], out regionId);
			Region region = Region.SelectUsingPK(regionId);
			if (region != null)
			{
				lblRegionId.Text = region.RegionId.ToString();
				txtDescription.Text = region.RegionDescription;
			}
			else
			{
				lblRegionId.Text = "New Item";
			}

		}

		#endregion

		#region Event Handlers

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Response.Redirect(SessionHelper.LastRegionListSearch);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			int regionId;
			int.TryParse(this.Request["id"], out regionId);
			Region region = Region.SelectUsingPK(regionId);
			if (region == null)
			{
				//For some reason Microsoft did not make this an autonumber
				int? maxRegionId = RegionCollection.GetMax(x => x.RegionId);
				RegionCollection regionCollection = new RegionCollection();
				region = regionCollection.NewItem(maxRegionId.Value + 1);
				region.RegionDescription = txtDescription.Text;
				regionCollection.AddItem(region);
				regionCollection.Persist();
			}
			else
			{
				region.RegionDescription = txtDescription.Text;
				region.Persist();
			}

			this.Response.Redirect(SessionHelper.LastRegionListSearch);
		}

		#endregion

	}
}