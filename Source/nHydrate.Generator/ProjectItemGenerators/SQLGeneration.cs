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
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators
{
	internal class SQLGeneration
	{
		public static string GetSQLCreateTable(ModelRoot model, Table table)
		{
			StringBuilder sb = new StringBuilder();
			string tableName = Globals.GetTableDatabaseName(model, table);
			sb.AppendLine("if not exists(select * from sysobjects where name = '" + tableName + "' and xtype = 'U')");
			sb.AppendLine("CREATE TABLE [dbo].[" + tableName + "] (");

			bool firstLoop = true;
			foreach (Reference columnRef in table.Columns)
			{
				Column column = (Column)columnRef.Object;
				if (!firstLoop) sb.AppendLine(",");
				else firstLoop = false;
				sb.Append(AppendColumnDefinition(column, true, true));
			}
			AppendModifiedAudit(model, table, sb);
			AppendCreateAudit(model, table, sb);
			AppendTimestamp(model, table, sb);
			sb.AppendLine();
			sb.AppendLine(") ON [PRIMARY]");
			sb.AppendLine();

			return sb.ToString();

		}

		public static string GetSQLCreateAuditTable(ModelRoot model, Table table)
		{
			StringBuilder sb = new StringBuilder();
			string tableName = "__AUDIT__" + Globals.GetTableDatabaseName(model, table);
			sb.AppendLine("if not exists(select * from sysobjects where name = '" + tableName + "' and xtype = 'U')");
			sb.AppendLine("CREATE TABLE [dbo].[" + tableName + "] (");
			sb.AppendLine("[__rowid] [INT] NOT NULL IDENTITY,");
			sb.AppendLine("[__action] [INT] NOT NULL,");
			sb.AppendLine("[__insertdate] [DATETIME] CONSTRAINT [DF__" + table.DatabaseName + "__AUDIT] DEFAULT " + (model.UseUTCTime ? "GetUTCDate()" : "GetDate()") + " NOT NULL,");
			
			ColumnCollection columnList = table.GetColumns();
			foreach (Column column in columnList)
			{				
				sb.Append(AppendColumnDefinition(column, false, false, false));
				if (columnList.IndexOf(column) < columnList.Count - 1) sb.Append(",");
				sb.AppendLine();
			}			
			sb.AppendLine(") ON [PRIMARY]");
			sb.AppendLine();
			return sb.ToString();

		}

		public static string GetSQLAddColumn(Column column)
		{
			StringBuilder sb = new StringBuilder();
			string tName = "" + ((Table)column.ParentTableRef.Object).DatabaseName;
			sb.AppendLine("if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + column.DatabaseName + "' and o.name = '" + tName + "')");
			sb.AppendLine("ALTER TABLE [" + tName + "] ADD " + AppendColumnDefinition(column, true, true));
			//sb.Append(AppendColumnDefault(column));
			return sb.ToString();
		}

		public static string GetSQLDropColumn(Column column)
		{
			StringBuilder sb = new StringBuilder();

			Table t = (Table)column.ParentTableRef.Object;

			#region Delete Defaults
			sb.Append("select 'ALTER TABLE [" + t.DatabaseName + "] DROP CONSTRAINT ' + [name] as 'sql' ");
			sb.Append("into #t ");
			sb.Append("from sysobjects ");
			sb.Append("where id IN( ");
			sb.Append("select sc.cdefault ");
			sb.Append("FROM dbo.sysobjects SO INNER JOIN dbo.syscolumns SC ON SO.id = SC.id ");
			sb.Append("LEFT JOIN dbo.syscomments SM ON SC.cdefault = SM.id ");
			sb.Append("WHERE SO.xtype = 'U' and SO.NAME = '" + t.DatabaseName + "' and SC.NAME = '" + column.DatabaseName + "' ");
			sb.AppendLine(")");
			sb.AppendLine("declare @sql nvarchar (1000)");
			sb.AppendLine("SELECT @sql = MAX([sql]) from #t");
			sb.AppendLine("exec (@sql)");
			sb.AppendLine("drop table #t");
			#endregion

			#region Delete Parent Relations
			for (int ii = t.ParentRoleRelations.Count - 1; ii >= 0; ii--)
			{
				Relation parentR = (Relation)t.ParentRoleRelations[ii];
				Table parentT = (Table)parentR.ParentTableRef.Object;
				Table childT = (Table)parentR.ChildTableRef.Object;
				if (parentR.ParentTableRef.Object == t)
				{
					bool removeRelationship = false;
					foreach (ColumnRelationship cr in parentR.ColumnRelationships)
					{
						if (cr.ParentColumnRef.Object == column)
							removeRelationship = true;
					}

					if (removeRelationship)
					{
						string objectName = "FK_" +
							parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
							"_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);

						//sb.AppendLine("--MARK 6");
						sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
						sb.AppendLine("ALTER TABLE [" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
						sb.AppendLine();
					}
				}
			}
			#endregion

			#region Delete Child Relations
			for (int ii = t.ChildRoleRelations.Count - 1; ii >= 0; ii--)
			{
				Relation childR = (Relation)t.ChildRoleRelations[ii];
				Table parentT = (Table)childR.ParentTableRef.Object;
				Table childT = (Table)childR.ChildTableRef.Object;
				for (int jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
				{
					Relation parentR = (Relation)parentT.ParentRoleRelations[jj];
					if (parentR.ChildTableRef.Object == t)
					{
						bool removeRelationship = false;
						foreach (ColumnRelationship cr in childR.ColumnRelationships)
						{
							if ((cr.ChildColumnRef.Object == column) || (cr.ParentColumnRef.Object == column))
								removeRelationship = true;
						}

						if (removeRelationship)
						{
							string objectName = "FK_" +
														parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
														"_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);

							//sb.AppendLine("--MARK 1");
							sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
							sb.AppendLine("ALTER TABLE [" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
							sb.AppendLine();
						}
					}
				}

			}
			#endregion

			#region Delete if Primary Key
			bool removePrimaryKey = false;
			foreach (Column c in t.PrimaryKeyColumns)
			{
				if (c == column)
					removePrimaryKey = true;
			}

			if (removePrimaryKey)
			{
				string objectName = "PK_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, t) + "";

				//Delete Primary Key
				//sb.AppendLine("--MARK 5");
				sb.AppendLine("--PRIMARY KEY FOR TABLE [" + t.DatabaseName + "]");
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
				sb.AppendLine("ALTER TABLE [" + t.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
				sb.AppendLine();
			}
			#endregion

			#region Delete Indexes
			foreach (Reference reference in t.Columns)
			{
				Column c = (Column)reference.Object;
				if (string.Compare(column.DatabaseName, c.DatabaseName, true) == 0)
				{
					string indexName = "IDX_" + t.Name.Replace("-", "") + "_" + c.Name.Replace("-", "");
					indexName = indexName.ToUpper();
					sb.AppendLine("if exists (select * from dbo.sysindexes where name = '" + indexName + "')");
					sb.AppendLine("DROP INDEX [" + indexName + "] ON [" + t.DatabaseName + "]");
					sb.AppendLine();
				}
			}
			#endregion

			#region Delete actual column
			sb.AppendLine("if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + column.DatabaseName + "' and o.name = '" + t.DatabaseName + "')");
			sb.AppendLine("ALTER TABLE [" + t.DatabaseName + "] DROP COLUMN [" + column.DatabaseName + "]");
			#endregion

			return sb.ToString();

		}

		public static string GetSQLModifyColumn(Column newColumn)
		{
			StringBuilder sb = new StringBuilder();
			Table table = ((Table)newColumn.ParentTableRef.Object);
			ModelRoot model = (ModelRoot)newColumn.Root;

			#region Delete Indexes
			string indexName = "IDX_" + table.Name.Replace("-", "") + "_" + newColumn.Name.Replace("-", "");
			indexName = indexName.ToUpper();
			sb.AppendLine("if exists (select * from dbo.sysindexes where name = '" + indexName + "')");
			sb.AppendLine("DROP INDEX [" + indexName + "] ON [" + table.DatabaseName + "]");
			sb.AppendLine();
			#endregion

			#region Delete Defaults
			sb.Append("select 'ALTER TABLE [" + table.DatabaseName + "] DROP CONSTRAINT ' + [name] as 'sql' ");
			sb.Append("into #t ");
			sb.Append("from sysobjects ");
			sb.Append("where id IN( ");
			sb.Append("select sc.cdefault ");
			sb.Append("FROM dbo.sysobjects SO INNER JOIN dbo.syscolumns SC ON SO.id = SC.id ");
			sb.Append("LEFT JOIN dbo.syscomments SM ON SC.cdefault = SM.id ");
			sb.Append("WHERE SO.xtype = 'U' and SO.NAME = '" + table.DatabaseName + "' and SC.NAME = '" + newColumn.DatabaseName + "' ");
			sb.AppendLine(")");
			sb.AppendLine("declare @sql nvarchar (1000)");
			sb.AppendLine("SELECT @sql = MAX([sql]) from #t");
			sb.AppendLine("exec (@sql)");
			sb.AppendLine("drop table #t");
			#endregion

			sb.AppendLine("if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + newColumn.DatabaseName + "' and o.name = '" + table.DatabaseName + "')");
			sb.AppendLine("BEGIN");
			sb.AppendLine("if exists (select * from dbo.sysindexes where name = '" + indexName + "')");
			sb.AppendLine("DROP INDEX [" + indexName + "] ON [" + table.DatabaseName + "]");
			sb.AppendLine(AppendColumnDefaultRemoveSQL(newColumn));
			sb.AppendLine("ALTER TABLE [" + table.DatabaseName + "] ALTER COLUMN " + AppendColumnDefinition(newColumn, false, false));
			sb.AppendLine("END");

			sb.Append(AppendColumnDefaultSQL(newColumn));

			return sb.ToString();
		}

		public static string GetSQLDropTable(Table t)
		{
			StringBuilder sb = new StringBuilder();

			string objectName = "PK_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, t);

			#region Delete Parent Relations
			for (int ii = t.ParentRoleRelations.Count - 1; ii >= 0; ii--)
			{
				Relation parentR = (Relation)t.ParentRoleRelations[ii];
				Table parentT = (Table)parentR.ParentTableRef.Object;
				Table childT = (Table)parentR.ChildTableRef.Object;
				for (int jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
				{
					//Relation chlidR = (Relation)parentT.ParentRoleRelations[jj];
					if (parentR.ParentTableRef.Object == t)
					{
						objectName = "FK_" +
							parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
							"_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);

						//sb.AppendLine("--MARK 3");
						sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
						sb.AppendLine("ALTER TABLE [" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
						sb.AppendLine();
					}
				}
			}
			#endregion

			#region Delete Child Relations
			for (int ii = t.ChildRoleRelations.Count - 1; ii >= 0; ii--)
			{
				Relation childR = (Relation)t.ChildRoleRelations[ii];
				Table parentT = (Table)childR.ParentTableRef.Object;
				Table childT = (Table)childR.ChildTableRef.Object;
				for (int jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
				{
					Relation parentR = (Relation)parentT.ParentRoleRelations[jj];
					if (parentR.ChildTableRef.Object == t)
					{
						objectName = "FK_" +
							parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
							"_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);

						//sb.AppendLine("--MARK 4");
						sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
						sb.AppendLine("ALTER TABLE [" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
						sb.AppendLine();
					}
				}

			}
			#endregion

			#region Delete Primary Key
			//sb.AppendLine("--MARK 2");
			sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
			sb.AppendLine("ALTER TABLE [" + t.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
			sb.AppendLine();
			#endregion

			#region Delete Indexes
			foreach (Reference reference in t.Columns)
			{
				Column c = (Column)reference.Object;
				string indexName = "IDX_" + t.Name.Replace("-", "") + "_" + c.Name.Replace("-", "");
				indexName = indexName.ToUpper();
				sb.AppendLine("if exists (select * from dbo.sysindexes where name = '" + indexName + "')");
				sb.AppendLine("DROP INDEX [" + indexName + "] ON [" + t.DatabaseName + "]");
				sb.AppendLine();
			}
			#endregion

			//Drop the actual table
			sb.AppendLine("if exists (select * from sysobjects where name = '" + t.DatabaseName + "' and xtype = 'U')");
			sb.AppendLine("DROP TABLE [" + t.DatabaseName + "]");

			return sb.ToString();
		}

		public static string GetSQLInsertStaticData(Table table)
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				ModelRoot model = (ModelRoot)table.Root;

				//Generate static data
				if (table.StaticData.Count > 0)
				{
					bool isIdentity = false;
					foreach (Column column in table.PrimaryKeyColumns)
						isIdentity |= (column.Identity == IdentityTypeConstants.Database);

					sb.AppendLine("--INSERT STATIC DATA FOR TABLE [" + Globals.GetTableDatabaseName(model, table) + "]");
					if (isIdentity)
						sb.AppendLine("SET identity_insert [" + Globals.GetTableDatabaseName(model, table) + "] on");

					foreach (RowEntry rowEntry in table.StaticData)
					{
						string fieldList = "";
						string valueList = "";
						foreach (CellEntry cellEntry in rowEntry.CellEntries)
						{
							Column column = ((Column)cellEntry.ColumnRef.Object);
							fieldList += "[" + column.Name + "],";

							string sqlValue = cellEntry.GetSQLData();
							if (sqlValue == "")
							{
								if (column.AllowNull)
								{
									valueList += sqlValue + "NULL,";
								}
								else if (column.Default != "")
								{
									if (ModelHelper.IsTextType(column.DataType)) valueList += sqlValue + "'" + column.Default.Replace("'", "''") + "',";
									else valueList += sqlValue + column.Default + ",";
								}
								else
								{
									if (ModelHelper.IsTextType(column.DataType)) valueList += sqlValue + "'',";
									else valueList += sqlValue + "0,";
								}
							}
							else
							{
								valueList += sqlValue + ",";
							}
						}

						if (fieldList.EndsWith(","))
							fieldList = fieldList.Substring(0, fieldList.Length - 1);
						if (valueList.EndsWith(","))
							valueList = valueList.Substring(0, valueList.Length - 1);

						sb.Append("if not exists(select * from [" + Globals.GetTableDatabaseName(model, table) + "] where ");
						foreach(Column column in table.PrimaryKeyColumns)
						{
							string pkData = rowEntry.CellEntries[column.Name].GetSQLData();
							sb.Append("([" + column.DatabaseName + "] = " + pkData + ")");
							if (table.PrimaryKeyColumns.IndexOf(column) < table.PrimaryKeyColumns.Count - 1)
								sb.Append(" AND ");
						}
						sb.Append(") ");
						sb.AppendLine("INSERT INTO [" + Globals.GetTableDatabaseName(model, table) + "] (" + fieldList + ") values (" + valueList + ")");

						//if (table.PrimaryKeyColumns.Count == 1)
						//{
						//  Column pkColumn = ((Column)table.PrimaryKeyColumns[0]);
						//  string pkData = rowEntry.CellEntries[pkColumn.Name].GetSQLData();

						//  sb.Append("if not exists(select * from [" + Globals.GetTableDatabaseName(model, table) + "] where [" + pkColumn.DatabaseName + "] = " + pkData + ") ");

						//  sb.AppendLine("INSERT INTO [" + Globals.GetTableDatabaseName(model, table) + "] (" + fieldList + ") values (" + valueList + ")");
						//}
						//else
						//{
						//  //Fallback is to just insert
						//  sb.AppendLine("INSERT INTO [" + Globals.GetTableDatabaseName(model, table) + "] (" + fieldList + ") values (" + valueList + ")");
						//}

					}

					if (isIdentity)
						sb.AppendLine("SET identity_insert [" + Globals.GetTableDatabaseName(model, table) + "] off");

					sb.AppendLine();
				}

				return sb.ToString();
			}
			catch (Exception ex)
			{				
				throw;
			}
			
		}

		//public static string GetSQLUpdateStaticData(Table table)
		//{
		//  StringBuilder sb = new StringBuilder();
		//  ModelRoot model = (ModelRoot)table.Root;

		//  //Generate static data
		//  if (table.StaticData.Count > 0)
		//  {
		//    string tableName = Globals.GetTableDatabaseName(model, table);
		//    Column pkColumn = ((Column)table.PrimaryKeyColumns[0]);
		//    sb.AppendLine("--UPDATE STATIC DATA FOR TABLE [" + tableName + "]");				
		//    string idList = "";
		//    foreach (RowEntry rowEntry in table.StaticData)
		//    {
		//      string valueList = "";
		//      foreach (CellEntry cellEntry in rowEntry.CellEntries)
		//      {
		//        Column column = ((Column)cellEntry.ColumnRef.Object);
		//        if (!table.PrimaryKeyColumns.Contains(column))
		//          valueList += "[" + column.Name + "] = " + cellEntry.GetSQLData() + ",";
		//      }
		//      if (valueList.EndsWith(",")) valueList = valueList.Substring(0, valueList.Length - 1);

		//      if ((table.PrimaryKeyColumns.Count == 1) && (valueList != ""))
		//      {
		//        string pkData = rowEntry.CellEntries[pkColumn.Name].GetSQLData();
		//        idList += pkData + ",";
		//        sb.AppendLine("UPDATE " + tableName + " SET " + valueList + " WHERE [" + pkColumn.DatabaseName + "] = " + pkData);
		//      }

		//    }

		//    sb.AppendLine();

		//    //Strip off comma
		//    if (idList.EndsWith(","))
		//      idList = idList.Substring(0, idList.Length - 1);

		//    if (idList != null)
		//    {
		//      //Delete all non-included items
		//      sb.AppendLine("DELETE FROM [" + tableName + "] WHERE NOT ([" + pkColumn.DatabaseName + "] IN (" + idList + "))");
		//      sb.AppendLine();
		//    }

		//  }

		//  return sb.ToString();

		//}

		public static string AppendColumnDefaultSQL(Column column)
		{
			StringBuilder sb = new StringBuilder();
			if (column.Default != string.Empty)
			{
				//We know a default was specified so render the SQL				
				string defaultClause = GetDefaultValueClause(column);
				Table table = (Table)column.ParentTableRef.Object;
				string defaultName = "DF__" + table.DatabaseName + "_" + column.DatabaseName + "";
				defaultName = defaultName.ToUpper();
				if (defaultClause != "")
				{
					sb.AppendLine("if not exists(select * from sysobjects where name = '" + defaultName + "' and xtype = 'D')");
					sb.Append("ALTER TABLE [" + table.DatabaseName + "] ");
					sb.Append("ADD " + defaultClause);
					sb.AppendLine(" FOR [" + column.DatabaseName + "]");
				}
			}
			return sb.ToString();
		}

		public static string AppendColumnDefaultRemoveSQL(Column column)
		{
			StringBuilder sb = new StringBuilder();
			if (column.Default != string.Empty)
			{
				//We know a default was specified so render the SQL				
				Table table = (Table)column.ParentTableRef.Object;
				string defaultName = "DF__" + table.DatabaseName + "_" + column.DatabaseName + "";
				defaultName = defaultName.ToUpper();
				sb.AppendLine("if exists(select * from sysobjects where name = '" + defaultName + "' and xtype = 'D')");
				sb.Append("ALTER TABLE [" + table.DatabaseName + "] ");
				sb.Append("DROP CONSTRAINT [" + defaultName + "] ");
			}
			return sb.ToString();
		}

		#region Private Methods

		private static string AppendColumnDefinition(Column column, bool allowDefault, bool allowIdentity)
		{
			return AppendColumnDefinition(column, allowDefault, allowIdentity, true);
		}

		private static string AppendColumnDefinition(Column column, bool allowDefault, bool allowIdentity, bool allowNull)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("[" + column.DatabaseName + "] [" + column.DataType.ToString() + "]");
			if (ModelHelper.VariableLengthType(column.DataType))
			{
				if (column.DataType == SqlDbType.Decimal)
					sb.Append(" (" + column.Length + ", 4)");
				else
					sb.Append(" (" + column.GetLengthString() + ")");
			}
			if (allowIdentity && (column.Identity == IdentityTypeConstants.Database))
			{
				sb.Append(" IDENTITY (1, 1)");
			}
			if (allowNull && !column.AllowNull)
			{
				sb.Append(" NOT");
			}
			sb.Append(" NULL");

			if (allowDefault)
				sb.Append(" " + GetDefaultValueClause(column));

			return sb.ToString();

		}

		private static string GetDefaultValueClause(Column column)
		{			
			StringBuilder sb = new StringBuilder();
			if (column.Default != string.Empty)
			{
				//We know that something was typed in so create the default clause
				Table table = (Table)column.ParentTableRef.Object;
				string defaultName = "DF__" + table.DatabaseName + "_" + column.DatabaseName;
				defaultName = defaultName.ToUpper();
				sb.Append(" CONSTRAINT [" + defaultName + "] ");

				bool failed = false;
				StringBuilder tempBuilder = new StringBuilder();

				tempBuilder.Append("DEFAULT (");
				if ((column.DataType == System.Data.SqlDbType.DateTime) || (column.DataType == System.Data.SqlDbType.SmallDateTime))
				{
					if (column.Default == "getdate")
					{
						tempBuilder.Append(column.Default + "()");
					}
					else if (column.Default.StartsWith("getdate+"))
					{
						string t = column.Default.Substring(8, column.Default.Length - 8);
						string[] tarr = t.Split('-');
						if (tarr.Length == 2)
						{
							if (tarr[1] == "day")
								tempBuilder.Append("DATEADD(DAY, " + tarr[0] + ", getdate())");
							else if (tarr[1] == "month")
								tempBuilder.Append("DATEADD(MONTH, " + tarr[0] + ", getdate())");
							else if (tarr[1] == "year")
								tempBuilder.Append("DATEADD(YEAR, " + tarr[0] + ", getdate())");
						}
					}
					else failed = true;
				}
				else
				{
					if (column.DataType == SqlDbType.UniqueIdentifier)
					{
						if (column.Default.ToLower() == "newid")
						{
							tempBuilder.Append(GetDefaultValue(column.Default));
						}
						else
						{
							string v = GetDefaultValue(column.Default.Replace("'", ""));
							if (v.Length == 36) v = "'" + v + "'";
							tempBuilder.Append(v); //in case they put quotes around it
						}
					}
					else
					{
						if (ModelHelper.DefaultIsString(column.DataType)) tempBuilder.Append("'");
						tempBuilder.Append(GetDefaultValue(column.Default));
						if (ModelHelper.DefaultIsString(column.DataType)) tempBuilder.Append("'");
					}					
				}
				tempBuilder.Append(")");

				//Append default if no errors
				if (!failed) sb.Append(tempBuilder.ToString());

			}
			return sb.ToString();

		}

		private static void AppendTimestamp(ModelRoot model, Table table, StringBuilder sb)
		{
			if (table.AllowTimestamp)
			{
				sb.AppendLine(",");
				sb.Append("[" + model.Database.TimestampColumnName + "] [timestamp] NOT NULL ");
			}
		}

		private static void AppendCreateAudit(ModelRoot model, Table table, StringBuilder sb)
		{
			if (table.AllowCreateAudit)
			{
				string defaultName = "DF__" + table.DatabaseName + "_" + model.Database.CreatedDateColumnName;
				defaultName = defaultName.ToUpper();
				sb.AppendLine(",");
				sb.Append("[" + model.Database.CreatedByColumnName + "] [varchar] (50) NULL ");
				sb.AppendLine(",");
				sb.Append("[" + model.Database.CreatedDateColumnName + "] [datetime] CONSTRAINT [" + defaultName + "] DEFAULT " + (model.UseUTCTime ? "GetUTCDate()" : "GetDate()") + " NULL ");
			}
		}

		private static void AppendModifiedAudit(ModelRoot model, Table table, StringBuilder sb)
		{
			if (table.AllowModifiedAudit)
			{
				string defaultName = "DF__" + table.DatabaseName + "_" + model.Database.ModifiedDateColumnName;
				defaultName = defaultName.ToUpper();
				sb.AppendLine(",");
				sb.Append("[" + model.Database.ModifiedByColumnName + "] [varchar] (50) NULL ");
				sb.AppendLine(",");
				sb.Append("[" + model.Database.ModifiedDateColumnName + "] [datetime] CONSTRAINT [" + defaultName + "] DEFAULT " + (model.UseUTCTime ? "GetUTCDate()" : "GetDate()") + " NULL ");
			}
		}

		private static string GetDefaultValue(string modelDefault)
		{
			string retVal = modelDefault;
			if (StringHelper.Match(modelDefault, "newid"))
			{
				retVal = "NewId()";
			}
			else if (StringHelper.Match(modelDefault, "getdate"))
			{
				retVal = "GetDate()";
			}
			else if (StringHelper.Match(modelDefault, "getutcdate"))
			{
				retVal = "GetUTCDate()";
			}
			else if ((modelDefault == "''") || (modelDefault == "\"\""))
			{
				retVal = "";
			}

			return retVal;
		}

		#endregion

	}
}