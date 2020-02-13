using System;
using System.Text;

namespace nHydrate.Generator.Common.Util
{
	public sealed class ByteEncoding
	{
				
		private ByteEncoding()
		{}

		#region Hex

		private static int GetByteCount(string hexString)
		{
			var numHexChars = 0;
			
			for (var i=0; i < hexString.Length; i++)
			{
				var c = hexString[i];
				if (IsHexDigit(c))
					numHexChars++;
			}
			
			return numHexChars / 2; 		}

		public static byte[] HexToBytes(string hexString)
		{
			int discarded;
			return HexToBytes(hexString, out discarded);
		}

		public static byte[] HexToBytes(string hexString, out int discarded)
		{
			discarded = 0;
			var newString = new StringBuilder(hexString.Length);
			char c;
			for (var i=0; i < hexString.Length; i++)
			{
				c = hexString[i];
				if (IsHexDigit(c))
					newString.Append(c);
				else
					discarded++;
			}
			if (newString.Length % 2 != 0)
			{
				discarded++;
				newString = newString.Remove(newString.Length-1, 1);
			}

			var byteLength = newString.Length / 2;
			var bytes = new byte[byteLength];
			for (int i=0, j = 0; i < byteLength; i++, j+=2)
			{
				var hex = new String(new Char[] {newString[j], newString[j+1]});
				bytes[i] = HexToByte(hex);
			}

			return bytes;
		}


		public static string BytesToHex(byte[] bytes)
		{
			var hexString = new StringBuilder(bytes.Length);
			for (var i=0; i < bytes.Length; i++)
			{
				hexString.Append(bytes[i].ToString("X2"));
			}
			return hexString.ToString();
		}


		public static bool InHexFormat(string hexString)
		{
			foreach (var digit in hexString)
			{
				if (!IsHexDigit(digit))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsHexDigit(Char c)
		{
			var numA = Convert.ToInt32('A');
			var num1 = Convert.ToInt32('0');
			c = Char.ToUpper(c);
			var numChar = Convert.ToInt32(c);
			if (numChar >= numA && numChar < (numA + 6)
				|| numChar >= num1 && numChar < (num1 + 10))
				return true;
			return false;
		}
		private static byte HexToByte(string hex)
		{
			if (hex.Length > 2 || hex.Length <= 0)
				throw new ArgumentException("hex must be 1 or 2 characters in length");
			return byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
		}

		#endregion

		#region Base64

		public static string BytesToBase64(byte[] bytes)
		{
			return Convert.ToBase64String(bytes);
		}

		public static byte[] Base64ToBytes( string b64String)
		{
			return Convert.FromBase64String(b64String);
		}

		#endregion

		#region String

		public static string BytesToString(byte[] bytes)
		{
			return System.Text.Encoding.Unicode.GetString(bytes);
		}

		public static byte[] StringToBytes(string data)
		{
			return System.Text.Encoding.Unicode.GetBytes(data);
		}

		#endregion


	}
}

