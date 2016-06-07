using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace ConvertCode
{
    /// <summary>
    /// Summary description for AspStringBlock.
    /// </summary>
    public class AspStringBlock
    {
        private string _inputString;
        private bool _codeBlock;

        #region Constructors
        public AspStringBlock(string inputString, bool codeBlock)
        {
            _inputString = inputString;
            if (!_inputString.EndsWith(System.Environment.NewLine))
            {
                _inputString = _inputString + System.Environment.NewLine;
            }
            _codeBlock = codeBlock;
        }
        #endregion

        public void ForceTabs()
        {
            _inputString = _inputString.Replace("\r\n", "\n");
            _inputString = _inputString.Replace("\r", "\n");
            var lines = _inputString.Split(new char[] { '\n' });

            for (var ii = 0; ii < lines.Length; ii++)
            {
                var count = 0;
                foreach (var c in lines[ii])
                {
                    if (c == ' ') count++;
                    else break;
                }

                if (count > 0 && count % 4 == 0)
                {
                    lines[ii] = lines[ii].Remove(0, count);
                    lines[ii] = new string('\t', count / 4) + lines[ii];
                }
            }

            _inputString = string.Join("\r\n", lines);

        }

        public string GetOutput()
        {
            if (_codeBlock)
                return _inputString;
            else
                return GetAspStringBlock();
        }

        private string GetAspStringBlock()
        {
            var linesRegEx = new Regex("(.*)" + System.Environment.NewLine);
            var inLineAspRegEx = new Regex("<%=(.*?)%>");
            var sb = new StringBuilder();

            const string EMPTYCHECK = "sb.AppendLine(\"\");";
            const string EMPTY = "sb.AppendLine();";

            MatchCollection lineMatches = linesRegEx.Matches(_inputString);
            foreach (Match lineMatch in lineMatches)
            {
                string lineText = "sb.AppendLine(";
                string line = lineMatch.Groups[1].Value;
                MatchCollection inLineAspTags = inLineAspRegEx.Matches(line);
                int currentPosition = 0;
                foreach (Match inLineMatch in inLineAspTags)
                {
                    string stringBlock = line.Substring(currentPosition, inLineMatch.Index - currentPosition);
                    stringBlock = ReplaceQuotes(stringBlock);
                    string codeBlock = inLineMatch.Groups[1].Value;
                    lineText += "\"" + stringBlock + "\"" + " + " + codeBlock + " + ";
                    currentPosition = inLineMatch.Index + inLineMatch.Length;
                }
                string lastBlock = line.Substring(currentPosition, line.Length - currentPosition);
                lastBlock = ReplaceQuotes(lastBlock);
                lineText += "\"" + lastBlock + "\");";

                if (lineText == EMPTYCHECK) sb.AppendLine(EMPTY);
                else sb.AppendLine(lineText);
            }

            return sb.ToString();
        }

        private string ReplaceQuotes(string input)
        {
            input = input.Replace(@"\", @"\\");
            return input.Replace("\"", "\\\"");
        }

        public static string ConvertCode(string inAsp, bool forceTabs)
        {
            var outputStrings = new List<AspStringBlock>();
            var codeSegmentRegex = new Regex(@"<%([^=][\s\S]*?[^\\]|[^=])%>");
            var codeSegments = codeSegmentRegex.Matches(inAsp);
            var currentPosition = 0;
            foreach (Match codeSegment in codeSegments)
            {
                var stringBlock = new AspStringBlock(inAsp.Substring(currentPosition, codeSegment.Index - currentPosition), false);
                var codeBlock = new AspStringBlock(codeSegment.Groups[1].Value, true);
                outputStrings.Add(stringBlock);
                outputStrings.Add(codeBlock);
                currentPosition = codeSegment.Index + codeSegment.Length;
            }
            AspStringBlock lastBlock = new AspStringBlock(inAsp.Substring(currentPosition, inAsp.Length - currentPosition), false);
            outputStrings.Add(lastBlock);

            if (forceTabs) outputStrings.ForEach(x => x.ForceTabs());

            var output = new StringBuilder();
            foreach (var outputStringBlock in outputStrings)
            {
                output.Append(outputStringBlock.GetOutput());
            }

            return output.ToString();
        }

    }
}