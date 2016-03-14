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

        public enum ServiceProviderEnum : int
        {
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            MD5
        }

        public HashHelper()
        {
            mCryptoService = new SHA1Managed();
        }

        public HashHelper(ServiceProviderEnum serviceProvider)
        {
            // Select hash algorithm
            switch (serviceProvider)
            {
                case ServiceProviderEnum.MD5:
                    mCryptoService = new MD5CryptoServiceProvider();
                    break;
                case ServiceProviderEnum.SHA1:
                    mCryptoService = new SHA1Managed();
                    break;
                case ServiceProviderEnum.SHA256:
                    mCryptoService = new SHA256Managed();
                    break;
                case ServiceProviderEnum.SHA384:
                    mCryptoService = new SHA384Managed();
                    break;
                case ServiceProviderEnum.SHA512:
                    mCryptoService = new SHA512Managed();
                    break;
            }
        }

        public HashHelper(string serviceProviderName)
        {
            try
            {
                // Set Hash algorithm
                mCryptoService = (HashAlgorithm)CryptoConfig.CreateFromName(
                    serviceProviderName.ToUpper());
            }
            catch
            {
                throw;
            }
        }

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

        public string Salt
        {
            // Salt value
            get { return mSalt; }
            set { mSalt = value; }
        }
        #endregion
    }

}