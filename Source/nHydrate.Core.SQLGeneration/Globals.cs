#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var model = table.Root as ModelRoot;
            var keyList = new List<string>();

            //If tenant table then the tenant id should be first field
            //if (table.IsTenant)
            //    keyList.Add($"[{model.TenantColumnName}]");

            foreach (var indexColumn in tableIndex.IndexColumnList)
            {
                var column = table.GetColumns().FirstOrDefault(x => new Guid(x.Key) == indexColumn.FieldID);
                keyList.Add($"[{column.DatabaseName}]");
            }
            return string.Join(", ", keyList);
        }

        public static string GetDbClustered(this TableIndex obj) => obj.Clustered ? "CLUSTERED" : "NONCLUSTERED";
    }
}
