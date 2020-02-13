using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace nHydrate.Dsl.Design.Converters
{
	internal class ModuleConverter : TypeConverter
	{
		public ModuleConverter()
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
				var g = (Guid)value;
				var moduleRule = context.Instance as ModuleRule;
				var modulelist = moduleRule.Module.nHydrateModel.Modules.Where(x => x.Id != moduleRule.Module.Id).ToList();
				var selected = modulelist.FirstOrDefault(x => x.Id == moduleRule.DependentModule);
				if (selected != null)
					value = selected.Name;
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
