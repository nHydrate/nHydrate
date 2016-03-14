#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;

namespace nHydrate.EFCore.DataAccess
{
	#region AuditTypeConstants Enumeration

	/// <summary>
	/// A set of values for the types of audits
	/// </summary>
	public enum AuditTypeConstants
	{
		/// <summary>
		/// Represents a row insert
		/// </summary>
		Insert = 1,
		/// <summary>
		/// Represents a row update
		/// </summary>
		Update = 2,
		/// <summary>
		/// Represents a row delete
		/// </summary>
		Delete = 3,
	}

	#endregion

	/// <summary>
	/// The base interface for all audit objects
	/// </summary>
	public interface IAudit
	{
		/// <summary>
		/// The type of audit
		/// </summary>
		AuditTypeConstants AuditType { get; }

		/// <summary>
		/// The date of the audit
		/// </summary>
		DateTime AuditDate { get; }

		/// <summary>
		/// The modifier value of the audit
		/// </summary>
		string ModifiedBy { get; }
	}

}

