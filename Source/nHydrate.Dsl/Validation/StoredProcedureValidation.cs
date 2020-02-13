#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling.Validation;
using System.Collections;

namespace nHydrate.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    partial class StoredProcedure
    {
        #region Dirty
        [System.ComponentModel.Browsable(false)]
        internal bool IsDirty
        {
            get { return _isDirty || this.Fields.IsDirty() || this.Parameters.IsDirty(); }
            set
            {
                _isDirty = value;
                if (!value)
                {
                    this.Fields.ForEach(x => x.IsDirty = false);
                    this.Parameters.ForEach(x => x.IsDirty = false);
                }
            }
        }
        private bool _isDirty = false;
        #endregion

        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
        public void Validate(ValidationContext context)
        {
            if (!this.IsGenerated) return;

            System.Windows.Forms.Application.DoEvents();
            var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
            try
            {
                //if (!this.IsDirty) return;
                var columnList = this.Fields.Where(x => x.IsGenerated).ToList();

                #region Check valid name

                ////Check valid name
                //if (!ValidationHelper.ValidDatabaseIdenitifer(this.DatabaseName))
                //  context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.Name), string.Empty, this);
                //else if (!ValidationHelper.ValidCodeIdentifier(this.PascalName))
                //  context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.Name), string.Empty, this);

                #endregion

                #region Check for duplicate names

                var nameList = new Hashtable();
                foreach (var column in columnList)
                {
                    var name = column.Name.ToLower();
                    if (nameList.ContainsKey(name))
                        context.LogError(string.Format(ValidationHelper.ErrorTextDuplicateName, column.Name), string.Empty, this);
                    else
                        nameList.Add(name, string.Empty);
                }

                #endregion

                #region Check StoredProcedure SQL

                if (!this.IsExisting && string.IsNullOrEmpty(this.SQL))
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextSQLRequiredStoredProcedure, this.Name), string.Empty, this);
                }

                #endregion

                #region Check existing database name

                if (this.IsExisting && string.IsNullOrEmpty(this.DatabaseObjectName))
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextExistingSPNeedsDBName, this.Name), string.Empty, this);
                }

                #endregion

                #region Verify that no column has same name as container

                foreach (var field in this.Fields)
                {
                    if (string.Compare(field.PascalName, this.PascalName, true) == 0)
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextTableColumnNameMatch, field.Name, this.Name), string.Empty, this);
                    }
                }

                #endregion

                #region Verify there are columns (fix for EF 4.1 bug)

                if (!this.Fields.Any(x => x.IsGenerated))
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextStoredProcNoColumns, this.Name), string.Empty, this);
                }

                #endregion

                #region Check Parameters Duplicates

                var paramList = this.Parameters.Select(x => x.Name.ToLower());
                if (paramList.Count() != paramList.Distinct().Count())
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextDuplicateParameters, this.Name), string.Empty, this);
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Stored Procedure Validate - Main");
            }

        }
    }
}