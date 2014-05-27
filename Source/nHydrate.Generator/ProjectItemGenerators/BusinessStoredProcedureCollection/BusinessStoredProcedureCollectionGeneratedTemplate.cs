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

namespace Widgetsphere.Generator.ProjectItemGenerators.BusinessStoredProcedureCollection
{
  class BusinessStoredProcedureCollectionGeneratedTemplate : BaseClassTemplate
  {
    private StringBuilder sb = new StringBuilder();
    private CustomStoredProcedure _currentStoredProcedure;

    public BusinessStoredProcedureCollectionGeneratedTemplate(ModelRoot model, CustomStoredProcedure currentStoredProcedure)
    {
      _model = model;
      _currentStoredProcedure = currentStoredProcedure;
    }

    #region BaseClassTemplate overrides
    public override string FileName
    {
      get { return string.Format("{0}Collection.Generated.cs", _currentStoredProcedure.PascalName); }
    }

    public string ParentItemName
    {
      get { return string.Format("{0}Collection.cs", _currentStoredProcedure.PascalName); }
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
        sb.AppendLine("namespace " + DefaultNamespace + ".Business.StoredProcedures" );
        sb.AppendLine("{" );
        this.AppendClass();
        sb.AppendLine("}" );
      }
      catch(Exception ex)
      {
        throw;
      }
    }

    #endregion

    #region namespace / objects

    public void AppendUsingStatements()
    {
      sb.AppendLine("using System;" );
      sb.AppendLine("using System.Data;" );
      sb.AppendLine("using System.Xml;" );
      sb.AppendLine("using System.Collections;" );
      sb.AppendLine("using System.Runtime.Serialization;" );
      sb.Append("using System.ComponentModel;").AppendLine();
      sb.AppendLine("using " + DefaultNamespace + ".Business.Rules;" );
      sb.AppendLine("using " + DefaultNamespace + ".Business.SelectCommands;" );
      sb.AppendLine("using " + DefaultNamespace + ".Domain.Objects;" );
      sb.AppendLine("using " + DefaultNamespace + ".Domain.StoredProcedures;" );
      sb.AppendLine("using Widgetsphere.Core.DataAccess;" );
      sb.AppendLine("using Widgetsphere.Core.Util;" );
      sb.AppendLine("using Widgetsphere.Core.Logging;" );
      sb.AppendLine("using Widgetsphere.Core.Exceptions;" );
      sb.AppendLine("using System.IO;");
			sb.AppendLine();
    }

    private void AppendClass()
    {
      sb.AppendLine();
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// The class representing the '" + _currentStoredProcedure.PascalName + "' virtual database table" );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("	 [Serializable()]" );
			sb.AppendLine("	 public partial class " + _currentStoredProcedure.PascalName + "Collection : BusinessCollectionBase, IReadOnlyBusinessCollection" );
      sb.AppendLine("	 {");
      sb.AppendLine();
      this.AppendMemberVariables();
      this.AppendConstructors();
      this.AppendProperties();
      this.AppendOperatorIndexer();
      this.AppendMethods();			
      //this.AppendIListImplementation();
      this.AppendClassEnumerator();
      this.AppendIBusinessCollectionRegion();      
      sb.AppendLine("	}");
      sb.AppendLine();

    }

