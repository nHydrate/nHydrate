//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq;

namespace Acme.Northwind.EFDAL.Interfaces.Entity
{
	/// <summary>
	/// This is the interface for the entity ProductsbyCategory
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial interface IProductsbyCategory
	{
		#region Properties

		/// <summary>
		/// The property for the field 'CategoryName'
		/// </summary>
		string CategoryName { get; }

		/// <summary>
		/// The property for the field 'Discontinued'
		/// </summary>
		bool Discontinued { get; }

		/// <summary>
		/// The property for the field 'ProductName'
		/// </summary>
		string ProductName { get; }

		/// <summary>
		/// The property for the field 'QuantityPerUnit'
		/// </summary>
		string QuantityPerUnit { get; }

		/// <summary>
		/// The property for the field 'UnitsInStock'
		/// </summary>
		short? UnitsInStock { get; }

		#endregion

	}

}

