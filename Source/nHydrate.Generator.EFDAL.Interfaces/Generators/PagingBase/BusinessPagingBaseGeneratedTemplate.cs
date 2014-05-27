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
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;
using System.Collections;

namespace Widgetsphere.Generator.EFDAL.Generators.PagingBase
{
	class PagingBaseGeneratedTemplate : EFDALBaseTemplate
	{
		private StringBuilder sb = new StringBuilder();

		public PagingBaseGeneratedTemplate(ModelRoot model)
		{
			_model = model;
		}

		#region BaseClassTemplate overrides

		public override string FileName
		{
			get { return "PagingBase.Generated.cs"; }
		}

		public string ParentItemName
		{
			get { return "PagingBase.cs"; }
		}

		public override string FileContent
		{
			get
			{
				this.GenerateContent();
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
				sb.AppendLine("namespace " + DefaultNamespace);
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
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using " + this.DefaultNamespace + ";");
			sb.AppendLine("using Widgetsphere.EFCore.DataAccess;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// This is the base class for all custom paging objects");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class PagingBase<T> : Widgetsphere.EFCore.DataAccess.IPagingObject");
			sb.AppendLine("		where T : Widgetsphere.EFCore.DataAccess.IPagingFieldItem");
			sb.AppendLine("	{");
			sb.AppendLine("		#region Class Members");
			sb.AppendLine();
			sb.AppendLine("		private int _pageIndex = 1;");
			sb.AppendLine("		private int _recordsperPage = 10;");
			sb.AppendLine("		private List<T> _orderByList = new List<T>();");
			sb.AppendLine("		private int _recordCount = 0;");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region Constructors");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a paging object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public PagingBase()");
			sb.AppendLine("			: this(1, 10, default(T))");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a paging object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"pageIndex\">The page number to load</param>");
			sb.AppendLine("		/// <param name=\"recordsperPage\">The number of records per page.</param>");
			sb.AppendLine("		public PagingBase(int pageIndex, int recordsperPage)");
			sb.AppendLine("			: this(pageIndex, recordsperPage, default(T))");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a paging object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"pageIndex\">The page number to load</param>");
			sb.AppendLine("		/// <param name=\"recordsperPage\">The number of items per page.</param>");
			sb.AppendLine("		/// <param name=\"orderByField\">The field on which to sort.</param>");
			sb.AppendLine("		public PagingBase(int pageIndex, int recordsperPage, T orderByField)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (pageIndex < 1) throw new Exception(\"The PageIndex must be 1 or greater.\");");
			sb.AppendLine("			if (recordsperPage < 1) throw new Exception(\"The RecordsPerPage must be 1 or greater.\");");
			sb.AppendLine();
			sb.AppendLine("			_pageIndex = pageIndex;");
			sb.AppendLine("			_recordsperPage = recordsperPage;");
			sb.AppendLine("			if (orderByField != null)");
			sb.AppendLine("				_orderByList.Add(orderByField);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region Property Implementations");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The page number of load.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int PageIndex");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return _pageIndex; }");
			sb.AppendLine("			set");
			sb.AppendLine("			{");
			sb.AppendLine("				if (value < 1) throw new Exception(\"The PageIndex must be 1 or greater.\");");
			sb.AppendLine("				_pageIndex = value;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The number of items per page.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int RecordsperPage");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return _recordsperPage; }");
			sb.AppendLine("			set");
			sb.AppendLine("			{");
			sb.AppendLine("				if (value < 1) throw new Exception(\"The RecordsperPage must be 1 or greater.\");");
			sb.AppendLine("				_recordsperPage = value;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// A list of fields on which to sort.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public List<T> OrderByList");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return _orderByList; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The total number of non-paged items returned for the search.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int RecordCount");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return _recordCount; }");
			sb.AppendLine("			set { _recordCount = value; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region IPagingObject Members");
			sb.AppendLine();
			sb.AppendLine("		IEnumerable<IPagingFieldItem> IPagingObject.GetOrderByList()");
			sb.AppendLine("		{");
			sb.AppendLine("			List<IPagingFieldItem> retval = new List<IPagingFieldItem>();");
			sb.AppendLine("			foreach (IPagingFieldItem item in this.OrderByList)");
			sb.AppendLine("			{");
			sb.AppendLine("				retval.Add(item);");
			sb.AppendLine("			}");
			sb.AppendLine("			return retval.AsEnumerable();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("	}");
		}

		#endregion

	}

}