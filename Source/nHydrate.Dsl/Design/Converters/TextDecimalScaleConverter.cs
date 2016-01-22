#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
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
                    else if (context.Instance is nHydrate.Dsl.StoredProcedureField)
                    {
                        var column = context.Instance as nHydrate.Dsl.StoredProcedureField;
                        if (column.DataType.GetPredefinedScale() != -1)
                            retval = "undefined";
                        else
                            retval = column.Scale.ToString();
                    }
                    else if (context.Instance is nHydrate.Dsl.StoredProcedureParameter)
                    {
                        var column = context.Instance as nHydrate.Dsl.StoredProcedureParameter;
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
                    else if (context.Instance is nHydrate.Dsl.SecurityFunctionParameter)
                    {
                        var column = context.Instance as nHydrate.Dsl.SecurityFunctionParameter;
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
                var column = context.Instance as nHydrate.Dsl.Field;
                if (sourceType == typeof(string))
                    return true;
                else if (sourceType == typeof(int))
                    return true;
                else
                    return false;
            }
            else if (context.Instance is nHydrate.Dsl.StoredProcedureField)
            {
                var column = context.Instance as nHydrate.Dsl.StoredProcedureField;
                if (sourceType == typeof(string))
                    return true;
                else if (sourceType == typeof(int))
                    return true;
                else
                    return false;
            }
            else if (context.Instance is nHydrate.Dsl.StoredProcedureParameter)
            {
                var column = context.Instance as nHydrate.Dsl.StoredProcedureParameter;
                if (sourceType == typeof(string))
                    return true;
                else if (sourceType == typeof(int))
                    return true;
                else
                    return false;
            }
            else if (context.Instance is nHydrate.Dsl.ViewField)
            {
                var column = context.Instance as nHydrate.Dsl.ViewField;
                if (sourceType == typeof(string))
                    return true;
                else if (sourceType == typeof(int))
                    return true;
                else
                    return false;
            }
            else if (context.Instance is nHydrate.Dsl.SecurityFunctionParameter)
            {
                var column = context.Instance as nHydrate.Dsl.SecurityFunctionParameter;
                if (sourceType == typeof(string))
                    return true;
                else if (sourceType == typeof(int))
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
                int v;
                if (int.TryParse(value.ToString(), out v))
                {
                    return v;
                }
            }
            return 0;
        }

    }
}