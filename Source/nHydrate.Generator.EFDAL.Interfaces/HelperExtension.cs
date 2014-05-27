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
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Interfaces
{
	public static class RelationExtension
	{
		public static string GetCodeFkName(this Relation relation)
		{
			var parentEntity = (Table)relation.ParentTableRef.Object;
			var childEntity = (Table)relation.ChildTableRef.Object;
			var fkName = string.Format("FK_{0}_{1}_{2}", relation.PascalRoleName, childEntity.PascalName, parentEntity.PascalName);
			return fkName;
		}

		public static string GetDatabaseFkName(this Relation relation)
		{
			return GetCodeFkName(relation);
			//Table parentEntity = (Table)relation.ParentTableRef.Object;
			//Table childEntity = (Table)relation.ChildTableRef.Object;
			//string fkName = string.Format("FK_{0}{1}{2}", relation.DatabaseRoleName, parentEntity.DatabaseName, childEntity.DatabaseName);
			//return fkName;
		}

		public static bool IsRequired(this Relation relation)
		{
			var retVal = false;
			foreach (ColumnRelationship columnRelation in relation.ColumnRelationships)
			{
				var childColumn = (Column)columnRelation.ChildColumnRef.Object;
				if (!childColumn.AllowNull)
				{
					retVal = true;
				}
			}

			return retVal;
		}

		public static string ParentMultiplicity(this Relation relation)
		{
			if (relation.IsOneToOne)
				return "1";
			else if (relation.IsRequired())
				return "1";
			else
				return "0..1";
		}

		public static string ChildMultiplicity(this Relation relation)
		{
			if (relation.IsOneToOne)
				return "0..1";
			else
				return "*";
		}
	}


	public static class ColumnExtension
	{
		public static bool IsForeignKey(this Column column)
		{
			var retVal = false;
			var parentTable = (Table)column.ParentTableRef.Object;
			if (parentTable.ChildRoleRelations.Count > 0)
			{
				var relationParentTable = parentTable.ChildRoleRelations[0].ParentTableRef.Object as Table;
				if (relationParentTable.Generated)
				{
					foreach (var childRelation in parentTable.ChildRoleRelations)
					{
						foreach (ColumnRelationship columnRelationship in childRelation.ColumnRelationships)
						{
							if (columnRelationship.ChildColumnRef.Object.Key == column.Key)
								retVal = true;
						}
					}
				}
			}
			return retVal;
		}

		public static bool IsForeignKeyIgnoreInheritance(this Column column)
		{
			var retVal = false;
			var table = (Table)column.ParentTableRef.Object;
			foreach (var childRelation in table.ChildRoleRelations)
			{
				var parentTable = childRelation.ParentTableRef.Object as Table;
				var childTable = childRelation.ChildTableRef.Object as Table;
				if (
					parentTable.Generated &&
					childTable.Generated &&
					!childTable.IsInheritedFrom(parentTable))
				{
					foreach (ColumnRelationship columnRelationship in childRelation.ColumnRelationships)
					{
						if (columnRelationship.ChildColumnRef.Object.Key == column.Key)
							retVal = true;
					}
				}
			}
			return retVal;
		}

		public static string EFGetDatabaseMaxLengthString(this Column column)
		{
			string retVal = null;
			if (column.DataType == System.Data.SqlDbType.Binary ||
				column.DataType == System.Data.SqlDbType.Char ||
				column.DataType == System.Data.SqlDbType.NChar ||
				column.DataType == System.Data.SqlDbType.NVarChar ||
				column.DataType == System.Data.SqlDbType.VarBinary ||
				column.DataType == System.Data.SqlDbType.VarChar )
			{
				if (column.GetLengthString().ToLower() != "max")
					retVal = column.GetLengthString();
			}
			return retVal;
		}

		public static string EFDatabaseType(this Column column)
		{
			return column.EFDatabaseType(true);
		}

		public static string EFDatabaseType(this Column column, bool appendMax)
		{
			string retVal;
			retVal = column.DataType.ToString().ToLower();
			if (column.GetLengthString().ToLower() == "max" && appendMax)
			{
				retVal += "(max)";
			}
			return retVal;
		}

		public static bool EFSupportsPrecision(this Column column)
		{
			var retVal = false;
			if (column.DataType == System.Data.SqlDbType.Decimal ||
				column.DataType == System.Data.SqlDbType.Money ||
				column.DataType == System.Data.SqlDbType.SmallMoney)
				retVal = true;
			return retVal;
		}

		public static string EFGetCodeMaxLengthString(this Column column)
		{
			string retVal = null;
			if (column.DataType == System.Data.SqlDbType.Binary ||
				column.DataType == System.Data.SqlDbType.Char ||
				column.DataType == System.Data.SqlDbType.NChar ||
				column.DataType == System.Data.SqlDbType.NVarChar ||
				column.DataType == System.Data.SqlDbType.Timestamp ||
				column.DataType == System.Data.SqlDbType.VarBinary ||
				column.DataType == System.Data.SqlDbType.VarChar ||
				column.DataType == System.Data.SqlDbType.Xml)
			{
				retVal = column.GetLengthString();
				if (retVal.ToLower() == "max")
					retVal = "Max";
			}
			else if (column.DataType == System.Data.SqlDbType.Image ||
				column.DataType == System.Data.SqlDbType.NText ||
				column.DataType == System.Data.SqlDbType.Text)
			{
				retVal = "Max";
			}
			return retVal;
		}

		public static bool? EFUnicode(this Column column)
		{
			bool? retVal = null;
			if (column.DataType == System.Data.SqlDbType.NChar ||
				column.DataType == System.Data.SqlDbType.NText ||
				column.DataType == System.Data.SqlDbType.NVarChar ||
				column.DataType == System.Data.SqlDbType.Xml)
			{
				retVal = true;
			}
			else if (column.DataType == System.Data.SqlDbType.Char ||
				column.DataType == System.Data.SqlDbType.Text ||
				column.DataType == System.Data.SqlDbType.VarChar
			)
			{
				retVal = false;
			}
			return retVal;
		}

		public static string EFCodeType(this Column column)
		{
			string retVal;
			switch (column.DataType)
			{
				case System.Data.SqlDbType.BigInt: retVal = "Int64"; break;
				case System.Data.SqlDbType.Binary: retVal = "Binary"; break;
				case System.Data.SqlDbType.Bit: retVal = "Boolean"; break;
				case System.Data.SqlDbType.Char: retVal = "String"; break;
				case System.Data.SqlDbType.Date: retVal = "DateTime"; break;
				case System.Data.SqlDbType.DateTime: retVal = "DateTime"; break;
				case System.Data.SqlDbType.DateTime2: retVal = "DateTime"; break;
				case System.Data.SqlDbType.DateTimeOffset: retVal = "DateTimeOffset"; break;
				case System.Data.SqlDbType.Decimal: retVal = "Decimal"; break;
				case System.Data.SqlDbType.Float: retVal = "Double"; break;
				case System.Data.SqlDbType.Image: retVal = "Binary"; break;
				case System.Data.SqlDbType.Int: retVal = "Int32"; break;
				case System.Data.SqlDbType.Money: retVal = "Decimal"; break;
				case System.Data.SqlDbType.NChar: retVal = "String"; break;
				case System.Data.SqlDbType.NText: retVal = "String"; break;
				case System.Data.SqlDbType.NVarChar: retVal = "String"; break;
				case System.Data.SqlDbType.Real: retVal = "Single"; break;
				case System.Data.SqlDbType.SmallDateTime: retVal = "DateTime"; break;
				case System.Data.SqlDbType.SmallInt: retVal = "Int16"; break;
				case System.Data.SqlDbType.SmallMoney: retVal = "Decimal"; break;
				case System.Data.SqlDbType.Text: retVal = "String"; break;
				case System.Data.SqlDbType.Time: retVal = "Time"; break;
				case System.Data.SqlDbType.Timestamp: retVal = "Binary"; break;
				case System.Data.SqlDbType.TinyInt: retVal = "Byte"; break;
				case System.Data.SqlDbType.UniqueIdentifier: retVal = "Guid"; break;
				case System.Data.SqlDbType.VarBinary: retVal = "Binary"; break;
				case System.Data.SqlDbType.VarChar: retVal = "String"; break;
				case System.Data.SqlDbType.Xml: retVal = "String"; break;
				default: retVal = "String"; break;
			}
			return retVal;
		}

		public static string EFStoreGeneratedPattern(this Column column)
		{
			string retVal = null;
			if (column.Identity == IdentityTypeConstants.Database)
				retVal = "Identity";
			if (column.DataType == System.Data.SqlDbType.Timestamp)
				retVal = "Computed";
			return retVal;
		}

		public static bool? EFIsFixedLength(this Column column)
		{
			bool? retVal = null;
			if (column.DataType == System.Data.SqlDbType.Binary ||
				column.DataType == System.Data.SqlDbType.Char ||
				column.DataType == System.Data.SqlDbType.NChar ||
				column.DataType == System.Data.SqlDbType.Timestamp)
			{
				retVal = true;
			}
			else if (column.DataType == System.Data.SqlDbType.Image ||
				column.DataType == System.Data.SqlDbType.NText ||
				column.DataType == System.Data.SqlDbType.NVarChar ||
				column.DataType == System.Data.SqlDbType.Text ||
				column.DataType == System.Data.SqlDbType.VarBinary ||
				column.DataType == System.Data.SqlDbType.VarChar ||
				column.DataType == System.Data.SqlDbType.Xml)
			{
				retVal = false;
			}
			return retVal;
		}
	}
}

