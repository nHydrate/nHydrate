using System;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	#region CustomerPrimaryKey

	/// <summary>
	/// A strongly-typed primary key object for the 'Customer' table.
	/// </summary>
	[Serializable()]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.0.1.100")]
	public partial class CustomerPrimaryKey : nHydrate.EFCore.DataAccess.IPrimaryKey
	{
		/// <summary>
		/// A primary key field of the Customer entity
		/// </summary>
		protected string _customerID;

		/// <summary>
		/// The constructor for this object which takes the fields that comprise the primary key for the 'Customer' table.
		/// </summary>
		public CustomerPrimaryKey(string customerID)
		{
			_customerID = customerID;
		}

		/// <summary>
		/// A primary key for the 'Customer' table.
		/// </summary>
		public string CustomerID
		{
			get { return _customerID; }
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
				retval &= (this.CustomerID == ((Acme.Northwind.EFDAL.Entity.CustomerPrimaryKey)obj).CustomerID);
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
