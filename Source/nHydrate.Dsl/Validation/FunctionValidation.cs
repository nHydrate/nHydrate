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
            //if (!this.IsDirty) return;
            System.Windows.Forms.Application.DoEvents();

            var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
            try
            {
                var columnList = this.Fields.Where(x => x.IsGenerated).ToList();

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

                if (this.Fields.Count(x => x.IsGenerated) == 0)
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextFunctionZeroFields, this.Name), string.Empty, this);
                }
                if (this.Fields.Count(x => x.IsGenerated) != 1 && !this.IsTable)
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