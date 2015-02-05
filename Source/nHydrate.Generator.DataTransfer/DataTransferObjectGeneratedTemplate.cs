#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.DataTransfer
{
	public class DataTransferObjectGeneratedTemplate : BaseDataTransferTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();
		private readonly Table _item;

		public DataTransferObjectGeneratedTemplate(ModelRoot model, Table currentTable)
			: base(model)
		{
			_item = currentTable;
		}

		#region BaseClassTemplate overrides

		public override string FileName
		{
			get { return string.Format("{0}.Generated.cs", _item.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}.cs", _item.PascalName); }
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
				nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + this.GetLocalNamespace());
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
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.Linq;");
			//sb.AppendLine("using System.Text;");
			//sb.AppendLine("using System.Xml;");
			//sb.AppendLine("using System.ComponentModel;");
			//sb.AppendLine("using " + this.GetLocalNamespace() + ";");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using System.ComponentModel;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			try
			{
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// Object data transfer definition for the '" + _item.DatabaseName + "' table");
				if (!string.IsNullOrEmpty(_item.Description))
					StringHelper.LineBreakCode(sb, _item.Description, "	/// ");
				sb.AppendLine("	/// </summary>");

				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	[DataContract]");
				if (_item.ParentTable == null)
					sb.AppendLine("	public partial class " + _item.PascalName + " : " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.PascalName + ", " + this.DefaultNamespace + ".EFDAL.Interfaces.IDTO");
				else
					sb.AppendLine("	public partial class " + _item.PascalName + " : " + _item.ParentTable.PascalName + ", " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.ParentTable.PascalName + "");

				sb.AppendLine("	{");

				#region Enumerations

				//sb.AppendLine("		#region Class Members");
				//sb.AppendLine();

				//var imageColumnList = _currentTable.GetColumnsByType(System.Data.SqlDbType.Image);
				//if (imageColumnList.Count() != 0)
				//{
				//  sb.AppendLine("		#region FieldImageConstants Enumeration");
				//  sb.AppendLine();
				//  sb.AppendLine("		/// <summary>");
				//  sb.AppendLine("		/// An enumeration of this object's image type fields");
				//  sb.AppendLine("		/// </summary>");
				//  sb.AppendLine("		public enum FieldImageConstants");
				//  sb.AppendLine("		{");
				//  foreach (var column in imageColumnList.OrderBy(x => x.Name))
				//  {
				//    sb.AppendLine("			 /// <summary>");
				//    sb.AppendLine("			 /// Field mapping for the image column '" + column.PascalName + "' property");
				//    sb.AppendLine("			 /// </summary>");
				//    sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the image column '" + column.PascalName + "' property\")]");
				//    sb.AppendLine("			" + column.PascalName + ",");
				//  }
				//  sb.AppendLine("		}");
				//  sb.AppendLine();
				//  sb.AppendLine("		#endregion");
				//  sb.AppendLine();
				//}

				//sb.AppendLine("		#region FieldNameConstants Enumeration");
				//sb.AppendLine();
				//sb.AppendLine("		/// <summary>");
				//sb.AppendLine("		/// An enumeration of this object's fields");
				//sb.AppendLine("		/// </summary>");
				//sb.AppendLine("		public " + (_currentTable.ParentTable == null ? "" : "new ") + "enum FieldNameConstants");
				//sb.AppendLine("		{");
				//foreach (var column in _currentTable.GeneratedColumnsFullHierarchy)
				//{
				//  sb.AppendLine("			 /// <summary>");
				//  sb.AppendLine("			 /// Field mapping for the '" + column.PascalName + "' property");
				//  sb.AppendLine("			 /// </summary>");
				//  sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
				//  sb.AppendLine("			" + column.PascalName + ",");
				//}

				//if (_currentTable.AllowCreateAudit)
				//{
				//  sb.AppendLine("			 /// <summary>");
				//  sb.AppendLine("			 /// Field mapping for the '" + _model.Database.CreatedByPascalName + "' property");
				//  sb.AppendLine("			 /// </summary>");
				//  sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.CreatedByPascalName + "' property\")]");
				//  sb.AppendLine("			" + _model.Database.CreatedByPascalName + ",");
				//  sb.AppendLine("			 /// <summary>");
				//  sb.AppendLine("			 /// Field mapping for the '" + _model.Database.CreatedDatePascalName + "' property");
				//  sb.AppendLine("			 /// </summary>");
				//  sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.CreatedDatePascalName + "' property\")]");
				//  sb.AppendLine("			" + _model.Database.CreatedDatePascalName + ",");
				//}

				//if (_currentTable.AllowModifiedAudit)
				//{
				//  sb.AppendLine("			 /// <summary>");
				//  sb.AppendLine("			 /// Field mapping for the '" + _model.Database.ModifiedByPascalName + "' property");
				//  sb.AppendLine("			 /// </summary>");
				//  sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.ModifiedByPascalName + "' property\")]");
				//  sb.AppendLine("			" + _model.Database.ModifiedByPascalName + ",");
				//  sb.AppendLine("			 /// <summary>");
				//  sb.AppendLine("			 /// Field mapping for the '" + _model.Database.ModifiedDatePascalName + "' property");
				//  sb.AppendLine("			 /// </summary>");
				//  sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.ModifiedDatePascalName + "' property\")]");
				//  sb.AppendLine("			" + _model.Database.ModifiedDatePascalName + ",");
				//}

				//sb.AppendLine("		}");
				//sb.AppendLine();
				//sb.AppendLine("		#endregion");
				//sb.AppendLine();
				//sb.AppendLine("		#endregion");
				//sb.AppendLine();

				#endregion

				sb.AppendLine("		#region Constructors");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The default contructor for the " + _item.PascalName + " DTO");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _item.PascalName + "()");
				sb.AppendLine("		{");

				//DEFAULT PROPERTIES START
				foreach (var column in _item.GeneratedColumns)
				{
					if ((column.Default != null) && (!string.IsNullOrEmpty(column.Default)))
					{
						var defaultValue = column.GetCodeDefault();

						//Skip SeqID
						if (column.DataType == System.Data.SqlDbType.UniqueIdentifier && column.Default.ToLower() == "newsequentialid")
							defaultValue = "Guid.NewGuid()";

						//Write the actual code
						if (!string.IsNullOrEmpty(defaultValue))
							sb.AppendLine("			this." + column.PascalName + " = " + defaultValue + ";");
					}
				}
				//DEFAULT PROPERTIES END

				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				this.AppendEnum();
				sb.AppendLine("		#region Object Properties");
				sb.AppendLine();

				foreach (var column in _item.GeneratedColumns)
				{
					#region Add new fake property for typed enumerations
					var roleName = string.Empty;
					if (_item.IsColumnRelatedToTypeTable(column, out roleName))
					{
						var typeTable = _item.GetRelatedTypeTableByColumn(column, out roleName);
						if (typeTable != null)
						{
							var nullSuffix = string.Empty;
							if (column.AllowNull)
								nullSuffix = "?";

							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// This property is a wrapper for the typed enumeration for the '" + column.PascalName + "' field.");
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode]");
							sb.AppendLine("		public virtual " + this.InterfaceProjectNamespace + "." + typeTable.PascalName + "Constants" + nullSuffix + " " + roleName + typeTable.PascalName + "Value");
							sb.AppendLine("		{");
							sb.AppendLine("			get { return (" + this.InterfaceProjectNamespace + "." + typeTable.PascalName + "Constants" + nullSuffix + ")this." + column.PascalName + "; }");
							sb.AppendLine("			set { this." + column.PascalName + " = (" + column.GetCodeType(true) + ")value; }");
							sb.AppendLine("		}");
							sb.AppendLine();
						}
					}
					#endregion

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The " + column.PascalName + " field");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[DataMember]");

					if (!string.IsNullOrEmpty(column.Category))
						sb.AppendLine("		[System.ComponentModel.Category(\"" + column.Category + "\")]");

					sb.AppendLine("		[System.ComponentModel.DisplayName(\"" + column.GetFriendlyName() + "\")]");

					if (column.UIDataType != System.ComponentModel.DataAnnotations.DataType.Custom)
					{
						sb.AppendLine("		[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType." + column.UIDataType.ToString() + ")]");
					}

					if (!string.IsNullOrEmpty(column.Mask))
					{
						sb.AppendLine("		[System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = @\"" + column.Mask.Replace(@"\\", @"\\\\") + "\")]");
					}

					if (column.PrimaryKey)
						sb.AppendLine("		[System.ComponentModel.DataAnnotations.Key()]");

					if (column.PrimaryKey || _item.Immutable || column.ComputedColumn || column.IsReadOnly)
						sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");

					if (!string.IsNullOrEmpty(column.Description))
						sb.AppendLine("		[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(column.Description) + "\")]");

					if (_item.IsColumnInherited(column))
						sb.AppendLine("		public override " + column.GetCodeType(true) + " " + column.PascalName + " { get; set; }");
					else
						sb.AppendLine("		public virtual " + column.GetCodeType(true) + " " + column.PascalName + " { get; set; }");

					sb.AppendLine();
				}

				if (_item.AllowCreateAudit)
				{
					var inheritor = (_item.InheritsCreateAudit ? "override" : "virtual");
					var createdFieldName = _model.Database.CreatedDatePascalName;
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The " + createdFieldName + " field");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[DataMember]");
					sb.AppendLine("		public " + inheritor + " DateTime? " + createdFieldName + " { get; set; }");
					sb.AppendLine();

					createdFieldName = _model.Database.CreatedByPascalName;
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The " + createdFieldName + " field");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[DataMember]");
					sb.AppendLine("		public " + inheritor + " string " + createdFieldName + " { get; set; }");
					sb.AppendLine();
				}

				if (_item.AllowModifiedAudit)
				{
					var inheritor = (_item.InheritsModifyAudit ? "override" : "virtual");
					var modifiedFieldName = _model.Database.ModifiedDatePascalName;
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The " + modifiedFieldName + " field");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[DataMember]");
					sb.AppendLine("		public " + inheritor + " DateTime? " + modifiedFieldName + " { get; set; }");
					sb.AppendLine();

					modifiedFieldName = _model.Database.ModifiedByPascalName;
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The " + modifiedFieldName + " field");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[DataMember]");
					sb.AppendLine("		public " + inheritor + " string " + modifiedFieldName + " { get; set; }");
					sb.AppendLine();
				}

				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				sb.AppendLine("		#region Navigation Properties");
				sb.AppendLine();

				//Child relations
				foreach (Relation relation in _item.GetRelations())
				{
					var parentTable = relation.ParentTable;
					var childTable = relation.ChildTable;

					if (parentTable.TypedTable == TypedTableConstants.EnumOnly || childTable.TypedTable == TypedTableConstants.EnumOnly)
					{
						//Do Nothing
					}

					else if (!parentTable.AssociativeTable && !childTable.AssociativeTable && parentTable.Generated && childTable.Generated)
					{
						//This is a 1:1
						if (relation.IsOneToOne)
						{
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// The relation " + parentTable.PascalName + " -> " + childTable.PascalName + (string.IsNullOrEmpty(relation.RoleName) ? string.Empty : " (Role: " + relation.PascalRoleName + ")"));
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[DataMember]");
							sb.AppendLine("		public virtual " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + childTable.PascalName + " " + relation.PascalRoleName + childTable.PascalName + " { get; set; }");
							sb.AppendLine();
						}
						else //This is a 1:N
						{
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// The relation " + parentTable.PascalName + " -> " + childTable.PascalName + (string.IsNullOrEmpty(relation.RoleName) ? string.Empty : " (Role: " + relation.PascalRoleName + ")"));
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[DataMember]");
							sb.AppendLine("		public virtual ICollection<" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List { get; set; }");
							sb.AppendLine();
						}
					}

					else if (relation.IsManyToMany && parentTable.Generated && childTable.Generated)
					{
						var otherTable = relation.GetSecondaryAssociativeTable();

						//This is a M:N
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// The relation " + parentTable.PascalName + " -> " + otherTable.PascalName + (string.IsNullOrEmpty(relation.RoleName) ? string.Empty : " (Role: " + relation.PascalRoleName + ")"));
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		[DataMember]");
						sb.AppendLine("		public virtual ICollection<" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + otherTable.PascalName + "> " + relation.PascalRoleName + otherTable.PascalName + "List { get; set; }");
						sb.AppendLine();
					}

				}

				//Parent relations
				foreach (var relation in _item.GetRelationsWhereChild())
				{
					var parentTable = relation.ParentTable;
					var childTable = relation.ChildTable;

					if (parentTable.TypedTable == TypedTableConstants.EnumOnly || childTable.TypedTable == TypedTableConstants.EnumOnly)
					{
						//Do Nothing
					}

					else if (!parentTable.AssociativeTable && !childTable.AssociativeTable && parentTable.Generated && childTable.Generated)
					{
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// The relation " + parentTable.PascalName + " -> " + childTable.PascalName + (string.IsNullOrEmpty(relation.RoleName) ? string.Empty : " (Role: " + relation.PascalRoleName + ")"));
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		[DataMember]");
						sb.AppendLine("		public virtual " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + parentTable.PascalName + " " + relation.PascalRoleName + parentTable.PascalName + " { get; set; }");
						sb.AppendLine();
					}
				}

				this.AppendClone();

				sb.AppendLine("		#endregion");
				sb.AppendLine();

				sb.AppendLine("	}");
				sb.AppendLine();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendClone()
		{
			if (_item.IsAbstract)
				return;

			var modifieraux = string.Empty;
			if (_item.ParentTable == null)
			{
				modifieraux = "virtual";
			}
			else
			{
				if (_item.ParentTable.IsAbstract)
					modifieraux = "virtual";
				else
					modifieraux = "new";
			}

			sb.AppendLine("		#region Clone");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a shallow copy of this object of all simple properties");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public " + modifieraux + " object Clone()");
			sb.AppendLine("		{");
			sb.AppendLine("			var newItem = new " + _item.PascalName + "();");
			foreach (var column in _item.GetColumnsFullHierarchy().Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendLine("			newItem." + column.PascalName + " = this." + column.PascalName + ";");
			}
			sb.AppendLine("			return newItem;");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a shallow copy of this object of all simple properties");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _item.PascalName + " Clone(" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.PascalName + " item)");
			sb.AppendLine("		{");
			sb.AppendLine("			var newItem = new " + _item.PascalName + "();");
			foreach (var column in _item.GetColumnsFullHierarchy().Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendLine("			newItem." + column.PascalName + " = item." + column.PascalName + ";");
			}
			sb.AppendLine("			return newItem;");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendEnum()
		{
			try
			{
				if ((_item.TypedTable != TypedTableConstants.None) && _item.PrimaryKeyColumns.Count == 1)
				{
					sb.AppendLine("		#region StaticDataConstants Enumeration");
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Enumeration to define static data items and their ids '" + _item.PascalName + "' table.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public enum " + _item.PascalName + "Constants");
					sb.AppendLine("		{");
					foreach (RowEntry rowEntry in _item.StaticData)
					{
						//ToDo: More than one column as primary key blows up
						var id = string.Empty;
						var name = string.Empty;
						var description = string.Empty;
						foreach (CellEntry cellEntry in rowEntry.CellEntries)
						{
							var column = (Column) cellEntry.ColumnRef.Object;
							var pk = (Column) _item.PrimaryKeyColumns[0];
							if (column.Key == pk.Key)
								id = cellEntry.Value;
							if (StringHelper.Match(column.Name, "name"))
								name = ValidationHelper.MakeCodeIdentifer(cellEntry.Value);
							if (StringHelper.Match(column.Name, "description"))
								description = cellEntry.Value;
						}

						if (string.IsNullOrEmpty(name)) name = description;
						if (!string.IsNullOrEmpty(description))
						{
							sb.AppendLine("			/// <summary>");
							sb.AppendLine("			/// " + description);
							sb.AppendLine("			/// </summary>");
							sb.AppendLine("			[Description(\"" + description + "\")]");
						}
						else
						{
							sb.AppendLine("			/// <summary>");
							sb.AppendLine("			/// Enumeration for the '" + name + "' item");
							sb.AppendLine("			/// </summary>");
							sb.AppendLine("			[Description(\"" + description + "\")]");
						}

						var key = ValidationHelper.MakeDatabaseIdentifier(name.Replace(" ", ""));
						if ((key.Length > 0) && ("0123456789".Contains(key[0])))
							key = "_" + key;

						sb.AppendLine("			" + key + " = " + id + ",");
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

	}
}