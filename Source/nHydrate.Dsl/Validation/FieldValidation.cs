#pragma warning disable 0168
using System;
using Microsoft.VisualStudio.Modeling.Validation;

namespace nHydrate.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    partial class Field
    {
        #region Dirty
        [System.ComponentModel.Browsable(false)]
        public bool IsDirty { get; set; } = false;

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
                //if (!this.IsDirty) return;

                #region Check valid name
                if (!ValidationHelper.ValidDatabaseIdentifier(this.DatabaseName))
                    context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.Entity.Name + "." + this.Name), string.Empty, this);
                else if (!ValidationHelper.ValidCodeIdentifier(this.PascalName))
                    context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.Entity.Name + "." + this.Name), string.Empty, this);
                else if (!ValidationHelper.ValidFieldIdentifier(this.PascalName))
                    context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.Entity.Name + "." + this.Name), string.Empty, this);

                #endregion

                #region Check valid name based on codefacade
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !ValidationHelper.ValidDatabaseIdentifier(this.CodeFacade))
                    context.LogError(ValidationHelper.ErrorTextInvalidCodeFacade, string.Empty, this);
                #endregion

                #region Validate identity field
                if (this.Identity != IdentityTypeConstants.None && !this.DataType.SupportsIdentity())
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentityColumn, this.Name), string.Empty, this);
                }
                #endregion

                #region Columns cannot be 0 length

                if (!this.DataType.SupportsMax() && this.Length == 0)
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextColumnLengthNotZero, this.Name), string.Empty, this);
                }

                #endregion

                #region Validate Decimals

                if (this.DataType == DataTypeConstants.Decimal)
                {
                    if (this.Length < 1 || this.Length > 38)
                        context.LogError(string.Format(ValidationHelper.ErrorTextColumnDecimalPrecision, this.Name), string.Empty, this);
                    if (this.Scale < 0 || this.Scale > this.Length)
                        context.LogError(string.Format(ValidationHelper.ErrorTextColumnDecimalScale, this.Name), string.Empty, this);
                }

                #endregion

                #region Validate max lengths

                var validatedLength = this.DataType.ValidateDataTypeMax(this.Length);
                if (validatedLength != this.Length)
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextColumnMaxLengthViolation, this.Entity.Name + "." + this.Name, validatedLength, this.DataType.ToString()), string.Empty, this);
                }

                #endregion

                #region Verify Datatypes for SQL 2005/2008

                if (!this.DataType.IsSupportedType())
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextDataTypeNotSupported, this.Name), string.Empty, this);
                }

                #endregion

                #region Computed Column

                if (this.IsCalculated)
                {
                    if (this.Formula.Trim() == "")
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextComputeColumnNoFormula, this.Name), string.Empty, this);
                    }

                    if (this.IsPrimaryKey)
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextComputeColumnNoPK, this.Name), string.Empty, this);
                    }

                }

                if (!this.IsCalculated && !string.IsNullOrEmpty(this.Formula))
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextComputeNonColumnHaveFormula, this.Name), string.Empty, this);
                }

                #endregion

                #region Validate Defaults

                if (!string.IsNullOrEmpty(this.Default))
                {
                    if (!this.CanHaveDefault())
                    {
                        context.LogError(string.Format(ValidationHelper.ErrorTextColumnCannotHaveDefault, this.Name), string.Empty, this);
                    }
                    else if (!this.IsValidDefault())
                    {
                        context.LogWarning(string.Format(ValidationHelper.ErrorTextColumnInvalidDefault, this.Name), string.Empty, this);
                    }
                }

                #endregion

                #region Check Decimals for common error

                if (this.DataType == DataTypeConstants.Decimal)
                {
                    if (this.Length == 1)
                        context.LogError(string.Format(ValidationHelper.ErrorTextDecimalColumnTooSmall, this.Name, this.Length.ToString()), string.Empty, this);
                }

                #endregion

                #region Identity Columns cannot have defaults

                if (this.Identity != IdentityTypeConstants.None && !string.IsNullOrEmpty(this.Default))
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextColumnIdentityHasDefault, this.Entity.Name + "." + this.Name), string.Empty, this);
                }

                #endregion

                #region Non-nullable, ReadOnly propeties must have a default (except identities)
                if (this.IsReadOnly && !this.Nullable && (this.Identity != IdentityTypeConstants.Database) && string.IsNullOrEmpty(this.Default))
                {
                    context.LogError(string.Format(ValidationHelper.ErrorTextColumnReadonlyNeedsDefault, this.Entity.Name + "." + this.Name), string.Empty, this);
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Field Validate - Fields");
            }
        }

    }
}