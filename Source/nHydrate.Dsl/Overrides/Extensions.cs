#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;

namespace nHydrate.Dsl
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns the field mappings for a relation
        /// </summary>
        public static IEnumerable<RelationField> FieldMapList(this EntityHasEntities item)
        {
            return item.ParentEntity.nHydrateModel.RelationFields
                       .Where(x => x.RelationID == item.Id)
                       .ToList();
        }

        /// <summary>
        /// Returns the field mappings for a relation
        /// </summary>
        public static IEnumerable<RelationField> FieldMapList(this EntityHasViews item)
        {
            return item.ParentEntity.nHydrateModel.RelationFields
                       .Where(x => x.RelationID == item.Id)
                       .ToList();
        }

        public static bool VariableLengthType(this Field field)
        {
            switch (field.DataType)
            {
                case DataTypeConstants.BigInt:
                    return false;
                case DataTypeConstants.Binary:
                    return true;
                case DataTypeConstants.Bit:
                    return false;
                case DataTypeConstants.Char:
                    return true;
                case DataTypeConstants.DateTime:
                    return false;
                case DataTypeConstants.Decimal:
                    return true;
                case DataTypeConstants.Float:
                    return false;
                case DataTypeConstants.Image:
                    return false;
                case DataTypeConstants.Int:
                    return false;
                case DataTypeConstants.Money:
                    return false;
                case DataTypeConstants.NChar:
                    return true;
                case DataTypeConstants.NText:
                    return false;
                case DataTypeConstants.NVarChar:
                    return true;
                case DataTypeConstants.Real:
                    return false;
                case DataTypeConstants.SmallDateTime:
                    return false;
                case DataTypeConstants.SmallInt:
                    return false;
                case DataTypeConstants.SmallMoney:
                    return false;
                case DataTypeConstants.Text:
                    return false;
                case DataTypeConstants.Timestamp:
                    return false;
                case DataTypeConstants.TinyInt:
                    return false;
                case DataTypeConstants.Udt:
                    return false;
                case DataTypeConstants.UniqueIdentifier:
                    return false;
                case DataTypeConstants.VarBinary:
                    return true;
                case DataTypeConstants.VarChar:
                    return true;
                case DataTypeConstants.Variant:
                    return false;
                case DataTypeConstants.Xml:
                    return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the data type is a database character type of Binary,Image,VarBinary
        /// </summary>
        public static bool IsBinaryType(this Field field)
        {
            switch (field.DataType)
            {
                case DataTypeConstants.Binary:
                case DataTypeConstants.Image:
                case DataTypeConstants.VarBinary:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the data type is a database character type of Char,NChar,NText,NVarChar,Text,VarChar,Xml
        /// </summary>
        public static bool IsTextType(this Field field)
        {
            switch (field.DataType)
            {
                case DataTypeConstants.Char:
                case DataTypeConstants.NChar:
                case DataTypeConstants.NText:
                case DataTypeConstants.NVarChar:
                case DataTypeConstants.Text:
                case DataTypeConstants.VarChar:
                case DataTypeConstants.Xml:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the data type is a database character type of DateTime,DateTime2,DateTimeOffset,SmallDateTime
        /// </summary>
        public static bool IsDateType(this Field field)
        {
            switch (field.DataType)
            {
                case DataTypeConstants.DateTime:
                case DataTypeConstants.DateTime2:
                case DataTypeConstants.DateTimeOffset:
                case DataTypeConstants.SmallDateTime:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the type is a number wither floating point or integer
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(this Field field)
        {
            return field.IsDecimalType() || field.IsIntegerType();
        }

        /// <summary>
        /// Determines if the data type is a database character type of Decimal,Float,Real
        /// </summary>
        public static bool IsDecimalType(this Field field)
        {
            switch (field.DataType)
            {
                case DataTypeConstants.Decimal:
                case DataTypeConstants.Float:
                case DataTypeConstants.Real:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the data type is a database character type of Int,BigInt,TinyInt,SmallInt
        /// </summary>
        public static bool IsIntegerType(this Field field)
        {
            switch (field.DataType)
            {
                case DataTypeConstants.BigInt:
                case DataTypeConstants.Int:
                case DataTypeConstants.TinyInt:
                case DataTypeConstants.SmallInt:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if this type supports the MAX syntax in SQL 2008
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool SupportsMax(this DataTypeConstants type)
        {
            switch (type)
            {
                case DataTypeConstants.VarChar:
                case DataTypeConstants.NVarChar:
                case DataTypeConstants.VarBinary:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Verifies that length does not exceed the maximum length value for the specified variable length data type
        /// Predefined length like integers are not processed
        /// </summary>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <returns>The lesser of the values maximum length and the specified length</returns>
        public static int ValidateDataTypeMax(this DataTypeConstants type, int length)
        {
            switch (type)
            {
                case DataTypeConstants.Char:
                    return (length > 8000) ? 8000 : length;
                case DataTypeConstants.VarChar:
                    return (length > 8000) ? 8000 : length;
                case DataTypeConstants.NChar:
                    return (length > 4000) ? 4000 : length;
                case DataTypeConstants.NVarChar:
                    return (length > 4000) ? 4000 : length;
                case DataTypeConstants.Decimal:
                    return (length > 38) ? 38 : length;
                case DataTypeConstants.DateTime2:
                    return (length > 7) ? 7 : length;
                case DataTypeConstants.Binary:
                case DataTypeConstants.VarBinary:
                    return (length > 8000) ? 8000 : length;
            }
            return length;
        }

        /// <summary>
        /// Gets the scale of the data type (decimals only)
        /// </summary>
        public static int GetPredefinedScale(this DataTypeConstants type)
        {
            //Returns -1 if variable
            switch (type)
            {
                case DataTypeConstants.Decimal:
                    return -1;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Gets the size of the data type
        /// </summary>
        /// <returns></returns>
        /// <remarks>Returns -1 for variable types and 1 for blob fields</remarks>
        public static int GetPredefinedSize(this DataTypeConstants type)
        {
            //Returns -1 if variable
            switch (type)
            {
                case DataTypeConstants.BigInt:
                    return 8;
                case DataTypeConstants.Bit:
                    return 1;
                case DataTypeConstants.DateTime:
                    return 8;
                case DataTypeConstants.Date:
                    return 3;
                case DataTypeConstants.Time:
                    return 5;
                case DataTypeConstants.DateTimeOffset:
                    return 10;
                case DataTypeConstants.Float:
                    return 8;
                case DataTypeConstants.Int:
                    return 4;
                case DataTypeConstants.Money:
                    return 8;
                case DataTypeConstants.Real:
                    return 4;
                case DataTypeConstants.SmallDateTime:
                    return 4;
                case DataTypeConstants.SmallInt:
                    return 2;
                case DataTypeConstants.SmallMoney:
                    return 4;
                case DataTypeConstants.Timestamp:
                    return 8;
                case DataTypeConstants.TinyInt:
                    return 1;
                case DataTypeConstants.UniqueIdentifier:
                    return 16;

                case DataTypeConstants.Image:
                    return 1;
                case DataTypeConstants.Text:
                case DataTypeConstants.NText:
                    return 1;
                case DataTypeConstants.Xml:
                    return 1;

                default:
                    return -1;
            }
        }

        public static bool DefaultIsString(this Field field)
        {
            switch (field.DataType)
            {
                case DataTypeConstants.BigInt:
                    return false;
                case DataTypeConstants.Binary:
                    return true;
                case DataTypeConstants.Bit:
                    return false;
                case DataTypeConstants.Char:
                    return true;
                case DataTypeConstants.DateTime:
                    return false;
                case DataTypeConstants.Decimal:
                    return false;
                case DataTypeConstants.Float:
                    return false;
                case DataTypeConstants.Image:
                    return true;
                case DataTypeConstants.Int:
                    return false;
                case DataTypeConstants.Money:
                    return false;
                case DataTypeConstants.NChar:
                    return true;
                case DataTypeConstants.NText:
                    return true;
                case DataTypeConstants.NVarChar:
                    return true;
                case DataTypeConstants.Real:
                    return false;
                case DataTypeConstants.SmallDateTime:
                    return false;
                case DataTypeConstants.SmallInt:
                    return false;
                case DataTypeConstants.SmallMoney:
                    return false;
                case DataTypeConstants.Text:
                    return true;
                case DataTypeConstants.Timestamp:
                    return false;
                case DataTypeConstants.TinyInt:
                    return false;
                case DataTypeConstants.Udt:
                    return false;
                case DataTypeConstants.UniqueIdentifier:
                    return true;
                case DataTypeConstants.VarBinary:
                    return true;
                case DataTypeConstants.VarChar:
                    return true;
                case DataTypeConstants.Variant:
                    return true;
                case DataTypeConstants.Xml:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the nullable type in code is native like 'string' or made up of a composite type like 'decimal?'
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CodeNullableTypeIsNative(this Field field)
        {
            switch (field.DataType)
            {
                case DataTypeConstants.BigInt:
                    return false;
                case DataTypeConstants.Binary:
                    return false;
                case DataTypeConstants.Bit:
                    return false;
                case DataTypeConstants.Char:
                    return true;
                case DataTypeConstants.DateTime:
                    return false;
                case DataTypeConstants.Decimal:
                    return false;
                case DataTypeConstants.Float:
                    return false;
                case DataTypeConstants.Image:
                    return false;
                case DataTypeConstants.Int:
                    return false;
                case DataTypeConstants.Money:
                    return false;
                case DataTypeConstants.NChar:
                    return true;
                case DataTypeConstants.NText:
                    return true;
                case DataTypeConstants.NVarChar:
                    return true;
                case DataTypeConstants.Real:
                    return false;
                case DataTypeConstants.SmallDateTime:
                    return false;
                case DataTypeConstants.SmallInt:
                    return false;
                case DataTypeConstants.SmallMoney:
                    return false;
                case DataTypeConstants.Text:
                    return true;
                case DataTypeConstants.Timestamp:
                    return false;
                case DataTypeConstants.TinyInt:
                    return false;
                case DataTypeConstants.Udt:
                    return false;
                case DataTypeConstants.UniqueIdentifier:
                    return false;
                case DataTypeConstants.VarBinary:
                    return false;
                case DataTypeConstants.VarChar:
                    return true;
                case DataTypeConstants.Variant:
                    return false;
                case DataTypeConstants.Xml:
                    return true;
                default:
                    return false;
            }
        }

        public static string DefaultValueCodeString(this Field field)
        {
            switch (field.DataType)
            {

                case DataTypeConstants.BigInt:
                    return "long.MinValue";
                case DataTypeConstants.Binary:
                    return "new System.Byte[]{}";
                case DataTypeConstants.Bit:
                    return "false";
                case DataTypeConstants.Char:
                    return "string.Empty";
                case DataTypeConstants.DateTime:
                    return "DateTime.MinValue";
                case DataTypeConstants.Decimal:
                    return "0.0m";
                case DataTypeConstants.Float:
                    return "0.0f";
                case DataTypeConstants.Image:
                    return "new System.Byte[]{}";
                case DataTypeConstants.Int:
                    return "int.MinValue";
                case DataTypeConstants.Money:
                    return "0.00m";
                case DataTypeConstants.NChar:
                    return "string.Empty";
                case DataTypeConstants.NText:
                    return "string.Empty";
                case DataTypeConstants.NVarChar:
                    return "string.Empty";
                case DataTypeConstants.Real:
                    return "System.Single.MinValue";
                case DataTypeConstants.SmallDateTime:
                    return "DateTime.MinValue";
                case DataTypeConstants.SmallInt:
                    return "0";
                case DataTypeConstants.SmallMoney:
                    return "0.00m";
                case DataTypeConstants.Text:
                    return "string.Empty";
                case DataTypeConstants.Timestamp:
                    return "new System.Byte[]{}";
                case DataTypeConstants.TinyInt:
                    return "0";
                case DataTypeConstants.Udt:
                    return "string.Empty";
                case DataTypeConstants.UniqueIdentifier:
                    return "Guid.NewGuid()";
                case DataTypeConstants.VarBinary:
                    return "new System.Byte[]{}";
                case DataTypeConstants.VarChar:
                    return "string.Empty";
                case DataTypeConstants.Variant:
                    return "string.Empty";
                case DataTypeConstants.Xml:
                    return "string.Empty";
                default:
                    return "string.Empty";
            }
        }

        /// <summary>
        /// Determines if this field type can be made into a range query
        /// </summary>
        public static bool IsRangeType(this Field field)
        {
            switch (field.DataType)
            {
                case DataTypeConstants.BigInt:
                //case DataTypeConstants.Char:
                case DataTypeConstants.Date:
                case DataTypeConstants.DateTime:
                case DataTypeConstants.DateTime2:
                case DataTypeConstants.Decimal:
                case DataTypeConstants.Float:
                case DataTypeConstants.Int:
                case DataTypeConstants.Money:
                //case DataTypeConstants.NChar:
                //case DataTypeConstants.NVarChar:
                case DataTypeConstants.Real:
                case DataTypeConstants.SmallDateTime:
                case DataTypeConstants.SmallInt:
                case DataTypeConstants.SmallMoney:
                case DataTypeConstants.Time:
                case DataTypeConstants.TinyInt:
                    //case DataTypeConstants.VarChar:
                    return true;
            }
            return false;
        }

        public static Field GetSourceField(this RelationField relationField, EntityHasEntities relation)
        {
            return relation.ParentEntity.Fields.FirstOrDefault(x => x.Id == relationField.SourceFieldId);
        }

        public static Field GetTargetField(this RelationField relationField, EntityHasEntities relation)
        {
            return relation.ChildEntity.Fields.FirstOrDefault(x => x.Id == relationField.TargetFieldId);
        }

        public static Field GetSourceField(this RelationField relationField, EntityHasViews relation)
        {
            return relation.ParentEntity.Fields.FirstOrDefault(x => x.Id == relationField.SourceFieldId);
        }

        public static ViewField GetTargetField(this RelationField relationField, EntityHasViews relation)
        {
            return relation.ChildView.Fields.FirstOrDefault(x => x.Id == relationField.TargetFieldId);
        }

        /// <summary>
        /// Determine if the specified type is supported
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSupportedType(this DataTypeConstants type, DatabaseTypeConstants sqlVersion)
        {
            if (sqlVersion == DatabaseTypeConstants.SQL2005)
            {
                switch (type)
                {
                    //case DataTypeConstants.Xml:
                    case DataTypeConstants.Udt:
                    case DataTypeConstants.Structured:
                    case DataTypeConstants.Variant:
                    case DataTypeConstants.DateTimeOffset:
                    case DataTypeConstants.DateTime2:
                    case DataTypeConstants.Time:
                    case DataTypeConstants.Date:
                        return false;
                    default:
                        return true;
                }
            }
            else if ((sqlVersion == DatabaseTypeConstants.SQL2008) || (sqlVersion == DatabaseTypeConstants.SQLAzure))
            {
                switch (type)
                {
                    //case DataTypeConstants.Xml:
                    case DataTypeConstants.Udt:
                    case DataTypeConstants.Structured:
                    case DataTypeConstants.Variant:
                        //case DataTypeConstants.DateTimeOffset:
                        //case DataTypeConstants.DateTime2:
                        //case DataTypeConstants.Time:
                        //case DataTypeConstants.Date:
                        return false;
                    default:
                        return true;
                }
            }
            else
            {
                return false;
            }

        }

        public static double ToDouble(this string s)
        {
            double d;
            if (double.TryParse(s, out d))
                return d;
            else
                return double.NaN;
        }

        public static bool IsHex(this string s)
        {
            try
            {
                int i;
                return Int32.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out i);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Converts a string in the format of '0x123F' to '0x12, 0x3F'
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertToHexArrayString(this string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            s = s.Replace("0x", string.Empty);
            if (s.Length % 2 != 0) return string.Empty;

            var l = new List<string>();
            for (var ii = 0; ii < s.Length / 2; ii++)
            {
                l.Add("0x" + s.Substring(ii * 2, 2));
            }
            return string.Join(", ", l.ToArray());
        }

        public static int Remove<T>(this IList<T> list, Func<T, bool> where)
        {
            var l = list.Where(where).ToList().ToList();
            foreach (var n in l)
            {
                list.Remove(n);
            }
            return l.Count;
        }

        public static int RemoveAll<T>(this IList<T> list, IList<T> removalList)
        {
            var count = 0;
            foreach (var n in removalList)
            {
                if (list.Remove(n))
                    count++;
            }
            return count;
        }

        #region Modules

        /// <summary>
        /// Determines if an module rule passes for the current model
        /// </summary>
        /// <param name="module"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static bool IsValidRule(this Module module, ModuleRule rule, ref List<string> issueList)
        {
            if (module == null || rule == null)
                return false;

            var dModule = rule.GetDependentModuleObject();
            if (dModule == null)
                return false;

            if (rule.Status == ModuleRuleStatusTypeConstants.Outerset)
            {
                #region Outerset
                //Verify that the current module is an outerset of the dependent module
                var isValid = true;

                //Tables, there should be no objects in common
                if (((int)rule.Inclusion & (int)ModuleRuleInclusionTypeConstants.Entity) != 0)
                {
                    var g1 = dModule.GetEntities().ToList();
                    var g2 = module.GetEntities().ToList();
                    var indexListInModule1 = module.nHydrateModel.IndexModules.Where(z => z.ModuleId == dModule.Id).Select(x => x.IndexID);
                    var indexListInModule2 = module.nHydrateModel.IndexModules.Where(z => z.ModuleId == module.Id).Select(x => x.IndexID);
                    var i1 = g1.SelectMany(x => x.Indexes).ToList().Where(x => indexListInModule1.Contains(x.Id)).ToList();
                    var i2 = g2.SelectMany(x => x.Indexes).ToList().Where(x => indexListInModule2.Contains(x.Id)).ToList();
                    var intersectGroup = g1.Intersect(g2).ToList();
                    var intersectGroup2 = i1.Intersect(i2).ToList();
                    isValid &= intersectGroup.Count == 0;
                    isValid &= intersectGroup2.Count == 0;
                    if (intersectGroup.Count < g2.Count)
                    {
                        issueList.Add("The following entities are in both modules " + dModule.Name + " and " + module.Name + ":" + string.Join(", ", intersectGroup.Select(x => x.Name).ToList()));
                    }
                    if (intersectGroup2.Count < i2.Count)
                    {
                        issueList.Add("The following indexes are in both modules " + dModule.Name + " and " + module.Name + ":" + string.Join(", ", intersectGroup2.Select(x => x.Entity.Name + ".[" + x.ToString() + "]").ToList()));
                    }
                }

                //Fields
                if (isValid && ((int)rule.Inclusion & (int)ModuleRuleInclusionTypeConstants.Entity) != 0)
                {
                    var g1 = dModule.GetFields().ToList();
                    var g2 = module.GetFields().ToList();
                    var intersectGroup = g1.Intersect(g2).ToList();
                    isValid &= intersectGroup.Count == 0;
                    if (intersectGroup.Count < g2.Count)
                    {
                        issueList.Add("The following fields are in both modules " + dModule.Name + " and " + module.Name + ":" + string.Join(", ", intersectGroup.Select(x => x.Entity.Name + "." + x.Name).ToList()));
                    }
                }

                //Stored Procedures
                if (((int)rule.Inclusion & (int)ModuleRuleInclusionTypeConstants.StoredProcedure) != 0)
                {
                    var g1 = dModule.GetStoredProcedures().ToList();
                    var g2 = module.GetStoredProcedures().ToList();
                    var intersectGroup = g1.Intersect(g2).ToList();
                    isValid &= intersectGroup.Count == 0;
                    if (intersectGroup.Count < g2.Count)
                    {
                        issueList.Add("The following stored procedures are in both modules " + dModule.Name + " and " + module.Name + ":" + string.Join(", ", intersectGroup.Select(x => x.Name).ToList()));
                    }
                }

                //Views
                if (((int)rule.Inclusion & (int)ModuleRuleInclusionTypeConstants.View) != 0)
                {
                    var g1 = dModule.GetViews().ToList();
                    var g2 = module.GetViews().ToList();
                    var intersectGroup = g1.Intersect(g2).ToList();
                    isValid &= intersectGroup.Count == 0;
                    if (intersectGroup.Count < g2.Count)
                    {
                        issueList.Add("The following views are in both modules " + dModule.Name + " and " + module.Name + ":" + string.Join(", ", intersectGroup.Select(x => x.Name).ToList()));
                    }
                }

                //Functions
                if (((int)rule.Inclusion & (int)ModuleRuleInclusionTypeConstants.Function) != 0)
                {
                    var g1 = dModule.GetFunctions().ToList();
                    var g2 = module.GetFunctions().ToList();
                    var intersectGroup = g1.Intersect(g2).ToList();
                    isValid &= intersectGroup.Count == 0;
                    if (intersectGroup.Count < g2.Count)
                    {
                        issueList.Add("The following views are in both modules " + dModule.Name + " and " + module.Name + ":" + string.Join(", ", intersectGroup.Select(x => x.Name).ToList()));
                    }
                }

                return isValid;
                #endregion
            }
            else if (rule.Status == ModuleRuleStatusTypeConstants.Subset)
            {
                #region Subset

                //Verify that the current module is a subset of the dependent module
                var isValid = true;

                //Tables, the intersection must be the same size as the module's number of objects
                if (((int)rule.Inclusion & (int)ModuleRuleInclusionTypeConstants.Entity) != 0)
                {
                    var g1 = dModule.GetEntities().ToList();
                    var g2 = module.GetEntities().ToList();
                    var indexListInModule1 = module.nHydrateModel.IndexModules.Where(z => z.ModuleId == dModule.Id).Select(x => x.IndexID);
                    var indexListInModule2 = module.nHydrateModel.IndexModules.Where(z => z.ModuleId == module.Id).Select(x => x.IndexID);
                    var i1 = g1.SelectMany(x => x.Indexes).ToList().Where(x => indexListInModule1.Contains(x.Id)).ToList();
                    var i2 = g2.SelectMany(x => x.Indexes).ToList().Where(x => indexListInModule2.Contains(x.Id)).ToList();
                    var intersectGroup = g1.Intersect(g2).ToList();
                    var intersectGroup2 = i1.Intersect(i2).ToList();
                    isValid &= intersectGroup.Count == g2.Count;
                    isValid &= intersectGroup2.Count == i2.Count;
                    if (intersectGroup.Count < g2.Count)
                    {
                        var missing = g2.Except(intersectGroup).ToList();
                        issueList.Add("The following entities are missing from the " + dModule.Name + " module: " + string.Join(", ", missing.Select(x => x.Name).ToList()));
                    }
                    if (intersectGroup2.Count < i2.Count)
                    {
                        var missing = i2.Except(intersectGroup2).ToList();
                        issueList.Add("The following indexes are missing from the " + dModule.Name + " module: " + string.Join(", ", missing.Select(x => x.Entity.Name + ".[" + x.ToString() + "]").ToList()));
                    }
                }

                //Fields
                if (isValid && ((int)rule.Inclusion & (int)ModuleRuleInclusionTypeConstants.Entity) != 0)
                {
                    var g1 = dModule.GetFields().ToList();
                    var g2 = module.GetFields().ToList();
                    var intersectGroup = g1.Intersect(g2).ToList();
                    isValid &= intersectGroup.Count == g2.Count;
                    if (intersectGroup.Count < g2.Count)
                    {
                        var missing = g2.Except(intersectGroup).ToList();
                        issueList.Add("The following fields are missing from the " + dModule.Name + " module: " + string.Join(", ", missing.Select(x => x.Entity.Name + "." + x.Name).ToList()));
                    }
                }

                //Stored Procedures
                if (((int)rule.Inclusion & (int)ModuleRuleInclusionTypeConstants.StoredProcedure) != 0)
                {
                    var g1 = dModule.GetStoredProcedures().ToList();
                    var g2 = module.GetStoredProcedures().ToList();
                    var intersectGroup = g1.Intersect(g2).ToList();
                    isValid &= intersectGroup.Count == g2.Count;
                    if (intersectGroup.Count < g2.Count)
                    {
                        var missing = g2.Except(intersectGroup).ToList();
                        issueList.Add("The following stored procedures are missing from the " + dModule.Name + " module: " + string.Join(", ", missing.Select(x => x.Name).ToList()));
                    }
                }

                //Views
                if (((int)rule.Inclusion & (int)ModuleRuleInclusionTypeConstants.View) != 0)
                {
                    var g1 = dModule.GetViews().ToList();
                    var g2 = module.GetViews().ToList();
                    var intersectGroup = g1.Intersect(g2).ToList();
                    isValid &= intersectGroup.Count == g2.Count;
                    if (intersectGroup.Count < g2.Count)
                    {
                        var missing = g2.Except(intersectGroup).ToList();
                        issueList.Add("The following views are missing from the " + dModule.Name + " module: " + string.Join(", ", missing.Select(x => x.Name).ToList()));
                    }
                }

                //Functions
                if (((int)rule.Inclusion & (int)ModuleRuleInclusionTypeConstants.Function) != 0)
                {
                    var g1 = dModule.GetFunctions().ToList();
                    var g2 = module.GetFunctions().ToList();
                    var intersectGroup = g1.Intersect(g2).ToList();
                    isValid &= intersectGroup.Count == g2.Count;
                    if (intersectGroup.Count < g2.Count)
                    {
                        var missing = g2.Except(intersectGroup).ToList();
                        issueList.Add("The following functions are missing from the " + dModule.Name + " module: " + string.Join(", ", missing.Select(x => x.Name).ToList()));
                    }
                }

                return isValid;

                #endregion

            }

            return false;
        }

        public static Module GetDependentModuleObject(this ModuleRule rule)
        {
            if (rule == null) return null;
            return rule.Module.nHydrateModel.Modules.FirstOrDefault(x => x.Id == rule.DependentModule);
        }

        public static IList<Entity> GetEntities(this Module module)
        {
            return module.nHydrateModel.Entities.Where(x => x.Modules.Contains(module)).ToList();
        }

        public static IList<Field> GetFields(this Module module)
        {
            return module.GetEntities().SelectMany(x => x.Fields).Where(x => x.Modules.Contains(module)).ToList();
        }

        public static IList<StoredProcedure> GetStoredProcedures(this Module module)
        {
            return module.nHydrateModel.StoredProcedures.Where(x => x.Modules.Contains(module)).ToList();
        }

        public static IList<View> GetViews(this Module module)
        {
            return module.nHydrateModel.Views.Where(x => x.Modules.Contains(module)).ToList();
        }

        public static IList<Function> GetFunctions(this Module module)
        {
            return module.nHydrateModel.Functions.Where(x => x.Modules.Contains(module)).ToList();
        }

        #endregion

        /// <summary>
        /// Given a datatype and length, it will return a display string for this datatype
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetLengthString(this DataTypeConstants dataType, int length)
        {
            if (dataType.SupportsMax() && length == 0)
                return "max";
            else
                return length.ToString();
        }

        /// <summary>
        /// Gets the SQL Server type mapping for this data type
        /// </summary>
        public static string GetSQLDefaultType(this DataTypeConstants dataType, int length, int scale)
        {
            return dataType.GetSQLDefaultType(false, length, scale);
        }

        /// <summary>
        /// Gets the SQL Server type mapping for this data type
        /// </summary>
        /// <param name="isRaw">Determines if the square brackets '[]' are around the type</param>
        /// <returns>The SQL ready datatype like '[Int]' or '[Varchar] (100)'</returns>
        public static string GetSQLDefaultType(this DataTypeConstants dataType, bool isRaw, int length, int scale)
        {
            var retval = string.Empty;

            if (!isRaw) retval += "[";
            retval += dataType.ToString();
            if (!isRaw) retval += "]";

            if (dataType == DataTypeConstants.Variant)
            {
                retval = string.Empty;
                if (!isRaw) retval += "[";
                retval += "sql_variant";
                if (!isRaw) retval += "]";
            }
            else if (dataType == DataTypeConstants.Binary ||
                     dataType == DataTypeConstants.Char ||
                     dataType == DataTypeConstants.Decimal ||
                     dataType == DataTypeConstants.NChar ||
                     dataType == DataTypeConstants.NVarChar ||
                     dataType == DataTypeConstants.VarBinary ||
                     dataType == DataTypeConstants.VarChar)
            {
                if (dataType == DataTypeConstants.Decimal)
                    retval += " (" + length + ", " + scale + ")";
                else if (dataType == DataTypeConstants.DateTime2)
                    retval += " (" + length + ")";
                else
                    retval += " (" + dataType.GetLengthString(length) + ")";
            }
            return retval;
        }

        public static int GetDefaultSize(this DataTypeConstants dataType, int length)
        {
            var size = length;
            switch (dataType)
            {
                case DataTypeConstants.Decimal:
                case DataTypeConstants.Real:
                    size = 18;
                    break;

                case DataTypeConstants.Binary:
                case DataTypeConstants.NVarChar:
                case DataTypeConstants.VarBinary:
                case DataTypeConstants.VarChar:
                    size = 50;
                    break;

                case DataTypeConstants.Char:
                case DataTypeConstants.NChar:
                    size = 10;
                    break;

                case DataTypeConstants.DateTime2:
                    size = 2;
                    break;

            }
            return size;
        }

        public static bool SupportsIdentity(this DataTypeConstants dataType)
        {
            return dataType == DataTypeConstants.BigInt ||
                   dataType == DataTypeConstants.Int ||
                   dataType == DataTypeConstants.SmallInt ||
                   dataType == DataTypeConstants.UniqueIdentifier;
        }

        public static bool IsDirty(this IEnumerable<Field> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<Index> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<IndexColumn> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<ViewField> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<StoredProcedureField> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<StoredProcedureParameter> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<FunctionField> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<FunctionParameter> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<Entity> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<View> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<StoredProcedure> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static bool IsDirty(this IEnumerable<Function> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static void ResetDirty(this IEnumerable<Entity> list, bool newValue)
        {
            foreach (var item in list)
                item.IsDirty = newValue;
        }

        public static void ResetDirty(this IEnumerable<View> list, bool newValue)
        {
            foreach (var item in list)
                item.IsDirty = newValue;
        }

        public static void ResetDirty(this IEnumerable<StoredProcedure> list, bool newValue)
        {
            foreach (var item in list)
                item.IsDirty = newValue;
        }

        public static void ResetDirty(this IEnumerable<Function> list, bool newValue)
        {
            foreach (var item in list)
                item.IsDirty = newValue;
        }

        public static string ToIndentedString(this XmlDocument doc)
        {
            var stringWriter = new StringWriter(new StringBuilder());
            var xmlTextWriter = new XmlTextWriter(stringWriter)
                                    {
                                        Formatting = Formatting.Indented,
                                        IndentChar = '\t'
                                    };
            doc.Save(xmlTextWriter);
            var t = stringWriter.ToString();
            t = t.Replace(@" encoding=""utf-16""", string.Empty);
            return t;
        }

        public static Field GetField(this IndexColumn column)
        {
            return column.Index.Entity.Fields.FirstOrDefault(x => x.Id == column.FieldID);
        }

        public static string ToXmlValue(this Microsoft.VisualStudio.Modeling.Diagrams.RectangleD item)
        {
            //We do not need height as it is calculated
            //return item.X + "," + item.Y + "," + item.Width + "," + item.Height;
            return item.X + "," + item.Y + "," + item.Width;
        }

        public static Microsoft.VisualStudio.Modeling.Diagrams.RectangleD ConvertRectangleDFromXmlValue(string xmlValue)
        {
            var retval = new Microsoft.VisualStudio.Modeling.Diagrams.RectangleD();
            var arr = xmlValue.Split(',');
            if (arr.Length >= 3)
            {
                retval.X = double.Parse(arr[0]);
                retval.Y = double.Parse(arr[1]);
                retval.Width = double.Parse(arr[2]);
                if (arr.Length >= 4)
                    retval.Height = double.Parse(arr[3]);
            }
            return retval;
        }

        public static IEnumerable<T> Subtract<T>(IEnumerable<T> source, IEnumerable<T> other)
        {
            return Subtract(source, other, EqualityComparer<T>.Default);
        }

        public static IEnumerable<T> Subtract<T>(IEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comp)
        {
            var dict = new Dictionary<T, object>(comp);
            foreach (var item in source)
            {
                dict[item] = null;
            }

            foreach (var item in other)
            {
                dict.Remove(item);
            }

            return dict.Keys;
        }

        public static string GetString(this MemoryStream ms)
        {
            ms.Position = 0;
            var sr = new StreamReader(ms, Encoding.Unicode);
            var myStr = sr.ReadToEnd();
            return myStr;
        }

        public static DataTypeConstants? GetDataTypeFromName(string name)
        {
            DataTypeConstants d;
            if (Enum.TryParse<DataTypeConstants>(name, true, out d))
            {
                return d;
            }
            else if (name.ToLower() == "guid") //special case
            {
                return DataTypeConstants.UniqueIdentifier;
            }
            else
            {
                //If the name matches the start of exactly one item then return it
                var l = Enum.GetNames(typeof(DataTypeConstants)).ToList();
                var l2 = l.Where(x => x.StartsWith(name, true, null)).ToList();
                if (l2.Count == 1)
                    return (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), l2.First(), true);
            }
            return null;
        }

    }

    public class FieldOrderComparer : IComparer<Field>
    {
        public int Compare(Field x, Field y)
        {
            if (x.SortOrder == y.SortOrder) return 0;
            return (x.SortOrder > y.SortOrder) ? 1 : -1;
        }
    }
}