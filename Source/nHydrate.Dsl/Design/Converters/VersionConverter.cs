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

