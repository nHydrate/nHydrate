using System.Security.Cryptography;

namespace nHydrate.Generator.Common.Util
{
	public enum CryptographyAlgorithm 
	{
		Des,
		Rc2,
		Rijndael, 
		TripleDes
	}

	public sealed class CryptoFactory
	{
		private CryptoFactory(){}

		#region Public Create Methods

		public static ICryptoHelper Create(CryptographyAlgorithm algorithm)
		{
			switch(algorithm)
			{
				case CryptographyAlgorithm.Des:
				case CryptographyAlgorithm.Rc2:
				case CryptographyAlgorithm.Rijndael:
				case CryptographyAlgorithm.TripleDes:
					return new SymmetricCryptographyHelper(algorithm);
				default:
				{
					throw new CryptographicException("Algorithm '" + 
						algorithm + 
						"' not supported.");
				}
			}
			
		}

		public static ICryptoHelper Create(CryptographyAlgorithm algorithm, string entropy)
		{
			switch(algorithm)
			{
				case CryptographyAlgorithm.Des:
				case CryptographyAlgorithm.Rc2:
				case CryptographyAlgorithm.Rijndael:
				case CryptographyAlgorithm.TripleDes:
					return new SymmetricCryptographyHelper(algorithm, entropy);
				default:
				{
					throw new CryptographicException("Algorithm '" + 
						algorithm + 
						"' not supported.");
				}
			}
		}

		public static ICryptoHelper Create(string algorithmName, string entropy)
		{
			var algorithm = (CryptographyAlgorithm)System.Enum.Parse(typeof(CryptographyAlgorithm), algorithmName, true);
			return Create(algorithm, entropy);
		}

		#endregion

	}
}

#if UNITTEST
namespace nHydrate.Generator.Common.Util.UnitTest
{
	using NUnit.Framework;	
	
	[TestFixture] 
	public class CryptoFactoryTester
	{
		private const string ENCRYPTION_TEST_STRING = "Help;Me;To;Encrypt;SomethingValueable";
		private const string ENCRYPTION_VALID_PASSWORD = "Password";
		private const string ENCRYPTION_INVALID_PASSWORD = "Password2";
		
		[Test]
		public void RijndaelEncrypt()
		{
			ICryptoHelper crytographyHelper = CryptoFactory.Create(CryptographyAlgorithm.Rijndael);
			crytographyHelper.Entropy = ENCRYPTION_VALID_PASSWORD;
			string encryptedString = crytographyHelper.Encrypt(ENCRYPTION_TEST_STRING, StringEncodingType.Hex);
			Assert.IsFalse(StringHelper.Match(encryptedString,ENCRYPTION_TEST_STRING,true), "Encrypted String Equals String Passed In");

		}

		[Test]
		[ExpectedException(typeof(System.Security.Cryptography.CryptographicException))]
		public void RijndaelDecryptInvalidPassword()
		{
			ICryptoHelper crytographyHelper = CryptoFactory.Create(CryptographyAlgorithm.Rijndael);
			crytographyHelper.Entropy = ENCRYPTION_VALID_PASSWORD;
			string encryptedString = crytographyHelper.Encrypt(ENCRYPTION_TEST_STRING, StringEncodingType.Hex);

			ICryptoHelper crytographyHelper2 = CryptoFactory.Create(CryptographyAlgorithm.Rijndael);
			crytographyHelper2.Entropy = ENCRYPTION_INVALID_PASSWORD;
			string decriptedString = crytographyHelper2.Decrypt(encryptedString,StringEncodingType.Hex);

			Assert.AreEqual(ENCRYPTION_TEST_STRING, decriptedString);
		}

		[Test]
		public void RijndaelDecryptValidPassword()
		{
			ICryptoHelper crytographyHelper = CryptoFactory.Create(CryptographyAlgorithm.Rijndael);
			crytographyHelper.Entropy = ENCRYPTION_VALID_PASSWORD;
			string encryptedString = crytographyHelper.Encrypt(ENCRYPTION_TEST_STRING, StringEncodingType.Hex);

			ICryptoHelper crytographyHelper2 = CryptoFactory.Create(CryptographyAlgorithm.Rijndael);
			crytographyHelper2.Entropy = ENCRYPTION_VALID_PASSWORD;
			string decriptedString = crytographyHelper2.Decrypt(encryptedString,StringEncodingType.Hex);

						Assert.AreEqual(ENCRYPTION_TEST_STRING, decriptedString);
		}

	}
}
#endif

