using System;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	#region CategoryPrimaryKey

	/// <summary>
	/// A strongly-typed primary key object for the 'Category' table.
	/// </summary>
	[Serializable()]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.0.1.100")]
	public partial class CategoryPrimaryKey : nHydrate.EFCore.DataAccess.IPrimaryKey
	{
		/// <summary>
		/// A primary key field of the Category entity
		/// </summary>
		protected int _categoryID;

		/// <summary>
		/// The constructor for this object which takes the fields that comprise the primary key for the 'Category' table.
		/// </summary>
		public CategoryPrimaryKey(int categoryID)
		{
			_categoryID = categoryID;
		}

		/// <summary>
		/// A primary key for the 'Category' table.
		/// </summary>
		public int CategoryID
		{
			get { return _categoryID; }
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
				retval &= (this.CategoryID == ((Acme.Northwind.EFDAL.Entity.CategoryPrimaryKey)obj).CategoryID);
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
