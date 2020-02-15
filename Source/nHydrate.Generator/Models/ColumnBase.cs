using System;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    /// <summary>
    /// This is the base for all column classes
    /// </summary>
    public abstract class ColumnBase : BaseModelObject
    {
        #region Member Variables

        protected const System.Data.SqlDbType _def_type = System.Data.SqlDbType.VarChar;
        protected const int _def_length = 50;
        protected const int _def_scale = 0;
        protected const bool _def_allowNull = true;
        protected const bool _def_generated = true;
        protected const string _def_description = "";
        protected const string _def_prompt = "";
        protected const bool _def_isBrowsable = true;

        protected string _description = _def_description;
        protected string _prompt = _def_prompt;
        protected System.Data.SqlDbType _dataType = _def_type;
        protected int _length = _def_length;
        protected int _scale = _def_scale;
        protected bool _generated = _def_generated;
        protected bool _allowNull = _def_allowNull;
        protected bool _isBrowsable = _def_isBrowsable;

        #endregion

        #region Constructor

        protected ColumnBase(INHydrateModelObject root)
            : base(root)
        {
        }

        #endregion

        #region Property Implementations

        public string Category { get; set; } = string.Empty;

        public virtual bool Generated
        {
            get { return _generated; }
            set
            {
                _generated = value;
            }
        }

        public virtual string Description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }

        public virtual string Prompt
        {
            get { return _prompt; }
            set
            {
                _prompt = value;
            }
        }

        public virtual int Length
        {
            get
            {
                var retval = this.PredefinedSize;
                if (retval == -1) retval = _length;
                return retval;
            }
            set
            {
                if (value < 0) value = 0;
                _length = value;
            }
        }

        public virtual int Scale
        {
            get
            {
                var retval = this.PredefinedScale;
                if (retval == -1) retval = _scale;
                return retval;
            }
            set
            {
                if (value < 0) value = 0;
                _scale = value;
            }
        }

        public virtual System.Data.SqlDbType DataType
        {
            get { return _dataType; }
            set
            {
                _length = this.GetDefaultSize(value);
                _dataType = value;
            }
        }

        public virtual bool AllowNull
        {
            get { return this.DataType != System.Data.SqlDbType.Variant && _allowNull; }
            set
            {
                _allowNull = value;
            }
        }

        public virtual bool IsBrowsable
        {
            get { return _isBrowsable; }
            set
            {
                _isBrowsable = value;
            }
        }

        public virtual string DatabaseType
        {
            get { return this.GetSQLDefaultType(); }
        }

        public virtual string DatabaseTypeRaw
        {
            get { return this.GetSQLDefaultType(true); }
        }

        /// <summary>
        /// Determines if this field type can be made into a range query
        /// </summary>
        public virtual bool IsRangeType
        {
            get
            {
                switch (this.DataType)
                {
                    case System.Data.SqlDbType.BigInt:
                    //case System.Data.SqlDbType.Char:
                    case System.Data.SqlDbType.Date:
                    case System.Data.SqlDbType.DateTime:
                    case System.Data.SqlDbType.DateTime2:
                    case System.Data.SqlDbType.Decimal:
                    case System.Data.SqlDbType.Float:
                    case System.Data.SqlDbType.Int:
                    case System.Data.SqlDbType.Money:
                    //case System.Data.SqlDbType.NChar:
                    //case System.Data.SqlDbType.NVarChar:
                    case System.Data.SqlDbType.Real:
                    case System.Data.SqlDbType.SmallDateTime:
                    case System.Data.SqlDbType.SmallInt:
                    case System.Data.SqlDbType.SmallMoney:
                    case System.Data.SqlDbType.Time:
                    case System.Data.SqlDbType.TinyInt:
                        //case System.Data.SqlDbType.VarChar:
                        return true;
                }
                return false;
            }
        }

        public virtual string CorePropertiesHash
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.AllowNull + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.DataType.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        public virtual string CorePropertiesHashNoPK
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.AllowNull + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.DataType.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines if this data type supports a user-defined size
        /// </summary>
        public virtual bool IsDefinedSize
        {
            get { return this.PredefinedSize != -1; }
        }

        /// <summary>
        /// Determines if this data type supports a user-defined scale
        /// </summary>
        public virtual bool IsDefinedScale
        {
            get { return this.PredefinedScale == 0; }
        }

        /// <summary>
        /// Determines if this field has no max length defined
        /// </summary>
        /// <returns></returns>
        public virtual bool IsMaxLength()
        {
            switch (this.DataType)
            {
                case System.Data.SqlDbType.VarChar:
                case System.Data.SqlDbType.NVarChar:
                case System.Data.SqlDbType.VarBinary:
                    return (this.Length == 0);
                case System.Data.SqlDbType.Text:
                case System.Data.SqlDbType.NText:
                    return true;
                default:
                    return false;
            }
        }

        public virtual string GetLengthString()
        {
            if (this.DataType.SupportsMax() && this.Length == 0)
                return "max";
            else
                return this.Length.ToString();
        }

        /// <summary>
        /// This is the length used for annotations and meta data for class descriptions
        /// </summary>
        public virtual int GetAnnotationStringLength()
        {
            switch (this.DataType)
            {
                case System.Data.SqlDbType.NText:
                    return 1073741823;
                case System.Data.SqlDbType.Text:
                    return int.MaxValue;
                case System.Data.SqlDbType.Image:
                    return int.MaxValue;
            }

            if (this.DataType.SupportsMax() && this.Length == 0)
                return int.MaxValue;
            else
                return this.Length;
        }

        public override string ToString()
        {
            var retval = this.Name;
            return retval;
        }

        /// <summary>
        /// Determine if the specified type is 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSupportedType(System.Data.SqlDbType type)
        {
            switch (type)
            {
                //case System.Data.SqlDbType.Xml:
                case System.Data.SqlDbType.Udt:
                case System.Data.SqlDbType.Structured:
                case System.Data.SqlDbType.Variant:
                    //case System.Data.SqlDbType.DateTimeOffset:
                    //case System.Data.SqlDbType.DateTime2:
                    //case System.Data.SqlDbType.Time:
                    //case System.Data.SqlDbType.Date:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Create a valid T-SQL variable from the name
        /// </summary>
        /// <returns></returns>
        public virtual string ToDatabaseCodeIdentifier()
        {
            return ValidationHelper.MakeDatabaseScriptIdentifier(this.DatabaseName);
        }

        /// <summary>
        /// Determines if this column can be used in a LIKE database operation
        /// </summary>
        /// <returns></returns>
        public bool SupportsLikeOperator()
        {
            switch (this.DataType)
            {
                case System.Data.SqlDbType.NText:
                case System.Data.SqlDbType.Text:
                case System.Data.SqlDbType.Image:
                case System.Data.SqlDbType.Xml:
                    return false;
                default:
                    return true;
            }
        }

        #endregion

        #region IXMLable Members

        public abstract override void XmlAppend(XmlNode node);
        public abstract override void XmlLoad(XmlNode node);

        #endregion

        #region Helpers

        public abstract Reference CreateRef();
        public abstract Reference CreateRef(string key);

        public virtual string CamelName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToCamelCase(this.PascalName);
                else
                    return StringHelper.FirstCharToLower(this.PascalName);
            }
        }

        public abstract string PascalName { get; }

        public virtual string DatabaseName
        {
            get { return this.Name; }
        }

        /// <summary>
        /// Gets the SQL Server type mapping for this data type
        /// </summary>
        public virtual string GetSQLDefaultType()
        {
            return GetSQLDefaultType(false);
        }

        /// <summary>
        /// Gets the SQL Server type mapping for this data type
        /// </summary>
        /// <param name="isRaw">Determines if the square brackets '[]' are around the type</param>
        /// <returns>The SQL ready datatype like '[Int]' or '[Varchar] (100)'</returns>
        public virtual string GetSQLDefaultType(bool isRaw)
        {
            var retval = string.Empty;

            if (!isRaw) retval += "[";
            retval += this.DataType.ToString();
            if (!isRaw) retval += "]";

            if (this.DataType == System.Data.SqlDbType.Variant)
            {
                retval = string.Empty;
                if (!isRaw) retval += "[";
                retval += "sql_variant";
                if (!isRaw) retval += "]";
            }
            else if (this.DataType == System.Data.SqlDbType.Binary ||
                     this.DataType == System.Data.SqlDbType.Char ||
                     this.DataType == System.Data.SqlDbType.Decimal ||
                     this.DataType == System.Data.SqlDbType.DateTime2 ||
                     this.DataType == System.Data.SqlDbType.NChar ||
                     this.DataType == System.Data.SqlDbType.NVarChar ||
                     this.DataType == System.Data.SqlDbType.VarBinary ||
                     this.DataType == System.Data.SqlDbType.VarChar)
            {
                if (this.DataType == System.Data.SqlDbType.Decimal)
                    retval += " (" + this.Length + ", " + this.Scale + ")";
                else if (this.DataType == System.Data.SqlDbType.DateTime2)
                    retval += " (" + this.Length + ")";
                else
                    retval += " (" + this.GetLengthString() + ")";
            }
            return retval;
        }

        /// <summary>
        /// Gets a string reprsenting the data length for comments
        /// </summary>
        /// <remarks>This is not to be used for actual C# code or SQL</remarks>
        public virtual string GetCommentLengthString()
        {
            if (this.DataType == System.Data.SqlDbType.Binary ||
                this.DataType == System.Data.SqlDbType.Char ||
                this.DataType == System.Data.SqlDbType.Decimal ||
                this.DataType == System.Data.SqlDbType.DateTime2 ||
                this.DataType == System.Data.SqlDbType.NChar ||
                this.DataType == System.Data.SqlDbType.NVarChar ||
                this.DataType == System.Data.SqlDbType.VarBinary ||
                this.DataType == System.Data.SqlDbType.VarChar)
            {
                if (this.DataType == System.Data.SqlDbType.Decimal)
                    return this.Length + $" (scale:{this.Scale})";
                else if (this.DataType == System.Data.SqlDbType.DateTime2)
                    return this.Length.ToString();
                else
                    return this.GetLengthString();
            }
            return string.Empty;
        }

        public virtual string GetCodeType()
        {
            return GetCodeType(true, false);
        }

        public virtual string GetCodeType(bool allowNullable)
        {
            return GetCodeType(allowNullable, false);
        }

        public virtual string GetCodeType(bool allowNullable, bool forceNull)
        {
            var retval = string.Empty;
            if (StringHelper.Match(this.DataType.ToString(), "bigint", true))
            {
                retval = "long";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "binary", true))
            {
                return "System.Byte[]";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "bit", true))
            {
                retval = "bool";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "char", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "datetime", true))
            {
                retval = "DateTime";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "datetime2", true))
            {
                retval = "DateTime";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "date", true))
            {
                retval = "DateTime";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "time", true))
            {
                retval = "TimeSpan";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "datetimeoffset", true))
            {
                retval = "DateTimeOffset";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "decimal", true))
            {
                retval = "decimal";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "float", true))
            {
                retval = "double";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "image", true))
            {
                return "System.Byte[]";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "int", true))
            {
                retval = "int";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "money", true))
            {
                retval = "decimal";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "nchar", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "ntext", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "numeric", true))
            {
                retval = "decimal";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "nvarchar", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "real", true))
            {
                retval = "System.Single";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "smalldatetime", true))
            {
                retval = "DateTime";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "smallint", true))
            {
                retval = "short";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "smallmoney", true))
            {
                retval = "decimal";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "variant", true))
            {
                retval = "object";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "text", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "tinyint", true))
            {
                retval = "byte";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "uniqueidentifier", true))
            {
                retval = "System.Guid";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "varbinary", true))
            {
                return "System.Byte[]";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "varchar", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "timestamp", true))
            {
                return "System.Byte[]";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "xml", true))
            {
                return "string";
            }
            else
            {
                throw new Exception("Cannot Map Sql Value '" + this.DataType.ToString() + "' To C# Value");
            }

            if (allowNullable && (this.AllowNull || forceNull))
                retval += "?";

            return retval;

        }

        /// <summary>
        /// Gets the method of a datareader that corresponds to this specified datatype
        /// </summary>
        public virtual string GetDataReaderMethodName()
        {
            switch (this.DataType)
            {
                case System.Data.SqlDbType.BigInt:
                    return "GetInt64";
                case System.Data.SqlDbType.Binary:
                    return "GetBytes";
                case System.Data.SqlDbType.Bit:
                    return "GetBoolean";
                case System.Data.SqlDbType.Char:
                    return "GetString";
                case System.Data.SqlDbType.DateTime:
                    return "GetDateTime";
                case System.Data.SqlDbType.Decimal:
                    return "GetDecimal";
                case System.Data.SqlDbType.Float:
                    return "GetDouble";
                case System.Data.SqlDbType.Image:
                    return "GetBytes";
                case System.Data.SqlDbType.Int:
                    return "GetInt32";
                case System.Data.SqlDbType.Money:
                    return "GetDecimal";
                case System.Data.SqlDbType.NChar:
                    return "GetString";
                case System.Data.SqlDbType.NText:
                    return "GetString";
                case System.Data.SqlDbType.NVarChar:
                    return "GetString";
                case System.Data.SqlDbType.Real:
                    return "GetFloat";
                case System.Data.SqlDbType.UniqueIdentifier:
                    return "GetGuid";
                case System.Data.SqlDbType.SmallDateTime:
                    return "GetDateTime";
                case System.Data.SqlDbType.SmallInt:
                    return "GetInt16";
                case System.Data.SqlDbType.SmallMoney:
                    return "GetDecimal";
                case System.Data.SqlDbType.Text:
                    return "GetString";
                case System.Data.SqlDbType.Timestamp:
                    return "GetBytes";
                case System.Data.SqlDbType.TinyInt:
                    return "GetByte";
                case System.Data.SqlDbType.VarBinary:
                    return "GetBytes";
                case System.Data.SqlDbType.VarChar:
                    return "GetString";
                case System.Data.SqlDbType.Variant:
                    return "GetObject";
                case System.Data.SqlDbType.Xml:
                    return "GetString";
                case System.Data.SqlDbType.Udt:
                    return "GetObject";
                case System.Data.SqlDbType.Structured:
                    return "GetData";
                case System.Data.SqlDbType.Date:
                    return "GetDateTime";
                case System.Data.SqlDbType.Time:
                    return "GetDateTime";
                case System.Data.SqlDbType.DateTime2:
                    return "GetDateTime";
                case System.Data.SqlDbType.DateTimeOffset:
                    return "GetDateTime";
                default:
                    throw new Exception("Cannot Map Sql Value To data reader method name");
            }

        }

        /// <summary>
        /// Determines if the Datatype supports the 'Parse' method
        /// </summary>
        public virtual bool AllowStringParse
        {
            get
            {
                if (StringHelper.Match(this.DataType.ToString(), "bigint", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "binary", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "bit", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "char", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "datetime", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "datetime2", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "datetimeoffset", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "date", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "time", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "decimal", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "float", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "image", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "int", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "money", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "nchar", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "ntext", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "numeric", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "nvarchar", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "real", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "smalldatetime", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "smallint", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "smallmoney", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "variant", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "text", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "tinyint", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "uniqueidentifier", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "varbinary", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "varchar", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "timestamp", true))
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public static int GetPredefinedSize(System.Data.SqlDbType dataType)
        {
            //Returns -1 if variable
            switch (dataType)
            {
                case System.Data.SqlDbType.BigInt:
                    return 8;
                case System.Data.SqlDbType.Bit:
                    return 1;
                case System.Data.SqlDbType.DateTime:
                    return 8;
                case System.Data.SqlDbType.Date:
                    return 3;
                case System.Data.SqlDbType.Time:
                    return 5;
                case System.Data.SqlDbType.DateTimeOffset:
                    return 10;
                case System.Data.SqlDbType.Float:
                    return 8;
                case System.Data.SqlDbType.Int:
                    return 4;
                case System.Data.SqlDbType.Money:
                    return 8;
                case System.Data.SqlDbType.Real:
                    return 4;
                case System.Data.SqlDbType.SmallDateTime:
                    return 4;
                case System.Data.SqlDbType.SmallInt:
                    return 2;
                case System.Data.SqlDbType.SmallMoney:
                    return 4;
                case System.Data.SqlDbType.Timestamp:
                    return 8;
                case System.Data.SqlDbType.TinyInt:
                    return 1;
                case System.Data.SqlDbType.UniqueIdentifier:
                    return 16;

                case System.Data.SqlDbType.Image:
                case System.Data.SqlDbType.Text:
                case System.Data.SqlDbType.NText:
                case System.Data.SqlDbType.Xml:
                    return 1;

                default:
                    return -1;
            }
        }

        /// <summary>
        /// Gets the size of the data type
        /// </summary>
        /// <returns></returns>
        /// <remarks>Returns -1 for variable types and 1 for blob fields</remarks>
        public virtual int PredefinedSize
        {
            get { return GetPredefinedSize(this.DataType); }
        }

        public static int GetPredefinedScale(System.Data.SqlDbType dataType)
        {
            //Returns -1 if variable
            switch (dataType)
            {
                case System.Data.SqlDbType.Decimal:
                    return -1;
                default:
                    return 0;
            }
        }

        public virtual int PredefinedScale
        {
            get { return GetPredefinedScale(this.DataType); }
        }

        private int GetDefaultSize(System.Data.SqlDbType dataType)
        {
            var size = _length;
            switch (dataType)
            {
                case System.Data.SqlDbType.Decimal:
                case System.Data.SqlDbType.Real:
                    size = 18;
                    break;

                case System.Data.SqlDbType.Binary:
                case System.Data.SqlDbType.NVarChar:
                case System.Data.SqlDbType.VarBinary:
                case System.Data.SqlDbType.VarChar:
                    size = 50;
                    break;

                case System.Data.SqlDbType.Char:
                case System.Data.SqlDbType.NChar:
                    size = 10;
                    break;

                case System.Data.SqlDbType.DateTime2:
                    size = 7;
                    break;

            }
            return size;
        }

        #endregion

    }
}
