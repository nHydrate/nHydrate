#region Copyright (c) 2006-2009 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2009 All Rights reserved              *
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
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using System.Collections;

namespace Widgetsphere.Generator.ProjectItemGenerators.DefinedViewSelectCommand
{
	class DefinedViewSelectCommandGeneratedTemplate : BaseClassTemplate
	{

		private StringBuilder sb = new StringBuilder();
		private CustomView _currentView;

		public DefinedViewSelectCommandGeneratedTemplate(ModelRoot model, CustomView currentView)
		{
			_model = model;
			_currentView = currentView;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get
			{
				return string.Format("{0}.Generated.cs", _currentView.PascalName);
			}
		}

		public string ParentItemName
		{
			get
			{
				return string.Format("{0}.cs", _currentView.PascalName);
			}
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

		private void GenerateContent()
		{
			try
			{
				ValidationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.SelectCommands");
				sb.AppendLine("{");
				this.AppendClass();
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}


		#region namespace / objects

		public void AppendUsingStatements()
		{
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Data;");
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using Widgetsphere.Core.Util;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Views;");
			sb.AppendLine("using " + DefaultNamespace + ".Domain.Views;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			this.AppendFullTemplate();
			this.AppendSearchClass();
		}

		private void AppendSearchClass()
		{
			ArrayList validColumns = new ArrayList();
			foreach (Reference reference in _currentView.GeneratedColumns)
			{
				CustomViewColumn dc = (CustomViewColumn)reference.Object;
				if (!(dc.DataType == System.Data.SqlDbType.Binary ||
					dc.DataType == System.Data.SqlDbType.Image ||
					dc.DataType == System.Data.SqlDbType.NText ||
					dc.DataType == System.Data.SqlDbType.Text ||
					dc.DataType == System.Data.SqlDbType.Timestamp ||
					dc.DataType == System.Data.SqlDbType.Udt ||
					dc.DataType == System.Data.SqlDbType.VarBinary ||
					dc.DataType == System.Data.SqlDbType.Variant ||
					dc.DataType == System.Data.SqlDbType.Money))
				{
					validColumns.Add(dc);
				}
			}

			if (validColumns.Count != 0)
			{
				sb.AppendLine("	#region " + _currentView.PascalName + "SelectBySearch");
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The select command used to select all rows from the '" + _currentView.PascalName + "' table");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	public partial class " + _currentView.PascalName + "SelectBySearch : SelectCommand, ISerializable");
				sb.AppendLine("	{");
				sb.AppendLine("		private " + _currentView.PascalName + "Search " + _currentView.PascalName + ";");
				sb.AppendLine();
				sb.AppendLine("		#region Serialization");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Serialization constructor");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected " + _currentView.PascalName + "SelectBySearch(SerializationInfo info, StreamingContext context)");
				sb.AppendLine("			: base(info, context)");
				sb.AppendLine("		{");
				sb.AppendLine("			" + _currentView.PascalName + " = (" + _currentView.PascalName + "Search)info.GetValue(\"" + _currentView.PascalName + "\", typeof(" + _currentView.PascalName + "Search));");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Method used internally for serialization");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
				sb.AppendLine("		{");
				sb.AppendLine("			base.GetObjectData(info, context);");
				sb.AppendLine("			info.AddValue(\"" + _currentView.PascalName + "\", " + _currentView.PascalName + ");");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion ");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by " + _currentView.PascalName + " search object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentView.PascalName + "SelectBySearch(" + _currentView.PascalName + "Search searchObject)");
				sb.AppendLine("		{");
				sb.AppendLine("			" + _currentView.PascalName + " = searchObject;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendLine("	");
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Creates a persistable domainCollection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentView.PascalName + "Collection colDomain = new Domain" + _currentView.PascalName + "Collection();");
				sb.AppendLine("			return colDomain;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The stored procedure name to select values by primary key to populate a '" + _currentView.PascalName + "' collection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override string StoredProcedureName");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				if (" + _currentView.PascalName + ".SearchType == SearchType.OR)");
				sb.AppendLine("					return \"gen_" + Globals.GetPascalName(_model, _currentView) + "SelectBySearchOr\";");
				sb.AppendLine("				else");
				sb.AppendLine("					return \"gen_" + Globals.GetPascalName(_model, _currentView) + "SelectBySearchAnd\";");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Initializes the parameters for this select command");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (" + _currentView.PascalName + " != null)");
				sb.AppendLine("			{");
				foreach (CustomViewColumn dc in validColumns)
				{
					sb.AppendLine("				if (" + _currentView.PascalName + "." + dc.PascalName + " != null)");
					sb.AppendLine("				{");
					sb.AppendLine("					ParameterValues.Add(\"@" + dc.DatabaseName + "\", " + _currentView.PascalName + "." + dc.PascalName + ");");
					sb.AppendLine("				}");
				}
				sb.AppendLine("				ParameterValues.Add(\"@max_row_count\", " + _currentView.PascalName + ".MaxRowCount);");
				sb.AppendLine();
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#region SelectCommand Abstact Override");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The connection string used to connect to the database");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override string ConnectionString");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return ConfigurationValues.GetInstance().ConnectionString; }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Determines the default time out");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override int DefaultTimeOut");
				sb.AppendLine("		{");
				sb.AppendLine("			get {	return ConfigurationValues.GetInstance().DefaultTimeOut; }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");				
				sb.AppendLine();
				sb.AppendLine("	}");
				sb.AppendLine();
				sb.AppendLine("	#endregion");
				sb.AppendLine();
			}
		}

