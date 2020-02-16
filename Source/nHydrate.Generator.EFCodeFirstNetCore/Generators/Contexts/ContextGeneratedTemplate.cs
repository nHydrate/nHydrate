#pragma warning disable 0168
using System;
using System.Linq;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using nHydrate.Generator.Common;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Contexts
{
    public class ContextGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private ModelConfiguration _modelConfiguration = null;

        public ContextGeneratedTemplate(ModelRoot model)
            : base(model)
        {
            if (model.ModelConfigurations.ContainsKey(typeof(EFCodeFirstNetCoreProjectGenerator).Name))
                _modelConfiguration = model.ModelConfigurations[typeof(EFCodeFirstNetCoreProjectGenerator).Name] as ModelConfiguration;
            if (_modelConfiguration == null)
                _modelConfiguration = new ModelConfiguration();
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
                sb.AppendLine("#pragma warning disable 612");
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");
                this.AppendTableMapping();
                this.AppendClass();
                sb.AppendLine("}");
                sb.AppendLine();

                sb.AppendLine($"namespace {this.GetLocalNamespace()}.Entity");
                sb.AppendLine("{");
                sb.AppendLine("}");
                sb.AppendLine("#pragma warning restore 612");
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

            foreach (var table in _model.Database.Tables.Where(x => (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine($"		/// A mapping for the the {table.PascalName} entity");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine($"		{table.PascalName},");
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
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine($"using {this.GetLocalNamespace()}.Entity;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using System.Configuration;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore.ChangeTracking;");
            sb.AppendLine();
        }

        private void AppendClass()
        {
            sb.AppendLine("	#region Entity Context");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// The entity context for the defined model schema");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	public partial class " + _model.ProjectName + "Entities : Microsoft.EntityFrameworkCore.DbContext, " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities, IContext");
            sb.AppendLine("	{");

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static Action<string> QueryLogger { get; set; }");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// A unique key for this object instance");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public Guid InstanceKey { get; private set; }");
            sb.AppendLine();

            //Create the modifier property
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The audit modifier used to mark database edits");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected ContextStartup _contextStartup = new ContextStartup(null);");
            sb.AppendLine();

            sb.AppendLine("		private static object _seqCacheLock = new object();");
            sb.AppendLine();

            // Create consts for version and modelKey
            sb.AppendLine("		private const string _version = \"" + _model.Version + "." + _model.GeneratedVersion + "\";");
            sb.AppendLine("		private const string _modelKey = \"" + _model.Key + "\";");
            sb.AppendLine("		protected string _connectionString = null;");
            sb.AppendLine();

            //NETCORE REMOVED
            //Events
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs> BeforeSaveModifiedEntity;");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected virtual void OnBeforeSaveModifiedEntity(" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if(this.BeforeSaveModifiedEntity != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				this.BeforeSaveModifiedEntity(this, e);");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs> BeforeSaveAddedEntity;");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected virtual void OnBeforeSaveAddedEntity(" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if(this.BeforeSaveAddedEntity != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				this.BeforeSaveAddedEntity(this, e);");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs> AfterSaveModifiedEntity;");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected virtual void OnAfterSaveModifiedEntity(" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if(this.AfterSaveModifiedEntity != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				this.AfterSaveModifiedEntity(this, e);");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs> AfterSaveAddedEntity;");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected virtual void OnAfterSaveAddedEntity(" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if(this.AfterSaveAddedEntity != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				this.AfterSaveAddedEntity(this, e);");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            #region Constructors
            sb.AppendLine("		#region Constructors");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine($"	/// Initializes a new {_model.ProjectName}Entities object using the connection string found in the '{_model.ProjectName}Entities' section of the application configuration file.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		public {_model.ProjectName}Entities() :");
            sb.AppendLine("			base()");
            sb.AppendLine("		{");
            sb.AppendLine("			_connectionString = ConfigurationManager.ConnectionStrings[\"" + _model.ProjectName + "Entities\"]?.ConnectionString;");
            sb.AppendLine("			InstanceKey = Guid.NewGuid();");
            sb.AppendLine("			_contextStartup = new ContextStartup(null, true);");
            sb.AppendLine("			this.CommandTimeout = _contextStartup.CommandTimeout;");
            sb.AppendLine("			this.OnContextCreated();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine($"	/// Initialize a new {_model.ProjectName}Entities object with an audit modifier.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		public {_model.ProjectName}Entities(ContextStartup contextStartup) :");
            sb.AppendLine("				base()");
            sb.AppendLine("		{");
            sb.AppendLine("			_connectionString = ConfigurationManager.ConnectionStrings[\"" + _model.ProjectName + "Entities\"]?.ConnectionString;");
            sb.AppendLine("			InstanceKey = Guid.NewGuid();");
            sb.AppendLine("			_contextStartup = contextStartup;");
            sb.AppendLine("			this.CommandTimeout = _contextStartup.CommandTimeout;");
            sb.AppendLine("			this.OnContextCreated();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine($"	/// Initialize a new {_model.ProjectName}Entities object with an audit modifier.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		public {_model.ProjectName}Entities(ContextStartup contextStartup, string connectionString) :");
            sb.AppendLine("				base()");
            sb.AppendLine("		{");
            sb.AppendLine("			_connectionString = connectionString;");
            sb.AppendLine("			InstanceKey = Guid.NewGuid();");
            sb.AppendLine("			_contextStartup = contextStartup;");
            sb.AppendLine("			this.CommandTimeout = _contextStartup.CommandTimeout;");
            sb.AppendLine("			this.OnContextCreated();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine($"	/// Initialize a new {_model.ProjectName}Entities object with an audit modifier.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		public {_model.ProjectName}Entities(string connectionString) :");
            sb.AppendLine("				base()");
            sb.AppendLine("		{");
            sb.AppendLine("			_connectionString = connectionString;");
            sb.AppendLine("			InstanceKey = Guid.NewGuid();");
            sb.AppendLine("			_contextStartup = new ContextStartup(null, true);");
            sb.AppendLine("			this.CommandTimeout = _contextStartup.CommandTimeout;");
            sb.AppendLine("			this.OnContextCreated();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		partial void OnContextCreated();");
            sb.AppendLine("		partial void OnBeforeSaveChanges(ref bool cancel);");
            sb.AppendLine("		partial void OnAfterSaveChanges();");
            sb.AppendLine("		partial void OnModelCreated(ModelBuilder modelBuilder);");
            sb.AppendLine();

            #region OnModelCreating

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Model creation event");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected override void OnModelCreating(ModelBuilder modelBuilder)");
            sb.AppendLine("		{");
            sb.AppendLine("			base.OnModelCreating(modelBuilder);");
            sb.AppendLine();

            #region Map Tables
            sb.AppendLine("			#region Map Tables");

            //Tables
            foreach (var item in _model.Database.Tables.Where(x => !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                string schema = null;
                if (!string.IsNullOrEmpty(item.DBSchema)) schema = item.DBSchema;
                var dbTableName = item.DatabaseName;
                if (item.IsTenant)
                    dbTableName = _model.TenantPrefix + "_" + item.DatabaseName;

                if (string.IsNullOrEmpty(schema))
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">().ToTable(\"" + dbTableName + "\");");
                else
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">().ToTable(\"" + dbTableName + "\", \"" + schema + "\");");
            }

            //Views
            foreach (var item in _model.Database.CustomViews.OrderBy(x => x.DatabaseName))
            {
                string schema = null;
                if (!string.IsNullOrEmpty(item.DBSchema)) schema = item.DBSchema;
                var dbTableName = item.DatabaseName;

                if (string.IsNullOrEmpty(schema))
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">().ToTable(\"" + dbTableName + "\");");
                else
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">().ToTable(\"" + dbTableName + "\", \"" + schema + "\");");
            }

            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            #region Create annotations for properties
            sb.AppendLine("			#region Setup Fields");
            sb.AppendLine();

            //Tables
            foreach (var table in _model.Database.Tables.Where(x => (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                sb.AppendLine("			//Field setup for " + table.PascalName + " entity");
                foreach (var column in table.GetColumns().OrderBy(x => x.Name))
                {
                    #region Determine if this is a type table Value field and if so ignore
                    {
                        Table typeTable = null;
                        if (table.IsColumnRelatedToTypeTable(column, out string pascalRoleName) || (column.PrimaryKey && table.TypedTable != TypedTableConstants.None))
                        {
                            typeTable = table.GetRelatedTypeTableByColumn(column, out pascalRoleName);
                            if (typeTable == null) typeTable = table;
                            if (typeTable != null)
                                sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().Ignore(d => d." + pascalRoleName + typeTable.PascalName + "Value);");
                        }
                    }
                    #endregion

                    //If the column is not a PK then process it
                    //if (!column.PrimaryKey)
                    {
                        sb.Append($"			modelBuilder.Entity<{this.GetLocalNamespace()}.Entity.{table.PascalName}>()");
                        sb.Append($".Property(d => d.{column.PascalName})");

                        if (column.AllowNull)
                            sb.Append(".IsRequired(false)");
                        else
                            sb.Append(".IsRequired(true)");

                        if (column.Identity == IdentityTypeConstants.Database && column.DataType.IsIntegerType())
                        {
                            switch (_modelConfiguration.DatabaseType)
                            {
                                case DatabaseTypeConstants.SqlServer:
                                    sb.Append(".ValueGeneratedOnAdd()");
                                    break;
                                case DatabaseTypeConstants.Postgress:
                                    sb.Append(".ValueGeneratedOnAdd()");
                                    break;
                                case DatabaseTypeConstants.Sqlite:
                                    sb.Append(".ValueGeneratedOnAdd()");
                                    break;
                            }
                        }

                        if (column.DataType.IsTextType() && column.Length > 0 && column.DataType != System.Data.SqlDbType.Xml)
                            sb.Append($".HasMaxLength({column.GetAnnotationStringLength()})");
                        if (column.DatabaseName != column.PascalName)
                            sb.Append($".HasColumnName(\"{column.DatabaseName}\")");
                        sb.AppendLine(";");
                    }
                }

                if (table.AllowCreateAudit)
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().Property(d => d." + _model.Database.CreatedDateColumnName + ").IsRequired();");
                if (table.AllowModifiedAudit)
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().Property(d => d." + _model.Database.ModifiedDateColumnName + ").IsRequired();");

                if (table.AllowTimestamp)
                {
                    if (!String.Equals(_model.Database.TimestampDatabaseName, _model.Database.TimestampPascalName, StringComparison.OrdinalIgnoreCase))
                    {
                        sb.Append("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">()");
                        sb.Append(".Property(d => d." + _model.Database.TimestampPascalName + ")");
                        sb.Append(".HasColumnName(\"" + _model.Database.TimestampDatabaseName + "\")");
                        sb.AppendLine(";");
                    }
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().Property(d => d." + _model.Database.TimestampPascalName + ").HasMaxLength(8).IsRowVersion();");
                }

                sb.AppendLine();
            }

            //Views
            foreach (var item in _model.Database.CustomViews.OrderBy(x => x.Name))
            {
                sb.AppendLine("			//Field setup for " + item.PascalName + " entity");
                foreach (var column in item.GetColumns().OrderBy(x => x.Name))
                {
                    sb.Append("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">()");
                    sb.Append(".Property(d => d." + column.PascalName + ")");
                    if (!column.AllowNull)
                        sb.Append(".IsRequired()");

                    if (column.DataType.IsTextType() && column.Length > 0 && column.DataType != System.Data.SqlDbType.Xml) sb.Append(".HasMaxLength(" + column.GetAnnotationStringLength() + ")");
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
            foreach (var table in _model.Database.Tables.Where(x => (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                foreach (var column in table.GetColumns().OrderBy(x => x.Name))
                {
                    if (table.IsColumnRelatedToTypeTable(column, out var pascalRoleName) || (column.PrimaryKey && table.TypedTable != TypedTableConstants.None))
                    {
                        var typeTable = table.GetRelatedTypeTableByColumn(column, out pascalRoleName);
                        if (typeTable == null) typeTable = table;
                        if (typeTable != null)
                        {
                            sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().Ignore(t => t." + pascalRoleName + typeTable.PascalName + "Value);");
                        }
                    }
                }
            }
            sb.AppendLine();
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            #region Primary Keys
            sb.AppendLine("			#region Primary Keys");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables
                .Where(x => (x.TypedTable != Models.TypedTableConstants.EnumOnly))
                .OrderBy(x => x.Name))
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

            foreach (var table in _model.Database.CustomViews.OrderBy(x => x.Name))
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

            #region Create annotations for relationships
            sb.AppendLine("			#region Relations");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                foreach (Relation relation in table.GetRelations())
                {
                    Table childTable = relation.ChildTableRef.Object as Table;
                    if (!childTable.IsInheritedFrom(table) && !childTable.AssociativeTable)
                    {
                        if (relation.IsOneToOne)
                        {
                            sb.AppendLine($"			//Relation [{table.PascalName}] -> [{childTable.PascalName}] (Multiplicity 1:1)");
                            sb.AppendLine($"			modelBuilder.Entity<{this.GetLocalNamespace()}.Entity.{table.PascalName}>()");
                            sb.AppendLine($"							.HasOne(a => a.{relation.PascalRoleName}{childTable.PascalName})");
                            sb.AppendLine($"							.WithOne(x => x.{relation.PascalRoleName}{table.PascalName})");
                            sb.AppendLine("							.HasForeignKey<" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + ">(q => new { " + string.Join(",", relation.ColumnRelationships.Select(x => x.ChildColumn.Name).OrderBy(x => x).Select(c => "q." + c)) + " })");
                            sb.AppendLine("							.HasPrincipalKey<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">(q => new { " + string.Join(",", relation.ColumnRelationships.Select(x => x.ParentColumn.Name).OrderBy(x => x).Select(c => "q." + c)) + " })");
                            if (relation.IsRequired)
                                sb.AppendLine("							.IsRequired(true)");
                            else
                                sb.AppendLine("							.IsRequired(false)");

                            //Specify what to do on delete
                            if (relation.DeleteAction == Relation.DeleteActionConstants.Cascade)
                                sb.AppendLine("							.OnDelete(DeleteBehavior.Cascade);");
                            else if (relation.DeleteAction == Relation.DeleteActionConstants.SetNull)
                                sb.AppendLine("							.OnDelete(DeleteBehavior.SetNull);");
                            else if (relation.DeleteAction == Relation.DeleteActionConstants.NoAction)
                                sb.AppendLine("							.OnDelete(DeleteBehavior.Restrict);");

                        }
                        else
                        {
                            sb.AppendLine($"			//Relation [{table.PascalName}] -> [{childTable.PascalName}] (Multiplicity 1:N)");
                            sb.AppendLine($"			modelBuilder.Entity<{this.GetLocalNamespace()}.Entity.{childTable.PascalName}>()");
                            sb.AppendLine($"							.HasOne(a => a.{relation.PascalRoleName}{table.PascalName})");

                            if (relation.IsOneToOne)
                                sb.AppendLine($"							.WithOne(x => x.{relation.PascalRoleName}{childTable.PascalName})");
                            else
                                sb.AppendLine($"							.WithMany(b => b.{relation.PascalRoleName}{childTable.PascalName}List)");

                            if (relation.IsRequired)
                                sb.AppendLine("							.IsRequired(true)");
                            else
                                sb.AppendLine("							.IsRequired(false)");

                            sb.AppendLine("							.HasPrincipalKey(q => new { " + string.Join(", ", relation.ColumnRelationships.Select(x => x.ParentColumn.Name).OrderBy(x => x).Select(c => "q." + c)) + " })");
                            sb.Append("							.HasForeignKey(u => new { ");

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

                            var indexName = ("FK_" + relation.RoleName + "_" + relation.ChildTable.DatabaseName + "_" + relation.ParentTable.DatabaseName).ToUpper();
                            sb.AppendLine($"							.HasConstraintName(\"{indexName}\")");

                            //Specify what to do on delete
                            if (relation.DeleteAction == Relation.DeleteActionConstants.Cascade)
                                sb.AppendLine("							.OnDelete(DeleteBehavior.Cascade);");
                            else if (relation.DeleteAction == Relation.DeleteActionConstants.SetNull)
                                sb.AppendLine("							.OnDelete(DeleteBehavior.SetNull);");
                            else if (relation.DeleteAction == Relation.DeleteActionConstants.NoAction)
                                sb.AppendLine("							.OnDelete(DeleteBehavior.Restrict);");
                        }

                        sb.AppendLine();
                    }
                }
            }

            //Associative tables
            foreach (var table in _model.Database.Tables.Where(x => x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                var relations = table.GetRelationsWhereChild().ToList();
                if (relations.Count == 2)
                {
                    var relation1 = relations.First();
                    var relation2 = relations.Last();

                    var index1Name = ("FK_" + relation1.RoleName + "_" + relation1.ChildTable.DatabaseName + "_" + relation1.ParentTable.DatabaseName).ToUpper();
                    var index2Name = ("FK_" + relation2.RoleName + "_" + relation2.ChildTable.DatabaseName + "_" + relation2.ParentTable.DatabaseName).ToUpper();

                    sb.AppendLine($"			//Relation for [{relation1.ParentTable.PascalName}] -> [{relation2.ParentTable.PascalName}] (Multiplicity N:M)");
                    sb.AppendLine($"			modelBuilder.Entity<{this.GetLocalNamespace()}.Entity.{relation1.ParentTable.PascalName}>()");
                    sb.AppendLine($"							.HasMany(q => q.{relation1.PascalRoleName}{table.PascalName}List)");
                    sb.AppendLine($"							.WithOne(q => q.{relation1.PascalRoleName}{relation1.ParentTable.PascalName})");
                    sb.AppendLine($"							.HasConstraintName(\"{index1Name}\")");
                    sb.AppendLine("							.HasPrincipalKey(q => new { " + string.Join(", ", relation1.ColumnRelationships.Select(x => x.ParentColumn.Name).OrderBy(x => x).Select(c => "q." + c)) + " })");
                    sb.AppendLine("							.HasForeignKey(q => new { " + string.Join(", ", relation1.ColumnRelationships.Select(x => x.ChildColumn.Name).OrderBy(x => x).Select(c => "q." + c)) + " })");
                    sb.AppendLine("							.OnDelete(DeleteBehavior.Restrict);");
                    sb.AppendLine();

                    sb.AppendLine($"			//Relation for [{relation2.ParentTable.PascalName}] -> [{relation1.ParentTable.PascalName}] (Multiplicity N:M)");
                    sb.AppendLine($"			modelBuilder.Entity<{this.GetLocalNamespace()}.Entity.{relation2.ParentTable.PascalName}>()");
                    sb.AppendLine($"							.HasMany(q => q.{relation2.PascalRoleName}{table.PascalName}List)");
                    sb.AppendLine($"							.WithOne(q => q.{relation2.PascalRoleName}{relation2.ParentTable.PascalName})");
                    sb.AppendLine($"							.HasConstraintName(\"{index2Name}\")");
                    sb.AppendLine("							.HasPrincipalKey(q => new { " + string.Join(", ", relation2.ColumnRelationships.Select(x => x.ParentColumn.Name).OrderBy(x => x).Select(c => "q." + c)) + " })");
                    sb.AppendLine("							.HasForeignKey(q => new { " + string.Join(", ", relation2.ColumnRelationships.Select(x => x.ChildColumn.Name).OrderBy(x => x).Select(c => "q." + c)) + " })");
                    sb.AppendLine("							.OnDelete(DeleteBehavior.Restrict);");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("			#endregion");
            sb.AppendLine();

            #endregion

            #region Stored Procedures
            sb.AppendLine("			#region Stored Procedures");
            sb.AppendLine();
            
            foreach (var item in _model.Database.CustomStoredProcedures.OrderBy(x => x.Name))
            {
                //Need a fake PK
                sb.Append("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + ">().HasKey(x => new { ");
                var columnList = item.GetColumns().OrderBy(x => x.Name).ToList();
                foreach (var c in columnList)
                {
                    sb.Append("x." + c.PascalName);
                    if (columnList.IndexOf(c) < columnList.Count - 1)
                        sb.Append(", ");
                }
                sb.AppendLine(" }); // Need fake PK");
            }

            sb.AppendLine();
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("			// Override this event in the partial class to add any custom model changes or validation");
            sb.AppendLine("			this.OnModelCreated(modelBuilder);");
            sb.AppendLine();
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

            sb.AppendLine("			var markedTime = " + (_model.UseUTCTime ? "System.DateTime.UtcNow" : "System.DateTime.Now") + ";");
            sb.AppendLine();

            #region Added Items
            sb.AppendLine("			//Get the added list");
            sb.AppendLine("			var addedList = this.ChangeTracker.Entries().Where(x => x.State == EntityState.Added);");
            sb.AppendLine("			//Process added list");
            sb.AppendLine("			foreach (var item in addedList)");
            sb.AppendLine("			{");
            sb.AppendLine("				var entity = item.Entity as IAuditable;");
            sb.AppendLine("				if (entity != null)");
            sb.AppendLine("				{");
            sb.AppendLine("					var audit = entity as IAuditableSet;");
            sb.AppendLine("					if (entity.IsModifyAuditImplemented && entity.ModifiedBy != this.ContextStartup.Modifier)");
            sb.AppendLine("					{");
            sb.AppendLine("						if (audit != null) audit.CreatedBy = this.ContextStartup.Modifier;");
            sb.AppendLine("						if (audit != null) audit.ModifiedBy = this.ContextStartup.Modifier;");
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
            sb.AppendLine("			var modifiedList = this.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);");
            sb.AppendLine("			foreach (var item in modifiedList)");
            sb.AppendLine("			{");
            sb.AppendLine("				var entity = item.Entity as IAuditable;");
            sb.AppendLine("				if (entity != null)");
            sb.AppendLine("				{");
            sb.AppendLine("					var audit = entity as IAuditableSet;");
            sb.AppendLine("					if (entity.IsModifyAuditImplemented && entity.ModifiedBy != this.ContextStartup.Modifier)");
            sb.AppendLine("					{");
            sb.AppendLine("						if (audit != null) audit.ModifiedBy = this.ContextStartup.Modifier;");
            sb.AppendLine("					}");
            sb.AppendLine("					audit.ModifiedDate = markedTime;");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			this.OnBeforeSaveModifiedEntity(new EventArguments.EntityListEventArgs { List = modifiedList });");
            sb.AppendLine();
            #endregion

            sb.AppendLine("			var retval = 0;");
            sb.AppendLine("			Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction customTrans = null;");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				_paramList.Clear();");
            sb.AppendLine("				if (base.Database.CurrentTransaction == null)");
            sb.AppendLine("					customTrans = base.Database.BeginTransaction();");
            sb.AppendLine("				retval += base.SaveChanges();");
            sb.AppendLine("				if (customTrans != null)");
            sb.AppendLine("					customTrans.Commit();");
            sb.AppendLine("			}");
            sb.AppendLine("			catch");
            sb.AppendLine("			{");
            sb.AppendLine("				throw;");
            sb.AppendLine("			}");
            sb.AppendLine("			finally");
            sb.AppendLine("			{");
            sb.AppendLine("				if (customTrans != null)");
            sb.AppendLine("					customTrans.Dispose();");
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

            foreach (var item in _model.Database.Tables.Where(x => (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                var name = item.PascalName;
                var scope = "public";
                if (item.AssociativeTable)
                    scope = "protected";

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Entity set for " + item.PascalName);
                sb.AppendLine("		/// </summary>");
                sb.AppendLine($"		{scope} virtual DbSet<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + name + " { get; set; }");
                sb.AppendLine();
            }

            foreach (var item in _model.Database.CustomViews.OrderBy(x => x.Name))
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

            //TODO: Make for all databases
            //Removed for EF core because it is SQL Server specific
            //sb.AppendLine("		/// <summary>");
            //sb.AppendLine("		/// Retrieves the latest database version for the current model");
            //sb.AppendLine("		/// </summary>");
            //sb.AppendLine("		public string GetDBVersion(string connectionString)");
            //sb.AppendLine("		{");
            //sb.AppendLine("			var conn = new System.Data.SqlClient.SqlConnection();");
            //sb.AppendLine("			try");
            //sb.AppendLine("			{");
            //sb.AppendLine("				conn.ConnectionString = connectionString;");
            //sb.AppendLine("				conn.Open();");
            //sb.AppendLine();
            //sb.AppendLine("				var command = new SqlCommand(\"SELECT dbVersion FROM [__nhydrateschema] where [ModelKey] = '\" + this.ModelKey + \"'\", conn);");
            //sb.AppendLine("				using (var reader = command.ExecuteReader())");
            //sb.AppendLine("				{");
            //sb.AppendLine("					while (reader.Read())");
            //sb.AppendLine("					{");
            //sb.AppendLine("						return (string)reader[0];");
            //sb.AppendLine("					}");
            //sb.AppendLine("				}");
            //sb.AppendLine("				return string.Empty;");
            //sb.AppendLine("			}");
            //sb.AppendLine("			catch (Exception)");
            //sb.AppendLine("			{");
            //sb.AppendLine("				return string.Empty;");
            //sb.AppendLine("			}");
            //sb.AppendLine("			finally");
            //sb.AppendLine("			{");
            //sb.AppendLine("				if (conn != null)");
            //sb.AppendLine("					conn.Close();");
            //sb.AppendLine("			}");
            //sb.AppendLine("		}");
            //sb.AppendLine();
            #endregion

            #region Add Functionality
            //Add an strongly-typed extension for "AddItem" method
            sb.AppendLine("		#region AddItem Methods");
            sb.AppendLine();

            sb.AppendLine("        public override EntityEntry Add( object entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            //No model validation. You should use the AddItem method.");
            sb.AppendLine("            return base.Add(entity);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public override EntityEntry<TEntity> Add<TEntity>( TEntity entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            //No model validation. You should use the AddItem method.");
            sb.AppendLine("            return base.Add(entity);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public override void AddRange(IEnumerable<object> entities)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (entities == null) return;");
            sb.AppendLine("            //This will enforce model validation.");
            sb.AppendLine("            foreach (var item in entities)");
            sb.AppendLine("            {");
            sb.AppendLine("                var entity = item as IBusinessObject;");
            sb.AppendLine("                if (entity == null)");
            sb.AppendLine("                    throw new Exception(\"Unknown entity type\");");
            sb.AppendLine("                this.AddItem(entity);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public override void AddRange(params object[] entities)");
            sb.AppendLine("        {");
            sb.AppendLine("            this.AddRange(entities?.AsEnumerable());");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public virtual void AddRange(IEnumerable<IBusinessObject> entities)");
            sb.AppendLine("        {");
            sb.AppendLine("            this.AddRange(entities?.AsEnumerable<object>());");
            sb.AppendLine("        }");
            sb.AppendLine();

            #region Tables
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Adds an entity of to the object context");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"entity\">The entity to add</param>");
            sb.AppendLine($"		public virtual {this.GetLocalNamespace()}.IBusinessObject AddItem({this.GetLocalNamespace()}.IBusinessObject entity)");
            sb.AppendLine("		{");

            sb.AppendLine("			if (entity == null) throw new NullReferenceException();");
            sb.AppendLine($"			var audit = entity as {this.GetLocalNamespace()}.IAuditableSet;");
            sb.AppendLine("			if (audit != null)");
            sb.AppendLine("			{");
            sb.AppendLine("				audit.CreatedBy = _contextStartup.Modifier;");
            sb.AppendLine("				audit.ModifiedBy = _contextStartup.Modifier;");
            sb.AppendLine("			}");

            sb.AppendLine("			if (false) { }");
            foreach (var table in _model.Database.Tables.Where(x => !x.Immutable).OrderBy(x => x.PascalName))
            {
                sb.AppendLine($"			else if (entity is {GetLocalNamespace()}.Entity.{table.PascalName})");
                sb.AppendLine("			{");
                sb.AppendLine($"				this.Add(entity);");
                sb.AppendLine("			}");
            }

            //If not an entity then throw exception
            sb.AppendLine($"			else");
            sb.AppendLine("			{");
            sb.AppendLine("				//If not an entity then throw exception");
            sb.AppendLine("				throw new Exception(\"Unknown entity type\");");
            sb.AppendLine("			}");

            sb.AppendLine("			return entity;");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Delete Functionality
            //Add an strongly-typed extension for "RemoveItem" method
            sb.AppendLine("		#region RemoveItem Methods");
            sb.AppendLine();

            sb.AppendLine("        public override EntityEntry Remove( object entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            //No model validation. You should use the RemoveItem method.");
            sb.AppendLine("            return base.Remove(entity);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public override EntityEntry<TEntity> Remove<TEntity>( TEntity entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            //No model validation. You should use the RemoveItem method.");
            sb.AppendLine("            return base.Remove(entity);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public override void RemoveRange(IEnumerable<object> entities)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (entities == null) return;");
            sb.AppendLine("            foreach (var item in entities)");
            sb.AppendLine("            {");
            sb.AppendLine("                var entity = item as IBusinessObject;");
            sb.AppendLine("                if (entity == null)");
            sb.AppendLine("                    throw new Exception(\"Unknown entity type\");");
            sb.AppendLine("                this.RemoveItem(entity);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public override void RemoveRange(params object[] entities)");
            sb.AppendLine("        {");
            sb.AppendLine("            this.RemoveRange(entities?.AsEnumerable());");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public virtual void RemoveRange(IEnumerable<IBusinessObject> entities)");
            sb.AppendLine("        {");
            sb.AppendLine("            this.RemoveRange(entities?.AsEnumerable<object>());");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Deletes an entity from the context");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"entity\">The entity to delete</param>");
            sb.AppendLine("		public virtual void RemoveItem(IBusinessObject entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (entity == null) return;");
            sb.AppendLine("			else this.Remove(entity);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Connection String
            sb.AppendLine("		#region Connection String");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the connection string used for this context object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public string ConnectionString");
            sb.AppendLine("		{");
            sb.AppendLine("			get");
            sb.AppendLine("			{");
            sb.AppendLine("				try");
            sb.AppendLine("				{");
            sb.AppendLine("					if (this.Database.GetDbConnection() != null && !string.IsNullOrEmpty(this.Database.GetDbConnection().ConnectionString))");
            sb.AppendLine("						return Util.StripEFCS2Normal(this.Database.GetDbConnection().ConnectionString);");
            sb.AppendLine("					else return null;");
            sb.AppendLine("				}");
            sb.AppendLine("				catch (Exception) { return null; }");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            #endregion

            #region Context Interface Members
            sb.AppendLine($"		#region I{_model.ProjectName} Members");
            sb.AppendLine();

            #region Tables
            foreach (var item in _model.Database.Tables.Where(x => (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities." + item.PascalName);
                sb.AppendLine("		{");
                sb.AppendLine("			get { return this." + item.PascalName + ".AsQueryable(); }");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            #region View
            foreach (var item in _model.Database.CustomViews.OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + this.GetLocalNamespace() + ".I" + _model.ProjectName + "Entities." + item.PascalName);
                sb.AppendLine("		{");
                sb.AppendLine("			get { return this." + item.PascalName + ".AsQueryable(); }");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            #region Stored Procedures
            foreach (var item in _model.Database.CustomStoredProcedures.Where(x => x.GeneratedColumns.Count > 0).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                sb.Append("		public List<" + item.PascalName + "> " + item.PascalName + "(");
                var parameterList = item.GetParameters().ToList();
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

                sb.Append("			var retval = this.Set<" + item.PascalName + ">().FromSql(\"[" + item.GetDatabaseObjectName() + "] " + string.Join(", ", spParamString) + "\"");

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
                var paramset = item.GetParameters().ToList();
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
            foreach (var item in _model.Database.CustomStoredProcedures.Where(x => x.GeneratedColumns.Count == 0).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Calls the " + item.PascalName + " action");
                sb.AppendLine("		/// </summary>");
                sb.Append("		public void " + item.PascalName + "(");

                var parameterList = item.GetParameters().ToList();
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
            foreach (var item in _model.Database.Functions.Where(x => x.IsTable).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		/// " + item.Description);
                sb.AppendLine("		/// </summary>");

                sb.Append("		public virtual IQueryable<" + item.PascalName + "> " + item.PascalName + "(");
                var parameterList = item.GetParameters().ToList();
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
            foreach (var table in _model.Database.Tables.Where(x => !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
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
            foreach (var table in _model.Database.Tables.Where(x => !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
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

            //NETCORE REMOVED
            //sb.AppendLine("		/// <summary>");
            //sb.AppendLine("		/// Detaches the the object from context");
            //sb.AppendLine("		/// </summary>");
            //sb.AppendLine("		public void DetachItem(BaseEntity entity)");
            //sb.AppendLine("		{");
            //sb.AppendLine("			this.ObjectContext.Detach(entity);");
            //sb.AppendLine("		}");
            //sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region ObjectContext
            sb.AppendLine("		#region ObjectContext");
            sb.AppendLine();
            
            //NETCORE REMOVED
            //sb.AppendLine("		/// <summary>");
            //sb.AppendLine("		/// Gets the object context");
            //sb.AppendLine("		/// </summary>");
            //sb.AppendLine("		public System.Data.Entity.Core.Objects.ObjectContext ObjectContext");
            //sb.AppendLine("		{");
            //sb.AppendLine("			get");
            //sb.AppendLine("			{");
            //sb.AppendLine("				if (_objectContext == null)");
            //sb.AppendLine("					_objectContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext;");
            //sb.AppendLine("				return _objectContext;");
            //sb.AppendLine("			}");
            //sb.AppendLine("		}");
            //sb.AppendLine("		private System.Data.Entity.Core.Objects.ObjectContext _objectContext = null;");
            //sb.AppendLine();

            //NETCORE REMOVED
            //sb.AppendLine("		/// <summary>");
            //sb.AppendLine("		/// Accepts all changes made to objects in the object context");
            //sb.AppendLine("		/// </summary>");
            //sb.AppendLine("		public void AcceptAllChanges()");
            //sb.AppendLine("		{");
            //sb.AppendLine("			this.ObjectContext.AcceptAllChanges();");
            //sb.AppendLine("		}");
            //sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the timeout of the database connection");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public int? CommandTimeout");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return this.Database.GetCommandTimeout(); }");
            sb.AppendLine("			set { this.Database.SetCommandTimeout(value); }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("	}");
            sb.AppendLine("	#endregion");
            sb.AppendLine();
        }

        #endregion

    }
}