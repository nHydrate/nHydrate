#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.Data;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFCodeFirst.Generators.Contexts
{
    public class ContextMockGeneratedTemplate : EFCodeFirstBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public ContextMockGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return string.Format("{0}MockEntities.Generated.cs", _model.ProjectName); }
        }

        public string ParentItemName
        {
            get { return string.Format("{0}MockEntities.cs", _model.ProjectName); }
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
                sb.AppendLine("	#region Entity Context");
                sb.AppendLine();
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
                sb.AppendLine("	public partial class " + _model.ProjectName + "MockEntities : " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities, System.IDisposable, " + this.GetLocalNamespace() + ".IContext");
                sb.AppendLine("	{");
                sb.AppendLine();

                //Constructor
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public " + _model.ProjectName + "MockEntities()");
                sb.AppendLine("		{");

                //Create objects for type tables
                foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && x.TypedTable == TypedTableConstants.DatabaseTable).OrderBy(x => x.PascalName))
                {
                    #region Add values for type table

                    sb.AppendLine("			//Add values for " + table.PascalName);

                    foreach (var rowEntry in table.StaticData.AsEnumerable<RowEntry>())
                    {
                        var valueList = new List<string>();
                        foreach (var cellEntry in rowEntry.CellEntries.ToList())
                        {
                            var column = cellEntry.ColumnRef.Object as Column;

                            // When mocking, need to check to see if the user defined a CodeFacade for static data
                            var columnName = !string.IsNullOrWhiteSpace(column.CodeFacade) ? column.CodeFacade : column.Name;

                            var sqlValue = cellEntry.GetCodeData();
                            if (sqlValue == null) //Null is actually returned if the value can be null
                            {
                                if (!string.IsNullOrEmpty(column.Default))
                                {
                                    if (ModelHelper.IsTextType(column.DataType) || ModelHelper.IsDateType(column.DataType))
                                        valueList.Add(columnName + " = \"" + column.Default.Replace("\"", @"""") + "\"");
                                    else
                                        valueList.Add(columnName + " = " + column.Default);
                                }
                                else
                                {
                                    valueList.Add(columnName + " = null");
                                }
                            }
                            else
                            {
                                if (column.DataType == SqlDbType.Bit)
                                {
                                    sqlValue = sqlValue.ToLower().Trim();

                                    switch (sqlValue)
                                    {
                                        case "true":
                                        case "1":
                                            sqlValue = "true";
                                            break;
                                        
                                        case "false":
                                        case "0":
                                        default:
                                            sqlValue = "false";
                                            break;
                                    }
                                    
                                    valueList.Add(columnName + " = " + sqlValue);
                                }
                                else
                                {
                                    valueList.Add(columnName + " = " + sqlValue);
                                }
                            }
                        }

                        if (table.AllowCreateAudit)
                            valueList.Add(_model.Database.CreatedDateColumnName + " = new DateTime(2000, 1, 1)");
                        if (table.AllowModifiedAudit)
                            valueList.Add(_model.Database.ModifiedDateColumnName + " = new DateTime(2000, 1, 1)");
                        if (table.AllowTimestamp)
                            valueList.Add(_model.Database.TimestampPascalName + " = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }");

                        sb.AppendLine("			this." + table.PascalName + ".Add(new Entity." + table.PascalName + " { " + string.Join(", ", valueList) + " });");
                    }

                    sb.AppendLine();
                    #endregion
                }
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		private static Dictionary<string, SequentialIdGenerator> _sequentialIdGeneratorCache = new Dictionary<string, SequentialIdGenerator>();");
                sb.AppendLine("		private static object _seqCacheLock = new object();");
                sb.AppendLine();

                sb.AppendLine("		#region I" + _model.ProjectName + "Entities Members");
                sb.AppendLine();
                AppendAddDelete();
                AppendProperties();
                AppendSaveDispose();
                AppendVersioning();
                AppendGetEntityField();
                AppendSeqId();

                #region Extra
                sb.AppendLine("		#region Interface Extras");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public void ReloadItem(BaseEntity entity)");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public void DetachItem(BaseEntity entity)");
                sb.AppendLine("		{");
                var index = 0;
                foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly && !x.AssociativeTable && !x.Security.IsValid()).ToList())
                {
                    sb.AppendLine("			" + (index > 0 ? "else " : string.Empty) + "if (entity is EFDAL.Entity." + table.PascalName + ")");
                    sb.AppendLine("				this." + table.PascalName + ".Remove(entity as EFDAL.Entity." + table.PascalName + ");");
                    index++;
                }
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		#endregion");
                sb.AppendLine();
                #endregion

                sb.AppendLine("	}");
                sb.AppendLine();
                sb.AppendLine("	#endregion");
                sb.AppendLine();
                this.AppendMockSets();
                sb.AppendLine("}");
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AppendSeqId()
        {
            #region SequentialID functionality

            sb.AppendLine("		#region SequentialGuid Stuff");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void ResetSequentialGuid(EntityMappingConstants entity, string key, Guid seed)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (string.IsNullOrEmpty(key))");
            sb.AppendLine("				throw new Exception(\"Invalid key\");");
            sb.AppendLine();
            sb.AppendLine("			lock (_seqCacheLock)");
            sb.AppendLine("			{");
            sb.AppendLine("				var k = entity.ToString() + \"|\" + key;");
            sb.AppendLine("				if (!_sequentialIdGeneratorCache.ContainsKey(k))");
            sb.AppendLine("					_sequentialIdGeneratorCache.Add(k, new SequentialIdGenerator(seed));");
            sb.AppendLine("				else");
            sb.AppendLine("					_sequentialIdGeneratorCache[k].LastValue = seed;");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static Guid GetNextSequentialGuid(EntityMappingConstants entity, string key)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (string.IsNullOrEmpty(key))");
            sb.AppendLine("				throw new Exception(\"Invalid key\");");
            sb.AppendLine();
            sb.AppendLine("			lock (_seqCacheLock)");
            sb.AppendLine("			{");
            sb.AppendLine("				var k = entity.ToString() + \"|\" + key;");
            sb.AppendLine("				if (!_sequentialIdGeneratorCache.ContainsKey(k))");
            sb.AppendLine("					ResetSequentialGuid(entity, key, Guid.NewGuid());");
            sb.AppendLine("				return _sequentialIdGeneratorCache[k].NewId();");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            #region Internal Helper Class SequentialIdGenerator
            sb.AppendLine("		#region SequentialIdGenerator");
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
            sb.AppendLine("		}");
            sb.AppendLine("	#endregion");
            sb.AppendLine();
            #endregion

            #endregion
        }

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data.Entity.Core.Objects;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
        }

        private void AppendSaveDispose()
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Persists all changes made to this context since creation or load");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public int SaveChanges()");
            sb.AppendLine("		{");
            sb.AppendLine("			return 1;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Defines a method to release allocated resources.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void Dispose() { }");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendAddDelete()
        {
            #region Tables AddItem
            sb.AppendLine("		#region AddItem");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.PascalName))
            {
                var name = table.PascalName;
                if (table.Security.IsValid()) name += "__INTERNAL";

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Adds an item of type '" + table.PascalName + "' to the object context.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to add</param>");
                sb.AppendLine("		public virtual void AddItem(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + " entity)");
                sb.AppendLine("		{");
                sb.AppendLine("			this." + name + ".AddObject((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")entity);");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Adds an item of type '" + table.PascalName + "' to the object context.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to add</param>");
                sb.AppendLine("		void " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities.AddItem(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + " entity)");
                sb.AppendLine("		{");
                sb.AppendLine("			this." + name + ".AddObject(entity);");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Tables DeleteItem
            sb.AppendLine("		#region DeleteItem");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.PascalName))
            {
                var name = table.PascalName;
                if (table.Security.IsValid()) name += "__INTERNAL";

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Deletes an item of type '" + table.PascalName + "' to the object context.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to Delete</param>");
                sb.AppendLine("		public virtual void DeleteItem(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + " entity)");
                sb.AppendLine("		{");
                sb.AppendLine("			this." + name + ".DeleteObject((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")entity);");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Deletes an item of type '" + table.PascalName + "' to the object context.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to Delete</param>");
                sb.AppendLine("		void " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities.DeleteItem(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + " entity)");
                sb.AppendLine("		{");
                sb.AppendLine("			this." + name + ".DeleteObject(entity);");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion
        }

        private void AppendProperties()
        {
            #region Tables
            foreach (var item in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                var hasSecurity = item.Security.IsValid();
                var name = item.PascalName;
                if (hasSecurity) name += "__INTERNAL";

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The mock set for the '" + item.PascalName + "' entity");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		" + (hasSecurity ? "protected internal" : "public") + " MockObjectSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + name);
                sb.AppendLine("		{");
                sb.AppendLine("			get { return _" + name + " ?? (_" + name + " = new MockObjectSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">()); }");
                sb.AppendLine("			set { _" + name + " = value as MockObjectSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">; }");
                sb.AppendLine("		}");
                sb.AppendLine("		private MockObjectSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> _" + name + ";");
                sb.AppendLine();

                //Interface
                if (hasSecurity)
                {
                    var paramset = string.Join(", ", item.Security.GetParameters().ToList().Select(x => x.GetCodeType() + " " + x.CamelName).ToList());
                    sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities." + item.PascalName + "(" + paramset + ")");
                    sb.AppendLine("		{");
                    sb.AppendLine("			return this." + item.PascalName + "__INTERNAL;");
                    sb.AppendLine("		}");
                }
                else
                {
                    sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities." + item.PascalName + "");
                    sb.AppendLine("		{");
                    sb.AppendLine("			get { return this." + name + "; }");
                    sb.AppendLine("		}");
                }
                sb.AppendLine();
            }
            #endregion

            #region Views
            foreach (var item in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The mock set for the '" + item.PascalName + "' entity");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public MockObjectSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + item.PascalName + "");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return _" + item.PascalName + " ?? (_" + item.PascalName + " = new MockObjectSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">()); }");
                sb.AppendLine("			set { _" + item.PascalName + " = value as MockObjectSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">; }");
                sb.AppendLine("		}");
                sb.AppendLine("		private MockObjectSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> _" + item.PascalName + ";");
                sb.AppendLine();

                //Interface
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities." + item.PascalName);
                sb.AppendLine("		{");
                sb.AppendLine("			get { return (IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">)this." + item.PascalName + "; }");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            #region Stored Procs
            foreach (var item in _model.Database.CustomStoredProcedures.Where(x => x.Generated && x.GeneratedColumns.Count > 0).OrderBy(x => x.PascalName))
            {
                //Interface
                var paramset = item.GetParameters().Where(x => x.Generated).ToList();
                var paramString1 = string.Join(", ", paramset.Select(x => (x.IsOutputParameter ? "out " : "") + x.GetCodeType(true) + " " + x.CamelName).ToList());
                var paramString2 = string.Join(", ", paramset.Select(x => (x.IsOutputParameter ? "out " : "") + x.CamelName).ToList());

                sb.AppendLine("		//TODO: This is not right");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + item.PascalName + "(" + paramString1 + ")");
                sb.AppendLine("		{");
                sb.AppendLine("			return (IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">)this." + item.PascalName + "(" + paramString2 + ");");
                sb.AppendLine("		}");
                sb.AppendLine();

                //sb.AppendLine("		/// <summary />");
                //sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities." + item.PascalName + "(" + paramString1 + ")");
                //sb.AppendLine("		{");
                //sb.AppendLine("			return (IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">)this." + item.PascalName + "(" + paramString2 + ");");
                //sb.AppendLine("		}");
                //sb.AppendLine();
            }
            #endregion

            #region Functions
            foreach (var item in _model.Database.Functions.Where(x => x.Generated && x.IsTable).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		//TODO: This is not right");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		/// " + item.Description);
                sb.AppendLine("		/// </summary>");
                sb.Append("		public virtual IQueryable<" + GetLocalNamespace() + ".Entity." + item.PascalName + "> " + item.PascalName + "(");
                var parameterList = item.GetParameters().Where(x => x.Generated).ToList();
                foreach (var parameter in parameterList)
                {
                    if (parameter.IsOutputParameter) sb.Append("out ");
                    sb.Append(parameter.GetCodeType() + " " + parameter.CamelName);
                    if (parameterList.IndexOf(parameter) < parameterList.Count - 1)
                        sb.Append(", ");
                }
                sb.AppendLine(")");
                sb.AppendLine("		{");
                sb.AppendLine("			return (new List<" + GetLocalNamespace() + ".Entity." + item.PascalName + ">()).AsQueryable();");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion
        }

        private void AppendVersioning()
        {
            sb.AppendLine("		#region Version");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the version of the model that created this library.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual string Version");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return \"" + _model.Version + "." + _model.GeneratedVersion + "\"; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the key of the model that created this library.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual string ModelKey");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return \"" + _model.Key + "\"; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if the API matches the database connection");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public virtual bool IsValidConnection()");
            sb.AppendLine("		{");
            sb.AppendLine("			return IsValidConnection(true);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if the API matches the database connection");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"checkVersion\">Determines if the check also includes the exact version of the model</param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public virtual bool IsValidConnection(bool checkVersion)");
            sb.AppendLine("		{");
            sb.AppendLine("			return true;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendGetEntityField()
        {
            sb.AppendLine("		#region GetEntityFromField");
            sb.AppendLine();

            sb.AppendLine("		Enum " + this.GetLocalNamespace() + ".IContext.GetEntityFromField(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetEntityFromField(field);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		object " + this.GetLocalNamespace() + ".IContext.GetMetaData(Enum entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetMetaData((EntityMappingConstants)entity);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		System.Type " + this.GetLocalNamespace() + ".IContext.GetFieldType(Enum field)");
            sb.AppendLine("		{");

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("			if (field is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants)");
                sb.AppendLine("			{");
                sb.AppendLine("				switch ((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants)field)");
                sb.AppendLine("				{");
                foreach (var column in table.GeneratedColumnsFullHierarchy)
                {
                    sb.AppendLine("					case " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants." + column.PascalName + ": return typeof(" + column.GetCodeType() + ");");
                }
                sb.AppendLine("				}");
                sb.AppendLine("			}");
            }
            sb.AppendLine("			return null;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the entity from one of its fields");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static EntityMappingConstants GetEntityFromField(Enum field)");
            sb.AppendLine("		{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("			if (field is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".FieldNameConstants) return " + this.GetLocalNamespace() + ".EntityMappingConstants." + table.PascalName + ";");
            }
            sb.AppendLine("			throw new Exception(\"Unknown field type!\");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the meta data object for an entity");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static " + this.GetLocalNamespace() + ".IMetadata GetMetaData(" + this.GetLocalNamespace() + ".EntityMappingConstants table)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (table)");
            sb.AppendLine("			{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.Append("				case " + this.GetLocalNamespace() + ".EntityMappingConstants." + table.PascalName + ": ");
                sb.AppendLine("return new " + this.GetLocalNamespace() + ".Entity.Metadata." + table.PascalName + "Metadata();");
            }
            sb.AppendLine("			}");
            sb.AppendLine("			throw new Exception(\"Entity not found!\");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendMockSets()
        {
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// This object is used to mock entity sets on the mock context");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	public partial class MockObjectSet<T> : IObjectSet<T> where T : class");
            sb.AppendLine("	{");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// ");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected HashSet<T> _data;");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// ");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected IQueryable _query;");
            sb.AppendLine();
            sb.AppendLine("		#region Constructor");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Default constructor for MockObjectSet");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public MockObjectSet() : this(new List<T>()) { }");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// ");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public MockObjectSet(IEnumerable<T> initialData)");
            sb.AppendLine("		{");
            sb.AppendLine("			_data = new HashSet<T>(initialData);");
            sb.AppendLine("			_query = _data.AsQueryable();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region Methods");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Adds an item to this list");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void Add(T item)");
            sb.AppendLine("		{");
            sb.AppendLine("			_data.Add(item);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Adds an item to this list");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void AddObject(T item)");
            sb.AppendLine("		{");
            sb.AppendLine("			_data.Add(item);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Removes an item to this list");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void Remove(T item)");
            sb.AppendLine("		{");
            sb.AppendLine("			_data.Remove(item);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Removes an item to this list");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void DeleteObject(T item)");
            sb.AppendLine("		{");
            sb.AppendLine("			_data.Remove(item);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Attaches an item to this list");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void Attach(T item)");
            sb.AppendLine("		{");
            sb.AppendLine("			_data.Add(item);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Detaches an item to this list");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void Detach(T item)");
            sb.AppendLine("		{");
            sb.AppendLine("			_data.Remove(item);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region Base Functionality");
            sb.AppendLine();
            sb.AppendLine("		Type IQueryable.ElementType");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _query.ElementType; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		System.Linq.Expressions.Expression IQueryable.Expression");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _query.Expression; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		IQueryProvider IQueryable.Provider");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _query.Provider; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()");
            sb.AppendLine("		{");
            sb.AppendLine("			return _data.GetEnumerator();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		IEnumerator<T> IEnumerable<T>.GetEnumerator()");
            sb.AppendLine("		{");
            sb.AppendLine("			return _data.GetEnumerator();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("	}");
            sb.AppendLine();
        }

        #endregion

    }
}