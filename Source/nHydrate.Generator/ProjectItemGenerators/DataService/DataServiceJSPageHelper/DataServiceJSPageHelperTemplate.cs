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
	class DataServiceJSPageHelperTemplate : BaseDataTransferServiceTemplate
	{
		private StringBuilder sb = new StringBuilder();		

		#region Constructors
		public DataServiceJSPageHelperTemplate(ModelRoot model)
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
			get { return "pagehelper.js"; }
		}

		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
      try
      {
				sb.AppendLine("/// <reference path=\"jquery/jquery.js\" />");

				foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
				{
					if (table.Generated)
					{
						sb.AppendLine("/// <reference path=\"scripts/generated/objects/" + table.PascalName.ToLower() + ".js\" />");
					}
				}

				sb.AppendLine("var outerLayout, innerLayout;");
				sb.AppendLine();
				sb.AppendLine("$(document).ready(SetLayout);");
				sb.AppendLine("$(document).ready(SetActions);");
				sb.AppendLine();
				sb.AppendLine("function SetLayout() {");
				sb.AppendLine();
				sb.AppendLine("	// create the OUTER LAYOUT");
				sb.AppendLine("	outerLayout = $(\"body\").layout(layoutSettings_Outer);");
				sb.AppendLine();
				sb.AppendLine("	// BIND events to hard-coded buttons in the NORTH toolbar");
				sb.AppendLine("	//		outerLayout.addToggleBtn( \"#tbarToggleNorth\", \"north\" );");
				sb.AppendLine("	//		outerLayout.addOpenBtn( \"#tbarOpenSouth\", \"south\" );");
				sb.AppendLine("	//		outerLayout.addCloseBtn( \"#tbarCloseSouth\", \"south\" );");
				sb.AppendLine("	//		outerLayout.addPinBtn( \"#tbarPinWest\", \"west\" );");
				sb.AppendLine("	// ");
				sb.AppendLine("	// save selector strings to vars so we don't have to repeat it");
				sb.AppendLine("	// must prefix paneClass with \"body > \" to target ONLY the outerLayout panes");
				sb.AppendLine("	var westSelector = \"body > .ui-layout-west\"; // outer-west pane");
				sb.AppendLine();
				sb.AppendLine("	// CREATE SPANs for pin-buttons - using a generic class as identifiers");
				sb.AppendLine("	$(\"<span></span>\").addClass(\"pin-button\").prependTo(westSelector);");
				sb.AppendLine("	// BIND events to pin-buttons to make them functional");
				sb.AppendLine("	outerLayout.addPinBtn(westSelector + \" .pin-button\", \"west\");");
				sb.AppendLine();
				sb.AppendLine("	// CREATE SPANs for close-buttons - using unique IDs as identifiers");
				sb.AppendLine("	$(\"<span></span>\").attr(\"id\", \"west-closer\").prependTo(westSelector);");
				sb.AppendLine("	// BIND layout events to close-buttons to make them functional");
				sb.AppendLine("	outerLayout.addCloseBtn(\"#west-closer\", \"west\");");
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendLine("	/* Create the INNER LAYOUT - nested inside the 'center pane' of the outer layout");
				sb.AppendLine("	* Inner Layout is create by createInnerLayout() function - on demand");
				sb.AppendLine("	*");
				sb.AppendLine("	innerLayout = $(\"div.pane-center\").layout( layoutSettings_Inner );");
				sb.AppendLine("	*");
				sb.AppendLine("	*/");
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendLine("	// DEMO HELPER: prevent hyperlinks from reloading page when a 'base.href' is set");
				sb.AppendLine("	$(\"a\").each(function() {");
				sb.AppendLine("		var path = document.location.href;");
				sb.AppendLine("		if (path.substr(path.length - 1) == \"#\") path = path.substr(0, path.length - 1);");
				sb.AppendLine("		if (this.href.substr(this.href.length - 1) == \"#\") this.href = path + \"#\";");
				sb.AppendLine("	});");
				sb.AppendLine();
				sb.AppendLine("}");
				sb.AppendLine();
				sb.AppendLine("function SetActions() {");
				sb.AppendLine("	$().Ribbon({ theme: 'base' });");
				sb.AppendLine();
				sb.AppendLine("	//INSERT TABLE HANDLERS HERE");

				foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
				{
					if (table.Generated)
					{
						sb.AppendLine("	$('#" + table.PascalName + "List').click(Setup" + table.PascalName + "Grid);");
					}
				}

				sb.AppendLine("}");
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendLine("/*");
				sb.AppendLine("*#######################");
				sb.AppendLine("* OUTER LAYOUT SETTINGS");
				sb.AppendLine("*#######################");
				sb.AppendLine("*");
				sb.AppendLine("* This configuration illustrates how extensively the layout can be customized");
				sb.AppendLine("* ALL SETTINGS ARE OPTIONAL - and there are more available than shown below");
				sb.AppendLine("*");
				sb.AppendLine("* These settings are set in 'sub-key format' - ALL data must be in a nested data-structures");
				sb.AppendLine("* All default settings (applied to all panes) go inside the defaults:{} key");
				sb.AppendLine("* Pane-specific settings go inside their keys: north:{}, south:{}, center:{}, etc");
				sb.AppendLine("*/");
				sb.AppendLine("var layoutSettings_Outer = {");
				sb.AppendLine("	name: \"outerLayout\" // NO FUNCTIONAL USE, but could be used by custom code to 'identify' a layout");
				sb.AppendLine("	// options.defaults apply to ALL PANES - but overridden by pane-specific settings");
				sb.AppendLine("	, defaults: {");
				sb.AppendLine("		size: \"auto\"");
				sb.AppendLine("		, minSize: 50");
				sb.AppendLine("		, paneClass: \"pane\" 		// default = 'ui-layout-pane'");
				sb.AppendLine("		, resizerClass: \"resizer\"	// default = 'ui-layout-resizer'");
				sb.AppendLine("		, togglerClass: \"toggler\"	// default = 'ui-layout-toggler'");
				sb.AppendLine("		, buttonClass: \"button\"	// default = 'ui-layout-button'");
				sb.AppendLine("		, contentSelector: \".content\"	// inner div to auto-size so only it scrolls, not the entire pane!");
				sb.AppendLine("		, contentIgnoreSelector: \"span\"		// 'paneSelector' for content to 'ignore' when measuring room for content");
				sb.AppendLine("		, togglerLength_open: 35			// WIDTH of toggler on north/south edges - HEIGHT on east/west edges");
				sb.AppendLine("		, togglerLength_closed: 35			// \"100%\" OR -1 = full height");
				sb.AppendLine("		, hideTogglerOnSlide: true		// hide the toggler when pane is 'slid open'");
				sb.AppendLine("		, togglerTip_open: \"Close This Pane\"");
				sb.AppendLine("		, togglerTip_closed: \"Open This Pane\"");
				sb.AppendLine("		, resizerTip: \"Resize This Pane\"");
				sb.AppendLine("		//	effect defaults - overridden on some panes");
				sb.AppendLine("		, fxName: \"slide\"		// none, slide, drop, scale");
				sb.AppendLine("		, fxSpeed_open: 750");
				sb.AppendLine("		, fxSpeed_close: 1500");
				sb.AppendLine("		, fxSettings_open: { easing: \"easeInQuint\" }");
				sb.AppendLine("		, fxSettings_close: { easing: \"easeOutQuint\" }");
				sb.AppendLine("	}");
				sb.AppendLine("	, north: {");
				sb.AppendLine("		spacing_open: 0			// cosmetic spacing");
				sb.AppendLine("		, size: 122");
				sb.AppendLine("		, togglerLength_open: 0			// HIDE the toggler button");
				sb.AppendLine("		, togglerLength_closed: -1			// \"100%\" OR -1 = full width of pane");
				sb.AppendLine("		, resizable: false");
				sb.AppendLine("		, slidable: false");
				sb.AppendLine("		//	override default effect");
				sb.AppendLine("		, fxName: \"none\"");
				sb.AppendLine("	}");
				sb.AppendLine("	, south: {");
				sb.AppendLine("		maxSize: 200");
				sb.AppendLine("		, spacing_closed: 0			// HIDE resizer & toggler when 'closed'");
				sb.AppendLine("		, slidable: false		// REFERENCE - cannot slide if spacing_closed = 0");
				sb.AppendLine("		, initClosed: true");
				sb.AppendLine("		//	CALLBACK TESTING...");
				sb.AppendLine("		, onhide_start: function() { return confirm(\"START South pane hide \\n\\n onhide_start callback \\n\\n Allow pane to hide?\"); }");
				sb.AppendLine("		, onhide_end: function() { alert(\"END South pane hide \\n\\n onhide_end callback\"); }");
				sb.AppendLine("		, onshow_start: function() { return confirm(\"START South pane show \\n\\n onshow_start callback \\n\\n Allow pane to show?\"); }");
				sb.AppendLine("		, onshow_end: function() { alert(\"END South pane show \\n\\n onshow_end callback\"); }");
				sb.AppendLine("		, onopen_start: function() { return confirm(\"START South pane open \\n\\n onopen_start callback \\n\\n Allow pane to open?\"); }");
				sb.AppendLine("		, onopen_end: function() { alert(\"END South pane open \\n\\n onopen_end callback\"); }");
				sb.AppendLine("		, onclose_start: function() { return confirm(\"START South pane close \\n\\n onclose_start callback \\n\\n Allow pane to close?\"); }");
				sb.AppendLine("		, onclose_end: function() { alert(\"END South pane close \\n\\n onclose_end callback\"); }");
				sb.AppendLine("		//,	onresize_start:			function () { return confirm(\"START South pane resize \\n\\n onresize_start callback \\n\\n Allow pane to be resized?)\"); }");
				sb.AppendLine("		, onresize_end: function() { alert(\"END South pane resize \\n\\n onresize_end callback \\n\\n NOTE: onresize_start event was skipped.\"); }");
				sb.AppendLine("	}");
				sb.AppendLine("	, west: {");
				sb.AppendLine("		size: 250");
				sb.AppendLine("		, spacing_closed: 21			// wider space when closed");
				sb.AppendLine("		, togglerLength_closed: 21			// make toggler 'square' - 21x21");
				sb.AppendLine("		, togglerAlign_closed: \"top\"		// align to top of resizer");
				sb.AppendLine("		, togglerLength_open: 0			// NONE - using custom togglers INSIDE west-pane");
				sb.AppendLine("		, togglerTip_open: \"Close West Pane\"");
				sb.AppendLine("		, togglerTip_closed: \"Open West Pane\"");
				sb.AppendLine("		, resizerTip_open: \"Resize West Pane\"");
				sb.AppendLine("		, slideTrigger_open: \"click\" 	// default");
				sb.AppendLine("		, initClosed: true");
				sb.AppendLine("		, fxName: \"drop\"");
				sb.AppendLine("		, fxSpeed: \"normal\"");
				sb.AppendLine("		, fxSettings: { easing: \"\"} // nullify default easing");
				sb.AppendLine("	}");
				sb.AppendLine("	, east: {");
				sb.AppendLine("		size: 250");
				sb.AppendLine("		, spacing_closed: 21			// wider space when closed");
				sb.AppendLine("		, togglerLength_closed: 21			// make toggler 'square' - 21x21");
				sb.AppendLine("		, togglerAlign_closed: \"top\"		// align to top of resizer");
				sb.AppendLine("		, togglerLength_open: 0 			// NONE - using custom togglers INSIDE east-pane");
				sb.AppendLine("		, togglerTip_open: \"Close East Pane\"");
				sb.AppendLine("		, togglerTip_closed: \"Open East Pane\"");
				sb.AppendLine("		, resizerTip_open: \"Resize East Pane\"");
				sb.AppendLine("		, slideTrigger_open: \"mouseover\"");
				sb.AppendLine("		, initClosed: true");
				sb.AppendLine("		, fxName: \"drop\"");
				sb.AppendLine("		, fxSpeed: \"normal\"");
				sb.AppendLine("		, fxSettings: { easing: \"\"} // nullify default easing");
				sb.AppendLine("	}");
				sb.AppendLine("	, center: {");
				sb.AppendLine("		onresize: ResizeMe	// resize INNER LAYOUT when center pane resizes");
				sb.AppendLine("		, minWidth: 200");
				sb.AppendLine("		, minHeight: 200");
				sb.AppendLine("	}");
				sb.AppendLine("};");
				sb.AppendLine();
				sb.AppendLine("function ResizeMe(testobj, centerLayout) {");
				sb.AppendLine("	try {");
				sb.AppendLine("		var thegrid = jQuery(\"#grid\");");
				sb.AppendLine("		thegrid.setGridWidth(centerLayout[0].clientWidth - 30);");
				sb.AppendLine("		thegrid.setGridHeight(outerLayout.panes.center.innerHeight() - 105);");
				sb.AppendLine();
				sb.AppendLine("	}");
				sb.AppendLine("	catch (ex) {");
				sb.AppendLine("		alert(ex);");
				sb.AppendLine("	}");
				sb.AppendLine("}");
				sb.AppendLine();

				foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
				{
					if (table.Generated)
					{
						sb.AppendLine("/// <reference path=\"scripts/generated/objects/" + table.PascalName.ToLower() + ".js\" />");
						sb.AppendLine("function Setup" + table.PascalName + "Grid() {");
						sb.AppendLine("	var grid = new " + table.PascalName + "Grid();");
						sb.AppendLine("	grid.SetupGrid('#content');");
						sb.AppendLine("}");
						sb.AppendLine();
					}
				}			
				
      }
      catch(Exception ex)
      {
        throw;
      }
		}

		#endregion
		
	}
}
