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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using System.Collections;
using Widgetsphere.Generator.Common.Util;

namespace Widgetsphere.Generator.ProjectItemGenerators.SelectCommand
{
	class SelectCommandGeneratedTemplate : BaseClassTemplate
	{
		#region Class Members

		private StringBuilder sb = new StringBuilder();
		private Table _currentTable;

		#endregion

		#region Constructor

		public SelectCommandGeneratedTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}

		#endregion

		#region BaseClassTemplate overrides

		public override string FileName
		{
			get { return string.Format("{0}.Generated.cs", _currentTable.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}.cs", _currentTable.PascalName); }
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
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.SelectCommands");
				sb.AppendLine("{");
				this.AppendClass();

				if (!_model.Database.AllowZeroTouch)
				{
					this.AppendSearchClass();
				}

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
			sb.AppendLine("using System.Data;");
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using Widgetsphere.Core.Util;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects;");
			sb.AppendLine("using " + DefaultNamespace + ".Domain.Objects;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			this.AppendFullTemplate();
		}

		#endregion

		#region append regions

		#region AppendFullTemplate

		private void AppendFullTemplate()
		{
			try
			{
				if (!_model.Database.AllowZeroTouch)
				{
					this.AppendRegionSearchable();
					this.AppendRegionSelectAll();
					this.AppendRegionSelectByCustomRules();
				}

				this.AppendRegionPagedSelect();
				this.AppendRegionSelectByPks();
				this.AppendRegionSelectByForeignKeys();
				this.AppendRegionSelectByChildTables();
				this.AppendRegionSelectByDates();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region AppendRegionSelectByDates

		private void AppendRegionSelectByDates()
		{
			ArrayList validColumns = new ArrayList();
			foreach (Reference reference in _currentTable.GeneratedColumns)
			{
				Column dc = (Column)reference.Object;
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

			if (_currentTable.Generated && _currentTable.AllowCreateAudit)
			{
				sb.AppendLine("	#region " + _currentTable.PascalName + "SelectByCreatedDateRange");
				sb.AppendLine();
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// Select objects by their created date.");
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	public partial class " + _currentTable.PascalName + "SelectByCreatedDateRange : SelectCommand, ISerializable");
				sb.AppendLine("	{");
				sb.AppendLine("		private DateTime? mStartDate;");
				sb.AppendLine("		private DateTime? mEndDate;");
				sb.AppendLine();
				sb.AppendLine("		#region Serialization");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by their created date.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected " + _currentTable.PascalName + "SelectByCreatedDateRange(SerializationInfo info, StreamingContext context): base(info,context)");
				sb.AppendLine("		{");
				sb.AppendLine("			mStartDate = info." + Globals.GetSerializationMethod(System.Data.SqlDbType.DateTime) + "(\"mStartDate\");");
				sb.AppendLine("			mEndDate = info." + Globals.GetSerializationMethod(System.Data.SqlDbType.DateTime) + "(\"mEndDate\");");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Method used internally for serialization");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
				sb.AppendLine("		{");
				sb.AppendLine("			base.GetObjectData(info, context);");
				sb.AppendLine("			info.AddValue(\"mStartDate\", mStartDate);");
				sb.AppendLine("			info.AddValue(\"mEndDate\", mEndDate);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion ");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by their created date.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentTable.PascalName + "SelectByCreatedDateRange(DateTime? startDate, DateTime? endDate)");
				sb.AppendLine("		{");
				sb.AppendLine("			mStartDate = startDate;");
				sb.AppendLine("			mEndDate = endDate;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Creates a persistable domainCollection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
				sb.AppendLine("			return colDomain;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentTable.PascalName + "' collection in a paged fashion");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override string StoredProcedureName");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectByCreatedDateRange\"; }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Initializes the parameters for this select command");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
				sb.AppendLine("		{");
				sb.AppendLine("			ParameterValues.Add(\"@start_date\", mStartDate);");
				sb.AppendLine("			ParameterValues.Add(\"@end_date\", mEndDate);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Execute this select command on the database");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override IDomainCollection Execute(SubDomainBase subDomain)");
				sb.AppendLine("		{");
				sb.AppendLine("			this.SetupParameterValues(subDomain);");
				sb.AppendLine("			PersistableDomainCollectionBase returnTable = (PersistableDomainCollectionBase)this.CreateDomainCollection();");
				sb.AppendLine();
				sb.AppendLine("			IDbCommand fillCommand = PersistableDomainCollectionBase.GetFillCommand(this.ConnectionString, this.StoredProcedureName);");
				sb.AppendLine("			fillCommand.CommandTimeout = this.DefaultTimeOut;");
				sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@start_date\", mStartDate);");
				sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@end_date\", mEndDate);");
				sb.AppendLine("			returnTable.FillDataTable(this.ConnectionString, fillCommand);");
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
			}

			if (_currentTable.Generated && _currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("	#region " + _currentTable.PascalName + "SelectByModifiedDateRange");
				sb.AppendLine();
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// Select objects by their modified date.");
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	public partial class " + _currentTable.PascalName + "SelectByModifiedDateRange : SelectCommand, ISerializable");
				sb.AppendLine("	{");
				sb.AppendLine("		private DateTime? mStartDate;");
				sb.AppendLine("		private DateTime? mEndDate;");
				sb.AppendLine();
				sb.AppendLine("		#region Serialization");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by their modified date.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected " + _currentTable.PascalName + "SelectByModifiedDateRange(SerializationInfo info, StreamingContext context): base(info,context)");
				sb.AppendLine("		{");
				sb.AppendLine("			mStartDate = info." + Globals.GetSerializationMethod(System.Data.SqlDbType.DateTime) + "(\"mStartDate\");");
				sb.AppendLine("			mEndDate = info." + Globals.GetSerializationMethod(System.Data.SqlDbType.DateTime) + "(\"mEndDate\");");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Method used internally for serialization");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
				sb.AppendLine("		{");
				sb.AppendLine("			base.GetObjectData(info, context);");
				sb.AppendLine("			info.AddValue(\"mStartDate\", mStartDate);");
				sb.AppendLine("			info.AddValue(\"mEndDate\", mEndDate);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion ");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by their modified date.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentTable.PascalName + "SelectByModifiedDateRange(DateTime? startDate, DateTime? endDate)");
				sb.AppendLine("		{");
				sb.AppendLine("			mStartDate = startDate;");
				sb.AppendLine("			mEndDate = endDate;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Creates a persistable domainCollection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
				sb.AppendLine("			return colDomain;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentTable.PascalName + "' collection in a paged fashion");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override string StoredProcedureName");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectByModifiedDateRange\"; }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Initializes the parameters for this select command");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
				sb.AppendLine("		{");
				sb.AppendLine("			ParameterValues.Add(\"@start_date\", mStartDate);");
				sb.AppendLine("			ParameterValues.Add(\"@end_date\", mEndDate);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Execute this select command on the database");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override IDomainCollection Execute(SubDomainBase subDomain)");
				sb.AppendLine("		{");
				sb.AppendLine("			this.SetupParameterValues(subDomain);");
				sb.AppendLine("			PersistableDomainCollectionBase returnTable = (PersistableDomainCollectionBase)this.CreateDomainCollection();");
				sb.AppendLine();
				sb.AppendLine("			IDbCommand fillCommand = PersistableDomainCollectionBase.GetFillCommand(this.ConnectionString, this.StoredProcedureName);");
				sb.AppendLine("			fillCommand.CommandTimeout = this.DefaultTimeOut;");
				sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@start_date\", mStartDate);");
				sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@end_date\", mEndDate);");
				sb.AppendLine("			returnTable.FillDataTable(this.ConnectionString, fillCommand);");
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
			}

		}

		#endregion

		#region AppendRegionSelectByCustomRules

		private void AppendRegionSelectByCustomRules()
		{
			ArrayList validColumns = new ArrayList();
			foreach (Reference reference in _currentTable.GeneratedColumns)
			{
				Column dc = (Column)reference.Object;
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

			foreach (Reference reference in _currentTable.CustomRetrieveRules)
			{
				CustomRetrieveRule rule = (CustomRetrieveRule)reference.Object;
				if (rule.Generated)
				{
					sb.AppendLine("	#region " + _currentTable.PascalName + "CustomSelectBy" + rule.PascalName);
					sb.AppendLine();
					sb.AppendLine("	/// <summary>");
					if (rule.Description != "")
						sb.AppendLine("	/// " + rule.Description);
					else
						sb.AppendLine("	/// Select command used to select all rows based on the '" + rule.PascalName + "' rule.");
					sb.AppendLine("	/// </summary>");
					sb.AppendLine("	[Serializable]");
					sb.AppendLine("	public partial class " + _currentTable.PascalName + "CustomSelectBy" + rule.PascalName + " : SelectCommand, ISerializable");
					sb.AppendLine("	{");
					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						sb.AppendLine("		private " + parameter.GetCodeType() + " m" + parameter.PascalName + "; ");
					}
					if (rule.UseSearchObject)
						sb.AppendLine("		private " + _currentTable.PascalName + "Search " + _currentTable.PascalName + ";");

					sb.AppendLine();
					sb.AppendLine("		#region Serialization");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Serialization constructor");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected " + _currentTable.PascalName + "CustomSelectBy" + rule.PascalName + "(SerializationInfo info, StreamingContext context): base(info,context)");
					sb.AppendLine("		{");
					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (parameter.DataType == System.Data.SqlDbType.UniqueIdentifier)
							sb.AppendLine("			m" + parameter.PascalName + " = new Guid(info." + Globals.GetSerializationMethod(parameter.DataType) + "(\"m" + parameter.PascalName + "\"));");
						else
							sb.AppendLine("			m" + parameter.PascalName + " = info." + Globals.GetSerializationMethod(parameter.DataType) + "(\"m" + parameter.PascalName + "\");");
					}

					if (rule.UseSearchObject)
						sb.AppendLine("			this." + _currentTable.PascalName + " = (" + _currentTable.PascalName + "Search)info.GetValue(\"" + _currentTable.PascalName + "\", typeof(" + _currentTable.PascalName + "Search));");

					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Method used internally for serialization");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
					sb.AppendLine("		{");
					sb.AppendLine("			base.GetObjectData(info, context);");
					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (!parameter.IsOutputParameter)
						{
							sb.AppendLine("			info.AddValue(\"m" + parameter.PascalName + "\", m" + parameter.PascalName + ");");
						}
					}

					if (rule.UseSearchObject)
						sb.AppendLine("			info.AddValue(\"" + _currentTable.PascalName + "\", this." + _currentTable.PascalName + ");");

					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		#endregion ");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Select command that allows for the selection by field of database rows");
					sb.AppendLine("		/// </summary>");
					sb.Append("		public " + _currentTable.PascalName + "CustomSelectBy" + rule.PascalName + "(");
					if (rule.UseSearchObject)
					{
						sb.Append(_currentTable.PascalName + "Search " + _currentTable.CamelName + ", ");
					}
					if (sb.ToString().EndsWith(", ")) sb.Remove(sb.ToString().Length - 2, 2);

					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (!parameter.IsOutputParameter)
						{
							if (parameter.IsOutputParameter)
								sb.Append("out ");

							sb.Append(parameter.GetCodeType() + " " + parameter.CamelName + ", ");
						}
					}
					if (sb.ToString().EndsWith(", ")) sb.Remove(sb.ToString().Length - 2, 2);
					sb.AppendLine(")");
					sb.AppendLine("		{");
					if (rule.UseSearchObject)
						sb.AppendLine("			this." + _currentTable.PascalName + " = " + _currentTable.CamelName + ";");

					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (!parameter.IsOutputParameter)
						{
							sb.AppendLine("			m" + parameter.PascalName + " = " + parameter.CamelName + ";");
						}
					}
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Creates a persistable domainCollection");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
					sb.AppendLine("		{");
					sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
					sb.AppendLine("			return colDomain;");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentTable.PascalName + "' collection in a paged fashion");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public override string StoredProcedureName");
					sb.AppendLine("		{");
					sb.AppendLine("			get");
					sb.AppendLine("			{");

