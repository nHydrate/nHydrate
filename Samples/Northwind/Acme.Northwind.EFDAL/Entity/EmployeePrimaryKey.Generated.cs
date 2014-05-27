using System;
using nHydrate.EFCore.DataAccess;

namespace Acme.Northwind.EFDAL.Entity
{
	#region EmployeePrimaryKey

	/// <summary>
	/// A strongly-typed primary key object for the 'Employee' table.
	/// </summary>
	[Serializable()]
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.0.1.100")]
	public partial class EmployeePrimaryKey : nHydrate.EFCore.DataAccess.IPrimaryKey
	{
		/// <summary>
		/// A primary key field of the Employee entity
		/// </summary>
		protected int _employeeID;

		/// <summary>
		/// The constructor for this object which takes the fields that comprise the primary key for the 'Employee' table.
		/// </summary>
		public EmployeePrimaryKey(int employeeID)
		{
			_employeeID = employeeID;
		}

		/// <summary>
		/// A primary key for the 'Employee' table.
		/// </summary>
		public int EmployeeID
		{
			get { return _employeeID; }
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
				retval &= (this.EmployeeID == ((Acme.Northwind.EFDAL.Entity.EmployeePrimaryKey)obj).EmployeeID);
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
