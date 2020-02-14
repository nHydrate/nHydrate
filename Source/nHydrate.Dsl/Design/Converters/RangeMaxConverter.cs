#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Dsl.Design.Converters
{
	internal class RangeMaxConverter : TypeConverter
	{
		public RangeMaxConverter()
		{
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			var column = context.Instance as nHydrate.Dsl.Field;
			if (destinationType == typeof(string)) return true;
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			var column = context.Instance as nHydrate.Dsl.Field;
			try
			{
				if (destinationType == typeof(string))
				{
					var retval = string.Empty;
					if (column != null && column.DataType.IsNumericType())
					{
						retval = column.Max.ToString();
						if (double.IsNaN(column.Max))
							retval = string.Empty;
						else if (column.IsCalculated)
							retval = "undefined";
					}
					return retval;
				}
			}
			catch (Exception ex) { }
			return null;
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			var column = context.Instance as nHydrate.Dsl.Field;
			if (sourceType == typeof(string))
				return true;
			else if (sourceType == typeof(double))
				return true;
			else
				return false;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			var column = context.Instance as nHydrate.Dsl.Field;
			if (value is string)
			{
				if (((string)value).Trim() == string.Empty)
				{
					return double.NaN;
				}
				else
				{
					double d;
					if (double.TryParse((string)value, out d))
						return d;
					//else
					//  throw new Exception("Cannot convert to double!");
				}
				return column.Max;
			}
			return base.ConvertFrom(context, culture, value);
		}

	}
}

