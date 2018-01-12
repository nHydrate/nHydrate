#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
using nHydrate.Generator.Models;
using System.Data;

namespace nHydrate.Generator.XSDSchema
{
	class XSDSchemaTemplate : BaseXSDSchemaTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();

		private string _rawFileName = string.Empty;
		private string _schemaName = string.Empty;
		private bool _droprelations = false;

		#region Constructors
		public XSDSchemaTemplate(ModelRoot model, string schemaName, string rawFileName, bool droprelations)
			: base(model)
		{
			_rawFileName = rawFileName;
			_schemaName = schemaName;
			_droprelations = droprelations;
		}
		#endregion

		#region BaseClassTemplate overrides
		public override string FileContent
		{
			get
			{
				this.GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get { return _rawFileName; }
		}

		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				var setName = "XSDSchemaDataset";
				if (!string.IsNullOrEmpty(_schemaName))
				{
					setName = _model.ModuleName + _schemaName;
				}

				sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				sb.AppendLine("<xs:schema id=\"" + setName + "\" targetNamespace=\"http://tempuri.org/" + _rawFileName + "\" xmlns:mstns=\"http://tempuri.org/" + _rawFileName + "\" xmlns=\"http://tempuri.org/" + _rawFileName + "\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\" xmlns:msprop=\"urn:schemas-microsoft-com:xml-msprop\" attributeFormDefault=\"qualified\" elementFormDefault=\"qualified\">");
				sb.AppendLine();
				sb.AppendLine("	<xs:element name=\"" + setName + "\" msdata:IsDataSet=\"true\" msdata:UseCurrentLocale=\"true\" msprop:Generator_DataSetName=\"" + setName + "\" msprop:Generator_UserDSName=\"" + setName + "\">");
				sb.AppendLine("		<xs:complexType>");
				sb.AppendLine();
				sb.AppendLine("			<xs:choice minOccurs=\"0\" maxOccurs=\"unbounded\">");
				sb.AppendLine();

				#region Tables
				foreach (var table in _model.Database.Tables.OrderBy(x => x.Name))
				{
					//sb.AppendLine("				<xs:element name=\"" + table.DatabaseName + "\" msprop:Generator_UserTableName=\"" + table.DatabaseName + "\" msprop:Generator_RowEvArgName=\"" + table.DatabaseName + "RowChangeEvent\" msprop:Generator_TableVarName=\"table" + table.DatabaseName + "\" msprop:Generator_TablePropName=\"" + table.DatabaseName + "\" msprop:Generator_RowDeletingName=\"" + table.DatabaseName + "RowDeleting\" msprop:Generator_RowChangingName=\"" + table.DatabaseName + "RowChanging\" msprop:Generator_RowDeletedName=\"" + table.DatabaseName + "RowDeleted\" msprop:Generator_TableClassName=\"" + table.DatabaseName + "DataTable\" msprop:Generator_RowChangedName=\"" + table.DatabaseName + "RowChanged\" msprop:Generator_RowEvHandlerName=\"" + table.DatabaseName + "RowChangeEventHandler\" msprop:Generator_RowClassName=\"" + table.DatabaseName + "Row\">");
					sb.AppendLine("				<xs:element name=\"" + table.DatabaseName.Replace(" ", "_x0020_") + "\" msprop:Generator_UserTableName=\"" + table.DatabaseName + "\">");
					sb.AppendLine("					<xs:complexType>");
					sb.AppendLine("						<xs:sequence>");

					#region Columns
					foreach (var column in table.GeneratedColumns)
					{
						sb.Append("							<xs:element name=\"" + column.DatabaseName + "\" ");

						if (column.Identity != IdentityTypeConstants.None || table.Immutable)
							sb.Append("msdata:ReadOnly=\"true\" ");

						if (column.Identity == IdentityTypeConstants.Database)
							sb.Append("msdata:AutoIncrement=\"true\" msdata:AutoIncrementSeed=\"1\" ");

						if (column.AllowNull)
							sb.Append("minOccurs=\"0\" ");

						sb.Append("msdata:Caption=\"" + column.PascalName + "\" ");

						sb.Append("msprop:Generator_ColumnVarNameInTable=\"column" + column.DatabaseName + "\" ");
						sb.Append("msprop:Generator_ColumnPropNameInRow=\"" + column.DatabaseName + "\" ");
						sb.Append("msprop:Generator_ColumnPropNameInTable=\"" + column.DatabaseName + "Column\" ");
						sb.Append("msprop:Generator_UserColumnName=\"" + column.DatabaseName + "\" ");

						//Do not emit type as this is in the SimpleType element below
						if (!column.IsTextType)
							sb.Append("type=\"" + GetSchemaDatatype(column.DataType) + "\" ");

						var msDataType = GetMSDatatype(column.DataType);
						if (!string.IsNullOrEmpty(msDataType))
							sb.Append("msdata:DataType=\"" + msDataType + "\" ");

						//Do not support Binary and Guid
						//Default must be a literal type
						if (!string.IsNullOrEmpty(column.Default) && 
							column.IsLiteralDefaultValue &&
							!column.IsBinaryType && 
							column.DataType != SqlDbType.UniqueIdentifier)
						{
							if (column.DataType == SqlDbType.Bit && column.Default == "0")
								sb.Append("default=\"false\" ");
							else if (column.DataType == SqlDbType.Bit && column.Default == "1")
								sb.Append("default=\"true\" ");
							else
								sb.Append("default=\"" + column.Default + "\" ");
						}

						sb.AppendLine(">");

						//Add length
						if (column.IsTextType)
						{
							sb.AppendLine("								<xs:simpleType>");
							sb.AppendLine("									<xs:restriction base=\"xs:string\">");
							if (column.IsMaxLength())
							{
								switch (column.DataType)
								{
									case SqlDbType.NText:
									case SqlDbType.NVarChar:
										sb.AppendLine("										<xs:maxLength value=\"1073741823\" />");
										break;
									default:
										sb.AppendLine("										<xs:maxLength value=\"2147483647\" />");
										break;
								}
							}
							else
							{
								sb.AppendLine("										<xs:maxLength value=\"" + column.Length + "\" />");
							}
							sb.AppendLine("									</xs:restriction>");
							sb.AppendLine("								</xs:simpleType>");
						}

						//line end
						sb.AppendLine("							</xs:element>");
					}
					#endregion

					sb.AppendLine("						</xs:sequence>");
					sb.AppendLine("					</xs:complexType>");
					sb.AppendLine("				</xs:element>");
					sb.AppendLine();
				}
				#endregion

				sb.AppendLine("			</xs:choice>");
				sb.AppendLine("		</xs:complexType>");

				#region Add unique constraints for singular PK
				foreach (var table in _model.Database.Tables.OrderBy(x => x.Name))
				{
					sb.AppendLine("		<xs:unique name=\"" + table.PascalName.Replace(" ", "_x0020_") + "_PK" + "\" msdata:ConstraintName=\"Constraint1\" msdata:PrimaryKey=\"true\">");
					sb.AppendLine("			<xs:selector xpath=\".//mstns:" + table.DatabaseName.Replace(" ", "_x0020_") + "\" />");
					foreach (var column in table.PrimaryKeyColumns)
					{
						sb.AppendLine("			<xs:field xpath=\"mstns:" + column.DatabaseName + "\" />");
					}
					sb.AppendLine("		</xs:unique>");
				}
				#endregion

				#region Relations
				if (!_droprelations)
				{
					foreach (var table in _model.Database.Tables.OrderBy(x => x.Name))
					{
						foreach (var relation in table.GetRelations().Where(x => x.Enforce).AsEnumerable())
						{
							sb.AppendLine("		<xs:keyref name=\"" + relation.PascalRoleName.Replace(" ", "_x0020_") + table.DatabaseName.Replace(" ", "_x0020_") + "_" + relation.ChildTable.DatabaseName.Replace(" ", "_x0020_") + "\" " +
								"refer=\"" + table.PascalName.Replace(" ", "_x0020_") + "_PK" + "\" msprop:rel_Generator_UserChildTable=\"" + relation.ChildTable.DatabaseName + "\" " +
								"msprop:rel_Generator_UserRelationName=\"" + relation.PascalRoleName.Replace(" ", "_x0020_") + table.DatabaseName.Replace(" ", "_x0020_") + "_" + relation.ChildTable.DatabaseName.Replace(" ", "_x0020_") + "\" " +
								"msprop:rel_Generator_UserParentTable=\"" + table.DatabaseName.Replace(" ", "_x0020_") + "\" " +
								"msdata:UpdateRule=\"None\" msdata:DeleteRule=\"None\">");

							sb.AppendLine("			<xs:selector xpath=\".//mstns:" + relation.ChildTable.DatabaseName.Replace(" ", "_x0020_") + "\" />");
							//foreach (var column in relation.ColumnRelationships.AsEnumerable())
							//{
							//  sb.AppendLine("			<xs:field xpath=\"mstns:" + column.ChildColumn.DatabaseName.Replace(" ", "_x0020_") + "\" />");
							//}

							foreach (var column in table.PrimaryKeyColumns)
							{
								var crelation = relation.ColumnRelationships.FirstOrDefault(x => x.ParentColumn == column);
								if (crelation != null)
								{
									sb.AppendLine("			<xs:field xpath=\"mstns:" + crelation.ChildColumn.DatabaseName.Replace(" ", "_x0020_") + "\" />");
								}
							}

							sb.AppendLine("		</xs:keyref>");
							sb.AppendLine();
						}
					}
				}
				#endregion

				sb.AppendLine("	</xs:element>");

				#region Relations
				//sb.AppendLine("	<xs:annotation>");
				//sb.AppendLine("		<xs:appinfo>");

				//foreach (var table in _model.Database.Tables.OrderBy(x => x.Name))
				//{
				//  foreach (var relation in table.GetRelations().AsEnumerable())
				//  {
				//    sb.AppendLine("			<msdata:Relationship " +
				//    "name=\"" + relation.PascalRoleName.Replace(" ", "_x0020_") + table.DatabaseName.Replace(" ", "_x0020_") + "_" + relation.ChildTable.DatabaseName.Replace(" ", "_x0020_") + "\" " +
				//    "msdata:parent=\"" + table.DatabaseName.Replace(" ", "_x0020_") + "\" " +
				//    "msdata:child=\"" + relation.ChildTable.DatabaseName.Replace(" ", "_x0020_") + "\" " +
				//    "msdata:parentkey=\"" + string.Join(" ", relation.ColumnRelationships.Select(x => x.ParentColumn.Name)) + "\" " +
				//    "msdata:childkey=\"" + string.Join(" ", relation.ColumnRelationships.Select(x => x.ChildColumn.Name)) + "\" " +
				//    "msprop:Generator_UserChildTable=\"" + relation.ChildTable.DatabaseName.Replace(" ", "_x0020_") + "\" " +
				//    "msprop:Generator_UserParentTable=\"" + table.DatabaseName.Replace(" ", "_x0020_") + "\" " +
				//    "msprop:Generator_UserRelationName=\"" + relation.PascalRoleName.Replace(" ", "_x0020_") + table.DatabaseName.Replace(" ", "_x0020_") + "_" + relation.ChildTable.DatabaseName.Replace(" ", "_x0020_") + "\" />");
				//  }
				//}

				//sb.AppendLine("		</xs:appinfo>");
				//sb.AppendLine("	</xs:annotation>");
				#endregion

				sb.AppendLine();
				sb.AppendLine("</xs:schema>");
				sb.AppendLine();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region namespace / objects

		private string GetMSDatatype(SqlDbType dataType)
		{
			switch (dataType)
			{
				case SqlDbType.UniqueIdentifier:
					return "System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
				default:
					return string.Empty;
			}
		}

		private string GetSchemaDatatype(SqlDbType dataType)
		{
			switch (dataType)
			{
				case SqlDbType.BigInt:
					return "xs:long";
				case SqlDbType.Binary:
					return "xs:base64Binary";
				case SqlDbType.Bit:
					return "xs:boolean";
				case SqlDbType.Char:
					return "xs:string";
				case SqlDbType.Date:
					return "xs:date";
				case SqlDbType.DateTime:
					return "xs:dateTime";
				case SqlDbType.DateTime2:
					return "xs:dateTime";
				case SqlDbType.Decimal:
					return "xs:decimal";
				case SqlDbType.Float:
					return "xs:decimal";
				case SqlDbType.Image:
					return "xs:base64Binary";
				case SqlDbType.Int:
					return "xs:int";
				case SqlDbType.Money:
					return "xs:decimal";
				case SqlDbType.NChar:
					return "xs:string";
				case SqlDbType.NText:
					return "xs:string";
				case SqlDbType.NVarChar:
					return "xs:string";
				case SqlDbType.Real:
					return "xs:float";
				case SqlDbType.SmallDateTime:
					return "xs:dateTime";
				case SqlDbType.SmallInt:
					return "xs:short";
				case SqlDbType.SmallMoney:
					return "xs:decimal";
				case SqlDbType.Text:
					return "xs:string";
				case SqlDbType.Time:
					return "xs:time";
				case SqlDbType.Timestamp:
					return "xs:base64Binary";
				case SqlDbType.TinyInt:
					return "xs:unsignedByte";
				case SqlDbType.UniqueIdentifier:
					return "xs:string";
				case SqlDbType.VarBinary:
					return "xs:base64Binary";
				case SqlDbType.VarChar:
					return "xs:string";
				case SqlDbType.Xml:
					return "xs:string";
				default:
					return string.Empty;
			}
		}

		#endregion

	}
}
