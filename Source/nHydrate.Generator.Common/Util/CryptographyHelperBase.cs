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

namespace nHydrate.Generator.Common.Util
{
	internal abstract class CryptographyHelperBase : ICryptoHelper
	{
				
		#region ICryptoHelper Members

												public abstract byte[] Encrypt(byte[] plaintext);

														public string Encrypt( string plaintext, StringEncodingType encoding )
		{
			var bytesIn = ByteEncoding.StringToBytes(plaintext);
			var encryptedBytes = Encrypt(bytesIn);
			string returnValue = null;
			switch(encoding)
			{
				case StringEncodingType.Base64:
					returnValue = ByteEncoding.BytesToBase64(encryptedBytes);
					break;
				case StringEncodingType.Hex:
					returnValue = ByteEncoding.BytesToHex(encryptedBytes);
					break;
				default:
					throw new ArgumentException("Unknown encoding type.");
			}
			Array.Clear(bytesIn, 0, bytesIn.Length);
			Array.Clear(encryptedBytes, 0, encryptedBytes.Length);
			return returnValue;
		}

												public string Encrypt( string plaintext)
		{
			return Encrypt(plaintext, StringEncodingType.Base64);
		}


												public abstract byte[] Decrypt(byte[] cipherText);
		

														public string Decrypt( string cipherText, StringEncodingType encoding )
		{
			byte[] bytesIn = null;
			switch(encoding)
			{
				case StringEncodingType.Base64:
					bytesIn = ByteEncoding.Base64ToBytes(cipherText);
					break;
				case StringEncodingType.Hex:
					bytesIn = ByteEncoding.HexToBytes(cipherText);
					break;
				default:
					throw new ArgumentException("Unknown encoding type.");
			}
			var bytesOut = Decrypt(bytesIn);
			var returnValue = ByteEncoding.BytesToString(bytesOut);
			Array.Clear(bytesIn, 0, bytesIn.Length);
			Array.Clear(bytesOut, 0, bytesOut.Length);
			return returnValue;
		}

												public string Decrypt( string cipherText)
		{
			return Decrypt(cipherText, StringEncodingType.Base64);
		}


		#endregion

		#region IDisposable Members

		public abstract void Dispose();

		#endregion

		#region ICryptoHelper Properties

		public abstract string Entropy
		{
			get;
			set;
		}

		public abstract CryptographyAlgorithm Algorithm
		{
			get;
		}

		#endregion
	}
}

