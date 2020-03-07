using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;
using System.Xml;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.SQLInstaller.ProjectItemGenerators;
using nHydrate.Core.SQLGeneration;

namespace nHydrate.Generator.SQLInstaller
{
    internal static class SqlHelper
    {
        #region GetModelDifferenceSQL

        public static string GetModelDifferenceSql(ModelRoot modelOld, ModelRoot modelNew)
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

            foreach (var oldT in modelOld.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly))
            {
                var newT = modelNew.Database.Tables.FirstOrDefault(x => (x.TypedTable != TypedTableConstants.EnumOnly) && x.Key.ToLower() == oldT.Key.ToLower());
                if (newT == null)
                {
                    //DELETE TABLE
                    sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlDropTable(modelOld, oldT));
                    sb.AppendLine("GO");
                    //TODO - Delete Tenant View
                    sb.AppendLine();
                }

                //else if (tList[0].DatabaseName != oldT.DatabaseName)
                //{
                //  //RENAME TABLE
                //  sb.AppendLine("if exists(select * from sys.objects where name = '" + oldT.DatabaseName + "' and type = 'U')");
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
                            //string sql = "if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + oldC.DatabaseName + "' and o.name = '" + newT.DatabaseName + "')" +
                            //             "AND not exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + newC.DatabaseName + "' and o.name = '" + newT.DatabaseName + "')" + Environment.NewLine +
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
                            defaultName = $"DF__{newT.PascalName}_{modelNew.TenantColumnName}".ToUpper();
                            sb.AppendLine($"--DELETE TENANT DEFAULT FOR [{newT.DatabaseName}]");
                            sb.AppendLine($"if exists (select name from sys.objects where name = '{defaultName}'  AND type = 'D')");
                            sb.AppendLine($"ALTER TABLE [{newT.GetSQLSchema()}].[{newT.DatabaseName}] DROP CONSTRAINT [{defaultName}]");
                            sb.AppendLine();
                        }

                        //Drop Index
                        var indexName = "IDX_" + newT.DatabaseName.Replace("-", string.Empty) + "_" + modelNew.TenantColumnName;
                        indexName = indexName.ToUpper();
                        sb.AppendLine($"if exists (select * from sys.indexes where name = '{indexName}')");
                        sb.AppendLine($"DROP INDEX [{indexName}] ON [{newT.GetSQLSchema()}].[{newT.DatabaseName}]");
                        sb.AppendLine();

                        //Drop the associated view
                        var viewName = $"{modelOld.TenantPrefix}_{oldT.DatabaseName}";
                        sb.AppendLine($"if exists (select name from sys.objects where name = '{viewName}'  AND type = 'V')");
                        sb.AppendLine($"DROP VIEW [{viewName}]");
                        sb.AppendLine();

                        //Drop the tenant field
                        sb.AppendLine($"if exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id where c.name = '{modelNew.TenantColumnName}' and t.name = '{newT.DatabaseName}')");
                        sb.AppendLine($"ALTER TABLE [{newT.GetSQLSchema()}].[{newT.DatabaseName}] DROP COLUMN [{modelNew.TenantColumnName}]");
                        sb.AppendLine();
                    }
                    else if (!oldT.IsTenant && newT.IsTenant)
                    {
                        //Add the tenant field
                        sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlCreateTenantColumn(modelNew, newT));

                        //Add tenant view
                        var grantSB = new StringBuilder();
                        sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlTenantView(modelNew, newT, grantSB));
                        if (grantSB.ToString() != string.Empty)
                            sb.AppendLine(grantSB.ToString());
                    }
                    else if (oldT.IsTenant && newT.IsTenant && oldT.DatabaseName != newT.DatabaseName)
                    {
                        //If rename tenant table then delete old view and create new view

                        //Drop the old view
                        var viewName = modelOld.TenantPrefix + "_" + oldT.DatabaseName;
                        sb.AppendLine($"--DROP OLD TENANT VIEW FOR TABLE [{oldT.DatabaseName}]");
                        sb.AppendLine($"if exists (select name from sys.objects where name = '{viewName}'  AND type = 'V')");
                        sb.AppendLine($"DROP VIEW [{viewName}]");
                        sb.AppendLine("GO");

                        //Add tenant view
                        var grantSB = new StringBuilder();
                        sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlTenantView(modelNew, newT, grantSB));
                        if (grantSB.ToString() != string.Empty)
                            sb.AppendLine(grantSB.ToString());

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
                            sb.AppendLine();
                            sb.AppendLine("--GENERATION NOTE **");
                            sb.AppendLine("--THE PRIMARY KEY HAS CHANGED, THIS MAY REQUIRE MANUAL INTERVENTION");
                            sb.AppendLine("--THE FOLLOWING SCRIPT WILL DROP AND RE-ADD THE PRIMARY KEY HOWEVER IF THERE ARE RELATIONSHIPS");
                            sb.AppendLine("--BASED ON THIS IT, THE SCRIPT WILL FAIL. YOU MUST DROP ALL FOREIGN KEYS FIRST.");
                            sb.AppendLine();

                            //Before drop PK remove all FK to the table
                            foreach (var r1 in oldT.GetRelations().ToList())
                            {
                                sb.Append(SQLEmit.GetSqlRemoveFK(r1));
                                sb.AppendLine("GO");
                                sb.AppendLine();
                            }

                            var tableName = Globals.GetTableDatabaseName(modelNew, newT);
                            var pkName = "PK_" + tableName;
                            pkName = pkName.ToUpper();
                            sb.AppendLine($"----DROP PRIMARY KEY FOR TABLE [{tableName}]");
                            sb.AppendLine($"--if exists(select * from sys.objects where name = '{pkName}' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')");
                            sb.AppendLine($"--ALTER TABLE [{newT.GetSQLSchema()}].[{tableName}] DROP CONSTRAINT [{pkName}]");
                            sb.AppendLine("--GO");

                            var sql = SQLEmit.GetSqlCreatePK(newT) + "GO\r\n";
                            var lines = sql.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

                            //Comment the whole SQL block
                            var index = 0;
                            foreach (var s in lines)
                            {
                                var l = s;
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
                        sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.CreatedByColumnName, modelNew.Database.CreatedByColumnName));
                        sb.AppendLine("GO");
                    }

                    if (modelOld.Database.CreatedDateColumnName != modelNew.Database.CreatedDateColumnName)
                    {
                        sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.CreatedDateColumnName, modelNew.Database.CreatedDateColumnName));
                        sb.AppendLine("GO");
                    }

                    if (modelOld.Database.ModifiedByColumnName != modelNew.Database.ModifiedByColumnName)
                    {
                        sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.ModifiedByColumnName, modelNew.Database.ModifiedByColumnName));
                        sb.AppendLine("GO");
                    }

                    if (modelOld.Database.ModifiedDateColumnName != modelNew.Database.ModifiedDateColumnName)
                    {
                        sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.ModifiedDateColumnName, modelNew.Database.ModifiedDateColumnName));
                        sb.AppendLine("GO");
                    }

                    if (modelOld.Database.TimestampColumnName != modelNew.Database.TimestampColumnName)
                    {
                        sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.TimestampColumnName, modelNew.Database.TimestampColumnName));
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
            foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly))
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
                        sb.AppendLine($"if not exists(select * from sys.schemas where name = '{newT.GetSQLSchema()}')");
                        sb.AppendLine($"exec('CREATE SCHEMA [{newT.GetSQLSchema()}]')");
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

            //Views
            var removedItems = 0;
            foreach (var oldT in modelOld.Database.CustomViews.OrderBy(x => x.Name))
            {
                var newT = modelNew.Database.CustomViews.FirstOrDefault(x => x.Key == oldT.Key);
                if (newT == null)
                {
                    sb.AppendLine($"if exists (select * from sys.objects where name = '{oldT.DatabaseName}' and [type] in ('V'))");
                    sb.AppendLine($"drop view [{oldT.DatabaseName}]");
                    removedItems++;
                }
                else if (newT.DatabaseName != oldT.DatabaseName)
                {
                    //Name changed so remove old
                    sb.AppendLine($"if exists (select * from sys.objects where name = '{oldT.DatabaseName}' and [type] in ('V'))");
                    sb.AppendLine($"drop view [{oldT.DatabaseName}]");
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

            if (removedItems > 0)
            {
                sb.AppendLine("GO");
                sb.AppendLine();
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
                            sb.AppendLine(
                                $"if exists(select t.name, c.name from sys.columns c inner join sys.tables t on c.object_id = t.object_id inner join sys.schemas s on t.schema_id = s.schema_id where and t.name = '{newT.DatabaseName}' and c.name = '{newC.DatabaseName}' and s.name = '{newT.GetSQLSchema()}')");
                            sb.AppendLine($"ALTER TABLE [{newT.GetSQLSchema()}].[{newT.DatabaseName}] DROP COLUMN [{newC.DatabaseName}]");
                            sb.AppendLine("GO");
                            sb.AppendLine($"ALTER TABLE [{newT.GetSQLSchema()}].[{newT.DatabaseName}] ADD [{newC.DatabaseName}] AS ({newC.Formula})");
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

        #endregion

    }
}