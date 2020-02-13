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
		internal bool IsDirty
		{
			get { return _isDirty; }
			set { _isDirty = value; }
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

