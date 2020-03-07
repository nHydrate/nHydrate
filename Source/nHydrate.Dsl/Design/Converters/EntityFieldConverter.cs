using System;
using System.Linq;
using System.ComponentModel;

namespace nHydrate.Dsl.Design.Converters
{
	internal class EntityFieldConverter : TypeConverter
	{
		public EntityFieldConverter()
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
				var indexColumn = context.Instance as IndexColumn;
				var field = indexColumn.Index.Entity.Fields.FirstOrDefault(x => x.Id == indexColumn.FieldID);
				if (field != null)
					value = field.Name;
				return value;
			}
			return null;
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return false;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			return value;
		}

	}
}
