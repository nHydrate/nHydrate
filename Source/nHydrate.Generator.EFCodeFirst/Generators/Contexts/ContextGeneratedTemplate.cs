#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
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
using nHydrate.Generator.Common;

namespace nHydrate.Generator.EFCodeFirst.Generators.Contexts
{
    public class ContextGeneratedTemplate : EFCodeFirstBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();

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
                this.AppendTableMapping();
                this.AppendClass();
                sb.AppendLine("}");
                sb.AppendLine();

                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
                sb.AppendLine("{");
                sb.AppendLine("}");
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

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data.Entity;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Data.Entity.ModelConfiguration;");
            sb.AppendLine("using " + this.GetLocalNamespace() + ".Entity;");
            sb.AppendLine("using System.Data.Entity.Core.Objects;");
            sb.AppendLine();
        }

        private void AppendClass()
        {
            if (_model.Database.Tables.Any(x => x.IsTenant))
            {
                #region Admin Context

                sb.AppendLine("	#region Admin Entity Context");
                sb.AppendLine();
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The object context for the schema tied to this generated model.");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[DataContract]");
                sb.AppendLine("	[Serializable]");
                sb.AppendLine("	public partial class " + _model.ProjectName + "AdminEntities : " + _model.ProjectName + "Entities");
                sb.AppendLine("	{");

                #region Constructors
                sb.AppendLine("		#region Constructors");
                sb.AppendLine();

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Initializes a new " + _model.ProjectName + "AdminEntities object using the connection string found in the '" + _model.ProjectName + "AdminEntities' section of the application configuration file.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public " + _model.ProjectName + "AdminEntities() :");
                sb.AppendLine("			base(new EFDAL.ContextStartup(null, true, 30, true))");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Initialize a new " + _model.ProjectName + "AdminEntities object with an audit modifier.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public " + _model.ProjectName + "AdminEntities(ContextStartup contextStartup) :");
                sb.AppendLine("			base(contextStartup.AsAdmin())");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Initialize a new " + _model.ProjectName + "AdminEntities object with an audit modifier.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public " + _model.ProjectName + "AdminEntities(ContextStartup contextStartup, string connectionString) :");
                sb.AppendLine("			base(contextStartup.AsAdmin(), Util.ConvertNormalCS2EF(connectionString, contextStartup.AsAdmin()))");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Initialize a new " + _model.ProjectName + "AdminEntities object.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public " + _model.ProjectName + "AdminEntities(string connectionString) :");
                sb.AppendLine("			base(new EFDAL.ContextStartup(null, true, 30, true), connectionString)");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		#endregion");
                sb.AppendLine();
                #endregion

                sb.AppendLine("	}");
                sb.AppendLine();
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                #endregion
            }

            sb.AppendLine("	#region Entity Context");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// The object context for the schema tied to this generated model.");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	[DataContract]");
            sb.AppendLine("	[Serializable]");
            sb.AppendLine("	public partial class " + _model.ProjectName + "Entities : System.Data.Entity.DbContext, " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities, IContext");
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

            //Events
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs> BeforeSaveModifiedEntity;");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected virtual void OnBeforeSaveModifiedEntity(" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if(BeforeSaveModifiedEntity != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				BeforeSaveModifiedEntity(this, e);");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs> BeforeSaveAddedEntity;");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected virtual void OnBeforeSaveAddedEntity(" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if(BeforeSaveAddedEntity != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				BeforeSaveAddedEntity(this, e);");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs> AfterSaveModifiedEntity;");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected virtual void OnAfterSaveModifiedEntity(" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if(AfterSaveModifiedEntity != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				AfterSaveModifiedEntity(this, e);");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs> AfterSaveAddedEntity;");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected virtual void OnAfterSaveAddedEntity(" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if(AfterSaveAddedEntity != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				AfterSaveAddedEntity(this, e);");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            #region Constructors
            sb.AppendLine("		#region Constructors");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initializes a new " + _model.ProjectName + "Entities object using the connection string found in the '" + _model.ProjectName + "Entities' section of the application configuration file.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + _model.ProjectName + "Entities() :");
            sb.AppendLine("			base(Util.ConvertNormalCS2EFFromConfig(\"name=" + _model.ProjectName + "Entities\"))");
            sb.AppendLine("		{");
            sb.AppendLine("			_contextStartup = new EFDAL.ContextStartup(null, true, 30, false);");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				var builder = new System.Data.Odbc.OdbcConnectionStringBuilder(Util.StripEFCS2Normal(this.Database.Connection.ConnectionString));");
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
            sb.AppendLine("			base(Util.ConvertNormalCS2EFFromConfig(\"name=" + _model.ProjectName + "Entities\", contextStartup))");
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
            sb.AppendLine("			base(Util.ConvertNormalCS2EF(connectionString, contextStartup))");
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
            sb.AppendLine("			base(Util.ConvertNormalCS2EF(connectionString))");
            sb.AppendLine("		{");
            sb.AppendLine("			_contextStartup = new EFDAL.ContextStartup(null, true, 30, false);");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				var builder = new System.Data.Odbc.OdbcConnectionStringBuilder(Util.StripEFCS2Normal(this.Database.Connection.ConnectionString));");
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

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		partial void OnContextCreated();");
            sb.AppendLine("		partial void OnBeforeSaveChanges(ref bool cancel);");
            sb.AppendLine("		partial void OnAfterSaveChanges();");
            sb.AppendLine();

            #region OnModelCreating

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Model creation event");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected override void OnModelCreating(DbModelBuilder modelBuilder)");
            sb.AppendLine("		{");
            sb.AppendLine("			base.OnModelCreating(modelBuilder);");
            sb.AppendLine("			modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();");
            sb.AppendLine("			Database.SetInitializer(new CustomDatabaseInitializer<" + _model.ProjectName + "Entities>());");
            sb.AppendLine();

            //For entities with security functions we need to manually set the 
            sb.AppendLine("			//Manually set the entities that have a security function because their DbSet<> is protected and not set");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                if (table.Security.IsValid())
                {
                    sb.AppendLine("			this." + table.PascalName + "__INTERNAL = Set<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">();");
                }
            }
            sb.AppendLine();

            #region Hierarchy Mapping

            var hierarchyList = new List<Table>();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly) && x.ParentTable != null).OrderBy(x => x.Name).ToList())
            {
                hierarchyList.Add(table);
                hierarchyList.Add(table.ParentTable);
            }
            hierarchyList = hierarchyList.Distinct().ToList();

            sb.AppendLine("			#region Hierarchy Mapping");
            foreach (var table in hierarchyList)
            {
                sb.AppendLine("			modelBuilder.Entity<" + table.PascalName + ">().ToTable(\"" + table.DatabaseName + "\");");
            }
            sb.AppendLine("			#endregion");
            sb.AppendLine();

            #endregion

            #region Rename Tables
            sb.AppendLine("			#region Rename Tables");
            sb.AppendLine("			if (_contextStartup.IsAdmin)");
            sb.AppendLine("			{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                if (table.DatabaseName != table.PascalName)
                    sb.AppendLine("				modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().ToTable(\"" + table.DatabaseName + "\");");
            }
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                if (table.IsTenant)
                {
                    sb.AppendLine("				modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().ToTable(\"" + _model.TenantPrefix + "_" + table.DatabaseName + "\");");
                }
                else if (table.DatabaseName != table.PascalName)
                {
                    sb.AppendLine("				modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().ToTable(\"" + table.DatabaseName + "\");");
                }
            }
            sb.AppendLine("			}");
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            #region Create annotations for properties
            sb.AppendLine("			#region Setup Fields");
            sb.AppendLine();

            //Tables
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                sb.AppendLine("			//Field setup for " + table.PascalName + " entity");
                foreach (var column in table.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    var isTypeValue = false;
                    #region Determine if this is a type table Value field and if so ignore
                    {
                        string roleName;
                        string pascalRoleName;
                        Table typeTable = null;
                        if (table.IsColumnRelatedToTypeTable(column, out pascalRoleName) || (column.PrimaryKey && table.TypedTable != TypedTableConstants.None))
                        {
                            typeTable = table.GetRelatedTypeTableByColumn(column, out pascalRoleName);
                            if (typeTable == null) typeTable = table;
                            if (typeTable != null)
                            {
                                sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().Ignore(d => d." + pascalRoleName + typeTable.PascalName + "Value);");
                                isTypeValue = true;
                            }
                        }
                    }
                    #endregion

                    //If this is a base table OR the column is not a PK then process it
                    //Primary key code should be emited ONLY for base tables
                    if (table.ParentTable == null || !column.PrimaryKey)
                    {
                        sb.Append("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">()");
                        sb.Append(".Property(d => d." + column.PascalName + ")");
                        if (column.AllowNull) sb.Append(".IsOptional()");
                        else sb.Append(".IsRequired()");

                        if (column.PrimaryKey && column.IsIntegerType && column.Identity != IdentityTypeConstants.Database)
                            sb.Append(".HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)");
                        else if (column.ComputedColumn)
                            sb.Append(".HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)");
                        else if (column.Identity == IdentityTypeConstants.Database && column.IsIntegerType)
                            sb.Append(".HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)");

                        if (column.IsTextType && column.DataType != System.Data.SqlDbType.Xml) sb.Append(".HasMaxLength(" + column.GetAnnotationStringLength() + ")");
                        if (column.DataType == System.Data.SqlDbType.VarChar) sb.Append(".HasColumnType(\"VARCHAR\")");
                        if (column.DatabaseName != column.PascalName) sb.Append(".HasColumnName(\"" + column.DatabaseName + "\")");
                        sb.AppendLine(";");
                    }
                }

                if (table.AllowTimestamp)
                {
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().Property(d => d." + _model.Database.TimestampPascalName + ").IsConcurrencyToken(true);");
                }

                sb.AppendLine();
            }

            //Views
            foreach (var item in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("			//Field setup for " + item.PascalName + " entity");
                foreach (var column in item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    sb.Append("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">()");
                    sb.Append(".Property(d => d." + column.PascalName + ")");
                    if (!column.AllowNull)
                        sb.Append(".IsRequired()");

                    if (column.IsTextType && column.DataType != System.Data.SqlDbType.Xml) sb.Append(".HasMaxLength(" + column.GetAnnotationStringLength() + ")");
                    if (column.DatabaseName != column.PascalName) sb.Append(".HasColumnName(\"" + column.DatabaseName + "\")");
                    sb.AppendLine(";");
                }
                sb.AppendLine();
            }

            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            #region Setup ignores for Enum properties
            sb.AppendLine("			#region Ignore Enum Properties");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                foreach (var column in table.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    string pascalRoleName;
                    if (table.IsColumnRelatedToTypeTable(column, out pascalRoleName) || (column.PrimaryKey && table.TypedTable != TypedTableConstants.None))
                    {
                        var typeTable = table.GetRelatedTypeTableByColumn(column, out pascalRoleName);
                        if (typeTable == null) typeTable = table;
                        if (typeTable != null)
                        {
                            sb.AppendLine("			modelBuilder.Entity<" + table.PascalName + ">().Ignore(t => t." + pascalRoleName + typeTable.PascalName + "Value);");
                        }
                    }
                }
            }
            sb.AppendLine();
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            #region Create annotations for relationships
            sb.AppendLine("			#region Relations");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                foreach (Relation relation in table.GetRelations())
                {
                    Table childTable = relation.ChildTableRef.Object as Table;
                    if (childTable.Generated && !childTable.IsInheritedFrom(table) && !childTable.AssociativeTable)
                    {
                        if (relation.IsOneToOne)
                        {
                            sb.AppendLine("			//Relation " + table.PascalName + " -> " + childTable.PascalName);
                            sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + ">()");

                            if (relation.IsRequired)
                                sb.AppendLine("							 .HasRequired(a => a." + relation.PascalRoleName + table.PascalName + ")");
                            else
                                sb.AppendLine("							 .HasOptional(a => a." + relation.PascalRoleName + table.PascalName + ")");

                            sb.AppendLine("							 .WithOptional(x => x." + childTable.PascalName + ")");
                            sb.AppendLine("							 .WillCascadeOnDelete(false);");
                        }
                        else
                        {
                            sb.AppendLine("			//Relation " + table.PascalName + " -> " + childTable.PascalName);
                            sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + ">()");

                            if (relation.IsRequired)
                                sb.AppendLine("							 .HasRequired(a => a." + relation.PascalRoleName + table.PascalName + ")");
                            else
                                sb.AppendLine("							 .HasOptional(a => a." + relation.PascalRoleName + table.PascalName + ")");

                            sb.AppendLine("							 .WithMany(b => b." + relation.PascalRoleName + childTable.PascalName + "List)");
                            sb.Append("							 .HasForeignKey(u => new { ");

                            var index = 0;
                            foreach (var columnPacket in relation.ColumnRelationships
                                .Select(x => new { Child = x.ChildColumnRef.Object as Column, Parent = x.ParentColumnRef.Object as Column })
                                .Where(x => x.Child != null && x.Parent != null)
                                .OrderBy(x => x.Parent.Name)
                                .ToList())
                            {
                                sb.Append("u." + columnPacket.Child.PascalName);
                                if (index < relation.ColumnRelationships.Count - 1)
                                    sb.Append(", ");
                                index++;
                            }

                            sb.AppendLine(" })");
                            sb.AppendLine("							 .WillCascadeOnDelete(false);");
                        }

                        sb.AppendLine();
                    }
                }
            }

            //Associative tables
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                var relations = table.GetRelationsWhereChild().ToList();
                if (relations.Count == 2)
                {
                    var relation1 = relations.First();
                    var relation2 = relations.Last();
                    sb.AppendLine("			modelBuilder.Entity<" + relation1.ParentTable.PascalName + ">()");
                    sb.AppendLine("				.HasMany(q => q." + relation2.PascalRoleName + relation2.ParentTable.PascalName + "List)");
                    sb.AppendLine("				.WithMany(q => q." + relation1.PascalRoleName + relation1.ParentTable.PascalName + "List)");
                    sb.AppendLine("				.Map(q =>");
                    sb.AppendLine("			{");
                    sb.AppendLine("				q.ToTable(\"" + table.PascalName + "\");");
                    sb.AppendLine("				q.MapLeftKey(" + string.Join(",", relation1.ColumnRelationships.Select(x => "\"" + x.ChildColumn.DatabaseName + "\"").ToList()) + ");");
                    sb.AppendLine("				q.MapRightKey(" + string.Join(",", relation2.ColumnRelationships.Select(x => "\"" + x.ChildColumn.DatabaseName + "\"").ToList()) + ");");
                    sb.AppendLine("			});");
                    sb.AppendLine();
                }
            }

            sb.AppendLine();
            sb.AppendLine("			#endregion");
            sb.AppendLine();

            #endregion

            #region Functions
            sb.AppendLine("			#region Functions");
            sb.AppendLine();
            if (_model.Database.Functions.Any(x => x.Generated && x.IsTable) || _model.Database.Tables.Any(x => x.Security.IsValid()))
            {
                sb.AppendLine("			//You must add the NUGET package 'CodeFirstStoreFunctions' for this functionality");
                sb.AppendLine("			modelBuilder.Conventions.Add(new CodeFirstStoreFunctions.FunctionsConvention<" + _model.ProjectName + "Entities>(\"dbo\"));");
            }

            foreach (var table in _model.Database.Functions.Where(x => x.Generated && x.IsTable).OrderBy(x => x.Name))
            {
                sb.AppendLine("			modelBuilder.ComplexType<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">();");
            }
            foreach (var table in _model.Database.CustomStoredProcedures.Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("			modelBuilder.ComplexType<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">();");
            }

            sb.AppendLine();
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            #region Primary Keys
            sb.AppendLine("			#region Primary Keys");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                sb.Append("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().HasKey(x => new { ");
                var columnList = table.GetColumns().Where(x => x.PrimaryKey).OrderBy(x => x.Name).ToList();
                foreach (var c in columnList)
                {
                    sb.Append("x." + c.PascalName);
                    if (columnList.IndexOf(c) < columnList.Count - 1)
                        sb.Append(", ");
                }
                sb.AppendLine(" });");
            }

            foreach (var table in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.Append("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().HasKey(x => new { ");
                var columnList = table.GetColumns().Where(x => x.IsPrimaryKey).OrderBy(x => x.Name).ToList();
                foreach (var c in columnList)
                {
                    sb.Append("x." + c.PascalName);
                    if (columnList.IndexOf(c) < columnList.Count - 1)
                        sb.Append(", ");
                }
                sb.AppendLine(" });");
            }

            sb.AppendLine();
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		}");
            sb.AppendLine();

            #endregion

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

            #region Auditing
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Persists all updates to the data source and resets change tracking in the object context.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <returns>The number of objects in an System.Data.Entity.EntityState.Added, System.Data.Entity.EntityState.Modified, or System.Data.Entity.EntityState.Deleted state when System.Data.Objects.ObjectContext.SaveChanges() was called.</returns>");
            sb.AppendLine("		public override int SaveChanges()");
            sb.AppendLine("		{");

            sb.AppendLine("			var cancel = false;");
            sb.AppendLine("			OnBeforeSaveChanges(ref cancel);");
            sb.AppendLine("			if (cancel) return 0;");
            sb.AppendLine();
            sb.AppendLine("			//Get the added list");
            sb.AppendLine("			var addedList = this.ObjectContext.ObjectStateManager.GetObjectStateEntries(System.Data.Entity.EntityState.Added);");
            sb.AppendLine();
            sb.AppendLine("			if (_contextStartup.IsAdmin)");
            sb.AppendLine("			{");

            var tenantTables = _model.Database.Tables.Where(x => x.IsTenant).OrderBy(x => x.Name).ToList();
            if (tenantTables.Any())
            {
                sb.AppendLine("				foreach (var item in addedList)");
                sb.AppendLine("				{");
                foreach (var table in tenantTables)
                {
                    sb.AppendLine("					if (item.Entity is " + this.GetLocalNamespace() + ".Entity." + table.Name + ")");
                    sb.AppendLine("						throw new Exception(\"You cannot add items to the tenant table '" + table.Name + "' in Admin mode.\");");
                }
                sb.AppendLine("				}");
                sb.AppendLine("			}");
                sb.AppendLine();
            }

            sb.AppendLine("			//Process deleted list");
            sb.AppendLine("			var deletedList = this.ObjectContext.ObjectStateManager.GetObjectStateEntries(System.Data.Entity.EntityState.Deleted);");
            sb.AppendLine("			foreach (var item in deletedList)");
            sb.AppendLine("			{");
            sb.AppendLine("				var entity = item.Entity as IAuditable;");
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

            #region Added Items
            sb.AppendLine("			var markedTime = " + (_model.UseUTCTime ? "System.DateTime.UtcNow" : "System.DateTime.Now") + ";");
            sb.AppendLine("			//Process added list");
            sb.AppendLine("			foreach (var item in addedList)");
            sb.AppendLine("			{");
            sb.AppendLine("				var entity = item.Entity as IAuditable;");
            sb.AppendLine("				if (entity != null)");
            sb.AppendLine("				{");
            sb.AppendLine("					var audit = entity as IAuditableSet;");
            sb.AppendLine("					if (entity.IsModifyAuditImplemented && entity.ModifiedBy != this.ContextStartup.Modifer)");
            sb.AppendLine("					{");
            sb.AppendLine("						if (audit != null) audit.ResetCreatedBy(this.ContextStartup.Modifer);");
            sb.AppendLine("						if (audit != null) audit.ResetModifiedBy(this.ContextStartup.Modifer);");
            sb.AppendLine("					}");
            sb.AppendLine("					audit.CreatedDate = markedTime;");
            sb.AppendLine("					audit.ModifiedDate = markedTime;");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			this.OnBeforeSaveAddedEntity(new EventArguments.EntityListEventArgs { List = addedList });");
            sb.AppendLine();
            #endregion

            #region Modified Items
            sb.AppendLine("			//Process modified list");
            sb.AppendLine("			var modifiedList = this.ObjectContext.ObjectStateManager.GetObjectStateEntries(System.Data.Entity.EntityState.Modified);");
            sb.AppendLine("			foreach (var item in modifiedList)");
            sb.AppendLine("			{");
            sb.AppendLine("				var entity = item.Entity as IAuditable;");
            sb.AppendLine("				if (entity != null)");
            sb.AppendLine("				{");
            sb.AppendLine("					var audit = entity as IAuditableSet;");
            sb.AppendLine("					if (entity.IsModifyAuditImplemented && entity.ModifiedBy != this.ContextStartup.Modifer)");
            sb.AppendLine("					{");
            sb.AppendLine("						if (audit != null) audit.ResetModifiedBy(this.ContextStartup.Modifer);");
            sb.AppendLine("					}");
            sb.AppendLine("					audit.ModifiedDate = markedTime;");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			this.OnBeforeSaveModifiedEntity(new EventArguments.EntityListEventArgs { List = modifiedList });");
            sb.AppendLine();
            #endregion

            sb.AppendLine("			var retval = 0;");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				_paramList.Clear();");
            sb.AppendLine("				retval = base.SaveChanges();");
            sb.AppendLine("			}");
            sb.AppendLine("			catch (System.Data.Entity.Validation.DbEntityValidationException ex)");
            sb.AppendLine("			{");
            sb.AppendLine("				var sb = new System.Text.StringBuilder();");
            sb.AppendLine("				foreach (var error in ex.EntityValidationErrors)");
            sb.AppendLine("				{");
            sb.AppendLine("					foreach (var validationError in error.ValidationErrors)");
            sb.AppendLine("					{");
            sb.AppendLine("						sb.AppendLine(validationError.PropertyName + \": \" + validationError.ErrorMessage);");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine("				throw new System.Data.Entity.Validation.DbEntityValidationException(sb.ToString(), ex.EntityValidationErrors);");
            sb.AppendLine("			}");
            sb.AppendLine("			catch");
            sb.AppendLine("			{");
            sb.AppendLine("				throw;");
            sb.AppendLine("			}");
            sb.AppendLine("			this.OnAfterSaveAddedEntity(new EventArguments.EntityListEventArgs { List = addedList });");
            sb.AppendLine("			this.OnAfterSaveModifiedEntity(new EventArguments.EntityListEventArgs { List = modifiedList });");
            sb.AppendLine("			OnAfterSaveChanges();");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            #region Entity Sets
            sb.AppendLine("		#region Entity Sets");
            sb.AppendLine();

            foreach (var item in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                var hasSecurity = item.Security.IsValid();
                var name = item.PascalName;
                if (hasSecurity) name += "__INTERNAL";

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Entity set for " + item.PascalName);
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		" + (hasSecurity ? "protected internal" : "public") + " virtual DbSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + name + " { get; set; }");
                sb.AppendLine();
            }

            foreach (var item in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Entity set for " + item.PascalName);
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public virtual DbSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + item.PascalName + " { get; set; }");
                sb.AppendLine();
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The global settings of this context");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual ContextStartup ContextStartup");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _contextStartup; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public virtual System.Data.Entity.Core.Objects.ObjectContextOptions ContextOptions");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return this.ObjectContext.ContextOptions; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            #region Configuration API/Database verification
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
            sb.AppendLine("			catch (Exception)");
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

            #region Add Functionality
            //Add an strongly-typed extension for "AddItem" method
            sb.AppendLine("		#region AddItem Methods");
            sb.AppendLine();

            #region Tables
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Adds an entity of to the object context");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"entity\">The entity to add</param>");
            sb.AppendLine("		public virtual " + this.GetLocalNamespace() + ".IBusinessObject AddItem(" + this.GetLocalNamespace() + ".IBusinessObject entity)");
            sb.AppendLine("		{");

            sb.AppendLine("			if (entity == null) throw new NullReferenceException();");
            sb.AppendLine("			var audit = entity as " + this.GetLocalNamespace() + ".IAuditableSet;");
            sb.AppendLine("			if (audit != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				audit.CreatedBy = _contextStartup.Modifer;");
            sb.AppendLine("				audit.ModifiedBy = _contextStartup.Modifer;");
            sb.AppendLine("			}");
            sb.AppendLine("			if (entity is " + this.GetLocalNamespace() + ".ICreatable)");
            sb.AppendLine("			{");
            sb.AppendLine("				var extra = string.Empty;");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && x.Security.IsValid()).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("				if (entity is "+ table.PascalName +") extra = \"__INTERNAL\";");
            }
            sb.AppendLine("				this.ObjectContext.AddObject(entity.GetType().Name + extra, entity);");
            sb.AppendLine("			}");

            sb.AppendLine("			return entity;");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Delete Functionality
            //Add an strongly-typed extension for "DeleteItem" method
            sb.AppendLine("		#region DeleteItem Methods");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Deletes an entity from the context");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"entity\">The entity to delete</param>");
            sb.AppendLine("		public virtual void DeleteItem(IBusinessObject entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (entity == null) return;");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable).OrderBy(x => x.PascalName))
            {
                var name = table.GetAbsoluteBaseTable().PascalName;
                if (table.Security.IsValid()) name += "__INTERNAL";
                sb.AppendLine("			else if (entity is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ") this." + name + ".Remove(entity as " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ");");
            }

            sb.AppendLine("			else");
            sb.AppendLine("				throw new Exception(\"Unknown entity type\");");
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
            sb.AppendLine("					if (this.Database.Connection != null && !string.IsNullOrEmpty(this.Database.Connection.ConnectionString))");
            sb.AppendLine("					{");
            sb.AppendLine("						return Util.StripEFCS2Normal(this.Database.Connection.ConnectionString);");
            sb.AppendLine("					}");
            sb.AppendLine("					else");
            sb.AppendLine("					{");
            sb.AppendLine("						return null;");
            sb.AppendLine("					}");
            sb.AppendLine();
            sb.AppendLine("				}");
            sb.AppendLine("				catch (Exception)");
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
            sb.AppendLine("			catch (Exception)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(\"The connection string was not found.\");");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            #region Context Interface Members
            sb.AppendLine("		#region I" + _model.ProjectName + " Members");
            sb.AppendLine();

            #region Tables
            foreach (var item in _model.Database.Tables.Where(x => x.Generated && !x.Security.IsValid() && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities." + item.PascalName);
                sb.AppendLine("		{");
                sb.AppendLine("			get { return (IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">)this." + item.PascalName + "; }");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            #region View
            foreach (var item in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities." + item.PascalName);
                sb.AppendLine("		{");
                sb.AppendLine("			get { return (IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">)this." + item.PascalName + "; }");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            #region Stored Procedures
            foreach (var item in _model.Database.CustomStoredProcedures.Where(x => x.Generated && x.GeneratedColumns.Count > 0).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                sb.Append("		public List<" + item.PascalName + "> " + item.PascalName + "(");
                var parameterList = item.GetParameters().Where(x => x.Generated).ToList();
                foreach (var parameter in parameterList)
                {
                    if (parameter.IsOutputParameter) sb.Append("out ");
                    sb.Append(parameter.GetCodeType() + " " + parameter.CamelName);
                    if (parameterList.IndexOf(parameter) < parameterList.Count() - 1)
                        sb.Append(", ");
                }
                sb.AppendLine(")");
                sb.AppendLine("		{");

                var spParamString = new List<string>();
                foreach (var parameter in parameterList)
                {
                    if (parameter.IsOutputParameter)
                    {
                        sb.AppendLine("			var " + parameter.CamelName + "Parameter = new SqlParameter(\"" + parameter.DatabaseName + "\", typeof(" + parameter.GetCodeType() + "));");
                        sb.AppendLine("			" + parameter.CamelName + "Parameter.Direction = ParameterDirection.Output;");
                        sb.AppendLine("			" + parameter.CamelName + "Parameter.Value = " + parameter.GetCodeDefault() + ";");
                        spParamString.Add("@" + parameter.DatabaseName + " output");
                    }
                    else if (parameter.AllowNull)
                    {
                        sb.AppendLine("			SqlParameter " + parameter.CamelName + "Parameter = null;");
                        sb.AppendLine("			if (" + parameter.CamelName + " != null) { " + parameter.CamelName + "Parameter = new SqlParameter(\"" + parameter.DatabaseName + "\", " + parameter.CamelName + "); }");
                        sb.AppendLine("			else { " + parameter.CamelName + "Parameter = new SqlParameter(\"" + parameter.DatabaseName + "\", typeof(" + parameter.GetCodeType() + ")); }");
                        spParamString.Add("@" + parameter.DatabaseName);
                    }
                    else
                    {
                        sb.AppendLine("			var " + parameter.CamelName + "Parameter = new SqlParameter(\"" + parameter.DatabaseName + "\", " + parameter.CamelName + ");");
                        spParamString.Add("@" + parameter.DatabaseName);
                    }
                }

                sb.Append("			var retval = this.Database.SqlQuery<" + item.PascalName + ">(\"[" + item.GetDatabaseObjectName() + "] " + string.Join(", ", spParamString) + "\"");
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
                    sb.AppendLine("			if (" + parameter.CamelName + "Parameter.Value == System.DBNull.Value) " + parameter.CamelName + " = default("+ parameter.GetCodeType(parameter.AllowNull) +");");
                    sb.AppendLine("			else " + parameter.CamelName + " = (" + parameter.GetCodeType() + ")" + parameter.CamelName + "Parameter.Value;");
                }

                sb.AppendLine("			return retval;");

                sb.AppendLine("		}");
                sb.AppendLine();

                //Interface
                var paramset = item.GetParameters().Where(x => x.Generated).ToList();
                var paramString1 = string.Join(", ", paramset.Select(x => (x.IsOutputParameter ? "out " : "") + x.GetCodeType(true) + " " + x.CamelName).ToList());
                var paramString2 = string.Join(", ", paramset.Select(x => (x.IsOutputParameter ? "out " : "") + x.CamelName).ToList());
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities." + item.PascalName + "(" + paramString1 + ")");
                sb.AppendLine("		{");
                sb.AppendLine("			return (IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">)this." + item.PascalName + "(" + paramString2 + ");");
                sb.AppendLine("		}");
                sb.AppendLine();
            }

            //Stored procs that are Action (no columns)
            foreach (var item in _model.Database.CustomStoredProcedures.Where(x => x.Generated && x.GeneratedColumns.Count == 0).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Calls the " + item.PascalName + " action");
                sb.AppendLine("		/// </summary>");
                sb.Append("		public void " + item.PascalName + "(");

                var parameterList = item.GetParameters().Where(x => x.Generated).ToList();
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
                        sb.AppendLine("			var " + parameter.CamelName + "Parameter = new SqlParameter(\"" + parameter.DatabaseName + "\", typeof(" + parameter.GetCodeType() + "));");
                    }
                    else if (parameter.AllowNull)
                    {
                        sb.AppendLine("			SqlParameter " + parameter.CamelName + "Parameter = null;");
                        sb.AppendLine("			if (" + parameter.CamelName + " != null) { " + parameter.CamelName + "Parameter = new SqlParameter(\"" + parameter.DatabaseName + "\", " + parameter.CamelName + "); }");
                        sb.AppendLine("			else { " + parameter.CamelName + "Parameter = new SqlParameter(\"" + parameter.DatabaseName + "\", typeof(" + parameter.GetCodeType() + ")); }");
                    }
                    else
                    {
                        sb.AppendLine("			var " + parameter.CamelName + "Parameter = new SqlParameter(\"" + parameter.DatabaseName + "\", " + parameter.CamelName + ");");
                    }
                }
                
                //sb.AppendLine("			this.ObjectContext.ExecuteFunction(\"" + item.PascalName + "\", parameters.ToArray());");
                sb.Append("			var retval = this.Database.SqlQuery<" + item.PascalName + ">(\"[" + item.GetDatabaseObjectName() + "]\"");
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

                sb.AppendLine("		}");
                sb.AppendLine();
            }

            #endregion

            #region Functions
            foreach (var item in _model.Database.Functions.Where(x => x.Generated && x.IsTable).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		/// " + item.Description);
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		[DbFunction(\"" + _model.ProjectName + "Entities\", \"" + item.PascalName + "\")]");
                sb.Append("		public virtual IQueryable<" + item.PascalName + "> " + item.PascalName + "(");
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

                sb.AppendLine("			var retval = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext.CreateQuery<" + item.PascalName + ">(\"[" + _model.ProjectName + "Entities].[" + item.PascalName + "](" + string.Join(", ", parameterList.Select(x => "@" + x.PascalName)) + ")\", " + string.Join(", ", parameterList.Select(x => x.CamelName + "Parameter")) + ");");

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

            #region Table Security
            var securedTables = _model.Database.Tables.Where(x => x.Generated && x.Security.IsValid()).OrderBy(x => x.PascalName).ToList();
            sb.AppendLine("		private List<string> _paramList = new List<string>();");

            foreach (var item in securedTables)
            {
                var function = item.Security;
                var objectName = ValidationHelper.MakeDatabaseIdentifier("__security__" + item.Name);

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Security function for '" + item.PascalName + "' entity");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		[DbFunction(\"" + _model.ProjectName + "Entities\", \"" + objectName + "\")]");
                sb.Append("		public virtual IQueryable<" + item.PascalName + "> " + item.PascalName + "(");
                var parameterList = function.GetParameters().Where(x => x.Generated).ToList();
                foreach (var parameter in parameterList)
                {
                    if (parameter.IsOutputParameter) sb.Append("out ");
                    sb.Append(parameter.GetCodeType() + " " + parameter.CamelName);
                    if (parameterList.IndexOf(parameter) < parameterList.Count - 1)
                        sb.Append(", ");
                }
                sb.AppendLine(")");
                sb.AppendLine("		{");

                if (parameterList.Any())
                    sb.AppendLine("			var index = 0;");

                var paramIndex = 1;
                foreach (var parameter in parameterList)
                {
                    var pid = HashHelper.HashAlphaNumeric(parameter.Key);
                    sb.AppendLine("			var paramName" + paramIndex + " = \"X" + pid + "\";");
                    sb.AppendLine("			index = 0;");
                    sb.AppendLine("			while (_paramList.Contains(paramName1 + index)) index++;");
                    sb.AppendLine("			paramName1 = paramName1 + index;");
                    sb.AppendLine("			_paramList.Add(paramName1);");
                    sb.AppendLine("			if (_paramList.Count > 25) _paramList.RemoveAt(0);");

                    if (parameter.IsOutputParameter)
                    {
                        sb.AppendLine("			var " + parameter.CamelName + "Parameter = new ObjectParameter(paramName" + paramIndex + ", typeof(" + parameter.GetCodeType() + "));");
                    }
                    else if (parameter.AllowNull)
                    {
                        sb.AppendLine("			ObjectParameter " + parameter.CamelName + "Parameter = null;");
                        sb.AppendLine("			if (" + parameter.CamelName + " != null) { " + parameter.CamelName + "Parameter = new ObjectParameter(paramName" + paramIndex + ", " + parameter.CamelName + "); }");
                        sb.AppendLine("			else { " + parameter.CamelName + "Parameter = new ObjectParameter(paramName" + paramIndex + ", typeof(" + parameter.GetCodeType() + ")); }");
                    }
                    else
                    {
                        sb.AppendLine("			var " + parameter.CamelName + "Parameter = new ObjectParameter(paramName" + paramIndex + ", " + parameter.CamelName + ");");
                    }
                    paramIndex++;
                }

                paramIndex = 1;
                var inputParams = string.Join(", ", (parameterList.Select(x => "@\" + paramName" + paramIndex++ + " + \"")));

                var inputParamVars = string.Join(", ", (parameterList.Select(x => x.CamelName + "Parameter")));
                sb.AppendLine("			var retval = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext.CreateQuery<" + item.PascalName + ">(\"[" + _model.ProjectName + "Entities].[" + objectName + "](" + inputParams + ")\"" + (string.IsNullOrEmpty(inputParamVars) ? string.Empty : ", " + inputParamVars) + ");");

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

            #region IContext Interface
            sb.AppendLine("		#region IContext Interface");
            sb.AppendLine();
            sb.AppendLine("		Enum IContext.GetEntityFromField(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetEntityFromField(field);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		object IContext.GetMetaData(Enum entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetMetaData((EntityMappingConstants)entity);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		System.Type IContext.GetFieldType(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return this.GetFieldType(field);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region GetEntityFromField
            sb.AppendLine("		#region GetEntityFromField");
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
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region GetMetaData
            sb.AppendLine("		#region GetMetaData");
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
                //sb.AppendLine("return Activator.CreateInstance(((System.ComponentModel.DataAnnotations.MetadataTypeAttribute)typeof(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ").GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MetadataTypeAttribute), true).FirstOrDefault()).MetadataClassType) as " + this.GetLocalNamespace() + ".Entity.Metadata." + table.PascalName + "Metadata;");
                sb.AppendLine("return new " + GetLocalNamespace() + ".Entity.Metadata." + table.PascalName + "Metadata();");
            }
            sb.AppendLine("			}");
            sb.AppendLine("			throw new Exception(\"Entity not found!\");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static string GetTableName(" + this.GetLocalNamespace() + ".EntityMappingConstants entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			var item = GetMetaData(entity);");
            sb.AppendLine("			if (item == null) return null;");
            sb.AppendLine("			return item.GetTableName();");
            sb.AppendLine("		}");

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Extra
            sb.AppendLine("		#region Interface Extras");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Reloads the context object from database");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void ReloadItem(BaseEntity entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			this.Entry(entity).Reload();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Detaches the the object from context");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void DetachItem(BaseEntity entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			this.ObjectContext.Detach(entity);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region ObjectContext
            sb.AppendLine("		#region ObjectContext");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the object context");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public System.Data.Entity.Core.Objects.ObjectContext ObjectContext");
            sb.AppendLine("		{");
            sb.AppendLine("			get");
            sb.AppendLine("			{");
            sb.AppendLine("				if (_objectContext == null)");
            sb.AppendLine("					_objectContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext;");
            sb.AppendLine("				return _objectContext;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine("		private System.Data.Entity.Core.Objects.ObjectContext _objectContext = null;");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Accepts all changes made to objects in the object context");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void AcceptAllChanges()");
            sb.AppendLine("		{");
            sb.AppendLine("			this.ObjectContext.AcceptAllChanges();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the timeout of the database connection");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public int? CommandTimeout");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return this.Database.CommandTimeout; }");
            sb.AppendLine("			set { this.Database.CommandTimeout = value; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine();

            sb.AppendLine("	}");
            sb.AppendLine("	#endregion");
            sb.AppendLine();

            sb.AppendLine("	internal class CustomDatabaseInitializer<TContext> : IDatabaseInitializer<TContext> where TContext : global::System.Data.Entity.DbContext");
            sb.AppendLine("	{");
            sb.AppendLine("		public void InitializeDatabase(TContext context)");
            sb.AppendLine("		{");
            sb.AppendLine("		}");
            sb.AppendLine("	}");
            sb.AppendLine();
        }

        #endregion

    }
}