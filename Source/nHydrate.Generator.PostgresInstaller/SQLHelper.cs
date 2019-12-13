#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using nHydrate.Generator.ProjectItemGenerators;
using System.Xml;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.PostgresInstaller.ProjectItemGenerators;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Data;

namespace nHydrate.Generator.PostgresInstaller
{
    internal static class SqlHelper
    {
        #region GetModelDifferenceSQL

        //public static string GetModelDifferenceSql(ModelRoot modelOld, ModelRoot modelNew)
        //{
        //    return "NOT IMPLEMENTED";
        //}

        #region TODO
        public static string GetModelDifferenceSql(ModelRoot modelOld, ModelRoot modelNew)
        {
            try
            {
                var sb = new StringBuilder();

                #region Loop and Add tables

                foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                {
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT == null)
                    {
                        //Add table, indexes
                        sb.AppendLine(SQLEmit.GetSQLCreateTable(modelNew, newT));
                        sb.AppendLine("--GO");
                        sb.AppendLine(SQLEmit.GetSqlCreatePK(newT));
                        sb.AppendLine("--GO");

                        //DO NOT process primary keys
                        foreach (var index in newT.TableIndexList.Where(x => !x.PrimaryKey))
                        {
                            sb.Append(SQLEmit.GetSQLCreateIndex(newT, index, false));
                            sb.AppendLine("--GO");
                            sb.AppendLine();
                        }

                        if (newT.StaticData.Count > 0)
                        {
                            sb.Append(SQLEmit.GetSqlInsertStaticData(newT));
                            sb.AppendLine("--GO");
                            sb.AppendLine();
                        }

                        //If this is a tenant table then add the view as well
                        if (newT.IsTenant)
                        {
                            var grantSB = new StringBuilder();
                            var q1 = SQLEmit.GetSqlTenantView(modelNew, newT, grantSB);
                            sb.AppendLine(q1);
                            if (grantSB.ToString() != string.Empty)
                                sb.AppendLine(grantSB.ToString());
                        }

                    }
                }

                #endregion

                //        #region Delete Indexes
                //        foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                //        {
                //            var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                //            if (oldT != null)
                //            {
                //                //If old exists new does NOT, so delete index
                //                foreach (var oldIndex in oldT.TableIndexList)
                //                {
                //                    var newIndex = newT.TableIndexList.FirstOrDefault(x => x.Key == oldIndex.Key);
                //                    if (newIndex == null)
                //                    {
                //                        sb.AppendLine(SQLEmit.GetSQLDropIndex(newT, oldIndex));
                //                        sb.AppendLine("--GO");
                //                    }
                //                }

                //                //Both exist, so if different, drop and re-create
                //                foreach (var newIndex in newT.TableIndexList)
                //                {
                //                    var oldIndex = oldT.TableIndexList.FirstOrDefault(x => x.Key == newIndex.Key);
                //                    if (oldIndex != null && oldIndex.CorePropertiesHashNoNames != newIndex.CorePropertiesHashNoNames)
                //                    {
                //                        sb.AppendLine(SQLEmit.GetSQLDropIndex(newT, oldIndex));
                //                        sb.AppendLine("--GO");
                //                    }
                //                }
                //            }
                //        }
                //        #endregion

                //        #region Loop and DELETE tables
                //        foreach (var oldT in modelOld.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly))
                //        {
                //            var newT = modelNew.Database.Tables.FirstOrDefault(x => x.Generated && (x.TypedTable != TypedTableConstants.EnumOnly) && x.Key.ToLower() == oldT.Key.ToLower());
                //            if (newT == null)
                //            {
                //                //DELETE TABLE
                //                sb.Append(SQLEmit.GetSqlDropTable(modelOld, oldT));
                //                sb.AppendLine("--GO");
                //                //TODO - Delete Tenant View
                //                sb.AppendLine();
                //            }
                //            else if (newT != null && oldT.AllowAuditTracking && !newT.AllowAuditTracking)
                //            {
                //                //If the old model had audit tracking and the new one does not, add a TODO in the script
                //                var tableName = "__AUDIT__" + Globals.GetTableDatabaseName(modelOld, oldT);
                //                sb.AppendLine("--TODO: REMOVE AUDIT TABLE '" + tableName + "'");
                //                sb.AppendLine("--The previous model had audit tracking turn on for table '" + Globals.GetTableDatabaseName(modelOld, oldT) + "' and now it is turned off.");
                //                sb.AppendLine("--The audit table will not be removed automatically. If you want to remove it, uncomment the following script.");
                //                sb.AppendLine("--DROP TABLE [" + tableName + "]");
                //                sb.AppendLine("--GO");
                //                sb.AppendLine();
                //            }
                //            //else if (tList[0].DatabaseName != oldT.DatabaseName)
                //            //{
                //            //  //RENAME TABLE
                //            //  sb.AppendLine("if exists(select * from sys.objects where name = '" + oldT.DatabaseName + "' and type = 'U')");
                //            //  sb.AppendLine("exec sp_rename [" + oldT.DatabaseName + "], [" + tList[0].DatabaseName + "]");
                //            //}
                //        }
                //        #endregion

                //        #region Loop and Modify tables
                //        foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                //        {
                //            var schemaChanged = false;
                //            var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                //            if (oldT != null)
                //            {
                //                var querylist = new List<string>();

                //                #region Rename table if need be
                //                if (oldT.DatabaseName != newT.DatabaseName)
                //                {
                //                    sb.AppendLine(SQLEmit.GetSqlRenameTable(oldT, newT));
                //                    sb.AppendLine("--GO");
                //                }
                //                #endregion

                //                #region Add columns
                //                foreach (var newC in newT.GetColumns())
                //                {
                //                    var oldC = Globals.GetColumnByKey(oldT.Columns, newC.Key);
                //                    if (oldC == null)
                //                    {
                //                        //ADD COLUMN
                //                        sb.AppendLine(SQLEmit.GetSqlAddColumn(newC));
                //                        sb.AppendLine("--GO");
                //                        sb.AppendLine();
                //                        schemaChanged = true;
                //                    }
                //                    //else if (newC.DatabaseName != oldC.DatabaseName)
                //                    //{
                //                    //  //RENAME COLUMN
                //                    //  sb.AppendLine(SQLEmit.GetSQLRenameColumn(oldC, newC));
                //                    //  sb.AppendLine("--GO");
                //                    //  sb.AppendLine();
                //                    //}

                //                }
                //                #endregion

                //                #region Delete Columns
                //                foreach (Reference oldRef in oldT.Columns)
                //                {
                //                    var oldC = oldRef.Object as Column;
                //                    var newC = Globals.GetColumnByKey(newT.Columns, oldC.Key);
                //                    if (newC == null)
                //                    {
                //                        //DELETE COLUMN
                //                        sb.AppendLine(SQLEmit.GetSqlDropColumn(modelNew, oldC));
                //                        sb.AppendLine("--GO");
                //                        sb.AppendLine();
                //                        schemaChanged = true;
                //                    }
                //                    else if (newC.DatabaseName != oldC.DatabaseName)
                //                    {
                //                        ////RENAME COLUMN
                //                        //string sql = "if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + oldC.DatabaseName + "' and o.name = '" + newT.DatabaseName + "')" +
                //                        //             "AND not exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + newC.DatabaseName + "' and o.name = '" + newT.DatabaseName + "')" + Environment.NewLine +
                //                        //             "EXEC sp_rename @objname = '" + newT.DatabaseName + "." + oldC.DatabaseName + "', @newname = '" + newC.DatabaseName + "', @objtype = 'COLUMN'";
                //                        //if (!querylist.Contains(sql))
                //                        //{
                //                        //  querylist.Add(sql);
                //                        //  sb.AppendLine(sql);
                //                        //  sb.AppendLine("--GO");
                //                        //  sb.AppendLine();
                //                        //}
                //                    }

                //                }
                //                #endregion

                //                #region Modify Columns
                //                foreach (var newC in newT.GetColumns())
                //                {
                //                    var oldC = Globals.GetColumnByKey(oldT.Columns, newC.Key);
                //                    if (oldC != null)
                //                    {
                //                        var document = new XmlDocument();
                //                        document.LoadXml("<a></a>");
                //                        var n1 = XmlHelper.AddElement(document.DocumentElement, "q");
                //                        var n2 = XmlHelper.AddElement(document.DocumentElement, "q");
                //                        oldC.XmlAppend(n1);
                //                        newC.XmlAppend(n2);

                //                        //Check column, ignore defaults
                //                        if (newC.CorePropertiesHashNoPK != oldC.CorePropertiesHashNoPK)
                //                        {
                //                            //MODIFY COLUMN
                //                            sb.AppendLine(SQLEmit.GetSqlModifyColumn(oldC, newC));
                //                            sb.AppendLine("--GO");
                //                            sb.AppendLine();
                //                            schemaChanged = true;
                //                        }

                //                        //Drop add defaults if column
                //                        //if ((newC.CorePropertiesHashNoPK != oldC.CorePropertiesHashNoPK) || (oldC.Default != newC.Default))
                //                        //{
                //                        //		if (!string.IsNullOrEmpty(oldC.Default))
                //                        //		{
                //                        //			//Old default was something so drop it
                //                        //			sb.AppendLine(SQLEmit.GetSqlDropColumnDefault(newC));
                //                        //			sb.AppendLine("--GO");
                //                        //			sb.AppendLine();
                //                        //		}

                //                        //	if (!string.IsNullOrEmpty(newC.Default))
                //                        //	{
                //                        //		//New default is something so add it
                //                        //		sb.AppendLine(SQLEmit.GetSqlCreateColumnDefault(modelNew, newC));
                //                        //		sb.AppendLine("--GO");
                //                        //		sb.AppendLine();
                //                        //	}
                //                        //}

                //                        if (!string.IsNullOrEmpty(newC.Default) && ((oldC.Default != newC.Default) || (oldC.DataType != newC.DataType) || (oldC.DatabaseName != newC.DatabaseName)))
                //                        {
                //                            //New default is something so add it
                //                            sb.AppendLine(SQLEmit.GetSqlCreateColumnDefault(modelNew, newC));
                //                            sb.AppendLine("--GO");
                //                            sb.AppendLine();
                //                        }

                //                    }
                //                }
                //                #endregion

                //                #region Process Table Splits
                //                {
                //                    var splits = modelNew.Refactorizations
                //                        .Where(x => x is RefactorTableSplit)
                //                        .Cast<RefactorTableSplit>()
                //                        .Where(x => x.EntityKey1 == new Guid(newT.Key))
                //                        .ToList();

                //                    foreach (var split in splits)
                //                    {
                //                        var splitTable = modelNew.Database.Tables.FirstOrDefault(x => new Guid(x.Key) == split.EntityKey2);
                //                        var origFields = oldT.GeneratedColumns.Where(x => split.ReMappedFieldIDList.Keys.Contains(new Guid(x.Key))).ToList();
                //                        if (splitTable != null && origFields.Count > 0)
                //                        {
                //                            var newFields = new List<Column>();
                //                            foreach (var item in origFields)
                //                            {
                //                                var newF = splitTable.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == split.ReMappedFieldIDList[new Guid(item.Key)]);
                //                                if (newF != null)
                //                                    newFields.Add(newF);
                //                            }

                //                            newFields = newFields.Distinct().ToList();

                //                            //If there are columns then process
                //                            if (newFields.Count > 0)
                //                            {
                //                                sb.AppendLine("--PROCESS TABLE SPLIT [" + newT.DatabaseName + "] -> [" + splitTable.DatabaseName + "]");

                //                                //Get the fields for generation
                //                                //This may be a different number than original split since user can remove fields
                //                                var genFields = new Dictionary<Column, Column>();
                //                                foreach (var f in origFields)
                //                                {
                //                                    //Get the new column from the new table as the name might have changed
                //                                    var newID = split.ReMappedFieldIDList[new Guid(f.Key)];
                //                                    var newF = splitTable.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == newID);
                //                                    if (newF != null)
                //                                    {
                //                                        genFields.Add(f, newF);
                //                                    }
                //                                }

                //                                //Process the actual script fields
                //                                if (genFields.Count > 0)
                //                                {
                //                                    //Turn on identity insert if necessary
                //                                    if (splitTable.PrimaryKeyColumns.Count(x => x.Identity == IdentityTypeConstants.Database) > 0)
                //                                        sb.AppendLine("SET identity_insert [" + splitTable.GetPostgresSchema() + "].[" + Globals.GetTableDatabaseName(modelNew, splitTable) + "] on");

                //                                    sb.Append("INSERT INTO [" + splitTable.GetPostgresSchema() + "].[" + splitTable.DatabaseName + "] (");
                //                                    foreach (var f in genFields.Keys)
                //                                    {
                //                                        //Get the new column from the new table as the name might have changed
                //                                        var newF = genFields[f];
                //                                        sb.Append("[" + newF.DatabaseName + "]");
                //                                        if (genFields.Keys.IndexOf(f) < genFields.Keys.Count - 1) sb.Append(", ");
                //                                    }
                //                                    sb.AppendLine(")");
                //                                    sb.Append("SELECT ");
                //                                    foreach (var f in genFields.Keys)
                //                                    {
                //                                        sb.Append("[" + f.DatabaseName + "]");
                //                                        if (genFields.Keys.IndexOf(f) < genFields.Keys.Count - 1) sb.Append(", ");
                //                                    }

                //                                    sb.AppendLine(" FROM [" + newT.GetPostgresSchema() + "].[" + newT.DatabaseName + "]");

                //                                    //Turn off identity insert if necessary
                //                                    if (splitTable.PrimaryKeyColumns.Count(x => x.Identity == IdentityTypeConstants.Database) > 0)
                //                                        sb.AppendLine("SET identity_insert [" + splitTable.GetPostgresSchema() + "].[" + Globals.GetTableDatabaseName(modelNew, splitTable) + "] off");

                //                                    sb.AppendLine("--GO");
                //                                    sb.AppendLine();
                //                                }
                //                            }
                //                        }
                //                    }
                //                } //Table Splits
                //                #endregion

                //                #region Process Table Combines
                //                {
                //                    var splits = modelNew.Refactorizations
                //                        .Where(x => x is RefactorTableCombine)
                //                        .Cast<RefactorTableCombine>()
                //                        .Where(x => x.EntityKey1 == new Guid(newT.Key))
                //                        .ToList();

                //                    foreach (var split in splits)
                //                    {
                //                        var deletedTable = modelOld.Database.Tables.FirstOrDefault(x => new Guid(x.Key) == split.EntityKey2);
                //                        if (deletedTable != null)
                //                        {
                //                            var deletedOrigDeletedT = modelOld.Database.Tables.GetByKey(deletedTable.Key).FirstOrDefault();
                //                            if (deletedOrigDeletedT != null)
                //                            {
                //                                var deletedFields = deletedOrigDeletedT.GeneratedColumns.Where(x => split.ReMappedFieldIDList.Keys.Contains(new Guid(x.Key))).ToList();
                //                                if (deletedFields.Count > 0)
                //                                {
                //                                    var targetFields = new List<Column>();
                //                                    foreach (var item in deletedFields)
                //                                    {
                //                                        var newF = newT.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == split.ReMappedFieldIDList[new Guid(item.Key)]);
                //                                        if (newF != null)
                //                                            targetFields.Add(newF);
                //                                    }

                //                                    targetFields = targetFields.Distinct().ToList();

                //                                    //If there are columns then process
                //                                    if (targetFields.Count > 0)
                //                                    {
                //                                        //Get the fields for generation
                //                                        //This may be a different number than original combine since user can remove fields
                //                                        var genFields = new Dictionary<Column, Column>();
                //                                        foreach (var f in deletedFields)
                //                                        {
                //                                            //Get the new column from the new table as the name might have changed
                //                                            var newID = split.ReMappedFieldIDList[new Guid(f.Key)];
                //                                            var newF = newT.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == newID);
                //                                            if (newF != null)
                //                                            {
                //                                                genFields.Add(f, newF);
                //                                            }
                //                                        }

                //                                        //Process the actual script fields
                //                                        if (genFields.Count > 0)
                //                                        {
                //                                            sb.AppendLine("--PROCESS TABLE COMBINE [" + deletedTable.DatabaseName + "] into [" + newT.DatabaseName + "]");
                //                                            sb.Append("UPDATE [" + newT.GetPostgresSchema() + "].[" + newT.DatabaseName + "] SET ");
                //                                            foreach (var f in genFields.Keys)
                //                                            {
                //                                                //Get the new column from the new table as the name might have changed
                //                                                var newF = genFields[f];
                //                                                sb.Append("[" + newF.DatabaseName + "] = [_deleted].[" + f.DatabaseName + "]");
                //                                                if (genFields.Keys.IndexOf(f) < genFields.Keys.Count - 1) sb.Append(", ");
                //                                            }
                //                                            sb.Append(" FROM [" + newT.GetPostgresSchema() + "].[" + newT.DatabaseName + "] AS [_A] INNER JOIN ");
                //                                            sb.Append("[" + deletedTable.GetPostgresSchema() + "].[" + deletedTable.DatabaseName + "] AS [_deleted] ON ");
                //                                            sb.Append("[_A].[" + newT.PrimaryKeyColumns.First().DatabaseName + "] = [_deleted].[" + deletedTable.PrimaryKeyColumns.First().DatabaseName + "]");
                //                                            sb.AppendLine();
                //                                            sb.AppendLine("--GO");
                //                                            sb.AppendLine();
                //                                        }

                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                } //Table Combine
                //                #endregion

