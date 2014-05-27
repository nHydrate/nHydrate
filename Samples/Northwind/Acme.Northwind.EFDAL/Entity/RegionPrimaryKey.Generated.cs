using System;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	#region RegionPrimaryKey

	/// <summary>
	/// A strongly-typed primary key object for the 'Region' table.
	/// </summary>
	[Serializable()]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.0.1.100")]
	public partial class RegionPrimaryKey : nHydrate.EFCore.DataAccess.IPrimaryKey
	{
		/// <summary>
		/// A primary key field of the Region entity
		/// </summary>
		protected int _regionID;

		/// <summary>
		/// The constructor for this object which takes the fields that comprise the primary key for the 'Region' table.
		/// </summary>
		public RegionPrimaryKey(int regionID)
		{
			_regionID = regionID;
		}

		/// <summary>
		/// A primary key for the 'Region' table.
		/// </summary>
		public int RegionID
		{
			get { return _regionID; }
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
				retval &= (this.RegionID == ((Acme.Northwind.EFDAL.Entity.RegionPrimaryKey)obj).RegionID);
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
