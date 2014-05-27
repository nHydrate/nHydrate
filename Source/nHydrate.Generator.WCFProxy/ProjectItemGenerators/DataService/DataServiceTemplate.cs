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
using Widgetsphere.Generator.ProjectItemGenerators;

namespace Widgetsphere.Generator.IOCExtensions.ProjectItemGenerators.DataService
{
	class DataServiceTemplate : BaseWCFProxyTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();

		#region Constructors
		public DataServiceTemplate(ModelRoot model)
		{
			_model = model;
		}
		#endregion

		#region BaseClassTemplate overrides
		public override string FileContent
		{
			get
			{
				GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get { return "WCFService.Generated.cs"; }
		}

		public string ParentItemName
		{
			get { return "WCFService.cs"; }
		}

		#endregion

		#region GenerateContent

		private void GenerateContent()
		{
			try
			{
				Widgetsphere.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
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
			sb.AppendLine("using " + this.DefaultNamespace + ".DataTransfer;");
			sb.AppendLine();
		}

		public void AppendClass()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// The data service stub that interacts with the WCF service");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public partial class DataService");
			sb.AppendLine("	{");
			sb.AppendLine("		private static DataServiceClient _instance = null;");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The static instance of this class");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static DataServiceClient Instance");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				if (_instance == null)");
			sb.AppendLine("				{");
			sb.AppendLine("					_instance = new DataServiceClient(\"" + this.GetLocalNamespace() + "_IDataService\");");
			sb.AppendLine("				}");
			sb.AppendLine("				return _instance;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// ");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCodeAttribute(\"System.ServiceModel\", \"3.0.0.0\")]");
			sb.AppendLine("	[System.ServiceModel.ServiceContractAttribute(ConfigurationName = \"IDataService\")]");
			sb.AppendLine("	public partial interface IDataService");
			sb.AppendLine("	{");

			foreach (var table in (from x in _model.Database.Tables where x.Generated && !x.AssociativeTable orderby x.Name select x))
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Get all " + table.PascalName + " DTO items");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[System.ServiceModel.OperationContractAttribute(Action = \"http://tempuri.org/IDataService/Get" + table.PascalName + "List\", ReplyAction = \"http://tempuri.org/IDataService/Get" + table.PascalName + "ListResponse\")]");
				sb.AppendLine("		" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "[] Get" + table.PascalName + "List();");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Get a single " + table.PascalName + " DTO item by primary key");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[System.ServiceModel.OperationContractAttribute(Action = \"http://tempuri.org/IDataService/Get" + table.PascalName + "\", ReplyAction = \"http://tempuri.org/IDataService/Get" + table.PascalName + "Response\")]");
				sb.Append("		" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " Get" + table.PascalName + "(");

				var ii = 0;
				foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
				{
					sb.Append(column.GetCodeType() + " " + column.CamelName);
					if (ii < table.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
					ii++;
				}
				sb.AppendLine(");");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Get a paginated list of " + table.PascalName + " DTO items using LINQ");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[System.ServiceModel.OperationContractAttribute(Action = \"http://tempuri.org/IDataService/Get" + table.PascalName + "ListPaged\", ReplyAction = \"http://tempuri.org/IDataService/Get" + table.PascalName + "ListPagedResponse\")]");
				sb.AppendLine("		PagedQueryResults<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "ListPaged(int pageIndex, int pageSize, bool sortOrder, string sortColumn, string linq);");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Get a list of " + table.PascalName + " DTO items using LINQ");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[System.ServiceModel.OperationContractAttribute(Action = \"http://tempuri.org/IDataService/Get" + table.PascalName + "ListByLINQ\", ReplyAction = \"http://tempuri.org/IDataService/Get" + table.PascalName + "ListByLINQResponse\")]");
				sb.AppendLine("		" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "[] Get" + table.PascalName + "ListByLINQ(string linq);");
				sb.AppendLine();

				foreach (var column in table.GetColumnsFullHierarchy().Where(x => x.Generated && x.IsSearchable))
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Get a list of " + table.PascalName + " DTO items by the '" + column.PascalName + "' field");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[System.ServiceModel.OperationContractAttribute(Action = \"http://tempuri.org/IDataService/Get" + table.PascalName + "By" + column.PascalName + "Field\", ReplyAction = \"http://tempuri.org/IDataService/Get" + table.PascalName + "By" + column.PascalName + "FieldResponse\")]");
					sb.AppendLine("		" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "[] Get" + table.PascalName + "By" + column.PascalName + "Field(" + column.GetCodeType() + " " + column.CamelName + ");");
					sb.AppendLine();
				}

				if (!table.Immutable)
				{
					//Persist
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Persist a single " + table.PascalName + " DTO");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[System.ServiceModel.OperationContractAttribute(Action = \"http://tempuri.org/IDataService/Persist" + table.PascalName + "\", ReplyAction = \"http://tempuri.org/IDataService/Persist" + table.PascalName + "Response\")]");
					sb.AppendLine("		" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " Persist" + table.PascalName + "(" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " item);");
					sb.AppendLine();

					//Delete
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Delete a single " + table.PascalName + "");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[System.ServiceModel.OperationContractAttribute(Action = \"http://tempuri.org/IDataService/Delete" + table.PascalName + "\", ReplyAction = \"http://tempuri.org/IDataService/Delete" + table.PascalName + "Response\")]");
					sb.AppendLine("		bool Delete" + table.PascalName + "(" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " item);");
					sb.AppendLine();
				}
			}

			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// ");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCodeAttribute(\"System.ServiceModel\", \"3.0.0.0\")]");
			sb.AppendLine("	public partial interface IDataServiceChannel : " + this.GetLocalNamespace() + ".IDataService, System.ServiceModel.IClientChannel");
			sb.AppendLine("	{");
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// ");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCodeAttribute(\"System.ServiceModel\", \"3.0.0.0\")]");
			sb.AppendLine("	public partial class DataServiceClient : System.ServiceModel.ClientBase<" + this.GetLocalNamespace() + ".IDataService>, " + this.GetLocalNamespace() + ".IDataService");
			sb.AppendLine("	{");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public DataServiceClient()");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public DataServiceClient(string endpointConfigurationName) :");
			sb.AppendLine("			base(endpointConfigurationName)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public DataServiceClient(string endpointConfigurationName, string remoteAddress) :");
			sb.AppendLine("			base(endpointConfigurationName, remoteAddress)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public DataServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :");
			sb.AppendLine("			base(endpointConfigurationName, remoteAddress)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public DataServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :");
			sb.AppendLine("			base(binding, remoteAddress)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();

			foreach (var table in (from x in _model.Database.Tables where x.Generated && !x.AssociativeTable orderby x.Name select x))
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Get a list of all " + table.PascalName + " DTO items");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "[] Get" + table.PascalName + "List()");
				sb.AppendLine("		{");
				sb.AppendLine("			return base.Channel.Get" + table.PascalName + "List();");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Get a " + table.PascalName + " DTO item by primary key");
				sb.AppendLine("		/// </summary>");
				sb.Append("		public " + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " Get" + table.PascalName + "(");

				var ii = 0;
				foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
				{
					sb.Append(column.GetCodeType() + " " + column.CamelName);
					if (ii < table.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
					ii++;
				}
				sb.AppendLine(")");
				sb.AppendLine("		{");
				sb.Append("			return base.Channel.Get" + table.PascalName + "(");
			
				ii = 0;
				foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
				{
					sb.Append(column.CamelName);
					if (ii < table.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
					ii++;
				}
				sb.AppendLine(");");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Get a paginated list of " + table.PascalName + " DTO items");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public PagedQueryResults<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "ListPaged(int pageIndex, int pageSize, bool sortOrder, string sortColumn, string linq)");
				sb.AppendLine("		{");
				sb.AppendLine("			return base.Channel.Get" + table.PascalName + "ListPaged(pageIndex, pageSize, sortOrder, sortColumn, linq);");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Get a list of " + table.PascalName + " DTO items using LINQ");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "[] Get" + table.PascalName + "ListByLINQ(string linq)");
				sb.AppendLine("		{");
				sb.AppendLine("			return base.Channel.Get" + table.PascalName + "ListByLINQ(linq);");
				sb.AppendLine("		}");
				sb.AppendLine();

				foreach (var column in table.GetColumnsFullHierarchy().Where(x => x.Generated && x.IsSearchable))
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Get a list of " + table.PascalName + " DTO items by the " + column.PascalName + " field");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "[] Get" + table.PascalName + "By" + column.PascalName + "Field(" + column.GetCodeType() + " " + column.CamelName + ")");
					sb.AppendLine("		{");
					sb.AppendLine("			return base.Channel.Get" + table.PascalName + "By" + column.PascalName + "Field(" + column.CamelName + ");");
					sb.AppendLine("		}");
					sb.AppendLine();
				}

				if (!table.Immutable)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Persist a single " + table.PascalName + " DTO item to store");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " Persist" + table.PascalName + "(" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " item)");
					sb.AppendLine("		{");
					sb.AppendLine("			return base.Channel.Persist" + table.PascalName + "(item);");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Delete a single " + table.PascalName + " DTO item");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public bool Delete" + table.PascalName + "(" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " item)");
					sb.AppendLine("		{");
					sb.AppendLine("			return base.Channel.Delete" + table.PascalName + "(item);");
					sb.AppendLine("		}");
					sb.AppendLine();
				}

			}

			sb.AppendLine("	}");
			sb.AppendLine();
		}

		#endregion


	}
}