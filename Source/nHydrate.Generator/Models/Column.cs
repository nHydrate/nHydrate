#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public enum IdentityTypeConstants
    {
        None,
        Database,
        Code,
    }

    public class Column : ColumnBase, ICodeFacadeObject, INamedObject
    {
        #region Member Variables

        protected const bool _def_primaryKey = false;
        protected const IdentityTypeConstants _def_identity = IdentityTypeConstants.None;
        protected const bool _def_codeImplementedIdentity = false;
        protected const int _def_sortOrder = 0;
        protected const bool _def_UIVisible = false;
        protected const string _def_mask = "";
        protected const double _def_min = double.NaN;
        protected const double _def_max = double.NaN;
        protected const bool _def_isIndexed = false;
        protected const bool _def_isUnique = false;
        protected const string _def_formula = "";
        protected const bool _def_computedColumn = false;
        protected const string _def_collate = "";
        protected const string _def_codefacade = "";
        protected const string _def_friendlyName = "";
        protected const string _def_default = "";
        protected const bool _def_defaultIsFunc = false;
        protected const string _def_validationExpression = "";
        protected const bool _def_isReadOnly = false;
        protected const bool _def_obsolete = false;

        protected string _codeFacade = _def_codefacade;
        protected bool _primaryKey = _def_primaryKey;
        protected IdentityTypeConstants _identity = _def_identity;
        protected string _default = _def_default;
        protected bool _defaultIsFunc = _def_defaultIsFunc;
        protected Reference _parentTableRef = null;
        protected Reference _relationshipRef = null;
        private string _enumType = string.Empty;
        private string _friendlyName = _def_friendlyName;
        private int _sortOrder = _def_sortOrder;
        private bool _UIVisible = _def_UIVisible;
        private string _mask = _def_mask;
        private double _min = _def_min;
        private double _max = _def_max;
        private bool _isIndexed = _def_isIndexed;
        //private DateTime _createdDate = DateTime.Now;
        protected bool _isUnique = _def_isUnique;
        protected string _collate = string.Empty;
        protected bool _computedColumn = _def_computedColumn;
        protected string _formula = _def_formula;
        protected string _validationExpression = _def_validationExpression;
        protected bool _isReadOnly = _def_isReadOnly;
        protected bool _obsolete = _def_obsolete;

        #endregion

        #region Constructor

        public Column(INHydrateModelObject root)
            : base(root)
        {
            this.MetaData = new MetadataItemCollection();
        }

        #endregion

        #region Property Implementations

        [Browsable(false)]
        public MetadataItemCollection MetaData { get; private set; }

        [Browsable(true)]
        [Description("Determines if this column can be updated.")]
        [Category("Data")]
        [DefaultValue(_def_isReadOnly)]
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsReadOnly"));
            }
        }

        public override bool AllowNull
        {
            get
            {
                if (this.ComputedColumn) return true;
                else if (this.IsUnique) return false;
                else return base.AllowNull && !this.PrimaryKey;
            }
            set { base.AllowNull = value; }
        }

        [
        Browsable(true),
        Description("Determines if this column is used in update or insert statments. Can be used to support calculated fields."),
        Category("Data"),
        DefaultValue(_def_computedColumn),
        ]
        public bool ComputedColumn
        {
            get { return _computedColumn; }
            set
            {
                _computedColumn = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("computedColumn"));
            }
        }

        [
        Browsable(true),
        Description("Formula for the computed column. This is only considered when the computed column is set to true."),
        Category("Data"),
        DefaultValue(""),
        ]
        public string Formula
        {
            get { return _formula; }
            set
            {
                _formula = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("formula"));
            }
        }

        [
        Browsable(true),
        Description("Determines the regular expression used to validate this field in the UI."),
        Category("Data"),
        DefaultValue(_def_validationExpression),
        ]
        public string ValidationExpression
        {
            get { return _validationExpression; }
            set
            {
                _validationExpression = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("validationExpression"));
            }
        }

        [
        Browsable(true),
        Description("Determine if this column is the table primary key."),
        Category("Data"),
        DefaultValue(_def_primaryKey),
        ]
        public bool PrimaryKey
        {
            get
            {
                if (this.ComputedColumn) return false;
                else return _primaryKey;
            }
            set
            {
                _primaryKey = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("primaryKey"));
            }
        }

        [
        Browsable(true),
        Description("Determines the type of identity for this column."),
        Category("Data"),
        DefaultValue(_def_identity),
        ]
        public IdentityTypeConstants Identity
        {
            get
            {
                if (!this.SupportsIdentity()) return IdentityTypeConstants.None;
                else if (this.ComputedColumn) return IdentityTypeConstants.None;
                else return _identity;
            }
            set
            {
                _identity = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Identity"));
            }
        }

        [Browsable(false)]
        public Reference RelationshipRef
        {
            get { return _relationshipRef; }
            set { _relationshipRef = value; }
        }

        [
        Browsable(true),
        Description("Determines the default value of this column."),
        Category("Data"),
        DefaultValue(_def_default),
        ]
        public string Default
        {
            get
            {
                if (this.ComputedColumn) return string.Empty;
                else return _default;
            }
            set
            {
                _default = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Default"));
            }
        }

        [
        Browsable(true),
        Description("Determines the default value of this column is a function."),
        Category("Data"),
        DefaultValue(_def_defaultIsFunc),
        ]
        public bool DefaultIsFunc
        {
            get
            {
                if (this.ComputedColumn) return false;
                else return _defaultIsFunc;
            }
            set
            {
                _defaultIsFunc = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("DefaultIsFunc"));
            }
        }

        [
        Browsable(true),
        Description("Determines the minimum value for a int, long, float value."),
        Category("Data"),
        DefaultValue(_def_min),
        ]
        public double Min
        {
            get
            {
                if (this.ComputedColumn) return _def_min;
                else return _min;
            }
            set
            {
                _min = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Min"));
            }
        }

        [Browsable(true)]
        [Description("Determines the maximum value for a int, long, float value.")]
        [Category("Data")]
        [DefaultValue(_def_max)]
        public double Max
        {
            get
            {
                if (this.ComputedColumn) return _def_max;
                else return _max;
            }
            set
            {
                _max = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Max"));
            }
        }

        [
        Browsable(true),
        Description("Determines if this field has a database index."),
        Category("Data"),
        DefaultValue(_def_isIndexed),
        ]
        public bool IsIndexed
        {
            get { return (_isIndexed || _primaryKey) && !_computedColumn; }
            set
            {
                _isIndexed = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsIndexed"));
            }
        }

        [
        Browsable(true),
        Description("Determines if this field is marked as unique."),
        Category("Data"),
        DefaultValue(_def_isUnique),
        ]
        public bool IsUnique
        {
            get { return (_isUnique || _primaryKey) && !_computedColumn; }
            set
            {
                _isUnique = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsUnique"));
            }
        }

        [
        Browsable(true),
        Description("Determines the field collation."),
        Category("Data"),
        DefaultValue(_def_collate),
        ]
        public string Collate
        {
            get
            {
                if (this.ComputedColumn) return _def_collate;
                else return _collate;
            }
            set
            {
                _collate = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Collate"));
            }
        }

        [Browsable(false)]
        public Reference ParentTableRef
        {
            get { return _parentTableRef; }
            set { _parentTableRef = value; }
        }

        [Browsable(false)]
        public Table ParentTable
        {
            get { return _parentTableRef.Object as Table; }
        }

        /// <summary>
        /// Determines a friend name to display to users
        /// </summary>
        [
        Browsable(true),
        Description("Determines a friend name to display to users"),
        Category("Appearance"),
        DefaultValue(_def_friendlyName)
        ]
        public string FriendlyName
        {
            get { return _friendlyName; }
            set
            {
                _friendlyName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("friendlyName"));
            }
        }

        /// <summary>
        /// Determines the sort order of this field in relation to other data visible fields.
        /// </summary>
        [
          Browsable(false),
        Description("Determines the sort order of this field in relation to other data visible fields."),
        Category("Appearance"),
        DefaultValue(_def_sortOrder),
        ]
        public int SortOrder
        {
            get { return _sortOrder; }
            set
            {
                _sortOrder = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("sortOrder"));
            }
        }

        /// <summary>
        /// Determines if the column is visible in grids.
        /// </summary>
        [
        Browsable(false),
        Description("Determines if the column is visible in grids."),
        Category("Appearance"),
        DefaultValue(_def_UIVisible),
        ]
        public bool UIVisible
        {
            get { return _UIVisible; }
            set
            {
                _UIVisible = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("UIVisible"));
            }
        }

        /// <summary>
        /// Identifies the mask for data input.
        /// </summary>
        [
        Browsable(false),
        Description("Identifies the mask for data input and presentation."),
        Category("Mask"),
        DefaultValue(_def_mask),
        ]
        public string Mask
        {
            get { return _mask; }
            set
            {
                _mask = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("mask"));
            }
        }

        /// <summary>
        /// Determines if the field is marked obsolete
        /// </summary>
        [
        Browsable(false),
        Description("Determines if the field is marked obsolete"),
        Category("Code"),
        DefaultValue(_def_obsolete),
        ]
        public bool Obsolete
        {
            get { return _obsolete; }
            set
            {
                _obsolete = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("obsolete"));
            }
        }

        [Browsable(false)]
        public override string CorePropertiesHash
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.Identity + "|" +
                    this.AllowNull + "|" +
                    this.Default + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.Collate + "|" +
                    this.PrimaryKey + "|" +
                    this.DataType.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        [Browsable(false)]
        public override string CorePropertiesHashNoPK
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.Identity + "|" +
                    this.AllowNull + "|" +
                    this.Default + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.Collate + "|" +
                    //this.PrimaryKey + "|" +
                    this.DataType.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        [Browsable(false)]
        public virtual string CorePropertiesHashNoPKOrDefault
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.Identity + "|" +
                    this.AllowNull + "|" +
                    //this.Default + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.Collate + "|" +
                    //this.PrimaryKey + "|" +
                    this.DataType.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines if the default value is a valid literal for the specified data type
        /// </summary>
        [Browsable(false)]
        public virtual bool IsLiteralDefaultValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.Default))
                    return false;

                var value = this.Default;
                switch (this.DataType)
                {
                    case System.Data.SqlDbType.BigInt:
                        {
                            long v;
                            return long.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.Binary:
                    case System.Data.SqlDbType.Image:
                    case System.Data.SqlDbType.VarBinary:
                        if (value.Length <= 2) return false;
                        if ((value.Substring(0, 2) == "0x") && (value.Length % 2 == 0) && value.Substring(2, value.Length - 2).IsHex()) return true;
                        return false;
                    case System.Data.SqlDbType.Bit:
                        {
                            var q = value.ToLower();
                            if (q == "1" || q == "0") return true;
                            bool v;
                            return bool.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.Char:
                        return true;
                    case System.Data.SqlDbType.Date:
                    case System.Data.SqlDbType.DateTime:
                    case System.Data.SqlDbType.DateTime2:
                    case System.Data.SqlDbType.SmallDateTime:
                    case System.Data.SqlDbType.Time:
                        {
                            DateTime v;
                            return DateTime.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.DateTimeOffset:
                        return false;
                    case System.Data.SqlDbType.Decimal:
                        {
                            decimal v;
                            return decimal.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.Float:
                        {
                            decimal v;
                            return decimal.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.Int:
                        {
                            int v;
                            return int.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.Money:
                    case System.Data.SqlDbType.SmallMoney:
                        {
                            decimal v;
                            return decimal.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.NChar:
                    case System.Data.SqlDbType.NText:
                    case System.Data.SqlDbType.NVarChar:
                    case System.Data.SqlDbType.Text:
                    case System.Data.SqlDbType.VarChar:
                        return true;
                    case System.Data.SqlDbType.Real:
                        {
                            decimal v;
                            return decimal.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.SmallInt:
                        {
                            Int16 v;
                            return Int16.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.TinyInt:
                        {
                            byte v;
                            return byte.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.UniqueIdentifier:
                        {
                            Guid v;
                            return Guid.TryParse(value, out v);
                        }
                    case System.Data.SqlDbType.Structured:
                    case System.Data.SqlDbType.Timestamp:
                    case System.Data.SqlDbType.Udt:
                    case System.Data.SqlDbType.Variant:
                    case System.Data.SqlDbType.Xml:
                        return false;
                    default:
                        return false;
                }

            }
        }

        [Browsable(false)]
        public virtual string GetFriendlyName()
        {
            if (string.IsNullOrEmpty(this.FriendlyName))
                return this.PascalName;
            else
                return this.FriendlyName;
        }

        [Browsable(false)]
        public bool SupportsIdentity()
        {
            return this.DataType == System.Data.SqlDbType.BigInt ||
                   this.DataType == System.Data.SqlDbType.Int ||
                   this.DataType == System.Data.SqlDbType.SmallInt ||
                   this.DataType == System.Data.SqlDbType.UniqueIdentifier;
        }

        /// <summary>
        /// Builds an Intellisnse string for the remarks section
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public string GetIntellisenseRemarks()
        {
            var text = "Field: [" + (this.ParentTableRef.Object as Table).DatabaseName + "].[" + this.DatabaseName + "], ";

            var length = this.GetCommentLengthString();
            if (!string.IsNullOrEmpty(length))
                text += "Field Length: " + length + ", ";

            text += (this.AllowNull ? "" : "Not ") + "Nullable, ";

            if (this.PrimaryKey)
                text += "Primary Key, ";

            if (this.Identity == IdentityTypeConstants.Database)
                text += "AutoNumber, ";

            if (this.IsUnique)
                text += "Unique, ";

            if (this.IsIndexed)
                text += "Indexed, ";

            if (!double.IsNaN(this.Min) || !double.IsNaN(this.Max))
            {
                text += "Range [" + (double.IsNaN(this.Min) ? "INF" : this.Min.ToString()) + ".." + (double.IsNaN(this.Max) ? "INF" : this.Max.ToString()) + "], ";
            }

            if (!string.IsNullOrEmpty(this.Default))
                text += "Default Value: " + this.Default;

            //Strip off last comma
            if (text.EndsWith(", "))
                text = text.Substring(0, text.Length - 2);

            return text;
        }

        public virtual bool CanHaveDefault()
        {
            switch (this.DataType)
            {
                case System.Data.SqlDbType.BigInt:
                    return true;
                case System.Data.SqlDbType.Binary:
                    return true;
                case System.Data.SqlDbType.Bit:
                    return true;
                case System.Data.SqlDbType.Char:
                    return true;
                case System.Data.SqlDbType.Date:
                    return true;
                case System.Data.SqlDbType.DateTime:
                    return true;
                case System.Data.SqlDbType.DateTime2:
                    return true;
                case System.Data.SqlDbType.DateTimeOffset:
                    return false;
                case System.Data.SqlDbType.Decimal:
                    return true;
                case System.Data.SqlDbType.Float:
                    return true;
                case System.Data.SqlDbType.Image:
                    return true;
                case System.Data.SqlDbType.Int:
                    return true;
                case System.Data.SqlDbType.Money:
                    return true;
                case System.Data.SqlDbType.NChar:
                    return true;
                case System.Data.SqlDbType.NText:
                    return true;
                case System.Data.SqlDbType.NVarChar:
                    return true;
                case System.Data.SqlDbType.Real:
                    return true;
                case System.Data.SqlDbType.SmallDateTime:
                    return true;
                case System.Data.SqlDbType.SmallInt:
                    return true;
                case System.Data.SqlDbType.SmallMoney:
                    return true;
                case System.Data.SqlDbType.Structured:
                    return false;
                case System.Data.SqlDbType.Text:
                    return true;
                case System.Data.SqlDbType.Time:
                    return true;
                case System.Data.SqlDbType.Timestamp:
                    return false;
                case System.Data.SqlDbType.TinyInt:
                    return true;
                case System.Data.SqlDbType.Udt:
                    return false;
                case System.Data.SqlDbType.UniqueIdentifier:
                    return true;
                case System.Data.SqlDbType.VarBinary:
                    return true;
                case System.Data.SqlDbType.VarChar:
                    return true;
                case System.Data.SqlDbType.Variant:
                    return false;
                case System.Data.SqlDbType.Xml:
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Gets the C# code equivalent for this default value
        /// </summary>
        /// <returns></returns>
        public virtual string GetCodeDefault()
        {
            var defaultValue = string.Empty;
            if (this.IsDateType)
            {
                var scrubbed = this.Default.Replace("(", string.Empty).Replace(")", string.Empty);
                if (scrubbed == "getdate")
                {
                    defaultValue = String.Format("DateTime.Now", this.PascalName);
                }
                else if (scrubbed == "getutcdate")
                {
                    defaultValue = String.Format("DateTime.UtcNow", this.PascalName);
                }
                else if (scrubbed.StartsWith("getdate+"))
                {
                    var t = this.Default.Substring(8, this.Default.Length - 8);
                    var tarr = t.Split('-');
                    if (tarr.Length == 2)
                    {
                        if (tarr[1] == "year")
                            defaultValue = String.Format("DateTime.Now.AddYears(" + tarr[0] + ")", this.PascalName);
                        else if (tarr[1] == "month")
                            defaultValue = String.Format("DateTime.Now.AddMonths(" + tarr[0] + ")", this.PascalName);
                        else if (tarr[1] == "day")
                            defaultValue = String.Format("DateTime.Now.AddDays(" + tarr[0] + ")", this.PascalName);
                    }
                }
                else
                {
                    defaultValue = string.Empty;
                }
                //else if (this.DataType == System.Data.SqlDbType.SmallDateTime)
                //{
                //  defaultValue = String.Format("new DateTime(1900, 1, 1)", this.PascalName);
                //}
                //else
                //{
                //  defaultValue = String.Format("new DateTime(1753, 1, 1)", this.PascalName);
                //}
            }
            else if (this.DataType == System.Data.SqlDbType.Char)
            {
                defaultValue = "\" \"";
                if (this.Default.Length == 1)
                    defaultValue = "@\"" + this.Default[0].ToString().Replace("\"", @"""") + "\"";
            }
            else if (this.IsBinaryType)
            {
                defaultValue = "new System.Byte[] { " + this.Default.ConvertToHexArrayString() + " }";
            }
            //else if (this.DataType == System.Data.SqlDbType.DateTimeOffset)
            //{
            //  defaultValue = "DateTimeOffset.MinValue";
            //}
            //else if (this.IsDateType)
            //{
            //  defaultValue = "System.DateTime.MinValue";
            //}
            //else if (this.DataType == System.Data.SqlDbType.Time)
            //{
            //  defaultValue = "0";
            //}
            else if (this.DataType == System.Data.SqlDbType.UniqueIdentifier)
            {
                if ((StringHelper.Match(this.Default, "newid", true)) || (StringHelper.Match(this.Default, "newid()", true)))
                    defaultValue = String.Format("Guid.NewGuid()");
                else if (string.IsNullOrEmpty(this.Default))
                    defaultValue = "System.Guid.Empty";
                else if (this.Default.ToLower().Contains("newsequentialid"))
                    defaultValue = (_root as ModelRoot).ProjectName + "Entities.GetNextSequentialGuid(EntityMappingConstants." + ParentTable.PascalName + ", Entity." + ParentTable.PascalName + ".FieldNameConstants." + this.PascalName + ".ToString())";
                else if (!string.IsNullOrEmpty(this.Default) && this.Default.Length == 36)
                    defaultValue = "new Guid(\"" + this.Default.Replace("'", "") + "\")";
            }
            else if (this.IsIntegerType)
            {
                defaultValue = "0";
                int i;
                if (int.TryParse(this.Default, out i))
                    defaultValue = this.Default;
                if (this.DataType == System.Data.SqlDbType.BigInt) defaultValue += "L";
            }
            else if (this.IsNumericType)
            {
                defaultValue = "0";
                double d;
                if (double.TryParse(this.Default, out d))
                {
                    defaultValue = this.Default;
                    if (this.GetCodeType(false) == "decimal") defaultValue += "M";
                }
            }
            else if (this.DataType == System.Data.SqlDbType.Bit)
            {
                defaultValue = "false";
                if (this.Default == "0")
                    defaultValue = String.Format("false");
                else if (this.Default == "1")
                    defaultValue = String.Format("true");
            }
            else
            {
                if (ModelHelper.IsTextType(this.DataType))
                    defaultValue = "\"" + this.Default.Replace("''", "") + "\"";
                else
                    defaultValue = "\"" + this.Default + "\"";
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the SQL equivalent for this default value
        /// </summary>
        /// <returns></returns>
        public virtual string GetSQLDefault()
        {
            return ModelHelper.GetSQLDefault(this.DataType, this.Default);
        }

        [Browsable(false)]
        public virtual bool IsValidDefault()
        {
            if (!this.CanHaveDefault())
                return string.IsNullOrEmpty(this.Default);

            return IsValidDefault(this.Default);
        }

        [Browsable(false)]
        public virtual bool IsValidDefault(string value)
        {
            //No default is valid for everything
            if (string.IsNullOrEmpty(value)) return true;
            //There is a default and one is not valid so always false
            if (!this.CanHaveDefault()) return false;

            switch (this.DataType)
            {
                case System.Data.SqlDbType.BigInt:
                    {
                        long v;
                        return long.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.Binary:
                case System.Data.SqlDbType.Image:
                case System.Data.SqlDbType.VarBinary:
                    if (string.IsNullOrEmpty(value)) return false;
                    if (value.Length <= 2) return false;
                    if ((value.Substring(0, 2) == "0x") && (value.Length % 2 == 0) && value.Substring(2, value.Length - 2).IsHex()) return true;
                    return false;
                case System.Data.SqlDbType.Bit:
                    {
                        var q = value.ToLower();
                        if (q == "1" || q == "0") return true;
                        bool v;
                        return bool.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.Char:
                    return true;
                case System.Data.SqlDbType.Date:
                    {
                        var d = this.GetCodeDefault();
                        return !string.IsNullOrEmpty(d);
                    }
                case System.Data.SqlDbType.DateTime:
                    {
                        var d = this.GetCodeDefault();
                        return !string.IsNullOrEmpty(d);
                    }
                case System.Data.SqlDbType.DateTime2:
                    {
                        var d = this.GetCodeDefault();
                        return !string.IsNullOrEmpty(d);
                    }
                case System.Data.SqlDbType.DateTimeOffset:
                    return false;
                case System.Data.SqlDbType.Decimal:
                    {
                        decimal v;
                        return decimal.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.Float:
                    {
                        decimal v;
                        return decimal.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.Int:
                    {
                        int v;
                        return int.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.Money:
                    {
                        long v;
                        return long.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.NChar:
                    return true;
                case System.Data.SqlDbType.NText:
                    return true;
                case System.Data.SqlDbType.NVarChar:
                    return true;
                case System.Data.SqlDbType.Real:
                    {
                        decimal v;
                        return decimal.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.SmallDateTime:
                    {
                        var d = this.GetCodeDefault();
                        return !string.IsNullOrEmpty(d);
                    }
                case System.Data.SqlDbType.SmallInt:
                    {
                        Int16 v;
                        return Int16.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.SmallMoney:
                    {
                        int v;
                        return int.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.Structured:
                    return false;
                case System.Data.SqlDbType.Text:
                    return true;
                case System.Data.SqlDbType.Time:
                    {
                        var d = this.GetCodeDefault();
                        return !string.IsNullOrEmpty(d);
                    }
                case System.Data.SqlDbType.Timestamp:
                    return false;
                case System.Data.SqlDbType.TinyInt:
                    {
                        byte v;
                        return byte.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.Udt:
                    return false;
                case System.Data.SqlDbType.UniqueIdentifier:
                    {
                        if (value.ToLower() == "newid") return true;
                        if (value.ToLower() == "newsequentialid") return true;
                        try
                        {
                            var v = new Guid(value);
                            return true;
                        }
                        catch { return false; }
                    }
                case System.Data.SqlDbType.VarChar:
                    return true;
                case System.Data.SqlDbType.Variant:
                    return false;
                case System.Data.SqlDbType.Xml:
                    return false;
            }
            return false;
        }

        protected internal void ResetKey()
        {
            _key = Guid.NewGuid().ToString();
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                if (this.PrimaryKey != _def_primaryKey)
                    XmlHelper.AddAttribute(node, "primaryKey", this.PrimaryKey);

                if (this.ComputedColumn != _def_computedColumn)
                    XmlHelper.AddAttribute(node, "computedColumn", this.ComputedColumn);

                if (this.IsReadOnly != _def_isReadOnly)
                    XmlHelper.AddAttribute(node, "isReadOnly", this.IsReadOnly);

                if (this.Formula != _def_formula)
                    XmlHelper.AddAttribute(node, "formula", this.Formula);

                if (this.ValidationExpression != _def_validationExpression)
                    XmlHelper.AddAttribute(node, "validationExpression", this.ValidationExpression);

                if (this.Generated != _def_generated)
                    XmlHelper.AddAttribute(node, "generated", this.Generated);

                if (this.UIDataType != _def_uIDataType)
                    XmlHelper.AddAttribute(node, "uidatatype", this.UIDataType.ToString());

                if (this.Identity != _def_identity)
                    XmlHelper.AddAttribute(node, "identity", (int)this.Identity);

                XmlHelper.AddAttribute(node, "name", this.Name);

                if (this.CodeFacade != _def_codefacade)
                    XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

                if (this.Description != _def_description)
                    XmlHelper.AddAttribute(node, "description", this.Description);

                if (this.Prompt != _def_prompt)
                    XmlHelper.AddAttribute(node, "prompt", this.Prompt);

                if (this.FriendlyName != _def_friendlyName)
                    XmlHelper.AddAttribute(node, "dataFieldFriendlyName", this.FriendlyName);

                if (this.UIVisible != _def_UIVisible)
                    XmlHelper.AddAttribute(node, "dataFieldVisibility", this.UIVisible);

                if (this.SortOrder != _def_sortOrder)
                    XmlHelper.AddAttribute(node, "dataFieldSortOrder", this.SortOrder);

                if (this.Default != _def_default)
                    XmlHelper.AddAttribute(node, "default", this.Default);

                if (this.DefaultIsFunc != _def_defaultIsFunc)
                    XmlHelper.AddAttribute(node, "defaultIsFunc", this.DefaultIsFunc);

                if ((this.Length != _def_length) && !this.IsDefinedSize)
                    XmlHelper.AddAttribute(node, "length", this.Length);

                if (this.Scale != _def_scale && !this.IsDefinedScale)
                    XmlHelper.AddAttribute(node, "scale", this.Scale);

                if (this.IsIndexed != _def_isIndexed)
                    XmlHelper.AddAttribute(node, "isIndexed", this.IsIndexed);

                if (this.IsUnique != _def_isUnique)
                    XmlHelper.AddAttribute(node, "isUnique", this.IsUnique);

                if (this.Collate != _def_collate)
                    XmlHelper.AddAttribute(node, "collate", this.Collate);

                XmlHelper.AddAttribute(node, "id", this.Id);

                XmlHelper.AddAttribute(node, "type", (int)this.DataType);

                if (this.AllowNull != _def_allowNull)
                    XmlHelper.AddAttribute(node, "allowNull", this.AllowNull);

                if (this.IsBrowsable != _def_isBrowsable)
                    XmlHelper.AddAttribute(node, "isBrowsable", this.IsBrowsable);

                if (this.Category != string.Empty)
                    XmlHelper.AddAttribute(node, "category", this.Category);

                if (!double.IsNaN(this.Min)) XmlHelper.AddAttribute(node, "min", this.Min);

                if (!double.IsNaN(this.Max)) XmlHelper.AddAttribute(node, "max", this.Max);

                if (this.Mask != _def_mask)
                    XmlHelper.AddAttribute(node, "mask", this.Mask);

                if (this.Obsolete != _def_obsolete)
                    XmlHelper.AddAttribute(node, "obsolete", this.Obsolete);

                //XmlHelper.AddAttribute(node, "createdDate", _createdDate.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));

                if (RelationshipRef != null)
                {
                    var relationshipRefNode = oDoc.CreateElement("relationshipRef");
                    RelationshipRef.XmlAppend(relationshipRefNode);
                    node.AppendChild(relationshipRefNode);
                }

                var parentTableRefNode = oDoc.CreateElement("pt");
                ParentTableRef.XmlAppend(parentTableRefNode);
                node.AppendChild(parentTableRefNode);

                if (this.MetaData.Count > 0)
                {
                    var metadataNode = oDoc.CreateElement("metadata");
                    this.MetaData.XmlAppend(metadataNode);
                    node.AppendChild(metadataNode);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void XmlLoad(XmlNode node)
        {
            try
            {
                _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.PrimaryKey = XmlHelper.GetAttributeValue(node, "primaryKey", _def_primaryKey);
                this.ComputedColumn = XmlHelper.GetAttributeValue(node, "computedColumn", _def_computedColumn);
                this.IsReadOnly = XmlHelper.GetAttributeValue(node, "isReadOnly", _def_isReadOnly);
                this.Formula = XmlHelper.GetAttributeValue(node, "formula", _def_formula);
                this.ValidationExpression = XmlHelper.GetAttributeValue(node, "validationExpression", _def_validationExpression);
                this.Generated = XmlHelper.GetAttributeValue(node, "generated", _def_generated);
                this.UIDataType = (System.ComponentModel.DataAnnotations.DataType)Enum.Parse(typeof(System.ComponentModel.DataAnnotations.DataType), XmlHelper.GetAttributeValue(node, "uidatatype", _def_uIDataType.ToString()), true);
                this.Identity = (IdentityTypeConstants)XmlHelper.GetAttributeValue(node, "identity", (int)_def_identity);
                this.Name = XmlHelper.GetAttributeValue(node, "name", _name);
                this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", string.Empty);
                this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
                this.Prompt = XmlHelper.GetAttributeValue(node, "prompt", _def_prompt);
                this.FriendlyName = XmlHelper.GetAttributeValue(node, "dataFieldFriendlyName", _def_friendlyName);
                this.UIVisible = XmlHelper.GetAttributeValue(node, "dataFieldVisibility", _def_UIVisible);
                this.SortOrder = XmlHelper.GetAttributeValue(node, "dataFieldSortOrder", _def_sortOrder);
                this.Min = XmlHelper.GetAttributeValue(node, "min", _def_min);
                this.Max = XmlHelper.GetAttributeValue(node, "max", _def_max);
                this.Mask = XmlHelper.GetAttributeValue(node, "mask", _def_mask);
                this.Obsolete = XmlHelper.GetAttributeValue(node, "obsolete", _def_obsolete);
                //_createdDate = DateTime.ParseExact(XmlHelper.GetAttributeValue(node, "createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                this.IsIndexed = XmlHelper.GetAttributeValue(node, "isIndexed", _def_isIndexed);
                this.IsUnique = XmlHelper.GetAttributeValue(node, "isUnique", _def_isUnique);
                this.Collate = XmlHelper.GetAttributeValue(node, "collate", _def_collate);
                var relationshipRefNode = node.SelectSingleNode("relationshipRef");
                if (relationshipRefNode != null)
                {
                    RelationshipRef = new Reference(this.Root);
                    RelationshipRef.XmlLoad(relationshipRefNode);
                }

                var defaultValue = XmlHelper.GetAttributeValue(node, "default", _def_default);
                defaultValue = defaultValue.Replace("\n", string.Empty);
                defaultValue = defaultValue.Replace("\r", string.Empty);
                this.Default = defaultValue;

                _defaultIsFunc = XmlHelper.GetAttributeValue(node, "defaultIsFunc", _def_defaultIsFunc);

                _length = XmlHelper.GetAttributeValue(node, "length", _def_length);
                _scale = XmlHelper.GetAttributeValue(node, "scale", _def_scale);
                this.ResetId(XmlHelper.GetAttributeValue(node, "id", _id));

                var parentTableRefNode = node.SelectSingleNode("parentTableRef"); //deprecated, use "pt"
                if (parentTableRefNode == null) parentTableRefNode = node.SelectSingleNode("pt");
                ParentTableRef = new Reference(this.Root);
                ParentTableRef.XmlLoad(parentTableRefNode);

                var typeString = node.Attributes["type"].Value;
                if (!string.IsNullOrEmpty(typeString))
                {
                    //DO NOT set the real field as there is validation on it.
                    _dataType = (System.Data.SqlDbType)int.Parse(typeString);
                }

                this.AllowNull = XmlHelper.GetAttributeValue(node, "allowNull", _def_allowNull);
                this.IsBrowsable = XmlHelper.GetAttributeValue(node, "isBrowsable", _def_isBrowsable);
                this.Category = XmlHelper.GetAttributeValue(node, "category", string.Empty);

                var metadataNode = node.SelectSingleNode("metadata");
                if (metadataNode != null)
                    this.MetaData.XmlLoad(metadataNode);

                this.Dirty = false;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region Helpers

        public override Reference CreateRef()
        {
            return CreateRef(Guid.NewGuid().ToString());
        }

        public override Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.Column;
            return returnVal;
        }

        [Browsable(false)]
        public override string PascalName
        {
            get
            {
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                else if ((this.CodeFacade == "") && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot)this.Root).TransformNames))
                    return this.CodeFacade;
                else if ((this.CodeFacade == "") && !(((ModelRoot)this.Root).TransformNames))
                    return this.Name; //return StringHelper.FirstCharToUpper(this.Name);

                //return StringHelper.FirstCharToUpper(this.Name); //Default
                return this.Name; //Default
            }
        }

        [Browsable(false)]
        public string EnumType
        {
            get { return _enumType; }
            set { _enumType = value; }
        }

        public override string GetCodeType(bool allowNullable, bool forceNull)
        {
            var retval = string.Empty;
            if (!string.IsNullOrEmpty(this.EnumType))
            {
                retval = this.EnumType;
                if (allowNullable && (this.AllowNull || forceNull))
                    retval += "?";
                return retval;
            }
            else
            {
                return base.GetCodeType(allowNullable, forceNull);
            }
        }

        #endregion

        #region ICodeFacadeObject Members

        [
        Browsable(true),
        Description("Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier."),
        Category("Design"),
        DefaultValue(""),
        ]
        public string CodeFacade
        {
            get { return _codeFacade; }
            set
            {
                _codeFacade = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("codeFacade"));
            }
        }

        public string GetCodeFacade()
        {
            if (string.IsNullOrEmpty(this.CodeFacade))
                return this.Name;
            else
                return this.CodeFacade;
        }

        #endregion

    }
}