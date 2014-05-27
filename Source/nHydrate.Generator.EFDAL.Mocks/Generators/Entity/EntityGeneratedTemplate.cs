#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.EFDAL.Mocks.Generators
{
	public class EntityGeneratedTemplate : EFDALMockBaseTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private readonly Table _item;

		public EntityGeneratedTemplate(ModelRoot model, Table currentTable)
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
				try
				{
					sb = new StringBuilder();
					this.GenerateContent();
					return sb.ToString();
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}
		#endregion

		#region GenerateContent

		private void GenerateContent()
		{
			try
			{
				nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
				nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
				sb.AppendLine("{");
				this.AppendEntityClass();
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendUsingStatements()
		{
			sb.AppendLine("#pragma warning disable 1522"); //Empty switch block
			sb.AppendLine("#pragma warning disable 0108"); //Hides inherited member IAuditable, look into this
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using System.Data.Objects.DataClasses;");
			sb.AppendLine("using System.Xml.Serialization;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.Text;");
			sb.AppendLine("using " + this.GetLocalNamespace() + ";");
			sb.AppendLine("using " + this.DefaultNamespace + ".EFDAL.Interfaces;");
			sb.AppendLine("using " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity;");
			sb.AppendLine("using System.Collections.ObjectModel;");
			sb.AppendLine();
		}

		private void AppendEntityClass()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// This is a mock class for the EF entity " + _item.PascalName);
			sb.AppendLine("	/// </summary>");

			sb.AppendLine("	[System.ComponentModel.DataAnnotations.MetadataType(typeof(" + this.InterfaceProjectNamespace + ".Entity.Metadata." + _item.PascalName + "Metadata))]");
			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
			if (_item.ParentTable == null)
				sb.Append("	public partial class " + _item.PascalName + " : nHydrate.EFCore.DataAccess.NHEntityObject, " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.PascalName);
			else
				sb.Append("	public partial class " + _item.PascalName + " : " + this.GetLocalNamespace() + ".Entity." + _item.ParentTable.PascalName + ", " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.PascalName);

			if (_item.AllowCreateAudit || _item.AllowModifiedAudit || _item.AllowTimestamp)
			{
			  sb.Append(", nHydrate.EFCore.DataAccess.IAuditable");
			}

			var boInterface = ", nHydrate.EFCore.DataAccess.IBusinessObject, nHydrate.EFCore.DataAccess.INHEntityObject, System.ComponentModel.IDataErrorInfo";
			if (_item.Immutable) boInterface = ", nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject, nHydrate.EFCore.DataAccess.INHEntityObject, System.ComponentModel.IDataErrorInfo";
			boInterface += ", System.IEquatable<I" + _item.PascalName + ">";
			sb.Append(boInterface);

			sb.AppendLine();

			sb.AppendLine("	{");
			this.AppendedFieldEnum();
			this.AppendRegionSetValue();
			this.AppendRegionGetValue();
			this.AppendConstructor();
			this.AppendProperties();
			this.AppendMethods();
			this.AppendNavigationProperties();
			this.AppendIBusinessObject();
			this.AppendIAuditable();
			this.AppendIEquatable();
			this.AppendIDataErrorInfo();
			sb.AppendLine("	}");
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
			var allColumns = _item.GetColumnsFullHierarchy(true).Where(x => x.Generated).ToList();
			foreach (var column in allColumns.OrderBy(x => x.Name))
			{
				var relationParentTable = (Table)column.ParentTableRef.Object;
				var childColumnList = relationParentTable.AllRelationships.FindByChildColumn(column);
				sb.AppendLine("			if (field == FieldNameConstants." + column.PascalName + ")");
				if (column.AllowNull)
					sb.AppendLine("				return ((this." + column.PascalName + " == null) ? defaultValue : this." + column.PascalName + ");");
				else
					sb.AppendLine("				return this." + column.PascalName + ";");
			}

			if (_item.AllowCreateAudit)
			{
				sb.AppendLine("			if (field == FieldNameConstants." + _model.Database.CreatedByPascalName + ")");
				sb.AppendLine("				return ((this." + _model.Database.CreatedByPascalName + " == null) ? defaultValue : this." + _model.Database.CreatedByPascalName + ");");
				sb.AppendLine("			if (field == FieldNameConstants." + _model.Database.CreatedDatePascalName + ")");
				sb.AppendLine("				return ((this." + _model.Database.CreatedDatePascalName + " == null) ? defaultValue : this." + _model.Database.CreatedDatePascalName + ");");
			}

			if (_item.AllowModifiedAudit)
			{
				sb.AppendLine("			if (field == FieldNameConstants." + _model.Database.ModifiedByPascalName + ")");
				sb.AppendLine("				return ((this." + _model.Database.ModifiedByPascalName + " == null) ? defaultValue : this." + _model.Database.ModifiedByPascalName + ");");
				sb.AppendLine("			if (field == FieldNameConstants." + _model.Database.ModifiedDatePascalName + ")");
				sb.AppendLine("				return ((this." + _model.Database.ModifiedDatePascalName + " == null) ? defaultValue : this." + _model.Database.ModifiedDatePascalName + ");");
			}

			//Now do the primary keys
			foreach (var dbColumn in _item.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				//TODO
			}

			sb.AppendLine("			throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int GetInteger(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetInteger(field, int.MinValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int GetInteger(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, int defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				int a;");
			sb.AppendLine("				if (int.TryParse((string)o, out a))");
			sb.AppendLine("					return a;");
			sb.AppendLine("				else");
			sb.AppendLine("					throw new System.InvalidCastException();");
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
			sb.AppendLine("		public double GetDouble(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDouble(field, double.MinValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public double GetDouble(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, double defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				double a;");
			sb.AppendLine("				if (double.TryParse((string)o, out a))");
			sb.AppendLine("					return a;");
			sb.AppendLine("				else");
			sb.AppendLine("					throw new System.InvalidCastException();");
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
			sb.AppendLine("		public System.DateTime GetDateTime(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetDateTime(field, System.DateTime.MinValue);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public System.DateTime GetDateTime(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, DateTime defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
			sb.AppendLine("			if (o is string)");
			sb.AppendLine("			{");
			sb.AppendLine("				System.DateTime a;");
			sb.AppendLine("				if (System.DateTime.TryParse((string)o, out a))");
			sb.AppendLine("					return a;");
			sb.AppendLine("				else");
			sb.AppendLine("					throw new System.InvalidCastException();");
			sb.AppendLine("			}");
			sb.AppendLine("			else");
			sb.AppendLine("			{");
			sb.AppendLine("				return (System.DateTime)o;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string GetString(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetString(field, string.Empty);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the value of one of this object's properties.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string GetString(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, string defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
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

		private void AppendRegionSetValue()
		{
			if (_item.Immutable)
				return;

			sb.AppendLine("		#region SetValue");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Assigns a value to a field on this object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"field\">The field to set</param>");
			sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
			sb.AppendLine("		public virtual void SetValue(FieldNameConstants field, object newValue)");
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
			sb.AppendLine("		public virtual void SetValue(FieldNameConstants field, object newValue, bool fixLength)");
			sb.AppendLine("		{");

			var allColumns = _item.GetColumnsFullHierarchy(true).Where(x => x.Generated).ToList();
			var count = 0;
			foreach (var column in allColumns.OrderBy(x => x.Name))
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
					else if (column.ComputedColumn)
					{
						sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' is a computed column and cannot be set!\");");
					}
					else
					{
						if (ModelHelper.IsTextType(column.DataType))
						{
							sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperInternal((string)newValue, fixLength, GetMaxLength(field));");
							#region old code
							//sb.AppendLine("				if (newValue == null)");
							//sb.AppendLine("				{");
							//sb.AppendLine("					this." + column.PascalName + " = null;");
							//sb.AppendLine("				}");
							//sb.AppendLine("				else");
							//sb.AppendLine("				{");
							//sb.AppendLine("					string v = newValue.ToString();");
							//sb.AppendLine("					if (fixLength)");
							//sb.AppendLine("					{");
							//sb.AppendLine("						int fieldLength = GetMaxLength(FieldNameConstants." + column.PascalName + ");");
							//sb.AppendLine("						if ((fieldLength > 0) && (v.Length > fieldLength)) v = v.Substring(0, fieldLength);");
							//sb.AppendLine("					}");
							//sb.AppendLine("					this." + column.PascalName + " = v;");
							//sb.AppendLine("				}");
							//sb.AppendLine("				return;");
							#endregion
						}
						else if (column.DataType == System.Data.SqlDbType.Float)
						{
							if (column.AllowNull)
								sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDoubleNullableInternal(newValue);");
							else
								sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDoubleNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
						}
						else if (ModelHelper.IsDateType(column.DataType))
						{
							if (column.AllowNull)
								sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDateTimeNullableInternal(newValue);");
							else
								sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDateTimeNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
						}
						else if (column.DataType == System.Data.SqlDbType.Bit)
						{
							if (column.AllowNull)
								sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperBoolNullableInternal(newValue);");
							else
								sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperBoolNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
						}
						else if (column.DataType == System.Data.SqlDbType.Int)
						{
							if (column.AllowNull)
								sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperIntNullableInternal(newValue);");
							else
								sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperIntNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
						}
						else if (column.DataType == System.Data.SqlDbType.BigInt)
						{
							if (column.AllowNull)
								sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperLongNullableInternal(newValue);");
							else
								sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperLongNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
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
							var relationParentTable = column.ParentTable;
							var list = relationParentTable.AllRelationships.FindByChildColumn(column).ToList();
							if (list.Count > 0)
							{
								var relation = list.First();
								var pTable = relation.ParentTable;
								if (pTable.Generated && pTable.TypedTable != TypedTableConstants.EnumOnly)
								{
									var s = pTable.PascalName;
									sb.AppendLine("					if(newValue is " + pTable.PascalName + ")");
									sb.AppendLine("					{");
									if (column.EnumType == string.Empty)
									{
										var columnRelationship = relation.ColumnRelationships.GetByParentField(column);
										var parentColumn = columnRelationship.ParentColumn;
										sb.AppendLine("						this." + column.PascalName + " = ((" + pTable.PascalName + ")newValue)." + parentColumn.PascalName + ";");
										sb.AppendLine("					}");
										sb.AppendLine("					else if (newValue is nHydrate.EFCore.DataAccess.IPrimaryKey)");
										sb.AppendLine("					{");
										sb.AppendLine("						this." + column.PascalName + " = ((" + pTable.PascalName + "PrimaryKey)newValue)." + parentColumn.PascalName + ";");
									}
									else //This is an Enumeration type
										sb.AppendLine("						throw new Exception(\"Field '" + column.PascalName + "' does not allow values of this type!\");");

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
									sb.AppendLine("						this." + column.PascalName + " = (" + column.EnumType + ")Enum.Parse(typeof(" + column.EnumType + "), newValue.ToString());");
								}
								sb.AppendLine("					}");
								sb.AppendLine("					else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
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
							//sb.AppendLine("				return;");
						} //All non-string types

					}
					sb.AppendLine("			}");
					count++;
				}

			}

			sb.AppendLine("		else");
			sb.AppendLine("			throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendedFieldEnum()
		{
			var imageColumnList = _item.GetColumnsByType(System.Data.SqlDbType.Image);
			if (imageColumnList.Count() != 0)
			{
				sb.AppendLine("		#region FieldNameConstants Enumeration");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// An enumeration of this object's image type fields");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public enum FieldImageConstants");
				sb.AppendLine("		{");
				foreach (var column in imageColumnList)
				{
					sb.AppendLine("			/// <summary>");
					sb.AppendLine("			/// Field mapping for the image column '" + column.PascalName + "' property");
					sb.AppendLine("			/// </summary>");
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
			sb.AppendLine("		/// Enumeration to define each property that maps to a database field for the '" + _item.PascalName + "' table.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + (_item.ParentTable == null ? "" : "new ") + "enum FieldNameConstants");
			sb.AppendLine("		{");
			foreach (var column in _item.GeneratedColumnsFullHierarchy)
			{
				sb.AppendLine("			/// <summary>");
				sb.AppendLine("			/// Field mapping for the '" + column.PascalName + "' property");
				sb.AppendLine("			/// </summary>");
				if (column.PrimaryKey)
					sb.AppendLine("			[nHydrate.EFCore.Attributes.PrimaryKeyAttribute()]");
				sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
				sb.AppendLine("			" + column.PascalName + ",");
			}

			if (_item.AllowCreateAudit)
			{
				sb.AppendLine("			/// <summary>");
				sb.AppendLine("			/// Field mapping for the '" + _model.Database.CreatedByPascalName + "' property");
				sb.AppendLine("			/// </summary>");
				sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.CreatedByPascalName + "' property\")]");
				sb.AppendLine("			" + _model.Database.CreatedByPascalName + ",");
				sb.AppendLine("			/// <summary>");
				sb.AppendLine("			/// Field mapping for the '" + _model.Database.CreatedDatePascalName + "' property");
				sb.AppendLine("			/// </summary>");
				sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.CreatedDatePascalName + "' property\")]");
				sb.AppendLine("			" + _model.Database.CreatedDatePascalName + ",");
			}

			if (_item.AllowModifiedAudit)
			{
				sb.AppendLine("			/// <summary>");
				sb.AppendLine("			/// Field mapping for the '" + _model.Database.ModifiedByPascalName + "' property");
				sb.AppendLine("			/// </summary>");
				sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.ModifiedByPascalName + "' property\")]");
				sb.AppendLine("			" + _model.Database.ModifiedByPascalName + ",");
				sb.AppendLine("			/// <summary>");
				sb.AppendLine("			/// Field mapping for the '" + _model.Database.ModifiedDatePascalName + "' property");
				sb.AppendLine("			/// </summary>");
				sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.ModifiedDatePascalName + "' property\")]");
				sb.AppendLine("			" + _model.Database.ModifiedDatePascalName + ",");
			}

			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendConstructor()
		{
			sb.AppendLine("		#region Constructor");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Method called when an instance of this class is created");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		partial void OnCreated();");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The default constructor for this class");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _item.PascalName + "()");
			sb.AppendLine("		{");

			sb.Append(this.SetInitialValues());

			var relationList = _item.GetRelations().AsEnumerable();
			foreach (var relation in relationList)
			{
				var parentTable = relation.ParentTable;
				var childTable = relation.ChildTable;

				//If not both generated then do not process this code block
				if (!parentTable.Generated || !childTable.Generated)
				{
					//Do Nothing
				}

				//inheritance relationship
				else if (parentTable == childTable.ParentTable && relation.IsOneToOne)
				{
					//Do Nothing
				}
				//Do not walk to associative
				else if (parentTable.TypedTable != TypedTableConstants.None || childTable.TypedTable != TypedTableConstants.None)
				{
					//Do Nothing
				}

				//Skip 1-1 relations
				else if (relation.IsOneToOne)
				{
					//Do Nothing
				}

				//Process the associative tables
				else if (childTable.AssociativeTable)
				{
					var associativeRelations = childTable.GetRelationsWhereChild();
					if (associativeRelations.Count() == 2)
					{
						Relation targetRelation = null;
						Relation otherRelation = null;
						var relation1 = associativeRelations.First();
						var relation2 = associativeRelations.Last();
						if (_item == relation1.ParentTable) targetRelation = relation2;
						else targetRelation = relation1;
						if (targetRelation == relation2) otherRelation = relation1;
						else otherRelation = relation2;
						var targetTable = targetRelation.ParentTable;

						if (targetTable.Generated && targetTable.TypedTable == TypedTableConstants.None)
						{
							sb.AppendLine("			//Setup the " + targetTable.PascalName + " child collection");
							sb.AppendLine("			_" + otherRelation.PascalRoleName + targetTable.PascalName + "List = new ObservableCollection<" + this.InterfaceProjectNamespace + ".Entity.I" + targetTable.PascalName + ">();");
							sb.AppendLine("			((ObservableCollection<" + this.InterfaceProjectNamespace + ".Entity.I" + targetTable.PascalName + ">)_" + otherRelation.PascalRoleName + targetTable.PascalName + "List).CollectionChanged += (sender, e) =>");
							sb.AppendLine("			{");
							sb.AppendLine("				if (e.NewItems != null)");
							sb.AppendLine("				{");
							sb.AppendLine("					foreach (" + this.InterfaceProjectNamespace + ".Entity.I" + targetTable.PascalName + " item in e.NewItems)");
							sb.AppendLine("					{");
							sb.AppendLine("						if (item." + targetRelation.PascalRoleName + parentTable.PascalName + "List != this)");
							sb.AppendLine("						{");
							sb.AppendLine("							item." + targetRelation.PascalRoleName + parentTable.PascalName + "List.Add(this);");
							sb.AppendLine("						}");
							sb.AppendLine("					}");
							sb.AppendLine("				}");
							sb.AppendLine("				if (e.OldItems != null)");
							sb.AppendLine("				{");
							sb.AppendLine("					foreach (" + this.InterfaceProjectNamespace + ".Entity.I" + targetTable.PascalName + " item in e.OldItems)");
							sb.AppendLine("					{");
							sb.AppendLine("						if (item." + targetRelation.PascalRoleName + parentTable.PascalName + "List == this)");
							sb.AppendLine("						{");
							sb.AppendLine("							item." + targetRelation.PascalRoleName + parentTable.PascalName + "List.Remove(this);");
							sb.AppendLine("						}");
							sb.AppendLine("					}");
							sb.AppendLine("				}");
							sb.AppendLine("			};");
							sb.AppendLine();
						}
					}
				}

				//Process relations where Current Table is the parent
				else if (parentTable == _item && parentTable.Generated && childTable.Generated && (childTable.TypedTable == TypedTableConstants.None) && !childTable.AssociativeTable)
				{
					sb.AppendLine("			//Setup the " + childTable.PascalName + " child collection");
					sb.AppendLine("			_" + relation.PascalRoleName + childTable.PascalName + "List = new ObservableCollection<" + this.InterfaceProjectNamespace + ".Entity.I" + childTable.PascalName + ">();");
					sb.AppendLine("			((ObservableCollection<" + this.InterfaceProjectNamespace + ".Entity.I" + childTable.PascalName + ">)_" + relation.PascalRoleName + childTable.PascalName + "List).CollectionChanged += (sender, e) =>");
					sb.AppendLine("			{");
					sb.AppendLine("				if (e.NewItems != null)");
					sb.AppendLine("				{");
					sb.AppendLine("					foreach (" + this.InterfaceProjectNamespace + ".Entity.I" + childTable.PascalName + " item in e.NewItems)");
					sb.AppendLine("					{");
					sb.AppendLine("						if (item." + relation.PascalRoleName + parentTable.PascalName + " != this)");
					sb.AppendLine("						{");
					sb.AppendLine("							item." + relation.PascalRoleName + parentTable.PascalName + " = this;");
					sb.AppendLine("						}");
					sb.AppendLine("					}");
					sb.AppendLine("				}");
					sb.AppendLine("				if (e.OldItems != null)");
					sb.AppendLine("				{");
					sb.AppendLine("					foreach (" + this.InterfaceProjectNamespace + ".Entity.I" + childTable.PascalName + " item in e.OldItems)");
					sb.AppendLine("					{");
					sb.AppendLine("						if (item." + relation.PascalRoleName + parentTable.PascalName + " == this)");
					sb.AppendLine("						{");
					sb.AppendLine("							item." + relation.PascalRoleName + parentTable.PascalName + " = null;");
					sb.AppendLine("						}");
					sb.AppendLine("					}");
					sb.AppendLine("				}");
					sb.AppendLine("			};");
					sb.AppendLine();
				}
			}

			sb.AppendLine("			this.OnCreated();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendProperties()
		{
			sb.AppendLine("		#region Properties");
			sb.AppendLine();

			foreach (var column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
			{
				//Add new fake property for typed enumerations
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

				if (column.PrimaryKey && _item.ParentTable != null)
				{
					//PK in descendant, do not process
				}
				//else if (_currentTable.IsColumnRelatedToTypeTable(column))
				//{
				//  Table typeTable = _currentTable.GetRelatedTypeTableByColumn(column);

				//  //If this column is a type table column then generate a special property
				//  sb.AppendLine("		/// <summary>");
				//  if (!string.IsNullOrEmpty(column.Description))
				//    StringHelper.LineBreakCode(sb, column.Description, "		/// ");
				//  else
				//    sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field");
				//  sb.AppendLine("		/// </summary>");
				//  sb.Append("		public virtual " + typeTable.PascalName + "Wrapper " + typeTable.PascalName);
				//  sb.Append(" { get; ");
				//  if (column.PrimaryKey || _currentTable.Immutable)
				//    sb.Append("private ");
				//  sb.Append("set; ");
				//  sb.AppendLine("}");
				//  sb.AppendLine();
				//}
				else
				{
					sb.AppendLine("		/// <summary>");
					if (!string.IsNullOrEmpty(column.Description))
						StringHelper.LineBreakCode(sb, column.Description, "		/// ");
					else
						sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		/// <remarks>" + column.GetIntellisenseRemarks() + "</remarks>");
					sb.AppendLine("		[System.ComponentModel.Browsable(" + column.IsBrowsable.ToString().ToLower() + ")]");

					if (!string.IsNullOrEmpty(column.Category))
						sb.AppendLine("		[System.ComponentModel.Category(\"" + column.Category + "\")]");

					if (column.PrimaryKey)
						sb.AppendLine("		[System.ComponentModel.DataAnnotations.Key()]");

					if (column.PrimaryKey || _item.Immutable || column.ComputedColumn || column.IsReadOnly)
						sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");

					if (!string.IsNullOrEmpty(column.Description))
						sb.AppendLine("		[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(column.Description) + "\")]");

					sb.AppendLine("		[System.ComponentModel.DisplayName(\"" + column.GetFriendlyName() + "\")]");

					if (column.UIDataType != System.ComponentModel.DataAnnotations.DataType.Custom)
					{
						sb.AppendLine("		[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType." + column.UIDataType.ToString() + ")]");
					}

					if (!string.IsNullOrEmpty(column.Mask))
					{
						sb.AppendLine("		[System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = @\"" + column.Mask.Replace(@"\\", @"\\\\") + "\")]");
					}

					sb.Append("		public virtual " + column.GetCodeType() + " " + column.PascalName);
					sb.Append(" { get; ");
					//if (column.PrimaryKey || _currentTable.Immutable)
					if (_item.Immutable || column.ComputedColumn || column.IsReadOnly)
						sb.Append("private ");
					sb.Append("set; ");
					sb.AppendLine("}");
					sb.AppendLine();
				}
			}

			//Audit Fields
			if (_item.ParentTable == null)
			{
				if (_item.AllowCreateAudit) GenerateAuditField(_model.Database.CreatedByPascalName, "string", "The audit field for the 'Created By' column.", false);
				if (_item.AllowCreateAudit) GenerateAuditField(_model.Database.CreatedDatePascalName, "DateTime?", "The audit field for the 'Created Date' column.", false);
				if (_item.AllowModifiedAudit) GenerateAuditField(_model.Database.ModifiedByPascalName, "string", "The audit field for the 'Modified By' column.", false);
				if (_item.AllowModifiedAudit) GenerateAuditField(_model.Database.ModifiedDatePascalName, "DateTime?", "The audit field for the 'Modified Date' column.", false);
				if (_item.AllowTimestamp) GenerateAuditField(_model.Database.TimestampPascalName, "byte[]", "The audit field for the 'Timestamp' parameter.", false);
			}

			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendMethods()
		{
			sb.AppendLine("		#region Overrides");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines if all of the fields for the specified object exactly matches the current object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"item\">The object to compare</param>");
			sb.AppendLine("		public override bool IsEquivalent(nHydrate.EFCore.DataAccess.INHEntityObject item)");
			sb.AppendLine("		{");
			sb.AppendLine("			return ((System.IEquatable<" + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + ">)this).Equals(item as " + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + ");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void GenerateAuditField(string columnName, string codeType, string description, bool allowsetter)
		{
			if (!string.IsNullOrEmpty(description))
			{
				sb.AppendLine("		/// <summary>");
				StringHelper.LineBreakCode(sb, description, "		/// ");
				sb.AppendLine("		/// </summary>");
			}

			sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
			sb.Append("		public virtual " + codeType + " " + columnName);
			sb.Append(" { get;");
			if (allowsetter) sb.Append(" set;");
			else sb.Append(" internal set;");
			sb.AppendLine(" }");
			sb.AppendLine();
		}

		private void AppendNavigationProperties()
		{
			sb.AppendLine("		#region Navigation Properties");
			sb.AppendLine();

			#region View Relations
			{
				var relationList = _item.GetViewRelations().Where(x => x.IsValidEFRelation);
				foreach (var relation in relationList)
				{
					var parentTable = relation.ParentTable;
					var childTable = relation.ChildView;

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The back navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName);
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public virtual ICollection<" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List");
					sb.AppendLine("		{");
					sb.AppendLine("			get { return _" + relation.PascalRoleName + childTable.PascalName + "List; }");
					sb.AppendLine("		}");
					sb.AppendLine("		private ICollection<" + this.InterfaceProjectNamespace + ".Entity.I" + childTable.PascalName + "> _" + relation.PascalRoleName + childTable.PascalName + "List;");
					sb.AppendLine();
				}
			} //block
			#endregion

			#region Parent Relations
			{
				var relationList = _item.GetRelations().Where(x => x.IsValidEFRelation).AsEnumerable();
				foreach (var relation in relationList)
				{
					var parentTable = (Table)relation.ParentTableRef.Object;
					var childTable = (Table)relation.ChildTableRef.Object;

					//If not both generated then do not process this code block
					if (!parentTable.Generated || !childTable.Generated)
					{
						//Do Nothing
					}

					//inheritance relationship
					else if (parentTable == childTable.ParentTable && relation.IsOneToOne)
					{
						//Do Nothing
					}
					//Do not walk to associative
					//else if (parentTable.IsTypeTable || childTable.IsTypeTable)
					//{
					//  //Do Nothing
					//}

					//Skip 1-1 relations
					else if (relation.IsOneToOne)
					{
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// The navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (for role '" + relation.PascalRoleName + "')"));
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		" + this.InterfaceProjectNamespace + ".Entity.I" + childTable.PascalName + " " + this.InterfaceProjectNamespace + ".Entity.I" + parentTable.PascalName + "." + relation.PascalRoleName + childTable.PascalName);
						sb.AppendLine("		{");
						sb.AppendLine("			get { return _" + relation.PascalRoleName + childTable.PascalName + "; }");
						if (relation.AreAllFieldsPK)
						{
							sb.AppendLine("			set { _" + relation.PascalRoleName + childTable.PascalName + " = value; }");
						}
						sb.AppendLine("		}");
						sb.AppendLine("		private " + this.InterfaceProjectNamespace + ".Entity.I" + childTable.PascalName + " _" + relation.PascalRoleName + childTable.PascalName + " = null;");

						sb.AppendLine();
					}

					//Process the associative tables
					else if (childTable.AssociativeTable)
					{
						var associativeRelations = childTable.GetRelationsWhereChild().ToList();
						if (associativeRelations.Count == 2)
						{
							Relation targetRelation = null;
							Relation otherRelation = null;
							var relation1 = associativeRelations.First();
							var relation2 = associativeRelations.Last();
							if (_item == relation1.ParentTable) targetRelation = relation2;
							else targetRelation = relation1;
							if (targetRelation == relation2) otherRelation = relation1;
							else otherRelation = relation2;
							var targetTable = targetRelation.ParentTable;

							if (targetTable.Generated && targetTable.TypedTable == TypedTableConstants.None)
							{
								sb.AppendLine("		/// <summary>");
								sb.AppendLine("		/// The back navigation definition for walking " + _item.PascalName + "->" + otherRelation.PascalRoleName + targetTable.PascalName);
								sb.AppendLine("		/// </summary>");
								sb.AppendLine("		public virtual ICollection<" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + targetTable.PascalName + "> " + otherRelation.PascalRoleName + targetTable.PascalName + "List");
								sb.AppendLine("		{");
								sb.AppendLine("			get { return _" + otherRelation.PascalRoleName + targetTable.PascalName + "List; }");
								sb.AppendLine("		}");
								sb.AppendLine("		private ICollection<" + this.InterfaceProjectNamespace + ".Entity.I" + targetTable.PascalName + "> _" + otherRelation.PascalRoleName + targetTable.PascalName + "List;");
								sb.AppendLine();
							}
						}
					}

					//Process relations where Current Table is the parent and self-referencing
					else if (parentTable == _item &&
						parentTable.Generated && childTable.Generated &&
						childTable.TypedTable == TypedTableConstants.None && !childTable.AssociativeTable &&
						parentTable == childTable)
					{
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// The back navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName);
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		public virtual ICollection<" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List");
						sb.AppendLine("		{");
						sb.AppendLine("			get { return _" + relation.PascalRoleName + childTable.PascalName + "List; }");
						sb.AppendLine("		}");
						sb.AppendLine("		private ICollection<" + this.InterfaceProjectNamespace + ".Entity.I" + childTable.PascalName + "> _" + relation.PascalRoleName + childTable.PascalName + "List;");
						sb.AppendLine();
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// The back navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (role: '" + relation.PascalRoleName + "')"));
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.PascalName + " " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.PascalName + "." + relation.PascalRoleName + "" + _item.PascalName);
						sb.AppendLine("		{");
						sb.AppendLine("			get { return this." + relation.PascalRoleName + "" + _item.PascalName + "; }");
						sb.AppendLine("			set { this." + relation.PascalRoleName + "" + _item.PascalName + " = value; }");
						sb.AppendLine("		}");
						sb.AppendLine();
					}

					//Process relations where Current Table is the parent
					else if (parentTable == _item && parentTable.Generated && childTable.Generated && childTable.TypedTable == TypedTableConstants.None && !childTable.AssociativeTable)
					{
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// The back navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName);
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		public virtual ICollection<" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List");
						sb.AppendLine("		{");
						sb.AppendLine("			get { return _" + relation.PascalRoleName + childTable.PascalName + "List; }");
						sb.AppendLine("		}");
						sb.AppendLine("		private ICollection<" + this.InterfaceProjectNamespace + ".Entity.I" + childTable.PascalName + "> _" + relation.PascalRoleName + childTable.PascalName + "List;");
						sb.AppendLine();
					}

				}
			} //block
			#endregion

			#region Child Relations
			{
				var relationList = _item.GetRelationsWhereChild().Where(x => x.IsValidEFRelation);
				foreach (var relation in relationList)
				{
					var parentTable = relation.ParentTable;
					var childTable = relation.ChildTable;

					//Do not walk to associative
					//if (parentTable.IsTypeTable || childTable.IsTypeTable)
					//{
					//  sb.AppendLine("		" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + parentTable.PascalName + "Wrapper " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + childTable.PascalName + "." + parentTable.PascalName + "");
					//  sb.AppendLine("		{");
					//  sb.AppendLine("			get { return this." + parentTable.PascalName + "; }");
					//  sb.AppendLine("			set { this." + parentTable.PascalName + " = (" + this.GetLocalNamespace() + ".Entity." + parentTable.PascalName + "Wrapper)value; }");
					//  sb.AppendLine("		}");
					//  sb.AppendLine();
					//}

					//inheritance relationship
					if (parentTable == childTable.ParentTable && relation.IsOneToOne)
					{
						//Do Nothing
					}

					else if (parentTable.TypedTable == TypedTableConstants.EnumOnly)
					{
						//Do Nothing
					}

					//Self Referencing
					//else if ((childTable == _currentTable) && (childTable == parentTable) && parentTable.Generated && childTable.Generated)
					//{
					//}

					//1-1 Relation
					else if (childTable == _item && parentTable.Generated && childTable.Generated && (childTable != parentTable) && !childTable.IsInheritedFrom(parentTable) && relation.IsOneToOne)
					{
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// The navigation definition for walking " + childTable.PascalName + "->" + parentTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (for role '" + relation.PascalRoleName + "')"));
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		public virtual " + this.InterfaceProjectNamespace + ".Entity.I" + parentTable.PascalName + " " + relation.PascalRoleName + parentTable.PascalName + "");
						sb.AppendLine("		{");
						sb.AppendLine("			get { return _" + relation.PascalRoleName + parentTable.PascalName + "; }");
						sb.AppendLine("			set");
						sb.AppendLine("			{");
						sb.AppendLine("				if (!Object.ReferenceEquals(_" + relation.PascalRoleName + parentTable.PascalName + ", value))");
						sb.AppendLine("				{");
						sb.AppendLine("					var existingValue = _" + relation.PascalRoleName + parentTable.PascalName + ";");
						sb.AppendLine();
						sb.AppendLine("					_" + relation.PascalRoleName + parentTable.PascalName + " = value;");
						sb.AppendLine();
						sb.AppendLine("					if (value != null)");
						sb.AppendLine("					{");
						foreach (var columnRelationship in relation.ColumnRelationships.ToList())
						{
							var parentColumn = columnRelationship.ParentColumn;
							var childColumn = columnRelationship.ChildColumn;
							sb.AppendLine("						this." + childColumn.PascalName + " = value." + parentColumn.PascalName + ";");
						}
						sb.AppendLine("					}");

						//if (relation.AreAllFieldsPK)
						if (relation.AreAllFieldsPK && !parentTable.IsInheritedFrom(childTable))
						{
							sb.AppendLine("					if ((existingValue != null) && (existingValue." + relation.PascalRoleName + childTable.PascalName + " != this))");
							sb.AppendLine("						existingValue." + relation.PascalRoleName + childTable.PascalName + " = this;");
							sb.AppendLine("					if ((value != null) && value." + relation.PascalRoleName + childTable.PascalName + " != this)");
							sb.AppendLine("						value." + relation.PascalRoleName + childTable.PascalName + " = this;");
						}

						sb.AppendLine("				}");
						sb.AppendLine("			}");
						sb.AppendLine("		}");
						sb.AppendLine("		private " + this.InterfaceProjectNamespace + ".Entity.I" + parentTable.PascalName + " _" + relation.PascalRoleName + parentTable.PascalName + " = null;");
						sb.AppendLine();
					}

					//Process relations where Current Table is the child
					else if (childTable == _item && parentTable.Generated && childTable.Generated & !childTable.IsInheritedFrom(parentTable))
					{
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// The navigation definition for walking " + childTable.PascalName + "->" + parentTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (for role '" + relation.PascalRoleName + "')"));
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		public virtual " + this.InterfaceProjectNamespace + ".Entity.I" + parentTable.PascalName + " " + relation.PascalRoleName + parentTable.PascalName + "");
						sb.AppendLine("		{");
						sb.AppendLine("			get { return _" + relation.PascalRoleName + parentTable.PascalName + "; }");
						sb.AppendLine("			set");
						sb.AppendLine("			{");
						sb.AppendLine("				if (!Object.ReferenceEquals(_" + relation.PascalRoleName + parentTable.PascalName + ", value))");
						sb.AppendLine("				{");
						sb.AppendLine("					var existingValue = _" + relation.PascalRoleName + parentTable.PascalName + ";");
						sb.AppendLine();
						sb.AppendLine("					_" + relation.PascalRoleName + parentTable.PascalName + " = value;");
						sb.AppendLine();
						sb.AppendLine("					if (value != null)");
						sb.AppendLine("					{");
						foreach (var columnRelationship in relation.ColumnRelationships.ToList())
						{
							var parentColumn = columnRelationship.ParentColumnRef.Object as Column;
							var childColumn = columnRelationship.ChildColumnRef.Object as Column;
							sb.AppendLine("						this." + childColumn.PascalName + " = value." + parentColumn.PascalName + ";");
						}
						sb.AppendLine("					}");

						sb.AppendLine("					if ((existingValue != null) && (existingValue." + relation.PascalRoleName + childTable.PascalName + "List.Contains(this)))");
						sb.AppendLine("					{");
						sb.AppendLine("						existingValue." + relation.PascalRoleName + childTable.PascalName + "List.Remove(this);");
						sb.AppendLine("					}");
						sb.AppendLine();
						sb.AppendLine("					if ((value != null) && !value." + relation.PascalRoleName + childTable.PascalName + "List.Contains(this))");
						sb.AppendLine("					{");
						sb.AppendLine("						value." + relation.PascalRoleName + childTable.PascalName + "List.Add(this);");
						sb.AppendLine("					}");

						sb.AppendLine("				}");
						sb.AppendLine("			}");
						sb.AppendLine("		}");
						sb.AppendLine("		private " + this.InterfaceProjectNamespace + ".Entity.I" + parentTable.PascalName + " _" + relation.PascalRoleName + parentTable.PascalName + " = null;");
						sb.AppendLine();
					}
				}
			} //block
			#endregion

			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendIBusinessObject()
		{
			sb.AppendLine("		#region IBusinessObject Members");
			sb.AppendLine();
			sb.AppendLine("		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetValue((FieldNameConstants)field, null);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(Enum field, object defaultValue)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.GetValue((FieldNameConstants)field, defaultValue);");
			sb.AppendLine("		}");
			sb.AppendLine();

			if (!_item.Immutable)
			{
				sb.AppendLine("		void nHydrate.EFCore.DataAccess.IBusinessObject.SetValue(System.Enum field, object newValue)");
				sb.AppendLine("		{");
				sb.AppendLine("			this.SetValue((FieldNameConstants)field, newValue);");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			sb.AppendLine("		Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldNameConstants()");
			sb.AppendLine("		{");
			sb.AppendLine("			return typeof(FieldNameConstants);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		int nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetMaxLength(System.Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			return " + _item.PascalName + ".GetMaxLength((FieldNameConstants)field);");
			sb.AppendLine("		}");
			sb.AppendLine();

			#region Static Method GetMaxLength

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Given a field definition this will return the maximum length of that field");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static int GetMaxLength(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			switch (field)");
			sb.AppendLine("			{");
			foreach (var column in _item.GetColumnsFullHierarchy().Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendLine("				case FieldNameConstants." + column.PascalName + ":");
				if (_item.GeneratedColumns.Contains(column))
				{
					//This is an actual column in this table
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
				else
				{
					//This is an inherited column so check the base
					sb.AppendLine("					return " + _item.ParentTable.PascalName + ".GetMaxLength(" + _item.ParentTable.PascalName + ".FieldNameConstants." + column.PascalName + ");");
				}
			}
			sb.AppendLine("			}");
			sb.AppendLine("			return 0;");
			sb.AppendLine("		}");
			sb.AppendLine();

			#endregion

			#region GetFieldType
			sb.AppendLine("		#region GetFieldType");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets the system type of a field on this object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static System.Type GetFieldType(FieldNameConstants field)");
			sb.AppendLine("		{");
			sb.AppendLine("			var o = new " + _item.PascalName + "();");
			sb.AppendLine("			return ((nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject)o).GetFieldType(field);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldType(Enum field)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (field.GetType() != typeof(FieldNameConstants))");
			sb.AppendLine("				throw new Exception(\"The '\" + field.GetType().ReflectedType.ToString() + \".FieldNameConstants' value is not valid. The field parameter must be of type '\" + this.GetType().ToString() + \".FieldNameConstants'.\");");
			sb.AppendLine();
			sb.AppendLine("			switch ((FieldNameConstants)field)");
			sb.AppendLine("			{");
			foreach (var column in _item.GeneratedColumnsFullHierarchy)
			{
				sb.AppendLine("				case FieldNameConstants." + column.PascalName + ": return typeof(" + column.GetCodeType() + ");");
			}
			sb.AppendLine("			}");
			sb.AppendLine("			return null;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			#endregion

			sb.AppendLine("		bool nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.IsEquivalent(nHydrate.EFCore.DataAccess.INHEntityObject item)");
			sb.AppendLine("		{");
			sb.AppendLine("			return ((System.IEquatable<" + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + ">)this).Equals(item as " + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + ");");
			sb.AppendLine("		}");
			sb.AppendLine();

			//If this is not derived then add the Primary key stuff
			if (_item.ParentTable == null)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Hold the primary key for this object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected nHydrate.EFCore.DataAccess.IPrimaryKey _primaryKey = null;");
				sb.AppendLine("		nHydrate.EFCore.DataAccess.IPrimaryKey nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.PrimaryKey");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				if (_primaryKey == null)");
				sb.Append("					_primaryKey = new " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + "PrimaryKey(");

				var ii = 0;
				foreach (var column in _item.PrimaryKeyColumns.OrderBy(x => x.Name))
				{
					sb.Append("this." + column.PascalName);
					if (ii < _item.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
					ii++;
				}
				sb.AppendLine(");");

				sb.AppendLine("				return _primaryKey;");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendIAuditable()
		{
			if (!_item.AllowCreateAudit && !_item.AllowModifiedAudit && !_item.AllowTimestamp)
				return;

			sb.AppendLine("		#region Auditing");
			sb.AppendLine("		string nHydrate.EFCore.DataAccess.IAuditable.CreatedBy");
			sb.AppendLine("		{");
			if (_item.AllowCreateAudit)
				sb.AppendLine("			get { return this." + _model.Database.CreatedByPascalName + "; }");
			else
				sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		System.DateTime? nHydrate.EFCore.DataAccess.IAuditable.CreatedDate");
			sb.AppendLine("		{");
			if (_item.AllowCreateAudit)
				sb.AppendLine("			get { return this." + _model.Database.CreatedDatePascalName + "; }");
			else
				sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		bool nHydrate.EFCore.DataAccess.IAuditable.IsCreateAuditImplemented");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return " + (_item.AllowCreateAudit ? "true" : "false") + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		bool nHydrate.EFCore.DataAccess.IAuditable.IsModifyAuditImplemented");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return " + (_item.AllowModifiedAudit ? "true" : "false") + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		bool nHydrate.EFCore.DataAccess.IAuditable.IsTimestampAuditImplemented");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return " + (_item.AllowTimestamp ? "true" : "false") + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		string nHydrate.EFCore.DataAccess.IAuditable.ModifiedBy");
			sb.AppendLine("		{");
			if (_item.AllowModifiedAudit)
				sb.AppendLine("			get { return this." + _model.Database.ModifiedByPascalName + "; }");
			else
				sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		System.DateTime? nHydrate.EFCore.DataAccess.IAuditable.ModifiedDate");
			sb.AppendLine("		{");
			if (_item.AllowModifiedAudit)
				sb.AppendLine("			get { return this." + _model.Database.ModifiedDatePascalName + "; }");
			else
				sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		byte[] nHydrate.EFCore.DataAccess.IAuditable.TimeStamp");
			sb.AppendLine("		{");
			if (_item.AllowTimestamp)
				sb.AppendLine("			get { return this." + _model.Database.TimestampPascalName + "; }");
			else
				sb.AppendLine("			get { return new byte[0]; }");
			sb.AppendLine("		}");
			sb.AppendLine();

			var auditMethodModifier = "virtual";
			if (_item.ParentTable != null)
				auditMethodModifier = "override";

			sb.AppendLine("		internal " + auditMethodModifier + " void ResetModifiedBy(string modifier)");
			sb.AppendLine("		{");
			if (_item.AllowModifiedAudit)
			{
				sb.AppendLine("			if (this." + _model.Database.ModifiedByPascalName + " != modifier)");
				sb.AppendLine("				this." + _model.Database.ModifiedByPascalName + " = modifier;");
			}
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		internal " + auditMethodModifier + " void ResetCreatedBy(string modifier)");
			sb.AppendLine("		{");
			if (_item.AllowCreateAudit)
			{
				sb.AppendLine("			if (this." + _model.Database.CreatedByPascalName + " != modifier)");
				sb.AppendLine("			this." + _model.Database.CreatedByPascalName + " = modifier;");
			}
			sb.AppendLine("			this.ResetModifiedBy(modifier);");
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendIEquatable()
		{
			sb.AppendLine("		#region Equals");
			sb.AppendLine("		bool System.IEquatable<" + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + ">.Equals(" + this.InterfaceProjectNamespace + ".Entity.I" + _item.PascalName + " other)");
			sb.AppendLine("		{");
			sb.AppendLine("			return this.Equals(other);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		public override bool Equals(object obj)");
			sb.AppendLine("		{");
			sb.AppendLine("			var other = obj as " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ";");
			sb.AppendLine("			if (other == null) return false;");
			sb.AppendLine("			return (");

			var allColumns = _item.GetColumnsFullHierarchy(true).Where(x => x.Generated);
			var index = 0;
			foreach (var column in allColumns)
			{
				sb.Append("				other." + column.PascalName + " == this." + column.PascalName);
				if (index < allColumns.Count() - 1) sb.Append(" &&");
				sb.AppendLine();
				index++;
			}

			sb.AppendLine("				);");
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendIDataErrorInfo()
		{
			sb.AppendLine("		#region IDataErrorInfo");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		string System.ComponentModel.IDataErrorInfo.Error");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.GetObjectDataErrorInfo(); }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"columnName\"></param>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		string System.ComponentModel.IDataErrorInfo.this[string columnName]");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				if (string.IsNullOrEmpty(columnName))");
			sb.AppendLine("					return string.Empty;");
			sb.AppendLine();
			sb.AppendLine("				var retval = GetObjectPropertyDataErrorInfo(columnName);");
			sb.AppendLine("				if (string.IsNullOrEmpty(retval))");
			sb.AppendLine("				{");

			//Validate all required fields
			sb.AppendLine("					switch (columnName.ToLower())");
			sb.AppendLine("					{");
			foreach (var column in _item.GeneratedColumns.Where(x => !x.AllowNull && x.IsTextType))
			{
				sb.AppendLine("						case \"" + column.PascalName.ToLower() + "\":");
				sb.AppendLine("							if (string.IsNullOrEmpty(this." + column.PascalName + ") || string.IsNullOrEmpty(this." + column.PascalName + ".Trim()))");
				sb.AppendLine("								retval = \"" + column.GetFriendlyName() + " is required!\";");
				sb.AppendLine("							break;");
			}
			sb.AppendLine("						default:");
			sb.AppendLine("							break;");
			sb.AppendLine("					}");

			//Validate all regular expression fields
			sb.AppendLine("					switch (columnName.ToLower())");
			sb.AppendLine("					{");
			foreach (var column in _item.GeneratedColumns.Where(x => !string.IsNullOrEmpty(x.ValidationExpression) && x.IsTextType))
			{
				sb.AppendLine("						case \"" + column.PascalName.ToLower() + "\":");
				sb.AppendLine("							if (!string.IsNullOrEmpty(this." + column.PascalName + ") && !System.Text.RegularExpressions.Regex.IsMatch(" + column.PascalName + ", @\"" + column.ValidationExpression.Replace("\"", @"""""") + "\", System.Text.RegularExpressions.RegexOptions.None))");
				sb.AppendLine("								retval = \"" + column.GetFriendlyName() + " is not the correct format!\";");
				sb.AppendLine("							break;");
			}
			sb.AppendLine("						default:");
			sb.AppendLine("							break;");
			sb.AppendLine("					}");

			sb.AppendLine("				}");
			sb.AppendLine("				return retval;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private string SetInitialValues()
		{
			//TODO - Audit Trail not implemented
			var setOriginalGuid = String.Format("\t\t\tthis.{0} = System.Guid.NewGuid();", _item.PrimaryKeyColumns.First().PascalName);

			var returnVal = new StringBuilder();
			if (_item.PrimaryKeyColumns.Count == 1 && ((Column)_item.PrimaryKeyColumns.First()).DataType == System.Data.SqlDbType.UniqueIdentifier)
			{
				if (_item.PrimaryKeyColumns.First().DataType == System.Data.SqlDbType.UniqueIdentifier)
					returnVal.AppendLine(setOriginalGuid);
			}

			//DEFAULT PROPERTIES START
			foreach (var column in _item.GeneratedColumns)
			{
				if ((column.Default != null) && (!string.IsNullOrEmpty(column.Default)))
				{
					var defaultValue = column.GetCodeDefault();

					//Write the actual code
					if (!string.IsNullOrEmpty(defaultValue))
						returnVal.AppendLine("			this." + column.PascalName + " = " + defaultValue + ";");
				}
			}
			//DEFAULT PROPERTIES END

			return returnVal.ToString();
		}

		#endregion

	}
}
