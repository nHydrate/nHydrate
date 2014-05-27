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
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.BusinessView
{
	class BusinessViewGeneratedTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private CustomView _currentView;

		public BusinessViewGeneratedTemplate(ModelRoot model, CustomView currentView)
		{
			_model = model;
			_currentView = currentView;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("{0}.Generated.cs", _currentView.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}.cs", _currentView.PascalName); }
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
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.Views");
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
			sb.AppendLine("using System.Data;");
			sb.AppendLine("using System.Xml;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using Widgetsphere.Core.Exceptions;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using " + DefaultNamespace + ".Business;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Rules;");
			sb.AppendLine("using " + DefaultNamespace + ".Domain.Views;");
			sb.AppendLine("using System.ComponentModel;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			try
			{
				sb.AppendLine("		/// <summary>");
				if (_currentView.Description != "")
					sb.AppendLine("		/// " + _currentView.Description);
				else
					sb.AppendLine("		/// Object definition for the '" + _currentView.PascalName + "' view");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("	[Serializable()]");
				sb.AppendLine("	public partial class " + _currentView.PascalName + " : BusinessObjectBase, IReadOnlyBusinessObject");

				sb.AppendLine("	{");
				this.AppendFullTemplate();
				this.AppendPropertyContainer();
				this.AppendRegionIBusinessObject();
				sb.AppendLine("	}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendRegionIBusinessObject()
		{
			sb.AppendLine("		#region IBusinessObject Members");
			sb.AppendLine();
			sb.AppendLine("		IPrimaryKey IBusinessObject.PrimaryKey");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		object IBusinessObject.GetValue(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetValue((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		object IBusinessObject.GetValue(Enum field, object defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetValue((FieldNameConstants)field, defaultValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		double IBusinessObject.GetDouble(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDouble((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		double IBusinessObject.GetDouble(Enum field, double defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDouble((FieldNameConstants)field, defaultValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		DateTime IBusinessObject.GetDateTime(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDateTime((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		DateTime IBusinessObject.GetDateTime(Enum field, DateTime defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDateTime((FieldNameConstants)field, defaultValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		int IBusinessObject.GetInteger(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetInteger((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		int IBusinessObject.GetInteger(Enum field, int defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetInteger((FieldNameConstants)field, defaultValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		string IBusinessObject.GetString(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetString((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		string IBusinessObject.GetString(Enum field, string defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetString((FieldNameConstants)field, defaultValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		Enum IBusinessObject.Container");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.Container; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines if all of the fields for the specified object exactly matches the current object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"item\">The object to compare</param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public override bool IsEquivalent(IBusinessObject item)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (item == null) return false;");
			sb.AppendLine("			if (!(item is " + _currentView.PascalName + ")) return false;");
			sb.AppendLine("			" + _currentView.PascalName + " o = item as " + _currentView.PascalName + ";");
			sb.AppendLine("			return (");

			int index = 0;
			foreach (Reference reference in _currentView.GeneratedColumns)
			{
				CustomViewColumn column = (CustomViewColumn)reference.Object;
				sb.Append("				o." + column.PascalName + " == this." + column.PascalName);
				if (index < _currentView.GeneratedColumns.Count - 1) sb.Append(" &&");
				sb.AppendLine();
				index++;
			}

			sb.AppendLine("				);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionGetMask()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the mask (if one is defined) for the specified field.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static string GetMask(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Reference reference in _currentView.Columns)
			{
				CustomViewColumn column = (CustomViewColumn)reference.Object;
				if (column.Generated)
				{
					sb.AppendLine("				case FieldNameConstants." + column.PascalName + ":");
					sb.AppendLine("					return \"\";");
				}
			}
			sb.AppendLine("			}");
			sb.AppendLine("			return \"\";");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		string IBusinessObject.GetMask(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetMask((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendRegionGetMaxLength()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the maximum size of the field value.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static int GetMaxLength(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Reference reference in _currentView.Columns)
			{
				CustomViewColumn column = (CustomViewColumn)reference.Object;
				if (column.Generated)
				{
					sb.AppendLine("				case FieldNameConstants." + column.PascalName + ":");
					if (ModelHelper.IsTextType(column.DataType))
						sb.AppendLine("					return " + column.Length + ";");
					else
						sb.AppendLine("					return 0;");
				}
			}
			sb.AppendLine("			}");
			sb.AppendLine("			return 0;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		int IBusinessObject.GetMaxLength(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetMaxLength((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendRegionGetFriendlyName()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the friendly name (if one is defined) of the field.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static string GetFriendlyName(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Reference reference in _currentView.Columns)
			{
				CustomViewColumn column = (CustomViewColumn)reference.Object;
				if (column.Generated)
				{
					sb.AppendLine("				case FieldNameConstants." + column.PascalName + ":");
					sb.AppendLine("					return \"" + column.FriendlyName + "\";");
				}
			}
			sb.AppendLine("			}");
			sb.AppendLine("			return \"\";");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		string IBusinessObject.GetFriendlyName(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetFriendlyName((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendRegionGetDefault()
		{
			sb.AppendLine("		#region GetDefault");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the default value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public object GetDefault(FieldNameConstants field)");
			sb.AppendLine("		{");

			foreach (Reference reference in _currentView.GeneratedColumns)
			{
				CustomViewColumn dbColumn = (CustomViewColumn)reference.Object;
				sb.AppendLine("			if(field == FieldNameConstants." + dbColumn.PascalName + ")");
				switch (dbColumn.DataType)
				{
					case System.Data.SqlDbType.Char:
					case System.Data.SqlDbType.NChar:
					case System.Data.SqlDbType.NText:
					case System.Data.SqlDbType.NVarChar:
					case System.Data.SqlDbType.Text:
					case System.Data.SqlDbType.VarChar:
					case System.Data.SqlDbType.UniqueIdentifier:
						sb.AppendLine("				return \"" + dbColumn.Default + "\";");
						break;
					case System.Data.SqlDbType.BigInt:
					case System.Data.SqlDbType.Int:
					case System.Data.SqlDbType.SmallInt:
					case System.Data.SqlDbType.TinyInt:
						if (dbColumn.AllowNull)
							sb.AppendLine("				return null;");
						else
						{
							long lvalue;
							if (long.TryParse("0" + dbColumn.Default, out lvalue))
								sb.AppendLine("				return 0" + dbColumn.Default + ";");
							else //THIS IS AN ERROR CONDITION
								sb.AppendLine("				return 0;");
						}
						break;
					case System.Data.SqlDbType.DateTime:
					case System.Data.SqlDbType.SmallDateTime:
						if (dbColumn.AllowNull)
							sb.AppendLine("				return null;");
						else
						{
							DateTime dtvalue;
							if (dbColumn.Default == "")
								sb.AppendLine("				return DateTime.Now;");
							else if (DateTime.TryParse(dbColumn.Default, out dtvalue))
								sb.AppendLine("				return DateTime.Parse(\"" + dbColumn.Default + "\");");
							else //THIS IS AN ERROR CONDITION
								sb.AppendLine("				return DateTime.Now;");
						}
						break;
					case System.Data.SqlDbType.Bit:
						bool bvalue;
						if (bool.TryParse(dbColumn.Default, out bvalue))
							sb.AppendLine("				return " + (bvalue ? "true" : "false") + ";");
						else //THIS IS AN ERROR CONDITION
							sb.AppendLine("				return false;");
						break;
					case System.Data.SqlDbType.Decimal:
					case System.Data.SqlDbType.Float:
					case System.Data.SqlDbType.Money:
					case System.Data.SqlDbType.Real:
					case System.Data.SqlDbType.SmallMoney:
						if (dbColumn.AllowNull)
							sb.AppendLine("				return null;");
						else
						{
							decimal dvalue;
							if (decimal.TryParse("0" + dbColumn.Default, out dvalue))
								sb.AppendLine("				return 0" + dbColumn.Default + ";");
							else //THIS IS AN ERROR CONDITION
								sb.AppendLine("				return 0;");
						}
						break;
				}
			}
			sb.AppendLine();
			sb.AppendLine("			return null;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		object IBusinessObject.GetDefault(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDefault((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendPropertyContainer()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the type of collection that contains this object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	public Collections Container");
			sb.AppendLine("	{");
			sb.AppendLine("		get { return Collections." + _currentView.PascalName + "Collection; }");
			sb.AppendLine("	}");
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
			sb.AppendLine("		internal " + _currentView.PascalName + "CollectionRules(Domain" + _currentView.PascalName + "Collection in" + _currentView.PascalName + "List)");
			sb.AppendLine("		{");
			sb.AppendLine("			col" + _currentView.PascalName + "List = in" + _currentView.PascalName + "List;");
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

		private void AppendMethodGetPropertyItem()
		{
			sb.AppendLine("		private " + _currentView.PascalName + "PropertyItem GetPropertyItem(IPropertyDefine definition)");
			sb.AppendLine("		{");
			sb.AppendLine("			" + _currentView.PascalName + "PropertyItem propertyItem = null;");
			sb.AppendLine("			foreach(" + _currentView.PascalName + "PropertyItem item in this." + _currentView.PascalName + "PropertyItemList)");
			sb.AppendLine("			{");
			sb.AppendLine("				if(item.PropertyItemDefineId == definition.PropertyItemDefineId)");
			sb.AppendLine("				{");
			sb.AppendLine("					propertyItem = item;");
			sb.AppendLine("					break;");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine("			return propertyItem;");
			sb.AppendLine("		}");
		}

		private void AppendMethodSetProperty()
		{
			sb.AppendLine("		public void SetProperty(IPropertyDefine definition, string value)");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				" + _currentView.PascalName + "PropertyItem propertyItem = GetPropertyItem(definition);");
			sb.AppendLine();
			sb.AppendLine("				//Create a new one if need be");
			sb.AppendLine("				" + _currentView.PascalName + "PropertyItemCollection propertyItemCollection = null;");
			sb.AppendLine("				if(propertyItem == null)");
			sb.AppendLine("				{");
			sb.AppendLine("					propertyItemCollection = (" + _currentView.PascalName + "PropertyItemCollection)this.ParentCollection.SubDomain[Collections." + _currentView.PascalName + "PropertyItemCollection];");
			sb.AppendLine("					propertyItem = (" + _currentView.PascalName + "PropertyItem)propertyItemCollection.NewItem();");
			//TODO - Make work with composite primary keys
			sb.AppendLine("					propertyItem." + ((Column)_currentView.PrimaryKeyColumns[0]).PascalName + " = this." + ((Column)_currentView.PrimaryKeyColumns[0]).PascalName + ";");
			sb.AppendLine("					propertyItem.PropertyItemDefineId = definition.PropertyItemDefineId;");
			sb.AppendLine("				}");
			sb.AppendLine();
			sb.AppendLine("				//Set the actual value");
			sb.AppendLine("				propertyItem.ItemValue = value;");
			sb.AppendLine();
			sb.AppendLine("				//Add to collection if new");
			sb.AppendLine("				if(!propertyItem.IsParented)");
			sb.AppendLine("					propertyItemCollection.AddItem(propertyItem);");
			sb.AppendLine();
			sb.AppendLine("			}");
			sb.AppendLine("			catch(Exception ex)");
			sb.AppendLine("			{");
			sb.AppendLine("				throw;");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("		}");
		}

		private void AppendMethodGetPropertyDefinitions()
		{
			sb.AppendLine("		public List<IPropertyDefine> GetPropertyDefinitions()");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.ParentCollection.wrappedClass.GetPropertyDefinitions();");
			sb.AppendLine("		}");
		}

		private void AppendMethodGetProperty()
		{
			sb.AppendLine("		public string GetProperty(IPropertyDefine definition)");
			sb.AppendLine("		{");
			sb.AppendLine("			" + _currentView.PascalName + "PropertyItem propertyItem = GetPropertyItem(definition);");
			sb.AppendLine("			if(propertyItem == null)");
			sb.AppendLine("				return null;");
			sb.AppendLine("			else");
			sb.AppendLine("				return propertyItem.ItemValue;");
			sb.AppendLine("		}");
		}

		#endregion

		#region append operator overloads
		#endregion

		#region AppendFullTemplate

		public void AppendFullTemplate()
		{
			try
			{
				sb.AppendLine("		#region Class Members");
				sb.AppendLine();
				sb.AppendLine("		#region FieldNameConstants Enumeration");
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Enumeration to define each property that maps to a database field for the '" + _currentView.PascalName + "' view.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public enum FieldNameConstants");
				sb.AppendLine("		{");
				foreach (Reference reference in _currentView.GeneratedColumns)
				{
					CustomViewColumn column = (CustomViewColumn)reference.Object;
					sb.AppendLine("			 /// <summary>");
					sb.AppendLine("			 /// Field mapping for the '" + column.PascalName + "' property");
					sb.AppendLine("			 /// </summary>");
					sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
					sb.AppendLine("			" + column.PascalName + ",");
				}
				sb.AppendLine("		}");
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		internal Domain" + _currentView.PascalName + " wrappedClass;");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				//Constructors
				sb.AppendLine("		#region Constructors");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Creates a '" + _currentView.PascalName + "' object from a domain object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		internal " + _currentView.PascalName + "(Domain" + _currentView.PascalName + " classToWrap)");
				sb.AppendLine("		{");
				sb.AppendLine("			wrappedClass = classToWrap;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Creates a '" + _currentView.PascalName + "' object from a DataRowView");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentView.PascalName + "(DataRowView drvToWrap)");
				sb.AppendLine("		{");
				sb.AppendLine("			wrappedClass = (Domain" + _currentView.PascalName + ")drvToWrap.Row;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				//Events
				sb.AppendLine("		#region Events");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// This event is raised when any field is changed.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public event PropertyChangedEventHandler PropertyChanged;");

				foreach (Reference reference in _currentView.GeneratedColumns)
				{
					CustomViewColumn dbColumn = (CustomViewColumn)reference.Object;
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// This event is raised when the '" + dbColumn.PascalName + "' field is changed.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public event EventHandler " + dbColumn.PascalName + "Changed;");
				}
				sb.AppendLine();

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// This event is raised when any field is changed.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (this.PropertyChanged != null)");
				sb.AppendLine("				this.PropertyChanged(this, e);");
				sb.AppendLine("		}");
				sb.AppendLine();

				foreach (Reference reference in _currentView.GeneratedColumns)
				{
					CustomViewColumn dbColumn = (CustomViewColumn)reference.Object;
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Raises the " + dbColumn.PascalName + "Changed event.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected virtual void On" + dbColumn.PascalName + "Changed(System.EventArgs e)");
					sb.AppendLine("		{");
					sb.AppendLine("			if (this." + dbColumn.PascalName + "Changed != null)");
					sb.AppendLine("				this." + dbColumn.PascalName + "Changed(this, e);");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				//Properties
				sb.AppendLine("		#region Properties");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Determines if this object is part of a collection or is detached");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
				sb.AppendLine("		public bool IsParented");
				sb.AppendLine("		{");
				sb.AppendLine("		  get { return (((DataRow)this.WrappedClass).RowState != DataRowState.Detached); }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Returns the internal object that this object wraps");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
				sb.AppendLine("		public object WrappedClass");
				sb.AppendLine("		{");
				sb.AppendLine("			get{return wrappedClass;}");
				sb.AppendLine("			set{wrappedClass = (Domain" + _currentView.PascalName + ")value;}");
				sb.AppendLine("		}");
				sb.AppendLine();

				foreach (Reference reference in _currentView.GeneratedColumns)
				{
					CustomViewColumn dbColumn = (CustomViewColumn)reference.Object;

					sb.AppendLine("			 	");
					sb.AppendLine();
					sb.AppendLine();
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					if (dbColumn.Description != "")
						sb.AppendLine("		/// " + dbColumn.Description);
					else
						sb.AppendLine("		/// Property for View field: " + dbColumn.PascalName);
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(\"" + dbColumn.FriendlyName + "\", " + dbColumn.GridVisible.ToString().ToLower() + ", " + dbColumn.SortOrder.ToString() + ")]");
					sb.AppendLine("		public " + dbColumn.GetCodeType() + " " + EnsureValidPropertyName(dbColumn.PascalName) + "");
					sb.AppendLine("		{");
					sb.AppendLine("			get");
					sb.AppendLine("			{");
					sb.AppendLine("				try");
					sb.AppendLine("				{");
					sb.AppendLine("					return wrappedClass." + dbColumn.PascalName + ";");
					sb.AppendLine("				}");
					Globals.AppendBusinessEntryCatch(sb);
					sb.AppendLine("			}");
					sb.AppendLine();
					sb.AppendLine("		}");
				}
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Null Methods");

				foreach (Reference reference in _currentView.GeneratedColumns)
				{
					CustomViewColumn dbColumn = (CustomViewColumn)reference.Object;
					if (dbColumn.AllowNull)
					{
						sb.AppendLine();
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// Determines if the field '" + dbColumn.PascalName + "' is null");
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		/// <returns>Boolean value indicating if the specified value is null</returns>");
						sb.AppendLine("		public bool Is" + dbColumn.PascalName + "Null() ");
						sb.AppendLine("		{");
						sb.AppendLine("			try");
						sb.AppendLine("			{");
						sb.AppendLine("				return wrappedClass.Is" + dbColumn.PascalName + "Null();");
						sb.AppendLine("			}");
						Globals.AppendBusinessEntryCatch(sb);
						sb.AppendLine("		}");
						sb.AppendLine();

					}
				}

				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Collection Operation Methods");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Removes this object from its parent collection. It will not be deleted from the database.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public void Remove()");
				sb.AppendLine("		{");
				sb.AppendLine("			try");
				sb.AppendLine("			{");
				sb.AppendLine("				wrappedClass.Remove();");
				sb.AppendLine("			}");
				Globals.AppendBusinessEntryCatch(sb);
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Overrides");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Serves as a hash function for this particular type.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override int GetHashCode()");
				sb.AppendLine("		{");
				sb.AppendLine("		  return base.GetHashCode();");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Returns a value indicating whether the current object is equal to a specified object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override bool Equals(object obj)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (obj == null)");
				sb.AppendLine("			{");
				sb.AppendLine("				return false;");
				sb.AppendLine("			}");
				sb.AppendLine("			else if (obj.GetType() == this.GetType())");
				sb.AppendLine("			{");
				sb.AppendLine("				if (this.wrappedClass == ((" + _currentView.PascalName + ")obj).wrappedClass)");
				sb.AppendLine("					return true;");
				sb.AppendLine("			}");
				sb.AppendLine("			return false;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				//IBusinessObject.ParentCollection  
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// This is a reference back to the generic collection that holds this object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		#region IBusinessObject ParentCollection");
				sb.AppendLine();
				sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
				sb.AppendLine("		IBusinessCollection IBusinessObject.ParentCollection");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				return ParentCollection;");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				//ParentCollection
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// This is a reference back to the collection that holds this object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		#region ParentCollection");
				sb.AppendLine();
				sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
				sb.AppendLine("		public " + _currentView.PascalName + "Collection ParentCollection");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				return new " + _currentView.PascalName + "Collection(wrappedClass.ParentCol);");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				this.AppendRegionGetFriendlyName();
				this.AppendRegionGetMaxLength();
				this.AppendRegionGetMask();
				this.AppendRegionGetDefault();
				this.AppendRegionGetValue();
				this.AppendRegionGetFieldLength();

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendRegionGetFieldLength()
		{
			sb.AppendLine("		#region GetFieldLength");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the length of the specified field if it can be set.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int GetFieldLength(" + _currentView.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			foreach (Reference colRef in _currentView.Columns)
			{
				CustomViewColumn column = (CustomViewColumn)colRef.Object;
				if (ModelHelper.IsTextType(column.DataType))
				{
					sb.AppendLine("			if(field == FieldNameConstants." + column.PascalName + ")");
					sb.AppendLine("				return " + column.Length + ";");
				}
			}
			sb.AppendLine("			//Catch all for all other data types");
			sb.AppendLine("			return -1;");
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionGetValue()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		#region GetValue");
			sb.AppendLine();
			sb.AppendLine("		public object GetValue(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return GetValue(field, null);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public object GetValue(FieldNameConstants field, object defaultValue)");
			sb.AppendLine("		{");
			foreach (Reference reference in _currentView.GeneratedColumns)
			{
				CustomViewColumn dbColumn = (CustomViewColumn)reference.Object;
				sb.AppendLine("		if(field == FieldNameConstants." + dbColumn.PascalName + ")");
				sb.AppendLine("		{");
				if (dbColumn.AllowNull)
					sb.AppendLine("			return (this.Is" + dbColumn.PascalName + "Null() ? defaultValue : this." + EnsureValidPropertyName(dbColumn.PascalName) + ");");
				else
					sb.AppendLine("			return this." + EnsureValidPropertyName(dbColumn.PascalName) + ";");

				sb.AppendLine("		}");
			}
			sb.AppendLine("		throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
			sb.AppendLine("		}");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int GetInteger(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetInteger(field, int.MinValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int GetInteger(FieldNameConstants field, int defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			object o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				int a;");
			sb.AppendLine("				if (int.TryParse((string)o, out a))");
			sb.AppendLine("					return a;");
			sb.AppendLine("				else");
			sb.AppendLine("					throw new InvalidCastException();");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				return (int)o;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public double GetDouble(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDouble(field, double.MinValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public double GetDouble(FieldNameConstants field, double defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			object o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				double a;");
			sb.AppendLine("				if (double.TryParse((string)o, out a))");
			sb.AppendLine("					return a;");
			sb.AppendLine("				else");
			sb.AppendLine("					throw new InvalidCastException();");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				return (double)o;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public DateTime GetDateTime(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDateTime(field, DateTime.MinValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public DateTime GetDateTime(FieldNameConstants field, DateTime defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			object o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				DateTime a;");
			sb.AppendLine("				if (DateTime.TryParse((string)o, out a))");
			sb.AppendLine("					return a;");
			sb.AppendLine("				else");
			sb.AppendLine("					throw new InvalidCastException();");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				return (DateTime)o;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string GetString(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetString(field, \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string GetString(FieldNameConstants field, string defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			object o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				return (string)o;");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				return o.ToString();");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

		#region string helpers
		protected string EnsureValidPropertyName(string propertyName)
		{
			string appendVal = string.Empty;
			if (StringHelper.Match(propertyName, _currentView.PascalName, false))
			{
				appendVal = "Member";
			}
			return propertyName + appendVal;
		}
		#endregion

	}
}