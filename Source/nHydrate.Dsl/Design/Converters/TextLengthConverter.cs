#pragma warning disable 0168
using System;
using System.ComponentModel;

namespace nHydrate.Dsl.Design.Converters
{
    internal class TextLengthConverter : TypeConverter
    {
        public TextLengthConverter()
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
                        if (column.DataType.SupportsMax())
                        {
                            if (column.Length == 0) retval = "max";
                            else retval = column.Length.ToString();
                        }
                        else if (column.DataType.GetPredefinedSize() != -1)
                        {
                            retval = "predefined";
                        }
                        else
                        {
                            retval = column.Length.ToString();
                        }
                    }
                    else if (context.Instance is nHydrate.Dsl.ViewField)
                    {
                        var column = context.Instance as nHydrate.Dsl.ViewField;
                        if (column.DataType.SupportsMax())
                        {
                            if (column.Length == 0) retval = "max";
                            else retval = column.Length.ToString();
                        }
                        else if (column.DataType.GetPredefinedSize() != -1)
                        {
                            retval = "predefined";
                        }
                        else
                        {
                            retval = column.Length.ToString();
                        }
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
                    return v;
                else if (value.ToString().ToLower() == "max")
                    return 0;
            }
            return 0;
        }

    }
}