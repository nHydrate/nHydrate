namespace nHydrate.DataImport
{
    public class Field : DatabaseBaseObject
    {
        private int _length = 50;

        public Field()
        {
            this.DataType = System.Data.SqlDbType.VarChar;
            this.IsBrowsable = true;
            this.DefaultValue = string.Empty;
        }

        public System.Data.SqlDbType DataType { get; set; }
        public bool Identity { get; set; }
        public string DefaultValue { get; set; }
        public bool PrimaryKey { get; set; }
        public bool Nullable { get; set; }
        public int Scale { get; set; }
        public string ImportedDefaultName { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsBrowsable { get; set; }

        private bool _isUnique;
        public bool IsUnique
        {
            get { return _isUnique || this.PrimaryKey; }
            set { _isUnique = value; }
        }

        private bool _isIndexed;
        public bool IsIndexed
        {
            get { return _isIndexed || this.PrimaryKey; }
            set { _isIndexed = value; }
        }

        public int SortOrder { get; set; }
        public bool IsComputed { get; set; }
        public string Formula { get; set; }

        public int Length
        {
            get { return _length; }
            set
            {
                if (value < 0) value = 0;
                _length = value;
            }
        }

        public override string ObjectType => "Field";

        public override string ToString()
        {
            return this.Name;
        }

        public string CorePropertiesHash
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.Identity + "|" +
                    this.Nullable + "|" +
                    this.DefaultValue + "|" +
                    this.Length + "|" +
                    this.IsComputed + "|" +
                    this.Scale + "|" +
                    this.PrimaryKey + "|" +
                    this.IsBrowsable + "|" +
                    this.DataType.ToString();
                return prehash;
            }
        }

        public override bool Equals(object obj)
        {
            var o = obj as Field;
            if (o == null)
                return false;

            if (this.Name != o.Name) return false;
            if (this.IsIndexed != o.IsIndexed) return false;
            if (this.IsUnique != o.IsUnique) return false;
            if (this.DefaultValue != o.DefaultValue) return false;
            if (this.IsComputed != o.IsComputed) return false;
            if (this.Identity != o.Identity) return false;
            if (this.PrimaryKey != o.PrimaryKey) return false;
            if (this.Nullable != o.Nullable) return false;
            if (this.Length != o.Length) return false;
            if (this.Scale != o.Scale) return false;
            if (this.DataType != o.DataType) return false;

            return true;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
