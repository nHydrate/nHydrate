using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Northwind.TestSite.Objects;

namespace Northwind.TestSite.UserControls
{
	public partial class PagingControl : BaseUserControl
	{
		#region Class Members

		private URL _query = null;

		#endregion

		#region Page Load

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (!this.IsPostBack)
			{
				//Populate the page per combo
				cboPagePer.Items.Add(new ListItem("5" + " " + this.ObjectPlural + " per Page", "5"));
				cboPagePer.Items.Add(new ListItem("10" + " " + this.ObjectPlural + " per Page", "10"));
				cboPagePer.Items.Add(new ListItem("20" + " " + this.ObjectPlural + " per Page", "20"));
				cboPagePer.Items.Add(new ListItem("30" + " " + this.ObjectPlural + " per Page", "30"));
				cboPagePer.Items.Add(new ListItem("40" + " " + this.ObjectPlural + " per Page", "40"));
				cboPagePer.Items.Add(new ListItem("50" + " " + this.ObjectPlural + " per Page", "50"));

				URL url = new URL(this.Request.Url.AbsoluteUri);
				cboPagePer.SelectedValue = url.RecordsPerPage.ToString();
				//cboPagePer.SelectedValue = this.RecordsPerPage.ToString();
				this.rpp.Value = cboPagePer.SelectedValue;
				pnlPagePer.Visible = true;
			}

			this.cboPagePer.SelectedIndexChanged += new EventHandler(cboPagePer_SelectedIndexChanged);

		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			cboPagePer.Attributes.Add("onclick", "javascript:if (document.getElementById('" + this.rpp.ClientID + "').value!=document.getElementById('" + this.cboPagePer.ClientID + "').value) document.getElementById('" + this.rpp.ClientID + "').value=document.getElementById('" + this.cboPagePer.ClientID + "').value;");
			this.PopulateClickBar();
			//this.rpp.ValueChanged += new EventHandler(cboPagePer_SelectedIndexChanged);			
		}

		#endregion

		#region Events

		public event EventHandler PageIndexChanged;
		public event EventHandler RecordsPerPageChanged;		

		protected virtual void OnPageIndexChanged(System.EventArgs e)
		{
			if (this.PageIndexChanged != null)
				this.PageIndexChanged(this, e);
		}

		protected virtual void OnRecordsPerPageChanged(System.EventArgs e)
		{
			if (this.RecordsPerPageChanged != null)
				this.RecordsPerPageChanged(this, e);
		}

		#endregion

		#region Methods