                //                #region Tenant
                //                if (oldT.IsTenant && !newT.IsTenant)
                //                {
                //                    //Drop default
                //                    var defaultName = "DF__" + newT.DatabaseName.ToUpper() + "_" + modelNew.TenantColumnName.ToUpper();
                //                    sb.AppendLine("--DELETE TENANT DEFAULT FOR [" + newT.DatabaseName + "]");
                //                    sb.AppendLine("if exists (select name from sys.objects where name = '" + defaultName + "'  AND type = 'D')");
                //                    sb.AppendLine("ALTER TABLE [" + newT.GetPostgresSchema() + "].[" + newT.DatabaseName + "] DROP CONSTRAINT [" + defaultName + "]");
                //                    sb.AppendLine();

                //                    if (newT.PascalName != newT.DatabaseName)
                //                    {
                //                        //This is for the mistake in name when released. Remove this default June 2013
                //                        defaultName = $"DF__{newT.PascalName}_{modelNew.TenantColumnName}".ToUpper();
                //                        sb.AppendLine($"--DELETE TENANT DEFAULT FOR [{newT.DatabaseName}]");
                //                        sb.AppendLine($"if exists (select name from sys.objects where name = '{defaultName}'  AND type = 'D')");
                //                        sb.AppendLine($"ALTER TABLE [{newT.GetPostgresSchema()}].[{newT.DatabaseName}] DROP CONSTRAINT [{defaultName}]");
                //                        sb.AppendLine();
                //                    }