					if (rule.UseSearchObject)
					{
						sb.AppendLine("				if (" + _currentTable.PascalName + ".SearchType == SearchType.OR)");
						sb.AppendLine("					return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "CustomSelectBy" + rule.PascalName + "Or\";");
						sb.AppendLine("				else");
						sb.AppendLine("					return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "CustomSelectBy" + rule.PascalName + "And\";");
					}
					else
					{
						sb.AppendLine("					return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "CustomSelectBy" + rule.PascalName + "\";");
					}

					sb.AppendLine("			}");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Initializes the parameters for this select command");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
					sb.AppendLine("		{");
					if (rule.UseSearchObject)
					{
						sb.AppendLine("			if (" + _currentTable.PascalName + " != null)");
						sb.AppendLine("			{");
						foreach (Column dc in validColumns)
						{
							sb.AppendLine("				if (" + _currentTable.PascalName + "." + dc.PascalName + " != null)");
							sb.AppendLine("				{");
							sb.AppendLine("					ParameterValues.Add(\"@" + dc.DatabaseName + "\", " + _currentTable.PascalName + "." + dc.PascalName + ");");
							sb.AppendLine("				}");
						}
						sb.AppendLine("				ParameterValues.Add(\"@max_row_count\", " + _currentTable.PascalName + ".MaxRowCount);");
						sb.AppendLine("			}");
						sb.AppendLine();
					}

