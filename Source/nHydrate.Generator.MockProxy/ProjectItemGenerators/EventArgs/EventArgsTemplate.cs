#region Copyright (c) 2006-2011 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2011 All Rights reserved              *
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
using System.Text;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.MockProxy.ProjectItemGenerators.EventArgs
{
	class EventArgsTemplate : BaseMockProxyTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();
		private readonly Table _currentTable = null;

		#region Constructors
		public EventArgsTemplate(ModelRoot model, Table table)
		{
			_model = model;
			_currentTable = table;
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
			get { return string.Format("{0}EventArgs.Generated.cs", _currentTable.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}EventArgs.cs", _currentTable.PascalName); }
		}

		#endregion

		#region GenerateContent

		private void GenerateContent()
		{
			try
			{
				Widgetsphere.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + this.GetLocalNamespace() + ".EventArgs");
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
			sb.AppendLine();
		}

		public void AppendClass()
		{
			sb.AppendLine("	#region " + _currentTable.PascalName + " EventArgs");
			sb.AppendLine();
			this.AppendMethodPrimaryKeyEventArgs();
			this.AppendMethodPersistEventArgs();
			this.AppendMethodDeleteEventArgs();
			this.AppendMethodRunSelectEventArgs();
			this.AppendMethodDTOEventArgs();
			sb.AppendLine("	#endregion");
			sb.AppendLine();

		}

		private void AppendMethodRunSelectEventArgs()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// An event arguments class for object selection");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "RunSelectEventArgs : System.EventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The default constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "RunSelectEventArgs()");
			sb.AppendLine("		{");
			sb.AppendLine("			this.ReturnValue = new List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + ">();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The returned list of DTO items");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> ReturnValue { get; set; }");
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// An event arguments class for object selection with LINQ");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "RunSelectLINQEventArgs : " + _currentTable.PascalName + "RunSelectEventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The default constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "RunSelectLINQEventArgs(string linq)");
			sb.AppendLine("			: base()");
			sb.AppendLine("		{");
			sb.AppendLine("			this.LINQ = linq;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The LINQ statement to use for filtering");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string LINQ { get; set; }");
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// An event arguments class for paginated object selection");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "RunSelectPagedEventArgs : System.EventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The default constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "RunSelectPagedEventArgs(int pageIndex, int pageSize, bool ascending, string sortColumn, string linq)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.ReturnValue = new " + this.DefaultNamespace + ".DataTransfer.PagedQueryResults<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + ">();");
			sb.AppendLine("			this.PageIndex = pageIndex;");
			sb.AppendLine("			this.PageSize = pageSize;");
			sb.AppendLine("			this.Ascending = ascending;");
			sb.AppendLine("			this.SortColumn = sortColumn;");
			sb.AppendLine("			this.LINQ = linq;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The returned list of DTO items");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer.PagedQueryResults<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> ReturnValue { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The LINQ statement to use for filtering");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string LINQ { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The page index to load");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int PageIndex { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The number of records per page");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int PageSize { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines if the sort direction");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public bool Ascending { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the column on which to sort");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string SortColumn { get; set; }");
			sb.AppendLine("	}");
			sb.AppendLine();
		}

		private void AppendMethodPersistEventArgs()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// An event arguments class for object persistence");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "PersistEventArgs : System.EventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for single DTO events");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "PersistEventArgs(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " dto)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.DTOList = new List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + ">();");
			sb.AppendLine("			this.DTOList.Add(dto);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for multi DTO events");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "PersistEventArgs(List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> list)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.DTOList = list;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The list of DTO items to persist");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> DTOList { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The return value for success");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public bool ReturnValue { get; set; }");
			sb.AppendLine("	}");
			sb.AppendLine();
		}

		private void AppendMethodDeleteEventArgs()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// An event arguments class for object deletion");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "DeleteEventArgs : " + _currentTable.PascalName + "PersistEventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for single DTO events");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "DeleteEventArgs(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " dto) : base(dto)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for multi DTO events");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "DeleteEventArgs(List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> list) : base (list)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();			
			sb.AppendLine("	}");
			sb.AppendLine();
		}

		private void AppendMethodPrimaryKeyEventArgs()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// An event arguments class for object selection by primary key");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "PrimaryKeyEventArgs : System.EventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The default constructor");
			sb.AppendLine("		/// </summary>");
			sb.Append("		public " + _currentTable.PascalName + "PrimaryKeyEventArgs(");

			var ii = 0;
			foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.Append(column.GetCodeType() + " " + column.CamelName);
				if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
				ii++;
			}
			sb.AppendLine(")");
			sb.AppendLine("		{");
			sb.AppendLine("			this.ReturnValue = new " + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The returned DTO item");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " ReturnValue { get; set; }");
			sb.AppendLine("	}");
			sb.AppendLine();
		}

		private void AppendMethodDTOEventArgs()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// An event arguments class for DTO specific events");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "DTOEventArgs : System.EventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The default constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "DTOEventArgs(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " item)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.DTO = item;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The passed in DTO item");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " DTO { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The returned DTO item");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer.IDTO ReturnDTOItem { get; set; }");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The returned DTO list");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer.IDTO> ReturnDTOList { get; set; }");

			sb.AppendLine("	}");
			sb.AppendLine();
		}

		#endregion


	}
}