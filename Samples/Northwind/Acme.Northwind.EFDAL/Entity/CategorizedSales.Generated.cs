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
	[EdmComplexTypeAttribute(NamespaceName = "Acme.Northwind.EFDAL.Entity", Name = "CategorizedSales")]
	[DataContractAttribute(IsReference = true)]
	[Serializable]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial class CategorizedSales : nHydrate.EFCore.DataAccess.NHComplexObject, nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject, Acme.Northwind.EFDAL.Interfaces.Entity.ICategorizedSales, System.ICloneable, System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.ICategorizedSales>
	{
		#region FieldNameConstants Enumeration

		/// <summary>
		/// Enumeration to define each property that maps to a database field for the 'CategorizedSales' table.
		/// </summary>
		public enum FieldNameConstants
		{
			/// <summary>
			/// Field mapping for the 'ProductName' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'ProductName' property")]
			ProductName,
			/// <summary>
			/// Field mapping for the 'TotalPurchase' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'TotalPurchase' property")]
			TotalPurchase,
		}
		#endregion

		#region Primitive Properties

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
		public decimal? TotalPurchase
		{
			get { return _totalPurchase; }
			protected set
			{
				_totalPurchase = StructuralObject.SetValidValue(value);
			}
		}
		private decimal? _totalPurchase;

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
			if (field == FieldNameConstants.ProductName)
				return ((this.ProductName == null) ? defaultValue : this.ProductName);
			if (field == FieldNameConstants.TotalPurchase)
				return ((this.TotalPurchase == null) ? defaultValue : this.TotalPurchase);
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
			var newItem = new CategorizedSales();
			newItem._productName = this._productName;
			newItem._totalPurchase = this._totalPurchase;
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
				case FieldNameConstants.ProductName:
					return 50;
				case FieldNameConstants.TotalPurchase:
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
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type 'Acme.Northwind.EFDAL.Entity.CategorizedSales.FieldNameConstants'.");

			switch ((FieldNameConstants)field)
			{
				case FieldNameConstants.ProductName: return typeof(string);
				case FieldNameConstants.TotalPurchase: return typeof(decimal?);
			}
			return null;
		}

		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldType(Enum field)
		{
			if (field.GetType() != typeof(FieldNameConstants))
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type 'Acme.Northwind.EFDAL.Entity.CategorizedSales.FieldNameConstants'.");

			return GetFieldType((Acme.Northwind.EFDAL.Entity.CategorizedSales.FieldNameConstants)field);
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
			return ((System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.ICategorizedSales>)this).Equals(item as Acme.Northwind.EFDAL.Interfaces.Entity.ICategorizedSales);
		}

		#endregion

		#region Equals
		bool System.IEquatable<Acme.Northwind.EFDAL.Interfaces.Entity.ICategorizedSales>.Equals(Acme.Northwind.EFDAL.Interfaces.Entity.ICategorizedSales other)
		{
			return this.Equals(other);
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		public override bool Equals(object obj)
		{
			var other = obj as Acme.Northwind.EFDAL.Entity.CategorizedSales;
			if (other == null) return false;
			return (
				other.ProductName == this.ProductName &&
				other.TotalPurchase == this.TotalPurchase
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
