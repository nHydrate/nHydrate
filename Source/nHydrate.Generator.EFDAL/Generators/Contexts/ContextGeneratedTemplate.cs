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
using System.Linq;
using System.Text;
using nHydrate.Generator.Common;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.EFDAL.Generators.Contexts
{
    public class ContextGeneratedTemplate : EFDALBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public ContextGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return _model.ProjectName + "Entities.Generated.cs"; }
        }

        public string ParentItemName
        {
            get { return _model.ProjectName + "Entities.cs"; }
        }

        public override string FileContent
        {
            get
            {
                try
                {
                    GenerateContent();
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.ToString());
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
                sb.AppendLine("#pragma warning disable 0168");
                this.AppendUsingStatements();
                AppendEDMMetaData();
                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");
                this.AppendTableMapping();
                //this.AppendTypeTableEnums();
                this.AppendClass();
                sb.AppendLine("}");
                sb.AppendLine();

                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
                sb.AppendLine("{");
                //this.AppendTypeTableWrappers();
                sb.AppendLine("}");
                sb.AppendLine("#pragma warning restore 0168");
                sb.AppendLine();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AppendTableMapping()
        {
            sb.AppendLine("	#region EntityMappingConstants Enumeration");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// A map for all entity types in this library");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	public enum EntityMappingConstants");
            sb.AppendLine("	{");

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// A mapping for the the " + table.PascalName + " entity");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		" + table.PascalName + ",");
            }

            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	#endregion");
            sb.AppendLine();
        }

        private void AppendEDMMetaData()
        {
            try
            {
                sb.AppendLine("[assembly: EdmSchemaAttribute()]");
                sb.AppendLine("#region EDM Relationship Metadata");
                sb.AppendLine();

                foreach (var relation in _model.Database.Relations.Where(x => x.IsGenerated && x.IsValidEFRelation))
                {
                    var parentTable = relation.ParentTable;
                    var childTable = relation.ChildTable;
                    if (
                        parentTable.Generated &&
                        childTable.Generated &&
                        !childTable.AssociativeTable &&
                        (!childTable.IsInheritedFrom(parentTable)) &&
                        (parentTable.TypedTable != TypedTableConstants.EnumOnly) &&
                        (childTable.TypedTable != TypedTableConstants.EnumOnly))
                    {
                        if (relation.IsOneToOne && relation.AreAllFieldsPK)
                            sb.AppendLine("[assembly: EdmRelationshipAttribute(\"" + this.GetLocalNamespace() + ".Entity" + "\", \"FK_" + relation.PascalRoleName + "_" + childTable.PascalName + "_" + parentTable.PascalName + "\", \"" + relation.PascalRoleName + parentTable.PascalName + "\", System.Data.Metadata.Edm.RelationshipMultiplicity." + (relation.IsRequired ? "One" : "ZeroOrOne") + ", typeof(" + this.GetLocalNamespace() + ".Entity." + parentTable.PascalName + "), \"" + relation.PascalRoleName + childTable.PascalName + "\", System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + "), true)]");
                        else
                            sb.AppendLine("[assembly: EdmRelationshipAttribute(\"" + this.GetLocalNamespace() + ".Entity" + "\", \"FK_" + relation.PascalRoleName + "_" + childTable.PascalName + "_" + parentTable.PascalName + "\", \"" + relation.PascalRoleName + parentTable.PascalName + "\", System.Data.Metadata.Edm.RelationshipMultiplicity." + (relation.IsRequired ? "One" : "ZeroOrOne") + ", typeof(" + this.GetLocalNamespace() + ".Entity." + parentTable.PascalName + "), \"" + relation.PascalRoleName + childTable.PascalName + "List\", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + "), true)]");
                    }
                }

                foreach (var relation in _model.Database.ViewRelations.Where(x => x.IsGenerated && x.IsValidEFRelation))
                {
                    var parentTable = relation.ParentTable;
                    var childTable = relation.ChildView;
                    if (
                        parentTable.Generated &&
                        childTable.Generated &&
                        (parentTable.TypedTable != TypedTableConstants.EnumOnly))
                    {
                        sb.AppendLine("[assembly: EdmRelationshipAttribute(\"" + this.GetLocalNamespace() + ".Entity" + "\", \"FK_" + relation.PascalRoleName + "_" + childTable.PascalName + "_" + parentTable.PascalName + "\", \"" + relation.PascalRoleName + parentTable.PascalName + "\", System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(" + this.GetLocalNamespace() + ".Entity." + parentTable.PascalName + "), \"" + relation.PascalRoleName + childTable.PascalName + "List\", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + "), true)]");
                    }
                }

                //Add the associative table links
                foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
                {
                    var associativeRelations = table.GetRelationsWhereChild().ToList();
                    if (associativeRelations.Count == 0)
                    {
                        System.Diagnostics.Debug.Write(string.Empty);
                    }
                    else
                    {
                        var relation1 = associativeRelations.First();
                        var relation2 = associativeRelations.Last();
                        var table1 = relation1.ParentTableRef.Object as Table;
                        var table2 = relation2.ParentTableRef.Object as Table;
                        //if (table1.Generated && table2.Generated && !table1.IsTypeTable && !table2.IsTypeTable)
                        if (table1.Generated && table2.Generated)
                            sb.AppendLine("[assembly: EdmRelationshipAttribute(\"" + this.GetLocalNamespace() + ".Entity" + "\", \"" + table.PascalName + "\", \"" + relation1.PascalRoleName + table1.PascalName + "List\", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(" + this.GetLocalNamespace() + ".Entity." + table1.PascalName + "), \"" + relation2.PascalRoleName + table2.PascalName + "List\", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(" + this.GetLocalNamespace() + ".Entity." + table2.PascalName + "))]");
                    }
                }

                sb.AppendLine();
                sb.AppendLine("#endregion");
                sb.AppendLine();
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
            sb.AppendLine("using System.Data.Objects;");
            sb.AppendLine("using System.Data.Objects.DataClasses;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using " + this.GetLocalNamespace() + ".Entity;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine();
        }

        private void AppendClass()
        {
            sb.AppendLine("	#region Entity Context");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// There context " + _model.ProjectName + "Entities");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.AppendLine("	public partial class " + _model.ProjectName + "Entities : System.Data.Objects.ObjectContext, " + this.GetLocalNamespace() + ".Interfaces.I" + _model.ProjectName + "Entities, nHydrate.EFCore.DataAccess.IContext");
            sb.AppendLine("	{");

            //Create the modifier property
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The audit modifier used to mark database edits");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected ContextStartup _contextStartup = new ContextStartup(null);");
            sb.AppendLine();

            sb.AppendLine("		private static Dictionary<string, SequentialIdGenerator> _sequentialIdGeneratorCache = new Dictionary<string, SequentialIdGenerator>();");
            sb.AppendLine("		private static object _seqCacheLock = new object();");
            sb.AppendLine();

            // Create consts for version and modelKey
            sb.AppendLine("		private const string _version = \"" + _model.Version + "." + _model.GeneratedVersion + "\";");
            sb.AppendLine("		private const string _modelKey = \"" + _model.Key + "\";");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initializes a new " + _model.ProjectName + "Entities object using the connection string found in the '" + _model.ProjectName + "Entities' section of the application configuration file.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + _model.ProjectName + "Entities() :");
            sb.AppendLine("			base(Util.ConvertNormalCS2EFFromConfig(\"name=" + _model.ProjectName + "Entities\"), \"" + _model.ProjectName + "Entities\")");
            sb.AppendLine("		{");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				var builder = new System.Data.Odbc.OdbcConnectionStringBuilder(Util.StripEFCS2Normal(this.Connection.ConnectionString));");
            sb.AppendLine("				var timeoutValue = \"30\";");
            sb.AppendLine("				if (builder.ContainsKey(\"connect timeout\"))");
            sb.AppendLine("					timeoutValue = (string) builder[\"connect timeout\"];");
            sb.AppendLine("				else if (builder.ContainsKey(\"connection timeout\"))");
            sb.AppendLine("					timeoutValue = (string) builder[\"connection timeout\"];");
            sb.AppendLine("				var v = Convert.ToInt32(timeoutValue);");
            sb.AppendLine("				if (v > 0)");
            sb.AppendLine("					this.CommandTimeout = v;");
            sb.AppendLine("			}");
            sb.AppendLine("			catch { }");
            sb.AppendLine("			this.OnContextCreated();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initialize a new " + _model.ProjectName + "Entities object with an audit modifier.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + _model.ProjectName + "Entities(ContextStartup contextStartup) :");
            sb.AppendLine("			base(Util.ConvertNormalCS2EFFromConfig(\"name=" + _model.ProjectName + "Entities\", contextStartup), \"" + _model.ProjectName + "Entities\")");
            sb.AppendLine("		{");
            sb.AppendLine("			_contextStartup = contextStartup;");
            sb.AppendLine("			this.ContextOptions.LazyLoadingEnabled = contextStartup.AllowLazyLoading;");
            sb.AppendLine("			this.CommandTimeout = contextStartup.CommandTimeout;");
            sb.AppendLine("			this.OnContextCreated();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initialize a new " + _model.ProjectName + "Entities object with an audit modifier.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + _model.ProjectName + "Entities(ContextStartup contextStartup, string connectionString) :");
            sb.AppendLine("			base(Util.ConvertNormalCS2EF(connectionString, contextStartup), \"" + _model.ProjectName + "Entities\")");
            sb.AppendLine("		{");
            sb.AppendLine("			_contextStartup = contextStartup;");
            sb.AppendLine("			this.ContextOptions.LazyLoadingEnabled = contextStartup.AllowLazyLoading;");
            sb.AppendLine("			this.CommandTimeout = contextStartup.CommandTimeout;");
            sb.AppendLine("			this.OnContextCreated();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initialize a new " + _model.ProjectName + "Entities object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + _model.ProjectName + "Entities(string connectionString) :");
            sb.AppendLine("			base(Util.ConvertNormalCS2EF(connectionString), \"" + _model.ProjectName + "Entities\")");
            sb.AppendLine("		{");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				var builder = new System.Data.Odbc.OdbcConnectionStringBuilder(Util.StripEFCS2Normal(this.Connection.ConnectionString));");
            sb.AppendLine("				var timeoutValue = \"30\";");
            sb.AppendLine("				if (builder.ContainsKey(\"connect timeout\"))");
            sb.AppendLine("					timeoutValue = (string) builder[\"connect timeout\"];");
            sb.AppendLine("				else if (builder.ContainsKey(\"connection timeout\"))");
            sb.AppendLine("					timeoutValue = (string) builder[\"connection timeout\"];");
            sb.AppendLine("				var v = Convert.ToInt32(timeoutValue);");
            sb.AppendLine("				if (v > 0)");
            sb.AppendLine("					this.CommandTimeout = v;");
            sb.AppendLine("			}");
            sb.AppendLine("			catch { }");
            sb.AppendLine("			this.OnContextCreated();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initialize a new " + _model.ProjectName + "Entities object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + _model.ProjectName + "Entities(System.Data.EntityClient.EntityConnection connection) :");
            sb.AppendLine("			base(connection, \"" + _model.ProjectName + "Entities\")");
            sb.AppendLine("		{");
            sb.AppendLine("			this.OnContextCreated();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		partial void OnContextCreated();");
            sb.AppendLine();

            #region SequentialID functionality
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
            sb.AppendLine();
            #endregion

            #region Tables
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// ");
                if (!string.IsNullOrEmpty(table.Description))
                {
                    StringHelper.LineBreakCode(sb, table.Description, "	/// ");
                }
                sb.AppendLine("		/// Entity for the '" + table.DatabaseName + "' database table");

                var outbondRelationList = table.GetRelations().ToList();
                if (outbondRelationList.Any())
                {
                    sb.AppendLine("		/// <para> </para>"); //char 255 - not space
                    sb.AppendLine("		/// <para>Outbound Relations</para>");
                    foreach (var relation in outbondRelationList)
                    {
                        sb.AppendLine("		/// <para>" + relation.ParentTable.PascalName + " -> " + relation.ChildTable.PascalName + "</para>");
                    }
                }

                var inbondRelationList = table.GetRelationsWhereChild().ToList();
                if (inbondRelationList.Any())
                {
                    sb.AppendLine("		/// <para> </para>"); //char 255 - not space
                    sb.AppendLine("		/// <para>Inbound Relations</para>");
                    foreach (var relation in inbondRelationList)
                    {
                        sb.AppendLine("		/// <para>" + relation.ParentTable.PascalName + " -> " + relation.ChildTable.PascalName + "</para>");
                    }
                }

                sb.AppendLine("		/// </summary>");

                if (table.ParentTable != null)
                {
                    sb.AppendLine("		public virtual System.Data.Objects.ObjectQuery<" + table.PascalName + "> " + table.PascalName + "");
                    sb.AppendLine("		{");
                    sb.AppendLine("			get { return this." + table.GetAbsoluteBaseTable().PascalName + ".OfType<" + table.PascalName + ">(); }");
                    sb.AppendLine("		}");
                }
                else
                {
                    sb.AppendLine("		public virtual System.Data.Objects.ObjectSet<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> " + table.PascalName + "");
                    sb.AppendLine("		{");
                    sb.AppendLine("			get");
                    sb.AppendLine("			{");
                    sb.AppendLine("				lock (_locker" + table.CamelName + ")");
                    sb.AppendLine("				{");
                    sb.AppendLine("					if ((this._" + table.CamelName + " == null))");
                    sb.AppendLine("					{");
                    sb.AppendLine("						this._" + table.CamelName + " = base.CreateObjectSet<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">(\"" + table.PascalName + "\");");
                    sb.AppendLine("					}");
                    sb.AppendLine("					return this._" + table.CamelName + ";");
                    sb.AppendLine("				}");
                    sb.AppendLine("			}");
                    sb.AppendLine("		}");
                    sb.AppendLine();
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// The threading locker object to use when accessing the " + table.PascalName + " entity");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		private object _locker" + table.CamelName + " = new object();");
                    sb.AppendLine();
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// The internal reference variable for the '" + table.PascalName + "' object set");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		protected System.Data.Objects.ObjectSet<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> _" + table.CamelName + ";");
                }
                sb.AppendLine();
            }
            #endregion

            #region Views
            foreach (var view in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                if (!string.IsNullOrEmpty(view.Description))
                    StringHelper.LineBreakCode(sb, view.Description, "		/// ");
                sb.AppendLine("		/// </summary>");

                sb.AppendLine("		public virtual System.Data.Objects.ObjectSet<" + this.GetLocalNamespace() + ".Entity." + view.PascalName + "> " + view.PascalName + "");
                sb.AppendLine("		{");
                sb.AppendLine("			get");
                sb.AppendLine("			{");
                sb.AppendLine("				lock (_locker" + view.CamelName + ")");
                sb.AppendLine("				{");
                sb.AppendLine("					if ((this._" + view.CamelName + " == null))");
                sb.AppendLine("					{");
                sb.AppendLine("						this._" + view.CamelName + " = base.CreateObjectSet<" + this.GetLocalNamespace() + ".Entity." + view.PascalName + ">(\"" + view.PascalName + "\");");
                sb.AppendLine("					}");
                sb.AppendLine("					return this._" + view.CamelName + ";");
                sb.AppendLine("				}");
                sb.AppendLine("			}");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		private object _locker" + view.CamelName + " = new object();");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The internal reference variable for the '" + view.PascalName + "' object set");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		protected System.Data.Objects.ObjectSet<" + this.GetLocalNamespace() + ".Entity." + view.PascalName + "> _" + view.CamelName + ";");
                sb.AppendLine();
            }
            #endregion

            #region Scaler Functions

            foreach (var function in _model.Database.Functions.Where(x => !x.IsTable))
            {
                var returnColumn = function.GeneratedColumns.FirstOrDefault() as FunctionColumn;
                var parameterList = function.GetParameters().Where(x => x.Generated).ToList();

                sb.AppendLine("		/// <summary>");
                if (!string.IsNullOrEmpty(function.Description))
                    StringHelper.LineBreakCode(sb, function.Description, "		/// ");
                sb.AppendLine("		/// </summary>");
                sb.Append("		public " + returnColumn.GetCodeType() + " " + function.PascalName + "(");
                sb.Append(string.Join(", ", parameterList.Select(x => x.GetCodeType() + " " + x.CamelName).ToList().ToArray()));
                sb.AppendLine(")");
                sb.AppendLine("		{");
                sb.AppendLine("			var paramArray = new List<System.Data.Objects.ObjectParameter>();");

                foreach (var parameter in parameterList)
                {
                    if (parameter.AllowNull)
                    {
                        sb.AppendLine("			System.Data.Objects.ObjectParameter " + parameter.CamelName + "Parameter = null;");
                        sb.AppendLine("			if (" + parameter.CamelName + " != null) { " + parameter.CamelName + "Parameter = new System.Data.Objects.ObjectParameter(\"" + parameter.DatabaseName + "\", " + parameter.CamelName + "); }");
                        sb.AppendLine("			else { " + parameter.CamelName + "Parameter = new System.Data.Objects.ObjectParameter(\"" + parameter.DatabaseName + "\", typeof(" + parameter.GetCodeType() + ")); }");
                        sb.AppendLine("			paramArray.Add(" + parameter.CamelName + "Parameter);");
                    }
                    else
                    {
                        sb.AppendLine("			paramArray.Add(new System.Data.Objects.ObjectParameter(\"" + parameter.DatabaseName + "\", " + parameter.CamelName + "));");
                    }
                }

                sb.Append("			return this.CreateQuery<" + returnColumn.GetCodeType() + ">(\"SELECT VALUE " + this.DefaultNamespace + ".EFDAL.Store." + function.DatabaseName + "(");
                sb.Append(string.Join(", ", parameterList.Select(x => "@" + x.DatabaseName).ToList().ToArray()));
                sb.AppendLine(") FROM {1}\", paramArray.ToArray()).First();");
                sb.AppendLine("		}");
                sb.AppendLine();
            }

            #endregion

            #region Add Functionality
            //Add an strongly-typed extension for "AddItem" method
            sb.AppendLine("		#region AddItem Methods");
            sb.AppendLine();

            #region Tables
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Adds an entity of type '" + table.PascalName + "' to the object context");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to add</param>");
                sb.AppendLine("		public virtual void AddItem(" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + table.PascalName + " entity)");
                sb.AppendLine("		{");

                sb.AppendLine("			if (entity is " + GetLocalNamespace() + ".Entity." + table.PascalName + ")");
                sb.AppendLine("			{");
                if (table.AllowCreateAudit)
                    sb.AppendLine("				((" + GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + _model.Database.CreatedByPascalName + " = _contextStartup.Modifer;");
                if (table.AllowModifiedAudit)
                    sb.AppendLine("				((" + GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + _model.Database.ModifiedByPascalName + " = _contextStartup.Modifer;");
                sb.AppendLine("			}");

                if (table.ParentTable == null)
                    sb.AppendLine("			base.AddObject(\"" + table.PascalName + "\", entity);");
                else
                    sb.AppendLine("			base.AddObject(\"" + table.GetAbsoluteBaseTable().PascalName + "\", entity);");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            #region Views
            foreach (var view in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Adds an entity of type '" + view.PascalName + "' to the object context.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to add</param>");
                sb.AppendLine("		public virtual void AddItem(" + this.GetLocalNamespace() + ".Entity." + view.PascalName + " entity)");
                sb.AppendLine("		{");
                sb.AppendLine("			base.AddObject(\"" + view.PascalName + "\", entity);");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            //Overload the original signature as an error
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Adds an object to the object context.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[Obsolete(\"This method signature is no longer used. Use the AddItem method.\", true)]");
            sb.AppendLine("		[System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]");
            sb.AppendLine("		public new void AddObject(string entitySetName, object entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			throw new Exception(\"This method signature is no longer used. Use the AddItem method.\");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Delete Functionality
            //Add an strongly-typed extension for "DeleteItem" method
            sb.AppendLine("		#region DeleteItem Methods");
            sb.AppendLine();

            #region Tables
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Deletes an entity of type '" + table.PascalName + "'");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to delete</param>");
                sb.AppendLine("		public virtual void DeleteItem(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + " entity)");
                sb.AppendLine("		{");
                sb.AppendLine("			if (entity == null) return;");
                sb.AppendLine("			this." + table.GetAbsoluteBaseTable().PascalName + ".DeleteObject(entity);");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Deletes an entity of type '" + table.PascalName + "'");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to delete</param>");
                sb.AppendLine("		void " + this.DefaultNamespace + ".EFDAL.Interfaces.I" + _model.ProjectName + "Entities.DeleteItem(" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + table.PascalName + " entity)");
                sb.AppendLine("		{");

                sb.AppendLine("			if (entity == null) return;");
                sb.AppendLine("			if (entity is " + GetLocalNamespace() + ".Entity." + table.PascalName + ")");
                sb.AppendLine("			{");
                if (table.AllowCreateAudit)
                    sb.AppendLine("				((" + GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + _model.Database.CreatedByPascalName + " = _contextStartup.Modifer;");
                if (table.AllowModifiedAudit)
                    sb.AppendLine("				((" + GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + _model.Database.ModifiedByPascalName + " = _contextStartup.Modifer;");
                sb.AppendLine("			}");

                if (table.ParentTable == null)
                    sb.AppendLine("			base.DeleteObject(entity);");
                else
                    sb.AppendLine("			base.DeleteObject(entity);");

                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Marks an object for deletion.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[Obsolete(\"This method signature is no longer used. Use the AddItem method.\", true)]");
            sb.AppendLine("		[System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]");
            sb.AppendLine("		public new void DeleteObject(object entity)");
            sb.AppendLine("		{");

            var index = 0;
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable && (x.TypedTable != TypedTableConstants.EnumOnly)))
            {
                sb.AppendLine("			" + (index > 0 ? "else " : string.Empty) + "if (entity is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")");
                sb.AppendLine("				this.DeleteItem(entity as " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ");");
                index++;
            }

            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Connection String

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the connection string used for this context object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public string ConnectionString");
            sb.AppendLine("		{");
            sb.AppendLine("			get");
            sb.AppendLine("			{");
            sb.AppendLine("				try");
            sb.AppendLine("				{");
            sb.AppendLine("					if (this.Connection != null && !string.IsNullOrEmpty(this.Connection.ConnectionString))");
            sb.AppendLine("					{");
            sb.AppendLine("						return Util.StripEFCS2Normal(this.Connection.ConnectionString);");
            sb.AppendLine("					}");
            sb.AppendLine("					else");
            sb.AppendLine("					{");
            sb.AppendLine("						return null;");
            sb.AppendLine("					}");
            sb.AppendLine();
            sb.AppendLine("				}");
            sb.AppendLine("				catch (Exception ex)");
            sb.AppendLine("				{");
            sb.AppendLine("					return null;");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the globally configured connection string for this context type");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		internal static string GetConnectionString()");
            sb.AppendLine("		{");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				var a = System.Configuration.ConfigurationManager.ConnectionStrings[\"" + _model.ProjectName + "Entities\"];");
            sb.AppendLine("				if (a != null)");
            sb.AppendLine("				{");
            sb.AppendLine("					var s = a.ConnectionString;");
            sb.AppendLine("					var regEx = new System.Text.RegularExpressions.Regex(\"provider connection string\\\\s*=\\\\s*\\\"([^\\\"]*)\");");
            sb.AppendLine("					var m = regEx.Match(s);");
            sb.AppendLine("					var connString = s;");
            sb.AppendLine("					if (m != null && m.Groups.Count > 1)");
            sb.AppendLine("					{");
            sb.AppendLine("						connString = m.Groups[1].Value;");
            sb.AppendLine("					}");
            sb.AppendLine("					return connString;");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"The connection string was not found.\");");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			catch (Exception ex)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(\"The connection string was not found.\");");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The global settings of this context");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public ContextStartup ContextStartup");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _contextStartup; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            #region Context Interface Members
            sb.AppendLine("		#region I" + _model.ProjectName + " Members");
            sb.AppendLine();

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Security.IsValid() && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		System.Linq.IQueryable<" + this.GetLocalNamespace() + ".Interfaces.Entity.I" + table.PascalName + "> " + this.GetLocalNamespace() + ".Interfaces.I" + _model.ProjectName + "Entities." + table.PascalName);
                sb.AppendLine("		{");
                sb.AppendLine("			get { return (System.Linq.IQueryable<" + this.GetLocalNamespace() + ".Interfaces.Entity.I" + table.PascalName + ">)this." + table.PascalName + "; }");
                sb.AppendLine("		}");
                sb.AppendLine();
            }

            #region Stored Procedures
            foreach (var storedProcedure in _model.Database.CustomStoredProcedures.Where(x => x.Generated && x.GeneratedColumns.Count > 0).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// ");
                sb.AppendLine("		/// </summary>");
                sb.Append("		public IEnumerable<" + storedProcedure.PascalName + "> " + storedProcedure.PascalName + "(");
                var parameterList = storedProcedure.GetParameters().Where(x => x.Generated).ToList();
                foreach (var parameter in parameterList)
                {
                    if (parameter.IsOutputParameter) sb.Append("out ");
                    sb.Append(parameter.GetCodeType() + " " + parameter.CamelName);
                    if (parameterList.IndexOf(parameter) < parameterList.Count() - 1)
                        sb.Append(", ");
                }
                sb.AppendLine(")");
                sb.AppendLine("		{");

                foreach (var parameter in parameterList)
                {
                    if (parameter.IsOutputParameter)
                    {
                        sb.AppendLine("			var " + parameter.CamelName + "Parameter = new ObjectParameter(\"" + parameter.DatabaseName + "\", typeof(" + parameter.GetCodeType() + "));");
                    }
                    else if (parameter.AllowNull)
                    {
                        sb.AppendLine("			ObjectParameter " + parameter.CamelName + "Parameter = null;");
                        sb.AppendLine("			if (" + parameter.CamelName + " != null) { " + parameter.CamelName + "Parameter = new ObjectParameter(\"" + parameter.DatabaseName + "\", " + parameter.CamelName + "); }");
                        sb.AppendLine("			else { " + parameter.CamelName + "Parameter = new ObjectParameter(\"" + parameter.DatabaseName + "\", typeof(" + parameter.GetCodeType() + ")); }");
                    }
                    else
                    {
                        sb.AppendLine("			var " + parameter.CamelName + "Parameter = new ObjectParameter(\"" + parameter.DatabaseName + "\", " + parameter.CamelName + ");");
                    }
                }

                sb.Append("			var retval = base.ExecuteFunction<" + storedProcedure.PascalName + ">(\"" + storedProcedure.GetDatabaseObjectName() + "\"");

                if (parameterList.Count > 0)
                {
                    sb.Append(", ");
                    foreach (var parameter in parameterList)
                    {
                        sb.Append(parameter.CamelName + "Parameter");
                        if (parameterList.IndexOf(parameter) < parameterList.Count() - 1)
                            sb.Append(", ");
                    }
                }

                sb.AppendLine(").ToList();");

                //Add code here to handle output parameters
                foreach (var parameter in parameterList.Where(x => x.IsOutputParameter))
                {
                    sb.AppendLine("			" + parameter.CamelName + " = (" + parameter.GetCodeType() + ")" + parameter.CamelName + "Parameter.Value;");
                }

                sb.AppendLine("			return retval;");

                sb.AppendLine("		}");
                sb.AppendLine();
            }

            //Stoed procs that are Action (no columns)
            foreach (var storedProcedure in _model.Database.CustomStoredProcedures.Where(x => x.Generated && x.GeneratedColumns.Count == 0).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Calls the " + storedProcedure.PascalName + " action");
                sb.AppendLine("		/// </summary>");
                sb.Append("		public void " + storedProcedure.PascalName + "(");

                var parameters = storedProcedure.GetParameters().OrderBy(x => x.IsOutputParameter).ThenBy(x => x.Name).ToList();

                index = 0;
                foreach (var param in parameters)
                {
                    sb.Append((param.IsOutputParameter ? "out " : string.Empty) + param.GetCodeType() + " " + param.CamelName);
                    if (index < storedProcedure.Parameters.Count - 1)
                        sb.Append(", ");
                    index++;
                }

                sb.AppendLine(")");
                sb.AppendLine("		{");

                sb.AppendLine("			var parameters = new List<ObjectParameter>();");

                foreach (var param in parameters)
                {
                    if (param.IsOutputParameter)
                        sb.AppendLine("			parameters.Add(new ObjectParameter(\"" + param.PascalName + "\", typeof(" + param.GetCodeType() + ")));");
                    else
                        sb.AppendLine("			parameters.Add(new ObjectParameter(\"" + param.PascalName + "\", " + param.CamelName + "));");
                }

                sb.AppendLine("			base.ExecuteFunction(\"" + storedProcedure.PascalName + "\", parameters.ToArray());");

                index = 0;
                foreach (var param in parameters)
                {
                    if (param.IsOutputParameter)
                    {
                        if (param.AllowNull)
                            sb.AppendLine("			if (parameters[" + index + "].Value == System.DBNull.Value) " + param.CamelName + " = null; else " + param.CamelName + " = (" + param.GetCodeType() + ")parameters[" + index + "].Value;");
                        else
                            sb.AppendLine("			" + param.CamelName + " = (" + param.GetCodeType() + ")parameters[" + index + "].Value;");
                    }
                    index++;
                }

                sb.AppendLine("		}");
                sb.AppendLine();
            }

            #endregion

            #region Functions
            foreach (var function in _model.Database.Functions.Where(x => x.Generated && x.IsTable).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// ");
                sb.AppendLine("		/// </summary>");
                sb.Append("		public IEnumerable<" + function.PascalName + "> " + function.PascalName + "(");
                var parameterList = function.GetParameters().Where(x => x.Generated).ToList();
                foreach (var parameter in parameterList)
                {
                    if (parameter.IsOutputParameter) sb.Append("out ");
                    sb.Append(parameter.GetCodeType() + " " + parameter.CamelName);
                    if (parameterList.IndexOf(parameter) < parameterList.Count() - 1)
                        sb.Append(", ");
                }
                sb.AppendLine(")");
                sb.AppendLine("		{");

                foreach (var parameter in parameterList)
                {
                    if (parameter.IsOutputParameter)
                    {
                        sb.AppendLine("			var " + parameter.CamelName + "Parameter = new ObjectParameter(\"" + parameter.DatabaseName + "\", typeof(" + parameter.GetCodeType() + "));");
                    }
                    else if (parameter.AllowNull)
                    {
                        sb.AppendLine("			ObjectParameter " + parameter.CamelName + "Parameter = null;");
                        sb.AppendLine("			if (" + parameter.CamelName + " != null) { " + parameter.CamelName + "Parameter = new ObjectParameter(\"" + parameter.DatabaseName + "\", " + parameter.CamelName + "); }");
                        sb.AppendLine("			else { " + parameter.CamelName + "Parameter = new ObjectParameter(\"" + parameter.DatabaseName + "\", typeof(" + parameter.GetCodeType() + ")); }");
                    }
                    else
                    {
                        sb.AppendLine("			var " + parameter.CamelName + "Parameter = new ObjectParameter(\"" + parameter.DatabaseName + "\", " + parameter.CamelName + ");");
                    }
                }

                sb.Append("			var retval = base.ExecuteFunction<" + function.PascalName + ">(\"" + function.PascalName + "_SPWrapper\"");

                if (parameterList.Count > 0)
                {
                    sb.Append(", ");
                    foreach (var parameter in parameterList)
                    {
                        sb.Append(parameter.CamelName + "Parameter");
                        if (parameterList.IndexOf(parameter) < parameterList.Count() - 1)
                            sb.Append(", ");
                    }
                }

                sb.AppendLine(").ToList();");

                //Add code here to handle output parameters
                foreach (var parameter in parameterList.Where(x => x.IsOutputParameter))
                {
                    sb.AppendLine("			" + parameter.CamelName + " = (" + parameter.GetCodeType() + ")" + parameter.CamelName + "Parameter.Value;");
                }

                sb.AppendLine("			return retval;");

                sb.AppendLine("		}");
                sb.AppendLine();
            }

            #endregion

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Configuration API/Database verification
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the version of the model that created this library.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual string Version");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _version; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the key of the model that created this library.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual string ModelKey");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _modelKey; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if the API matches the database connection");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual bool IsValidConnection()");
            sb.AppendLine("		{");
            sb.AppendLine("			return IsValidConnection(GetConnectionString(), true);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if the API matches the database connection");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual bool IsValidConnection(bool checkVersion)");
            sb.AppendLine("		{");
            sb.AppendLine("			return IsValidConnection(GetConnectionString(), checkVersion);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if the API matches the database connection");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"checkVersion\">Determines if the check also includes the exact version of the model</param>");
            sb.AppendLine("		/// <param name=\"connectionString\">Determines the connection string to use when connecting to the database</param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public virtual bool IsValidConnection(string connectionString, bool checkVersion = true)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (string.IsNullOrEmpty(connectionString))");
            sb.AppendLine("				return false;");
            sb.AppendLine();
            sb.AppendLine("			//Get current version");
            sb.AppendLine("			var version = GetDBVersion(connectionString);");
            sb.AppendLine();
            sb.AppendLine("			//If there is any version then the ModelKey was found, if not found then the database does not contain this model");
            sb.AppendLine("			if (string.IsNullOrEmpty(version))");
            sb.AppendLine("				return false;");
            sb.AppendLine();
            sb.AppendLine("			if (checkVersion)");
            sb.AppendLine("			{");
            sb.AppendLine("				if (version != this.Version)");
            sb.AppendLine("					return false;");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("			return true;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Retrieves the latest database version for the current model");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public string GetDBVersion(string connectionString)");
            sb.AppendLine("		{");
            sb.AppendLine("			var conn = new System.Data.SqlClient.SqlConnection();");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				conn.ConnectionString = connectionString;");
            sb.AppendLine("				conn.Open();");
            sb.AppendLine();
            sb.AppendLine("				var da = new SqlDataAdapter(\"select * from sys.tables where name = '__nhydrateschema'\", conn);");
            sb.AppendLine("				var ds = new DataSet();");
            sb.AppendLine("				da.Fill(ds);");
            sb.AppendLine("				if (ds.Tables[0].Rows.Count > 0)");
            sb.AppendLine("				{");
            sb.AppendLine("					da = new SqlDataAdapter(\"SELECT * FROM __nhydrateschema where [ModelKey] = '\" + this.ModelKey + \"'\", conn);");
            sb.AppendLine("					ds = new DataSet();");
            sb.AppendLine("					da.Fill(ds);");
            sb.AppendLine("					var t = ds.Tables[0];");
            sb.AppendLine("					if (t.Rows.Count > 0)");
            sb.AppendLine("					{");
            sb.AppendLine("						return (string) t.Rows[0][\"dbVersion\"];");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine("				return string.Empty;");
            sb.AppendLine("			}");
            sb.AppendLine("			catch (Exception ex)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw;");
            sb.AppendLine("			}");
            sb.AppendLine("			finally");
            sb.AppendLine("			{");
            sb.AppendLine("				if (conn != null)");
            sb.AppendLine("					conn.Close();");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            #region Auditing
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Persists all updates to the data source and resets change tracking in the object context.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"options\"></param>");
            sb.AppendLine("		/// <returns>The number of objects in an System.Data.EntityState.Added, System.Data.EntityState.Modified, or System.Data.EntityState.Deleted state when System.Data.Objects.ObjectContext.SaveChanges() was called.</returns>");
            sb.AppendLine("		public override int SaveChanges(SaveOptions options)");
            sb.AppendLine("		{");
            sb.AppendLine("			//Process deleted list");
            sb.AppendLine("			var deletedList = this.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Deleted);");
            sb.AppendLine("			foreach (var item in deletedList)");
            sb.AppendLine("			{");
            sb.AppendLine("				var entity = item.Entity as nHydrate.EFCore.DataAccess.IAuditable;");
            sb.AppendLine("				if (entity != null)");
            sb.AppendLine("				{");
            sb.AppendLine("					if (entity.IsModifyAuditImplemented && entity.ModifiedBy != this.ContextStartup.Modifer)");
            sb.AppendLine("					{");
            sb.AppendLine("						System.Data.SqlClient.SqlConnection connection = null;");
            sb.AppendLine("						try");
            sb.AppendLine("						{");
            sb.AppendLine("							connection = new System.Data.SqlClient.SqlConnection(GetConnectionString());");
            sb.AppendLine("							connection.Open();");
            sb.AppendLine("							System.Data.SqlClient.SqlCommand command = null;");

            var index2 = 0;
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable == TypedTableConstants.None) && !x.AssociativeTable && x.AllowModifiedAudit && x.AllowAuditTracking).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("							" + (index2 > 0 ? "else " : string.Empty) + "if (entity is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")");
                sb.AppendLine("							{");
                sb.Append("								command = new System.Data.SqlClient.SqlCommand(\"UPDATE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] SET [" + _model.Database.ModifiedByDatabaseName + "] = @mod WHERE ");

                var ii = 1;
                foreach (var column in table.PrimaryKeyColumns)
                {
                    sb.Append("[" + column.DatabaseName + "] = @pk" + ii + " AND ");
                    ii++;
                }
                sb.Append("(([" + _model.Database.ModifiedByDatabaseName + "] != @mod) OR ([" + _model.Database.ModifiedByDatabaseName + "] IS NULL AND @mod IS NOT NULL) OR ([" + _model.Database.ModifiedByDatabaseName + "] IS NOT NULL AND @mod IS NULL))");
                sb.AppendLine("\", connection);");

                ii = 1;
                foreach (var column in table.PrimaryKeyColumns)
                {
                    sb.AppendLine("								command.Parameters.Add(new System.Data.SqlClient.SqlParameter(\"@pk" + ii + "\", ((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + column.PascalName + "));");
                    ii++;
                }

                sb.AppendLine("							}");
                index2++;
            }

            sb.AppendLine("							if (command != null)");
            sb.AppendLine("							{");
            sb.AppendLine("								command.CommandType = System.Data.CommandType.Text;");
            sb.AppendLine("								if (this.ContextStartup.Modifer == null)");
            sb.AppendLine("									command.Parameters.Add(new System.Data.SqlClient.SqlParameter(\"@mod\", System.DBNull.Value));");
            sb.AppendLine("								else");
            sb.AppendLine("									command.Parameters.Add(new System.Data.SqlClient.SqlParameter(\"@mod\", this.ContextStartup.Modifer));");
            sb.AppendLine("								command.ExecuteNonQuery();");
            sb.AppendLine("							}");
            sb.AppendLine("						}");
            sb.AppendLine("						catch");
            sb.AppendLine("						{");
            sb.AppendLine("							throw;");
            sb.AppendLine("						}");
            sb.AppendLine("						finally");
            sb.AppendLine("						{");
            sb.AppendLine("							if (connection != null && connection.State == System.Data.ConnectionState.Open)");
            sb.AppendLine("								connection.Close();");
            sb.AppendLine("						}");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine();

            sb.AppendLine("			var markedTime = " + (_model.UseUTCTime ? "System.DateTime.UtcNow" : "System.DateTime.Now") + ";");
            sb.AppendLine("			//Process added list");
            sb.AppendLine("			var addedList = this.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added);");
            sb.AppendLine("			foreach (var item in addedList)");
            sb.AppendLine("			{");
            sb.AppendLine("				var entity = item.Entity as nHydrate.EFCore.DataAccess.IAuditable;");
            sb.AppendLine("				if (entity != null)");
            sb.AppendLine("				{");
            sb.AppendLine("					if (entity.IsModifyAuditImplemented && entity.ModifiedBy != this.ContextStartup.Modifer)");
            sb.AppendLine("					{");

            var index3 = 0;
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable == TypedTableConstants.None) && !x.AssociativeTable && x.AllowCreateAudit).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("						" + (index3 > 0 ? "else " : string.Empty) + "if (entity is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ") ((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")entity).ResetCreatedBy(this.ContextStartup.Modifer);");
                index3++;
            }
            sb.AppendLine("					}");

            index3 = 0;
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable == TypedTableConstants.None) && !x.AssociativeTable && x.AllowCreateAudit).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("					" + (index3 > 0 ? "else " : string.Empty) + "if (entity is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ") ((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + _model.Database.CreatedDatePascalName + " = markedTime;");
                index3++;
            }

            index3 = 0;
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable == TypedTableConstants.None) && !x.AssociativeTable && x.AllowModifiedAudit).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("					" + (index3 > 0 ? "else " : string.Empty) + "if (entity is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ") ((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + _model.Database.ModifiedDatePascalName + " = markedTime;");
                index3++;
            }
            sb.AppendLine();

            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine();

            sb.AppendLine("			//Process modified list");
            sb.AppendLine("			var modifiedList = this.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Modified);");
            sb.AppendLine("			foreach (var item in modifiedList)");
            sb.AppendLine("			{");
            sb.AppendLine("				var entity = item.Entity as nHydrate.EFCore.DataAccess.IAuditable;");
            sb.AppendLine("				if (entity != null)");
            sb.AppendLine("				{");

            #region IsModifyAuditImplemented
            sb.AppendLine("					if (entity.IsModifyAuditImplemented && entity.ModifiedBy != this.ContextStartup.Modifer)");
            sb.AppendLine("					{");

            index3 = 0;
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable == TypedTableConstants.None) && !x.AssociativeTable && x.AllowModifiedAudit).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("						" + (index3 > 0 ? "else " : string.Empty) + "if (entity is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ") ((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + _model.Database.ModifiedByPascalName + " = this.ContextStartup.Modifer;");
                index3++;
            }

            sb.AppendLine("					}");
            sb.AppendLine();
            #endregion

            index3 = 0;
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable == TypedTableConstants.None) && !x.AssociativeTable && x.AllowModifiedAudit).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("					" + (index3 > 0 ? "else " : string.Empty) + "if (entity is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ") ((" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + _model.Database.ModifiedDatePascalName + " = markedTime;");
                index3++;
            }

            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine();

            sb.AppendLine("			return base.SaveChanges(options);");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            #region GetEntityFromField
            sb.AppendLine("		#region IContext Interface");
            sb.AppendLine();
            sb.AppendLine("		Enum nHydrate.EFCore.DataAccess.IContext.GetEntityFromField(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetEntityFromField(field);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		object nHydrate.EFCore.DataAccess.IContext.GetMetaData(Enum entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetMetaData((EntityMappingConstants)entity);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		System.Type nHydrate.EFCore.DataAccess.IContext.GetFieldType(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return this.GetFieldType(field);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the entity from one of its fields");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static EntityMappingConstants GetEntityFromField(Enum field)");
            sb.AppendLine("		{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("			if (field is " + this.GetLocalNamespace() + ".Interfaces.Entity." + table.PascalName + "FieldNameConstants) return " + this.GetLocalNamespace() + ".EntityMappingConstants." + table.PascalName + ";");
            }
            sb.AppendLine("			throw new Exception(\"Unknown field type!\");");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            #region GetMetaData
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the meta data object for an entity");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static " + this.DefaultNamespace + ".EFDAL.Interfaces.IMetadata GetMetaData(" + this.GetLocalNamespace() + ".EntityMappingConstants table)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (table)");
            sb.AppendLine("			{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.Append("				case " + this.GetLocalNamespace() + ".EntityMappingConstants." + table.PascalName + ": ");
                //sb.AppendLine("return Activator.CreateInstance(((System.ComponentModel.DataAnnotations.MetadataTypeAttribute)typeof(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ").GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MetadataTypeAttribute), true).FirstOrDefault()).MetadataClassType) as " + this.GetLocalNamespace() + ".Interfaces.Entity.Metadata." + table.PascalName + "Metadata;");
                sb.AppendLine("return new " + GetLocalNamespace() + ".Interfaces.Entity.Metadata." + table.PascalName + "Metadata();");
            }
            sb.AppendLine("			}");
            sb.AppendLine("			throw new Exception(\"Entity not found!\");");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            sb.AppendLine("	}");
            sb.AppendLine();

            sb.AppendLine("	#endregion");
            sb.AppendLine();

        }

        #endregion

    }
}