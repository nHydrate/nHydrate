using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using System.Collections;

namespace nHydrate.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    partial class View
    {
        #region Dirty
        [System.ComponentModel.Browsable(false)]
        public bool IsDirty
        {
            get { return _isDirty || this.Fields.IsDirty(); }
            set
            {
                _isDirty = value;
                if (!value)
                {
                    this.Fields.ForEach(x => x.IsDirty = false);
                }
            }
        }
        private bool _isDirty = false;
        #endregion

        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
        public void Validate(ValidationContext context)
        {
            //if (!this.IsDirty) return;
            var columnList = this.Fields.ToList();

            #region Check valid name
            if (!ValidationHelper.ValidDatabaseIdentifier(this.DatabaseName))
                context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifier, this.Name), string.Empty, this);
            else if (!ValidationHelper.ValidCodeIdentifier(this.PascalName))
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

            #region Check View SQL
            if (this.SQL == string.Empty)
                context.LogError(string.Format(ValidationHelper.ErrorTextSQLRequiredView, this.Name), string.Empty, this);
            #endregion

            #region Check that object has at least one generated column
            if (this.Fields.Count() == 0)
                context.LogError(ValidationHelper.ErrorTextColumnsRequired, string.Empty, this);
            #endregion

            #region Verify that there is at least one PK
            if (this.Fields.Count(x => x.IsPrimaryKey) == 0)
                context.LogError(string.Format(ValidationHelper.ErrorTextNoPrimaryKey, this.Name), string.Empty, this);
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

        }
    }
}
