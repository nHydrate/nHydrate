using System;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	#region OrderPrimaryKey

	/// <summary>
	/// A strongly-typed primary key object for the 'Order' table.
	/// </summary>
	[Serializable()]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.0.1.100")]
	public partial class OrderPrimaryKey : nHydrate.EFCore.DataAccess.IPrimaryKey
	{
		/// <summary>
		/// A primary key field of the Order entity
		/// </summary>
		protected int _orderID;

		/// <summary>
		/// The constructor for this object which takes the fields that comprise the primary key for the 'Order' table.
		/// </summary>
		public OrderPrimaryKey(int orderID)
		{
			_orderID = orderID;
		}

		/// <summary>
		/// A primary key for the 'Order' table.
		/// </summary>
		public int OrderID
		{
			get { return _orderID; }
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
				retval &= (this.OrderID == ((Acme.Northwind.EFDAL.Entity.OrderPrimaryKey)obj).OrderID);
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
