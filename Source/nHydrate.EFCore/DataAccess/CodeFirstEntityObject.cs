using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;

namespace Widgetsphere.EFCore.DataAccess
{
	/// <summary>
	/// The base class for all entity objects
	/// </summary>
	[Serializable()]
	[System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
	public abstract partial class NHEntityObject : EntityObject
	{
		/// <summary>
		/// Get the validation rule violations
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<IRuleViolation> GetRuleViolations()
		{
			List<IRuleViolation> retval = new List<IRuleViolation>();
			return retval;
		}

		/// <summary>
		/// Determines if all of the validation rules have been met
		/// </summary>
		/// <returns></returns>
		public virtual bool IsValid()
		{
			return (GetRuleViolations().Count() == 0);
		}

		/// <summary>
		/// Test another entity object for equivalence against the current instance
		/// </summary>
		public abstract bool IsEquivalent(NHEntityObject item);

	}
}