					//Now load the specified parameters
					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (!parameter.IsOutputParameter)
						{
							sb.AppendLine("			ParameterValues.Add(\"@" + parameter.DatabaseName + "\", m" + parameter.PascalName + ");");
						}
					}

					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Execute this select command on the database");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public override IDomainCollection Execute(SubDomainBase subDomain)");
					sb.AppendLine("		{");
					sb.AppendLine("			this.SetupParameterValues(subDomain);");
					sb.AppendLine("			PersistableDomainCollectionBase returnTable = (PersistableDomainCollectionBase)this.CreateDomainCollection();");
					sb.AppendLine();
					sb.AppendLine("			IDbCommand fillCommand = PersistableDomainCollectionBase.GetFillCommand(this.ConnectionString, this.StoredProcedureName);");
					sb.AppendLine("			fillCommand.CommandTimeout = this.DefaultTimeOut;");

					if (rule.UseSearchObject)
					{
						//sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@max_row_count\", this." + _currentTable.PascalName + ".MaxRowCount);");
						foreach (Column dc in validColumns)
							sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@" + dc.DatabaseName + "\", this." + _currentTable.PascalName + "." + dc.PascalName + ");");
					}

					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (!parameter.IsOutputParameter)
						{
							sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@" + parameter.DatabaseName + "\", m" + parameter.PascalName + ");");
						}
					}

					//Do the output parameters
					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (parameter.IsOutputParameter)
						{
							sb.AppendLine("			IDbDataParameter paramOut" + parameter.Name + " = PersistableDomainCollectionBase.GetOutputParameter(fillCommand, \"@" + parameter.Name + "\");");
						}
					}

					sb.AppendLine("			returnTable.FillDataTable(this.ConnectionString, fillCommand);");

					//Load the output parameters
					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (parameter.IsOutputParameter)
						{
							sb.AppendLine("			m" + parameter.PascalName + " = (" + parameter.GetCodeType(true) + ")paramOut" + parameter.Name + ".Value;");
						}
					}

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
					sb.AppendLine("		#region Property Implementations");
					sb.AppendLine();
					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (parameter.IsOutputParameter)
						{
							sb.AppendLine("		public " + parameter.GetCodeType(true) + " " + parameter.PascalName);
							sb.AppendLine("		{");
							sb.AppendLine("			get {	return m" + parameter.PascalName + "; }");
							sb.AppendLine("		}");
							sb.AppendLine();
						}
					}
					sb.AppendLine("		#endregion");

					sb.AppendLine();
					sb.AppendLine("	}");
					sb.AppendLine();
					sb.AppendLine("	#endregion");
					sb.AppendLine();
				}
			}
		}

		#endregion

		#region AppendRegionSelectByForeignKeys

		private void AppendRegionSelectByForeignKeys()
		{
			List<string> generatedClasses = new List<string>();
			foreach (Relation relation in _currentTable.GetChildRoleRelationsFullHierarchy().Where(x => x.IsGenerated))
			{
				Table parentTable = (Table)relation.ParentTableRef.Object;
				Table childTable = (Table)relation.ChildTableRef.Object;

				//if (childTable.IsInheritedFrom(parentTable))
				//{
				//  //Do Nothing
				//}
				//else if (!relation.IsPrimaryKeyRelation())
				if (!relation.IsPrimaryKeyRelation())
				{
					//Create a select command not only for this parent table, but ALL table that inherit from this table
					List<Table> allTables = parentTable.GetTablesInheritedFromHierarchy();
					allTables.Add(parentTable);
					foreach (Table currentParent in allTables.Where(x => x.Generated))
					{
						#region Code for select parent by relation (non-primary key)
						string objectName = _currentTable.PascalName + "SelectBy" + relation.PascalRoleName + currentParent.PascalName + "ParentRelation";
						if (!generatedClasses.Contains(objectName))
						{
							generatedClasses.Add(objectName);

							sb.AppendLine("	#region " + objectName);
							sb.AppendLine();
							sb.AppendLine("	/// <summary>");
							sb.AppendLine("	/// Select command used to select all rows from the '" + _currentTable.PascalName + "' table based on the parent relation");
							sb.AppendLine("	/// </summary>");
							sb.AppendLine("	[Serializable]");
							sb.AppendLine("	public partial class " + objectName + " : SelectCommand, ISerializable");
							sb.AppendLine("	{");
							sb.AppendLine("		private string xmlKey = null;");
							if (_currentTable.SelfReference)
							{
								sb.AppendLine("		private RecurseDirection mDirection;");
								sb.AppendLine("		private int mLevels;");
							}
							sb.AppendLine();
							sb.AppendLine("		#region Serialization");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Serialization constructor");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		protected " + objectName + "(SerializationInfo info, StreamingContext context): base(info,context)");
							sb.AppendLine("		{");
							if (_currentTable.SelfReference)
							{
								sb.AppendLine("			mDirection = (RecurseDirection)info.GetValue(\"mDirection\", typeof(RecurseDirection));");
								sb.AppendLine("			mLevels = (int)info.GetValue(\"mLevels\", typeof(int));");
							}
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Method used internally for serialization");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
							sb.AppendLine("		{");
							sb.AppendLine("			base.GetObjectData(info, context);");
							if (_currentTable.SelfReference)
							{
								sb.AppendLine("			info.AddValue(\"mDirection\", mDirection);");
								sb.AppendLine("			info.AddValue(\"mLevels\", mLevels);");
							}
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		#endregion ");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Default constructor");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public " + objectName + "()");
							sb.AppendLine("		{");
							if (_currentTable.SelfReference)
							{
								sb.AppendLine("			mDirection = RecurseDirection.NONE;");
								sb.AppendLine("			mLevels = -1;");
							}
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Constructor that takes a list of primary keys to select");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public " + objectName + "(ArrayList in" + relation.PascalRoleName + _currentTable.PascalName + childTable.PascalName + relation.RoleName + "RelationKeys)");
							sb.AppendLine("		{");
							sb.AppendLine("			xmlKey = Domain" + currentParent.PascalName + "Collection.Get" + relation.RoleName + _currentTable.PascalName + "RelationKeyXml(in" + relation.PascalRoleName + _currentTable.PascalName + childTable.PascalName + relation.RoleName + "RelationKeys);");
							if (_currentTable.SelfReference)
							{
								sb.AppendLine("			mDirection = RecurseDirection.NONE;");
								sb.AppendLine("			mLevels = -1;");
							}
							sb.AppendLine("		}");
							sb.AppendLine();
							if (_currentTable.SelfReference)
							{
								sb.AppendLine("		public " + objectName + "(RecurseDirection direction)");
								sb.AppendLine("		{");
								sb.AppendLine("			mDirection = direction;");
								sb.AppendLine("			mLevels = -1;");
								sb.AppendLine("		}");
								sb.AppendLine();
								sb.AppendLine("		public " + objectName + "(RecurseDirection direction, int level)");
								sb.AppendLine("		{");
								sb.AppendLine("			mDirection = direction;");
								sb.AppendLine("			mLevels = level;");
								sb.AppendLine("		}");
								sb.AppendLine();
							}
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Creates a PersistableDomainCollectionBase of a collection type that holds objects selected by this select command");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
							sb.AppendLine("		{");
							sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
							sb.AppendLine("			return colDomain;");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// The stored procedure name to select values by a foreign key to populate a '" + _currentTable.PascalName + "' collection");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public override string StoredProcedureName");
							sb.AppendLine("		{");
							sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectBy" + relation.PascalRoleName + "" + currentParent.PascalName + "Pks\"; }");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Initializes the parameters for this select command");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
							sb.AppendLine("		{");
							sb.AppendLine("			SubDomain sd = (SubDomain)subDomain;");
							sb.AppendLine("			if(xmlKey != string.Empty)");
							sb.AppendLine("			{");
							sb.AppendLine("				if (!ParameterValues.ContainsKey(\"@xml\"))");
							sb.AppendLine("					ParameterValues.Add(\"@xml\", xmlKey);");
							sb.AppendLine("			}");
							sb.AppendLine("			else");
							sb.AppendLine("			{");
							sb.AppendLine("				Domain" + currentParent.PascalName + "Collection ic = (Domain" + currentParent.PascalName + "Collection)sd.GetDomainCollection(Collections." + currentParent.PascalName + "Collection);");

							//This MUST be the REAL child table name
							sb.AppendLine("				ic." + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled = true;");

							sb.AppendLine("				if (!ParameterValues.ContainsKey(\"@xml\"))");
							sb.AppendLine("					ParameterValues.Add(\"@xml\", ic.Get" + relation.RoleName + _currentTable.PascalName + "RelationKeyXml());");
							sb.AppendLine("			}");
							if (_currentTable.SelfReference)
							{
								sb.AppendLine("			if (mPrimaryKeys.Count > 1)");
								sb.AppendLine("			{");
								sb.AppendLine("				ParameterValues.Add(\"@direction\", mDirection.ToString());");
								sb.AppendLine("				if (mLevels >= 0) ParameterValues.Add(\"@levels\", mLevels);");
								sb.AppendLine("				else ParameterValues.Add(\"@levels\", 50);");
								sb.AppendLine("			}");
							}
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
						#endregion
					}
				}
				else if (parentTable.Generated && parentTable.Key != _currentTable.Key)
				{
					childTable = _currentTable; //For inherited tables

					//Create a select command not only for this parent table, but ALL table that inherit from this table
					List<Table> allTables = new List<Table>();
					allTables.Add(parentTable);
					//if (_currentTable == parentTable) allTables.AddRange(parentTable.GetTablesInheritedFromHierarchy());					
					allTables.AddRange(parentTable.GetTablesInheritedFromHierarchy());
					foreach (Table currentParent in allTables.Where(x => x.Generated))
					{
						#region Code for select parent object by primary key
						string objectName = childTable.PascalName + "SelectBy" + relation.PascalRoleName + currentParent.PascalName + "Pks";
						if (!generatedClasses.Contains(objectName))
						{
							generatedClasses.Add(objectName);

							sb.AppendLine("	#region " + objectName);
							sb.AppendLine();
							sb.AppendLine("	/// <summary>");
							sb.AppendLine("	/// Select command used to select all rows from the '" + childTable.PascalName + "' table based on primary keys");
							sb.AppendLine("	/// </summary>");
							sb.AppendLine("	[Serializable]");
							sb.AppendLine("	public partial class " + objectName + " : SelectCommand, ISerializable");
							sb.AppendLine("	{");
							sb.AppendLine("		private string m" + relation.PascalRoleName + "" + currentParent.PascalName + "PrimaryKeysXml = null;");
							if (childTable.SelfReference)
							{
								sb.AppendLine("		private RecurseDirection mDirection;");
								sb.AppendLine("		private int mLevels;");
							}
							sb.AppendLine();
							sb.AppendLine("		#region Serialization");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Serialization constructor");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		protected " + objectName + "(SerializationInfo info, StreamingContext context): base(info,context)");
							sb.AppendLine("		{");
							if (childTable.SelfReference)
							{
								sb.AppendLine("			mDirection = (RecurseDirection)info.GetValue(\"mDirection\", typeof(RecurseDirection));");
								sb.AppendLine("			mLevels = (int)info.GetValue(\"mLevels\", typeof(int));");
							}
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Method used internally for serialization");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
							sb.AppendLine("		{");
							sb.AppendLine("			base.GetObjectData(info, context);");
							if (childTable.SelfReference)
							{
								sb.AppendLine("			info.AddValue(\"mDirection\", mDirection);");
								sb.AppendLine("			info.AddValue(\"mLevels\", mLevels);");
							}
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		#endregion ");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Default constructor");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public " + objectName + "()");
							sb.AppendLine("		{");
							if (childTable.SelfReference)
							{
								sb.AppendLine("			mDirection = RecurseDirection.NONE;");
								sb.AppendLine("			mLevels = -1;");
							}
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Constructor that takes a list of primary keys to select");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public " + objectName + "(ArrayList in" + relation.PascalRoleName + "" + currentParent.PascalName + "PrimaryKeys)");
							sb.AppendLine("		{");
							sb.AppendLine("			m" + relation.PascalRoleName + currentParent.PascalName + "PrimaryKeysXml = Domain" + currentParent.PascalName + "Collection.GetPrimaryKeyXml(in" + relation.PascalRoleName + "" + currentParent.PascalName + "PrimaryKeys);");
							if (childTable.SelfReference)
							{
								sb.AppendLine("			mDirection = RecurseDirection.NONE;");
								sb.AppendLine("			mLevels = -1;");
							}
							sb.AppendLine("		}");
							sb.AppendLine();
							if (childTable.SelfReference)
							{
								sb.AppendLine();
								sb.AppendLine("		public " + objectName + "(RecurseDirection direction)");
								sb.AppendLine("		{");
								sb.AppendLine("			mDirection = direction;");
								sb.AppendLine("			mLevels = -1;");
								sb.AppendLine("		}");
								sb.AppendLine();
								sb.AppendLine("		public " + objectName + "(RecurseDirection direction, int level)");
								sb.AppendLine("		{");
								sb.AppendLine("			mDirection = direction;");
								sb.AppendLine("			mLevels = level;");
								sb.AppendLine("		}");
							}
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Creates a PersistableDomainCollectionBase of a collection type that holds objects selected by this select command");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
							sb.AppendLine("		{");
							sb.AppendLine("			Domain" + childTable.PascalName + "Collection colDomain = new Domain" + childTable.PascalName + "Collection();");
							sb.AppendLine("			return colDomain;");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// The stored procedure name to select values by a foreign key to populate a '" + childTable.PascalName + "' collection");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public override string StoredProcedureName");
							sb.AppendLine("		{");
							sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, childTable) + "SelectBy" + relation.PascalRoleName + "" + currentParent.PascalName + "Pks\"; }");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Initializes the parameters for this select command");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
							sb.AppendLine("		{");
							sb.AppendLine("			SubDomain sd = (SubDomain)subDomain;");
							sb.AppendLine("			if(m" + relation.PascalRoleName + "" + currentParent.PascalName + "PrimaryKeysXml != null)");
							sb.AppendLine("			{");
							sb.AppendLine("				if (!ParameterValues.ContainsKey(\"@xml\"))");
							sb.AppendLine("					ParameterValues.Add(\"@xml\", m" + relation.PascalRoleName + "" + currentParent.PascalName + "PrimaryKeysXml);");
							sb.AppendLine("			}");
							sb.AppendLine("			else");
							sb.AppendLine("			{");
							sb.AppendLine("				Domain" + currentParent.PascalName + "Collection ic = (Domain" + currentParent.PascalName + "Collection)sd.GetDomainCollection(Collections." + currentParent.PascalName + "Collection);");

							//This MUST be the REAL child table name
							sb.AppendLine("				ic." + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled = true;");

							sb.AppendLine("				if (!ParameterValues.ContainsKey(\"@xml\"))");
							sb.AppendLine("					ParameterValues.Add(\"@xml\", ic.GetPrimaryKeyXml());");
							sb.AppendLine("			}");
							if (childTable.SelfReference)
							{
								sb.AppendLine("			ParameterValues.Add(\"@direction\", mDirection.ToString());");
								sb.AppendLine("			if (mLevels >= 0) ParameterValues.Add(\"@levels\", mLevels);");
								sb.AppendLine("			else ParameterValues.Add(\"@levels\", 50);");
							}
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
						#endregion
					}
				}
			}
		}

		#endregion

		#region AppendRegionSelectByChildTables

		/// <summary>
		/// This generates a select command to get a child table from a loaded parent when 
		/// the relationship is NOT based on a primary key. If the relationship is based on a primary key
		/// then the SelectBy[XXX]Pk procedure is called.
		/// </summary>
		private void AppendRegionSelectByChildTables()
		{
			//foreach (Relation relation in _currentTable.ParentRoleRelations.Where(x => x.IsGenerated))
			foreach (Relation relation in _currentTable.GetParentRoleRelationsFullHierarchy().Where(x => x.IsGenerated))
			{
				//This is NOT a primary key based relationship
				if (!relation.IsPrimaryKeyRelation())
				{
					string objectName = _currentTable.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "RelationCommand";
					sb.AppendLine();
					sb.AppendLine("	#region " + objectName);
					sb.AppendLine();
					sb.AppendLine("	/// <summary>");
					sb.AppendLine("	/// Select command used to select all rows from the '" + _currentTable.PascalName + "' table based on the relationship with " + ((Table)relation.ChildTableRef.Object).PascalName);
					sb.AppendLine("	/// </summary>");
					sb.AppendLine("	[Serializable]");
					sb.AppendLine("	public partial class " + objectName + ": SelectCommand, ISerializable");
					sb.AppendLine("	{");

					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						Column relationColumn = ((Column)columnRelationship.ParentColumnRef.Object);
						sb.AppendLine("		private " + relationColumn.GetCodeType(true) + " _" + relationColumn.CamelName + ";");
					}

					if (_currentTable.SelfReference)
					{
						sb.AppendLine("		private RecurseDirection mDirection;");
						sb.AppendLine("		private int mLevels;");
					}

					sb.AppendLine();
					sb.AppendLine("		#region Serialization");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Serialization constructor");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected " + objectName + "(SerializationInfo info, StreamingContext context): base(info,context)");
					sb.AppendLine("		{");
					if (_currentTable.SelfReference)
					{
						sb.AppendLine("			mDirection = (RecurseDirection)info.GetValue(\"mDirection\", typeof(RecurseDirection));");
						sb.AppendLine("			mLevels = (int)info.GetValue(\"mLevels\", typeof(int));");
					}
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Method used internally for serialization");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
					sb.AppendLine("		{");
					sb.AppendLine("			base.GetObjectData(info, context);");
					if (_currentTable.SelfReference)
					{
						sb.AppendLine("			info.AddValue(\"mDirection\", mDirection);");
						sb.AppendLine("			info.AddValue(\"mLevels\", mLevels);");
					}
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		#endregion ");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Default constructor");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public " + objectName + "()");
					sb.AppendLine("		{");
					if (_currentTable.SelfReference)
					{
						sb.AppendLine("			mDirection = RecurseDirection.NONE;");
						sb.AppendLine("			mLevels = -1;");
					}
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Constructor that takes a list of field on which to select");
					sb.AppendLine("		/// </summary>");
					sb.Append("		public " + objectName + "(");
					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						Column relationColumn = ((Column)columnRelationship.ParentColumnRef.Object);
						sb.Append(relationColumn.GetCodeType(true) + " " + relationColumn.CamelName);
						if (relation.ColumnRelationships.IndexOf(columnRelationship) < relation.ColumnRelationships.Count - 1)
							sb.Append(", ");
					}
					sb.AppendLine(")");
					sb.AppendLine("		{");
					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						Column relationColumn = ((Column)columnRelationship.ParentColumnRef.Object);
						sb.AppendLine("			_" + relationColumn.CamelName + " = " + relationColumn.CamelName + ";");
					}

					if (_currentTable.SelfReference)
					{
						sb.AppendLine("			mDirection = RecurseDirection.NONE;");
						sb.AppendLine("			mLevels = -1;");
					}
					sb.AppendLine("		}");
					sb.AppendLine();
					if (_currentTable.SelfReference)
					{
						sb.AppendLine("		public " + objectName + "(RecurseDirection direction)");
						sb.AppendLine("		{");
						sb.AppendLine("			mDirection = direction;");
						sb.AppendLine("			mLevels = -1;");
						sb.AppendLine("		}");
						sb.AppendLine();
						sb.AppendLine("		public " + objectName + "(RecurseDirection direction, int level)");
						sb.AppendLine("		{");
						sb.AppendLine("			mDirection = direction;");
						sb.AppendLine("			mLevels = level;");
						sb.AppendLine("		}");
						sb.AppendLine();
					}
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Creates a PersistableDomainCollectionBase of a collection type that holds objects selected by this select command");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
					sb.AppendLine("		{");
					sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
					sb.AppendLine("			return colDomain;");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The stored procedure name to select values by a foreign key to populate a '" + _currentTable.PascalName + "' collection");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public override string StoredProcedureName");
					sb.AppendLine("		{");
					sb.AppendLine("			get { return \"gen_" + objectName + "\"; }");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Initializes the parameters for this select command");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
					sb.AppendLine("		{");
					sb.AppendLine("			SubDomain sd = (SubDomain)subDomain;");

					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						Column relationColumn = ((Column)columnRelationship.ParentColumnRef.Object);
						sb.AppendLine("			ParameterValues.Add(\"@" + relationColumn.PascalName + "\", _" + relationColumn.CamelName + ");");
					}

					if (_currentTable.SelfReference)
					{
						sb.AppendLine("			if (mPrimaryKeys.Count > 1)");
						sb.AppendLine("			{");
						sb.AppendLine("				ParameterValues.Add(\"@direction\", mDirection.ToString());");
						sb.AppendLine("				if (mLevels >= 0) ParameterValues.Add(\"@levels\", mLevels);");
						sb.AppendLine("				else ParameterValues.Add(\"@levels\", 50);");
						sb.AppendLine("			}");
					}
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
		}

		#endregion

		#region AppendRegionSelectByPks

		private void AppendRegionSelectByPks()
		{
			sb.AppendLine("	#region " + _currentTable.PascalName + "SelectByPks");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// A select command that selects objects by primary key");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "SelectByPks : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine("		private List<" + _currentTable.PascalName + "PrimaryKey> mPrimaryKeys = null;");
			if (_currentTable.SelfReference)
			{
				sb.AppendLine("		private RecurseDirection mDirection;");
				sb.AppendLine("		private int mLevels;");
			}
			sb.AppendLine();
			sb.AppendLine("		#region Serialization");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected " + _currentTable.PascalName + "SelectByPks(SerializationInfo info, StreamingContext context): base(info,context)");
			sb.AppendLine("		{");
			sb.AppendLine("			mPrimaryKeys = new List<" + _currentTable.PascalName + "PrimaryKey>();");
			sb.AppendLine("			mPrimaryKeys.AddRange((" + _currentTable.PascalName + "PrimaryKey[])info.GetValue(\"mPrimaryKeys\", typeof(" + _currentTable.PascalName + "PrimaryKey[])));");
			if (_currentTable.SelfReference)
			{
				sb.AppendLine("			mDirection = (RecurseDirection)info.GetValue(\"mDirection\", typeof(RecurseDirection));");
				sb.AppendLine("			mLevels = (int)info.GetValue(\"mLevels\", typeof(int));");
			}
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Method used internally for serialization");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
			sb.AppendLine("		{");
			sb.AppendLine("			base.GetObjectData(info, context);");
			sb.AppendLine("			info.AddValue(\"mPrimaryKeys\", mPrimaryKeys.ToArray());");
			if (_currentTable.SelfReference)
			{
				sb.AppendLine("			info.AddValue(\"mDirection\", mDirection);");
				sb.AppendLine("			info.AddValue(\"mLevels\", mLevels);");
			}
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion ");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select objects by primary key");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "SelectByPks(" + PrimaryKeyParameterList() + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			mPrimaryKeys = new List<" + _currentTable.PascalName + "PrimaryKey>();");
			sb.AppendLine("			mPrimaryKeys.Add(new " + _currentTable.PascalName + "PrimaryKey(" + PrimaryKeyInputParameterList() + "));");
			sb.AppendLine();
			if (_currentTable.SelfReference)
			{
				sb.AppendLine("			mDirection = RecurseDirection.NONE;");
				sb.AppendLine("			mLevels = -1;");
			}
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select objects by a list of primary keys");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "SelectByPks(IEnumerable<" + _currentTable.PascalName + "PrimaryKey> primaryKeys)");
			sb.AppendLine("		{");
			sb.AppendLine("			mPrimaryKeys = new List<" + _currentTable.PascalName + "PrimaryKey>();");
			sb.AppendLine("			mPrimaryKeys.AddRange(primaryKeys);");
			if (_currentTable.SelfReference)
			{
				sb.AppendLine("			mDirection = RecurseDirection.NONE;");
				sb.AppendLine("			mLevels = -1;");
			}
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select objects by a list of primary keys");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		[Obsolete(\"This method has been deprecated. Please use another overload.\")]");
			sb.AppendLine("		public " + _currentTable.PascalName + "SelectByPks(ArrayList primaryKeys)");
			sb.AppendLine("		{");
			sb.AppendLine("			mPrimaryKeys = new List<" + _currentTable.PascalName + "PrimaryKey>();");
			sb.AppendLine("			foreach (" + _currentTable.PascalName + "PrimaryKey item in primaryKeys)");
			sb.AppendLine("				mPrimaryKeys.Add(item);");
			if (_currentTable.SelfReference)
			{
				sb.AppendLine("			mDirection = RecurseDirection.NONE;");
				sb.AppendLine("			mLevels = -1;");
			}
			sb.AppendLine("		}");
			sb.AppendLine();

			if (_currentTable.SelfReference)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by a list of primary keys");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentTable.PascalName + "SelectByPks(IEnumerable<" + _currentTable.PascalName + "PrimaryKey> primaryKeys, RecurseDirection direction)");
				sb.AppendLine("		{");
				sb.AppendLine("			mPrimaryKeys = new List<" + _currentTable.PascalName + "PrimaryKey>();");
				sb.AppendLine("			mPrimaryKeys.AddRange(primaryKeys);");
				sb.AppendLine("			mDirection = direction;");
				sb.AppendLine("			mLevels = -1;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by a list of primary keys");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentTable.PascalName + "SelectByPks(IEnumerable<" + _currentTable.PascalName + "PrimaryKey> primaryKeys, RecurseDirection direction, int level)");
				sb.AppendLine("		{");
				sb.AppendLine("			mPrimaryKeys = new List<" + _currentTable.PascalName + "PrimaryKey>();");
				sb.AppendLine("			mPrimaryKeys.AddRange(primaryKeys);");
				sb.AppendLine("			mDirection = direction;");
				sb.AppendLine("			mLevels = level;");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by a list of primary keys");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[Obsolete(\"This method has been deprecated. Please use another overload.\")]");
				sb.AppendLine("		public " + _currentTable.PascalName + "SelectByPks(ArrayList primaryKeys, RecurseDirection direction)");
				sb.AppendLine("		{");
				sb.AppendLine("			mPrimaryKeys = new List<" + _currentTable.PascalName + "PrimaryKey>();");
				sb.AppendLine("			foreach (" + _currentTable.PascalName + "PrimaryKey item in primaryKeys)");
				sb.AppendLine("				mPrimaryKeys.Add(item);");
				sb.AppendLine("			mDirection = direction;");
				sb.AppendLine("			mLevels = -1;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by a list of primary keys");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[Obsolete(\"This method has been deprecated. Please use another overload.\")]");
				sb.AppendLine("		public " + _currentTable.PascalName + "SelectByPks(ArrayList primaryKeys, RecurseDirection direction, int level)");
				sb.AppendLine("		{");
				sb.AppendLine("			mPrimaryKeys = new List<" + _currentTable.PascalName + "PrimaryKey>();");
				sb.AppendLine("			foreach (" + _currentTable.PascalName + "PrimaryKey item in primaryKeys)");
				sb.AppendLine("				mPrimaryKeys.Add(item);");
				sb.AppendLine("			mDirection = direction;");
				sb.AppendLine("			mLevels = level;");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
			
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a persistable domainCollection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values by primary key to populate a '" + _currentTable.PascalName + "' collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				if (mPrimaryKeys.Count == 1) return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectBy" + _currentTable.PascalName + "SinglePk\";");
			sb.AppendLine("				else return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectBy" + _currentTable.PascalName + "Pks\";");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Initializes the parameters for this select command");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
			sb.AppendLine("		{");

			sb.AppendLine("			if (mPrimaryKeys.Count == 1)");
			sb.AppendLine("			{");
			sb.AppendLine("				" + _currentTable.PascalName + "PrimaryKey key = ((" + _currentTable.PascalName + "PrimaryKey)mPrimaryKeys[0]);");
			foreach (Column c in _currentTable.PrimaryKeyColumns)
			{
				sb.AppendLine("				ParameterValues.Add(\"@" + c.DatabaseName + "\", key." + c.PascalName + ");");
			}
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("				ParameterValues.Add(\"@xml\", Domain" + _currentTable.PascalName + "Collection.GetPrimaryKeyXml(new ArrayList(mPrimaryKeys)));");			
			if (_currentTable.SelfReference)
			{
				sb.AppendLine("			if (mPrimaryKeys.Count > 1)");
				sb.AppendLine("			{");
				sb.AppendLine("				ParameterValues.Add(\"@direction\", mDirection.ToString());");
				sb.AppendLine("				if (mLevels >= 0) ParameterValues.Add(\"@levels\", mLevels);");
				sb.AppendLine("				else ParameterValues.Add(\"@levels\", 50);");
				sb.AppendLine("			}");
			}
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

		#endregion

		#region AppendRegionSelectAll

		private void AppendRegionSelectAll()
		{
			sb.AppendLine("	#region " + _currentTable.PascalName + "SelectAll");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// The select command used to select all rows from the '" + _currentTable.PascalName + "' table");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "SelectAll : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine();
			sb.AppendLine("		#region Serialization");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected " + _currentTable.PascalName + "SelectAll(SerializationInfo info, StreamingContext context): base(info,context)");
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
			sb.AppendLine("		public " + _currentTable.PascalName + "SelectAll()");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a persistable domainCollection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentTable.PascalName + "' collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "Select\";");
			sb.AppendLine("			}");
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

		#endregion

		#region AppendRegionPagedSelect

		private void AppendRegionPagedSelect()
		{
			sb.AppendLine("	#region " + _currentTable.PascalName + "PagedSelect");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// Select command used to perform paged selects from the '" + _currentTable.PascalName + "' table");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "PagedSelect : SelectCommand, ISerializable");
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
			sb.AppendLine("		protected " + _currentTable.PascalName + "PagedSelect(SerializationInfo info, StreamingContext context): base(info,context)");
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
			sb.AppendLine("		public " + _currentTable.PascalName + "PagedSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter)");
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
			sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentTable.PascalName + "' collection in a paged fashion");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "PagingSelect\"; }");
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
			sb.AppendLine("			PersistableDomainCollectionBase returnTable = (PersistableDomainCollectionBase)this.CreateDomainCollection();");
			sb.AppendLine();
			sb.AppendLine("			IDbCommand fillCommand = PersistableDomainCollectionBase.GetFillCommand(this.ConnectionString, this.StoredProcedureName);");
			sb.AppendLine("			fillCommand.CommandTimeout = this.DefaultTimeOut;");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@page\", mPage);");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@pageSize\", mPageSize);");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@orderByColumn\", mOrderByColumn);");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@ascending\", mAscending);");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@filter\", mFilter);");
			//sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@junk\", Guid.NewGuid().ToString());");
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
		}

		#endregion

		#region AppendRegionSearchable

		private void AppendRegionSearchable()
		{
			//Get a list of all base column selects
			List<Table> tList = _currentTable.GetTableHierarchy();
			tList.Remove(_currentTable);
			List<Column> baseColumns = new List<Column>();
			foreach (Table t in tList)
			{
				baseColumns.AddRange(t.GetColumnsSearchable());
			}

			//Make commands for searchable fields
			foreach (Reference reference in _currentTable.Columns)
			{
				Column column = (Column)reference.Object;
				if (column.IsSearchable)
				{
					AppendRegionSearchableSingleField(column);

					//Make commands for searchable fields (RANGES)
					if (column.IsRangeType)
						AppendRegionSearchableSingleFieldRange(column);
				}
			}

			//Create selects to override all base tables selects
			foreach (Column column in baseColumns)
			{
				AppendRegionSearchableSingleField(column);

				//Make commands for searchable fields (RANGES)
				if (column.IsRangeType)
					AppendRegionSearchableSingleFieldRange(column);
			}

		}

		#endregion

		#region AppendRegionSearchableSingleFieldRange

		private void AppendRegionSearchableSingleFieldRange(Column column)
		{
			sb.AppendLine("	#region " + _currentTable.PascalName + "SelectBy" + column.PascalName + "Range");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// Select command used to perform selects for a range by field '" + column.PascalName + "'");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "SelectBy" + column.PascalName + "Range : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine("		private " + column.GetCodeType() + " m" + column.PascalName + "Start; ");
			sb.AppendLine("		private " + column.GetCodeType() + " m" + column.PascalName + "End; ");
			sb.AppendLine("		private " + _currentTable.PascalName + "Paging _paging = null;");
			sb.AppendLine("		private int mCount = -1;");
			sb.AppendLine();
			sb.AppendLine("		#region Serialization");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected " + _currentTable.PascalName + "SelectBy" + column.PascalName + "Range(SerializationInfo info, StreamingContext context): base(info,context)");
			sb.AppendLine("		{");
			sb.AppendLine("			m" + column.PascalName + "Start = info." + Globals.GetSerializationMethod(column.DataType) + "(\"m" + column.PascalName + "Start\");");
			sb.AppendLine("			m" + column.PascalName + "End = info." + Globals.GetSerializationMethod(column.DataType) + "(\"m" + column.PascalName + "End\");");
			sb.AppendLine("			_paging = (" + _currentTable.PascalName + "Paging)info.GetValue(\"paging\", typeof(" + _currentTable.PascalName + "Paging));");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Method used internally for serialization");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
			sb.AppendLine("		{");
			sb.AppendLine("			base.GetObjectData(info, context);");
			sb.AppendLine("			info.AddValue(\"m" + column.PascalName + "Start\", m" + column.PascalName + "Start);");
			sb.AppendLine("			info.AddValue(\"m" + column.PascalName + "End\", m" + column.PascalName + "End);");
			sb.AppendLine("			info.AddValue(\"paging\", _paging);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion ");
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
			sb.AppendLine("		/// Select command that allows for the selection by field of database rows");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "SelectBy" + column.PascalName + "Range(" + column.GetCodeType() + " " + column.CamelName + "Start, " + column.GetCodeType() + " " + column.CamelName + "End)");
			sb.AppendLine("		{");
			sb.AppendLine("			m" + column.PascalName + "Start = " + column.CamelName + "Start; ");
			sb.AppendLine("			m" + column.PascalName + "End = " + column.CamelName + "End; ");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select command that allows for the paged selection by field of database rows");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "SelectBy" + column.PascalName + "Range(" + column.GetCodeType() + " " + column.CamelName + "Start, " + column.GetCodeType() + " " + column.CamelName + "End, " + _currentTable.PascalName + "Paging paging)");
			sb.AppendLine("				:this(" + column.CamelName + "Start, " + column.CamelName + "End)");
			sb.AppendLine("		{");
			sb.AppendLine("			_paging = paging; ");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a persistable domainCollection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentTable.PascalName + "' collection in a paged fashion");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectBy" + column.PascalName + "Range\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Initializes the parameters for this select command");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (_paging != null)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (_paging.OrderByList.Count != 1)");
			sb.AppendLine("					throw new Exception(\"The select operation supports exactly one OrderBy field.\");");
			sb.AppendLine();
			sb.AppendLine("				ParameterValues.Add(\"@paging_Ascending\", _paging.OrderByList[0].Ascending);");
			sb.AppendLine("				ParameterValues.Add(\"@paging_OrderByColumn\", " + _currentTable.PascalName + "Collection.GetDatabaseFieldName(_paging.OrderByList[0].Field));");
			sb.AppendLine("				ParameterValues.Add(\"@paging_PageIndex\", _paging.PageIndex);");
			sb.AppendLine("				ParameterValues.Add(\"@paging_RecordsperPage\", _paging.RecordsperPage);");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Execute this select command on the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection Execute(SubDomainBase subDomain)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.SetupParameterValues(subDomain);");
			sb.AppendLine("			PersistableDomainCollectionBase returnTable = (PersistableDomainCollectionBase)this.CreateDomainCollection();");
			sb.AppendLine();
			sb.AppendLine("			IDbCommand fillCommand = PersistableDomainCollectionBase.GetFillCommand(this.ConnectionString, this.StoredProcedureName);");
			sb.AppendLine("			fillCommand.CommandTimeout = this.DefaultTimeOut;");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@" + column.DatabaseName + "Start\", m" + column.PascalName + "Start);");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@" + column.DatabaseName + "End\", m" + column.PascalName + "End);");
			sb.AppendLine();
			sb.AppendLine("			//Add the parameters");
			sb.AppendLine("			foreach (string key in ParameterValues.Keys)");
			sb.AppendLine("			{");
			sb.AppendLine("				PersistableDomainCollectionBase.SetParameterValue(fillCommand, key, ParameterValues[key]);");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("			IDbDataParameter paramCount = null;");
			sb.AppendLine("			paramCount = PersistableDomainCollectionBase.GetOutputParameter(fillCommand, \"@paging_Count\");");
			sb.AppendLine();
			sb.AppendLine("			returnTable.FillDataTable(this.ConnectionString, fillCommand);");
			sb.AppendLine("			if (_paging != null)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (paramCount.Value != System.DBNull.Value) mCount = (int)paramCount.Value;");
			sb.AppendLine("				else mCount = 0;");
			sb.AppendLine("			}");
			sb.AppendLine();
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
		}

		#endregion

		#region AppendRegionSearchableSingleField

		private void AppendRegionSearchableSingleField(Column column)
		{
			sb.AppendLine("	#region " + _currentTable.PascalName + "SelectBy" + column.PascalName);
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// Select command used to perform selects by field '" + column.PascalName + "'");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "SelectBy" + column.PascalName + " : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine("		private " + column.GetCodeType() + " m" + column.PascalName + "; ");
			sb.AppendLine("		private " + _currentTable.PascalName + "Paging _paging = null;");
			sb.AppendLine("		private int mCount = -1;");
			sb.AppendLine();
			sb.AppendLine("		#region Serialization");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected " + _currentTable.PascalName + "SelectBy" + column.PascalName + "(SerializationInfo info, StreamingContext context): base(info,context)");
			sb.AppendLine("		{");
			if (column.DataType == System.Data.SqlDbType.UniqueIdentifier)
				sb.AppendLine("			m" + column.PascalName + " = new Guid(info." + Globals.GetSerializationMethod(column.DataType) + "(\"m" + column.PascalName + "\"));");
			else
				sb.AppendLine("			m" + column.PascalName + " = info." + Globals.GetSerializationMethod(column.DataType) + "(\"m" + column.PascalName + "\");");
			sb.AppendLine("			_paging = (" + _currentTable.PascalName + "Paging)info.GetValue(\"paging\", typeof(" + _currentTable.PascalName + "Paging));");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Method used internally for serialization");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
			sb.AppendLine("		{");
			sb.AppendLine("			base.GetObjectData(info, context);");
			sb.AppendLine("			info.AddValue(\"m" + column.PascalName + "\", m" + column.PascalName + ");");
			sb.AppendLine("			info.AddValue(\"paging\", _paging);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion ");
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
			sb.AppendLine("		/// Select command that allows for the selection by field of database rows");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "SelectBy" + column.PascalName + "(" + column.GetCodeType() + " " + column.CamelName + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			m" + column.PascalName + " = " + column.CamelName + "; ");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select command that allows for the paged selection by field of database rows");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "SelectBy" + column.PascalName + "(" + column.GetCodeType() + " " + column.CamelName + ", " + _currentTable.PascalName + "Paging paging)");
			sb.AppendLine("				:this(" + column.CamelName + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			_paging = paging; ");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a persistable domainCollection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentTable.PascalName + "' collection in a paged fashion");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get {return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectBy" + column.PascalName + "\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Initializes the parameters for this select command");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (_paging != null)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (_paging.OrderByList.Count != 1)");
			sb.AppendLine("					throw new Exception(\"The select operation supports exactly one OrderBy field.\");");
			sb.AppendLine();
			sb.AppendLine("				ParameterValues.Add(\"@paging_Ascending\", _paging.OrderByList[0].Ascending);");
			sb.AppendLine("				ParameterValues.Add(\"@paging_OrderByColumn\", " + _currentTable.PascalName + "Collection.GetDatabaseFieldName(_paging.OrderByList[0].Field));");
			sb.AppendLine("				ParameterValues.Add(\"@paging_PageIndex\", _paging.PageIndex);");
			sb.AppendLine("				ParameterValues.Add(\"@paging_RecordsperPage\", _paging.RecordsperPage);");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Execute this select command on the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection Execute(SubDomainBase subDomain)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.SetupParameterValues(subDomain);");
			sb.AppendLine("			PersistableDomainCollectionBase returnTable = (PersistableDomainCollectionBase)this.CreateDomainCollection();");
			sb.AppendLine();
			sb.AppendLine("			IDbCommand fillCommand = PersistableDomainCollectionBase.GetFillCommand(this.ConnectionString, this.StoredProcedureName);");
			sb.AppendLine("			fillCommand.CommandTimeout = this.DefaultTimeOut;");
			sb.AppendLine("			PersistableDomainCollectionBase.SetParameterValue(fillCommand, \"@" + column.DatabaseName + "\", m" + column.PascalName + ");");
			sb.AppendLine();
			sb.AppendLine("			//Add the parameters");
			sb.AppendLine("			foreach (string key in ParameterValues.Keys)");
			sb.AppendLine("			{");
			sb.AppendLine("				PersistableDomainCollectionBase.SetParameterValue(fillCommand, key, ParameterValues[key]);");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("			IDbDataParameter paramCount = null;");
			sb.AppendLine("			paramCount = PersistableDomainCollectionBase.GetOutputParameter(fillCommand, \"@paging_Count\");");
			sb.AppendLine();
			sb.AppendLine("			returnTable.FillDataTable(this.ConnectionString, fillCommand);");
			sb.AppendLine("			if (_paging != null)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (paramCount.Value != System.DBNull.Value) mCount = (int)paramCount.Value;");
			sb.AppendLine("				else mCount = 0;");
			sb.AppendLine("			}");
			sb.AppendLine();
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
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				return ConfigurationValues.GetInstance().ConnectionString;");
			sb.AppendLine("			}");
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

		#endregion

		#region AppendSearchClass

		private void AppendSearchClass()
		{
			ArrayList validColumns = new ArrayList();
			foreach (Reference reference in _currentTable.GeneratedColumns)
			{
				Column dc = (Column)reference.Object;
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
				sb.AppendLine("	#region " + _currentTable.PascalName + "SelectBySearch");
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// The select command used to select all rows from the '" + _currentTable.PascalName + "' table");
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	public partial class " + _currentTable.PascalName + "SelectBySearch : SelectCommand, ISerializable");
				sb.AppendLine("	{");
				sb.AppendLine("		private " + _currentTable.PascalName + "Search " + _currentTable.PascalName + ";");
				sb.AppendLine("		private " + _currentTable.PascalName + "Paging _paging = null;");
				sb.AppendLine("		private int mCount = -1;");
				sb.AppendLine();
				sb.AppendLine("		#region Serialization");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Serialization constructor");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected " + _currentTable.PascalName + "SelectBySearch(SerializationInfo info, StreamingContext context)");
				sb.AppendLine("			: base(info, context)");
				sb.AppendLine("		{");
				sb.AppendLine("			" + _currentTable.PascalName + " = (" + _currentTable.PascalName + "Search)info.GetValue(\"" + _currentTable.PascalName + "\", typeof(" + _currentTable.PascalName + "Search));");
				sb.AppendLine("			_paging = (" + _currentTable.PascalName + "Paging)info.GetValue(\"paging\", typeof(" + _currentTable.PascalName + "Paging));");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Method used internally for serialization");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
				sb.AppendLine("		{");
				sb.AppendLine("			base.GetObjectData(info, context);");
				sb.AppendLine("			info.AddValue(\"" + _currentTable.PascalName + "\", " + _currentTable.PascalName + ");");
				sb.AppendLine("			info.AddValue(\"paging\", _paging);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion ");
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
				sb.AppendLine("		/// Select of objects by the " + _currentTable.PascalName + " search object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentTable.PascalName + "SelectBySearch(" + _currentTable.PascalName + "Search searchObject)");
				sb.AppendLine("		{");
				sb.AppendLine("			" + _currentTable.PascalName + " = searchObject;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Paged select of objects by the " + _currentTable.PascalName + " search object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentTable.PascalName + "SelectBySearch(" + _currentTable.PascalName + "Search searchObject, " + _currentTable.PascalName + "Paging paging)");
				sb.AppendLine("		{");
				sb.AppendLine("			" + _currentTable.PascalName + " = searchObject;");
				sb.AppendLine("			_paging = paging;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Creates a persistable domainCollection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection colDomain = new Domain" + _currentTable.PascalName + "Collection();");
				sb.AppendLine("			return colDomain;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The stored procedure name to select values by primary key to populate a '" + _currentTable.PascalName + "' collection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override string StoredProcedureName");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				if (" + _currentTable.PascalName + ".SearchType == SearchType.OR)");
				sb.AppendLine("					return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectBySearchOr\";");
				sb.AppendLine("				else");
				sb.AppendLine("					return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectBySearchAnd\";");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Initializes the parameters for this select command");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (_paging != null)");
				sb.AppendLine("			{");
				sb.AppendLine("				if (_paging.OrderByList.Count != 1)");
				sb.AppendLine("					throw new Exception(\"The select operation supports exactly one OrderBy field.\");");
				sb.AppendLine();
				sb.AppendLine("				ParameterValues.Add(\"@paging_Ascending\", _paging.OrderByList[0].Ascending);");
				sb.AppendLine("				ParameterValues.Add(\"@paging_OrderByColumn\", " + _currentTable.PascalName + "Collection.GetDatabaseFieldName(_paging.OrderByList[0].Field));");
				sb.AppendLine("				ParameterValues.Add(\"@paging_PageIndex\", _paging.PageIndex);");
				sb.AppendLine("				ParameterValues.Add(\"@paging_RecordsperPage\", _paging.RecordsperPage);");
				sb.AppendLine("			}");
				sb.AppendLine();
				sb.AppendLine("			if (" + _currentTable.PascalName + " != null)");
				sb.AppendLine("			{");
				foreach (Column dc in validColumns)
				{
					sb.AppendLine("				if (" + _currentTable.PascalName + "." + dc.PascalName + " != null)");
					sb.AppendLine("				{");
					sb.AppendLine("					ParameterValues.Add(\"@" + dc.DatabaseName + "\", " + _currentTable.PascalName + "." + dc.PascalName + ");");
					sb.AppendLine("				}");
				}
				//sb.AppendLine("				ParameterValues.Add(\"@max_row_count\", " + _currentTable.PascalName + ".MaxRowCount);");
				sb.AppendLine();
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();



				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Execute this select command on the database");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override IDomainCollection Execute(SubDomainBase subDomain)");
				sb.AppendLine("		{");
				sb.AppendLine("			this.SetupParameterValues(subDomain);");
				sb.AppendLine("			PersistableDomainCollectionBase returnTable = (PersistableDomainCollectionBase)this.CreateDomainCollection();");
				sb.AppendLine();
				sb.AppendLine("			IDbCommand fillCommand = PersistableDomainCollectionBase.GetFillCommand(this.ConnectionString, this.StoredProcedureName);");
				sb.AppendLine("			fillCommand.CommandTimeout = this.DefaultTimeOut;");
				sb.AppendLine();
				sb.AppendLine("			//Add the parameters");
				sb.AppendLine("			foreach (string key in ParameterValues.Keys)");
				sb.AppendLine("			{");
				sb.AppendLine("				PersistableDomainCollectionBase.SetParameterValue(fillCommand, key, ParameterValues[key]);");
				sb.AppendLine("			}");
				sb.AppendLine();
				sb.AppendLine("			IDbDataParameter paramCount = null;");
				sb.AppendLine("			paramCount = PersistableDomainCollectionBase.GetOutputParameter(fillCommand, \"@paging_Count\");");
				sb.AppendLine();
				sb.AppendLine("			returnTable.FillDataTable(this.ConnectionString, fillCommand);");
				sb.AppendLine();
				sb.AppendLine("			if (_paging != null)");
				sb.AppendLine("			{");
				sb.AppendLine("				if (paramCount.Value != System.DBNull.Value) mCount = (int)paramCount.Value;");
				sb.AppendLine("				else mCount = 0;");
				sb.AppendLine("			}");
				sb.AppendLine();
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
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				return ConfigurationValues.GetInstance().ConnectionString;");
				sb.AppendLine("			}");
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
				sb.AppendLine();
				sb.AppendLine("	}");
				sb.AppendLine();
				sb.AppendLine("	#endregion");
			}
		}

		#endregion

		#endregion

		#region append member variables
		public void AppendMemberVariables()
		{
			sb.AppendLine("		private Domain" + _currentTable.PascalName + "Collection col" + _currentTable.PascalName + "List;");
		}
		#endregion

		#region append constructors
		public void AppendConstructor()
		{
			sb.AppendLine("		internal " + _currentTable.PascalName + "CollectionRules(Domain" + _currentTable.PascalName + "Collection in" + _currentTable.PascalName + "List)");
			sb.AppendLine("		{");
			sb.AppendLine("			col" + _currentTable.PascalName + "List = in" + _currentTable.PascalName + "List;");
			sb.AppendLine("			Initialize();");
			sb.AppendLine("		}");
		}
		#endregion

		#region append properties
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

		#region stringhelpers
		protected string PrimaryKeyParameterList()
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Reference reference in _currentTable.GeneratedColumns)
				{
					Column dc = (Column)reference.Object;
					if (dc.PrimaryKey)
					{
						output.Append(dc.GetCodeType() + " ");
						output.Append(StringHelper.DatabaseNameToCamelCase(dc.PascalName));
						output.Append(", ");
					}
				}
				if (output.Length > 2)
				{
					output.Remove(output.Length - 2, 2);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(_currentTable.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}

		protected string PrimaryKeyInputParameterList()
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Reference reference in _currentTable.GeneratedColumns)
				{
					Column dc = (Column)reference.Object;
					if (dc.PrimaryKey)
					{
						output.Append(StringHelper.DatabaseNameToCamelCase(dc.PascalName));
						output.Append(", ");
					}
				}
				if (output.Length > 2)
				{
					output.Remove(output.Length - 2, 2);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(_currentTable.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}
		#endregion

	}
}