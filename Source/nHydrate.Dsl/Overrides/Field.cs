#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using nHydrate.Generator.Common.Util;
using System.ComponentModel;
using nHydrate.Generator.Common.GeneratorFramework;
using DslModeling = global::Microsoft.VisualStudio.Modeling;

namespace nHydrate.Dsl
{
    partial class Field : nHydrate.Dsl.IContainerParent, nHydrate.Dsl.IField, IDirtyable
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public Field(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public Field(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
        }
        #endregion

        /// <summary>
        /// This is the image to use on the diagram. If null one will be calculated
        /// </summary>
        internal System.Drawing.Bitmap CachedImage { get; set; }

        #region Names
        public string PascalName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.CodeFacade))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                else
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
            }
        }

        public string DatabaseName => this.Name;

        #endregion

        #region Methods

        public virtual bool CanHaveDefault()
        {
            switch (this.DataType)
            {
                case DataTypeConstants.BigInt:
                case DataTypeConstants.Binary:
                case DataTypeConstants.Bit:
                case DataTypeConstants.Char:
                case DataTypeConstants.Date:
                case DataTypeConstants.DateTime:
                case DataTypeConstants.DateTime2:
                case DataTypeConstants.Decimal:
                case DataTypeConstants.Float:
                case DataTypeConstants.Image:
                case DataTypeConstants.Int:
                case DataTypeConstants.Money:
                case DataTypeConstants.NChar:
                case DataTypeConstants.NText:
                case DataTypeConstants.NVarChar:
                case DataTypeConstants.Real:
                case DataTypeConstants.SmallDateTime:
                case DataTypeConstants.SmallInt:
                case DataTypeConstants.SmallMoney:
                case DataTypeConstants.Text:
                case DataTypeConstants.Time:
                case DataTypeConstants.TinyInt:
                case DataTypeConstants.UniqueIdentifier:
                case DataTypeConstants.VarBinary:
                case DataTypeConstants.VarChar:
                    return true;
                case DataTypeConstants.Structured:
                case DataTypeConstants.Timestamp:
                case DataTypeConstants.Udt:
                case DataTypeConstants.Variant:
                case DataTypeConstants.Xml:
                case DataTypeConstants.DateTimeOffset:
                    return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets the C# code equivalent for this default value
        /// </summary>
        /// <returns></returns>
        public virtual string GetCodeDefault()
        {
            var defaultValue = string.Empty;
            var userValue = this.Default + string.Empty;
            if ((this.DataType == DataTypeConstants.DateTime) || (this.DataType == DataTypeConstants.SmallDateTime))
            {
                if (StringHelper.Match(userValue, "getdate", true) || StringHelper.Match(userValue, "getdate()", true) ||
                    StringHelper.Match(userValue, "sysdatetime", true) || StringHelper.Match(userValue, "sysdatetime()", true))
                {
                    defaultValue = String.Format("DateTime.Now", this.PascalName);
                }
                else if (StringHelper.Match(userValue, "getutcdate", true) || StringHelper.Match(userValue, "getutcdate()", true))
                {
                    defaultValue = String.Format("DateTime.UtcNow", this.PascalName);
                }
                else if (userValue.ToLower().StartsWith("getdate+") || userValue.ToLower().StartsWith("sysdatetime+"))
                {
                    var br = userValue.IndexOf("+") + 1;
                    var t = userValue.Substring(br, userValue.Length - br);
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
                //else if (this.DataType == DataTypeConstants.SmallDateTime)
                //{
                //  defaultValue = String.Format("new DateTime(1900, 1, 1)", this.PascalName);
                //}
                //else
                //{
                //  defaultValue = String.Format("new DateTime(1753, 1, 1)", this.PascalName);
                //}
            }
            else if (this.DataType == DataTypeConstants.Char)
            {
                defaultValue = "\" \"";
                if (userValue.Length == 1)
                    defaultValue = "@\"" + userValue.First().ToString().Replace("\"", @"""") + "\"";
            }
            else if (this.DataType.IsBinaryType())
            {
                defaultValue = "new System.Byte[] { " + userValue.ConvertToHexArrayString() + " }";
            }
            //else if (this.DataType == DataTypeConstants.DateTimeOffset)
            //{
            //  defaultValue = "DateTimeOffset.MinValue";
            //}
            //else if (this.IsDateType)
            //{
            //  defaultValue = "System.DateTime.MinValue";
            //}
            //else if (this.DataType == DataTypeConstants.Time)
            //{
            //  defaultValue = "0";
            //}
            else if (this.DataType == DataTypeConstants.UniqueIdentifier)
            {
                if ((StringHelper.Match(userValue, "newid", true)) || (StringHelper.Match(userValue, "newid()", true)))
                    defaultValue = "Guid.NewGuid()";
                else if (string.IsNullOrEmpty(userValue))
                    defaultValue = "System.Guid.Empty";
                else
                {
                    var gv = userValue.Replace("'", string.Empty);
                    if (Guid.TryParse(gv, out _))
                        defaultValue = "new Guid(\"" + gv + "\")";
                }
            }
            else if (this.DataType.IsIntegerType())
            {
                defaultValue = "0";
                int i;
                if (int.TryParse(userValue, out i))
                    defaultValue = userValue;
                if (this.DataType == DataTypeConstants.BigInt) defaultValue += "L";
            }
            else if (this.DataType.IsNumericType())
            {
                defaultValue = "0";
                double d;
                if (double.TryParse(userValue, out d))
                {
                    defaultValue = userValue;
                    if (this.GetCodeType(false) == "decimal") defaultValue += "M";
                }
            }
            else if (this.DataType == DataTypeConstants.Bit)
            {
                if (userValue == "0" || StringHelper.Match(userValue, "false", true))
                    defaultValue = "false";
                else if (userValue == "1" || StringHelper.Match(userValue, "true", true))
                    defaultValue = "true";
            }
            else
            {
                if (this.DataType.IsTextType())
                    defaultValue = "\"" + userValue.Replace("''", "") + "\"";
                else
                    defaultValue = "\"" + userValue + "\"";
            }
            return defaultValue;
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
                case DataTypeConstants.BigInt:
                    return long.TryParse(value, out _);
                case DataTypeConstants.Binary:
                case DataTypeConstants.Image:
                case DataTypeConstants.VarBinary:
                    if (string.IsNullOrEmpty(value)) return false;
                    if (value.Length <= 2) return false;
                    if ((value.Substring(0, 2) == "0x") && (value.Length % 2 == 0) && value.Substring(2, value.Length - 2).IsHex()) return true;
                    return false;
                case DataTypeConstants.Bit:
                {
                    var q = value.ToLower();
                    if (q == "1" || q == "0") return true;
                    return bool.TryParse(value, out _);
                }
                case DataTypeConstants.Date:
                    return !string.IsNullOrEmpty(this.GetCodeDefault());
                case DataTypeConstants.DateTime:
                    return !string.IsNullOrEmpty(this.GetCodeDefault());
                case DataTypeConstants.DateTime2:
                    return !string.IsNullOrEmpty(this.GetCodeDefault());
                case DataTypeConstants.Decimal:
                    return decimal.TryParse(value, out _);
                case DataTypeConstants.Float:
                    return decimal.TryParse(value, out _);
                case DataTypeConstants.Int:
                    return int.TryParse(value, out _);
                case DataTypeConstants.Money:
                    return long.TryParse(value, out _);
                case DataTypeConstants.Char:
                case DataTypeConstants.NChar:
                case DataTypeConstants.NText:
                case DataTypeConstants.NVarChar:
                case DataTypeConstants.Text:
                case DataTypeConstants.VarChar:
                    return true;
                case DataTypeConstants.Real:
                    return decimal.TryParse(value, out _);
                case DataTypeConstants.SmallDateTime:
                    return !string.IsNullOrEmpty(this.GetCodeDefault());
                case DataTypeConstants.SmallInt:
                    return Int16.TryParse(value, out _);
                case DataTypeConstants.SmallMoney:
                    return int.TryParse(value, out _);
                case DataTypeConstants.Time:
                    return !string.IsNullOrEmpty(this.GetCodeDefault());
                case DataTypeConstants.TinyInt:
                    return byte.TryParse(value, out byte _);
                case DataTypeConstants.UniqueIdentifier:
                {
                    if (value.ToLower() == "newid") return true;
                    try
                    {
                        var v = new Guid(value);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                case DataTypeConstants.Timestamp:
                case DataTypeConstants.Structured:
                case DataTypeConstants.DateTimeOffset:
                case DataTypeConstants.Udt:
                case DataTypeConstants.Variant:
                case DataTypeConstants.Xml:
                    return false;
            }

            return false;
        }

        public virtual string GetCodeType(bool allowNullable)
        {
            return GetCodeType(allowNullable, false);
        }

        public virtual string GetCodeType(bool allowNullable, bool forceNull)
        {
            var retval = string.Empty;
            if (StringHelper.Match(this.DataType.ToString(), "bigint", true))
                retval = "long";
            else if (StringHelper.Match(this.DataType.ToString(), "binary", true))
                return "System.Byte[]";
            else if (StringHelper.Match(this.DataType.ToString(), "bit", true))
                retval = "bool";
            else if (StringHelper.Match(this.DataType.ToString(), "char", true))
                return "string";
            else if (StringHelper.Match(this.DataType.ToString(), "datetime", true))
                retval = "DateTime";
            else if (StringHelper.Match(this.DataType.ToString(), "datetime2", true))
                retval = "DateTime";
            else if (StringHelper.Match(this.DataType.ToString(), "date", true))
                retval = "DateTime";
            else if (StringHelper.Match(this.DataType.ToString(), "time", true))
                retval = "TimeSpan";
            else if (StringHelper.Match(this.DataType.ToString(), "datetimeoffset", true))
                retval = "DateTimeOffset";
            else if (StringHelper.Match(this.DataType.ToString(), "decimal", true))
                retval = "decimal";
            else if (StringHelper.Match(this.DataType.ToString(), "float", true))
                retval = "double";
            else if (StringHelper.Match(this.DataType.ToString(), "image", true))
                return "System.Byte[]";
            else if (StringHelper.Match(this.DataType.ToString(), "int", true))
                retval = "int";
            else if (StringHelper.Match(this.DataType.ToString(), "money", true))
                retval = "decimal";
            else if (StringHelper.Match(this.DataType.ToString(), "nchar", true))
                return "string";
            else if (StringHelper.Match(this.DataType.ToString(), "ntext", true))
                return "string";
            else if (StringHelper.Match(this.DataType.ToString(), "numeric", true))
                retval = "decimal";
            else if (StringHelper.Match(this.DataType.ToString(), "nvarchar", true))
                return "string";
            else if (StringHelper.Match(this.DataType.ToString(), "real", true))
                retval = "System.Single";
            else if (StringHelper.Match(this.DataType.ToString(), "smalldatetime", true))
                retval = "DateTime";
            else if (StringHelper.Match(this.DataType.ToString(), "smallint", true))
                retval = "short";
            else if (StringHelper.Match(this.DataType.ToString(), "smallmoney", true))
                retval = "decimal";
            else if (StringHelper.Match(this.DataType.ToString(), "variant", true))
                retval = "object";
            else if (StringHelper.Match(this.DataType.ToString(), "text", true))
                return "string";
            else if (StringHelper.Match(this.DataType.ToString(), "tinyint", true))
                retval = "byte";
            else if (StringHelper.Match(this.DataType.ToString(), "uniqueidentifier", true))
                retval = "System.Guid";
            else if (StringHelper.Match(this.DataType.ToString(), "varbinary", true))
                return "System.Byte[]";
            else if (StringHelper.Match(this.DataType.ToString(), "varchar", true))
                return "string";
            else if (StringHelper.Match(this.DataType.ToString(), "timestamp", true))
                return "System.Byte[]";
            else if (StringHelper.Match(this.DataType.ToString(), "xml", true))
                return "string";
            else
                throw new Exception("Cannot Map Sql Value To C# Value");

            if (allowNullable && (this.Nullable || forceNull))
                retval += "?";

            return retval;

        }

        public override string ToString() => this.Name;

        #endregion

        #region Properties

        public override bool IsIndexed
        {
            get { return (base.IsIndexed || base.IsPrimaryKey) && !base.IsCalculated; }
            set { base.IsIndexed = value; }
        }

        public override bool IsUnique
        {
            get { return (base.IsUnique || base.IsPrimaryKey) && !base.IsCalculated; }
            set { base.IsUnique = value; }
        }

        public override string Default
        {
            get
            {
                if (this.IsCalculated) return string.Empty;
                return base.Default;
            }
            set { base.Default = value; }
        }

        public override IdentityTypeConstants Identity
        {
            get
            {
                if (!this.DataType.SupportsIdentity()) return IdentityTypeConstants.None;
                else if (this.IsCalculated) return IdentityTypeConstants.None;
                else return base.Identity;
            }
            set { base.Identity = value; }
        }

        public override bool IsPrimaryKey
        {
            get
            {
                if (this.IsCalculated) return false;
                else return base.IsPrimaryKey;
            }
            set { base.IsPrimaryKey = value; }
        }

        public override bool Nullable
        {
            get
            {
                if (this.IsCalculated) return true;
                else if (this.IsUnique) return false;
                else if (this.DataType == DataTypeConstants.Variant) return false;
                else return base.Nullable && !this.IsPrimaryKey;
            }
            set { base.Nullable = value; }
        }

        public override int Length
        {
            get
            {
                var retval = this.DataType.GetPredefinedSize();
                if (retval == -1) retval = base.Length;
                return retval;
            }
            set { base.Length = value; }
        }

        public override int Scale
        {
            get
            {
                var retval = this.DataType.GetPredefinedScale();
                if (retval == -1) retval = base.Scale;
                return retval;
            }
            set { base.Scale = value; }
        }

        public override DataTypeConstants DataType
        {
            get { return base.DataType; }
            set
            {
                try
                {
                    base.DataType = value;

                    if (this.Entity != null && this.Entity.nHydrateModel != null)
                    {
                        //Reset length if necessary
                        if (!this.Entity.nHydrateModel.IsLoading)
                        {
                            base.Length = value.GetDefaultSize(base.Length);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        #endregion

        #region OnDeleting
        protected override void OnDeleting()
        {
            //This will remove the managed index before removing this field
            this.IsIndexed = false;

            //Remove from relation mapped collection
            if (this.Entity != null &&
                    this.Entity.nHydrateModel != null &&
                    this.Entity.nHydrateModel.RelationFields != null)
            {
                var relationFieldList = this.Entity.nHydrateModel.RelationFields.Where(x => x.SourceFieldId == this.Id || x.TargetFieldId == this.Id).ToList();
                foreach (var item in relationFieldList)
                {
                    var relation = this.Entity.nHydrateModel.AllRelations.FirstOrDefault(x => x.Id == item.RelationID);
                    if (relation != null)
                        relation.Delete();
                }

                //This is redundant. Should be zero. Used for debugging.
                var count1 = this.Entity.nHydrateModel.RelationFields.Remove(x => x.SourceFieldId == this.Id || x.TargetFieldId == this.Id);
            }

            base.OnDeleting();

        }
        #endregion

        #region IContainerParent Members

        DslModeling.ModelElement IContainerParent.ContainerParent => this.Entity;

        #endregion
    }

    partial class FieldBase
    {
        partial class NamePropertyHandler
        {
            protected override void OnValueChanged(FieldBase element, string oldValue, string newValue)
            {
                //If not laoding then parse the name for the data type
                var hasChanged = false;
                if (element.Entity != null && !element.Entity.nHydrateModel.IsLoading)
                {
                    if (!string.IsNullOrEmpty(newValue))
                    {
                        var arr = newValue.Split(':');
                        if (arr.Length == 2)
                        {
                            var typearr = arr[1].Split(' ');
                            var d = Extensions.GetDataTypeFromName(typearr[0]);
                            if (d != null)
                            {
                                if (typearr.Length == 2)
                                {
                                    if (int.TryParse(typearr[1], out var len))
                                    {
                                        element.DataType = d.Value;
                                        element.Length = len;
                                        newValue = arr[0];
                                        hasChanged = true;
                                    }
                                    else
                                    {
                                        throw new Exception("Unrecognized data type! Valid format is 'Name:Datatype length'");
                                    }
                                }
                                else
                                {
                                    element.DataType = d.Value;
                                    newValue = arr[0];
                                    hasChanged = true;
                                }

                            }
                            else
                            {
                                throw new Exception("Unrecognized data type! Valid format is 'Name:Datatype length'");
                            }
                        }
                    }
                }

                base.OnValueChanged(element, oldValue, newValue);

                //Reset after we set datatype
                if (hasChanged)
                    element.Name = newValue;
                else
                    base.OnValueChanged(element, oldValue, newValue);
            }
        }

        partial class LengthPropertyHandler
        {
            protected override void OnValueChanged(FieldBase element, int oldValue, int newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

                //this will trigger another event
                var v = newValue;
                if (v < 0) v = 0;
                v = element.DataType.ValidateDataTypeMax(v);
                if (newValue != v)
                    element.Length = element.DataType.ValidateDataTypeMax(v);
            }
        }

        partial class ScalePropertyHandler
        {
            protected override void OnValueChanged(FieldBase element, int oldValue, int newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

                //this will trigger another event
                if (newValue < 0) element.Scale = 0;
            }
        }

        partial class IsPrimaryKeyPropertyHandler
        {
            protected override void OnValueChanged(FieldBase element, bool oldValue, bool newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

                try
                {
                    if (element.Entity != null && element.Entity.nHydrateModel != null && !element.Entity.nHydrateModel.IsLoading)
                    {
                        //Processes Index list
                        if (element.IsPrimaryKey) //Must use real property since there is logic there
                        {
                            //This is a PK so determine if there is a key for this and add this field to a new or the existing index
                            var existing = element.Entity.Indexes.FirstOrDefault(x => x.IndexType == IndexTypeConstants.PrimaryKey);
                            if (existing == null)
                            {
                                //The PK index does not exist so create one
                                using (var transaction = element.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                                {
                                    var newIndex = new Index(element.Partition);
                                    newIndex.ParentEntityID = element.Entity.Id;
                                    newIndex.Clustered = true;
                                    element.Entity.Indexes.Add(newIndex);

                                    var newColumn = new IndexColumn(element.Partition);
                                    newColumn.FieldID = element.Id;
                                    newIndex.IndexColumns.Add(newColumn);
                                    newColumn.IsInternal = true;

                                    newIndex.IndexType = IndexTypeConstants.PrimaryKey; //Do this last

                                    transaction.Commit();
                                }
                            }
                            else
                            {
                                //The PK does exist so add this field to it
                                using (var transaction = element.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                                {
                                    if (existing.IndexColumns.Count(x => x.FieldID == element.Id) == 0)
                                    {
                                        existing.IndexType = IndexTypeConstants.User;
                                        var newColumn = new IndexColumn(element.Partition);
                                        newColumn.FieldID = element.Id;
                                        newColumn.IsInternal = true;
                                        existing.IndexColumns.Add(newColumn);

                                        //Just in case there are invalid fields
                                        existing.IndexColumns.Remove(x => x.GetField() == null);

                                        existing.IndexType = IndexTypeConstants.PrimaryKey; //Do this last
                                        transaction.Commit();
                                    }
                                }
                            }

                            //Remove the IsIndex ones if exist
                            Func<Index, bool> where = x => x.IndexType == IndexTypeConstants.IsIndexed &&
                                x.IndexColumns.Count == 1 &&
                                x.IndexColumns.First().FieldID == element.Id &&
                                x.IndexType == IndexTypeConstants.IsIndexed;

                            element.Entity.Indexes.Where(where).ToList().ForEach(x => x.IndexType = IndexTypeConstants.User);
                            element.Entity.Indexes.Remove(where);

                        }
                        else //Remove Index
                        {
                            var existing = element.Entity.Indexes.FirstOrDefault(x => x.IndexType == IndexTypeConstants.PrimaryKey);
                            if (existing != null)
                            {
                                using (var transaction = element.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                                {
                                    existing.IndexType = IndexTypeConstants.User;
                                    existing.IndexColumns.Remove(x => x.FieldID == element.Id);
                                    if (element.Entity.Fields.Count(x => x.IsPrimaryKey) == 0) //No more primary keys
                                        element.Entity.Indexes.Remove(existing);
                                    else
                                        existing.IndexType = IndexTypeConstants.PrimaryKey;

                                    transaction.Commit();
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        partial class IsIndexedPropertyHandler
        {
            protected override void OnValueChanged(FieldBase element, bool oldValue, bool newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

                if (element.Entity != null && element.Entity.nHydrateModel != null && !element.Entity.nHydrateModel.IsLoading)
                {
                    //Processes Index list
                    if (newValue) //element.IsIndexed //Must use real property since there is logic there
                    {
                        //Add an Asc single field index
                        var existing = element.Entity.Indexes.FirstOrDefault(x => x.IndexColumns.Count == 1 && x.IndexColumns.First().FieldID == element.Id && x.IndexColumns.First().Ascending && x.IndexType == IndexTypeConstants.IsIndexed);
                        if (existing == null)
                        {
                            using (var transaction = element.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                            {
                                var newIndex = new Index(element.Partition);
                                newIndex.ParentEntityID = element.Entity.Id;
                                newIndex.IndexType = IndexTypeConstants.IsIndexed;
                                newIndex.Clustered = false;
                                element.Entity.Indexes.Add(newIndex);

                                var newColumn = new IndexColumn(element.Partition);
                                newColumn.FieldID = element.Id;
                                newColumn.IsInternal = true;
                                newIndex.IndexColumns.Add(newColumn);

                                transaction.Commit();
                            }
                        }
                    }
                    else //Remove Index
                    {
                        var existingList = element.Entity.Indexes
                            .Where(x => x.IndexType == IndexTypeConstants.IsIndexed && x.IndexColumns.Count == 1 && x.IndexColumns.First().FieldID == element.Id && x.IndexType == IndexTypeConstants.IsIndexed)
                            .ToList();

                        using (var transaction = element.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                        {
                            while (existingList.Count > 0)
                            {
                                var item = existingList.First();
                                item.IndexType = IndexTypeConstants.User;
                                element.Entity.Indexes.Remove(item);
                                existingList.Remove(item);
                            }

                            transaction.Commit();
                        }
                    }
                }
            }
        }

        partial class DataTypePropertyHandler
        {
            protected override void OnValueChanged(FieldBase element, DataTypeConstants oldValue, DataTypeConstants newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

                if (element.Entity != null && element.Entity.nHydrateModel != null)
                {
                    if (!element.Entity.nHydrateModel.IsLoading)
                    {
                        element.Length = newValue.GetDefaultSize(element.Length);
                    }
                }
            }
        }

    }

}