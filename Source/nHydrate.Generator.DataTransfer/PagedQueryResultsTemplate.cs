#region Copyright (c) 2006-2012 nHydrate.org, All Rights Reserved
//--------------------------------------------------------------------- *
//                          NHYDRATE.ORG                                *
//             Copyright (c) 2006-2012 All Rights reserved              *
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
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF THE NHYDRATE GROUP *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS THIS PRODUCT                 *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF NHYDRATE,          *
//THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO             *
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
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.DataTransfer
{
	class PagedQueryResultsTemplate : BaseDataTransferTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();

		#region Constructors
		public PagedQueryResultsTemplate(ModelRoot model)
			: base(model)
		{
		}
		#endregion

		#region BaseClassTemplate overrides
		public override string FileContent
		{
			get
			{
				this.GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get { return "PagedQueryResults.Generated.cs"; }
		}

		public string ParentItemName
		{
			get { return "PagedQueryResults.cs"; }
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
				this.AppendClass();
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region namespace / objects

		public void AppendUsingStatements()
		{
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Text;");
		}

		public void AppendClass()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// This is the return type of all DTO paged select queries.");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	/// <typeparam name=\"T\">A DTO type</typeparam>");
			sb.AppendLine("	public partial class PagedQueryResults<T> where T : nHydrate.EFCore.DataAccess.IDTO");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The current page to load");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int CurrentPage { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The returned total number of pages for the query");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int TotalPages { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The returned total number of items for the query");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int TotalRecords { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The actual returned data for the query");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public List<T> GridData { get; set; }");
			sb.AppendLine("	}");
		}

		#endregion

	}
}