namespace nHydrate.Generator.Common.EventArgs
{
	public class PropertyChangedEventArgs
	{
		#region Contructors

		public PropertyChangedEventArgs(string propertyName, string newValue)
		{
			this.PropertyName = propertyName;
			this.NewValue = newValue;
		}

		#endregion

		#region Property Implementations

		public string PropertyName { get; set; }

		public string NewValue { get; set; }

		#endregion
	}
}

