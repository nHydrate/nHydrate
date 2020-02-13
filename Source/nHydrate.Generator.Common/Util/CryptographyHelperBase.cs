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

