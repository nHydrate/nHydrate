#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
namespace nHydrate.Generator.Common.Util
{
	public class EmbeddedResourceName
	{
		#region members
		private string _fullName = string.Empty;
		private string _fileName = string.Empty;
		private string _asmLocation = string.Empty;
		#endregion

		#region construction
		public EmbeddedResourceName()
		{
		}

		public EmbeddedResourceName(string resourceName)
		{
			var splitResourceName = resourceName.Split('.');
			for(var ii = 0; ii < splitResourceName.Length -2; ii++)
			{
				_asmLocation += splitResourceName[ii];
				if (ii < splitResourceName.Length - 3)
				{
					_asmLocation += ".";
				}
			}
			_fullName = resourceName;
			_fileName = splitResourceName[splitResourceName.Length - 2] + "." + splitResourceName[splitResourceName.Length - 1];
		}
		#endregion

		#region properties
		public string FullName
		{
			get { return _fullName; }
			set { _fullName = value; }
		}

		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}

		public string AsmLocation
		{
			get { return _asmLocation; }
			set { _asmLocation = value; }
		}
		#endregion
	}
}

