namespace nHydrate.Dsl
{
    public interface IField
    {
        string Name { get; set; }
        int Length { get; set; }
        DataTypeConstants DataType { get; set; }
        bool Nullable { get; set; }
    }
}
