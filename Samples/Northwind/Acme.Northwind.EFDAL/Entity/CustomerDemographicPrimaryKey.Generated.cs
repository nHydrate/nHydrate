using System;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	#region CustomerDemographicPrimaryKey

	/// <summary>
	/// A strongly-typed primary key object for the 'CustomerDemographic' table.
	/// </summary>
	[Serializable()]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.0.1.100")]
	public partial class CustomerDemographicPrimaryKey : nHydrate.EFCore.DataAccess.IPrimaryKey
	{
		/// <summary>
		/// A primary key field of the CustomerDemographic entity
		/// </summary>
		protected string _customerTypeID;

		/// <summary>
		/// The constructor for this object which takes the fields that comprise the primary key for the 'CustomerDemographic' table.
		/// </summary>
		public CustomerDemographicPrimaryKey(string customerTypeID)
		{
			_customerTypeID = customerTypeID;
		}

		/// <summary>
		/// A primary key for the 'CustomerDemographic' table.
		/// </summary>
		public string CustomerTypeID
		{
			get { return _customerTypeID; }
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
				retval &= (this.CustomerTypeID == ((Acme.Northwind.EFDAL.Entity.CustomerDemographicPrimaryKey)obj).CustomerTypeID);
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