		private void PopulateClickBar()
		{
			
			try
			{
				var query = new URL(this.Request.Url.AbsoluteUri);

				pnlGotoPage.Controls.Clear();
				HtmlAnchor link = null;

				if (this.PageIndex > 1)
				{
					//Add the 'Previous' page link
					link = new HtmlAnchor();
					link.Attributes.Add("rel", "nofollow");
					link.ID = "linkPrevious";
					link.InnerText = "<";
					pnlGotoPage.Controls.Add(link);
					query.PageOffset = this.PageIndex - 1;
					link.HRef = query.ToString();
				}

				Literal literal = new Literal();
				literal.Text = "&nbsp;&nbsp;";
				pnlGotoPage.Controls.Add(literal);

				//Try to position the current PageIndex in the middle of the selection			
				int startIndex = this.PageIndex - 5;
				if (startIndex < 1)
					startIndex = 1;
				int endIndex = startIndex + 9;
				if (endIndex > this.PageCount)
					endIndex = this.PageCount;

				if (startIndex == 1 && endIndex <= 1)
				{
					pnlGotoContainer.Visible = false;
					return;
				}
				pnlGotoContainer.Visible = true;

				for (int ii = startIndex; ii <= endIndex; ii++)
				{
					link = new HtmlAnchor();
					link.Attributes.Add("rel", "nofollow");
					link.ID = "linkPage" + ii.ToString();
					link.InnerText = ii.ToString();
					if (ii == this.PageIndex)
						link.Attributes.Add("class", "paginggotopageselected");
					query.PageOffset = ii;
					link.HRef = query.ToString();
					pnlGotoPage.Controls.Add(link);

					literal = new Literal();
					literal.Text = "&nbsp;&nbsp;";
					pnlGotoPage.Controls.Add(literal);
				}

				if (this.PageIndex < this.PageCount)
				{
					//Add the 'Next' page link
					link = new HtmlAnchor();
					link.Attributes.Add("rel", "nofollow");
					link.InnerText = ">";
					link.ID = "linkNext";
					query.PageOffset = this.PageIndex + 1;
					link.HRef = query.ToString();
					pnlGotoPage.Controls.Add(link);
				}

				//If there are many more records then 
				//add a paging mechanism to this paging control
				int nextIndex = endIndex + 9;
				nextIndex = nextIndex - (nextIndex % 10);
				if (nextIndex + 9 <= this.PageCount)
				{
					literal = new Literal();
					literal.Text = "&nbsp;&nbsp;";
					pnlGotoPage.Controls.Add(literal);

					link = new HtmlAnchor();
					link.Attributes.Add("rel", "nofollow");
					link.ID = "linkPageN1";
					//link.Text = nextIndex.ToString() + "-" + (nextIndex + 9).ToString();
					link.InnerText = nextIndex.ToString() + "+";
					query.PageOffset = nextIndex;
					link.HRef = query.ToString();
					pnlGotoPage.Controls.Add(link);
				}

				//If there are many more records then 
				//add a paging mechanism to this paging control
				if (nextIndex + 19 <= this.PageCount)
				{
					literal = new Literal();
					literal.Text = "&nbsp;&nbsp;";
					pnlGotoPage.Controls.Add(literal);

					link = new HtmlAnchor();
					link.Attributes.Add("rel", "nofollow");
					link.ID = "linkPageN2";
					link.InnerText = (nextIndex + 10).ToString() + "+";
					query.PageOffset = (nextIndex + 10);
					if (this.PageIndex == this.PageCount)
						pnlGotoPage.Controls.Add(link);
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region Event Handlers

		private void GotoPageClick(object sender, EventArgs e)
		{
			URL url = new URL(this.Request.Url.AbsoluteUri);
			int pageOffset = int.Parse(url.Parameters.GetValue("po", "1"));

			LinkButton button = (LinkButton)sender;
			if (button.Text == "<")
			{
				pageOffset--;
			}
			else if (button.Text == ">")
			{
				pageOffset++;
			}
			else if (button.Text.IndexOf("+") != -1)
			{
				//This is a paging mechanism			
				pageOffset = int.Parse(button.CommandArgument);
			}
			else
			{
				//The actual page number
				pageOffset = int.Parse(button.CommandArgument);
			}

			url.Parameters.SetValue("po", pageOffset.ToString());
			this.Response.Redirect(url.ToString());

		}

		protected void cboPagePer_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.OnRecordsPerPageChanged(new EventArgs());
		}

		#endregion

		#region Property Implementations

		public PlaceHolder SortPlaceHolder
		{
			get { return pnlSort; }
		}

		public int StartIndex
		{
			get
			{
				if (this.ViewState["StartIndex"] == null)
					this.ViewState["StartIndex"] = 1;
				int retval = (int)this.ViewState["StartIndex"];
				if (retval < 1)
					retval = 1;
				return retval;
			}
			set { this.ViewState["StartIndex"] = value; }
		}

		private URL QueryObject
		{
			get
			{
				if (_query == null)
					_query = new URL(this.Request.Url.AbsoluteUri);
				return _query;
			}
		}

		public int PageIndex
		{
			get { return this.QueryObject.PageOffset; }
		}

		public int PageCount
		{
			get
			{
				if (this.ViewState["PageCount"] == null)
					this.ViewState["PageCount"] = 1;
				return (int)this.ViewState["PageCount"];
			}
			set
			{
				this.ViewState["PageCount"] = value;
				this.PopulateClickBar();
			}
		}

		public int RecordsPerPage
		{
		  get { return int.Parse(this.rpp.Value); }
		}

		public int ItemCount
		{
			get
			{

				if (this.ViewState["ItemCount"] == null)
					this.ViewState["ItemCount"] = 0;
				return (int)this.ViewState["ItemCount"];
			}
			set
			{
				this.ViewState["ItemCount"] = value;
				if (this.RecordsPerPage == 0)
					this.PageCount = 0;
				else
					this.PageCount = (value / this.RecordsPerPage) + ((value % this.RecordsPerPage == 0 ? 0 : 1));
				this.UpdateDisplay();
			}
		}

		public string ObjectSingular
		{
			get
			{
				if (this.ViewState["ObjectSingular"] == null)
					this.ViewState["ObjectSingular"] = "";
				return (string)this.ViewState["ObjectSingular"];
			}
			set
			{
				this.ViewState["ObjectSingular"] = value;				
			}
		}

		public string ObjectPlural
		{
			get
			{
				if (this.ViewState["ObjectPlural"] == null)
					this.ViewState["ObjectPlural"] = "";
				return (string)this.ViewState["ObjectPlural"];
			}
			set
			{
				this.ViewState["ObjectPlural"] = value;
			}
		}

		public string FoundHeaderText
		{
			get
			{
				if (this.ViewState["FoundHeaderText"] == null)
					this.ViewState["FoundHeaderText"] = "";
				return (string)this.ViewState["FoundHeaderText"];
			}
			set
			{
				this.ViewState["FoundHeaderText"] = value;
			}
		}

		public bool AllowHeader
		{
			get
			{
				if (this.ViewState["AllowHeader"] == null)
					this.ViewState["AllowHeader"] = true;
				return (bool)this.ViewState["AllowHeader"];
			}
			set
			{
				this.ViewState["AllowHeader"] = value;
				pnlHeader.Visible = this.AllowHeader;
			}
		}

		public bool UseH1Tag
		{
			get
			{
				if (this.ViewState["UseH1Tag"] == null)
					this.ViewState["UseH1Tag"] = true;
				return (bool)this.ViewState["UseH1Tag"];
			}
			set { this.ViewState["UseH1Tag"] = value; }
		}

		#endregion

		#region Methods

		private void UpdateDisplay()
		{
			string text = "";

			//Determine if this is an H1 tag or not
			if (this.UseH1Tag) text += "<h1>";
			else text += "<span class=\"pagingheader\">";
			
			if (this.FoundHeaderText == "")
				text += this.ItemCount.ToString() + " " + this.ObjectPlural + " found";
			else
				text += this.FoundHeaderText.Replace("{count}", this.ItemCount.ToString());

			//Determine if this is an H1 tag or not
			if (this.UseH1Tag) text += "</h1>";
			else text += "</span>";

			lblFound.Text = text;
		}

		#endregion

	}
}