		#endregion

		#region append regions

		private void AppendFullTemplate()
		{
			sb.AppendLine("	#region " + _currentView.PascalName + "PagedSelect");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// Select command used to perform paged selects from the '" + _currentView.PascalName + "' table");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentView.PascalName + "PagedSelect : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine("		private int mPage; ");
			sb.AppendLine("		private int mPageSize;");
			sb.AppendLine("		private string mOrderByColumn;");
			sb.AppendLine("		private bool mAscending;");
			sb.AppendLine("		private string mFilter;");
			sb.AppendLine("		private int mCount = -1;");
			sb.AppendLine();
			sb.AppendLine("		#region Serialization");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected " + _currentView.PascalName + "PagedSelect(SerializationInfo info, StreamingContext context): base(info,context)");
			sb.AppendLine("		{");
			sb.AppendLine("			mPage = info.GetInt32(\"mPage\");");
			sb.AppendLine("			mPageSize = info.GetInt32(\"mPageSize\");");
			sb.AppendLine("			mOrderByColumn = info.GetString(\"mOrderByColumn\");");
			sb.AppendLine("			mAscending = info.GetBoolean(\"mAscending\");");
			sb.AppendLine("			mFilter = info.GetString(\"mFilter\");");
			sb.AppendLine("			mCount = info.GetInt32(\"mCount\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Method used internally for serialization");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
			sb.AppendLine("		{");
			sb.AppendLine("			base.GetObjectData(info, context);");
			sb.AppendLine("			info.AddValue(\"mPage\", mPage);");
			sb.AppendLine("			info.AddValue(\"mPageSize\", mPageSize);");
			sb.AppendLine("			info.AddValue(\"mOrderByColumn\", mOrderByColumn);");
			sb.AppendLine("			info.AddValue(\"mAscending\", mAscending);");
			sb.AppendLine("			info.AddValue(\"mFilter\", mFilter);");
			sb.AppendLine("			info.AddValue(\"mCount\", mCount);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion ");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Paging select command that allows for the selective page-based selection of database rows");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentView.PascalName + "PagedSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter)");
			sb.AppendLine("		{");
			sb.AppendLine("			mPage = page; ");
			sb.AppendLine("			mPageSize = pageSize;");
			sb.AppendLine("			mOrderByColumn = orderByColumn;");
			sb.AppendLine("			mAscending = ascending;");
			sb.AppendLine("			mFilter = filter;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The total number of rows returned. This is populated after the database is queried.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int Count");
			sb.AppendLine("		{");
			sb.AppendLine("		  get { return mCount; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a persistable domainCollection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentView.PascalName + "Collection colDomain = new Domain" + _currentView.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentView.PascalName + "' collection in a paged fashion");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentView) + "PagingSelect\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Initializes the parameters for this select command");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Execute this select command on the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection Execute(SubDomainBase subDomain)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.SetupParameterValues(subDomain);");
			sb.AppendLine("			DomainCollectionBase returnTable = (DomainCollectionBase)this.CreateDomainCollection();");
			sb.AppendLine();
			sb.AppendLine("			IDbCommand fillCommand = DomainCollectionBase.GetFillCommand(this.ConnectionString, this.StoredProcedureName);");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@page\", mPage);");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@pageSize\", mPageSize);");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@orderByColumn\", mOrderByColumn);");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@ascending\", mAscending);");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@filter\", mFilter);");
			//sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@junk\", Guid.NewGuid().ToString());" );
			sb.AppendLine("			IDbDataParameter paramCount = PersistableDomainCollectionBase.GetOutputParameter(fillCommand, \"@count\");");
			sb.AppendLine("			returnTable.FillDataTable(this.ConnectionString, fillCommand);");
			sb.AppendLine("			if(paramCount.Value != System.DBNull.Value)");
			sb.AppendLine("			{");
			sb.AppendLine("				mCount = (int)paramCount.Value;");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				mCount = 0;");
			sb.AppendLine("			}");
			sb.AppendLine("			return returnTable;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#region SelectCommand Abstact Override");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The connection string used to connect to the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string ConnectionString");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return ConfigurationValues.GetInstance().ConnectionString; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the default time out");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override int DefaultTimeOut");
			sb.AppendLine("		{");
			sb.AppendLine("			get {	return ConfigurationValues.GetInstance().DefaultTimeOut; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();

			sb.AppendLine("	#region " + _currentView.PascalName + "Select");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The select command used to select all rows from the '" + _currentView.PascalName + "' view");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentView.PascalName + "Select : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine();

			sb.AppendLine("		//Define custom parameters");
			foreach (Reference reference in _currentView.Parameters)
			{
				Parameter parameter = (Parameter)reference.Object;
				sb.AppendLine("		" + parameter.GetCodeType() + " m" + parameter.PascalName + ";");
			}

			sb.AppendLine();
			sb.AppendLine("		#region Serialization");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected " + _currentView.PascalName + "Select(SerializationInfo info, StreamingContext context): base(info,context)");
			sb.AppendLine("		{");

			//Serialize the parameters
			foreach (Reference reference in _currentView.Parameters)
			{
				Parameter parameter = (Parameter)reference.Object;
				sb.AppendLine("			m" + parameter.PascalName + " = (" + parameter.GetCodeType() + ")info.GetValue(\"m" + parameter.PascalName + "\", typeof(" + parameter.GetCodeType() + "));");
			}

			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Method used internally for serialization");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
			sb.AppendLine("		{");
			sb.AppendLine("			base.GetObjectData(info, context);");

			foreach (Reference reference in _currentView.Parameters)
			{
				Parameter parameter = (Parameter)reference.Object;
				sb.AppendLine("			info.AddValue(\"m" + parameter.PascalName + "\", m" + parameter.PascalName + ");");
			}

			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion ");
			sb.AppendLine();

			//Default constructor
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Default constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentView.PascalName + "Select()");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();

			//Overloaded constructor with parameters
			if (_currentView.Parameters.Count > 0)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Default constructor");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentView.PascalName + "Select(" + this.GetParameterList(true) + ")");
				sb.AppendLine("		{");

				//Serialize the parameters
				foreach (Reference reference in _currentView.Parameters)
				{
					Parameter parameter = (Parameter)reference.Object;
					if (!parameter.IsOutputParameter)
						sb.AppendLine("			m" + parameter.PascalName + " = " + parameter.CamelName + ";");
				}

				sb.AppendLine("		}");
				sb.AppendLine();
			}

			//Other
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a persistable domainCollection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentView.PascalName + "Collection colDomain = new Domain" + _currentView.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentView.PascalName + "' collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentView) + "Select\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine();
			this.AppendRegionAbstactOverrides();
			this.AppendRegionProperties();
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();



			sb.AppendLine("	#region " + _currentView.PascalName + "SelectAll");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The select command used to select all rows from the '" + _currentView.PascalName + "' table");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentView.PascalName + "SelectAll : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine();
			sb.AppendLine("		#region Serialization");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected " + _currentView.PascalName + "SelectAll(SerializationInfo info, StreamingContext context): base(info,context)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Method used internally for serialization");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
			sb.AppendLine("		{");
			sb.AppendLine("			base.GetObjectData(info, context);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion ");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Default constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentView.PascalName + "SelectAll()");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a persistable domainCollection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentView.PascalName + "Collection colDomain = new Domain" + _currentView.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentView.PascalName + "' collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentView) + "Select\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Initializes the parameters for this select command");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#region SelectCommand Abstact Override");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The connection string used to connect to the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string ConnectionString");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return ConfigurationValues.GetInstance().ConnectionString; }"); 
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the default time out");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override int DefaultTimeOut");
			sb.AppendLine("		{");
			sb.AppendLine("			get {	return ConfigurationValues.GetInstance().DefaultTimeOut; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();

		}

		private void AppendRegionAbstactOverrides()
		{
			sb.AppendLine("		#region SelectCommand Abstact Override");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Execute this select command on the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection Execute(SubDomainBase subDomain)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.SetupParameterValues(subDomain);");
			sb.AppendLine("			ReadOnlyDomainCollection returnTable = (ReadOnlyDomainCollection)this.CreateDomainCollection();");
			sb.AppendLine();
			sb.AppendLine("			IDbCommand fillCommand = ReadOnlyDomainCollection.GetFillCommand(this.ConnectionString, this.StoredProcedureName);");
			sb.AppendLine("			fillCommand.CommandTimeout = this.DefaultTimeOut;");

			//Setup all paremeters for the stored procedure
			foreach (Reference reference in _currentView.Parameters)
			{
				Parameter parameter = (Parameter)reference.Object;
				if (parameter.IsOutputParameter)
					sb.AppendLine("			IDbDataParameter param" + parameter.PascalName + " = ReadOnlyDomainCollection.GetOutputParameter(fillCommand, \"@" + parameter.Name + "\");");
				else
					sb.AppendLine("			ReadOnlyDomainCollection.SetParameterValue(fillCommand, \"@" + parameter.Name + "\", m" + parameter.PascalName + ");");
			}

			//Exeecute the query
			sb.AppendLine("			returnTable.FillDataTable(this.ConnectionString, fillCommand);");

			//Load all of the out parameters
			foreach (Reference reference in _currentView.Parameters)
			{
				Parameter parameter = (Parameter)reference.Object;
				if (parameter.IsOutputParameter)
				{
					sb.AppendLine("			if(param" + parameter.PascalName + ".Value != System.DBNull.Value)");
					sb.AppendLine("			{");
					sb.AppendLine("				m" + parameter.PascalName + " = (" + parameter.GetCodeType() + ")param" + parameter.PascalName + ".Value;");
					sb.AppendLine("			}");
					sb.AppendLine("			else");
					sb.AppendLine("			{");
					sb.AppendLine("				m" + parameter.PascalName + " = " + parameter.Default + ";");
					sb.AppendLine("			}");
				}
			}

			sb.AppendLine("			return returnTable;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Initializes the parameters for this select command");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The connection string used to connect to the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string ConnectionString");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return ConfigurationValues.GetInstance().ConnectionString; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the default time out");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override int DefaultTimeOut");
			sb.AppendLine("		{");
			sb.AppendLine("			get {	return ConfigurationValues.GetInstance().DefaultTimeOut; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

		#region append member variables

		public void AppendMemberVariables()
		{
			sb.AppendLine("		private Domain" + _currentView.PascalName + "Collection col" + _currentView.PascalName + "List;");
		}

		#endregion

		#region append constructors

		public void AppendConstructor()
		{
			sb.AppendLine("		internal " + _currentView.PascalName + "CollectionViews(Domain" + _currentView.PascalName + "Collection in" + _currentView.PascalName + "List)");
			sb.AppendLine("		{");
			sb.AppendLine("			col" + _currentView.PascalName + "List = in" + _currentView.PascalName + "List;");
			sb.AppendLine("			Initialize();");
			sb.AppendLine("		}");
		}

		#endregion

		#region append properties

		/// <summary>
		/// Append the public properties for output parameters
		/// </summary>
		private void AppendRegionProperties()
		{
			sb.AppendLine("		#region Property Implementations");
			sb.AppendLine();

			foreach (Reference reference in _currentView.Parameters)
			{
				Parameter parameter = (Parameter)reference.Object;
				if (parameter.IsOutputParameter)
				{
					sb.AppendLine("		public " + parameter.GetCodeType() + " " + parameter.PascalName);
					sb.AppendLine("		{");
					sb.AppendLine("		  get { return m" + parameter.PascalName + "; }");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
			}

			sb.AppendLine("		#endregion");
			sb.AppendLine();

		}

		#endregion

		#region append methods
		public void AppendInitializeMethod()
		{
			sb.AppendLine("		private void Initialize()");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
		}
		#endregion

		#region append operator overloads
		#endregion

		#region string helpers

		private string GetParameterList(bool useDataTypes)
		{
			try
			{
				StringBuilder retval = new StringBuilder();
				ArrayList al = new ArrayList();
				foreach (Reference reference in _currentView.Parameters)
				{
					Parameter parameter = (Parameter)reference.Object;
					if (!parameter.IsOutputParameter)
						al.Add(parameter);
				}

				int ii = 0;
				foreach (Parameter parameter in al)
				{
					if (useDataTypes)
					{
						retval.Append(parameter.GetCodeType() + " ");
					}
					retval.Append(parameter.CamelName);
					if (ii < al.Count - 1)
						retval.Append(", ");
					ii++;
				}

				return retval.ToString();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

	}
}