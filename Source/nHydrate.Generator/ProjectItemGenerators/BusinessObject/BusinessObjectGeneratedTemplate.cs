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
using System.Linq;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;
using System.Collections;
using System.Collections.ObjectModel;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.BusinessObject
{
	class BusinessObjectGeneratedTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _currentTable;

		public BusinessObjectGeneratedTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}

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
				this.AppendEnum();
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendEnum()
		{
			try
			{
				if (_currentTable.IsTypeTable && _currentTable.PrimaryKeyColumns.Count == 1)
				{
					sb.AppendLine("		#region StaticDataConstants Enumeration");
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Enumeration to define static data items and their ids '" + _currentTable.PascalName + "' table.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public enum " + _currentTable.PascalName + "Constants");
					sb.AppendLine("		{");
					foreach (RowEntry rowEntry in _currentTable.StaticData)
					{
						////ToDo: More than one column as primary key blows up
						//string id = string.Empty;
						//string name = string.Empty;
						//string description = string.Empty;
						//foreach (CellEntry cellEntry in rowEntry.CellEntries)
						//{
						//  Column column = (Column)cellEntry.ColumnRef.Object;
						//  Column pk = (Column)_currentTable.PrimaryKeyColumns[0];
						//  if (column.Key == pk.Key)
						//    id = cellEntry.Value;
						//  if (StringHelper.Match(column.Name, "name"))
						//    name = ValidationHelper.MakeCodeIdenitifer(cellEntry.Value);
						//  if (StringHelper.Match(column.Name, "description"))
						//    description = cellEntry.Value;
						//}

						//if (string.IsNullOrEmpty(name)) name = description;
						//if (description != "")
						//{
						//  sb.AppendLine("			/// <summary>");
						//  sb.AppendLine("			/// " + description);
						//  sb.AppendLine("			/// </summary>");
						//  sb.AppendLine("			[Description(\"" + description + "\")]");
						//}
						//else
						//{
						//  sb.AppendLine("			/// <summary>");
						//  sb.AppendLine("			/// Enumeration for the '" + name + "' item");
						//  sb.AppendLine("			/// </summary>");
						//  sb.AppendLine("			[Description(\"" + description + "\")]");
						//}

						string idValue = rowEntry.GetCodeIdValue(_currentTable);
						string identifier = rowEntry.GetCodeIdentifier(_currentTable);
						string description = rowEntry.GetCodeDescription(_currentTable);
						if (description != "")
						{
							sb.AppendLine("			/// <summary>");
							sb.AppendLine("			/// " + description);
							sb.AppendLine("			/// </summary>");
							sb.AppendLine("			[Description(\"" + description + "\")]");
						}
						else
						{
							sb.AppendLine("			/// <summary>");
							sb.AppendLine("			/// Enumeration for the '" + identifier + "' item");
							sb.AppendLine("			/// </summary>");
							sb.AppendLine("			[Description(\"" + description + "\")]");
						}

						string key = ValidationHelper.MakeDatabaseIdenitifer(identifier.Replace(" ", ""));
						if ((key.Length > 0) && ("0123456789".Contains(key[0])))
							key = "_" + key;

						sb.AppendLine("			" + key + " = " + idValue + ",");
					}
					sb.AppendLine("		}");
					sb.AppendLine("		#endregion");
					sb.AppendLine();
				}
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
			sb.AppendLine("using Widgetsphere.Core.Logging;");
			sb.AppendLine("using Widgetsphere.Core.Exceptions;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using " + DefaultNamespace + ".Business;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Rules;");
			sb.AppendLine("using Widgetsphere.Core.WinUI;");
			sb.AppendLine("using " + DefaultNamespace + ".Domain.Objects;");
			sb.AppendLine("using System.ComponentModel;");
			sb.AppendLine("using Widgetsphere.Core.EventArgs;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects.Composites;");
			sb.AppendLine("using System.IO;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			try
			{
				//Inherit from base class if need be
				string baseClass = "BusinessObjectPersistableBase";
				string baseInterface = "IPersistableBusinessObject";
				string baseObjectInterface = "";
				if (_currentTable.Immutable)
				{
					baseClass = "BusinessObjectBase";
					baseInterface = "IBusinessObject";
				}

				if (_currentTable.ParentTable != null)
				{
					baseClass = _currentTable.ParentTable.PascalName;
					baseObjectInterface = "I" + _currentTable.ParentTable.PascalName;
				}

				string auditableInterface = "";
				if (_currentTable.IsAuditable) auditableInterface = ", IAuditable";

				string cloneableInterface = "";
				if (!_currentTable.Immutable) cloneableInterface = ", ICloneable";

				#region Build the object Interface class

				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// Interface definition for the '" + _currentTable.PascalName + "' entity");
				if (_currentTable.Description != "")
					sb.AppendLine("	/// " + _currentTable.Description);
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	public interface I" + _currentTable.PascalName + " : " + baseInterface + (baseObjectInterface == "" ? "" : ", " + baseObjectInterface) + auditableInterface + cloneableInterface);
				sb.AppendLine("	{");
				sb.AppendLine("		#region Properties");
				sb.AppendLine();
				foreach (Reference reference in _currentTable.GeneratedColumns)
				{
					Column column = (Column)reference.Object;

					if (_currentTable.PrimaryKeyColumns.Contains(column) && (_currentTable.ParentTable != null))
					{
						//Do Nothing
					}
					else
					{
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// The " + column.PascalName + " field");
						sb.AppendLine("		/// </summary>");
						if (_currentTable.PrimaryKeyColumns.Contains(column) || _currentTable.Immutable)
							sb.AppendLine("		" + column.GetCodeType(true) + " " + column.PascalName + " { get; }");
						else
							sb.AppendLine("		" + column.GetCodeType(true) + " " + column.PascalName + " { get; set; }");
					}
				}

				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Methods");
				sb.AppendLine();
				if (_currentTable.ParentTable == null)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// This event is called before persisting to the database.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		event EventHandler<BusinessObjectEventArgs> Validate;");
					sb.AppendLine();
				}

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Removes this object from its parent collection. It will not be deleted from the database.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		void Remove();");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The primary key for this object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		new " + _currentTable.PascalName + "PrimaryKey PrimaryKey { get; }");
				sb.AppendLine();

				if (!_currentTable.Immutable)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Persists this object to the database.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		new void Persist();");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Creates a clone of this object.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		new IBusinessObject Clone();");
					sb.AppendLine();
				}

				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("	}");
				sb.AppendLine();

				#endregion

				string description = _currentTable.Description;
				description = description.Replace("\r\n", "\n");
				description = description.Replace("\r", "\n");
				string[] descriptionArray = description.Split('\n');
				
				sb.AppendLine("	/// <summary>");
				if (_currentTable.Description == "")
				{
					sb.AppendLine("	/// Class definition for the '" + _currentTable.PascalName + "' entity");
				}
				else
				{
					foreach (string s in descriptionArray)
					{
						sb.AppendLine("	/// " + s);
					}
				}
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[Serializable()]");

				if (_currentTable.IsMetaDataDefinition)
					sb.AppendLine("	public partial class " + _currentTable.PascalName + " : " + baseClass + ", " + baseInterface + ", I" + _currentTable.PascalName + ", IPropertyDefine");
				else if (_currentTable.CreateMetaData)
					sb.AppendLine("	public partial class " + _currentTable.PascalName + " : " + baseClass + ", " + baseInterface + ", I" + _currentTable.PascalName + ", IPropertyBag");
				else
					sb.AppendLine("	public partial class " + _currentTable.PascalName + " : " + baseClass + ", " + baseInterface + ", I" + _currentTable.PascalName);

				sb.AppendLine("	{");
				this.AppendFullTemplate();
				this.AppendRegionPropertyItemBag();
				this.AppendMethodIPropertyItemDefine();
				this.AppendMethodCloneItem();
				this.AppendImageMethods();

				sb.AppendLine("		#region Helper Methods");
				sb.AppendLine();
				this.AppendRegionGetFriendlyName();
				this.AppendRegionGetMaxLength();
				this.AppendRegionGetMask();
				this.AppendMethodSelectByPrimaryKey();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				this.AppendRegionIBusinessObject();
				this.AppendIAuditable();
				this.AppendRegionFieldDescriptor();
				sb.AppendLine("	}");
				sb.AppendLine();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendMethodSelectByPrimaryKey()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a single object from this collection by its primary key.");
			sb.AppendLine("		/// </summary>");
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public static " + _currentTable.PascalName + " SelectUsingPK(" + PrimaryKeyParameterList(true) + ")");
			else
				sb.AppendLine("		public new static " + _currentTable.PascalName + " SelectUsingPK(" + PrimaryKeyParameterList(true) + ")");

			sb.AppendLine("		{");
			sb.AppendLine("			return SelectUsingPK(" + PrimaryKeyParameterList(false) + ", \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Select a single object from this collection by its primary key.");
			sb.AppendLine("		/// </summary>");
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public static " + _currentTable.PascalName + " SelectUsingPK(" + PrimaryKeyParameterList(true) + ", string modifier)");
			else
				sb.AppendLine("		public new static " + _currentTable.PascalName + " SelectUsingPK(" + PrimaryKeyParameterList(true) + ", string modifier)");

			sb.AppendLine("		{");
			sb.AppendLine("			" + _currentTable.PascalName + "Collection " + StringHelper.DatabaseNameToCamelCase(_currentTable.PascalName) + " = " + _currentTable.PascalName + "Collection.SelectUsingPK(" + PrimaryKeyInputParameterList() + ", modifier);");
			sb.AppendLine("			if (" + _currentTable.CamelName + ".Count > 1) throw new Exception(\"" + _currentTable.PascalName + " primary key returned more than one value.\");");
			sb.AppendLine("			else if (" + _currentTable.CamelName + ".Count == 0) return null;");
			sb.AppendLine("			else return " + _currentTable.CamelName + "[0];");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendRegionFieldDescriptor()
		{
			//REMOVED 03/26/2009

			//sb.AppendLine("		#region FieldDescriptor");
			//sb.AppendLine();
			//sb.AppendLine("		public static List<IFieldDescriptor> GetFieldDescriptors(IList<Type> types)");
			//sb.AppendLine("		{");
			//sb.AppendLine("			List<IFieldDescriptor> retval = new List<IFieldDescriptor>();");
			//sb.AppendLine("			foreach (Type type in types)");
			//sb.AppendLine("			{");
			//sb.AppendLine("				List<IFieldDescriptor> list2 = GetFieldDescriptors(type);");
			//sb.AppendLine("				foreach (IFieldDescriptor pd in list2)");
			//sb.AppendLine("				{");
			//sb.AppendLine("					if (!retval.Contains(pd))");
			//sb.AppendLine("						retval.Add(pd);");
			//sb.AppendLine("				}");
			//sb.AppendLine("			}");
			//sb.AppendLine("			return retval;");
			//sb.AppendLine("		}");
			//sb.AppendLine();
			//sb.AppendLine("		public static List<IFieldDescriptor> GetFieldDescriptors(System.Type type)");
			//sb.AppendLine("		{");
			//sb.AppendLine("			List<IFieldDescriptor> retval = new List<IFieldDescriptor>();");
			//sb.AppendLine("			foreach (IFieldDescriptor d in GetFieldDescriptors())");
			//sb.AppendLine("			{");
			//sb.AppendLine("				if (d.GetType().GetInterface(type.Name) != null)");
			//sb.AppendLine("					retval.Add(d);");
			//sb.AppendLine("			}");
			//sb.AppendLine("			return retval;");
			//sb.AppendLine("		}");
			//sb.AppendLine();
			//sb.AppendLine("		public static List<IFieldDescriptor> GetFieldDescriptors()");
			//sb.AppendLine("		{");
			//sb.AppendLine("			List<IFieldDescriptor> retval = new List<IFieldDescriptor>();");
			//foreach (Reference reference in _currentTable.GeneratedColumns)
			//{
			//  Column column = (Column)reference.Object;
			//  if (column.Generated)
			//  {
			//    sb.AppendLine("			retval.Add(new " + _currentTable.PascalName + column.PascalName + "FieldDescriptor());");
			//  }
			//}
			//sb.AppendLine("			return retval;");
			//sb.AppendLine("		}");
			//sb.AppendLine();
			//sb.AppendLine("		#endregion");
			//sb.AppendLine();
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

			ColumnCollection allColumns = _currentTable.GetColumnsFullHierarchy(true);
			foreach (Column column in allColumns)
			{
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

		private void AppendRegionGetMaxLength()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the maximum size of the field value.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static int GetMaxLength(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Reference reference in _currentTable.GeneratedColumns)
			{
				Column column = (Column)reference.Object;
				if (column.Generated)
				{
					sb.AppendLine("				case FieldNameConstants." + column.PascalName + ":");
					switch (column.DataType)
					{
						case System.Data.SqlDbType.Text:
							sb.AppendLine("					return int.MaxValue;");
							break;
						case System.Data.SqlDbType.NText:
							sb.AppendLine("					return int.MaxValue;");
							break;
						case System.Data.SqlDbType.Image:
							sb.AppendLine("					return int.MaxValue;");
							break;
						case System.Data.SqlDbType.Char:
						case System.Data.SqlDbType.NChar:
						case System.Data.SqlDbType.NVarChar:
						case System.Data.SqlDbType.VarChar:
							if ((column.Length == 0) && (ModelHelper.SupportsMax(column.DataType)))
								sb.AppendLine("					return int.MaxValue;");
							else
								sb.AppendLine("					return " + column.Length + ";");
							break;
						default:
							sb.AppendLine("					return 0;");
							break;
					}
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

		private void AppendRegionGetMask()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the mask (if one is defined) for the specified field.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static string GetMask(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");

			ColumnCollection allColumns = _currentTable.GetColumnsFullHierarchy(true);
			foreach (Column column in allColumns)
			{
				if (column.Generated)
				{
					sb.AppendLine("				case FieldNameConstants." + column.PascalName + ":");
					sb.AppendLine("					return \"" + column.Mask + "\";");
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

		private void AppendRegionIBusinessObject()
		{
			sb.AppendLine("		#region IBusinessObject Members");
			sb.AppendLine();
			sb.AppendLine("		IPrimaryKey IBusinessObject.PrimaryKey");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.PrimaryKey; }");
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

			if (!_currentTable.Immutable)
			{
				sb.AppendLine("		void IPersistableBusinessObject.SetValue(Enum field, object newValue)");
				sb.AppendLine("		{");
				sb.AppendLine("			this.SetValue((FieldNameConstants)field, newValue);");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

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
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public override bool IsEquivalent(IBusinessObject item)");
			else
				sb.AppendLine("		public override bool IsEquivalent(IBusinessObject item)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (item == null) return false;");
			sb.AppendLine("			if (!(item is " + _currentTable.PascalName + ")) return false;");
			sb.AppendLine("			" + _currentTable.PascalName + " o = item as " + _currentTable.PascalName + ";");
			sb.AppendLine("			return (");

			IEnumerable<Column> allColumns = _currentTable.GetColumnsFullHierarchy(true).Where(x => x.Generated);
			int index = 0;
			foreach (Column column in allColumns)
			{
				sb.Append("				o." + column.PascalName + " == this." + column.PascalName);
				if (index < allColumns.Count() - 1) sb.Append(" &&");
				sb.AppendLine();
				index++;
			}

			sb.AppendLine("				);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendIAuditable()
		{
			//If there are no audits tehn do not do this
			if (!_currentTable.IsAuditable) return;

			sb.AppendLine("		#region IAuditable Members");
			sb.AppendLine();
			sb.AppendLine("		string IAuditable.CreatedBy");
			sb.AppendLine("		{");
			if (!_currentTable.AllowCreateAudit) sb.AppendLine("			get { throw new NotImplementedException(); }");
			else sb.AppendLine("			get { return this." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		DateTime? IAuditable.CreatedDate");
			sb.AppendLine("		{");
			if (!_currentTable.AllowCreateAudit) sb.AppendLine("			get { throw new NotImplementedException(); }");
			else sb.AppendLine("			get { return this." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		bool IAuditable.IsCreateAuditImplemented");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return " + _currentTable.AllowCreateAudit.ToString().ToLower() + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		bool IAuditable.IsModifyAuditImplemented");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return " + _currentTable.AllowModifiedAudit.ToString().ToLower() + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		bool IAuditable.IsTimestampAuditImplemented");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return " + _currentTable.AllowTimestamp.ToString().ToLower() + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		string IAuditable.ModifiedBy");
			sb.AppendLine("		{");
			if (!_currentTable.AllowModifiedAudit) sb.AppendLine("			get { throw new NotImplementedException(); }");
			else sb.AppendLine("			get { return this." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		DateTime? IAuditable.ModifiedDate");
			sb.AppendLine("		{");
			if (!_currentTable.AllowModifiedAudit) sb.AppendLine("			get { throw new NotImplementedException(); }");
			else sb.AppendLine("			get { return this." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		byte[] IAuditable.TimeStamp");
			sb.AppendLine("		{");
			if (!_currentTable.AllowTimestamp) sb.AppendLine("			get { throw new NotImplementedException(); }");
			else sb.AppendLine("			get { return this." + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendPropertyContainer()
		{
			if (_currentTable.ParentTable == null)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Determines the type of collection that contains this object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
				sb.AppendLine("		public Collections Container");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return Collections." + _currentTable.PascalName + "Collection; }");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
		}

		#endregion

		#region append regions

		private void AppendRegionPropertyItemBag()
		{
			if (_currentTable.CreateMetaData)
			{
				sb.AppendLine();
				sb.AppendLine("		#region IPropertyBag Members");
				sb.AppendLine();
				this.AppendMethodGetProperty();
				this.AppendMethodGetPropertyDefinitions();
				this.AppendMethodSetProperty();
				this.AppendMethodGetPropertyItem();
				sb.AppendLine();
				sb.AppendLine("		#endregion");
			}

		}

		private void AppendMethodCloneItem()
		{
			if (_currentTable.IsTypeTable || _currentTable.Immutable)
				return;

			string scope = "public";
			if (_currentTable.Immutable)
				scope = "protected internal";

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create a copy of this object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		" + scope + (_currentTable.ParentTable == null ? " virtual" : " override") + " IBusinessObject Clone()");
			sb.AppendLine("		{");
			sb.AppendLine("			lock (this)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (!this.IsParented) throw new Exception(\"The current item must be parented.\");");
			sb.AppendLine("				" + _currentTable.PascalName + " newItem = null;");
			sb.AppendLine("				lock (this.ParentCollection.SubDomain)");
			sb.AppendLine("				{");
			sb.AppendLine("					newItem = new " + _currentTable.PascalName + "(this.ParentCollection.wrappedClass.NewItem());");
			sb.AppendLine("				}");
			sb.AppendLine("				this.FillClone(newItem);");
			sb.AppendLine("				return newItem;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create a copy of this object.");
			foreach (Column column in _currentTable.PrimaryKeyColumns)
			{
				sb.AppendLine("		/// <param name=\"" + column.CamelName + "\">The " + column.CamelName + " field</param>");
			}

			sb.AppendLine("		/// </summary>");
			sb.Append("		" + scope + (_currentTable.ParentTable == null ? " virtual" : " override") + " IBusinessObject Clone(");
			foreach (Column column in _currentTable.PrimaryKeyColumns)
			{
				sb.Append(column.GetCodeType(true) + " " + column.CamelName);
				if (_currentTable.PrimaryKeyColumns.IndexOf(column) < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(")");
			sb.AppendLine("		{");
			sb.AppendLine("			lock (this)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (!this.IsParented) throw new Exception(\"The current item must be parented.\");");
			sb.AppendLine("				" + _currentTable.PascalName + " newItem = null;");
			sb.AppendLine("				lock (this.ParentCollection.wrappedClass.SubDomain)");
			sb.AppendLine("				{");
			sb.Append("				newItem = new " + _currentTable.PascalName + "(this.ParentCollection.wrappedClass.NewItem(");
			foreach (Column column in _currentTable.PrimaryKeyColumns)
			{
				sb.Append(column.CamelName);
				if (_currentTable.PrimaryKeyColumns.IndexOf(column) < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine("));");
			sb.AppendLine("				}");
			sb.AppendLine("				this.FillClone(newItem);");
			sb.AppendLine("				return newItem;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		private void FillClone(" + _currentTable.PascalName + " newItem)");
			sb.AppendLine("		{");
			sb.AppendLine("			lock (this)");
			sb.AppendLine("			{");
			sb.AppendLine("				lock (newItem)");
			sb.AppendLine("				{");
			ColumnCollection allColumns = _currentTable.GetColumnsFullHierarchy(true);
			foreach (Column column in allColumns.Where(x => x.Generated))
			{
				if (!_currentTable.PrimaryKeyColumns.Contains(column))
					sb.AppendLine("					newItem." + column.PascalName + " = this." + column.PascalName + ";");
			}
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();

			if (!_currentTable.Immutable)
			{
				sb.AppendLine("		object ICloneable.Clone()");
				sb.AppendLine("		{");
				sb.AppendLine("			return this.Clone();");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
		}

		private void AppendMethodIPropertyItemDefine()
		{
			if (_currentTable.IsMetaDataDefinition)
			{
				string tableName = _currentTable.PascalName.Replace("Define", "");
				sb.AppendLine("		#region IPropertyDefine Members");
				sb.AppendLine();
				sb.AppendLine("		BusinessObjectList<IBusinessObject> IPropertyDefine.PropertyItemList");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				BusinessObjectList<IBusinessObject> retval = new BusinessObjectList<IBusinessObject>();");
				sb.AppendLine("				foreach (IBusinessObject bo in this." + tableName + "List)");
				sb.AppendLine("					retval.Add(bo);");
				sb.AppendLine("				return retval;");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
			}
		}

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

		#region append methods

		public void AppendInitializeMethod()
		{
			sb.AppendLine("		private void Initialize()");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
		}

		private void AppendMethodGetPropertyItem()
		{
			sb.AppendLine("		private " + _currentTable.PascalName + "PropertyItem GetPropertyItem(" + _currentTable.PascalName + "PropertyItemDefine definition)");
			sb.AppendLine("		{");
			sb.AppendLine("			" + _currentTable.PascalName + "PropertyItem propertyItem = null;");
			sb.AppendLine("			foreach (" + _currentTable.PascalName + "PropertyItem item in this." + _currentTable.PascalName + "PropertyItemList)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (item.PropertyItemDefineId == definition.PropertyItemDefineId)");
			sb.AppendLine("				{");
			sb.AppendLine("					propertyItem = item;");
			sb.AppendLine("					break;");
			sb.AppendLine("				}");
			sb.AppendLine("			}");
			sb.AppendLine("			return propertyItem;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodSetProperty()
		{
			sb.AppendLine("		void IPropertyBag.SetProperty(IPropertyDefine definition, string value)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.SetProperty((" + _currentTable.PascalName + "PropertyItemDefine)definition, value);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public void SetProperty(" + _currentTable.PascalName + "PropertyItemDefine definition, string value)");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				" + _currentTable.PascalName + "PropertyItem propertyItem = GetPropertyItem(definition);");
			sb.AppendLine();
			sb.AppendLine("				//Create a new one if need be");
			sb.AppendLine("				" + _currentTable.PascalName + "PropertyItemCollection propertyItemCollection = null;");
			sb.AppendLine("				if (propertyItem == null)");
			sb.AppendLine("				{");
			sb.AppendLine("					propertyItemCollection = (" + _currentTable.PascalName + "PropertyItemCollection)this.ParentCollection.SubDomain[Collections." + _currentTable.PascalName + "PropertyItemCollection];");
			sb.AppendLine("					propertyItem = (" + _currentTable.PascalName + "PropertyItem)propertyItemCollection.NewItem();");


			//TODO - Make work with composite primary keys
			foreach (Column column in _currentTable.PrimaryKeyColumns)
			{
				sb.AppendLine("					propertyItem." + column.PascalName + " = this." + column.PascalName + ";");
			}
			//sb.AppendLine("					propertyItem." + ((Column)_currentTable.PrimaryKeyColumns[0]).PascalName + " = this." + ((Column)_currentTable.PrimaryKeyColumns[0]).PascalName + ";");

			sb.AppendLine("					propertyItem.PropertyItemDefineId = definition.PropertyItemDefineId;");
			sb.AppendLine("				}");
			sb.AppendLine();
			sb.AppendLine("				//Set the actual value");
			sb.AppendLine("				propertyItem.ItemValue = value;");
			sb.AppendLine();
			sb.AppendLine("				//Add to collection if new");
			sb.AppendLine("				if (!propertyItem.IsParented)");
			sb.AppendLine("					propertyItemCollection.AddItem(propertyItem);");
			sb.AppendLine();
			sb.AppendLine("			}");
			sb.AppendLine("			catch (Exception ex)");
			sb.AppendLine("			{");
			sb.AppendLine("				throw;");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodGetPropertyDefinitions()
		{
			sb.AppendLine("		List<IPropertyDefine> IPropertyBag.GetPropertyDefinitions()");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.ParentCollection.wrappedClass.GetPropertyDefinitions();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public List<" + _currentTable.PascalName + "PropertyItemDefine> GetPropertyDefinitions()");
			sb.AppendLine("		{");
			sb.AppendLine("			List<" + _currentTable.PascalName + "PropertyItemDefine> retval = new List<" + _currentTable.PascalName + "PropertyItemDefine>();");
			sb.AppendLine("			foreach (" + _currentTable.PascalName + "PropertyItemDefine bo in this.ParentCollection.wrappedClass.GetPropertyDefinitions())");
			sb.AppendLine("				retval.Add(bo);");
			sb.AppendLine("			return retval;");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodGetProperty()
		{
			sb.AppendLine("		string IPropertyBag.GetProperty(IPropertyDefine definition)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetProperty((" + _currentTable.PascalName + "PropertyItemDefine)definition);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public string GetProperty(" + _currentTable.PascalName + "PropertyItemDefine definition)");
			sb.AppendLine("		{");
			sb.AppendLine("			" + _currentTable.PascalName + "PropertyItem propertyItem = GetPropertyItem(definition);");
			sb.AppendLine("			if (propertyItem == null)");
			sb.AppendLine("				return null;");
			sb.AppendLine("			else");
			sb.AppendLine("				return propertyItem.ItemValue;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodReleaseNonIdentifyingRelationships()
		{
			if (_currentTable.ParentTable == null)
				sb.Append("		internal virtual void ReleaseNonIdentifyingRelationships()").AppendLine();
			else
				sb.Append("		internal override void ReleaseNonIdentifyingRelationships()").AppendLine();

			sb.Append("		{").AppendLine();
			sb.Append("			try").AppendLine();
			sb.Append("			{").AppendLine();
			sb.Append("				wrappedClass.ReleaseNonIdentifyingRelationships();").AppendLine();
			sb.Append("			}").AppendLine();
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendImageMethods()
		{
			List<Column> imageColumnList = _currentTable.GetColumnsByType(System.Data.SqlDbType.Image);
			if (imageColumnList.Count == 0) return;

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Load the specified image file into the specified image column");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"field\">The image column of this object</param>");
			sb.AppendLine("		/// <param name=\"fileName\">The disk file to load</param>");
			sb.AppendLine("		public void LoadImageFromFile(FieldImageConstants field, string fileName)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (!File.Exists(fileName)) throw new Exception(\"The file does not exist!\");");
			sb.AppendLine("			FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);");
			sb.AppendLine("			byte[] barr = new byte[fs.Length];");
			sb.AppendLine("			fs.Read(barr, 0, barr.Length);");
			sb.AppendLine("			fs.Close();");
			sb.AppendLine();
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Column column in imageColumnList)
			{
				sb.AppendLine("				case FieldImageConstants." + column.PascalName + ":");
				sb.AppendLine("					this." + column.PascalName + " = barr;");
				sb.AppendLine("					break;");
			}
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Save the specified image to file");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"field\">The image column of this object</param>");
			sb.AppendLine("		/// <param name=\"fileName\">The new disk file to save</param>");
			sb.AppendLine("		public void SaveImageToFile(FieldImageConstants field, string fileName)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (File.Exists(fileName)) throw new Exception(\"The file already exists!\");");
			sb.AppendLine("			byte[] barr;");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (Column column in imageColumnList)
			{
				sb.AppendLine("				case FieldImageConstants." + column.PascalName + ":");
				sb.AppendLine("					if (this." + column.PascalName + " == null) throw new Exception(\"The image cannot be null!\");");
				sb.AppendLine("					barr = this." + column.PascalName + ";");
				sb.AppendLine("					break;");
			}
			sb.AppendLine("				default:");
			sb.AppendLine("					throw new Exception(\"Unknown field!\");");
			sb.AppendLine("			}");
			sb.AppendLine("			FileStream fs = File.Create(fileName, barr.Length);");
			sb.AppendLine("			fs.Write(barr, 0, barr.Length);");
			sb.AppendLine("			fs.Close();		");
			sb.AppendLine("		}");
			sb.AppendLine();

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

				List<Column> imageColumnList = _currentTable.GetColumnsByType(System.Data.SqlDbType.Image);
				if (imageColumnList.Count != 0)
				{
					sb.AppendLine("		#region FieldNameConstants Enumeration");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// An enumeration of this object's image type fields");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public enum FieldImageConstants");
					sb.AppendLine("		{");
					foreach (Column column in imageColumnList)
					{
						sb.AppendLine("			 /// <summary>");
						sb.AppendLine("			 /// Field mapping for the image column '" + column.PascalName + "' property");
						sb.AppendLine("			 /// </summary>");
						sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the image column '" + column.PascalName + "' property\")]");
						sb.AppendLine("			" + column.PascalName + ",");
					}
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		#endregion");
					sb.AppendLine();
				}

				sb.AppendLine("		#region FieldNameConstants Enumeration");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Enumeration to define each property that maps to a database field for the '" + _currentTable.PascalName + "' table.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + (_currentTable.ParentTable == null ? "" : "new ") + "enum FieldNameConstants");
				sb.AppendLine("		{");
				foreach (Column column in _currentTable.GetColumnsFullHierarchy())
				{
					sb.AppendLine("			 /// <summary>");
					sb.AppendLine("			 /// Field mapping for the '" + column.PascalName + "' property");
					sb.AppendLine("			 /// </summary>");
					sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
					sb.AppendLine("			" + column.PascalName + ",");
				}

				if (_currentTable.AllowCreateAudit)
				{
					sb.AppendLine("			 /// <summary>");
					sb.AppendLine("			 /// Field mapping for the '" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "' property");
					sb.AppendLine("			 /// </summary>");
					sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "' property\")]");
					sb.AppendLine("			" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + ",");
					sb.AppendLine("			 /// <summary>");
					sb.AppendLine("			 /// Field mapping for the '" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "' property");
					sb.AppendLine("			 /// </summary>");
					sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "' property\")]");
					sb.AppendLine("			" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + ",");
				}

				if (_currentTable.AllowModifiedAudit)
				{
					sb.AppendLine("			 /// <summary>");
					sb.AppendLine("			 /// Field mapping for the '" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "' property");
					sb.AppendLine("			 /// </summary>");
					sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "' property\")]");
					sb.AppendLine("			" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + ",");
					sb.AppendLine("			 /// <summary>");
					sb.AppendLine("			 /// Field mapping for the '" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "' property");
					sb.AppendLine("			 /// </summary>");
					sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "' property\")]");
					sb.AppendLine("			" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + ",");
				}

				sb.AppendLine("		}");
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				if (_currentTable.ParentTable == null)
					sb.AppendLine("		internal Domain" + _currentTable.PascalName + " wrappedClass;");
				else
					sb.AppendLine("		internal new Domain" + _currentTable.PascalName + " wrappedClass;");

				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				//Constructors
				sb.AppendLine("		#region Constructors");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Creates a '" + _currentTable.PascalName + "' object from a domain object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		internal " + _currentTable.PascalName + "(Domain" + _currentTable.PascalName + " classToWrap)");
				if (_currentTable.ParentTable != null) sb.AppendLine("			:base(classToWrap)");
				sb.AppendLine("		{");
				sb.AppendLine("			wrappedClass = classToWrap;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Creates a '" + _currentTable.PascalName + "' object from a DataRowView");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		internal " + _currentTable.PascalName + "(DataRowView drvToWrap)");
				if (_currentTable.ParentTable != null) sb.AppendLine("			:base(drvToWrap)");
				sb.AppendLine("		{");
				sb.AppendLine("			wrappedClass = (Domain" + _currentTable.PascalName + ")drvToWrap.Row;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				//Events
				sb.AppendLine("		#region Events");
				sb.AppendLine();

				if (_currentTable.ParentTable == null)
				{
					//sb.AppendLine("		/// <summary>");
					//sb.AppendLine("		/// This event is raised when any field is changed.");
					//sb.AppendLine("		/// </summary>");
					//sb.AppendLine("		public override event PropertyChangedEventHandler PropertyChanged;");
					//sb.AppendLine();
				}

				if (_currentTable.ParentTable == null)
				{
					//sb.AppendLine("		/// <summary>");
					//sb.AppendLine("		/// This event is raised when any field is changed.");
					//sb.AppendLine("		/// </summary>");
					//sb.AppendLine("		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)");
					//sb.AppendLine("		{");
					//sb.AppendLine("			if (this.PropertyChanged != null)");
					//sb.AppendLine("				this.PropertyChanged(this, e);");
					//sb.AppendLine("		}");
					//sb.AppendLine();
				}

				if (!_currentTable.Immutable)
				{
					foreach (Reference reference in _currentTable.GeneratedColumns)
					{
						Column dbColumn = (Column)reference.Object;
						bool allowProperty = !_currentTable.PropertyExistsInBase(dbColumn.Name);
						if (allowProperty)
						{
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// This event is raised before the '" + dbColumn.PascalName + "' field is changed providing a chance to cancel the action.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public event EventHandler<BusinessObjectCancelEventArgs<" + dbColumn.GetCodeType(true) + ">> " + dbColumn.PascalName + "Changing;");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// This event is raised when the '" + dbColumn.PascalName + "' field is changed.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public event EventHandler " + dbColumn.PascalName + "Changed;");
							sb.AppendLine();
						}
					}

					foreach (Reference reference in _currentTable.GeneratedColumns)
					{
						Column dbColumn = (Column)reference.Object;
						bool allowProperty = !_currentTable.PropertyExistsInBase(dbColumn.Name);
						if (allowProperty)
						{
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Raises the " + dbColumn.PascalName + "Changing event.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		protected virtual void On" + dbColumn.PascalName + "Changing(BusinessObjectCancelEventArgs<" + dbColumn.GetCodeType(true) + "> e)");
							sb.AppendLine("		{");
							sb.AppendLine("			if (this." + dbColumn.PascalName + "Changing != null)");
							sb.AppendLine("				this." + dbColumn.PascalName + "Changing(this, e);");
							sb.AppendLine("		}");
							sb.AppendLine();
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
					}
				}

				sb.AppendLine("		#endregion");
				sb.AppendLine();


				//Properties
				sb.AppendLine("		#region Properties");
				sb.AppendLine();

				//Append all TableComposites
				foreach (TableComposite tableComposite in _currentTable.CompositeList)
				{
					sb.AppendLine("		protected " + _currentTable.PascalName + tableComposite.PascalName + "Composite _" + tableComposite.PascalName + "Composite = null;");
					sb.AppendLine("		public " + _currentTable.PascalName + tableComposite.PascalName + "Composite " + tableComposite.PascalName + "Item");
					sb.AppendLine("		{");
					sb.AppendLine("			get");
					sb.AppendLine("			{");
					sb.AppendLine("				if (_" + tableComposite.PascalName + "Composite == null)");
					sb.AppendLine("					_" + tableComposite.PascalName + "Composite = new " + _currentTable.PascalName + tableComposite.PascalName + "Composite(this);");
					sb.AppendLine("				return _" + tableComposite.PascalName + "Composite;");
					sb.AppendLine("			}");
					sb.AppendLine("		}");
					sb.AppendLine();
				}

				//This need only be added once per hierarchy
				if (_currentTable.ParentTable == null)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The current state of this object.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public Widgetsphere.Core.DataAccess.SubDomainBase.ItemStateConstants ItemState");
					sb.AppendLine("		{");
					sb.AppendLine("			get { return (Widgetsphere.Core.DataAccess.SubDomainBase.ItemStateConstants)int.Parse(wrappedClass.RowState.ToString(\"d\")); }");
					sb.AppendLine("		}");
					sb.AppendLine();
				}

				this.AppendPropertyContainer();

				#region Create/Modified properties

				//If there is a parent table then it will have this stuff in there already
				if (_currentTable.ParentTable == null)
				{
					#region Audits
					if (_currentTable.IsAuditable)
					{
						sb.AppendLine("		#region CreatedBy / ModifiedBy Section");
						sb.AppendLine();
						if (_currentTable.AllowModifiedAudit)
						{
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// The audit field for the 'Modified By' column.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
							sb.AppendLine("		public virtual string " + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName));
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				object o = this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "Column];");
							sb.AppendLine("				if (o == DBNull.Value) return null;");
							sb.AppendLine("				else return (string)o;");
							sb.AppendLine("			}");
							sb.AppendLine("			internal set { this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "Column] = value; }");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// The audit field for the 'Modified Date' column.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
							sb.AppendLine("		public virtual DateTime? " + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName));
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				object o = this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "Column];");
							sb.AppendLine("				if (o == DBNull.Value) return null;");
							sb.AppendLine("				else return (DateTime)o;");
							sb.AppendLine("			}");
							sb.AppendLine("			internal set { this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "Column] = value; }");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Sets 'ModifiedBy' column.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public virtual void Set" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "(DateTime newValue)");
							sb.AppendLine("		{");
							sb.AppendLine("			this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "Column] = newValue;");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Determines if the 'Modified By' column is null.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[Obsolete(\"Use the 'null' to set/check the corresponding property.\", false)]");
							sb.AppendLine("		internal virtual bool Is" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "Null()");
							sb.AppendLine("		{");
							sb.AppendLine("			object o = this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "Column];");
							sb.AppendLine("			return (o == DBNull.Value);");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Determines if the 'Modified Date' column is null.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[Obsolete(\"Use the 'null' to set/check the corresponding property.\", false)]");
							sb.AppendLine("		internal virtual bool Is" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "Null()");
							sb.AppendLine("		{");
							sb.AppendLine("			object o = this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "Column];");
							sb.AppendLine("			return (o == DBNull.Value);");
							sb.AppendLine("		}");
							sb.AppendLine();
						}

						if (_currentTable.AllowCreateAudit)
						{
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// The audit field for the 'Created By' column.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
							sb.AppendLine("		public virtual string " + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName));
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				object o = this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "Column];");
							sb.AppendLine("				if (o == DBNull.Value) return null;");
							sb.AppendLine("				else return (string)o;");
							sb.AppendLine("			}");
							sb.AppendLine("			internal set { this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "Column] = value; }");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// The audit field for the 'Created Date' column.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
							sb.AppendLine("		public virtual DateTime? " + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName));
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				object o = this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "Column];");
							sb.AppendLine("				if (o == DBNull.Value) return null;");
							sb.AppendLine("				else return (DateTime)o;");
							sb.AppendLine("			}");
							sb.AppendLine("			internal set { this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "Column] = value; }");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Sets 'ModifiedBy' column.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		public virtual void Set" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "(DateTime newValue)");
							sb.AppendLine("		{");
							sb.AppendLine("			this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "Column] = newValue;");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Determines if the 'Created By' column is null.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[Obsolete(\"Use the 'null' to set/check the corresponding property.\", false)]");
							sb.AppendLine("		internal virtual bool Is" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "Null()");
							sb.AppendLine("		{");
							sb.AppendLine("			object o = this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "Column];");
							sb.AppendLine("			return (o == DBNull.Value);");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Determines if the 'Created Date' column is null.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[Obsolete(\"Use the 'null' to set/check the corresponding property.\", false)]");
							sb.AppendLine("		internal virtual bool Is" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "Null()");
							sb.AppendLine("		{");
							sb.AppendLine("			object o = this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "Column];");
							sb.AppendLine("			return (o == DBNull.Value);");
							sb.AppendLine("		}");
							sb.AppendLine();
						}

						if (_currentTable.AllowTimestamp)
						{
							sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
							sb.AppendLine("		internal virtual byte[] " + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName));
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				 object o = this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "Column];");
							sb.AppendLine("				 return (byte[])o;");
							sb.AppendLine("			}");
							sb.AppendLine("			set { this.wrappedClass[this.wrappedClass.ParentCol." + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "Column] = value; }");
							sb.AppendLine("		}");
							sb.AppendLine();
						}

						sb.AppendLine("		#endregion");
						sb.AppendLine();
					}

					#endregion
				} //ParentTable exists

				#endregion

				#region IsParented
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Determines if this object is part of a collection or is detached");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[System.ComponentModel.Browsable(false)]");

				if (_currentTable.ParentTable == null)
					sb.AppendLine("		public virtual bool IsParented");
				else
					sb.AppendLine("		public override bool IsParented");

				sb.AppendLine("		{");
				sb.AppendLine("		  get { return (((DataRow)this.WrappedClass).RowState != DataRowState.Detached); }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Returns the internal object that this object wraps");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[System.ComponentModel.Browsable(false)]");

				if (_currentTable.ParentTable == null)
					sb.AppendLine("		internal virtual object WrappedClass");
				else
					sb.AppendLine("		internal override object WrappedClass");

				sb.AppendLine("		{");
				sb.AppendLine("			get{ return wrappedClass; }");
				sb.AppendLine("			set{ wrappedClass = (Domain" + _currentTable.PascalName + ")value; }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		//WrappingClass Interface");
				sb.AppendLine("		object IWrappingClass.WrappedClass");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this.WrappedClass;}");
				sb.AppendLine("			set { this.WrappedClass = value;}");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The primary key for this object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		[System.ComponentModel.Browsable(false)]");

				if (_currentTable.ParentTable == null)
					sb.AppendLine("		public virtual " + _currentTable.PascalName + "PrimaryKey PrimaryKey");
				else
					sb.AppendLine("		public new " + _currentTable.PascalName + "PrimaryKey PrimaryKey");

				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				try");
				sb.AppendLine("				{");
				sb.AppendLine("					return wrappedClass.PrimaryKey;");
				sb.AppendLine("				}");
				Globals.AppendBusinessEntryCatch(sb);
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
				#endregion

				foreach (Reference reference in _currentTable.GeneratedColumns)
				{
					Column column = (Column)reference.Object;
					bool allowProperty = !_currentTable.PropertyExistsInBase(column.Name);
					if (allowProperty)
					{
						sb.AppendLine("		/// <summary>");
						if (column.Description != "") sb.AppendLine("		/// " + column.Description);
						else sb.AppendLine("		/// Property for table field: " + column.DatabaseName);
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		[System.ComponentModel.Browsable(" + column.GridVisible.ToString().ToLower() + ")]");
						sb.AppendLine("		[System.ComponentModel.Description(\"" + column.Description + "\")]");
						sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(\"" + column.FriendlyName + "\", " + column.GridVisible.ToString().ToLower() + ", " + column.SortOrder.ToString() + ")]");
						if (column.GridVisible && column.FriendlyName != "")
							sb.AppendLine("		[System.ComponentModel.DisplayName(\"" + column.FriendlyName + "\")]");

						List<TableComposite> tableCompositeList = _currentTable.GetTableCompositesFullHierarchy(true);
						List<Column> fullCompositeColumnList = new List<Column>();
						foreach (TableComposite tableComposite in tableCompositeList)
						{
							fullCompositeColumnList.AddRange(_currentTable.CompositeList.GetAllColumns(true));
						}

						//If this column is part of a composite then hide it
						var q = from x in fullCompositeColumnList
										where x == column
										select x;

						if (q.FirstOrDefault() != null)
							sb.AppendLine("		protected internal virtual " + column.GetCodeType() + " " + EnsureValidPropertyName(column.PascalName));
						else
							sb.AppendLine("		public virtual " + column.GetCodeType() + " " + EnsureValidPropertyName(column.PascalName));

						sb.AppendLine("		{");
						sb.AppendLine("			get");
						sb.AppendLine("			{");
						sb.AppendLine("				try");
						sb.AppendLine("				{");
						sb.AppendLine("					return wrappedClass." + column.PascalName + ";");
						sb.AppendLine("				}");
						Globals.AppendBusinessEntryCatch(sb);
						sb.AppendLine("			}");
						if (!column.PrimaryKey && !_currentTable.Immutable)
						{
							sb.AppendLine("			set");
							sb.AppendLine("			{");

							//Error Check for NULL
							//if (!column.AllowNull)
							//{
							//  sb.AppendLine("				if (value == null) throw new Exception(\"The data cannot be null for the " + _currentTable.PascalName + "." + column.PascalName + " field.\");");
							//}

							//Error Check for field size
							if (ModelHelper.IsTextType(column.DataType))
							{
								sb.AppendLine("				if ((value != null) && (value.Length > GetMaxLength(FieldNameConstants." + column.PascalName + "))) throw new Exception(\"The data '\" + value + \"' is too large for the " + _currentTable.PascalName + "." + column.PascalName + " field which has a length of \" + GetMaxLength(FieldNameConstants." + column.PascalName + ") + \".\");");
							}

							sb.AppendLine("				try");
							sb.AppendLine("				{");

							//Enforce the Min/Max values
							if (ModelHelper.IsNumericType(column.DataType))
							{
								if ((column.Min >= double.MinValue) && (column.Max >= double.MinValue))
								{
									sb.AppendLine("					if ((value < " + column.Min + ") || (" + column.Max + " < value))");
									sb.AppendLine("						throw new Exception(\"The specified value is outside defined bounds [" + column.Min + ".." + column.Max + "]\");");
								}
								else if (column.Min >= double.MinValue)
								{
									sb.AppendLine("					if (value < " + column.Min + ")");
									sb.AppendLine("						throw new Exception(\"The specified value is less than the minimum bounds [" + column.Min + "]\");");
								}
								else if (column.Min >= double.MinValue)
								{
									sb.AppendLine("					if (" + column.Max + " < value)");
									sb.AppendLine("						throw new Exception(\"The specified value is less than the maximum bounds [" + column.Max + "]\");");
								}
								sb.AppendLine();
							}

							sb.AppendLine("					BusinessObjectCancelEventArgs<" + column.GetCodeType(true) + "> cancelArgs = new BusinessObjectCancelEventArgs<" + column.GetCodeType(true) + ">(value);");
							sb.AppendLine("					this.On" + column.PascalName + "Changing(cancelArgs);");
							sb.AppendLine("					if (!cancelArgs.Cancel)");
							sb.AppendLine("					{");
							sb.AppendLine("						lock (this)");
							sb.AppendLine("						{");
							sb.AppendLine("							wrappedClass." + column.PascalName + " = cancelArgs.NewValue;");
							sb.AppendLine("						}");

							//If there is a modify audit then set it
							if (_currentTable.AllowModifiedAudit)
								sb.AppendLine("						this." + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + " = " + Globals.GetDateTimeNowCode(_model) + ";");

							sb.AppendLine("						this.OnPropertyChanged(new PropertyChangedEventArgs(\"" + column.PascalName + "\"));");
							sb.AppendLine("						this.On" + column.PascalName + "Changed(new EventArgs());");
							sb.AppendLine("					}");
							sb.AppendLine("				}");
							Globals.AppendBusinessEntryCatch(sb);
							sb.AppendLine("			}");
							sb.AppendLine();
						}
						sb.AppendLine("		}");
						sb.AppendLine();
					}
				}

				this.AppendMethodReleaseNonIdentifyingRelationships();
				//foreach (Relation relation in _currentTable.ParentRoleRelations.Where(x => x.IsGenerated))
				foreach (Relation relation in _currentTable.GetParentRoleRelationsFullHierarchy().Where(x => x.IsGenerated))
				{					
					Table relatedtable = (Table)relation.ChildTableRef.Object;
					if (relatedtable.Generated && (relatedtable != _currentTable) && !_currentTable.ShareAncestor(relatedtable))
					{
						//If not Associative and not a child table of the current table and the related table is not a descendant
						if (!relatedtable.AssociativeTable && 
							!_currentTable.GetTablesInheritedFromHierarchy().Contains(relatedtable) &&
							!_currentTable.IsInheritedFrom(relatedtable) &&							
							relation.IsOneToOne)
						{
							Table parentTable = (Table)relation.ParentTableRef.Object;
							string scope = (parentTable == _currentTable ? "virtual" : "override");
							string propertyName = relation.PascalRoleName + relatedtable.PascalName + "Item";

							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Returns the related '" + relatedtable.PascalName + "' object");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
							sb.AppendLine("		public " + scope + " " + relatedtable.PascalName + " " + propertyName);
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				try");
							sb.AppendLine("				{");
							sb.AppendLine("					if (wrappedClass." + relation.PascalRoleName + relatedtable.PascalName + "List.Count > 0) return wrappedClass." + relation.PascalRoleName + relatedtable.PascalName + "List[0];");
							sb.AppendLine("					else return null;");
							sb.AppendLine("				}");
							Globals.AppendBusinessEntryCatch(sb);
							sb.AppendLine("			}");
							sb.AppendLine("			set");
							sb.AppendLine("			{");
							sb.AppendLine("				try");
							sb.AppendLine("				{");
							sb.AppendLine("					wrappedClass." + relation.PascalRoleName + relatedtable.PascalName + "List.Clear();");
							sb.AppendLine("					if (value != null) wrappedClass." + relation.PascalRoleName + relatedtable.PascalName + "List.Add(value);");
							sb.AppendLine("				}");
							Globals.AppendBusinessEntryCatch(sb);
							sb.AppendLine("			}");
							sb.AppendLine("		}");
							sb.AppendLine();
						}
						//If NOT Associative and NOT a child table of the current table
						else if (!relatedtable.AssociativeTable &&
							!_currentTable.IsInheritedFrom(relatedtable) &&							
							!_currentTable.GetTablesInheritedFromHierarchy().Contains(relatedtable))
						{
							Table parentTable = (Table)relation.ParentTableRef.Object;
							string scope = (parentTable == _currentTable ? "virtual" : "override");
							string propertyName = relation.PascalRoleName + "" + relatedtable.PascalName + "List";

							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// A list of related '" + relatedtable.PascalName + "' items");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
							sb.AppendLine("		public " + scope + " BusinessObjectList<" + relatedtable.PascalName + "> " + propertyName);
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				try");
							sb.AppendLine("				{");
							sb.AppendLine("					if (this.ParentCollection == null)");
							sb.AppendLine("					{");
							sb.AppendLine("						lock (this)");
							sb.AppendLine("						{");
							sb.AppendLine("							return wrappedClass." + relation.PascalRoleName + relatedtable.PascalName + "List;");
							sb.AppendLine("						}");
							sb.AppendLine("					}");
							sb.AppendLine("					else");
							sb.AppendLine("					{");
							sb.AppendLine("						lock (this.ParentCollection.SubDomain)");
							sb.AppendLine("						{");
							sb.AppendLine("							return wrappedClass." + relation.PascalRoleName + relatedtable.PascalName + "List;");
							sb.AppendLine("						}");
							sb.AppendLine("					}");
							sb.AppendLine("				}");
							Globals.AppendBusinessEntryCatch(sb);
							sb.AppendLine("			}");
							sb.AppendLine("		}");
							sb.AppendLine();
						}
						else if (relatedtable.AssociativeTable)
						{
							Table parentTable = _currentTable;
							Table childTable = _currentTable;
							foreach (Relation childrelation in relatedtable.ChildRoleRelations)
							{
								Table childRelationParentTable = (Table)childrelation.ParentTableRef.Object;
								if ((childRelationParentTable != _currentTable) && 
									(!_currentTable.IsInheritedFrom(childRelationParentTable)))
								{
									childTable = childRelationParentTable;
								}
							}

							//Table parentTable = (Table)relation.ParentTableRef.Object;
							//Table childTable = (Table)relation.ChildTableRef.Object;
							string scope = ((Table)relation.ParentTableRef.Object == _currentTable ? "virtual" : "override");
							string propertyName = relation.PascalRoleName + "" + childTable.PascalName + "List";

							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// A list of related '" + relatedtable.PascalName + "' items");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
							sb.AppendLine("		public " + scope + " BusinessObjectList<" + childTable.PascalName + "> " + propertyName);
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				try");
							sb.AppendLine("				{");
							sb.AppendLine("					if (this.ParentCollection == null)");
							sb.AppendLine("					{");
							sb.AppendLine("						lock (this)");
							sb.AppendLine("						{");
							sb.AppendLine("							return wrappedClass." + relation.PascalRoleName + "" + childTable.PascalName + "List;");
							sb.AppendLine("						}");
							sb.AppendLine("					}");
							sb.AppendLine("					else");
							sb.AppendLine("					{");
							sb.AppendLine("						lock (this.ParentCollection.SubDomain)");
							sb.AppendLine("						{");
							sb.AppendLine("							return wrappedClass." + relation.PascalRoleName + "" + childTable.PascalName + "List;");
							sb.AppendLine("						}");
							sb.AppendLine("					}");
							sb.AppendLine("				}");
							Globals.AppendBusinessEntryCatch(sb);
							sb.AppendLine("			}");
							sb.AppendLine("		}");
							sb.AppendLine();
						}
					}
				}

				foreach (Relation relation in _currentTable.ChildRoleRelations.Where(x => x.IsGenerated))
				{
					Table dependentTable = ((Table)relation.ParentTableRef.Object);
					if (dependentTable.Generated && (dependentTable != _currentTable) && !_currentTable.GetTableHierarchy().Contains(dependentTable))
					{
						if (dependentTable.IsTypeTable && relation.ColumnRelationships.Count == 1)
						{
							//The enumeration
							Column column = (Column)relation.ColumnRelationships[0].ChildColumnRef.Object;
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Gets or sets the related '" + dependentTable.PascalName + "' object's enum value.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
							sb.AppendLine("		public virtual " + dependentTable.PascalName + "Constants " + EnsureValidPropertyName(relation.PascalRoleName + dependentTable.PascalName));
							sb.AppendLine("		{");
							sb.AppendLine("			get { return (" + dependentTable.PascalName + "Constants)this." + column.PascalName + "; }");
							if (!column.PrimaryKey)
							{
								sb.AppendLine("			set { this." + column.PascalName + " = (int)value; }");
							}
							sb.AppendLine("		}");
							sb.AppendLine();
						}

						//The actual object
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// Gets or sets the related '" + dependentTable.PascalName + "' object.");
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
						sb.AppendLine("		public virtual " + dependentTable.PascalName + " " + EnsureValidPropertyName(relation.PascalRoleName + dependentTable.PascalName + "Item"));
						sb.AppendLine("		{");
						sb.AppendLine("			get");
						sb.AppendLine("			{");
						sb.AppendLine("				try");
						sb.AppendLine("				{");
						sb.AppendLine("					if (wrappedClass." + relation.PascalRoleName + "" + dependentTable.PascalName + "Item != null) return new " + dependentTable.PascalName + "(wrappedClass." + relation.PascalRoleName + "" + dependentTable.PascalName + "Item);");
						sb.AppendLine("					else return null;");
						sb.AppendLine("				}");
						Globals.AppendBusinessEntryCatch(sb);
						sb.AppendLine("			}");
						sb.AppendLine("			set");
						sb.AppendLine("			{");
						sb.AppendLine("				try");
						sb.AppendLine("				{");
						sb.AppendLine("					wrappedClass." + relation.PascalRoleName + "" + dependentTable.PascalName + "Item = value.wrappedClass;");
						sb.AppendLine("				}");
						Globals.AppendBusinessEntryCatch(sb);
						sb.AppendLine("			}");
						sb.AppendLine("		}");

					}
				}
				sb.AppendLine();
				sb.AppendLine("		#endregion");

				if (_currentTable.SelfReference)
				{
					sb.AppendLine();
					sb.AppendLine("		#region Self Reference");
					sb.AppendLine();
					sb.AppendLine("		public virtual BusinessObjectList<" + _currentTable.PascalName + "> Child" + _currentTable.SelfReferenceParentColumn.PascalName + "" + _currentTable.PascalName + "List");
					sb.AppendLine("		{");
					sb.AppendLine("			get");
					sb.AppendLine("			{");
					sb.AppendLine("				try");
					sb.AppendLine("				{");
					sb.AppendLine("					return wrappedClass.Child" + _currentTable.SelfReferenceParentColumn.PascalName + "" + _currentTable.PascalName + "List;");
					sb.AppendLine("				}");
					Globals.AppendBusinessEntryCatch(sb);
					sb.AppendLine("			}");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		public virtual " + _currentTable.PascalName + " Parent" + _currentTable.SelfReferenceParentColumn.PascalName + "" + _currentTable.PascalName + "Item");
					sb.AppendLine("		{");
					sb.AppendLine("			get");
					sb.AppendLine("			{");
					sb.AppendLine("				try");
					sb.AppendLine("				{");
					sb.AppendLine("					if (wrappedClass.Parent" + _currentTable.SelfReferenceParentColumn.PascalName + "" + _currentTable.PascalName + "Item != null)");
					sb.AppendLine("					{");
					sb.AppendLine("						return new " + _currentTable.PascalName + "(wrappedClass.Parent" + _currentTable.SelfReferenceParentColumn.PascalName + "" + _currentTable.PascalName + "Item);");
					sb.AppendLine("					}");
					sb.AppendLine("					else");
					sb.AppendLine("					{");
					sb.AppendLine("						return null;");
					sb.AppendLine("					}");
					sb.AppendLine("				}");
					Globals.AppendBusinessEntryCatch(sb);
					sb.AppendLine("			}");
					sb.AppendLine("			set");
					sb.AppendLine("			{");
					sb.AppendLine("				try");
					sb.AppendLine("				{");
					sb.AppendLine("					wrappedClass.Parent" + _currentTable.SelfReferenceParentColumn.PascalName + "" + _currentTable.PascalName + "Item = value.wrappedClass;");
					sb.AppendLine("				}");
					Globals.AppendBusinessEntryCatch(sb);
					sb.AppendLine("			}");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		#endregion");

				}
				sb.AppendLine();

				//Get all NULL columns
				List<Column> nullColumns = new List<Column>();
				foreach (Reference reference in _currentTable.GeneratedColumns)
				{
					Column dbColumn = (Column)reference.Object;
					if (dbColumn.AllowNull)
					{
						nullColumns.Add(dbColumn);
					}
				}

				//Write all NULL property accessor methods
				if (nullColumns.Count > 0)
				{
					sb.AppendLine("		#region Null Methods");
					foreach (Column dbColumn in nullColumns)
					{
						if (dbColumn.AllowNull)
						{
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Determines if the field '" + dbColumn.PascalName + "' is null");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		/// <returns>Boolean value indicating if the specified value is null</returns>");
							sb.AppendLine("		[Obsolete(\"Use the 'null' to set/check the corresponding property.\", false)]");
							sb.AppendLine("		internal bool Is" + dbColumn.PascalName + "Null() ");
							sb.AppendLine("		{");
							sb.AppendLine("			try");
							sb.AppendLine("			{");
							sb.AppendLine("				return wrappedClass.Is" + dbColumn.PascalName + "Null();");
							sb.AppendLine("			}");
							Globals.AppendBusinessEntryCatch(sb);
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// Sets the field '" + dbColumn.PascalName + "' to null");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[Obsolete(\"Use the 'null' to set/check the corresponding property.\", false)]");
							sb.AppendLine("		internal void Set" + dbColumn.PascalName + "Null() ");
							sb.AppendLine("		{");
							sb.AppendLine("			try");
							sb.AppendLine("			{");
							sb.AppendLine("				wrappedClass.Set" + dbColumn.PascalName + "Null();");
							sb.AppendLine("			}");
							Globals.AppendBusinessEntryCatch(sb);
							sb.AppendLine("		}");
							sb.AppendLine();
						}
					}
					sb.AppendLine("		#endregion");
					sb.AppendLine();
				}

				sb.AppendLine("		#region Collection Operation Methods");
				sb.AppendLine();

				//Only need to add this once per hierarchy
				if (_currentTable.ParentTable == null)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// This event is called before persisting to the database.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public event EventHandler<BusinessObjectEventArgs> Validate;");
					sb.AppendLine();

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// This event is called before persisting to store.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		/// <param name=\"item\"></param>");
					if (_currentTable.ParentTable == null)
						sb.AppendLine("		protected internal void OnValidate(BusinessObjectEventArgs item)");
					else
						sb.AppendLine("		protected internal new void OnValidate(BusinessObjectEventArgs item)");
					sb.AppendLine("		{");
					sb.AppendLine("			if (this.Validate != null)");
					sb.AppendLine("				this.Validate(this, item);");
					sb.AppendLine("		}");
					sb.AppendLine();

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Persists this object to the database.");
					sb.AppendLine("		/// </summary>");

					bool allowOverride = true;
					if (_currentTable.AnyBaseNonModifiable)
						allowOverride = false;

					sb.AppendLine("		public " + (allowOverride ? "override" : "") + " void Persist()");
					sb.AppendLine("		{");
					sb.AppendLine("		  if (this.IsParented)");
					sb.AppendLine("		  {");
					sb.AppendLine("			this.OnValidate(new BusinessObjectEventArgs(this));");
					sb.AppendLine("			  wrappedClass.Persist();");
					sb.AppendLine("		  }");
					sb.AppendLine("		  else");
					sb.AppendLine("		    throw new Exception(\"This item is not part of a collection and cannot be persisted!\");");
					sb.AppendLine("		}");
					sb.AppendLine();

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Removes this object from its parent collection. It will not be deleted from the database.");
					sb.AppendLine("		/// </summary>");

					if (_currentTable.ParentTable == null)
						sb.AppendLine("		public void Remove()");
					else
						sb.AppendLine("		public new void Remove()");

					sb.AppendLine("		{");
					sb.AppendLine("			try");
					sb.AppendLine("			{");
					sb.AppendLine("				if (this.ParentCollection == null)");
					sb.AppendLine("				{");
					sb.AppendLine("					lock (this)");
					sb.AppendLine("					{");
					sb.AppendLine("						wrappedClass.Remove();");
					sb.AppendLine("					}");
					sb.AppendLine("				}");
					sb.AppendLine("				else");
					sb.AppendLine("				{");
					sb.AppendLine("					lock (this.ParentCollection.SubDomain)");
					sb.AppendLine("					{");
					sb.AppendLine("						wrappedClass.Remove();");
					sb.AppendLine("					}");
					sb.AppendLine("				}");
					sb.AppendLine("			}");
					Globals.AppendBusinessEntryCatch(sb);
					sb.AppendLine("		}");
					sb.AppendLine();
				}

				if (!_currentTable.Immutable)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Marks this object for deletion. The next call to the Persist method will delete it from the database.");
					sb.AppendLine("		/// </summary>");
					if ((_currentTable.ParentTable == null) || (_currentTable.NeedOverridePersistable))
						sb.AppendLine("		public void Delete()");
					else
						sb.AppendLine("		public new void Delete()");
					sb.AppendLine("		{");
					sb.AppendLine("			try");
					sb.AppendLine("			{");
					sb.AppendLine("				if (this.ParentCollection == null)");
					sb.AppendLine("				{");
					sb.AppendLine("					lock (this)");
					sb.AppendLine("					{");
					sb.AppendLine("						wrappedClass.Delete();");
					sb.AppendLine("					}");
					sb.AppendLine("				}");
					sb.AppendLine("				else");
					sb.AppendLine("				{");
					sb.AppendLine("					lock (this.ParentCollection.SubDomain)");
					sb.AppendLine("					{");
					sb.AppendLine("						wrappedClass.Delete();");
					sb.AppendLine("					}");
					sb.AppendLine("				}");
					sb.AppendLine("			}");
					Globals.AppendBusinessEntryCatch(sb);
					sb.AppendLine("		}");
					sb.AppendLine();
				}
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Overrides");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Serves as a hash function for this particular type.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override int GetHashCode()");
				sb.AppendLine("		{");
				sb.AppendLine("			return base.GetHashCode();");
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
				sb.AppendLine("				if (this.wrappedClass.Equals(((" + _currentTable.PascalName + ")obj).wrappedClass))");
				sb.AppendLine("					return true;");
				sb.AppendLine("			}");
				sb.AppendLine("			else if(obj.GetType() == this.PrimaryKey.GetType())");
				sb.AppendLine("			{");
				sb.AppendLine("			  if(this.PrimaryKey.Equals(obj))");
				sb.AppendLine("			    return true;");
				sb.AppendLine("			}");
				Column pkColumn = (Column)_currentTable.PrimaryKeyColumns[0];
				sb.AppendLine("			else if (obj.GetType() == this.PrimaryKey." + pkColumn.PascalName + ".GetType())");
				sb.AppendLine("			{");
				sb.AppendLine("				if (this.PrimaryKey." + pkColumn.PascalName + ".Equals(obj))");
				sb.AppendLine("					return true;");
				sb.AppendLine("			}");
				sb.AppendLine("			return false;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				this.AppendRegionParentCollection();
				this.AppendRegionGetDefault();
				this.AppendRegionGetValue();
				this.AppendRegionSetValue();
				this.AppendRegionGetFieldLength();

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendRegionParentCollection()
		{
			sb.AppendLine("		#region IBusinessObject ParentCollection");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// This is a reference back to the generic collection that holds this object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
			sb.AppendLine("		IBusinessCollection IBusinessObject.ParentCollection");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.ParentCollection; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();

			//ParentCollection
			sb.AppendLine("		#region ParentCollection");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// This is a reference back to the collection that holds this object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		[System.ComponentModel.Browsable(false)]");

			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public " + _currentTable.PascalName + "Collection ParentCollection");
			else
				sb.AppendLine("		public new " + _currentTable.PascalName + "Collection ParentCollection");

			sb.AppendLine("		{");
			sb.AppendLine("			get { return new " + _currentTable.PascalName + "Collection(wrappedClass.ParentCol); }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionGetFieldLength()
		{
			try
			{
				sb.AppendLine("		#region GetFieldLength");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Determines the length of the specified field if it can be set.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public int GetFieldLength(" + _currentTable.PascalName + ".FieldNameConstants field)");
				sb.AppendLine("		{");

				ColumnCollection allColumns = _currentTable.GetColumnsFullHierarchy(true);
				foreach (Column column in allColumns)
				{
					if (column.Generated)
					{
						if (ModelHelper.IsTextType(column.DataType))
						{
							sb.AppendLine("			if(field == FieldNameConstants." + column.PascalName + ")");
							sb.AppendLine("				return " + column.Length + ";");
						}
					}
				}
				sb.AppendLine("			//Catch all for all other data types");
				sb.AppendLine("			return -1;");
				sb.AppendLine("		}");
				sb.AppendLine("		#endregion");
				sb.AppendLine();

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendRegionSetValue()
		{
			if (_currentTable.Immutable)
				return;

			sb.AppendLine("		#region SetValue");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Assigns a value to a field on this object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"field\">The field to set</param>");
			sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
			sb.AppendLine("		public void SetValue(FieldNameConstants field, object newValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			SetValue(field, newValue, false);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Assigns a value to a field on this object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"field\">The field to set</param>");
			sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
			sb.AppendLine("		/// <param name=\"fixLength\">Determines if the length should be truncated if too long. When false, an error will be raised if data is too large to be assigned to the field.</param>");
			sb.AppendLine("		public void SetValue(FieldNameConstants field, object newValue, bool fixLength)");
			sb.AppendLine("		{");

			ColumnCollection allColumns = _currentTable.GetColumnsFullHierarchy(true);
			int count = 0;
			foreach (Column column in allColumns)
			{
				if (column.Generated)
				{
					//If not the first one, add an 'ELSE' for speed
					if (count == 0) sb.Append("			");
					else sb.Append("			else ");

					sb.AppendLine("if (field == FieldNameConstants." + column.PascalName + ")");
					sb.AppendLine("			{");
					if (column.PrimaryKey)
					{
						sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' is a primary key and cannot be set!\");");
					}
					else
					{
						if (ModelHelper.IsTextType(column.DataType))
						{
							sb.AppendLine("				if (newValue == null)");
							sb.AppendLine("				{");
							sb.AppendLine("					this." + column.PascalName + " = null;");
							sb.AppendLine("				}");
							sb.AppendLine("				else");
							sb.AppendLine("				{");
							sb.AppendLine("					string v = newValue.ToString();");
							sb.AppendLine("					if (fixLength)");
							sb.AppendLine("					{");
							sb.AppendLine("						int fieldLength = GetFieldLength(FieldNameConstants." + column.PascalName + ");");
							sb.AppendLine("						if (v.Length > fieldLength) v = v.Substring(0, fieldLength);");
							sb.AppendLine("					}");
							sb.AppendLine("					this." + column.PascalName + " = v;");
							sb.AppendLine("				}");
							sb.AppendLine("				return;");
						}
						else
						{
							//All other types
							sb.AppendLine("				if (newValue == null)");
							sb.AppendLine("				{");
							if (column.AllowNull)
								sb.AppendLine("					this." + column.PascalName + " = null;");
							else
								sb.AppendLine("					throw new Exception(\"Field '" + column.PascalName + "' does not allow null values!\");");
							sb.AppendLine("				}");
							sb.AppendLine("				else");
							sb.AppendLine("				{");
							Table relationParentTable = (Table)column.ParentTableRef.Object;
							ReadOnlyCollection<Relation> list = relationParentTable.AllRelationships.FindByChildColumn(column);
							if (list.Count > 0)
							{
								Relation relation = (Relation)list[0];
								Table pTable = ((Table)relation.ParentTableRef.Object);
								if (pTable.Generated)
								{
									Table cTable = ((Table)relation.ChildTableRef.Object);
									string s = pTable.PascalName;
									sb.AppendLine("					if(Widgetsphere.Core.Util.ReflectionHelper.ImplementsInterface(newValue, typeof(IWrappingClass)))");
									sb.AppendLine("					{");
									if (column.EnumType == "")
									{
										ColumnRelationship columnRelationship = relation.ColumnRelationships.GetByParentField(column);
										Column parentColumn = (Column)columnRelationship.ParentColumnRef.Object;
										sb.AppendLine("						this." + EnsureValidPropertyName(column.PascalName) + " = ((" + pTable.PascalName + ")newValue)." + parentColumn.PascalName + ";");
										sb.AppendLine("					} else if(newValue is IPrimaryKey)");
										sb.AppendLine("					{");
										sb.AppendLine("						this." + column.PascalName + " = ((" + pTable.PascalName + "PrimaryKey)newValue)." + parentColumn.PascalName + ";");
									}
									else //This is an Enumeration type
										sb.AppendLine("						throw new Exception(\"Field '" + EnsureValidPropertyName(column.PascalName) + "' does not allow values of this type!\");");

									sb.AppendLine("					} else");
								}

							}
							if (column.AllowStringParse)
							{
								sb.AppendLine("					if (newValue is string)");
								sb.AppendLine("					{");
								if (column.EnumType == "")
								{
									sb.AppendLine("						this." + column.PascalName + " = " + column.GetCodeType(false) + ".Parse((string)newValue);");
									sb.AppendLine("					} else if (!(newValue is " + column.GetCodeType() + ")) {");
									if (column.GetCodeType() == "int")
										sb.AppendLine("						this." + column.PascalName + " = Convert.ToInt32(newValue);");
									else
										sb.AppendLine("						this." + column.PascalName + " = " + column.GetCodeType(false) + ".Parse(newValue.ToString());");
								}
								else //This is an Enumeration type
								{
									sb.AppendLine("						this." + EnsureValidPropertyName(column.PascalName) + " = (" + column.EnumType + ")Enum.Parse(typeof(" + column.EnumType + "), newValue.ToString());");
								}
								sb.AppendLine("					}");
								sb.AppendLine("					else if (newValue is IBusinessObject)");
								sb.AppendLine("					{");
								sb.AppendLine("						throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
								sb.AppendLine("					}");
								sb.AppendLine("					else");
							}

							if (column.GetCodeType() == "string")
								sb.AppendLine("					this." + column.PascalName + " = newValue.ToString();");
							else
								sb.AppendLine("					this." + column.PascalName + " = (" + column.GetCodeType() + ")newValue;");

							sb.AppendLine("				}");
							sb.AppendLine("				return;");
						} //All non-string types

					}
					sb.AppendLine("			}");
					count++;
				}

			}
			sb.AppendLine("			throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionGetValue()
		{
			sb.AppendLine("		#region GetValue");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
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
			ColumnCollection allColumns = _currentTable.GetColumnsFullHierarchy(true);
			foreach (Column column in allColumns.Where(x => x.Generated))
			{
				Table relationParentTable = (Table)column.ParentTableRef.Object;
				ReadOnlyCollection<Relation> childColumnList = relationParentTable.AllRelationships.FindByChildColumn(column);
				//if (childColumnList.Count == 0)
				//{
				sb.AppendLine("			if(field == FieldNameConstants." + column.PascalName + ")");
				sb.AppendLine("			{");
				if (column.AllowNull)
					sb.AppendLine("				return ((this." + EnsureValidPropertyName(column.PascalName) + " == null) ? defaultValue : this." + EnsureValidPropertyName(column.PascalName) + ");");
				else
					sb.AppendLine("				return this." + EnsureValidPropertyName(column.PascalName) + ";");
				sb.AppendLine("			}");
				//}
			}

			////TODO - Handle Composite Primary Keys
			//Relation relation = (Relation)childColumnList[0];
			//Table parentTable = ((Table)relation.ParentTableRef.Object);
			//if (dbColumn.AllowNull)
			//  sb.AppendLine("				return ((this." + EnsureValidPropertyName(dbColumn.PascalName) + " == null) ? defaultValue : new " + parentTable.PascalName + "PrimaryKey(this." + dbColumn.PascalName + "));");
			//else if (dbColumn.EnumType == "")
			//{
			//  Table foreignTable = (Table)dbColumn.ParentTableRef.Object;
			//  sb.Append("        return new " + parentTable.PascalName + "PrimaryKey(");
			//  int pkIndex = 0;
			//  foreach (Column pkColumn in _currentTable.PrimaryKeyColumns)
			//  {
			//    sb.Append("this." + EnsureValidPropertyName(dbColumn.PascalName));
			//    if (pkIndex < _currentTable.PrimaryKeyColumns.Count - 1)
			//      sb.Append(", ");
			//    pkIndex++;
			//  }
			//  sb.AppendLine(");");
			//  //sb.AppendLine("				return new " + parentTable.PascalName + "PrimaryKey(this." + EnsureValidPropertyName(dbColumn.PascalName) + ");");
			//}
			//else if (dbColumn.EnumType != "")
			//  sb.AppendLine("				return new " + parentTable.PascalName + "PrimaryKey((int)this." + EnsureValidPropertyName(dbColumn.PascalName) + ");");


			//Now do the primary keys
			foreach (Column dbColumn in _currentTable.PrimaryKeyColumns)
			{
				//TODO
			}

			sb.AppendLine("			throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
			sb.AppendLine("		}");
			sb.AppendLine();
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

		private void AppendRegionGetDefault()
		{
			sb.AppendLine("		#region GetDefault");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the default value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public object GetDefault(FieldNameConstants field)");
			sb.AppendLine("		{");

			foreach (Reference reference in _currentTable.GeneratedColumns)
			{
				Column dbColumn = (Column)reference.Object;
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
						if (dbColumn.Default.ToLower().StartsWith("newid"))
							sb.AppendLine("				return \"\";");
						else
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
					default:
						sb.AppendLine("				return null;");
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

		#endregion

		#region string helpers

		protected string PrimaryKeyParameterList(bool useType)
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Column dc in _currentTable.PrimaryKeyColumns)
				{
					if (useType) output.Append(dc.GetCodeType() + " ");
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

		protected string EnsureValidPropertyName(string propertyName)
		{
			string appendVal = string.Empty;
			if (StringHelper.Match(propertyName, _currentTable.PascalName, false))
			{
				appendVal = "Member";
			}
			return propertyName + appendVal;
		}

		#endregion

	}
}