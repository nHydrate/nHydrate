#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace nHydrate.Generator.Common.Util
{
	public static class XmlHelper
	{
		private static readonly CultureInfo _cultureProvider;

		static XmlHelper()
		{
			_cultureProvider = new CultureInfo("en-US");
		}

		public static XPathNavigator CreateXPathNavigator(XmlReader reader)
		{
			var document = new XPathDocument(reader);
			return document.CreateNavigator();
		}

		public static XPathNodeIterator GetIterator(XPathNavigator navigator, string xPath)
		{
			return (XPathNodeIterator)navigator.Evaluate(xPath);
		}

		#region GetXmlReader

		public static XmlReader GetXmlReader(FileInfo fileInfo)
		{
			var textReader = new XmlTextReader(fileInfo.FullName);
			return textReader;
		}

		#endregion

		#region GetNode

		public static XmlNode GetNode(XmlNode xmlNode, string XPath)
		{
			try
			{
				XmlNode node = null;
				node = xmlNode.SelectSingleNode(XPath);
				return node;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region GetNodeValue

		public static string GetNodeValue(XmlDocument document, string XPath, string defaultValue)
		{
			try
			{
				return GetNodeValue(document.DocumentElement, XPath, defaultValue);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static string GetNodeValue(XmlNode element, string XPath, string defaultValue)
		{
			try
			{
				XmlNode node = null;
				node = element.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else
					return node.InnerText;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region GetNodeXML

		public static string GetNodeXML(XmlDocument document, string XPath, string defaultValue, bool useOuter)
		{
			try
			{
				XmlNode node = null;
				node = document.SelectSingleNode(XPath);
				if (node == null)
					return defaultValue;
				else if (useOuter)
					return node.OuterXml;
				else
					return node.InnerXml;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static string GetNodeXML(XmlDocument document, string XPath, string defaultValue)
		{
			return GetNodeXML(document, XPath, defaultValue, false);
		}

		#endregion

		#region GetAttributeValue

		public static string GetAttributeValue(XmlNode element, string attributeName, string defaultValue)
		{
			try
			{
				var attribute = element.Attributes[attributeName];
				if (attribute == null) return defaultValue;
				else return attribute.Value;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static double GetAttributeValue(XmlNode element, string attributeName, double defaultValue)
		{
			try
			{
				var attribute = element.Attributes[attributeName];
				if (attribute == null) return defaultValue;
				else
				{
					double d;
					if (double.TryParse(attribute.Value, NumberStyles.Number, _cultureProvider, out d))
						return d;
					else
						return defaultValue;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static Guid GetAttributeValue(XmlNode element, string attributeName, Guid defaultValue)
		{
			try
			{
				var attribute = element.Attributes[attributeName];
				if (attribute == null) return defaultValue;
				else
				{
					Guid d;
					if (Guid.TryParse(attribute.Value, out d))
						return d;
					else
						return defaultValue;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static int GetAttributeValue(XmlNode element, string attributeName, int defaultValue)
		{
			try
			{
				var attribute = element.Attributes[attributeName];
				if (attribute == null) return defaultValue;
				else
				{
					int d;
					if (int.TryParse(attribute.Value, NumberStyles.Number, _cultureProvider, out d))
						return d;
					else
						return defaultValue;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static bool GetAttributeValue(XmlNode element, string attributeName, bool defaultValue)
		{
			try
			{
				var attribute = element.Attributes[attributeName];
				if (attribute == null) return defaultValue;
				else if (attribute.Value == "0") return false;
				else if (attribute.Value == "1") return true;
				else
				{
					bool d;
					if (bool.TryParse(attribute.Value, out d))
						return d;
					else
						return defaultValue;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region AddElement

		#region WhiteSpace

		public static void AddLineBreak(XmlElement element)
		{
			element.AppendChild(element.OwnerDocument.CreateWhitespace("\r\n"));
		}

		public static void AddIndent(XmlElement element, int tabs)
		{
			if (tabs <= 0) return;
			element.AppendChild(element.OwnerDocument.CreateWhitespace(new string('\t', tabs)));
		}
		
		#endregion

		#region Add CData

		public static XmlNode AddCData(XmlElement element, string name, string value)
		{
			var n = AddElement(element, name);
			element.AppendChild(n);

			var elemNew = element.OwnerDocument.CreateCDataSection(name);
			elemNew.Data = value;
			return n.AppendChild(elemNew);
		}

		#endregion

		public static XmlNode AddElement(XmlElement element, string name, string value)
		{
			var elemNew = element.OwnerDocument.CreateElement(name);
			if (!string.IsNullOrEmpty(value))
				elemNew.InnerText = value;
			return element.AppendChild(elemNew);
		}

		public static XmlNode AddElement(XmlElement element, string name)
		{
			var elemNew = element.OwnerDocument.CreateElement(name);
			return element.AppendChild(elemNew);
		}

		public static XmlNode AddElement(XmlDocument document, string name)
		{
			var elemNew = document.CreateElement(name);
			return document.AppendChild(elemNew);
		}

		public static XmlNode AddElement(XmlDocument document, string name, string value)
		{
			var elemNew = document.CreateElement(name);
			elemNew.InnerText = value;
			return document.AppendChild(elemNew);
		}

		public static XmlAttribute AddAttribute(XmlNode node, string name, string value)
		{
			var attrNew = node.OwnerDocument.CreateAttribute(name);
			attrNew.InnerText = value;
			node.Attributes.Append(attrNew);
			return attrNew;
		}

		public static XmlAttribute AddAttribute(XmlElement node, string name, string value)
		{
			return AddAttribute((XmlNode)node, name, value);
		}

		public static XmlAttribute AddAttribute(XmlNode element, string name, double value)
		{
			var attrNew = element.OwnerDocument.CreateAttribute(name);
			attrNew.InnerText = value.ToString(_cultureProvider);
			element.Attributes.Append(attrNew);
			return attrNew;
		}

		public static XmlAttribute AddAttribute(XmlNode element, string name, Guid value)
		{
			var attrNew = element.OwnerDocument.CreateAttribute(name);
			attrNew.InnerText = value.ToString();
			element.Attributes.Append(attrNew);
			return attrNew;
		}

		public static XmlAttribute AddAttribute(XmlElement node, string name, double value)
		{
			return AddAttribute((XmlNode)node, name, value);
		}

		public static XmlAttribute AddAttribute(XmlNode element, string name, int value)
		{
			var attrNew = element.OwnerDocument.CreateAttribute(name);
			attrNew.InnerText = value.ToString(_cultureProvider);
			element.Attributes.Append(attrNew);
			return attrNew;
		}

		public static XmlAttribute AddAttribute(XmlElement node, string name, int value)
		{
			return AddAttribute((XmlNode)node, name, value);
		}

		public static XmlAttribute AddAttribute(XmlNode element, string name, bool value)
		{
			XmlDocument docOwner = null;
			XmlAttribute attrNew = null;

			docOwner = element.OwnerDocument;
			attrNew = docOwner.CreateAttribute(name);
			//attrNew.InnerText = value.ToString(_cultureProvider);
			if (value) attrNew.InnerText = "1";
			else attrNew.InnerText = "0";
			element.Attributes.Append(attrNew);
			return attrNew;
		}

		public static XmlAttribute AddAttribute(XmlElement element, string name, bool value)
		{
			return AddAttribute((XmlNode)element, name, value);
		}

		#endregion

		#region RemoveElement

		public static void RemoveElement(XmlDocument document, string XPath)
		{
			XmlNode parentNode = null;
			XmlNodeList nodes = null;

			nodes = document.SelectNodes(XPath);
			foreach (XmlElement node in nodes)
			{
				parentNode = node.ParentNode;
				node.RemoveAll();
				parentNode.RemoveChild(node);
			}
		}

		public static void RemoveElement(XmlElement element)
		{
			var parentNode = element.ParentNode;
			parentNode.RemoveChild(element);
		}

		public static void RemoveAttribute(XmlElement element, string attributeName)
		{
			XmlAttribute attrDelete = null;
			attrDelete = (XmlAttribute)element.Attributes.GetNamedItem(attributeName);
			element.Attributes.Remove(attrDelete);
		}

		#endregion

		#region UpdateElement

		public static void UpdateElement(XmlElement element, string newValue)
		{
			element.InnerText = newValue;
		}

		public static void UpdateElement(ref XmlDocument document, string Xpath, string newValue)
		{
			document.SelectSingleNode(Xpath).InnerText = newValue;
		}

		public static void UpdateAttribute(XmlElement element, string attributeName, string newValue)
		{
			XmlAttribute attrTemp = null;
			attrTemp = (XmlAttribute)element.Attributes.GetNamedItem(attributeName);
			if (attrTemp == null)
				throw new Exception("The attribute was not found!");
			attrTemp.InnerText = newValue;
		}

		public static void UpdateAttribute(XmlElement element, string attributeName, bool newValue)
		{
			UpdateAttribute(element, attributeName, newValue.ToString().ToLower());
		}

		public static void UpdateAttribute(XmlElement element, string attributeName, int newValue)
		{
			UpdateAttribute(element, attributeName, newValue.ToString());
		}

		public static void UpdateAttribute(XmlElement element, string attributeName, Single newValue)
		{
			UpdateAttribute(element, attributeName, newValue.ToString());
		}

		public static void UpdateAttribute(XmlElement element, string attributeName, double newValue)
		{
			UpdateAttribute(element, attributeName, newValue.ToString());
		}

		#endregion

		#region GetElement

		public static XmlElement GetElement(XmlElement parentElement, string tagName)
		{
			var list = parentElement.GetElementsByTagName(tagName);
			if (list.Count > 0)
				return (XmlElement)list[0];
			else
				return null;
		}

		#endregion
	}
}