                //                    //Drop Index
                //                    var indexName = "IDX_" + newT.DatabaseName.Replace("-", string.Empty) + "_" + modelNew.TenantColumnName;
                //                    indexName = indexName.ToUpper();
                //                    sb.AppendLine($"if exists (select * from sys.indexes where name = '{indexName}')");
                //                    sb.AppendLine($"DROP INDEX [{indexName}] ON [{newT.GetPostgresSchema()}].[{newT.DatabaseName}]");
                //                    sb.AppendLine();

                //                    //Drop the associated view
                //                    var viewName = $"{modelOld.TenantPrefix}_{oldT.DatabaseName}";
                //                    sb.AppendLine($"if exists (select name from sys.objects where name = '{viewName}'  AND type = 'V')");
                //                    sb.AppendLine($"DROP VIEW [{viewName}]");
                //                    sb.AppendLine();

                //                    //Drop the tenant field
                //                    sb.AppendLine($"if exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id where c.name = '{modelNew.TenantColumnName}' and t.name = '{newT.DatabaseName}')");
                //                    sb.AppendLine($"ALTER TABLE [{newT.GetPostgresSchema()}].[{newT.DatabaseName}] DROP COLUMN [{modelNew.TenantColumnName}]");
                //                    sb.AppendLine();
                //                }
                //                else if (!oldT.IsTenant && newT.IsTenant)
                //                {
                //                    //Add the tenant field
                //                    sb.AppendLine(SQLEmit.GetSqlCreateTenantColumn(modelNew, newT));

                //                    //Add tenant view
                //                    var grantSB = new StringBuilder();
                //                    sb.AppendLine(SQLEmit.GetSqlTenantView(modelNew, newT, grantSB));
                //                    if (grantSB.ToString() != string.Empty)
                //                        sb.AppendLine(grantSB.ToString());
                //                }
                //                else if (oldT.IsTenant && newT.IsTenant && oldT.DatabaseName != newT.DatabaseName)
                //                {
                //                    //If rename tenant table then delete old view and create new view

                //                    //Drop the old view
                //                    var viewName = modelOld.TenantPrefix + "_" + oldT.DatabaseName;
                //                    sb.AppendLine($"--DROP OLD TENANT VIEW FOR TABLE [{oldT.DatabaseName}]");
                //                    sb.AppendLine($"if exists (select name from sys.objects where name = '{viewName}'  AND type = 'V')");
                //                    sb.AppendLine($"DROP VIEW [{viewName}]");
                //                    sb.AppendLine("--GO");

                //                    //Add tenant view
                //                    var grantSB = new StringBuilder();
                //                    sb.AppendLine(SQLEmit.GetSqlTenantView(modelNew, newT, grantSB));
                //                    if (grantSB.ToString() != string.Empty)
                //                        sb.AppendLine(grantSB.ToString());

                //                }
                //                #endregion

                //                #region Primary Key Changed

                //                //If the primary key changed, then generate a commented script that marks where the user can manually intervene
                //                var newPKINdex = newT.TableIndexList.FirstOrDefault(x => x.PrimaryKey);
                //                var oldPKINdex = oldT.TableIndexList.FirstOrDefault(x => x.PrimaryKey);
                //                if (newPKINdex != null && oldPKINdex != null)
                //                {
                //                    var newPKHash = newPKINdex.CorePropertiesHash;
                //                    var oldPKHash = oldPKINdex.CorePropertiesHash;
                //                    if (newPKHash != oldPKHash)
                //                    {
                //                        sb.AppendLine();
                //                        sb.AppendLine("--GENERATION NOTE **");
                //                        sb.AppendLine("--THE PRIMARY KEY HAS CHANGED, THIS MAY REQUIRE MANUAL INTERVENTION");
                //                        sb.AppendLine("--THE FOLLOWING SCRIPT WILL DROP AND READD THE PRIMARY KEY HOWEVER IF THERE ARE RELATIONSHIPS");
                //                        sb.AppendLine("--BASED ON THIS IT, THE SCRIPT WILL FAIL. YOU MUST DROP ALL FOREIGN KEYS FIRST.");
                //                        sb.AppendLine();

                //                        //Before drop PK remove all FK to the table
                //                        foreach (var r1 in oldT.GetRelations().ToList())
                //                        {
                //                            sb.Append(SQLEmit.GetSqlRemoveFK(r1));
                //                            sb.AppendLine("--GO");
                //                            sb.AppendLine();
                //                        }

                //                        var tableName = Globals.GetTableDatabaseName(modelNew, newT);
                //                        var pkName = "PK_" + tableName;
                //                        pkName = pkName.ToUpper();
                //                        sb.AppendLine($"----DROP PRIMARY KEY FOR TABLE [{tableName}]");
                //                        sb.AppendLine($"--if exists(select * from sys.objects where name = '{pkName}' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')");
                //                        sb.AppendLine($"--ALTER TABLE [{newT.GetPostgresSchema()}].[{tableName}] DROP CONSTRAINT [{pkName}]");
                //                        sb.AppendLine("--GO");

