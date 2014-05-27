using System;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	#region ProductPrimaryKey

	/// <summary>
	/// A strongly-typed primary key object for the 'Product' table.
	/// </summary>
	[Serializable()]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.0.1.100")]
	public partial class ProductPrimaryKey : nHydrate.EFCore.DataAccess.IPrimaryKey
	{
		/// <summary>
		/// A primary key field of the Product entity
		/// </summary>
		protected int _productID;

		/// <summary>
		/// The constructor for this object which takes the fields that comprise the primary key for the 'Product' table.
		/// </summary>
		public ProductPrimaryKey(int productID)
		{
			_productID = productID;
		}

		/// <summary>
		/// A primary key for the 'Product' table.
		/// </summary>
		public int ProductID
		{
			get { return _productID; }
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
				retval &= (this.ProductID == ((Acme.Northwind.EFDAL.Entity.ProductPrimaryKey)obj).ProductID);
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
