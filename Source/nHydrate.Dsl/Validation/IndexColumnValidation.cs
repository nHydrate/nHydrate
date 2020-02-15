using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling.Validation;

namespace nHydrate.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    partial class IndexColumn
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
        }

    }
}

