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
	[EdmComplexTypeAttribute(NamespaceName = "Acme.Northwind.EFDAL.Entity", Name = "SalesbyYear")]
	[DataContractAttribute(IsReference = true)]
	[Serializable]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial class SalesbyYear : nHydrate.EFCore.DataAccess.NHComplexObject, nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject, Acme.Northwind.EFDAL.Interfaces.Entity.ISalesbyYear, System.ICloneable, System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.ISalesbyYear>
	{
		#region FieldNameConstants Enumeration

		/// <summary>
		/// Enumeration to define each property that maps to a database field for the 'SalesbyYear' table.
		/// </summary>
		public enum FieldNameConstants
		{
			/// <summary>
			/// Field mapping for the 'OrderID' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'OrderID' property")]
			OrderID,
			/// <summary>
			/// Field mapping for the 'ShippedDate' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'ShippedDate' property")]
			ShippedDate,
			/// <summary>
			/// Field mapping for the 'Subtotal' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'Subtotal' property")]
			Subtotal,
			/// <summary>
			/// Field mapping for the 'Year' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'Year' property")]
			Year,
		}
		#endregion

		#region Primitive Properties

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
		public DateTime? ShippedDate
		{
			get { return _shippedDate; }
			protected set
			{
				_shippedDate = StructuralObject.SetValidValue(value);
			}
		}
		private DateTime? _shippedDate;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public decimal? Subtotal
		{
			get { return _subtotal; }
			protected set
			{
				_subtotal = StructuralObject.SetValidValue(value);
			}
		}
		private decimal? _subtotal;

		/// <summary>
		/// 
		/// </summary>
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
		[DataMemberAttribute()]
		public string Year
		{
			get { return _year; }
			protected set
			{
				_year = StructuralObject.SetValidValue(value, true);
			}
		}
		private string _year;

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
			if (field == FieldNameConstants.OrderID)
				return ((this.OrderID == null) ? defaultValue : this.OrderID);
			if (field == FieldNameConstants.ShippedDate)
				return ((this.ShippedDate == null) ? defaultValue : this.ShippedDate);
			if (field == FieldNameConstants.Subtotal)
				return ((this.Subtotal == null) ? defaultValue : this.Subtotal);
			if (field == FieldNameConstants.Year)
				return ((this.Year == null) ? defaultValue : this.Year);
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
			var newItem = new SalesbyYear();
			newItem._orderID = this._orderID;
			newItem._shippedDate = this._shippedDate;
			newItem._subtotal = this._subtotal;
			newItem._year = this._year;
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
				case FieldNameConstants.OrderID:
					return 0;
				case FieldNameConstants.ShippedDate:
					return 0;
				case FieldNameConstants.Subtotal:
					return 0;
				case FieldNameConstants.Year:
					return 50;
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
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type 'Acme.Northwind.EFDAL.Entity.SalesbyYear.FieldNameConstants'.");

			switch ((FieldNameConstants)field)
			{
				case FieldNameConstants.OrderID: return typeof(int?);
				case FieldNameConstants.ShippedDate: return typeof(DateTime?);
				case FieldNameConstants.Subtotal: return typeof(decimal?);
				case FieldNameConstants.Year: return typeof(string);
			}
			return null;
		}

		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldType(Enum field)
		{
			if (field.GetType() != typeof(FieldNameConstants))
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type 'Acme.Northwind.EFDAL.Entity.SalesbyYear.FieldNameConstants'.");

			return GetFieldType((Acme.Northwind.EFDAL.Entity.SalesbyYear.FieldNameConstants)field);
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
			return ((System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.ISalesbyYear>)this).Equals(item as Acme.Northwind.EFDAL.Interfaces.Entity.ISalesbyYear);
		}

		#endregion

		#region Equals
		bool System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.ISalesbyYear>.Equals(Acme.Northwind.EFDAL.Interfaces.Entity.ISalesbyYear other)
		{
			return this.Equals(other);
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		public override bool Equals(object obj)
		{
			var other = obj as Acme.Northwind.EFDAL.Entity.SalesbyYear;
			if (other == null) return false;
			return (
				other.OrderID == this.OrderID &&
				other.ShippedDate == this.ShippedDate &&
				other.Subtotal == this.Subtotal &&
				other.Year == this.Year
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
