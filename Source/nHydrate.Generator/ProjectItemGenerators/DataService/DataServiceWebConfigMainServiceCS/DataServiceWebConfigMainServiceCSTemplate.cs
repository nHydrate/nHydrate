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
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.DataService.DataServiceWebConfigMainServiceCS
{
	class DataServiceWebConfigMainServiceCSTemplate : BaseDataTransferServiceTemplate
	{
		private StringBuilder sb = new StringBuilder();

		#region Constructors

		public DataServiceWebConfigMainServiceCSTemplate(ModelRoot model)
		{
			_model = model;
		}

		#endregion

		#region BaseClassTemplate overrides

		public override string FileContent
		{
			get
			{
				try
				{
					GenerateContent();
					return sb.ToString();
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}

		public override string FileName
		{
			get { return "Rest.svc.cs"; }
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
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Web;");
			sb.AppendLine("using System.Web.Services;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using System.ServiceModel.Web;");
			sb.AppendLine("using Microsoft.ServiceModel.Web;");
			sb.AppendLine("using System.ServiceModel.Activation;");
			sb.AppendLine("using System.Net;");
			sb.AppendLine("using Microsoft.ServiceModel.Web.SpecializedServices;");
			sb.AppendLine("using " + _model.CompanyName + "." + _model.ProjectName + ".Business;");
			sb.AppendLine("using System.Configuration;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using System.Data;");
			sb.AppendLine("using " + _model.CompanyName + "." + _model.ProjectName + ".Business.Objects;");
			sb.AppendLine("using " + _model.CompanyName + "." + _model.ProjectName + ".DataTransfer;");
			sb.AppendLine("using " + _model.CompanyName + "." + _model.ProjectName + ".Business.SelectCommands;");
			sb.AppendLine("using " + _model.CompanyName + "." + _model.ProjectName + ".DALProxy.Extensions;");
			sb.AppendLine("using System.Text;");
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using System.ServiceModel;");
			sb.AppendLine("using Widgetsphere.Core.Util;");
			sb.AppendLine();
			sb.AppendLine("[assembly: ContractNamespace(\"\", ClrNamespace = \"" + DefaultNamespace + "\")]");
			sb.AppendLine("namespace " + DefaultNamespace);
			sb.AppendLine("{");
			sb.AppendLine("	[ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]");
			sb.AppendLine("	[ServiceContract]");
			sb.AppendLine("	public partial class Rest");
			sb.AppendLine("	{");
			sb.AppendLine("		public Rest() {}");
			sb.AppendLine();

			foreach (Table table in (from x in _model.Database.GetGeneratedTables() orderby x.Name select x))
			{
				SelectAll(table);

				SelectByPrimaryKey(table);

				SelectPaged(table);

				SelectByColumn(table);

				SelectByForeignKey(table);

				AddObject(table);

				DeleteObject(table);

			}
			sb.AppendLine();
			sb.AppendLine("	}");
			sb.AppendLine("}");
		}

		private void DeleteObject(Table table)
		{
			if (table.Immutable)
				return;

			sb.AppendLine("		#region " + table.PascalName + " Delete");
			sb.AppendLine();
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "?orderdetailid=somevalue");
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebInvoke(UriTemplate = \"" + table.PascalName + "(" + PrimaryKeyColumnList(table, false, true) + ")/\", Method = \"DELETE\",  ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		bool Delete" + table.PascalName + "(" + PrimaryKeyPublicParameterList(table) + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			" + table.PascalName + "DTO " + table.CamelName + " = new " + table.PascalName + "DTO();");
			sb.AppendLine("			" + table.CamelName + ".SelectUsingPK(" + PrimaryKeyColumnTypeConversionList(table) + ");");
			sb.AppendLine("			return " + table.CamelName + ".Delete();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "?orderdetailid=somevalue");
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebInvoke(UriTemplate = \"" + table.PascalName + "(" + PrimaryKeyColumnList(table, false, true) + ")/?format=json\", Method = \"DELETE\", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		bool Delete" + table.PascalName + "Json(" + PrimaryKeyPublicParameterList(table) + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.Delete" + table.PascalName + "(" + PrimaryKeyColumnList(table, false) + ");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "?orderdetailid=somevalue");
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebInvoke(UriTemplate = \"" + table.PascalName + "(" + PrimaryKeyColumnList(table, false, true) + ")/?format=xml\", Method = \"DELETE\", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		bool Delete" + table.PascalName + "XML(" + PrimaryKeyPublicParameterList(table) + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.Delete" + table.PascalName + "(" + PrimaryKeyColumnList(table, false) + ");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void SelectPaged(Table table)
		{
			sb.AppendLine("		#region " + table.PascalName + " select paged");
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "ListPage/?pageIndex={pageIndex}&pageSize={pageSize}&sortOrder={sortOrder}&sortColumn={sortColumn}&linq={linq}");
			sb.AppendLine("		[WebHelp(Comment = \"Returns a page of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "ListPage/?pageIndex={pageIndex}&pageSize={pageSize}&sortOrder={sortOrder}&sortColumn={sortColumn}&linq={linq}\", ResponseFormat = WebMessageFormat.Json)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		PagedQueryResults<" + table.PascalName + "DTO> " + table.PascalName + "ListPaged(string pageIndex, string pageSize, string sortOrder, string sortColumn, string linq)");
			sb.AppendLine("		{");
			sb.AppendLine("			List<" + table.PascalName + "DTO> dtoList = new List<" + table.PascalName + "DTO>();");
			sb.AppendLine("			return dtoList.RunSelect(pageSize, pageIndex, sortOrder, sortColumn, linq);");
			sb.AppendLine("		}");
			sb.AppendLine("		");
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "ListPage/?pageIndex={pageIndex}&pageSize={pageSize}&sortOrder={sortOrder}&sortColumn={sortColumn}&linq={linq}&format=json");
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "ListPage/json/?pageIndex={pageIndex}&pageSize={pageSize}&sortOrder={sortOrder}&sortColumn={sortColumn}&linq={linq}\", ResponseFormat = WebMessageFormat.Json)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		PagedQueryResults<" + table.PascalName + "DTO> " + table.PascalName + "ListPagedJson(string pageIndex, string pageSize, string sortOrder, string sortColumn, string linq)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this." + table.PascalName + "ListPaged(pageIndex, pageSize, sortOrder, sortColumn, linq);");
			sb.AppendLine("		}");
			sb.AppendLine("		");
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "ListPage/?pageIndex={pageIndex}&pageSize={pageSize}&sortOrder={sortOrder}&sortColumn={sortColumn}&linq={linq}&format=xml");
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "ListPage/xml/?pageIndex={pageIndex}&pageSize={pageSize}&sortOrder={sortOrder}&sortColumn={sortColumn}&linq={linq}\", ResponseFormat = WebMessageFormat.Xml)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		PagedQueryResults<" + table.PascalName + "DTO> " + table.PascalName + "ListPagedXML(string pageIndex, string pageSize, string sortOrder, string sortColumn, string linq)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this." + table.PascalName + "ListPaged(pageIndex, pageSize, sortOrder, sortColumn, linq);");
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AddObject(Table table)
		{
			#region Add Object
			if (!table.Immutable)
			{
				sb.AppendLine("		#region " + table.PascalName + " Add Item");
				sb.AppendLine();
				sb.AppendLine("		[WebHelp(Comment = \"Submits a new or updated " + table.PascalName + " to be persisted\")]");
				sb.AppendLine("		[WebInvoke(UriTemplate = \"" + table.PascalName + "\", Method = \"PUT\", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]");
				sb.AppendLine("		[OperationContract]");
				sb.AppendLine("		" + table.PascalName + "DTO Persist" + table.PascalName + "(" + table.PascalName + "DTO " + table.CamelName + ")");
				sb.AppendLine("		{");
				sb.AppendLine("			return " + table.CamelName + ".Persist();");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
			}
			#endregion
		}

		private void SelectByForeignKey(Table table)
		{
			#region Select By Foreign Key
			sb.AppendLine("		#region " + table.PascalName + " Select By Foreign Key");
			sb.AppendLine();
			List<Relation> allRelations = table.GetRelationsFullHierarchy();
			List<string> methodList = new List<string>();
			foreach (Relation relation in allRelations.Where(x => x.IsGenerated))
			{
				Table parentTable = (Table)relation.ParentTableRef.Object;
				Table childTable = (Table)relation.ChildTableRef.Object;

				//Handle relations by inheritance
				if (table.IsInheritedFrom(childTable))
					childTable = table;

				if (childTable.IsInheritedFrom(parentTable))
				{
					//Do Nothing
				}
				else if (childTable == table)
				{
					string methodName = "" + childTable.PascalName + "By" + relation.PascalRoleName + parentTable.PascalName;
					if (!methodList.Contains(methodName))
					{
						methodList.Add(methodName);

						sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + methodName + "?" + PrimaryKeyUrlFacade(parentTable));
						sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + childTable.PascalName + " objects for the parent " + parentTable.PascalName + " object.\")]");
						sb.AppendLine("		[WebGet(UriTemplate = \"" + parentTable.PascalName + "(" + PrimaryKeyColumnList(parentTable, false, true) + ")/" + childTable.PascalName + "\", ResponseFormat = WebMessageFormat.Json)]");
						sb.AppendLine("		[OperationContract]");
						sb.AppendLine("		List<" + childTable.PascalName + "DTO> " + methodName + "(" + PrimaryKeyPublicParameterList(parentTable) + ")");
						sb.AppendLine("		{");
						sb.AppendLine("			List<" + childTable.PascalName + "DTO> retval = new List<" + childTable.PascalName + "DTO>();");
						sb.Append("			retval.SelectBy" + relation.PascalRoleName + parentTable.PascalName + "(");
						foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
						{
							string parameter = "";
							Column column = ((Column)columnRelationship.ParentColumnRef.Object);
							if (ModelHelper.IsTextType(column.DataType)) parameter = column.CamelName;
							else if (column.DataType == System.Data.SqlDbType.UniqueIdentifier) parameter = "new Guid(" + column.CamelName + ")";
							else parameter = column.GetCodeType(false) + ".Parse(" + column.CamelName + ")";

							sb.Append(parameter);
							if (relation.ColumnRelationships.IndexOf(columnRelationship) < relation.ColumnRelationships.Count - 1)
								sb.Append(", ");
						}
						sb.AppendLine(");");
						sb.AppendLine("			return retval;");
						sb.AppendLine("		}");
						sb.AppendLine();

						//Method 2
						sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + methodName + "?" + PrimaryKeyUrlFacade(parentTable));
						sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + childTable.PascalName + " objects for the parent " + parentTable.PascalName + " object.\")]");
						sb.AppendLine("		[WebGet(UriTemplate = \"" + parentTable.PascalName + "(" + PrimaryKeyColumnList(parentTable, false, true) + ")/" + childTable.PascalName + "?format=json\", ResponseFormat = WebMessageFormat.Json)]");
						sb.AppendLine("		[OperationContract]");
						sb.AppendLine("		List<" + childTable.PascalName + "DTO> " + methodName + "Json(" + PrimaryKeyPublicParameterList(parentTable) + ")");
						sb.AppendLine("		{");
						sb.AppendLine("			return " + methodName + "(" + PrimaryKeyColumnList(parentTable, false) + ");");
						sb.AppendLine("		}");
						sb.AppendLine();

						//Method 3
						sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + methodName + "?" + PrimaryKeyUrlFacade(parentTable));
						sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + childTable.PascalName + " objects for the parent " + parentTable.PascalName + " object.\")]");
						sb.AppendLine("		[WebGet(UriTemplate = \"" + parentTable.PascalName + "(" + PrimaryKeyColumnList(parentTable, false, true) + ")/" + childTable.PascalName + "?format=xml\", ResponseFormat = WebMessageFormat.Xml)]");
						sb.AppendLine("		[OperationContract]");
						sb.AppendLine("		List<" + childTable.PascalName + "DTO> " + methodName + "XML(" + PrimaryKeyPublicParameterList(parentTable) + ")");
						sb.AppendLine("		{");
						sb.AppendLine("			return " + methodName + "(" + PrimaryKeyColumnList(parentTable, false) + ");");
						sb.AppendLine("		}");
						sb.AppendLine();
					}

				}
			}
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion
		}

		private void SelectByColumn(Table table)
		{
			#region Search By Column

			sb.AppendLine("		#region " + table.PascalName + " Select by Column");
			sb.AppendLine();
			foreach (Column column in table.GetColumnsSearchable(true))
			{
				sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "By" + column.PascalName + "Field?" + column.CamelName + "=somevalue");
				sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
				sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "By" + column.PascalName + "Field({" + column.CamelName + "})/\", ResponseFormat = WebMessageFormat.Json)]");
				sb.AppendLine("		[OperationContract]");
				sb.AppendLine("		List<" + table.PascalName + "DTO> " + table.PascalName + "By" + column.PascalName + "Field(string " + column.CamelName + ")");
				sb.AppendLine("		{");
				sb.AppendLine("			List<" + table.PascalName + "DTO> retval = new List<" + table.PascalName + "DTO>();");

				if (ModelHelper.IsTextType(column.DataType))
					sb.AppendLine("			retval.SelectBy" + column.PascalName + "Field(" + column.CamelName + ");");
				else if (column.DataType == System.Data.SqlDbType.UniqueIdentifier)
					sb.AppendLine("			retval.SelectBy" + column.PascalName + "Field(new Guid(" + column.CamelName + "));");
				else
					sb.AppendLine("			retval.SelectBy" + column.PascalName + "Field(" + column.GetCodeType(false) + ".Parse(" + column.CamelName + "));");

				sb.AppendLine("			return retval;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "By" + column.PascalName + "Field?" + column.CamelName + "=somevalue");
				sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
				sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "By" + column.PascalName + "Field({" + column.CamelName + "})/?format=json\", ResponseFormat = WebMessageFormat.Json)]");
				sb.AppendLine("		[OperationContract]");
				sb.AppendLine("		List<" + table.PascalName + "DTO> " + table.PascalName + "By" + column.PascalName + "FieldJson(string " + column.CamelName + ")");
				sb.AppendLine("		{");
				sb.AppendLine("			return this." + table.PascalName + "By" + column.PascalName + "FieldJson(" + column.CamelName + ");");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "By" + column.PascalName + "Field?" + column.CamelName + "=somevalue");
				sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
				sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "By" + column.PascalName + "Field({" + column.CamelName + "})/?format=xml\", ResponseFormat = WebMessageFormat.Xml)]");
				sb.AppendLine("		[OperationContract]");
				sb.AppendLine("		List<" + table.PascalName + "DTO> " + table.PascalName + "By" + column.PascalName + "FieldXML(string " + column.CamelName + ")");
				sb.AppendLine("		{");
				sb.AppendLine("			return this." + table.PascalName + "By" + column.PascalName + "FieldJson(" + column.CamelName + ");");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
			sb.AppendLine("		#endregion");
			sb.AppendLine();

			#endregion
		}

		private void SelectByPrimaryKey(Table table)
		{
			#region Select By Primary Key
			sb.AppendLine("		#region " + table.PascalName + " Select By Primary Key");
			sb.AppendLine();
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "?" + PrimaryKeyUrlFacade(table));
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "(" + PrimaryKeyColumnList(table, false, true) + ")/\", ResponseFormat = WebMessageFormat.Json)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		" + table.PascalName + "DTO " + table.PascalName + "(" + PrimaryKeyPublicParameterList(table) + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			" + table.PascalName + "DTO retVal = new " + table.PascalName + "DTO();");
			sb.AppendLine("			retVal.SelectUsingPK(" + PrimaryKeyColumnTypeConversionList(table) + ");");
			sb.AppendLine("			return retVal;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "?" + PrimaryKeyUrlFacade(table));
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "(" + PrimaryKeyColumnList(table, false, true) + ")/?format=json\", ResponseFormat = WebMessageFormat.Json)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		" + table.PascalName + "DTO " + table.PascalName + "Json(" + PrimaryKeyPublicParameterList(table) + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			return this." + table.PascalName + "(" + PrimaryKeyColumnList(table, false) + ");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "?" + PrimaryKeyUrlFacade(table));
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "(" + PrimaryKeyColumnList(table, false, true) + ")/?format=xml\", ResponseFormat = WebMessageFormat.Xml)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		" + table.PascalName + "DTO " + table.PascalName + "XML(" + PrimaryKeyPublicParameterList(table) + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			return this." + table.PascalName + "(" + PrimaryKeyColumnList(table, false) + ");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion
		}

		private void SelectAll(Table table)
		{
			#region Select All
			sb.AppendLine("		#region " + table.PascalName + " Select All");
			sb.AppendLine();
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "List");
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "List\", ResponseFormat = WebMessageFormat.Json)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		List<" + table.PascalName + "DTO> " + table.PascalName + "List()");
			sb.AppendLine("		{");
			sb.AppendLine("			List<" + table.PascalName + "DTO> retval = new List<" + table.PascalName + "DTO>();");
			sb.AppendLine("			retval.RunSelect();");
			sb.AppendLine("			return retval;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "List");
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "List/?format=json\", ResponseFormat = WebMessageFormat.Json)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		List<" + table.PascalName + "DTO> " + table.PascalName + "ListJson()");
			sb.AppendLine("		{");
			sb.AppendLine("			return this." + table.PascalName + "List();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		// returns data in response to a HTTP GET request with URIs of the form http://<url-for-svc-file>/" + table.PascalName + "List");
			sb.AppendLine("		[WebHelp(Comment = \"Returns a list of " + table.PascalName + " objects to the user\")]");
			sb.AppendLine("		[WebGet(UriTemplate = \"" + table.PascalName + "List/?format=xml\", ResponseFormat = WebMessageFormat.Xml)]");
			sb.AppendLine("		[OperationContract]");
			sb.AppendLine("		List<" + table.PascalName + "DTO> " + table.PascalName + "ListXML()");
			sb.AppendLine("		{");
			sb.AppendLine("			return this." + table.PascalName + "List();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion
		}

		private string PrimaryKeyColumnList(Table table, bool includeType)
		{
			return PrimaryKeyColumnList(table, includeType, false);
		}

		private string PrimaryKeyColumnList(Table table, bool includeType, bool useBraces)
		{
			StringBuilder output = new StringBuilder();
			foreach (Column dc in table.PrimaryKeyColumns)
			{
				if (useBraces) output.Append("{");

				if (includeType)
					output.Append(dc.GetCodeType() + " ");

				output.Append(dc.CamelName);
				if (useBraces) output.Append("}");

				output.Append(", ");
			}
			string retval = output.ToString();
			if (retval.EndsWith(", ")) retval = retval.Substring(0, retval.Length - 2);
			return retval;
		}

		private string PrimaryKeyPublicParameterList(Table table)
		{
			StringBuilder output = new StringBuilder();
			foreach (Column dc in table.PrimaryKeyColumns)
			{
				output.Append("string " + dc.CamelName + ", ");
			}
			string retval = output.ToString();
			if (retval.EndsWith(", ")) retval = retval.Substring(0, retval.Length - 2);
			return retval;
		}

		private string PrimaryKeyColumnTypeConversionList(Table table)
		{
			StringBuilder output = new StringBuilder();
			foreach (Column dc in table.PrimaryKeyColumns)
			{
				if (ModelHelper.IsTextType(dc.DataType))
					output.Append(dc.CamelName + ", ");
				else if (dc.DataType == System.Data.SqlDbType.UniqueIdentifier)
					output.Append("new Guid(" + dc.CamelName + "), ");
				else
					output.Append(dc.GetCodeType(false) + ".Parse(" + dc.CamelName + "), ");
			}
			string retval = output.ToString();
			if (retval.EndsWith(", ")) retval = retval.Substring(0, retval.Length - 2);
			return retval;
		}

		private string PrimaryKeyUrlFacade(Table table)
		{
			StringBuilder output = new StringBuilder();
			foreach (Column dc in table.PrimaryKeyColumns)
			{
				output.Append(dc.CamelName + "=somevalue");
				output.Append("&");
			}
			string retval = output.ToString();
			if (retval.EndsWith("&")) retval = retval.Substring(0, retval.Length - 1);
			return retval;
		}

		#endregion

	}
}