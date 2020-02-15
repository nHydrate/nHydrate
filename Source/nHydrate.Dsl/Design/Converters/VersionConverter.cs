using System;
using System.ComponentModel;

namespace nHydrate.Dsl.Design.Converters
{
	internal class VersionConverter : TypeConverter
	{
		public VersionConverter()
		{
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string)) return true;
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return value;
			}
			return null;
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string)) return true;
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			value = this.GetVersion((string)value);
			return value;
		}

		private string GetVersion(string versionString)
		{
			var arr = versionString.Split('.');
			var s1 = ((arr.Length >= 1) ? arr[0] : "0");
			var s2 = ((arr.Length >= 2) ? arr[1] : "0");
			var s3 = ((arr.Length >= 3) ? arr[2] : "0");
			var s4 = ((arr.Length >= 4) ? arr[3] : "0");

			s1 = this.GetValue(s1).ToString();
			s2 = this.GetValue(s2).ToString();
			s3 = this.GetValue(s3).ToString();
			s4 = this.GetValue(s4).ToString();
			return s1 + (s2 == string.Empty ? string.Empty : "." + s2) + (s3 == string.Empty ? string.Empty : "." + s3) + (s4 == string.Empty ? string.Empty : "." + s4);

		}

		private int GetValue(string s)
		{
			try
			{
				return int.Parse(s);
			}
			catch
			{
				return 0;
			}
		}

	}
}

