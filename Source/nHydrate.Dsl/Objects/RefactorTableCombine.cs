#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Xml;

namespace nHydrate.Dsl.Objects
{
	public class RefactorTableCombine : IRefactor
	{
		public RefactorTableCombine()
		{
			this.ReMappedFieldIDList = new Dictionary<Guid, Guid>();
		}

		public RefactorTableCombine(XmlElement node)
			: this()
		{
			this.FromXML(node);
		}

		public Guid Id { get; set; }
		public Guid EntityKey1 { get; set; }
		public Guid EntityKey2 { get; set; }

		public Dictionary<Guid, Guid> ReMappedFieldIDList { get; private set; }

		public void ToXML(System.Xml.XmlElement node)
		{
			try
			{
				XmlHelper.AddAttribute(node, "type", "combinetable");
				XmlHelper.AddAttribute(node, "entityid1", this.EntityKey1.ToString());
				XmlHelper.AddAttribute(node, "entityid2", this.EntityKey1.ToString());

				var fieldsNode = XmlHelper.AddElement(node, "fields");
				foreach (var k in this.ReMappedFieldIDList.Keys)
				{
					var n = XmlHelper.AddElement((XmlElement)fieldsNode, "field");
					XmlHelper.AddAttribute(n, "source", k.ToString());
					XmlHelper.AddAttribute(n, "target", this.ReMappedFieldIDList[k].ToString());
				}
			}
			catch (Exception ex)
			{
				//Do Nothing
			}
		}

		public void FromXML(System.Xml.XmlElement node)
		{
			try
			{
				this.EntityKey1 = new Guid(XmlHelper.GetAttributeValue(node, "entityid1", this.EntityKey1.ToString()));
				this.EntityKey2 = new Guid(XmlHelper.GetAttributeValue(node, "entityid2", this.EntityKey1.ToString()));

				var fieldsNode = node.SelectSingleNode("fields");
				if (fieldsNode != null)
				{
					foreach (XmlNode n in fieldsNode.ChildNodes)
					{
						var source = XmlHelper.GetAttributeValue(n, "source", Guid.Empty);
						var target = XmlHelper.GetAttributeValue(n, "target", Guid.Empty);
						this.ReMappedFieldIDList.Add(source, target);
					}
				}
			}
			catch (Exception ex)
			{
				//Do Nothing
			}
		}

	}
}

