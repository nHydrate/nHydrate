using System;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	#region SupplierPrimaryKey

	/// <summary>
	/// A strongly-typed primary key object for the 'Supplier' table.
	/// </summary>
	[Serializable()]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.0.1.100")]
	public partial class SupplierPrimaryKey : nHydrate.EFCore.DataAccess.IPrimaryKey
	{
		/// <summary>
		/// A primary key field of the Supplier entity
		/// </summary>
		protected int _supplierID;

		/// <summary>
		/// The constructor for this object which takes the fields that comprise the primary key for the 'Supplier' table.
		/// </summary>
		public SupplierPrimaryKey(int supplierID)
		{
			_supplierID = supplierID;
		}

		/// <summary>
		/// A primary key for the 'Supplier' table.
		/// </summary>
		public int SupplierID
		{
			get { return _supplierID; }
		}

		/// <summary>
		/// Returns a value indicating whether the current object is equal to a specified object.
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() == this.GetType())
			{
				var retval = true;
				retval &= (this.SupplierID == ((Acme.Northwind.EFDAL.Entity.SupplierPrimaryKey)obj).SupplierID);
				return retval;
			}
			return false;
		}

		/// <summary>
		/// Serves as a hash function for this particular type.
		/// </summary>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

	}

	#endregion

}
