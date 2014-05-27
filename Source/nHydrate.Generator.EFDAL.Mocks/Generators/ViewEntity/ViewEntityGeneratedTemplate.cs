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
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.EFDAL.Mocks.Generators.ViewEntity
{
	public class ViewEntityGeneratedTemplate : EFDALMockBaseTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private readonly CustomView _item;

		public ViewEntityGeneratedTemplate(ModelRoot model, CustomView view)
			: base(model)
		{
			_item = view;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("{0}.Generated.cs", _item.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}.cs", _item.PascalName); }
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
				sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
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
			sb.AppendLine("	/// This is a mock class for the EF entity " + _item.PascalName);
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
			sb.AppendLine("	public partial class " + _item.PascalName + " : nHydrate.EFCore.DataAccess.NHEntityObject, " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.PascalName + ", nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject, System.IEquatable<" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.PascalName + ">");
			sb.AppendLine("	{");
			this.AppendedFieldEnum();
			this.AppendRegionGetValue();
			this.AppendProperties();
			this.AppendIBusinessObject();
			this.AppendIsEquivalent();
			this.AppendIEquatable();
			sb.AppendLine("	}");
			sb.AppendLine();
		}

		private void AppendProperties()
		{
			sb.AppendLine("		#region Properties");
			sb.AppendLine();

			foreach (var column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendLine("		/// <summary>");
				if (!string.IsNullOrEmpty(column.Description))
					StringHelper.LineBreakCode(sb, column.Description, "		/// ");
				else
					sb.AppendLine("		/// The property that the view field '" + column.DatabaseName + "'");

				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public virtual " + column.GetCodeType() + " " + column.PascalName + " { get; private set; }");
				sb.AppendLine();
			}

			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionGetValue()
		{
			sb.AppendLine("		#region GetValue");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public object GetValue(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetValue(field, null);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public object GetValue(FieldNameConstants field, object defaultValue)");
			sb.AppendLine("		{");
			var allColumns = _item.GetColumns().Where(x => x.Generated).ToList();
			foreach (var column in allColumns.OrderBy(x => x.Name))
			{
				sb.AppendLine("			if (field == FieldNameConstants." + column.PascalName + ")");
				if (column.AllowNull)
					sb.AppendLine("				return ((this." + column.PascalName + " == null) ? defaultValue : this." + column.PascalName + ");");
				else
					sb.AppendLine("				return this." + column.PascalName + ";");
			}
			sb.AppendLine("			throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int GetInteger(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetInteger(field, int.MinValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int GetInteger(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, int defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				int a;");
			sb.AppendLine("				if (int.TryParse((string)o, out a))");
			sb.AppendLine("					return a;");
			sb.AppendLine("				else");
			sb.AppendLine("					throw new System.InvalidCastException();");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				return (int)o;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public double GetDouble(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDouble(field, double.MinValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public double GetDouble(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, double defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				double a;");
			sb.AppendLine("				if (double.TryParse((string)o, out a))");
			sb.AppendLine("					return a;");
			sb.AppendLine("				else");
			sb.AppendLine("					throw new System.InvalidCastException();");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				return (double)o;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public System.DateTime GetDateTime(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDateTime(field, System.DateTime.MinValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public System.DateTime GetDateTime(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, DateTime defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				System.DateTime a;");
			sb.AppendLine("				if (System.DateTime.TryParse((string)o, out a))");
			sb.AppendLine("					return a;");
			sb.AppendLine("				else");
			sb.AppendLine("					throw new System.InvalidCastException();");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				return (System.DateTime)o;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string GetString(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetString(field, string.Empty);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string GetString(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, string defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				return (string)o;");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				return o.ToString();");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendedFieldEnum()
		{
			var imageColumnList = _item.GetColumnsByType(System.Data.SqlDbType.Image);
			if (imageColumnList.Count() != 0)
			{
				sb.AppendLine("		#region FieldNameConstants Enumeration");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// An enumeration of this object's image type fields");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public enum FieldImageConstants");
				sb.AppendLine("		{");
				foreach (var column in imageColumnList)
				{
					sb.AppendLine("			/// <summary>");
					sb.AppendLine("			/// Field mapping for the image column '" + column.PascalName + "' property");
					sb.AppendLine("			/// </summary>");
					sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the image column '" + column.PascalName + "' property\")]");
					sb.AppendLine("			" + column.PascalName + ",");
				}
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
			}

			sb.AppendLine("		#region FieldNameConstants Enumeration");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Enumeration to define each property that maps to a database field for the '" + _item.PascalName + "' view.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public enum FieldNameConstants");
			sb.AppendLine("		{");
			foreach (var column in _item.GeneratedColumns)
			{
				sb.AppendLine("			/// <summary>");
				sb.AppendLine("			/// Field mapping for the '" + column.PascalName + "' property");
				sb.AppendLine("			/// </summary>");
				sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
				sb.AppendLine("			" + column.PascalName + ",");
			}

			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendIsEquivalent()
		{
			sb.AppendLine("		#region IsEquivalent");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines if all of the fields for the specified object exactly matches the current object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"item\">The object to compare</param>");
			sb.AppendLine("		public override bool IsEquivalent(nHydrate.EFCore.DataAccess.INHEntityObject other)");
			sb.AppendLine("		{");
			sb.AppendLine("			return ((System.IEquatable<" + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + ">)this).Equals(other as " + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + ");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendIBusinessObject()
		{
			sb.AppendLine("		#region IBusinessObject Members");
			sb.AppendLine();
			sb.AppendLine("		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetValue((FieldNameConstants)field, null);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(Enum field, object defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetValue((FieldNameConstants)field, defaultValue);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldNameConstants()");
			sb.AppendLine("		{");
			sb.AppendLine("			return typeof(FieldNameConstants);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		int nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetMaxLength(System.Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return " + _item.PascalName + ".GetMaxLength((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();

			#region Static Method GetMaxLength

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Given a field definition this will return the maximum length of that field");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static int GetMaxLength(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (var column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendLine("				case FieldNameConstants." + column.PascalName + ":");
				//This is an actual column in this table
				switch (column.DataType)
				{
					case System.Data.SqlDbType.Text:
						sb.AppendLine("					return int.MaxValue;");
						break;
					case System.Data.SqlDbType.NText:
						sb.AppendLine("					return int.MaxValue;");
						break;
					case System.Data.SqlDbType.Image:
						sb.AppendLine("					return int.MaxValue;");
						break;
					case System.Data.SqlDbType.Char:
					case System.Data.SqlDbType.NChar:
					case System.Data.SqlDbType.NVarChar:
					case System.Data.SqlDbType.VarChar:
						if ((column.Length == 0) && (ModelHelper.SupportsMax(column.DataType)))
							sb.AppendLine("					return int.MaxValue;");
						else
							sb.AppendLine("					return " + column.Length + ";");
						break;
					default:
						sb.AppendLine("					return 0;");
						break;
				}
			}
			sb.AppendLine("			}");
			sb.AppendLine("			return 0;");
			sb.AppendLine("		}");
			sb.AppendLine();

			#endregion

			#region GetFieldType
			sb.AppendLine("		#region GetFieldType");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the system type of a field on this object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static System.Type GetFieldType(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = new " + _item.PascalName + "();");
			sb.AppendLine("			return ((nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject)o).GetFieldType(field);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldType(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (field.GetType() != typeof(FieldNameConstants))");
			sb.AppendLine("				throw new Exception(\"The '\" + field.GetType().ReflectedType.ToString() + \".FieldNameConstants' value is not valid. The field parameter must be of type '\" + this.GetType().ToString() + \".FieldNameConstants'.\");");
			sb.AppendLine();
			sb.AppendLine("			switch ((FieldNameConstants)field)");
			sb.AppendLine("			{");
			foreach (var column in _item.GeneratedColumns)
			{
				sb.AppendLine("				case FieldNameConstants." + column.PascalName + ": return typeof(" + column.GetCodeType() + ");");
			}
			sb.AppendLine("			}");
			sb.AppendLine("			return null;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion

			sb.AppendLine("		bool nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.IsEquivalent(nHydrate.EFCore.DataAccess.INHEntityObject other)");
			sb.AppendLine("		{");
			sb.AppendLine("			return ((System.IEquatable<" + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + ">)this).Equals(other as " + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + ");");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Hold the primary key for this object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected nHydrate.EFCore.DataAccess.IPrimaryKey _primaryKey = null;");
			sb.AppendLine("		nHydrate.EFCore.DataAccess.IPrimaryKey nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.PrimaryKey");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendIEquatable()
		{
			sb.AppendLine("		#region Equals");
			sb.AppendLine("		bool System.IEquatable<" + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + ">.Equals(" + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + " other)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.Equals(other);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		public override bool Equals(object obj)");
			sb.AppendLine("		{");
			sb.AppendLine("			var other = obj as " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ";");
			sb.AppendLine("			if (other == null) return false;");
			sb.AppendLine("			return (");

			var allColumns = _item.GetColumns().Where(x => x.Generated);
			var index = 0;
			foreach (var column in allColumns)
			{
#if notimplementedyet
				Table typeTable = _currentView.GetRelatedTypeTableByColumn(column, true);
				if (typeTable != null)
				{
					sb.Append("				other." + typeTable.PascalName + " == this." + typeTable.PascalName);
				}
				else
				{
					sb.Append("				other." + column.PascalName + " == this." + column.PascalName);
				}
#else
				sb.Append("				other." + column.PascalName + " == this." + column.PascalName);
#endif
				if (index < allColumns.Count() - 1) sb.Append(" &&");
				sb.AppendLine();
				index++;
			}

			sb.AppendLine("				);");
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

	}
}