                //                        var sql = SQLEmit.GetSqlCreatePK(newT) + "GO\r\n";
                //                        var lines = sql.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                //                        //Comment the whole SQL block
                //                        var index = 0;
                //                        foreach (var s in lines)
                //                        {
                //                            var l = s;
                //                            l = "--" + l;
                //                            lines[index] = l;
                //                            index++;
                //                        }

                //                        sb.AppendLine(string.Join("\r\n", lines));
                //                        sb.AppendLine();
                //                    }
                //                }

                //                #endregion

                //                #region Drop Foreign Keys
                //                foreach (var r1 in oldT.GetRelations().ToList())
                //                {
                //                    var r2 = newT.Relationships.FirstOrDefault(x => x.Key == r1.Key);
                //                    if (r2 == null)
                //                    {
                //                        sb.Append(SQLEmit.GetSqlRemoveFK(r1));
                //                        sb.AppendLine("--GO");
                //                        sb.AppendLine();
                //                    }
                //                }
                //                #endregion

                //                #region Rename audit columns if necessary
                //                if (modelOld.Database.CreatedByColumnName != modelNew.Database.CreatedByColumnName)
                //                {
                //                    sb.AppendLine(SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.CreatedByColumnName, modelNew.Database.CreatedByColumnName));
                //                    sb.AppendLine("--GO");
                //                }
                //                if (modelOld.Database.CreatedDateColumnName != modelNew.Database.CreatedDateColumnName)
                //                {
                //                    sb.AppendLine(SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.CreatedDateColumnName, modelNew.Database.CreatedDateColumnName));
                //                    sb.AppendLine("--GO");
                //                }
                //                if (modelOld.Database.ModifiedByColumnName != modelNew.Database.ModifiedByColumnName)
                //                {
                //                    sb.AppendLine(SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.ModifiedByColumnName, modelNew.Database.ModifiedByColumnName));
                //                    sb.AppendLine("--GO");
                //                }
                //                if (modelOld.Database.ModifiedDateColumnName != modelNew.Database.ModifiedDateColumnName)
                //                {
                //                    sb.AppendLine(SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.ModifiedDateColumnName, modelNew.Database.ModifiedDateColumnName));
                //                    sb.AppendLine("--GO");
                //                }
                //                if (modelOld.Database.TimestampColumnName != modelNew.Database.TimestampColumnName)
                //                {
                //                    sb.AppendLine(SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.TimestampColumnName, modelNew.Database.TimestampColumnName));
                //                    sb.AppendLine("--GO");
                //                }
                //                #endregion

                //                #region Emit Tenant View if need be

                //                //If the table schema has changed then emit the Tenant view
                //                if (schemaChanged && newT.IsTenant)
                //                {
                //                    var grantSB = new StringBuilder();
                //                    var q1 = SQLEmit.GetSqlTenantView(modelNew, newT, grantSB);
                //                    sb.AppendLine(q1);
                //                    if (grantSB.ToString() != string.Empty)
                //                        sb.AppendLine(grantSB.ToString());
                //                }

                //                #endregion

                //                #region Static Data

                //                //For right now just emit NEW if different.
                //                //TODO: Generate difference scripts for delete and change too.
                //                var oldStaticScript = SQLEmit.GetSqlInsertStaticData(oldT);
                //                var newStaticScript = SQLEmit.GetSqlInsertStaticData(newT);
                //                if (oldStaticScript != newStaticScript)
                //                {
                //                    sb.AppendLine(newStaticScript);
                //                    sb.AppendLine(SQLEmit.GetSqlUpdateStaticData(oldT, newT));
                //                    sb.AppendLine("--GO");
                //                    sb.AppendLine();
                //                }

                //                #endregion

                //                //TODO - Check hash porperties and if changed recompile tenant view

                //            }
                //        }

                //        //Do another look for second pass at changes.
                //        //These things can only be done after the above loop
                //        foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                //        {
                //            var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                //            if (oldT != null)
                //            {
                //                #region Add Foreign Keys

                //                foreach (var r1 in newT.GetRelations().ToList())
                //                {
                //                    var r2 = oldT.GetRelations().ToList().FirstOrDefault(x => x.Key == r1.Key);
                //                    if (r2 == null)
                //                    {
                //                        //There is no OLD relation so it is new so add it
                //                        sb.Append(SQLEmit.GetSqlAddFK(r1));
                //                        sb.AppendLine("--GO");
                //                        sb.AppendLine();
                //                    }
                //                    else if (r1.CorePropertiesHash != r2.CorePropertiesHash)
                //                    {
                //                        //The relation already exists and it has changed, so drop and re-add
                //                        sb.Append(SQLEmit.GetSqlRemoveFK(r2));
                //                        sb.AppendLine("--GO");
                //                        sb.AppendLine();
                //                        sb.Append(SQLEmit.GetSqlAddFK(r1));
                //                        sb.AppendLine("--GO");
                //                        sb.AppendLine();
                //                    }
                //                }

                //                #endregion
                //            }
                //        }

                //        #endregion

                //        #region Move tables between schemas

                //        var reschema = 0;
                //        foreach (Table newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly))
                //        {
                //            var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                //            if (oldT != null)
                //            {
                //                if (string.Compare(oldT.GetPostgresSchema(), newT.GetPostgresSchema(), true) != 0)
                //                {
                //                    if (reschema == 0)
                //                        sb.AppendLine("--MOVE TABLES TO PROPER SCHEMA IF NEED BE");

                //                    //This table has changed schema so script it
                //                    sb.AppendLine("--CREATE DATABASE SCHEMAS");
                //                    sb.AppendLine("if not exists(select * from sys.schemas where name = '" + newT.GetPostgresSchema() + "')");
                //                    sb.AppendLine("exec('CREATE SCHEMA [" + newT.GetPostgresSchema() + "]')");
                //                    sb.AppendLine("--GO");
                //                    sb.AppendLine("if exists (select * from sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id where t.name = '" + newT.DatabaseName + "' and s.name = '" + oldT.GetPostgresSchema() + "')");
                //                    sb.AppendLine("	ALTER SCHEMA [" + newT.GetPostgresSchema() + "] TRANSFER [" + oldT.GetPostgresSchema() + "].[" + newT.DatabaseName + "];");
                //                    sb.AppendLine("--GO");
                //                    reschema++;
                //                }
                //            }
                //        }

                //        if (reschema > 0) sb.AppendLine();

                //        #endregion

                //        #region Add Indexes
                //        foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
                //        {
                //            var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                //            if (oldT != null)
                //            {
                //                //If old exists and does old NOT, so create index
                //                foreach (var newIndex in newT.TableIndexList)
                //                {
                //                    var oldIndex = oldT.TableIndexList.FirstOrDefault(x => x.Key == newIndex.Key);
                //                    if (oldIndex == null)
                //                    {
                //                        sb.AppendLine(SQLEmit.GetSQLCreateIndex(newT, newIndex, false));
                //                        sb.AppendLine("--GO");
                //                        sb.AppendLine();
                //                    }
                //                }

                //                //Both exist, so if different, drop and re-create
                //                foreach (var newIndex in newT.TableIndexList)
                //                {
                //                    var oldIndex = oldT.TableIndexList.FirstOrDefault(x => x.Key == newIndex.Key);
                //                    if (oldIndex != null && oldIndex.CorePropertiesHashNoNames != newIndex.CorePropertiesHashNoNames)
                //                    {
                //                        sb.AppendLine(SQLEmit.GetSQLCreateIndex(newT, newIndex, false));
                //                        sb.AppendLine("--GO");
                //                        sb.AppendLine();
                //                    }
                //                }
                //            }
                //        }
                //        #endregion

                //        #region Add/Remove deleted SP, Views, and Funcs

