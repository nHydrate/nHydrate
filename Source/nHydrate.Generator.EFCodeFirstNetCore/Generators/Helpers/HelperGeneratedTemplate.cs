#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
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
using nHydrate.Generator.EFCodeFirstNetCore;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Helpers
{
    public class HelperGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public HelperGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return "Globals.Generated.cs"; }
        }

        public string ParentItemName
        {
            get { return "Globals.cs"; }
        }

        public override string FileContent
        {
            get
            {
                try
                {
                    GenerateContent();
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

        public void GenerateContent()
        {
            try
            {
                nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
                nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);

                #region Using
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using System.Linq.Expressions;");
                sb.AppendLine("using System.Runtime.Serialization;");
                sb.AppendLine("using System.Collections.Concurrent;");
                sb.AppendLine("using System.Reflection;");

                sb.AppendLine();
                #endregion

                #region  This is a Polyfill/Shim that does nothing but allows Serializable to compile
                sb.AppendLine("namespace System");
                sb.AppendLine("{");
                sb.AppendLine("	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate)]");
                sb.AppendLine("	internal class SerializableAttribute : Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine("	public interface ICloneable");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine("}");
                sb.AppendLine();

                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");

                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public partial interface IMetadata");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		string GetTableName();");
                sb.AppendLine("	}");
                sb.AppendLine();
                #endregion

                #region GlobalValues

                sb.AppendLine("	#region GlobalValues");
                sb.AppendLine();
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
                sb.AppendLine("		private const string INVALID_BUSINIESSOBJECT = \"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\";");
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
                sb.AppendLine("				var v = newValue.ToString();");
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
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("				{");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("				{");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("				{");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("				{");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("				{");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("				{");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("				{");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("				{");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("				{");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("				{");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                //sb.AppendLine("			catch (Exceptions.ConcurrencyException dbcex) { throw new Exceptions.ConcurrencyException(GlobalValues.ERROR_CONCURRENCY_FAILURE, dbcex); }");
                //sb.AppendLine("			catch (System.Data.SqlClient.SqlException sqlexp) { if (sqlexp.Number == 547 || sqlexp.Number == 2627) throw new UniqueConstraintViolatedException(GlobalValues.ERROR_CONSTRAINT_FAILURE, sqlexp); else throw; }");
                sb.AppendLine("			catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); throw; }");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		internal static void PropertySetterLambdaErrorHandler(System.Action action)");
                sb.AppendLine("		{");
                sb.AppendLine("			try");
                sb.AppendLine("			{");
                sb.AppendLine("				action();");
                sb.AppendLine("			}");
                //sb.AppendLine("			catch (Exceptions.ConcurrencyException dbcex) { throw new Exceptions.ConcurrencyException(GlobalValues.ERROR_CONCURRENCY_FAILURE, dbcex); }");
                //sb.AppendLine("			catch (System.Data.SqlClient.SqlException sqlexp) { if (sqlexp.Number == 547 || sqlexp.Number == 2627) throw new UniqueConstraintViolatedException(GlobalValues.ERROR_CONSTRAINT_FAILURE, sqlexp); else throw; }");
                sb.AppendLine("			catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); throw; }");
                sb.AppendLine("		}");
                sb.AppendLine();

                #endregion

                sb.AppendLine("	}");
                sb.AppendLine();
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                #endregion

                #region Util
                sb.AppendLine("	#region Util");
                sb.AppendLine("	internal static partial class Util");
                sb.AppendLine("	{");

                sb.AppendLine("		public static string HashPK(params object[] p)");
                sb.AppendLine("		{");
                sb.AppendLine("			var retval = string.Empty;");
                sb.AppendLine("			for (var ii = 0; ii < p.Length; ii++)");
                sb.AppendLine("			{");
                sb.AppendLine("				retval += p[ii] + \"|\" + ii + \"|\";");
                sb.AppendLine("			}");
                sb.AppendLine("			return retval;");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		public static UInt64 HashFast(string read)");
                sb.AppendLine("		{");
                sb.AppendLine("			UInt64 hashedValue = 3074457345618258791ul;");
                sb.AppendLine("			for (int i = 0; i < read.Length; i++)");
                sb.AppendLine("			{");
                sb.AppendLine("				hashedValue += read[i];");
                sb.AppendLine("				hashedValue *= 3074457345618258799ul;");
                sb.AppendLine("			}");
                sb.AppendLine("			return hashedValue;");
                sb.AppendLine("		}");
                sb.AppendLine();

                //sb.AppendLine("		internal static string ConvertNormalCS2EFFromConfig(string configSettings)");
                //sb.AppendLine("		{");
                //sb.AppendLine("			return ConvertNormalCS2EFFromConfig(configSettings, new ContextStartup(string.Empty, false));");
                //sb.AppendLine("		}");
                //sb.AppendLine();

                //sb.AppendLine("		private static Dictionary<string, string> _csConvertCache = new Dictionary<string, string>();");
                //sb.AppendLine("		internal static string ConvertNormalCS2EFFromConfig(string configSettings, ContextStartup contextStartup)");
                //sb.AppendLine("		{");
                //sb.AppendLine("			if (string.IsNullOrEmpty(configSettings)) return configSettings;");
                //sb.AppendLine("			var arr = configSettings.Split('=');");
                //sb.AppendLine("			if (arr.Length != 2) return configSettings;");
                //sb.AppendLine("			if (arr[0] != \"name\") return configSettings;");
                //sb.AppendLine("			try");
                //sb.AppendLine("			{");
                //sb.AppendLine("				var cs = System.Configuration.ConfigurationManager.ConnectionStrings[arr[1]].ConnectionString;");
                //sb.AppendLine("				if (!cs.StartsWith(\"metadata=\")) return ConvertNormalCS2EF(cs, contextStartup);");
                //sb.AppendLine("				return configSettings;");
                //sb.AppendLine("			}");
                //sb.AppendLine("			catch");
                //sb.AppendLine("			{");
                //sb.AppendLine("				return configSettings;");
                //sb.AppendLine("			}");
                //sb.AppendLine("		}");
                //sb.AppendLine();
                //sb.AppendLine("		internal static string ConvertNormalCS2EF(string connectionString)");
                //sb.AppendLine("		{");
                //sb.AppendLine("			return ConvertNormalCS2EF(connectionString, new ContextStartup(string.Empty, false));");
                //sb.AppendLine("		}");
                //sb.AppendLine();
                //sb.AppendLine("		internal static string ConvertNormalCS2EF(string connectionString, ContextStartup contextStartup)");
                //sb.AppendLine("		{");
                //sb.AppendLine("			return ConvertNormalCS2EFFromConfig(connectionString, contextStartup);");
                //sb.AppendLine("		}");
                //sb.AppendLine();
                sb.AppendLine("		internal static string StripEFCS2Normal(string connectionString)");
                sb.AppendLine("		{");
                sb.AppendLine("			const string PROVIDER = \"provider connection string\";");
                sb.AppendLine("			if (connectionString.StartsWith(\"metadata=\"))");
                sb.AppendLine("			{");
                sb.AppendLine("				var retval = string.Empty;");
                sb.AppendLine("				var index = connectionString.IndexOf(PROVIDER);");
                sb.AppendLine();
                sb.AppendLine("				var index1 = -1;");
                sb.AppendLine("				var index2 = -1;");
                sb.AppendLine("				if (index == -1) return connectionString;");
                sb.AppendLine();
                sb.AppendLine("				var foundEQ = false;");
                sb.AppendLine("				for (var ii = index + PROVIDER.Length; ii < connectionString.Length; ii++)");
                sb.AppendLine("				{");
                sb.AppendLine("					if (connectionString[ii] == '=')");
                sb.AppendLine("					{");
                sb.AppendLine("						foundEQ = true;");
                sb.AppendLine("					}");
                sb.AppendLine("					else if (foundEQ)");
                sb.AppendLine("					{");
                sb.AppendLine("						connectionString = connectionString.Substring(ii, connectionString.Length - ii);");
                sb.AppendLine("						index1 = connectionString.IndexOf('\"');");
                sb.AppendLine("						index2 = connectionString.LastIndexOf('\"');");
                sb.AppendLine("					}");
                sb.AppendLine();
                sb.AppendLine("					if (index1 != -1 && index2 != -1)");
                sb.AppendLine("					{");
                sb.AppendLine("						return connectionString.Substring(index1 + 1, index2 - index1 - 1);");
                sb.AppendLine("					}");
                sb.AppendLine();
                sb.AppendLine("				}");
                sb.AppendLine();
                sb.AppendLine("			}");
                sb.AppendLine("			return connectionString;");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();
                #endregion

                #region Imported from EFCore

                sb.AppendLine("	#region AuditTypeConstants Enumeration");
                sb.AppendLine();
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// A set of values for the types of audits");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public enum AuditTypeConstants");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Represents a row insert");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		Insert = 1,");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Represents a row update");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		Update = 2,");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Represents a row delete");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		Delete = 3,");
                sb.AppendLine("	}");
                sb.AppendLine();
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region IAudit");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The base interface for all audit objects");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public interface IAudit");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The type of audit");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		AuditTypeConstants AuditType { get; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The date of the audit");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		DateTime AuditDate { get; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The modifier value of the audit");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		string ModifiedBy { get; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region ICreatable");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public partial interface ICreatable");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region IAuditable");

                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	internal partial interface IAuditableSet");
                sb.AppendLine("	{");
                sb.AppendLine("		DateTime? CreatedDate { get; set; }");
                sb.AppendLine("		DateTime? ModifiedDate { get; set; }");
                sb.AppendLine("		string ModifiedBy { get; set; }");
                sb.AppendLine("		string CreatedBy { get; set; }");
                sb.AppendLine("		void ResetCreatedBy(string modifier);");
                sb.AppendLine("		void ResetModifiedBy(string modifier);");
                sb.AppendLine("	}");
                sb.AppendLine();

                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public partial interface IAuditable");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		bool IsCreateAuditImplemented { get; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		bool IsModifyAuditImplemented { get; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		bool IsTimestampAuditImplemented { get; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		string CreatedBy { get; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		DateTime? CreatedDate { get; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		string ModifiedBy { get; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		DateTime? ModifiedDate { get; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		byte[] TimeStamp { get; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region CustomMetadata");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]");
                sb.AppendLine("	public class CustomMetadata : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string Key { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string Value { get; set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region EntityFieldMetadata");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	[AttributeUsage(AttributeTargets.Property)]");
                sb.AppendLine("	public partial class EntityFieldMetadata : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public EntityFieldMetadata(");
                sb.AppendLine("			string name,");
                sb.AppendLine("			int sortOrder,");
                sb.AppendLine("			bool uiVisible,");
                sb.AppendLine("			int maxLength,");
                sb.AppendLine("			string mask,");
                sb.AppendLine("			string friendlyName,");
                sb.AppendLine("			string defaultValue,");
                sb.AppendLine("			bool allowNull,");
                sb.AppendLine("			string description,");
                sb.AppendLine("			bool isComputed,");
                sb.AppendLine("			bool isUnique,");
                sb.AppendLine("			double min,");
                sb.AppendLine("			double max,");
                sb.AppendLine("			bool isPrimaryKey");
                sb.AppendLine("		)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.Name = name;");
                sb.AppendLine("			this.SortOrder = sortOrder;");
                sb.AppendLine("			this.UIVisible = uiVisible;");
                sb.AppendLine("			this.MaxLength = maxLength;");
                sb.AppendLine("			this.Mask = mask;");
                sb.AppendLine("			this.FriendlyName = friendlyName;");
                sb.AppendLine("			this.Default = defaultValue;");
                sb.AppendLine("			this.AllowNull = allowNull;");
                sb.AppendLine("			this.Description = description;");
                sb.AppendLine("			this.IsComputed = isComputed;");
                sb.AppendLine("			this.IsUnique = isUnique;");
                sb.AppendLine("			this.Min = min;");
                sb.AppendLine("			this.Max = max;");
                sb.AppendLine("			this.IsPrimaryKey = isPrimaryKey;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string Name { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public int SortOrder { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool UIVisible { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public int MaxLength { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string Mask { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string FriendlyName { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public object Default { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool AllowNull { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string Description { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool IsComputed { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool IsUnique { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public double Min { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public double Max { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool IsPrimaryKey { get; set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region EntityHistory");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Identities an entity class as having an audit history");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[AttributeUsage(AttributeTargets.Class)]");
                sb.AppendLine("	public class EntityHistory : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public EntityHistory(System.Type auditType) { this.AuditType = auditType; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public System.Type AuditType { get; private set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region EntityMetadata");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	[AttributeUsage(AttributeTargets.Class)]");
                sb.AppendLine("	public partial class EntityMetadata : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public EntityMetadata(");
                sb.AppendLine("			string name,");
                sb.AppendLine("			bool allowAuditTracking,");
                sb.AppendLine("			bool allowCreateAudit,");
                sb.AppendLine("			bool allowModifiedAudit,");
                sb.AppendLine("			bool allowConcurrencyAudit,");
                sb.AppendLine("			string description,");
                sb.AppendLine("			bool enforcePrimaryKey,");
                sb.AppendLine("			bool immutable,");
                sb.AppendLine("			bool isTypeTable,");
                sb.AppendLine("			string dbSchema");
                sb.AppendLine("		)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.Name = name;");
                sb.AppendLine("			this.AllowAuditTracking = allowAuditTracking;");
                sb.AppendLine("			this.AllowCreateAudit = allowCreateAudit;");
                sb.AppendLine("			this.AllowModifiedAudit = allowModifiedAudit;");
                sb.AppendLine("			this.AllowConcurrencyAudit = allowConcurrencyAudit;");
                sb.AppendLine("			this.Description = description;");
                sb.AppendLine("			this.EnforcePrimaryKey = enforcePrimaryKey;");
                sb.AppendLine("			this.Immutable = immutable;");
                sb.AppendLine("			this.IsTypeTable = isTypeTable;");
                sb.AppendLine("			this.DBSchema = dbSchema;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string Name { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool AllowAuditTracking { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool AllowCreateAudit { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool AllowModifiedAudit { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool AllowConcurrencyAudit { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string Description { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool EnforcePrimaryKey { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool Immutable { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public bool IsTypeTable { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string DBSchema { get; set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region FieldNameConstantsAttribute");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Identities the type of IBusinessObject for an enumeration");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[AttributeUsage(AttributeTargets.Class)]");
                sb.AppendLine("	public class FieldNameConstantsAttribute : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public FieldNameConstantsAttribute(System.Type targetType) { this.TargetType = targetType; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public System.Type TargetType { get; private set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region PrimaryKeyAttribute");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Identities the primary key of an IBusinessObject enumeration");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[AttributeUsage(AttributeTargets.Field)]");
                sb.AppendLine("	public class PrimaryKeyAttribute : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region IContextInclude");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public partial interface IContextInclude");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region IContext");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The interface for a context object");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public partial interface IContext");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		ContextStartup ContextStartup { get; }");
                sb.AppendLine();
                //sb.AppendLine("		/// <summary>");
                //sb.AppendLine("		/// Gets the object context.");
                //sb.AppendLine("		/// </summary>");
                //sb.AppendLine("		System.Data.Entity.Core.Objects.ObjectContext ObjectContext { get; }");
                //sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// A unique key for this object instance");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		Guid InstanceKey { get; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Determines the key of the model that created this library.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		string ModelKey { get; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Determines the version of the model that created this library.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		string Version { get; }");
                sb.AppendLine();
                //sb.AppendLine("		/// <summary>");
                //sb.AppendLine("		/// Determines if the API matches the database connection");
                //sb.AppendLine("		/// </summary>");
                //sb.AppendLine("		bool IsValidConnection();");
                //sb.AppendLine();
                //sb.AppendLine("		/// <summary>");
                //sb.AppendLine("		/// Determines if the API matches the database connection");
                //sb.AppendLine("		/// </summary>");
                //sb.AppendLine("		/// <param name=\"checkVersion\">Determines if the check also includes the exact version of the model</param>");
                //sb.AppendLine("		bool IsValidConnection(bool checkVersion);");
                //sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Given a field enumeration value, returns an entity enumeration value designating the source entity of the field");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		Enum GetEntityFromField(Enum field);");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Given an entity enumeration value, returns a metadata object for the entity");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\"></param>");
                sb.AppendLine("		/// <returns></returns>");
                sb.AppendLine("		object GetMetaData(Enum entity);");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Given a field enumeration value, returns the system type of the associated property");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"field\"></param>");
                sb.AppendLine("		/// <returns></returns>");
                sb.AppendLine("		System.Type GetFieldType(Enum field);");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region BaseEntity");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The base class for all entity objects using EF 6");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[Serializable]");
                sb.AppendLine("	[System.Runtime.Serialization.DataContract(IsReference = true)]");
                sb.AppendLine("	public abstract partial class BaseEntity");
                sb.AppendLine("	{");
                if (_model.EnableCustomChangeEvents)
                {
                    sb.AppendLine("		/// <summary />");
                    sb.AppendLine("		[field:NonSerialized]");
                    sb.AppendLine("		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
                    sb.AppendLine("		/// <summary />");
                    sb.AppendLine("		[field:NonSerialized]");
                    sb.AppendLine("		public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;");
                    sb.AppendLine();
                    sb.AppendLine("		/// <summary />");
                    sb.AppendLine("		protected virtual void OnPropertyChanging(System.ComponentModel.PropertyChangingEventArgs e)");
                    sb.AppendLine("		{");
                    sb.AppendLine("			if (this.PropertyChanging != null)");
                    sb.AppendLine("				this.PropertyChanging(this, e);");
                    sb.AppendLine("		}");
                    sb.AppendLine();
                    sb.AppendLine("		/// <summary />");
                    sb.AppendLine("		protected virtual void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)");
                    sb.AppendLine("		{");
                    sb.AppendLine("			if (this.PropertyChanged != null)");
                    sb.AppendLine("				this.PropertyChanged(this, e);");
                    sb.AppendLine("		}");
                }
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region IBusinessObject");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The interface for all entities");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public partial interface IBusinessObject : " + GetLocalNamespace() + ".IReadOnlyBusinessObject");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Sets the value of a field");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"field\">The field to set</param>");
                sb.AppendLine("		/// <param name=\"newValue\">The new value to set</param>");
                sb.AppendLine("		/// <returns></returns>");
                sb.AppendLine("		void SetValue(Enum field, object newValue);");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Sets the value of a field");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"field\">The field to set</param>");
                sb.AppendLine("		/// <param name=\"newValue\">The new value to set</param>");
                sb.AppendLine("		/// <param name=\"fixLength\">Determines if the length should be truncated if too long. When false, an error will be raised if data is too large to be assigned to the field.</param>");
                sb.AppendLine("		/// <returns></returns>");
                sb.AppendLine("		void SetValue(Enum field, object newValue, bool fixLength);");
                sb.AppendLine();
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region QueryOptimizer");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// This class can be used to optimize queries or report information about the operations");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public partial class QueryOptimizer");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Determines if the query use select locks");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public bool NoLocking { get; set; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Determines the total time a query took to run");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public long TotalMilliseconds { get; set; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The maximum number of rows to affect with a query. 0 is no limit.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public int ChunkSize { get; set; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Default constructor");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public QueryOptimizer()");
                sb.AppendLine("		{");
                sb.AppendLine("			this.NoLocking = false;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Initializes a new instance of this object using the specified NoLocking property");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"noLocking\">Determines if the query use select locks</param>");
                sb.AppendLine("		public QueryOptimizer(bool noLocking)");
                sb.AppendLine("			: this()");
                sb.AppendLine("		{");
                sb.AppendLine("			this.NoLocking = noLocking;");
                sb.AppendLine("		}");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region IPrimaryKey");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public partial interface IPrimaryKey");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		long Hash { get; }");
                sb.AppendLine("	}");
                sb.AppendLine();
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public partial class PrimaryKey : IPrimaryKey");
                sb.AppendLine("	{");
                sb.AppendLine("		internal PrimaryKey(string key)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.Hash = (long)Util.HashFast(key);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public override bool Equals(object obj)");
                sb.AppendLine("		{");
                sb.AppendLine("			if (obj == null) return false;");
                sb.AppendLine("			if (!(obj is PrimaryKey)) return false;");
                sb.AppendLine("				return (((PrimaryKey)obj).Hash == this.Hash);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public override int GetHashCode()");
                sb.AppendLine("		{");
                sb.AppendLine("			return base.GetHashCode();");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public static bool operator ==(PrimaryKey a, PrimaryKey b)");
                sb.AppendLine("		{");
                sb.AppendLine("			if (System.Object.ReferenceEquals(a, b))");
                sb.AppendLine("			{");
                sb.AppendLine("				return true;");
                sb.AppendLine("			}");
                sb.AppendLine("			// If one is null, but not both, return false.");
                sb.AppendLine("			if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))");
                sb.AppendLine("			{");
                sb.AppendLine("				return false;");
                sb.AppendLine("			}");
                sb.AppendLine();
                sb.AppendLine("			return a.Hash == b.Hash;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public static bool operator !=(PrimaryKey a, PrimaryKey b)");
                sb.AppendLine("		{");
                sb.AppendLine("			return !(a == b);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public long Hash { get; private set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region IReadOnlyBusinessObject");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public partial interface IReadOnlyBusinessObject");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// If applicable, returns the maximum number of characters the specified field can hold");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"field\"></param>");
                sb.AppendLine("		/// <returns>If not applicable, the return value is 0</returns>");
                sb.AppendLine("		int GetMaxLength(Enum field);");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Returns the primary key for this object");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		IPrimaryKey PrimaryKey { get; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		System.Type GetFieldNameConstants();");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Gets the value of a field specified by the enumeration");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"field\">The field from which to get the value</param>");
                sb.AppendLine("		/// <returns></returns>");
                sb.AppendLine("		object GetValue(Enum field);");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Gets the value of a field specified by the enumeration");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"field\">The field from which to get the value</param>");
                sb.AppendLine("		/// <param name=\"defaultValue\">The default value to return if the value is null</param>");
                sb.AppendLine("		/// <returns></returns>");
                sb.AppendLine("		object GetValue(Enum field, object defaultValue);");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Returns the system type of the specified field");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"field\"></param>");
                sb.AppendLine("		/// <returns></returns>");
                sb.AppendLine("		System.Type GetFieldType(Enum field);");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region IBusinessObjectLINQQuery");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public partial interface IBusinessObjectLINQQuery");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region AuditPaging");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// ");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	/// <typeparam name=\"T\">An audit object</typeparam>");
                sb.AppendLine("	public partial class AuditPaging<T>");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public AuditPaging() { }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public int PageOffset { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public int RecordsPerPage { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public IEnumerable<T> InnerList { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public int TotalRecordCount { get; set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region AuditResult");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// A result structure for audit records");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	/// <typeparam name=\"T\"></typeparam>");
                sb.AppendLine("	public class AuditResult<T>");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public AuditResult(T item1, T item2)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.Item1 = item1;");
                sb.AppendLine("			this.Item2 = item2;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public IEnumerable<IAuditResultFieldCompare> Differences { get; set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public T Item1 { get; internal set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public T Item2 { get; internal set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region IAuditResultFieldCompare");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("	public interface IAuditResultFieldCompare");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		System.Enum Field { get; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		object Value1 { get; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		object Value2 { get; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		System.Type DataType { get; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region AuditResultFieldCompare");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public class AuditResultFieldCompare<R, E> : IAuditResultFieldCompare");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public AuditResultFieldCompare(R value1, R value2, E field, System.Type dataType)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.Field = field;");
                sb.AppendLine("			this.Value1 = value1;");
                sb.AppendLine("			this.Value2 = value2;");
                sb.AppendLine("			this.DataType = dataType;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public E Field { get; internal set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public R Value1 { get; internal set; }");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public R Value2 { get; internal set; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public System.Type DataType { get; internal set; }");
                sb.AppendLine();
                sb.AppendLine("		#region IAuditResultFieldCompare");
                sb.AppendLine();
                sb.AppendLine("		System.Enum IAuditResultFieldCompare.Field");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return (System.Enum)Enum.Parse(typeof(E), this.Field.ToString()); }");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		object IAuditResultFieldCompare.Value1");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return this.Value1; }");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		object IAuditResultFieldCompare.Value2");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return this.Value2; }");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		#endregion");
                sb.AppendLine();
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                #endregion

                sb.AppendLine("}");
                sb.AppendLine();

                #region EventArgs
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".EventArguments");
                sb.AppendLine("{");
                sb.AppendLine("	#region ChangedEventArgs");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The event argument type of all property setters after the property is changed");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	/// <typeparam name=\"T\"></typeparam>");
                sb.AppendLine("	public partial class ChangedEventArgs<T> : System.ComponentModel.PropertyChangingEventArgs");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Initializes a new instance of the ChangingEventArgs class");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"newValue\">The new value of the property being set</param>");
                sb.AppendLine("		/// <param name=\"propertyName\">The name of the property being set</param>");
                sb.AppendLine("		public ChangedEventArgs(T newValue, string propertyName)");
                sb.AppendLine("			: base(propertyName)");
                sb.AppendLine("		{");
                sb.AppendLine("			this.Value = newValue;");
                sb.AppendLine("		}");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The new value of the property");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public T Value { get; set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();
                sb.AppendLine("	#region ChangingEventArgs");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The event argument type of all property setters before the property is changed");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public partial class ChangingEventArgs<T> : ChangedEventArgs<T>");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Initializes a new instance of the ChangingEventArgs class");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"newValue\">The new value of the property being set</param>");
                sb.AppendLine("		/// <param name=\"propertyName\">The name of the property being set</param>");
                sb.AppendLine("		public ChangingEventArgs(T newValue, string propertyName)");
                sb.AppendLine("			: base(newValue, propertyName)");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Determines if this operation is cancelled.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public bool Cancel { get; set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public class EntityEventArgs : System.EventArgs");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public IBusinessObject Entity { get; set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public class EntityListEventArgs : System.EventArgs");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> List { get; set; }");
                sb.AppendLine("	}");
                sb.AppendLine("}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();
                #endregion

                #region Exceptions
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Exceptions");
                sb.AppendLine("{");

                sb.AppendLine("	#region ConcurrencyException");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Summary description for ConcurrencyException.");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[Serializable]");
                sb.AppendLine("	public partial class ConcurrencyException : nHydrateException");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public ConcurrencyException(string message)");
                sb.AppendLine("			: base(message)");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public ConcurrencyException(string message, System.Exception ex)");
                sb.AppendLine("			: base(message, ex)");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region nHydrateException");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	[Serializable]");
                sb.AppendLine("	public partial class nHydrateException : System.Exception");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string ErrorCode = null;");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public string []Arguments = null;");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public nHydrateException (): base ()");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public nHydrateException ( string Message ) : base ( Message )");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine("		");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public nHydrateException ( string Message, System.Exception InnerException ) : base ( Message, InnerException )");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public nHydrateException ( string ErrorCode, string Message ) : base ( Message )");
                sb.AppendLine("		{");
                sb.AppendLine("			this.ErrorCode = ErrorCode;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public nHydrateException ( string ErrorCode, params object [] Arguments )");
                sb.AppendLine("		{");
                sb.AppendLine("			this.ErrorCode = ErrorCode;");
                sb.AppendLine("			//this.arguments = arguments;");
                sb.AppendLine();
                sb.AppendLine("			this.Arguments = new string [Arguments.Length];");
                sb.AppendLine();
                sb.AppendLine("			for ( var length = 0; length < Arguments.Length; ++ length )");
                sb.AppendLine("			{");
                sb.AppendLine("				this.Arguments[length] = (string)Arguments [length];");
                sb.AppendLine("			}");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public nHydrateException ( string ErrorCode, string Message, System.Exception InnerException ) : base ( Message, InnerException )");
                sb.AppendLine("		{");
                sb.AppendLine("			this.ErrorCode = ErrorCode;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("	#region UniqueConstraintViolatedException");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Summary description for UniqueConstraintViolatedException.");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[Serializable]");
                sb.AppendLine("	public partial class UniqueConstraintViolatedException : nHydrateException");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public UniqueConstraintViolatedException(string message)");
                sb.AppendLine("			: base(message)");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public UniqueConstraintViolatedException(string message, System.Exception ex)");
                sb.AppendLine("			: base(message, ex)");
                sb.AppendLine("		{");
                sb.AppendLine("		}");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();

                sb.AppendLine("}");
                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

    }
}