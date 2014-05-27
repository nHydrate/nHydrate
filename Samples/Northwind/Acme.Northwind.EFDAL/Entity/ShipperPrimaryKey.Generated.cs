using System;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	#region ShipperPrimaryKey

	/// <summary>
	/// A strongly-typed primary key object for the 'Shipper' table.
	/// </summary>
	[Serializable()]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.0.1.100")]
	public partial class ShipperPrimaryKey : nHydrate.EFCore.DataAccess.IPrimaryKey
	{
		/// <summary>
		/// A primary key field of the Shipper entity
		/// </summary>
		protected int _shipperID;

		/// <summary>
		/// The constructor for this object which takes the fields that comprise the primary key for the 'Shipper' table.
		/// </summary>
		public ShipperPrimaryKey(int shipperID)
		{
			_shipperID = shipperID;
		}

		/// <summary>
		/// A primary key for the 'Shipper' table.
		/// </summary>
		public int ShipperID
		{
			get { return _shipperID; }
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
				retval &= (this.ShipperID == ((Acme.Northwind.EFDAL.Entity.ShipperPrimaryKey)obj).ShipperID);
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
