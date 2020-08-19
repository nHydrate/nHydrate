using nHydrate.Generator.Common.Models;
using nHydrate.ModelManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static nHydrate.Generator.Common.Models.Relation;

namespace nHydrate.Command.Core
{
    internal static class ModelHelper
    {
        //Copy from Dsl
        public enum IndexTypeConstants
        {
            PrimaryKey,
            IsIndexed,
            User,
        }

        //Copy from Dsl
        public enum DataTypeConstants
        {
            BigInt,
            Binary,
            Bit,
            Char,
            Date,
            DateTime,
            DateTime2,
            DateTimeOffset,
            Decimal,
            Float,
            Image,
            Int,
            Money,
            NChar,
            NText,
            NVarChar,
            Real,
            SmallDateTime,
            SmallInt,
            SmallMoney,
            Structured,
            Text,
            Time,
            Timestamp,
            TinyInt,
            Udt,
            UniqueIdentifier,
            VarBinary,
            VarChar,
            Variant,
            Xml,
        }

        public static nHydrate.Generator.Common.Models.ModelRoot CreatePOCOModel(string modelFile)
        {
            var fi = new FileInfo(modelFile);
            if (!fi.Exists)
                throw new ModelException("The model file does not exist.");

            var diskModel = FileManagement.Load(fi.DirectoryName, fi.Name);
            try
            {
                var root = new nHydrate.Generator.Common.Models.ModelRoot(null);
                //root.EnableCustomChangeEvents = diskModel.EmitChangeScripts;
                //root.CompanyName = model.CompanyName;
                //root.EmitSafetyScripts = model.EmitSafetyScripts;
                //root.DefaultNamespace = model.DefaultNamespace;
                //root.ProjectName = model.ProjectName;
                //root.SupportLegacySearchObject = false;
                //root.UseUTCTime = model.UseUTCTime;
                //root.Version = model.Version;
                //root.Database.ResetKey(model.Id.ToString());
                //root.OutputTarget = string.Empty; //model.OutputTarget;
                ////These have the same mapping values flags so we need convert to int and then convert to the other enumeration
                //root.TenantColumnName = model.TenantColumnName;
                //root.Database.CreatedByColumnName = model.CreatedByColumnName;
                //root.Database.CreatedDateColumnName = model.CreatedDateColumnName;
                //root.Database.ModifiedByColumnName = model.ModifiedByColumnName;
                //root.Database.ModifiedDateColumnName = model.ModifiedDateColumnName;
                //root.Database.TimestampColumnName = model.TimestampColumnName;
                //root.Database.GrantExecUser = model.GrantUser;

                #region Load the entities
                foreach (var entity in diskModel.Entities)
                {
                    #region Table Info
                    var newTable = root.Database.Tables.Add();
                    newTable.ResetKey(entity.id.ToString());
                    newTable.ResetId(HashString(newTable.Key));
                    newTable.AllowCreateAudit = entity.allowcreateaudit.ToBool();
                    newTable.AllowModifiedAudit = entity.allowmodifyaudit.ToBool();
                    newTable.AllowTimestamp = entity.allowtimestamp.ToBool();
                    newTable.AssociativeTable = entity.isassociative.ToBool();
                    newTable.CodeFacade = entity.codefacade;
                    newTable.DBSchema = entity.schema;
                    newTable.Description = entity.summary;
                    newTable.Immutable = entity.immutable.ToBool();
                    newTable.TypedTable = (nHydrate.Generator.Common.Models.TypedTableConstants)Enum.Parse(typeof(nHydrate.Generator.Common.Models.TypedTableConstants), entity.typedentity.ToString(), true);
                    newTable.Name = entity.name;
                    newTable.GeneratesDoubleDerived = entity.generatesdoublederived.ToBool();
                    newTable.IsTenant = entity.isTenant.ToBool();
                    #endregion

                    #region Load the fields for this entity
                    var fieldList = entity.fieldset.ToList();
                    foreach (var field in fieldList.OrderBy(x => x.sortorder))
                    {
                        var newColumn = root.Database.Columns.Add();
                        newColumn.ResetKey(field.id.ToString());
                        newColumn.ResetId(HashString(newColumn.Key));
                        newColumn.AllowNull = field.nullable.ToBool();
                        newColumn.CodeFacade = field.codefacade;
                        newColumn.ComputedColumn = field.Iscalculated.ToBool();
                        newColumn.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.datatype.ToString());
                        newColumn.Default = field.@default;
                        newColumn.DefaultIsFunc = field.defaultisfunc.ToBool();
                        newColumn.Description = field.summary;
                        newColumn.Formula = field.formula;
                        newColumn.Identity = (nHydrate.Generator.Common.Models.IdentityTypeConstants)Enum.Parse(typeof(nHydrate.Generator.Common.Models.IdentityTypeConstants), field.identity.ToString());
                        newColumn.IsIndexed = field.isindexed.ToBool();
                        newColumn.IsReadOnly = field.isreadonly.ToBool();
                        newColumn.IsUnique = field.isunique.ToBool();
                        newColumn.Length = field.length;
                        newColumn.Name = field.name;
                        newColumn.ParentTableRef = newTable.CreateRef(newTable.Key);
                        newColumn.PrimaryKey = field.isprimarykey.ToBool();
                        newColumn.Scale = field.scale;
                        newColumn.SortOrder = field.sortorder;
                        newColumn.Obsolete = field.obsolete.ToBool();
                        newTable.Columns.Add(newColumn.CreateRef(newColumn.Key));
                    }
                    #endregion
                }

                #endregion

                #region Indexes

                foreach (var outter in diskModel.Indexes)
                {
                    var newTable = root.Database.Tables.First(x => x.Key == outter.id);
                    foreach (var index in outter.index)
                    {
                        var newIndex = new nHydrate.Generator.Common.Models.TableIndex(newTable.Root)
                        {
                            Description = index.summary,
                            IsUnique = index.isunique.ToBool(),
                            Clustered = index.clustered.ToBool(),
                            PrimaryKey = (index.indextype == (byte)IndexTypeConstants.PrimaryKey)
                        };
                        newTable.TableIndexList.Add(newIndex);
                        newIndex.ResetKey(index.id.ToString());
                        newIndex.ResetId(HashString(newIndex.Key));
                        newIndex.ImportedName = index.importedname;

                        //Add index columns
                        foreach (var ic in index.indexcolumnset.OrderBy(x => x.sortorder))
                        {
                            var newColumn = new nHydrate.Generator.Common.Models.TableIndexColumn(newTable.Root)
                            {
                                Ascending = ic.ascending.ToBool(),
                                FieldID = new Guid(ic.fieldid)
                            };
                            newIndex.IndexColumnList.Add(newColumn);
                        }
                    }
                }

                #endregion

                #region Static Data
                foreach (var entity in diskModel.Entities)
                {
                    //Determine how many rows there are
                    var dataList = diskModel.StaticData.Where(x => x.id == entity.id).SelectMany(x => x.data).ToList();
                    var orderKeyList = dataList.Select(x => x.orderkey).Distinct().ToList();
                    var rowCount = orderKeyList.Count;

                    var newTable = root.Database.Tables.First(x => x.Key == entity.id);

                    //Create a OLD static data row for each one
                    for (var ii = 0; ii < rowCount; ii++)
                    {
                        //For each row create N cells one for each column
                        var rowEntry = new nHydrate.Generator.Common.Models.RowEntry(newTable.Root);
                        var staticDataFieldList = entity.fieldset.Where(x => !(GetDataType(x.datatype).IsBinaryType()) && GetDataType(x.datatype) != DataTypeConstants.Timestamp).ToList();
                        for (var jj = 0; jj < staticDataFieldList.Count; jj++)
                        {
                            var cellEntry = new nHydrate.Generator.Common.Models.CellEntry(newTable.Root);
                            var column = newTable.GetColumns().ToList()[jj];
                            cellEntry.ColumnRef = column.CreateRef(column.Key);

                            var currentColumn = entity.fieldset.FirstOrDefault(x => x.id == column.Key);
                            if (currentColumn != null)
                            {
                                var dataum = dataList.FirstOrDefault(x =>
                                    x.columnkey == currentColumn.id &&
                                    x.orderkey == orderKeyList[ii]);

                                if (dataum != null)
                                {
                                    cellEntry.Value = dataum.value;
                                    cellEntry.ResetKey(dataum.columnkey);
                                }

                                //Add the cell to the row
                                rowEntry.CellEntries.Add(cellEntry);
                            }
                        }
                        newTable.StaticData.Add(rowEntry);
                    }
                }
                #endregion

                #region Relations
                foreach (var entity in diskModel.Entities)
                {
                    foreach (var relationNode in diskModel.Relations.Where(x => x.id == entity.id).ToList())
                    {
                        //var relationConnectors = diagram.NestedChildShapes.Where(x => x is EntityAssociationConnector).Cast<EntityAssociationConnector>().ToList();
                        foreach (var shape in relationNode.relation)
                        {
                            //var connector = shape as EntityAssociationConnector;
                            //var parent = connector.FromShape.ModelElement as Entity;
                            //var child = connector.ToShape.ModelElement as Entity;

                            //var relation = connector.ModelElement as EntityHasEntities;
                            //var fieldList = model.RelationFields.Where(x => x.RelationID == relation.Id);

                            var parentTable = root.Database.Tables.FirstOrDefault(x => x.Key == shape.id);
                            var childTable = root.Database.Tables.FirstOrDefault(x => x.Key == shape.childid);

                            //If we found both parent and child tables...
                            if (parentTable != null && childTable != null)
                            {
                                var isValidRelation = true;
                                if (isValidRelation)
                                {
                                    var newRelation = root.Database.Relations.Add();
                                    newRelation.ResetKey(shape.id);
                                    newRelation.ResetId(HashString(newRelation.Key));
                                    newRelation.ParentTableRef = parentTable.CreateRef(parentTable.Key);
                                    newRelation.ChildTableRef = childTable.CreateRef(childTable.Key);
                                    newRelation.RoleName = shape.rolename;
                                    var da = (DeleteActionConstants)Enum.Parse(typeof(DeleteActionConstants), shape.deleteaction);
                                    switch (da)
                                    {
                                        case DeleteActionConstants.Cascade:
                                            newRelation.DeleteAction = Relation.DeleteActionConstants.Cascade;
                                            break;
                                        case DeleteActionConstants.NoAction:
                                            newRelation.DeleteAction = Relation.DeleteActionConstants.NoAction;
                                            break;
                                        case DeleteActionConstants.SetNull:
                                            newRelation.DeleteAction = Relation.DeleteActionConstants.SetNull;
                                            break;
                                    }

                                    newRelation.Enforce = shape.isenforced.ToBool();

                                    //Create the column links
                                    foreach (var columnSet in shape.relationfieldset)
                                    {
                                        var field1 = parentTable.GetColumns().FirstOrDefault(x => x.Key == columnSet.sourcefieldid);
                                        var field2 = childTable.GetColumns().FirstOrDefault(x => x.Key == columnSet.targetfieldid);

                                        var column1 = parentTable.GetColumnsFullHierarchy().FirstOrDefault(x => x.Name == field1.Name);
                                        var column2 = childTable.GetColumnsFullHierarchy().FirstOrDefault(x => x.Name == field2.Name);

                                        newRelation.ColumnRelationships.Add(new nHydrate.Generator.Common.Models.ColumnRelationship(root)
                                        {
                                            ParentColumnRef = column1.CreateRef(column1.Key),
                                            ChildColumnRef = column2.CreateRef(column2.Key),
                                        }
                                        );
                                    }

                                    //Actually add the relation to the collection
                                    if (newRelation.ColumnRelationships.Count > 0)
                                        parentTable.Relationships.Add(newRelation.CreateRef(newRelation.Key));
                                }
                            }
                        }

                    } //inner block
                }
                #endregion

                #region Views
                foreach (var view in diskModel.Views)
                {
                    var newView = root.Database.CustomViews.Add();
                    newView.ResetKey(view.id);
                    newView.ResetId(HashString(newView.Key));
                    newView.CodeFacade = view.codefacade;
                    newView.DBSchema = view.schema;
                    newView.Description = view.summary;
                    newView.Name = view.name;
                    newView.SQL = view.sql;
                    newView.GeneratesDoubleDerived = view.generatesdoublederived.ToBool();

                    foreach (var field in view.fieldset)
                    {
                        var newField = root.Database.CustomViewColumns.Add();
                        newField.ResetKey(field.id);
                        newField.ResetId(HashString(newField.Key));
                        newField.AllowNull = field.nullable.ToBool();
                        newField.CodeFacade = field.codefacade;
                        newField.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.datatype.ToString());
                        newField.Default = field.@default;
                        newField.Description = field.summary;
                        newField.IsPrimaryKey = field.isprimarykey.ToBool();
                        newField.Length = field.length;
                        newField.Name = field.name;
                        newField.Scale = field.scale;
                        newView.Columns.Add(newField.CreateRef(newField.Key));
                        newField.ParentViewRef = newView.CreateRef(newView.Key);
                    }

                }
                #endregion

                return root;

            }
            catch (Exception ex)
            {
                throw new ModelException("The model file could not be loaded.");
            }
        }

        private static int HashString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            uint hash = 0;
            foreach (var b in System.Text.Encoding.Unicode.GetBytes(s))
            {
                hash += b;
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }
            // final avalanche
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            return (int)(hash % int.MaxValue);
        }

        private static bool ToBool(this byte value)
        {
            return value != 0;
        }

        private static DataTypeConstants GetDataType(string str)
        {
            Enum.TryParse<DataTypeConstants>(str, out DataTypeConstants v);
            return v;
        }

        private static bool IsBinaryType(this DataTypeConstants dataType)
        {
            switch (dataType)
            {
                case DataTypeConstants.Binary:
                case DataTypeConstants.Image:
                case DataTypeConstants.VarBinary:
                    return true;
                default:
                    return false;
            }
        }
    }
}
