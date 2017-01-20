#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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

namespace nHydrate.Generator.EFDAL.Interfaces.Generators
{
	public class AuditEntityGeneratedTemplate : EFDALInterfaceBaseTemplate
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
			get { return string.Format("I{0}Audit.Generated.cs", _currentTable.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("I{0}Audit.cs", _currentTable.PascalName); }
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
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine();
		}

		private void AppendEntityClass()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// The interface for the '" + _currentTable.PascalName + "Audit' entity");
			if (!string.IsNullOrEmpty(_currentTable.Description))
				StringHelper.LineBreakCode(sb, _currentTable.Description, "	/// ");
			sb.AppendLine("	/// </summary>");

			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
			sb.AppendLine("	public partial interface I" + _currentTable.PascalName + "Audit : System.IComparable, System.IComparable<" + this.GetLocalNamespace() + ".Audit.I" + _currentTable.PascalName + "Audit>");
			sb.AppendLine("	{");
			this.AppendProperties();
			sb.AppendLine("	}");
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
				sb.AppendLine("		" + column.GetCodeType(true, true) + " " + column.PascalName + " { get; }");
				sb.AppendLine();
			}

			//sb.AppendLine("		/// <summary>");
			//sb.AppendLine("		/// The type of audit");
			//sb.AppendLine("		/// </summary>");
			//sb.AppendLine("		nHydrate.EFCore.DataAccess.AuditTypeConstants AuditType { get; }");
			//sb.AppendLine();
			//sb.AppendLine("		/// <summary>");
			//sb.AppendLine("		/// The date of the audit");
			//sb.AppendLine("		/// </summary>");
			//sb.AppendLine("		DateTime AuditDate { get; }");
			//sb.AppendLine();

			////The interface demands that this property always exist even if the table does NOT have a modified audit
			//sb.AppendLine("		/// <summary>");
			//sb.AppendLine("		/// The modifier value of the audit");
			//sb.AppendLine("		/// </summary>");
			//sb.AppendLine("		string ModifiedBy { get; }");
			//sb.AppendLine();

			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}
		#endregion

	}
}
