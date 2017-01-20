#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
using nHydrate.Generator.Models;

namespace nHydrate.Core.SQLGeneration
{
    internal static class Globals
    {
        public static string GetTableDatabaseName(ModelRoot model, Table table)
        {
            var retval = model.Database.TablePrefix;
            if (!string.IsNullOrEmpty(retval))
                return retval + "_" + table.DatabaseName;
            else
                return table.DatabaseName;
        }

        public static string GetSQLIndexField(Table table, TableIndex tableIndex)
        {
            try
            {
                var sb = new StringBuilder();
                var index = 0;
                foreach (var indexColumn in tableIndex.IndexColumnList)
                {
                    var column = table.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == indexColumn.FieldID);
                    sb.Append("[" + column.DatabaseName + "]");
                    if (index < tableIndex.IndexColumnList.Count - 1)
                        sb.Append(",");
                    index++;
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}