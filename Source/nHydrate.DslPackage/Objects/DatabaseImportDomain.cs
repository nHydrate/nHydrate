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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using nHydrate.Dsl;
using nHydrate.DslPackage;
using System.IO;
using System.Windows.Forms;

namespace nHydrate.DslPackage.Objects
{
    internal static class DatabaseImportDomain
    {
        public static void ImportDatabase(nHydrate.Dsl.nHydrateModel model, Store store, Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram, nHydrate.DataImport.Database database, Module module)
        {

            try
            {
                //Find Stored procs with no loaded columns
                var noColumList = new List<string>();
                database.StoredProcList
                        .Where(x => (x.ImportState == DataImport.ImportStateConstants.Added || x.ImportState == DataImport.ImportStateConstants.Modified) && x.ColumnFailure)
                        .ToList()
                        .ForEach(x => noColumList.Add(x.Name));

                if (noColumList.Count > 0)
                {
                    MessageBox.Show("The output fields could not be determined for the following stored procedures. The fields collection of each will not be modified.\r\n\r\n" + string.Join("\r\n", noColumList.ToArray()), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            ((nHydrate.Dsl.nHydrateDiagram)diagram).IsLoading = true;
            model.IsLoading = true;
            var pkey = ProgressHelper.ProgressingStarted("Processing Import...", true);
            model.IsLoading = true;
            try
            {
                var addedEntities = new List<Entity>();
                var diagramEntities = model.Entities.ToList();
                var diagramStoredProcs = model.StoredProcedures.ToList();
                var diagramViews = model.Views.ToList();
                var diagramFunctions = model.Functions.ToList();
                using (var transaction = store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                {
                    #region TEMP TEMP - RESET THE PARAMETERS AND FIELDS OF ALL NON-ENTITY OBJECTS - USED FOR DEBUGGING

                    //foreach (var entity in model.Entities)
                    //{
                    //  var table = database.EntityList.FirstOrDefault(x => x.Name == entity.Name);
                    //  if (table != null)
                    //  {
                    //    foreach (var field in entity.Fields)
                    //    {
                    //      var column = table.FieldList.FirstOrDefault(x => x.Name == field.Name);
                    //      if (column != null)
                    //      {
                    //        field.SortOrder = column.SortOrder;
                    //      }
                    //    }
                    //  }
                    //  entity.Fields.Sort((x, y) => (x.SortOrder < y.SortOrder ? -1 : 0));
                    //}

                    //var spList = database.StoredProcList.Where(x => x.ParameterList.Count(z => z.IsOutputParameter) > 0).ToList();
                    //foreach (var sp in spList)
                    //{
                    //  var newSP = model.StoredProcedures.FirstOrDefault(x => x.Name == sp.Name);
                    //  if (newSP != null)
                    //  {
                    //    foreach (var p in sp.ParameterList.Where(x => x.IsOutputParameter))
                    //    {
                    //      var newParameter = newSP.Parameters.FirstOrDefault(x => x.Name == p.Name);
                    //      if (newParameter != null)
                    //      {
                    //        newParameter.IsOutputParameter = true;
                    //      }
                    //    }
                    //  }
                    //}

                    //int paramCount = 0;
                    //int fieldCount = 0;
                    //foreach (var storedProc in database.StoredProcList.Where(x => x.ImportState == DataImport.ImportStateConstants.Added || x.ImportState == DataImport.ImportStateConstants.Modified))
                    //{
                    //  var newStoredProc = diagramStoredProcs.FirstOrDefault(x => x.Name.ToLower() == storedProc.Name.ToLower());
                    //  if (newStoredProc != null)
                    //  {
                    //    //Fields
                    //    foreach (var field in storedProc.FieldList)
                    //    {
                    //      var newField = newStoredProc.Fields.FirstOrDefault(x => x.Name == field.Name);
                    //      if (newField == null)
                    //      {
                    //        newField = new nHydrate.Dsl.StoredProcedureField(model.Partition);
                    //        newStoredProc.Fields.Add(newField);
                    //        newField.Name = field.Name;
                    //        newField.Length = field.Length;
                    //        newField.Nullable = field.Nullable;
                    //        newField.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), field.DataType.ToString());
                    //        newField.Default = field.DefaultValue;
                    //        newField.Scale = field.Scale;
                    //        fieldCount++;
                    //      }

                    //    }

                    //    //Parameters
                    //    foreach (var parameter in storedProc.ParameterList)
                    //    {
                    //      var newParameter = newStoredProc.Parameters.FirstOrDefault(x => x.Name == parameter.Name);
                    //      if (newParameter == null)
                    //      {
                    //        newParameter = new nHydrate.Dsl.StoredProcedureParameter(model.Partition);
                    //        newStoredProc.Parameters.Add(newParameter);
                    //        newParameter.Name = parameter.Name;
                    //        newParameter.SortOrder = parameter.SortOrder;
                    //        newParameter.Length = parameter.Length;
                    //        newParameter.Nullable = parameter.Nullable;
                    //        newParameter.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), parameter.DataType.ToString());
                    //        newParameter.Default = parameter.DefaultValue;
                    //        newParameter.Scale = parameter.Scale;
                    //        newParameter.IsOutputParameter = parameter.IsOutputParameter;
                    //        paramCount++;
                    //      }

                    //    }
                    //  }
                    //}

                    //foreach (var storedProc in database.StoredProcList)
                    //{
                    //  var newStoredProc = diagramStoredProcs.FirstOrDefault(x => x.Name.ToLower() == storedProc.Name.ToLower());
                    //  if (newStoredProc != null)
                    //  {
                    //    foreach (var field in storedProc.FieldList)
                    //    {
                    //      var newField = newStoredProc.Fields.FirstOrDefault(x => x.Name.ToLower() == field.Name.ToLower());
                    //      if (newField != null) newField.Nullable = field.Nullable;
                    //    }

                    //    foreach (var parameter in storedProc.ParameterList)
                    //    {
                    //      var newParameter = newStoredProc.Parameters.FirstOrDefault(x => x.Name.ToLower() == parameter.Name.ToLower());
                    //      if (newParameter != null) newParameter.Nullable = parameter.Nullable;
                    //    }
                    //  }
                    //}

                    //foreach (var view in database.ViewList)
                    //{
                    //  var newView = diagramViews.FirstOrDefault(x => x.Name.ToLower() == view.Name.ToLower());
                    //  if (newView != null)
                    //  {
                    //    foreach (var field in view.FieldList)
                    //    {
                    //      var newField = newView.Fields.FirstOrDefault(x => x.Name.ToLower() == field.Name.ToLower());
                    //      if (newField != null) newField.Nullable = field.Nullable;
                    //    }
                    //  }
                    //}

                    //foreach (var function in database.FunctionList)
                    //{
                    //  var newFunction = diagramFunctions.FirstOrDefault(x => x.Name.ToLower() == function.Name.ToLower());
                    //  if (newFunction != null)
                    //  {
                    //    foreach (var field in function.FieldList)
                    //    {
                    //      var newField = newFunction.Fields.FirstOrDefault(x => x.Name.ToLower() == field.Name.ToLower());
                    //      if (newField != null) newField.Nullable = field.Nullable;
                    //    }

                    //    foreach (var parameter in function.ParameterList)
                    //    {
                    //      var newParameter = newFunction.Parameters.FirstOrDefault(x => x.Name.ToLower() == parameter.Name.ToLower());
                    //      if (newParameter != null) newParameter.Nullable = parameter.Nullable;
                    //    }
                    //  }
                    //}

                    //foreach (var index in database.IndexList.Where(x => x.FieldList.Count == 1 && !x.FieldList.First().IsDescending))
                    //{
                    //  var entity = model.Entities.FirstOrDefault(x => x.Name == index.TableName);
                    //  if (entity != null)
                    //  {
                    //    var field = entity.Fields.FirstOrDefault(x => x.Name == index.FieldList.First().Name);
                    //    if (field != null)
                    //    {
                    //      field.IsIndexed = true;
                    //    }
                    //  }
                    //}

                    //transaction.Commit();
                    //return;

                    #endregion

                    #region Load Entities

                    #region Merge Module
                    foreach (var item in database.EntityList.Where(x => x.ImportState == DataImport.ImportStateConstants.Merge))
                    {
                        //Add module if necessary
                        var modelItem = diagramEntities.FirstOrDefault(x => x.Name.ToLower() == item.Name.ToLower());
                        if (module != null && !modelItem.Modules.Contains(module))
                        {
                            modelItem.Modules.Add(module);
                        }

                        foreach (var field in item.FieldList)
                        {
                            var newField = modelItem.Fields.FirstOrDefault(x => x.Name.ToLower() == field.Name.ToLower());

                            //Add module if necessary
                            if (module != null && newField != null && !newField.Modules.Contains(module))
                            {
                                newField.Modules.Add(module);
                            }
                        }

                    }
                    #endregion

                    var addedChangedEntities = database.EntityList.Where(x => x.ImportState == DataImport.ImportStateConstants.Added || x.ImportState == DataImport.ImportStateConstants.Modified).ToList();

                    #region Entities

                    foreach (var entity in addedChangedEntities)
                    {
                        var newEntity = diagramEntities.FirstOrDefault(x => x.Id == entity.ID);
                        if (newEntity == null) newEntity = diagramEntities.FirstOrDefault(x => x.Name.ToLower() == entity.Name.ToLower());
                        if (newEntity == null)
                        {
                            newEntity = new Entity(model.Partition) { Name = entity.Name };
                            model.Entities.Add(newEntity);
                            addedEntities.Add(newEntity);

                            //Correct for invalid identifiers
                            //if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newEntity.Name) && !nHydrate.Dsl.ValidationHelper.IsReservedWord(newEntity.Name))
                            if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newEntity.Name))
                            {
                                newEntity.CodeFacade = nHydrate.Dsl.ValidationHelper.MakeCodeIdentifer(newEntity.Name, string.Empty);
                            }
                        }
                        newEntity.AllowCreateAudit = entity.AllowCreateAudit;
                        newEntity.AllowModifyAudit = entity.AllowModifyAudit;
                        newEntity.AllowTimestamp = entity.AllowTimestamp;
                        newEntity.IsTenant = entity.IsTenant;
                        newEntity.Name = entity.Name;
                        newEntity.Schema = entity.Schema;

                        //Add module if necessary
                        if (module != null && !newEntity.Modules.Contains(module))
                        {
                            newEntity.Modules.Add(module);
                        }

                        PopulateFields(model, module, entity, newEntity);

                        //Order columns by database
                        //newEntity.Fields.Sort((x, y) => x.Name.CompareTo(y.Name));
                        newEntity.Fields.Sort((x, y) => (x.SortOrder < y.SortOrder ? -1 : 0));
                    }

                    #endregion

                    //Find all fields in the removed entities
                    var removalFieldIdList = model.Entities
                        .Where(x => database.EntityList
                            .Where(z => z.ImportState == DataImport.ImportStateConstants.Deleted)
                            .Select(a => a.Name)
                            .ToList()
                            .Contains(x.Name)).ToList()
                            .SelectMany(x => x.Fields).Select(x => x.Id);

                    ////Remove these fields from the relation field map collection
                    //model.RelationFields.Remove(x => removalFieldIdList.Contains(x.SourceFieldId) || removalFieldIdList.Contains(x.TargetFieldId));

                    //Remove the ones that need to be remove
                    model.Entities.Remove(x => database.EntityList.Where(z => z.ImportState == DataImport.ImportStateConstants.Deleted).Select(a => a.Name).ToList().Contains(x.Name));

                    #endregion

                    #region Load Relations
                    if (!database.IgnoreRelations)
                    {
                        var allRelationElementList = store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements
                            .ToList()
                            .Where(x => x is EntityHasEntities)
                            .ToList()
                            .Cast<EntityHasEntities>()
                            .ToList();

                        foreach (var entity in database.EntityList)
                        {
                            foreach (var relation in entity.RelationshipList)
                            {
                                var isNewConnection = false;
                                var connection = allRelationElementList.FirstOrDefault(x => x.ImportData == relation.ImportData);

                                //Now verify that this is the table has not been renamed
                                if (connection != null)
                                {
                                    //If the table names no longer match then create a new relation
                                    if (string.Compare(connection.ChildEntity.Name, relation.TargetEntity.Name, true) != 0 ||
                                        string.Compare(connection.ParentEntity.Name, relation.SourceEntity.Name, true) != 0)
                                    {
                                        connection.ImportData = string.Empty;
                                        connection = null;
                                    }
                                }

                                if (connection == null)
                                {
                                    //try to find this relation by table/fields/role
                                    connection = allRelationElementList.FirstOrDefault(x => x.GetCorePropertiesHash() == relation.CorePropertiesHash);
                                }
                                var parent = model.Entities.FirstOrDefault(x => x.Name == relation.SourceEntity.Name);
                                var child = model.Entities.FirstOrDefault(x => x.Name == relation.TargetEntity.Name);
                                if (connection == null)
                                {
                                    var existingRelation = diagram.NestedChildShapes.FirstOrDefault(x => x.Id == relation.ID);
                                    if (existingRelation == null)
                                    {
                                        if (child != null && parent != null)
                                        {

                                            //var currentList = store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
                                            //parent.ChildEntities.Add(child);
                                            //var updatedList = store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.ToList();
                                            //var last = updatedList.Last();
                                            //updatedList.RemoveAll(x => currentList.Contains(x));
                                            //connection = updatedList.First() as EntityHasEntities;
                                            //if (connection != last) System.Diagnostics.Debug.Write("");

                                            parent.ChildEntities.Add(child);
                                            connection = store.CurrentContext.Partitions.First().Value.ElementDirectory.AllElements.Last() as EntityHasEntities;

                                            isNewConnection = true;
                                            connection.ImportData = relation.ImportData;
                                        }
                                    }
                                } //Relation does not exist

                                //Add the relation fields
                                if (parent != null && child != null)
                                {
                                    foreach (var ritem in relation.RelationshipColumnList)
                                    {
                                        var parentField = parent.Fields.FirstOrDefault(x => x.Name == ritem.ParentField.Name);
                                        var childField = child.Fields.FirstOrDefault(x => x.Name == ritem.ChildField.Name);
                                        if (parentField != null && childField != null)
                                        {
                                            //Do not import the role name again.
                                            if (isNewConnection)
                                            {
                                                connection.RoleName = relation.RoleName;
                                                connection.ImportedConstraintName = relation.ConstraintName;
                                            }

                                            var currentRelationField = model.RelationFields.FirstOrDefault(x =>
                                                x.SourceFieldId == parentField.Id &&
                                                x.TargetFieldId == childField.Id &&
                                                x.RelationID == connection.Id);

                                            //Only add if not there already
                                            if (currentRelationField == null)
                                            {
                                                model.RelationFields.Add(
                                                    new RelationField(model.Partition)
                                                    {
                                                        SourceFieldId = parentField.Id,
                                                        TargetFieldId = childField.Id,
                                                        RelationID = connection.Id,
                                                    }
                                                    );
                                            }
                                        }
                                    } //Relation Columns
                                }

                            }
                        }
                    }
                    #endregion

                    #region Process Indexes

                    //Only get the single column indexes ascending
                    var isIndexedList = database.IndexList.Where(x => x.FieldList.Count == 1 && !x.FieldList.First().IsDescending).ToList();
                    var allIndexList = database.IndexList.Where(x => addedChangedEntities.Select(z => z.Name.ToLower()).Contains(x.TableName.ToLower())).ToList();

                    //Delete existing indexes by name. These will be recreated.
                    foreach (var index in allIndexList)
                    {
                        var existing = model.Entities.SelectMany(x => x.Indexes).FirstOrDefault(x => x.ImportedName == index.IndexName);
                        if (existing != null)
                        {
                            existing.Delete();
                        }
                    }

                    //Delete existing IsIndexed indexes with no import names as they will be recreated
                    var existingIsIndexedList = model.Entities.Where(x => addedChangedEntities.Select(z => z.Name.ToLower()).Contains(x.Name.ToLower())).SelectMany(x => x.Indexes).Where(x => x.IsIndexedType).ToList();
                    foreach (var index in isIndexedList)
                    {
                        foreach (var existing in existingIsIndexedList)
                        {
                            if (index.IsMatch(existing))
                                existing.Delete();
                        }
                    }

                    //Create all indexes
                    foreach (var index in allIndexList)
                    {
                        var entity = model.Entities.FirstOrDefault(x => x.Name == index.TableName);
                        if (entity != null)
                        {
                            var realFields = entity.Fields.Where(x => index.FieldList.Select(z => z.Name).Contains(x.Name)).ToList();
                            if (realFields.Count > 0)
                            {
                                //Try to get the PK if one exists
                                var isNew = true;
                                Index newIndex = null;
                                if (index.IsPrimaryKey)
                                {
                                    newIndex = entity.Indexes.FirstOrDefault(x => x.IndexType == IndexTypeConstants.PrimaryKey);
                                    if (newIndex != null)
                                    {
                                        isNew = false;
                                        newIndex.IndexColumns.Clear();
                                    }
                                }

                                //Create an index
                                if (newIndex == null)
                                    newIndex = new Index(entity.Partition);

                                newIndex.ParentEntityID = entity.Id;
                                newIndex.ImportedName = index.IndexName;
                                newIndex.Clustered = index.Clustered;
                                newIndex.IsUnique = index.IsUnique;

                                if (index.IsPrimaryKey)
                                    newIndex.IndexType = IndexTypeConstants.PrimaryKey;
                                else if (isIndexedList.Contains(index))
                                    newIndex.IndexType = IndexTypeConstants.IsIndexed;
                                else
                                    newIndex.IndexType = IndexTypeConstants.User;

                                if (isNew)
                                    entity.Indexes.Add(newIndex);

                                foreach (var ic in index.FieldList)
                                {
                                    var field = realFields.FirstOrDefault(x => x.Name == ic.Name);
                                    if (field != null)
                                    {
                                        var newIndexColumn = new IndexColumn(entity.Partition);
                                        newIndexColumn.Ascending = !ic.IsDescending;
                                        newIndexColumn.FieldID = field.Id;
                                        newIndexColumn.IsInternal = true;
                                        newIndexColumn.SortOrder = ic.OrderIndex;
                                        newIndex.IndexColumns.Add(newIndexColumn);
                                    }
                                }
                            }

                        }
                    }

                    //Create the special IsIndexed settings
                    //This will not create a new index since it was created above
                    foreach (var index in isIndexedList)
                    {
                        var entity = model.Entities.FirstOrDefault(x => x.Name == index.TableName);
                        if ((entity != null) && addedChangedEntities.Select(z => z.Name.ToLower()).Contains(entity.Name.ToLower()))
                        {
                            var field = entity.Fields.FirstOrDefault(x => x.Name == index.FieldList.First().Name);
                            if (field != null)
                            {
                                field.IsIndexed = true;
                            }
                        }
                    }

                    #endregion

                    #region Add Stored Procedures

                    #region Merge Module
                    foreach (var item in database.StoredProcList.Where(x => x.ImportState == DataImport.ImportStateConstants.Merge))
                    {
                        //Add module if necessary
                        var modelItem = diagramStoredProcs.FirstOrDefault(x => x.Name.ToLower() == item.Name.ToLower());
                        if (module != null && !modelItem.Modules.Contains(module))
                        {
                            modelItem.Modules.Add(module);
                        }
                    }
                    #endregion

                    foreach (var storedProc in database.StoredProcList.Where(x => x.ImportState == DataImport.ImportStateConstants.Added || x.ImportState == DataImport.ImportStateConstants.Modified))
                    {
                        var newStoredProc = diagramStoredProcs.Where(x => x.Id == storedProc.ID).FirstOrDefault();
                        if (newStoredProc == null) newStoredProc = diagramStoredProcs.FirstOrDefault(x => x.Name.ToLower() == storedProc.Name.ToLower());
                        if (newStoredProc == null)
                        {
                            newStoredProc = new StoredProcedure(model.Partition) { Name = storedProc.Name };
                            model.StoredProcedures.Add(newStoredProc);

                            //Correct for invalid identifiers
                            //if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newStoredProc.Name) && !nHydrate.Dsl.ValidationHelper.IsReservedWord(newStoredProc.Name))
                            if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newStoredProc.Name))
                            {
                                newStoredProc.CodeFacade = nHydrate.Dsl.ValidationHelper.MakeCodeIdentifer(newStoredProc.Name, string.Empty);
                            }
                        }
                        newStoredProc.Name = storedProc.Name;
                        newStoredProc.DatabaseObjectName = newStoredProc.Name; //Ensures the "gen_" prefix is not prepended

                        //Add module if necessary
                        if (module != null && !newStoredProc.Modules.Contains(module))
                        {
                            newStoredProc.Modules.Add(module);
                        }

                        newStoredProc.Schema = storedProc.Schema;
                        newStoredProc.SQL = storedProc.SQL;

                        PopulateFields(model, storedProc, newStoredProc);
                        PopulateParameters(model, storedProc, newStoredProc);
                    }

                    //Remove the ones that need to be remove
                    model.StoredProcedures.Remove(x => database.StoredProcList.Where(z => z.ImportState == DataImport.ImportStateConstants.Deleted).Select(a => a.Name).ToList().Contains(x.Name));

                    #endregion

                    #region Add Views

                    #region Merge Module
                    foreach (var item in database.ViewList.Where(x => x.ImportState == DataImport.ImportStateConstants.Merge))
                    {
                        //Add module if necessary
                        var modelItem = diagramViews.FirstOrDefault(x => x.Name.ToLower() == item.Name.ToLower());
                        if (module != null && !modelItem.Modules.Contains(module))
                        {
                            modelItem.Modules.Add(module);
                        }
                    }
                    #endregion

                    foreach (var view in database.ViewList.Where(x => x.ImportState == DataImport.ImportStateConstants.Added || x.ImportState == DataImport.ImportStateConstants.Modified))
                    {
                        var newView = diagramViews.Where(x => x.Id == view.ID).FirstOrDefault();
                        if (newView == null) newView = diagramViews.FirstOrDefault(x => x.Name.ToLower() == view.Name.ToLower());
                        if (newView == null)
                        {
                            newView = new nHydrate.Dsl.View(model.Partition) { Name = view.Name };
                            model.Views.Add(newView);

                            //Correct for invalid identifiers
                            //if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newView.Name) && !nHydrate.Dsl.ValidationHelper.IsReservedWord(newView.Name))
                            if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newView.Name))
                            {
                                newView.CodeFacade = nHydrate.Dsl.ValidationHelper.MakeCodeIdentifer(newView.Name, string.Empty);
                            }
                        }
                        newView.Name = view.Name;

                        //Add module if necessary
                        if (module != null && !newView.Modules.Contains(module))
                        {
                            newView.Modules.Add(module);
                        }

                        newView.Schema = view.Schema;
                        newView.SQL = view.SQL;

                        PopulateFields(model, view, newView);

                    }

                    //Remove the ones that need to be remove
                    model.Views.Remove(x => database.ViewList.Where(z => z.ImportState == DataImport.ImportStateConstants.Deleted).Select(a => a.Name).ToList().Contains(x.Name));

                    #endregion

                    #region Add Functions

                    #region Merge Module
                    foreach (var item in database.FunctionList.Where(x => x.ImportState == DataImport.ImportStateConstants.Merge))
                    {
                        //Add module if necessary
                        var modelItem = diagramFunctions.FirstOrDefault(x => x.Name.ToLower() == item.Name.ToLower());
                        if (module != null && !modelItem.Modules.Contains(module))
                        {
                            modelItem.Modules.Add(module);
                        }
                    }
                    #endregion

                    foreach (var function in database.FunctionList.Where(x => x.ImportState == DataImport.ImportStateConstants.Added || x.ImportState == DataImport.ImportStateConstants.Modified))
                    {
                        var newFunction = diagramFunctions.Where(x => x.Id == function.ID).FirstOrDefault();
                        if (newFunction == null) newFunction = diagramFunctions.FirstOrDefault(x => x.Name.ToLower() == function.Name.ToLower());
                        if (newFunction == null)
                        {
                            newFunction = new Function(model.Partition) { Name = function.Name };
                            model.Functions.Add(newFunction);

                            //Correct for invalid identifiers
                            if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newFunction.Name))
                            {
                                newFunction.CodeFacade = nHydrate.Dsl.ValidationHelper.MakeCodeIdentifer(newFunction.Name, string.Empty);
                            }
                        }
                        newFunction.Name = function.Name;
                        newFunction.ReturnVariable = function.ReturnVariable;

