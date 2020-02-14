using System;
using System.Security.Cryptography;
using System.Text;

namespace nHydrate.Generator.Common.Util
{
    public class HashHelper
    {
        #region Private member variables...
        private string mSalt;
        private readonly HashAlgorithm mCryptoService;
        #endregion

        #region Public interfaces...

        public virtual string Encrypt(string plainText)
        {
            var cryptoByte = mCryptoService.ComputeHash(
                ASCIIEncoding.ASCII.GetBytes(plainText + mSalt));

            // Convert into base 64 to enable result to be used in Xml
            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.Length);
        }

        public static string Hash(string plainText)
        {
            var mCryptoService = new SHA1Managed();
            var cryptoByte = mCryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(plainText));

            // Convert into base 64 to enable result to be used in Xml
            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.Length);
        }

        public static string HashAlphaNumeric(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                var v = (int)c % 36;
                if (v <= 25)
                    sb.Append((char)(v + 97));
                else
                    sb.Append((char)(v - 26 + 48));
            }
            return sb.ToString();
        }

        public bool Match(string input, string hashValue)
        {
            return StringHelper.Match(this.Encrypt(input), hashValue, true);
        }

        #endregion
    }

}
