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
using System.Collections;

namespace Widgetsphere.Generator.ProjectItemGenerators.DomainStoredProcedureCollection
{
	class DomainStoredProcedureCollectionGeneratedTemplate : BaseClassTemplate
	{

		private StringBuilder sb = new StringBuilder();
		private CustomStoredProcedure _currentStoredProcedure;

		public DomainStoredProcedureCollectionGeneratedTemplate(ModelRoot model, CustomStoredProcedure currentStoredProcedure)
		{
			_model = model;
			_currentStoredProcedure = currentStoredProcedure;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get
			{
				return string.Format("Domain{0}Collection.Generated.cs", _currentStoredProcedure.PascalName);
			}
		}

		public string ParentItemName
		{
			get
			{
				return string.Format("Domain{0}Collection.cs", _currentStoredProcedure.PascalName);
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
				sb.AppendLine("namespace " + DefaultNamespace + ".Domain.StoredProcedures");
				sb.AppendLine("{");
				this.AppendClass();
				sb.AppendLine("}");
				sb.AppendLine();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendUsingStatements()
		{
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Data;");
			sb.AppendLine("using System.Xml;");
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.ComponentModel;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using Widgetsphere.Core.Util;");
			sb.AppendLine("using Widgetsphere.Core.Logging;");
			sb.AppendLine("using Widgetsphere.Core.Exceptions;");
			sb.AppendLine("using " + DefaultNamespace + ".Business;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.StoredProcedures;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Rules;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.SelectCommands;");
			sb.AppendLine("using System.IO;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			sb.AppendLine("	#region internal class");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// This is an customizable extender for the domain class associated with the '" + _currentStoredProcedure.PascalName + "' object collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[Serializable()]");
			sb.AppendLine("	[System.ComponentModel.DesignerCategoryAttribute(\"code\")]");
			sb.AppendLine("	partial class Domain" + _currentStoredProcedure.PascalName + "Collection : ReadOnlyDomainCollection, IEnumerable, ISerializable");
			sb.AppendLine("	{");
			this.AppendRegionSerializable();
			this.AppendRegionProperties();
			this.AppendRegionConstructors();
			this.AppendRegionSelectMethods();
			this.AppendRegionCollectionMethods();
			this.AppendRegionDataColumnsSetup();
			this.AppendRegionProtectedOverrides();
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();
		}

		private void AppendRegionSerializable()
		{
			sb.AppendLine("		#region ISerializable Members");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected Domain" + _currentStoredProcedure.PascalName + "Collection(SerializationInfo info, StreamingContext context):this()");
			sb.AppendLine("		{");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Method used internally for serialization");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
			sb.AppendLine("		{");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionProperties()
		{
			sb.AppendLine("		#region Properties");
			sb.AppendLine();
			sb.AppendLine("		internal SubDomain SubDomain");
			sb.AppendLine("		{");
			sb.AppendLine("			get{return (SubDomain)this.DataSet;}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
		}

		private void AppendRegionConstructors()
		{
			sb.AppendLine("		#region Contructors /Initialization");
			sb.AppendLine();
			sb.AppendLine("		internal Domain" + _currentStoredProcedure.PascalName + "Collection():base(\"" + _currentStoredProcedure.PascalName + "\")");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				base.TableName = \"" + _currentStoredProcedure.PascalName + "Collection\";");
			sb.AppendLine("				this.InitClass();");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();			
			sb.AppendLine("		private void InitClass() ");
			sb.AppendLine("		{");
			sb.AppendLine("			BeginInit();");

			//Create columns
			foreach (Reference reference in _currentStoredProcedure.GeneratedColumns)
			{
				CustomStoredProcedureColumn column = (CustomStoredProcedureColumn)reference.Object;
				sb.AppendLine("			this.column" + column.PascalName + " = new DataColumn(\"" + column.DatabaseName + "\", typeof(" + column.GetCodeType() + "), null, System.Data.MappingType.Element);");
				sb.AppendLine("			base.Columns.Add(this.column" + column.PascalName + ");");
				sb.AppendLine("			this.column" + column.PascalName + ".AllowDBNull = true;");
				sb.AppendLine("			this.column" + column.PascalName + ".Caption = \"" + column.PascalName + "\";");
				sb.AppendLine("			this.column" + column.PascalName + ".ReadOnly = true;");
			}

			sb.AppendLine("			EndInit();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionSelectMethods()
		{
			sb.AppendLine("		#region Select Methods");
			sb.AppendLine();

			//Search RunSelect
			//if(_currentStoredProcedure.Parameters.Count > 0)
			//{
			sb.Append("		internal static Domain" + _currentStoredProcedure.PascalName + "Collection RunSelect(");
			for (int ii = 0; ii < _currentStoredProcedure.Parameters.Count; ii++)
			{
				Parameter parameter = (Parameter)_currentStoredProcedure.Parameters[ii].Object;
				if (parameter.IsOutputParameter)
					sb.Append("out ");

				sb.Append(parameter.GetCodeType() + " " + parameter.CamelName + ", ");
			}
			sb.AppendLine("string modifier)");
			sb.AppendLine("		{");

			sb.AppendLine("			Domain" + _currentStoredProcedure.PascalName + "Collection returnVal = null;");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("			" + _currentStoredProcedure.PascalName + "Select retrieveRule = new " + _currentStoredProcedure.PascalName + "Select(" + this.GetParameterList(false) + ");");
			sb.AppendLine("			sd.AddSelectCommand(retrieveRule);");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			returnVal = (Domain" + _currentStoredProcedure.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentStoredProcedure.PascalName + "Collection);");

			for (int ii = 0; ii < _currentStoredProcedure.Parameters.Count; ii++)
			{
				Parameter parameter = (Parameter)_currentStoredProcedure.Parameters[ii].Object;
				if (parameter.IsOutputParameter)
				{
					sb.AppendLine("			" + parameter.CamelName + " = retrieveRule." + parameter.PascalName + ";");
				}
			}

			sb.AppendLine("			return returnVal;");
			sb.AppendLine("		}");
			sb.AppendLine();
			//}

			sb.AppendLine("		#endregion");
			sb.AppendLine();

		}

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

		private void AppendRegionCollectionMethods()
		{
			sb.AppendLine("		#region Domain" + _currentStoredProcedure.PascalName + " Collection Methods");
			sb.AppendLine();
			sb.AppendLine("		internal void Remove" + _currentStoredProcedure.PascalName + "(Domain" + _currentStoredProcedure.PascalName + " element) ");
			sb.AppendLine("		{");
			sb.AppendLine("			base.Rows.Remove(element);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		internal int Count");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return base.Rows.Count; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns an enumerator that can be used to iterate through the collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns>An Enumerator that can iterate through the objects in this collection.</returns>");
			sb.AppendLine("		public System.Collections.IEnumerator GetEnumerator() ");
			sb.AppendLine("		{");
			sb.AppendLine("			return base.Rows.GetEnumerator();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets or sets the element at the specified index.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"index\">The zero-based index of the element to get or set. </param>");
			sb.AppendLine("		/// <returns>The element at the specified index.</returns>");
			sb.AppendLine("		internal Domain" + _currentStoredProcedure.PascalName + " this[int index]");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return ((Domain" + _currentStoredProcedure.PascalName + ")(base.Rows[index])); }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		internal DataView GetSorted" + _currentStoredProcedure.PascalName + "List(string sortString)");
			sb.AppendLine("		{");
			sb.AppendLine("			DataView returnView = base.DefaultView;");
			sb.AppendLine("			returnView.Sort = sortString;");
			sb.AppendLine("			return returnView;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionDataColumnsSetup()
		{
			sb.AppendLine("		#region Data Columns Setup");
			foreach (Reference reference in _currentStoredProcedure.GeneratedColumns)
			{
				CustomStoredProcedureColumn column = (CustomStoredProcedureColumn)reference.Object;
				sb.AppendLine("		private DataColumn column" + column.PascalName + ";");
				sb.AppendLine();
				sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(\"\", false, 0)]");
				sb.AppendLine("		public DataColumn " + column.PascalName + "Column");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this.column" + column.PascalName + "; }");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionProtectedOverrides()
		{
			sb.AppendLine("		#region Protected Overrides");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create a new instance of this object type");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override DataTable CreateInstance()");
			sb.AppendLine("		{");
			sb.AppendLine("			return new Domain" + _currentStoredProcedure.PascalName + "Collection();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create new row from a DataRowBuilder");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"builder\">The DataRowBuilder object that is used to create the new row</param>");
			sb.AppendLine("		/// <returns>A new datarow of type '" + _currentStoredProcedure.PascalName + "'</returns>");
			sb.AppendLine("		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) ");
			sb.AppendLine("		{");
			sb.AppendLine("			return new Domain" + _currentStoredProcedure.PascalName + "(builder);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the type of the datarow that this collection holds");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override System.Type GetRowType() ");
			sb.AppendLine("		{");
			sb.AppendLine("			return typeof(Domain" + _currentStoredProcedure.PascalName + ");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

	}
}