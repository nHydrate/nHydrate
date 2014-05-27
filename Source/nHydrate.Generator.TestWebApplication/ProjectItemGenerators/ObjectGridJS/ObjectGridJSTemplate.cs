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
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;
using Widgetsphere.Generator.Common.Util;

namespace Widgetsphere.Generator.TestWebApplication.ProjectItemGenerators.ObjectListPages
{
	class ObjectGridJSTemplate : ObjectListPagesTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _currentTable;

		#region Constructors
		public ObjectGridJSTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
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
			get
			{
				return string.Format("{0}Grid.js", _currentTable.PascalName);
			}
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				sb.AppendLine("function " + _currentTable.PascalName + "Grid() {");
				sb.AppendLine("}");
				sb.AppendLine();
				sb.AppendLine("" + _currentTable.PascalName + "Grid.prototype.performRetrieve = function(configuration, otherstuff) {");
				sb.AppendLine("		var appService = new " + _currentTable.PascalName + "Service();");
				sb.AppendLine("		appService.retrievePaged(" + _currentTable.PascalName + "Grid.prototype.fillGrid, configuration.page, configuration.rows, configuration.sord, configuration.sidx, \"x => true\");");
				sb.AppendLine("};");
				sb.AppendLine();
				sb.AppendLine("" + _currentTable.PascalName + "Grid.prototype.fillGrid = function(jsonData) {");
				sb.AppendLine("		var thegrid = jQuery(\"#grid\")[0];");
				sb.AppendLine("		thegrid.addJSONData(jsonData);");
				sb.AppendLine("};");
				sb.AppendLine();
				sb.AppendLine("" + _currentTable.PascalName + "Grid.prototype.gridConfiguration = {");
				sb.AppendLine("		datatype: " + _currentTable.PascalName + "Grid.prototype.performRetrieve,");

				sb.Append("		colNames: [");
				ColumnCollection columnList = _currentTable.GetColumns();
				foreach (Column column in columnList)
				{
					sb.Append("'" + column.PascalName + "'");
					if (columnList.IndexOf(column) < columnList.Count - 1)
						sb.Append(",");
				}
				sb.AppendLine("],");

				sb.AppendLine("		colModel: [");
				foreach (Column column in columnList)
				{
					sb.Append("				{ name: '" + column.PascalName + "', index: '" + column.PascalName + "', width: 200, align: 'left' }");
					if (columnList.IndexOf(column) < columnList.Count - 1)
						sb.Append(",");
					sb.AppendLine();
				}
				sb.AppendLine("],");
				sb.AppendLine("    rowNum: 10,");
				sb.AppendLine("    rowList: [5, 10, 20, 50, 100],");
				if (_currentTable.AllowCreateAudit)
					sb.AppendLine("    sortname: '" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "',");
				else
					sb.AppendLine("    sortname: '" + _currentTable.PrimaryKeyColumns[0].PascalName + "',");

				sb.AppendLine("    pager: '#pager',");
				sb.AppendLine("    sortorder: \"asc\",");
				sb.AppendLine("    viewrecords: true,");
				sb.AppendLine("    autowidth: true,");
				sb.AppendLine("    height: '100%',");
				sb.AppendLine("    jsonReader: { root: \"GridData\", page: \"CurrentPage\", total: \"TotalPages\", records: \"TotalRecords\", repeatitems: false, id: \"0\" }");
				sb.AppendLine("};");
				sb.AppendLine();
				sb.AppendLine("" + _currentTable.PascalName + "Grid.prototype.SetupGrid = function(selector) {");
				sb.AppendLine("    jQuery(selector).html('<table id=\"grid\"></table><div id=\"pager\"></div>');");
				sb.AppendLine("    jQuery(\"#grid\").jqGrid(" + _currentTable.PascalName + "Grid.prototype.gridConfiguration).navGrid('#pager');");
				sb.AppendLine("};");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

	}
}