#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling.Validation;

namespace nHydrate.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    partial class FunctionField
    {
        #region Dirty
        [System.ComponentModel.Browsable(false)]
        public bool IsDirty { get; set; } = false;

        #endregion

        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
        public void Validate(ValidationContext context)
        {
            var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
            try
            {
                if (!this.IsGenerated) return;
                //if (!this.IsDirty) return;

                #region Check valid name
                if (!ValidationHelper.ValidDatabaseIdenitifer(this.DatabaseName))
                    context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierFuncField, this.Name, this.Function.Name), string.Empty, this);
                else if (!ValidationHelper.ValidCodeIdentifier(this.PascalName))
                    context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierFuncField, this.Name, this.Function.Name), string.Empty, this);
                #endregion

                #region Validate max lengths

                var validatedLength = this.DataType.ValidateDataTypeMax(this.Length);
                if (validatedLength != this.Length)
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextColumnMaxLengthViolation, this.Function.Name + "." + this.Name, validatedLength, this.DataType.ToString()), string.Empty, this);
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Stored Procedure Parameter Validate - Main");
            }

        }
    }
}