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
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;
using System.Collections;

namespace Widgetsphere.Generator.ProjectItemGenerators.BusinessCollection
{
	class BusinessCollectionGeneratedTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _currentTable;

		public BusinessCollectionGeneratedTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}

		#region BaseClassTemplate overrides

		public override string FileName
		{
			get { return string.Format("{0}Collection.Generated.cs", _currentTable.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}Collection.cs", _currentTable.PascalName); }
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
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.Objects");
				sb.AppendLine("{");
				this.AppendClass();
				this.AppendSearchClass();
				this.AppendPagingClass();
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
			sb.AppendLine("using System.Xml;");
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using System.ComponentModel;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Rules;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.SelectCommands;");
			sb.AppendLine("using " + DefaultNamespace + ".Domain.Objects;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using Widgetsphere.Core.Util;");
			sb.AppendLine("using Widgetsphere.Core.Logging;");
			sb.AppendLine("using Widgetsphere.Core.Exceptions;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.LINQ;");
			sb.AppendLine("using System.IO;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.Linq.Expressions;");
			sb.AppendLine("using System.Data.Linq;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Text;");
			sb.AppendLine("using Widgetsphere.Core.EventArgs;");
			sb.AppendLine("using System.Text.RegularExpressions;");
			sb.AppendLine();
		}

		private void AppendClass()
		{

			string baseClass = "BusinessCollectionPersistableBase";
			string baseInterface = "IPersistableBusinessCollection";

			if (_currentTable.Immutable)
			{
				baseClass = "BusinessCollectionBase";
				baseInterface = "IBusinessCollection";
			}

			if (_currentTable.ParentTable != null)
				baseClass = _currentTable.ParentTable.PascalName + "Collection";

			sb.AppendLine();
			sb.AppendLine("	 /// <summary>");
			sb.AppendLine("	 /// The collection to hold '" + _currentTable.PascalName + "' entities");
			if (_currentTable.Description != "")
				sb.AppendLine("	 /// " + _currentTable.Description);
			sb.AppendLine("	 /// </summary>");
			sb.AppendLine("	 [Serializable()]");
			sb.AppendLine("	 public partial class " + _currentTable.PascalName + "Collection : " + baseClass + ", " + baseInterface + ", IDisposable, IEnumerator, IEnumerable<" + _currentTable.PascalName + ">, IWrappingClass");
			sb.AppendLine("	 {");
			this.AppendMemberVariables();
			this.AppendConstructors();
			this.AppendProperties();
			this.AppendOperatorIndexer();
			this.AppendMethods();
			//this.AppendIListImplementation();
			this.AppendRegionIBusinessCollectionExplicit();
			this.AppendRegionGetFilteredList();
			this.AppendRegionGetDatabaseFieldName();
			this.AppendClassEnumerator();
			this.AppendRegionSearch();
			this.AppendRegionCustomSelectByRules();
			this.AppendRegionSelectByDates();
			this.AppendRegionEnumerator();
			this.AppendInterfaces();

			sb.AppendLine("	}");
			sb.AppendLine();
		}

		private void AppendRegionSearch()
		{
			sb.AppendLine("		#region Search Members");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a search object to query this collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		IBusinessObjectSearch IBusinessCollection.CreateSearchObject(SearchType searchType)");
			sb.AppendLine("		{");
			sb.AppendLine("			return (IBusinessObjectSearch)this.CreateSearchObject(searchType);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a search object to query this collection.");
			sb.AppendLine("		/// </summary>");
			if (_model.Database.AllowZeroTouch)
			{
				if (_currentTable.ParentTable == null)
					sb.AppendLine("		public virtual IBusinessObjectSearch CreateSearchObject(SearchType searchType)");
				else
					sb.AppendLine("		public new IBusinessObjectSearch CreateSearchObject(SearchType searchType)");

				sb.AppendLine("		{");
				sb.AppendLine("			return null;");
				sb.AppendLine("		}");
			}
			else
			{
				if (_currentTable.ParentTable == null)
					sb.AppendLine("		public virtual " + _currentTable.PascalName + "Search CreateSearchObject(SearchType searchType)");
				else
					sb.AppendLine("		public new " + _currentTable.PascalName + "Search CreateSearchObject(SearchType searchType)");

				sb.AppendLine("		{");
				sb.AppendLine("			return new " + _currentTable.PascalName + "Search(searchType);");
				sb.AppendLine("		}");
			}
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionSelectByDates()
		{
			sb.AppendLine("		#region Select By Dates");
			sb.AppendLine();
			if (_currentTable.Generated && _currentTable.AllowCreateAudit)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by their created date.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"startDate\">The inclusive date on which to start the search.</param>");
				sb.AppendLine("		/// <param name=\"endDate\">The inclusive date on which to end the search.</param>");
				sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
				if (_currentTable.ParentTable == null)
					sb.AppendLine("		public static " + _currentTable.PascalName + "Collection SelectByCreatedDateRange(DateTime? startDate, DateTime? endDate, string modifier)");
				else
					sb.AppendLine("		public new static " + _currentTable.PascalName + "Collection SelectByCreatedDateRange(DateTime? startDate, DateTime? endDate, string modifier)");
				sb.AppendLine("		{");
				sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
				sb.AppendLine("			" + _currentTable.PascalName + "SelectByCreatedDateRange selectCommand = new " + _currentTable.PascalName + "SelectByCreatedDateRange(startDate, endDate);");
				sb.AppendLine("			subDomain.AddSelectCommand(selectCommand);");
				sb.AppendLine("			subDomain.RunSelectCommands();");
				sb.AppendLine("			return (" + _currentTable.PascalName + "Collection)subDomain[Collections." + _currentTable.PascalName + "Collection];");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			if (_currentTable.Generated && _currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by their modified date.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"startDate\">The inclusive date on which to start the search.</param>");
				sb.AppendLine("		/// <param name=\"endDate\">The inclusive date on which to end the search.</param>");
				sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
				if (_currentTable.ParentTable == null)
					sb.AppendLine("		public static " + _currentTable.PascalName + "Collection SelectByModifiedDateRange(DateTime? startDate, DateTime? endDate, string modifier)");
				else
					sb.AppendLine("		public new static " + _currentTable.PascalName + "Collection SelectByModifiedDateRange(DateTime? startDate, DateTime? endDate, string modifier)");
				sb.AppendLine("		{");
				sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
				sb.AppendLine("			" + _currentTable.PascalName + "SelectByModifiedDateRange selectCommand = new " + _currentTable.PascalName + "SelectByModifiedDateRange(startDate, endDate);");
				sb.AppendLine("			subDomain.AddSelectCommand(selectCommand);");
				sb.AppendLine("			subDomain.RunSelectCommands();");
				sb.AppendLine("			return (" + _currentTable.PascalName + "Collection)subDomain[Collections." + _currentTable.PascalName + "Collection];");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionCustomSelectByRules()
		{
			if (_model.Database.AllowZeroTouch) return;

			if (_currentTable.CustomRetrieveRules.Count == 0)
				return;

			sb.AppendLine("		#region CustomSelectBy CustomRules");
			sb.AppendLine();

			//Create a method for all custom retrieve rules
			foreach (Reference reference in _currentTable.CustomRetrieveRules)
			{
				CustomRetrieveRule rule = (CustomRetrieveRule)reference.Object;
				if (rule.Generated)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Select rows based on the '" + rule.PascalName + "' custom retrieve rule for the '" + _currentTable.DatabaseName + "' table.");
					sb.AppendLine("		/// </summary>");
					//sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
					sb.AppendLine("		/// <returns></returns>");
					sb.Append("		public static " + _currentTable.PascalName + "Collection SelectBy" + rule.PascalName + "(");
					if (rule.UseSearchObject)
					{
						sb.Append(_currentTable.PascalName + "Search " + _currentTable.CamelName);
						if (rule.Parameters.Count > 0)
							sb.Append(", ");
					}
					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (parameter.IsOutputParameter)
							sb.Append("out ");
						sb.Append(parameter.GetCodeType() + " " + parameter.CamelName + ", ");
					}
					sb.AppendLine("string modifier)");

					sb.AppendLine("		{");
					sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
					sb.Append("			" + _currentTable.PascalName + "CustomSelectBy" + rule.PascalName + " selectCommand = new " + _currentTable.PascalName + "CustomSelectBy" + rule.PascalName + "(");
					if (rule.UseSearchObject)
					{
						sb.Append(_currentTable.CamelName);
						if (rule.Parameters.Count > 0)
							sb.Append(", ");
					}
					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (!parameter.IsOutputParameter)
						{
							sb.Append(parameter.CamelName + ", ");
						}
					}
					if (sb.ToString().EndsWith(", ")) sb.Remove(sb.ToString().Length - 2, 2);
					sb.AppendLine(");");

					sb.AppendLine("			subDomain.AddSelectCommand(selectCommand);");
					sb.AppendLine("			subDomain.RunSelectCommands();");

					for (int ii = 0; ii < rule.Parameters.Count; ii++)
					{
						Parameter parameter = (Parameter)rule.Parameters[ii].Object;
						if (parameter.IsOutputParameter)
						{
							sb.AppendLine("			" + parameter.CamelName + " = selectCommand." + parameter.PascalName + ";");
						}
					}

					sb.AppendLine("			return (" + _currentTable.PascalName + "Collection)subDomain[Collections." + _currentTable.PascalName + "Collection];");
					sb.AppendLine("		}");
					sb.AppendLine();
				}

			}
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendInterfaces()
		{
			sb.AppendLine("		#region IDisposable Members");
			sb.AppendLine();
			sb.AppendLine("		void IDisposable.Dispose()");
			sb.AppendLine("		{");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionEnumerator()
		{
			sb.AppendLine("		#region IEnumerator Members");
			sb.AppendLine();
			sb.AppendLine("		object IEnumerator.Current");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.GetEnumerator().Current; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		bool IEnumerator.MoveNext()");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetEnumerator().MoveNext();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		void IEnumerator.Reset()");
			sb.AppendLine("		{");
			sb.AppendLine("			this.GetEnumerator().Reset();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();

			sb.AppendLine("		#region IEnumerable<" + _currentTable.PascalName + "> Members");
			sb.AppendLine();
			sb.AppendLine("		IEnumerator<" + _currentTable.PascalName + "> IEnumerable<" + _currentTable.PascalName + ">.GetEnumerator()");
			sb.AppendLine("		{");
			sb.AppendLine("			List<" + _currentTable.PascalName + "> retval = new List<" + _currentTable.PascalName + ">();");
			sb.AppendLine("			foreach (" + _currentTable.PascalName + " item in this)");
			sb.AppendLine("			{");
			sb.AppendLine("				retval.Add(item);");
			sb.AppendLine("			}");
			sb.AppendLine("			return retval.GetEnumerator();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();

		}

		private void AppendRegionIBusinessCollectionExplicit()
		{
			if (!_currentTable.Immutable)
			{
				sb.AppendLine("		#region IPersistableBusinessCollection Explicit Members");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Returns an item in this collection by index.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"index\">The zero-based index of the element to get or set. </param>");
				sb.AppendLine("		/// <returns>The element at the specified index.</returns>");
				sb.AppendLine("		IPersistableBusinessObject IPersistableBusinessCollection.this[int index]");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this[index]; }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		void IPersistableBusinessCollection.AddItem(IPersistableBusinessObject newItem)");
				sb.AppendLine("		{");
				sb.AppendLine("			this.AddItem((" + _currentTable.PascalName + ")newItem);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		IPersistableBusinessObject IPersistableBusinessCollection.NewItem()");
				sb.AppendLine("		{");
				sb.AppendLine("			return this.NewItem();");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
			}

			sb.AppendLine("		#region IBusinessCollection Explicit Members");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns an item in this collection by index.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"index\">The zero-based index of the element to get or set. </param>");
			sb.AppendLine("		/// <returns>The element at the specified index.</returns>");
			sb.AppendLine("		IBusinessObject IBusinessCollection.this[int index]");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this[index]; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		Enum IBusinessCollection.Collection");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.Collection; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		Type IBusinessCollection.ContainedType");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.ContainedType; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		SubDomainBase IBusinessCollection.SubDomain");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.SubDomain; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		Type ITyped.GetContainedType()");
			sb.AppendLine("		{");
			sb.AppendLine("		return typeof(" + _currentTable.PascalName + ");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionGetFilteredList()
		{
			sb.AppendLine("		#region GetFilteredList");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns a list filtered on the specified field with the specified value.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		[Obsolete(\"Use a LINQ query for more robust functionality\")]");
			sb.AppendLine("		public BusinessObjectList<" + _currentTable.PascalName + "> GetFilteredList(" + _currentTable.PascalName + ".FieldNameConstants field, object value)");
			sb.AppendLine("		{");
			sb.AppendLine("			BusinessObjectList<" + _currentTable.PascalName + "> retval = new BusinessObjectList<" + _currentTable.PascalName + ">();");
			sb.AppendLine("			if (value == null)");
			sb.AppendLine("				return retval;");
			sb.AppendLine();
			sb.AppendLine("			string fieldName = \"\";");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Reference reference in _currentTable.Columns)
			{
				Column column = (Column)reference.Object;
				sb.AppendLine("				case " + _currentTable.PascalName + ".FieldNameConstants." + column.PascalName + ": fieldName = \"" + column.DatabaseName + "\"; break;");
			}

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("				case " + _currentTable.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ": fieldName = \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "\"; break;");
				sb.AppendLine("				case " + _currentTable.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ": fieldName = \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "\"; break;");
			}

			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("				case " + _currentTable.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ": fieldName = \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "\"; break;");
				sb.AppendLine("				case " + _currentTable.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ": fieldName = \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "\"; break;");
			}

			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("			DataRow[] rows = null;");
			sb.AppendLine("			if (value == null)");
			sb.AppendLine("			{");
			sb.AppendLine("				rows = this.wrappedClass.Select(fieldName + \" IS NULL\");");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				string tick = \"\";");
			sb.AppendLine("				if ((this.wrappedClass.Columns[fieldName].DataType == typeof(string)) || (this.wrappedClass.Columns[fieldName].DataType == typeof(Guid)))");
			sb.AppendLine("					tick = \"'\";");
			sb.AppendLine();
			sb.AppendLine("				rows = this.wrappedClass.Select(fieldName + \" = \" + tick + value.ToString().Replace(\"'\", \"''\") + tick);");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("			foreach (DataRow dr in rows)");
			sb.AppendLine("				retval.Add(new " + _currentTable.PascalName + "((Domain" + _currentTable.PascalName + ")dr));");
			sb.AppendLine();
			sb.AppendLine("			return retval;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionGetDatabaseFieldName()
		{
			sb.AppendLine("		#region GetDatabaseFieldName");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the actual database name of the specified field.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		internal static string GetDatabaseFieldName(" + _currentTable.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Reference reference in _currentTable.GeneratedColumns)
			{
				Column column = (Column)reference.Object;
				if (column.Generated)
					sb.AppendLine("				case " + _currentTable.PascalName + ".FieldNameConstants." + column.PascalName + ": return \"" + column.Name + "\";");
			}

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("				case " + _currentTable.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ": return \"" + _model.Database.CreatedByColumnName + "\";");
				sb.AppendLine("				case " + _currentTable.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ": return \"" + _model.Database.CreatedDateColumnName + "\";");
			}

			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("				case " + _currentTable.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ": return \"" + _model.Database.ModifiedByColumnName + "\";");
				sb.AppendLine("				case " + _currentTable.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ": return \"" + _model.Database.ModifiedDateColumnName + "\";");
			}

			sb.AppendLine("			}");
			sb.AppendLine("			return \"\";");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the actual database name of the specified field.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		internal " + (_currentTable.ParentTable == null ? "" : "new ") + "static string GetDatabaseFieldName(string field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Reference reference in _currentTable.GeneratedColumns)
			{
				Column column = (Column)reference.Object;
				if (column.Generated)
					sb.AppendLine("				case \"" + column.PascalName + "\": return \"" + column.Name + "\";");
			}

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "\": return \"" + _model.Database.CreatedByColumnName + "\";");
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "\": return \"" + _model.Database.CreatedDateColumnName + "\";");
			}

			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "\": return \"" + _model.Database.ModifiedByColumnName + "\";");
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "\": return \"" + _model.Database.ModifiedDateColumnName + "\";");
			}

			sb.AppendLine("			}");
			sb.AppendLine("			return \"\";");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendSearchClass()
		{
			if (_model.Database.AllowZeroTouch) return;

			IEnumerable<Column> validColumns = GetValidSearchColumns();
			if (validColumns.Count() != 0)
			{
				sb.AppendLine("	#region " + _currentTable.PascalName + "Search");
				sb.AppendLine();
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// The search object for the " + _currentTable.PascalName + "Collection.");
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	public class " + _currentTable.PascalName + "Search : IBusinessObjectSearch");
				sb.AppendLine("	{");
				sb.AppendLine();
				sb.AppendLine("		private int _maxRowCount = 0;");
				foreach (Column column in validColumns.OrderBy(x => x.Name))
				{
					sb.AppendLine("		private " + column.GetCodeType(true, true) + " _" + column.CamelName + ";");
				}
				sb.AppendLine("		private SearchType _searchType;");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Determines the maximum number of rows that are returned. Use 0 for no limit.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public int MaxRowCount");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return _maxRowCount; }");
				sb.AppendLine("			set { _maxRowCount = value; }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Determines the maximum number of rows that are returned. Use 0 for no limit.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		int IBusinessObjectSearch.MaxRowCount");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this.MaxRowCount; }");
				sb.AppendLine("			set { this.MaxRowCount = value; }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Determines the type of search to be performed.");
				sb.AppendLine("		/// </summary>");
				sb.Append("		public SearchType SearchType").AppendLine();
				sb.AppendLine("		{");
				sb.Append("			get { return _searchType; }").AppendLine();
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// A search object for the '" + _currentTable.PascalName + "' collection.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentTable.PascalName + "Search(SearchType searchType) ");
				sb.AppendLine("		{");
				sb.AppendLine("			_searchType = searchType;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		void IBusinessObjectSearch.SetValue(Enum field, object value)");
				sb.AppendLine("		{");
				sb.AppendLine("			this.SetValue((" + _currentTable.PascalName + ".FieldNameConstants)field, value);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Set the specified value on this object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public void SetValue(" + _currentTable.PascalName + ".FieldNameConstants field, object value)");
				sb.AppendLine("		{");
				foreach (Column column in validColumns.OrderBy(x => x.Name))
				{
					sb.AppendLine("			if (field == " + _currentTable.PascalName + ".FieldNameConstants." + column.PascalName + ") this." + column.PascalName + " = (" + column.GetCodeType(false) + ")value;");
				}
				sb.AppendLine("		}");
				sb.AppendLine();
				foreach (Column column in validColumns.OrderBy(x => x.Name))
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// This field determines the value of the '" + column.PascalName + "' field on the '" + _currentTable.PascalName + "' object when this search object is applied.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public " + column.GetCodeType(true, true) + " " + column.PascalName);
					sb.AppendLine("		{");
					sb.AppendLine("			get { return _" + column.CamelName + "; }");
					sb.AppendLine("			set { _" + column.CamelName + " = value; }");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
				sb.AppendLine("	}");
				sb.AppendLine();
				sb.AppendLine("	#endregion ");
				sb.AppendLine();
			}
		}

		private void AppendPagingClass()
		{
			sb.AppendLine("	#region " + _currentTable.PascalName + "Paging");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// A field sort object for the " + _currentTable.PascalName + "Paging object.");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public class " + _currentTable.PascalName + "PagingFieldItem : IPagingFieldItem");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the direction of the sort.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public bool Ascending { get; set; }");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the field on which to sort.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + DefaultNamespace + ".Business.Objects." + _currentTable.PascalName + ".FieldNameConstants Field { get; set; }");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create a sorting field object for the " + _currentTable.PascalName + "Paging object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"field\">The field on which to sort.</param>");
			sb.AppendLine("		public " + _currentTable.PascalName + "PagingFieldItem(" + DefaultNamespace + ".Business.Objects." + _currentTable.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.Field = field;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create a sorting field object for the " + _currentTable.PascalName + "Paging object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"field\">The field on which to sort.</param>");
			sb.AppendLine("		/// <param name=\"ascending\">Determines the direction of the sort.</param>");
			sb.AppendLine("		public " + _currentTable.PascalName + "PagingFieldItem(" + DefaultNamespace + ".Business.Objects." + _currentTable.PascalName + ".FieldNameConstants field, bool ascending)");
			sb.AppendLine("			: this(field)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.Ascending = ascending;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#region IPagingFieldItem Members");
			sb.AppendLine();
			sb.AppendLine("		Enum IPagingFieldItem.GetField()");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.Field;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// The paging object for the " + _currentTable.PascalName + " collection");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public class " + _currentTable.PascalName + "Paging : IPagingObject");
			sb.AppendLine("	{");
			sb.AppendLine("		#region Class Members");
			sb.AppendLine();
			sb.AppendLine("		private int _pageIndex = 1;");
			sb.AppendLine("		private int _recordsperPage = 10;");
			sb.AppendLine("		private List<" + _currentTable.PascalName + "PagingFieldItem> _orderByList = new List<" + _currentTable.PascalName + "PagingFieldItem>();		");
			sb.AppendLine("		private int _recordCount = 0;");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region Constructors");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a paging object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "Paging()");
			sb.AppendLine("			: this(1, 10, null)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a paging object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"pageIndex\">The page number to load</param>");
			sb.AppendLine("		/// <param name=\"recordsperPage\">The number of records per page.</param>");
			sb.AppendLine("		public " + _currentTable.PascalName + "Paging(int pageIndex, int recordsperPage)");
			sb.AppendLine("			: this(pageIndex, recordsperPage, null)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a paging object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"pageIndex\">The page number to load</param>");
			sb.AppendLine("		/// <param name=\"recordsperPage\">The number of records per page.</param>");
			sb.AppendLine("		/// <param name=\"field\">The field on which to sort.</param>");
			sb.AppendLine("		/// <param name=\"ascending\">Determines the sorted direction.</param>");
			sb.AppendLine("		public " + _currentTable.PascalName + "Paging(int pageIndex, int recordsperPage, " + DefaultNamespace + ".Business.Objects." + _currentTable.PascalName + ".FieldNameConstants field, bool ascending)");
			sb.AppendLine("			: this(pageIndex, recordsperPage, new " + _currentTable.PascalName + "PagingFieldItem(field, ascending))");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a paging object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"pageIndex\">The page number to load</param>");
			sb.AppendLine("		/// <param name=\"recordsperPage\">The number of items per page.</param>");
			sb.AppendLine("		/// <param name=\"orderByField\">The field on which to sort.</param>");
			sb.AppendLine("		public " + _currentTable.PascalName + "Paging(int pageIndex, int recordsperPage, " + _currentTable.PascalName + "PagingFieldItem orderByField)");
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
			sb.AppendLine("		public List<" + _currentTable.PascalName + "PagingFieldItem> OrderByList");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return _orderByList; }			");
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
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();
		}

		private IEnumerable<Column> GetValidSearchColumns()
		{
			ColumnCollection allColumns = _currentTable.GetColumnsFullHierarchy();
			List<Column> validColumns = new List<Column>();
			foreach (Column column in allColumns.OrderBy(x => x.Name))
			{
				if (!(column.DataType == System.Data.SqlDbType.Binary ||
					column.DataType == System.Data.SqlDbType.Image ||
					column.DataType == System.Data.SqlDbType.NText ||
					column.DataType == System.Data.SqlDbType.Text ||
					column.DataType == System.Data.SqlDbType.Timestamp ||
					column.DataType == System.Data.SqlDbType.Udt ||
					column.DataType == System.Data.SqlDbType.VarBinary ||
					column.DataType == System.Data.SqlDbType.Variant ||
					column.DataType == System.Data.SqlDbType.Money))
				{
					validColumns.Add(column);
				}
			}
			return validColumns;
		}

		private void AppendClassEnumerator()
		{
			sb.AppendLine("		#region IEnumerator");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// An strongly-typed enumerator for the '" + _currentTable.PascalName + "' object collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public class " + _currentTable.PascalName + "Enumerator : IEnumerator ");
			sb.AppendLine("		{");
			sb.AppendLine("			private IEnumerator internalEnumerator;");
			sb.AppendLine("			internal " + _currentTable.PascalName + "Enumerator(IEnumerator icg)");
			sb.AppendLine("			{");
			sb.AppendLine("				internalEnumerator = icg;");
			sb.AppendLine("			}");
			sb.AppendLine("			");
			sb.AppendLine("			#region IEnumerator Members");
			sb.AppendLine("			");
			sb.AppendLine("			/// <summary>");
			sb.AppendLine("			/// Reset the enumerator to the first object in this collection");
			sb.AppendLine("			/// </summary>");
			sb.AppendLine("			public void Reset()");
			sb.AppendLine("			{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				internalEnumerator.Reset();");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the current element in the collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns>The current element in the collection.</returns>");
			sb.AppendLine("			public object Current");
			sb.AppendLine("			{");
			sb.AppendLine("				get");
			sb.AppendLine("				{");
			sb.AppendLine("					try");
			sb.AppendLine("					{");
			sb.AppendLine("						return new " + _currentTable.PascalName + "((Domain" + _currentTable.PascalName + ")internalEnumerator.Current);");
			sb.AppendLine("					}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("			/// <summary>");
			sb.AppendLine("			/// Advances the enumerator to the next element of the collection.");
			sb.AppendLine("			/// </summary>");
			sb.AppendLine("			public bool MoveNext()");
			sb.AppendLine("			{");
			sb.AppendLine("				try");
			sb.AppendLine("				{");
			sb.AppendLine("					bool movedNext = internalEnumerator.MoveNext();");
			sb.AppendLine("					if(movedNext)");
			sb.AppendLine("					{");
			sb.AppendLine("						Domain" + _currentTable.PascalName + " currentRow = (Domain" + _currentTable.PascalName + ")internalEnumerator.Current;");
			sb.AppendLine("						while(currentRow.RowState == System.Data.DataRowState.Deleted || currentRow.RowState == System.Data.DataRowState.Detached)");
			sb.AppendLine("						{");
			sb.AppendLine("							movedNext = internalEnumerator.MoveNext();");
			sb.AppendLine("							if(!movedNext)");
			sb.AppendLine("								break;");
			sb.AppendLine("							currentRow = (Domain" + _currentTable.PascalName + ")internalEnumerator.Current;");
			sb.AppendLine("						}");
			sb.AppendLine("					}");
			sb.AppendLine("					return movedNext;");
			sb.AppendLine("				}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("			#endregion");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

		#region append member variables

		public void AppendMemberVariables()
		{
			sb.AppendLine("		#region Member Variables");
			sb.AppendLine();

			if (_currentTable.ParentTable == null)
			{
				sb.AppendLine("		internal Domain" + _currentTable.PascalName + "Collection wrappedClass;");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The parent subdomain object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected SubDomain _subDomain = null;");
			}
			else
			{
				sb.AppendLine("		internal new Domain" + _currentTable.PascalName + "Collection wrappedClass;");
			}

			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}
		#endregion

		#region append constructors

		public void AppendConstructors()
		{
			sb.AppendLine("		#region Constructor / Initialize");
			sb.AppendLine();
			AppendConstructorDomainClass();
			AppendConstructorModifier();
			AppendConstructorDefault();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendConstructorDomainClass()
		{
			if (_currentTable.ParentTable == null)
			{
				sb.AppendLine("		internal " + _currentTable.PascalName + "Collection(Domain" + _currentTable.PascalName + "Collection classToWrap)");
				sb.AppendLine("		{");
				sb.AppendLine("			_subDomain = classToWrap.SubDomain;");
				sb.AppendLine("			wrappedClass = classToWrap;");
				sb.AppendLine("		}");
			}
			else
			{
				sb.AppendLine("		internal " + _currentTable.PascalName + "Collection(Domain" + _currentTable.PascalName + "Collection classToWrap):base(classToWrap)");
				sb.AppendLine("		{");
				sb.AppendLine("			wrappedClass = classToWrap;");
				sb.AppendLine("			base.wrappedClass = wrappedClass;");
				sb.AppendLine("		}");
			}

			sb.AppendLine();
		}

		private void AppendConstructorModifier()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Constructor that enables you to specify a modifier");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"modifier\">Used in audit operations to track changes</param>");
			sb.AppendLine("		public " + _currentTable.PascalName + "Collection(string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			_subDomain = new SubDomain(modifier);");
			sb.AppendLine("			ResetWrappedClass((Domain" + _currentTable.PascalName + "Collection)_subDomain.GetDomainCollection(Collections." + _currentTable.PascalName + "Collection));");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendConstructorDefault()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The default constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "Collection() ");
			sb.AppendLine("		{");
			sb.AppendLine("			_subDomain = new SubDomain(\"\");");
			sb.AppendLine("			ResetWrappedClass((Domain" + _currentTable.PascalName + "Collection)_subDomain.GetDomainCollection(Collections." + _currentTable.PascalName + "Collection));");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		#endregion

		#region append properties

		private void AppendProperties()
		{
			sb.AppendLine("		#region Property Implementations");
			sb.AppendLine();
			AppendPropertyWrappedClass();
			AppendPropertyModifier();
			AppendPropertySubDomain();
			AppendPropertyCount();
			AppendPropertyContainedType();
			AppendPropertyCollection();
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendPropertyWrappedClass()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the internal object that this object wraps");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		[System.ComponentModel.Browsable(false)]");

			if (_currentTable.ParentTable == null)
				sb.AppendLine("		internal virtual object WrappedClass");
			else
				sb.AppendLine("		internal override object WrappedClass");

			sb.AppendLine("		{");
			sb.AppendLine("			get { return wrappedClass ; }");
			sb.AppendLine("			set { wrappedClass = (Domain" + _currentTable.PascalName + "Collection)value ; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		//WrappingClass Interface");
			sb.AppendLine("		object IWrappingClass.WrappedClass");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.WrappedClass;}");
			sb.AppendLine("			set { this.WrappedClass = value;}");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendPropertySubDomain()
		{
			if (_currentTable.ParentTable != null)
				return;

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// A reference to the SubDomain that holds this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public SubDomain SubDomain");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				try");
			sb.AppendLine("				{");
			sb.AppendLine("					return wrappedClass.SubDomain;");
			sb.AppendLine("				}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendPropertyModifier()
		{
			if (_currentTable.ParentTable != null)
				return;

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Specifies a modifier string to be used in the audit functionality");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string Modifier");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				try");
			sb.AppendLine("				{");
			sb.AppendLine("					return wrappedClass.Modifier;");
			sb.AppendLine("				}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendPropertyCount()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the number of items in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override int Count");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				try");
			sb.AppendLine("				{");
			sb.AppendLine("					lock (wrappedClass.SubDomain)");
			sb.AppendLine("					{");
			sb.AppendLine("						DataTable dt = wrappedClass.GetChanges(DataRowState.Deleted);");
			sb.AppendLine("						if (dt == null) return wrappedClass.Count;");
			sb.AppendLine("						else return wrappedClass.Count - dt.Rows.Count;");
			sb.AppendLine("					}");
			sb.AppendLine("				}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendPropertyContainedType()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the type of object contained by this collection.");
			sb.AppendLine("		/// </summary>");
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public virtual Type ContainedType");
			else
				sb.AppendLine("		public override Type ContainedType");

			sb.AppendLine("		{");
			sb.AppendLine("			get { return typeof(" + _currentTable.PascalName + "); }");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendPropertyCollection()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the type of collection for this object.");
			sb.AppendLine("		/// </summary>");
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public virtual Collections Collection");
			else
				sb.AppendLine("		public override Collections Collection");

			sb.AppendLine("		{");
			sb.AppendFormat("      get {{ return Collections.{0}Collection; }}", _currentTable.PascalName).AppendLine();
			sb.AppendLine("		}");
		}

		#endregion

		#region append methods
		public void AppendMethods()
		{
			sb.AppendLine("		#region Methods");
			sb.AppendLine();
			this.AppendMethodNewItems();
			this.AppendMethodAddNewItem();
			this.AppendMethodVisitor();
			this.AppendStaticSQLMethods();
			this.AppendMethodPersist();
			this.AppendMethodSelectAll();
			this.AppendMethodSelectSearch();
			this.AppendAggregateMethods();
			this.AppendUpdateDataScaler();
			this.AppendDeleteDataScaler();
			this.AppendMethodSelectPaged();
			this.AppendMethodRetieveByPrimaryKey();
			this.AppendMethodRetieveBySearchableFields();
			this.AppendMethodGet();
			this.AppendMethodGetEnumerator();
			this.AppendRejectChanges();
			this.AppendResetWrappedClass();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendUpdateDataScaler()
		{
			//All types that are valid for these operations
			List<string> typeList = new List<string>();
			typeList.Add("int");
			typeList.Add("Single");
			typeList.Add("double");
			typeList.Add("string");
			typeList.Add("DateTime");
			typeList.Add("bool");
			typeList.Add("Guid");

			List<Table> tableList = _currentTable.GetTableHierarchy();
			tableList.Remove(_currentTable);
			Dictionary<string, Column> columnList = new Dictionary<string, Column>();

			//Need the columns in order from base UP
			foreach (Table t in tableList.OrderBy(x => x.Name))
			{
				foreach (Reference r in t.Columns)
				{
					Column c = (Column)r.Object;
					if (!columnList.ContainsKey(c.DatabaseName.ToLower()))
						columnList.Add(c.DatabaseName.ToLower(), c);
				}
			}

			//Add primary Keys
			foreach (Column c in _currentTable.PrimaryKeyColumns)
			{
				if (columnList.ContainsKey(c.DatabaseName.ToLower()))
					columnList.Remove(c.DatabaseName.ToLower());
			}

			foreach (string typeName in typeList)
			{
				string nullTypeName = typeName + (typeName == "string" ? "" : "?");

				//The NON-nullable call
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
				sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
				sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
				sb.AppendLine("		/// <returns>The number of records affected</returns>");
				sb.AppendLine("		public static int UpdateData(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select, Expression<Func<" + _currentTable.PascalName + "Query, bool>> where, " + typeName + " newValue)");
				sb.AppendLine("		{");
				sb.AppendLine("			SubDomain subDomain = new SubDomain(\"\");");
				sb.AppendLine();
				sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
				sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");
				sb.AppendLine();

				/*
				sb.AppendLine("			var cmd = dc.GetCommand(template");
				sb.AppendLine("				.Where(where)");
				sb.AppendLine("				.Select(x => x));");
				 */


				sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
				sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");
				sb.AppendLine();
				sb.AppendLine("			bool isCreateBy = false;");
				sb.AppendLine("			bool isCreateDate = false;");
				sb.AppendLine("			bool isModifyBy = false;");
				sb.AppendLine("			bool isModifyDate = false;");
				sb.AppendLine("			if (isCreateBy && isCreateDate && isModifyBy && isModifyDate && false) System.Diagnostics.Debug.Write(\"\");");
				sb.AppendLine("			string fieldName = ((System.Linq.Expressions.MemberExpression)(select.Body)).Member.Name;");
				sb.AppendLine("			switch (fieldName.ToLower())");
				sb.AppendLine("			{");
				foreach (string s in columnList.Keys)
				{
					Column column = columnList[s];
					sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
				}
				foreach (Reference reference in _currentTable.Columns)
				{
					Column column = (Column)reference.Object;
					sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
				}
				if (_currentTable.AllowCreateAudit)
				{
					sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "\": fieldName = \"" + _model.Database.CreatedByColumnName + "\"; isCreateBy = true; break;");
					sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "\": fieldName = \"" + _model.Database.CreatedDateColumnName + "\"; isCreateDate = true; break;");
				}
				if (_currentTable.AllowModifiedAudit)
				{
					sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "\": fieldName = \"" + _model.Database.ModifiedByColumnName + "\"; isModifyBy = true; break;");
					sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "\": fieldName = \"" + _model.Database.ModifiedDateColumnName + "\"; isModifyDate = true; break;");
				}
				sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
				sb.AppendLine("			}");
				sb.AppendLine();
				sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
				sb.AppendLine("			string sql = \"UPDATE [\" + parser.GetTableAlias(fieldName, \"" + _currentTable.DatabaseName + "\") + \"]\\r\\n\";");
				sb.AppendLine("			sql += \"SET [\" + parser.GetTableAlias(fieldName, \"" + _currentTable.DatabaseName + "\") + \"].[\" + fieldName + \"] = @newValue\\r\\n\";");

				if (_currentTable.AllowModifiedAudit)
				{
					string fieldName = _model.Database.ModifiedByColumnName;
					sb.AppendLine("			if (!isModifyBy) sql += \", [\" + parser.GetTableAlias(\"" + fieldName + "\", \"" + _currentTable.DatabaseName + "\") + \"].[" + fieldName + "] = NULL\\r\\n\";");

					fieldName = _model.Database.ModifiedDateColumnName;
					sb.AppendLine("			if (!isModifyDate) sql += \", [\" + parser.GetTableAlias(\"" + fieldName + "\", \"" + _currentTable.DatabaseName + "\") + \"].[" + fieldName + "] = getdate()\\r\\n\";");
				}

				sb.AppendLine("			sql += parser.GetFromClause()+ \"\\r\\n\";");
				sb.AppendLine("			sql += parser.GetWhereClause();");
				sb.AppendLine("			sql += \";select @@rowcount\";");
				sb.AppendLine("			cmd.CommandText = sql;");
				sb.AppendLine("			cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(\"newValue\", newValue));");
				sb.AppendLine("			dc.Connection.Open();");
				sb.AppendLine("			object p = cmd.ExecuteScalar();");
				sb.AppendLine("			dc.Connection.Close();");
				sb.AppendLine("			return (int)p;");
				sb.AppendLine("		}");
				sb.AppendLine();


				//The Nullable call
				if (nullTypeName != typeName)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
					sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
					sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
					sb.AppendLine("		/// <returns>The number of records affected</returns>");
					sb.AppendLine("		public static int UpdateData(Expression<Func<" + _currentTable.PascalName + "Query, " + nullTypeName + ">> select, Expression<Func<" + _currentTable.PascalName + "Query, bool>> where, " + nullTypeName + " newValue)");
					sb.AppendLine("		{");
					sb.AppendLine("			SubDomain subDomain = new SubDomain(\"\");");
					sb.AppendLine();
					sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
					sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");
					sb.AppendLine();

					/*
					sb.AppendLine("			var cmd = dc.GetCommand(template");
					sb.AppendLine("				.Where(where)");
					sb.AppendLine("				.Select(x => x));");
					*/

					sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
					sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");

					sb.AppendLine();
					sb.AppendLine("			bool isCreateBy = false;");
					sb.AppendLine("			bool isCreateDate = false;");
					sb.AppendLine("			bool isModifyBy = false;");
					sb.AppendLine("			bool isModifyDate = false;");
					sb.AppendLine("			if (isCreateBy && isCreateDate && isModifyBy && isModifyDate && false) System.Diagnostics.Debug.Write(\"\");");
					sb.AppendLine("			string fieldName = ((System.Linq.Expressions.MemberExpression)(select.Body)).Member.Name;");
					sb.AppendLine("			switch (fieldName.ToLower())");
					sb.AppendLine("			{");
					foreach (string s in columnList.Keys)
					{
						Column column = columnList[s];
						sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
					}
					foreach (Reference reference in _currentTable.Columns)
					{
						Column column = (Column)reference.Object;
						sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
					}
					if (_currentTable.AllowCreateAudit)
					{
						sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "\": fieldName = \"" + _model.Database.CreatedByColumnName + "\"; isCreateBy = true; break;");
						sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "\": fieldName = \"" + _model.Database.CreatedDateColumnName + "\"; isCreateDate = true; break;");
					}
					if (_currentTable.AllowModifiedAudit)
					{
						sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "\": fieldName = \"" + _model.Database.ModifiedByColumnName + "\"; isModifyBy = true; break;");
						sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "\": fieldName = \"" + _model.Database.ModifiedDateColumnName + "\"; isModifyDate = true; break;");
					}
					sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
					sb.AppendLine("			}");
					sb.AppendLine();
					sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
					sb.AppendLine("			string sql = \"UPDATE [\" + parser.GetTableAlias(fieldName, \"" + _currentTable.DatabaseName + "\") + \"]\\r\\n\";");
					sb.AppendLine("			sql += \"SET [\" + parser.GetTableAlias(fieldName, \"" + _currentTable.DatabaseName + "\") + \"].[\" + fieldName + \"] = @newValue\\r\\n\";");

					if (_currentTable.AllowModifiedAudit)
					{
						string fieldName = _model.Database.ModifiedByColumnName;
						sb.AppendLine("			if (!isModifyBy) sql += \", [\" + parser.GetTableAlias(\"" + fieldName + "\", \"" + _currentTable.DatabaseName + "\") + \"].[" + fieldName + "] = NULL\\r\\n\";");

						fieldName = _model.Database.ModifiedDateColumnName;
						sb.AppendLine("			if (!isModifyDate) sql += \", [\" + parser.GetTableAlias(\"" + fieldName + "\", \"" + _currentTable.DatabaseName + "\") + \"].[" + fieldName + "] = getdate()\\r\\n\";");
					}

					sb.AppendLine("			sql += parser.GetFromClause()+ \"\\r\\n\";");
					sb.AppendLine("			sql += parser.GetWhereClause();");
					sb.AppendLine("			sql += \";select @@rowcount\";");
					sb.AppendLine("			cmd.CommandText = sql;");
					sb.AppendLine("			if (newValue == null) cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(\"newValue\", System.DBNull.Value));");
					sb.AppendLine("			else cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(\"newValue\", newValue));");
					sb.AppendLine("			dc.Connection.Open();");
					sb.AppendLine("			object p = cmd.ExecuteScalar();");
					sb.AppendLine("			dc.Connection.Close();");
					sb.AppendLine("			return (int)p;");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
			}

		}

		private void AppendAggregateMethods()
		{
			string scope = "";

			List<Table> tableList = _currentTable.GetTableHierarchy();
			tableList.Remove(_currentTable);
			Dictionary<string, Column> columnList = new Dictionary<string, Column>();

			//Need the colums in order from base UP
			foreach (Table t in tableList)
			{
				foreach (Reference r in t.Columns)
				{
					Column c = (Column)r.Object;
					if (!columnList.ContainsKey(c.DatabaseName.ToLower()))
						columnList.Add(c.DatabaseName.ToLower(), c);
				}
			}

			//Add primary Keys
			foreach (Column c in _currentTable.PrimaryKeyColumns)
			{
				if (columnList.ContainsKey(c.DatabaseName.ToLower()))
					columnList.Remove(c.DatabaseName.ToLower());
			}

			//All types that are valid for these operations
			List<string> typeList = new List<string>();
			typeList.Add("int");
			typeList.Add("Single");
			typeList.Add("double");
			typeList.Add("decimal");
			typeList.Add("string");
			typeList.Add("DateTime");

			//Max
			this.AppendAggregateMax(scope, tableList, columnList, typeList);

			//Min
			this.AppendAggregateMin(scope, tableList, columnList, typeList);

			typeList = new List<string>();
			typeList.Add("int");
			typeList.Add("Single");
			typeList.Add("double");
			typeList.Add("decimal");

			//Average
			this.AppendAggregateAverage(scope, tableList, columnList, typeList);

			//Sum
			this.AppendAggregateSum(scope, tableList, columnList, typeList);

			typeList = new List<string>();
			typeList.Add("int");
			typeList.Add("Single");
			typeList.Add("double");
			typeList.Add("decimal");
			typeList.Add("string");
			typeList.Add("DateTime");
			typeList.Add("Guid");

			//Count
			this.AppendAggregateCount(scope, tableList, columnList);

			//Distinct
			this.AppendAggregateDistinct(scope, tableList, columnList, typeList);

		}

		private void AppendAggregateSum(string scope, List<Table> tableList, Dictionary<string, Column> columnList, List<string> typeList)
		{
			foreach (string typeName in typeList)
			{
				AppendAggregateSumBody(scope, columnList, typeName);
				if (typeName != "string")
					AppendAggregateSumBody(scope, columnList, typeName + "?");
			}
		}

		private void AppendAggregateSumBody(string scope, Dictionary<string, Column> columnList, string typeName)
		{
			string returnType = "double";
			if (typeName.StartsWith("decimal")) returnType = "decimal";

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the summary value of the field for all objects");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"select\">The field to aggregate</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + returnType + "? GetSum(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select)");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetSum(select, x => true);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the summary value of the field in the set of records that match the Where condition");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"select\">The field to aggregate</param>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + returnType + "? GetSum(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select, Expression<Func<" + _currentTable.PascalName + "Query, bool>> where)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(\"\");");
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");

			/*
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			*/

			sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
			sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");

			sb.AppendLine();
			sb.AppendLine("			string fieldName = ((System.Linq.Expressions.MemberExpression)(select.Body)).Member.Name;");
			sb.AppendLine("			switch (fieldName.ToLower())");
			sb.AppendLine("			{");
			foreach (string s in columnList.Keys)
			{
				Column column = columnList[s];
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
			}
			foreach (Reference reference in _currentTable.Columns)
			{
				Column column = (Column)reference.Object;
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
			}
			sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
			sb.AppendLine("			}");
			sb.AppendLine();

			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
			sb.AppendLine("			cmd.CommandText = \"SELECT SUM([\" + parser.GetTableAlias(fieldName, \"" + _currentTable.DatabaseName + "\") + \"].\" + fieldName + \") \" + parser.GetFromClause() + \" \" + parser.GetWhereClause() + \"\";");
			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			object p = cmd.ExecuteScalar();");
			sb.AppendLine("			dc.Connection.Close();");
			sb.AppendLine("			if (p == System.DBNull.Value)");
			sb.AppendLine("				return null;");
			sb.AppendLine("			else");
			sb.AppendLine("				return (" + typeName + ")p;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendAggregateAverage(string scope, List<Table> tableList, Dictionary<string, Column> columnList, List<string> typeList)
		{
			foreach (string typeName in typeList)
			{
				AppendAggregateAverageBody(scope, columnList, typeName);
				if (typeName != "string")
					AppendAggregateAverageBody(scope, columnList, typeName + "?");
			}
		}

		private void AppendAggregateAverageBody(string scope, Dictionary<string, Column> columnList, string typeName)
		{
			string returnType = "double";
			if (typeName.StartsWith("decimal")) returnType = "decimal";

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the average value of the field for all objects");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"select\">The field to aggregate</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + returnType + "? GetAverage(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select)");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetAverage(select, x => true);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the average value of the field in the set of records that match the Where condition");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"select\">The field to aggregate</param>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + returnType + "? GetAverage(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select, Expression<Func<" + _currentTable.PascalName + "Query, bool>> where)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(\"\");");
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");

			/*
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			*/

			sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
			sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");

			sb.AppendLine();
			sb.AppendLine("			string fieldName = ((System.Linq.Expressions.MemberExpression)(select.Body)).Member.Name;");
			sb.AppendLine("			switch (fieldName.ToLower())");
			sb.AppendLine("			{");
			foreach (string s in columnList.Keys)
			{
				Column column = columnList[s];
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
			}
			foreach (Reference reference in _currentTable.Columns)
			{
				Column column = (Column)reference.Object;
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
			}
			sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
			sb.AppendLine("			}");
			sb.AppendLine();

			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
			sb.AppendLine("			cmd.CommandText = \"SELECT AVG([\" + parser.GetTableAlias(fieldName, \"" + _currentTable.DatabaseName + "\") + \"].\" + fieldName + \") \" + parser.GetFromClause() + \" \" + parser.GetWhereClause() + \"\";");
			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			object p = cmd.ExecuteScalar();");
			sb.AppendLine("			dc.Connection.Close();");
			sb.AppendLine("			if (p == System.DBNull.Value)");
			sb.AppendLine("				return null;");
			sb.AppendLine("			else");
			sb.AppendLine("				return (" + typeName + ")p;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendAggregateDistinct(string scope, List<Table> tableList, Dictionary<string, Column> columnList, List<string> typeList)
		{
			foreach (string typeName in typeList)
			{
				AppendAggregateDistinctBody(scope, columnList, typeName);
				if (typeName != "string")
					AppendAggregateDistinctBody(scope, columnList, typeName + "?");
			}
		}

		private void AppendAggregateDistinctBody(string scope, Dictionary<string, Column> columnList, string typeName)
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get a list of distinct values of the field for all objects");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"select\">The field to aggregate</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + "IEnumerable<" + typeName + "> GetDistinct(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select)");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetDistinct(select, x => true);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get a list of distinct values of the field in the set of records that match the Where condition");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"select\">The field to aggregate</param>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + "IEnumerable<" + typeName + "> GetDistinct(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select, Expression<Func<" + _currentTable.PascalName + "Query, bool>> where)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(\"\");");
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");

			/*
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			*/

			sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
			sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");

			sb.AppendLine();
			sb.AppendLine("			string fieldName = ((System.Linq.Expressions.MemberExpression)(select.Body)).Member.Name;");
			sb.AppendLine("			switch (fieldName.ToLower())");
			sb.AppendLine("			{");

			foreach (string s in columnList.Keys)
			{
				Column column = columnList[s];
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
			}

			foreach (Reference reference in _currentTable.Columns)
			{
				Column column = (Column)reference.Object;
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
			}

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "\": fieldName = \"" + _model.Database.CreatedByColumnName + "\"; break;");
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "\": fieldName = \"" + _model.Database.CreatedDateColumnName + "\"; break;");
			}

			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "\": fieldName = \"" + _model.Database.ModifiedByColumnName + "\"; break;");
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "\": fieldName = \"" + _model.Database.ModifiedDateColumnName + "\"; break;");
			}

			if (_currentTable.AllowTimestamp)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName).ToLower() + "\": fieldName = \"" + _model.Database.TimestampColumnName + "\"; break;");
			}

			sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
			sb.AppendLine("			}");
			sb.AppendLine();

			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
			sb.AppendLine("			cmd.CommandText = \"SELECT DISTINCT([\" + parser.GetTableAlias(fieldName, \"" + _currentTable.DatabaseName + "\") + \"].\" + fieldName + \") \" + parser.GetFromClause() + \" \" + parser.GetWhereClause() + \"\";");
			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			System.Data.Common.DbDataReader dr = cmd.ExecuteReader();");
			sb.AppendLine();
			sb.AppendLine("			List<" + typeName + "> retval = new List<" + typeName + ">();");
			sb.AppendLine("			while (dr.Read())");
			sb.AppendLine("			{");
			sb.AppendLine("				if (!dr.IsDBNull(0))");
			sb.AppendLine("				{");
			sb.AppendLine("					object o = dr[0];");
			sb.AppendLine("					retval.Add((" + typeName + ")o);");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("			dc.Connection.Close();");
			sb.AppendLine();
			sb.AppendLine("			return retval;");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendAggregateMin(string scope, List<Table> tableList, Dictionary<string, Column> columnList, List<string> typeList)
		{
			foreach (string typeName in typeList)
			{
				AppendAggregateMinBody(scope, columnList, typeName);
				if (typeName != "string")
					AppendAggregateMinBody(scope, columnList, typeName + "?");
			}
		}

		private void AppendAggregateMinBody(string scope, Dictionary<string, Column> columnList, string typeName)
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the minimum value of the field for all objects");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"select\">The field to aggregate</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + typeName.Replace("?", "") + ((typeName == "string") ? "" : "?") + " GetMin(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select)");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetMin(select, x => true);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the minimum value of the field in the set of records that match the Where condition");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"select\">The field to aggregate</param>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + typeName.Replace("?", "") + ((typeName == "string") ? "" : "?") + " GetMin(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select, Expression<Func<" + _currentTable.PascalName + "Query, bool>> where)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(\"\");");
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");

			/*
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			*/

			sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
			sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");

			sb.AppendLine();
			sb.AppendLine("			string fieldName = ((System.Linq.Expressions.MemberExpression)(select.Body)).Member.Name;");
			sb.AppendLine("			switch (fieldName.ToLower())");
			sb.AppendLine("			{");
			foreach (string s in columnList.Keys)
			{
				Column column = columnList[s];
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
			}
			foreach (Reference reference in _currentTable.Columns)
			{
				Column column = (Column)reference.Object;
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
			}
			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "\": fieldName = \"" + _model.Database.CreatedByColumnName + "\"; break;");
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "\": fieldName = \"" + _model.Database.CreatedDateColumnName + "\"; break;");
			}
			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "\": fieldName = \"" + _model.Database.ModifiedByColumnName + "\"; break;");
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "\": fieldName = \"" + _model.Database.ModifiedDateColumnName + "\"; break;");
			}
			sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
			sb.AppendLine("			}");
			sb.AppendLine();

			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
			sb.AppendLine("			cmd.CommandText = \"SELECT MIN([\" + parser.GetTableAlias(fieldName, \"" + _currentTable.DatabaseName + "\") + \"].\" + fieldName + \") \" + parser.GetFromClause() + \" \" + parser.GetWhereClause() + \"\";");
			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			object p = cmd.ExecuteScalar();");
			sb.AppendLine("			dc.Connection.Close();");
			sb.AppendLine("			if (p == System.DBNull.Value)");
			sb.AppendLine("				return null;");
			sb.AppendLine("			else");
			sb.AppendLine("				return (" + typeName + ")p;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendAggregateMax(string scope, List<Table> tableList, Dictionary<string, Column> columnList, List<string> typeList)
		{
			foreach (string typeName in typeList)
			{
				AppendAggregateMaxBody(scope, columnList, typeName);
				if (typeName != "string")
					AppendAggregateMaxBody(scope, columnList, typeName + "?");
			}
		}

		private void AppendAggregateMaxBody(string scope, Dictionary<string, Column> columnList, string typeName)
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the maximum value of the field for all objects");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"select\">The field to aggregate</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + typeName.Replace("?", "") + ((typeName == "string") ? "" : "?") + " GetMax(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select)");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetMax(select, x => true);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the maximum value of the field in the set of records that match the Where condition");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"select\">The field to aggregate</param>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + typeName.Replace("?", "") + ((typeName == "string") ? "" : "?") + " GetMax(Expression<Func<" + _currentTable.PascalName + "Query, " + typeName + ">> select, Expression<Func<" + _currentTable.PascalName + "Query, bool>> where)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(\"\");");
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");

			/*
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			 */

			sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
			sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");

			sb.AppendLine();
			sb.AppendLine("			string fieldName = ((System.Linq.Expressions.MemberExpression)(select.Body)).Member.Name;");
			sb.AppendLine("			switch (fieldName.ToLower())");
			sb.AppendLine("			{");
			foreach (string s in columnList.Keys)
			{
				Column column = columnList[s];
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
			}
			foreach (Reference reference in _currentTable.Columns)
			{
				Column column = (Column)reference.Object;
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": fieldName = \"" + column.DatabaseName + "\"; break;");
			}
			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "\": fieldName = \"" + _model.Database.CreatedByColumnName + "\"; break;");
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "\": fieldName = \"" + _model.Database.CreatedDateColumnName + "\"; break;");
			}
			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "\": fieldName = \"" + _model.Database.ModifiedByColumnName + "\"; break;");
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "\": fieldName = \"" + _model.Database.ModifiedDateColumnName + "\"; break;");
			}
			sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
			sb.AppendLine("			}");
			sb.AppendLine();

			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
			sb.AppendLine("			cmd.CommandText = \"SELECT MAX([\" + parser.GetTableAlias(fieldName, \"" + _currentTable.DatabaseName + "\") + \"].\" + fieldName + \") \" + parser.GetFromClause() + \" \" + parser.GetWhereClause() + \"\";");
			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			object p = cmd.ExecuteScalar();");
			sb.AppendLine("			dc.Connection.Close();");
			sb.AppendLine("			if (p == System.DBNull.Value)");
			sb.AppendLine("				return null;");
			sb.AppendLine("			else");
			sb.AppendLine("				return (" + typeName + ")p;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendAggregateCount(string scope, List<Table> tableList, Dictionary<string, Column> columnList)
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the count of all objects");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public " + (_currentTable.ParentTable == null ? "" : "new ") + "static " + scope + "int GetCount()");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetCount(x => true);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the count of objects that match the Where condition");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + scope + "int GetCount(Expression<Func<" + _currentTable.PascalName + "Query, bool>> where)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(\"\");");
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");
			sb.AppendLine();

			/*
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			*/

			sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
			sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");

			sb.AppendLine();
			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
			sb.AppendLine("			cmd.CommandText = \"select count(*) \" + parser.GetFromClause() + \" \" + parser.GetWhereClause() + \"\";");
			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			object p = cmd.ExecuteScalar();");
			sb.AppendLine("			dc.Connection.Close();");
			sb.AppendLine("			return (int)p;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendDeleteDataScaler()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Delete all records that match a where condition");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records deleted</param>");
			sb.AppendLine("		/// <returns>The number of rows deleted</returns>");
			sb.AppendLine("		public static int DeleteData(Expression<Func<" + _currentTable.PascalName + "Query, bool>> where)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(\"\");");
			sb.AppendLine();
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");
			sb.AppendLine();

			/*
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			*/

			sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
			sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");

			sb.AppendLine();
			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
			sb.Append("			string sql = \"SELECT ");
			foreach (Column column in _currentTable.PrimaryKeyColumns)
			{
				sb.Append("[t0].[" + column.DatabaseName + "]");
				if (_currentTable.PrimaryKeyColumns.IndexOf(column) < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine("INTO #t\\r\\n\";");
			sb.AppendLine("			sql += parser.GetFromClause() + \"\\r\\n\";");
			sb.AppendLine("			sql += parser.GetWhereClause();");
			sb.AppendLine("			sql += \"\\r\\n\";");
			sb.AppendLine();

			List<Table> tableList = _currentTable.GetTableHierarchy();
			tableList.Reverse();
			foreach (Table table in tableList)
			{
				sb.Append("			sql += \"DELETE [" + table.DatabaseName + "] FROM [" + table.DatabaseName + "] INNER JOIN #t ON ");
				foreach (Column column in _currentTable.PrimaryKeyColumns)
				{
					sb.Append("[" + table.DatabaseName + "].[" + column.DatabaseName + "] = #t.[" + column.DatabaseName + "]");
					if (_currentTable.PrimaryKeyColumns.IndexOf(column) < _currentTable.PrimaryKeyColumns.Count - 1)
						sb.Append(" AND ");
				}
				sb.AppendLine("\\r\\n\";");
			}

			sb.AppendLine("			sql += \";select @@rowcount\";");
			sb.AppendLine("			cmd.CommandText = sql;");
			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			object p = cmd.ExecuteScalar();");
			sb.AppendLine("			dc.Connection.Close();");
			sb.AppendLine("			return (int)p;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		public void AppendMethodNewItems()
		{
			string scope = "public";
			if (_currentTable.Immutable)
				scope = "protected internal";

			if (_currentTable.ParentTable == null)
				scope += " virtual";
			else
				scope += " new";

			Column column = (Column)_currentTable.PrimaryKeyColumns[0];
			if (_currentTable.PrimaryKeyColumns.Count == 1 && column.DataType == System.Data.SqlDbType.UniqueIdentifier)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Create a new object to later add to this collection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		" + scope + " " + _currentTable.PascalName + " NewItem(Guid key) ");
				sb.AppendLine("		{");
				sb.AppendLine("			try");
				sb.AppendLine("			{");
				sb.AppendLine("				lock (wrappedClass.SubDomain)");
				sb.AppendLine("				{");
				sb.AppendLine("					return new " + _currentTable.PascalName + "(wrappedClass.NewItem(key));");
				sb.AppendLine("				}");
				sb.AppendLine("			}");
				Globals.AppendBusinessEntryCatch(sb);
				sb.AppendLine("		}");
			}
			else
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Create a new object to later add to this collection");
				sb.AppendLine("		/// </summary>");
				sb.Append("		" + scope + " " + _currentTable.PascalName + " NewItem(");

				int index = 0;
				foreach (Column dc in _currentTable.PrimaryKeyColumns)
				{
					sb.Append(dc.GetCodeType() + " " + dc.PascalName);
					if (index < _currentTable.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
					index++;
				}
				sb.AppendLine(")");

				sb.AppendLine("		{");
				sb.AppendLine("			try");
				sb.AppendLine("			{");
				sb.AppendLine("				lock (wrappedClass.SubDomain)");
				sb.AppendLine("				{");
				sb.Append("					return new " + _currentTable.PascalName + "(wrappedClass.NewItem(");

				index = 0;
				foreach (Column dc in _currentTable.PrimaryKeyColumns)
				{
					sb.Append(dc.PascalName);
					if (index < _currentTable.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
					index++;
				}
				sb.Append("));");
				sb.AppendLine();

				sb.AppendLine("				}");
				sb.AppendLine("			}");
				Globals.AppendBusinessEntryCatch(sb);
				sb.AppendLine("		}");
			}
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create a new object to later add to this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		" + scope + " " + _currentTable.PascalName + " NewItem() ");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				lock (wrappedClass.SubDomain)");
			sb.AppendLine("				{");
			sb.AppendLine("					return new " + _currentTable.PascalName + "(wrappedClass.NewItem());");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();

		}

		private void AppendMethodGet()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get an object from this collection by its unique key.");
			sb.AppendLine("		/// </summary>");

			if (_currentTable.ParentTable == null)
				sb.Append("		public virtual " + _currentTable.PascalName + " GetItemByPK(");
			else
				sb.Append("		public new " + _currentTable.PascalName + " GetItemByPK(");

			for (int ii = 0; ii < _currentTable.PrimaryKeyColumns.Count; ii++)
			{
				Column dc = (Column)_currentTable.PrimaryKeyColumns[ii];
				sb.Append(dc.GetCodeType() + " " + dc.CamelName);
				if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
				{
					sb.Append(", ");
				}
			}
			sb.AppendLine(") ");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				lock (wrappedClass.SubDomain)");
			sb.AppendLine("				{");
			sb.Append("				if (wrappedClass.Get" + _currentTable.PascalName + "(");
			for (int ii = 0; ii < _currentTable.PrimaryKeyColumns.Count; ii++)
			{
				Column dc = (Column)_currentTable.PrimaryKeyColumns[ii];
				sb.Append(dc.CamelName);
				if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
				{
					sb.Append(", ");
				}
			}
			sb.AppendLine(") != null)");
			sb.AppendLine("				{");
			sb.Append("					return new " + _currentTable.PascalName + "(wrappedClass.Get" + _currentTable.PascalName + "(");
			for (int ii = 0; ii < _currentTable.PrimaryKeyColumns.Count; ii++)
			{
				Column dc = (Column)_currentTable.PrimaryKeyColumns[ii];
				sb.Append(dc.CamelName);
				if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
				{
					sb.Append(", ");
				}
			}
			sb.AppendLine("));");
			sb.AppendLine("					}");
			sb.AppendLine("					else");
			sb.AppendLine("					{");
			sb.AppendLine("						return null;");
			sb.AppendLine("					}");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodGetEnumerator()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns an enumerator that can be used to iterate through the collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns>An Enumerator that can iterate through the objects in this collection.</returns>");

			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public virtual System.Collections.IEnumerator GetEnumerator()");
			else
				sb.AppendLine("		public override System.Collections.IEnumerator GetEnumerator()");

			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				lock (wrappedClass.SubDomain)");
			sb.AppendLine("				{");
			sb.AppendLine("					return new " + _currentTable.PascalName + "Enumerator(wrappedClass.GetEnumerator());");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodRetieveByPrimaryKey()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a single object from this collection by its primary key.");
			sb.AppendLine("		/// </summary>");
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public static " + _currentTable.PascalName + "Collection SelectUsingPK(" + PrimaryKeyParameterList(true) + ")");
			else
				sb.AppendLine("		public new static " + _currentTable.PascalName + "Collection SelectUsingPK(" + PrimaryKeyParameterList(true) + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			return SelectUsingPK(" + PrimaryKeyParameterList(false) + ", \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a single object from this collection by its primary key.");
			sb.AppendLine("		/// </summary>");
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public static " + _currentTable.PascalName + "Collection SelectUsingPK(" + PrimaryKeyParameterList(true) + ", string modifier)");
			else
				sb.AppendLine("		public new static " + _currentTable.PascalName + "Collection SelectUsingPK(" + PrimaryKeyParameterList(true) + ", string modifier)");

			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			if (_model.Database.AllowZeroTouch)
			{
				sb.Append("				" + _currentTable.PascalName + "Collection retval = " + _currentTable.PascalName + "Collection.RunSelect(x => ");
				foreach (Column column in _currentTable.PrimaryKeyColumns)
				{
					sb.Append("x." + column.PascalName + " == " + column.CamelName);
					if (_currentTable.PrimaryKeyColumns.IndexOf(column) < _currentTable.PrimaryKeyColumns.Count - 1)
						sb.Append(" && ");
				}
				sb.AppendLine(", modifier);");
				sb.AppendLine("				return retval;");
			}
			else
			{
				sb.AppendLine("				List<" + _currentTable.PascalName + "PrimaryKey> primaryKeys = new List<" + _currentTable.PascalName + "PrimaryKey>();");
				sb.AppendLine("				primaryKeys.Add(new " + _currentTable.PascalName + "PrimaryKey(" + PrimaryKeyInputParameterList() + "));");
				sb.AppendLine("				" + _currentTable.PascalName + "Collection returnVal = new " + _currentTable.PascalName + "Collection(Domain" + _currentTable.PascalName + "Collection.SelectBy" + _currentTable.PascalName + "Pks(primaryKeys, modifier));");
				sb.AppendLine("				return returnVal;");
			}
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodRetieveBySearchableFields()
		{
			//Get a list of all base column selects
			List<Table> tList = _currentTable.GetTableHierarchy();
			tList.Remove(_currentTable);
			List<Column> baseColumns = new List<Column>();
			foreach (Table t in tList)
			{
				baseColumns.AddRange(t.GetColumnsSearchable());
			}

			//Create selects for this table's fields
			foreach (Reference reference in _currentTable.Columns)
			{
				AppendMethodRetiveBySearchableFieldsSingleField((Column)reference.Object, false);
			}

			//Create selects to override all base tables selects
			foreach (Column c in baseColumns)
			{
				AppendMethodRetiveBySearchableFieldsSingleField(c, true);
			}

		}

		private void AppendMethodRetiveBySearchableFieldsSingleField(Column column, bool isNew)
		{
			if (column.IsSearchable)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select an object list by '" + column.PascalName + "' field.");
				sb.AppendLine("		/// <param name=\"" + column.CamelName + "\">The " + column.CamelName + " field</param>");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public static " + (isNew ? "new " : "") + _currentTable.PascalName + "Collection SelectBy" + column.PascalName + "(" + column.GetCodeType() + " " + column.CamelName + ")");
				sb.AppendLine("		{");
				sb.AppendLine("			return SelectBy" + column.PascalName + "(" + column.CamelName + ", \"\");");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select an object list by '" + column.PascalName + "' field.");
				sb.AppendLine("		/// <param name=\"" + column.CamelName + "\">The " + column.CamelName + " field</param>");
				sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public static " + (isNew ? "new " : "") + _currentTable.PascalName + "Collection SelectBy" + column.PascalName + "(" + column.GetCodeType() + " " + column.CamelName + ", string modifier)");
				sb.AppendLine("		{");
				sb.AppendLine("			return SelectBy" + column.PascalName + "(" + column.CamelName + ", null, modifier);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select an object list by '" + column.PascalName + "' field.");
				sb.AppendLine("		/// <param name=\"" + column.CamelName + "\">The " + column.CamelName + " field</param>");
				sb.AppendLine("		/// <param name=\"paging\">The paging object to determine how the results are paged.</param>");
				sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public static " + (isNew ? "new " : "") + _currentTable.PascalName + "Collection SelectBy" + column.PascalName + "(" + column.GetCodeType() + " " + column.CamelName + ", " + _currentTable.PascalName + "Paging paging, string modifier)");
				sb.AppendLine("		{");
				sb.AppendLine("			try");
				sb.AppendLine("			{");
				if (_model.Database.AllowZeroTouch)
				{
					sb.AppendLine("			  return " + _currentTable.PascalName + "Collection.RunSelect(x => x." + column.PascalName + " == " + column.CamelName + ", paging, modifier);");
				}
				else
				{
					sb.AppendLine("			  SubDomain sd = new SubDomain(modifier);");
					sb.AppendLine("			  " + _currentTable.PascalName + "SelectBy" + column.PascalName + " command = new " + _currentTable.PascalName + "SelectBy" + column.PascalName + "(" + column.CamelName + ", paging);");
					sb.AppendLine("			  sd.AddSelectCommand(command);");
					sb.AppendLine("			  sd.RunSelectCommands();");
					sb.AppendLine("			  if (paging != null) paging.RecordCount = command.Count;");
					sb.AppendLine("			  return (" + _currentTable.PascalName + "Collection)sd[Collections." + _currentTable.PascalName + "Collection];");
				}
				sb.AppendLine("			}");
				Globals.AppendBusinessEntryCatch(sb);
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			if (column.IsSearchable && column.IsRangeType)
			{
				string var1 = column.CamelName + "Start";
				string var2 = column.CamelName + "End";
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select an object list by range by '" + column.PascalName + "' field.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"" + var1 + "\">The inclusive starting point of the range.</param>");
				sb.AppendLine("		/// <param name=\"" + var2 + "\">The inclusive ending point of the range.</param>");
				sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
				sb.AppendLine("		public static " + (isNew ? "new " : "") + _currentTable.PascalName + "Collection SelectBy" + column.PascalName + "Range(" + column.GetCodeType() + " " + var1 + ", " + column.GetCodeType() + " " + var2 + ", string modifier)");
				sb.AppendLine("		{");
				sb.AppendLine("			return SelectBy" + column.PascalName + "Range(" + column.CamelName + "Start, " + column.CamelName + "End, null, modifier);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select a paged object list by range by '" + column.PascalName + "' field.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"" + var1 + "\">The inclusive starting point of the range.</param>");
				sb.AppendLine("		/// <param name=\"" + var2 + "\">The inclusive ending point of the range.</param>");
				sb.AppendLine("		/// <param name=\"paging\">The paging object to determine how the results are paged.</param>");
				sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
				sb.AppendLine("		public static " + (isNew ? "new " : "") + _currentTable.PascalName + "Collection SelectBy" + column.PascalName + "Range(" + column.GetCodeType() + " " + var1 + ", " + column.GetCodeType() + " " + var2 + ", " + _currentTable.PascalName + "Paging paging, string modifier)");
				sb.AppendLine("		{");
				sb.AppendLine("			try");
				sb.AppendLine("			{");
				sb.AppendLine("			  SubDomain sd = new SubDomain(modifier);");
				sb.AppendLine("			  " + _currentTable.PascalName + "SelectBy" + column.PascalName + "Range command = new " + _currentTable.PascalName + "SelectBy" + column.PascalName + "Range(" + column.CamelName + "Start, " + column.CamelName + "End, paging);");
				sb.AppendLine("			  sd.AddSelectCommand(command);");
				sb.AppendLine("			  sd.RunSelectCommands();");
				sb.AppendLine("			  if (paging != null) paging.RecordCount = command.Count;");
				sb.AppendLine("			  return (" + _currentTable.PascalName + "Collection)sd[Collections." + _currentTable.PascalName + "Collection];");
				sb.AppendLine("			}");
				Globals.AppendBusinessEntryCatch(sb);
				sb.AppendLine("		}");
				sb.AppendLine();
			}
		}

		private void AppendMethodSelectSearch()
		{
			//LINQ
			this.AppendMethodSelectSearchLINQInherit();
			this.AppendMethodSelectSearchLINQPaging();

			//for a zero touch API there is no custom serach object
			if (_model.Database.AllowZeroTouch) return;

			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a set of objects from the database with custom search object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"search\">The search object to use for searching.</param>");
			sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
			sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(" + _currentTable.PascalName + "Search search, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (search == null) throw new Exception(\"The 'Search' object cannot be null!\");");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				" + _currentTable.PascalName + "Collection returnVal = new " + _currentTable.PascalName + "Collection(Domain" + _currentTable.PascalName + "Collection.RunSelect(search, modifier));");
			sb.AppendLine("				return returnVal;");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database with custom search object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"search\">The search object to use for searching.</param>");
			sb.AppendLine("		/// <param name=\"paging\">The paging object to determine how the results are paged.</param>");
			sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
			sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(" + _currentTable.PascalName + "Search search, " + _currentTable.PascalName + "Paging paging, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (search == null) throw new Exception(\"The 'Search' object cannot be null!\");");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				" + _currentTable.PascalName + "Collection returnVal = new " + _currentTable.PascalName + "Collection(Domain" + _currentTable.PascalName + "Collection.RunSelect(search, paging, modifier));");
			sb.AppendLine("				return returnVal;");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();

		}

		private void AppendMethodSelectSearchLINQNoInherit()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Using the specified Where expression, execute a query against the database to return a result set");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(Expression<Func<" + _currentTable.PascalName + "Query, bool>> where)");
			sb.AppendLine("		{");
			sb.AppendLine("			return RunSelect(where, \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Using the specified Where expression, execute a query against the database to return a result set");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <param name=\"modifier\">The modified audit trail</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(Expression<Func<" + _currentTable.PascalName + "Query, bool>> where, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
			sb.AppendLine();
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");
			sb.AppendLine();
			sb.AppendLine("			var result = template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x);");
			sb.AppendLine();
			sb.AppendLine("			" + _currentTable.PascalName + "Collection retval = (" + _currentTable.PascalName + "Collection)subDomain[Collections." + _currentTable.PascalName + "Collection];");

			#region Set Properties 1-1
			sb.AppendLine("			foreach (" + _currentTable.PascalName + "Query item in result)");
			sb.AppendLine("			{");

			sb.Append("			if (retval.GetItemByPK(");
			foreach (Column pk in _currentTable.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);
				if (_currentTable.PrimaryKeyColumns.IndexOf(pk) < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(") == null)");
			sb.AppendLine("			{");

			sb.Append("				" + _currentTable.PascalName + " newItem = retval.NewItem(");
			foreach (Column pk in _currentTable.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);

				if (_currentTable.PrimaryKeyColumns.IndexOf(pk) < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(");");

			foreach (Column c in _currentTable.GetColumnsFullHierarchy())
			{
				if (c.AllowNull)
					sb.AppendLine("				newItem.wrappedClass[\"" + c.DatabaseName + "\"] = StringHelper.ConvertToDatabase(item." + c.PascalName + ");");
				else
					sb.AppendLine("				newItem.wrappedClass[\"" + c.DatabaseName + "\"] = item." + c.PascalName + ";");
			}

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ");");
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ");");
			}
			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ");");
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ");");
			}
			if (_currentTable.AllowTimestamp)
			{
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "\"] = item." + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + ";");
			}


			sb.AppendLine("				retval.AddItem(newItem);");
			sb.AppendLine("			}");
			sb.AppendLine("			}");
			#endregion

			sb.AppendLine("			retval.wrappedClass.AcceptChanges();");
			sb.AppendLine("			retval.wrappedClass.EndLoadData();");
			sb.AppendLine("			return retval;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodSelectSearchLINQPaging()
		{
			List<Table> tableList = _currentTable.GetTableHierarchy();
			tableList.Remove(_currentTable);
			Dictionary<string, Column> columnList = new Dictionary<string, Column>();

			//Need the colums in order from base UP
			foreach (Table t in tableList)
			{
				foreach (Reference r in t.Columns)
				{
					Column c = (Column)r.Object;
					if (!columnList.ContainsKey(c.DatabaseName.ToLower()))
						columnList.Add(c.DatabaseName.ToLower(), c);
				}
			}

			//Add primary Keys
			foreach (Column c in _currentTable.PrimaryKeyColumns)
			{
				if (columnList.ContainsKey(c.DatabaseName.ToLower()))
					columnList.Remove(c.DatabaseName.ToLower());
			}

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Using the specified Where expression, execute a query against the database to return a result set");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <param name=\"paging\">The paging object to determine how the results are paged.</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(Expression<Func<" + _currentTable.PascalName + "Query, bool>> where, " + _currentTable.PascalName + "Paging paging)");
			sb.AppendLine("		{");
			sb.AppendLine("			return RunSelect(where, paging, \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Using the specified Where expression, execute a query against the database to return a result set");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <param name=\"paging\">The paging object to determine how the results are paged.</param>");
			sb.AppendLine("		/// <param name=\"modifier\">The modified audit trail</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(Expression<Func<" + _currentTable.PascalName + "Query, bool>> where, " + _currentTable.PascalName + "Paging paging, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (paging == null) throw new Exception(\"The paging object cannot be null.\");");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
			sb.AppendLine();
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");
			sb.AppendLine();

			/*
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			*/

			sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
			sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");

			sb.AppendLine();
			//sb.AppendLine("			string sql = cmd.CommandText;");
			//sb.AppendLine();
			//sb.AppendLine("			//Parse the string");
			//sb.AppendLine("			int index = sql.IndexOf(\"\\r\\nFROM\") + 2;");
			//sb.AppendLine("			string selectClause = sql.Substring(0, index).Replace(\"\\r\\n\", \" \");");
			//sb.AppendLine("			index = sql.IndexOf(\"\\r\\nWHERE\") ;");
			//sb.AppendLine("			if (index == -1) index = sql.Length;");
			//sb.AppendLine("			string fromClause = sql.Substring(selectClause.Length + 1, index - selectClause.Length - 1).Replace(\"\\r\\n\", \" \");");
			//sb.AppendLine("			string whereClause = \"\"; ");
			//sb.AppendLine("			index = sql.IndexOf(\"\\r\\nWHERE\");");
			//sb.AppendLine("			if (index != -1) whereClause = sql.Substring(index + 2, sql.Length - index - 2).Replace(\"\\r\\n\", \" \");");
			//sb.AppendLine();
			//sb.AppendLine("			fromClause = fromClause.Replace(\"[" + _currentTable.DatabaseName + "] AS [t0]\", \"" + _currentTable.GetFullHierarchyTableJoin(true) + "\");");
			//sb.AppendLine();

			////Add the paging column determination code
			//sb.AppendLine("			//Determine the paging sort");
			//ColumnCollection allColumns = _currentTable.GetColumnsFullHierarchy();
			//sb.AppendLine("			bool isPrimary = false;");
			//sb.AppendLine("			string orderByClause = \"\";");
			//sb.AppendLine("			foreach (" + _currentTable.PascalName + "PagingFieldItem fieldItem in paging.OrderByList)");
			//sb.AppendLine("			{");
			//sb.AppendLine("				if (orderByClause != \"\") orderByClause += \", \";");
			//sb.AppendLine("				switch (fieldItem.Field.ToString().ToLower())");
			//sb.AppendLine("				{");
			//foreach (Column column in allColumns)
			//{
			//  string tableAlias = "[t0]";
			//  int tIndex = tableList.IndexOf((Table)column.ParentTableRef.Object);
			//  if (tIndex != -1) tableAlias = "[z" + (tIndex + 1) + "]";

			//  sb.Append("					case \"" + column.PascalName.ToLower() + "\": orderByClause += \"" + tableAlias + ".[" + column.DatabaseName + "]\" + (fieldItem.Ascending ? \"\" : \" DESC\"); ");				
			//  if (_currentTable.PrimaryKeyColumns.Count == 1 && (_currentTable.PrimaryKeyColumns[0] == column))
			//    sb.Append("isPrimary = true;");
			//  sb.AppendLine("break;");
			//}
			//if (_currentTable.CreateAudit)
			//{
			//  sb.AppendLine("					case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "\": orderByClause += \"[t0].[" + _model.Database.CreatedDateColumnName + "]\" + (fieldItem.Ascending ? \"\" : \" DESC\"); break;");
			//  sb.AppendLine("					case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "\": orderByClause += \"[t0].[" + _model.Database.CreatedByColumnName + "]\" + (fieldItem.Ascending ? \"\" : \" DESC\"); break;");
			//}
			//if (_currentTable.ModifiedAudit)
			//{
			//  sb.AppendLine("					case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "\": orderByClause += \"[t0].[" + _model.Database.ModifiedDateColumnName + "]\" + (fieldItem.Ascending ? \"\" : \" DESC\"); break;");
			//  sb.AppendLine("					case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "\": orderByClause += \"[t0].[" + _model.Database.ModifiedByColumnName + "]\" + (fieldItem.Ascending ? \"\" : \" DESC\"); break;");
			//}

			//sb.AppendLine("					default: throw new Exception(\"The order by clause is not valid.\");");
			//sb.AppendLine("				}");
			//sb.AppendLine("			}");
			//sb.AppendLine();
			//sb.AppendLine("			string selectClauseOrig = selectClause;");
			//sb.AppendLine();

			////Replace all fields with the appropriate column
			//foreach (string s in columnList.Keys)
			//{
			//  Column column = columnList[s];
			//  string tableAlias = "[t0]";
			//  int tIndex = tableList.IndexOf((Table)column.ParentTableRef.Object);
			//  if (tIndex != -1) tableAlias = "[z" + (tIndex + 1) + "]";

			//  sb.AppendLine("			whereClause = ConfigurationValues.GetSQLTableMap(whereClause, \"[t0].[" + column.DatabaseName + "]\", \"[" + tableAlias + "]\", \"" + column.DatabaseName + "\");");
			//  sb.AppendLine("			selectClause = ConfigurationValues.GetSQLTableMap(selectClause, \"[t0].[" + column.DatabaseName + "]\", \"[" + tableAlias + "]\", \"" + column.DatabaseName + "\");");
			//  sb.AppendLine("			fromClause = ConfigurationValues.GetSQLTableMap(fromClause, \"[t0].[" + column.DatabaseName + "]\", \"[" + tableAlias + "]\", \"" + column.DatabaseName + "\");");

			//  //sb.AppendLine("			whereClause = whereClause.Replace(\"[t0].[" + column.DatabaseName + "]\", \"" + tableAlias + ".[" + column.DatabaseName + "]\");");
			//  //sb.AppendLine("			selectClause = selectClause.Replace(\"[t0].[" + column.DatabaseName + "]\", \"" + tableAlias + ".[" + column.DatabaseName + "]\");");
			//  //sb.AppendLine("			fromClause = fromClause.Replace(\"[t0].[" + column.DatabaseName + "]\", \"" + tableAlias + ".[" + column.DatabaseName + "]\");");
			//}
			//sb.AppendLine();

			//sb.AppendLine("			//Get the paging sub-query");
			//sb.AppendLine("			string selectClauseSubMod = selectClause;");
			//sb.AppendLine("			while (selectClauseSubMod.Contains(\" AS \"))");
			//sb.AppendLine("			{");
			//sb.AppendLine("				int i = selectClauseSubMod.IndexOf(\" AS \");");
			//sb.AppendLine("				int j = selectClauseSubMod.IndexOf(\"]\", i);");
			//sb.AppendLine("				selectClauseSubMod = selectClauseSubMod.Remove(i, j - i + 1);");
			//sb.AppendLine("			}");
			//sb.AppendLine();
			//sb.AppendLine("			string selectClauseSub = selectClauseOrig;");
			//sb.AppendLine("			while (selectClauseSub.Contains(\" AS \"))");
			//sb.AppendLine("			{");
			//sb.AppendLine("				int i = selectClauseSub.IndexOf(\" AS \");");
			//sb.AppendLine("				int j = selectClauseSub.IndexOf(\"]\", i);");
			//sb.AppendLine("				selectClauseSub = selectClauseSub.Remove(i, j - i + 1);");
			//sb.AppendLine("			}");
			//sb.AppendLine();

			//sb.AppendLine("			//Get the primary key for predictable sorts");
			//sb.AppendLine("			string primarySortKey = \"\";");
			//if (_currentTable.PrimaryKeyColumns.Count == 1)
			//{
			//  sb.AppendLine("			primarySortKey = \"[t0].[" + ((Column)_currentTable.PrimaryKeyColumns[0]).DatabaseName + "]\";");
			//}

			//sb.AppendLine();
			//sb.AppendLine("			//Build the execution query");
			//sb.AppendLine("			StringBuilder sb = new StringBuilder();");
			//sb.AppendLine("			sb.AppendLine(selectClauseSub);");
			//sb.AppendLine("			sb.AppendLine(\"FROM (\");");
			//sb.AppendLine("			sb.Append(selectClauseSubMod);");
			//sb.AppendLine("			sb.AppendLine(\", ROW_NUMBER() OVER (ORDER BY \" + orderByClause + ((primarySortKey == \"\") || ((isPrimary && (primarySortKey != \"\"))) ? \"\" : \", \" + primarySortKey) + \") AS Row\");");
			//sb.AppendLine("			sb.AppendLine(fromClause);");
			//sb.AppendLine("			sb.AppendLine(whereClause);");
			//sb.AppendLine("			sb.AppendLine(\") AS [t0]\");");
			//sb.AppendLine("			sb.AppendLine(\"WHERE (Row >= \" + (((paging.PageIndex - 1) * paging.RecordsperPage) + 1) + \") AND (Row <= \" + (paging.PageIndex * paging.RecordsperPage) + \")\");");
			//sb.AppendLine();
			//sb.AppendLine("			cmd.CommandText = sb.ToString();");

			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText, paging);");
			sb.AppendLine("			cmd.CommandText = parser.GetSQL();");

			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			var result = dc.Translate<" + _currentTable.PascalName + "Query>(cmd.ExecuteReader());");
			sb.AppendLine();
			sb.AppendLine("			" + _currentTable.PascalName + "Collection retval = (" + _currentTable.PascalName + "Collection)subDomain[Collections." + _currentTable.PascalName + "Collection];");
			sb.AppendLine("			foreach (" + _currentTable.PascalName + "Query item in result)");
			sb.AppendLine("			{");

			sb.Append("				if (retval.GetItemByPK(");
			foreach (Column pk in _currentTable.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);
				if (_currentTable.PrimaryKeyColumns.IndexOf(pk) < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(") == null)");
			sb.AppendLine("				{");

			sb.Append("					" + _currentTable.PascalName + " newItem = retval.NewItem(");
			foreach (Column pk in _currentTable.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);

				if (_currentTable.PrimaryKeyColumns.IndexOf(pk) < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(");");

			foreach (Column c in _currentTable.GetColumnsFullHierarchy())
			{
				if (c.AllowNull)
					sb.AppendLine("					newItem.wrappedClass[\"" + c.DatabaseName + "\"] = StringHelper.ConvertToDatabase(item." + c.PascalName + ");");
				else
					sb.AppendLine("					newItem.wrappedClass[\"" + c.DatabaseName + "\"] = item." + c.PascalName + ";");
			}

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ");");
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ");");
			}
			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ");");
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ");");
			}
			if (_currentTable.AllowTimestamp)
			{
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "\"] = item." + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + ";");
			}


			sb.AppendLine("					retval.AddItem(newItem);");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine("			retval.wrappedClass.AcceptChanges();");
			sb.AppendLine("			retval.wrappedClass.EndLoadData();");
			sb.AppendLine("			dc.Connection.Close();");
			sb.AppendLine("			paging.RecordCount = GetCount(where);");
			sb.AppendLine("			return retval;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodSelectSearchLINQInherit()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Using the specified Where expression, execute a query against the database to return a result set");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(Expression<Func<" + _currentTable.PascalName + "Query, bool>> where)");
			sb.AppendLine("		{");
			sb.AppendLine("			return RunSelect(where, \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Using the specified Where expression, execute a query against the database to return a result set");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <param name=\"modifier\">The modified audit trail</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(Expression<Func<" + _currentTable.PascalName + "Query, bool>> where, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
			sb.AppendLine();
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentTable.PascalName + "Query> template = dc.GetTable<" + _currentTable.PascalName + "Query>();");
			sb.AppendLine();

			/*
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			*/

			sb.AppendLine("			var cmd = BusinessCollectionPersistableBase.GetCommand<" + _currentTable.PascalName + "Query>(dc, template, where);");
			sb.AppendLine("			cmd.CommandTimeout = ConfigurationValues.GetInstance().DefaultTimeOut;");

			sb.AppendLine();
			//sb.AppendLine("			string sql = cmd.CommandText;");
			//sb.AppendLine();
			//sb.AppendLine("			//Get the where clause");
			//sb.AppendLine("			StringBuilder sb = new StringBuilder();");
			//sb.AppendLine("			int index = sql.IndexOf(\"\\nWHERE \");");
			//sb.AppendLine("			string whereClause = sql.Substring(index, sql.Length - index);");

			//sb.AppendLine();
			//sb.Append("			sql = \"SELECT ");

			//List<Table> tableList = _currentTable.GetTableHierarchy();
			//tableList.Remove(_currentTable);
			//Dictionary<string, Column> columnList = new Dictionary<string, Column>();

			////This is used to create the ItemArray save
			//List<string> identifierList = new List<string>();

			////Need the colums in order from base UP
			//foreach (Table t in tableList)
			//{
			//  foreach (Reference r in t.Columns)
			//  {
			//    Column c = (Column)r.Object;
			//    if (!columnList.ContainsKey(c.DatabaseName.ToLower()))
			//      columnList.Add(c.DatabaseName.ToLower(), c);
			//  }
			//}

			////Add primary Keys
			//foreach (Column c in _currentTable.PrimaryKeyColumns)
			//{
			//  sb.Append("[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + c.DatabaseName + "],");
			//  if (columnList.ContainsKey(c.DatabaseName.ToLower()))
			//    columnList.Remove(c.DatabaseName.ToLower());

			//  identifierList.Add(c.PascalName);
			//}

			////Add columns from base class UP
			//foreach (string s in columnList.Keys)
			//{
			//  Column c = columnList[s];
			//  sb.Append("[" + Globals.GetTableDatabaseName(_model, (Table)c.ParentTableRef.Object) + "].[" + c.DatabaseName + "],");
			//  identifierList.Add(c.PascalName);
			//}

			////add columns for audits
			//if (_currentTable.CreateAudit)
			//{
			//  sb.Append("[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _model.Database.CreatedByColumnName + "],");
			//  sb.Append("[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _model.Database.CreatedDateColumnName + "],");
			//  identifierList.Add(StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName));
			//  identifierList.Add(StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName));
			//}
			//if (_currentTable.ModifiedAudit)
			//{
			//  sb.Append("[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _model.Database.ModifiedByColumnName + "],");
			//  sb.Append("[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _model.Database.ModifiedDateColumnName + "],");
			//  identifierList.Add(StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName));
			//  identifierList.Add(StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName));
			//}
			//if (_currentTable.Timestamp)
			//{
			//  sb.Append("[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _model.Database.TimestampColumnName + "],");
			//  identifierList.Add(StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName));
			//}

			////Add columns for this table
			//foreach (Reference r in _currentTable.GeneratedColumns)
			//{
			//  Column c = (Column)r.Object;
			//  if (!_currentTable.PrimaryKeyColumns.Contains(c))
			//  {
			//    sb.Append("[" + Globals.GetTableDatabaseName(_model, (Table)c.ParentTableRef.Object) + "].[" + c.DatabaseName + "],");
			//    identifierList.Add(c.PascalName);
			//  }
			//}

			//sb.AppendLine("-1 \\n\" +");

			//sb.AppendLine("				\"FROM \" +");
			//sb.AppendLine("				\"" + _currentTable.GetFullHierarchyTableJoin() + "\";");

			////Append where clause

			////Replace all fields with the appropriate column
			//foreach (string s in columnList.Keys)
			//{
			//  Column column = columnList[s];
			//  //sb.AppendLine("			whereClause = whereClause.Replace(\"[t0].[" + c.DatabaseName + "]\", \"[" + Globals.GetTableDatabaseName(_model, (Table)c.ParentTableRef.Object) + "].[" + c.DatabaseName + "]\");");
			//  sb.AppendLine("			whereClause = ConfigurationValues.GetSQLTableMap(whereClause, \"[t0].[" + column.DatabaseName + "]\", \"[" + Globals.GetTableDatabaseName(_model, (Table)column.ParentTableRef.Object) + "]\", \"" + column.DatabaseName + "\");");
			//}
			//sb.AppendLine("			whereClause = whereClause.Replace(\"[t0]\", \"[" + Globals.GetTableDatabaseName(_model, _currentTable) + "]\");");

			//sb.AppendLine();
			//sb.AppendLine("			cmd.CommandText = sql + whereClause;");

			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
			sb.AppendLine("			cmd.CommandText = parser.GetSQL();");
			sb.AppendLine();
			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			var result = dc.Translate<" + _currentTable.PascalName + "Query>(cmd.ExecuteReader());");
			sb.AppendLine();
			sb.AppendLine("			" + _currentTable.PascalName + "Collection retval = (" + _currentTable.PascalName + "Collection)subDomain[Collections." + _currentTable.PascalName + "Collection];");
			sb.AppendLine("			foreach (" + _currentTable.PascalName + "Query item in result)");
			sb.AppendLine("			{");

			sb.Append("			if (retval.GetItemByPK(");
			foreach (Column pk in _currentTable.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);
				if (_currentTable.PrimaryKeyColumns.IndexOf(pk) < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(") == null)");
			sb.AppendLine("			{");

			sb.Append("				" + _currentTable.PascalName + " newItem = retval.NewItem(");
			foreach (Column pk in _currentTable.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);

				if (_currentTable.PrimaryKeyColumns.IndexOf(pk) < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(");");

			foreach (Column c in _currentTable.GetColumnsFullHierarchy())
			{
				if (c.AllowNull)
					sb.AppendLine("				newItem.wrappedClass[\"" + c.DatabaseName + "\"] = StringHelper.ConvertToDatabase(item." + c.PascalName + ");");
				else
					sb.AppendLine("				newItem.wrappedClass[\"" + c.DatabaseName + "\"] = item." + c.PascalName + ";");
			}

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ");");
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ");");
			}
			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ");");
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ");");
			}
			if (_currentTable.AllowTimestamp)
			{
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "\"] = item." + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + ";");
			}


			sb.AppendLine("				retval.AddItem(newItem);");
			sb.AppendLine("			}");
			sb.AppendLine("			}");
			sb.AppendLine("			retval.wrappedClass.AcceptChanges();");
			sb.AppendLine("			retval.wrappedClass.EndLoadData();");
			sb.AppendLine("			dc.Connection.Close();");
			sb.AppendLine("			return retval;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodSelectPaged()
		{
			if (_model.Database.AllowZeroTouch) return;

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(int page, int pageSize, " + _currentTable.PascalName + ".FieldNameConstants orderByColumn, bool ascending, string filter, out int count)");
			sb.AppendLine("		{");
			sb.AppendLine("		  return RunSelect(page, pageSize, orderByColumn.ToString(), ascending, filter, out count, \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(int page, int pageSize, " + _currentTable.PascalName + ".FieldNameConstants orderByColumn, bool ascending, string filter, out int count, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("		  return RunSelect(page, pageSize, orderByColumn.ToString(), ascending, filter, out count, modifier);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database");
			sb.AppendLine("		/// </summary>");
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter, out int count)");
			else
				sb.AppendLine("		public new static " + _currentTable.PascalName + "Collection RunSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter, out int count)");
			sb.AppendLine("		{");
			sb.AppendLine("		  return RunSelect(page, pageSize, orderByColumn, ascending, filter, out count, \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database");
			sb.AppendLine("		/// </summary>");
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter, out int count, string modifier)");
			else
				sb.AppendLine("		public new static " + _currentTable.PascalName + "Collection RunSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter, out int count, string modifier)");

			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				int returnCount;");
			sb.AppendLine("				" + _currentTable.PascalName + "Collection returnVal = new " + _currentTable.PascalName + "Collection(Domain" + _currentTable.PascalName + "Collection.RunSelect(page, pageSize, orderByColumn, ascending, filter, out returnCount, modifier));");
			sb.AppendLine("				count = returnCount;");
			sb.AppendLine("				return returnVal;");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodSelectAll()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select all objects from store.");
			sb.AppendLine("		/// </summary>");
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect()");
			else
				sb.AppendLine("		public new static " + _currentTable.PascalName + "Collection RunSelect()");
			sb.AppendLine("		{");
			sb.AppendLine("			return RunSelect(\"\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select all objects from store.");
			sb.AppendLine("		/// </summary>");
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public static " + _currentTable.PascalName + "Collection RunSelect(string modifier)");
			else
				sb.AppendLine("		public new static " + _currentTable.PascalName + "Collection RunSelect(string modifier)");

			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			if (_model.Database.AllowZeroTouch)
			{
				//LINQ select all (just match)
				sb.AppendLine("				return " + _currentTable.PascalName + "Collection.RunSelect(x => true);");
			}
			else
			{
				sb.AppendLine("				" + _currentTable.PascalName + "Collection returnVal = new " + _currentTable.PascalName + "Collection(Domain" + _currentTable.PascalName + "Collection.RunSelect(modifier));");
				sb.AppendLine("				return returnVal;");
			}
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodPersist()
		{
			if (_currentTable.Immutable)
				return;

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Persists this collection to store.");
			sb.AppendLine("		/// </summary>");

			if ((_currentTable.ParentTable == null) || (_currentTable.NeedOverridePersistable))
				sb.AppendLine("		public override void Persist()");
			else
				sb.AppendLine("		public new void Persist()");

			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");

			sb.AppendLine("				foreach (" + _currentTable.PascalName + " item in this)");
			sb.AppendLine("				{");
			sb.AppendLine("					if (item.wrappedClass.RowState != DataRowState.Unchanged)");
			sb.AppendLine("						item.OnValidate(new BusinessObjectEventArgs(item));");
			sb.AppendLine("				}"); sb.AppendLine();

			sb.AppendLine("				lock (wrappedClass.SubDomain)");
			sb.AppendLine("				{");
			sb.AppendLine("					wrappedClass.Persist();");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodAddNewItem()
		{
			string scope = "public";
			if (_currentTable.Immutable)
				scope = "protected internal";

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Add a newly created entity to this collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		" + scope + " virtual void AddItem(" + _currentTable.PascalName + " " + _currentTable.CamelName + ") ");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				lock (wrappedClass.SubDomain)");
			sb.AppendLine("				{");
			sb.AppendLine("					wrappedClass.Add" + _currentTable.PascalName + "((Domain" + _currentTable.PascalName + ")" + _currentTable.CamelName + ".WrappedClass);");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodVisitor()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Takes an IVisitor object and iterates through each item in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"visitor\">The object that processes each collection item</param>");
			sb.AppendLine("		public " + (_currentTable.ParentTable == null ? "virtual" : "new") + " void ProcessVisitor(IVisitor visitor)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (visitor == null) throw new Exception(\"This object cannot be null.\");");
			sb.AppendLine("			lock(this)");
			sb.AppendLine("			{");
			sb.AppendLine("				foreach (IBusinessObject item in this)");
			sb.AppendLine("				{");
			sb.AppendLine("					visitor.Visit(item);");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendStaticSQLMethods()
		{
			sb.AppendLine("		#region Static SQL Methods");
			sb.AppendLine();

			#region GetTableFromFieldAliasSqlMapping
			sb.AppendLine("		internal " + (_currentTable.ParentTable == null ? "" : "new ") + "static string GetTableFromFieldAliasSqlMapping(string alias)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (alias.ToLower())");
			sb.AppendLine("			{");

			ColumnCollection allColumns = _currentTable.GetColumnsFullHierarchy(true);
			foreach (Column column in allColumns)
			{
				sb.AppendLine("				case \"" + column.PascalName.ToLower() + "\": return \"" + ((Table)column.ParentTableRef.Object).DatabaseName + "\";");
			}

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "\": return \"" + _currentTable.DatabaseName + "\";");
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "\": return \"" + _currentTable.DatabaseName + "\";");
			}
			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "\": return \"" + _currentTable.DatabaseName + "\";");
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "\": return \"" + _currentTable.DatabaseName + "\";");
			}
			if (_currentTable.AllowTimestamp)
			{
				sb.AppendLine("				case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName).ToLower() + "\": return \"" + _currentTable.DatabaseName + "\";");
			}

			sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			#endregion

			#region GetTableFromFieldNameSqlMapping
			sb.AppendLine("		internal " + (_currentTable.ParentTable == null ? "" : "new ") + "static string GetTableFromFieldNameSqlMapping(string field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field.ToLower())");
			sb.AppendLine("			{");

			foreach (Column column in allColumns)
			{
				sb.AppendLine("				case \"" + column.DatabaseName.ToLower() + "\": return \"" + ((Table)column.ParentTableRef.Object).DatabaseName + "\";");
			}

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("				case \"" + _model.Database.CreatedByColumnName.ToLower() + "\": return \"" + _currentTable.DatabaseName + "\";");
				sb.AppendLine("				case \"" + _model.Database.CreatedDateColumnName.ToLower() + "\": return \"" + _currentTable.DatabaseName + "\";");
			}
			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("				case \"" + _model.Database.ModifiedByColumnName.ToLower() + "\": return \"" + _currentTable.DatabaseName + "\";");
				sb.AppendLine("				case \"" + _model.Database.ModifiedDateColumnName.ToLower() + "\": return \"" + _currentTable.DatabaseName + "\";");
			}
			if (_currentTable.AllowTimestamp)
			{
				sb.AppendLine("				case \"" + _model.Database.TimestampColumnName.ToLower() + "\": return \"" + _currentTable.DatabaseName + "\";");
			}

			sb.AppendLine("				default: throw new Exception(\"The select clause is not valid.\");");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			#endregion

			#region GetRemappedLinqSql
			sb.AppendLine("		internal " + (_currentTable.ParentTable == null ? "" : "new ") + "static string GetRemappedLinqSql(string sql, string parentAlias, LinqSQLFromClauseCollection childTables)");
			sb.AppendLine("		{");
			foreach (Column column in allColumns)
			{				
				//sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + column.DatabaseName.ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + ((Table)column.ParentTableRef.Object).DatabaseName + "\") + \"].[" + column.DatabaseName.ToLower() + "]\");");
				sb.AppendLine("			sql = Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + column.DatabaseName.ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + ((Table)column.ParentTableRef.Object).DatabaseName + "\") + \"].[" + column.DatabaseName.ToLower() + "]\", RegexOptions.IgnoreCase);");
			}
			if (_currentTable.AllowCreateAudit)
			{
				//sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "]\");");
				//sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "]\");");
				sb.AppendLine("			sql = Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "]\", RegexOptions.IgnoreCase);");
				sb.AppendLine("			sql = Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "]\", RegexOptions.IgnoreCase);");
			}
			if (_currentTable.AllowModifiedAudit)
			{
				//sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "]\");");
				//sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "]\");");
				sb.AppendLine("			sql = Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "]\", RegexOptions.IgnoreCase);");
				sb.AppendLine("			sql = Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "]\", RegexOptions.IgnoreCase);");
			}
			if (_currentTable.AllowTimestamp)
			{
				//sb.AppendLine("			sql = sql.Replace(\"[\" + parentAlias + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName).ToLower() + "]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName).ToLower() + "]\");");
				sb.AppendLine("			sql = Regex.Replace(sql, \"\\\\[\" + parentAlias + \"\\\\]\\\\.\\\\[" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName).ToLower() + "\\\\]\", \"[\" + childTables.GetBaseAliasTable(parentAlias, \"" + _currentTable.DatabaseName + "\") + \"].[" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName).ToLower() + "]\", RegexOptions.IgnoreCase);");
			}

			sb.AppendLine("			return sql;");
			sb.AppendLine("		}");
			sb.AppendLine();
			#endregion

			sb.AppendLine("		#endregion");
			sb.AppendLine();

			//GetPagedSQL
			sb.AppendLine("		internal " + (_currentTable.ParentTable == null ? "" : "new ") + "static string GetPagedSQL(List<LinqSQLField> fieldList, LinqSQLFromClauseCollection fromLinkList, string whereClause, object paging)");
			sb.AppendLine("		{");
			sb.AppendLine("			StringBuilder sb = new StringBuilder();");
			sb.AppendLine("			" + _currentTable.PascalName + "Paging paging2 = (" + _currentTable.PascalName + "Paging)paging;");
			sb.AppendLine();
			sb.AppendLine("			//Calculate the SELECT clause");
			sb.AppendLine("			sb.Append(\"SELECT \");");
			sb.AppendLine("			int index = 0;");
			sb.AppendLine("			foreach (LinqSQLField field in fieldList)");
			sb.AppendLine("			{");
			sb.AppendLine("				sb.Append(\"[t0].[\" + field.Name + \"]\");");
			sb.AppendLine("				if (index < fieldList.Count - 1) sb.Append(\", \");");
			sb.AppendLine("				index++;");
			sb.AppendLine("			}");
			sb.AppendLine("			sb.AppendLine();");
			sb.AppendLine("			sb.AppendLine(\"FROM (\");");
			sb.AppendLine();
			sb.AppendLine("			//Calculate the Inner SELECT clause");
			sb.AppendLine("			sb.Append(\"SELECT \");");
			sb.AppendLine("			index = 0;");
			sb.AppendLine("			foreach (LinqSQLField field in fieldList)");
			sb.AppendLine("			{");
			sb.AppendLine("				sb.Append(field.GetSQL(false));");
			sb.AppendLine("				if (index < fieldList.Count - 1) sb.Append(\", \");");
			sb.AppendLine("				index++;");
			sb.AppendLine("			}");
			sb.AppendLine("			sb.AppendLine();");

			//Add the paging column determination code
			sb.AppendLine("			//Determine the paging sort");
			sb.AppendLine("			bool isPrimary = false;");
			sb.AppendLine("			string orderByClause = \"\";");
			sb.AppendLine("			foreach (" + _currentTable.PascalName + "PagingFieldItem fieldItem in paging2.OrderByList)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (orderByClause != \"\") orderByClause += \", \";");
			sb.AppendLine("				switch (fieldItem.Field.ToString().ToLower())");
			sb.AppendLine("				{");
			foreach (Column column in allColumns)
			{
				sb.Append("					case \"" + column.PascalName.ToLower() + "\": orderByClause += \"[\" + (from x in fieldList where x.Alias == fieldItem.Field.ToString() select x).FirstOrDefault().Table + \"].[" + column.DatabaseName + "]\" + (fieldItem.Ascending ? \"\" : \" DESC\"); ");
				if (_currentTable.PrimaryKeyColumns.Count == 1 && (_currentTable.PrimaryKeyColumns[0] == column))
					sb.Append("isPrimary = true;");
				sb.AppendLine("break;");
			}
			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("					case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName).ToLower() + "\": orderByClause += \"[t0].[" + _model.Database.CreatedDateColumnName + "]\" + (fieldItem.Ascending ? \"\" : \" DESC\"); break;");
				sb.AppendLine("					case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName).ToLower() + "\": orderByClause += \"[t0].[" + _model.Database.CreatedByColumnName + "]\" + (fieldItem.Ascending ? \"\" : \" DESC\"); break;");
			}
			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("					case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName).ToLower() + "\": orderByClause += \"[t0].[" + _model.Database.ModifiedDateColumnName + "]\" + (fieldItem.Ascending ? \"\" : \" DESC\"); break;");
				sb.AppendLine("					case \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName).ToLower() + "\": orderByClause += \"[t0].[" + _model.Database.ModifiedByColumnName + "]\" + (fieldItem.Ascending ? \"\" : \" DESC\"); break;");
			}

			sb.AppendLine("					default: throw new Exception(\"The order by clause is not valid.\");");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine();

			sb.AppendLine("			//Get the primary key for predictable sorts");
			sb.AppendLine("			string primarySortKey = \"\";");
			if (_currentTable.PrimaryKeyColumns.Count == 1)
			{
				sb.AppendLine("			primarySortKey = \"[t0].[" + ((Column)_currentTable.PrimaryKeyColumns[0]).DatabaseName + "]\";");
			}

			sb.AppendLine();
			sb.AppendLine("			sb.AppendLine(\", ROW_NUMBER() OVER (ORDER BY \" + orderByClause + ((primarySortKey == \"\") || ((isPrimary && (primarySortKey != \"\"))) ? \"\" : \", \" + primarySortKey) + \") AS Row\");");

			sb.AppendLine("			//Calculate the FROM clause");
			sb.AppendLine("			index = 0;");
			sb.AppendLine("			sb.Append(\"FROM \");");
			sb.AppendLine("			foreach (LinqSQLFromClause fromClause in fromLinkList)");
			sb.AppendLine("			{");
			sb.AppendLine("				sb.Append(\"[\" + fromClause.TableName + \"] AS [\" + fromClause.Alias + \"] \");");
			sb.AppendLine("				if (fromClause.LinkClause != \"\") sb.Append(fromClause.LinkClause + \" \");");
			sb.AppendLine();
			sb.AppendLine("				if (index < fromLinkList.Count - 1)");
			sb.AppendLine("				{");
			sb.AppendLine("					sb.AppendLine();");
			sb.AppendLine("					sb.Append(\"LEFT OUTER JOIN \");");
			sb.AppendLine("				}");
			sb.AppendLine();
			sb.AppendLine("				index++;");
			sb.AppendLine("			}");
			sb.AppendLine("			sb.AppendLine();");
			sb.AppendLine();
			sb.AppendLine("			//Calculate the WHERE clause");
			sb.AppendLine("			if (whereClause != \"\")");
			sb.AppendLine("			{");
			sb.AppendLine("				foreach (LinqSQLFromClause fromClause in fromLinkList)");
			sb.AppendLine("				{");
			sb.AppendLine("					//Only process table that were original and not inserted above");
			sb.AppendLine("					if (fromClause.AnchorAlias == \"\")");
			sb.AppendLine("					{");

			int index = 0;
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.Generated)
				{
					sb.Append("						");
					if (index > 0) sb.Append("else ");
					sb.Append("if (fromClause.TableName == \"" + table.DatabaseName + "\")");
					sb.AppendLine(" whereClause = " + table.PascalName + "Collection.GetRemappedLinqSql(whereClause, fromClause.Alias, fromLinkList);");
					index++;
				}
			}

			sb.AppendLine("					}");
			sb.AppendLine("				}");
			sb.AppendLine("				sb.Append(\"WHERE \" + whereClause);");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("			sb.AppendLine(\") AS [t0]\");");
			sb.AppendLine("			sb.AppendLine(\"WHERE (Row >= \" + (((paging2.PageIndex - 1) * paging2.RecordsperPage) + 1) + \") AND (Row <= \" + (paging2.PageIndex * paging2.RecordsperPage) + \")\");");
			sb.AppendLine();
			sb.AppendLine("			return sb.ToString();");
			sb.AppendLine("		}");
			sb.AppendLine();

		}

		#endregion

		#region append IList Implementation

		private void AppendIListImplementation()
		{
			sb.AppendLine("		#region IList Implementation");
			sb.AppendLine();

			if (_currentTable.ParentTable == null)
			{
				sb.AppendLine("		public bool ContainsListCollection { get { return false; } }");
				sb.AppendLine();
				sb.AppendLine("		public IList GetList()");
				sb.AppendLine("		{");
				sb.AppendLine("		 IList arrayList = new ArrayList();");
				sb.AppendLine();
				sb.AppendLine("			foreach (object o in GetBusinessObjectList())");
				sb.AppendLine("			{");
				sb.AppendLine("				arrayList.Add(o);");
				sb.AppendLine("			}");
				sb.AppendLine();
				sb.AppendLine("			return arrayList;");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public virtual BusinessObjectList<" + _currentTable.PascalName + "> GetBusinessObjectList()");
			else
				sb.AppendLine("		public new BusinessObjectList<" + _currentTable.PascalName + "> GetBusinessObjectList()");

			sb.AppendLine("		{");
			sb.AppendLine("			BusinessObjectList<" + _currentTable.PascalName + "> retVal = new BusinessObjectList<" + _currentTable.PascalName + ">();");
			sb.AppendLine("			foreach (" + _currentTable.PascalName + " currentVal in this)");
			sb.AppendLine("			{");
			sb.AppendLine("				retVal.Add(currentVal);");
			sb.AppendLine("			}");
			sb.AppendLine("			return retVal;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

		#region append operator overloads

		private void AppendOperatorIndexer()
		{
			sb.AppendLine("		#region Enumerator");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets or sets the element at the specified index.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"index\">The zero-based index of the element to get or set. </param>");
			sb.AppendLine("		/// <returns>The element at the specified index.</returns>");

			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public virtual " + _currentTable.PascalName + " this[int index]");
			else
				sb.AppendLine("		public new " + _currentTable.PascalName + " this[int index]");

			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				try");
			sb.AppendLine("				{");
			sb.AppendLine("					IEnumerator internalEnumerator = this.GetEnumerator();");
			sb.AppendLine("					internalEnumerator.Reset();");
			sb.AppendLine("					int ii = -1;");
			sb.AppendLine("					while(ii < index)");
			sb.AppendLine("					{");
			sb.AppendLine("						internalEnumerator.MoveNext();");
			sb.AppendLine("						ii++;");
			sb.AppendLine("					}");
			//sb.AppendLine("					return (" + _currentTable.PascalName + ")internalEnumerator.Current;");
			sb.AppendLine("					" + _currentTable.PascalName + " retval = (" + _currentTable.PascalName + ")internalEnumerator.Current;");
			sb.AppendLine("					if (retval.wrappedClass == null)");
			sb.AppendLine("					{");
			sb.AppendLine("						if (!((0 <= index) && (index < this.Count)))");
			sb.AppendLine("							throw new IndexOutOfRangeException();");
			sb.AppendLine("						else");
			sb.AppendLine("							throw new Exception(\"The item is null. This is not a valid state.\");");
			sb.AppendLine("					}");
			sb.AppendLine("					else return (" + _currentTable.PascalName + ")internalEnumerator.Current;");
			sb.AppendLine("				}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

		#region Reject Changes

		private void AppendRejectChanges()
		{
			sb.AppendLine("		#region Reject Changes");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Rejects all the changes for all objects in this collecion, since the last load");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns>void</returns>");

			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public virtual void RejectChanges()");
			else
				sb.AppendLine("		public override void RejectChanges()");

			sb.AppendLine("		{");
			sb.Append("		  Domain" + _currentTable.PascalName + "Collection coll = (Domain" + _currentTable.PascalName + "Collection)this.wrappedClass;");
			sb.Append("		  coll.RejectChanges();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

		#region AppendResetWrappedClass

		private void AppendResetWrappedClass()
		{
			sb.AppendLine("		internal virtual void ResetWrappedClass(Domain" + _currentTable.PascalName + "Collection wc)");
			sb.AppendLine("		{");
			sb.AppendLine("			wrappedClass = wc;");
			if (_currentTable.ParentTable != null)
			{
				sb.AppendLine("			base.ResetWrappedClass(wc);");
			}
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		#endregion

		#region stringbuilders

		private string GetFieldSQL(List<string> fieldSQLList)
		{
			StringBuilder retval = new StringBuilder();
			foreach (string s in fieldSQLList)
			{
				retval.Append(s);
				if (fieldSQLList.IndexOf(s) < fieldSQLList.Count - 1)
					retval.Append(",");
			}
			return retval.ToString();
		}

		protected string PrimaryKeyParameterList(bool includeDataType)
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Column dc in _currentTable.PrimaryKeyColumns)
				{
					if (includeDataType) output.Append(dc.GetCodeType() + " ");
					output.Append(dc.CamelName);
					output.Append(", ");
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
				foreach (Column dc in _currentTable.PrimaryKeyColumns)
				{
					output.Append(dc.CamelName);
					output.Append(", ");
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

		protected string PrimaryKeyColumnList()
		{
			StringBuilder output = new StringBuilder();
			foreach (Column dc in _currentTable.PrimaryKeyColumns)
			{
				output.Append("this.column");
				output.Append(dc.PascalName);
				output.Append(", ");
			}
			if (output.Length > 2)
			{
				output.Remove(output.Length - 2, 2);
			}
			return output.ToString();
		}

		protected string GetDefaultForRequiredParam(string codeType)
		{
			if (StringHelper.Match(codeType, "long", true))
			{
				return "long.MinValue";
			}
			else if (StringHelper.Match(codeType, "System.Byte[]", true))
			{
				return "new byte[1]";
			}
			else if (StringHelper.Match(codeType, "bool", true))
			{
				return "false";
			}
			else if (StringHelper.Match(codeType, "string", true))
			{
				return "string.Empty";
			}
			else if (StringHelper.Match(codeType, "System.DateTime", true))
			{
				return "DateTime.MinValue";
			}
			else if (StringHelper.Match(codeType, "System.Decimal", true))
			{
				return "Decimal.MinValue";
			}
			else if (StringHelper.Match(codeType, "System.Double", true))
			{
				return "Double.MinValue";
			}
			else if (StringHelper.Match(codeType, "int", true))
			{
				return "int.MinValue";
			}
			else if (StringHelper.Match(codeType, "System.Single", true))
			{
				return "Single.MinValue";
			}
			else if (StringHelper.Match(codeType, "short", true))
			{
				return "short.MinValue";
			}
			else if (StringHelper.Match(codeType, "object", true))
			{
				return "new object";
			}
			else if (StringHelper.Match(codeType, "System.Byte", true))
			{
				return "System.Byte.MinValue";
			}
			else if (StringHelper.Match(codeType, "System.Guid", true))
			{
				return "System.Guid.Empty";
			}
			else
			{
				throw new Exception("No Default Value For Type Specified");
			}

		}

		protected string SetInitialValues()
		{
			//TODO - Audit Trail not implemented
			string setModifiedBy = String.Format("//" + "\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "Column] = Modifier;", _currentTable.PascalName);
			string setModifiedDate = String.Format("//" + "\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "Column] = " + Globals.GetDateTimeNowCode(_model) + ";", _currentTable.PascalName);
			string setCreatedBy = String.Format("//" + "\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "Column] = Modifier;", _currentTable.PascalName);
			string setCreatedDate = String.Format("//" + "\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "Column] = " + Globals.GetDateTimeNowCode(_model) + ";", _currentTable.PascalName);
			string setOriginalGuid = String.Format("\t\t\t((DataRow)return{0})[{0}GuidColumn] = Guid.NewGuid().ToString().ToUpper();", _currentTable.PascalName);
			string setOriginalGuidWhenUniqueIdentifier = String.Format("\t\t\t((DataRow)return{0})[{0}GuidColumn] = Guid.NewGuid();", _currentTable.PascalName);

			StringBuilder returnVal = new StringBuilder();

			if (_currentTable.AllowModifiedAudit)
			{
				returnVal.AppendLine(setModifiedBy);
				returnVal.AppendLine(setModifiedDate);
			}

			if (_currentTable.AllowCreateAudit)
			{
				returnVal.AppendLine(setCreatedBy);
				returnVal.AppendLine(setCreatedDate);
			}

			if (_currentTable.PrimaryKeyColumns.Count == 1)
			{
				string actualKeyName = ((Column)_currentTable.PrimaryKeyColumns[0]).DatabaseName;
				string guidKeyName = _currentTable.DatabaseName.ToLower() + "_guid";
				if (StringHelper.Match(actualKeyName, guidKeyName, true))
				{
					if (StringHelper.Match(((Column)_currentTable.PrimaryKeyColumns[0]).GetCodeType(), "System.Guid", false))
					{
						returnVal.AppendLine(setOriginalGuidWhenUniqueIdentifier);
					}
					else
					{
						returnVal.AppendLine(setOriginalGuid);
					}
				}
			}
			return returnVal.ToString();
		}

		#endregion

	}

}