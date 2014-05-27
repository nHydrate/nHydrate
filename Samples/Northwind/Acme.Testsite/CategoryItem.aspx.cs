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
	public partial class CategoryItem : BasePage
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
			int categoryId;
			int.TryParse(this.Request["id"], out categoryId);
			Category category = Category.SelectUsingPK(categoryId);
			if (category == null)
			{
				this.Response.Redirect(SessionHelper.LastCategoryListSearch);
				return;
			}

			lblCategoryId.Text = category.CategoryId.ToString();
			txtName.Text = category.CategoryName;
			txtDescription.Text = category.Description;

		}

		#endregion

		#region Event Handlers

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.Response.Redirect(SessionHelper.LastCategoryListSearch);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			int categoryId;
			int.TryParse(this.Request["id"], out categoryId);
			Category category = Category.SelectUsingPK(categoryId);

			category.CategoryName = txtName.Text;
			category.Description = txtDescription.Text;
			category.Persist();

			this.Response.Redirect(SessionHelper.LastCategoryListSearch);
		}

		#endregion

	}
}