#region Copyright (c) 2006-2009 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2009 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;

namespace Widgetsphere.Generator.ProjectItemGenerators.DataService.DataServiceObjectListPages
{
	class DataServiceIndexPageTemplate : BaseDataTransferServiceTemplate
	{
		private StringBuilder sb = new StringBuilder();		

		#region Constructors
		public DataServiceIndexPageTemplate(ModelRoot model)
    {
      _model = model;      
		}
		#endregion 

		#region BaseClassTemplate overrides

		public override string FileContent
		{
			get 
			{
				GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get { return "index.html"; }
		}

		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
      try
      {

				sb.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
				sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
				sb.AppendLine("<head>");
				sb.AppendLine("<title></title>");
				sb.AppendLine();
				sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"styles/themes/redmond/jquery-ui.custom.css\" />");
				sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"styles/themes/redmond/main.css\" />");
				sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"styles/themes/redmond/ui.theme.css\" />");
				sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"styles/themes/redmond/ui.all.css\" />");
				sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"styles/themes/redmond/ui.ribbon.css\" />");
				sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"styles/themes/redmond/ui.layout.css\" />");
				sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"styles/themes/redmond/ui.jqgrid.css\" />");
				sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"styles/themes/redmond/jquery.searchFilter.css\" />");
				sb.AppendLine();
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/jquery.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/jquery.ribbon.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/jquery.layout.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/ui.core.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/ui.draggable.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/effects.core.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/effects.slide.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/effects.drop.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/effects.scale.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/JSON2.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/wwscriptlibrary.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/serviceProxy.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/grid.loader.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/jquery/jqModal.js\"></script>");
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/generated/objects/objectcore.js\"></script>");

				foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
				{
					if (table.Generated)
					{
						sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/generated/grid/" + table.PascalName.ToLower() + "grid.js\"></script>");
						sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/generated/objects/" + table.PascalName.ToLower() + ".js\"></script>");
					}
				}
				sb.AppendLine("<script type=\"text/javascript\" src=\"scripts/generated/pagehelper.js\"></script>");

				sb.AppendLine("</head>");
				sb.AppendLine("<body>");
				sb.AppendLine();
				sb.AppendLine("<div id=\"content\" class=\"ui-layout-center\">");
				sb.AppendLine("</div>");
				sb.AppendLine("<div class=\"ui-layout-south\">");
				sb.AppendLine("</div>");
				sb.AppendLine("<div class=\"ui-layout-west\">");
				sb.AppendLine("</div>");
				sb.AppendLine("<div class=\"ui-layout-north\">");
				sb.AppendLine("		<div class=\"mainContainer\">");
				sb.AppendLine("			<ul class=\"ribbon\">");
				sb.AppendLine("				<li>");
				sb.AppendLine("					<ul class=\"orb\">");
				sb.AppendLine("						<li><a href=\"javascript:void(0);\" accesskey=\"1\" class=\"orbButton\">&nbsp;</a><span>Menu</span>");
				sb.AppendLine("							<ul>");

				foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
				{
					if (table.Generated)
					{
						sb.AppendLine("<li><a id=\"" + table.PascalName + "List\" href=\"#\"><img src=\"styles/themes/redmond/images/icon_doc.png\" alt=\"" + table.PascalName + " List\" /><span>" + table.PascalName + " List</span></a> </li>");
					}
				}
				sb.AppendLine("							</ul>");
				sb.AppendLine("						</li>");
				sb.AppendLine("					</ul>");
				sb.AppendLine("				</li>");
				sb.AppendLine("				<li>");
				sb.AppendLine("					<ul class=\"menu\">");
				sb.AppendLine("						<li><a href=\"#home\" accesskey=\"2\">Home</a>");
				sb.AppendLine("							<ul>");
				sb.AppendLine("								<li>");
				sb.AppendLine("									<h2>");
				sb.AppendLine("										<span>Test</span></h2>");
				sb.AppendLine("									<div id=\"Customers\">");
				sb.AppendLine("										<img src=\"styles/core/images/Application32.png\" alt=\"Categories\" />");
				sb.AppendLine("										Customers");
				sb.AppendLine("									</div>");
				sb.AppendLine("								</li>");
				sb.AppendLine("							</ul>");
				sb.AppendLine("						</li>");
				sb.AppendLine("					</ul>");
				sb.AppendLine("				</li>");
				sb.AppendLine("			</ul>");
				sb.AppendLine("		</div>");
				sb.AppendLine("	</div>");
						
			sb.AppendLine();
				
				sb.AppendLine("</body>");
				sb.AppendLine("</html>");

      }
      catch(Exception ex)
      {
        throw;
      }
		}

		#endregion
		
	}
}