                //        //Stored procedures
                //        var removedItems = 0;
                //        foreach (var oldT in modelOld.Database.CustomStoredProcedures.OrderBy(x => x.Name))
                //        {
                //            var newT = modelNew.Database.CustomStoredProcedures.FirstOrDefault(x => x.Key == oldT.Key);
                //            if (newT == null)
                //            {
                //                sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('P'))");
                //                sb.AppendLine("drop procedure [" + oldT.DatabaseName + "]");
                //                removedItems++;
                //            }
                //            else if (newT.DatabaseName != oldT.DatabaseName)
                //            {
                //                //Name changed so remove old
                //                sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('P'))");
                //                sb.AppendLine("drop procedure [" + oldT.DatabaseName + "]");
                //                removedItems++;
                //            }
                //        }

                //        if (removedItems > 0)
                //        {
                //            sb.AppendLine("--GO");
                //            sb.AppendLine();
                //        }

                //        foreach (var newT in modelNew.Database.CustomStoredProcedures.OrderBy(x => x.Name))
                //        {
                //            var oldT = modelOld.Database.CustomStoredProcedures.FirstOrDefault(x => x.Key == newT.Key);
                //            if (oldT == null || (oldT.CorePropertiesHash != newT.CorePropertiesHash))
                //            {
                //                sb.Append(SQLEmit.GetSQLCreateStoredProc(newT, false));
                //            }
                //        }

                //        //Views
                //        removedItems = 0;
                //        foreach (var oldT in modelOld.Database.CustomViews.OrderBy(x => x.Name))
                //        {
                //            var newT = modelNew.Database.CustomViews.FirstOrDefault(x => x.Key == oldT.Key);
                //            if (newT == null)
                //            {
                //                sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('V'))");
                //                sb.AppendLine("drop view [" + oldT.DatabaseName + "]");
                //                removedItems++;
                //            }
                //            else if (newT.DatabaseName != oldT.DatabaseName)
                //            {
                //                //Name changed so remove old
                //                sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('V'))");
                //                sb.AppendLine("drop view [" + oldT.DatabaseName + "]");
                //                removedItems++;
                //            }
                //        }

                //        if (removedItems > 0)
                //        {
                //            sb.AppendLine("--GO");
                //            sb.AppendLine();
                //        }

                //        foreach (var newT in modelNew.Database.CustomViews.OrderBy(x => x.Name))
                //        {
                //            var oldT = modelOld.Database.CustomViews.FirstOrDefault(x => x.Key == newT.Key);
                //            if (oldT == null || (oldT.CorePropertiesHash != newT.CorePropertiesHash))
                //            {
                //                sb.Append(SQLEmit.GetSqlCreateView(newT, false));
                //            }
                //        }

                //        //Functions
                //        removedItems = 0;
                //        foreach (var oldT in modelOld.Database.Functions.OrderBy(x => x.Name))
                //        {
                //            var newT = modelNew.Database.Functions.FirstOrDefault(x => x.Key == oldT.Key);
                //            if (newT == null)
                //            {
                //                sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('FN','IF','TF','FS','FT'))");
                //                sb.AppendLine("drop function [" + oldT.DatabaseName + "]");
                //                removedItems++;
                //            }
                //            else if (newT.DatabaseName != oldT.DatabaseName)
                //            {
                //                //Name changed so remove old
                //                sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('FN','IF','TF','FS','FT'))");
                //                sb.AppendLine("drop function [" + oldT.DatabaseName + "]");
                //                removedItems++;
                //            }
                //        }

                //        if (removedItems > 0)
                //        {
                //            sb.AppendLine("--GO");
                //            sb.AppendLine();
                //        }

                //        foreach (var newT in modelNew.Database.Functions.OrderBy(x => x.Name))
                //        {
                //            var oldT = modelOld.Database.Functions.FirstOrDefault(x => x.Key == newT.Key);
                //            if (oldT == null || (oldT.CorePropertiesHash != newT.CorePropertiesHash))
                //            {
                //                sb.Append(SQLEmit.GetSQLCreateFunction(newT, false, modelNew.EFVersion));
                //            }
                //        }

                //        #endregion

                //        #region Add/Remove Audit fields

                //        foreach (var newT in modelNew.Database.Tables.OrderBy(x => x.Name))
                //        {
                //            var oldT = modelOld.Database.Tables.FirstOrDefault(x => x.Key == newT.Key);
                //            if (oldT != null)
                //            {
                //                if (!oldT.AllowCreateAudit && newT.AllowCreateAudit)
                //                    Globals.AppendCreateAudit(newT, modelNew, sb);
                //                if (!oldT.AllowModifiedAudit && newT.AllowModifiedAudit)
                //                    Globals.AppendModifiedAudit(newT, modelNew, sb);
                //                if (!oldT.AllowTimestamp && newT.AllowTimestamp)
                //                    Globals.AppendTimestampAudit(newT, modelNew, sb);

                //                if (oldT.AllowCreateAudit && !newT.AllowCreateAudit)
                //                    Globals.DropCreateAudit(newT, modelNew, sb);
                //                if (oldT.AllowModifiedAudit && !newT.AllowModifiedAudit)
                //                    Globals.DropModifiedAudit(newT, modelNew, sb);
                //                if (oldT.AllowTimestamp && !newT.AllowTimestamp)
                //                    Globals.DropTimestampAudit(newT, modelNew, sb);
                //            }
                //        }

                //        #endregion

                //        #region Loop and change computed fields

                //        foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                //        {
                //            //If the table exists...
                //            var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                //            if (oldT != null)
                //            {
                //                var tChanged = false;
                //                //If there is a computed field with a different value
                //                foreach (var newC in newT.GetColumns().Where(x => x.ComputedColumn).ToList())
                //                {
                //                    var oldC = Globals.GetColumnByKey(oldT.Columns, newC.Key);
                //                    if (oldC != null && oldC.Formula != newC.Formula)
                //                    {
                //                        tChanged = true;
                //                        sb.AppendLine($"if exists(select t.name, c.name from sys.columns c inner join sys.tables t on c.object_id = t.object_id inner join sys.schemas s on t.schema_id = s.schema_id where and t.name = '{newT.DatabaseName}' and c.name = '{newC.DatabaseName}' and s.name = '{newT.GetPostgresSchema()}')");
                //                        sb.AppendLine($"ALTER TABLE [{newT.GetPostgresSchema()}].[{newT.DatabaseName}] DROP COLUMN [{newC.DatabaseName}]");
                //                        sb.AppendLine("--GO");
                //                        sb.AppendLine($"ALTER TABLE [{newT.GetPostgresSchema()}].[{newT.DatabaseName}] ADD [{newC.DatabaseName}] AS ({newC.Formula})");
                //                        sb.AppendLine("--GO");
                //                        sb.AppendLine();
                //                    }
                //                }

                //                if (newT.IsTenant && tChanged)
                //                {
                //                    var grantSB = new StringBuilder();
                //                    var q1 = SQLEmit.GetSqlTenantView(modelNew, newT, grantSB);
                //                    sb.AppendLine(q1);
                //                    if (grantSB.ToString() != string.Empty)
                //                        sb.AppendLine(grantSB.ToString());
                //                }
                //            }
                //        }

                //        #endregion

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #endregion

        #region GetPostgresSchema

        public static string GetPostgresSchema(this Table item)
        {
            if (string.IsNullOrEmpty(item.DBSchema)) return "public";
            return item.DBSchema;
        }

        public static string GetPostgresSchema(this TableComponent item)
        {
            if (string.IsNullOrEmpty(item.Parent.DBSchema)) return "public";
            return item.Parent.DBSchema;
        }

        public static string GetPostgresSchema(this CustomStoredProcedure item)
        {
            if (string.IsNullOrEmpty(item.DBSchema)) return "public";
            return item.DBSchema;
        }

        public static string GetPostgresSchema(this CustomView item)
        {
            if (string.IsNullOrEmpty(item.DBSchema)) return "public";
            return item.DBSchema;
        }

        #endregion

    }

