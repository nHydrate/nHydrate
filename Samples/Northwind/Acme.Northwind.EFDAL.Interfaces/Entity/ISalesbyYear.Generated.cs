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
	/// This is the interface for the entity SalesbyYear
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial interface ISalesbyYear
	{
		#region Properties

		/// <summary>
		/// The property for the field 'OrderID'
		/// </summary>
		int? OrderID { get; }

		/// <summary>
		/// The property for the field 'ShippedDate'
		/// </summary>
		DateTime? ShippedDate { get; }

		/// <summary>
		/// The property for the field 'Subtotal'
		/// </summary>
		decimal? Subtotal { get; }

		/// <summary>
		/// The property for the field 'Year'
		/// </summary>
		string Year { get; }

		#endregion

	}

}

