#pragma warning disable 0168
using System;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
    internal class SQLSelectAuditBusinessObjectTemplate : ISQLGenerate
    {
        private ModelRoot _model;
        private Table _currentTable;

        #region Constructors
        public SQLSelectAuditBusinessObjectTemplate(ModelRoot model, Table currentTable)
        {
            _model = model;
            _currentTable = currentTable;
        }
        #endregion

        #region GenerateContent

        public void GenerateContent(StringBuilder sb)
        {
            if (!_currentTable.AllowAuditTracking) return;

            try
            {
                this.AppendFullTemplate(sb);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        #endregion

        private void AppendFullTemplate(StringBuilder sb)
        {
            this.AppendQuerySpecificRecords(sb);
            this.AppendQueryAllRecords(sb);
        }

        private void AppendQuerySpecificRecords(StringBuilder sb)
        {
            var storedProcName = _model.GetStoredProcedurePrefix() + "_" + _currentTable.PascalName + "__AUDIT_SELECT";

            try
            {
                sb.AppendLine("if exists(select * from sys.objects where name = '" + storedProcName + "' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')");
                sb.AppendLine("	drop procedure [" + _currentTable.GetSQLSchema() + "].[" + storedProcName + "]");
                sb.AppendLine("GO");
                sb.AppendLine();

                sb.AppendLine("CREATE PROCEDURE [" + _currentTable.GetSQLSchema() + "].[" + storedProcName + "]");
                sb.AppendLine("(");

                foreach (var pk in _currentTable.PrimaryKeyColumns)
                {
                    sb.AppendLine(" @" + pk.DatabaseName + " " + pk.GetSQLDefaultType() + ",");
                }

                sb.AppendLine(" @__auditType [int] = null,");
                sb.AppendLine(" @__pageOffset [int] = 1,");
                sb.AppendLine(" @__recordsPerPage [int] = null,");
                sb.AppendLine(" @__startDate [datetime] = null,");
                sb.AppendLine(" @__endDate [datetime] = null");
                sb.AppendLine(")");
                sb.AppendLine("AS");
                sb.AppendLine();
                sb.AppendLine("SET NOCOUNT ON;");
                sb.AppendLine();

                //Get all the columns for this table
                var columnList = _currentTable.GetColumns().Where(x => x.Generated &
                    x.DataType != System.Data.SqlDbType.Image &&
                    x.DataType != System.Data.SqlDbType.Text &&
                    x.DataType != System.Data.SqlDbType.NText
                    ).ToList();

                sb.AppendLine("if(@__pageOffset < 1 ) SET @__pageOffset = 1");
                sb.AppendLine("if((NOT (@__recordsPerPage IS NULL)) AND (@__recordsPerPage < 1 )) SET @__recordsPerPage = 1");

                sb.AppendLine("IF (@__recordsPerPage IS NULL)");
                sb.AppendLine("BEGIN");
                #region All Records
                sb.AppendLine("SELECT");

                sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "].[__action],");
                sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "].[__insertdate],");
                if (_currentTable.AllowModifiedAudit)
                {
                    sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "].[" + _model.Database.ModifiedByDatabaseName + "],");
                }

                var index = 0;
                foreach (var column in columnList)
                {
                    sb.Append("	[__AUDIT__" + _currentTable.DatabaseName + "].[" + column.DatabaseName + "]");
                    if (index < columnList.Count - 1) sb.AppendLine(",");
                    else sb.AppendLine();
                    index++;
                }

                sb.AppendLine("FROM");
                sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "]");

                sb.AppendLine("WHERE");
                sb.AppendLine("	((@__auditType IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__action = @__auditType)) AND");
                sb.AppendLine("	((@__startDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate >= @__startDate)) AND");
                sb.AppendLine("	((@__endDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate < @__endDate)) AND");

                index = 0;
                foreach (var pk in _currentTable.PrimaryKeyColumns)
                {
                    sb.Append("	(@" + pk.DatabaseName + " = [__AUDIT__" + _currentTable.DatabaseName + "]." + pk.DatabaseName + ")");
                    if (index < _currentTable.PrimaryKeyColumns.Count - 1) sb.AppendLine(" AND");
                    else sb.AppendLine();
                    index++;
                }

                sb.AppendLine("	ORDER BY [__AUDIT__" + _currentTable.DatabaseName + "].__insertdate DESC, [__AUDIT__" + _currentTable.DatabaseName + "].__rowid DESC");
                sb.AppendLine();
                #endregion
                sb.AppendLine("END");
                sb.AppendLine("ELSE");
                sb.AppendLine("BEGIN");
                #region Paginated SQL
                sb.AppendLine("SELECT");

                sb.AppendLine("	[__TempTable].[__action],");
                sb.AppendLine("	[__TempTable].[__insertdate],");
                if (_currentTable.AllowModifiedAudit)
                {
                    sb.AppendLine("	[__TempTable].[" + _model.Database.ModifiedByDatabaseName + "],");
                }

                index = 0;
                foreach (var column in columnList)
                {
                    sb.Append("	[__TempTable].[" + column.DatabaseName + "]");
                    if (index < columnList.Count - 1) sb.AppendLine(",");
                    else sb.AppendLine();
                    index++;
                }

                sb.AppendLine("FROM");
                sb.AppendLine("	(SELECT  ROW_NUMBER() OVER (ORDER BY [__AUDIT__" + _currentTable.DatabaseName + "].__insertdate DESC, [__AUDIT__" + _currentTable.DatabaseName + "].__rowid DESC)");
                sb.AppendLine("	AS __row,");

                sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "].[__action],");
                sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "].[__insertdate],");
                if (_currentTable.AllowModifiedAudit)
                {
                    sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "]." + _model.Database.ModifiedByDatabaseName + ",");
                }

                index = 0;
                foreach (var column in columnList)
                {
                    sb.Append("	[__AUDIT__" + _currentTable.DatabaseName + "].[" + column.DatabaseName + "]");
                    if (index < columnList.Count - 1) sb.AppendLine(",");
                    else sb.AppendLine();
                    index++;
                }

                sb.AppendLine("	FROM ");
                sb.AppendLine("		[__AUDIT__" + _currentTable.DatabaseName + "]");
                sb.AppendLine("	WHERE");
                sb.AppendLine("		((@__auditType IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__action = @__auditType)) AND");
                sb.AppendLine("	((@__startDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate >= @__startDate)) AND");
                sb.AppendLine("	((@__endDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate < @__endDate)) AND");

                index = 0;
                foreach (var pk in _currentTable.PrimaryKeyColumns)
                {
                    sb.Append("		(@" + pk.DatabaseName + " = [__AUDIT__" + _currentTable.DatabaseName + "]." + pk.DatabaseName + ")");
                    if (index < _currentTable.PrimaryKeyColumns.Count - 1) sb.AppendLine(" AND");
                    else sb.AppendLine();
                    index++;
                }

                sb.AppendLine("	)");
                sb.AppendLine("	AS [__TempTable]");
                sb.AppendLine("WHERE");
                sb.AppendLine("	__row >= (((@__pageOffset-1)*@__recordsPerPage)+1) AND __row <= (@__pageOffset*@__recordsPerPage)");
                sb.AppendLine();
                #endregion
                sb.AppendLine("END");

                //Return value
                sb.AppendLine("return (select COUNT(*) ");
                sb.AppendLine("	FROM [__AUDIT__" + _currentTable.DatabaseName + "] WHERE");
                sb.AppendLine("		((@__auditType IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__action = @__auditType)) AND");
                sb.AppendLine("	((@__startDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate >= @__startDate)) AND");
                sb.AppendLine("	((@__endDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate < @__endDate)) AND");

                index = 0;
                foreach (var pk in _currentTable.PrimaryKeyColumns)
                {
                    sb.Append("		(@" + pk.DatabaseName + " = [__AUDIT__" + _currentTable.DatabaseName + "]." + pk.DatabaseName + ")");
                    if (index < _currentTable.PrimaryKeyColumns.Count - 1) sb.AppendLine(" AND");
                    else sb.AppendLine();
                    index++;
                }

                sb.AppendLine("		)");
                sb.AppendLine();

                sb.AppendLine("GO");
                sb.AppendLine();
                if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
                {
                    sb.AppendFormat("GRANT EXECUTE ON [" + _currentTable.GetSQLSchema() + "].[{0}] TO [{1}]", storedProcName, _model.Database.GrantExecUser).AppendLine();
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void AppendQueryAllRecords(StringBuilder sb)
        {
            var storedProcName = _model.GetStoredProcedurePrefix() + "_" + _currentTable.PascalName + "__AUDIT_SELECT_ALL";
            try
            {
                sb.AppendLine("if exists(select * from sys.objects where name = '" + storedProcName + "' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')");
                sb.AppendLine("	drop procedure [" + _currentTable.GetSQLSchema() + "].[" + storedProcName + "]");
                sb.AppendLine("GO");
                sb.AppendLine();
                sb.AppendLine("CREATE PROCEDURE [" + _currentTable.GetSQLSchema() + "].[" + storedProcName + "]");
                sb.AppendLine("(");

                sb.AppendLine(" @__auditType [int] = null,");
                sb.AppendLine(" @__pageOffset [int] = 1,");
                sb.AppendLine(" @__recordsPerPage [int] = null,");
                sb.AppendLine(" @__startDate [datetime] = null,");
                sb.AppendLine(" @__endDate [datetime] = null");
                sb.AppendLine(")");
                sb.AppendLine("AS");
                sb.AppendLine();
                sb.AppendLine("SET NOCOUNT ON;");
                sb.AppendLine();

                //Get all the columns for this table
                var columnList = _currentTable.GetColumns().Where(x => x.Generated &
                    x.DataType != System.Data.SqlDbType.Image &&
                    x.DataType != System.Data.SqlDbType.Text &&
                    x.DataType != System.Data.SqlDbType.NText
                    ).ToList();

                sb.AppendLine("if(@__pageOffset < 1 ) SET @__pageOffset = 1");
                sb.AppendLine("if((NOT (@__recordsPerPage IS NULL)) AND (@__recordsPerPage < 1 )) SET @__recordsPerPage = 1");

                sb.AppendLine("IF (@__recordsPerPage IS NULL)");
                sb.AppendLine("BEGIN");
                #region All Records
                sb.AppendLine("SELECT");

                sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "].[__action],");
                sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "].[__insertdate],");
                if (_currentTable.AllowModifiedAudit)
                {
                    sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "].[" + _model.Database.ModifiedByDatabaseName + "],");
                }

                var index = 0;
                foreach (var column in columnList)
                {
                    sb.Append("	[__AUDIT__" + _currentTable.DatabaseName + "].[" + column.DatabaseName + "]");
                    if (index < columnList.Count - 1) sb.AppendLine(",");
                    else sb.AppendLine();
                    index++;
                }

                sb.AppendLine("FROM");
                sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "]");

                sb.AppendLine("WHERE");
                sb.AppendLine("	((@__auditType IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__action = @__auditType)) AND");
                sb.AppendLine("	((@__startDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate >= @__startDate)) AND");
                sb.AppendLine("	((@__endDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate < @__endDate))");

                sb.AppendLine("	ORDER BY [__AUDIT__" + _currentTable.DatabaseName + "].__insertdate DESC, [__AUDIT__" + _currentTable.DatabaseName + "].__rowid DESC");
                sb.AppendLine();
                #endregion
                sb.AppendLine("END");
                sb.AppendLine("ELSE");
                sb.AppendLine("BEGIN");
                #region Paginated SQL
                sb.AppendLine("SELECT");

                sb.AppendLine("	[__TempTable].[__action],");
                sb.AppendLine("	[__TempTable].[__insertdate],");
                if (_currentTable.AllowModifiedAudit)
                {
                    sb.AppendLine("	[__TempTable].[" + _model.Database.ModifiedByDatabaseName + "],");
                }

                index = 0;
                foreach (var column in columnList)
                {
                    sb.Append("	[__TempTable].[" + column.DatabaseName + "]");
                    if (index < columnList.Count - 1) sb.AppendLine(",");
                    else sb.AppendLine();
                    index++;
                }

                sb.AppendLine("FROM");
                sb.AppendLine("	(SELECT  ROW_NUMBER() OVER (ORDER BY [__AUDIT__" + _currentTable.DatabaseName + "].__insertdate DESC, [__AUDIT__" + _currentTable.DatabaseName + "].__rowid DESC)");
                sb.AppendLine("	AS __row,");

                sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "].[__action],");
                sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "].[__insertdate],");
                if (_currentTable.AllowModifiedAudit)
                {
                    sb.AppendLine("	[__AUDIT__" + _currentTable.DatabaseName + "]." + _model.Database.ModifiedByDatabaseName + ",");
                }

                index = 0;
                foreach (var column in columnList)
                {
                    sb.Append("	[__AUDIT__" + _currentTable.DatabaseName + "].[" + column.DatabaseName + "]");
                    if (index < columnList.Count - 1) sb.AppendLine(",");
                    else sb.AppendLine();
                    index++;
                }

                sb.AppendLine("	FROM ");
                sb.AppendLine("		[__AUDIT__" + _currentTable.DatabaseName + "]");
                sb.AppendLine("	WHERE");
                sb.AppendLine("		((@__auditType IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__action = @__auditType)) AND");
                sb.AppendLine("	((@__startDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate >= @__startDate)) AND");
                sb.AppendLine("	((@__endDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate < @__endDate))");

                sb.AppendLine("	)");
                sb.AppendLine("	AS [__TempTable]");
                sb.AppendLine("WHERE");
                sb.AppendLine("	__row >= (((@__pageOffset-1)*@__recordsPerPage)+1) AND __row <= (@__pageOffset*@__recordsPerPage)");
                sb.AppendLine();
                #endregion
                sb.AppendLine("END");

                //Return value
                sb.AppendLine("return (select COUNT(*) ");
                sb.AppendLine("	FROM [__AUDIT__" + _currentTable.DatabaseName + "] WHERE");
                sb.AppendLine("		((@__auditType IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__action = @__auditType)) AND");
                sb.AppendLine("	((@__startDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate >= @__startDate)) AND");
                sb.AppendLine("	((@__endDate IS NULL) OR ([__AUDIT__" + _currentTable.DatabaseName + "].__insertdate < @__endDate))");

                sb.AppendLine("		)");
                sb.AppendLine();

                sb.AppendLine("GO");
                sb.AppendLine();
                if(_model.Database.GrantExecUser != string.Empty)
                {
                    sb.AppendFormat("GRANT EXECUTE ON [" + _currentTable.GetSQLSchema() + "].[{0}] TO [{1}]", storedProcName, _model.Database.GrantExecUser).AppendLine();
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

            }
            catch(Exception ex)
            {
                throw;
            }

        }

    }
}
