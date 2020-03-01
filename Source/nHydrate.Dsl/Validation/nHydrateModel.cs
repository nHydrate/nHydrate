#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using System.Collections;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    partial class nHydrateModel
    {
        #region Dirty
        [System.ComponentModel.Browsable(false)]
        public bool IsDirty
        {
            get
            {
                return _isDirty ||
                    this.Entities.IsDirty() ||
                    this.Views.IsDirty();
            }
            set
            {
                _isDirty = value;
                this.Entities.ResetDirty(value);
                this.Views.ResetDirty(value);
            }
        }
        private bool _isDirty = false;

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.IsDirty = true;
            base.OnPropertyChanged(e);
        }
        #endregion

        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
        public void Validate(ValidationContext context)
        {
            var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();

            try
            {
                #region Validate some global settings
                if (!ValidationHelper.ValidDatabaseIdentifier(this.CompanyName) || !ValidationHelper.ValidCodeIdentifier(this.CompanyName))
                    context.LogError(ValidationHelper.ErrorTextInvalidCompany, string.Empty, this);
                if (!ValidationHelper.ValidDatabaseIdentifier(this.ProjectName) || !ValidationHelper.ValidCodeIdentifier(this.ProjectName))
                    context.LogError(ValidationHelper.ErrorTextInvalidProject, string.Empty, this);

                if (!string.IsNullOrEmpty(this.DefaultNamespace))
                {
                    if (!ValidationHelper.IsValidNamespace(this.DefaultNamespace))
                        context.LogError(ValidationHelper.ErrorTextInvalidNamespace, string.Empty, this);
                }
                #endregion

                #region Validate audit fields
                var auditFieldList = new List<string>();
                auditFieldList.Add(this.CreatedByColumnName);
                if (!auditFieldList.Contains(this.CreatedDateColumnName))
                    auditFieldList.Add(this.CreatedDateColumnName);
                if (!auditFieldList.Contains(this.ModifiedByColumnName))
                    auditFieldList.Add(this.ModifiedByColumnName);
                if (!auditFieldList.Contains(this.ModifiedDateColumnName))
                    auditFieldList.Add(this.ModifiedDateColumnName);
                if (!auditFieldList.Contains(this.TimestampColumnName))
                    auditFieldList.Add(this.TimestampColumnName);

                if (auditFieldList.Count != 5)
                {
                    context.LogError(ValidationHelper.ErrorTextAuditFieldsNotUnique, string.Empty, this);
                }
                else
                {
                    auditFieldList = new List<string>();
                    auditFieldList.Add(this.CreatedByPascalName);
                    if (!auditFieldList.Contains(this.CreatedDatePascalName))
                        auditFieldList.Add(this.CreatedDatePascalName);
                    if (!auditFieldList.Contains(this.ModifiedByPascalName))
                        auditFieldList.Add(this.ModifiedByPascalName);
                    if (!auditFieldList.Contains(this.ModifiedDatePascalName))
                        auditFieldList.Add(this.ModifiedDatePascalName);
                    if (!auditFieldList.Contains(this.TimestampPascalName))
                        auditFieldList.Add(this.TimestampPascalName);

                    if (auditFieldList.Count != 5)
                        context.LogError(ValidationHelper.ErrorTextAuditFieldsNotUnique, string.Empty, this);
                }
                #endregion

                #region Check for Global Uniqueness
                var nameList = new HashSet<string>();

                //Check all entities
                foreach (var entity in this.Entities)
                {
                    {
                        var check = entity.PascalName.ToLower();
                        if (nameList.Contains(check))
                            context.LogError(string.Format(ValidationHelper.ErrorTextDuplicateName, entity.PascalName), string.Empty, entity);
                        else
                            nameList.Add(check);
                    }

                }

                //Check Views
                foreach (var view in this.Views)
                {
                    var check = view.PascalName.ToLower();
                    if (nameList.Contains(check))
                        context.LogError(string.Format(ValidationHelper.ErrorTextDuplicateName, view.PascalName), string.Empty, view);
                    else
                        nameList.Add(check);
                }

                #endregion

                #region Validate OutputTarget

                if (!string.IsNullOrEmpty(this.OutputTarget))
                {
                    try
                    {
                        var fi = new System.IO.FileInfo(System.IO.Path.Combine(@"c:\", this.OutputTarget));
                    }
                    catch (Exception)
                    {
                        context.LogError(ValidationHelper.ErrorTextOutputTargetInvalid, string.Empty, this);
                    }
                }

                #endregion

                #region Tenant
                if (this.Entities.Any(x => x.IsTenant))
                {
                    if (!ValidationHelper.ValidCodeIdentifier(this.TenantColumnName))
                        context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.TenantColumnName), string.Empty, this);
                    if (!ValidationHelper.ValidCodeIdentifier(this.TenantPrefix))
                        context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.TenantPrefix), string.Empty, this);
                }
                #endregion

                #region Version

                if (Convert.ToInt32(this.Version.Split('.').FirstOrDefault()) < 0)
                {
                    context.LogError(ValidationHelper.ErrorTextVersionNegative, string.Empty, this);
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Model Validate - Main");
            }

        }

        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
        public void ValidateEntities(ValidationContext context)
        {
            var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
            try
            {
                #region Check for zero tables
                if (this.Entities.Count == 0)
                {
                    context.LogError(ValidationHelper.ErrorTextNoTables, string.Empty, this);
                    return;
                }
                #endregion

                #region Verify that the name is valid
                foreach (var item in this.Entities)
                {
                    if (!ValidationHelper.ValidCodeIdentifier(item.PascalName) || !ValidationHelper.ValidEntityName(item.PascalName))
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, item.Name), string.Empty, this);
                    }

                    foreach (var field in item.Fields)
                    {
                        if (!ValidationHelper.ValidCodeIdentifier(field.PascalName))
                        {
                            context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, field.Name), string.Empty, this);
                        }
                    }

                }
                #endregion

                #region Check for duplicate names
                var nameList = new Hashtable();
                foreach (var table in this.Entities)
                {
                    var name = table.Name.ToLower();
                    if (nameList.ContainsKey(name))
                        context.LogError(string.Format(ValidationHelper.ErrorTextDuplicateName, table.Name), string.Empty, this);
                    else
                        nameList.Add(name, string.Empty);
                }
                #endregion

                #region Check for duplication relationship names

                var duplicateList = new Dictionary<string, RelationshipChecker>();
                var relationList = this.Store.ElementDirectory.AllElements.Where(x => x is EntityHasEntities).ToList().Cast<EntityHasEntities>();
                foreach (var relation in relationList)
                {
                    var childTable = relation.TargetEntity;
                    var entity = relation.SourceEntity;
                    var relationFields = this.RelationFields.Where(x => x.RelationID == relation.Id).ToList();

                    if (childTable != null && entity != null)
                    {
                        var key = string.Empty;
                        if (entity.Name.Match(childTable.Name))
                        {
                            if (string.Compare(entity.Name, childTable.Name, false) < 0)
                                key = childTable.Name + "|" + relation.RoleName + "|" + entity.Name;
                            else
                                key = entity.Name + "|" + relation.RoleName + "|" + childTable.Name;
                        }
                        else
                        {
                            if (string.Compare(entity.Name, childTable.Name, false) < 0)
                                key = entity.Name + "|" + relation.RoleName + "|" + childTable.Name;
                            else
                                key = childTable.Name + "|" + relation.RoleName + "|" + entity.Name;
                        }

                        if (duplicateList.ContainsKey(key))
                        {
                            if (entity.Name.Match(childTable.Name))
                                duplicateList[key].TableList.Add(entity);
                            else duplicateList[key].TableList.Add(childTable);
                        }
                        else
                        {
                            var rc = new RelationshipChecker(relation);
                            if (string.Compare(entity.Name, childTable.Name, true) < 0)
                                rc.TableList.Add(childTable);
                            else rc.TableList.Add(entity);
                            duplicateList.Add(key, rc);
                        }

                        //Verify that a FK has an index on it
                        foreach (var field in relationFields)
                        {
                            var targetField = field.GetTargetField(relation);
                            if (targetField != null)
                            {
                                if (!childTable.Indexes.SelectMany(x => x.FieldList.Select(z => z.Id)).Any(x => x == targetField.Id))
                                    context.LogWarning(string.Format(ValidationHelper.ErrorTextFKNeedIndex, childTable.Name + "." + targetField.Name), string.Empty, this);
                            }
                        }
                    }

                }

                foreach (var key in duplicateList.Keys)
                {
                    if (duplicateList[key].TableList.Count > 1)
                    {
                        var t1 = duplicateList[key].Relationship.SourceEntity.Name;
                        var t2 = duplicateList[key].Relationship.TargetEntity.Name;
                        context.LogError(string.Format(ValidationHelper.ErrorTextConflictingRelationships, "'" + t1 + "' and '" + t2 + "'"), string.Empty, this);
                    }
                }

                #endregion

                #region Check for duplicate codefacades
                if (context.CurrentViolations.Count == 0)
                {
                    nameList = new Hashtable();
                    foreach (var table in this.Entities)
                    {
                        var name = table.PascalName.ToLower();
                        if (nameList.ContainsKey(name))
                            context.LogError(string.Format(ValidationHelper.ErrorTextDuplicateCodeFacade, table.Name), string.Empty, this);
                        else
                            nameList.Add(name, string.Empty);
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Model Validate - Entities");
            }

        }

        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
        public void ValidateView(ValidationContext context)
        {
            var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
            try
            {
                #region Verify that the name is valid
                foreach (var item in this.Views)
                {
                    if (!ValidationHelper.ValidCodeIdentifier(item.PascalName))
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierView, item.Name), string.Empty, this);
                    }

                    foreach (var field in item.Fields)
                    {
                        if (!ValidationHelper.ValidCodeIdentifier(field.PascalName))
                        {
                            context.LogError(string.Format(ValidationHelper.ErrorTextInvalidFieldIdentifierView, item.Name, field.Name), string.Empty, this);
                        }
                    }

                }
                #endregion

                #region Check for duplicate names
                var nameList = new Hashtable();
                foreach (var customView in this.Views)
                {
                    var name = customView.Name.ToLower();
                    if (nameList.ContainsKey(name))
                        context.LogError(string.Format(ValidationHelper.ErrorTextDuplicateName, name), string.Empty, this);
                    else
                        nameList.Add(name, string.Empty);
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Model Validate - Views");
            }

        }

        #region Helper Class

        private class RelationshipChecker
        {
            public RelationshipChecker(EntityHasEntities relationship)
            {
                this.Relationship = relationship;
                this.TableList = new List<Entity>();
            }

            public List<Entity> TableList { get; }
            public EntityHasEntities Relationship { get; }
        }

        #endregion

    }
}