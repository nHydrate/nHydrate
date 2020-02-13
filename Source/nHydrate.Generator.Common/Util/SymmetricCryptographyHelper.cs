using System;
using System.IO;
using System.Security.Cryptography;

namespace nHydrate.Generator.Common.Util
{

	internal class SymmetricCryptographyHelper : CryptographyHelperBase
	{

		#region Private Fields

		protected CryptographyAlgorithm algorithmId; 		protected string entropy;							private SymmetricAlgorithm algorithm;			private int keyLength;					
		#endregion

		#region Properties

		public override CryptographyAlgorithm Algorithm
		{
			get{ return algorithmId;}
		}

		public override string Entropy
		{
			get
			{
				return entropy;
			}
			set
			{
				entropy = value;
			}
		}

		#endregion

		#region Constructors

		public SymmetricCryptographyHelper(CryptographyAlgorithm algId)
		{
			algorithmId = algId;
		}

		public SymmetricCryptographyHelper(CryptographyAlgorithm algId, string password)
		{
			algorithmId = algId;
			entropy = password;
		}

		#endregion

		#region ICryptoHelper Members

		public override byte[] Encrypt( byte[] plaintext )
		{
			if( algorithm == null )
			{
				GetCryptoAlgorithm();
			}

			using(var dataStream = new MemoryStream())
			{
				var salt = GetSalt();
				dataStream.Write(salt, 0, salt.Length);
				algorithm.GenerateIV();
				dataStream.Write(algorithm.IV, 0, algorithm.IV.Length);
				algorithm.Key = GetKey(salt);
				using(var transform = algorithm.CreateEncryptor())
				{
					using(var crypto = 
								new CryptoStream(dataStream, transform, 
								CryptoStreamMode.Write))
					{
						crypto.Write(plaintext, 0, plaintext.Length);
						crypto.FlushFinalBlock();
						dataStream.Flush();
						var encData = dataStream.ToArray();
						crypto.Close();
						return encData;
					}
				}
			}
		}

		
		
		public override byte[] Decrypt(byte[] cipherText)
		{
			if( algorithm == null )
			{
				GetCryptoAlgorithm();
			}

			using(var dataStream = new MemoryStream(cipherText))
			{
				var salt = new byte[16];
				var iv = new byte[algorithm.IV.Length];
				dataStream.Read(salt, 0, salt.Length);
				dataStream.Read(iv, 0, iv.Length);
				algorithm.Key = GetKey(salt);
				algorithm.IV = iv;
				using(var crypto = 
							new CryptoStream(dataStream, algorithm.CreateDecryptor(), 
							CryptoStreamMode.Read))
				{
					var buffer = new byte[256];
					var bytesRead = 0;
					using(var decryptedData = new MemoryStream())
					{
						do
						{
							bytesRead = crypto.Read(buffer, 0, 256);
							decryptedData.Write(buffer, 0, bytesRead);
						}
						while(bytesRead > 0);
						crypto.Close();
						var decData = decryptedData.ToArray();
						decryptedData.Close();
						return decData;
					}
				}
			}
		}

		
		#endregion

		#region Private Methods

		private void GetCryptoAlgorithm()
		{

			switch (algorithmId)
			{
				case CryptographyAlgorithm.Des:
				{
					algorithm = new DESCryptoServiceProvider();
					break;
				}
				case CryptographyAlgorithm.TripleDes:
				{
					algorithm = new TripleDESCryptoServiceProvider();
					break;
				}
				case CryptographyAlgorithm.Rc2:
				{
					algorithm = new RC2CryptoServiceProvider();
					break;
				}
				case CryptographyAlgorithm.Rijndael:
				{
					algorithm = new RijndaelManaged();
					break;
				} 
				default:
				{
					throw new CryptographicException("Algorithm Id '" + 
						algorithmId + 
						"' not supported.");
				}
			}

			algorithm.Mode = CipherMode.CBC;

			keyLength = (algorithm.LegalKeySizes[0].MaxSize / 8);
		}

		private byte[] GetSalt()
		{
			return CryptographyUtility.GetRandomBytes(16);
		}

		private byte[] GetKey(byte[] salt)
		{
			byte[] key;
			if(entropy == null || entropy.Trim().Length == 0)
			{
				entropy = CryptographyUtility.GetEntropy(keyLength);
			}

			var passBytes = new PasswordDeriveBytes(entropy, salt);
			key = passBytes.GetBytes(keyLength);
			
			return key;
		}

		#endregion

		#region IDisposable Members

		public override void Dispose()
		{
			if( algorithm != null )
				algorithm.Clear();
			GC.SuppressFinalize(this);
		}

		#endregion
	}

}

