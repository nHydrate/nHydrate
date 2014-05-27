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
            sb.AppendLine("using nHydrate.EFCore.Exceptions;");
            sb.AppendLine();
        }

        private void AppendClass()
        {
            sb.AppendLine("	/// <summary/>");
            sb.AppendLine("	public enum DatabasePlatformConstants");
            sb.AppendLine("	{");
            sb.AppendLine("		/// <summary/>");
            sb.AppendLine("		SQLServer,");
            sb.AppendLine("		/// <summary/>");
            sb.AppendLine("		MySql,");
            sb.AppendLine("	}");
            sb.AppendLine();

            sb.AppendLine("	#region Entity Context");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// The object context for the schema tied to this generated model.");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	[DataContract]");
            sb.AppendLine("	[Serializable]");
            sb.AppendLine("	public partial class " + _model.ProjectName + "Entities : System.Data.Entity.DbContext, " + this.GetLocalNamespace() + ".Interfaces.I" + _model.ProjectName + "Entities, nHydrate.EFCore.DataAccess.IContext");
            sb.AppendLine("	{");

            //Create the modifier property
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The audit modifier used to mark database edits");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected ContextStartup _contextStartup = new ContextStartup(null);");
            sb.AppendLine();

            #region Constructors
            sb.AppendLine("	#region Constructors");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initializes a new " + _model.ProjectName + "Entities object using the connection string found in the '" + _model.ProjectName + "Entities' section of the application configuration file.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + _model.ProjectName + "Entities() :");
            sb.AppendLine("			base(Util.ConvertNormalCS2EFFromConfig(\"name=" + _model.ProjectName + "Entities\"))");
            sb.AppendLine("		{");
            sb.AppendLine("			this.CurrentPlatform = Util.GetDefinedPlatform();");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				var builder = new System.Data.Odbc.OdbcConnectionStringBuilder(Util.StripEFCS2Normal(this.ObjectContext.Connection.ConnectionString));");
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
            sb.AppendLine("			this.CurrentPlatform = contextStartup.CurrentPlatform;");
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
            sb.AppendLine("			this.CurrentPlatform = contextStartup.CurrentPlatform;");
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
            sb.AppendLine("			this.CurrentPlatform = Util.GetDefinedPlatform();");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				var builder = new System.Data.Odbc.OdbcConnectionStringBuilder(Util.StripEFCS2Normal(this.ObjectContext.Connection.ConnectionString));");
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

            sb.AppendLine("	#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		partial void OnContextCreated();");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Model creation event");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected override void OnModelCreating(DbModelBuilder modelBuilder)");
            sb.AppendLine("		{");
            sb.AppendLine("			base.OnModelCreating(modelBuilder);");
            sb.AppendLine("			modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();");
            sb.AppendLine("			Database.SetInitializer(new CustomDatabaseInitializer<" + _model.ProjectName + "Entities>());");
            sb.AppendLine();

            #region Rename Tables
            sb.AppendLine("			#region Rename Tables");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                if (table.IsTenant)
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().ToTable(\"" + _model.TenantPrefix + "_" + table.DatabaseName + "\");");
                else if (table.DatabaseName != table.PascalName)
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">().ToTable(\"" + table.DatabaseName + "\");");
            }
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            #region Create annotations for properties
            sb.AppendLine("			#region Setup Fields");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                sb.AppendLine("			//Field setup for " + table.PascalName + " entity");
                foreach (var column in table.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    //If this is a base table OR the column is not a PK then process it
                    //Primary key code should be emited ONLY for base tables
                    if (table.ParentTable == null || !column.PrimaryKey)
                    {
                        sb.Append("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + ">()");
                        sb.Append(".Property(d => d." + column.PascalName + ")");
                        if (!column.AllowNull) sb.Append(".IsRequired()");
                        if (column.IsTextType && column.DataType != System.Data.SqlDbType.Xml) sb.Append(".HasMaxLength(" + column.GetAnnotationStringLength() + ")");
                        if (column.DatabaseName != column.PascalName) sb.Append(".HasColumnName(\"" + column.DatabaseName + "\")");
                        sb.AppendLine(";");
                    }
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

                            sb.AppendLine("							 .WithOptional()");
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
                            foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
                            {
                                Column childColumn = columnRelationship.ChildColumnRef.Object as Column;
                                sb.Append("u." + childColumn.PascalName);
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
                    sb.AppendLine("				.HasMany(q => q." + relation2.ParentTable.PascalName + "List)");
                    sb.AppendLine("				.WithMany(q => q." + relation1.ParentTable.PascalName + "List)");
                    sb.AppendLine("				.Map(q =>");
                    sb.AppendLine("			{");
                    sb.AppendLine("				q.ToTable(\"" + table.PascalName + "\");");
                    sb.AppendLine("				q.MapLeftKey(" + string.Join(",", relation1.ParentTable.PrimaryKeyColumns.ToList().Select(x => "\"" + x.DatabaseName + "\"").ToList()) + ");");
                    sb.AppendLine("				q.MapRightKey(" + string.Join(",", relation2.ParentTable.PrimaryKeyColumns.ToList().Select(x => "\"" + x.DatabaseName + "\"").ToList()) + ");");
                    sb.AppendLine("			});");
                    sb.AppendLine();
                }
            }

            #endregion

            sb.AppendLine("			#endregion");
            sb.AppendLine();

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
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		}");
            sb.AppendLine();

            #region Entity Sets
            sb.AppendLine("		#region Entity Sets");
            sb.AppendLine();

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Entity set for " + table.PascalName);
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public DbSet<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> " + table.PascalName + " { get; set; }");
                sb.AppendLine();
            }

            foreach (var table in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Entity set for " + table.PascalName);
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public DbSet<" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "> " + table.PascalName + " { get; set; }");
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

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the supported database platforms");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public DatabasePlatformConstants CurrentPlatform { get; private set; }");
            sb.AppendLine();

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

                if (table.AllowCreateAudit || table.AllowModifiedAudit)
                {
                    sb.AppendLine("			if (entity is " + GetLocalNamespace() + ".Entity." + table.PascalName + ")");
                    sb.AppendLine("			{");
                    if (table.AllowCreateAudit)
                        sb.AppendLine("				((" + GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + _model.Database.CreatedByPascalName + " = _contextStartup.Modifer;");
                    if (table.AllowModifiedAudit)
                        sb.AppendLine("				((" + GetLocalNamespace() + ".Entity." + table.PascalName + ")entity)." + _model.Database.ModifiedByPascalName + " = _contextStartup.Modifer;");
                    sb.AppendLine("			}");
                }
                sb.AppendLine("			this." + table.PascalName + ".Add((" + GetLocalNamespace() + ".Entity." + table.PascalName + ")entity);");
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
                sb.AppendLine("			this." + view.PascalName + ".Add(entity);");
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
                sb.AppendLine("			this." + table.GetAbsoluteBaseTable().PascalName + ".Remove(entity);");
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
                sb.AppendLine("			this.DeleteItem(entity as " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ");");
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

            var index2 = 0;
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable && (x.TypedTable != TypedTableConstants.EnumOnly)))
            {
                sb.AppendLine("			" + (index2 > 0 ? "else " : string.Empty) + "if (entity is " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ")");
                sb.AppendLine("				this.DeleteItem(entity as " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ");");
                index2++;
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
            sb.AppendLine("				catch (Exception ex)");
            sb.AppendLine("				{");
            sb.AppendLine("					return null;");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("		}");

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the globally configured connection string for this context type");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static string GetConnectionString()");
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

            #region Context Interface Members
            sb.AppendLine("		#region I" + _model.ProjectName + " Members");
            sb.AppendLine();

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		System.Data.Objects.IObjectSet<" + this.GetLocalNamespace() + ".Interfaces.Entity.I" + table.PascalName + "> " + this.GetLocalNamespace() + ".Interfaces.I" + _model.ProjectName + "Entities." + table.PascalName);
                sb.AppendLine("		{");
                sb.AppendLine("			get { return (System.Data.Objects.IObjectSet<" + this.GetLocalNamespace() + ".Interfaces.Entity.I" + table.PascalName + ">)this." + table.PascalName + "; }");
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

                {
                    var index = 0;
                    foreach (var param in parameters)
                    {
                        sb.Append((param.IsOutputParameter ? "out " : string.Empty) + param.GetCodeType() + " " + param.CamelName);
                        if (index < storedProcedure.Parameters.Count - 1)
                            sb.Append(", ");
                        index++;
                    }
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

                {
                    var index = 0;
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

            #region IContext Interface
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
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region ObjectContext
            sb.AppendLine("		#region ObjectContext");
            sb.AppendLine();
            sb.AppendLine("		public System.Data.Entity.Core.Objects.ObjectContext ObjectContext");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public void AcceptAllChanges()");
            sb.AppendLine("		{");
            sb.AppendLine("			this.ObjectContext.AcceptAllChanges();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public int? CommandTimeout");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return this.ObjectContext.CommandTimeout; }");
            sb.AppendLine("			set { this.ObjectContext.CommandTimeout = value;; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("	#endregion");
            sb.AppendLine();

            sb.AppendLine("	}");
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