#pragma warning disable 0168
using System;
using nHydrate.Generator.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Contexts
{
    public class ContextAuditExtenderTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();

        public ContextAuditExtenderTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return _model.ProjectName + "AuditEntities.cs"; }
        }

        public override string FileContent
        {
            get
            {
                try
                {
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

        public void GenerateContent()
        {
            try
            {
                sb.AppendLine("using System;");
                sb.AppendLine("using Microsoft.EntityFrameworkCore;");
                sb.AppendLine();
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Audit");
                sb.AppendLine("{");
                sb.AppendLine("	partial class " + _model.ProjectName + "AuditEntities");
                sb.AppendLine("	{");
                sb.AppendLine("		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)");
                sb.AppendLine("		{");
                sb.AppendLine("			CONFIGURE_THIS");
                sb.AppendLine("			//READ ME!!!!");
                sb.AppendLine("			//STEP 1: Add database provider");
                sb.AppendLine("			//Depending on your database provider add one of the following libraries in Nuget");
                sb.AppendLine("			//SQLServer: Microsoft.EntityFrameworkCore.SqlServer");
                sb.AppendLine("			//Postgres: Npgsql.EntityFrameworkCore.PostgreSQL");
                sb.AppendLine("			//Sqlite: Microsoft.EntityFrameworkCore.Sqlite");
                sb.AppendLine();
                sb.AppendLine("			if (string.IsNullOrEmpty(_connectionString?.Trim()))");
                sb.AppendLine("				throw new Exception(\"Missing connection string\");");
                sb.AppendLine();
                sb.AppendLine("			//STEP 2: Uncomment one of these based on database provider");
                sb.AppendLine("			//Add the appropriate line based on your database provider and delete the exception line below");
                sb.AppendLine("			throw new Exception(\"Object not configured!\"); //Delete this line");
                sb.AppendLine("			//optionsBuilder.UseSqlServer(_connectionString); //Sql Server");
                sb.AppendLine("			//optionsBuilder.UseNpgsql(_connectionString); //Postgres");
                sb.AppendLine("			//optionsBuilder.UseSqlite(_connectionString); //Sqlite");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("	}");
                sb.AppendLine("}");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

    }
}