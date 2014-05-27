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
using System.Runtime.Serialization;
using System.Data.Objects.DataClasses;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using Acme.Northwind.EFDAL.Interfaces;

namespace Acme.Northwind.EFDAL.Interfaces.Entity
{
	/// <summary>
	/// This is the interface for the entity CustomerCustomerDemo
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial interface ICustomerCustomerDemo
	{
		#region Properties

		/// <summary>
		/// The property that maps back to the database 'CustomerCustomerDemo.CustomerID' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DisplayName("CustomerID")]
		string CustomerID { get; set; }

		/// <summary>
		/// The property that maps back to the database 'CustomerCustomerDemo.CustomerTypeID' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DisplayName("CustomerTypeID")]
		string CustomerTypeID { get; set; }

		/// <summary>
		/// The audit field for the 'Created By' column.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		string CreatedBy { get; }

		/// <summary>
		/// The audit field for the 'Created Date' column.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		DateTime? CreatedDate { get; }

		/// <summary>
		/// The audit field for the 'Modified By' column.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		string ModifiedBy { get; }

		/// <summary>
		/// The audit field for the 'Modified Date' column.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		DateTime? ModifiedDate { get; }

		#endregion

		#region Navigation Properties

		/// <summary>
		/// The navigation definition for walking CustomerCustomerDemo->CustomerDemographic (for role 'Customercustomerdemo')
		/// </summary>
		Acme.Northwind.EFDAL.Interfaces.Entity.ICustomerDemographic CustomercustomerdemoCustomerDemographic { get; set; }

		/// <summary>
		/// The navigation definition for walking CustomerCustomerDemo->Customer
		/// </summary>
		Acme.Northwind.EFDAL.Interfaces.Entity.ICustomer Customer { get; set; }

		#endregion

	}

}

#region Metadata Class

namespace Acme.Northwind.EFDAL.Interfaces.Entity.Metadata
{
	/// <summary>
	/// Metadata class for the 'CustomerCustomerDemo' entity
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial class CustomerCustomerDemoMetadata : Acme.Northwind.EFDAL.Interfaces.IMetadata
	{
		#region Properties

		/// <summary>
		/// Metadata information for the 'CustomerID' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'CustomerID' is required.")]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.DataAnnotations.Editable(false)]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DataAnnotations.StringLength(5, ErrorMessage = "The property 'CustomerID' has a maximum length of 5")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "CustomerID", AutoGenerateField = true)]
		public object CustomerID;

		/// <summary>
		/// Metadata information for the 'CustomerTypeID' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'CustomerTypeID' is required.")]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.DataAnnotations.Editable(false)]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DataAnnotations.StringLength(10, ErrorMessage = "The property 'CustomerTypeID' has a maximum length of 10")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "CustomerTypeID", AutoGenerateField = true)]
		public object CustomerTypeID;

		/// <summary>
		/// Metadata information for the 'CreatedBy' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "The property 'CreatedBy' has a maximum length of 100")]
		[System.ComponentModel.ReadOnly(true)]
		public object CreatedBy;

		/// <summary>
		/// Metadata information for the 'CreatedDate' parameter
		/// </summary>
		[System.ComponentModel.ReadOnly(true)]
		public object CreatedDate;

		/// <summary>
		/// Metadata information for the 'ModifiedBy' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "The property 'ModifiedBy' has a maximum length of 100")]
		[System.ComponentModel.ReadOnly(true)]
		public object ModifiedBy;

		/// <summary>
		/// Metadata information for the 'ModifiedDate' parameter
		/// </summary>
		[System.ComponentModel.ReadOnly(true)]
		public object ModifiedDate;

		/// <summary>
		/// Metadata information for the 'Timestamp' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Timestamp()]
		[System.ComponentModel.ReadOnly(true)]
		public object Timestamp;

		#endregion

	}

}

#endregion

