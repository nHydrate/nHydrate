using Microsoft.VisualStudio.Modeling.Validation;

namespace nHydrate.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    partial class ViewField
    {
        #region Dirty
        [System.ComponentModel.Browsable(false)]
        public bool IsDirty { get; set; } = false;

        #endregion

        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
        public void Validate(ValidationContext context)
        {
            //if (!this.IsDirty) return;
            #region Check valid name
            if (!ValidationHelper.ValidDatabaseIdentifier(this.DatabaseName))
                context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierViewField, this.Name, this.View.Name), string.Empty, this);
            else if (!ValidationHelper.ValidCodeIdentifier(this.PascalName))
                context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierViewField, this.Name, this.View.Name), string.Empty, this);
            #endregion

            #region Validate max lengths

            var validatedLength = this.DataType.ValidateDataTypeMax(this.Length);
            if (validatedLength != this.Length)
            {
                context.LogError(string.Format(ValidationHelper.ErrorTextColumnMaxLengthViolation, this.View.Name + "." + this.Name, validatedLength, this.DataType.ToString()), string.Empty, this);
            }

            #endregion
        }
    }
}