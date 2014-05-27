using System;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	#region EmployeeTerritoriePrimaryKey

	/// <summary>
	/// A strongly-typed primary key object for the 'EmployeeTerritorie' table.
	/// </summary>
	[Serializable()]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.0.1.100")]
	public partial class EmployeeTerritoriePrimaryKey : nHydrate.EFCore.DataAccess.IPrimaryKey
	{
		/// <summary>
		/// A primary key field of the EmployeeTerritorie entity
		/// </summary>
		protected int _employeeID;
		/// <summary>
		/// A primary key field of the EmployeeTerritorie entity
		/// </summary>
		protected string _territoryID;

		/// <summary>
		/// The constructor for this object which takes the fields that comprise the primary key for the 'EmployeeTerritorie' table.
		/// </summary>
		public EmployeeTerritoriePrimaryKey(int employeeID, string territoryID)
		{
			_employeeID = employeeID;
			_territoryID = territoryID;
		}

		/// <summary>
		/// A primary key for the 'EmployeeTerritorie' table.
		/// </summary>
		public int EmployeeID
		{
			get { return _employeeID; }
		}

		/// <summary>
		/// A primary key for the 'EmployeeTerritorie' table.
		/// </summary>
		public string TerritoryID
		{
			get { return _territoryID; }
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
				retval &= (this.EmployeeID == ((Acme.Northwind.EFDAL.Entity.EmployeeTerritoriePrimaryKey)obj).EmployeeID);
				retval &= (this.TerritoryID == ((Acme.Northwind.EFDAL.Entity.EmployeeTerritoriePrimaryKey)obj).TerritoryID);
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
