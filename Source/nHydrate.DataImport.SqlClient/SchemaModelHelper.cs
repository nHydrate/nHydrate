#pragma warning disable 0168
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace nHydrate.DataImport.SqlClient
{
    public class SchemaModelHelper
    {
        #region Public Methods

        public bool IsValidConnectionString(string connectionString)
        {
            var valid = false;
            var conn = new System.Data.SqlClient.SqlConnection();
            try
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                valid = true;
            }
            catch (Exception ex)
            {
                valid = false;
            }
            finally
            {
                conn.Close();
            }
            return valid;
        }

        #endregion

        internal static string GetSqlDatabaseTables()
        {
            var sb = new StringBuilder();
            sb.AppendLine("DECLARE @bar varchar(150)");
            sb.AppendLine("DECLARE @val varchar(150)");
            sb.AppendLine("DECLARE @tab table");
            sb.AppendLine("(");
            sb.AppendLine("xName varchar(150) NOT NULL,");
            sb.AppendLine("xValue varchar(150) NULL,");
            sb.AppendLine("xSchema varchar(150) NOT NULL");
            sb.AppendLine(")");
            sb.AppendLine("INSERT INTO @tab SELECT so.name, null, sc.name [schema] FROM sys.tables so INNER JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name <> 'dtproperties' AND (so.name <> 'sysdiagrams') AND (so.name <> '__nhydrateschema')AND (so.name <> '__nhydrateobjects') AND NOT (so.name like '__AUDIT__%')");
            sb.AppendLine("select xName as name, xSchema as [schema], xValue selectionCriteria from @tab WHERE xName <> 'dtproperties' ORDER BY xName");
            return sb.ToString();
        }

        internal static string GetSqlColumnsForTable()
        {
            return GetSqlColumnsForTable(null);
        }

        internal static string GetSqlForUniqueConstraints()
        {
            var sb = new StringBuilder();
            sb.AppendLine("select o.name as TableName, col.name as ColumnName");
            sb.AppendLine("from sys.indexes i inner join sys.objects o on i.object_id = o.object_id ");
            sb.AppendLine("	inner join sys.index_columns ic on i.index_id = ic.index_id and ic.object_id = o.object_id");
            sb.AppendLine("	INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id");
            sb.AppendLine("where i.is_unique = 1 and is_primary_key = 0 and is_unique_constraint = 1");
            return sb.ToString();
        }

        internal static string GetSqlForIndexes()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("SELECT ");
            sb.AppendLine("     ind.name as indexname");
            sb.AppendLine("     ,ind.is_primary_key");
            sb.AppendLine("    --,ind.index_id ");
            sb.AppendLine("    --,ic.index_column_id ");
            sb.AppendLine("    ,t.name as tablename");
            sb.AppendLine("    ,col.name as columnname");
            sb.AppendLine("    ,ic.is_descending_key");
            sb.AppendLine("    ,ic.key_ordinal");
            sb.AppendLine("    ,ind.type_desc");
            sb.AppendLine("    ,ind.is_unique_constraint");
            sb.AppendLine("    ,ind.is_primary_key");
            sb.AppendLine("    --,ic.* ");
            sb.AppendLine("    --,col.* ");
            sb.AppendLine("    --,ind.* ");
            sb.AppendLine("FROM sys.indexes ind ");
            sb.AppendLine("INNER JOIN sys.index_columns ic ");
            sb.AppendLine("    ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id ");
            sb.AppendLine("INNER JOIN sys.columns col ");
            sb.AppendLine("    ON ic.object_id = col.object_id and ic.column_id = col.column_id ");
            sb.AppendLine("INNER JOIN sys.tables t ");
            sb.AppendLine("    ON ind.object_id = t.object_id ");
            sb.AppendLine("WHERE (1=1) ");
            //sb.AppendLine("    AND ind.is_primary_key = 0 ");
            //sb.AppendLine("    AND ind.is_unique = 0 ");
            //sb.AppendLine("    AND ind.is_unique_constraint = 0 ");
            sb.AppendLine("    AND t.is_ms_shipped = 0 ");
            sb.AppendLine("    AND ic.key_ordinal <> 0");
            sb.AppendLine("ORDER BY ");
            sb.AppendLine("    ind.name, ic.key_ordinal");
            return sb.ToString();
        }

        internal static string GetSqlColumnsForComputed()
        {
            var sb = new StringBuilder();
            sb.AppendLine("select o.name as tablename, c.name as columnname, c.definition");
            sb.AppendLine("from sys.computed_columns c inner join sys.objects o on c.object_id = o.object_id");
            return sb.ToString();
        }

        internal static string GetSqlColumnsForTable(string tableName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine(" 	c.ORDINAL_POSITION as colorder,");
            sb.AppendLine(" 	c.TABLE_NAME as tablename,");
            sb.AppendLine(" 	c.COLUMN_NAME as columnname,");
            sb.AppendLine("(");
            sb.AppendLine("select top 1 c1.name");
            sb.AppendLine("from sys.indexes i");
            sb.AppendLine("join sys.objects o ON i.object_id = o.object_id");
            sb.AppendLine("join sys.objects pk ON i.name = pk.name");
            sb.AppendLine("AND pk.parent_object_id = i.object_id");
            sb.AppendLine("AND pk.type = 'PK'");
            sb.AppendLine("join sys.index_columns ik on i.object_id = ik.object_id");
            sb.AppendLine("and i.index_id = ik.index_id");
            sb.AppendLine("join sys.columns c1 ON ik.object_id = c1.object_id");
            sb.AppendLine("AND ik.column_id = c1.column_id");
            sb.AppendLine("AND c1.name = c.COLUMN_NAME");
            sb.AppendLine("where o.name = c.TABLE_NAME");
            sb.AppendLine(") as [isPrimaryKey],");
            sb.AppendLine(" 	case WHEN");
            sb.AppendLine(" 	(");
            sb.AppendLine(" 		SELECT ");
            sb.AppendLine(" 				count(*) ");
            sb.AppendLine(" 			FROM ");
            sb.AppendLine(" 				INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE foreignkeyccu");
            sb.AppendLine(" 				INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS foreignkeytc on foreignkeyccu.CONSTRAINT_NAME = foreignkeytc.CONSTRAINT_NAME AND");
            sb.AppendLine(" 																												foreignkeyccu.CONSTRAINT_SCHEMA = foreignkeytc.CONSTRAINT_SCHEMA AND");
            sb.AppendLine(" 																												foreignkeytc.CONSTRAINT_TYPE = 'FOREIGN KEY'");
            sb.AppendLine(" 			WHERE");
            sb.AppendLine(" 				foreignkeyccu.TABLE_SCHEMA = c.TABLE_SCHEMA AND");
            sb.AppendLine(" 				foreignkeyccu.TABLE_NAME = c.TABLE_NAME AND");
            sb.AppendLine(" 				foreignkeyccu.COLUMN_NAME = c.COLUMN_NAME ");
            sb.AppendLine(" 	) > 0 THEN 'true' ELSE 'false' END as isForeignKey,");
            sb.AppendLine(" 	c.DATA_TYPE as datatype,");
            sb.AppendLine(" 	s.system_type_id,");
            sb.AppendLine(" 	c.numeric_precision AS [precision], c.numeric_scale AS [scale],");
            sb.AppendLine(" 		case when	c.CHARACTER_MAXIMUM_LENGTH is null or c.CHARACTER_MAXIMUM_LENGTH > 8000 then s.max_length else c.CHARACTER_MAXIMUM_LENGTH end as max_length,");
            sb.AppendLine(" 	case when c.IS_NULLABLE = 'No' then 0 else 1 end as allow_null, ");
            sb.AppendLine(" 	case when c.COLUMN_DEFAULT is null then '' else c.COLUMN_DEFAULT end as default_value,");
            sb.AppendLine(" 	case when COLUMNPROPERTY(OBJECT_ID(c.TABLE_SCHEMA+'.'+c.TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 then 1 else 0 end as is_identity,");
            sb.AppendLine(" 	c.COLLATION_NAME AS collation");
            sb.AppendLine(" FROM ");
            sb.AppendLine(" 	INFORMATION_SCHEMA.COLUMNS c ");
            sb.AppendLine(" 	INNER JOIN sys.types s on s.name = c.DATA_TYPE");
            if (!string.IsNullOrEmpty(tableName))
                sb.AppendLine(" WHERE c.TABLE_NAME = '" + tableName + "'");
            sb.AppendLine(" ORDER BY");
            sb.AppendLine(" 	c.TABLE_NAME,");
            sb.AppendLine(" 	c.ORDINAL_POSITION");
            return sb.ToString();
        }

        internal static string GetSqlForRelationships()
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 KCU1.CONSTRAINT_NAME AS 'FK_CONSTRAINT_NAME'");
            sb.AppendLine("	, KCU1.TABLE_NAME AS 'FK_TABLE_NAME'");
            sb.AppendLine("	, KCU1.COLUMN_NAME AS 'FK_COLUMN_NAME' ");
            sb.AppendLine("	, KCU2.TABLE_NAME AS 'UQ_TABLE_NAME'");
            sb.AppendLine("	, KCU2.COLUMN_NAME AS 'UQ_COLUMN_NAME'");
            sb.AppendLine("	, so.object_id");
            sb.AppendLine("FROM ");
            sb.AppendLine("	INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC");
            sb.AppendLine("	JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1");
            sb.AppendLine("		ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG ");
            sb.AppendLine("		AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA");
            sb.AppendLine("		AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME");
            sb.AppendLine("JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2");
            sb.AppendLine("	ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG ");
            sb.AppendLine("		AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA");
            sb.AppendLine("		AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME");
            sb.AppendLine("		AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION");
            sb.AppendLine("JOIN sys.objects so");
            sb.AppendLine("	ON KCU1.CONSTRAINT_NAME = so.name");
            sb.AppendLine("WHERE");
            sb.AppendLine("	so.type = 'F'");
            sb.AppendLine("ORDER BY");
            sb.AppendLine("	KCU1.CONSTRAINT_NAME,");
            sb.AppendLine("	KCU1.ORDINAL_POSITION");
            return sb.ToString();
        }

        internal static string GetSqlIndexesForTable()
        {
            var sb = new StringBuilder();

            sb.AppendLine("select t.name as tablename, i.name as indexname, c.name as columnname, i.is_primary_key");
            sb.AppendLine("from sys.tables t");
            sb.AppendLine("inner join sys.indexes i on i.object_id = t.object_id");
            sb.AppendLine("inner join sys.index_columns ic on ic.object_id = t.object_id");
            sb.AppendLine("inner join sys.columns c on c.object_id = t.object_id and");
            sb.AppendLine("ic.column_id = c.column_id");

            //sb.AppendLine("select o.name as tablename, i.name as indexname, i.is_primary_key from sys.objects o inner join sys.indexes i on o.object_id = i.object_id where o.[type] = 'U'");
            return sb.ToString();
        }

        internal static string GetSqlForViews()
        {
            var sb = new StringBuilder();
            sb.AppendLine("select s.name as schemaname, v.name, m.definition from sys.views v inner join sys.sql_modules m on v.object_id = m.object_id inner join sys.schemas s on s.schema_id= v.schema_id");
            return sb.ToString();
        }

        internal static string GetViewBody(string sql)
        {
            var regEx = new Regex(@"CREATE VIEW[\r\n\s]*[a-zA-Z0-9\[\]_\.]*[\r\n\s]*AS[\r\n\s]*([\s\S\r\n]*)", RegexOptions.IgnoreCase);
            var match = regEx.Match(sql);
            if (match != null && match.Groups != null && match.Groups.Count == 2)
                sql = match.Groups[1].Value;
            else
            {
                sql = sql.Replace("\r", string.Empty);
                var arr = sql.Split('\n').ToList();
                var sb = new StringBuilder();

                var inBody = false;
                foreach (var lineText in arr)
                {
                    //This is FAR from perfect. It assumes the creation line ends with the "AS" keyword for a stored proc
                    if (inBody)
                    {
                        sb.AppendLine(lineText);
                    }
                    else if (!inBody && (lineText.ToLower().Trim().EndsWith(" as") || lineText.ToLower().Trim() == "as"))
                    {
                        inBody = true;
                    }
                }
                sql = sb.ToString();

            }

            return sql.Trim();
        }

        internal static string GetSqlForViewsColumns()
        {
            var sb = new StringBuilder();
            sb.AppendLine("select v.name as viewname, c.name as columnname, c.system_type_id, c.max_length, c.precision, c.scale, c.is_nullable from sys.views v inner join sys.columns c on v.object_id = c.object_id order by v.name, c.name");
            return sb.ToString();
        }

    }
}