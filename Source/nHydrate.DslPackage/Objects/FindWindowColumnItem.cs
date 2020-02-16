namespace nHydrate.DslPackage.Objects
{
	public enum FindWindowColumnTypeConstants
	{
		DataType,
		Length,
		Nullable,
		PrimaryKey,
		Identity,
		Default,
		CodeFacade,
		TypedEntity,
		Schema,
		IsAssociative,
		Immutable,
	}

	public class FindWindowColumnItem
	{
		public string Name { get; set; }
		public bool Visible { get; set; }
		public FindWindowColumnTypeConstants Type { get; set; }
		public System.Windows.Forms.ColumnHeader ColumnHeader { get; set; }

		public override string ToString()
		{
			return this.Name;
		}
	}
}

