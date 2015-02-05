#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.Text.RegularExpressions;

namespace nHydrate.Generator.Common.Util
{
	class RegExHelper
	{
		public enum RegexType
		{
			Email,
			ZipCode,
			Year,
			ComplexPassword,
			SpecialCharacters
		}

		public static bool ValidateAgainstRegex(RegexType expressionType, string text)
		{
			var ExpressionString = string.Empty;

			switch (expressionType)
			{
				case RegexType.Email:
					ExpressionString = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
					break;
				case RegexType.ZipCode:
					ExpressionString = "\\d{5}(-\\d{4})?";
					break;
				case RegexType.Year:
					ExpressionString = "\\d{4}";
					break;
				case RegexType.ComplexPassword:
					ExpressionString = "\\w{6,}";
					break;
				case RegexType.SpecialCharacters:
					ExpressionString = @"[^\w\.@-]";
					break;
			}

			return ValidateAgainstRegex(ExpressionString, text);
		}

		public static bool ValidateAgainstRegex(string expressionString, string text)
		{
			var oRegex = new Regex(expressionString);
			var oMatch = oRegex.Match(text);
			if (oMatch.Success)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

	}
}

