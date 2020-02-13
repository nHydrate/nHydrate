namespace nHydrate.Generator.Common.EventArgs
{
	public class ProgressEventArgs : System.EventArgs
	{
		public ProgressEventArgs()
		{
			this.PercentProgress = 0;
		}

		public ProgressEventArgs(int percentProgress)
			: this()
		{
			this.PercentProgress = percentProgress;
		}

		public virtual int PercentProgress { get; }

	}
}

