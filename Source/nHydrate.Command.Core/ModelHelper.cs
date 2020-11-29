using nHydrate.Generator.Common.Models;
using nHydrate.ModelManagement;
using System;
using System.IO;
using System.Linq;
using static nHydrate.Generator.Common.Models.Relation;
using nHydrate.Generator.Common.Util;

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
                throw new ModelException($"The model file '{fi.FullName}' does not exist.");

            var diskModel = FileManagement.Load2(fi.DirectoryName, fi.Name, out bool wasLoaded);
            if (!wasLoaded)
                throw new ModelException($"The model file '{fi.FullName}' does not exist.");

            var q = 0;
            if (q == 1)
                FileManagement.Save2(fi.DirectoryName, fi.Name.Replace(".nhydrate", ".model"), diskModel);

            try
            {
                var root = new nHydrate.Generator.Common.Models.ModelRoot(null);
                root.CompanyName = diskModel.ModelProperties.CompanyName;
                root.EmitSafetyScripts = diskModel.ModelProperties.EmitSafetyScripts;
                root.DefaultNamespace = diskModel.ModelProperties.DefaultNamespace;
                root.ProjectName = diskModel.ModelProperties.ProjectName;
                root.SupportLegacySearchObject = false;
                root.UseUTCTime = diskModel.ModelProperties.UseUTCTime;
                root.Version = diskModel.ModelProperties.Version;
                root.ResetKey(diskModel.ModelProperties.Id);
                root.OutputTarget = string.Empty;
                root.TenantColumnName = diskModel.ModelProperties.TenantColumnName;
                root.Database.CreatedByColumnName = diskModel.ModelProperties.CreatedByColumnName;
                root.Database.CreatedDateColumnName = diskModel.ModelProperties.CreatedDateColumnName;
                root.Database.ModifiedByColumnName = diskModel.ModelProperties.ModifiedByColumnName;
                root.Database.ModifiedDateColumnName = diskModel.ModelProperties.ModifiedDateColumnName;
                root.Database.ConcurrencyCheckColumnName = diskModel.ModelProperties.ConcurrencyCheckColumnName;
                root.Database.GrantExecUser = diskModel.ModelProperties.GrantExecUser;

                #region Load the entities
                foreach (var entity in diskModel.Entities)
                {
                    #region Table Info
                    var newTable = root.Database.Tables.Add();
                    newTable.ResetKey(entity.Id.ToString());
                    newTable.ResetId(HashString(newTable.Key));
                    newTable.AllowCreateAudit = entity.AllowCreateAudit;
                    newTable.AllowModifiedAudit = entity.AllowModifyAudit;
                    newTable.AllowConcurrencyCheck = entity.AllowTimestamp;
                    newTable.AssociativeTable = entity.IsAssociative;
                    newTable.CodeFacade = entity.CodeFacade;
                    newTable.DBSchema = entity.Schema;
                    newTable.Description = entity.Summary;
                    newTable.Immutable = entity.Immutable;
                    newTable.TypedTable = entity.TypedTable.Convert<TypedTableConstants>();
                    newTable.Name = entity.Name;
                    newTable.GeneratesDoubleDerived = entity.GeneratesDoubleDerived;
                    newTable.IsTenant = entity.IsTenant;
                    #endregion

                    #region Load the fields for this entity
                    var loopIndex = 0;
                    foreach (var field in entity.Fields)
                    {
                        var newColumn = root.Database.Columns.Add();
                        newColumn.ResetKey(field.Id.ToString());
                        newColumn.ResetId(HashString(newColumn.Key));
                        newColumn.AllowNull = field.Nullable;
                        newColumn.CodeFacade = field.CodeFacade;
                        newColumn.ComputedColumn = field.IsCalculated;
                        newColumn.DataType = field.Datatype.Convert<System.Data.SqlDbType>();
                        newColumn.Default = field.Default;
                        newColumn.DefaultIsFunc = field.DefaultIsFunc;
                        newColumn.Description = field.Summary;
                        newColumn.Formula = field.Formula;
                        newColumn.Identity = field.Identity.Convert<IdentityTypeConstants>();
                        newColumn.IsIndexed = field.IsIndexed;
                        newColumn.IsReadOnly = field.IsReadonly;
                        newColumn.IsUnique = field.IsUnique;
                        newColumn.Length = field.Length;
                        newColumn.Name = field.Name;
                        newColumn.ParentTableRef = newTable.CreateRef(newTable.Key);
                        newColumn.PrimaryKey = field.IsPrimaryKey;
                        newColumn.Scale = field.Scale;
                        newColumn.SortOrder = loopIndex++;
                        newColumn.Obsolete = field.Obsolete;
                        newTable.Columns.Add(newColumn.CreateRef(newColumn.Key));
                    }
                    #endregion

                    #region Indexes

                    foreach (var index in entity.Indexes)
                    {
                        var newIndex = new nHydrate.Generator.Common.Models.TableIndex(newTable.Root)
                        {
                            Description = index.Summary,
                            IsUnique = index.IsUnique,
                            Clustered = index.Clustered,
                            PrimaryKey = (index.IndexType == (byte)IndexTypeConstants.PrimaryKey)
                        };
                        newTable.TableIndexList.Add(newIndex);
                        //newIndex.ResetKey(index.Id.ToString());
                        newIndex.ResetId(HashString(newIndex.Key));
                        newIndex.ImportedName = index.ImportedName;

                        //Add index columns
                        foreach (var ic in index.Fields)
                        {
                            var newColumn = new nHydrate.Generator.Common.Models.TableIndexColumn(newTable.Root)
                            {
                                Ascending = ic.Ascending,
                                FieldID = ic.FieldId,
                            };
                            newIndex.IndexColumnList.Add(newColumn);
                        }
                    }

                    #endregion

                    #region Static Data
                    //foreach (var entity in diskModel.Entities)
                    {
                        //Determine how many rows there are
                        //var dataList = entity.StaticData.Where(x => x.id == entity.id).SelectMany(x => x.data).ToList();
                        var orderKeyList = entity.StaticData.Select(x => x.SortOrder).Distinct().ToList();
                        //var rowCount = orderKeyList.Count;

                        //Create a OLD static data row for each one
                        for (var ii = 0; ii < orderKeyList.Count; ii++)
                        {
                            //For each row create N cells one for each column
                            var rowEntry = new nHydrate.Generator.Common.Models.RowEntry(newTable.Root);
                            var staticDataFieldList = entity.Fields.Where(x => !(GetDataType(x.Datatype.ToString()).IsBinaryType()) && GetDataType(x.Datatype.ToString()) != DataTypeConstants.Timestamp).ToList();
                            for (var jj = 0; jj < staticDataFieldList.Count; jj++)
                            {
                                var cellEntry = new nHydrate.Generator.Common.Models.CellEntry(newTable.Root);
                                var column = newTable.GetColumns().ToList()[jj];
                                cellEntry.ColumnRef = column.CreateRef(column.Key);

                                var currentColumn = entity.Fields.FirstOrDefault(x => x.Id == column.Key.ToGuid());
                                if (currentColumn != null)
                                {
                                    var dataum = entity.StaticData.FirstOrDefault(x =>
                                        x.ColumnId == currentColumn.Id &&
                                        x.SortOrder == orderKeyList[ii]);

                                    if (dataum != null)
                                    {
                                        cellEntry.Value = dataum.Value;
                                        cellEntry.ResetKey(dataum.ColumnId.ToString());
                                    }

                                    //Add the cell to the row
                                    rowEntry.CellEntries.Add(cellEntry);
                                }
                            }
                            newTable.StaticData.Add(rowEntry);
                        }
                    }
                    #endregion
                }

                #endregion

                #region Relations
                foreach (var entity in diskModel.Entities)
                {
                    foreach (var shape in entity.Relations)
                    {
                        //var relationConnectors = diagram.NestedChildShapes.Where(x => x is EntityAssociationConnector).Cast<EntityAssociationConnector>().ToList();
                        //foreach (var shape in relationNode.relation)
                        {
                            //var connector = shape as EntityAssociationConnector;
                            //var parent = connector.FromShape.ModelElement as Entity;
                            //var child = connector.ToShape.ModelElement as Entity;

                            //var relation = connector.ModelElement as EntityHasEntities;
                            //var fieldList = model.RelationFields.Where(x => x.RelationID == relation.Id);

                            //var parentTable = root.Database.Tables.FirstOrDefault(x => x.Key == shape.id);
                            var parentTable = root.Database.Tables.FirstOrDefault(x => x.Key.ToGuid() == entity.Id);
                            var childTable = root.Database.Tables.FirstOrDefault(x => x.Key.ToGuid() == shape.ForeignEntityId);

                            //If we found both parent and child tables...
                            if (parentTable != null && childTable != null)
                            {
                                var isValidRelation = true;
                                if (isValidRelation)
                                {
                                    var newRelation = root.Database.Relations.Add();
                                    //newRelation.ResetKey(shape.Id);
                                    newRelation.ResetId(HashString(newRelation.Key));
                                    newRelation.ParentTableRef = parentTable.CreateRef(parentTable.Key);
                                    newRelation.ChildTableRef = childTable.CreateRef(childTable.Key);
                                    newRelation.RoleName = shape.RoleName;
                                    newRelation.Enforce = shape.IsEnforced;
                                    newRelation.DeleteAction = shape.DeleteAction.Convert<DeleteActionConstants>();

                                    //Create the column links
                                    foreach (var columnSet in shape.Fields)
                                    {
                                        var field1 = parentTable.GetColumns().FirstOrDefault(x => new Guid(x.Key) == columnSet.PrimaryFieldId);
                                        var field2 = childTable.GetColumns().FirstOrDefault(x => new Guid(x.Key) == columnSet.ForeignFieldId);

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
                            else
                            {
                                //Should never get here
                            }
                        }

                    } //inner block
                }
                #endregion

                #region Views
                foreach (var view in diskModel.Views)
                {
                    var newView = root.Database.CustomViews.Add();
                    newView.ResetKey(view.Id.ToString());
                    newView.ResetId(HashString(newView.Key));
                    newView.CodeFacade = view.CodeFacade;
                    newView.DBSchema = view.Schema;
                    newView.Description = view.Summary;
                    newView.Name = view.Name;
                    newView.SQL = view.Sql;
                    newView.GeneratesDoubleDerived = view.GeneratesDoubleDerived;

                    foreach (var field in view.Fields)
                    {
                        var newField = root.Database.CustomViewColumns.Add();
                        newField.ResetKey(field.Id);
                        newField.ResetId(HashString(newField.Key));
                        newField.AllowNull = field.Nullable;
                        newField.CodeFacade = field.CodeFacade;
                        newField.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.Datatype.ToString());
                        newField.Default = field.Default;
                        newField.Description = field.Summary;
                        newField.IsPrimaryKey = field.IsPrimaryKey;
                        newField.Length = field.Length;
                        newField.Name = field.Name;
                        newField.Scale = field.Scale;
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

        private static bool ToBool(this byte value) => value != 0;

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

        public static string ToIndentedString(this System.Xml.XmlDocument doc)
        {
            var stringWriter = new System.IO.StringWriter(new System.Text.StringBuilder());
            var xmlTextWriter = new System.Xml.XmlTextWriter(stringWriter)
            {
                Formatting = System.Xml.Formatting.Indented,
                IndentChar = '\t'
            };
            doc.Save(xmlTextWriter);
            var t = stringWriter.ToString();
            t = t.Replace(@" encoding=""utf-16""", string.Empty);
            return t;
        }
    }
}
