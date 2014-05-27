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
using System.Data;
using Widgetsphere.Generator.Models;
using System.Collections;

namespace Widgetsphere.Generator.ProjectItemGenerators
{
	class Globals
	{
		public static string GetDateTimeNowCode(ModelRoot model)
		{
			if (model.UseUTCTime) return "DateTime.UtcNow";
			else return "DateTime.Now";
		}

		public static ArrayList GetValidSearchColumns(Table _currentTable)
		{
			try
			{
				ArrayList validColumns = new ArrayList();
				foreach (Reference reference in _currentTable.GeneratedColumns)
				{
					Column dc = (Column)reference.Object;
					if (!(dc.DataType == System.Data.SqlDbType.Binary ||
						dc.DataType == System.Data.SqlDbType.Image ||
						dc.DataType == System.Data.SqlDbType.NText ||
						dc.DataType == System.Data.SqlDbType.Text ||
						dc.DataType == System.Data.SqlDbType.Timestamp ||
						dc.DataType == System.Data.SqlDbType.Udt ||
						dc.DataType == System.Data.SqlDbType.VarBinary ||
						dc.DataType == System.Data.SqlDbType.Variant ||
					dc.DataType == System.Data.SqlDbType.Money))
					{
						validColumns.Add(dc);
					}
				}
				return validColumns;

			}
			catch (Exception ex)
			{
				throw new Exception(_currentTable.DatabaseName + ": Failed on generation of select or template", ex);
			}
		}

		public static void AppendBusinessEntryCatch(StringBuilder sb)
		{
			sb.AppendLine("			catch (System.Data.DBConcurrencyException dbcex)");
			sb.AppendLine("			{");
			sb.AppendLine("				throw new ConcurrencyException(\"Concurrency failure\", dbcex);");
			sb.AppendLine("			}");
			sb.AppendLine("			catch (System.Data.SqlClient.SqlException sqlexp)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (sqlexp.Number == 547 || sqlexp.Number == 2627)");
			sb.AppendLine("				{");
			sb.AppendLine("					throw new UniqueConstraintViolatedException(\"Constraint Failure\", sqlexp);");
			sb.AppendLine("				}");
			sb.AppendLine("				else");
			sb.AppendLine("				{");
			sb.AppendLine("					throw;");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine("			catch(Exception ex)");
			sb.AppendLine("			{");
			sb.AppendLine("				System.Diagnostics.Debug.WriteLine(ex.ToString());");
			sb.AppendLine("				throw;");
			sb.AppendLine("			}");
		}

		public static string BuildSelectList(TableComponent component, ModelRoot model)
		{
			int index = 0;
			StringBuilder output = new StringBuilder();
			List<Column> columnList = new List<Column>();
			foreach (Reference r in component.Columns)
				columnList.Add((Column)r.Object);

			foreach (Column dc in columnList)
			{
				Table parentTable = (Table)dc.ParentTableRef.Object;
				output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, parentTable), dc.DatabaseName.ToLower());
				if ((index < columnList.Count - 1) || (component.Parent.AllowCreateAudit) || (component.Parent.AllowModifiedAudit) || (component.Parent.AllowTimestamp))
					output.Append(",");
				output.AppendLine();
				index++;
			}

