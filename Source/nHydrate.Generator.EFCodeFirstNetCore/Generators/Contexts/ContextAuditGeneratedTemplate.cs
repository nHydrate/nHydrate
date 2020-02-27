#pragma warning disable 0168
using System;
using System.Linq;
using nHydrate.Generator.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Contexts
{
    public class ContextAuditGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();

        public ContextAuditGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileName => $"{_model.ProjectName}AuditEntities.Generated.cs";
        public string ParentItemName => $"{_model.ProjectName}AuditEntities.cs";

        public override string FileContent
        {
            get
            {
                GenerateContent();
                return sb.ToString();
            }
        }

        private void GenerateContent()
        {
            try
            {
                nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
                sb.AppendLine("#pragma warning disable 612");
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Audit");
                sb.AppendLine("{");
                this.AppendClass();
                sb.AppendLine("}");
                sb.AppendLine();

                sb.AppendLine($"namespace {this.GetLocalNamespace()}" + ".Audit");
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

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using System.Threading;");
            sb.AppendLine("using System.Configuration;");
            sb.AppendLine();
        }

        private void AppendClass()
        {
            sb.AppendLine("	#region Entity Context");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// The object context for the Audit schema tied to this generated model.");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine($"	public partial class {_model.ProjectName}AuditEntities : DbContext");
            sb.AppendLine("	{");
            sb.AppendLine("		protected string _connectionString = null;");
            #region Constructors
            sb.AppendLine("		#region Constructors");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initializes a new " + _model.ProjectName + "AuditEntities object using the connection string found in the '" + _model.ProjectName + "AuditEntities' section of the application configuration file.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + _model.ProjectName + "AuditEntities() :");
            sb.AppendLine("			base()");
            sb.AppendLine("		{");
            sb.AppendLine("			_connectionString = ConfigurationManager.ConnectionStrings[\"" + _model.ProjectName + "Entities\"]?.ConnectionString;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initialize a new " + _model.ProjectName + "AuditEntities object with a connection string.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + _model.ProjectName + "AuditEntities(string connectionString) :");
            sb.AppendLine("			base()");
            sb.AppendLine("		{");
            sb.AppendLine("			_connectionString = connectionString;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

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
            foreach (var item in _model.Database.Tables.Where(x => x.AllowAuditTracking && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                string schema = null;
                if (!string.IsNullOrEmpty(item.DBSchema)) schema = item.DBSchema;

                if (string.IsNullOrEmpty(schema))
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Audit." + item.PascalName + "Audit>().ToTable(\"__AUDIT__" + item.DatabaseName + "\").HasKey(\"__RowId\");");
                else
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Audit." + item.PascalName + "Audit>().ToTable(\"__AUDIT__" + item.DatabaseName + "\", \"" + schema + "\").HasKey(\"__RowId\");");

            }
            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            #region Create annotations for properties
            sb.AppendLine("			#region Setup Fields");
            sb.AppendLine();

            //Tables
            foreach (var table in _model.Database.Tables.Where(x => x.AllowAuditTracking && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                sb.AppendLine($"			//Field setup for {table.PascalName}Audit entity");
                foreach (var column in table.GetColumns().OrderBy(x => x.Name))
                {
                    sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Audit." + table.PascalName + "Audit>().Property(d => d." + column.PascalName + ").HasColumnName(\"" + column.DatabaseName + "\");");
                }
                sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Audit." + table.PascalName + "Audit>().Property(d => d.__RowId).IsRequired().HasColumnName(\"__rowid\");");
                sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Audit." + table.PascalName + "Audit>().Property(d => d.AuditType).IsRequired().HasColumnName(\"__action\");");
                sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Audit." + table.PascalName + "Audit>().Property(d => d.AuditDate).IsRequired().HasColumnName(\"__insertdate\");");
                sb.AppendLine("			modelBuilder.Entity<" + this.GetLocalNamespace() + ".Audit." + table.PascalName + "Audit>().Property(d => d.ModifiedBy).IsRequired().HasColumnName(\"" + _model.Database.ModifiedByDatabaseName + "\");");
                sb.AppendLine();
            }

            sb.AppendLine("			#endregion");
            sb.AppendLine();
            #endregion

            sb.AppendLine("		}");
            sb.AppendLine();

            #endregion

            #region Entity Sets
            sb.AppendLine("		#region Entity Sets");
            sb.AppendLine();

            foreach (var item in _model.Database.Tables.Where(x => x.AllowAuditTracking && !x.AssociativeTable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Entity set for " + item.PascalName);
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public virtual DbSet<" + this.GetLocalNamespace() + ".Audit." + item.PascalName + "Audit> " + item.PascalName + "Audit { get; set; }");
                sb.AppendLine();
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            #endregion

            #region Override write functionality so cannot call it
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Write-only object. This should never be called.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public override int SaveChanges()");
            sb.AppendLine("		{");
            sb.AppendLine("			this.ChangeTracker.AcceptAllChanges();");
            sb.AppendLine("			return 0;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Write-only object. This should never be called.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))");
            sb.AppendLine("		{");
            sb.AppendLine("			this.ChangeTracker.AcceptAllChanges();");
            sb.AppendLine("			return System.Threading.Tasks.Task.Run(() => { return 0; });");
            sb.AppendLine("		}");
            sb.AppendLine();
            #endregion

            sb.AppendLine("	}");
            sb.AppendLine("	#endregion");
            sb.AppendLine();
        }

    }
}