using System;
using System.Linq;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Acme.Northwind.EFDAL.Entity;
using System.Linq.Expressions;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	/// <summary>
	/// An object based on a stored procedure
	/// </summary>
	[EdmComplexTypeAttribute(NamespaceName = "Acme.Northwind.EFDAL.Entity", Name = "CustOrdersDetail")]
	[DataContractAttribute(IsReference = true)]
	[Serializable]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial class CustOrdersDetail : nHydrate.EFCore.DataAccess.NHComplexObject, nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject, Acme.Northwind.EFDAL.Interfaces.Entity.ICustOrdersDetail, System.ICloneable, System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.ICustOrdersDetail>
	{
		#region FieldNameConstants Enumeration

		/// <summary>
		/// Enumeration to define each property that maps to a database field for the 'CustOrdersDetail' table.
		/// </summary>
		public enum FieldNameConstants
		{
			/// <summary>
			/// Field mapping for the 'Discount' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'Discount' property")]
			Discount,
			/// <summary>
			/// Field mapping for the 'ExtendedPrice' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'ExtendedPrice' property")]
			ExtendedPrice,
			/// <summary>
			/// Field mapping for the 'ProductName' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'ProductName' property")]
			ProductName,
			/// <summary>
			/// Field mapping for the 'Quantity' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'Quantity' property")]
			Quantity,
			/// <summary>
			/// Field mapping for the 'UnitPrice' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'UnitPrice' property")]
			UnitPrice,
		}
		#endregion

		#region Primitive Properties

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public int? Discount
		{
			get { return _discount; }
			protected set
			{
				_discount = StructuralObject.SetValidValue(value);
			}
		}
		private int? _discount;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public decimal? ExtendedPrice
		{
			get { return _extendedPrice; }
			protected set
			{
				_extendedPrice = StructuralObject.SetValidValue(value);
			}
		}
		private decimal? _extendedPrice;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public string ProductName
		{
			get { return _productName; }
			protected set
			{
				_productName = StructuralObject.SetValidValue(value, true);
			}
		}
		private string _productName;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public short? Quantity
		{
			get { return _quantity; }
			protected set
			{
				_quantity = StructuralObject.SetValidValue(value);
			}
		}
		private short? _quantity;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public decimal? UnitPrice
		{
			get { return _unitPrice; }
			protected set
			{
				_unitPrice = StructuralObject.SetValidValue(value);
			}
		}
		private decimal? _unitPrice;

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
			if (field == FieldNameConstants.Discount)
				return ((this.Discount == null) ? defaultValue : this.Discount);
			if (field == FieldNameConstants.ExtendedPrice)
				return ((this.ExtendedPrice == null) ? defaultValue : this.ExtendedPrice);
			if (field == FieldNameConstants.ProductName)
				return ((this.ProductName == null) ? defaultValue : this.ProductName);
			if (field == FieldNameConstants.Quantity)
				return ((this.Quantity == null) ? defaultValue : this.Quantity);
			if (field == FieldNameConstants.UnitPrice)
				return ((this.UnitPrice == null) ? defaultValue : this.UnitPrice);
			throw new Exception("Field '" + field.ToString() + "' not found!");
		}

		#endregion

		#region Clone

		/// <summary>
		/// Creates a shallow copy of this object of all simple properties
		/// </summary>
		/// <returns></returns>
		public virtual object Clone()
		{
			var newItem = new CustOrdersDetail();
			newItem._discount = this._discount;
			newItem._extendedPrice = this._extendedPrice;
			newItem._productName = this._productName;
			newItem._quantity = this._quantity;
			newItem._unitPrice = this._unitPrice;
			return newItem;
		}

		#endregion

		#region GetMaxLength

		/// <summary>
		/// Gets the maximum size of the field value.
		/// </summary>
		public static int GetMaxLength(FieldNameConstants field)
		{
			switch (field)
			{
				case FieldNameConstants.Discount:
					return 0;
				case FieldNameConstants.ExtendedPrice:
					return 0;
				case FieldNameConstants.ProductName:
					return 50;
				case FieldNameConstants.Quantity:
					return 0;
				case FieldNameConstants.UnitPrice:
					return 0;
			}
			return 0;
		}

		int nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetMaxLength(Enum field)
		{
			return GetMaxLength((FieldNameConstants)field);
		}

		#endregion

		#region GetFieldNameConstants

		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldNameConstants()
		{
			return typeof(FieldNameConstants);
		}

		#endregion

		#region GetFieldType

		/// <summary>
		/// Gets the system type of a field on this object
		/// </summary>
		public static System.Type GetFieldType(FieldNameConstants field)
		{
			if (field.GetType() != typeof(FieldNameConstants))
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type 'Acme.Northwind.EFDAL.Entity.CustOrdersDetail.FieldNameConstants'.");

			switch ((FieldNameConstants)field)
			{
				case FieldNameConstants.Discount: return typeof(int?);
				case FieldNameConstants.ExtendedPrice: return typeof(decimal?);
				case FieldNameConstants.ProductName: return typeof(string);
				case FieldNameConstants.Quantity: return typeof(short?);
				case FieldNameConstants.UnitPrice: return typeof(decimal?);
			}
			return null;
		}

		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldType(Enum field)
		{
			if (field.GetType() != typeof(FieldNameConstants))
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type 'Acme.Northwind.EFDAL.Entity.CustOrdersDetail.FieldNameConstants'.");

			return GetFieldType((Acme.Northwind.EFDAL.Entity.CustOrdersDetail.FieldNameConstants)field);
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

		#region IsEquivalent

		/// <summary>
		/// Determines if all of the fields for the specified object exactly matches the current object.
		/// </summary>
		/// <param name="item">The object to compare</param>
		public override bool IsEquivalent(nHydrate.EFCore.DataAccess.INHEntityObject item)
		{
			return ((System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.ICustOrdersDetail>)this).Equals(item as Acme.Northwind.EFDAL.Interfaces.Entity.ICustOrdersDetail);
		}

		#endregion

		#region Equals
		bool System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.ICustOrdersDetail>.Equals(Acme.Northwind.EFDAL.Interfaces.Entity.ICustOrdersDetail other)
		{
			return this.Equals(other);
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		public override bool Equals(object obj)
		{
			var other = obj as Acme.Northwind.EFDAL.Entity.CustOrdersDetail;
			if (other == null) return false;
			return (
				other.Discount == this.Discount &&
				other.ExtendedPrice == this.ExtendedPrice &&
				other.ProductName == this.ProductName &&
				other.Quantity == this.Quantity &&
				other.UnitPrice == this.UnitPrice
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

	}
}
