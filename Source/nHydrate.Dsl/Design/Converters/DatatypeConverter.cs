using System;
using System.Linq;
using System.ComponentModel;

namespace nHydrate.Dsl.Design.Converters
{
    public class DatatypeConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var list = Enum.GetNames(typeof(DataTypeConstants)).ToList();

            //These are unsupported
            list.Remove(DataTypeConstants.Structured.ToString());
            list.Remove(DataTypeConstants.Udt.ToString());
            list.Remove(DataTypeConstants.Variant.ToString());

            var cols = new StandardValuesCollection(list);
            return cols;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                return Enum.Parse(typeof(DataTypeConstants), (string)value);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

}
