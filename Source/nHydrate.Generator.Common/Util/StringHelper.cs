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
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace nHydrate.Generator.Common.Util
{
	public class StringHelper
	{

		private StringHelper()
		{
		}

		public static bool GuidTryParse(string s, out Guid result)
		{
			if (s == null)
				throw new ArgumentNullException("s");
			var format = new Regex(
					"^[A-Fa-f0-9]{32}$|" +
					"^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
					"^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
			var match = format.Match(s);
			if (match.Success)
			{
				result = new Guid(s);
				return true;
			}
			else
			{
				result = Guid.Empty;
				return false;
			}
		}
		public static string FirstCharToUpper(string inputString)
		{
			var sb = new StringBuilder();
			if (inputString.Length > 0)
			{
				sb.Append(inputString.Substring(0, 1).ToUpper()).Append(inputString.Substring(1, inputString.Length - 1));
			}
			return sb.ToString();
		}

		public static string FirstCharToLower(string inputString)
		{
			var sb = new StringBuilder();
			if (inputString.Length > 0)
			{
				sb.Append(inputString.Substring(0, 1).ToLower()).Append(inputString.Substring(1, inputString.Length - 1));
			}
			return sb.ToString();

		}

		/// <summary>
		/// Case Insensitive String Replace
		/// </summary>
		public static string StringReplace(string text, string oldValue, string newValue)
		{
			var iPos = text.ToLower().IndexOf(oldValue.ToLower());
			var retval = string.Empty;
			while (iPos != -1)
			{
				retval += text.Substring(0, iPos) + newValue;
				text = text.Substring(iPos + oldValue.Length);
				iPos = text.ToLower().IndexOf(oldValue.ToLower());
			}
			if (text.Length > 0)
				retval += text;
			return retval;
		}

		/// <summary>
		/// Take the specified text and break it into lines and write it as a C# comment
		/// </summary>
		/// <param name="tabCount">The number of preceding tabs</param>
		/// <param name="writer">The string builder to write the text</param>
		/// <param name="text">The text to process</param>
		public static void WriteGeneratedCommentSection(int tabCount, StringBuilder writer, string text)
		{
			if (string.IsNullOrEmpty(text)) return;
			if (tabCount < 0) tabCount = 0;
			text = text.Replace("\r\n", "\n");
			text = text.Replace("\r", "\n");
			var arr = text.Split('\n');

			foreach (var s in arr)
			{
				writer.AppendLine(new string('	', tabCount) + "/// " + s);
			}

		}

		/// <summary>
		/// Convert the specified text to a single line text
		/// </summary>
		public static string ConvertTextToSingleLineCodeString(string text)
		{
			return ConvertTextToSingleLineCodeString(text, false);
		}

		/// <summary>
		/// Convert the specified text to a single line text
		/// </summary>
		public static string ConvertTextToSingleLineCodeString(string text, bool convertBreaks)
		{
			if (string.IsNullOrEmpty(text)) return string.Empty;
			text = text.Replace("\r\n", "\n");
			text = text.Replace("\r", "\n");
			var arr = text.Split('\n');

			var retval = string.Empty;
			if (convertBreaks)
				retval = string.Join(@"\n", arr);
			else
				retval = string.Join(" ", arr);

			//Replace quotes with escaped chars
			return retval.Trim().Replace("\"", "\\\"");
		}

		/// <summary>
		/// Given text prepend the specified prefix and add each line to a string builder
		/// </summary>
		/// <param name="sb">The StringBuilder to which to add the processed lines</param>
		/// <param name="text">The text to break into lines</param>
		/// <param name="prepend">The text to prepend each line</param>
		public static void LineBreakCode(StringBuilder sb, string text, string prepend)
		{
			(text + string.Empty)
				.Replace("\r", string.Empty)
				.Split('\n')
				.ToList()
				.ForEach(x => sb.AppendLine(prepend + x));
		}

		#region String Match
		public static bool Match(object s1, string s2, bool ignoreCase)
		{
			if (s1 == null)
				if (s2 == null) return true;
				else return false;
			else
				if (s2 == null) return false;
				else if (s1.ToString().Length != s2.Length) return false;
				else if (s1.ToString().Length == 0) return true;

			return (String.Compare(s1.ToString(), s2, ignoreCase) == 0);
		}

		public static bool Match(string s1, string s2, bool ignoreCase)
		{
			if (s1 == null)
				if (s2 == null) return true;
				else return false;
			else
				if (s2 == null) return false;
				else if (s1.Length != s2.Length) return false;
				else if (s1.Length == 0) return true;

			return (String.Compare(s1, s2, ignoreCase) == 0);
		}

		public static bool Match(string s1, string s2)
		{
			return Match(s1, s2, true);
		}
		#endregion

		#region Variable Case Conversion
		public static string MakeValidDatabaseCaseVariableName(string inputString)
		{
			var pascalCase = MakeValidPascalCaseVariableName(inputString);
			return PascalCaseToDatabase(pascalCase);
		}

		public static string MakeValidCamelCaseVariableName(string inputString)
		{
			var camelCase = MakeValidPascalCaseVariableName(inputString);
			if (camelCase.Length > 0)
			{
				camelCase = camelCase.Insert(0, camelCase[0].ToString().ToLower());
				camelCase = camelCase.Remove(1, 1);
			}
			return camelCase;
		}

		public static string MakeValidPascalCaseVariableName(string inputString)
		{
			var output = new StringBuilder();
			var regexp = "[A-Z,a-z,0-9]+";
			var matches = Regex.Matches(inputString, regexp);
			foreach (Match match in matches)
			{
				var appendString = match.Value;
				appendString = appendString.Insert(0, appendString[0].ToString().ToUpper());
				appendString = appendString.Remove(1, 1);
				output.Append(appendString);
			}
			var returnVal = output.ToString();
			returnVal = returnVal.TrimStart(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
			if (returnVal.Length < 0)
				throw new Exception("Cannot turn string( " + inputString + " ) into a valid variable name");
			return returnVal;
		}

		public static string DatabaseNameToCamelCase(string databaseName)
		{
			databaseName = databaseName.ToLower();
			var regexp = "_.";
			var digitregex = new Regex(regexp);
			var parameterName = digitregex.Replace(databaseName, new MatchEvaluator(ReplaceWithUpper));
			return parameterName;
		}

		public static string DatabaseNameToPascalCase(string databaseName)
		{
			var pascalCase = DatabaseNameToCamelCase(databaseName);
			if (pascalCase.Length > 0)
			{
				pascalCase = pascalCase.Insert(0, pascalCase[0].ToString().ToUpper());
				pascalCase = pascalCase.Remove(1, 1);
			}
			return pascalCase;
		}

		public static string PascalCaseToDatabase(string pascalCase)
		{
			var digitregex = new Regex("(?<caps>[A-Z])");
			var parameterName = digitregex.Replace(pascalCase, "_$+");
			parameterName = parameterName.ToLower().TrimStart('_');
			return parameterName;
		}

		public static string CamelCaseToDatabase(string camelCase)
		{
			return PascalCaseToDatabase(camelCase);
		}

		private static string ReplaceWithUpper(Match m)
		{
			var character = m.ToString().TrimStart('_');
			return character.ToUpper();
		}
		#endregion

		#region File Path Conversions
		public static string EnsureDirectorySeperatorAtEnd(string directory)
		{
			if (!(directory.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString())))
			{
				directory += System.IO.Path.DirectorySeparatorChar;
			}
			return directory;
		}
		#endregion

		#region byte array conversions
		public static MemoryStream StringToMemoryStream(string str)
		{
			var ms = new MemoryStream(StringToByteArray(str));
			return ms;
		}

		public static String MemoryStreamToString(MemoryStream memStream)
		{
			return ByteArrayToString(memStream.GetBuffer(), (int)memStream.Length);
		}

		public static Byte[] StringToByteArray(string str)
		{
			var enc = new UTF8Encoding();
			return enc.GetBytes(str);
		}

		public static string ByteArrayToHexString(byte[] bytes)
		{
			var hexString = string.Empty;
			for (var i = 0; i < bytes.Length; i++)
			{
				hexString += bytes[i].ToString("X2");
			}
			return hexString;
		}


		public static string ByteArrayToString(byte[] byteArray)
		{
			var enc = new UTF8Encoding();
			return enc.GetString(byteArray, 0, byteArray.Length);
		}

		public static string ByteArrayToString(byte[] byteArray, Encoding encoder)
		{
			return encoder.GetString(byteArray, 0, byteArray.Length);
		}

		public static string ByteArrayToString(byte[] byteArray, int length)
		{
			var enc = new UTF8Encoding();
			return enc.GetString(byteArray, 0, length);
		}
		#endregion
	}
}
