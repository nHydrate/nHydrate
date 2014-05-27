//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Data.Objects.DataClasses;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data.Objects;
using System.Text;
using Acme.Northwind.EFDAL;
using nHydrate.EFCore.DataAccess;
using nHydrate.EFCore.EventArgs;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Data.Linq;

namespace Acme.Northwind.EFDAL.Entity
{
	/// <summary>
	/// The collection to hold 'ProductsbyCategory' entities
	/// </summary>
	[EdmEntityTypeAttribute(NamespaceName = "Acme.Northwind.EFDAL.Entity", Name = "ProductsbyCategory")]
	[Serializable]
	[DataContractAttribute(IsReference = true)]
	[nHydrate.EFCore.Attributes.FieldNameConstantsAttribute(typeof(Acme.Northwind.EFDAL.Entity.ProductsbyCategory.FieldNameConstants))]
	[System.ComponentModel.ImmutableObject(true)]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial class ProductsbyCategory : nHydrate.EFCore.DataAccess.NHEntityObject, nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject, Acme.Northwind.EFDAL.Interfaces.Entity.IProductsbyCategory, System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.IProductsbyCategory>
	{
		#region FieldNameConstants Enumeration

		/// <summary>
		/// Enumeration to define each property that maps to a database field for the 'ProductsbyCategory' customView.
		/// </summary>
		public enum FieldNameConstants
		{
			/// <summary>
			/// Field mapping for the 'CategoryName' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'CategoryName' property")]
			CategoryName,
			/// <summary>
			/// Field mapping for the 'Discontinued' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'Discontinued' property")]
			Discontinued,
			/// <summary>
			/// Field mapping for the 'ProductName' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'ProductName' property")]
			ProductName,
			/// <summary>
			/// Field mapping for the 'QuantityPerUnit' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'QuantityPerUnit' property")]
			QuantityPerUnit,
			/// <summary>
			/// Field mapping for the 'UnitsInStock' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'UnitsInStock' property")]
			UnitsInStock,
		}
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Acme.Northwind.EFDAL.Entity.ProductsbyCategory class
		/// </summary>
		protected internal ProductsbyCategory()
		{
		}

		#endregion

		#region Properties

		[System.ComponentModel.Browsable(false)]
		[EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
		[DataMemberAttribute()]
		[System.ComponentModel.DisplayName("pk")]
		[System.ComponentModel.ReadOnly(true)]
		[System.Diagnostics.DebuggerNonUserCode]
		private string pk
		{
			get { return _pk; }
			set { _pk = value; }
		}
		private string _pk;

		/// <summary>
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
		[DataMemberAttribute()]
		[System.ComponentModel.DisplayName("CategoryName")]
		[System.ComponentModel.ReadOnly(true)]
		[System.Diagnostics.DebuggerNonUserCode]
		public virtual string CategoryName
		{
			get { return _categoryName; }
			set
			{
				if ((value != null) && (value.Length > GetMaxLength(FieldNameConstants.CategoryName))) throw new Exception(string.Format(GlobalValues.ERROR_DATA_TOO_BIG, value, "ProductsbyCategory.CategoryName", GetMaxLength(FieldNameConstants.CategoryName)));
				var eventArg = new nHydrate.EFCore.EventArgs.ChangingEventArgs<string>(value, "CategoryName");
				//this.OnCategoryNameChanging(eventArg);
				if (eventArg.Cancel) return;
				ReportPropertyChanging("CategoryName");
				_categoryName = eventArg.Value;
				ReportPropertyChanged("CategoryName");
				//this.OnCategoryNameChanged(eventArg);
			}
		}

		/// <summary>
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
		[DataMemberAttribute()]
		[System.ComponentModel.DisplayName("Discontinued")]
		[System.ComponentModel.ReadOnly(true)]
		[System.Diagnostics.DebuggerNonUserCode]
		public virtual bool Discontinued
		{
			get { return _discontinued; }
			set
			{
				var eventArg = new nHydrate.EFCore.EventArgs.ChangingEventArgs<bool>(value, "Discontinued");
				//this.OnDiscontinuedChanging(eventArg);
				if (eventArg.Cancel) return;
				ReportPropertyChanging("Discontinued");
				_discontinued = eventArg.Value;
				ReportPropertyChanged("Discontinued");
				//this.OnDiscontinuedChanged(eventArg);
			}
		}

		/// <summary>
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
		[DataMemberAttribute()]
		[System.ComponentModel.DisplayName("ProductName")]
		[System.ComponentModel.ReadOnly(true)]
		[System.Diagnostics.DebuggerNonUserCode]
		public virtual string ProductName
		{
			get { return _productName; }
			set
			{
				if ((value != null) && (value.Length > GetMaxLength(FieldNameConstants.ProductName))) throw new Exception(string.Format(GlobalValues.ERROR_DATA_TOO_BIG, value, "ProductsbyCategory.ProductName", GetMaxLength(FieldNameConstants.ProductName)));
				var eventArg = new nHydrate.EFCore.EventArgs.ChangingEventArgs<string>(value, "ProductName");
				//this.OnProductNameChanging(eventArg);
				if (eventArg.Cancel) return;
				ReportPropertyChanging("ProductName");
				_productName = eventArg.Value;
				ReportPropertyChanged("ProductName");
				//this.OnProductNameChanged(eventArg);
			}
		}

		/// <summary>
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		[System.ComponentModel.DisplayName("QuantityPerUnit")]
		[System.ComponentModel.ReadOnly(true)]
		[System.Diagnostics.DebuggerNonUserCode]
		public virtual string QuantityPerUnit
		{
			get { return _quantityPerUnit; }
			set
			{
				if ((value != null) && (value.Length > GetMaxLength(FieldNameConstants.QuantityPerUnit))) throw new Exception(string.Format(GlobalValues.ERROR_DATA_TOO_BIG, value, "ProductsbyCategory.QuantityPerUnit", GetMaxLength(FieldNameConstants.QuantityPerUnit)));
				var eventArg = new nHydrate.EFCore.EventArgs.ChangingEventArgs<string>(value, "QuantityPerUnit");
				//this.OnQuantityPerUnitChanging(eventArg);
				if (eventArg.Cancel) return;
				ReportPropertyChanging("QuantityPerUnit");
				_quantityPerUnit = eventArg.Value;
				ReportPropertyChanged("QuantityPerUnit");
				//this.OnQuantityPerUnitChanged(eventArg);
			}
		}

		/// <summary>
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		[System.ComponentModel.DisplayName("UnitsInStock")]
		[System.ComponentModel.ReadOnly(true)]
		[System.Diagnostics.DebuggerNonUserCode]
		public virtual short? UnitsInStock
		{
			get { return _unitsInStock; }
			set
			{
				var eventArg = new nHydrate.EFCore.EventArgs.ChangingEventArgs<short?>(value, "UnitsInStock");
				//this.OnUnitsInStockChanging(eventArg);
				if (eventArg.Cancel) return;
				ReportPropertyChanging("UnitsInStock");
				_unitsInStock = eventArg.Value;
				ReportPropertyChanged("UnitsInStock");
				//this.OnUnitsInStockChanged(eventArg);
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// The internal reference variable for the 'CategoryName' property
		/// </summary>
		protected string _categoryName;

		/// <summary>
		/// The internal reference variable for the 'Discontinued' property
		/// </summary>
		protected bool _discontinued;

		/// <summary>
		/// The internal reference variable for the 'ProductName' property
		/// </summary>
		protected string _productName;

		/// <summary>
		/// The internal reference variable for the 'QuantityPerUnit' property
		/// </summary>
		protected string _quantityPerUnit;

		/// <summary>
		/// The internal reference variable for the 'UnitsInStock' property
		/// </summary>
		protected short? _unitsInStock;

		#endregion

		#region GetMaxLength

		/// <summary>
		/// Gets the maximum size of the field value.
		/// </summary>
		public static int GetMaxLength(FieldNameConstants field)
		{
			switch (field)
			{
				case FieldNameConstants.CategoryName:
					return 30;
				case FieldNameConstants.Discontinued:
					return 0;
				case FieldNameConstants.ProductName:
					return 80;
				case FieldNameConstants.QuantityPerUnit:
					return 40;
				case FieldNameConstants.UnitsInStock:
					return 0;
			}
			return 0;
		}

		int nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetMaxLength(Enum field)
		{
			return GetMaxLength((FieldNameConstants)field);
		}

		#endregion

		#region GetFieldType

		/// <summary>
		/// Gets the system type of a field on this object
		/// </summary>
		public static System.Type GetFieldType(FieldNameConstants field)
		{
			var o = new ProductsbyCategory();
			return ((nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject)o).GetFieldType(field);
		}

		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldType(Enum field)
		{
			if (field.GetType() != typeof(FieldNameConstants))
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type '" + this.GetType().ToString() + ".FieldNameConstants'.");

			switch ((FieldNameConstants)field)
			{
				case FieldNameConstants.CategoryName: return typeof(string);
				case FieldNameConstants.Discontinued: return typeof(bool);
				case FieldNameConstants.ProductName: return typeof(string);
				case FieldNameConstants.QuantityPerUnit: return typeof(string);
				case FieldNameConstants.UnitsInStock: return typeof(short?);
			}
			return null;
		}

		#endregion

		#region GetFieldNameConstants

		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldNameConstants()
		{
			return typeof(FieldNameConstants);
		}

		#endregion

		#region Get/Set Value

		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(System.Enum field)
		{
			return ((nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject)this).GetValue(field, null);
		}

		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(System.Enum field, object defaultValue)
		{
			if (field.GetType() != typeof(FieldNameConstants))
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type '" + this.GetType().ToString() + ".FieldNameConstants'.");
			return this.GetValue((FieldNameConstants)field, defaultValue);
		}

		#endregion

		#region PrimaryKey

		/// <summary>
		/// Hold the primary key for this object
		/// </summary>
		protected nHydrate.EFCore.DataAccess.IPrimaryKey _primaryKey = null;
		nHydrate.EFCore.DataAccess.IPrimaryKey nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.PrimaryKey
		{
			get { return null; }
		}

		#endregion

		#region IsParented

		/// <summary>
		/// Determines if this object is part of a collection or is detached
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public virtual bool IsParented
		{
			get { return (this.EntityState != System.Data.EntityState.Detached); }
		}

		#endregion

		#region IsEquivalent

		/// <summary>
		/// Determines if all of the fields for the specified object exactly matches the current object.
		/// </summary>
		/// <param name="item">The object to compare</param>
		public override bool IsEquivalent(nHydrate.EFCore.DataAccess.INHEntityObject item)
		{
			return ((System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.IProductsbyCategory>)this).Equals(item as Acme.Northwind.EFDAL.Interfaces.Entity.IProductsbyCategory);
		}

		#endregion

		#region Equals
		bool System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.IProductsbyCategory>.Equals(Acme.Northwind.EFDAL.Interfaces.Entity.IProductsbyCategory other)
		{
			return this.Equals(other);
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		public override bool Equals(object obj)
		{
			var other = obj as Acme.Northwind.EFDAL.Entity.ProductsbyCategory;
			if (other == null) return false;
			return (
				other.CategoryName == this.CategoryName &&
				other.Discontinued == this.Discontinued &&
				other.ProductName == this.ProductName &&
				other.QuantityPerUnit == this.QuantityPerUnit &&
				other.UnitsInStock == this.UnitsInStock
				);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion

		#region GetValue

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public object GetValue(FieldNameConstants field)
		{
			return GetValue(field, null);
		}

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public object GetValue(FieldNameConstants field, object defaultValue)
		{
			if (field == FieldNameConstants.CategoryName)
				return this.CategoryName;
			if (field == FieldNameConstants.Discontinued)
				return this.Discontinued;
			if (field == FieldNameConstants.ProductName)
				return this.ProductName;
			if (field == FieldNameConstants.QuantityPerUnit)
				return ((this.QuantityPerUnit == null) ? defaultValue : this.QuantityPerUnit);
			if (field == FieldNameConstants.UnitsInStock)
				return ((this.UnitsInStock == null) ? defaultValue : this.UnitsInStock);
			throw new Exception("Field '" + field.ToString() + "' not found!");
		}

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public int GetInteger(Acme.Northwind.EFDAL.Entity.ProductsbyCategory.FieldNameConstants field)
		{
			return this.GetInteger(field, int.MinValue);
		}

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public int GetInteger(Acme.Northwind.EFDAL.Entity.ProductsbyCategory.FieldNameConstants field, int defaultValue)
		{
			var o = this.GetValue(field, defaultValue);
			if (o is string)
			{
				int a;
				if (int.TryParse((string)o, out a))
					return a;
				else
					throw new System.InvalidCastException();
			}
			else
			{
				return (int)o;
			}
		}

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public double GetDouble(Acme.Northwind.EFDAL.Entity.ProductsbyCategory.FieldNameConstants field)
		{
			return this.GetDouble(field, double.MinValue);
		}

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public double GetDouble(Acme.Northwind.EFDAL.Entity.ProductsbyCategory.FieldNameConstants field, double defaultValue)
		{
			var o = this.GetValue(field, defaultValue);
			if (o is string)
			{
				double a;
				if (double.TryParse((string)o, out a))
					return a;
				else
					throw new System.InvalidCastException();
			}
			else
			{
				return (double)o;
			}
		}

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public System.DateTime GetDateTime(Acme.Northwind.EFDAL.Entity.ProductsbyCategory.FieldNameConstants field)
		{
			return this.GetDateTime(field, System.DateTime.MinValue);
		}

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public System.DateTime GetDateTime(Acme.Northwind.EFDAL.Entity.ProductsbyCategory.FieldNameConstants field, System.DateTime defaultValue)
		{
			var o = this.GetValue(field, defaultValue);
			if (o is string)
			{
				System.DateTime a;
				if (System.DateTime.TryParse((string)o, out a))
					return a;
				else
					throw new System.InvalidCastException();
			}
			else
			{
				return (System.DateTime)o;
			}
		}

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public string GetString(Acme.Northwind.EFDAL.Entity.ProductsbyCategory.FieldNameConstants field)
		{
			return this.GetString(field, string.Empty);
		}

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public string GetString(Acme.Northwind.EFDAL.Entity.ProductsbyCategory.FieldNameConstants field, string defaultValue)
		{
			var o = this.GetValue(field, defaultValue);
			if (o is string)
			{
				return (string)o;
			}
			else
			{
				return o.ToString();
			}
		}

		#endregion

		#region GetDatabaseFieldName

		/// <summary>
		/// Returns the actual database name of the specified field.
		/// </summary>
		internal static string GetDatabaseFieldName(ProductsbyCategory.FieldNameConstants field)
		{
			return GetDatabaseFieldName(field.ToString());
		}

		/// <summary>
		/// Returns the actual database name of the specified field.
		/// </summary>
		internal static string GetDatabaseFieldName(string field)
		{
			switch (field)
			{
				case "CategoryName": return "CategoryName";
				case "Discontinued": return "Discontinued";
				case "ProductName": return "ProductName";
				case "QuantityPerUnit": return "QuantityPerUnit";
				case "UnitsInStock": return "UnitsInStock";
			}
			return string.Empty;
		}

		#endregion

	}

}

