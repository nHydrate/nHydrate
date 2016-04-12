#region Copyright (c) 2006-2011 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2011 All Rights reserved              *
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
using nHydrate.Generator.Common.GeneratorFramework;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using nHydrate.Generator.EFCodeFirst;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common;

namespace Widgetsphere.Generator.EFCodeFirst.Generators.EFCSDL
{
    public class AuditEntityGeneratedTemplate : EFCodeFirstBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private Table _item;

        public AuditEntityGeneratedTemplate(ModelRoot model, Table currentTable)
            : base(model)
        {
            _model = model;
            _item = currentTable;
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return string.Format("{0}Audit.Generated.cs", _item.PascalName); }
        }

        public string ParentItemName
        {
            get { return string.Format("{0}Audit.cs", _item.PascalName); }
        }

        public override string FileContent
        {
            get
            {
                try
                {
                    sb = new StringBuilder();
                    this.GenerateContent();
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        #endregion

        #region GenerateContent

        private void GenerateContent()
        {
            try
            {
                nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
                nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Audit");
                sb.AppendLine("{");
                this.AppendEntityClass();
                sb.AppendLine("}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Xml.Serialization;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine();
        }

        private void AppendEntityClass()
        {
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// The object to hold the '" + _item.PascalName + "Audit' entity");
            if (!string.IsNullOrEmpty(_item.Description))
                sb.AppendLine("	/// " + _item.Description);
            sb.AppendLine("	/// </summary>");

            sb.AppendLine("	public partial class " + _item.PascalName + "Audit : " + this.GetLocalNamespace() + ".IAudit, System.IComparable, System.IComparable<" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit>");

            sb.AppendLine("	{");
            this.AppendConstructors();
            this.AppendProperties();
            this.AppendRecordLoader();
            this.AppendCompare();
            sb.AppendLine("	}");
            sb.AppendLine();

            #region Add the AuditResultFieldCompare class
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// ");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	public interface I" + _item.PascalName + "AuditResultFieldCompare : " + this.GetLocalNamespace() + ".IAuditResultFieldCompare");
            sb.AppendLine("	{");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// ");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		new " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants Field { get; }");
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// A comparison class for audit comparison results");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	/// <typeparam name=\"T\"></typeparam>");
            sb.AppendLine("	public class " + _item.PascalName + "AuditResultFieldCompare<T> : " + this.GetLocalNamespace() + ".AuditResultFieldCompare<T, " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants>, I" + _item.PascalName + "AuditResultFieldCompare");
            sb.AppendLine("	{");
            sb.AppendLine("		internal " + _item.PascalName + "AuditResultFieldCompare(T value1, T value2, " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, System.Type dataType)");
            sb.AppendLine("			: base(value1, value2, field, dataType)");
            sb.AppendLine("		{");
            sb.AppendLine("		}");
            sb.AppendLine("	}");
            sb.AppendLine();
            #endregion

        }

        private void AppendConstructors()
        {
            sb.AppendLine("		#region Constructors");
            sb.AppendLine();
            sb.AppendLine("		internal " + _item.PascalName + "Audit()");
            sb.AppendLine("		{");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendProperties()
        {
            sb.AppendLine("		#region Properties");
            sb.AppendLine();

            Dictionary<string, Column> columnList = new Dictionary<string, Column>();
            List<Table> tableList = new List<Table>(new Table[] { _item });

            //This is for inheritance which is NOT supported right now
            //List<Table> tableList = new List<Table>(_currentTable.GetTableHierarchy().Where(x => x.AllowAuditTracking).Reverse());
            foreach (Table table in tableList)
            {
                foreach (Column column in table.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
                    {
                        if (!columnList.ContainsKey(column.Name))
                            columnList.Add(column.Name, column);
                    }
                }
            }

            foreach (Column column in columnList.Values)
            {
                sb.AppendLine("		/// <summary>");
                if (!string.IsNullOrEmpty(column.Description))
                    sb.AppendLine("		/// " + column.Description + "");
                else
                    sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public " + column.GetCodeType() + " " + column.PascalName + " { get; internal set; }");
            }

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The type of audit");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + this.GetLocalNamespace() + ".AuditTypeConstants AuditType { get; internal set; }");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The date of the audit");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public DateTime AuditDate { get; internal set; }");

            //if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The modifier value of the audit");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public string ModifiedBy { get; internal set; }");
                sb.AppendLine();
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendRecordLoader()
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets a set of audit records based on a primary key");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <returns>A set of audit records for the current record based on primary key</returns>");
            sb.Append("		public static IEnumerable<" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit> GetAuditRecords(");

            IEnumerable<Column> columnList = _item.GetColumns().Where(x =>
                x.Generated &&
                x.DataType != System.Data.SqlDbType.Text &&
                x.DataType != System.Data.SqlDbType.NText &&
                x.DataType != System.Data.SqlDbType.Image);

            int index = 0;
            foreach (Column column in _item.PrimaryKeyColumns)
            {
                sb.Append(column.GetCodeType() + " " + column.CamelName);
                if (index < _item.PrimaryKeyColumns.Count - 1)
                    sb.Append(", ");
                index++;
            }

            sb.AppendLine(")");
            sb.AppendLine("		{");
            sb.Append("			return GetAuditRecords(0, 0, null, null, ");

            index = 0;
            foreach (Column column in _item.PrimaryKeyColumns)
            {
                sb.Append(column.CamelName);
                if (index < _item.PrimaryKeyColumns.Count - 1)
                    sb.Append(", ");
                index++;
            }
            sb.AppendLine(").InnerList;");

            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets a set of audit records based on a primary key");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"pageOffset\">The page offset needed for pagination starting from page 1</param>");
            sb.AppendLine("		/// <param name=\"recordsPerPage\">The number of records to be returned on a page.</param>");
            sb.AppendLine("		/// <param name=\"startDate\">The starting date used when searching for records.</param>");
            sb.AppendLine("		/// <param name=\"endDate\">The ending date used when searching for records.</param>");
            foreach (Column column in _item.PrimaryKeyColumns)
            {
                sb.AppendLine("		/// <param name=\"" + column.CamelName + "\">A primary key field to use when searching for records.</param>");
            }
            sb.AppendLine("		/// <returns>A set of audit records for the current record based on primary key</returns>");
            sb.Append("		public static " + this.GetLocalNamespace() + ".AuditPaging<" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit> GetAuditRecords(int pageOffset, int recordsPerPage, DateTime? startDate, DateTime? endDate, ");

            index = 0;
            foreach (Column column in _item.PrimaryKeyColumns)
            {
                sb.Append(column.GetCodeType() + " " + column.CamelName);
                if (index < _item.PrimaryKeyColumns.Count - 1)
                    sb.Append(", ");
                index++;
            }

            sb.AppendLine(")");
            sb.AppendLine("		{");
            sb.AppendLine("			var retval = new " + this.GetLocalNamespace() + ".AuditPaging<" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit>();");
            sb.AppendLine("			retval.PageOffset = pageOffset;");
            sb.AppendLine("			retval.RecordsPerPage = recordsPerPage;");
            sb.AppendLine("			var innerList = new List<" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit>();");
            sb.AppendLine();
            sb.AppendLine("			System.Data.IDataReader dataReader = null;");
            sb.AppendLine("			System.Data.IDbConnection dbConnection = null;");
            sb.AppendLine("			System.Data.IDbCommand dbCommand = null;");
            sb.AppendLine();
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				dbConnection = new System.Data.SqlClient.SqlConnection(" + this.GetLocalNamespace() + "." + _model.ProjectName + "Entities.GetConnectionString());");
            sb.AppendLine("				dbConnection.Open();");
            sb.AppendLine();
            sb.AppendLine("				dbCommand = " + this.GetLocalNamespace() + ".DBHelper.GetCommand(\"[" + _item.GetSQLSchema() + "].[" + _model.GetStoredProcedurePrefix() + "_" + _item.PascalName + "__AUDIT_SELECT]\", CommandType.StoredProcedure, dbConnection);");
            sb.AppendLine("				" + this.GetLocalNamespace() + ".DBHelper.AddParameter(dbCommand, \"@__pageOffset\", pageOffset);");
            sb.AppendLine("				if (pageOffset != 0 && recordsPerPage != 0)");
            sb.AppendLine("					" + this.GetLocalNamespace() + ".DBHelper.AddParameter(dbCommand, \"@__recordsPerPage\", recordsPerPage);");
            sb.AppendLine("				" + this.GetLocalNamespace() + ".DBHelper.AddParameter(dbCommand, \"@__startDate\", startDate);");
            sb.AppendLine("				" + this.GetLocalNamespace() + ".DBHelper.AddParameter(dbCommand, \"@__endDate\", endDate);");

            foreach (Column column in _item.PrimaryKeyColumns)
            {
                sb.AppendLine("				" + this.GetLocalNamespace() + ".DBHelper.AddParameter(dbCommand, \"@" + column.DatabaseName + "\", " + column.CamelName + ");");
            }

            sb.AppendLine("				" + this.GetLocalNamespace() + ".DBHelper.AddReturnParameter(dbCommand);");
            sb.AppendLine("				dataReader = dbCommand.ExecuteReader();");
            sb.AppendLine();
            sb.AppendLine("				// Fill the list box with the values retrieved");
            sb.AppendLine("				int ordinal = 0;");
            sb.AppendLine("				while (dataReader.Read())");
            sb.AppendLine("				{");
            sb.AppendLine("					" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit si = new " + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit();");
            sb.AppendLine();
            sb.AppendLine("					ordinal = dataReader.GetOrdinal(\"__action\");");
            sb.AppendLine("					si.AuditType = (" + this.GetLocalNamespace() + ".AuditTypeConstants)dataReader.GetInt32(ordinal);");
            sb.AppendLine();
            sb.AppendLine("					ordinal = dataReader.GetOrdinal(\"__insertdate\");");
            sb.AppendLine("					si.AuditDate = dataReader.GetDateTime(ordinal);");
            sb.AppendLine();

            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("					ordinal = dataReader.GetOrdinal(\"" + _model.Database.ModifiedByDatabaseName + "\");");
                sb.AppendLine("					si.ModifiedBy = dataReader.IsDBNull(ordinal) ? null : dataReader.GetString(ordinal);");
                sb.AppendLine();
            }

            foreach (Column column in columnList.OrderBy(x => x.Name))
            {
                sb.AppendLine("					ordinal = dataReader.GetOrdinal(\"" + column.DatabaseName + "\");");
                if (column.DataType == System.Data.SqlDbType.DateTimeOffset)
                {
                    //DateTimeOffset is different
                    sb.AppendLine("					if (dataReader is System.Data.SqlClient.SqlDataReader)");
                    if (column.AllowNull)
                        sb.AppendLine("						si." + column.PascalName + " = dataReader.IsDBNull(ordinal) ? (DateTimeOffset?)null : ((System.Data.SqlClient.SqlDataReader)dataReader).GetDateTimeOffset(ordinal);");
                    else
                        sb.AppendLine("						si." + column.PascalName + " = ((System.Data.SqlClient.SqlDataReader)dataReader).GetDateTimeOffset(ordinal);");
                }
                else if (column.DataType == System.Data.SqlDbType.Time)
                {
                    //Timespan is different
                    sb.AppendLine("					if (dataReader is System.Data.SqlClient.SqlDataReader)");
                    if (column.AllowNull)
                        sb.AppendLine("						si." + column.PascalName + " = dataReader.IsDBNull(ordinal) ? (TimeSpan?)null : ((System.Data.SqlClient.SqlDataReader)dataReader).GetTimeSpan(ordinal);");
                    else
                        sb.AppendLine("						si." + column.PascalName + " = ((System.Data.SqlClient.SqlDataReader)dataReader).GetTimeSpan(ordinal);");
                }
                else if (column.IsBinaryType)
                {
                    //Binary types
                    if (column.AllowNull)
                        sb.AppendLine("					si." + column.PascalName + " = dataReader.IsDBNull(ordinal) ? (" + column.GetCodeType() + ")null : ReadFromByteArray(ordinal, dataReader);");
                    else
                        sb.AppendLine("					si." + column.PascalName + " = ReadFromByteArray(ordinal, dataReader);");
                }
                else
                {
                    //Non-binary types
                    if (column.AllowNull)
                        sb.AppendLine("					si." + column.PascalName + " = dataReader.IsDBNull(ordinal) ? (" + column.GetCodeType() + ")null : dataReader." + column.GetDataReaderMethodName() + "(ordinal);");
                    else
                        sb.AppendLine("					si." + column.PascalName + " = dataReader." + column.GetDataReaderMethodName() + "(ordinal);");
                }
                sb.AppendLine();
            }

            sb.AppendLine("					innerList.Add(si);");
            sb.AppendLine("				}");
            sb.AppendLine("				retval.InnerList = innerList;");
            sb.AppendLine("			}");
            sb.AppendLine("			catch { throw; }");
            sb.AppendLine("			finally");
            sb.AppendLine("			{");
            sb.AppendLine("				if (dataReader != null) dataReader.Close();");
            sb.AppendLine("				retval.TotalRecordCount = (int)((System.Data.IDbDataParameter)(dbCommand.Parameters[\"@RETURN_VALUE\"])).Value;");
            sb.AppendLine("				if (dbConnection.State == ConnectionState.Open) dbConnection.Close();");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();

            if (_item.GetColumns().Count(x => x.IsBinaryType) > 0)
            {
                //Read binary method
                sb.AppendLine("		private static byte[] ReadFromByteArray(int ordinal, System.Data.IDataReader dataReader)");
                sb.AppendLine("		{");
                sb.AppendLine("			int bufferSize = 1024 * 10;");
                sb.AppendLine("			int startIndex = 0;");
                sb.AppendLine("			var outbyte = new byte[bufferSize];");
                sb.AppendLine("			var retval = new List<byte>();");
                sb.AppendLine();
                sb.AppendLine("			// Read the bytes into outbyte[] and retain the number of bytes returned.");
                sb.AppendLine("			long count = dataReader.GetBytes(ordinal, startIndex, outbyte, 0, bufferSize);");
                sb.AppendLine();
                sb.AppendLine("			// Continue reading and writing while there are bytes beyond the size of the buffer.");
                sb.AppendLine("			while (count == bufferSize)");
                sb.AppendLine("			{");
                sb.AppendLine("				retval.AddRange(outbyte);");
                sb.AppendLine("				startIndex += bufferSize;");
                sb.AppendLine("				count = dataReader.GetBytes(1, startIndex, outbyte, 0, bufferSize);");
                sb.AppendLine("			}");
                sb.AppendLine("			return retval.ToArray();");
                sb.AppendLine("		}");
                sb.AppendLine();
            }

        }

        private void AppendCompare()
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Given two audit items this method returns a set of differences");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"item1\"></param>");
            sb.AppendLine("		/// <param name=\"item2\"></param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public static " + this.GetLocalNamespace() + ".AuditResult<" + _item.PascalName + "Audit> CompareAudits(" + _item.PascalName + "Audit item1, " + _item.PascalName + "Audit item2)");
            sb.AppendLine("		{");
            sb.AppendLine("			var retval = new " + this.GetLocalNamespace() + ".AuditResult<" + _item.PascalName + "Audit>(item1, item2);");
            sb.AppendLine("			var differences = new List<I" + _item.PascalName + "AuditResultFieldCompare>();");
            sb.AppendLine();

            foreach (Column column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
                {
                    sb.AppendLine("			if (item1." + column.PascalName + " != item2." + column.PascalName + ")");
                    sb.AppendLine("				differences.Add(new " + _item.PascalName + "AuditResultFieldCompare<" + column.GetCodeType(true) + ">(item1." + column.PascalName + ", item2." + column.PascalName + ", " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ", typeof(" + column.GetCodeType(false) + ")));");
                }
            }

            sb.AppendLine();
            sb.AppendLine("			retval.Differences = (IEnumerable<" + this.GetLocalNamespace() + ".IAuditResultFieldCompare>)differences;");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            //CompareTo
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Compares the current object with another object of the same type.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"other\">An object to compare with this object.</param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public int CompareTo(" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit other)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (other.AuditDate < this.AuditDate) return -1;");
            sb.AppendLine("			else if (this.AuditDate < other.AuditDate) return 1;");
            sb.AppendLine("			else return 0;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Compares the current object with another object of the same type.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"other\">An object to compare with this object.</param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		int IComparable.CompareTo(object other)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (other is " + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit) return this.CompareTo((" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit)other);");
            sb.AppendLine("			else return 0;");
            sb.AppendLine("		}");
            sb.AppendLine();
        }

        #endregion

    }
}