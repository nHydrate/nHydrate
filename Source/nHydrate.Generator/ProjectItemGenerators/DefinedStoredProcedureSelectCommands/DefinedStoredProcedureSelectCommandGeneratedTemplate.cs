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
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.DefinedStoredProcedureSelectCommand
{
	class DefinedStoredProcedureSelectCommandGeneratedTemplate : BaseClassTemplate
	{

		private StringBuilder sb = new StringBuilder();
		private CustomStoredProcedure _currentStoredProcedure;

		public DefinedStoredProcedureSelectCommandGeneratedTemplate(ModelRoot model, CustomStoredProcedure currentStoredProcedure)
		{
			_model = model;
			_currentStoredProcedure = currentStoredProcedure;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("{0}.Generated.cs", _currentStoredProcedure.PascalName); }
		}

		public string ParentItemName
		{
			get
			{
				return string.Format("{0}.cs", _currentStoredProcedure.PascalName);
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
			sb.AppendLine("using " + DefaultNamespace + ".Business.StoredProcedures;");
			sb.AppendLine("using " + DefaultNamespace + ".Domain.StoredProcedures;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			this.AppendFullTemplate();
		}

		#endregion

		#region append regions

		private void AppendFullTemplate()
		{
			sb.AppendLine("	#region " + _currentStoredProcedure.PascalName + "Select");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The select command used to select all rows from the '" + _currentStoredProcedure.PascalName + "' object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public partial class " + _currentStoredProcedure.PascalName + "Select : SelectCommand, ISerializable");
			sb.AppendLine("	{");
			sb.AppendLine();

			sb.AppendLine("		//Define custom parameters");
			foreach (Reference reference in _currentStoredProcedure.Parameters)
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
			sb.AppendLine("		protected " + _currentStoredProcedure.PascalName + "Select(SerializationInfo info, StreamingContext context): base(info,context)");
			sb.AppendLine("		{");

			//Serialize the parameters
			foreach (Reference reference in _currentStoredProcedure.Parameters)
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

			foreach (Reference reference in _currentStoredProcedure.Parameters)
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
			sb.AppendLine("		public " + _currentStoredProcedure.PascalName + "Select()");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();

			int inputParamCount = 0;
			foreach (Reference reference in _currentStoredProcedure.Parameters)
			{
				Parameter parameter = (Parameter)reference.Object;
				if (!parameter.IsOutputParameter) inputParamCount++;
			}

			//Overloaded constructor with parameters
			if (inputParamCount > 0)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Parameterized constructor");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentStoredProcedure.PascalName + "Select(" + this.GetParameterList(true) + ")");
				sb.AppendLine("		{");

				//Serialize the parameters
				foreach (Reference reference in _currentStoredProcedure.Parameters)
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
			sb.AppendLine("			Domain" + _currentStoredProcedure.PascalName + "Collection colDomain = new Domain" + _currentStoredProcedure.PascalName + "Collection();");
			sb.AppendLine("			return colDomain;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentStoredProcedure.PascalName + "' collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override string StoredProcedureName");
			sb.AppendLine("		{ ");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentStoredProcedure) + "Select\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine();
			this.AppendRegionAbstactOverrides();
			this.AppendRegionProperties();
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();



			//sb.AppendLine("	#region " + _currentStoredProcedure.PascalName + "SelectAll" );
			//sb.AppendLine();
			//sb.AppendLine("		/// <summary>" );
			//sb.AppendLine("		/// The select command used to select all rows from the '" + _currentStoredProcedure.PascalName + "' table" );
			//sb.AppendLine("		/// </summary>" );
			//sb.AppendLine("	[Serializable]" );
			//sb.AppendLine("	public partial class " + _currentStoredProcedure.PascalName + "SelectAll : SelectCommand, ISerializable" );
			//sb.AppendLine("	{" );
			//sb.AppendLine();
			//sb.AppendLine("		#region Serialization" );
			//sb.AppendLine();
			//sb.AppendLine("		/// <summary>" );
			//sb.AppendLine("		/// Serialization constructor" );
			//sb.AppendLine("		/// </summary>" );
			//sb.AppendLine("		protected " + _currentStoredProcedure.PascalName + "SelectAll(SerializationInfo info, StreamingContext context): base(info,context)" );
			//sb.AppendLine("		{" );
			//sb.AppendLine("		}" );
			//sb.AppendLine();
			//sb.AppendLine("		/// <summary>" );
			//sb.AppendLine("		/// Method used internally for serialization" );
			//sb.AppendLine("		/// </summary>" );
			//sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)" );
			//sb.AppendLine("		{" );
			//sb.AppendLine("			base.GetObjectData(info, context);" );
			//sb.AppendLine("		}" );
			//sb.AppendLine();
			//sb.AppendLine("		#endregion " );
			//sb.AppendLine();
			//sb.AppendLine("		/// <summary>" );
			//sb.AppendLine("		/// Default constructor" );
			//sb.AppendLine("		/// </summary>" );
			//sb.AppendLine("		public " + _currentStoredProcedure.PascalName + "SelectAll()" );
			//sb.AppendLine("		{" );
			//sb.AppendLine("		}" );
			//sb.AppendLine();
			//sb.AppendLine("		/// <summary>" );
			//sb.AppendLine("		/// Creates a persistable domainCollection" );
			//sb.AppendLine("		/// </summary>" );
			//sb.AppendLine("		public override IDomainCollection CreateDomainCollection()" );
			//sb.AppendLine("		{" );
			//sb.AppendLine("			Domain" + _currentStoredProcedure.PascalName + "Collection colDomain = new Domain" + _currentStoredProcedure.PascalName + "Collection();" );
			//sb.AppendLine("			return colDomain;" );
			//sb.AppendLine("		}" );
			//sb.AppendLine();
			//sb.AppendLine("		/// <summary>" );
			//sb.AppendLine("		/// The stored procedure name to select values to populate a '" + _currentStoredProcedure.PascalName + "' collection" );
			//sb.AppendLine("		/// </summary>" );
			//sb.AppendLine("		public override string StoredProcedureName" );
			//sb.AppendLine("		{" );
			//sb.AppendLine("			get { return \"gen_" + _currentStoredProcedure.PascalName + "Select\"; }" );
			//sb.AppendLine("		}" );
			//sb.AppendLine();
			//sb.AppendLine("		/// <summary>" );
			//sb.AppendLine("		/// Initializes the parameters for this select command" );
			//sb.AppendLine("		/// </summary>" );
			//sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)" );
			//sb.AppendLine("		{" );
			//sb.AppendLine("		}" );
			//sb.AppendLine();
			//sb.AppendLine("		#region SelectCommand Abstact Override" );
			//sb.AppendLine();
			//sb.AppendLine("		/// <summary>" );
			//sb.AppendLine("		/// The connection string used to connect to the database" );
			//sb.AppendLine("		/// </summary>" );
			//sb.AppendLine("		public override string ConnectionString" );
			//sb.AppendLine("		{" );
			//sb.AppendLine("			get { return ConfigurationValues.GetInstance().ConnectionString; }");
			//sb.AppendLine("		}" );
			//sb.AppendLine();
			//sb.AppendLine("		#endregion" );
			//sb.AppendLine();
			//sb.AppendLine("	}" );
			//sb.AppendLine();
			//sb.AppendLine("	#endregion" );
			//sb.AppendLine();

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
			foreach (Reference reference in _currentStoredProcedure.Parameters)
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
			foreach (Reference reference in _currentStoredProcedure.Parameters)
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
					string defaultValue = "";
					if (ModelHelper.IsTextType(parameter.DataType))
						defaultValue = "\"" + parameter.Default + "\"";
					else
						defaultValue = parameter.Default;

					sb.AppendLine("				m" + parameter.PascalName + " = " + defaultValue + ";");
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
			sb.AppendLine("		private Domain" + _currentStoredProcedure.PascalName + "Collection col" + _currentStoredProcedure.PascalName + "List;");
		}

		#endregion

		#region append constructors

		public void AppendConstructor()
		{
			sb.AppendLine("		internal " + _currentStoredProcedure.PascalName + "CollectionStoredProcedures(Domain" + _currentStoredProcedure.PascalName + "Collection in" + _currentStoredProcedure.PascalName + "List)");
			sb.AppendLine("		{");
			sb.AppendLine("			col" + _currentStoredProcedure.PascalName + "List = in" + _currentStoredProcedure.PascalName + "List;");
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

			foreach (Reference reference in _currentStoredProcedure.Parameters)
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
				foreach (Reference reference in _currentStoredProcedure.Parameters)
				{
					Parameter parameter = (Parameter)reference.Object;
					if (!parameter.IsOutputParameter)
						al.Add(parameter);
				}

				int ii = 0;
				foreach (Parameter parameter in al)
				{
					if (useDataTypes)
						retval.Append(parameter.GetCodeType() + " ");
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