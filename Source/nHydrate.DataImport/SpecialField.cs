namespace nHydrate.DataImport
{
	public enum SpecialFieldTypeConstants
	{
		CreatedBy,
		CreatedDate,
		ModifiedBy,
		ModifiedDate,
		Timestamp,
		Tenant,
	}

	public class SpecialField
	{
		public string Name { get; set; }
		public SpecialFieldTypeConstants Type { get; set; }
	}

}
