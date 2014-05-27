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
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace nHydrate.Generator.Common.Util
{
	/// <summary>
	/// basic Encrption/decryption functionaility
	/// </summary>
	public class CryptoHelper
	{
		#region enums, constants & fields
		//types of symmetric encyption
		public enum CryptoTypes
		{
			encTypeDES = 0,
			encTypeRC2 = 1,
			encTypeRijndael = 2,
			encTypeTripleDES = 3,
		}
		
		private const string CRYPT_DEFAULT_PASSWORD = "abcd!@#";
		private const CryptoTypes CRYPT_DEFAULT_METHOD = CryptoTypes.encTypeRijndael;

		private byte[] mKey = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24};
		private byte[] mIV = {65, 110, 68, 26, 69, 178, 200, 219};
		private readonly byte[] SaltByteArray  = {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76};
		private CryptoTypes mCryptoType = CRYPT_DEFAULT_METHOD;
		private string mPassword = CRYPT_DEFAULT_PASSWORD;
		#endregion

		#region Constructors

		public CryptoHelper()
		{
			calculateNewKeyAndIV();
		}

		public CryptoHelper(CryptoTypes CryptoType)
		{
			this.CryptoType = CryptoType;
		}
		#endregion

		#region Props

		/// <summary>
		///		type of encryption / decryption used
		/// </summary>
		public CryptoTypes CryptoType
		{
			get 
			{
				return mCryptoType;
			}
			set
			{
				if (mCryptoType != value) 
				{
					mCryptoType = value;
					calculateNewKeyAndIV();
				}
			}
		}

		/// <summary>
		///		Passsword Key Property.
		///		The password key used when encrypting / decrypting
		/// </summary>
		public string Password
		{
			get 
			{
				return mPassword;
			}
			set 
			{	
				if (mPassword != value)
				{
					mPassword = value;
					calculateNewKeyAndIV();
				}
			}
		}
		#endregion

		#region Encryption

		/// <summary>
		///		Encrypt a string
		/// </summary>
		/// <param name="inputText">text to encrypt</param>
		/// <returns>an encrypted string</returns>
		public string Encrypt(string inputText)
		{
			//declare a new encoder
			var UTF8Encoder = new UTF8Encoding();
			//get byte representation of string
			var inputBytes = UTF8Encoder.GetBytes(inputText);
			
			//convert back to a string
			return Convert.ToBase64String(EncryptDecrypt(inputBytes,true));
		}

		/// <summary>
		///		Encrypt string with user defined password
		/// </summary>
		/// <param name="inputText">text to encrypt</param>
		/// <param name="password">password to use when encrypting</param>
		/// <returns>an encrypted string</returns>
		public string Encrypt(string inputText, string password)
		{
			this.Password = password;
			return this.Encrypt(inputText);
		}

		/// <summary>
		///		Encrypt string acc. to cryptoType and with user defined password
		/// </summary>
		/// <param name="inputText">text to encrypt</param>
		/// <param name="password">password to use when encrypting</param>
		/// <param name="cryptoType">type of encryption</param>
		/// <returns>an encrypted string</returns>
		public string Encrypt(string inputText, string password, CryptoTypes cryptoType)
		{
			mCryptoType = cryptoType;
			return this.Encrypt(inputText,password);
		}
		
		/// <summary>
		///		Encrypt string acc. to cryptoType
		/// </summary>
		/// <param name="inputText">text to encrypt</param>
		/// <param name="cryptoType">type of encryption</param>
		/// <returns>an encrypted string</returns>
		public string Encrypt(string inputText, CryptoTypes cryptoType)
		{
			this.CryptoType = cryptoType;
			return this.Encrypt(inputText);
		}

		#endregion

		#region Decryption

		/// <summary>
		///		decrypts a string
		/// </summary>
		/// <param name="inputText">string to decrypt</param>
		/// <returns>a decrypted string</returns>
		public string Decrypt(string inputText)
		{	
			//declare a new encoder
			var UTF8Encoder = new UTF8Encoding();
			//get byte representation of string
			var inputBytes = Convert.FromBase64String(inputText);

			//convert back to a string
			return UTF8Encoder.GetString(EncryptDecrypt(inputBytes,false));
		}

		/// <summary>
		///		decrypts a string using a user defined password key
		/// </summary>
		/// <param name="inputText">string to decrypt</param>
		/// <param name="password">password to use when decrypting</param>
		/// <returns>a decrypted string</returns>
		public string Decrypt(string inputText, string password)
		{
			this.Password = password;
			return Decrypt(inputText);
		}

		/// <summary>
		///		decrypts a string acc. to decryption type and user defined password key
		/// </summary>
		/// <param name="inputText">string to decrypt</param>
		/// <param name="password">password key used to decrypt</param>
		/// <param name="cryptoType">type of decryption</param>
		/// <returns>a decrypted string</returns>
		public string Decrypt(string inputText, string password, CryptoTypes cryptoType)
		{
			mCryptoType = cryptoType;
			return Decrypt(inputText,password);
		}

		/// <summary>
		///		decrypts a string acc. to the decryption type
		/// </summary>
		/// <param name="inputText">string to decrypt</param>
		/// <param name="cryptoType">type of decryption</param>
		/// <returns>a decrypted string</returns>
		public string Decrypt(string inputText, CryptoTypes cryptoType)
		{
			this.CryptoType = cryptoType;
			return Decrypt(inputText);
		}
		#endregion

		#region Symmetric Engine

		/// <summary>
		///		performs the actual enc/dec.
		/// </summary>
		/// <param name="inputBytes">input byte array</param>
		/// <param name="Encrpyt">wheather or not to perform enc/dec</param>
		/// <returns>byte array output</returns>
		private byte[] EncryptDecrypt(byte[] inputBytes, bool Encrpyt)
		{
			//get the correct transform
			var transform = getCryptoTransform(Encrpyt);

			//memory stream for output
			var memStream = new MemoryStream();	

			try 
			{
				//setup the cryption - output written to memstream
				var cryptStream = new CryptoStream(memStream,transform,CryptoStreamMode.Write);

				//write data to cryption engine
				cryptStream.Write(inputBytes,0,inputBytes.Length);

				//we are finished
				cryptStream.FlushFinalBlock();
				
				//get result
				var output = memStream.ToArray();

				//finished with engine, so close the stream
				cryptStream.Close();

				return output;
			}
			catch (Exception e) 
			{
				//throw an error
				throw new Exception("Error in symmetric engine. Error : " + e.Message,e);
			}
		}

		/// <summary>
		///		returns the symmetric engine and creates the encyptor/decryptor
		/// </summary>
		/// <param name="encrypt">whether to return a encrpytor or decryptor</param>
		/// <returns>ICryptoTransform</returns>
		private ICryptoTransform getCryptoTransform(bool encrypt)
		{
			var SA = selectAlgorithm();
			SA.Key = mKey;
			SA.IV = mIV;
			if (encrypt) 
			{
				return SA.CreateEncryptor();
			}
			else 
			{
				return SA.CreateDecryptor();
			}
		}
		/// <summary>
		///		returns the specific symmetric algorithm acc. to the cryptotype
		/// </summary>
		/// <returns>SymmetricAlgorithm</returns>
		private SymmetricAlgorithm selectAlgorithm()
		{
			SymmetricAlgorithm SA;
			switch (mCryptoType)
			{
				case CryptoTypes.encTypeDES:
					SA = DES.Create();
					break;
				case CryptoTypes.encTypeRC2:
					SA = RC2.Create();
					break;
				case CryptoTypes.encTypeRijndael:
					SA = Rijndael.Create();
					break;
				case CryptoTypes.encTypeTripleDES:
					SA = TripleDES.Create();
					break;
				default:
					SA = TripleDES.Create();
					break;
			}
			return SA;
		}

		/// <summary>
		///		calculates the key and IV acc. to the symmetric method from the password
		///		key and IV size dependant on symmetric method
		/// </summary>
		private void calculateNewKeyAndIV()
		{
			//use salt so that key cannot be found with dictionary attack
			var pdb = new PasswordDeriveBytes(mPassword,SaltByteArray);
			var algo = selectAlgorithm();
			mKey = pdb.GetBytes(algo.KeySize / 8);
			mIV = pdb.GetBytes(algo.BlockSize / 8);
		}

		#endregion
	}

}



