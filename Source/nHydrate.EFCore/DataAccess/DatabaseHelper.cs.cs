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
using System.Data;
using System.Data.SqlClient;

namespace nHydrate.EFCore.DataAccess
{
	public class DatabaseHelper
	{
		public static IDbCommand GetCommand(string commandText, CommandType commandType, IDbConnection connection)
		{
			var cmd = new SqlCommand(commandText);
			cmd.CommandType = commandType;
			cmd.Connection = (SqlConnection)connection;
			return cmd;
		}

		public static void AddParameter(IDbCommand cmd, string parameterName, object value)
		{
			var sqlParam = new SqlParameter(parameterName, value);
			cmd.Parameters.Add(sqlParam);
		}

		public static void AddReturnParameter(IDbCommand cmd)
		{
			var sqlParam = new SqlParameter();
			sqlParam.ParameterName = "@RETURN_VALUE";
			sqlParam.Direction = ParameterDirection.ReturnValue;
			cmd.Parameters.Add(sqlParam);
		}

	}
}

