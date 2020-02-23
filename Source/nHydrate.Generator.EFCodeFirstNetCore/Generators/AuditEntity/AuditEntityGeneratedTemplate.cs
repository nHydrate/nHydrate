#pragma warning disable 0168
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.AuditEntity
{
    public class AuditEntityGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
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
        public override string FileName => $"{_item.PascalName}Audit.Generated.cs";
        public string ParentItemName => $"{_item.PascalName}Audit.cs";

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
            sb.AppendLine($"	/// The '{_item.PascalName}Audit' entity");
            if (!string.IsNullOrEmpty(_item.Description))
                sb.AppendLine("	/// " + _item.Description);
            sb.AppendLine("	/// </summary>");

            sb.AppendLine("	public partial class " + _item.PascalName + "Audit : " + this.GetLocalNamespace() + ".IAudit, System.IComparable, System.IComparable<" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit>");

            sb.AppendLine("	{");
            this.AppendConstructors();
            this.AppendProperties();
            this.AppendCompare();
            sb.AppendLine("	}");
            sb.AppendLine();

            #region Add the AuditResultFieldCompare class
            sb.AppendLine("	/// <summary />");
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
            sb.AppendLine($"		internal {_item.PascalName}Audit()");
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

            var columnList = new Dictionary<string, Column>();
            var tableList = new List<Table>(new Table[] { _item });

            //This is for inheritance which is NOT supported right now
            //List<Table> tableList = new List<Table>(_currentTable.GetTableHierarchy().Where(x => x.AllowAuditTracking).Reverse());
            foreach (Table table in tableList)
            {
                foreach (Column column in table.GetColumns().OrderBy(x => x.Name))
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
                    sb.AppendLine("		/// The property that maps back to the database '" + column.ParentTable.DatabaseName + "." + column.DatabaseName + "' field");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public " + column.GetCodeType() + " " + column.PascalName + " { get; protected internal set; }");
                sb.AppendLine();
            }

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The primary key");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public int __RowId { get; protected set; }");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The type of audit (Insert, Update, Delete)");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + this.GetLocalNamespace() + ".AuditTypeConstants AuditType { get; protected internal set; }");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The date of the audit");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public DateTime AuditDate { get; protected internal set; }");
            sb.AppendLine();

            //if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The modifier value of the audit");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public string ModifiedBy { get; protected internal set; }");
                sb.AppendLine();
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();
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

            foreach (Column column in _item.GetColumns().OrderBy(x => x.Name))
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