using Microsoft.VisualStudio.Modeling.Validation;

namespace nHydrate.Dsl
{
	[ValidationState(ValidationState.Enabled)]
	partial class Index
	{
		#region Dirty
		[System.ComponentModel.Browsable(false)]
		public bool IsDirty
		{
			get { return _isDirty || this.IndexColumns.IsDirty(); }
			set
			{
				_isDirty = value;
				if (!value)
				{
					this.IndexColumns.ForEach(x => x.IsDirty = false);
				}
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
		}

	}
}