#pragma warning disable 0168
using System;
using System.Text;
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
        public override string FileName => "Globals.Generated.cs";
        public string ParentItemName => "Globals.cs";

        public override string FileContent
        {
            get
            {
                try
                {
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

        public void GenerateContent()
        {
            try
            {
                nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);

                #region Using
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine();
                #endregion

                sb.AppendLine($"namespace {this.GetLocalNamespace()}");
                sb.AppendLine("{");

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
                sb.AppendLine("				retval = null;");
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
                sb.AppendLine("				retval = null;");
                sb.AppendLine("			else");
                sb.AppendLine("			{");
                sb.AppendLine("				if (newValue is string)");
                sb.AppendLine("					retval = double.Parse((string)newValue);");
                sb.AppendLine("				else if (!(newValue is double?))");
                sb.AppendLine("					retval = double.Parse(newValue.ToString());");
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				throw new Exception(nullMessage);");
                sb.AppendLine("			else");
                sb.AppendLine("			{");
                sb.AppendLine("				if (newValue is string)");
                sb.AppendLine("					retval = double.Parse((string)newValue);");
                sb.AppendLine("				else if (!(newValue is double))");
                sb.AppendLine("					retval = double.Parse(newValue.ToString());");
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				retval = null;");
                sb.AppendLine("			else");
                sb.AppendLine("			{");
                sb.AppendLine("				if (newValue is string)");
                sb.AppendLine("					retval = DateTime.Parse((string)newValue);");
                sb.AppendLine("				else if (!(newValue is DateTime?))");
                sb.AppendLine("					retval = DateTime.Parse(newValue.ToString());");
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				throw new Exception(nullMessage);");
                sb.AppendLine("			else");
                sb.AppendLine("			{");
                sb.AppendLine("				if (newValue is string)");
                sb.AppendLine("					retval = DateTime.Parse((string)newValue);");
                sb.AppendLine("				else if (!(newValue is DateTime))");
                sb.AppendLine("					retval = DateTime.Parse(newValue.ToString());");
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				retval = null;");
                sb.AppendLine("			else");
                sb.AppendLine("			{");
                sb.AppendLine("				if (newValue is string)");
                sb.AppendLine("					retval = bool.Parse((string)newValue);");
                sb.AppendLine("				else if (!(newValue is bool?))");
                sb.AppendLine("					retval = bool.Parse(newValue.ToString());");
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				throw new Exception(nullMessage);");
                sb.AppendLine("			else");
                sb.AppendLine("			{");
                sb.AppendLine("				if (newValue is string)");
                sb.AppendLine("					retval = bool.Parse((string)newValue);");
                sb.AppendLine("				else if (!(newValue is bool))");
                sb.AppendLine("					retval = bool.Parse(newValue.ToString());");
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				retval = null;");
                sb.AppendLine("			else");
                sb.AppendLine("			{");
                sb.AppendLine("				if (newValue is string)");
                sb.AppendLine("					retval = int.Parse((string)newValue);");
                sb.AppendLine("				else if (!(newValue is int?))");
                sb.AppendLine("					retval = int.Parse(newValue.ToString());");
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				throw new Exception(nullMessage);");
                sb.AppendLine("			else");
                sb.AppendLine("			{");
                sb.AppendLine("				if (newValue is string)");
                sb.AppendLine("					retval = int.Parse((string)newValue);");
                sb.AppendLine("				else if (!(newValue is int))");
                sb.AppendLine("					retval = int.Parse(newValue.ToString());");
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				retval = null;");
                sb.AppendLine("			else");
                sb.AppendLine("			{");
                sb.AppendLine("				if (newValue is string)");
                sb.AppendLine("					retval = long.Parse((string)newValue);");
                sb.AppendLine("				else if (!(newValue is long?))");
                sb.AppendLine("					retval = long.Parse(newValue.ToString());");
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("				throw new Exception(nullMessage);");
                sb.AppendLine("			else");
                sb.AppendLine("			{");
                sb.AppendLine("				if (newValue is string)");
                sb.AppendLine("					retval = long.Parse((string)newValue);");
                sb.AppendLine("				else if (!(newValue is long))");
                sb.AppendLine("					retval = long.Parse(newValue.ToString());");
                sb.AppendLine("				else if (newValue is IBusinessObject)");
                sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
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
                sb.AppendLine("		DateTime CreatedDate { set; }");
                sb.AppendLine("		DateTime ModifiedDate { set; }");
                sb.AppendLine("		string ModifiedBy { set; }");
                sb.AppendLine("		string CreatedBy { set; }");
                //sb.AppendLine("		void ResetCreatedBy(string modifier);");
                //sb.AppendLine("		void ResetModifiedBy(string modifier);");
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

                #region FieldNameConstantsAttribute
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
                #endregion

                #region IContext
                sb.AppendLine("	#region IContext");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The interface for a entity context");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public partial interface IContext");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		ContextStartup ContextStartup { get; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The database context");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The database connection string");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		string ConnectionString { get; }");
                sb.AppendLine();
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
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Given a field enumeration value, returns an entity enumeration value designating the source entity of the field");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		Enum GetEntityFromField(Enum field);");
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
                #endregion

                #region BaseEntity
                sb.AppendLine("	#region BaseEntity");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The base class for all entity objects using EF 6");
                sb.AppendLine("	/// </summary>");
                //sb.AppendLine("	[System.Runtime.Serialization.DataContract(IsReference = true)]");
                sb.AppendLine("	public abstract partial class BaseEntity");
                sb.AppendLine("	{");
                if (_model.EnableCustomChangeEvents)
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// Event raised after a property is changed");
                    sb.AppendLine("		/// </summary>");
                    //sb.AppendLine("		[field:NonSerialized]");
                    sb.AppendLine("		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// Event raised before a property is changed");
                    sb.AppendLine("		/// </summary>");
                    //sb.AppendLine("		[field:NonSerialized]");
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
                #endregion

                #region IBusinessObject
                sb.AppendLine("	#region IBusinessObject");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// An interface for writable entities");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine($"	public partial interface IBusinessObject : {GetLocalNamespace()}.IReadOnlyBusinessObject");
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
                #endregion

                #region IPrimaryKey
                sb.AppendLine("	#region IPrimaryKey");
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	public partial interface IPrimaryKey");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		long Hash { get; }");
                sb.AppendLine("	}");
                sb.AppendLine();
                #endregion

                #region PrimaryKey
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
                #endregion

                #region Audit Attributes
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Attribute used to decorate a concurrency field");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[System.AttributeUsage(System.AttributeTargets.Property)]");
                sb.AppendLine("	public partial class AuditTimestampAttribute : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine();

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Attribute used to decorate an Audit CreatedBy field");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[System.AttributeUsage(System.AttributeTargets.Property)]");
                sb.AppendLine("	public partial class AuditCreatedByAttribute : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine();

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Attribute used to decorate an Audit CreatedDate field");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[System.AttributeUsage(System.AttributeTargets.Property)]");
                sb.AppendLine("	public partial class AuditCreatedDateAttribute : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine();

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Attribute used to decorate an Audit ModifiedBy field");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[System.AttributeUsage(System.AttributeTargets.Property)]");
                sb.AppendLine("	public partial class AuditModifiedByAttribute : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine();

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Attribute used to decorate an Audit ModifiedDate field");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[System.AttributeUsage(System.AttributeTargets.Property)]");
                sb.AppendLine("	public partial class AuditModifiedDateAttribute : System.Attribute");
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                #endregion

                #region HasNoKeyAttribute
                sb.AppendLine("    /// <summary>");
                sb.AppendLine("    /// Indicates that a table has no primary key");
                sb.AppendLine("    /// </summary>");
                sb.AppendLine("    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]");
                sb.AppendLine("    public class HasNoKeyAttribute : System.Attribute");
                sb.AppendLine("    {");
                sb.AppendLine("    }");
                sb.AppendLine();
                #endregion

                #region StringLengthUnboundedAttribute
                sb.AppendLine("    /// <summary>");
                sb.AppendLine("    /// Indicates that a string has no set maximum length");
                sb.AppendLine("    /// </summary>");
                sb.AppendLine("    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]");
                sb.AppendLine("    public class StringLengthUnboundedAttribute : System.Attribute");
                sb.AppendLine("    {");
                sb.AppendLine("    }");
                sb.AppendLine();
                #endregion

                #region Tenant Attributes
                sb.AppendLine("    /// <summary>");
                sb.AppendLine("    /// Indicates that a database table is tenant based");
                sb.AppendLine("    /// </summary>");
                sb.AppendLine("    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]");
                sb.AppendLine("    public class TenantEntityAttribute : System.Attribute");
                sb.AppendLine("    {");
                sb.AppendLine("    }");
                sb.AppendLine();

                //sb.AppendLine("    /// <summary>");
                //sb.AppendLine("    /// Identifies the SQL user column for a tenant");
                //sb.AppendLine("    /// </summary>");
                //sb.AppendLine("    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]");
                //sb.AppendLine("    public class SqlUserAttribute : System.Attribute");
                //sb.AppendLine("    {");
                //sb.AppendLine("    }");
                //sb.AppendLine();
                #endregion

                #region IReadOnlyBusinessObject
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
                #endregion

                #region AuditPaging
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
                #endregion

                #region AuditResult
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
                #endregion

                #region IAuditResultFieldCompare
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
                #endregion

                #region AuditResultFieldCompare
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
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public System.Type DataType { get; internal set; }");
                sb.AppendLine();
                sb.AppendLine("		System.Enum IAuditResultFieldCompare.Field => (System.Enum)Enum.Parse(typeof(E), this.Field.ToString());");
                sb.AppendLine("		object IAuditResultFieldCompare.Value1 => this.Value1;");
                sb.AppendLine("		object IAuditResultFieldCompare.Value2 => this.Value2;");
                sb.AppendLine();
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine();
                #endregion

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
                sb.AppendLine("	public class EntityListEventArgs : System.EventArgs");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		public IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> List { get; set; }");
                sb.AppendLine("	}");
                sb.AppendLine("	#endregion");
                sb.AppendLine("}");
                sb.AppendLine();
                #endregion

                #region Exceptions
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Exceptions");
                sb.AppendLine("{");

                sb.AppendLine("	#region ConcurrencyException");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// Summary description for ConcurrencyException.");
                sb.AppendLine("	/// </summary>");
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