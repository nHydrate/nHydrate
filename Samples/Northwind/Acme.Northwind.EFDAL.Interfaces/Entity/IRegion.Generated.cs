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
	/// This is the interface for the entity Region
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial interface IRegion
	{
		#region Properties

		/// <summary>
		/// The property that maps back to the database 'Region.RegionDescription' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("RegionDescription")]
		string RegionDescription { get; set; }

		/// <summary>
		/// The property that maps back to the database 'Region.RegionID' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DisplayName("RegionID")]
		int RegionID { get; set; }

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
		/// The back navigation definition for walking Region->Territory
		/// </summary>
		[XmlIgnoreAttribute()]
		[SoapIgnoreAttribute()]
		[DataMemberAttribute()]
		[EdmRelationshipNavigationPropertyAttribute("Acme.Northwind.EFDAL.Interfaces.Entity", "FK__Territory_Region", "TerritoryList")]
		ICollection<Acme.Northwind.EFDAL.Interfaces.Entity.ITerritory> TerritoryList { get; }

		#endregion

	}

}

#region Metadata Class

namespace Acme.Northwind.EFDAL.Interfaces.Entity.Metadata
{
	/// <summary>
	/// Metadata class for the 'Region' entity
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial class RegionMetadata : Acme.Northwind.EFDAL.Interfaces.IMetadata
	{
		#region Properties

		/// <summary>
		/// Metadata information for the 'RegionDescription' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RegionDescription' is required.")]
		[System.ComponentModel.DataAnnotations.StringLength(50, ErrorMessage = "The property 'RegionDescription' has a maximum length of 50")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RegionDescription", AutoGenerateField = true)]
		public object RegionDescription;

		/// <summary>
		/// Metadata information for the 'RegionID' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RegionID' is required.")]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.DataAnnotations.Editable(false)]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RegionID", AutoGenerateField = true)]
		public object RegionID;

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

