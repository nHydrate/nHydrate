#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling.Validation;

namespace nHydrate.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    partial class Function
    {
        #region Dirty
        [System.ComponentModel.Browsable(false)]
        public bool IsDirty
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
            //if (!this.IsDirty) return;
            System.Windows.Forms.Application.DoEvents();

            var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
            try
            {
                var columnList = this.Fields.ToList();

                #region Check valid name
                if (!ValidationHelper.ValidDatabaseIdenitifer(this.DatabaseName))
                    context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.Name), string.Empty, this);
                else if (!ValidationHelper.ValidCodeIdentifier(this.PascalName))
                    context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.Name), string.Empty, this);
                else if (!ValidationHelper.ValidFieldIdentifier(this.PascalName))
                    context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.Name), string.Empty, this);

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

                #region Check valid name based on codefacade
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !ValidationHelper.ValidDatabaseIdenitifer(this.CodeFacade))
                    context.LogError(ValidationHelper.ErrorTextInvalidCodeFacade, string.Empty, this);
                #endregion

                #region Verify there is a result

                if (this.Fields.Count() == 0)
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextFunctionZeroFields, this.Name), string.Empty, this);
                }
                if (this.Fields.Count() != 1 && !this.IsTable)
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextFunctionScalerMultipleFields, this.Name), string.Empty, this);
                }

                #endregion

                #region Verify the return variable

                if (!string.IsNullOrEmpty(this.ReturnVariable))
                {
                    if (!ValidationHelper.ValidDatabaseIdenitifer(this.ReturnVariable))
                        context.LogError(string.Format(ValidationHelper.ErrorTextFunctionReturnVarNotValid, this.ReturnVariable, this.Name), string.Empty, this);

                    if (!this.IsTable)
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextFunctionReturnVarForTabelFunc, this.Name), string.Empty, this);
                    }
                }

                #endregion

                #region Check View SQL
                if (string.IsNullOrEmpty(this.SQL))
                    context.LogError(string.Format(ValidationHelper.ErrorTextSQLRequiredFunction, this.Name), string.Empty, this);
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
                nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Function Validate - Functions");
            }

        }
    }
}