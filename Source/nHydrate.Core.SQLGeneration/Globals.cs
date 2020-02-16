#pragma warning disable 0168
using System;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Core.SQLGeneration
{
    internal static class Globals
    {
        public static string GetTableDatabaseName(ModelRoot model, Table table)
        {
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
                    var column = table.GetColumns().FirstOrDefault(x => new Guid(x.Key) == indexColumn.FieldID);
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