			if (component.Parent.AllowCreateAudit)
			{
				output.AppendFormat("	[{0}].[{1}],", GetTableDatabaseName(model, component.Parent), model.Database.CreatedByColumnName);
				output.AppendLine();

				output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, component.Parent), model.Database.CreatedDateColumnName);
				if ((component.Parent.AllowModifiedAudit) || (component.Parent.AllowTimestamp))
					output.Append(",");
				output.AppendLine();
			}

			if (component.Parent.AllowModifiedAudit)
			{
				output.AppendFormat("	[{0}].[{1}],", GetTableDatabaseName(model, component.Parent), model.Database.ModifiedByColumnName);
				output.AppendLine();

				output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, component.Parent), model.Database.ModifiedDateColumnName);
				if (component.Parent.AllowTimestamp)
					output.Append(",");
				output.AppendLine();
			}

			if (component.Parent.AllowTimestamp)
			{
				output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, component.Parent), model.Database.TimestampColumnName);
				output.AppendLine();
			}

			return output.ToString();
		}

		public static string BuildSelectList(Table table, ModelRoot model)
		{
			return BuildSelectList(table, model, false);
		}

		public static string BuildSelectList(Table table, ModelRoot model, bool useFullHierarchy)
		{
			int index = 0;
			StringBuilder output = new StringBuilder();
			List<Column> columnList = new List<Column>();
			if (useFullHierarchy)
			{
				foreach (Column c in table.GetColumnsFullHierarchy())
					columnList.Add(c);
			}
			else
			{
				foreach (Reference r in table.GeneratedColumns)
					columnList.Add((Column)r.Object);
			}

			foreach (Column dc in columnList)
			{
				Table parentTable = (Table)dc.ParentTableRef.Object;
				output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, parentTable), dc.DatabaseName.ToLower());
				if ((index < columnList.Count - 1) || (table.AllowCreateAudit) || (table.AllowModifiedAudit) || (table.AllowTimestamp))
					output.Append(",");
				output.AppendLine();
				index++;
			}

			if (table.AllowCreateAudit)
			{
				output.AppendFormat("	[{0}].[{1}],", GetTableDatabaseName(model, table), model.Database.CreatedByColumnName);
				output.AppendLine();

				output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, table), model.Database.CreatedDateColumnName);
				if ((table.AllowModifiedAudit) || (table.AllowTimestamp))
					output.Append(",");
				output.AppendLine();
			}

			if (table.AllowModifiedAudit)
			{
				output.AppendFormat("	[{0}].[{1}],", GetTableDatabaseName(model, table), model.Database.ModifiedByColumnName);
				output.AppendLine();

				output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, table), model.Database.ModifiedDateColumnName);
				if (table.AllowTimestamp)
					output.Append(",");
				output.AppendLine();
			}

			if (table.AllowTimestamp)
			{
				output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, table), model.Database.TimestampColumnName);
				output.AppendLine();
			}

			return output.ToString();
		}

    public static string BuildSelectList(CustomView view, ModelRoot model)
    {
      int index = 0;
      StringBuilder output = new StringBuilder();
      foreach (Reference columnRef in view.GeneratedColumns)
      {
        CustomViewColumn dc = (CustomViewColumn)columnRef.Object;
        output.Append("CONVERT(" + dc.DatabaseType + ", [view_" + view.DatabaseName + "].[" + dc.DatabaseName + "]) AS [" + dc.DatabaseName + "]");
        if (index < view.GeneratedColumns.Count - 1)
          output.Append(",");
        output.AppendLine();
        index++;
      }
      return output.ToString();
    }

		public static string BuildPrimaryKeySelectList(ModelRoot model, Table table, bool qualifiedNames)
		{
			int index = 0;
			StringBuilder output = new StringBuilder();
			foreach (Column dc in table.PrimaryKeyColumns)
			{
				output.Append("	[");
				if (qualifiedNames)
				{
          output.Append(Globals.GetTableDatabaseName(model, table));
					output.Append("].[");
				}
				output.Append(dc.DatabaseName.ToLower() + "]");
				if (index < table.PrimaryKeyColumns.Count - 1)
					output.Append(",");
				output.AppendLine();
				index++;
			}
			return output.ToString();
		}

		public static string ReplaceAcmeSpecifics(string input, ModelRoot model)
		{
			try
			{
				string retVal = input;
				retVal = retVal.Replace("ACME", model.CompanyName);
				retVal = retVal.Replace("Acme", model.CompanyName);
				retVal = retVal.Replace("NorthwindNet", model.ProjectName);
				retVal = retVal.Replace("ZZ", model.CompanyAbbreviation);
				return retVal;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

    public static string GetSerializationMethod(System.Data.SqlDbType type)
    {
      switch(type)
      {
        case SqlDbType.BigInt:
        case SqlDbType.Money:
          return "GetInt64";        
        case SqlDbType.Bit:
          return "GetBoolean";          
        case SqlDbType.DateTime:
        case SqlDbType.SmallDateTime:
          return "GetDateTime";
        case SqlDbType.Decimal:
        case SqlDbType.Real:
          return "GetDouble";
        case SqlDbType.Float:
          return "GetSingle";        
        case SqlDbType.Int:
        case SqlDbType.SmallMoney:
        case SqlDbType.SmallInt:
          return "GetInt32";
        case SqlDbType.Char:
        case SqlDbType.NChar:
        case SqlDbType.NText:
        case SqlDbType.NVarChar:
        case SqlDbType.Text:        
        case SqlDbType.UniqueIdentifier:
        case SqlDbType.VarChar:
          return "GetString";
        case SqlDbType.TinyInt:
          return "GetByte";
        case SqlDbType.Binary:
        case SqlDbType.Timestamp:
        case SqlDbType.Udt:
        case SqlDbType.VarBinary:
        case SqlDbType.Variant:
        case SqlDbType.Xml:
        case SqlDbType.Image:
          return "";
      }
      return "";
    }

    public static string GetTableDatabaseName(ModelRoot model, Table table)
    {
      string retval = model.Database.TablePrefix;
      if(retval != "")
        return retval + "_" + table.DatabaseName;
      else
        return table.DatabaseName;
    }
	
		public static string GetPascalName(ModelRoot model, Table table)
    {
      string retval = model.Database.TablePrefix;
      if(retval != "")
        return retval + "_" + table.PascalName;
      else
        return table.PascalName;
    }

    public static string GetPascalName(ModelRoot model, CustomView view)
    {
      string retval = model.Database.TablePrefix;
      if(retval != "")
        return retval + "_" + view.PascalName;
      else
        return view.PascalName;
    }

    public static string GetPascalName(ModelRoot model, CustomStoredProcedure sp)
    {
      string retval = model.Database.TablePrefix;
      if(retval != "")
        return retval + "_" + sp.PascalName;
      else
        return sp.PascalName;
    }

		public static Column GetColumnByName(ReferenceCollection referenceCollection, string name)
		{
			foreach (Reference r in referenceCollection)
			{
				if (r.Object is Column)
				{
					if (string.Compare(((Column)r.Object).Name, name, true) == 0)
						return (Column)r.Object;
				}
			}
			return null;
		}

		public static Column GetColumnByKey(ReferenceCollection referenceCollection, string columnKey)
		{
			foreach (Reference r in referenceCollection)
			{
				if (r.Object is Column)
				{
					if (string.Compare(((Column)r.Object).Key, columnKey, true) == 0)
						return (Column)r.Object;
				}
			}
			return null;
		}

	}
}
