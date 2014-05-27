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
	[EdmComplexTypeAttribute(NamespaceName = "Acme.Northwind.EFDAL.Entity", Name = "EmployeeSalesbyCountry")]
	[DataContractAttribute(IsReference = true)]
	[Serializable]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial class EmployeeSalesbyCountry : nHydrate.EFCore.DataAccess.NHComplexObject, nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject, Acme.Northwind.EFDAL.Interfaces.Entity.IEmployeeSalesbyCountry, System.ICloneable, System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.IEmployeeSalesbyCountry>
	{
		#region FieldNameConstants Enumeration

		/// <summary>
		/// Enumeration to define each property that maps to a database field for the 'EmployeeSalesbyCountry' table.
		/// </summary>
		public enum FieldNameConstants
		{
			/// <summary>
			/// Field mapping for the 'Country' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'Country' property")]
			Country,
			/// <summary>
			/// Field mapping for the 'FirstName' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'FirstName' property")]
			FirstName,
			/// <summary>
			/// Field mapping for the 'LastName' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'LastName' property")]
			LastName,
			/// <summary>
			/// Field mapping for the 'OrderID' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'OrderID' property")]
			OrderID,
			/// <summary>
			/// Field mapping for the 'SaleAmount' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'SaleAmount' property")]
			SaleAmount,
			/// <summary>
			/// Field mapping for the 'ShippedDate' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'ShippedDate' property")]
			ShippedDate,
		}
		#endregion

		#region Primitive Properties

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public string Country
		{
			get { return _country; }
			protected set
			{
				_country = StructuralObject.SetValidValue(value, true);
			}
		}
		private string _country;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public string FirstName
		{
			get { return _firstName; }
			protected set
			{
				_firstName = StructuralObject.SetValidValue(value, true);
			}
		}
		private string _firstName;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public string LastName
		{
			get { return _lastName; }
			protected set
			{
				_lastName = StructuralObject.SetValidValue(value, true);
			}
		}
		private string _lastName;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public int? OrderID
		{
			get { return _orderID; }
			protected set
			{
				_orderID = StructuralObject.SetValidValue(value);
			}
		}
		private int? _orderID;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public decimal? SaleAmount
		{
			get { return _saleAmount; }
			protected set
			{
				_saleAmount = StructuralObject.SetValidValue(value);
			}
		}
		private decimal? _saleAmount;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public DateTime? ShippedDate
		{
			get { return _shippedDate; }
			protected set
			{
				_shippedDate = StructuralObject.SetValidValue(value);
			}
		}
		private DateTime? _shippedDate;

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
			if (field == FieldNameConstants.Country)
				return ((this.Country == null) ? defaultValue : this.Country);
			if (field == FieldNameConstants.FirstName)
				return ((this.FirstName == null) ? defaultValue : this.FirstName);
			if (field == FieldNameConstants.LastName)
				return ((this.LastName == null) ? defaultValue : this.LastName);
			if (field == FieldNameConstants.OrderID)
				return ((this.OrderID == null) ? defaultValue : this.OrderID);
			if (field == FieldNameConstants.SaleAmount)
				return ((this.SaleAmount == null) ? defaultValue : this.SaleAmount);
			if (field == FieldNameConstants.ShippedDate)
				return ((this.ShippedDate == null) ? defaultValue : this.ShippedDate);
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
			var newItem = new EmployeeSalesbyCountry();
			newItem._country = this._country;
			newItem._firstName = this._firstName;
			newItem._lastName = this._lastName;
			newItem._orderID = this._orderID;
			newItem._saleAmount = this._saleAmount;
			newItem._shippedDate = this._shippedDate;
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
				case FieldNameConstants.Country:
					return 50;
				case FieldNameConstants.FirstName:
					return 50;
				case FieldNameConstants.LastName:
					return 50;
				case FieldNameConstants.OrderID:
					return 0;
				case FieldNameConstants.SaleAmount:
					return 0;
				case FieldNameConstants.ShippedDate:
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
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type 'Acme.Northwind.EFDAL.Entity.EmployeeSalesbyCountry.FieldNameConstants'.");

			switch ((FieldNameConstants)field)
			{
				case FieldNameConstants.Country: return typeof(string);
				case FieldNameConstants.FirstName: return typeof(string);
				case FieldNameConstants.LastName: return typeof(string);
				case FieldNameConstants.OrderID: return typeof(int?);
				case FieldNameConstants.SaleAmount: return typeof(decimal?);
				case FieldNameConstants.ShippedDate: return typeof(DateTime?);
			}
			return null;
		}

		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldType(Enum field)
		{
			if (field.GetType() != typeof(FieldNameConstants))
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type 'Acme.Northwind.EFDAL.Entity.EmployeeSalesbyCountry.FieldNameConstants'.");

			return GetFieldType((Acme.Northwind.EFDAL.Entity.EmployeeSalesbyCountry.FieldNameConstants)field);
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
			return ((System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.IEmployeeSalesbyCountry>)this).Equals(item as Acme.Northwind.EFDAL.Interfaces.Entity.IEmployeeSalesbyCountry);
		}

		#endregion

		#region Equals
		bool System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.IEmployeeSalesbyCountry>.Equals(Acme.Northwind.EFDAL.Interfaces.Entity.IEmployeeSalesbyCountry other)
		{
			return this.Equals(other);
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		public override bool Equals(object obj)
		{
			var other = obj as Acme.Northwind.EFDAL.Entity.EmployeeSalesbyCountry;
			if (other == null) return false;
			return (
				other.Country == this.Country &&
				other.FirstName == this.FirstName &&
				other.LastName == this.LastName &&
				other.OrderID == this.OrderID &&
				other.SaleAmount == this.SaleAmount &&
				other.ShippedDate == this.ShippedDate
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
