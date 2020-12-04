using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace nHydrate.Generator.Common.Util
{
    public static class StringHelper
    {
        public static bool GuidTryParse(string s, out Guid result)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
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
            if (inputString.IsEmpty()) return string.Empty;
            var sb = new StringBuilder();
            if (inputString.Length > 0)
            {
                sb.Append(inputString.Substring(0, 1).ToUpper()).Append(inputString.Substring(1, inputString.Length - 1));
            }
            return sb.ToString();
        }

        public static string FirstCharToLower(string inputString)
        {
            if (inputString.IsEmpty()) return string.Empty;
            var sb = new StringBuilder();
            if (inputString.Length > 0)
            {
                sb.Append(inputString.Substring(0, 1).ToLower()).Append(inputString.Substring(1, inputString.Length - 1));
            }
            return sb.ToString();

        }

        /// <summary>
        /// Convert the specified text to a single line text
        /// </summary>
        public static string ConvertTextToSingleLineCodeString(string text) => ConvertTextToSingleLineCodeString(text, false);

        /// <summary>
        /// Convert the specified text to a single line text
        /// </summary>
        public static string ConvertTextToSingleLineCodeString(string text, bool convertBreaks)
        {
            if (text.IsEmpty()) return string.Empty;
            //Replace quotes with escaped chars
            if (convertBreaks)
                return string.Join(@"\n", text.BreakLines()).Trim().Replace("\"", "\\\"");
            else
                return string.Join(" ", text.BreakLines()).Trim().Replace("\"", "\\\"");
        }

        /// <summary>
        /// Given text prepend the specified prefix and add each line to a string builder
        /// </summary>
        /// <param name="sb">The StringBuilder to which to add the processed lines</param>
        /// <param name="text">The text to break into lines</param>
        /// <param name="prepend">The text to prepend each line</param>
        public static void LineBreakCode(StringBuilder sb, string text, string prepend)
        {
            (text + string.Empty).BreakLines().ForEach(x => sb.AppendLine(prepend + x));
        }

        public static bool Match(this string s1, string s2, bool ignoreCase = true)
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

        #region Variable Case Conversion

        public static string DatabaseNameToCamelCase(string databaseName)
        {
            databaseName = databaseName.ToLower();
            const string regexp = "_.";
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

        private static string ReplaceWithUpper(Match m) => m.ToString().TrimStart('_').ToUpper();

        #endregion

        #region File Path Conversions
        public static string EnsureDirectorySeparatorAtEnd(string directory)
        {
            if (!(directory.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString())))
            {
                directory += System.IO.Path.DirectorySeparatorChar;
            }
            return directory;
        }
        #endregion

    }
}
