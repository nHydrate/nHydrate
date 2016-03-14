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
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace nHydrate.Generator.Common.Util
{
	/// <summary>
	/// Summary description for AspStringBlock.
	/// </summary>
	public class AspStringBlock
	{
		private readonly string _inputString;
		private readonly bool _codeBlock;

		#region Constructors
		public AspStringBlock(string inputString, bool codeBlock)
		{
			_inputString = inputString;
			if(!_inputString.EndsWith(System.Environment.NewLine))
			{
				_inputString = _inputString +  System.Environment.NewLine;
			}
			_codeBlock = codeBlock;
		}
		#endregion

		public string GetOutput()
		{
			if(_codeBlock)
			{
				return _inputString;
			}
			else
			{
				return GetAspStringBlock();
			}
								
		}

		private string GetAspStringBlock()
		{
			var linesRegEx = new Regex("(.*)" + System.Environment.NewLine);
			var inLineAspRegEx = new Regex("<%=(.*?)%>");
			var sb = new StringBuilder();

			var lineMatches = linesRegEx.Matches(_inputString);
			foreach(Match lineMatch in lineMatches)
			{
				sb.Append("sb.AppendLine(");
				var line = lineMatch.Groups[1].Value;
				var inLineAspTags = inLineAspRegEx.Matches(line);
				var currentPosition = 0;
				foreach(Match inLineMatch in inLineAspTags)
				{
					var stringBlock = line.Substring(currentPosition, inLineMatch.Index - currentPosition);
					stringBlock = ReplaceQuotes(stringBlock);
					var codeBlock = inLineMatch.Groups[1].Value;
					sb.Append("\"" + stringBlock + "\"" + " + " + codeBlock + " + ");
					currentPosition = inLineMatch.Index + inLineMatch.Length;
				}
				var lastBlock = line.Substring(currentPosition, line.Length - currentPosition);
				lastBlock = ReplaceQuotes(lastBlock);
				sb.AppendLine("\"" + lastBlock + "\");");				
			}

			return sb.ToString();

		}

		private string ReplaceQuotes(string input)
		{
			input = input.Replace(@"\", @"\\");
			return input.Replace("\"", "\\\"");
		}

		public static string ParceAsp(string inAsp)
		{
			var outputStrings = new ArrayList();

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
			var lastBlock = new AspStringBlock(inAsp.Substring(currentPosition, inAsp.Length - currentPosition), false);
			outputStrings.Add(lastBlock);

			var output = new StringBuilder();
			foreach (AspStringBlock outputStringBlock in outputStrings)
			{
				output.Append(outputStringBlock.GetOutput());
			}


			return output.ToString();
		}


	}
}


