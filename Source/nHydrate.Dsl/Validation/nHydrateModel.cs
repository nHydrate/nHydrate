#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
                    this.Views.IsDirty() ||
                    this.StoredProcedures.IsDirty() ||
                    this.Functions.IsDirty();
            }
            set
            {
                _isDirty = value;
                this.Entities.ResetDirty(value);
                this.Views.ResetDirty(value);
                this.StoredProcedures.ResetDirty(value);
                this.Functions.ResetDirty(value);
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
                if (!ValidationHelper.ValidDatabaseIdenitifer(this.CompanyName) || !ValidationHelper.ValidCodeIdentifier(this.CompanyName))
                    context.LogError(ValidationHelper.ErrorTextInvalidCompany, string.Empty, this);
                if (!ValidationHelper.ValidDatabaseIdenitifer(this.ProjectName) || !ValidationHelper.ValidCodeIdentifier(this.ProjectName))
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
                foreach (var entity in this.Entities.Where(x => x.IsGenerated))
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
                foreach (var view in this.Views.Where(x => x.IsGenerated))
                {
                    var check = view.PascalName.ToLower();
                    if (nameList.Contains(check))
                        context.LogError(string.Format(ValidationHelper.ErrorTextDuplicateName, view.PascalName), string.Empty, view);
                    else
                        nameList.Add(check);
                }

                //Check Stored Procedures
                foreach (var sp in this.StoredProcedures.Where(x => x.IsGenerated))
                {
                    var check = sp.PascalName.ToLower();
                    if (nameList.Contains(check))
                        context.LogError(string.Format(ValidationHelper.ErrorTextDuplicateName, sp.PascalName), string.Empty, sp);
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

                if (string.IsNullOrEmpty(this.StoredProcedurePrefix))
                {
                    context.LogError(ValidationHelper.ErrorTextInvalidStoredProcPrefix, string.Empty, this);
                }

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
        public void ValidateModules(ValidationContext context)
        {
            var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
            try
            {
                //If we are not using modules then nothing to do
                if (!this.UseModules) return;

                #region Verify there are modules
                if (this.Modules.Count == 0)
                {
                    context.LogError(ValidationHelper.ErrorTextNoModules, string.Empty, this);
                    return;
                }
                #endregion

                #region Verify unique modules
                if (this.Modules.Select(x => x.Name.ToLower()).Distinct().Count() != this.Modules.Count)
                {
                    context.LogError(ValidationHelper.ErrorTextModulesNotUnique, string.Empty, this);
                    return;
                }
                #endregion

                #region Verify that the name is valid
                foreach (var item in this.Modules)
                {
                    if (!ValidationHelper.ValidCodeIdentifier(item.Name))
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierModule, item.Name), string.Empty, this);
                    }
                }
                #endregion

                #region If a field is in a module then its entity must be as well
                foreach (var entity in this.Entities.Where(x => x.IsGenerated))
                {
                    foreach (var field in entity.Fields.Where(x => x.IsGenerated))
                    {
                        var moduleList = field.Modules.Intersect(entity.Modules);
                        if (moduleList.Count() != field.Modules.Count)
                        {
                            context.LogError(string.Format(ValidationHelper.ErrorTextModuleEntityFieldMismatch, field.Name, moduleList.First().Name), string.Empty, this);
                        }
                    }
                }
                #endregion

                #region Warn that some entities are in NO modules
                foreach (var item in this.Entities.Where(x => x.IsGenerated))
                {
                    if (item.Modules.Count == 0)
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextModuleItemNotInModule, item.Name), string.Empty, this);
                    }
                }
                #endregion

                #region Warn that some SP are in NO modules
                foreach (var item in this.StoredProcedures.Where(x => x.IsGenerated))
                {
                    if (item.Modules.Count == 0)
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextModuleItemNotInModule, item.Name), string.Empty, this);
                    }
                }
                #endregion

                #region Warn that some views are in NO modules
                foreach (var item in this.Views.Where(x => x.IsGenerated))
                {
                    if (item.Modules.Count == 0)
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextModuleItemNotInModule, item.Name), string.Empty, this);
                    }
                }
                #endregion

                #region Warn that some functions are in NO modules
                foreach (var item in this.Functions.Where(x => x.IsGenerated))
                {
                    if (item.Modules.Count == 0)
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextModuleItemNotInModule, item.Name), string.Empty, this);
                    }
                }
                #endregion

                #region Verify that if an entity is in a module, at least one PKs is in the module too

                foreach (var item in this.Entities.Where(x => x.IsGenerated))
                {
                    if (item.PrimaryKeyFields.Count > 0)
                    {
                        foreach (var module in item.Modules)
                        {
                            var count = item.PrimaryKeyFields.Count(x => x.Modules.Contains(module));
                            if (count == 0)
                                context.LogError(string.Format(ValidationHelper.ErrorTextModuleEntityPKMismatch, item.Name, module.Name), string.Empty, this);
                            else if (item.PrimaryKeyFields.Count != count)
                                context.LogWarning(string.Format(ValidationHelper.WarningTextModuleEntityPKMismatch, item.Name, module.Name), string.Empty, this);

                            //foreach (var field in item.PrimaryKeyFields)
                            //{
                            //  if (!field.Modules.Contains(module))
                            //  {
                            //    context.LogError(string.Format(ValidationHelper.ErrorTextModuleEntityPKMismatch, item.Name, module.Name), string.Empty, this);
                            //  }
                            //}
                        }
                    }
                }

                #endregion

                #region Validation Relations that are in NO module

                foreach (var moduleLink in this.AllRelations.Cast<IModuleLink>().Where(x => !x.Modules.Any()))
                {
                    context.LogWarning(ValidationHelper.WarningTextRelationNotInModule, string.Empty, (Microsoft.VisualStudio.Modeling.ModelElement)moduleLink);
                }

                #endregion

                #region Find relations with parent/child in a module, but the relation is not present

                foreach (var moduleLink in this.AllRelations.Cast<IModuleLink>().Where(x => x.Modules.Any()))
                {
                    var relation = (EntityHasEntities)moduleLink;
                    //Find common modules for parent and child table
                    var moduleList = relation.ParentEntity.Modules.Intersect(relation.ChildEntity.Modules).ToList();
                    foreach (var module in moduleList)
                    {
                        //Check if the relation is in the specified module
                        if (!moduleLink.Modules.Contains(module))
                        {
                            context.LogWarning(string.Format(ValidationHelper.WarningTextRelationNotInModuleWithParentChildTables, relation.ParentEntity.Name, relation.ChildEntity.Name, module.Name), string.Empty, relation);
                        }
                    }
                }

                #endregion

                #region Verify Module Rules are setup correctly

                foreach (var module in this.Modules)
                {
                    foreach (var rule in module.ModuleRules)
                    {
                        var dModule = this.Modules.FirstOrDefault(x => x.Id == rule.DependentModule);
                        if (dModule == null)
                        {
                            context.LogError(string.Format(ValidationHelper.WarningErrorModuleRuleInvalid, module.Name), string.Empty, rule);
                        }
                    }
                }

                #endregion

                #region Module Rule apply logic

                foreach (var module in this.Modules)
                {
                    foreach (var rule in module.ModuleRules.Where(x => x.Enforced))
                    {
                        if (rule.GetDependentModuleObject() == null)
                        {
                            context.LogError(string.Format(ValidationHelper.WarningErrorModuleRuleNoDependentModule, module.Name, rule.Status.ToString().ToLower()), string.Empty, rule);
                        }
                        else
                        {
                            var issueList = new List<string>();
                            if (!module.IsValidRule(rule, ref issueList))
                            {
                                var dModule = this.Modules.FirstOrDefault(x => x.Id == rule.DependentModule);
                                if (dModule != null)
                                {
                                    context.LogError(string.Format(ValidationHelper.WarningErrorModuleRuleLogicFail, module.Name, rule.Status.ToString().ToLower(), dModule.Name)
                                        + string.Join(". ", issueList.ToArray()),
                                        string.Empty, rule);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Verify there is at least one entity in a module

                foreach (var module in this.Modules)
                {
                    if (module.Entities.Count(x => x.IsGenerated) == 0)
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextModuleIsEmpty, module.Name), string.Empty, module);
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
                nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Model Validate - Modules");
            }

        }

        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
        public void ValidateEntities(ValidationContext context)
        {
            var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
            try
            {
                #region Check for zero tables
                if (this.Entities.Count(x => x.IsGenerated) == 0)
                {
                    context.LogError(ValidationHelper.ErrorTextNoTables, string.Empty, this);
                    return;
                }
                #endregion

                #region Verify that the name is valid
                foreach (var item in this.Entities.Where(x => x.IsGenerated))
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
                foreach (var table in this.Entities.Where(x => x.IsGenerated))
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

                    if (childTable != null && entity != null && childTable.IsGenerated && entity.IsGenerated)
                    {
                        var key = string.Empty;
                        if (StringHelper.Match(entity.Name, childTable.Name, true))
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
                            if (StringHelper.Match(entity.Name, childTable.Name, true))
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
                    foreach (var table in this.Entities.Where(x => x.IsGenerated))
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
                foreach (var item in this.Views.Where(x => x.IsGenerated))
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
                foreach (var customView in this.Views.Where(x => x.IsGenerated))
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

        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
        public void ValidateStoredProcedures(ValidationContext context)
        {
            var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
            try
            {
                #region Verify that the name is valid
                foreach (var item in this.StoredProcedures.Where(x => x.IsGenerated))
                {
                    if (!ValidationHelper.ValidCodeIdentifier(item.PascalName))
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierSP, item.Name), string.Empty, this);
                    }

                    foreach (var field in item.Fields)
                    {
                        if (!ValidationHelper.ValidCodeIdentifier(field.PascalName))
                        {
                            context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierSPField, field.Name, item.Name), string.Empty, this);
                        }
                    }

                    foreach (var parameter in item.Parameters)
                    {
                        if (!ValidationHelper.ValidCodeIdentifier(parameter.PascalName))
                        {
                            context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierSPParam, parameter.Name, item.Name), string.Empty, this);
                        }
                    }
                }
                #endregion

                #region Check for duplicate names
                var nameList = new Hashtable();
                foreach (var customStoredProcedure in this.StoredProcedures.Where(x => x.IsGenerated))
                {
                    var name = customStoredProcedure.Name.ToLower();
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
                nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Model Validate - Stored Procedures");
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

            public List<Entity> TableList { get; set; }
            public EntityHasEntities Relationship { get; set; }
        }

        #endregion

    }
}