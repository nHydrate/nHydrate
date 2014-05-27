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
using Widgetsphere.Generator.Common.Util;

namespace Widgetsphere.Generator.ProjectItemGenerators.DataService.DataServiceJScripts
{
	class DataServiceJScriptsTemplate : BaseDataTransferServiceTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _currentTable = null;

		#region Constructors
		public DataServiceJScriptsTemplate(ModelRoot model, Table table)
		{
			_model = model;
			_currentTable = table;
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
			get { return _currentTable.PascalName.ToLower() + ".js"; }
		}

		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				this.AppendClass();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region namespace / objects

		public void AppendClass()
		{
			sb.AppendLine("function " + _currentTable.PascalName + "(jsonObject) {");
			sb.AppendLine("	this.internalObject = jsonObject;");
			sb.AppendLine("}");
			sb.AppendLine();
			sb.AppendLine("" + _currentTable.PascalName + ".prototype = {");

			foreach (Column column in _currentTable.GetColumns())
			{
				//Make a Getter
				sb.AppendLine("	get" + column.PascalName + ": function() { return this.internalObject." + column.PascalName + "; },");
				//Setter for only non-primary keys
				if (!_currentTable.PrimaryKeyColumns.Contains(column))
				{
					sb.AppendLine("	set" + column.PascalName + ": function(" + column.CamelName + ") { this.internalObject." + column.PascalName + " = " + column.CamelName + "; },");
				}
			}

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("	get" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ": function() { return this.internalObject." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "; },");
				sb.AppendLine("	get" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ": function() { return this.internalObject." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "; },");
			}

			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("	get" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ": function() { return this.internalObject." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "; },");
				sb.AppendLine("	get" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ": function() { return this.internalObject." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "; },");
			}

			sb.AppendLine("	get" + _currentTable.PascalName + "Object: function() { return this.internalObject; }");
			sb.AppendLine("};");
			sb.AppendLine();
			sb.AppendLine("function " + _currentTable.PascalName + "Service() {");
			sb.AppendLine("}");
			sb.AppendLine();
			sb.AppendLine("" + _currentTable.PascalName + "Service.prototype.retrieveAll = function(successCallback) {");
			sb.AppendLine("    viewServiceProxy.invokeGet(\"" + _currentTable.PascalName + "List\", successCallback, OnPageError, true);");
			sb.AppendLine("};");
			sb.AppendLine();
			sb.AppendLine("" + _currentTable.PascalName + "Service.prototype.retrievePaged = function(successCallback, pageIndex, pageSize, sortOrder, sortColumn, linq) {");
			sb.AppendLine("    var restUrl = \"" + _currentTable.PascalName + "ListPage\";");
			sb.AppendLine("    restUrl = restUrl + \"/?\";");
			sb.AppendLine();
			sb.AppendLine("    if (!IsNullOrEmpty(pageIndex)) {");
			sb.AppendLine("        restUrl = restUrl + \"pageIndex=\" + pageIndex;");
			sb.AppendLine("        if (!IsNullOrEmpty(pageSize) || !IsNullOrEmpty(sortOrder) || !IsNullOrEmpty(sortColumn) || !IsNullOrEmpty(linq)) {");
			sb.AppendLine("            restUrl = restUrl + \"&\";");
			sb.AppendLine("        }");
			sb.AppendLine("    }");
			sb.AppendLine("    if (!IsNullOrEmpty(pageSize)) {");
			sb.AppendLine("        restUrl = restUrl + \"pageSize=\" + pageSize;");
			sb.AppendLine("        if (!IsNullOrEmpty(sortOrder) || !IsNullOrEmpty(sortColumn) || !IsNullOrEmpty(linq)) {");
			sb.AppendLine("            restUrl = restUrl + \"&\";");
			sb.AppendLine("        }");
			sb.AppendLine("    }");
			sb.AppendLine("    if (!IsNullOrEmpty(sortOrder)) {");
			sb.AppendLine("        restUrl = restUrl + \"sortOrder=\" + sortOrder;");
			sb.AppendLine("        if (!IsNullOrEmpty(sortColumn) || !IsNullOrEmpty(linq)) {");
			sb.AppendLine("            restUrl = restUrl + \"&\";");
			sb.AppendLine("        }");
			sb.AppendLine("    }");
			sb.AppendLine("    if (!IsNullOrEmpty(sortColumn)) {");
			sb.AppendLine("        restUrl = restUrl + \"sortColumn=\" + sortColumn;");
			sb.AppendLine("        if (!IsNullOrEmpty(linq)) {");
			sb.AppendLine("            restUrl = restUrl + \"&\";");
			sb.AppendLine("        }");
			sb.AppendLine("    }");
			sb.AppendLine("    restUrl = restUrl + \"linq=\" + linq;");
			sb.AppendLine();
			sb.AppendLine("    viewServiceProxy.invokeGet(restUrl, successCallback, OnPageError, true);");
			sb.AppendLine("};");
			sb.AppendLine();
			sb.AppendLine("" + _currentTable.PascalName + "Service.prototype.save = function(" + _currentTable.CamelName + ", successCallback) {");
			sb.AppendLine("viewServiceProxy.invokePut(\"" + _currentTable.PascalName + "\", " + _currentTable.CamelName + ".get" + _currentTable.PascalName + "Object(), successCallback, OnPageError, true);");
			sb.AppendLine("};");
		}

		#endregion

	}
}