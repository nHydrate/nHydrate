namespace nHydrate.Generator.Common.EventArgs
{
	public class BooleanEventArgs
	{
		#region Contructors

		private BooleanEventArgs()
		{
			this.Value = false;
		}

		public BooleanEventArgs(bool value)
			: this()
		{
			this.Value = value;
		}

		#endregion

		#region Property Implementations

		public bool Value { get; }

		#endregion
	}
}
