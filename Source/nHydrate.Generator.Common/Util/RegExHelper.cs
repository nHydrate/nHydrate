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

