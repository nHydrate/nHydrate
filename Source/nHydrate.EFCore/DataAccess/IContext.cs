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
	/// <summary>
	/// The interface for a context object
	/// </summary>
	public partial interface IContext
	{
		/// <summary>
		/// Determines the key of the model that created this library.
		/// </summary>
		string ModelKey { get; }

		/// <summary>
		/// Determines the version of the model that created this library.
		/// </summary>
		string Version { get; }

		/// <summary>
		/// Determines if the API matches the database connection
		/// </summary>
		bool IsValidConnection();

		/// <summary>
		/// Determines if the API matches the database connection
		/// </summary>
		/// <param name="checkVersion">Determines if the check also includes the exact version of the model</param>
		bool IsValidConnection(bool checkVersion);

		/// <summary>
		/// Given a field enumeration value, returns an entity enumeration value designating the source entity of the field
		/// </summary>
		Enum GetEntityFromField(Enum field);

		/// <summary>
		/// Given an entity enumeration value, returns a metadata object for the entity
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		object GetMetaData(Enum entity);

		/// <summary>
		/// Given a field enumeration value, returns the system type of the associated property
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		System.Type GetFieldType(Enum field);

	}
}

