#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using System.Runtime.Serialization;

namespace nHydrate.EFCore.Exceptions
{
	//[Serializable] 
	public partial class nHydrateException : System.ApplicationException
	{
		/// <summary>
		/// 
		/// </summary>
		public string ErrorCode = null;
		public string []Arguments = null;

		/// <summary>
		/// 
		/// </summary>
		public nHydrateException (): base ()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public nHydrateException ( string Message ) : base ( Message )
		{
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public nHydrateException ( string Message, System.Exception InnerException ) : base ( Message, InnerException )
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="message"></param>
		public nHydrateException ( string ErrorCode, string Message ) : base ( Message )
		{
			this.ErrorCode = ErrorCode;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="arguments"></param>
		public nHydrateException ( string ErrorCode, params object [] Arguments )
		{
			this.ErrorCode = ErrorCode;
			//this.arguments = arguments;

			this.Arguments = new string [Arguments.Length];

			for ( var length = 0; length < Arguments.Length; ++ length )
			{
				this.Arguments[length] = (string)Arguments [length];
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public nHydrateException ( string ErrorCode, string Message, System.Exception InnerException ) : base ( Message, InnerException )
		{
			this.ErrorCode = ErrorCode;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public nHydrateException ( SerializationInfo SerializationInfo, StreamingContext Context ) : base ( SerializationInfo, Context )
		{
			this.ErrorCode = ( string ) SerializationInfo.GetValue ( "errorCode", typeof ( string ) );
		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="info"></param>
		///// <param name="context"></param>
		//public override void GetObjectData ( SerializationInfo SerializationInfo, StreamingContext Context )
		//{
		//  SerializationInfo.AddValue ( "errorCode", ErrorCode );
		//  base.GetObjectData ( SerializationInfo, Context ) ;
		//}

	}
}
