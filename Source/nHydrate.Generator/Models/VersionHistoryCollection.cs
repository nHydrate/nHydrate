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
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class VersionHistoryCollection : List<VersionHistory>, IXMLable
	{
		#region Member Variables

		#endregion

		#region Constructor

		public VersionHistoryCollection()
		{
		}

		#endregion

		#region Property Implementations

		#endregion

		#region IXMLable Members

		public void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;
				foreach (var item in this)
				{
					var newNode = XmlHelper.AddElement((XmlElement)node, "version");
					item.XmlAppend(newNode);
				}

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public void XmlLoad(XmlNode node)
		{
			var nodeList = node.SelectNodes("version");
			foreach (XmlNode childNode in nodeList)
			{
				var newVersion = new VersionHistory();
				newVersion.XmlLoad(childNode);
				this.Add(newVersion);
			}
		}

		#endregion

	}
}

