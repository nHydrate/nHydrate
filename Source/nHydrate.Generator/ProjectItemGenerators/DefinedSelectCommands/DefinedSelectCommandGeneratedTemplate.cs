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

namespace Widgetsphere.Generator.ProjectItemGenerators.DefinedSelectCommand
{
	class DefinedSelectCommandGeneratedTemplate : BaseClassTemplate
	{

		private StringBuilder sb = new StringBuilder();
		private CustomRetrieveRule _currentRule;
		private Table ParentTable = null;

		public DefinedSelectCommandGeneratedTemplate(ModelRoot model, CustomRetrieveRule currentRule)
		{
			_model = model;
			_currentRule = currentRule;
			this.ParentTable = (Table)currentRule.ParentTableRef.Object;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("{0}SelectBy{1}.Generated.cs", this.ParentTable.PascalName, _currentRule.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}SelectBy{1}.cs", this.ParentTable.PascalName, _currentRule.PascalName); }
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

		private void AppendFullTemplate()
		{
			try
			{
				sb.AppendLine("	#region " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName);
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The select command used to select specified rows from the '" + this.ParentTable.PascalName + "' table");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	public partial class " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + " : SelectCommand, ISerializable");
				sb.AppendLine("	{");
				sb.AppendLine();

				sb.AppendLine("		//Define custom parameters");
				foreach (Reference reference in _currentRule.Parameters)
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
				sb.AppendLine("		protected " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "(SerializationInfo info, StreamingContext context): base(info,context)");
				sb.AppendLine("		{");

				//Serialize the parameters
				foreach (Reference reference in _currentRule.Parameters)
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

				foreach (Reference reference in _currentRule.Parameters)
				{
					Parameter parameter = (Parameter)reference.Object;
					sb.AppendLine("			info.AddValue(\"m" + parameter.PascalName + "\", m" + parameter.PascalName + ");");
				}

				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion ");
				sb.AppendLine();

				sb.AppendLine("		public " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "(" + this.ParameterList() + ")");
				sb.AppendLine("		{");
				foreach (Reference reference in _currentRule.Parameters)
				{
					Parameter parameter = (Parameter)reference.Object;
					sb.AppendLine("		  m" + parameter.PascalName + " = " + parameter.CamelName + ";");
				}
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Creates a persistable domainCollection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override IDomainCollection CreateDomainCollection()");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + ParentTable.PascalName + "Collection colDomain = new Domain" + ParentTable.PascalName + "Collection();");
				sb.AppendLine("			return colDomain;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The stored procedure name to select values to populate a '" + this.ParentTable.PascalName + "' collection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override string StoredProcedureName");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, this.ParentTable) + "CustomSelectBy" + _currentRule.PascalName + "\"; }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Initializes the parameters for this select command");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected override void SetupParameterValues(SubDomainBase subDomain)");
				sb.AppendLine("		{");

