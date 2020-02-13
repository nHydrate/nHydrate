using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using nHydrate.Generator.Common.Logging;

namespace nHydrate.Generator.Common.Util
{
	public class SerializeHelper
	{
		public SerializeHelper()
		{
		}

		#region xmlserialize helpers
		public static void XMLSerialize(string fileName, object o)
		{
			var serializer = new XmlSerializer(o.GetType());
			using (TextWriter writer = new StreamWriter(fileName))
			{
				writer.Write(XMLSerialize(o));
			} 
		}

		public static object XMLDeserialize(string fileName, System.Type type)
		{
			using (TextReader reader = new StreamReader(fileName))
			{
				return XMLDeserializeFromString(reader.ReadToEnd(), type);
			}
		}

		public static string XMLSerialize(object o)
		{
			if(o.GetType() == typeof(Hashtable))
			{
				return XMLSerializeHashtable((Hashtable)o);
			}
			else
			{
				var sb = new StringBuilder();
				var serializer = new XmlSerializer(o.GetType());
				using (var writer = new StringWriter(sb))
				{
					serializer.Serialize(writer, o);
				} 
				return sb.ToString();
			}
		}

		public static object XMLDeserializeFromString(string xml, System.Type type)
		{
			if(type == typeof(Hashtable))
			{
				return XMLDeserializeHashtable(xml);
			}
			else
			{
				using (var reader = new StringReader(xml))
				{
					var serializer1 = new XmlSerializer(type);
					return serializer1.Deserialize(reader);
				}
			}
		}

		private static Hashtable XMLDeserializeHashtable(string xml)
		{
			var returnVal = new Hashtable();
			var sReader = new StringReader(xml);
			var xReader = new XmlTextReader(sReader);

			try
			{
				xReader.ReadStartElement("HashtableAnyType");
				while(xReader.Read())
				{
					var key = xReader.ReadElementString("Key");
					var keyType = (string)XMLDeserializeFromString(xReader.ReadElementString("KeyType"), typeof(string));
					var val = xReader.ReadElementString("Value");
					var valueType = (string)XMLDeserializeFromString(xReader.ReadElementString("ValueType"), typeof(string));
					returnVal.Add(XMLDeserializeFromString(key, Type.GetType(keyType)), XMLDeserializeFromString(val, Type.GetType(valueType)));
					xReader.ReadEndElement();
				}
				
			}
			finally
			{
				xReader.Close();
			}
			return returnVal;
		}

		private static string XMLSerializeHashtable(Hashtable ht)
		{
			var sWriter = new StringWriter();
			var xWriter = new XmlTextWriter(sWriter);
			try
			{		
				xWriter.WriteStartDocument();
				xWriter.WriteStartElement("HashtableAnyType");
				foreach(var key in ht.Keys)
				{
					xWriter.WriteStartElement("KeyValuePair");
					xWriter.WriteElementString("Key", XMLSerialize(key));
					xWriter.WriteElementString("KeyType", XMLSerialize(key.GetType().FullName));
					xWriter.WriteElementString("Value", XMLSerialize(ht[key]));
					xWriter.WriteElementString("ValueType", XMLSerialize(ht[key].GetType().FullName));
					xWriter.WriteEndElement();
				}
				xWriter.WriteEndElement();
				xWriter.WriteEndDocument();
			}
			finally
			{
				xWriter.Close();
			}
			return sWriter.ToString();

		}
		#endregion

		#region soapserialize helpers
		public static void SoapSerialize(string fileName, object o)
		{
			using (TextWriter writer = new StreamWriter(fileName))
			{
				writer.Write(SoapSerialize(o));
			} 
		}

		public static object SoapDeserialize(string fileName)
		{
			using (TextReader reader = new StreamReader(fileName))
			{
				return SoapDeserializeFromString(reader.ReadToEnd());
			}
		}

		public static string SoapSerialize(object o)
		{
			var returnVal = string.Empty;
			var sb = new StringBuilder();
			var serializer = new SoapFormatter();
			using (var ms = new MemoryStream())
			{
				serializer.Serialize(ms, o);
				returnVal = StringHelper.MemoryStreamToString(ms);		
			} 
			return returnVal;
		}

		public static object SoapDeserializeFromString(string soap)
		{
			try
			{
				using (var ms = StringHelper.StringToMemoryStream(soap))
				{
					var serializer = new SoapFormatter();
					return serializer.Deserialize(ms);
				}
			}
			catch(Exception ex)
			{
				nHydrateLog.LogError(ex);
				throw;
			}
		}
		#endregion


	}
}
