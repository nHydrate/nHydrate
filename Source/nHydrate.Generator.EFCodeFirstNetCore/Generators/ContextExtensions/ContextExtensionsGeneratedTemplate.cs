#pragma warning disable 0168
using System;
using System.Linq;
using nHydrate.Generator.Models;
using System.Text;

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
        public override string FileName => _model.ProjectName + "EntitiesExtensions.Generated.cs";
        public string ParentItemName => _model.ProjectName + "EntitiesExtensions.cs";

        public override string FileContent
        {
            get
            {
                this.GenerateContent();
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
                this.AppendUsingStatements();
                sb.AppendLine($"namespace {this.GetLocalNamespace()}");
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
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Reflection;");
            sb.AppendLine();
        }

        private void AppendExtensions()
        {
            sb.AppendLine($"	#region {_model.ProjectName}EntitiesExtensions");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// Extension methods for this library");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");
            sb.AppendLine($"	public static partial class {_model.ProjectName}EntitiesExtensions");
            sb.AppendLine("	{");

            #region GetFieldType
            sb.AppendLine("		#region GetFieldType Extension Method");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Get the system type of a field of one of the contained context objects");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		public static System.Type GetFieldType(this {this.GetLocalNamespace()}.{_model.ProjectName}Entities context, Enum field)");
            sb.AppendLine("		{");
            foreach (var table in _model.Database.Tables.Where(x => !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine($"			if (field is {this.GetLocalNamespace()}.Entity.{table.PascalName}.FieldNameConstants)");
                sb.AppendLine($"				return {this.GetLocalNamespace()}.Entity.{table.PascalName}.GetFieldType(({this.GetLocalNamespace()}.Entity.{table.PascalName}.FieldNameConstants)field);");
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
            foreach (var table in _model.Database.Tables.Where(x => !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
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

            #region Many-to-Many Convenience extensions

            sb.AppendLine("        #region Many-to-Many Convenience Extensions");

            foreach (var table in _model.Database.Tables.Where(x => x.AssociativeTable))
            {
                var relations = table.GetRelationsWhereChild().ToList();
                if (relations.Count == 2)
                {
                    var relation1 = relations.First();
                    var relation2 = relations.Last();

                    sb.AppendLine("        /// <summary>");
                    sb.AppendLine($"        /// Adds a {relation1.ParentTable.PascalName} child entity to a many-to-many relationship");
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine($"        public static void Associate{relation1.ParentTable.PascalName}(this EFDAL.Entity.{relation2.ParentTable.PascalName} item, EFDAL.Entity.{relation1.ParentTable.PascalName} child)");
                    sb.AppendLine("        {");
                    sb.AppendLine($"            if (item.{table.PascalName}List == null)");
                    sb.AppendLine($"                item.{table.PascalName}List = new List<Entity.{table.PascalName}>();");
                    sb.AppendLine("            item." + table.PascalName + "List.Add(new EFDAL.Entity." + table.PascalName + " { " + relation2.ParentTable.PascalName + " = item, " + relation1.ParentTable.PascalName + " = child });");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendLine("        /// <summary>");
                    sb.AppendLine($"        /// Removes a {relation1.ParentTable.PascalName} child entity from a many-to-many relationship");
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine($"        public static void Unassociate{relation1.ParentTable.PascalName}(this EFDAL.Entity.{relation2.ParentTable.PascalName} item, EFDAL.Entity.{relation1.ParentTable.PascalName} child)");
                    sb.AppendLine("        {");
                    sb.AppendLine($"            if (item.{table.PascalName}List == null)");
                    sb.AppendLine($"                item.{table.PascalName}List = new List<Entity.{table.PascalName}>();");
                    sb.AppendLine();
                    sb.AppendLine($"            var list = item.{table.PascalName}List.Where(x => x.{relation1.ParentTable.PascalName} == child).ToList();");
                    sb.AppendLine("            foreach (var cItem in list)");
                    sb.AppendLine($"                item.{table.PascalName}List.Remove(cItem);");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendLine("        /// <summary>");
                    sb.AppendLine($"        /// Adds a {relation2.ParentTable.PascalName} child entity to a many-to-many relationship");
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine($"        public static void Associate{relation2.ParentTable.PascalName}(this EFDAL.Entity.{relation1.ParentTable.PascalName} item, EFDAL.Entity.{relation2.ParentTable.PascalName} child)");
                    sb.AppendLine("        {");
                    sb.AppendLine($"            if (item.{table.PascalName}List == null)");
                    sb.AppendLine($"                item.{table.PascalName}List = new List<Entity.{table.PascalName}>();");
                    sb.AppendLine("            item." + table.PascalName + "List.Add(new EFDAL.Entity." + table.PascalName + " { " + relation1.ParentTable.PascalName + " = item, " + relation2.ParentTable.PascalName + " = child });");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendLine("        /// <summary>");
                    sb.AppendLine($"        /// Removes a {relation2.ParentTable.PascalName} child entity from a many-to-many relationship");
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine($"        public static void Unassociate{relation2.ParentTable.PascalName}(this EFDAL.Entity.{relation1.ParentTable.PascalName} item, EFDAL.Entity.{relation2.ParentTable.PascalName} child)");
                    sb.AppendLine("        {");
                    sb.AppendLine($"            if (item.{table.PascalName}List == null)");
                    sb.AppendLine($"                item.{table.PascalName}List = new List<Entity.{table.PascalName}>();");
                    sb.AppendLine();
                    sb.AppendLine($"            var list = item.{table.PascalName}List.Where(x => x.{relation2.ParentTable.PascalName} == child).ToList();");
                    sb.AppendLine("            foreach (var cItem in list)");
                    sb.AppendLine($"                item.{table.PascalName}List.Remove(cItem);");
                    sb.AppendLine("        }");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("        #endregion");

            #endregion

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

        #endregion

    }
}