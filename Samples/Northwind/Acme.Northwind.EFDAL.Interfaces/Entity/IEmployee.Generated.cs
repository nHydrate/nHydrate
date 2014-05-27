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
	/// This is the interface for the entity Employee
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial interface IEmployee
	{
		#region Properties

		/// <summary>
		/// The property that maps back to the database 'Employees.Address' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Address")]
		string Address { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.BirthDate' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("BirthDate")]
		DateTime? BirthDate { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.City' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("City")]
		string City { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.Country' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Country")]
		string Country { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.EmployeeID' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DisplayName("EmployeeID")]
		int EmployeeID { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.Extension' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Extension")]
		string Extension { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.FirstName' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("FirstName")]
		string FirstName { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.HireDate' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("HireDate")]
		DateTime? HireDate { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.HomePhone' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("HomePhone")]
		string HomePhone { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.LastName' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("LastName")]
		string LastName { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.Notes' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Notes")]
		string Notes { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.Photo' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Photo")]
		System.Byte[] Photo { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.PhotoPath' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("PhotoPath")]
		string PhotoPath { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.PostalCode' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("PostalCode")]
		string PostalCode { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.Region' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Region")]
		string Region { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.ReportsTo' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("ReportsTo")]
		int? ReportsTo { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.Title' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Title")]
		string Title { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Employees.TitleOfCourtesy' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("TitleOfCourtesy")]
		string TitleOfCourtesy { get; set; }

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
		/// The back navigation definition for walking Employee->Employee
		/// </summary>
		[XmlIgnoreAttribute()]
		[SoapIgnoreAttribute()]
		[DataMemberAttribute()]
		[EdmRelationshipNavigationPropertyAttribute("Acme.Northwind.EFDAL.Interfaces.Entity", "FK_ReportTo_Employee_Employee", "ReportToEmployeeList")]
		ICollection<Acme.Northwind.EFDAL.Interfaces.Entity.IEmployee> ReportToEmployeeList { get; }

		/// <summary>
		/// The back navigation definition for walking Employee->EmployeeTerritorie
		/// </summary>
		[XmlIgnoreAttribute()]
		[SoapIgnoreAttribute()]
		[DataMemberAttribute()]
		[EdmRelationshipNavigationPropertyAttribute("Acme.Northwind.EFDAL.Interfaces.Entity", "FK__EmployeeTerritorie_Employee", "EmployeeTerritorieList")]
		ICollection<Acme.Northwind.EFDAL.Interfaces.Entity.IEmployeeTerritorie> EmployeeTerritorieList { get; }

		/// <summary>
		/// The back navigation definition for walking Employee->Order
		/// </summary>
		[XmlIgnoreAttribute()]
		[SoapIgnoreAttribute()]
		[DataMemberAttribute()]
		[EdmRelationshipNavigationPropertyAttribute("Acme.Northwind.EFDAL.Interfaces.Entity", "FK__Order_Employee", "OrderList")]
		ICollection<Acme.Northwind.EFDAL.Interfaces.Entity.IOrder> OrderList { get; }

		/// <summary>
		/// The navigation definition for walking Employee->Employee (for role 'ReportTo')
		/// </summary>
		Acme.Northwind.EFDAL.Interfaces.Entity.IEmployee ReportToEmployee { get; set; }

		#endregion

	}

}

#region Metadata Class

namespace Acme.Northwind.EFDAL.Interfaces.Entity.Metadata
{
	/// <summary>
	/// Metadata class for the 'Employee' entity
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial class EmployeeMetadata : Acme.Northwind.EFDAL.Interfaces.IMetadata
	{
		#region Properties

		/// <summary>
		/// Metadata information for the 'Address' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(60, ErrorMessage = "The property 'Address' has a maximum length of 60")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Address", AutoGenerateField = true)]
		public object Address;

		/// <summary>
		/// Metadata information for the 'BirthDate' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "BirthDate", AutoGenerateField = true)]
		public object BirthDate;

		/// <summary>
		/// Metadata information for the 'City' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(15, ErrorMessage = "The property 'City' has a maximum length of 15")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "City", AutoGenerateField = true)]
		public object City;

		/// <summary>
		/// Metadata information for the 'Country' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(15, ErrorMessage = "The property 'Country' has a maximum length of 15")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Country", AutoGenerateField = true)]
		public object Country;

		/// <summary>
		/// Metadata information for the 'EmployeeID' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'EmployeeID' is required.")]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.DataAnnotations.Editable(false)]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "EmployeeID", AutoGenerateField = true)]
		public object EmployeeID;

		/// <summary>
		/// Metadata information for the 'Extension' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(4, ErrorMessage = "The property 'Extension' has a maximum length of 4")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Extension", AutoGenerateField = true)]
		public object Extension;

		/// <summary>
		/// Metadata information for the 'FirstName' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'FirstName' is required.")]
		[System.ComponentModel.DataAnnotations.StringLength(10, ErrorMessage = "The property 'FirstName' has a maximum length of 10")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "FirstName", AutoGenerateField = true)]
		public object FirstName;

		/// <summary>
		/// Metadata information for the 'HireDate' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "HireDate", AutoGenerateField = true)]
		public object HireDate;

		/// <summary>
		/// Metadata information for the 'HomePhone' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(24, ErrorMessage = "The property 'HomePhone' has a maximum length of 24")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "HomePhone", AutoGenerateField = true)]
		public object HomePhone;

		/// <summary>
		/// Metadata information for the 'LastName' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'LastName' is required.")]
		[System.ComponentModel.DataAnnotations.StringLength(20, ErrorMessage = "The property 'LastName' has a maximum length of 20")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "LastName", AutoGenerateField = true)]
		public object LastName;

		/// <summary>
		/// Metadata information for the 'Notes' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(1073741823, ErrorMessage = "The property 'Notes' has a maximum length of 1073741823")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Notes", AutoGenerateField = true)]
		public object Notes;

		/// <summary>
		/// Metadata information for the 'Photo' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Photo", AutoGenerateField = true)]
		public object Photo;

		/// <summary>
		/// Metadata information for the 'PhotoPath' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(255, ErrorMessage = "The property 'PhotoPath' has a maximum length of 255")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "PhotoPath", AutoGenerateField = true)]
		public object PhotoPath;

		/// <summary>
		/// Metadata information for the 'PostalCode' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(10, ErrorMessage = "The property 'PostalCode' has a maximum length of 10")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "PostalCode", AutoGenerateField = true)]
		public object PostalCode;

		/// <summary>
		/// Metadata information for the 'Region' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(15, ErrorMessage = "The property 'Region' has a maximum length of 15")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Region", AutoGenerateField = true)]
		public object Region;

		/// <summary>
		/// Metadata information for the 'ReportsTo' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "ReportsTo", AutoGenerateField = true)]
		public object ReportsTo;

		/// <summary>
		/// Metadata information for the 'Title' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(30, ErrorMessage = "The property 'Title' has a maximum length of 30")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Title", AutoGenerateField = true)]
		public object Title;

		/// <summary>
		/// Metadata information for the 'TitleOfCourtesy' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(25, ErrorMessage = "The property 'TitleOfCourtesy' has a maximum length of 25")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "TitleOfCourtesy", AutoGenerateField = true)]
		public object TitleOfCourtesy;

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

