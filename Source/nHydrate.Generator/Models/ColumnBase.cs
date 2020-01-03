#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
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

        protected const System.ComponentModel.DataAnnotations.DataType _def_uIDataType =
            System.ComponentModel.DataAnnotations.DataType.Custom;

        protected int _id = 0;
        protected string _name = string.Empty;
        protected string _description = _def_description;
        protected string _prompt = _def_prompt;
        protected System.Data.SqlDbType _dataType = _def_type;
        protected int _length = _def_length;
        protected int _scale = _def_scale;
        protected bool _generated = _def_generated;
        protected bool _allowNull = _def_allowNull;
        protected bool _isBrowsable = _def_isBrowsable;
        protected string _category = string.Empty;
        protected System.ComponentModel.DataAnnotations.DataType _uIDataType = _def_uIDataType;
        //private DateTime _createdDate = DateTime.Now;

        #endregion

        #region Constructor

        public ColumnBase(INHydrateModelObject root)
            : base(root)
        {
        }

        #endregion

        #region Property Implementations

        [Browsable(false)]
        public System.ComponentModel.DataAnnotations.DataType UIDataType
        {
            get { return _uIDataType; }
            set
            {
                _uIDataType = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("UIDataType"));
            }
        }

        [Browsable(false)]
        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("category"));
            }
        }

        [
            Browsable(true),
            Description("Determines if this item is used in the generation."),
            Category("Data"),
            DefaultValue(_def_generated),
        ]
        public virtual bool Generated
        {
            get { return _generated; }
            set
            {
                _generated = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("generated"));
            }
        }

        [
            Browsable(true),
            Description("Determines the name of this column."),
            Category("Design"),
            DefaultValue(""),
        ]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        [
            Browsable(true),
            Description("Determines description text were applicable."),
            Category("Data"),
            DefaultValue(""),
        ]
        public virtual string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        [
            Browsable(true),
            Description("Determines the prompt text were applicable."),
            Category("Data"),
            DefaultValue(""),
        ]
        public virtual string Prompt
        {
            get { return _prompt; }
            set
            {
                _prompt = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Prompt"));
            }
        }

        [
            Browsable(true),
            Description("Determines the size of this column in bytes."),
            Category("Data"),
            DefaultValue(_def_length),
        ]
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
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Length"));
            }
        }

        [Browsable(true)]
        [Description("Determines the scale of some data types.")]
        [Category("Data")]
        [DefaultValue(_def_scale)]
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
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Scale"));
            }
        }

        [Browsable(false)]
        public virtual int Id
        {
            get { return _id; }
        }

        [
            Browsable(true),
            Description("Determines the data type of this column."),
            Category("Data"),
            DefaultValue(System.Data.SqlDbType.VarChar),
            //TypeConverter(typeof(DataTypeConverter)),
            //Editor(typeof(nHydrate.Generator.Design.Editors.DataTypeEditor), typeof(System.Drawing.Design.UITypeEditor))
        ]
        public virtual System.Data.SqlDbType DataType
        {
            get { return _dataType; }
            set
            {
                var sqlType = ((ModelRoot)this.Root).SQLServerType;
                if (!Column.IsSupportedType(value, sqlType))
                {
                    throw new Exception("Unsupported type '" + value.ToString() + "' for SQL Server type '" + sqlType + "'.");
                }

                _length = this.GetDefaultSize(value);
                _dataType = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Type"));
            }
        }

        [
            Browsable(true),
            Description("Determines if this column allows null values."),
            Category("Data"),
            DefaultValue(_def_allowNull),
            //Editor(typeof(nHydrate.Generator.Design.Editors.AllowNullEditor), typeof(System.Drawing.Design.UITypeEditor)),
            //TypeConverter(typeof(AllowNullConverter)),
        ]
        public virtual bool AllowNull
        {
            get { return this.DataType != System.Data.SqlDbType.Variant && _allowNull; }
            set
            {
                _allowNull = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("allowNull"));
            }
        }

        [
            Browsable(true),
            Description("Determines if this column is browsable in the UI."),
            Category("Data"),
            DefaultValue(_def_isBrowsable),
        ]
        public virtual bool IsBrowsable
        {
            get { return _isBrowsable; }
            set
            {
                _isBrowsable = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("isBrowsable"));
            }
        }

        [Browsable(false)]
        public virtual string DatabaseType
        {
            get { return this.GetSQLDefaultType(); }
        }

        [Browsable(false)]
        public virtual string DatabaseTypeRaw
        {
            get { return this.GetSQLDefaultType(true); }
        }

        [Browsable(false)]
        public virtual bool IsTextType
        {
            get { return ModelHelper.IsTextType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsBinaryType
        {
            get { return ModelHelper.IsBinaryType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsIntegerType
        {
            get { return ModelHelper.IsIntegerType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsNumericType
        {
            get { return ModelHelper.IsNumericType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsMoneyType
        {
            get { return ModelHelper.IsMoneyType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsDecimalType
        {
            get { return ModelHelper.IsDecimalType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsDateType
        {
            get { return ModelHelper.IsDateType(this.DataType); }
        }

        /// <summary>
        /// Determines if this field type can be made into a range query
        /// </summary>
        [Browsable(false)]
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

        //[Browsable(true)]
        //[Category("Data")]
        //[Description("The date that this object was created.")]
        //[ReadOnlyAttribute(true)]
        //public virtual DateTime CreatedDate
        //{
        //  get { return _createdDate; }
        //}

        [Browsable(false)]
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

        [Browsable(false)]
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
        [Browsable(false)]
        public virtual bool IsDefinedSize
        {
            get { return this.PredefinedSize != -1; }
        }

        /// <summary>
        /// Determines if this data type supports a user-defined scale
        /// </summary>
        [Browsable(false)]
        public virtual bool IsDefinedScale
        {
            get { return this.PredefinedScale == 0; }
        }

        /// <summary>
        /// Determines if this field has no max length defined
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
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

        [Browsable(false)]
        public virtual string GetLengthString()
        {
            if (ModelHelper.SupportsMax(this.DataType) && this.Length == 0)
                return "max";
            else
                return this.Length.ToString();
        }

        /// <summary>
        /// This is the length used for annotations and meta data for class descriptions
        /// </summary>
        [Browsable(false)]
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

            if (ModelHelper.SupportsMax(this.DataType) && this.Length == 0)
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
        public static bool IsSupportedType(System.Data.SqlDbType type, SQLServerTypeConstants sqlVersion)
        {
            if (sqlVersion == SQLServerTypeConstants.SQL2005)
            {
                switch (type)
                {
                    //case System.Data.SqlDbType.Xml:
                    case System.Data.SqlDbType.Udt:
                    case System.Data.SqlDbType.Structured:
                    case System.Data.SqlDbType.Variant:
                    case System.Data.SqlDbType.DateTimeOffset:
                    case System.Data.SqlDbType.DateTime2:
                    case System.Data.SqlDbType.Time:
                    case System.Data.SqlDbType.Date:
                        return false;
                    default:
                        return true;
                }
            }
            else if ((sqlVersion == SQLServerTypeConstants.SQL2008) || (sqlVersion == SQLServerTypeConstants.SQLAzure))
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
            else
            {
                return false;
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

        [Browsable(false)]
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

        [Browsable(false)]
        public abstract string PascalName { get; }

        [Browsable(false)]
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
        [Browsable(false)]
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

        public virtual void ResetId(int id)
        {
            _id = id;
            this.OnPropertyChanged(this, new PropertyChangedEventArgs("Id"));
        }

        protected internal virtual void SetKey(string key)
        {
            _key = key;
            this.OnPropertyChanged(this, new PropertyChangedEventArgs("Key"));
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