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
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Mocks.Generators.SQLHelper
{
    public class SQLHelperGeneratedTemplate : EFDALMockBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public SQLHelperGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides

        public override string FileName
        {
            get { return "UtilityHelper.Generated.cs"; }
        }

        public string ParentItemName
        {
            get { return "UtilityHelper.cs"; }
        }

        public override string FileContent
        {
            get
            {
                this.GenerateContent();
                return sb.ToString();
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
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Reflection;");
            sb.AppendLine("using System.IO;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using " + this.GetLocalNamespace() + ";");
            sb.AppendLine("using " + this.DefaultNamespace + ".EFDAL.Interfaces;");
            sb.AppendLine("using nHydrate.EFCore.DataAccess;");
            sb.AppendLine("using nHydrate.EFCore.Exceptions;");
            sb.AppendLine();
        }

        private void AppendClass()
        {
            #region GlobalValues
            sb.AppendLine("	internal static class GlobalValues");
            sb.AppendLine("	{");
            sb.AppendLine("		public const string ERROR_PROPERTY_NULL = \"The value is null and in an invalid state.\";");
            sb.AppendLine("		public const string ERROR_PROPERTY_SETNULL = \"Cannot set value to null.\";");
            sb.AppendLine("		public const string ERROR_CONCURRENCY_FAILURE = \"Concurrency failure\";");
            sb.AppendLine("		public const string ERROR_CONSTRAINT_FAILURE = \"Constraint failure\";");
            sb.AppendLine("		public const string ERROR_DATA_TOO_BIG = \"The data '{0}' is too large for the {1} field which has a length of {2}.\";");
            sb.AppendLine("		public const string ERROR_INVALID_ENUM = \"The value '{0}' set to the '{1}' field is not valid based on the backing enumeration.\";");
            sb.AppendLine("		public static readonly DateTime MIN_DATETIME = new DateTime(1753, 1, 1);");
            sb.AppendLine("		public static readonly DateTime MAX_DATETIME = new DateTime(9999, 12, 31, 23, 59, 59);");
            sb.AppendLine();

            #region Other Helpers

            sb.AppendLine("		internal static string SetValueHelperInternal(string newValue, bool fixLength, int maxDataLength)");
            sb.AppendLine("		{");
            sb.AppendLine("			string retval = null;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				string v = newValue.ToString();");
            sb.AppendLine("				if (fixLength)");
            sb.AppendLine("				{");
            sb.AppendLine("					int fieldLength = maxDataLength;");
            sb.AppendLine("					if ((fieldLength > 0) && (v.Length > fieldLength)) v = v.Substring(0, fieldLength);");
            sb.AppendLine("				}");
            sb.AppendLine("				retval = v;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static double? SetValueHelperDoubleNullableInternal(object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			double? retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = double.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is double?))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = double.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (double?)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static double SetValueHelperDoubleNotNullableInternal(object newValue, string nullMessage)");
            sb.AppendLine("		{");
            sb.AppendLine("			double retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(nullMessage);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = double.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is double))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = double.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (double)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static DateTime? SetValueHelperDateTimeNullableInternal(object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			DateTime? retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = DateTime.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is DateTime?))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = DateTime.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (DateTime?)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static DateTime SetValueHelperDateTimeNotNullableInternal(object newValue, string nullMessage)");
            sb.AppendLine("		{");
            sb.AppendLine("			DateTime retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(nullMessage);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = DateTime.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is DateTime))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = DateTime.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (DateTime)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static bool? SetValueHelperBoolNullableInternal(object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			bool? retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = bool.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is bool?))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = bool.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (bool?)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static bool SetValueHelperBoolNotNullableInternal(object newValue, string nullMessage)");
            sb.AppendLine("		{");
            sb.AppendLine("			bool retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(nullMessage);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = bool.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is bool))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = bool.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (bool)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		internal static int? SetValueHelperIntNullableInternal(object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			int? retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = int.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is int?))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = int.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (int?)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static int SetValueHelperIntNotNullableInternal(object newValue, string nullMessage)");
            sb.AppendLine("		{");
            sb.AppendLine("			int retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(nullMessage);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = int.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is int))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = int.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (int)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		internal static long? SetValueHelperLongNullableInternal(object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			long? retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = long.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is long?))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = long.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (long?)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static long SetValueHelperLongNotNullableInternal(object newValue, string nullMessage)");
            sb.AppendLine("		{");
            sb.AppendLine("			long retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(nullMessage);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = long.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is long))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = long.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (long)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		internal static T PropertyGetterLambdaErrorHandler<T>(Func<T> func)");
            sb.AppendLine("		{");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				return func();");
            sb.AppendLine("			}");
            sb.AppendLine("			catch (System.Data.DBConcurrencyException dbcex) { throw new ConcurrencyException(GlobalValues.ERROR_CONCURRENCY_FAILURE, dbcex); }");
            sb.AppendLine("			catch (System.Data.SqlClient.SqlException sqlexp) { if (sqlexp.Number == 547 || sqlexp.Number == 2627) throw new UniqueConstraintViolatedException(GlobalValues.ERROR_CONSTRAINT_FAILURE, sqlexp); else throw; }");
            sb.AppendLine("			catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); throw; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static void PropertySetterLambdaErrorHandler(System.Action action)");
            sb.AppendLine("		{");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				action();");
            sb.AppendLine("			}");
            sb.AppendLine("			catch (System.Data.DBConcurrencyException dbcex) { throw new ConcurrencyException(GlobalValues.ERROR_CONCURRENCY_FAILURE, dbcex); }");
            sb.AppendLine("			catch (System.Data.SqlClient.SqlException sqlexp) { if (sqlexp.Number == 547 || sqlexp.Number == 2627) throw new UniqueConstraintViolatedException(GlobalValues.ERROR_CONSTRAINT_FAILURE, sqlexp); else throw; }");
            sb.AppendLine("			catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); throw; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            #endregion

            sb.AppendLine("	}");
            sb.AppendLine();

            #endregion

            #region Extensions
            sb.AppendLine("	internal static class Extensions");
            sb.AppendLine("	{");
            sb.AppendLine("		public static bool Contains(this DataRelationCollection relationList, DataRelation relation)");
            sb.AppendLine("		{");
            sb.AppendLine("			foreach (DataRelation r in relationList)");
            sb.AppendLine("			{");
            sb.AppendLine("				int matches = 0;");
            sb.AppendLine("				foreach (DataColumn c in r.ChildColumns)");
            sb.AppendLine("				{");
            sb.AppendLine("					if (relation.ChildColumns.Contains(c))");
            sb.AppendLine("						matches++;");
            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("				foreach (DataColumn c in r.ParentColumns)");
            sb.AppendLine("				{");
            sb.AppendLine("					if (relation.ParentColumns.Contains(c))");
            sb.AppendLine("						matches++;");
            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("				if (r.ChildColumns.Length == (matches * 2))");
            sb.AppendLine("					return true;");
            sb.AppendLine();
            sb.AppendLine("			}");
            sb.AppendLine("			return false;");
            sb.AppendLine("		}");
            sb.AppendLine("	}");
            sb.AppendLine();
            #endregion

        }

        #endregion

    }

}