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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
	public enum SqlNativeTypes
	{
		image = 34,
		text = 35,
		uniqueidentifier = 36,
		date = 40,
		time = 41,
		datetime2 = 42,
		datetimeoffset = 43,
		tinyint = 48,
		smallint = 52,
		@int = 56,
		smalldatetime = 58,
		real = 59,
		money = 60,
		datetime = 61,
		@float = 62,
		sql_variant = 98,
		ntext = 99,
		bit = 104,
		@decimal = 106,
		numeric = 108,
		smallmoney = 122,
		bigint = 127,
		varbinary = 165,
		varchar = 167,
		binary = 173,
		@char = 175,
		timestamp = 189,
		nvarchar = 231,
		sysname = 231,
		nchar = 239,
		hierarchyid = 240,
		geometry = 240,
		geography = 240,
		xml = 241,
	}

	public interface IImportDomain
	{
		Database Import(string connectionString, IEnumerable<SpecialField> auditFields);
		IEnumerable<string> GetEntityList(string connectionString);
		IEnumerable<string> GetViewList(string connectionString);
		IEnumerable<string> GetStoredProcedureList(string connectionString);
		IEnumerable<string> GetFunctionList(string connectionString);
		Entity GetEntity(string connectionString, string name, IEnumerable<SpecialField> auditFields);
		View GetView(string connectionString, string name, IEnumerable<SpecialField> auditFields);
		StoredProc GetStoredProcedure(string connectionString, string name, IEnumerable<SpecialField> auditFields);
		Function GetFunction(string connectionString, string name, IEnumerable<SpecialField> auditFields);
		IDatabaseHelper DatabaseDomain { get; }
	}
}

