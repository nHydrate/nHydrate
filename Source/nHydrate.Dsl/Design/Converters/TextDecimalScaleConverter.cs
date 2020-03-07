#pragma warning disable 0168
using System;
using System.ComponentModel;

namespace nHydrate.Dsl.Design.Converters
{
    internal class TextDecimalScaleConverter : TypeConverter
    {
        public TextDecimalScaleConverter()
        {
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            try
            {
                if (destinationType == typeof(string))
                {
                    var retval = string.Empty;
                    if (context.Instance is nHydrate.Dsl.Field)
                    {
                        var column = context.Instance as nHydrate.Dsl.Field;
                        if (column.DataType.GetPredefinedScale() != -1)
                            retval = "undefined";
                        else
                            retval = column.Scale.ToString();
                    }
                    else if (context.Instance is nHydrate.Dsl.ViewField)
                    {
                        var column = context.Instance as nHydrate.Dsl.ViewField;
                        if (column.DataType.GetPredefinedScale() != -1)
                            retval = "undefined";
                        else
                            retval = column.Scale.ToString();
                    }

                    return retval;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (context.Instance is nHydrate.Dsl.Field)
            {
                if (sourceType == typeof(string)) return true;
                else if (sourceType == typeof(int)) return true;
                else return false;
            }
            else if (context.Instance is nHydrate.Dsl.ViewField)
            {
                if (sourceType == typeof(string)) return true;
                else if (sourceType == typeof(int)) return true;
                else return false;
            }

            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                if (int.TryParse(value.ToString(), out var v))
                {
                    return v;
                }
            }
            return 0;
        }

    }
}