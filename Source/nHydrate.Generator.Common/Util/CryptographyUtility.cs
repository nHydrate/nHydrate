using System;
using System.Security.Cryptography;

namespace nHydrate.Generator.Common.Util
{
	public sealed class CryptographyUtility
	{
		private CryptographyUtility()
		{}
		
		public static string GetEntropy(int length)
		{
			return ByteEncoding.BytesToBase64(GetRandomBytes(length));
		}

		public static byte[] GetRandomBytes(int length)
		{
			var randNumGen = RandomNumberGenerator.Create();
			var entropy = new byte[length];
			randNumGen.GetBytes(entropy);
			return entropy;
		}

		public static void ZeroMemory(byte[] pByte)
		{
			Array.Clear(pByte, 0, pByte.Length);
		}
	}
}

