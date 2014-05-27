#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.EFDAL.Mocks.Generators
{
	public class AuditEntityGeneratedTemplate : EFDALMockBaseTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private readonly Table _currentTable;

		public AuditEntityGeneratedTemplate(ModelRoot model, Table currentTable)
			: base(model)
		{
			_currentTable = currentTable;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("{0}Audit.Generated.cs", _currentTable.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}Audit.cs", _currentTable.PascalName); }
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
			sb.AppendLine("using System.ComponentModel;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine();
		}

		private void AppendEntityClass()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// The object to hold the '" + _currentTable.PascalName + "Audit' entity");
			if (!string.IsNullOrEmpty(_currentTable.Description))
				StringHelper.LineBreakCode(sb, _currentTable.Description, "	/// ");
			sb.AppendLine("	/// </summary>");

			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "Audit : " + this.DefaultNamespace + ".EFDAL.Interfaces.Audit.I" + _currentTable.PascalName + "Audit, nHydrate.EFCore.DataAccess.IAudit, System.IComparable, System.IComparable<" + this.DefaultNamespace + ".EFDAL.Interfaces.Audit.I" + _currentTable.PascalName + "Audit>");

			sb.AppendLine("	{");
			this.AppendConstructors();
			this.AppendProperties();
			this.AppendCompare();
			sb.AppendLine("	}");
			sb.AppendLine();

			#region Add the AuditResultFieldCompare class
			sb.AppendLine("	#region AuditResultFieldCompare");
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// ");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public interface I" + _currentTable.PascalName + "AuditResultFieldCompare : nHydrate.EFCore.DataAccess.IAuditResultFieldCompare");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		new " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants Field { get; }");
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// A comparison class for audit comparison results");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	/// <typeparam name=\"T\"></typeparam>");
			sb.AppendLine("	public class " + _currentTable.PascalName + "AuditResultFieldCompare<T> : nHydrate.EFCore.DataAccess.AuditResultFieldCompare<T, " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants>, I" + _currentTable.PascalName + "AuditResultFieldCompare");
			sb.AppendLine("	{");
			sb.AppendLine("		internal " + _currentTable.PascalName + "AuditResultFieldCompare(T value1, T value2, " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants field, System.Type dataType)");
			sb.AppendLine("			: base(value1, value2, field, dataType)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine("	}");
			sb.AppendLine("	#endregion");
			sb.AppendLine();
			#endregion

		}

		private void AppendConstructors()
		{
			sb.AppendLine("		#region Constructors");
			sb.AppendLine();
			sb.AppendLine("		internal " + _currentTable.PascalName + "Audit()");
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
			var tableList = new List<Table>(new Table[] { _currentTable });

			//This is for inheritance which is NOT supported right now
			//List<Table> tableList = new List<Table>(_currentTable.GetTableHierarchy().Where(x => x.AllowAuditTracking).Reverse());
			foreach (var table in tableList)
			{
				foreach (var column in table.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
				{
					if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
					{
						if (!columnList.ContainsKey(column.Name))
							columnList.Add(column.Name, column);
					}
				}
			}

			foreach (var column in columnList.Values)
			{
				sb.AppendLine("		/// <summary>");
				if (!string.IsNullOrEmpty(column.Description))
					StringHelper.LineBreakCode(sb, column.Description, "		/// ");
				else
					sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + column.GetCodeType(true, true) + " " + column.PascalName + " { get; internal set; }");
			}

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The type of audit");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public nHydrate.EFCore.DataAccess.AuditTypeConstants AuditType { get; internal set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The date of the audit");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public DateTime AuditDate { get; internal set; }");

			//The interface demands that this property always exist even if the table does NOT have a modified audit
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The modifier value of the audit");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string ModifiedBy { get; internal set; }");

			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendCompare()
		{
			sb.AppendLine("		#region Compare");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Given two audit items this method returns a set of differences");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"item1\"></param>");
			sb.AppendLine("		/// <param name=\"item2\"></param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static nHydrate.EFCore.DataAccess.AuditResult<" + _currentTable.PascalName + "Audit> CompareAudits(" + _currentTable.PascalName + "Audit item1, " + _currentTable.PascalName + "Audit item2)");
			sb.AppendLine("		{");
			sb.AppendLine("			var retval = new nHydrate.EFCore.DataAccess.AuditResult<" + _currentTable.PascalName + "Audit>(item1, item2);");
			sb.AppendLine("			var differences = new List<I" + _currentTable.PascalName + "AuditResultFieldCompare>();");
			sb.AppendLine();

			foreach (var column in _currentTable.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
			{
				if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
				{
					if (column.IsBinaryType) //Binary is a difference comparison
					{
						sb.AppendLine("			if (item1." + column.PascalName + " == null ^ item2." + column.PascalName + " == null)");
						sb.AppendLine("				differences.Add(new " + _currentTable.PascalName + "AuditResultFieldCompare<" + column.GetCodeType(true) + ">(item1." + column.PascalName + ", item2." + column.PascalName + ", " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants." + column.PascalName + ", typeof(" + column.GetCodeType(false) + ")));");
						sb.AppendLine("			if (item1." + column.PascalName + " != null && item2." + column.PascalName + " != null && !item1." + column.PascalName + ".SequenceEqual(item2." + column.PascalName + "))");
						sb.AppendLine("				differences.Add(new " + _currentTable.PascalName + "AuditResultFieldCompare<" + column.GetCodeType(true) + ">(item1." + column.PascalName + ", item2." + column.PascalName + ", " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants." + column.PascalName + ", typeof(" + column.GetCodeType(false) + ")));");
					}
					else
					{
						sb.AppendLine("			if (item1." + column.PascalName + " != item2." + column.PascalName + ")");

						if (column.PrimaryKey)
							sb.AppendLine("				differences.Add(new " + _currentTable.PascalName + "AuditResultFieldCompare<" + column.GetCodeType(true) + ">((" + column.GetCodeType(false) + ")item1." + column.PascalName + ", (" + column.GetCodeType(false) + ")item2." + column.PascalName + ", " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants." + column.PascalName + ", typeof(" + column.GetCodeType(false) + ")));");
						else if (column.IsTextType || column.IsBinaryType)
							sb.AppendLine("				differences.Add(new " + _currentTable.PascalName + "AuditResultFieldCompare<" + column.GetCodeType(true) + ">(item1." + column.PascalName + ", item2." + column.PascalName + ", " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants." + column.PascalName + ", typeof(" + column.GetCodeType(false) + ")));");
						else
							sb.AppendLine("				differences.Add(new " + _currentTable.PascalName + "AuditResultFieldCompare<" + column.GetCodeType(true) + ">(item1." + column.PascalName + ".GetValueOrDefault(), item2." + column.PascalName + ".GetValueOrDefault(), " + this.GetLocalNamespace() + ".Entity." + _currentTable.PascalName + ".FieldNameConstants." + column.PascalName + ", typeof(" + column.GetCodeType(false) + ")));");
					}
				}
			}

			sb.AppendLine();
			sb.AppendLine("			retval.Differences = differences;");
			sb.AppendLine("			return retval;");
			sb.AppendLine("		}");
			sb.AppendLine();

			//CompareTo
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Compares the current object with another object of the same type.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"other\">An object to compare with this object.</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public virtual int CompareTo(" + this.DefaultNamespace + ".EFDAL.Interfaces.Audit.I" + _currentTable.PascalName + "Audit other)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = other as nHydrate.EFCore.DataAccess.IAudit;");
			sb.AppendLine("			if (o == null)");
			sb.AppendLine("				throw new Exception(\"The specified object is not the correct type.\");");
			sb.AppendLine("			");
			sb.AppendLine("			if (o.AuditDate < this.AuditDate) return -1;");
			sb.AppendLine("			else if (this.AuditDate < o.AuditDate) return 1;");
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
			sb.AppendLine("			return this.CompareTo(other as " + this.GetLocalNamespace() + ".Audit." + _currentTable.PascalName + "Audit);");
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

	}
}
