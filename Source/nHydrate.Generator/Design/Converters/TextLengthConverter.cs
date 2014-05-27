#region Copyright (c) 2006-2012 nHydrate.org, All Rights Reserved
//--------------------------------------------------------------------- *
//                          NHYDRATE.ORG                                *
//             Copyright (c) 2006-2012 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF THE NHYDRATE GROUP *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS THIS PRODUCT                 *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF NHYDRATE,          *
//THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO             *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion

using System;
using System.ComponentModel;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Design.Converters
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
					if (context.Instance is ColumnBase)
					{
						var column = context.Instance as ColumnBase;
						if (ModelHelper.SupportsMax(column.DataType))
						{
							if (column.Length == 0) retval = "max";
							else retval = column.Length.ToString();
						}
						else if (column.GetPredefinedSize() != -1)
						{
							retval = "predefined";
						}
						else
						{
							retval = column.Length.ToString();
						}
					}
					else if (context.Instance is Parameter)
					{
						var column = context.Instance as Parameter;
						if (ModelHelper.SupportsMax(column.DataType))
						{
							if (column.Length == 0) retval = "max";
							else retval = column.Length.ToString();
						}
						else if (column.GetPredefinedSize() != -1)
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
			if (context.Instance is ColumnBase)
			{
				var column = context.Instance as ColumnBase;
				if (sourceType == typeof(string))
					return true;
				else if (sourceType == typeof(int))
					return true;
				else
					return false;
			}
			else if (context.Instance is Parameter)
			{
				var column = context.Instance as Parameter;
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
				else if (value.ToString().ToLower() == "max")
				{
					return 0;
				}
			}
			return 0;
		}

	}
}
