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

namespace Widgetsphere.Generator.ProjectItemGenerators.ConfigValues
{
	class ConfigValuesExtenderTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();

		#region Constructors
		public ConfigValuesExtenderTemplate(ModelRoot model)
		{
			_model = model;
		}
		#endregion

		#region BaseClassTemplate overrides
		public override string FileContent
		{
			get
			{
				GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get
			{
				return string.Format("ConfigurationValues.cs");
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
				sb.AppendLine("namespace " + DefaultNamespace + ".Business");
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
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using System.Configuration;");
			sb.AppendLine();
		}

		public void AppendClass()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// This is a static class used to define the database connection properties");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	public class ConfigurationValues");
			sb.AppendLine("	{");
			sb.AppendLine("		private static ConfigurationValues _instance;");
			sb.AppendLine();
			sb.AppendLine("		private string mConnectionString = string.Empty;");
			sb.AppendLine("		private string mDataAccessServiceUrl = string.Empty;");
			sb.AppendLine("		private int _defaultTimeOut = 30;");
			sb.AppendLine();
			sb.AppendLine("		private ConfigurationValues()");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				ConfigurationFile cf = new ConfigurationFile();");
			sb.AppendLine("				if(cf.DirectConnect)");
			sb.AppendLine("					mConnectionString = cf.ConfigString;");
			sb.AppendLine("				else");
			sb.AppendLine("					mDataAccessServiceUrl = cf.ConfigString;");
			sb.AppendLine("			}");
			sb.AppendLine("			catch{}");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				mConnectionString = ConfigurationManager.ConnectionStrings[\"" + DefaultNamespace + "\"].ConnectionString;");
			sb.AppendLine("			}");
			sb.AppendLine("			catch { }");


			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns a singleton configuration object.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static ConfigurationValues GetInstance()");
			sb.AppendLine("		{");
			sb.AppendLine("			if (_instance == null)");
			sb.AppendLine("				_instance = new ConfigurationValues();");
			sb.AppendLine("			return _instance;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the connection string for all static select methods.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string ConnectionString");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return mConnectionString; }");
			sb.AppendLine("			set { mConnectionString = value; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines if the web service is used (remote conection) or this is a direct database connection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		[Obsolete(\"This property no longer applies.\")]");
			sb.AppendLine("		public bool DirectConnect");
			sb.AppendLine("		{");
			sb.AppendLine("			get");
			sb.AppendLine("			{");
			sb.AppendLine("				if(this.mDataAccessServiceUrl == string.Empty) return true;");
			sb.AppendLine("				else return false;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The URL to the web servive that is used to pass database across the wire.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		[Obsolete(\"This property no longer applies.\")]");
			sb.AppendLine("		public string DataAccessServiceUrl");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return mDataAccessServiceUrl; }");
			sb.AppendLine("			set { mDataAccessServiceUrl = value; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The timeout in seconds of the database connection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public int DefaultTimeOut");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return _defaultTimeOut; }");
			sb.AppendLine("			set");
			sb.AppendLine("			{");
			sb.AppendLine("				if (value < 0)");
			sb.AppendLine("					value = 0;");
			sb.AppendLine("				_defaultTimeOut = value;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines the version of the model that created this library.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public string Version");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"" + _model.Version + "\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines if the database version matches the current library model version.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public bool IsDataBaseVersionMatch()");
			sb.AppendLine("		{");
			sb.AppendLine("			string sql = \"SELECT CAST(p.value AS sql_variant) AS [Value] FROM sys.extended_properties AS p WHERE (p.name=N'dbVersion')\";");
			sb.AppendLine("			System.Data.DataSet ds = DatabaseHelper.ExecuteSql(this.ConnectionString, sql, new System.Data.SqlClient.SqlParameter[] { });");
			sb.AppendLine("			string dbVersion = \"\";");
			sb.AppendLine("			if (ds.Tables.Count == 1)");
			sb.AppendLine("				dbVersion = (string)ds.Tables[0].Rows[0][0];");
			sb.AppendLine("			return Version == dbVersion;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		internal static string GetSQLTableMap(string original, string baseColumnSQL, string newAlias, string fieldName)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (!newAlias.Contains(\"[\")) newAlias = \"[\" + newAlias + \"]\";");
			sb.AppendLine("			if (!fieldName.Contains(\"[\")) fieldName = \"[\" + fieldName + \"]\";");
			sb.AppendLine("			return original.Replace(baseColumnSQL, newAlias + \".\" + fieldName + \"\");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("	}");
		}
		#endregion

		#region append regions
		#endregion

		#region append member variables
		#endregion

		#region append constructors
		#endregion

		#region append properties
		#endregion

		#region append methods
		#endregion

		#region append operator overloads
		#endregion

	}
}