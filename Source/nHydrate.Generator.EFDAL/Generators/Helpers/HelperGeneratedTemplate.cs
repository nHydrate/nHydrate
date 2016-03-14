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
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Generators.Helpers
{
    public class HelperGeneratedTemplate : EFDALBaseTemplate
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

                sb.AppendLine("using System;");
                sb.AppendLine("using System.Data.Objects.DataClasses;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Data;");
                sb.AppendLine("using System.Data.SqlClient;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine();

                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");

                #region DBHelper

                sb.AppendLine("	internal class DBHelper");
                sb.AppendLine("	{");
                sb.AppendLine("		internal static IDbConnection GetConnection()");
                sb.AppendLine("		{");
                sb.AppendLine("			return GetConnection(" + this.GetLocalNamespace() + "." + _model.ProjectName + "Entities.GetConnectionString());");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		internal static IDbConnection GetConnection(string connectionString)");
                sb.AppendLine("		{");
                sb.AppendLine("			return new SqlConnection(connectionString);");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		internal static IDbCommand GetCommand(string commandText, CommandType commandType, IDbConnection connection)");
                sb.AppendLine("		{");
                sb.AppendLine("			var cmd = new SqlCommand(commandText);");
                sb.AppendLine("			cmd.CommandType = commandType;");
                sb.AppendLine("			cmd.Connection = (SqlConnection)connection;");
                sb.AppendLine("			return cmd;");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		internal static void AddParameter(IDbCommand cmd, string parameterName, object value)");
                sb.AppendLine("		{");
                sb.AppendLine("			var sqlParam = new SqlParameter(parameterName, value);");
                sb.AppendLine("			cmd.Parameters.Add(sqlParam);");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		internal static void AddReturnParameter(IDbCommand cmd)");
                sb.AppendLine("		{");
                sb.AppendLine("			var sqlParam = new SqlParameter();");
                sb.AppendLine("			sqlParam.ParameterName = \"@RETURN_VALUE\";");
                sb.AppendLine("			sqlParam.Direction = ParameterDirection.ReturnValue;");
                sb.AppendLine("			cmd.Parameters.Add(sqlParam);");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("	}");
                sb.AppendLine();

                #endregion

                #region EntityContextInterface

                sb.AppendLine("	internal interface IEntityWithContext");
                sb.AppendLine("	{");
                sb.AppendLine("		" + _model.ProjectName + "Entities Context { get; set; }");
                sb.AppendLine("	}");
                sb.AppendLine();

                #endregion

                #region Util
                sb.AppendLine("	internal class Util");
                sb.AppendLine("	{");

                sb.AppendLine("		internal static string ConvertNormalCS2EFFromConfig(string configSettings)");
                sb.AppendLine("		{");
                sb.AppendLine("			return ConvertNormalCS2EFFromConfig(configSettings, new ContextStartup(string.Empty, false, 0));");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		internal static string ConvertNormalCS2EFFromConfig(string configSettings, ContextStartup contextStartup)");
                sb.AppendLine("		{");
                sb.AppendLine("			if (string.IsNullOrEmpty(configSettings)) return configSettings;");
                sb.AppendLine("			var arr = configSettings.Split('=');");
                sb.AppendLine("			if (arr.Length != 2) return configSettings;");
                sb.AppendLine("			if (arr[0] != \"name\") return configSettings;");
                sb.AppendLine("			try");
                sb.AppendLine("			{");
                sb.AppendLine("				var cs = System.Configuration.ConfigurationManager.ConnectionStrings[arr[1]].ConnectionString;");
                sb.AppendLine("				if (!cs.StartsWith(\"metadata=\")) return ConvertNormalCS2EF(cs, contextStartup);");
                sb.AppendLine("				return configSettings;");
                sb.AppendLine("			}");
                sb.AppendLine("			catch");
                sb.AppendLine("			{");
                sb.AppendLine("				return configSettings;");
                sb.AppendLine("			}");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		internal static string ConvertNormalCS2EF(string connectionString)");
                sb.AppendLine("		{");
                sb.AppendLine("			return ConvertNormalCS2EF(connectionString, new ContextStartup(string.Empty, false, 0));");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		internal static string ConvertNormalCS2EF(string connectionString, ContextStartup contextStartup)");
                sb.AppendLine("		{");
                sb.AppendLine("			if (string.IsNullOrEmpty(connectionString)) return connectionString;");
                sb.AppendLine("			if (connectionString.StartsWith(\"metadata=\")) return connectionString;");
                sb.AppendLine();
                sb.AppendLine("			if (contextStartup.IsAdmin)");
                sb.AppendLine("				return @\"metadata=res://*/" + this.GetLocalNamespace() + "." + _model.ProjectName + ".csdl|res://*/" + this.GetLocalNamespace() + "." + _model.ProjectName + ".Admin.ssdl|res://*/" + this.GetLocalNamespace() + "." + _model.ProjectName + ".Admin.msl;provider=System.Data.SqlClient;provider connection string='\" + connectionString + \"'\";");
                sb.AppendLine("			else");
                sb.AppendLine("				return @\"metadata=res://*/" + this.GetLocalNamespace() + "." + _model.ProjectName + ".csdl|res://*/" + this.GetLocalNamespace() + "." + _model.ProjectName + ".ssdl|res://*/" + this.GetLocalNamespace() + "." + _model.ProjectName + ".msl;provider=System.Data.SqlClient;provider connection string='\" + connectionString + \"'\";");
                sb.AppendLine("		}");
                sb.AppendLine();

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
                sb.AppendLine("						if (connectionString.Substring(ii, 1) == \"\\\"\")");
                sb.AppendLine("						{");
                sb.AppendLine("							index1 = ii + 1;");
                sb.AppendLine("							index2 = connectionString.IndexOf(\"\\\"\", ii + 1);");
                sb.AppendLine("						}");
                sb.AppendLine("						else if (connectionString.Substring(ii, 1) == \"'\")");
                sb.AppendLine("						{");
                sb.AppendLine("							index1 = ii + 1;");
                sb.AppendLine("							index2 = connectionString.IndexOf(\"'\", ii + 1);");
                sb.AppendLine("						}");
                sb.AppendLine("						else if (connectionString.Substring(ii, 6) == \"&quot;\")");
                sb.AppendLine("						{");
                sb.AppendLine("							index1 = ii + 6;");
                sb.AppendLine("							index2 = connectionString.IndexOf(\"&quot;\", ii + 1);");
                sb.AppendLine("						}");
                sb.AppendLine("					}");
                sb.AppendLine();
                sb.AppendLine("					if (index1 != -1 && index2 != -1)");
                sb.AppendLine("					{");
                sb.AppendLine("						return connectionString.Substring(index1, index2 - index1);");
                sb.AppendLine("					}");
                sb.AppendLine();
                sb.AppendLine("				}");
                sb.AppendLine();
                sb.AppendLine("			}");
                sb.AppendLine("			return connectionString;");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("	}");
                sb.AppendLine();
                #endregion

                sb.AppendLine("}");
                sb.AppendLine();
                
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

    }
}
