#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
	/// 
	/// </summary>
	/// <typeparam name="R"></typeparam>
	/// <typeparam name="E"></typeparam>
	public class AuditResultFieldCompare<R, E> : IAuditResultFieldCompare
	{
		public AuditResultFieldCompare(R value1, R value2, E field, System.Type dataType)
		{
			this.Field = field;
			this.Value1 = value1;
			this.Value2 = value2;
			this.DataType = dataType;
		}

		/// <summary>
		/// 
		/// </summary>
		public E Field { get; internal set; }
		/// <summary>
		/// 
		/// </summary>
		public R Value1 { get; internal set; }
		/// <summary>
		/// 
		/// </summary>
		public R Value2 { get; internal set; }

		/// <summary>
		/// 
		/// </summary>
		public System.Type DataType { get; internal set; }

		#region IAuditResultFieldCompare

		System.Enum IAuditResultFieldCompare.Field
		{
			get { return (System.Enum)Enum.Parse(typeof(E), this.Field.ToString()); }
		}

		object IAuditResultFieldCompare.Value1
		{
			get { return this.Value1; }
		}

		object IAuditResultFieldCompare.Value2
		{
			get { return this.Value2; }
		}

		#endregion

	}
}

