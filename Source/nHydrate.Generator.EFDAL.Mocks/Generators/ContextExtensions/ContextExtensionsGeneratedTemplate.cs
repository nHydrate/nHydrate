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

namespace nHydrate.Generator.EFDAL.Mocks.Generators.ContextExtensions
{
	public class ContextExtensionsGeneratedTemplate : EFDALMockBaseTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();

		public ContextExtensionsGeneratedTemplate(ModelRoot model)
			: base(model)
		{
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return _model.ProjectName + "EntitiesExtensions.Generated.cs"; }
		}

		public string ParentItemName
		{
			get { return _model.ProjectName + "EntitiesExtensions.cs"; }
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
				nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
				nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + this.GetLocalNamespace());
				sb.AppendLine("{");
				this.AppendExtensions();
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendUsingStatements()
		{
			sb.AppendLine("#pragma warning disable 1591");
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Data.Objects;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using " + this.GetLocalNamespace() + ".Entity;");
			sb.AppendLine("using System.Linq.Expressions;");
			sb.AppendLine();
		}

		private void AppendExtensions()
		{
			sb.AppendLine("	#region " + _model.ProjectName + "EntitiesExtensions");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// Extension methods for this library");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
			sb.AppendLine("	public static partial class " + _model.ProjectName + "EntitiesExtensions");
			sb.AppendLine("	{");

			#region Include Extension Methods
			//Add an strongly-typed extension for "Include" method
			sb.AppendLine("		#region Include Extension Methods");
			sb.AppendLine();
			foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable == TypedTableConstants.None)).OrderBy(x => x.PascalName))
			{
				//Build relation list
				var relationList1 = table.GetRelationsFullHierarchy().Where(x =>
					x.ParentTableRef.Object == table &&
					!(x.ChildTableRef.Object as Table).IsInheritedFrom(x.ParentTableRef.Object as Table)
					);

				var relationList2 = table.GetRelationsWhereChild().Where(x =>
					x.ChildTableRef.Object == table &&
					!(x.ChildTableRef.Object as Table).IsInheritedFrom(x.ParentTableRef.Object as Table)
					);

				var relationList = new List<Relation>();
				relationList.AddRange(relationList1);
				relationList.AddRange(relationList2);

				//Generate an extension if there are relations for this table
				if (relationList.Count() != 0)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Specifies the related objects to include in the query results.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		/// <param name=\"item\">Related object to return in query results</param>");
					sb.AppendLine("		/// <param name=\"query\">The LINQ expresssion that maps an include path</param>");
					sb.AppendLine("		public static ObjectQuery<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> Include(this ObjectQuery<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> item, Expression<Func<" + this.GetLocalNamespace() + "." + table.PascalName + "Include, nHydrate.EFCore.DataAccess.IContextInclude>> query)");
					sb.AppendLine("		{");
					sb.AppendLine("			var strings = new List<string>(query.Body.ToString().Split('.'));");
					sb.AppendLine("			strings.RemoveAt(0);");
					sb.AppendLine("			var compoundString = string.Empty;");
					sb.AppendLine("			foreach (var s in strings)");
					sb.AppendLine("			{");
					sb.AppendLine("				if (!string.IsNullOrEmpty(compoundString)) compoundString += \".\";");
					sb.AppendLine("				compoundString += s;");
					sb.AppendLine("				item = item.Include(compoundString);");
					sb.AppendLine("			}");
					sb.AppendLine("			return item;");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
			}
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion

			#region GetFieldType
			sb.AppendLine("		#region GetFieldType Extension Method");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the system type of a field of one of the contained context objects");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static System.Type GetFieldType(this " + this.GetLocalNamespace() + "." + _model.ProjectName + "Entities context, Enum field)");
			sb.AppendLine("		{");
			foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
			{
				sb.AppendLine("			if (field is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants)");
				sb.AppendLine("				return " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".GetFieldType((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants)field);");
			}
			sb.AppendLine("			throw new Exception(\"Unknown field type!\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion

			#region Metadata Extension Methods
			sb.AppendLine("		#region Metadata Extension Methods");
			sb.AppendLine();

			//Main one for base NHEntityObject object
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates and returns a metadata object for an entity type");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"entity\">The source class</param>");
			sb.AppendLine("		/// <returns>A metadata object for the entity types in this assembly</returns>");
			sb.AppendLine("		public static " + this.DefaultNamespace + ".EFDAL.Interfaces.IMetadata GetMetaData(this nHydrate.EFCore.DataAccess.INHEntityObject entity)");
			sb.AppendLine("		{");
			sb.AppendLine("			var a = entity.GetType().GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MetadataTypeAttribute), true).FirstOrDefault();");
			sb.AppendLine("			if (a == null) return null;");
			sb.AppendLine("			var t = ((System.ComponentModel.DataAnnotations.MetadataTypeAttribute)a).MetadataClassType;");
			sb.AppendLine("			if (t == null) return null;");
			sb.AppendLine("			return Activator.CreateInstance(t) as " + this.DefaultNamespace + ".EFDAL.Interfaces.IMetadata;");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion

			#region GetEntityType

			sb.AppendLine("		#region GetEntityType");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the entity from one of its fields");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static System.Type GetEntityType(EntityMappingConstants entityType)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (entityType)");
			sb.AppendLine("			{");
			foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
				sb.AppendLine("				case EntityMappingConstants." + table.PascalName + ": return typeof(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ");");
			sb.AppendLine("			}");
			sb.AppendLine("			throw new Exception(\"Unknown entity type!\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();

			#endregion

			#region GetValue Methods
			sb.AppendLine("		#region GetValue Methods");
			sb.AppendLine();

			foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
			{
				//GetValue by lambda
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Gets the value of one of this object's properties.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <typeparam name=\"T\">The type of value to retrieve</typeparam>");
				sb.AppendLine("		/// <param name=\"item\">The item from which to pull the value.</param>");
				sb.AppendLine("		/// <param name=\"selector\">The field to retrieve</param>");
				sb.AppendLine("		/// <returns></returns>");
				sb.AppendLine("		public static T GetValue<T>(this " + this.GetLocalNamespace() + ".Entity." + table.PascalName + " item, System.Linq.Expressions.Expression<System.Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", T>> selector)");
				sb.AppendLine("		{");
				sb.AppendLine("			var b = selector.Body.ToString();");
				sb.AppendLine("			var arr = b.Split('.');");
				sb.AppendLine("			if (arr.Length != 2) throw new System.Exception(\"Invalid selector\");");
				sb.AppendLine("			var tn = arr.Last();");
				sb.AppendLine("			var te = (" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants)Enum.Parse(typeof(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants), tn, true);");
				sb.AppendLine("			return item.GetValue<T>(te, default(T));");
				sb.AppendLine("		}");
				sb.AppendLine();

				//GetValue by lambda with default
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Gets the value of one of this object's properties.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <typeparam name=\"T\">The type of value to retrieve</typeparam>");
				sb.AppendLine("		/// <param name=\"item\">The item from which to pull the value.</param>");
				sb.AppendLine("		/// <param name=\"selector\">The field to retrieve</param>");
				sb.AppendLine("		/// <param name=\"defaultValue\">The default value to return if the specified value is NULL</param>");
				sb.AppendLine("		/// <returns></returns>");
				sb.AppendLine("		public static T GetValue<T>(this " + this.GetLocalNamespace() + ".Entity." + table.PascalName + " item, System.Linq.Expressions.Expression<System.Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", T>> selector, T defaultValue)");
				sb.AppendLine("		{");
				sb.AppendLine("			var b = selector.Body.ToString();");
				sb.AppendLine("			var arr = b.Split('.');");
				sb.AppendLine("			if (arr.Length != 2) throw new System.Exception(\"Invalid selector\");");
				sb.AppendLine("			var tn = arr.Last();");
				sb.AppendLine("			var te = (" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants)Enum.Parse(typeof(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants), tn, true);");
				sb.AppendLine("			return item.GetValue<T>(te, defaultValue);");
				sb.AppendLine("		}");
				sb.AppendLine();

				//GetValue by by Enum
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Gets the value of one of this object's properties.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <typeparam name=\"T\">The type of value to retrieve</typeparam>");
				sb.AppendLine("		/// <param name=\"item\">The item from which to pull the value.</param>");
				sb.AppendLine("		/// <param name=\"field\">The field value to retrieve</param>");
				sb.AppendLine("		/// <returns></returns>");
				sb.AppendLine("		public static T GetValue<T>(this " + this.GetLocalNamespace() + ".Entity." + table.PascalName + " item, " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants field)");
				sb.AppendLine("		{");
				sb.AppendLine("			return item.GetValue<T>(field, default(T));");
				sb.AppendLine("		}");
				sb.AppendLine();

				//GetValue by by Enum with default
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Gets the value of one of this object's properties.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <typeparam name=\"T\">The type of value to retrieve</typeparam>");
				sb.AppendLine("		/// <param name=\"item\">The item from which to pull the value.</param>");
				sb.AppendLine("		/// <param name=\"field\">The field value to retrieve</param>");
				sb.AppendLine("		/// <param name=\"defaultValue\">The default value to return if the specified value is NULL</param>");
				sb.AppendLine("		/// <returns></returns>");
				sb.AppendLine("		public static T GetValue<T>(this " + this.GetLocalNamespace() + ".Entity." + table.PascalName + " item, " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants field, T defaultValue)");
				sb.AppendLine("		{");
				sb.AppendLine("			var valid = false;");
				sb.AppendLine("			if (typeof(T) == typeof(bool)) valid = true;");
				sb.AppendLine("			else if (typeof(T) == typeof(byte)) valid = true;");
				sb.AppendLine("			else if (typeof(T) == typeof(char)) valid = true;");
				sb.AppendLine("			else if (typeof(T) == typeof(DateTime)) valid = true;");
				sb.AppendLine("			else if (typeof(T) == typeof(decimal)) valid = true;");
				sb.AppendLine("			else if (typeof(T) == typeof(double)) valid = true;");
				sb.AppendLine("			else if (typeof(T) == typeof(int)) valid = true;");
				sb.AppendLine("			else if (typeof(T) == typeof(long)) valid = true;");
				sb.AppendLine("			else if (typeof(T) == typeof(Single)) valid = true;");
				sb.AppendLine("			else if (typeof(T) == typeof(string)) valid = true;");
				sb.AppendLine("			if (!valid)");
				sb.AppendLine("				throw new Exception(\"Cannot convert object to type '\" + typeof(T).ToString() + \"'!\");");
				sb.AppendLine();
				sb.AppendLine("			object o = item.GetValue(field, defaultValue);");
				sb.AppendLine("			if (o == null) return defaultValue;");
				sb.AppendLine();
				sb.AppendLine("			if (o is T)");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)o;");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (typeof(T) == typeof(bool))");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)(object)Convert.ToBoolean(o);");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (typeof(T) == typeof(byte))");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)(object)Convert.ToByte(o);");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (typeof(T) == typeof(char))");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)(object)Convert.ToChar(o);");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (typeof(T) == typeof(DateTime))");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)(object)Convert.ToDateTime(o);");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (typeof(T) == typeof(decimal))");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)(object)Convert.ToDecimal(o);");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (typeof(T) == typeof(double))");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)(object)Convert.ToDouble(o);");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (typeof(T) == typeof(int))");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)(object)Convert.ToInt32(o);");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (typeof(T) == typeof(long))");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)(object)Convert.ToInt64(o);");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (typeof(T) == typeof(Single))");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)(object)Convert.ToSingle(o);");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (typeof(T) == typeof(string))");
				sb.AppendLine("			{");
				sb.AppendLine("				return (T)(object)Convert.ToString(o);");
				sb.AppendLine("			}");
				sb.AppendLine("			throw new Exception(\"Cannot convert object!\");");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion

			#region SetValue
			sb.AppendLine("		#region SetValue");
			foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.Immutable && !x.AssociativeTable).OrderBy(x => x.Name))
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Assigns a value to a field on this object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"item\">The entity to set</param>");
				sb.AppendLine("		/// <param name=\"selector\">The field on the entity to set</param>");
				sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
				sb.AppendLine("		public static void SetValue<TResult>(this " + this.GetLocalNamespace() + ".Entity." + table.PascalName + " item, System.Linq.Expressions.Expression<System.Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", TResult>> selector, TResult newValue)");
				sb.AppendLine("		{");
				sb.AppendLine("			SetValue(item, selector, newValue, false);");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Assigns a value to a field on this object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"item\">The entity to set</param>");
				sb.AppendLine("		/// <param name=\"selector\">The field on the entity to set</param>");
				sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
				sb.AppendLine("		/// <param name=\"fixLength\">Determines if the length should be truncated if too long. When false, an error will be raised if data is too large to be assigned to the field.</param>");
				sb.AppendLine("		public static void SetValue<TResult>(this " + this.GetLocalNamespace() + ".Entity." + table.PascalName + " item, System.Linq.Expressions.Expression<System.Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", TResult>> selector, TResult newValue, bool fixLength)");
				sb.AppendLine("		{");
				sb.AppendLine("			var b = selector.Body.ToString();");
				sb.AppendLine("			var arr = b.Split('.');");
				sb.AppendLine("			if (arr.Length != 2) throw new System.Exception(\"Invalid selector\");");
				sb.AppendLine("			var tn = arr.Last();");
				sb.AppendLine("			var te = (" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants)Enum.Parse(typeof(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants), tn, true);");
				sb.AppendLine("			item.SetValue(te, newValue);");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion

			#region GetPagedResults
			sb.AppendLine("		#region GetPagedResults");
			foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
			{
				var listObjectType = "ObjectSet";
				if (table.ParentTable != null)
					listObjectType = "ObjectQuery";

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Pulls a paged set of data based on the paging criteria");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"item\">The " + listObjectType + " from which to pull data</param>");
				sb.AppendLine("		/// <param name=\"orderBy\">The sort order of this data set</param>");
				sb.AppendLine("		/// <param name=\"paging\">The paging object that controls how data is selected. It will contain additional paging information on output.</param>");
				sb.AppendLine("		public static IEnumerable<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> GetPagedResults<TKey>(");
				sb.AppendLine("			this System.Data.Objects." + listObjectType + "<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> item,");
				sb.AppendLine("			System.Linq.Expressions.Expression<Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", TKey>> orderBy,");
				sb.AppendLine("			nHydrate.EFCore.DataAccess.Paging paging)");
				sb.AppendLine("		{");
				sb.AppendLine("			return item.GetPagedResults(x => true, orderBy, true, paging);");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Pulls a paged set of data based on the paging criteria");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"item\">The " + listObjectType + " from which to pull data</param>");
				sb.AppendLine("		/// <param name=\"where\">The filter by which to pull data</param>");
				sb.AppendLine("		/// <param name=\"orderBy\">The sort order of this data set</param>");
				sb.AppendLine("		/// <param name=\"paging\">The paging object that controls how data is selected. It will contain additional paging information on output.</param>");
				sb.AppendLine("		public static IEnumerable<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> GetPagedResults<TKey>(");
				sb.AppendLine("			this System.Data.Objects." + listObjectType + "<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> item,");
				sb.AppendLine("			System.Linq.Expressions.Expression<Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", bool>> where,");
				sb.AppendLine("			System.Linq.Expressions.Expression<Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", TKey>> orderBy,");
				sb.AppendLine("			nHydrate.EFCore.DataAccess.Paging paging)");
				sb.AppendLine("		{");
				sb.AppendLine("			return item.GetPagedResults(where, orderBy, true, paging);");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Pulls a paged set of data based on the paging criteria");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"item\">The " + listObjectType + " from which to pull data</param>");
				sb.AppendLine("		/// <param name=\"orderBy\">The sort order of this data set</param>");
				sb.AppendLine("		/// <param name=\"orderAscending\">The direction of sort</param>");
				sb.AppendLine("		/// <param name=\"paging\">The paging object that controls how data is selected. It will contain additional paging information on output.</param>");
				sb.AppendLine("		public static IEnumerable<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> GetPagedResults<TKey>(");
				sb.AppendLine("			this System.Data.Objects." + listObjectType + "<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> item,");
				sb.AppendLine("			System.Linq.Expressions.Expression<Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", TKey>> orderBy,");
				sb.AppendLine("			bool orderAscending,");
				sb.AppendLine("			nHydrate.EFCore.DataAccess.Paging paging)");
				sb.AppendLine("		{");
				sb.AppendLine("			return item.GetPagedResults(x => true, orderBy, orderAscending, paging);");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Pulls a paged set of data based on the paging criteria");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"item\">The " + listObjectType + " from which to pull data</param>");
				sb.AppendLine("		/// <param name=\"where\">The filter by which to pull data</param>");
				sb.AppendLine("		/// <param name=\"orderBy\">The sort order of this data set</param>");
				sb.AppendLine("		/// <param name=\"orderAscending\">The direction of sort</param>");
				sb.AppendLine("		/// <param name=\"paging\">The paging object that controls how data is selected. It will contain additional paging information on output.</param>");
				sb.AppendLine("		public static IEnumerable<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> GetPagedResults<TKey>(");
				sb.AppendLine("			this System.Data.Objects." + listObjectType + "<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> item,");
				sb.AppendLine("			System.Linq.Expressions.Expression<Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", bool>> where,");
				sb.AppendLine("			System.Linq.Expressions.Expression<Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", TKey>> orderBy,");
				sb.AppendLine("			bool orderAscending,");
				sb.AppendLine("			nHydrate.EFCore.DataAccess.Paging paging)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (item == null) return null;");
				sb.AppendLine("			if (where == null) return null;");
				sb.AppendLine("			if (orderBy == null) return null;");
				sb.AppendLine("			var index = paging.PageIndex;");
				sb.AppendLine("			var rpp = paging.RecordsperPage;");
				sb.AppendLine("			if (index < 1) index = 1;");
				sb.AppendLine("			if (rpp < 1) rpp = 1;");
				sb.AppendLine();
				sb.AppendLine("			paging.RecordCount = item.Count(where);");
				sb.AppendLine("			paging.PageCount = paging.RecordCount / rpp;");
				sb.AppendLine("			if ((paging.RecordCount % rpp) != 0) paging.PageCount++;");
				sb.AppendLine();
				sb.AppendLine("			var q = item.Where(where);");
				sb.AppendLine();
				sb.AppendLine("			if (orderAscending)");
				sb.AppendLine("				q = q.OrderBy(orderBy);");
				sb.AppendLine("			else");
				sb.AppendLine("				q = q.OrderByDescending(orderBy);");
				sb.AppendLine();
				sb.AppendLine("			return q.Skip((index - 1) * rpp)");
				sb.AppendLine("							.Take(rpp)");
				sb.AppendLine("							.ToList();");
				sb.AppendLine();
				sb.AppendLine("		}");
				sb.AppendLine();

				//Overload for passing in FieldNameConstants for multiple-item order by clause
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Pulls a paged set of data based on the paging criteria");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"item\">The " + listObjectType + " from which to pull data</param>");
				sb.AppendLine("		/// <param name=\"where\">The filter by which to pull data</param>");
				sb.AppendLine("		/// <param name=\"orderByList\">The sort order of this data set</param>");
				sb.AppendLine("		/// <param name=\"paging\">The paging object that controls how data is selected. It will contain additional paging information on output.</param>");
				sb.AppendLine("		public static IEnumerable<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> GetPagedResults(");
				sb.AppendLine("			this System.Data.Objects." + listObjectType + "<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> item,");
				sb.AppendLine("			System.Linq.Expressions.Expression<Func<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ", bool>> where,");
				sb.AppendLine("			IEnumerable<nHydrate.EFCore.DataAccess.OrderedWrapper<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants>> orderByList,");
				sb.AppendLine("			nHydrate.EFCore.DataAccess.Paging paging)");
				sb.AppendLine("		{");
				sb.AppendLine("			var index = paging.PageIndex;");
				sb.AppendLine("			var rpp = paging.RecordsperPage;");
				sb.AppendLine("			if (index < 1) index = 1;");
				sb.AppendLine("			if (rpp < 1) rpp = 1;");
				sb.AppendLine();
				sb.AppendLine("			paging.RecordCount = item.Count(where);");
				sb.AppendLine("			paging.PageCount = paging.RecordCount / rpp;");
				sb.AppendLine("			if ((paging.RecordCount % rpp) != 0) paging.PageCount++;");
				sb.AppendLine();
				sb.AppendLine("			var q = item.Where(where);");
				sb.AppendLine();
				sb.AppendLine("			foreach (var ob in orderByList)");
				sb.AppendLine("			{");
				sb.AppendLine("				switch (ob.Field)");
				sb.AppendLine("				{");

				foreach (var column in table.GeneratedColumns)
				{
					sb.AppendLine("					case Entity." + table.PascalName + ".FieldNameConstants." + column.PascalName + ":");
					sb.AppendLine("						if (ob.Ascending) q = q.OrderBy(x => x." + column.PascalName + ");");
					sb.AppendLine("						else q = q.OrderByDescending(x => x." + column.PascalName + ");");
					sb.AppendLine("						break;");
				}

				if (table.AllowCreateAudit)
				{
					sb.AppendLine("					case Entity." + table.PascalName + ".FieldNameConstants." + _model.Database.CreatedDatePascalName + ":");
					sb.AppendLine("						if (ob.Ascending) q = q.OrderBy(x => x." + _model.Database.CreatedDatePascalName + ");");
					sb.AppendLine("						else q = q.OrderByDescending(x => x." + _model.Database.CreatedDatePascalName + ");");
					sb.AppendLine("						break;");
					sb.AppendLine("					case Entity." + table.PascalName + ".FieldNameConstants." + _model.Database.CreatedByPascalName + ":");
					sb.AppendLine("						if (ob.Ascending) q = q.OrderBy(x => x." + _model.Database.CreatedByPascalName + ");");
					sb.AppendLine("						else q = q.OrderByDescending(x => x." + _model.Database.CreatedByPascalName + ");");
					sb.AppendLine("						break;");
				}

				if (table.AllowModifiedAudit)
				{
					sb.AppendLine("					case Entity." + table.PascalName + ".FieldNameConstants." + _model.Database.ModifiedDatePascalName + ":");
					sb.AppendLine("						if (ob.Ascending) q = q.OrderBy(x => x." + _model.Database.ModifiedDatePascalName + ");");
					sb.AppendLine("						else q = q.OrderByDescending(x => x." + _model.Database.ModifiedDatePascalName + ");");
					sb.AppendLine("						break;");
					sb.AppendLine("					case Entity." + table.PascalName + ".FieldNameConstants." + _model.Database.ModifiedByPascalName + ":");
					sb.AppendLine("						if (ob.Ascending) q = q.OrderBy(x => x." + _model.Database.ModifiedByPascalName + ");");
					sb.AppendLine("						else q = q.OrderByDescending(x => x." + _model.Database.ModifiedByPascalName + ");");
					sb.AppendLine("						break;");
				}

				sb.AppendLine("				}");
				sb.AppendLine("			}");
				sb.AppendLine();
				sb.AppendLine("			return q.Skip((index - 1) * rpp)");
				sb.AppendLine("							.Take(rpp)");
				sb.AppendLine("							.ToList();");
				sb.AppendLine();
				sb.AppendLine("		}");
				sb.AppendLine();

			}
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion

			#region Purge
			sb.AppendLine("		#region ObservableCollection");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns an observable collection that can bound to UI controls");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static System.Collections.ObjectModel.ObservableCollection<T> AsObservable<T>(this System.Collections.Generic.IEnumerable<T> list)");
			sb.AppendLine("			where T : nHydrate.EFCore.DataAccess.NHEntityObject");
			sb.AppendLine("		{");
			sb.AppendLine("			var retval = new System.Collections.ObjectModel.ObservableCollection<T>();");
			sb.AppendLine("			foreach (var o in list)");
			sb.AppendLine("				retval.Add(o);");
			sb.AppendLine("			return retval;");
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion

			#region Purge
			sb.AppendLine("		#region Purge");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Marks all objects in the list for deletion");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <typeparam name=\"T\"></typeparam>");
			sb.AppendLine("		/// <param name=\"list\">The list of objects to remove</param>");
			sb.AppendLine("		/// <param name=\"context\">The context in which the calling object exists");
			sb.AppendLine("		/// </param>");
			sb.AppendLine("		public static void Purge<T>(this System.Data.Objects.DataClasses.EntityCollection<T> list, " + GetLocalNamespace() + "." + _model.ProjectName + "Entities context)");
			sb.AppendLine("			where T : nHydrate.EFCore.DataAccess.NHEntityObject");
			sb.AppendLine("		{");

			var fieldList = _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable && x.TypedTable != TypedTableConstants.EnumOnly).ToList();
			if (fieldList.Count > 0)
			{
				sb.AppendLine("			foreach(var item in list)");
				sb.AppendLine("			{");

				var index = 0;
				foreach (var table in fieldList)
				{
					sb.AppendLine("				" + (index > 0 ? "else " : string.Empty) + "if (item is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")");
					sb.AppendLine("					context.DeleteItem(item as " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ");");
					index++;
				}
				sb.AppendLine("				else");
				sb.AppendLine("					throw new Exception(\"Unknown type\");");
				sb.AppendLine("			}");
			}
			else
			{
				sb.AppendLine("			throw new Exception(\"Unknown type\");");
			}
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion

			sb.AppendLine("	}");
			sb.AppendLine();

			#region SequentialId
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// Generates Sequential Guid values that can be used for Sql Server UniqueIdentifiers to improve performance.");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	internal class SequentialIdGenerator");
			sb.AppendLine("	{");
			sb.AppendLine("		private readonly object _lock;");
			sb.AppendLine("		private Guid _lastGuid;");
			sb.AppendLine("		// 3 - the least significant byte in Guid ByteArray [for SQL Server ORDER BY clause]");
			sb.AppendLine("		// 10 - the most significant byte in Guid ByteArray [for SQL Server ORDERY BY clause]");
			sb.AppendLine("		private static readonly int[] SqlOrderMap = new int[] { 3, 2, 1, 0, 5, 4, 7, 6, 9, 8, 15, 14, 13, 12, 11, 10 };");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a new SequentialId class to generate sequential GUID values.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public SequentialIdGenerator() : this(Guid.NewGuid()) { }");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a new SequentialId class to generate sequential GUID values.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"seed\">Starting seed value.</param>");
			sb.AppendLine("		/// <remarks>You can save the last generated value <see cref=\"LastValue\"/> and then ");
			sb.AppendLine("		/// use this as the new seed value to pick up where you left off.</remarks>");
			sb.AppendLine("		public SequentialIdGenerator(Guid seed)");
			sb.AppendLine("		{");
			sb.AppendLine("			_lock = new object();");
			sb.AppendLine("			_lastGuid = seed;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Last generated guid value.  If no values have been generated, this will be the seed value.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public Guid LastValue");
			sb.AppendLine("		{");
			sb.AppendLine("			get {");
			sb.AppendLine("				lock (_lock)");
			sb.AppendLine("				{");
			sb.AppendLine("					return _lastGuid;");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine("			set");
			sb.AppendLine("			{");
			sb.AppendLine("				lock (_lock)");
			sb.AppendLine("				{");
			sb.AppendLine("					_lastGuid = value;");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Generate a new sequential id.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns>New sequential id value.</returns>");
			sb.AppendLine("		public Guid NewId()");
			sb.AppendLine("		{");
			sb.AppendLine("			Guid newId;");
			sb.AppendLine("			lock (_lock)");
			sb.AppendLine("			{");
			sb.AppendLine("				var guidBytes = _lastGuid.ToByteArray();");
			sb.AppendLine("				ReorderToSqlOrder(ref guidBytes);");
			sb.AppendLine("				newId = new Guid(guidBytes);");
			sb.AppendLine("				_lastGuid = newId;");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("			return newId;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		private static void ReorderToSqlOrder(ref byte[] bytes)");
			sb.AppendLine("		{");
			sb.AppendLine("			foreach (var bytesIndex in SqlOrderMap)");
			sb.AppendLine("			{");
			sb.AppendLine("				bytes[bytesIndex]++;");
			sb.AppendLine("				if (bytes[bytesIndex] != 0)");
			sb.AppendLine("				{");
			sb.AppendLine("					break;");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// IComparer.Compare compatible method to order Guid values the same way as MS Sql Server.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"x\">The first guid to compare</param>");
			sb.AppendLine("		/// <param name=\"y\">The second guid to compare</param>");
			sb.AppendLine("		/// <returns><see cref=\"System.Collections.IComparer.Compare\"/></returns>");
			sb.AppendLine("		public static int SqlCompare(Guid x, Guid y)");
			sb.AppendLine("		{");
			sb.AppendLine("			var result = 0;");
			sb.AppendLine("			var index = SqlOrderMap.Length - 1;");
			sb.AppendLine("			var xBytes = x.ToByteArray();");
			sb.AppendLine("			var yBytes = y.ToByteArray();");
			sb.AppendLine();
			sb.AppendLine("			while (result == 0 && index >= 0)");
			sb.AppendLine("			{");
			sb.AppendLine("				result = xBytes[SqlOrderMap[index]].CompareTo(yBytes[SqlOrderMap[index]]);");
			sb.AppendLine("				index--;");
			sb.AppendLine("			}");
			sb.AppendLine("			return result;");
			sb.AppendLine("		}");
			sb.AppendLine("	}");
			#endregion

			sb.AppendLine("	#endregion");
			sb.AppendLine();
		}

		#endregion

	}
}
