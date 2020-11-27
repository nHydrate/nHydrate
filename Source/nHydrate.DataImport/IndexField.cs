namespace nHydrate.DataImport
{
    public class IndexField : DatabaseBaseObject
    {
        public bool IsDescending { get; set; }
        public int OrderIndex { get; set; }
        public override string ObjectType => "Field";
    }
}