		private void AppendIBusinessCollectionRegion()
		{
      sb.AppendLine("		#region IBusinessCollection Explicit Members" );
      sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns an item in this collection by index.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"index\">The zero-based index of the element to get or set. </param>");
			sb.AppendLine("		/// <returns>The element at the specified index.</returns>");
			sb.AppendLine("		IBusinessObject IBusinessCollection.this[int index]");
      sb.AppendLine("		{" );
      sb.AppendLine("			get { return this[index]; }" );
      sb.AppendLine("		}" );
      sb.AppendLine();
      sb.AppendLine("		Enum IBusinessCollection.Collection" );
      sb.AppendLine("		{" );
      sb.AppendLine("			get { return this.Collection; }" );
      sb.AppendLine("		}" );
      sb.AppendLine();
      sb.AppendLine("		Type IBusinessCollection.ContainedType" );
      sb.AppendLine("		{" );
      sb.AppendLine("			get { return this.ContainedType; }" );
      sb.AppendLine("		}" );
      sb.AppendLine();
      sb.AppendLine("		SubDomainBase IBusinessCollection.SubDomain" );
      sb.AppendLine("			{" );
      sb.AppendLine("				get { return this.SubDomain; }" );
      sb.AppendLine("			}" );
      sb.AppendLine();
      sb.AppendLine("		 Type ITyped.GetContainedType()" );
      sb.AppendLine("		 {" );
      sb.AppendLine("			 return typeof(" + _currentStoredProcedure.PascalName + ");" );
      sb.AppendLine("		 }" );
      sb.AppendLine();
      sb.AppendLine("		#endregion" );
      sb.AppendLine();
    }

    private void AppendClassEnumerator()
    {
      sb.AppendLine();
      sb.AppendLine("		#region IEnumerator" );
      sb.AppendLine();
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// An strongly-typed enumerator for the '" + _currentStoredProcedure.PascalName + "' object collection" );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("		public class " + _currentStoredProcedure.PascalName + "Enumerator: IEnumerator " );
      sb.AppendLine("			{" );
      sb.AppendLine("			private IEnumerator internalEnumerator;" );
      sb.AppendLine("			internal " + _currentStoredProcedure.PascalName + "Enumerator(IEnumerator icg)" );
      sb.AppendLine("			{" );
      sb.AppendLine("				internalEnumerator = icg;" );
      sb.AppendLine("			}" );
      sb.AppendLine("			" );
      sb.AppendLine("			#region IEnumerator Members" );
      sb.AppendLine("			" );
      sb.AppendLine("			/// <summary>" );
      sb.AppendLine("			/// Reset the enumerator to the first object in this collection" );
      sb.AppendLine("			/// </summary>" );
      sb.AppendLine("			public void Reset()" );
      sb.AppendLine("			{" );
      sb.AppendLine("			try" );
      sb.AppendLine("			{" );
      sb.AppendLine("				internalEnumerator.Reset();" );
      sb.AppendLine("			}" );
      Globals.AppendBusinessEntryCatch(sb);
      sb.AppendLine("			}" );
      sb.AppendLine();
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// Gets the current element in the collection." );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("		/// <returns>The current element in the collection.</returns>" );
      sb.AppendLine("			public object Current" );
      sb.AppendLine("			{" );
      sb.AppendLine("				get" );
      sb.AppendLine("				{" );
      sb.AppendLine("					try" );
      sb.AppendLine("					{" );
      sb.AppendLine("						return new " + _currentStoredProcedure.PascalName + "((Domain" + _currentStoredProcedure.PascalName + ")internalEnumerator.Current);" );
      sb.AppendLine("					}" );
      Globals.AppendBusinessEntryCatch(sb);
      sb.AppendLine("				}" );
      sb.AppendLine("			}" );
      sb.AppendLine();
      sb.AppendLine("			/// <summary>" );
      sb.AppendLine("			/// Advances the enumerator to the next element of the collection." );
      sb.AppendLine("			/// </summary>" );
      sb.AppendLine("			public bool MoveNext()" );
      sb.AppendLine("			{" );
      sb.AppendLine("				try" );
      sb.AppendLine("				{" );
      sb.AppendLine("					bool movedNext = internalEnumerator.MoveNext();" );
      sb.AppendLine("					if(movedNext)" );
      sb.AppendLine("					{" );
      sb.AppendLine("						Domain" + _currentStoredProcedure.PascalName + " currentRow = (Domain" + _currentStoredProcedure.PascalName + ")internalEnumerator.Current;" );
      sb.AppendLine("						while(currentRow.RowState == System.Data.DataRowState.Deleted || currentRow.RowState == System.Data.DataRowState.Detached)" );
      sb.AppendLine("						{" );
      sb.AppendLine("							movedNext = internalEnumerator.MoveNext();" );
      sb.AppendLine("							if(!movedNext)" );
      sb.AppendLine("								break;" );
      sb.AppendLine("							currentRow = (Domain" + _currentStoredProcedure.PascalName + ")internalEnumerator.Current;" );
      sb.AppendLine("						}" );
      sb.AppendLine("					}" );
      sb.AppendLine("					return movedNext;" );
      sb.AppendLine("				}" );
      Globals.AppendBusinessEntryCatch(sb);
      sb.AppendLine("			}" );
      sb.AppendLine("			" );
      sb.AppendLine("			#endregion" );
      sb.AppendLine("			" );
      sb.AppendLine("		}" );
      sb.AppendLine("		#endregion" );
      sb.AppendLine();
    }

    #endregion

    #region append regions
    #endregion

    #region append member variables
    public void AppendMemberVariables()
    {
      sb.AppendLine("		#region Member Variables" );
      sb.AppendLine();
      sb.AppendLine("		internal Domain" + _currentStoredProcedure.PascalName + "Collection wrappedClass;" );
      sb.AppendLine();
      sb.AppendLine("		#endregion" );
      sb.AppendLine();
    }
    #endregion

    #region append constructors
    public void AppendConstructors()
    {
      sb.AppendLine("		#region Constructor / Initialize" );
      sb.AppendLine();
      AppendConstructorDomainClass();      
      AppendConstructorModifier();      
      AppendConstructorDefault();      
      sb.AppendLine("		#endregion" );
      sb.AppendLine();
    }

    private void AppendConstructorDomainClass()
    {
      sb.AppendLine("		internal " + _currentStoredProcedure.PascalName + "Collection(Domain" + _currentStoredProcedure.PascalName + "Collection classToWrap)" );
      sb.AppendLine("		{" );
      sb.AppendLine("			wrappedClass = classToWrap;" );
      sb.AppendLine("		}" );
      sb.AppendLine();
    }

    private void AppendConstructorModifier()
    {
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// Constructor that enables you to specify a modifier" );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("		/// <param name=\"modifier\">Used in audit operations to track changes</param>" );
      sb.AppendLine("		public " + _currentStoredProcedure.PascalName + "Collection(string modifier) " );
      sb.AppendLine("		{" );
      sb.AppendLine("			SubDomain sd = new SubDomain(modifier);" );
      sb.AppendLine("			wrappedClass = (Domain" + _currentStoredProcedure.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentStoredProcedure.PascalName + "Collection);" );
      sb.AppendLine("		}" );
      sb.AppendLine();
    }


    private void AppendConstructorDefault()
    {
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// The default constructor" );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("		public " + _currentStoredProcedure.PascalName + "Collection() " );
      sb.AppendLine("		{" );
			//sb.AppendLine("			SubDomain sd = new SubDomain(\"Default Contructor Called\");");
			sb.AppendLine("			SubDomain sd = new SubDomain(\"\");");
      sb.AppendLine("			wrappedClass = (Domain" + _currentStoredProcedure.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentStoredProcedure.PascalName + "Collection);" );
      sb.AppendLine("		}" );
      sb.AppendLine();
    }
    #endregion

    #region append properties
    private void AppendProperties()
    {
      sb.AppendLine("		#region Property Implementations" );
      sb.AppendLine();
      AppendPropertyWrappedClass();
      AppendPropertySubDomain();
      AppendPropertyCount();
      AppendPropertyContainedType();
      AppendPropertyCollection();
      sb.AppendLine();
      sb.AppendLine("		#endregion" );
      sb.AppendLine();
    }

    private void AppendPropertyWrappedClass()
    {
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// Returns the internal object that this object wraps" );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("		[System.ComponentModel.Browsable(false)]" );
      sb.AppendLine("		public object WrappedClass" );
      sb.AppendLine("		{" );
      sb.AppendLine("			get{return wrappedClass;}" );
      sb.AppendLine("			set{wrappedClass = (Domain" + _currentStoredProcedure.PascalName + "Collection)value;}" );
      sb.AppendLine("		}" );
      sb.AppendLine();
    }

    private void AppendPropertySubDomain()
    {
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// A reference to the SubDomain that holds this collection" );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("		public SubDomain SubDomain" );
      sb.AppendLine("		{" );
      sb.AppendLine("			get" );
      sb.AppendLine("			{" );
      sb.AppendLine("				try" );
      sb.AppendLine("				{" );
      sb.AppendLine("					return wrappedClass.SubDomain;" );
      sb.AppendLine("				}" );
      Globals.AppendBusinessEntryCatch(sb);
      sb.AppendLine("			}" );
      sb.AppendLine("		}" );
      sb.AppendLine();
    }

    private void AppendPropertyCount()
    {
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// Returns the number of items in this collection" );
      sb.AppendLine("		/// </summary>" );
			sb.AppendLine("		public override int Count");
      sb.AppendLine("		{" );
      sb.AppendLine("			get" );
      sb.AppendLine("			{" );
      sb.AppendLine("				try" );
      sb.AppendLine("				{" );
      sb.AppendLine("					DataTable dt = wrappedClass.GetChanges(DataRowState.Deleted);" );
      sb.AppendLine("					if (dt == null)" );
      sb.AppendLine("						return wrappedClass.Count;" );
      sb.AppendLine("					else" );
      sb.AppendLine("						return wrappedClass.Count - dt.Rows.Count;" );
      sb.AppendLine("				}" );
      Globals.AppendBusinessEntryCatch(sb);
      sb.AppendLine("			}" );
      sb.AppendLine("		}" );
      sb.AppendLine();
    }

    private void AppendPropertyContainedType()
    {
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the type of object contained by this collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public Type ContainedType");
      sb.AppendLine("		{" );
      sb.AppendLine("			get { return typeof(" + _currentStoredProcedure.PascalName + "); }" );
      sb.AppendLine("		}" );
      sb.AppendLine();
    }

    private void AppendPropertyCollection()
    {
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the type of collection for this object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public Collections Collection");
      sb.AppendLine("		{" );
      sb.AppendFormat("      get {{ return Collections.{0}Collection; }}", _currentStoredProcedure.PascalName).AppendLine();
      sb.AppendLine("		}" );
      sb.AppendLine();
    }

    #endregion

    #region append methods

    public void AppendMethods()
    {
      sb.AppendLine("		#region Methods" );
      sb.AppendLine();
      this.AppendMethodSelectParameter();
      this.AppendRegionSearch();
      this.AppendMethodGetSorted();
      this.AppendMethodGetEnumerator();
      this.AppendRejectChanges();
			this.AppendMethodVisitor();
      sb.AppendLine("		#endregion" );
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

    private void AppendMethodGetSorted()
    {
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// Creates a sorted dataview of this collection ordered by the specified parameter" );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("		public DataView GetSorted" + _currentStoredProcedure.PascalName + "List(string sortString)" );
      sb.AppendLine("		{" );
      sb.AppendLine("			try" );
      sb.AppendLine("			{" );
      sb.AppendLine("				return wrappedClass.GetSorted" + _currentStoredProcedure.PascalName + "List(sortString);" );
      sb.AppendLine("			}" );
      Globals.AppendBusinessEntryCatch(sb);
      sb.AppendLine("		}" );
      sb.AppendLine();
    }

    private void AppendMethodGetEnumerator()
    {
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// Returns an enumerator that can be used to iterate through the collection." );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("		/// <returns>An Enumerator that can iterate through the objects in this collection.</returns>" );
      sb.AppendLine("		public System.Collections.IEnumerator GetEnumerator() " );
      sb.AppendLine("		{" );
      sb.AppendLine("			try" );
      sb.AppendLine("			{" );
      sb.AppendLine("				return new " + _currentStoredProcedure.PascalName + "Enumerator(wrappedClass.GetEnumerator());" );
      sb.AppendLine("			}" );
      Globals.AppendBusinessEntryCatch(sb);
      sb.AppendLine("		}" );
      sb.AppendLine();
    }

    private void AppendRegionSearch()
    {
      sb.AppendLine("		#region Search Members" );
      sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a search object to query this collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		IBusinessObjectSearch IBusinessCollection.CreateSearchObject(SearchType searchType)");
      sb.AppendLine("		{" );
      sb.AppendLine("			return null;" );
      sb.AppendLine("		}" );
      sb.AppendLine();
			//sb.AppendLine("		void IBusinessCollection.RunSelect(IBusinessObjectSearch search)" );
			//sb.AppendLine("		{" );
			//sb.AppendLine("		}" );
			//sb.AppendLine();
			//sb.AppendLine("		void IBusinessCollection.RunSelect()" );
			//sb.AppendLine("		{" );
			//sb.AppendLine("			;" );
			//sb.AppendLine("		}" );
			//sb.AppendLine();
      sb.AppendLine("		#endregion" );
      sb.AppendLine();
    }

    private void AppendMethodSelectParameter()
    {
      //Do not generate if there are no parameters
      //if(_currentStoredProcedure.Parameters.Count == 0)
      // return;

      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// Execute the cached select commands" );
      sb.AppendLine("		/// </summary>" );
      sb.Append("		public static " + _currentStoredProcedure.PascalName + "Collection RunSelect(");
      for(int ii = 0 ; ii < _currentStoredProcedure.Parameters.Count ; ii++)
      {
        Parameter parameter = (Parameter)_currentStoredProcedure.Parameters[ii].Object;
				if (parameter.IsOutputParameter)
					sb.Append("out ");

				sb.Append(parameter.GetCodeType() + " " + parameter.CamelName + ", ");				
			}
      sb.AppendLine("string modifier)" );
      sb.AppendLine("		{" );
      sb.AppendLine("			try" );
      sb.AppendLine("			{" );
      sb.Append("				" + _currentStoredProcedure.PascalName + "Collection returnVal = new " + _currentStoredProcedure.PascalName + "Collection(Domain" + _currentStoredProcedure.PascalName + "Collection.RunSelect(");

      for(int ii = 0 ; ii < _currentStoredProcedure.Parameters.Count ; ii++)
      {
        Parameter parameter = (Parameter)_currentStoredProcedure.Parameters[ii].Object;
				if (parameter.IsOutputParameter)
					sb.Append("out ");

				sb.Append(parameter.CamelName + ", ");
			}

      sb.AppendLine("modifier));" );
      sb.AppendLine("				return returnVal;" );
      sb.AppendLine("			}" );
      Globals.AppendBusinessEntryCatch(sb);
      sb.AppendLine("		}" );
      sb.AppendLine();
    }

    #endregion

    #region append IList Implementation
    private void AppendIListImplementation()
    {
      sb.AppendLine();
      sb.AppendLine("		#region IList Implementation" );
      sb.AppendLine("		public bool ContainsListCollection { get { return false; } }" );
      sb.AppendLine("" );
      sb.AppendLine("		public IList GetList()" );
      sb.AppendLine("		{" );
      sb.AppendLine("		 IList arrayList = new ArrayList();" );
      sb.AppendLine("" );
      sb.AppendLine("			foreach (object o in GetBusinessObjectList())" );
      sb.AppendLine("			{" );
      sb.AppendLine("				arrayList.Add(o);" );
      sb.AppendLine("			}" );
      sb.AppendLine("" );
      sb.AppendLine("			return arrayList;       " );
      sb.AppendLine("		}" );
      sb.AppendLine("		public BusinessObjectList<" + _currentStoredProcedure.PascalName + "> GetBusinessObjectList()" );
      sb.AppendLine("		{" );
      sb.AppendLine("			BusinessObjectList<" + _currentStoredProcedure.PascalName + "> retVal = new BusinessObjectList<" + _currentStoredProcedure.PascalName + ">();" );
      sb.AppendLine("			foreach (" + _currentStoredProcedure.PascalName + " currentVal in this)" );
      sb.AppendLine("			{" );
      sb.AppendLine("				retVal.Add(currentVal);" );
      sb.AppendLine("			}" );
      sb.AppendLine("			return retVal;" );
      sb.AppendLine("		}" );
      sb.AppendLine("		#endregion" );
    }


    #endregion

    #region append operator overloads
    private void AppendOperatorIndexer()
    {
      sb.AppendLine("		#region Enumerator" );
      sb.AppendLine();
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// Gets or sets the element at the specified index." );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("		/// <param name=\"index\">The zero-based index of the element to get or set. </param>" );
      sb.AppendLine("		/// <returns>The element at the specified index.</returns>" );
      sb.AppendLine("		public " + _currentStoredProcedure.PascalName + " this[int index] " );
      sb.AppendLine("		{" );
      sb.AppendLine("			get" );
      sb.AppendLine("			{" );
      sb.AppendLine("				try" );
      sb.AppendLine("				{" );
      sb.AppendLine("					IEnumerator internalEnumerator = this.GetEnumerator();" );
      sb.AppendLine("					internalEnumerator.Reset();" );
      sb.AppendLine("					int ii = -1;" );
      sb.AppendLine("					while(ii < index)" );
      sb.AppendLine("					{" );
      sb.AppendLine("						internalEnumerator.MoveNext();" );
      sb.AppendLine("						ii++;" );
      sb.AppendLine("					}" );
      //sb.AppendLine("					return (" + _currentStoredProcedure.PascalName + ")internalEnumerator.Current;" );
			sb.AppendLine("					" + _currentStoredProcedure.PascalName + " retval = (" + _currentStoredProcedure.PascalName + ")internalEnumerator.Current;");
			sb.AppendLine("					if (retval.wrappedClass == null)");
			sb.AppendLine("					{");
			sb.AppendLine("						if (!((0 <= index) && (index < this.Count)))");
			sb.AppendLine("							throw new IndexOutOfRangeException();");
			sb.AppendLine("						else");
			sb.AppendLine("							throw new Exception(\"The item is null. This is not a valid state.\");");
			sb.AppendLine("					}");
			sb.AppendLine("					else return (" + _currentStoredProcedure.PascalName + ")internalEnumerator.Current;");
			sb.AppendLine("				}");
      Globals.AppendBusinessEntryCatch(sb);
      sb.AppendLine("			}" );
      sb.AppendLine("		}" );
      sb.AppendLine();
      sb.AppendLine("		#endregion" );
      sb.AppendLine();
    }
    #endregion

    #region Reject Changes

    private void AppendRejectChanges()
    {
      sb.AppendLine("		/// <summary>" );
      sb.AppendLine("		/// Rejects all the changes associate with business object in this collection." );
      sb.AppendLine("		/// </summary>" );
      sb.AppendLine("		/// <returns>void</returns>" );
      sb.AppendLine("		public void RejectChanges()" );
      sb.AppendLine("		{" );
      sb.AppendLine("		  Domain" + _currentStoredProcedure.PascalName + "Collection coll = (Domain" + _currentStoredProcedure.PascalName + "Collection)this.wrappedClass;" );
      sb.AppendLine("		  coll.RejectChanges();" );
      sb.AppendLine("		}" );
      sb.AppendLine();
    }

    #endregion

    #region stringbuilders

    protected string GetDefaultForRequiredParam(string codeType)
    {
      if(StringHelper.Match(codeType, "long", true))
      {
        return "long.MinValue";
      }
      else if(StringHelper.Match(codeType, "System.Byte[]", true))
      {
        return "new byte[1]";
      }
      else if(StringHelper.Match(codeType, "bool", true))
      {
        return "false";
      }
      else if(StringHelper.Match(codeType, "string", true))
      {
        return "string.Empty";
      }
      else if(StringHelper.Match(codeType, "System.DateTime", true))
      {
        return "DateTime.MinValue";
      }
      else if(StringHelper.Match(codeType, "System.Decimal", true))
      {
        return "Decimal.MinValue";
      }
      else if(StringHelper.Match(codeType, "System.Double", true))
      {
        return "Double.MinValue";
      }
      else if(StringHelper.Match(codeType, "int", true))
      {
        return "int.MinValue";
      }
      else if(StringHelper.Match(codeType, "System.Single", true))
      {
        return "Single.MinValue";
      }
      else if(StringHelper.Match(codeType, "short", true))
      {
        return "short.MinValue";
      }
      else if(StringHelper.Match(codeType, "object", true))
      {
        return "new object";
      }
      else if(StringHelper.Match(codeType, "System.Byte", true))
      {
        return "System.Byte.MinValue";
      }
      else if(StringHelper.Match(codeType, "System.Guid", true))
      {
        return "System.Guid.Empty";
      }
      else
      {
        throw new Exception("No Default Value For Type Specified");
      }

    }

    #endregion
  }
}