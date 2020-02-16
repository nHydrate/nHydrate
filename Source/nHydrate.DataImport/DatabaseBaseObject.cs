using System;

namespace nHydrate.DataImport
{
	public enum ImportStateConstants
	{
		Unchanged = 0,
		Added = 1,
		Modified = 2,
		Deleted = 3,
	}

	public abstract class DatabaseBaseObject
	{
		public DatabaseBaseObject()
		{
			this.ImportState = ImportStateConstants.Unchanged;
		}

		public string Name { get; set; }

		/// <summary>
		/// Used when comparing models but not importing them
		/// </summary>
		public Guid ID { get; set; }

		/// <summary>
		/// The state of this object after import
		/// </summary>
		public ImportStateConstants ImportState { get; set; }

		public abstract string ObjectType { get; }

	}
}

