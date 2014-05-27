using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using nHydrate.Dsl;
using System.ComponentModel.Design.Serialization;

namespace nHydrate.Dsl.Design.Converters
{
	internal class IsIndexedConverter : TypeConverter
	{
		public IsIndexedConverter()
		{
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string)) return true;
			else if (destinationType == typeof(InstanceDescriptor)) return true; 
			else if (destinationType == typeof(bool)) return true;
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			try
			{
				if (destinationType == typeof(string))
				{
					if (context.Instance is nHydrate.Dsl.Field)
					{
						var column = context.Instance as nHydrate.Dsl.Field;
						return column.IsIndexed.ToString();
					}
					return string.Empty;
				}
				else if (destinationType == typeof(InstanceDescriptor) && value is bool)
				{
					if (context.Instance is nHydrate.Dsl.Field)
					{
						var column = context.Instance as nHydrate.Dsl.Field;
						return destinationType;
					}
					return false;
				}
				if (destinationType == typeof(bool))
				{
					if (context.Instance is nHydrate.Dsl.Field)
					{
						var column = context.Instance as nHydrate.Dsl.Field;
						return column.IsIndexed;
					}
					return false;
				}
			}
			catch (Exception ex) { }
			return null;
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (context.Instance is nHydrate.Dsl.Field)
			{
				var column = context.Instance as nHydrate.Dsl.Field;
				if (sourceType == typeof(string))
				  return true;
				else if (sourceType == typeof(bool))
					return true;
				else
					return false;
			}
			return false;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string)
			{
				bool v;
				if (bool.TryParse(value.ToString(), out v))
					return v;
			}
			else if (value is bool)
			{
				return value;
			}
			return false;
		}

	}
}
