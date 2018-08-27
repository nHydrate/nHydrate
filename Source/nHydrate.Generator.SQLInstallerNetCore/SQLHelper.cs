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
using nHydrate.Generator.ProjectItemGenerators;
using System.Xml;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.SQLInstallerNetCore.ProjectItemGenerators;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Core.SQLGeneration;

namespace nHydrate.Generator.SQLInstallerNetCore
{
    internal static class SqlHelper
    {
        #region GetModelDifferenceSQL

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
                        sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSQLCreateTable(modelNew, newT));
                        sb.AppendLine("GO");
                        sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlCreatePK(newT));
                        sb.AppendLine("GO");

                        //DO NOT process primary keys
                        foreach (var index in newT.TableIndexList.Where(x => !x.PrimaryKey))
                        {
                            sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSQLCreateIndex(newT, index, false));
                            sb.AppendLine("GO");
                            sb.AppendLine();
                        }

                        if (newT.StaticData.Count > 0)
                        {
                            sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlInsertStaticData(newT));
                            sb.AppendLine("GO");
                            sb.AppendLine();
                        }

                        //If this is a tenant table then add the view as well
                        if (newT.IsTenant)
                        {
                            var grantSB = new StringBuilder();
                            var q1 = nHydrate.Core.SQLGeneration.SQLEmit.GetSqlTenantView(modelNew, newT, grantSB);
                            sb.AppendLine(q1);
                            if (grantSB.ToString() != string.Empty)
                                sb.AppendLine(grantSB.ToString());
                        }

                    }
                }

                #endregion

                #region Delete Indexes
                foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                {
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT != null)
                    {
                        //If old exists new does NOT, so delete index
                        foreach (var oldIndex in oldT.TableIndexList)
                        {
                            var newIndex = newT.TableIndexList.FirstOrDefault(x => x.Key == oldIndex.Key);
                            if (newIndex == null)
                            {
                                sb.AppendLine(SQLEmit.GetSQLDropIndex(newT, oldIndex));
                                sb.AppendLine("GO");
                            }
                        }

                        //Both exist, so if different, drop and re-create
                        foreach (var newIndex in newT.TableIndexList)
                        {
                            var oldIndex = oldT.TableIndexList.FirstOrDefault(x => x.Key == newIndex.Key);
                            if (oldIndex != null && oldIndex.CorePropertiesHashNoNames != newIndex.CorePropertiesHashNoNames)
                            {
                                sb.AppendLine(SQLEmit.GetSQLDropIndex(newT, oldIndex));
                                sb.AppendLine("GO");
                            }
                        }
                    }
                }
                #endregion

                #region Loop and DELETE tables
                foreach (var oldT in modelOld.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly))
                {
                    var newT = modelNew.Database.Tables.FirstOrDefault(x => x.Generated && (x.TypedTable != TypedTableConstants.EnumOnly) && x.Key.ToLower() == oldT.Key.ToLower());
                    if (newT == null)
                    {
                        //DELETE TABLE
                        sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlDropTable(modelOld, oldT));
                        sb.AppendLine("GO");
                        //TODO - Delete Tenant View
                        sb.AppendLine();
                    }
                    else if (newT != null && oldT.AllowAuditTracking && !newT.AllowAuditTracking)
                    {
                        //If the old model had audit tracking and the new one does not, add a TODO in the script
                        var tableName = "__AUDIT__" + Globals.GetTableDatabaseName(modelOld, oldT);
                        sb.AppendLine("--TODO: REMOVE AUDIT TABLE '" + tableName + "'");
                        sb.AppendLine("--The previous model had audit tracking turn on for table '" + Globals.GetTableDatabaseName(modelOld, oldT) + "' and now it is turned off.");
                        sb.AppendLine("--The audit table will not be removed automatically. If you want to remove it, uncomment the following script.");
                        sb.AppendLine("--DROP TABLE [" + tableName + "]");
                        sb.AppendLine("--GO");
                        sb.AppendLine();
                    }
                    //else if (tList[0].DatabaseName != oldT.DatabaseName)
                    //{
                    //  //RENAME TABLE
                    //  sb.AppendLine("if exists(select * from sysobjects where name = '" + oldT.DatabaseName + "' and xtype = 'U')");
                    //  sb.AppendLine("exec sp_rename [" + oldT.DatabaseName + "], [" + tList[0].DatabaseName + "]");
                    //}
                }
                #endregion

                #region Loop and Modify tables
                foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                {
                    var schemaChanged = false;
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT != null)
                    {
                        var querylist = new List<string>();

                        #region Rename table if need be
                        if (oldT.DatabaseName != newT.DatabaseName)
                        {
                            sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameTable(oldT, newT));
                            sb.AppendLine("GO");
                        }
                        #endregion

                        #region Add columns
                        foreach (var newC in newT.GetColumns())
                        {
                            var oldC = Globals.GetColumnByKey(oldT.Columns, newC.Key);
                            if (oldC == null)
                            {
                                //ADD COLUMN
                                sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlAddColumn(newC));
                                sb.AppendLine("GO");
                                sb.AppendLine();
                                schemaChanged = true;
                            }
                            //else if (newC.DatabaseName != oldC.DatabaseName)
                            //{
                            //  //RENAME COLUMN
                            //  sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSQLRenameColumn(oldC, newC));
                            //  sb.AppendLine("GO");
                            //  sb.AppendLine();
                            //}

                        }
                        #endregion

                        #region Delete Columns
                        foreach (Reference oldRef in oldT.Columns)
                        {
                            var oldC = oldRef.Object as Column;
                            var newC = Globals.GetColumnByKey(newT.Columns, oldC.Key);
                            if (newC == null)
                            {
                                //DELETE COLUMN
                                sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlDropColumn(modelNew, oldC));
                                sb.AppendLine("GO");
                                sb.AppendLine();
                                schemaChanged = true;
                            }
                            else if (newC.DatabaseName != oldC.DatabaseName)
                            {
                                ////RENAME COLUMN
                                //string sql = "if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + oldC.DatabaseName + "' and o.name = '" + newT.DatabaseName + "')" +
                                //             "AND not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + newC.DatabaseName + "' and o.name = '" + newT.DatabaseName + "')" + Environment.NewLine +
                                //             "EXEC sp_rename @objname = '" + newT.DatabaseName + "." + oldC.DatabaseName + "', @newname = '" + newC.DatabaseName + "', @objtype = 'COLUMN'";
                                //if (!querylist.Contains(sql))
                                //{
                                //  querylist.Add(sql);
                                //  sb.AppendLine(sql);
                                //  sb.AppendLine("GO");
                                //  sb.AppendLine();
                                //}
                            }

                        }
                        #endregion

                        #region Modify Columns
                        foreach (var newC in newT.GetColumns())
                        {
                            var oldC = Globals.GetColumnByKey(oldT.Columns, newC.Key);
                            if (oldC != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml("<a></a>");
                                var n1 = XmlHelper.AddElement(document.DocumentElement, "q");
                                var n2 = XmlHelper.AddElement(document.DocumentElement, "q");
                                oldC.XmlAppend(n1);
                                newC.XmlAppend(n2);

                                //Check column, ignore defaults
                                if (newC.CorePropertiesHashNoPK != oldC.CorePropertiesHashNoPK)
                                {
                                    //MODIFY COLUMN
                                    sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlModifyColumn(oldC, newC));
                                    sb.AppendLine("GO");
                                    sb.AppendLine();
                                    schemaChanged = true;
                                }

                                //Drop add defaults if column
                                //if ((newC.CorePropertiesHashNoPK != oldC.CorePropertiesHashNoPK) || (oldC.Default != newC.Default))
                                //{
                                //		if (!string.IsNullOrEmpty(oldC.Default))
                                //		{
                                //			//Old default was something so drop it
                                //			sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlDropColumnDefault(newC));
                                //			sb.AppendLine("GO");
                                //			sb.AppendLine();
                                //		}

                                //	if (!string.IsNullOrEmpty(newC.Default))
                                //	{
                                //		//New default is something so add it
                                //		sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlCreateColumnDefault(modelNew, newC));
                                //		sb.AppendLine("GO");
                                //		sb.AppendLine();
                                //	}
                                //}

                                if (!string.IsNullOrEmpty(newC.Default) && ((oldC.Default != newC.Default) || (oldC.DataType != newC.DataType) || (oldC.DatabaseName != newC.DatabaseName)))
                                {
                                    //New default is something so add it
                                    sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlCreateColumnDefault(modelNew, newC));
                                    sb.AppendLine("GO");
                                    sb.AppendLine();
                                }

                            }
                        }
                        #endregion

                        #region Process Table Splits
                        {
                            var splits = modelNew.Refactorizations
                                .Where(x => x is RefactorTableSplit)
                                .Cast<RefactorTableSplit>()
                                .Where(x => x.EntityKey1 == new Guid(newT.Key))
                                .ToList();

                            foreach (var split in splits)
                            {
                                var splitTable = modelNew.Database.Tables.FirstOrDefault(x => new Guid(x.Key) == split.EntityKey2);
                                var origFields = oldT.GeneratedColumns.Where(x => split.ReMappedFieldIDList.Keys.Contains(new Guid(x.Key))).ToList();
                                if (splitTable != null && origFields.Count > 0)
                                {
                                    var newFields = new List<Column>();
                                    foreach (var item in origFields)
                                    {
                                        var newF = splitTable.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == split.ReMappedFieldIDList[new Guid(item.Key)]);
                                        if (newF != null)
                                            newFields.Add(newF);
                                    }

                                    newFields = newFields.Distinct().ToList();

                                    //If there are columns then process
                                    if (newFields.Count > 0)
                                    {
                                        sb.AppendLine("--PROCESS TABLE SPLIT [" + newT.DatabaseName + "] -> [" + splitTable.DatabaseName + "]");

                                        //Get the fields for generation
                                        //This may be a different number than original split since user can remove fields
                                        var genFields = new Dictionary<Column, Column>();
                                        foreach (var f in origFields)
                                        {
                                            //Get the new column from the new table as the name might have changed
                                            var newID = split.ReMappedFieldIDList[new Guid(f.Key)];
                                            var newF = splitTable.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == newID);
                                            if (newF != null)
                                            {
                                                genFields.Add(f, newF);
                                            }
                                        }

                                        //Process the actual script fields
                                        if (genFields.Count > 0)
                                        {
                                            //Turn on identity insert if necessary
                                            if (splitTable.PrimaryKeyColumns.Count(x => x.Identity == IdentityTypeConstants.Database) > 0)
                                                sb.AppendLine("SET identity_insert [" + splitTable.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(modelNew, splitTable) + "] on");

                                            sb.Append("INSERT INTO [" + splitTable.GetSQLSchema() + "].[" + splitTable.DatabaseName + "] (");
                                            foreach (var f in genFields.Keys)
                                            {
                                                //Get the new column from the new table as the name might have changed
                                                var newF = genFields[f];
                                                sb.Append("[" + newF.DatabaseName + "]");
                                                if (genFields.Keys.IndexOf(f) < genFields.Keys.Count - 1) sb.Append(", ");
                                            }
                                            sb.AppendLine(")");
                                            sb.Append("SELECT ");
                                            foreach (var f in genFields.Keys)
                                            {
                                                sb.Append("[" + f.DatabaseName + "]");
                                                if (genFields.Keys.IndexOf(f) < genFields.Keys.Count - 1) sb.Append(", ");
                                            }

                                            sb.AppendLine(" FROM [" + newT.GetSQLSchema() + "].[" + newT.DatabaseName + "]");

                                            //Turn off identity insert if necessary
                                            if (splitTable.PrimaryKeyColumns.Count(x => x.Identity == IdentityTypeConstants.Database) > 0)
                                                sb.AppendLine("SET identity_insert [" + splitTable.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(modelNew, splitTable) + "] off");

                                            sb.AppendLine("GO");
                                            sb.AppendLine();
                                        }
                                    }
                                }
                            }
                        } //Table Splits
                        #endregion

                        #region Process Table Combines
                        {
                            var splits = modelNew.Refactorizations
                                .Where(x => x is RefactorTableCombine)
                                .Cast<RefactorTableCombine>()
                                .Where(x => x.EntityKey1 == new Guid(newT.Key))
                                .ToList();

                            foreach (var split in splits)
                            {
                                var deletedTable = modelOld.Database.Tables.FirstOrDefault(x => new Guid(x.Key) == split.EntityKey2);
                                if (deletedTable != null)
                                {
                                    var deletedOrigDeletedT = modelOld.Database.Tables.GetByKey(deletedTable.Key).FirstOrDefault();
                                    if (deletedOrigDeletedT != null)
                                    {
                                        var deletedFields = deletedOrigDeletedT.GeneratedColumns.Where(x => split.ReMappedFieldIDList.Keys.Contains(new Guid(x.Key))).ToList();
                                        if (deletedFields.Count > 0)
                                        {
                                            var targetFields = new List<Column>();
                                            foreach (var item in deletedFields)
                                            {
                                                var newF = newT.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == split.ReMappedFieldIDList[new Guid(item.Key)]);
                                                if (newF != null)
                                                    targetFields.Add(newF);
                                            }

                                            targetFields = targetFields.Distinct().ToList();

                                            //If there are columns then process
                                            if (targetFields.Count > 0)
                                            {
                                                //Get the fields for generation
                                                //This may be a different number than original combine since user can remove fields
                                                var genFields = new Dictionary<Column, Column>();
                                                foreach (var f in deletedFields)
                                                {
                                                    //Get the new column from the new table as the name might have changed
                                                    var newID = split.ReMappedFieldIDList[new Guid(f.Key)];
                                                    var newF = newT.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == newID);
                                                    if (newF != null)
                                                    {
                                                        genFields.Add(f, newF);
                                                    }
                                                }

                                                //Process the actual script fields
                                                if (genFields.Count > 0)
                                                {
                                                    sb.AppendLine("--PROCESS TABLE COMBINE [" + deletedTable.DatabaseName + "] into [" + newT.DatabaseName + "]");
                                                    sb.Append("UPDATE [" + newT.GetSQLSchema() + "].[" + newT.DatabaseName + "] SET ");
                                                    foreach (var f in genFields.Keys)
                                                    {
                                                        //Get the new column from the new table as the name might have changed
                                                        var newF = genFields[f];
                                                        sb.Append("[" + newF.DatabaseName + "] = [_deleted].[" + f.DatabaseName + "]");
                                                        if (genFields.Keys.IndexOf(f) < genFields.Keys.Count - 1) sb.Append(", ");
                                                    }
                                                    sb.Append(" FROM [" + newT.GetSQLSchema() + "].[" + newT.DatabaseName + "] AS [_A] INNER JOIN ");
                                                    sb.Append("[" + deletedTable.GetSQLSchema() + "].[" + deletedTable.DatabaseName + "] AS [_deleted] ON ");
                                                    sb.Append("[_A].[" + newT.PrimaryKeyColumns.First().DatabaseName + "] = [_deleted].[" + deletedTable.PrimaryKeyColumns.First().DatabaseName + "]");
                                                    sb.AppendLine();
                                                    sb.AppendLine("GO");
                                                    sb.AppendLine();
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        } //Table Combine
                        #endregion

                        #region Tenant
                        if (oldT.IsTenant && !newT.IsTenant)
                        {
                            //Drop default
                            var defaultName = "DF__" + newT.DatabaseName.ToUpper() + "_" + modelNew.TenantColumnName.ToUpper();
                            sb.AppendLine("--DELETE TENANT DEFAULT FOR [" + newT.DatabaseName + "]");
                            sb.AppendLine("if exists (select name from sys.objects where name = '" + defaultName + "'  AND type = 'D')");
                            sb.AppendLine("ALTER TABLE [" + newT.GetSQLSchema() + "].[" + newT.DatabaseName + "] DROP CONSTRAINT [" + defaultName + "]");
                            sb.AppendLine();

                            if (newT.PascalName != newT.DatabaseName)
                            {
                                //This is for the mistake in name when released. Remove this default June 2013
                                defaultName = "DF__" + newT.PascalName.ToUpper() + "_" + modelNew.TenantColumnName.ToUpper();
                                sb.AppendLine("--DELETE TENANT DEFAULT FOR [" + newT.DatabaseName + "]");
                                sb.AppendLine("if exists (select name from sys.objects where name = '" + defaultName + "'  AND type = 'D')");
                                sb.AppendLine("ALTER TABLE [" + newT.GetSQLSchema() + "].[" + newT.DatabaseName + "] DROP CONSTRAINT [" + defaultName + "]");
                                sb.AppendLine();
                            }

                            //Drop Index
                            var indexName = "IDX_" + newT.DatabaseName.Replace("-", string.Empty) + "_" + modelNew.TenantColumnName;
                            indexName = indexName.ToUpper();
                            sb.AppendLine("if exists (select * from sys.indexes where name = '" + indexName + "')");
                            sb.AppendLine("DROP INDEX [" + indexName + "] ON [" + newT.DatabaseName + "]");
                            sb.AppendLine();

                            //Drop the associated view
                            var viewName = modelOld.TenantPrefix + "_" + oldT.DatabaseName;
                            sb.AppendLine("if exists (select name from sys.objects where name = '" + viewName + "'  AND type = 'V')");
                            sb.AppendLine("DROP VIEW [" + viewName + "]");
                            sb.AppendLine();

                            //Drop the tenant field
                            sb.AppendLine("if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + modelNew.TenantColumnName + "' and o.name = '" + newT.DatabaseName + "')");
                            sb.AppendLine("ALTER TABLE [" + newT.GetSQLSchema() + "].[" + newT.DatabaseName + "] DROP COLUMN [" + modelNew.TenantColumnName + "]");
                            sb.AppendLine();
                        }
                        else if (!oldT.IsTenant && newT.IsTenant)
                        {
                            //Add the tenant field
                            sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlCreateTenantColumn(modelNew, newT));
                        }
                        #endregion

                        #region Primary Key Changed

                        //If the primary key changed, then generate a commented script that marks where the user can manually intervene
                        var newPKINdex = newT.TableIndexList.FirstOrDefault(x => x.PrimaryKey);
                        var oldPKINdex = oldT.TableIndexList.FirstOrDefault(x => x.PrimaryKey);
                        if (newPKINdex != null && oldPKINdex != null)
                        {
                            var newPKHash = newPKINdex.CorePropertiesHash;
                            var oldPKHash = oldPKINdex.CorePropertiesHash;
                            if (newPKHash != oldPKHash)
                            {
                                sb.AppendLine("--GENERATION NOTE **");
                                sb.AppendLine("--THE PRIMARY KEY HAS CHANGED, THIS MAY REQUIRE MANUAL INTERVENTION");
                                sb.AppendLine("--THE FOLLOWING SCRIPT WILL DROP AND READD THE PRIMARY KEY HOWEVER IF THERE ARE RELATIONSHIPS");
                                sb.AppendLine("--BASED ON THIS IT, THE SCRIPT WILL FAIL. YOU MUST DROP ALL RELATIONSHIPS FIRST.");

                                var tableName = Globals.GetTableDatabaseName(modelNew, newT);
                                var pkName = "PK_" + tableName;
                                pkName = pkName.ToUpper();
                                sb.AppendLine("--DROP PRIMARY KEY FOR TABLE [" + tableName + "]");
                                sb.AppendLine("--if exists(select * from sys.objects where name = '" + pkName + "' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')");
                                sb.AppendLine("--ALTER TABLE [" + newT.GetSQLSchema() + "].[" + tableName + "] DROP CONSTRAINT [" + pkName + "]");
                                sb.AppendLine("--GO");

                                var sql = SQLEmit.GetSqlCreatePK(newT) + "GO\r\n";
                                var lines = sql.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                                //Comment the whole SQL block
                                var index = 0;
                                foreach (var s in lines)
                                {
                                    var l = s;
                                    if (!l.StartsWith("--"))
                                        l = "--" + l;
                                    lines[index] = l;
                                    index++;
                                }

                                sb.AppendLine(string.Join("\r\n", lines));
                                sb.AppendLine();
                            }
                        }

                        #endregion

                        #region Drop Foreign Keys
                        foreach (var r1 in oldT.GetRelations().ToList())
                        {
                            var r2 = newT.Relationships.FirstOrDefault(x => x.Key == r1.Key);
                            if (r2 == null)
                            {
                                sb.Append(SQLEmit.GetSqlRemoveFK(r1));
                                sb.AppendLine("GO");
                                sb.AppendLine();
                            }
                        }
                        #endregion

                        #region Rename audit columns if necessary
                        if (modelOld.Database.CreatedByColumnName != modelNew.Database.CreatedByColumnName)
                        {
                            sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(newT.DatabaseName, modelOld.Database.CreatedByColumnName, modelNew.Database.CreatedByColumnName));
                            sb.AppendLine("GO");
                        }
                        if (modelOld.Database.CreatedDateColumnName != modelNew.Database.CreatedDateColumnName)
                        {
                            sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(newT.DatabaseName, modelOld.Database.CreatedDateColumnName, modelNew.Database.CreatedDateColumnName));
                            sb.AppendLine("GO");
                        }
                        if (modelOld.Database.ModifiedByColumnName != modelNew.Database.ModifiedByColumnName)
                        {
                            sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(newT.DatabaseName, modelOld.Database.ModifiedByColumnName, modelNew.Database.ModifiedByColumnName));
                            sb.AppendLine("GO");
                        }
                        if (modelOld.Database.ModifiedDateColumnName != modelNew.Database.ModifiedDateColumnName)
                        {
                            sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(newT.DatabaseName, modelOld.Database.ModifiedDateColumnName, modelNew.Database.ModifiedDateColumnName));
                            sb.AppendLine("GO");
                        }
                        if (modelOld.Database.TimestampColumnName != modelNew.Database.TimestampColumnName)
                        {
                            sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(newT.DatabaseName, modelOld.Database.TimestampColumnName, modelNew.Database.TimestampColumnName));
                            sb.AppendLine("GO");
                        }
                        #endregion

                        #region Emit Tenant View if need be

                        //If the table schema has changed then emit the Tenant view
                        if (schemaChanged && newT.IsTenant)
                        {
                            var grantSB = new StringBuilder();
                            var q1 = nHydrate.Core.SQLGeneration.SQLEmit.GetSqlTenantView(modelNew, newT, grantSB);
                            sb.AppendLine(q1);
                            if (grantSB.ToString() != string.Empty)
                                sb.AppendLine(grantSB.ToString());
                        }

                        #endregion

                        #region Static Data

                        //For right now just emit NEW if different.
                        //TODO: Generate difference scripts for delete and change too.
                        var oldStaticScript = SQLEmit.GetSqlInsertStaticData(oldT);
                        var newStaticScript = SQLEmit.GetSqlInsertStaticData(newT);
                        if (oldStaticScript != newStaticScript)
                        {
                            sb.AppendLine(newStaticScript);
                            sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlUpdateStaticData(oldT, newT));
                            sb.AppendLine("GO");
                            sb.AppendLine();
                        }

                        #endregion

                        //TODO - Check hash porperties and if changed recompile tenant view

                    }
                }

                //Do another look for second pass at changes.
                //These things can only be done after the above loop
                foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                {
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT != null)
                    {
                        #region Add Foreign Keys

                        foreach (var r1 in newT.GetRelations().ToList())
                        {
                            var r2 = oldT.GetRelations().ToList().FirstOrDefault(x => x.Key == r1.Key);
                            if (r2 == null)
                            {
                                //There is no OLD relation so it is new so add it
                                sb.Append(SQLEmit.GetSqlAddFK(r1));
                                sb.AppendLine("GO");
                                sb.AppendLine();
                            }
                            else if (r1.CorePropertiesHash != r2.CorePropertiesHash)
                            {
                                //The relation already exists and it has changed, so drop and re-add
                                sb.Append(SQLEmit.GetSqlRemoveFK(r2));
                                sb.AppendLine("GO");
                                sb.AppendLine();
                                sb.Append(SQLEmit.GetSqlAddFK(r1));
                                sb.AppendLine("GO");
                                sb.AppendLine();
                            }
                        }

                        #endregion
                    }
                }

                #endregion

                #region Move tables between schemas

                var reschema = 0;
                foreach (Table newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly))
                {
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT != null)
                    {
                        if (string.Compare(oldT.GetSQLSchema(), newT.GetSQLSchema(), true) != 0)
                        {
                            if (reschema == 0)
                                sb.AppendLine("--MOVE TABLES TO PROPER SCHEMA IF NEED BE");

                            //This table has changed schema so script it
                            sb.AppendLine("--CREATE DATABASE SCHEMAS");
                            sb.AppendLine("if not exists(select * from sys.schemas where name = '" + newT.GetSQLSchema() + "')");
                            sb.AppendLine("exec('CREATE SCHEMA [" + newT.GetSQLSchema() + "]')");
                            sb.AppendLine("GO");
                            sb.AppendLine("if exists (select * from sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id where t.name = '" + newT.DatabaseName + "' and s.name = '" + oldT.GetSQLSchema() + "')");
                            sb.AppendLine("	ALTER SCHEMA [" + newT.GetSQLSchema() + "] TRANSFER [" + oldT.GetSQLSchema() + "].[" + newT.DatabaseName + "];");
                            sb.AppendLine("GO");
                            reschema++;
                        }
                    }
                }

                if (reschema > 0) sb.AppendLine();

                #endregion

                #region Add Indexes
                foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
                {
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT != null)
                    {
                        //If old exists and does old NOT, so create index
                        foreach (var newIndex in newT.TableIndexList)
                        {
                            var oldIndex = oldT.TableIndexList.FirstOrDefault(x => x.Key == newIndex.Key);
                            if (oldIndex == null)
                            {
                                sb.AppendLine(SQLEmit.GetSQLCreateIndex(newT, newIndex, false));
                                sb.AppendLine("GO");
                                sb.AppendLine();
                            }
                        }

                        //Both exist, so if different, drop and re-create
                        foreach (var newIndex in newT.TableIndexList)
                        {
                            var oldIndex = oldT.TableIndexList.FirstOrDefault(x => x.Key == newIndex.Key);
                            if (oldIndex != null && oldIndex.CorePropertiesHashNoNames != newIndex.CorePropertiesHashNoNames)
                            {
                                sb.AppendLine(SQLEmit.GetSQLCreateIndex(newT, newIndex, false));
                                sb.AppendLine("GO");
                                sb.AppendLine();
                            }
                        }
                    }
                }
                #endregion

                #region Add/Remove deleted SP, Views, and Funcs

                //Stored procedures
                var removedItems = 0;
                foreach (var oldT in modelOld.Database.CustomStoredProcedures.OrderBy(x => x.Name))
                {
                    var newT = modelNew.Database.CustomStoredProcedures.FirstOrDefault(x => x.Key == oldT.Key);
                    if (newT == null)
                    {
                        sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('P'))");
                        sb.AppendLine("drop procedure [" + oldT.DatabaseName + "]");
                        removedItems++;
                    }
                    else if (newT.DatabaseName != oldT.DatabaseName)
                    {
                        //Name changed so remove old
                        sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('P'))");
                        sb.AppendLine("drop procedure [" + oldT.DatabaseName + "]");
                        removedItems++;
                    }
                }

                if (removedItems > 0)
                {
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

                foreach (var newT in modelNew.Database.CustomStoredProcedures.OrderBy(x => x.Name))
                {
                    var oldT = modelOld.Database.CustomStoredProcedures.FirstOrDefault(x => x.Key == newT.Key);
                    if (oldT == null || (oldT.CorePropertiesHash != newT.CorePropertiesHash))
                    {
                        sb.Append(SQLEmit.GetSQLCreateStoredProc(newT, false));
                    }
                }

                //Views
                removedItems = 0;
                foreach (var oldT in modelOld.Database.CustomViews.OrderBy(x => x.Name))
                {
                    var newT = modelNew.Database.CustomViews.FirstOrDefault(x => x.Key == oldT.Key);
                    if (newT == null)
                    {
                        sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('V'))");
                        sb.AppendLine("drop view [" + oldT.DatabaseName + "]");
                        removedItems++;
                    }
                    else if (newT.DatabaseName != oldT.DatabaseName)
                    {
                        //Name changed so remove old
                        sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('V'))");
                        sb.AppendLine("drop view [" + oldT.DatabaseName + "]");
                        removedItems++;
                    }
                }

                if (removedItems > 0)
                {
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

                foreach (var newT in modelNew.Database.CustomViews.OrderBy(x => x.Name))
                {
                    var oldT = modelOld.Database.CustomViews.FirstOrDefault(x => x.Key == newT.Key);
                    if (oldT == null || (oldT.CorePropertiesHash != newT.CorePropertiesHash))
                    {
                        sb.Append(SQLEmit.GetSqlCreateView(newT, false));
                    }
                }

                //Functions
                removedItems = 0;
                foreach (var oldT in modelOld.Database.Functions.OrderBy(x => x.Name))
                {
                    var newT = modelNew.Database.Functions.FirstOrDefault(x => x.Key == oldT.Key);
                    if (newT == null)
                    {
                        sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('FN','IF','TF','FS','FT'))");
                        sb.AppendLine("drop function [" + oldT.DatabaseName + "]");
                        removedItems++;
                    }
                    else if (newT.DatabaseName != oldT.DatabaseName)
                    {
                        //Name changed so remove old
                        sb.AppendLine("if exists (select * from sys.objects where name = '" + oldT.DatabaseName + "' and [type] in ('FN','IF','TF','FS','FT'))");
                        sb.AppendLine("drop function [" + oldT.DatabaseName + "]");
                        removedItems++;
                    }
                }

                if (removedItems > 0)
                {
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

                foreach (var newT in modelNew.Database.Functions.OrderBy(x => x.Name))
                {
                    var oldT = modelOld.Database.Functions.FirstOrDefault(x => x.Key == newT.Key);
                    if (oldT == null || (oldT.CorePropertiesHash != newT.CorePropertiesHash))
                    {
                        sb.Append(SQLEmit.GetSQLCreateFunction(newT, false, modelNew.EFVersion));
                    }
                }

                #endregion

                #region Add/Remove Audit fields

                foreach (var newT in modelNew.Database.Tables.OrderBy(x => x.Name))
                {
                    var oldT = modelOld.Database.Tables.FirstOrDefault(x => x.Key == newT.Key);
                    if (oldT != null)
                    {
                        if (!oldT.AllowCreateAudit && newT.AllowCreateAudit)
                            Globals.AppendCreateAudit(newT, modelNew, sb);
                        if (!oldT.AllowModifiedAudit && newT.AllowModifiedAudit)
                            Globals.AppendModifiedAudit(newT, modelNew, sb);
                        if (!oldT.AllowTimestamp && newT.AllowTimestamp)
                            Globals.AppendTimestampAudit(newT, modelNew, sb);

                        if (oldT.AllowCreateAudit && !newT.AllowCreateAudit)
                            Globals.DropCreateAudit(newT, modelNew, sb);
                        if (oldT.AllowModifiedAudit && !newT.AllowModifiedAudit)
                            Globals.DropModifiedAudit(newT, modelNew, sb);
                        if (oldT.AllowTimestamp && !newT.AllowTimestamp)
                            Globals.DropTimestampAudit(newT, modelNew, sb);
                    }
                }

                #endregion

                #region Loop and change computed fields

                foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                {
                    //If the table exists...
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT != null)
                    {
                        var tChanged = false;
                        //If there is a computed field with a different value
                        foreach (var newC in newT.GetColumns().Where(x => x.ComputedColumn).ToList())
                        {
                            var oldC = Globals.GetColumnByKey(oldT.Columns, newC.Key);
                            if (oldC != null && oldC.Formula != newC.Formula)
                            {
                                tChanged = true;
                                sb.AppendLine("if exists(select o.name, c.name from sys.columns c inner join sys.objects o on c.object_id = o.object_id where o.type = 'U' and o.name = '" + newT.DatabaseName + "' and c.name = '" + newC.DatabaseName + "')");
                                sb.AppendLine("ALTER TABLE [" + newT.DatabaseName + "] DROP COLUMN [" + newC.DatabaseName + "]");
                                sb.AppendLine("GO");
                                sb.AppendLine("ALTER TABLE [" + newT.DatabaseName + "] ADD [" + newC.DatabaseName + "] AS (" + newC.Formula + ")");
                                sb.AppendLine("GO");
                                sb.AppendLine();
                            }
                        }

                        if (newT.IsTenant && tChanged)
                        {
                            var grantSB = new StringBuilder();
                            var q1 = nHydrate.Core.SQLGeneration.SQLEmit.GetSqlTenantView(modelNew, newT, grantSB);
                            sb.AppendLine(q1);
                            if (grantSB.ToString() != string.Empty)
                                sb.AppendLine(grantSB.ToString());
                        }
                    }
                }

                #endregion

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region GetSQLSchema

        public static string GetSQLSchema(this Table item)
        {
            if (string.IsNullOrEmpty(item.DBSchema)) return "dbo";
            return item.DBSchema;
        }

        public static string GetSQLSchema(this TableComponent item)
        {
            if (string.IsNullOrEmpty(item.Parent.DBSchema)) return "dbo";
            return item.Parent.DBSchema;
        }

        public static string GetSQLSchema(this CustomStoredProcedure item)
        {
            if (string.IsNullOrEmpty(item.DBSchema)) return "dbo";
            return item.DBSchema;
        }

        public static string GetSQLSchema(this CustomView item)
        {
            if (string.IsNullOrEmpty(item.DBSchema)) return "dbo";
            return item.DBSchema;
        }

        #endregion

    }
}