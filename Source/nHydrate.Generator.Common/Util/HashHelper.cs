using System;
using System.Security.Cryptography;
using System.Text;

namespace nHydrate.Generator.Common.Util
{
    public class HashHelper
    {
        #region Private member variables...
        private string _salt;
        private readonly HashAlgorithm _cryptoService;
        #endregion

        #region Public interfaces...

        public virtual string Encrypt(string plainText)
        {
            var cryptoByte = _cryptoService.ComputeHash(
                ASCIIEncoding.ASCII.GetBytes(plainText + _salt));

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

        public bool Match(string input, string hashValue)
        {
            return StringHelper.Match(this.Encrypt(input), hashValue, true);
        }

        #endregion
    }

}
