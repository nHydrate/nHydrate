#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
	public partial interface IReadOnlyBusinessObject : nHydrate.EFCore.DataAccess.INHEntityObject
	{
		/// <summary>
		/// If applicable, returns the maximum number of characters the specified field can hold
		/// </summary>
		/// <param name="field"></param>
		/// <returns>If not applicable, the return value is 0</returns>
		int GetMaxLength(Enum field);

		/// <summary>
		/// Returns the primary key for this object
		/// </summary>
		IPrimaryKey PrimaryKey { get; }

		/// <summary>
		/// Determines if the specified object is equivalent to the current object
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		bool IsEquivalent(INHEntityObject item);
		System.Type GetFieldNameConstants();

		/// <summary>
		/// Gets the value of a field specified by the enumeration
		/// </summary>
		/// <param name="field">The from which to get the value</param>
		/// <returns></returns>
		object GetValue(Enum field);

		/// <summary>
		/// Gets the value of a field specified by the enumeration
		/// </summary>
		/// <param name="field">The from which to get the value</param>
		/// <param name="defaultValue">The default value to return if the value is null</param>
		/// <returns></returns>
		object GetValue(Enum field, object defaultValue);

		/// <summary>
		/// Returns the system type of the specified field
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		System.Type GetFieldType(Enum field);
	}
}