				foreach (Reference reference in _currentRule.Parameters)
				{
					Parameter parameter = (Parameter)reference.Object;
					sb.AppendLine("			ParameterValues.Add(\"@" + parameter.Name + "\", m" + parameter.PascalName + ");");
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

				foreach (Relation relation in this.ParentTable.ChildRoleRelations.Where(x => x.IsGenerated))
				{
					if (!relation.IsPrimaryKeyRelation())
					{
						#region Code for select parent by relation (non-primary key)
						sb.AppendLine("TODO: ADD CODE HERE");
						#endregion
					}
					else if (((Table)relation.ParentTableRef.Object).Generated && ((Table)relation.ParentTableRef.Object).Key != this.ParentTable.Key)
					{
						#region Code for select parent object by primary key
						sb.AppendLine("	#region " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks");
						sb.AppendLine();
						sb.AppendLine("	/// <summary>");
						sb.AppendLine("	/// Select command used to select specified rows from the '" + this.ParentTable.PascalName + "' table based on primary keys");
						sb.AppendLine("	/// </summary>");
						sb.AppendLine("	[Serializable]");
						sb.AppendLine("	public partial class " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks : SelectCommand, ISerializable");
						sb.AppendLine("	{");
						sb.AppendLine("		private string m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeysXml = null;");
						if (this.ParentTable.SelfReference)
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
						sb.AppendLine("		protected " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks(SerializationInfo info, StreamingContext context): base(info,context)");
						sb.AppendLine("		{");
						if (this.ParentTable.SelfReference)
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
						if (this.ParentTable.SelfReference)
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
						sb.AppendLine("		public " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks()");
						sb.AppendLine("		{");
						if (this.ParentTable.SelfReference)
						{
							sb.AppendLine("			mDirection = RecurseDirection.NONE;");
							sb.AppendLine("			mLevels = -1;");
						}
						sb.AppendLine("		}");
						sb.AppendLine();

						//sb.AppendLine("		/// <summary>");
						//sb.AppendLine("		/// Constructor that takes a list of primary keys to select");
						//sb.AppendLine("		/// </summary>");
						//sb.AppendLine("		public " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks(IEnumerable<"+_currentRule.PascalName+"PrimaryKey> in" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeys)");
						//sb.AppendLine("		{");
						//sb.AppendLine("			m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeysXml = Domain" + ((Table)relation.ParentTableRef.Object).PascalName + "Collection.GetPrimaryKeyXml(in" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeys);");
						//if (this.ParentTable.SelfReference)
						//{
						//  sb.AppendLine("			mDirection = RecurseDirection.NONE;");
						//  sb.AppendLine("			mLevels = -1;");
						//}
						//sb.AppendLine("		}");
						//sb.AppendLine();

						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// Constructor that takes a list of primary keys to select");
						sb.AppendLine("		/// </summary>");
						//sb.AppendLine("		[Obsolete(\"This method has been deprecated. Please use another overload.\")]");
						sb.AppendLine("		public " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks(ArrayList in" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeys)");
						sb.AppendLine("		{");
						sb.AppendLine("			m" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeysXml = Domain" + ((Table)relation.ParentTableRef.Object).PascalName + "Collection.GetPrimaryKeyXml(in" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeys);");
						if (this.ParentTable.SelfReference)
						{
							sb.AppendLine("			mDirection = RecurseDirection.NONE;");
							sb.AppendLine("			mLevels = -1;");
						}
						sb.AppendLine("		}");
						sb.AppendLine();

						if (this.ParentTable.SelfReference)
						{							
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Constructor that takes a the direction in which to search");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks(RecurseDirection direction)");
							sb.AppendLine("		{");
							sb.AppendLine("			mDirection = direction;");
							sb.AppendLine("			mLevels = -1;");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Constructor that takes a the direction in which to search");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks(RecurseDirection direction, int level)");
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
						sb.AppendLine("			Domain" + ParentTable.PascalName + "Collection colDomain = new Domain" + ParentTable.PascalName + "Collection();");
						sb.AppendLine("			return colDomain;");
						sb.AppendLine("		}");
						sb.AppendLine();
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// The stored procedure name to select values by a foreign key to populate a '" + this.ParentTable.PascalName + "' collection");
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		public override string StoredProcedureName");
						sb.AppendLine("		{");
						sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, this.ParentTable) + "CustomSelectBy" + _currentRule.PascalName + "SelectBy" + relation.PascalRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "Pks\"; }");
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
						if (this.ParentTable.SelfReference)
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
			catch (Exception ex)
			{
				throw;
			}

		}
		#endregion

		#region append member variables
		public void AppendMemberVariables()
		{
			sb.AppendLine("		private Domain" + ParentTable.PascalName + "Collection col" + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "List;");
		}
		#endregion

		#region append constructors
		public void AppendConstructor()
		{
			sb.AppendLine("		internal " + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "CollectionRules(Domain" + ParentTable.PascalName + "Collection in" + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "List)");
			sb.AppendLine("		{");
			sb.AppendLine("			col" + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "List = in" + this.ParentTable.PascalName + "CustomSelectBy" + _currentRule.PascalName + "List;");
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

		protected string ParameterList()
		{
			StringBuilder output = new StringBuilder();
			try
			{
				int ii = 0;
				foreach (Reference reference in _currentRule.Parameters)
				{
					Parameter parameter = (Parameter)reference.Object;
					output.Append(parameter.GetCodeType() + " " + parameter.CamelName);
					if (ii < _currentRule.Parameters.Count - 1)
						output.Append(", ");
					ii++;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			return output.ToString();
		}

		#endregion

	}
}