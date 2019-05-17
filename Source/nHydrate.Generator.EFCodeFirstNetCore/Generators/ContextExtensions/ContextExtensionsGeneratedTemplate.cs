#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextExtensions
{
    public class ContextExtensionsGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();

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
                this.AppendQueryableExtensions();
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
            sb.AppendLine("using " + this.GetLocalNamespace() + ".Entity;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using System.Reflection;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore.Internal;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore.Query;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore.Query.Internal;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore.Storage;");
            sb.AppendLine("using Remotion.Linq.Parsing.Structure;");
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
                sb.AppendLine("				return " + GetLocalNamespace() + ".Entity." + table.PascalName + ".GetFieldType((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants)field);");
            }
            sb.AppendLine("			throw new Exception(\"Unknown field type!\");");
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

            //GetValue by lambda
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <typeparam name=\"T\">The type of value to retrieve</typeparam>");
            sb.AppendLine("		/// <typeparam name=\"R\">The type of object from which retrieve the field value</typeparam>");
            sb.AppendLine("		/// <param name=\"item\">The item from which to pull the value.</param>");
            sb.AppendLine("		/// <param name=\"selector\">The field to retrieve</param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public static T GetValue<T, R>(this R item, System.Linq.Expressions.Expression<System.Func<R, T>> selector)");
            sb.AppendLine("			where R : BaseEntity");
            sb.AppendLine("		{");
            sb.AppendLine("			var b = selector.Body.ToString();");
            sb.AppendLine("			var arr = b.Split('.');");
            sb.AppendLine("			if (arr.Length != 2) throw new System.Exception(\"Invalid selector\");");
            sb.AppendLine("			var tn = arr.Last();");
            sb.AppendLine("			var ft = ((IReadOnlyBusinessObject)item).GetFieldNameConstants();");
            sb.AppendLine("			var te = (System.Enum)Enum.Parse(ft, tn, true);");
            sb.AppendLine("			return item.GetValueInternal<T, R>(field: te, defaultValue: default(T));");
            sb.AppendLine("		}");
            sb.AppendLine();

            //GetValue by lambda with default
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <typeparam name=\"T\">The type of value to retrieve</typeparam>");
            sb.AppendLine("		/// <typeparam name=\"R\">The type of object from which retrieve the field value</typeparam>");
            sb.AppendLine("		/// <param name=\"item\">The item from which to pull the value.</param>");
            sb.AppendLine("		/// <param name=\"selector\">The field to retrieve</param>");
            sb.AppendLine("		/// <param name=\"defaultValue\">The default value to return if the specified value is NULL</param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public static T GetValue<T, R>(this R item, System.Linq.Expressions.Expression<System.Func<R, T>> selector, T defaultValue)");
            sb.AppendLine("			where R : BaseEntity");
            sb.AppendLine("		{");
            sb.AppendLine("			var b = selector.Body.ToString();");
            sb.AppendLine("			var arr = b.Split('.');");
            sb.AppendLine("			if (arr.Length != 2) throw new System.Exception(\"Invalid selector\");");
            sb.AppendLine("			var tn = arr.Last();");
            sb.AppendLine("			var ft = ((IReadOnlyBusinessObject)item).GetFieldNameConstants();");
            sb.AppendLine("			var te = (System.Enum)Enum.Parse(ft, tn, true);");
            sb.AppendLine("			return item.GetValueInternal<T, R>(field: te, defaultValue: defaultValue);");
            sb.AppendLine("		}");
            sb.AppendLine();

            //GetValue by by Enum with default
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <typeparam name=\"T\">The type of value to retrieve</typeparam>");
            sb.AppendLine("		/// <typeparam name=\"R\">The type of object from which retrieve the field value</typeparam>");
            sb.AppendLine("		/// <param name=\"item\">The item from which to pull the value.</param>");
            sb.AppendLine("		/// <param name=\"field\">The field value to retrieve</param>");
            sb.AppendLine("		/// <param name=\"defaultValue\">The default value to return if the specified value is NULL</param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		private static T GetValueInternal<T, R>(this R item, System.Enum field, T defaultValue)");
            sb.AppendLine("			where R : BaseEntity");
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
            sb.AppendLine("			object o = ((IReadOnlyBusinessObject)item).GetValue(field, defaultValue);");
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
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region SetValue
            sb.AppendLine("		#region SetValue");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Assigns a value to a field on this object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"item\">The entity to set</param>");
            sb.AppendLine("		/// <param name=\"selector\">The field on the entity to set</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
            sb.AppendLine("		public static void SetValue<TResult, R>(this R item, System.Linq.Expressions.Expression<System.Func<R, TResult>> selector, TResult newValue)");
            sb.AppendLine("			where R : BaseEntity, IBusinessObject");
            sb.AppendLine("		{");
            sb.AppendLine("			SetValue(item: item, selector: selector, newValue: newValue, fixLength: false);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Assigns a value to a field on this object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"item\">The entity to set</param>");
            sb.AppendLine("		/// <param name=\"selector\">The field on the entity to set</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
            sb.AppendLine("		/// <param name=\"fixLength\">Determines if the length should be truncated if too long. When false, an error will be raised if data is too large to be assigned to the field.</param>");
            sb.AppendLine("		public static void SetValue<TResult, R>(this R item, System.Linq.Expressions.Expression<System.Func<R, TResult>> selector, TResult newValue, bool fixLength)");
            sb.AppendLine("			where R : BaseEntity, IBusinessObject");
            sb.AppendLine("		{");
            sb.AppendLine("			var b = selector.Body.ToString();");
            sb.AppendLine("			var arr = b.Split('.');");
            sb.AppendLine("			if (arr.Length != 2) throw new System.Exception(\"Invalid selector\");");
            sb.AppendLine("			var tn = arr.Last();");
            sb.AppendLine("			var ft = ((IReadOnlyBusinessObject)item).GetFieldNameConstants();");
            sb.AppendLine("			var te = (System.Enum)Enum.Parse(ft, tn, true);");
            sb.AppendLine("			((IBusinessObject)item).SetValue(field: te, newValue: newValue, fixLength: fixLength);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region ObservableCollection
            sb.AppendLine("		#region ObservableCollection");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns an observable collection that can bound to UI controls");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static System.Collections.ObjectModel.ObservableCollection<T> AsObservable<T>(this System.Collections.Generic.IEnumerable<T> list)");
            sb.AppendLine("			where T : " + this.GetLocalNamespace() + ".IReadOnlyBusinessObject");
            sb.AppendLine("		{");
            sb.AppendLine("			var retval = new System.Collections.ObjectModel.ObservableCollection<T>();");
            sb.AppendLine("			foreach (var o in list)");
            sb.AppendLine("				retval.Add(o);");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Delete Extensions
            sb.AppendLine("		#region Delete Extensions");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static void Delete<T>(this IQueryable<T> query)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			query.Delete(optimizer: null, connectionString: null);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static void Delete<T>(this IQueryable<T> query, QueryOptimizer optimizer)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			query.Delete(optimizer: optimizer, connectionString: null);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static void Delete<T>(this IQueryable<T> query, string connectionString)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			query.Delete(optimizer: new QueryOptimizer(), connectionString: connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static void  Delete<T>(this IQueryable<T> query, QueryOptimizer optimizer, string connectionString)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			if (optimizer == null)");
            sb.AppendLine("				optimizer = new QueryOptimizer();");
            sb.AppendLine();
            sb.AppendLine("         var obj1 = ((Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider)(query).Provider);");
            sb.AppendLine("         var obj2 = obj1.GetType().GetFieldInfo(\"_queryCompiler\").GetValue(obj1);");
            sb.AppendLine("         var obj3 = obj2.GetType().GetFieldInfo(\"_database\").GetValue(obj2);");
            sb.AppendLine("         var obj4 = obj3.GetType().GetRuntimeProperties().First(x => x.Name == \"Dependencies\").GetValue(obj3);");
            sb.AppendLine("         var obj5 = obj4.GetType().GetProperty(\"QueryCompilationContextFactory\").GetValue(obj4);");
            sb.AppendLine("         var obj6 = obj5.GetType().GetProperty(\"Dependencies\", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj5);");
            sb.AppendLine("         var obj7 = obj6.GetType().GetRuntimeProperty(\"CurrentContext\").GetValue(obj6) as Microsoft.EntityFrameworkCore.Internal.CurrentDbContext;");
            sb.AppendLine("         var context = obj7.Context as IContext;");
            sb.AppendLine();
            sb.AppendLine("			var sb = new System.Text.StringBuilder();");
            sb.AppendLine("			#region Per table code");

            //TODO: Row counts are not implemented for all providers so I turned them off

            var index = 0;
            foreach (var table in _model.Database.Tables
                .Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable == TypedTableConstants.None) && !x.Security.IsValid())
                .OrderBy(x => x.PascalName))
            {
                var tableName = table.DatabaseName;
                if (table.IsTenant)
                    tableName = _model.TenantPrefix + "_" + table.DatabaseName;

                var innerQueryToString = "((IQueryable<" + GetLocalNamespace() + ".Entity." + table.PascalName + ">)query).ToSql()";
                if (table.Security.IsValid())
                    innerQueryToString = "((System.Data.Entity.Core.Objects.ObjectQuery)(((System.Data.Entity.Core.Objects.ObjectQuery<" + GetLocalNamespace() + ".Entity." + table.PascalName + ">)query))).ToSql()";

                sb.AppendLine("			" + (index == 0 ? string.Empty : "else ") + "if (typeof(T) == typeof(" + GetLocalNamespace() + ".Entity." + table.PascalName + "))");
                sb.AppendLine("			{");
                if (table.ParentTable == null)
                {
                    //Single table no parent, so no special code, easy delete
                    //sb.AppendLine("				sb.AppendLine(\"set rowcount \" + optimizer.ChunkSize + \";\");");
                    sb.AppendLine("				sb.AppendLine(\"delete [X] from [" + table.GetSQLSchema() + "].[" + tableName + "] [X] inner join (\");");
                    sb.AppendLine("				sb.AppendLine(" + innerQueryToString + ");");
                    sb.AppendLine("				sb.AppendLine(\") AS [Extent2]\");");
                    sb.AppendLine("				sb.AppendLine(\"on " + string.Join(" AND ", table.PrimaryKeyColumns.Select(x => "[X].[" + x.Name + "] = [Extent2].[" + x.Name + "]").ToList()) + "\");");
                    //sb.AppendLine("				sb.AppendLine(\"select @@ROWCOUNT\");");
                }
                else
                {
                    //For parented tables the hierarchy must be deleted
                    var tableParams = string.Join(", ", table.PrimaryKeyColumns.Select(x => "[" + x.DatabaseName + "] " + x.DatabaseType));
                    sb.AppendLine("				sb.AppendLine(\"create table #t (" + tableParams + ")\");");
                    sb.AppendLine("				sb.AppendLine(\"insert into #t\");");
                    sb.AppendLine("				sb.AppendLine(" + innerQueryToString + " + \";\");");
                    sb.AppendLine("				var joinQuery = \"select " + string.Join(", ", table.PrimaryKeyColumns.Select(x => "[" + x.DatabaseName + "]")) + " from #t\";");
                    //sb.AppendLine("				sb.AppendLine(\"set rowcount \" + optimizer.ChunkSize + \";\");");

                    var allTables = table.GetParentTablesFullHierarchy().ToList();
                    allTables.Insert(0, table);
                    foreach (var tt in allTables)
                    {
                        sb.AppendLine("				sb.AppendLine(\"delete [X] from [" + tt.GetSQLSchema() + "].[" + tt.DatabaseName + "] [X] inner join (\");");
                        sb.AppendLine("				sb.AppendLine(joinQuery);");
                        sb.AppendLine("				sb.AppendLine(\") AS [Extent2]\");");
                        sb.AppendLine("				sb.AppendLine(\"on " + string.Join(" AND ", tt.PrimaryKeyColumns.Select(x => "[X].[" + x.Name + "] = [Extent2].[" + x.Name + "]").ToList()) + "\");");
                        //if (tt == table)
                        //    sb.AppendLine("				sb.AppendLine(\"select @@ROWCOUNT\");");
                    }

                    sb.AppendLine("				sb.AppendLine(\"drop table #t;\");");
                }
                sb.AppendLine("			}");
                index++;
            }
            sb.AppendLine("			else throw new Exception(\"Entity type not found\");");
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            sb.AppendLine("			QueryPreCache.AddDelete(context.InstanceKey, sb.ToString(), null, optimizer);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void Delete<T>(this DbSet<T> entitySet, Expression<Func<T, bool>> where)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			entitySet.Where(where).Delete(optimizer: null, connectionString: null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void Delete<T>(this DbSet<T> entitySet, Expression<Func<T, bool>> where, QueryOptimizer optimizer)");
            sb.AppendLine("			where T : DbSet<T>, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			entitySet.Where(where).Delete(optimizer: optimizer, connectionString: null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void Delete<T>(this DbSet<T> entitySet, Expression<Func<T, bool>> where, string connectionString)");
            sb.AppendLine("			where T : DbSet<T>, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			entitySet.Where(where).Delete(optimizer: null, connectionString: connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Update Extensions
            sb.AppendLine("		#region Update Extensions");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void Update<T>(this IQueryable<T> query, Expression<Func<T, T>> obj)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			query.Update(obj: obj, optimizer: null, connectionString: null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void Update<T>(this IQueryable<T> query, Expression<Func<T, T>> obj, QueryOptimizer optimizer)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			query.Update(obj: obj, optimizer: optimizer, connectionString: null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void Update<T>(this IQueryable<T> query, Expression<Func<T, T>> obj, string connectionString)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			query.Update(obj: obj, optimizer: null, connectionString: connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void Update<T>(this IQueryable<T> query, Expression<Func<T, T>> obj, QueryOptimizer optimizer, string connectionString)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine();
            sb.AppendLine("			if (optimizer == null)");
            sb.AppendLine("				optimizer = new QueryOptimizer();");
            sb.AppendLine();
            sb.AppendLine("			var obj1 = ((Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider)(query).Provider);");
            sb.AppendLine("			var obj2 = obj1.GetType().GetFieldInfo(\"_queryCompiler\").GetValue(obj1);");
            sb.AppendLine("			var obj3 = obj2.GetType().GetFieldInfo(\"_database\").GetValue(obj2);");
            sb.AppendLine("			var obj4 = obj3.GetType().GetFieldInfo(\"_batchExecutor\").GetValue(obj3);");
            sb.AppendLine("			var obj5 = obj4.GetType().GetRuntimeProperty(\"CurrentContext\").GetValue(obj4) as Microsoft.EntityFrameworkCore.Internal.CurrentDbContext;");
            sb.AppendLine("			var context = obj5.Context as IContext;");
            sb.AppendLine();

            sb.AppendLine("			var startTime = DateTime.Now;");
            sb.AppendLine("			var changedList = new Dictionary<string, object>();");
            sb.AppendLine();
            sb.AppendLine("			#region Parse Tree");
            sb.AppendLine("			{");
            sb.AppendLine("				var body = obj.Body;");
            sb.AppendLine("				if (body != null)");
            sb.AppendLine("				{");
            sb.AppendLine("					var propBindings = body.GetType().GetFieldInfo(\"_bindings\");");
            sb.AppendLine("					if (propBindings != null)");
            sb.AppendLine("					{");
            sb.AppendLine("						var members = (IEnumerable<System.Linq.Expressions.MemberBinding>)propBindings.GetValue(body);");
            sb.AppendLine("						foreach (System.Linq.Expressions.MemberAssignment item in members)");
            sb.AppendLine("						{");
            sb.AppendLine("							var name = item.Member.Name;");
            sb.AppendLine("							object value = null;");
            sb.AppendLine();
            sb.AppendLine("							if (item.Expression.Type == typeof(int?))");
            sb.AppendLine("								value = CompileValue<int?>(item.Expression);");
            sb.AppendLine("							else if (item.Expression.Type == typeof(int))");
            sb.AppendLine("								value = CompileValue<int>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else if (item.Expression.Type == typeof(string))");
            sb.AppendLine("								value = CompileValue<string>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else if (item.Expression.Type == typeof(bool?))");
            sb.AppendLine("								value = CompileValue<bool?>(item.Expression);");
            sb.AppendLine("							else if (item.Expression.Type == typeof(bool))");
            sb.AppendLine("								value = CompileValue<bool>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else if (item.Expression.Type == typeof(byte?))");
            sb.AppendLine("								value = CompileValue<byte?>(item.Expression);");
            sb.AppendLine("							else if (item.Expression.Type == typeof(byte))");
            sb.AppendLine("								value = CompileValue<byte>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else if (item.Expression.Type == typeof(char?))");
            sb.AppendLine("								value = CompileValue<char?>(item.Expression);");
            sb.AppendLine("							else if (item.Expression.Type == typeof(char))");
            sb.AppendLine("								value = CompileValue<char>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else if (item.Expression.Type == typeof(decimal?))");
            sb.AppendLine("								value = CompileValue<decimal?>(item.Expression);");
            sb.AppendLine("							else if (item.Expression.Type == typeof(decimal))");
            sb.AppendLine("								value = CompileValue<decimal>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else if (item.Expression.Type == typeof(double?))");
            sb.AppendLine("								value = CompileValue<double?>(item.Expression);");
            sb.AppendLine("							else if (item.Expression.Type == typeof(double))");
            sb.AppendLine("								value = CompileValue<double>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else if (item.Expression.Type == typeof(float?))");
            sb.AppendLine("								value = CompileValue<float?>(item.Expression);");
            sb.AppendLine("							else if (item.Expression.Type == typeof(float))");
            sb.AppendLine("								value = CompileValue<float>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else if (item.Expression.Type == typeof(long?))");
            sb.AppendLine("								value = CompileValue<long?>(item.Expression);");
            sb.AppendLine("							else if (item.Expression.Type == typeof(long))");
            sb.AppendLine("								value = CompileValue<long>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else if (item.Expression.Type == typeof(short?))");
            sb.AppendLine("								value = CompileValue<short?>(item.Expression);");
            sb.AppendLine("							else if (item.Expression.Type == typeof(short))");
            sb.AppendLine("								value = CompileValue<short>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else if (item.Expression.Type == typeof(DateTime?))");
            sb.AppendLine("								value = CompileValue<DateTime?>(item.Expression);");
            sb.AppendLine("							else if (item.Expression.Type == typeof(DateTime))");
            sb.AppendLine("								value = CompileValue<DateTime>(item.Expression);");
            sb.AppendLine();
            sb.AppendLine("							else");
            sb.AppendLine("								throw new Exception(\"Data type is not handled '\" + item.Expression.Type.Name + \"'\");");
            sb.AppendLine();
            sb.AppendLine("							changedList.Add(name, value);");
            sb.AppendLine("						}");
            sb.AppendLine("					}");
            sb.AppendLine("					else");
            sb.AppendLine("					{");
            sb.AppendLine("						throw new Exception(\"Update statement must be in format 'm => new Entity { Field = 0 }'\");");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            sb.AppendLine("			//Create a mapping for inheritance");
            sb.AppendLine("			var mapping = new List<UpdateSqlMapItem>();");
            sb.AppendLine("			IReadOnlyBusinessObject theObj = new T();");
            sb.AppendLine("			do");
            sb.AppendLine("			{");
            sb.AppendLine("				var md = theObj.GetMetaData();");
            sb.AppendLine("				mapping.Add(new UpdateSqlMapItem { TableName = md.GetTableName(), FieldList = md.GetFields(), Schema = md.Schema(), Metadata = md });");
            sb.AppendLine("				var newT = md.InheritsFrom();");
            sb.AppendLine("				if (newT == null)");
            sb.AppendLine("					theObj = default(T);");
            sb.AppendLine("				else");
            sb.AppendLine("					theObj = (IReadOnlyBusinessObject)Activator.CreateInstance(newT, false);");
            sb.AppendLine("			} while (theObj != null);");
            sb.AppendLine();
            sb.AppendLine("			var paramIndex = 0;");
            sb.AppendLine("			var parameters = new List<System.Data.Common.DbParameter>();");
            sb.AppendLine("			foreach (var key in changedList.Keys)");
            sb.AppendLine("			{");
            sb.AppendLine("				var map = mapping.First(x => x.FieldList.Any(z => z == key));");
            sb.AppendLine("				var fieldSql = map.SqlList;");
            sb.AppendLine("				var value = changedList[key];");
            sb.AppendLine("				if (value == null)");
            sb.AppendLine("					fieldSql.Add(\"[\" + map.Metadata.GetDatabaseFieldName(key) + \"] = NULL\");");
            sb.AppendLine("				else if (value is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					fieldSql.Add(\"[\" + map.Metadata.GetDatabaseFieldName(key) + \"] = @param\" + paramIndex);");
            sb.AppendLine("					var newParam = context.Database.GetDbConnection().CreateCommand().CreateParameter();");
            sb.AppendLine("					newParam.ParameterName = \"@param\" + paramIndex;");
            sb.AppendLine("					newParam.DbType = System.Data.DbType.String;");
            sb.AppendLine("					newParam.Value = changedList[key];");
            sb.AppendLine("					parameters.Add(newParam);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (value is DateTime)");
            sb.AppendLine("				{");
            sb.AppendLine("					fieldSql.Add(\"[\" + map.Metadata.GetDatabaseFieldName(key) + \"] = @param\" + paramIndex);");
            sb.AppendLine("					var newParam = context.Database.GetDbConnection().CreateCommand().CreateParameter();");
            sb.AppendLine("					newParam.ParameterName = \"@param\" + paramIndex;");
            sb.AppendLine("					newParam.DbType = System.Data.DbType.DateTime;");
            sb.AppendLine("					newParam.Value = changedList[key];");
            sb.AppendLine("					parameters.Add(newParam);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("				{");
            sb.AppendLine("					fieldSql.Add(\"[\" + map.Metadata.GetDatabaseFieldName(key) + \"] = @param\" + paramIndex);");
            sb.AppendLine("					var newParam = context.Database.GetDbConnection().CreateCommand().CreateParameter();");
            sb.AppendLine("					newParam.ParameterName = \"@param\" + paramIndex;");
            sb.AppendLine("					newParam.Value = changedList[key];");
            sb.AppendLine("					parameters.Add(newParam);");
            sb.AppendLine("				}");
            sb.AppendLine("				paramIndex++;");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("			var sb = new System.Text.StringBuilder();");
            sb.AppendLine("			#region Per table code");

            index = 0;
            foreach (var table in _model.Database.Tables
                .Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable == TypedTableConstants.None) && !x.Security.IsValid())
                .OrderBy(x => x.PascalName))
            {
                var tableName = table.DatabaseName;
                if (table.IsTenant)
                    tableName = _model.TenantPrefix + "_" + table.DatabaseName;

                var innerQueryToString = "((IQueryable<" + GetLocalNamespace() + ".Entity." + table.PascalName + ">)query).ToSql()";
                if (table.Security.IsValid())
                    innerQueryToString = "((System.Data.Entity.Core.Objects.ObjectQuery)(((System.Data.Entity.Core.Objects.ObjectQuery<" + GetLocalNamespace() + ".Entity." + table.PascalName + ">)query))).ToSql()";

                sb.AppendLine("			" + (index == 0 ? string.Empty : "else ") + "if (typeof(T) == typeof(" + GetLocalNamespace() + ".Entity." + table.PascalName + "))");
                sb.AppendLine("			{");
                sb.AppendLine("				sb.AppendLine(\"set rowcount \" + optimizer.ChunkSize + \";\");");
                sb.AppendLine("				foreach (var item in mapping.Where(x => x.SqlList.Any()).ToList())");
                sb.AppendLine("				{");
                sb.AppendLine("					sb.AppendLine(\"UPDATE [X] SET\");");
                sb.AppendLine("					sb.AppendLine(string.Join(\", \", item.SqlList));");
                sb.AppendLine("					sb.AppendLine(\"FROM [\" + item.Schema + \"].[\" + item.TableName + \"] AS [X] INNER JOIN (\");");
                sb.AppendLine("					sb.AppendLine(" + innerQueryToString + ");");
                sb.AppendLine("					sb.AppendLine(\") AS [Extent2]\");");
                sb.AppendLine("					sb.AppendLine(\"on " + string.Join(" AND ", table.PrimaryKeyColumns.Select(x => "[X].[" + x.Name + "] = [Extent2].[" + x.Name + "]").ToList()) + "\");");
                sb.AppendLine("				}");
                sb.AppendLine("			}");
                index++;
            }
            sb.AppendLine("			else throw new Exception(\"Entity type not found\");");
            sb.AppendLine("			#endregion");
            sb.AppendLine();

            sb.AppendLine("			QueryPreCache.AddUpdate(context.InstanceKey, sb.ToString(), parameters, optimizer);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		private class UpdateSqlMapItem");
            sb.AppendLine("		{");
            sb.AppendLine("			public string TableName { get; set; }");
            sb.AppendLine("			public List<string> FieldList { get; set; } = new List<string>();");
            sb.AppendLine("			public List<string> SqlList { get; set; } = new List<string>();");
            sb.AppendLine("			public string Schema { get; set; }");
            sb.AppendLine("			public IMetadata Metadata { get; set; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		private static T CompileValue<T>(this Expression exp)");
            sb.AppendLine("		{");
            sb.AppendLine("			var accessorExpression = Expression.Lambda<Func<T>>(exp);");
            sb.AppendLine("			var accessor = accessorExpression.Compile();");
            sb.AppendLine("			return accessor();");
            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void Update<T>(this DbSet<T> entitySet, Expression<Func<T, bool>> where, Expression<Func<T, T>> obj)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			entitySet.Where(where).Update(obj, optimizer: null, connectionString: null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void Update<T>(this DbSet<T> entitySet, Expression<Func<T, bool>> where, Expression<Func<T, T>> obj, QueryOptimizer optimizer)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			entitySet.Where(where).Update(obj, optimizer: optimizer, connectionString: null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void Update<T>(this DbSet<T> entitySet, Expression<Func<T, bool>> where, Expression<Func<T, T>> obj, string connectionString)");
            sb.AppendLine("			where T : class, " + GetLocalNamespace() + ".IBusinessObject, new()");
            sb.AppendLine("		{");
            sb.AppendLine("			entitySet.Where(where).Update(obj, optimizer: null, connectionString: connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");

            #endregion

            //Main one for base IReadOnlyBusinessObject object
            sb.AppendLine("		#region Metadata Extension Methods");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates and returns a metadata object for an entity type");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"entity\">The source class</param>");
            sb.AppendLine("		/// <returns>A metadata object for the entity types in this assembly</returns>");
            sb.AppendLine("		public static " + this.GetLocalNamespace() + ".IMetadata GetMetaData(this " + this.GetLocalNamespace() + ".IReadOnlyBusinessObject entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			var a = entity.GetType().GetTypeInfo().GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();");
            sb.AppendLine("			if (a == null) return null;");
            sb.AppendLine("			var t = ((MetadataTypeAttribute)a).MetadataClassType;");
            sb.AppendLine("			if (t == null) return null;");
            sb.AppendLine("			return Activator.CreateInstance(t) as " + this.GetLocalNamespace() + ".IMetadata;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            sb.AppendLine("	}");
            sb.AppendLine();

            #region SequentialId

            sb.AppendLine("	#region SequentialIdGenerator");
            sb.AppendLine();
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
            sb.AppendLine();
            sb.AppendLine("	#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("	#endregion");
            sb.AppendLine();
        }

        private void AppendQueryableExtensions()
        {
            sb.AppendLine("    internal static class IQueryableExtensions");
            sb.AppendLine("    {");
            sb.AppendLine("        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();");
            sb.AppendLine();
            sb.AppendLine("        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == \"_queryCompiler\");");
            sb.AppendLine();
            sb.AppendLine("        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == \"NodeTypeProvider\");");
            sb.AppendLine();
            sb.AppendLine("        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == \"CreateQueryParser\");");
            sb.AppendLine();
            sb.AppendLine("        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == \"_database\");");
            sb.AppendLine();
            sb.AppendLine("        private static readonly FieldInfo QueryCompilationContextFactoryField = typeof(Database).GetTypeInfo().DeclaredFields.Single(x => x.Name == \"_queryCompilationContextFactory\");");
            sb.AppendLine();
            sb.AppendLine("        public static string ToSql<TEntity>(this IQueryable<TEntity> query)");
            sb.AppendLine("          where TEntity : class, EFDAL.IBusinessObject, new()");
            sb.AppendLine("        {");
            sb.AppendLine("            if (!(query is EntityQueryable<TEntity>) && !(query is InternalDbSet<TEntity>))");
            sb.AppendLine("            {");
            sb.AppendLine("                throw new ArgumentException(\"Invalid query\");");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            var queryCompiler = (IQueryCompiler)QueryCompilerField.GetValue(query.Provider);");
            sb.AppendLine("            var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);");
            sb.AppendLine("            var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });");
            sb.AppendLine("            var queryModel = parser.GetParsedQuery(query.Expression);");
            sb.AppendLine("            var database = DataBaseField.GetValue(queryCompiler);");
            sb.AppendLine("            var queryCompilationContextFactory = (IQueryCompilationContextFactory)QueryCompilationContextFactoryField.GetValue(database);");
            sb.AppendLine("            var queryCompilationContext = queryCompilationContextFactory.Create(false);");
            sb.AppendLine("            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();");
            sb.AppendLine("            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);");
            sb.AppendLine("            var sql = modelVisitor.Queries.First().ToString();");
            sb.AppendLine();
            sb.AppendLine("            return sql;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");

            sb.AppendLine("    internal static class IQueryableUtils");
            sb.AppendLine("    {");
            sb.AppendLine("        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();");
            sb.AppendLine();
            sb.AppendLine("        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == \"_queryCompiler\");");
            sb.AppendLine();
            sb.AppendLine("        private static readonly FieldInfo QueryModelGeneratorField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == \"_queryModelGenerator\");");
            sb.AppendLine("        private static readonly FieldInfo queryContextFactoryField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == \"_queryContextFactory\");");
            sb.AppendLine("        private static readonly FieldInfo loggerField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == \"_logger\");");
            sb.AppendLine("        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == \"_database\");");
            sb.AppendLine();
            sb.AppendLine("        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == \"Dependencies\");");
            sb.AppendLine();
            sb.AppendLine("        //public static (string sql, IReadOnlyDictionary<string, object> parameters) ToSql2<TEntity>(this IQueryable<TEntity> query) where TEntity : class");
            sb.AppendLine("        public static string ToSql2<TEntity>(this IQueryable<TEntity> query) where TEntity : class");
            sb.AppendLine("        {");
            sb.AppendLine("            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);");
            sb.AppendLine("            var queryContextFactory = (IQueryContextFactory)queryContextFactoryField.GetValue(queryCompiler);");
            sb.AppendLine("            var logger = (Microsoft.EntityFrameworkCore.Diagnostics.IDiagnosticsLogger<DbLoggerCategory.Query>)loggerField.GetValue(queryCompiler);");
            sb.AppendLine("            var queryContext = queryContextFactory.Create();");
            sb.AppendLine("            var modelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);");
            sb.AppendLine("            var newQueryExpression = modelGenerator.ExtractParameters(logger, query.Expression, queryContext);");
            sb.AppendLine("            var queryModel = modelGenerator.ParseQuery(newQueryExpression);");
            sb.AppendLine("            var database = (IDatabase)DataBaseField.GetValue(queryCompiler);");
            sb.AppendLine("            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);");
            sb.AppendLine("            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);");
            sb.AppendLine("            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();");
            sb.AppendLine();
            sb.AppendLine("            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);");
            sb.AppendLine("            var command = modelVisitor.Queries.First().CreateDefaultQuerySqlGenerator()");
            sb.AppendLine("                .GenerateSql(queryContext.ParameterValues);");
            sb.AppendLine();
            sb.AppendLine("            //return (command.CommandText, queryContext.ParameterValues);");
            sb.AppendLine("            return command.CommandText;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");

        }

        #endregion

    }
}