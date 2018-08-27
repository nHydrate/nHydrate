#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
using System.Collections;
using System.IO;
using System.Security.Cryptography;

namespace nHydrate.Generator.Common.Util
{
	/// <summary>
	/// 
	/// </summary>
	public class CRC32HashAlgorithm : HashAlgorithm
	{
		protected static uint AllOnes = 0xffffffff;
		protected static Hashtable cachedCRC32Tables;
		protected static bool autoCache;
	
		protected uint[] crc32Table; 
		private uint m_crc;
		
		/// <summary>
		/// Returns the default polynomial (used in WinZip, Ethernet, etc)
		/// </summary>
		public static uint DefaultPolynomial
		{
			get { return 0x04C11DB7; }
		}

		/// <summary>
		/// Gets or sets the auto-cache setting of this class.
		/// </summary>
		public static bool AutoCache
		{
			get { return autoCache; }
			set { autoCache = value; }
		}

		/// <summary>
		/// Initialize the cache
		/// </summary>
		static CRC32HashAlgorithm()
		{
			cachedCRC32Tables = Hashtable.Synchronized( new Hashtable() );
			autoCache = true;
		}

		public static void ClearCache()
		{
			cachedCRC32Tables.Clear();
		}


		/// <summary>
		/// Builds a crc32 table given a polynomial
		/// </summary>
		/// <param name="ulPolynomial"></param>
		/// <returns></returns>
		protected static uint[] BuildCRC32Table( uint ulPolynomial )
		{
			uint dwCrc;
			var table = new uint[256];

			// 256 values representing ASCII character codes. 
			for (var i = 0; i < 256; i++)
			{
				dwCrc = (uint)i;
				for (var j = 8; j > 0; j--)
				{
					if((dwCrc & 1) == 1)
						dwCrc = (dwCrc >> 1) ^ ulPolynomial;
					else
						dwCrc >>= 1;
				}
				table[i] = dwCrc;
			}

			return table;
		}


		/// <summary>
		/// Creates a CRC32HashAlgorithm object using the DefaultPolynomial
		/// </summary>
		public CRC32HashAlgorithm() : this(DefaultPolynomial)
		{
		}

		/// <summary>
		/// Creates a CRC32HashAlgorithm object using the specified Creates a CRC32HashAlgorithm object 
		/// </summary>
		public CRC32HashAlgorithm(uint aPolynomial) : this(aPolynomial, CRC32HashAlgorithm.AutoCache)
		{
		}
	
		/// <summary>
		/// Construct the 
		/// </summary>
		public CRC32HashAlgorithm(uint aPolynomial, bool cacheTable)
		{
			this.HashSizeValue = 32;

			crc32Table = (uint []) cachedCRC32Tables[aPolynomial];
			if ( crc32Table == null )
			{
				crc32Table = CRC32HashAlgorithm.BuildCRC32Table(aPolynomial);
				if ( cacheTable )
					cachedCRC32Tables.Add( aPolynomial, crc32Table );
			}
			Initialize();
		}
	
		/// <summary>
		/// Initializes an implementation of HashAlgorithm.
		/// </summary>
		public override void Initialize()
		{
			m_crc = AllOnes;
		}
	
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		protected override void HashCore(byte[] buffer, int offset, int count)
		{
			// Save the text in the buffer. 
			for (var i = offset; i < count; i++)
			{
				ulong tabPtr = (m_crc & 0xFF) ^ buffer[i];
				m_crc >>= 8;
				m_crc ^= crc32Table[tabPtr];
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override byte[] HashFinal()
		{
			var finalHash = new byte [ 4 ];
			ulong finalCRC = m_crc ^ AllOnes;
		
			finalHash[0] = (byte) ((finalCRC >> 24) & 0xFF);
			finalHash[1] = (byte) ((finalCRC >> 16) & 0xFF);
			finalHash[2] = (byte) ((finalCRC >>  8) & 0xFF);
			finalHash[3] = (byte) ((finalCRC >>  0) & 0xFF);
		
			return finalHash;
		}
	
		/// <summary>
		/// Computes the hash value for the specified Stream.
		/// </summary>
		new public byte[] ComputeHash(Stream inputStream)
		{
			var buffer = new byte [4096];
			int bytesRead;
			while ( (bytesRead = inputStream.Read(buffer, 0, 4096)) > 0 )
			{
				HashCore(buffer, 0, bytesRead);
			}
			return HashFinal();
		}


		/// <summary>
		/// Overloaded. Computes the hash value for the input data.
		/// </summary>
		new public byte[] ComputeHash(byte[] buffer)
		{
			return ComputeHash(buffer, 0, buffer.Length);
		}

		/// <summary>
		/// Overloaded. Computes the hash value for the input data.
		/// </summary>
		public byte[] ComputeHash(string s)
		{
			var bytes = System.Text.Encoding.ASCII.GetBytes(s);
			return ComputeHash(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Overloaded. Computes the hash value for the input data.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		new public byte[] ComputeHash( byte[] buffer, int offset, int count )
		{
			HashCore(buffer, offset, count);
			return HashFinal();
		}
	}

}

