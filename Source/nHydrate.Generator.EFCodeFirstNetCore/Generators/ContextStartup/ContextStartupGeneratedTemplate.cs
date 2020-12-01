using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextStartup
{
    public class ContextStartupGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        public ContextStartupGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileName => "ContextStartup.Generated.cs";
        public string ParentItemName => "ContextStartup.cs";

        public override string FileContent { get => Generate(); }

        public override string Generate()
        {
            var sb = new StringBuilder();
            GenerationHelper.AppendFileGeneatedMessageInCode(sb);
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine("namespace " + this.GetLocalNamespace());
            sb.AppendLine("{");

            sb.AppendLine("	#region IContextStartup");
            sb.AppendLine("    public interface IContextStartup");
            sb.AppendLine("    {");
            sb.AppendLine("        string Modifier { get; }");
            sb.AppendLine("        bool AllowLazyLoading { get; }");
            sb.AppendLine("        int CommandTimeout { get; }");
            sb.AppendLine("    }");
            sb.AppendLine("	#endregion");
            sb.AppendLine();

            sb.AppendLine("	#region ContextStartup");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// This object holds the initialization information for a context");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");
            sb.AppendLine("	public partial class ContextStartup : IContextStartup, ICloneable");
            sb.AppendLine("	{");
            sb.AppendLine($"		protected internal string DebugInfo {GetSetSuffix}");
            sb.AppendLine("		protected internal bool DefaultTimeout { get; private set; }");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a new instance of the ContextStartup object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public ContextStartup(string modifier)");
            sb.AppendLine("		{");
            sb.AppendLine("			this.CommandTimeout = 30;");
            sb.AppendLine("			this.Modifier = modifier;");
            sb.AppendLine("			this.AllowLazyLoading = true;");
            sb.AppendLine("			this.DefaultTimeout = true;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a new instance of the ContextStartup object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public ContextStartup(string modifier, bool allowLazyLoading) :");
            sb.AppendLine("			this(modifier)");
            sb.AppendLine("		{");
            sb.AppendLine("			this.AllowLazyLoading = allowLazyLoading;");
            sb.AppendLine("			this.DefaultTimeout = true;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a new instance of the ContextStartup object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public ContextStartup(string modifier, bool allowLazyLoading, int commandTimeout) :");
            sb.AppendLine("			this(modifier, allowLazyLoading)");
            sb.AppendLine("		{");
            sb.AppendLine("			this.CommandTimeout = commandTimeout;");
            sb.AppendLine("			this.DefaultTimeout = false;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The modifier string used for auditing");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual string Modifier { get; protected internal set; }");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if relationships can be walked via 'Lazy Loading'");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual bool AllowLazyLoading { get; protected internal set; }");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the database timeout value in seconds");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual int CommandTimeout { get; protected internal set; }");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		object ICloneable.Clone()");
            sb.AppendLine("		{");
            sb.AppendLine("			return (ContextStartup)this.MemberwiseClone();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	#endregion");
            sb.AppendLine();

            sb.AppendLine("	#region TenantContextStartup");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// Initialization object used for tenant based data contexts");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public partial class TenantContextStartup : ContextStartup");
            sb.AppendLine("    {");
            sb.AppendLine("        public TenantContextStartup(string modifier, string tenantId)");
            sb.AppendLine("            : base(modifier)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (string.IsNullOrEmpty(tenantId))");
            sb.AppendLine("                throw new Exceptions.ContextConfigurationException(\"The tenant ID must be set!\");");
            sb.AppendLine();
            sb.AppendLine("            this.TenantId = tenantId;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public TenantContextStartup(string modifier, string tenantId, bool allowLazyLoading, int commandTimeout)");
            sb.AppendLine("            : base(modifier, allowLazyLoading, commandTimeout)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (string.IsNullOrEmpty(tenantId))");
            sb.AppendLine("                throw new Exceptions.ContextConfigurationException(\"The tenant ID must be set!\");");
            sb.AppendLine();
            sb.AppendLine("            this.TenantId = tenantId;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public string TenantId { get; }");
            sb.AppendLine("    }");
            sb.AppendLine("	#endregion");
            sb.AppendLine();

            sb.AppendLine("}");
            sb.AppendLine();
            return sb.ToString();
        }

    }
}
