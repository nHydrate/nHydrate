using nHydrate.Generator.Models;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextStartup
{
    public class ContextStartupGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();

        public ContextStartupGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string FileName => "ContextStartup.Generated.cs";
        public string ParentItemName => "ContextStartup.cs";

        public override string FileContent
        {
            get
            {
                this.GenerateContent();
                return sb.ToString();
            }
        }

        private void GenerateContent()
        {
            nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
            this.AppendUsingStatements();
            sb.AppendLine("namespace " + this.GetLocalNamespace());
            sb.AppendLine("{");
            sb.AppendLine("	#region ContextStartup");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// This object holds the modifier information for audits on an ObjectContext");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");
            sb.AppendLine("	public partial class ContextStartup : ICloneable");
            sb.AppendLine("	{");
            sb.AppendLine("		protected internal string DebugInfo { get; set; }");
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
            sb.AppendLine("		public object Clone()");
            sb.AppendLine("		{");
            sb.AppendLine("			return (ContextStartup)this.MemberwiseClone();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	#endregion");
            sb.AppendLine();
            sb.AppendLine("}");
            sb.AppendLine();
        }

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine();
        }

    }
}