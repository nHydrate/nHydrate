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

namespace nHydrate.Generator.EFCodeFirst.Generators.ContextStartup
{
    public class ContextStartupGeneratedTemplate : EFCodeFirstBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();

        public ContextStartupGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return "ContextStartup.Generated.cs"; }
        }

        public string ParentItemName
        {
            get { return "ContextStartup.cs"; }
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
                sb.AppendLine("	#region ContextStartup");
                sb.AppendLine();
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// This object holds the modifer information for audits on an ObjectContext");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
                sb.AppendLine("	public partial class ContextStartup");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Creates a new instance of the ContextStartup object");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public ContextStartup(string modifier)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.CurrentPlatform = DatabasePlatformConstants.SQLServer;");
                sb.AppendLine("			this.Modifer = modifier;");
                sb.AppendLine("			this.AllowLazyLoading = true;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Creates a new instance of the ContextStartup object");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public ContextStartup(string modifier, bool allowLazyLoading) :");
                sb.AppendLine("			this(modifier)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.AllowLazyLoading = allowLazyLoading;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Creates a new instance of the ContextStartup object");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public ContextStartup(string modifier, bool allowLazyLoading, int commandTimeout) :");
                sb.AppendLine("			this(modifier, allowLazyLoading)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.CommandTimeout = commandTimeout;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Creates a new instance of the ContextStartup object");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public ContextStartup(string modifier, bool allowLazyLoading, int commandTimeout, bool isAdmin) :");
                sb.AppendLine("			this(modifier, allowLazyLoading)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.CommandTimeout = commandTimeout;");
                sb.AppendLine("			this.IsAdmin = isAdmin;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Creates a new instance of the ContextStartup object");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public ContextStartup(string modifier, DatabasePlatformConstants currentPlatform) :");
                sb.AppendLine("			this(modifier)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.CurrentPlatform = currentPlatform;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Creates a new instance of the ContextStartup object");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public ContextStartup(string modifier, bool allowLazyLoading, int commandTimeout, DatabasePlatformConstants currentPlatform) :");
                sb.AppendLine("			this(modifier, allowLazyLoading)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.CurrentPlatform = currentPlatform;");
                sb.AppendLine("			this.CommandTimeout = commandTimeout;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The modifier string used for auditing");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public virtual string Modifer { get; protected internal set; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Determines if relationships can be walked via 'Lazy Loading'");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public virtual bool AllowLazyLoading { get; protected internal set; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Determines the database timeout value in seconds");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public virtual int? CommandTimeout { get; protected internal set; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Determines the connection string is for an admin user that can see all tenant based data");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public virtual bool IsAdmin { get; protected internal set; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Determines the database platform that the context instance will target");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public DatabasePlatformConstants CurrentPlatform { get; private set; }");
                sb.AppendLine();
                sb.AppendLine("	}");
                sb.AppendLine();
                sb.AppendLine("	#endregion");
                sb.AppendLine();
                sb.AppendLine("}");
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
            sb.AppendLine();
        }

        #endregion

    }
}