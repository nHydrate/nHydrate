#region Copyright (c) 2006-2010 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2010 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion
using System;
using System.Linq;
using Widgetsphere.Generator.Common.GeneratorFramework;
using Widgetsphere.Generator.Models;
using System.Text;
using Widgetsphere.Generator.Common.Util;
using System.Collections.Generic;

namespace Widgetsphere.Generator.EFDAL.Generators.NHEntityObject
{
	public class NHEntityObjectGeneratedTemplate : EFDALBaseTemplate
	{
		private StringBuilder sb = new StringBuilder();

		public NHEntityObjectGeneratedTemplate(ModelRoot model)
		{
			_model = model;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return "NHEntityObject.Generated.cs"; }
		}

		public string ParentItemName
		{
			get { return "NHEntityObject.cs"; }
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
				ValidationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + this.DefaultNamespace);
				sb.AppendLine("{");
				sb.AppendLine("	#region NHEntityObject Class");
				sb.AppendLine();
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// The base class for all entity objects");
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	public abstract partial class NHEntityObject : EntityObject");
				sb.AppendLine("	{");
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Get the validation rule violations");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <returns></returns>");
				sb.AppendLine("		public virtual IEnumerable<IRuleViolation> GetRuleViolations()");
				sb.AppendLine("		{");
				sb.AppendLine("			List<IRuleViolation> retval = new List<IRuleViolation>();");
				sb.AppendLine("			return retval;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Determines if all of the validation rules have been met");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <returns></returns>");
				sb.AppendLine("		public virtual bool IsValid()");
				sb.AppendLine("		{");
				sb.AppendLine("			return (GetRuleViolations().Count() == 0);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Test another entity object for equivalence against the current instance");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public abstract bool IsEquivalent(NHEntityObject item);");
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