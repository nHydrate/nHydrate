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
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;
using System.Collections;

namespace Widgetsphere.Generator.ProjectItemGenerators.BusinessComponentCollection
{
	class BusinessComponentCollectionGeneratedTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private TableComponent _currentComponent;

		public BusinessComponentCollectionGeneratedTemplate(ModelRoot model, TableComponent currentComponent)
		{
			_model = model;
			_currentComponent = currentComponent;
		}

		#region BaseClassTemplate overrides

		public override string FileName
		{
			get { return string.Format("{0}Collection.Generated.cs", _currentComponent.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}Collection.cs", _currentComponent.PascalName); }
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
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.Objects.Components");
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
			sb.Append("using System.ComponentModel;").AppendLine();
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
			sb.AppendLine();
		}

		private void AppendClass()
		{

			string baseClass = "BusinessCollectionPersistableBase";
			string baseInterface = "IPersistableBusinessCollection";

			if (_currentComponent.Parent.Immutable)
			{
				baseClass = "BusinessCollectionBase";
				baseInterface = "IBusinessCollection";
			}

			sb.AppendLine();
			sb.AppendLine("	 /// <summary>");
			sb.AppendLine("	 /// The class representing the '" + _currentComponent.DatabaseName + "' database table");
			if (_currentComponent.Description != "")
				sb.AppendLine("	 /// " + _currentComponent.Description);
			sb.AppendLine("	 /// </summary>");
			sb.AppendLine("	 [Serializable()]");
			sb.AppendLine("	 public partial class " + _currentComponent.PascalName + "Collection : " + baseClass + ", " + baseInterface + ", IDisposable, IEnumerator, IEnumerable<" + _currentComponent.PascalName + ">");
			sb.AppendLine("	 {");
			sb.AppendLine();
			this.AppendMemberVariables();
			this.AppendConstructors();
			this.AppendProperties();
			this.AppendOperatorIndexer();
			this.AppendMethods();			
			this.AppendRegionIBusinessCollectionExplicit();
			this.AppendRegionGetFilteredList();
			this.AppendRegionGetDatabaseFieldName();
			this.AppendClassEnumerator();
			this.AppendRegionSearch();
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
			//sb.AppendLine("		void IBusinessCollection.RunSelect(IBusinessObjectSearch search)");
			//sb.AppendLine("		{");
			//sb.AppendLine("			this.RunSelect((" + _currentTable.PascalName + "Search)search);");
			//sb.AppendLine("		}");
			//sb.AppendLine();
			//sb.AppendLine("		void IBusinessCollection.RunSelect()");
			//sb.AppendLine("		{");
			//sb.AppendLine("			this.RunSelect();");
			//sb.AppendLine("		}");
			//sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a search object to query this collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public virtual " + _currentComponent.PascalName + "Search CreateSearchObject(SearchType searchType)");

			sb.AppendLine("		{");
			sb.AppendLine("			return new " + _currentComponent.PascalName + "Search(searchType);");
			sb.AppendLine("		}");
			sb.AppendLine();
			//sb.AppendLine("		/// <summary>");
			//sb.AppendLine("		/// Get all objects from the database that match the specified search object.");
			//sb.AppendLine("		/// </summary>");
			//sb.AppendLine("		public void RunSelect(" + _currentTable.PascalName + "Search search)");
			//sb.AppendLine("		{");
			//sb.AppendLine("			if (search == null) throw new Exception(\"The 'Search' object cannot be null!\");");
			//sb.AppendLine("			this.SubDomain.AddSelectCommand(new " + _currentTable.PascalName + "SelectBySearch(search));");
			//sb.AppendLine("			this.SubDomain.RunSelectCommands();");
			//sb.AppendLine("		}");
			//sb.AppendLine();

			//sb.AppendLine("		/// <summary>");
			//sb.AppendLine("		/// Get all objects from the database.");
			//sb.AppendLine("		/// </summary>");
			//if (_currentTable.ParentTable == null)
			//  sb.AppendLine("		public virtual void RunSelect()");
			//else
			//  sb.AppendLine("		public override void RunSelect()");

			//sb.AppendLine("		{");
			//sb.AppendLine("			this.SubDomain.AddSelectCommand(new " + _currentTable.PascalName + "SelectAll());");
			//sb.AppendLine("			this.SubDomain.RunSelectCommands();");
			//sb.AppendLine("		}");
			//sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionSelectByDates()
		{
			sb.AppendLine("		#region Select By Dates");
			sb.AppendLine();

			if (_currentComponent.Parent.AllowCreateAudit)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by their created date.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"startDate\">The inclusive date on which to start the search.</param>");
				sb.AppendLine("		/// <param name=\"endDate\">The inclusive date on which to end the search.</param>");
				sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
				sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection SelectByCreatedDateRange(DateTime? startDate, DateTime? endDate, string modifier)");
				sb.AppendLine("		{");
				sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
				sb.AppendLine("			" + _currentComponent.PascalName + "SelectByCreatedDateRange selectCommand = new " + _currentComponent.PascalName + "SelectByCreatedDateRange(startDate, endDate);");
				sb.AppendLine("			subDomain.AddSelectCommand(selectCommand);");
				sb.AppendLine("			subDomain.RunSelectCommands();");
				sb.AppendLine("			return (" + _currentComponent.PascalName + "Collection)subDomain[Collections." + _currentComponent.PascalName + "Collection];");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select objects by their modified date.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		/// <param name=\"startDate\">The inclusive date on which to start the search.</param>");
				sb.AppendLine("		/// <param name=\"endDate\">The inclusive date on which to end the search.</param>");
				sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
				sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection SelectByModifiedDateRange(DateTime? startDate, DateTime? endDate, string modifier)");
				sb.AppendLine("		{");
				sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
				sb.AppendLine("			" + _currentComponent.PascalName + "SelectByModifiedDateRange selectCommand = new " + _currentComponent.PascalName + "SelectByModifiedDateRange(startDate, endDate);");
				sb.AppendLine("			subDomain.AddSelectCommand(selectCommand);");
				sb.AppendLine("			subDomain.RunSelectCommands();");
				sb.AppendLine("			return (" + _currentComponent.PascalName + "Collection)subDomain[Collections." + _currentComponent.PascalName + "Collection];");
				sb.AppendLine("		}");
				sb.AppendLine();
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
			sb.AppendLine("		#region IEnumerable<" + _currentComponent.DatabaseName + "> Members");
			sb.AppendLine();
			sb.AppendLine("		IEnumerator<" + _currentComponent.PascalName + "> IEnumerable<" + _currentComponent.PascalName + ">.GetEnumerator()");
			sb.AppendLine("		{");
			sb.AppendLine("			List<" + _currentComponent.PascalName + "> retval = new List<" + _currentComponent.PascalName + ">();");
			sb.AppendLine("			foreach (" + _currentComponent.PascalName + " item in this)");
			sb.AppendLine("				retval.Add(item);");
			sb.AppendLine("			return retval.GetEnumerator();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region IPersistableBusinessCollection Members");
			sb.AppendLine();
			sb.AppendLine("		IPersistableBusinessObject IPersistableBusinessCollection.this[int index]");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this[index]; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionIBusinessCollectionExplicit()
		{
			if (!_currentComponent.Parent.Immutable)
			{
				sb.AppendLine("		#region IPersistableBusinessCollection Explicit Members");
				sb.AppendLine();
				//sb.AppendLine("		/// <summary>");
				//sb.AppendLine("		/// Returns an item in this collection by index.");
				//sb.AppendLine("		/// </summary>");
				//sb.AppendLine("		/// <param name=\"index\">The zero-based index of the element to get or set. </param>");
				//sb.AppendLine("		/// <returns>The element at the specified index.</returns>");
				//sb.AppendLine("		IPersistableBusinessObject IPersistableBusinessCollection.this[int index]");
				//sb.AppendLine("		{");
				//sb.AppendLine("			get { return this[index]; }");
				//sb.AppendLine("		}");
				//sb.AppendLine();
				sb.AppendLine("		void IPersistableBusinessCollection.AddItem(IPersistableBusinessObject newItem)");
				sb.AppendLine("		{");
				sb.AppendLine("			this.AddItem((" + _currentComponent.PascalName + ")newItem);");
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
			sb.AppendLine("		return typeof(" + _currentComponent.PascalName + ");");
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
			sb.AppendLine("		public BusinessObjectList<" + _currentComponent.PascalName + "> GetFilteredList(" + _currentComponent.PascalName + ".FieldNameConstants field, object value)");
			sb.AppendLine("		{");
			sb.AppendLine("			BusinessObjectList<" + _currentComponent.PascalName + "> retval = new BusinessObjectList<" + _currentComponent.PascalName + ">();");
			sb.AppendLine("			if (value == null)");
			sb.AppendLine("				return retval;");
			sb.AppendLine();
			sb.AppendLine("			string fieldName = \"\";");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;
				sb.AppendLine("				case " + _currentComponent.PascalName + ".FieldNameConstants." + column.PascalName + ": fieldName = \"" + column.DatabaseName + "\"; break;");
			}

			if (_currentComponent.Parent.AllowCreateAudit)
			{
				sb.AppendLine("				case " + _currentComponent.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ": fieldName = \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "\"; break;");
				sb.AppendLine("				case " + _currentComponent.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ": fieldName = \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "\"; break;");
			}

			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendLine("				case " + _currentComponent.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ": fieldName = \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "\"; break;");
				sb.AppendLine("				case " + _currentComponent.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ": fieldName = \"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "\"; break;");
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
			sb.AppendLine("				retval.Add(new " + _currentComponent.PascalName + "((Domain" + _currentComponent.PascalName + ")dr));");
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
			sb.AppendLine("		internal static string GetDatabaseFieldName(" + _currentComponent.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;
				if (column.Generated)
					sb.AppendLine("				case " + _currentComponent.PascalName + ".FieldNameConstants." + column.PascalName + ": return \"" + column.Name + "\";");
			}

			if (_currentComponent.Parent.AllowCreateAudit)
			{
				sb.AppendLine("				case " + _currentComponent.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ": return \"" + _model.Database.CreatedByColumnName + "\";");
				sb.AppendLine("				case " + _currentComponent.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ": return \"" + _model.Database.CreatedDateColumnName + "\";");
			}

			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendLine("				case " + _currentComponent.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ": return \"" + _model.Database.ModifiedByColumnName + "\";");
				sb.AppendLine("				case " + _currentComponent.PascalName + ".FieldNameConstants." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ": return \"" + _model.Database.ModifiedDateColumnName + "\";");
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
			ArrayList validColumns = GetValidSearchColumns();
			if (validColumns.Count != 0)
			{
				sb.AppendLine("	#region " + _currentComponent.PascalName + "Search");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The search object for the " + _currentComponent.PascalName + "Collection.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	public class " + _currentComponent.PascalName + "Search : IBusinessObjectSearch");
				sb.AppendLine("	{");
				sb.AppendLine();
				sb.AppendLine("		private int _maxRowCount = 0;");
				foreach (Column dc in validColumns)
				{
					sb.AppendFormat("		private " + dc.GetCodeType(true, true) + " _{0};", dc.CamelName).AppendLine();
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
				sb.AppendLine("		/// A search object for the '" + _currentComponent.PascalName + "' collection.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentComponent.PascalName + "Search(SearchType searchType) ");
				sb.AppendLine("		{");
				sb.AppendLine("			_searchType = searchType;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		void IBusinessObjectSearch.SetValue(Enum field, object value)");
				sb.AppendLine("		{");
				sb.AppendLine("			this.SetValue((" + _currentComponent.PascalName + ".FieldNameConstants)field, value);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Set the specified value on this object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public void SetValue(" + _currentComponent.PascalName + ".FieldNameConstants field, object value)");
				sb.AppendLine("		{");
				foreach (Column dc in validColumns)
				{
					sb.AppendLine("			if (field == " + _currentComponent.PascalName + ".FieldNameConstants." + dc.PascalName + ")");
					sb.AppendLine("				this." + dc.PascalName + " = (" + dc.GetCodeType(false) + ")value;");
				}
				sb.AppendLine("		}");
				sb.AppendLine();
				foreach (Column dc in validColumns)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// This field determines the value of the '" + dc.PascalName + "' field on the '" + _currentComponent.PascalName + "' object when this search object is applied.");
					sb.AppendLine("		/// </summary>");
					sb.AppendFormat("		public " + dc.GetCodeType(true, true) + " {0}", dc.PascalName).AppendLine();
					sb.AppendLine("		{");
					sb.AppendFormat("			get {{ return _{0}; }}", dc.CamelName).AppendLine();
					sb.AppendFormat("			set {{ _{0} = value; }}", dc.CamelName).AppendLine();
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
			sb.AppendLine("	#region " + _currentComponent.PascalName + "Paging");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// A field sort object for the " + _currentComponent.PascalName + "Paging object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public class " + _currentComponent.PascalName + "PagingFieldItem : IPagingFieldItem");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the direction of the sort.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public bool Ascending { get; set; }");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the field on which to sort.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + DefaultNamespace + ".Business.Objects.Components." + _currentComponent.PascalName + ".FieldNameConstants Field { get; set; }");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create a sorting field object for the " + _currentComponent.PascalName + "Paging object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"field\">The field on which to sort.</param>");
			sb.AppendLine("		public " + _currentComponent.PascalName + "PagingFieldItem(" + DefaultNamespace + ".Business.Objects.Components." + _currentComponent.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.Field = field;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create a sorting field object for the " + _currentComponent.PascalName + "Paging object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"field\">The field on which to sort.</param>");
			sb.AppendLine("		/// <param name=\"ascending\">Determines the direction of the sort.</param>");
			sb.AppendLine("		public " + _currentComponent.PascalName + "PagingFieldItem(" + DefaultNamespace + ".Business.Objects.Components." + _currentComponent.PascalName + ".FieldNameConstants field, bool ascending)");
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
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The paging object for the " + _currentComponent.PascalName + " collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public class " + _currentComponent.PascalName + "Paging : IPagingObject");
			sb.AppendLine("	{");
			sb.AppendLine("		#region Class Members");
			sb.AppendLine();
			sb.AppendLine("		private int _pageIndex = 1;");
			sb.AppendLine("		private int _recordsperPage = 10;");
			sb.AppendLine("		private List<" + _currentComponent.PascalName + "PagingFieldItem> _orderByList = new List<" + _currentComponent.PascalName + "PagingFieldItem>();		");
			sb.AppendLine("		private int _recordCount = 0;");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region Constructors");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a paging object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentComponent.PascalName + "Paging()");
			sb.AppendLine("			: this(1, 10, null)");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a paging object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"pageIndex\">The page number to load</param>");
			sb.AppendLine("		/// <param name=\"recordsperPage\">The number of records per page.</param>");
			sb.AppendLine("		public " + _currentComponent.PascalName + "Paging(int pageIndex, int recordsperPage)");
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
			sb.AppendLine("		public " + _currentComponent.PascalName + "Paging(int pageIndex, int recordsperPage, " + DefaultNamespace + ".Business.Objects.Components." + _currentComponent.PascalName + ".FieldNameConstants field, bool ascending)");
			sb.AppendLine("			: this(pageIndex, recordsperPage, new " + _currentComponent.PascalName + "PagingFieldItem(field, ascending))");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a paging object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"pageIndex\">The page number to load</param>");
			sb.AppendLine("		/// <param name=\"recordsperPage\">The number of items per page.</param>");
			sb.AppendLine("		/// <param name=\"orderByField\">The field on which to sort.</param>");
			sb.AppendLine("		public " + _currentComponent.PascalName + "Paging(int pageIndex, int recordsperPage, " + _currentComponent.PascalName + "PagingFieldItem orderByField)");
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
			sb.AppendLine("		public List<" + _currentComponent.PascalName + "PagingFieldItem> OrderByList");
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

		private ArrayList GetValidSearchColumns()
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
			return validColumns;
		}

		private void AppendClassEnumerator()
		{
			sb.AppendLine("		#region IEnumerator");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// An strongly-typed enumerator for the '" + _currentComponent.PascalName + "' object collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public class " + _currentComponent.PascalName + "Enumerator : IEnumerator ");
			sb.AppendLine("		{");
			sb.AppendLine("			private IEnumerator internalEnumerator;");
			sb.AppendLine("			internal " + _currentComponent.PascalName + "Enumerator(IEnumerator icg)");
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
			sb.AppendLine("						return new " + _currentComponent.PascalName + "((Domain" + _currentComponent.PascalName + ")internalEnumerator.Current);");
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
			sb.AppendLine("						Domain" + _currentComponent.PascalName + " currentRow = (Domain" + _currentComponent.PascalName + ")internalEnumerator.Current;");
			sb.AppendLine("						while(currentRow.RowState == System.Data.DataRowState.Deleted || currentRow.RowState == System.Data.DataRowState.Detached)");
			sb.AppendLine("						{");
			sb.AppendLine("							movedNext = internalEnumerator.MoveNext();");
			sb.AppendLine("							if(!movedNext)");
			sb.AppendLine("								break;");
			sb.AppendLine("							currentRow = (Domain" + _currentComponent.PascalName + ")internalEnumerator.Current;");
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

			sb.AppendLine("		internal Domain" + _currentComponent.PascalName + "Collection wrappedClass;");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The parent subdomain object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected SubDomain _subDomain = null;");

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
			sb.AppendLine("		internal " + _currentComponent.PascalName + "Collection(Domain" + _currentComponent.PascalName + "Collection classToWrap)");
			sb.AppendLine("		{");
			sb.AppendLine("			_subDomain = classToWrap.SubDomain;");
			sb.AppendLine("			wrappedClass = classToWrap;");
			sb.AppendLine("		}");

			sb.AppendLine();
		}

		private void AppendConstructorModifier()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Constructor that enables you to specify a modifier");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"modifier\">Used in audit operations to track changes</param>");

			sb.AppendLine("		public " + _currentComponent.PascalName + "Collection(string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			_subDomain = new SubDomain(modifier);");
			sb.AppendLine("			wrappedClass = (Domain" + _currentComponent.PascalName + "Collection)_subDomain.GetDomainCollection(Collections." + _currentComponent.PascalName + "Collection);");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendConstructorDefault()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The default constructor");
			sb.AppendLine("		/// </summary>");

			sb.AppendLine("		public " + _currentComponent.PascalName + "Collection() ");
			sb.AppendLine("		{");
			//sb.AppendLine("			_subDomain = new SubDomain(\"Default Contructor Called\");");
			sb.AppendLine("			_subDomain = new SubDomain(\"\");");
			sb.AppendLine("			wrappedClass = (Domain" + _currentComponent.PascalName + "Collection)_subDomain.GetDomainCollection(Collections." + _currentComponent.PascalName + "Collection);");
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
			sb.AppendLine("		public virtual object WrappedClass");
			sb.AppendLine("		{");
			sb.AppendLine("			get{return wrappedClass;}");
			sb.AppendLine("			set{wrappedClass = (Domain" + _currentComponent.PascalName + "Collection)value;}");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendPropertySubDomain()
		{
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
			sb.AppendLine("					DataTable dt = wrappedClass.GetChanges(DataRowState.Deleted);");
			sb.AppendLine("					if (dt == null)");
			sb.AppendLine("						return wrappedClass.Count;");
			sb.AppendLine("					else");
			sb.AppendLine("						return wrappedClass.Count - dt.Rows.Count;");
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
			sb.AppendLine("		public virtual Type ContainedType");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return typeof(" + _currentComponent.PascalName + "); }");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendPropertyCollection()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the type of collection for this object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public virtual Collections Collection");
			sb.AppendLine("		{");
			sb.AppendFormat("      get {{ return Collections.{0}Collection; }}", _currentComponent.PascalName).AppendLine();
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
			this.AppendMethodPersist();
			this.AppendMethodSelectAll();
			this.AppendMethodSelectSearch();			
			this.AppendMethodSelectPaged();
			this.AppendAggregateMethods();
			this.AppendMethodRetieveByPrimaryKey();
			this.AppendMethodRetieveBySearchableFields();
			this.AppendMethodGet();
			this.AppendMethodGetEnumerator();
			this.AppendRejectChanges();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendAggregateMethods()
		{
			string scope = "";

			List<Table> tableList = _currentComponent.Parent.GetTableHierarchy();
			tableList.Remove(_currentComponent.Parent);
			Dictionary<string, Column> columnList = new Dictionary<string, Column>();

			//All types that are valid for these operations
			List<string> typeList = new List<string>();
			typeList.Add("int");
			typeList.Add("Single");
			typeList.Add("double");
			typeList.Add("string");
			typeList.Add("DateTime");

			//Count
			this.AppendAggregateCount(scope, tableList);

		}

		private void AppendAggregateCount(string scope, List<Table> tableList)
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get the count of objects that match the Where condition");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		internal static " + scope + "int GetCount(Expression<Func<" + _currentComponent.PascalName + "Query, bool>> where)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(\"\");");
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentComponent.PascalName + "Query> template = dc.GetTable<" + _currentComponent.PascalName + "Query>();");
			sb.AppendLine();
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
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

		public void AppendMethodNewItems()
		{
			Column column = (Column)_currentComponent.Parent.PrimaryKeyColumns[0];
			if (_currentComponent.Parent.PrimaryKeyColumns.Count == 1 && column.DataType == System.Data.SqlDbType.UniqueIdentifier)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Create a new object to later add to this collection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("			protected internal virtual " + _currentComponent.PascalName + " NewItem(Guid key) ");
				sb.AppendLine("			{");
				sb.AppendLine("				try");
				sb.AppendLine("				{");
				sb.AppendLine("					return new " + _currentComponent.PascalName + "(wrappedClass.NewItem(key));");
				sb.AppendLine("				}");
				Globals.AppendBusinessEntryCatch(sb);
				sb.AppendLine("			}");
			}
			else
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Create a new object to later add to this collection");
				sb.AppendLine("		/// </summary>");
				sb.Append("			protected internal virtual " + _currentComponent.PascalName + " NewItem(");

				int index = 0;
				foreach (Column dc in _currentComponent.Parent.PrimaryKeyColumns)
				{
					sb.Append(dc.GetCodeType() + " " + dc.PascalName);
					if (index < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
					index++;
				}
				sb.AppendLine(")");

				sb.AppendLine("			{");
				sb.AppendLine("				try");
				sb.AppendLine("				{");
				sb.Append("					return new " + _currentComponent.PascalName + "(wrappedClass.NewItem(");

				index = 0;
				foreach (Column dc in _currentComponent.Parent.PrimaryKeyColumns)
				{
					sb.Append(dc.PascalName);
					if (index < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
					index++;
				}
				sb.Append("));");
				sb.AppendLine();

				sb.AppendLine("				}");
				Globals.AppendBusinessEntryCatch(sb);
				sb.AppendLine("			}");
			}
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create a new object to later add to this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected internal virtual " + _currentComponent.PascalName + " NewItem() ");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				return new " + _currentComponent.PascalName + "(wrappedClass.NewItem());");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();

		}

		private void AppendMethodGet()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Get an object from this collection by its unique identifier.");
			sb.AppendLine("		/// </summary>");
			sb.Append("		public virtual " + _currentComponent.PascalName + " GetItemByPK(");
			for (int ii = 0; ii < _currentComponent.Parent.PrimaryKeyColumns.Count; ii++)
			{
				Column dc = (Column)_currentComponent.Parent.PrimaryKeyColumns[ii];
				sb.Append(dc.GetCodeType() + " " + dc.CamelName);
				if (ii < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
				{
					sb.Append(", ");
				}
			}
			sb.AppendLine(") ");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.Append("				if (wrappedClass.Get" + _currentComponent.PascalName + "(");
			for (int ii = 0; ii < _currentComponent.Parent.PrimaryKeyColumns.Count; ii++)
			{
				Column dc = (Column)_currentComponent.Parent.PrimaryKeyColumns[ii];
				sb.Append(dc.CamelName);
				if (ii < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
				{
					sb.Append(", ");
				}
			}
			sb.AppendLine(") != null)");
			sb.AppendLine("				{");
			sb.Append("					return new " + _currentComponent.PascalName + "(wrappedClass.Get" + _currentComponent.PascalName + "(");
			for (int ii = 0; ii < _currentComponent.Parent.PrimaryKeyColumns.Count; ii++)
			{
				Column dc = (Column)_currentComponent.Parent.PrimaryKeyColumns[ii];
				sb.Append(dc.CamelName);
				if (ii < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
				{
					sb.Append(", ");
				}
			}
			sb.AppendLine("));");
			sb.AppendLine("				}");
			sb.AppendLine("				else");
			sb.AppendLine("				{");
			sb.AppendLine("					return null;");
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
			sb.AppendLine("		public virtual System.Collections.IEnumerator GetEnumerator()");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				return new " + _currentComponent.PascalName + "Enumerator(wrappedClass.GetEnumerator());");
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
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection SelectUsingPK(" + PrimaryKeyParameterList(true) + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			return SelectUsingPK(" + PrimaryKeyParameterList(false) + ", \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a single object from this collection by its primary key.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection SelectUsingPK(" + PrimaryKeyParameterList(true) + ", string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				ArrayList primaryKeys = new ArrayList();");
			sb.AppendLine("				primaryKeys.Add(new " + _currentComponent.Parent.PascalName + "PrimaryKey(" + PrimaryKeyInputParameterList() + "));");
			sb.AppendLine("				" + _currentComponent.PascalName + "Collection returnVal = new " + _currentComponent.PascalName + "Collection(Domain" + _currentComponent.PascalName + "Collection.SelectBy" + _currentComponent.PascalName + "Pks(primaryKeys, modifier));");
			sb.AppendLine("				return returnVal;");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodRetieveBySearchableFields()
		{
			//Get a list of all base column selects
			List<Table> tList = _currentComponent.Parent.GetTableHierarchy();
			tList.Remove(_currentComponent.Parent);
			List<Column> baseColumns = new List<Column>();
			foreach (Table t in tList)
			{
				baseColumns.AddRange(t.GetColumnsSearchable());
			}

			//Create selects for the fields
			foreach (Reference reference in _currentComponent.Columns)
			{
				AppendMethodRetiveBySearchableFieldsSingleField((Column)reference.Object, false);
			}

		}

		private void AppendMethodRetiveBySearchableFieldsSingleField(Column column, bool isNew)
		{
			if (column.IsSearchable)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Select an object list by '" + column.PascalName + "' field.");
				sb.AppendLine("		/// <param name=\"" + column.CamelName + "\">The " + column.CamelName + " field</param>");
				sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public static " + (isNew ? "new " : "") + _currentComponent.PascalName + "Collection SelectBy" + column.PascalName + "(" + column.GetCodeType() + " " + column.CamelName + ", string modifier)");
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
				sb.AppendLine("		public static " + (isNew ? "new " : "") + _currentComponent.PascalName + "Collection SelectBy" + column.PascalName + "(" + column.GetCodeType() + " " + column.CamelName + ", " + _currentComponent.PascalName + "Paging paging, string modifier)");
				sb.AppendLine("		{");
				sb.AppendLine("			try");
				sb.AppendLine("			{");
				sb.AppendLine("			  SubDomain sd = new SubDomain(modifier);");
				sb.AppendLine("			  " + _currentComponent.PascalName + "SelectBy" + column.PascalName + " command = new " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "(" + column.CamelName + ", paging);");
				sb.AppendLine("			  sd.AddSelectCommand(command);");
				sb.AppendLine("			  sd.RunSelectCommands();");
				sb.AppendLine("			  if (paging != null) paging.RecordCount = command.Count;");
				sb.AppendLine("			  return (" + _currentComponent.PascalName + "Collection)sd[Collections." + _currentComponent.PascalName + "Collection];");
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
				sb.AppendLine("		public static " + (isNew ? "new " : "") + _currentComponent.PascalName + "Collection SelectBy" + column.PascalName + "Range(" + column.GetCodeType() + " " + var1 + ", " + column.GetCodeType() + " " + var2 + ", string modifier)");
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
				sb.AppendLine("		public static " + (isNew ? "new " : "") + _currentComponent.PascalName + "Collection SelectBy" + column.PascalName + "Range(" + column.GetCodeType() + " " + var1 + ", " + column.GetCodeType() + " " + var2 + ", " + _currentComponent.PascalName + "Paging paging, string modifier)");
				sb.AppendLine("		{");
				sb.AppendLine("			try");
				sb.AppendLine("			{");
				sb.AppendLine("			  SubDomain sd = new SubDomain(modifier);");
				sb.AppendLine("			  " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "Range command = new " + _currentComponent.PascalName + "SelectBy" + column.PascalName + "Range(" + column.CamelName + "Start, " + column.CamelName + "End, paging);");
				sb.AppendLine("			  sd.AddSelectCommand(command);");
				sb.AppendLine("			  sd.RunSelectCommands();");
				sb.AppendLine("			  if (paging != null) paging.RecordCount = command.Count;");
				sb.AppendLine("			  return (" + _currentComponent.PascalName + "Collection)sd[Collections." + _currentComponent.PascalName + "Collection];");
				sb.AppendLine("			}");
				Globals.AppendBusinessEntryCatch(sb);
				sb.AppendLine("		}");
				sb.AppendLine();
			}
		}

		private void AppendMethodSelectSearch()
		{
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"search\">The search object to use for searching.</param>");
			sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(" + _currentComponent.PascalName + "Search search, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("		 if (search == null) throw new Exception(\"The 'Search' object cannot be null!\");");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				" + _currentComponent.PascalName + "Collection returnVal = new " + _currentComponent.PascalName + "Collection(Domain" + _currentComponent.PascalName + "Collection.RunSelect(search, modifier));");
			sb.AppendLine("				return returnVal;");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");

			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database based on the specified search object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"search\">The search object to use for searching.</param>");
			sb.AppendLine("		/// <param name=\"paging\">The paging object to determine how the results are paged.</param>");
			sb.AppendLine("		/// <param name=\"modifier\">The identifier used for the create and modify audits.</param>");
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(" + _currentComponent.PascalName + "Search search, " + _currentComponent.PascalName + "Paging paging, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("		 if (search == null) throw new Exception(\"The 'Search' object cannot be null!\");");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				" + _currentComponent.PascalName + "Collection returnVal = new " + _currentComponent.PascalName + "Collection(Domain" + _currentComponent.PascalName + "Collection.RunSelect(search, paging, modifier));");
			sb.AppendLine("				return returnVal;");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();

			#region LINQ
			//if (_currentTable.ParentTable == null)
			//  this.AppendMethodSelectSearchLINQNoInherit();
			//else
			this.AppendMethodSelectSearchLINQInherit();

			this.AppendMethodSelectSearchLINQPaging();

			#endregion

		}

		private void AppendMethodSelectSearchLINQNoInherit()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Using the specified Where expression, execute a query against the database to return a result set");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(Expression<Func<" + _currentComponent.PascalName + "Query, bool>> where)");
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
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(Expression<Func<" + _currentComponent.PascalName + "Query, bool>> where, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
			sb.AppendLine();
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentComponent.PascalName + "Query> template = dc.GetTable<" + _currentComponent.PascalName + "Query>();");
			sb.AppendLine();
			sb.AppendLine("			var result = template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x);");
			sb.AppendLine();
			sb.AppendLine("			" + _currentComponent.PascalName + "Collection retval = (" + _currentComponent.PascalName + "Collection)subDomain[Collections." + _currentComponent.PascalName + "Collection];");

			#region Set Properties 1-1
			sb.AppendLine("			foreach (" + _currentComponent.PascalName + "Query item in result)");
			sb.AppendLine("			{");

			sb.Append("			if (retval.GetItemByPK(");
			foreach (Column pk in _currentComponent.Parent.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);
				if (_currentComponent.Parent.PrimaryKeyColumns.IndexOf(pk) < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(") == null)");
			sb.AppendLine("			{");

			sb.Append("				" + _currentComponent.PascalName + " newItem = retval.NewItem(");
			foreach (Column pk in _currentComponent.Parent.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);

				if (_currentComponent.Parent.PrimaryKeyColumns.IndexOf(pk) < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(");");

			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;
				if (column.AllowNull)
					sb.AppendLine("				newItem.wrappedClass[\"" + column.DatabaseName + "\"] = StringHelper.ConvertToDatabase(item." + column.PascalName + ");");
				else
					sb.AppendLine("				newItem.wrappedClass[\"" + column.DatabaseName + "\"] = item." + column.PascalName + ";");
			}

			if (_currentComponent.Parent.AllowCreateAudit)
			{
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ");");
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ");");
			}
			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ");");
				sb.AppendLine("				newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ");");
			}
			if (_currentComponent.Parent.AllowTimestamp)
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
			List<Table> tableList = _currentComponent.Parent.GetTableHierarchy();
			tableList.Remove(_currentComponent.Parent);
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
			foreach (Column c in _currentComponent.Parent.PrimaryKeyColumns)
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
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(Expression<Func<" + _currentComponent.PascalName + "Query, bool>> where, " + _currentComponent.PascalName + "Paging paging)");
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
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(Expression<Func<" + _currentComponent.PascalName + "Query, bool>> where, " + _currentComponent.PascalName + "Paging paging, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (paging == null) throw new Exception(\"The paging object cannot be null.\");");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
			sb.AppendLine();
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentComponent.PascalName + "Query> template = dc.GetTable<" + _currentComponent.PascalName + "Query>();");
			sb.AppendLine();
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			sb.AppendLine();
			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText, paging);");
			sb.AppendLine("			cmd.CommandText = parser.GetSQL();");
			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			var result = dc.Translate<" + _currentComponent.PascalName + "Query>(cmd.ExecuteReader());");
			sb.AppendLine();
			sb.AppendLine("			" + _currentComponent.PascalName + "Collection retval = (" + _currentComponent.PascalName + "Collection)subDomain[Collections." + _currentComponent.PascalName + "Collection];");
			sb.AppendLine("			foreach (" + _currentComponent.PascalName + "Query item in result)");
			sb.AppendLine("			{");

			sb.Append("				if (retval.GetItemByPK(");
			foreach (Column pk in _currentComponent.Parent.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);
				if (_currentComponent.Parent.PrimaryKeyColumns.IndexOf(pk) < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(") == null)");
			sb.AppendLine("				{");

			sb.Append("					" + _currentComponent.PascalName + " newItem = retval.NewItem(");
			foreach (Column pk in _currentComponent.Parent.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);

				if (_currentComponent.Parent.PrimaryKeyColumns.IndexOf(pk) < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(");");

			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;
				if (column.AllowNull)
					sb.AppendLine("					newItem.wrappedClass[\"" + column.DatabaseName + "\"] = StringHelper.ConvertToDatabase(item." + column.PascalName + ");");
				else
					sb.AppendLine("					newItem.wrappedClass[\"" + column.DatabaseName + "\"] = item." + column.PascalName + ";");
			}

			if (_currentComponent.Parent.AllowCreateAudit)
			{
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ");");
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ");");
			}
			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ");");
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ");");
			}
			if (_currentComponent.Parent.AllowTimestamp)
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
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(Expression<Func<" + _currentComponent.PascalName + "Query, bool>> where)");
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
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(Expression<Func<" + _currentComponent.PascalName + "Query, bool>> where, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain subDomain = new SubDomain(modifier);");
			sb.AppendLine();
			sb.AppendLine("			DataContext dc = new DataContext(ConfigurationValues.GetInstance().ConnectionString);");
			sb.AppendLine("			Table<" + _currentComponent.PascalName + "Query> template = dc.GetTable<" + _currentComponent.PascalName + "Query>();");
			sb.AppendLine();
			sb.AppendLine("			var cmd = dc.GetCommand(template");
			sb.AppendLine("				.Where(where)");
			sb.AppendLine("				.Select(x => x));");
			sb.AppendLine();
			sb.AppendLine("			LinqSQLParser parser = LinqSQLParser.Create(cmd.CommandText);");
			sb.AppendLine("			cmd.CommandText = parser.GetSQL();");
			sb.AppendLine();
			sb.AppendLine("			dc.Connection.Open();");
			sb.AppendLine("			var result = dc.Translate<" + _currentComponent.PascalName + "Query>(cmd.ExecuteReader());");
			sb.AppendLine();
			sb.AppendLine("			" + _currentComponent.PascalName + "Collection retval = (" + _currentComponent.PascalName + "Collection)subDomain[Collections." + _currentComponent.PascalName + "Collection];");
			sb.AppendLine("			foreach (" + _currentComponent.PascalName + "Query item in result)");
			sb.AppendLine("			{");

			sb.Append("				if (retval.GetItemByPK(");
			foreach (Column pk in _currentComponent.Parent.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);
				if (_currentComponent.Parent.PrimaryKeyColumns.IndexOf(pk) < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(") == null)");
			sb.AppendLine("				{");

			sb.Append("					" + _currentComponent.PascalName + " newItem = retval.NewItem(");
			foreach (Column pk in _currentComponent.Parent.PrimaryKeyColumns)
			{
				sb.Append("item." + pk.PascalName);

				if (_currentComponent.Parent.PrimaryKeyColumns.IndexOf(pk) < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(");");

			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;
				if (column.AllowNull)
					sb.AppendLine("					newItem.wrappedClass[\"" + column.DatabaseName + "\"] = StringHelper.ConvertToDatabase(item." + column.PascalName + ");");
				else
					sb.AppendLine("					newItem.wrappedClass[\"" + column.DatabaseName + "\"] = item." + column.PascalName + ";");
			}

			if (_currentComponent.Parent.AllowCreateAudit)
			{
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ");");
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ");");
			}
			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ");");
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "\"] = StringHelper.ConvertToDatabase(item." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ");");
			}
			if (_currentComponent.Parent.AllowTimestamp)
			{
				sb.AppendLine("					newItem.wrappedClass[\"" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "\"] = item." + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + ";");
			}

			sb.AppendLine("					retval.AddItem(newItem);");
			sb.AppendLine("				}");
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
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(int page, int pageSize, " + _currentComponent.PascalName + ".FieldNameConstants orderByColumn, bool ascending, string filter, out int count)");
			sb.AppendLine("		{");
			sb.AppendLine("		  return RunSelect(page, pageSize, orderByColumn.ToString(), ascending, filter, out count, \"\");");
			sb.AppendLine("		}");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(int page, int pageSize, " + _currentComponent.PascalName + ".FieldNameConstants orderByColumn, bool ascending, string filter, out int count, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("		  return RunSelect(page, pageSize, orderByColumn.ToString(), ascending, filter, out count, modifier);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter, out int count)");
			sb.AppendLine("		{");
			sb.AppendLine("		  return RunSelect(page, pageSize, orderByColumn, ascending, filter, out count, \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a paged set of objects from the database");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter, out int count, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				int returnCount;");
			sb.AppendLine("				" + _currentComponent.PascalName + "Collection returnVal = new " + _currentComponent.PascalName + "Collection(Domain" + _currentComponent.PascalName + "Collection.RunSelect(page, pageSize, orderByColumn, ascending, filter, out returnCount, modifier));");
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
			sb.AppendLine("		/// Select all objects from the database table.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect()");
			sb.AppendLine("		{");
			sb.AppendLine("			return RunSelect(\"\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select all objects from the database table.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _currentComponent.PascalName + "Collection RunSelect(string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				" + _currentComponent.PascalName + "Collection returnVal = new " + _currentComponent.PascalName + "Collection(Domain" + _currentComponent.PascalName + "Collection.RunSelect(modifier));");
			sb.AppendLine("				return returnVal;");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodPersist()
		{
			if (_currentComponent.Parent.Immutable)
				return;

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Persists this collection to the database.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override void Persist()");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");

			sb.AppendLine("				foreach (" + _currentComponent.PascalName + " item in this)");
			sb.AppendLine("				{");
			sb.AppendLine("					if (item.wrappedClass.RowState != DataRowState.Unchanged)");
			sb.AppendLine("						item.OnValidate(new BusinessObjectEventArgs(item));");
			sb.AppendLine("				}"); sb.AppendLine();

			sb.AppendLine("				wrappedClass.Persist();");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodAddNewItem()
		{
			//The add method is hidden so we cannot add to this collection
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Add a newly created item to this collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected internal void AddItem(" + _currentComponent.PascalName + " " + _currentComponent.CamelName + ") ");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				wrappedClass.Add" + _currentComponent.PascalName + "((Domain" + _currentComponent.PascalName + ")" + _currentComponent.CamelName + ".WrappedClass);");
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
			sb.AppendLine("		public virtual void ProcessVisitor(IVisitor visitor)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (visitor == null) throw new Exception(\"This object cannot be null.\");");
			sb.AppendLine("			foreach (IBusinessObject item in this)");
			sb.AppendLine("			{");
			sb.AppendLine("				visitor.Visit(item);");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
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
			sb.AppendLine("		public virtual " + _currentComponent.PascalName + " this[int index] ");
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
			//sb.AppendLine("					return (" + _currentComponent.PascalName + ")internalEnumerator.Current;");
			sb.AppendLine("					" + _currentComponent.PascalName + " retval = (" + _currentComponent.PascalName + ")internalEnumerator.Current;");
			sb.AppendLine("					if (retval.wrappedClass == null)");
			sb.AppendLine("					{");
			sb.AppendLine("						if (!((0 <= index) && (index < this.Count)))");
			sb.AppendLine("							throw new IndexOutOfRangeException();");
			sb.AppendLine("						else");
			sb.AppendLine("							throw new Exception(\"The item is null. This is not a valid state.\");");
			sb.AppendLine("					}");
			sb.AppendLine("					else return (" + _currentComponent.PascalName + ")internalEnumerator.Current;");
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
			sb.AppendLine("		/// Rejects all the changes associate with business object in this collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns>void</returns>");
			sb.AppendLine("		public virtual void RejectChanges()");
			sb.AppendLine("		{");
			sb.Append("		  Domain" + _currentComponent.PascalName + "Collection coll = (Domain" + _currentComponent.PascalName + "Collection)this.wrappedClass;");
			sb.Append("		  coll.RejectChanges();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
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
				foreach (Column dc in _currentComponent.Parent.PrimaryKeyColumns)
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
				throw new Exception(_currentComponent.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}

		protected string PrimaryKeyInputParameterList()
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Column dc in _currentComponent.Parent.PrimaryKeyColumns)
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
				throw new Exception(_currentComponent.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}

		protected string PrimaryKeyColumnList()
		{
			StringBuilder output = new StringBuilder();
			foreach (Column dc in _currentComponent.Parent.PrimaryKeyColumns)
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
			string setModifiedBy = String.Format("//" + "\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "Column] = Modifier;", _currentComponent.PascalName);
			string setModifiedDate = String.Format("//" + "\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "Column] = " + Globals.GetDateTimeNowCode(_model) + ";", _currentComponent.PascalName);
			string setCreatedBy = String.Format("//" + "\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "Column] = Modifier;", _currentComponent.PascalName);
			string setCreatedDate = String.Format("//" + "\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "Column] = " + Globals.GetDateTimeNowCode(_model) + ";", _currentComponent.PascalName);
			string setOriginalGuid = String.Format("\t\t\t((DataRow)return{0})[{0}GuidColumn] = Guid.NewGuid().ToString().ToUpper();", _currentComponent.PascalName);
			string setOriginalGuidWhenUniqueIdentifier = String.Format("\t\t\t((DataRow)return{0})[{0}GuidColumn] = Guid.NewGuid();", _currentComponent.PascalName);

			StringBuilder returnVal = new StringBuilder();

			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				returnVal.AppendLine(setModifiedBy);
				returnVal.AppendLine(setModifiedDate);
			}

			if (_currentComponent.Parent.AllowCreateAudit)
			{
				returnVal.AppendLine(setCreatedBy);
				returnVal.AppendLine(setCreatedDate);
			}

			if (_currentComponent.Parent.PrimaryKeyColumns.Count == 1)
			{
				string actualKeyName = ((Column)_currentComponent.Parent.PrimaryKeyColumns[0]).DatabaseName;
				string guidKeyName = _currentComponent.DatabaseName.ToLower() + "_guid";
				if (StringHelper.Match(actualKeyName, guidKeyName, true))
				{
					if (StringHelper.Match(((Column)_currentComponent.Parent.PrimaryKeyColumns[0]).GetCodeType(), "System.Guid", false))
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