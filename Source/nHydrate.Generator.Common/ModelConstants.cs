namespace nHydrate.Generator.Common
{
    public enum DeleteActionConstants
    {
        NoAction,
        Cascade,
        SetNull
    }

    public enum IdentityTypeConstants
    {
        None,
        Database,
        Code,
    }
    
    //Copy from Dsl
    public enum IndexTypeConstants
    {
        PrimaryKey,
        IsIndexed,
        User,
    }

    //Copy from Dsl
    public enum DataTypeConstants
    {
        BigInt,
        Binary,
        Bit,
        Char,
        Date,
        DateTime,
        DateTime2,
        DateTimeOffset,
        Decimal,
        Float,
        Image,
        Int,
        Money,
        NChar,
        NText,
        NVarChar,
        Real,
        SmallDateTime,
        SmallInt,
        SmallMoney,
        Structured,
        Text,
        Time,
        Timestamp,
        TinyInt,
        Udt,
        UniqueIdentifier,
        VarBinary,
        VarChar,
        Variant,
        Xml,
    }

    public enum TypedTableConstants
    {
        None,
        DatabaseTable,
        EnumOnly,
    }

}
