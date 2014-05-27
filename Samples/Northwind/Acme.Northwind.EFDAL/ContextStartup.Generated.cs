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
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Acme.Northwind.EFDAL
{
	#region ContextStartup

	/// <summary>
	/// This object holds the modifer information for audits on an ObjectContext
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.1.2")]
	public partial class ContextStartup
	{
		/// <summary>
		/// Creates a new instance of the ContextStartup object
		/// </summary>
		public ContextStartup(string modifier)
		{
			this.CurrentPlatform = DatabasePlatformConstants.SQLServer;
			this.Modifer = modifier;
			this.AllowLazyLoading = true;
		}

		/// <summary>
		/// Creates a new instance of the ContextStartup object
		/// </summary>
		public ContextStartup(string modifier, bool allowLazyLoading) :
			this(modifier)
		{
			this.AllowLazyLoading = allowLazyLoading;
		}

		/// <summary>
		/// Creates a new instance of the ContextStartup object
		/// </summary>
		public ContextStartup(string modifier, bool allowLazyLoading, int commandTimeout) :
			this(modifier, allowLazyLoading)
		{
			this.CommandTimeout = commandTimeout;
		}

		/// <summary>
		/// Creates a new instance of the ContextStartup object
		/// </summary>
		public ContextStartup(string modifier, DatabasePlatformConstants currentPlatform) :
			this(modifier)
		{
			this.CurrentPlatform = currentPlatform;
		}

		/// <summary>
		/// Creates a new instance of the ContextStartup object
		/// </summary>
		public ContextStartup(string modifier, bool allowLazyLoading, int commandTimeout, DatabasePlatformConstants currentPlatform) :
			this(modifier, allowLazyLoading)
		{
			this.CurrentPlatform = currentPlatform;
			this.CommandTimeout = commandTimeout;
		}

		/// <summary>
		/// The modifier string used for auditing
		/// </summary>
		public virtual string Modifer { get; protected internal set; }

		/// <summary>
		/// Determines if relationships can be walked via 'Lazy Loading'
		/// </summary>
		public virtual bool AllowLazyLoading { get; protected internal set; }

		/// <summary>
		/// Determines the database timeout value in seconds
		/// </summary>
		public virtual int? CommandTimeout { get; protected internal set; }

		/// <summary>
		/// Determines the database platform that the context instance will target
		/// </summary>
		public DatabasePlatformConstants CurrentPlatform { get; private set; }

	}

	#endregion

}