    internal static class SQLEmit
    {
        public static string GetSQLCreateTable(ModelRoot model, Table table, string tableAliasName = null, bool emitPK = true)
        {
            try
            {
                if (table.TypedTable == TypedTableConstants.EnumOnly)
                    return string.Empty;

                var sb = new StringBuilder();
                var tableName = Globals.GetTableDatabaseName(model, table);
                if (!string.IsNullOrEmpty(tableAliasName))
                    tableName = tableAliasName;

                sb.AppendLine("--CREATE TABLE [" + tableName + "]");
                sb.AppendLine($"CREATE TABLE IF NOT EXISTS \"{table.GetPostgresSchema()}\".\"{tableName}\" (");

                var firstLoop = true;
                foreach (var column in table.GeneratedColumns.OrderBy(x => x.SortOrder))
                {
                    if (!firstLoop) sb.AppendLine(",");
                    else firstLoop = false;
                    sb.Append("\t" + AppendColumnDefinition(column, allowDefault: true, allowIdentity: true));
                }
                AppendModifiedAudit(model, table, sb);
                AppendCreateAudit(model, table, sb);
                AppendTimestamp(model, table, sb);

                //Emit PK
                var tableIndex = table.TableIndexList.FirstOrDefault(x => x.PrimaryKey);
                if (tableIndex != null && emitPK)
                {
                    var indexName = "PK_" + table.DatabaseName.ToUpper();
                    sb.AppendLine(",");
                    //var clustered = tableIndex.Clustered ? "CLUSTERED" : "NONCLUSTERED";
                    var clustered = string.Empty; //TEMP until figure out clustered
                    sb.AppendLine($"\tCONSTRAINT \"{indexName}\" PRIMARY KEY {clustered}");
                    sb.AppendLine("\t" + "(");
                    sb.AppendLine("\t\t" + GetSQLIndexField(table, tableIndex));
                    sb.AppendLine("\t" + ")");
                }
                else
                    sb.AppendLine();

                sb.AppendLine(");");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetSqlAddColumn(Column column)
        {
            return GetSqlAddColumn(column, true);
        }

        public static string GetSqlAddColumn(Column column, bool useComment)
        {
            if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var tName = column.ParentTable.DatabaseName;

            if (useComment)
                sb.AppendLine($"--ADD COLUMN [{tName}].[{column.DatabaseName}]");

            sb.AppendLine($"ALTER TABLE \"{tName}\" ADD COLUMN IF NOT EXISTS " + AppendColumnDefinition(column, allowDefault: true, allowIdentity: true) + ";");

            return sb.ToString();
        }

        private static string AppendColumnDefinition(Column column, bool allowDefault, bool allowIdentity)
        {
            return AppendColumnDefinition(column, allowDefault: allowDefault, allowIdentity: allowIdentity, forceNull: false, allowFormula: true, allowComputed: true);
        }

        private static string AppendColumnDefinition(Column column, bool allowDefault, bool allowIdentity, bool forceNull)
        {
            return AppendColumnDefinition(column, allowDefault: allowDefault, allowIdentity: allowIdentity, forceNull: forceNull, allowFormula: true, allowComputed: true);
        }

        private static string AppendColumnDefinition(Column column, bool allowDefault, bool allowIdentity, bool forceNull, bool allowFormula)
        {
            return AppendColumnDefinition(column, allowDefault: allowDefault, allowIdentity: allowIdentity, forceNull: forceNull, allowFormula: true, allowComputed: true);
        }

        private static string AppendColumnDefinition(Column column, bool allowDefault, bool allowIdentity, bool forceNull, bool allowFormula, bool allowComputed)
        {
            var sb = new StringBuilder();

            if (!allowComputed || !column.ComputedColumn)
            {
                //Add column
                sb.Append("\"" + column.DatabaseName + "\" " + column.PostgresDatabaseType());

                //Add Identity
                if (allowIdentity && (column.Identity == IdentityTypeConstants.Database))
                {
                    if (column.DataType == SqlDbType.UniqueIdentifier)
                        sb.Append(" DEFAULT uuid_generate_v4()");
                    else
                        sb.Append(" GENERATED ALWAYS AS IDENTITY");
                }

                //Add NULLable
                if (!forceNull && !column.AllowNull) sb.Append(" NOT");
                sb.Append(" NULL");

                //Add default value
                var defaultValue = GetDefaultValueClause(column);
                if (allowDefault && defaultValue != null)
                    sb.Append(" " + GetDefaultValueClause(column));
            }
            else
            {
                //TODO: computed columns not supported
                sb.Append("COMPUTED COLUMNS NOT SUPPORTED!!!");
            }
            return sb.ToString();

        }

        public static string PostgresDatabaseType(this Column column)
        {
            //https://severalnines.com/database-blog/migrating-mssql-postgresql-what-you-should-know
            switch (column.DataType)
            {
                case SqlDbType.BigInt: return "BIGINT";
                case SqlDbType.Binary: return "BYTEA";
                case SqlDbType.Bit: return "BOOLEAN";
                case SqlDbType.Char: return "CHAR";
                case SqlDbType.DateTime: return "TIMESTAMP"; //precision 3
                case SqlDbType.Decimal: return "DOUBLE PRECISION";
                case SqlDbType.Float: return "DOUBLE PRECISION";
                case SqlDbType.Image: return "BYTEA";
                case SqlDbType.Int: return "INTEGER";
                case SqlDbType.Money: return "MONEY";
                case SqlDbType.NChar: return "CHAR";
                case SqlDbType.NText: return "TEXT";
                case SqlDbType.NVarChar:
                    if (column.Length == 0) return "TEXT";
                    else return "VARCHAR";
                case SqlDbType.Real: return "DOUBLE PRECISION";
                case SqlDbType.UniqueIdentifier: return "UUID";
                case SqlDbType.SmallDateTime: return "TIMESTAMP"; //precision 0
                case SqlDbType.SmallInt: return "SMALLINT";
                case SqlDbType.SmallMoney: return "MONEY";
                case SqlDbType.Text: return "TEXT";
                case SqlDbType.Timestamp: return "TIMESTAMP";
                case SqlDbType.TinyInt: return "SMALLINT";
                case SqlDbType.VarBinary: return "BYTEA";
                case SqlDbType.VarChar:
                    if (column.Length == 0) return "TEXT";
                    else return "VARCHAR";
                case SqlDbType.Variant: return "BYTEA";
                case SqlDbType.Xml: return "TEXT";
                case SqlDbType.Udt: throw new Exception("Udt not implemented");
                case SqlDbType.Structured: throw new Exception("Structured not implemented");
                case SqlDbType.Date: return "DATE";
                case SqlDbType.Time: return "TIME";
                case SqlDbType.DateTime2: return "TIMESTAMP"; //same precision as SQL
                case SqlDbType.DateTimeOffset: return "TIMESTAMP";
                default:
                    throw new Exception("Unknown data type");
            }

        }

        public static string GetDetailSQLValue(Column column)
        {
            var tempBuilder = new StringBuilder();

            var defaultValue = column.Default + string.Empty;
            if ((column.DataType == System.Data.SqlDbType.DateTime) || (column.DataType == System.Data.SqlDbType.SmallDateTime))
            {
                if (defaultValue.ToLower() == "getdate" || defaultValue.ToLower() == "getdate()" ||
                    defaultValue.ToLower() == "sysdatetime" || defaultValue.ToLower() == "sysdatetime()")
                {
                    tempBuilder.Append("CURRENT_TIMESTAMP");
                }
                else if (defaultValue.ToLower() == "getutcdate" || defaultValue.ToLower() == "getutcdate()")
                {
                    //TODO: Use UTC
                    tempBuilder.Append("CURRENT_TIMESTAMP");
                }
            }
            else if (column.DataType == SqlDbType.UniqueIdentifier)
            {
                if (defaultValue.ToLower() == "newid" ||
                    defaultValue.ToLower() == "newid()" ||
                    defaultValue.ToLower() == "newsequentialid" ||
                    defaultValue.ToLower() == "newsequentialid()" ||
                    column.Identity == IdentityTypeConstants.Database)
                {
                    tempBuilder.Append(GetDefaultValue(defaultValue));
                }
                else
                {
                    var v = GetDefaultValue(defaultValue
                        .Replace("'", string.Empty)
                        .Replace("\"", string.Empty)
                        .Replace("{", string.Empty)
                        .Replace("}", string.Empty));

                    Guid g;
                    if (Guid.TryParse(v, out g))
                        tempBuilder.Append("'" + g.ToString() + "'");
                }
            }
            else if (column.DataType == SqlDbType.Bit)
            {
                var d = defaultValue.ToLower();
                if ((d == "false") || (d == "0"))
                    tempBuilder.Append("0");
                else if ((d == "true") || (d == "1"))
                    tempBuilder.Append("1");
            }
            else if (column.IsBinaryType)
            {
                tempBuilder.Append(GetDefaultValue(defaultValue));
            }
            else if (ModelHelper.DefaultIsString(column.DataType) && !string.IsNullOrEmpty(defaultValue))
            {
                if (!column.DefaultIsFunc)
                    tempBuilder.Append("'");

                tempBuilder.Append(GetDefaultValue(defaultValue));

                if (!column.DefaultIsFunc)
                    tempBuilder.Append("'");
            }
            else
            {
                tempBuilder.Append(GetDefaultValue(defaultValue));
            }
            return tempBuilder.ToString();
        }

        internal static string GetDefaultValueClause(Column column)
        {
            var sb = new StringBuilder();
            var theValue = GetDetailSQLValue(column);
            if (!string.IsNullOrEmpty(theValue))
            {
                var tempBuilder = new StringBuilder();
                tempBuilder.Append("DEFAULT " + theValue);
                sb.Append(tempBuilder.ToString());
            }
            return sb.ToString();
        }

        private static string GetDefaultValue(string modelDefault)
        {
            var retVal = modelDefault;
            if (StringHelper.Match(modelDefault, "newid") || StringHelper.Match(modelDefault, "newid()"))
            {
                retVal = "uuid_generate_v4";
            }
            else if (StringHelper.Match(modelDefault, "getdate") || StringHelper.Match(modelDefault, "getdate()") ||
                StringHelper.Match(modelDefault, "sysdatetime") || StringHelper.Match(modelDefault, "sysdatetime()"))
            {
                retVal = "current_timestamp";
            }
            else if (StringHelper.Match(modelDefault, "getutcdate") || StringHelper.Match(modelDefault, "getutcdate()"))
            {
                //TODO: what to do for UTC
                retVal = "current_timestamp";
            }
            else if ((modelDefault == "''") || (modelDefault == "\"\""))
            {
                retVal = string.Empty;
            }
            return retVal;
        }

        public static string GetIndexName(Table table, TableIndex index)
        {
            //Make sure that the index name is the same each time
            var columnList = GetIndexColumns(table, index);
            var prefix = (index.PrimaryKey ? "PK" : "IDX");
            var indexName = prefix + "_" + table.Name.Replace("-", "") + "_" + string.Join("_", columnList.Select(x => x.Value.Name));
            indexName = indexName.ToUpper();
            return indexName;
        }

        public static string GetSQLCreateIndex(Table table, TableIndex index, bool includeDrop)
        {
            var sb = new StringBuilder();
            var model = table.Root as ModelRoot;
            var tableName = Globals.GetTableDatabaseName(model, table);
            var columnList = GetIndexColumns(table, index);
            var indexName = GetIndexName(table, index);

            if (columnList.Count > 0)
            {
                if (includeDrop)
                {
                    sb.AppendLine("--##SECTION BEGIN [SAFETY INDEX TYPE]");

                    //TODO: If this is to be a clustered index then check if it exists and is non-clustered and remove it
                    //TODO: If this is to be a non-clustered index then check if it exists and is clustered and remove it
                    sb.AppendLine("--DELETE INDEX");
                    sb.AppendLine($"DROP INDEX IF EXISTS \"{indexName}\"");
                    sb.AppendLine("--GO");
                    sb.AppendLine("--##SECTION END [SAFETY INDEX TYPE]");
                    sb.AppendLine();
                }

                //Do not create unique index for PK (it is already unique)
                if (!index.PrimaryKey)
                {
                    //TODO: handle UNIQUE
                    //TODO: handle CLUSTERED

                    sb.AppendLine($"--INDEX FOR TABLE [{table.DatabaseName}] COLUMNS:" + string.Join(", ", columnList.Select(x => "[" + x.Value.DatabaseName + "]")));
                    sb.Append($"CREATE INDEX IF NOT EXISTS \"" + indexName + "\" ON \"" + table.GetPostgresSchema() + "\".\"" + tableName + "\" (");
                    sb.Append(string.Join(",", columnList.Select(x => "\"" + x.Value.DatabaseName + "\" " + (x.Key.Ascending ? "ASC" : "DESC"))));
                    sb.AppendLine(")");
                }

            }

            return sb.ToString();
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
                    sb.Append("\"" + column.DatabaseName + "\"");
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

        public static Dictionary<TableIndexColumn, Column> GetIndexColumns(Table table, TableIndex index)
        {
            var columnList = new Dictionary<TableIndexColumn, Column>();
            foreach (var indexColumn in index.IndexColumnList)
            {
                var column = table.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == indexColumn.FieldID);
                if (column != null)
                    columnList.Add(indexColumn, column);
            }
            return columnList;
        }

        private static void AppendCreateAudit(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowCreateAudit)
            {
                var defaultName = "DF__" + table.DatabaseName + "_" + model.Database.CreatedDateColumnName;
                defaultName = defaultName.ToUpper();
                sb.AppendLine(",");
                sb.AppendLine("\t\"" + model.Database.CreatedByColumnName + "\" Varchar (50) NULL,");
                sb.Append("\t\"" + model.Database.CreatedDateColumnName + "\" timestamp CONSTRAINT " + defaultName + " DEFAULT current_timestamp NULL");
            }
        }

        private static void AppendModifiedAudit(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowModifiedAudit)
            {
                var defaultName = "DF__" + table.DatabaseName + "_" + model.Database.ModifiedDateColumnName;
                defaultName = defaultName.ToUpper();
                sb.AppendLine(",");
                sb.AppendLine("\t\"" + model.Database.ModifiedByColumnName + "\" Varchar (50) NULL,");
                sb.Append("\t\"" + model.Database.ModifiedDateColumnName + "\" timestamp CONSTRAINT " + defaultName + " DEFAULT current_timestamp NULL");
            }
        }

