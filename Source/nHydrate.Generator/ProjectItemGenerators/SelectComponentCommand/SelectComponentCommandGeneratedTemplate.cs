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

namespace Widgetsphere.Generator.ProjectItemGenerators.SelectComponentCommand
{
	class SelectComponentCommandGeneratedTemplate : BaseClassTemplate
	{

		private StringBuilder sb = new StringBuilder();
		private TableComponent _currentComponent;

		public SelectComponentCommandGeneratedTemplate(ModelRoot model, TableComponent currentComponent)
		{
			_model = model;
			_currentComponent = currentComponent;
		}

		#region BaseClassTemplate overrides

		public override string FileName
		{
			get { return string.Format("{0}.Generated.cs", _currentComponent.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}.cs", _currentComponent.PascalName); }
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
			if (_model.Database.AllowZeroTouch) return;
			try
			{
				ValidationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.SelectCommands");
				sb.AppendLine("{");
				this.AppendClass();
				this.AppendSearchClass();
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
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects.Components;");
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
				this.AppendRegionPagedSelect();
				this.AppendRegionSearchable();
				this.AppendRegionSelectAll();
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
			foreach (Reference reference in _currentComponent.Columns)
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

			if (_currentComponent.Parent.AllowCreateAudit)
			{
				sb.AppendLine("	#region " + _currentComponent.PascalName + "SelectByCreatedDateRange");
				sb.AppendLine();
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// Select objects by their created date.");
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	public partial class " + _currentComponent.PascalName + "SelectByCreatedDateRange : SelectCommand, ISerializable");
				sb.AppendLine("	{");
				sb.AppendLine("		private DateTime? mStartDate;");
				sb.AppendLine("		private DateTime? mEndDate;");
				sb.AppendLine();
				sb.AppendLine("		#region Serialization");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by their created date.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected " + _currentComponent.PascalName + "SelectByCreatedDateRange(SerializationInfo info, StreamingContext context): base(info,context)");
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
				sb.AppendLine("		public " + _currentComponent.PascalName + "SelectByCreatedDateRange(DateTime? startDate, DateTime? endDate)");
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
				sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
				sb.AppendLine("			return colDomain;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentComponent.PascalName + "' collection in a paged fashion");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override string StoredProcedureName");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return \"gen_" + _currentComponent.PascalName + "SelectByCreatedDateRange\"; }");
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

			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendLine("	#region " + _currentComponent.PascalName + "SelectByModifiedDateRange");
				sb.AppendLine();
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// Select objects by their modified date.");
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	public partial class " + _currentComponent.PascalName + "SelectByModifiedDateRange : SelectCommand, ISerializable");
				sb.AppendLine("	{");
				sb.AppendLine("		private DateTime? mStartDate;");
				sb.AppendLine("		private DateTime? mEndDate;");
				sb.AppendLine();
				sb.AppendLine("		#region Serialization");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by their modified date.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected " + _currentComponent.PascalName + "SelectByModifiedDateRange(SerializationInfo info, StreamingContext context): base(info,context)");
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
				sb.AppendLine("		public " + _currentComponent.PascalName + "SelectByModifiedDateRange(DateTime? startDate, DateTime? endDate)");
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
				sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
				sb.AppendLine("			return colDomain;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentComponent.PascalName + "' collection in a paged fashion");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override string StoredProcedureName");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return \"gen_" + _currentComponent.PascalName + "SelectByModifiedDateRange\"; }");
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

		#endregion

		#region AppendRegionSelectByForeignKeys

		private void AppendRegionSelectByForeignKeys()
		{
			foreach (Relation relation in _currentComponent.Parent.ChildRoleRelations.Where(x => x.IsGenerated))
			{
				if (!relation.IsPrimaryKeyRelation())
				{
					#region Code for select parent by relation (non-primary key)
					string objectName = _currentComponent.PascalName + "SelectBy" + relation.PascalRoleName + ((Table)relation.ParentTableRef.Object).PascalName + "ParentRelation";
					sb.AppendLine("	#region " + objectName);
					sb.AppendLine();
					sb.AppendLine("	/// <summary>");
					sb.AppendLine("	/// Select command used to select all rows from the '" + _currentComponent.PascalName + "' table based on the parent relation");
					sb.AppendLine("	/// </summary>");
					sb.AppendLine("	[Serializable]");
					sb.AppendLine("	public partial class " + objectName + " : SelectCommand, ISerializable");
					sb.AppendLine("	{");
					sb.AppendLine("		private string m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "KeysXml = null;");
					if (_currentComponent.Parent.SelfReference)
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
					if (_currentComponent.Parent.SelfReference)
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
					if (_currentComponent.Parent.SelfReference)
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
					if (_currentComponent.Parent.SelfReference)
					{
						sb.AppendLine("			mDirection = RecurseDirection.NONE;");
						sb.AppendLine("			mLevels = -1;");

					}
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Constructor that takes a list of primary keys to select");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public " + objectName + "(ArrayList in" + relation.PascalRoleName + _currentComponent.PascalName + ((Table)relation.ChildTableRef.Object).PascalName + relation.RoleName + "RelationKeys)");
					sb.AppendLine("		{");
					sb.AppendLine("			m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "KeysXml = Domain" + ((Table)relation.ParentTableRef.Object).PascalName + "Collection.Get" + _currentComponent.PascalName + "RelationKeyXml(in" + relation.PascalRoleName + _currentComponent.PascalName + ((Table)relation.ChildTableRef.Object).PascalName + relation.RoleName + "RelationKeys);");
					if (_currentComponent.Parent.SelfReference)
					{
						sb.AppendLine("			mDirection = RecurseDirection.NONE;");
						sb.AppendLine("			mLevels = -1;");
					}
					sb.AppendLine("		}");
					sb.AppendLine();
					if (_currentComponent.Parent.SelfReference)
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
					sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
					sb.AppendLine("			return colDomain;");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The stored procedure name to select values by a foreign key to populate a '" + _currentComponent.PascalName + "' collection");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public override string StoredProcedureName");
					sb.AppendLine("		{");
					sb.AppendLine("			get { return \"gen_" + _currentComponent.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks\"; }");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Initializes the parameters for this select command");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
					sb.AppendLine("		{");
					sb.AppendLine("			SubDomain sd = (SubDomain)subDomain;");
					sb.AppendLine("			if(m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "KeysXml != string.Empty)");
					sb.AppendLine("			{");
					sb.AppendLine("				if (!ParameterValues.ContainsKey(\"@xml\"))");
					sb.AppendLine("					ParameterValues.Add(\"@xml\", m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "KeysXml);");
					sb.AppendLine("			}");
					sb.AppendLine("			else");
					sb.AppendLine("			{");
					sb.AppendLine("				Domain" + ((Table)relation.ParentTableRef.Object).PascalName + "Collection ic = (Domain" + ((Table)relation.ParentTableRef.Object).PascalName + "Collection)sd.GetDomainCollection(Collections." + ((Table)relation.ParentTableRef.Object).PascalName + "Collection);");
					sb.AppendLine("				ic." + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled = true;");
					sb.AppendLine("				if (!ParameterValues.ContainsKey(\"@xml\"))");
					sb.AppendLine("					ParameterValues.Add(\"@xml\", ic.Get" + _currentComponent.PascalName + "RelationKeyXml());");
					sb.AppendLine("			}");
					if (_currentComponent.Parent.SelfReference)
					{
						sb.AppendLine("			ParameterValues.Add(\"@direction\",mDirection.ToString());");
						sb.AppendLine("			if(mLevels >= 0)");
						sb.AppendLine("			{");
						sb.AppendLine("				ParameterValues.Add(\"@levels\",mLevels);");
						sb.AppendLine("			}");
						sb.AppendLine("			else");
						sb.AppendLine("			{");
						sb.AppendLine("				ParameterValues.Add(\"@levels\",50);");
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
					#endregion
				}
				else if (((Table)relation.ParentTableRef.Object).Generated && ((Table)relation.ParentTableRef.Object).Key != _currentComponent.Key)
				{
					#region Code for select parent object by primary key
					string objectName = _currentComponent.PascalName + "SelectBy" + relation.PascalRoleName + ((Table)relation.ParentTableRef.Object).PascalName + "Pks";
					sb.AppendLine("	#region " + objectName);
					sb.AppendLine();
					sb.AppendLine("	/// <summary>");
					sb.AppendLine("	/// Select command used to select all rows from the '" + _currentComponent.PascalName + "' table based on primary keys");
					sb.AppendLine("	/// </summary>");
					sb.AppendLine("	[Serializable]");
					sb.AppendLine("	public partial class " + objectName + " : SelectCommand, ISerializable");
					sb.AppendLine("	{");
					sb.AppendLine("		private string m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeysXml = null;");
					if (_currentComponent.Parent.SelfReference)
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
					if (_currentComponent.Parent.SelfReference)
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
					if (_currentComponent.Parent.SelfReference)
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
					if (_currentComponent.Parent.SelfReference)
					{
						sb.AppendLine("			mDirection = RecurseDirection.NONE;");
						sb.AppendLine("			mLevels = -1;");

					}
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Constructor that takes a list of primary keys to select");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public " + objectName + "(ArrayList in" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeys)");
					sb.AppendLine("		{");
					sb.AppendLine("			m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeysXml = Domain" + ((Table)relation.ParentTableRef.Object).PascalName + "Collection.GetPrimaryKeyXml(in" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeys);");
					if (_currentComponent.Parent.SelfReference)
					{
						sb.AppendLine("			mDirection = RecurseDirection.NONE;");
						sb.AppendLine("			mLevels = -1;");

					}
					sb.AppendLine("		}");
					sb.AppendLine();
					if (_currentComponent.Parent.SelfReference)
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
					sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
					sb.AppendLine("			return colDomain;");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The stored procedure name to select values by a foreign key to populate a '" + _currentComponent.PascalName + "' collection");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public override string StoredProcedureName");
					sb.AppendLine("		{");
					sb.AppendLine("			get { return \"gen_" + _currentComponent.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks\"; }");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Initializes the parameters for this select command");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
					sb.AppendLine("		{");
					sb.AppendLine("			SubDomain sd = (SubDomain)subDomain;");
					sb.AppendLine("			if(m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeysXml != null)");
					sb.AppendLine("			{");
					sb.AppendLine("				if (!ParameterValues.ContainsKey(\"@xml\"))");
					sb.AppendLine("					ParameterValues.Add(\"@xml\", m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeysXml);");
					sb.AppendLine("			}");
					sb.AppendLine("			else");
					sb.AppendLine("			{");
					sb.AppendLine("				Domain" + ((Table)relation.ParentTableRef.Object).PascalName + "Collection ic = (Domain" + ((Table)relation.ParentTableRef.Object).PascalName + "Collection)sd.GetDomainCollection(Collections." + ((Table)relation.ParentTableRef.Object).PascalName + "Collection);");
					sb.AppendLine("				ic." + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled = true;");
					sb.AppendLine("				if (!ParameterValues.ContainsKey(\"@xml\"))");
					sb.AppendLine("					ParameterValues.Add(\"@xml\", ic.GetPrimaryKeyXml());");
					sb.AppendLine("			}");
					if (_currentComponent.Parent.SelfReference)
					{
						sb.AppendLine("			ParameterValues.Add(\"@direction\",mDirection.ToString());");
						sb.AppendLine("			if(mLevels >= 0)");
						sb.AppendLine("			{");
						sb.AppendLine("				ParameterValues.Add(\"@levels\",mLevels);");
						sb.AppendLine("			}");
						sb.AppendLine("			else");
						sb.AppendLine("			{");
						sb.AppendLine("				ParameterValues.Add(\"@levels\",50);");
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
					#endregion
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
			foreach (Relation relation in _currentComponent.Parent.ParentRoleRelations.Where(x => x.IsGenerated))
			{
				//This is NOT a primary key based relationship
				if (!relation.IsPrimaryKeyRelation())
				{
					string objectName = _currentComponent.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "RelationCommand";
					sb.AppendLine();
					sb.AppendLine("	#region " + objectName);
					sb.AppendLine();
					sb.AppendLine("	/// <summary>");
					sb.AppendLine("	/// Select command used to select all rows from the '" + _currentComponent.PascalName + "' table based on the relationship with " + ((Table)relation.ChildTableRef.Object).PascalName);
					sb.AppendLine("	/// </summary>");
					sb.AppendLine("	[Serializable]");
					sb.AppendLine("	public partial class " + objectName + ": SelectCommand, ISerializable");
					sb.AppendLine("	{");

					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						Column relationColumn = ((Column)columnRelationship.ParentColumnRef.Object);
						sb.AppendLine("		private " + relationColumn.GetCodeType(true) + " _" + relationColumn.CamelName + ";");
					}

					if (_currentComponent.Parent.SelfReference)
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
					if (_currentComponent.Parent.SelfReference)
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
					if (_currentComponent.Parent.SelfReference)
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
					if (_currentComponent.Parent.SelfReference)
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
					if (_currentComponent.Parent.SelfReference)
					{
						sb.AppendLine("			mDirection = RecurseDirection.NONE;");
						sb.AppendLine("			mLevels = -1;");
					}
					sb.AppendLine("		}");
					sb.AppendLine();

					if (_currentComponent.Parent.SelfReference)
					{
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// Constructor that takes a the direction in which to search");
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		public " + objectName + "(RecurseDirection direction)");
						sb.AppendLine("		{");
						sb.AppendLine("			mDirection = direction;");
						sb.AppendLine("			mLevels = -1;");
						sb.AppendLine("		}");
						sb.AppendLine();
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// Constructor that takes a the direction in which to search");
						sb.AppendLine("		/// </summary>");
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
					sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
					sb.AppendLine("			return colDomain;");
					sb.AppendLine("		}");
					sb.AppendLine();

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The stored procedure name to select values by a foreign key to populate a '" + _currentComponent.PascalName + "' collection");
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

					if (_currentComponent.Parent.SelfReference)
					{
						sb.AppendLine("			ParameterValues.Add(\"@direction\",mDirection.ToString());");
						sb.AppendLine("			if(mLevels >= 0)");
						sb.AppendLine("			{");
						sb.AppendLine("				ParameterValues.Add(\"@levels\",mLevels);");
						sb.AppendLine("			}");
						sb.AppendLine("			else");
						sb.AppendLine("			{");
						sb.AppendLine("				ParameterValues.Add(\"@levels\",50);");
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
			sb.AppendLine("	#region " + _currentComponent.PascalName + "SelectByPks");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// A select command that selects objects by primary key");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentComponent.PascalName + "SelectByPks : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine("		private List<" + _currentComponent.Parent.PascalName + "PrimaryKey> mPrimaryKeys = null;");
			if (_currentComponent.Parent.SelfReference)
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
			sb.AppendLine("		protected " + _currentComponent.PascalName + "SelectByPks(SerializationInfo info, StreamingContext context): base(info,context)");
			sb.AppendLine("		{");
			sb.AppendLine("			mPrimaryKeys = new List<" + _currentComponent.Parent.PascalName + "PrimaryKey>();");
			if (_currentComponent.Parent.SelfReference)
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
			sb.AppendLine("			info.AddValue(\"mPrimaryKeys\", mPrimaryKeys);");
			if (_currentComponent.Parent.SelfReference)
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
			sb.AppendLine("		public " + _currentComponent.PascalName + "SelectByPks(" + PrimaryKeyParameterList() + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			mPrimaryKeys = new List<" + _currentComponent.Parent.PascalName + "PrimaryKey>();");
			sb.AppendLine("			mPrimaryKeys.Add(new " + _currentComponent.Parent.PascalName + "PrimaryKey(" + PrimaryKeyInputParameterList() + "));");
			sb.AppendLine();
			if (_currentComponent.Parent.SelfReference)
			{
				sb.AppendLine("			mDirection = RecurseDirection.NONE;");
				sb.AppendLine("			mLevels = -1;");

			}
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select objects by a list of primary keys");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentComponent.PascalName + "SelectByPks(IEnumerable<" + _currentComponent.Parent.PascalName + "PrimaryKey> primaryKeys)");
			sb.AppendLine("		{");
			sb.AppendLine("			mPrimaryKeys = new List<" + _currentComponent.Parent.PascalName + "PrimaryKey>();");
			sb.AppendLine("			mPrimaryKeys.AddRange(primaryKeys);");
			if (_currentComponent.Parent.SelfReference)
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
			sb.AppendLine("		public " + _currentComponent.PascalName + "SelectByPks(ArrayList primaryKeys)");
			sb.AppendLine("		{");
			sb.AppendLine("			mPrimaryKeys = new List<" + _currentComponent.Parent.PascalName + "PrimaryKey>();");
			sb.AppendLine("			foreach (" + _currentComponent.Parent.PascalName + "PrimaryKey item in primaryKeys)");
			sb.AppendLine("				mPrimaryKeys.Add(item);");
			if (_currentComponent.Parent.SelfReference)
			{
				sb.AppendLine("			mDirection = RecurseDirection.NONE;");
				sb.AppendLine("			mLevels = -1;");

			}
			sb.AppendLine("		}");
			sb.AppendLine();

			if (_currentComponent.Parent.SelfReference)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by a list of primary keys");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentComponent.PascalName + "SelectByPks(IEnumerable<" + _currentComponent.Parent.PascalName + "PrimaryKey> primaryKeys, RecurseDirection direction)");
				sb.AppendLine("		{");
				sb.AppendLine("			mPrimaryKeys = new List<" + _currentComponent.Parent.PascalName + "PrimaryKey>();");
				sb.AppendLine("			mPrimaryKeys.AddRange(primaryKeys);");
				sb.AppendLine("			mDirection = direction;");
				sb.AppendLine("			mLevels = -1;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by a list of primary keys");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentComponent.PascalName + "SelectByPks(IEnumerable<" + _currentComponent.Parent.PascalName + "PrimaryKey> primaryKeys, RecurseDirection direction, int level)");
				sb.AppendLine("		{");
				sb.AppendLine("			mPrimaryKeys = new List<" + _currentComponent.Parent.PascalName + "PrimaryKey>();");
				sb.AppendLine("			mPrimaryKeys.AddRange(primaryKeys);");
				sb.AppendLine("			mDirection = direction;");
				sb.AppendLine("			mLevels = level;");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by a list of primary keys");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[Obsolete(\"This method has been deprecated. Please use another overload.\")]");
				sb.AppendLine("		public " + _currentComponent.PascalName + "SelectByPks(ArrayList primaryKeys, RecurseDirection direction)");
				sb.AppendLine("		{");
				sb.AppendLine("			mPrimaryKeys = new List<" + _currentComponent.Parent.PascalName + "PrimaryKey>();");
				sb.AppendLine("			foreach (" + _currentComponent.Parent.PascalName + "PrimaryKey item in primaryKeys)");
				sb.AppendLine("				mPrimaryKeys.Add(item);");
				sb.AppendLine("			mDirection = direction;");
				sb.AppendLine("			mLevels = -1;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by a list of primary keys");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[Obsolete(\"This method has been deprecated. Please use another overload.\")]");
				sb.AppendLine("		public " + _currentComponent.PascalName + "SelectByPks(ArrayList primaryKeys, RecurseDirection direction, int level)");
				sb.AppendLine("		{");
				sb.AppendLine("			mPrimaryKeys = new List<" + _currentComponent.Parent.PascalName + "PrimaryKey>();");
				sb.AppendLine("			foreach (" + _currentComponent.Parent.PascalName + "PrimaryKey item in primaryKeys)");
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
			sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values by primary key to populate a '" + _currentComponent.PascalName + "' collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				if (mPrimaryKeys.Count == 1) return \"gen_" + _currentComponent.PascalName + "SelectBy" + _currentComponent.PascalName + "SinglePk\";");
			sb.AppendLine("				else return \"gen_" + _currentComponent.PascalName + "SelectBy" + _currentComponent.PascalName + "Pks\";");
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
			sb.AppendLine("				" + _currentComponent.Parent.PascalName + "PrimaryKey key = ((" + _currentComponent.Parent.PascalName + "PrimaryKey)mPrimaryKeys[0]);");
			foreach (Column c in _currentComponent.Parent.PrimaryKeyColumns)
			{
				sb.AppendLine("				ParameterValues.Add(\"@" + c.DatabaseName + "\", key." + c.PascalName + ");");
			}
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("				ParameterValues.Add(\"@xml\", Domain" + _currentComponent.PascalName + "Collection.GetPrimaryKeyXml(new ArrayList(mPrimaryKeys)));");
			if (_currentComponent.Parent.SelfReference)
			{
				sb.AppendLine("			ParameterValues.Add(\"@direction\",mDirection.ToString());");
				sb.AppendLine("			if(mLevels >= 0)");
				sb.AppendLine("			{");
				sb.AppendLine("				ParameterValues.Add(\"@levels\",mLevels);");
				sb.AppendLine("			}");
				sb.AppendLine("			else");
				sb.AppendLine("			{");
				sb.AppendLine("				ParameterValues.Add(\"@levels\",50);");
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
			sb.AppendLine("	#region " + _currentComponent.PascalName + "SelectAll");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// The select command used to select all rows from the '" + _currentComponent.PascalName + "' table");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentComponent.PascalName + "SelectAll : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine();
			sb.AppendLine("		#region Serialization");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected " + _currentComponent.PascalName + "SelectAll(SerializationInfo info, StreamingContext context): base(info,context)");
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
			sb.AppendLine("		public " + _currentComponent.PascalName + "SelectAll()");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a persistable domainCollection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentComponent.PascalName + "' collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + _currentComponent.PascalName + "Select\"; }");
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
			sb.AppendLine("	#region " + _currentComponent.PascalName + "PagedSelect");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// Select command used to perform paged selects from the '" + _currentComponent.PascalName + "' table");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentComponent.PascalName + "PagedSelect : SelectCommand, ISerializable");
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
			sb.AppendLine("		protected " + _currentComponent.PascalName + "PagedSelect(SerializationInfo info, StreamingContext context): base(info,context)");
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
			sb.AppendLine("		public " + _currentComponent.PascalName + "PagedSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter)");
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
			sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentComponent.PascalName + "' collection in a paged fashion");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + _currentComponent.PascalName + "PagingSelect\"; }");
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
			//Make commands for searchable fields
			foreach (Reference reference in _currentComponent.Columns)
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

		}

		#endregion

		#region AppendRegionSearchableSingleFieldRange

		private void AppendRegionSearchableSingleFieldRange(Column column)
		{
			sb.AppendLine("	#region " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "Range");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// Select command used to perform selects for a range by field '" + column.PascalName + "'");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "Range : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine("		private " + column.GetCodeType() + " m" + column.PascalName + "Start; ");
			sb.AppendLine("		private " + column.GetCodeType() + " m" + column.PascalName + "End; ");
			sb.AppendLine("		private " + _currentComponent.PascalName + "Paging _paging = null;");
			sb.AppendLine("		private int mCount = -1;");
			sb.AppendLine();
			sb.AppendLine("		#region Serialization");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "Range(SerializationInfo info, StreamingContext context): base(info,context)");
			sb.AppendLine("		{");
			sb.AppendLine("			m" + column.PascalName + "Start = info." + Globals.GetSerializationMethod(column.DataType) + "(\"m" + column.PascalName + "Start\");");
			sb.AppendLine("			m" + column.PascalName + "End = info." + Globals.GetSerializationMethod(column.DataType) + "(\"m" + column.PascalName + "End\");");
			sb.AppendLine("			_paging = (" + _currentComponent.PascalName + "Paging)info.GetValue(\"paging\", typeof(" + _currentComponent.PascalName + "Paging));");
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
			sb.AppendLine("		public " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "Range(" + column.GetCodeType() + " " + column.CamelName + "Start, " + column.GetCodeType() + " " + column.CamelName + "End)");
			sb.AppendLine("		{");
			sb.AppendLine("			m" + column.PascalName + "Start = " + column.CamelName + "Start; ");
			sb.AppendLine("			m" + column.PascalName + "End = " + column.CamelName + "End; ");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select command that allows for the paged selection by field of database rows");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "Range(" + column.GetCodeType() + " " + column.CamelName + "Start, " + column.GetCodeType() + " " + column.CamelName + "End, " + _currentComponent.PascalName + "Paging paging)");
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
			sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentComponent.PascalName + "' collection in a paged fashion");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + _currentComponent.PascalName + "SelectBy" + column.PascalName + "Range\"; }");
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
			sb.AppendLine("				ParameterValues.Add(\"@paging_OrderByColumn\", " + _currentComponent.PascalName + "Collection.GetDatabaseFieldName(_paging.OrderByList[0].Field));");
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
			sb.AppendLine("	#region " + _currentComponent.PascalName + "SelectBy" + column.PascalName);
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// Select command used to perform selects by field '" + column.PascalName + "'");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentComponent.PascalName + "SelectBy" + column.PascalName + " : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine("		private " + column.GetCodeType() + " m" + column.PascalName + "; ");
			sb.AppendLine("		private " + _currentComponent.PascalName + "Paging _paging = null;");
			sb.AppendLine("		private int mCount = -1;");
			sb.AppendLine();
			sb.AppendLine("		#region Serialization");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "(SerializationInfo info, StreamingContext context): base(info,context)");
			sb.AppendLine("		{");
			if (column.DataType == System.Data.SqlDbType.UniqueIdentifier)
				sb.AppendLine("			m" + column.PascalName + " = new Guid(info." + Globals.GetSerializationMethod(column.DataType) + "(\"m" + column.PascalName + "\"));");
			else
				sb.AppendLine("			m" + column.PascalName + " = info." + Globals.GetSerializationMethod(column.DataType) + "(\"m" + column.PascalName + "\");");
			sb.AppendLine("			_paging = (" + _currentComponent.PascalName + "Paging)info.GetValue(\"paging\", typeof(" + _currentComponent.PascalName + "Paging));");
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
			sb.AppendLine("		public " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "(" + column.GetCodeType() + " " + column.CamelName + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			m" + column.PascalName + " = " + column.CamelName + "; ");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select command that allows for the paged selection by field of database rows");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "(" + column.GetCodeType() + " " + column.CamelName + ", " + _currentComponent.PascalName + "Paging paging)");
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
			sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentComponent.PascalName + "' collection in a paged fashion");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + _currentComponent.PascalName + "SelectBy" + column.PascalName + "\"; }");
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
			sb.AppendLine("				ParameterValues.Add(\"@paging_OrderByColumn\", " + _currentComponent.PascalName + "Collection.GetDatabaseFieldName(_paging.OrderByList[0].Field));");
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

		#region AppendSearchClass

		private void AppendSearchClass()
		{
			ArrayList validColumns = new ArrayList();
			foreach (Reference reference in _currentComponent.Columns)
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
				sb.AppendLine("	#region " + _currentComponent.PascalName + "SelectBySearch");
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// The select command used to select all rows from the '" + _currentComponent.PascalName + "' table");
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	public partial class " + _currentComponent.PascalName + "SelectBySearch : SelectCommand, ISerializable");
				sb.AppendLine("	{");
				sb.AppendLine("		private " + _currentComponent.PascalName + "Search " + _currentComponent.PascalName + ";");
				sb.AppendLine("		private " + _currentComponent.PascalName + "Paging _paging = null;");
				sb.AppendLine("		private int mCount = -1;");
				sb.AppendLine();
				sb.AppendLine("		#region Serialization");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Serialization constructor");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected " + _currentComponent.PascalName + "SelectBySearch(SerializationInfo info, StreamingContext context)");
				sb.AppendLine("			: base(info, context)");
				sb.AppendLine("		{");
				sb.AppendLine("			" + _currentComponent.PascalName + " = (" + _currentComponent.PascalName + "Search)info.GetValue(\"" + _currentComponent.PascalName + "\", typeof(" + _currentComponent.PascalName + "Search));");
				sb.AppendLine("			_paging = (" + _currentComponent.PascalName + "Paging)info.GetValue(\"paging\", typeof(" + _currentComponent.PascalName + "Paging));");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Method used internally for serialization");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
				sb.AppendLine("		{");
				sb.AppendLine("			base.GetObjectData(info, context);");
				sb.AppendLine("			info.AddValue(\"" + _currentComponent.PascalName + "\", " + _currentComponent.PascalName + ");");
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
				sb.AppendLine("		/// Select of objects by the " + _currentComponent.PascalName + " search object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentComponent.PascalName + "SelectBySearch(" + _currentComponent.PascalName + "Search searchObject)");
				sb.AppendLine("		{");
				sb.AppendLine("			" + _currentComponent.PascalName + " = searchObject;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Paged select of objects by the " + _currentComponent.PascalName + " search object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentComponent.PascalName + "SelectBySearch(" + _currentComponent.PascalName + "Search searchObject, " + _currentComponent.PascalName + "Paging paging)");
				sb.AppendLine("		{");
				sb.AppendLine("			" + _currentComponent.PascalName + " = searchObject;");
				sb.AppendLine("			_paging = paging;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Creates a persistable domainCollection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection colDomain = new Domain" + _currentComponent.PascalName + "Collection();");
				sb.AppendLine("			return colDomain;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The stored procedure name to select values by primary key to populate a '" + _currentComponent.PascalName + "' collection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override string StoredProcedureName");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				if (" + _currentComponent.PascalName + ".SearchType == SearchType.OR)");
				sb.AppendLine("					return \"gen_" + _currentComponent.PascalName + "SelectBySearchOr\";");
				sb.AppendLine("				else");
				sb.AppendLine("					return \"gen_" + _currentComponent.PascalName + "SelectBySearchAnd\";");
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
				sb.AppendLine("				ParameterValues.Add(\"@paging_OrderByColumn\", " + _currentComponent.PascalName + "Collection.GetDatabaseFieldName(_paging.OrderByList[0].Field));");
				sb.AppendLine("				ParameterValues.Add(\"@paging_PageIndex\", _paging.PageIndex);");
				sb.AppendLine("				ParameterValues.Add(\"@paging_RecordsperPage\", _paging.RecordsperPage);");
				sb.AppendLine("			}");
				sb.AppendLine();
				sb.AppendLine("			if (" + _currentComponent.PascalName + " != null)");
				sb.AppendLine("			{");
				foreach (Column dc in validColumns)
				{
					sb.AppendLine("				if (" + _currentComponent.PascalName + "." + dc.PascalName + " != null)");
					sb.AppendLine("				{");
					sb.AppendLine("					ParameterValues.Add(\"@" + dc.DatabaseName + "\", " + _currentComponent.PascalName + "." + dc.PascalName + ");");
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
			sb.AppendLine("		private Domain" + _currentComponent.PascalName + "Collection col" + _currentComponent.PascalName + "List;");
		}
		#endregion

		#region append constructors
		public void AppendConstructor()
		{
			sb.AppendLine("		internal " + _currentComponent.PascalName + "CollectionRules(Domain" + _currentComponent.PascalName + "Collection in" + _currentComponent.PascalName + "List)");
			sb.AppendLine("		{");
			sb.AppendLine("			col" + _currentComponent.PascalName + "List = in" + _currentComponent.PascalName + "List;");
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
				foreach (Reference reference in _currentComponent.Columns)
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
				throw new Exception(_currentComponent.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}

		protected string PrimaryKeyInputParameterList()
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Reference reference in _currentComponent.Columns)
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
				throw new Exception(_currentComponent.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}
		#endregion

	}
}