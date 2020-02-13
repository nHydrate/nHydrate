#pragma warning disable 0168
using System;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class CellEntry : BaseModelObject
    {
        #region Member Variables

        protected Reference _columnRef;
        protected string _value;

        #endregion

        #region Constructor

        public CellEntry(INHydrateModelObject root)
            : base(root)
        {
        }

        public CellEntry()
        {
        }

        #endregion

        #region Property Implementations

        public Reference ColumnRef
        {
            get { return _columnRef; }
            set { _columnRef = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public virtual bool IsDataValid()
        {
            var column = this.ColumnRef.Object as Column;
            if (column == null) return true; //No Verification

            //Some of these are not fool-proof but they are close enough!!

            var isNull = (this.Value == "(NULL)");
            switch (column.DataType)
            {
                case System.Data.SqlDbType.BigInt:
                    if (isNull && column.AllowNull) return true;
                    else return long.TryParse(this.Value, out _);
                case System.Data.SqlDbType.Binary:
                    return true; //no validation
                case System.Data.SqlDbType.Bit:
                    if (isNull && column.AllowNull) return true;
                    var v2 = this.Value + string.Empty;
                    if (v2 == "0") v2 = "false";
                    if (v2 == "1") v2 = "true";
                    return bool.TryParse(v2.ToLower(), out _);
                case System.Data.SqlDbType.Char:
                    return true;
                case System.Data.SqlDbType.Date:
                    if (isNull && column.AllowNull) return true;
                    else return DateTime.TryParse(this.Value, out _);
                case System.Data.SqlDbType.DateTime:
                    if (isNull && column.AllowNull) return true;
                    else return DateTime.TryParse(this.Value, out _);
                case System.Data.SqlDbType.DateTime2:
                    if (isNull && column.AllowNull) return true;
                    else return DateTime.TryParse(this.Value, out _);
                case System.Data.SqlDbType.DateTimeOffset:
                    return true; //no validation
                case System.Data.SqlDbType.Decimal:
                    if (isNull && column.AllowNull) return true;
                    else return decimal.TryParse(this.Value, out _);
                case System.Data.SqlDbType.Float:
                    if (isNull && column.AllowNull) return true;
                    else return decimal.TryParse(this.Value, out _);
                case System.Data.SqlDbType.Image:
                    return true;
                case System.Data.SqlDbType.Int:
                    if (isNull && column.AllowNull) return true;
                    else return int.TryParse(this.Value, out _);
                case System.Data.SqlDbType.Money:
                    if (isNull && column.AllowNull) return true;
                    else return decimal.TryParse(this.Value, out _);
                case System.Data.SqlDbType.NChar:
                    return true;
                case System.Data.SqlDbType.NText:
                    return true;
                case System.Data.SqlDbType.NVarChar:
                    return true;
                case System.Data.SqlDbType.Real:
                    if (isNull && column.AllowNull) return true;
                    else return decimal.TryParse(this.Value, out _);
                case System.Data.SqlDbType.SmallDateTime:
                    if (isNull && column.AllowNull) return true;
                    else return DateTime.TryParse(this.Value, out _);
                case System.Data.SqlDbType.SmallInt:
                    if (isNull && column.AllowNull) return true;
                    else return short.TryParse(this.Value, out _);
                case System.Data.SqlDbType.SmallMoney:
                    if (isNull && column.AllowNull) return true;
                    else return decimal.TryParse(this.Value, out _);
                case System.Data.SqlDbType.Structured:
                    return true;
                case System.Data.SqlDbType.Text:
                    return true;
                case System.Data.SqlDbType.Time:
                    if (isNull && column.AllowNull) return true;
                    else return DateTime.TryParse(this.Value, out _);
                case System.Data.SqlDbType.Timestamp:
                    return true; //no validation
                case System.Data.SqlDbType.TinyInt:
                    if (isNull && column.AllowNull) return true;
                    else return byte.TryParse(this.Value, out _);
                case System.Data.SqlDbType.Udt:
                    return true; //no validation
                case System.Data.SqlDbType.UniqueIdentifier:
                    if (isNull && column.AllowNull) return true;
                    try { var g = new Guid(this.Value); return true; }
                    catch { return false; }
                case System.Data.SqlDbType.VarBinary:
                    return true; //no validation
                case System.Data.SqlDbType.VarChar:
                    return true;
                case System.Data.SqlDbType.Variant:
                    return true; //no validation
                case System.Data.SqlDbType.Xml:
                    return true;
                default:
                    return true;
            }
        }

        #endregion

        #region Methods

        public string GetSQLData()
        {
            var column = this.ColumnRef.Object as Column;
            var v = this.Value + string.Empty;

            if (column.AllowNull && v == "(NULL)")
                return null;
            else if (ModelHelper.IsTextType(column.DataType) ||
                ModelHelper.IsDateType(column.DataType) ||
                column.DataType == System.Data.SqlDbType.UniqueIdentifier
                )
                return "'" + v.Replace("'", "''") + "'";
            else
                return v;
        }

        public string GetCodeData()
        {
            var column = this.ColumnRef.Object as Column;
            var v = this.Value + string.Empty;

            if (column.AllowNull && v == "(NULL)")
                return null;
            else if (ModelHelper.IsTextType(column.DataType) ||
                ModelHelper.IsDateType(column.DataType) ||
                column.DataType == System.Data.SqlDbType.UniqueIdentifier
                )
                return "\"" + v.Replace("\"", @"""") + "\"";
            else
                return v;
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                if (ColumnRef != null)
                {
                    var columnRefNode = oDoc.CreateElement("f");
                    if (this.ColumnRef == null)
                        this.ColumnRef = new Reference(this.Root);
                    this.ColumnRef.XmlAppend(columnRefNode);
                    node.AppendChild(columnRefNode);
                }

                XmlHelper.AddAttribute(node, "value", this.Value);
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
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                var columnRefNode = node.SelectSingleNode("columnRef"); //deprecated, use "f"
                if (columnRefNode == null) columnRefNode = node.SelectSingleNode("f");
                if (columnRefNode != null)
                {
                    if (this.ColumnRef == null)
                        this.ColumnRef = new Reference(this.Root);
                    this.ColumnRef.XmlLoad(columnRefNode);
                }

                this.Value = XmlHelper.GetAttributeValue(node, "value", string.Empty);

                this.Dirty = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}