        private static void AppendTimestamp(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowTimestamp)
            {
                sb.AppendLine(",");
                sb.Append("\t\"" + model.Database.TimestampColumnName + "\" timestamp NOT NULL");
            }
        }

        public static string GetSqlDropPK(Table table)
        {
            var sb = new StringBuilder();
            var pkName = $"PK_{table.DatabaseName}".ToUpper();
            sb.AppendLine($"--DROP PRIMARY KEY FOR TABLE [{table.DatabaseName}]");
            sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{pkName}\";");
            sb.AppendLine("--GO");
            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlCreatePK(Table table)
        {
            try
            {
                var sb = new StringBuilder();
                var tableIndex = table.TableIndexList.FirstOrDefault(x => x.PrimaryKey);
                if (tableIndex != null)
                {
                    var indexName = $"PK_{table.DatabaseName.ToUpper()}";
                    sb.AppendLine($"--PRIMARY KEY FOR TABLE [{table.DatabaseName}]");
                    sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" WITH NOCHECK ADD ");
                    //TODO: handle tableIndex.Clustered
                    sb.AppendLine($"CONSTRAINT \"{indexName}\" PRIMARY KEY ");
                    sb.AppendLine("(");
                    sb.AppendLine("\t" + GetSQLIndexField(table, tableIndex));
                    sb.AppendLine(");");
                    sb.AppendLine();
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetSqlCreateAuditPK(Table table)
        {
            var tableName = "__AUDIT__" + table.DatabaseName.ToUpper();
            var indexName = "PK_" + tableName.ToUpper();

            var sb = new StringBuilder();
            sb.AppendLine($"--PRIMARY KEY FOR TABLE [{tableName}]");
            sb.AppendLine($"if not exists(select * from sys.objects where name = '{indexName}' and type = 'PK')");
            sb.AppendLine($"ALTER TABLE [{table.GetPostgresSchema()}].[{tableName}] WITH NOCHECK ADD");
            sb.Append($"CONSTRAINT [{indexName}] PRIMARY KEY CLUSTERED ([__rowid]);");
            sb.AppendLine();
            sb.AppendLine("--GO");
            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlDropAuditPK(Table table)
        {
            var tableName = "__AUDIT__" + table.DatabaseName.ToUpper();
            var pkName = "PK_" + tableName.ToUpper();

            var sb = new StringBuilder();
            sb.AppendLine($"--DROP PRIMARY KEY FOR TABLE [{tableName}]");
            sb.AppendLine($"if exists(select * from sys.objects where name = '{pkName}' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')");
            sb.AppendLine($"ALTER TABLE [{table.GetPostgresSchema()}].[{tableName}] DROP CONSTRAINT [{pkName}]");
            sb.AppendLine("--GO");
            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlTenantIndex(ModelRoot model, Table table)
        {
            var indexName = "IDX_" + table.DatabaseName.Replace("-", string.Empty) + "_" + model.TenantColumnName;
            indexName = indexName.ToUpper();
            var sb = new StringBuilder();
            sb.AppendLine($"--INDEX FOR TABLE [{table.DatabaseName}] TENANT COLUMN: [{model.TenantColumnName}]");
            sb.AppendLine($"if not exists(select * from sys.indexes where name = '{indexName}')");
            sb.Append($"CREATE NONCLUSTERED INDEX [{indexName}] ON [{table.GetPostgresSchema()}].[{table.DatabaseName}] (");
            sb.Append($"[{model.TenantColumnName}])");
            sb.AppendLine();
            sb.AppendLine("--GO");
            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlDropColumnDefault(Column column, bool upgradeScript = false)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ALTER TABLE \"{column.ParentTable.GetPostgresSchema()}\".\"{column.ParentTable.DatabaseName}\" ALTER COLUMN \"{column.DatabaseName}\" DROP DEFAULT;");
            if (upgradeScript)
                sb.AppendLine("--GO");
            return sb.ToString();
        }

        public static string GetSqlTenantView(ModelRoot model, Table table, StringBuilder grantSB)
        {
            try
            {
                var itemName = model.TenantPrefix + "_" + table.DatabaseName;

                var sb = new StringBuilder();
                sb.AppendLine($"--DROP TENANT VIEW FOR TABLE [{table.DatabaseName}]");
                sb.AppendLine($"if exists (select * from sys.objects where name = '{itemName}' and [type] in ('V'))");
                sb.AppendLine($"DROP VIEW [{itemName}]");
                sb.AppendLine("--GO");
                sb.AppendLine();

                sb.AppendLine($"--CREATE TENANT VIEW FOR TABLE [{table.DatabaseName}]");
                sb.AppendLine($"CREATE VIEW [{table.GetPostgresSchema()}].[{itemName}] ");
                sb.AppendLine("AS");
                sb.AppendLine($"select * from [{table.DatabaseName}]");
                sb.AppendLine($"WHERE ([{model.TenantColumnName}] = SYSTEM_USER)");
                sb.AppendLine("--GO");

                if (!string.IsNullOrEmpty(model.Database.GrantExecUser))
                {
                    grantSB.AppendLine($"GRANT ALL ON [{table.GetPostgresSchema()}].[{itemName}] TO [{model.Database.GrantExecUser}]");
                    grantSB.AppendLine($"--MODELID: " + table.Key);
                    grantSB.AppendLine("--GO");
                    grantSB.AppendLine();
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string AppendColumnDefaultCreateSQL(Column column, bool includeDrop = true)
        {
            if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var table = column.ParentTable;
            var defaultClause = GetDefaultValueClause(column);

            if (!string.IsNullOrEmpty(column.Default))
            {
                //We know a default was specified so render the SQL
                if (!string.IsNullOrEmpty(defaultClause))
                {
                    sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" ALTER COLUMN \"{GetDefaultValueConstraintName(column)}\" SET DEFAULT {defaultClause}");
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        public static string GetDefaultValueConstraintName(Column column)
        {
            var table = column.ParentTableRef.Object as Table;
            var defaultName = "DF__" + table.DatabaseName + "_" + column.DatabaseName;
            defaultName = defaultName.ToUpper();
            return defaultName;
        }

        public static string GetSqlInsertStaticData(Table table)
        {
            try
            {
                var sb = new StringBuilder();
                var model = (ModelRoot)table.Root;

                //Generate static data
                if (table.StaticData.Count > 0)
                {
                    var isIdentity = false;
                    foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                        isIdentity |= (column.Identity == IdentityTypeConstants.Database);

                    sb.AppendLine("--INSERT STATIC DATA FOR TABLE [" + Globals.GetTableDatabaseName(model, table) + "]");

                    foreach (var rowEntry in table.StaticData.AsEnumerable<RowEntry>())
                    {

                        var fieldValues = new Dictionary<string, string>();
                        foreach (var cellEntry in rowEntry.CellEntries.ToList())
                        {
                            var column = cellEntry.ColumnRef.Object as Column;
                            var sqlValue = cellEntry.GetSQLData();
                            if (sqlValue == null) //Null is actually returned if the value can be null
                            {
                                if (!string.IsNullOrEmpty(column.Default))
                                {
                                    if (ModelHelper.IsTextType(column.DataType) || ModelHelper.IsDateType(column.DataType))
                                    {
                                        if (column.DataType == SqlDbType.NChar || column.DataType == SqlDbType.NText || column.DataType == SqlDbType.NVarChar)
                                            fieldValues.Add(column.Name, "N'" + column.Default.Replace("'", "''") + "'");
                                        else
                                            fieldValues.Add(column.Name, "'" + column.Default.Replace("'", "''") + "'");
                                    }
                                    else
                                    {
                                        fieldValues.Add(column.Name, column.Default);
                                    }
                                }
                                else
                                {
                                    fieldValues.Add(column.Name, "NULL");
                                }
                            }
                            else
                            {
                                if (column.DataType == SqlDbType.Bit)
                                {
                                    sqlValue = sqlValue.ToLower().Trim();
                                    if (sqlValue == "true") sqlValue = "1";
                                    else if (sqlValue == "false") sqlValue = "0";
                                    else if (sqlValue != "1") sqlValue = "0"; //catch all, must be true/false
                                }

                                if (column.DataType == SqlDbType.NChar || column.DataType == SqlDbType.NText || column.DataType == SqlDbType.NVarChar)
                                    fieldValues.Add(column.Name, "N" + sqlValue);
                                else
                                    fieldValues.Add(column.Name, sqlValue);

                            }
                        }

                        // this could probably be done smarter
                        // but I am concerned about the order of the keys and values coming out right
                        var fieldList = new List<string>();
                        var valueList = new List<string>();
                        var primaryKeyColumnNames = table.PrimaryKeyColumns.Select(x => x.Name);
                        foreach (var kvp in fieldValues)
                        {
                            fieldList.Add("\"" + kvp.Key + "\"");
                            valueList.Add(kvp.Value);
                        }

                        var fieldListString = string.Join(",", fieldList);
                        var valueListString = string.Join(",", valueList);

                        sb.AppendLine("INSERT INTO \"" + table.GetSQLSchema() + "\".\"" + Globals.GetTableDatabaseName(model, table) + "\" (" + fieldListString + ") OVERRIDING SYSTEM VALUE values (" + valueListString + ") ON CONFLICT DO NOTHING;");
                    }

                    sb.AppendLine();
                    sb.AppendLine("--GO");
                    sb.AppendLine();
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