                        newFunction.IsTable = function.IsTable;

                        //Add module if necessary
                        if (module != null && !newFunction.Modules.Contains(module))
                        {
                            newFunction.Modules.Add(module);
                        }

                        newFunction.Schema = function.Schema;
                        newFunction.SQL = function.SQL;

                        PopulateFields(model, function, newFunction);
                        PopulateParameters(model, function, newFunction);

                    }

                    //Remove the ones that need to be remove
                    model.Functions.Remove(x => database.FunctionList.Where(z => z.ImportState == DataImport.ImportStateConstants.Deleted).Select(a => a.Name).ToList().Contains(x.Name));
                    #endregion

                    //Reset Precedense if necessary
                    model.StoredProcedures.Where(x => x.PrecedenceOrder == 0).ToList().ForEach(x => x.PrecedenceOrder = ++model.MaxPrecedenceOrder);
                    model.Views.Where(x => x.PrecedenceOrder == 0).ToList().ForEach(x => x.PrecedenceOrder = ++model.MaxPrecedenceOrder);
                    model.Functions.Where(x => x.PrecedenceOrder == 0).ToList().ForEach(x => x.PrecedenceOrder = ++model.MaxPrecedenceOrder);

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                model.IsLoading = false;

                ProgressHelper.ProgressingComplete(pkey);
                ((nHydrate.Dsl.nHydrateDiagram)diagram).IsLoading = false;
                model.IsLoading = false;
            }

        }

        public static void PopulateParameters(nHydrate.Dsl.nHydrateModel model, DataImport.StoredProc storedProc, StoredProcedure newStoredProc)
        {
            //Parameters
            //newStoredProc.Parameters.Clear();
            foreach (var parameter in storedProc.ParameterList)
            {
                var newParameter = newStoredProc.Parameters.FirstOrDefault(x => x.Name.ToLower() == parameter.Name.ToLower());
                if (newParameter == null)
                {
                    newParameter = new nHydrate.Dsl.StoredProcedureParameter(model.Partition);
                    newParameter.Name = parameter.Name;
                    newParameter.SortOrder = parameter.SortOrder;

                    //Correct for invalid identifiers
                    //if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newParameter.Name) && !nHydrate.Dsl.ValidationHelper.IsReservedWord(newParameter.Name))
                    if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newParameter.Name))
                    {
                        newParameter.CodeFacade = nHydrate.Dsl.ValidationHelper.MakeCodeIdentifer(newParameter.Name, string.Empty);
                    }
                    newStoredProc.Parameters.Add(newParameter);
                }

                newParameter.Length = parameter.Length;
                newParameter.Nullable = parameter.Nullable;
                newParameter.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), parameter.DataType.ToString());
                newParameter.Default = parameter.DefaultValue;
                newParameter.Scale = parameter.Scale;
                newParameter.IsOutputParameter = parameter.IsOutputParameter;

            }

            //Remove the parameters that need to be remove
            newStoredProc.Parameters.Remove(x => !storedProc.ParameterList.Select(a => a.Name.ToLower()).ToList().Contains(x.Name.ToLower()));
        }

        public static void PopulateParameters(nHydrate.Dsl.nHydrateModel model, DataImport.Function function, Function newFunction)
        {
            //Parameters
            //newFunction.Parameters.Clear();
            foreach (var parameter in function.ParameterList)
            {
                var newParameter = newFunction.Parameters.FirstOrDefault(x => x.Name.ToLower() == parameter.Name.ToLower());
                if (newParameter == null)
                {
                    newParameter = new nHydrate.Dsl.FunctionParameter(model.Partition);
                    newParameter.Name = parameter.Name;
                    newParameter.SortOrder = parameter.SortOrder;

                    //Correct for invalid identifiers
                    if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newParameter.Name))
                    {
                        newParameter.CodeFacade = nHydrate.Dsl.ValidationHelper.MakeCodeIdentifer(newParameter.Name, string.Empty);
                    }
                    newFunction.Parameters.Add(newParameter);
                }

                newParameter.Length = parameter.Length;
                newParameter.Nullable = parameter.Nullable;
                newParameter.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), parameter.DataType.ToString());
                newParameter.Default = parameter.DefaultValue;
                newParameter.Scale = parameter.Scale;
            }

            //Remove the parameters that need to be remove
            newFunction.Parameters.Remove(x => !function.ParameterList.Select(a => a.Name.ToLower()).ToList().Contains(x.Name.ToLower()));
        }

        public static void PopulateFields(nHydrate.Dsl.nHydrateModel model, DataImport.Function function, Function newFunction)
        {
            //Fields
            //newFunction.Fields.Clear();
            foreach (var field in function.FieldList)
            {
                var newField = newFunction.Fields.FirstOrDefault(x => x.Name.ToLower() == field.Name.ToLower());
                if (newField == null)
                {
                    newField = new nHydrate.Dsl.FunctionField(model.Partition);
                    newField.Name = field.Name;

                    //Correct for invalid identifiers
                    if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newField.Name))
                    {
                        newField.CodeFacade = nHydrate.Dsl.ValidationHelper.MakeCodeIdentifer(newField.Name, string.Empty);
                    }
                    newFunction.Fields.Add(newField);
                }

                newField.Length = field.Length;
                newField.Nullable = field.Nullable;
                newField.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), field.DataType.ToString());
                newField.Default = field.DefaultValue;
                newField.Scale = field.Scale;
            }

            //Remove the fields that need to be remove
            newFunction.Fields.Remove(x => !function.FieldList.Select(a => a.Name.ToLower()).ToList().Contains(x.Name.ToLower()));
        }

        public static void PopulateFields(nHydrate.Dsl.nHydrateModel model, DataImport.View view, nHydrate.Dsl.View newView)
        {
            //newView.Fields.Clear();
            foreach (var field in view.FieldList)
            {
                var newField = newView.Fields.FirstOrDefault(x => x.Name.ToLower() == field.Name.ToLower());
                if (newField == null)
                {
                    newField = new nHydrate.Dsl.ViewField(model.Partition);
                    newField.Name = field.Name;
                    newView.Fields.Add(newField);

                    //Correct for invalid identifiers
                    if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newField.Name))
                    {
                        newField.CodeFacade = nHydrate.Dsl.ValidationHelper.MakeCodeIdentifer(newField.Name, string.Empty);
                    }
                }

                newField.Length = field.Length;
                newField.Nullable = field.Nullable;
                newField.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), field.DataType.ToString());
                newField.Default = field.DefaultValue;
                newField.Scale = field.Scale;
            }

            //Remove the fields that need to be remove
            newView.Fields.Remove(x => !view.FieldList.Select(a => a.Name.ToLower()).ToList().Contains(x.Name.ToLower()));
        }

        public static void PopulateFields(nHydrate.Dsl.nHydrateModel model, DataImport.StoredProc storedProc, StoredProcedure newStoredProc)
        {
            if (storedProc.ColumnFailure) return;

            //Fields
            //newStoredProc.Fields.Clear();
            foreach (var field in storedProc.FieldList)
            {
                var newField = newStoredProc.Fields.FirstOrDefault(x => x.Name.ToLower() == field.Name.ToLower());
                if (newField == null)
                {
                    newField = new nHydrate.Dsl.StoredProcedureField(model.Partition);
                    newField.Name = field.Name;
                    newStoredProc.Fields.Add(newField);

                    //Correct for invalid identifiers
                    if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newField.Name))
                    {
                        newField.CodeFacade = nHydrate.Dsl.ValidationHelper.MakeCodeIdentifer(newField.Name, string.Empty);
                    }
                }

                newField.Length = field.Length;
                newField.Nullable = field.Nullable;
                newField.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), field.DataType.ToString());
                newField.Default = field.DefaultValue;
                newField.Scale = field.Scale;
            }
            //Remove the fields that need to be remove
            newStoredProc.Fields.Remove(x => !storedProc.FieldList.Select(a => a.Name.ToLower()).ToList().Contains(x.Name.ToLower()));
        }

        public static void PopulateFields(nHydrate.Dsl.nHydrateModel model, Module module, DataImport.Entity importItem, Entity targetItem)
        {
            foreach (var field in importItem.FieldList)
            {
                var newField = targetItem.Fields.FirstOrDefault(x => x.Name.ToLower() == field.Name.ToLower());
                if (newField == null)
                    newField = new nHydrate.Dsl.Field(model.Partition);

                //Add module if necessary
                if (module != null && !newField.Modules.Contains(module))
                {
                    newField.Modules.Add(module);
                }

                if (!targetItem.Fields.Contains(newField))
                    targetItem.Fields.Add(newField);

                newField.SortOrder = field.SortOrder;
                newField.Name = field.Name;
                newField.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), field.DataType.ToString());
                newField.Length = field.Length;
                newField.Nullable = field.Nullable;
                newField.IsCalculated = field.IsComputed;
                newField.Default = field.DefaultValue;
                newField.IsUnique = field.IsUnique;
                newField.Formula = field.Formula;
                newField.Identity = (field.Identity ? IdentityTypeConstants.Database : IdentityTypeConstants.None);
                newField.IsPrimaryKey = field.PrimaryKey;
                newField.Scale = field.Scale;
                newField.ImportedDefaultName = field.ImportedDefaultName;
                //DO NOT IMPORT METADATA

                //Correct for invalid identifiers
                //if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newField.Name) && !nHydrate.Dsl.ValidationHelper.IsReservedWord(newField.Name))
                if (!nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(newField.Name))
                {
                    newField.CodeFacade = nHydrate.Dsl.ValidationHelper.MakeCodeIdentifer(newField.Name, string.Empty);
                }
            }
            var removedFields = targetItem.Fields.Remove(x => !importItem.FieldList.Select(y => y.Name.ToLower()).Contains(x.Name.ToLower()));
        }

        /// <summary>
        /// Convert a new DSL model into a data import model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="diagram"></param>
        /// <returns></returns>
        public static nHydrate.DataImport.Database Convert(nHydrateModel model, Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram)
        {
            var database = new nHydrate.DataImport.Database();
            database.Collate = model.Collate;

            #region Load the entities
            foreach (var entity in model.Entities.Where(x => x.IsGenerated))
            {
                var newEntity = new nHydrate.DataImport.Entity();
                newEntity.ID = entity.Id;
                newEntity.Name = entity.Name;
                newEntity.Schema = entity.Schema;
                newEntity.AllowCreateAudit = entity.AllowCreateAudit;
                newEntity.AllowModifyAudit = entity.AllowModifyAudit;
                newEntity.AllowTimestamp = entity.AllowTimestamp;
                newEntity.IsTenant = entity.IsTenant;
                database.EntityList.Add(newEntity);

                #region Load the fields
                foreach (var field in entity.Fields)
                {
                    var newField = new nHydrate.DataImport.Field();
                    newField.ID = field.Id;
                    newField.Nullable = field.Nullable;
                    newField.Collate = field.Collate;
                    newField.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.DataType.ToString());
                    newField.DefaultValue = field.Default;
                    newField.Identity = (field.Identity == IdentityTypeConstants.Database);
                    newField.IsIndexed = field.IsIndexed;
                    newField.IsReadOnly = field.IsReadOnly;
                    newField.Length = field.Length;
                    newField.Name = field.Name;
                    newField.PrimaryKey = field.IsPrimaryKey;
                    newField.Scale = field.Scale;
                    newField.IsBrowsable = field.IsBrowsable;
                    newEntity.FieldList.Add(newField);
                }
                #endregion
            }
            #endregion

            #region Load Relations
            foreach (var shape in diagram.NestedChildShapes)
            {
                if (shape is EntityAssociationConnector)
                {
                    var connector = shape as EntityAssociationConnector;
                    var parent = connector.FromShape.ModelElement as Entity;
                    var child = connector.ToShape.ModelElement as Entity;

                    var relation = connector.ModelElement as EntityHasEntities;

                    var parentTable = database.EntityList.FirstOrDefault(x => parent != null && x.Name == parent.Name);
                    var childTable = database.EntityList.FirstOrDefault(x => child != null && x.Name == child.Name);

                    //If we found both parent and child tables...
                    if (parentTable != null && childTable != null)
                    {
                        var newRelation = new nHydrate.DataImport.Relationship();
                        newRelation.ID = shape.Id;
                        newRelation.TargetEntity = childTable;
                        newRelation.RoleName = ((EntityHasEntities)connector.ModelElement).RoleName;
                        newRelation.SourceEntity = parentTable;
                        parentTable.RelationshipList.Add(newRelation);

                        //Create the column links
                        var fieldList = model.RelationFields.Where(x => relation != null && x.RelationID == relation.Id);
                        foreach (var columnSet in fieldList)
                        {
                            var field1 = parent.Fields.FirstOrDefault(x => x.Id == columnSet.SourceFieldId);
                            var field2 = child.Fields.FirstOrDefault(x => x.Id == columnSet.TargetFieldId);

                            var column1 = parentTable.FieldList.FirstOrDefault(x => field1 != null && x.Name == field1.Name);
                            var column2 = childTable.FieldList.FirstOrDefault(x => field2 != null && x.Name == field2.Name);

                            newRelation.RelationshipColumnList.Add(new nHydrate.DataImport.RelationshipDetail()
                            {
                                ParentField = column1,
                                ChildField = column2,
                            }
                            );
                        }

                    }

                }
            }
            #endregion

            #region Load Stored Procedures
            foreach (var storedProc in model.StoredProcedures.Where(x => x.IsGenerated))
            {
                var newStoredProc = new nHydrate.DataImport.StoredProc();
                newStoredProc.ID = storedProc.Id;
                newStoredProc.Name = storedProc.Name;
                newStoredProc.Schema = storedProc.Schema;
                newStoredProc.SQL = storedProc.SQL;
                database.StoredProcList.Add(newStoredProc);

                //Load the fields
                foreach (var field in storedProc.Fields)
                {
                    var newField = new nHydrate.DataImport.Field();
                    newField.ID = field.Id;
                    newField.Nullable = field.Nullable;
                    //newField.Collate = field.Collate;
                    newField.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.DataType.ToString());
                    newField.DefaultValue = field.Default;
                    //newField.Identity = (field.Identity == IdentityTypeConstants.Database);
                    //newField.IsIndexed = field.IsIndexed;
                    newField.Length = field.Length;
                    newField.Name = field.Name;
                    //newField.PrimaryKey = field.IsPrimaryKey;
                    newField.Scale = field.Scale;
                    newStoredProc.FieldList.Add(newField);
                }

                //Load the parameters
                foreach (var parameter in storedProc.Parameters)
                {
                    var newParameter = new nHydrate.DataImport.Parameter();
                    newParameter.ID = parameter.Id;
                    newParameter.Nullable = parameter.Nullable;
                    //newField.Collate = field.Collate;
                    newParameter.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), parameter.DataType.ToString());
                    newParameter.DefaultValue = parameter.Default;
                    //newField.Identity = (field.Identity == IdentityTypeConstants.Database);
                    //newField.IsIndexed = field.IsIndexed;
                    newParameter.Length = parameter.Length;
                    newParameter.Name = parameter.Name;
                    //newField.PrimaryKey = field.IsPrimaryKey;
                    newParameter.Scale = parameter.Scale;
                    newStoredProc.ParameterList.Add(newParameter);
                }

            }
            #endregion

            #region Load Views
            foreach (var view in model.Views.Where(x => x.IsGenerated))
            {
                var newView = new nHydrate.DataImport.View();
                newView.ID = view.Id;
                newView.Name = view.Name;
                newView.Schema = view.Schema;
                newView.SQL = view.SQL;
                database.ViewList.Add(newView);

                //Load the fields
                foreach (var field in view.Fields)
                {
                    var newField = new nHydrate.DataImport.Field();
                    newField.ID = field.Id;
                    newField.Nullable = field.Nullable;
                    //newField.Collate = field.Collate;
                    newField.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.DataType.ToString());
                    newField.DefaultValue = field.Default;
                    //newField.Identity = (field.Identity == IdentityTypeConstants.Database);
                    //newField.IsIndexed = field.IsIndexed;
                    newField.Length = field.Length;
                    newField.Name = field.Name;
                    //newField.PrimaryKey = field.IsPrimaryKey;
                    newField.Scale = field.Scale;
                    newView.FieldList.Add(newField);
                }

            }
            #endregion

            #region Load Functions
            foreach (var function in model.Functions.Where(x => x.IsGenerated))
            {
                var newFunction = new nHydrate.DataImport.Function();
                newFunction.ID = function.Id;
                newFunction.Name = function.Name;
                newFunction.Schema = function.Schema;
                newFunction.SQL = function.SQL;
                database.FunctionList.Add(newFunction);

                //Load the fields
                foreach (var field in function.Fields)
                {
                    var newField = new nHydrate.DataImport.Field();
                    newField.ID = field.Id;
                    newField.Nullable = field.Nullable;
                    //newField.Collate = field.Collate;
                    newField.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), field.DataType.ToString());
                    newField.DefaultValue = field.Default;
                    //newField.Identity = (field.Identity == IdentityTypeConstants.Database);
                    //newField.IsIndexed = field.IsIndexed;
                    newField.Length = field.Length;
                    newField.Name = field.Name;
                    //newField.PrimaryKey = field.IsPrimaryKey;
                    newField.Scale = field.Scale;
                    newFunction.FieldList.Add(newField);
                }

                //Load the parameters
                foreach (var parameter in function.Parameters)
                {
                    var newParameter = new nHydrate.DataImport.Parameter();
                    newParameter.ID = parameter.Id;
                    newParameter.Nullable = parameter.Nullable;
                    //newField.Collate = field.Collate;
                    newParameter.DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), parameter.DataType.ToString());
                    newParameter.DefaultValue = parameter.Default;
                    //newField.Identity = (field.Identity == IdentityTypeConstants.Database);
                    //newField.IsIndexed = field.IsIndexed;
                    newParameter.Length = parameter.Length;
                    newParameter.Name = parameter.Name;
                    //newField.PrimaryKey = field.IsPrimaryKey;
                    newParameter.Scale = parameter.Scale;
                    newFunction.ParameterList.Add(newParameter);
                }

            }
            #endregion

            return database;
        }

        public static void ImportLegacyModel(nHydrateModel model, Store store, nHydrateDiagram diagram, string legacyFileName)
        {
            diagram.IsLoading = true;
            model.IsLoading = true;
            try
            {
                var addedElements = new List<ModelElement>();
                using (var transaction = store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                {
                    //The loading assemblies have changed names, so we must manually intervene
                    var newFile = legacyFileName + ".converting";
                    nHydrate.Generator.nHydrateGeneratorProject project = null;
                    try
                    {
                        File.Copy(legacyFileName, newFile);
                        var fileText = File.ReadAllText(newFile);
                        fileText = fileText.Replace("Widgetsphere.Generator.", "nHydrate.Generator.");
                        fileText = fileText.Replace("WidgetsphereGeneratorProject", "nHydrateGeneratorProject");
                        File.WriteAllText(newFile, fileText);
                        System.Threading.Thread.Sleep(500);
                        project = nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper.OpenModel(newFile) as nHydrate.Generator.nHydrateGeneratorProject;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("An error occurred while importing the legacy modelRoot.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        if (File.Exists(newFile))
                            File.Delete(newFile);
                    }

                    try
                    {
                        var oldModel = project.Model as nHydrate.Generator.Models.ModelRoot;

                        model.TransformNames = oldModel.TransformNames;
                        model.CompanyName = oldModel.CompanyName;
                        model.ProjectName = oldModel.ProjectName;
                        model.Copyright = oldModel.Copyright;
                        model.CreatedByColumnName = oldModel.Database.CreatedByColumnName;
                        model.CreatedDateColumnName = oldModel.Database.CreatedDateColumnName;
                        model.DefaultNamespace = oldModel.DefaultNamespace;
                        model.ModifiedByColumnName = oldModel.Database.ModifiedByColumnName;
                        model.ModifiedDateColumnName = oldModel.Database.ModifiedDateColumnName;
                        model.SQLServerType = (DatabaseTypeConstants)Enum.Parse(typeof(DatabaseTypeConstants), oldModel.SQLServerType.ToString());
                        model.StoredProcedurePrefix = oldModel.StoredProcedurePrefix;
                        model.TenantPrefix = oldModel.TenantPrefix;
                        model.AllowMocks = oldModel.AllowMocks;
                        model.TimestampColumnName = oldModel.Database.TimestampColumnName;
                        model.UseUTCTime = oldModel.UseUTCTime;
                        model.Version = oldModel.Version;

                        #region Load Tables
                        foreach (var table in oldModel.Database.Tables.ToList())
                        {
                            var newEntity = new Entity(model.Partition);
                            model.Entities.Add(newEntity);
                            addedElements.Add(newEntity);
                            newEntity.AllowAuditTracking = table.AllowAuditTracking;
                            newEntity.AllowCreateAudit = table.AllowCreateAudit;
                            newEntity.AllowModifyAudit = table.AllowModifiedAudit;
                            newEntity.AllowTimestamp = table.AllowTimestamp;
                            newEntity.CodeFacade = table.CodeFacade;
                            newEntity.Summary = table.Description;
                            newEntity.Immutable = table.Immutable;
                            newEntity.IsAssociative = table.AssociativeTable;
                            newEntity.IsGenerated = table.Generated;
                            newEntity.TypedEntity = (TypedEntityConstants)Enum.Parse(typeof(TypedEntityConstants), table.TypedTable.ToString(), true);
                            newEntity.Name = table.Name;
                            newEntity.Schema = table.DBSchema;
                            //DO NOT IMPORT METADATA

                            #region Load the columns
                            var maxSortOrder = 1;
                            foreach (var column in table.GetColumns())
                            {
                                var newField = new Field(model.Partition);
                                newEntity.Fields.Add(newField);
                                newField.CodeFacade = column.CodeFacade;
                                newField.Collate = column.Collate;
                                newField.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), column.DataType.ToString());
                                newField.Default = column.Default;
                                newField.Summary = column.Description;
                                newField.Formula = column.Formula;
                                newField.FriendlyName = column.FriendlyName;
                                newField.Identity = (IdentityTypeConstants)Enum.Parse(typeof(IdentityTypeConstants), column.Identity.ToString());
                                newField.IsCalculated = column.ComputedColumn;
                                newField.IsGenerated = column.Generated;
                                newField.IsPrimaryKey = column.PrimaryKey;
                                newField.SortOrder = maxSortOrder++;

                                //Do not reset as it creates 2 indexes with PK
                                if (!newField.IsPrimaryKey)
                                    newField.IsIndexed = column.IsIndexed;

                                newField.IsReadOnly = column.IsReadOnly;
                                newField.IsUnique = column.IsUnique;
                                newField.Length = column.Length;
                                newField.Max = column.Max;
                                newField.Min = column.Min;
                                newField.Name = column.Name;
                                newField.Nullable = column.AllowNull;
                                newField.Scale = column.Scale;
                                newField.IsBrowsable = column.IsBrowsable;
                                newField.ValidationExpression = column.ValidationExpression;
                            }
                            #endregion

                            #region Composites
                            foreach (var component in table.ComponentList.ToList())
                            {
                                var newComposite = new Composite(model.Partition);
                                newEntity.Composites.Add(newComposite);
                                addedElements.Add(newComposite);
                                newComposite.CodeFacade = component.CodeFacade;
                                newComposite.Summary = component.Description;
                                newComposite.Name = component.Name;
                                newComposite.IsGenerated = component.Generated;

                                //Add the fields by looking them up in the new table
                                foreach (var column in component.GetColumns())
                                {
                                    var checkColumn = table.GetColumns().FirstOrDefault(x => x.Key == column.Key);
                                    if (checkColumn != null)
                                    {
                                        var tableField = newEntity.Fields.FirstOrDefault(x => x.Name == checkColumn.Name);
                                        if (tableField != null)
                                        {
                                            var newField = new CompositeField(model.Partition);
                                            newComposite.Fields.Add(newField);
                                            newField.FieldId = tableField.Id;
                                        }
                                    }
                                }

                            }
                            #endregion

                            #region Load Static Data
                            var index = 1;
                            foreach (nHydrate.Generator.Models.RowEntry data in table.StaticData)
                            {
                                var isProcessed = false;
                                foreach (nHydrate.Generator.Models.CellEntry cellEntry in data.CellEntries)
                                {
                                    var column = cellEntry.ColumnRef.Object as nHydrate.Generator.Models.Column;
                                    if (column != null)
                                    {
                                        var newColumn = newEntity.Fields.FirstOrDefault(x => x.Name == column.Name);
                                        if (newColumn != null)
                                        {
                                            var newData = new StaticData(model.Partition);
                                            newEntity.StaticDatum.Add(newData);
                                            newData.ColumnKey = newColumn.Id;
                                            newData.Value = cellEntry.Value;
                                            newData.OrderKey = index;
                                            isProcessed = true;
                                        }
                                    }
                                }
                                if (isProcessed)
                                    index++;
                            }
                            #endregion

                        }

                        foreach (var table in oldModel.Database.Tables.ToList())
                        {
                            if (table.ParentTable != null)
                            {
                                var child = model.Entities.FirstOrDefault(x => x.Name == table.Name);
                                var parent = model.Entities.FirstOrDefault(x => x.Name == table.ParentTable.Name);
                                child.ParentInheritedEntity = parent;
                            }
                        }

                        #endregion

                        #region Load Relations
                        foreach (nHydrate.Generator.Models.Table table in oldModel.Database.Tables)
                        {
                            foreach (nHydrate.Generator.Models.Relation relation in table.GetRelations())
                            {
                                var entity1 = model.Entities.First(x => x.Name == relation.ParentTable.Name);
                                var entity2 = model.Entities.First(x => x.Name == relation.ChildTable.Name);
                                if (entity2.ParentInheritedEntity != entity1)
                                {
                                    entity1.ChildEntities.Add(entity2);
                                    var newRelation = entity1.RelationshipList.First(x =>
                                        x.SourceEntity == entity1 &&
                                        x.TargetEntity == entity2 &&
                                        !x.FieldMapList().Any());
                                    newRelation.RoleName = relation.RoleName;

                                    foreach (nHydrate.Generator.Models.ColumnRelationship relationColumn in relation.ColumnRelationships)
                                    {
                                        var column1 = entity1.Fields.First(x => x.Name == relationColumn.ParentColumn.Name);
                                        var column2 = entity2.Fields.First(x => x.Name == relationColumn.ChildColumn.Name);
                                        var newRelationField = new RelationField(model.Partition);
                                        model.RelationFields.Add(newRelationField);
                                        newRelationField.SourceFieldId = column1.Id;
                                        newRelationField.TargetFieldId = column2.Id;
                                        newRelationField.RelationID = newRelation.Id;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Load Views
                        foreach (nHydrate.Generator.Models.CustomView item in oldModel.Database.CustomViews)
                        {
                            var newitem = new nHydrate.Dsl.View(model.Partition);
                            model.Views.Add(newitem);
                            addedElements.Add(newitem);
                            newitem.CodeFacade = item.CodeFacade;
                            newitem.Summary = item.Description;
                            newitem.IsGenerated = item.Generated;
                            newitem.Name = item.Name;
                            newitem.Schema = item.DBSchema;
                            newitem.SQL = item.SQL;

                            foreach (var column in item.GetColumns())
                            {
                                var newColumn = new ViewField(model.Partition);
                                newitem.Fields.Add(newColumn);
                                newColumn.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), column.DataType.ToString());
                                newColumn.Default = column.Default;
                                newColumn.Summary = column.Description;
                                newColumn.FriendlyName = column.FriendlyName;
                                newColumn.Length = column.Length;
                                newColumn.IsGenerated = column.Generated;
                                //newColumn.Max = column.ma;
                                //newColumn.Min = column.;
                                newColumn.Name = column.Name;
                                newColumn.Nullable = column.AllowNull;
                                newColumn.Scale = column.Scale;
                            }
                        }
                        #endregion

                        #region Load Stored Procedures
                        foreach (nHydrate.Generator.Models.CustomStoredProcedure item in oldModel.Database.CustomStoredProcedures)
                        {
                            var newitem = new StoredProcedure(model.Partition);
                            model.StoredProcedures.Add(newitem);
                            addedElements.Add(newitem);
                            newitem.CodeFacade = item.CodeFacade;
                            newitem.Summary = item.Description;
                            newitem.IsGenerated = item.Generated;
                            newitem.IsExisting = item.IsExisting;
                            newitem.Name = item.Name;
                            newitem.Schema = item.DBSchema;
                            newitem.SQL = item.SQL;
                            newitem.DatabaseObjectName = item.DatabaseObjectName;

                            foreach (var parameter in item.GetParameters())
                            {
                                var newParameter = new StoredProcedureParameter(model.Partition);
                                newitem.Parameters.Add(newParameter);
                                newParameter.Nullable = parameter.AllowNull;
                                newParameter.DataType = (nHydrate.Dsl.DataTypeConstants)Enum.Parse(typeof(nHydrate.Dsl.DataTypeConstants), parameter.DataType.ToString());
                                newParameter.Default = parameter.Default;
                                newParameter.Summary = parameter.Description;
                                newParameter.IsGenerated = parameter.Generated;
                                newParameter.IsOutputParameter = parameter.IsOutputParameter;
                                newParameter.Length = parameter.Length;
                                newParameter.Scale = parameter.Scale;
                                newParameter.Name = parameter.Name;
                            }

                            foreach (var column in item.GetColumns())
                            {
                                var newColumn = new StoredProcedureField(model.Partition);
                                newitem.Fields.Add(newColumn);
                                newColumn.DataType = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), column.DataType.ToString());
                                newColumn.Default = column.Default;
                                newColumn.Summary = column.Description;
                                newColumn.FriendlyName = column.FriendlyName;
                                newColumn.Length = column.Length;
                                //newColumn.Max = column.ma;
                                //newColumn.Min = column.;
                                newColumn.Name = column.Name;
                                newColumn.Nullable = column.AllowNull;
                                newColumn.Scale = column.Scale;
                            }

                        }
                        #endregion

                        nHydrateSerializationHelper.LoadInitialIndexes(model);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                }

                using (var transaction = store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                {
                    diagram.AutoLayoutShapeElements(
                        diagram.NestedChildShapes.Where(x => addedElements.Contains(x.ModelElement)).ToList(),
                        Microsoft.VisualStudio.Modeling.Diagrams.GraphObject.VGRoutingStyle.VGRouteTreeNS,
                        Microsoft.VisualStudio.Modeling.Diagrams.GraphObject.PlacementValueStyle.VGPlaceNS,
                        true);
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                diagram.IsLoading = false;
                model.IsLoading = false;
            }

        }
    }
}
