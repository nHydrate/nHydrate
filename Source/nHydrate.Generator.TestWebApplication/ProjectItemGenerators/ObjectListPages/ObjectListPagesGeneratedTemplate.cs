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
using System.Linq;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;
using System.Collections;
using System.Collections.ObjectModel;
using Widgetsphere.Generator.ProjectItemGenerators;

namespace Widgetsphere.Generator.TestWebApplication.ProjectItemGenerators.ObjectListPages
{
	class ObjectListPagesGeneratedTemplate : ObjectListPagesTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _currentTable;

		public ObjectListPagesGeneratedTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("{0}.Generated.cs", _currentTable.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}.cs", _currentTable.PascalName); }
		}

		public override string FileContent
		{
			get
			{
				GenerateContent();
				return sb.ToString();
			}
		}

		#endregion

		#region GenerateContent

		private void GenerateContent()
		{
			try
			{
				ValidationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + DefaultNamespace);
				sb.AppendLine("{");
				this.AppendClass();
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region namespace / objects

		public void AppendUsingStatements()
		{
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Text;");
			sb.AppendLine("using System.Xml;");
			sb.AppendLine("using System.ComponentModel;");
			sb.AppendLine("using System.Data;");
			sb.AppendLine("using " + _model.CompanyName + "." + _model.ProjectName + ".DataTransfer;");
			sb.AppendLine("using " + _model.CompanyName + "." + _model.ProjectName + ".Business.Objects;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using System.Linq.Expressions;");
			sb.AppendLine("using " + _model.CompanyName + "." + _model.ProjectName + ".Business.LINQ;");
			sb.AppendLine("using Widgetsphere.Core.Util;");
			sb.AppendLine("using System.Reflection;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			try
			{
				sb.AppendLine("	public static partial class " + _currentTable.PascalName + "Extension");
				sb.AppendLine("	{");

				#region Fill DAL
				sb.AppendLine("		#region Fill DAL");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Populate a DAL object from its corresponding DTO.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"item\">The empty DAL object.</param>");
				sb.AppendLine("		/// <param name=\"dtoItem\">The source DTO to load.</param>");
				sb.AppendLine("		public static void Fill(this " + _currentTable.PascalName + " item, " + _currentTable.PascalName + "DTO dtoItem)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (dtoItem == null) throw new Exception(\"The " + _currentTable.PascalName + " DTO cannot be null.\");");
				sb.AppendLine("			if (item == null) throw new Exception(\"The " + _currentTable.PascalName + " item cannot be null.\");");
				sb.AppendLine("			DataRow dr = ((DataRow)((IWrappingClass)item).WrappedClass);");
				foreach (Reference reference in _currentTable.GeneratedColumns)
				{
					Column column = (Column)reference.Object;
					if (column.AllowNull)
					{
						sb.AppendLine("			if (dtoItem." + column.PascalName + " == null) dr[\"" + column.DatabaseName + "\"] = System.DBNull.Value;");
						sb.AppendLine("			else dr[\"" + column.DatabaseName + "\"] = dtoItem." + column.PascalName + ";");
					}
					else
					{
						sb.AppendLine("			dr[\"" + column.DatabaseName + "\"] = dtoItem." + column.PascalName + ";");
					}
				}

				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				#endregion

				#region Fill DTO

				sb.AppendLine("		#region Fill DTO");
				sb.AppendLine();
				//Fill for DTO collection
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Populate a DTO object collection from its corresponding DAL object collection.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"list\">The destination DTO collection.</param>");
				sb.AppendLine("		/// <param name=\"" + _currentTable.CamelName + "Collection\">The source DAL collection.</param>");
				sb.AppendLine("		public static void Fill(this List<" + _currentTable.PascalName + "DTO> list, " + _currentTable.PascalName + "Collection " + _currentTable.CamelName + "Collection)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (" + _currentTable.CamelName + "Collection == null) throw new Exception(\"The " + _currentTable.PascalName + " collection cannot be null.\");");
				sb.AppendLine("			if (list == null) list = new List<" + _currentTable.PascalName + "DTO>();");
				sb.AppendLine("			foreach (" + _currentTable.PascalName + " item in " + _currentTable.CamelName + "Collection)");
				sb.AppendLine("			{");
				sb.AppendLine("				" + _currentTable.PascalName + "DTO newItem = new " + _currentTable.PascalName + "DTO();");
				sb.AppendLine("				newItem.Fill(item);");
				sb.AppendLine("				list.Add(newItem);");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Populate a DTO object from its corresponding DAL object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"item\">The source DAL object.</param>");
				sb.AppendLine("		/// <param name=\"dtoItem\">The empty DTO to load.</param>");
				sb.AppendLine("		public static void Fill(this " + _currentTable.PascalName + "DTO dtoItem, " + _currentTable.PascalName + " item)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (dtoItem == null) throw new Exception(\"The " + _currentTable.PascalName + " DTO cannot be null.\");");
				sb.AppendLine("			if (item == null) throw new Exception(\"The " + _currentTable.PascalName + " item cannot be null.\");");

				ColumnCollection allColumns = _currentTable.GetColumnsFullHierarchy();
				foreach (Column column in allColumns)
				{
					List<TableComposite> tableCompositeList = _currentTable.GetTableCompositesFullHierarchy(true);
					List<Column> fullComponentColumnList = new List<Column>();
					foreach (TableComposite TableComposite in tableCompositeList)
					{
						fullComponentColumnList.AddRange(_currentTable.CompositeList.GetAllColumns(true));
					}

					//If this column is part of a component then hide it
					var q = from x in _currentTable.CompositeList.GetAllColumns(true)
									where x == column
									select x;

					if (q.FirstOrDefault() == null)
					{
						//Normal property
						sb.AppendLine("			dtoItem." + column.PascalName + " = item." + column.PascalName + ";");
					}
				}

				//Set properties in components
				foreach (TableComposite TableComposite in _currentTable.CompositeList)
				{
					foreach (Reference reference in TableComposite.Columns)
					{
						Column column = (Column)reference.Object;
						sb.AppendLine("			dtoItem." + column.PascalName + " = item." + TableComposite.PascalName + "Item." + column.PascalName + ";");
					}
				}

				if (_currentTable.AllowCreateAudit)
				{
					sb.AppendLine("			dtoItem." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + " = item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ";");
					sb.AppendLine("			dtoItem." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + " = item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ";");
				}
				if (_currentTable.AllowModifiedAudit)
				{
					sb.AppendLine("			dtoItem." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + " = item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ";");
					sb.AppendLine("			dtoItem." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + " = item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ";");
				}

				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				#endregion

				#region RunSelect
				sb.AppendLine("		#region RunSelect");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select all objects from store.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public static void RunSelect(this List<" + _currentTable.PascalName + "DTO> list)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (list == null) list = new List<" + _currentTable.PascalName + "DTO>();");
				sb.AppendLine("			" + _currentTable.PascalName + "Collection retval = " + _currentTable.PascalName + "Collection.RunSelect();");
				sb.AppendLine("			foreach (" + _currentTable.PascalName + " item in retval)");
				sb.AppendLine("			{");
				sb.AppendLine("				" + _currentTable.PascalName + "DTO newItem = new " + _currentTable.PascalName + "DTO();");
				sb.AppendLine("				newItem.Fill(item);");
				sb.AppendLine("				list.Add(newItem);");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();

				//RunSelect LINQ
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Using the specified Where expression, execute a query against the database to return a result set.");
				sb.AppendLine("		/// </summary>");				
				sb.AppendLine("		public static void RunSelect(this List<" + _currentTable.PascalName + "DTO> list, Expression<Func<" + _currentTable.PascalName + "Query, bool>> where)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (list == null) list = new List<" + _currentTable.PascalName + "DTO>();");
				sb.AppendLine("			list.Fill(" + _currentTable.PascalName + "Collection.RunSelect(where));");
				sb.AppendLine("		}");
				sb.AppendLine();
				
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				#endregion

				#region RunSelect Paged

				sb.AppendLine("		#region RunSelect Paged");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Using the specified Where expression, execute a query against the database to return a paged result set.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public static PagedQueryResults<" + _currentTable.PascalName + "DTO> RunSelect(this List<" + _currentTable.PascalName + "DTO> list, string pageSize, string page, string sortOrder, string sortColumn, string linq)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (list == null) list = new List<" + _currentTable.PascalName + "DTO>();");
				sb.AppendLine("			#region Setup Variables and Error Check");
				sb.AppendLine("			int po;");
				sb.AppendLine("			int rpp;");
				sb.AppendLine();
				sb.AppendLine("			if (!int.TryParse(page, out po)) throw new Exception(\"The page number number is not a valid integer!\");");
				sb.AppendLine("			if (po < 1) po = 1;");
				sb.AppendLine();
				sb.AppendLine("			if (!int.TryParse(pageSize, out rpp)) throw new Exception(\"The page size is not a valid integer!\");");
				sb.AppendLine("			if (rpp < 1) rpp = 1;");
				sb.AppendLine();
				sb.AppendLine("			" + _currentTable.PascalName + ".FieldNameConstants sortField;");
				sb.AppendLine("			try");
				sb.AppendLine("			{");
				sb.AppendLine("				sortField = (" + _currentTable.PascalName + ".FieldNameConstants)Enum.Parse(typeof(" + _currentTable.PascalName + ".FieldNameConstants), sortColumn);");
				sb.AppendLine("			}");
				sb.AppendLine("			catch (Exception ex)");
				sb.AppendLine("			{");
				sb.AppendLine("				throw new Exception(\"The sort field is not valid!\");");
				sb.AppendLine("			}");
				sb.AppendLine();
				sb.AppendLine("			bool ascending = (sortOrder.ToLower() == \"asc\" || sortOrder.ToLower() == \"ascending\");");
				sb.AppendLine("			#endregion");
				sb.AppendLine();
				sb.AppendLine("			PagedQueryResults<" + _currentTable.PascalName + "DTO> retVal = new PagedQueryResults<" + _currentTable.PascalName + "DTO>();");
				sb.AppendLine("			" + _currentTable.PascalName + "Paging paging = new " + _currentTable.PascalName + "Paging();");
				sb.AppendLine("			paging.RecordsperPage = rpp;");
				sb.AppendLine("			paging.PageIndex = po;");
				sb.AppendLine("			paging.OrderByList.Add(new " + _currentTable.PascalName + "PagingFieldItem(sortField, ascending));");
				sb.AppendLine();
				sb.AppendLine("			" + _currentTable.PascalName + "Collection dalList = null;");
				sb.AppendLine("			if (string.IsNullOrEmpty(linq))");
				sb.AppendLine("			{");
				sb.AppendLine("				dalList = " + _currentTable.PascalName + "Collection.RunSelect(x => true, paging);");
				sb.AppendLine("			}");
				sb.AppendLine("			else");
				sb.AppendLine("			{");
				sb.AppendLine("				MethodInfo method = LINQDynamicCompile.GetMethod(linq, \"" + _model.CompanyName + "." + _model.ProjectName + ".Business\", \"" + _currentTable.PascalName + "\");");
				sb.AppendLine("				dalList = (" + _currentTable.PascalName + "Collection)method.Invoke(null, new object[] { paging });");
				sb.AppendLine("			}");
				sb.AppendLine();
				sb.AppendLine("			if (list == null) list = new List<" + _currentTable.PascalName + "DTO>();");
				sb.AppendLine("			foreach (" + _currentTable.PascalName + " item in dalList)");
				sb.AppendLine("			{");
				sb.AppendLine("				" + _currentTable.PascalName + "DTO newItem = new " + _currentTable.PascalName + "DTO();");
				sb.AppendLine("				newItem.Fill(item);");
				sb.AppendLine("				list.Add(newItem);");
				sb.AppendLine("			}");
				sb.AppendLine();
				sb.AppendLine("			retVal.CurrentPage = paging.PageIndex;");
				sb.AppendLine("			double totalPages = Math.Ceiling((double)paging.RecordCount / (double)paging.RecordsperPage);");
				sb.AppendLine("			retVal.TotalPages = (int)totalPages;");
				sb.AppendLine("			retVal.TotalRecords = paging.RecordCount;");
				sb.AppendLine("			retVal.GridData = list;");
				sb.AppendLine();
				sb.AppendLine("			return retVal;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				#endregion

				#region DeleteData

				//sb.AppendLine("		public static int DeleteData(this List<" + _currentTable.PascalName + "DTO> list, Expression<Func<" + _currentTable.PascalName + "Query, bool>> where);");
				//sb.AppendLine("		{");
				//sb.AppendLine("			return " + _currentTable.PascalName + "Collection.DeleteData(where);");
				//sb.AppendLine("		}");

				#endregion

				#region Persist

				if (!_currentTable.Immutable && !_currentTable.IsTypeTable)
				{
					sb.AppendLine("		#region Persist");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Persists this object to store.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public static " + _currentTable.PascalName + "DTO Persist(this " + _currentTable.PascalName + "DTO " + _currentTable.CamelName + ")");
					sb.AppendLine("		{");
					sb.AppendLine("			if (" + _currentTable.CamelName + " == null) throw new Exception(\"The " + _currentTable.PascalName + " DTO cannot be null.\");");
					sb.AppendLine("			//Find this object and if not exists, create a new one");
					sb.Append("			" + _currentTable.PascalName + " newItem = " + _model.CompanyName + "." + _model.ProjectName + ".Business.Objects." + _currentTable.PascalName + ".SelectUsingPK(");
					foreach (Column column in _currentTable.PrimaryKeyColumns)
					{
						sb.Append(_currentTable.CamelName + "." + column.PascalName + "");
						if (_currentTable.PrimaryKeyColumns.IndexOf(column) < _currentTable.PrimaryKeyColumns.Count - 1)
							sb.Append(", ");
					}
					sb.AppendLine(");");
					sb.AppendLine("			" + _currentTable.PascalName + "Collection " + _currentTable.CamelName + "Collection = null;");
					sb.AppendLine("			if (newItem == null)");
					sb.AppendLine("			{");
					sb.AppendLine("				" + _currentTable.CamelName + "Collection = new " + _currentTable.PascalName + "Collection();");
					sb.AppendLine("				newItem = " + _currentTable.CamelName + "Collection.NewItem();");
					sb.AppendLine("			}");
					sb.AppendLine("			else");
					sb.AppendLine("			{");
					sb.AppendLine("				" + _currentTable.CamelName + "Collection = newItem.ParentCollection;");
					sb.AppendLine("			}	");
					sb.AppendLine();
					sb.AppendLine("			//Populate the DAL and perist");
					sb.AppendLine("			newItem.Fill(" + _currentTable.CamelName + ");");
					sb.AppendLine("			if (!newItem.IsParented) " + _currentTable.CamelName + "Collection.AddItem(newItem);");
					sb.AppendLine("			" + _currentTable.CamelName + "Collection.Persist();");
					sb.AppendLine();
					sb.AppendLine("			//Re-populate the passed-in object from the DAL");
					sb.AppendLine("			" + _currentTable.CamelName + ".Fill(newItem);");
					sb.AppendLine("			return " + _currentTable.CamelName + ";");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Persists this collection to store.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public static bool Persist(this List<" + _currentTable.PascalName + "DTO> " + _currentTable.CamelName + "List)");
					sb.AppendLine("		{");
					sb.AppendLine("			if (" + _currentTable.CamelName + "List == null) throw new Exception(\"The " + _currentTable.PascalName + " DTO list cannot be null.\");");
					sb.AppendLine("			//Get all objects from the database that match PK");
					sb.AppendLine("			" + _currentTable.PascalName + "Collection " + _currentTable.CamelName + "Collection = new " + _currentTable.PascalName + "Collection();");
					sb.AppendLine("			System.Collections.ArrayList keyList = new System.Collections.ArrayList();");
					sb.AppendLine("			foreach (" + _currentTable.PascalName + "DTO dto in " + _currentTable.CamelName + "List)");
					sb.AppendLine("			{");
					sb.Append("				keyList.Add(new " + _currentTable.PascalName + "PrimaryKey(");

					foreach (Column column in _currentTable.PrimaryKeyColumns)
					{
						sb.Append("dto." + column.PascalName);
						if (_currentTable.PrimaryKeyColumns.IndexOf(column) < _currentTable.PrimaryKeyColumns.Count - 1)
							sb.Append(", ");
					}

					sb.AppendLine("));");
					sb.AppendLine("			}			");
					sb.AppendLine("			" + _currentTable.CamelName + "Collection.SubDomain.AddSelectCommand(new " + _model.CompanyName + "." + _model.ProjectName + ".Business.SelectCommands." + _currentTable.PascalName + "SelectByPks(keyList));");
					sb.AppendLine("			" + _currentTable.CamelName + "Collection.SubDomain.RunSelectCommands();");
					sb.AppendLine();
					sb.AppendLine("			//Now find and populate the objects for saving");
					sb.AppendLine("			foreach (" + _currentTable.PascalName + "DTO dto in " + _currentTable.CamelName + "List)");
					sb.AppendLine("			{");
					sb.AppendLine("				" + _currentTable.PascalName + " " + _currentTable.CamelName + " = (from x in (IEnumerable<" + _currentTable.PascalName + ">)" + _currentTable.CamelName + "Collection");

					foreach (Column column in _currentTable.PrimaryKeyColumns)
					{
						if (_currentTable.PrimaryKeyColumns.IndexOf(column) == 0)
							sb.Append("					where ");

						sb.Append("x." + column.PascalName + " == dto." + column.PascalName);
						if (_currentTable.PrimaryKeyColumns.IndexOf(column) < _currentTable.PrimaryKeyColumns.Count - 1)
							sb.Append(" && ");
					}
					sb.AppendLine();
					sb.AppendLine("					select x).FirstOrDefault();");
					
					sb.AppendLine();
					sb.AppendLine("				if (" + _currentTable.CamelName + " == null) " + _currentTable.CamelName + " = " + _currentTable.CamelName + "Collection.NewItem();");
					sb.AppendLine("				" + _currentTable.CamelName + ".Fill(dto);");
					sb.AppendLine("				if (!" + _currentTable.CamelName + ".IsParented) " + _currentTable.CamelName + "Collection.AddItem(" + _currentTable.CamelName + ");");
					sb.AppendLine("			}");
					sb.AppendLine("			" + _currentTable.CamelName + "Collection.Persist();");
					sb.AppendLine();					
					sb.AppendLine("			//Re-populate the passed-in object from the DAL");
					sb.AppendLine("			List<" + _currentTable.PascalName + "DTO> deletedList = new List<" + _currentTable.PascalName + "DTO>();");
					sb.AppendLine("			foreach (" + _currentTable.PascalName + "DTO dto in " + _currentTable.CamelName + "List)");
					sb.AppendLine("			{");
					sb.AppendLine("				" + _currentTable.PascalName + " " + _currentTable.CamelName + " = (from x in (IEnumerable<" + _currentTable.PascalName + ">)" + _currentTable.CamelName + "Collection");

					foreach (Column column in _currentTable.PrimaryKeyColumns)
					{
						if (_currentTable.PrimaryKeyColumns.IndexOf(column) == 0)
							sb.Append("					where ");

						sb.Append("x." + column.PascalName + " == dto." + column.PascalName);
						if (_currentTable.PrimaryKeyColumns.IndexOf(column) < _currentTable.PrimaryKeyColumns.Count - 1)
							sb.Append(" && ");
					}
					sb.AppendLine();

					sb.AppendLine("					select x).FirstOrDefault();");
					sb.AppendLine();
					sb.AppendLine("				if (" + _currentTable.CamelName + " != null) dto.Fill(" + _currentTable.CamelName + ");");
					sb.AppendLine("				else deletedList.Add(dto);");
					sb.AppendLine("			}");
					sb.AppendLine();
					sb.AppendLine("			//Remove the items that are no longer there (if any)");
					sb.AppendLine("			foreach (" + _currentTable.PascalName + "DTO dto in deletedList)");
					sb.AppendLine("			{");
					sb.AppendLine("				" + _currentTable.CamelName + "List.Remove(dto);");
					sb.AppendLine("			}");
					sb.AppendLine();
					sb.AppendLine("			return true;");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		#endregion");
					sb.AppendLine();
				}

				#endregion

				#region SelectByField

				sb.AppendLine("		#region " + _currentTable.PascalName + " Select by Column");
				sb.AppendLine();
				foreach (Column column in _currentTable.GetColumnsSearchable(true))
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Select an object list by '" + column.PascalName + "' field.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public static bool SelectBy" + column.PascalName + "Field(this List<" + _currentTable.PascalName + "DTO> " + _currentTable.CamelName + "List, " + column.GetCodeType(false) + " " + column.CamelName + ")");
					sb.AppendLine("		{");
					sb.AppendLine("			" + _currentTable.PascalName + "Collection list = " + _currentTable.PascalName + "Collection.SelectBy" + column.PascalName + "(" + column.CamelName + ");");
					sb.AppendLine("			foreach (" + _currentTable.PascalName + " item in list)");
					sb.AppendLine("			{");
					sb.AppendLine("				" + _currentTable.PascalName + "DTO newItem = new " + _currentTable.PascalName + "DTO();");
					sb.AppendLine("				newItem.Fill(item);");
					sb.AppendLine("				" + _currentTable.CamelName + "List.Add(newItem);");
					sb.AppendLine("			}");
					sb.AppendLine("			return true;");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				#endregion

				#region SelectUsingPK

				sb.AppendLine("		#region " + _currentTable.PascalName + " SelectUsingPK");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select a single object from this collection by its primary key.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public static bool SelectUsingPK(this " + _currentTable.PascalName + "DTO " + _currentTable.CamelName + ", " + PrimaryKeyColumnList(_currentTable, true) + ")");
				sb.AppendLine("		{");
				sb.AppendLine("			" + _currentTable.PascalName + " item = " + _model.CompanyName + "." + _model.ProjectName + ".Business.Objects." + _currentTable.PascalName + ".SelectUsingPK(" + PrimaryKeyColumnList(_currentTable, false) + ");");
				sb.AppendLine("			" + _currentTable.CamelName + ".Fill(item);");
				sb.AppendLine("			return true;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				#endregion

				#region SelectByFK

				sb.AppendLine("		#region " + _currentTable.PascalName + " Select By Foreign Key");
				sb.AppendLine();
				List<Relation> allRelations = _currentTable.GetRelationsFullHierarchy();
				List<string> methodList = new List<string>();
				foreach (Relation relation in allRelations)
				{
					Table parentTable = (Table)relation.ParentTableRef.Object;
					Table childTable = (Table)relation.ChildTableRef.Object;
					if (childTable == _currentTable)
					{
						string methodName = "SelectBy" + relation.PascalRoleName + parentTable.PascalName;
						if (!methodList.Contains(methodName))
						{
							methodList.Add(methodName);

							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Populate a '" + parentTable.PascalName + "' DTO object collection from its relationship with the '" + childTable.PascalName + "' table.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public static bool " + methodName + "(this List<" + childTable.PascalName + "DTO> " + childTable.PascalName + "List, " + PrimaryKeyColumnList(parentTable, true) + ")");
							sb.AppendLine("		{");
							sb.Append("			" + childTable.PascalName + "Collection list = " + childTable.PascalName + "Collection.RunSelect(x => ");
							foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
							{
								Column column = ((Column)columnRelationship.ParentColumnRef.Object);
								sb.Append("x." + ((Column)columnRelationship.ChildColumnRef.Object).PascalName + " == " + column.CamelName + "");
								if (relation.ColumnRelationships.IndexOf(columnRelationship) < relation.ColumnRelationships.Count - 1)
									sb.Append(" && ");
							}
							sb.AppendLine(");");
							sb.AppendLine("			foreach (" + childTable.PascalName + " item in list)");
							sb.AppendLine("			{");
							sb.AppendLine("				" + childTable.PascalName + "DTO newItem = new " + childTable.PascalName + "DTO();");
							sb.AppendLine("				newItem.Fill(item);");
							sb.AppendLine("				" + childTable.PascalName + "List.Add(newItem);");
							sb.AppendLine("			}");
							sb.AppendLine("			return true;");
							sb.AppendLine("		}");
							sb.AppendLine();
						}
					}

				}

				sb.AppendLine("		#endregion");
				sb.AppendLine();

				#endregion

				sb.AppendLine("	}");
				sb.AppendLine();

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		private string PrimaryKeyColumnList(Table table, bool includeType)
		{
			StringBuilder output = new StringBuilder();
			foreach (Column dc in table.PrimaryKeyColumns)
			{
				if (includeType) output.Append(dc.GetCodeType() + " ");
				output.Append(dc.CamelName);
				output.Append(", ");
			}
			string retval = output.ToString();
			if (retval.EndsWith(", ")) retval = retval.Substring(0, retval.Length - 2);
			return retval;
		}

	}
}