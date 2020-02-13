using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace nHydrate.Dsl.Design.Converters
{
	/// <summary>
	/// Flags enumeration type converter.
	/// </summary>
	internal class VisibilityTypeEnumConverter : EnumConverter
	{
		/// <summary>
		/// This class represents an enumeration field in the property grid.
		/// </summary>
		protected class EnumFieldDescriptor : SimplePropertyDescriptor
		{
			#region Fields
			/// <summary>
			/// Stores the context which the enumeration field descriptor was created in.
			/// </summary>
			ITypeDescriptorContext fContext;
			
			#endregion

			#region Methods
			/// <summary>
			/// Creates an instance of the enumeration field descriptor class.
			/// </summary>
			/// <param name="componentType">The type of the enumeration.</param>
			/// <param name="name">The name of the enumeration field.</param>
			/// <param name="context">The current context.</param>
			public EnumFieldDescriptor(Type componentType, string name, ITypeDescriptorContext context)
				: base(typeof(VisibilityTypeConstants), name, typeof(bool))
			{
				fContext = context;
			}

			/// <summary>
			/// Retrieves the value of the enumeration field.
			/// </summary>
			/// <param name="component">
			/// The instance of the enumeration type which to retrieve the field value for.
			/// </param>
			/// <returns>
			/// True if the enumeration field is included to the enumeration; 
			/// otherwise, False.
			/// </returns>
			public override object GetValue(object component)
			{
				return ((int)component & (int)Enum.Parse(ComponentType, Name)) != 0;
			}

			/// <summary>
			/// Sets the value of the enumeration field.
			/// </summary>
			/// <param name="component">
			/// The instance of the enumeration type which to set the field value to.
			/// </param>
			/// <param name="value">
			/// True if the enumeration field should included to the enumeration; 
			/// otherwise, False.
			/// </param>
			public override void SetValue(object component, object value)
			{
				var myValue = (bool)value;
				int myNewValue;
				if (myValue)
					myNewValue = ((int)component) | (int)Enum.Parse(ComponentType, Name);
				else
					myNewValue = ((int)component) & ~(int)Enum.Parse(ComponentType, Name);

				if (component is Enum)
				{
					var myField = component.GetType().GetField("value__", BindingFlags.Instance | BindingFlags.Public);
					myField.SetValue(component, myNewValue);
				}
				else
				{
					component = myNewValue;
				}
				fContext.PropertyDescriptor.SetValue(fContext.Instance, component);
			}

			/// <summary>
			/// Retrieves a value indicating whether the enumeration 
			/// field is set to a non-default value.
			/// </summary>
			public override bool ShouldSerializeValue(object component)
			{
				return (bool)GetValue(component) != GetDefaultValue();
			}

			/// <summary>
			/// Resets the enumeration field to its default value.
			/// </summary>
			public override void ResetValue(object component)
			{
				SetValue(component, GetDefaultValue());
			}

			/// <summary>
			/// Retrieves a value indicating whether the enumeration 
			/// field can be reset to the default value.
			/// </summary>
			public override bool CanResetValue(object component)
			{
				return ShouldSerializeValue(component);
			}

			/// <summary>
			/// Retrieves the enumerations field’s default value.
			/// </summary>
			private bool GetDefaultValue()
			{
				object myDefaultValue = null;
				var myPropertyName = fContext.PropertyDescriptor.Name;
				var myComponentType = fContext.PropertyDescriptor.ComponentType;

				// Get DefaultValueAttribute
				var myDefaultValueAttribute = (DefaultValueAttribute)Attribute.GetCustomAttribute(
					myComponentType.GetProperty(myPropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
					typeof(DefaultValueAttribute));
				if (myDefaultValueAttribute != null)
					myDefaultValue = myDefaultValueAttribute.Value;

				if (myDefaultValue != null)
					return ((int)myDefaultValue & (int)Enum.Parse(ComponentType, Name)) != 0;
				return false;
			}
			#endregion

			#region Properties
			public override AttributeCollection Attributes
			{
				get
				{
					return new AttributeCollection(new Attribute[] { RefreshPropertiesAttribute.Repaint });
				}
			}
			#endregion
		}

		#region Methods
		/// <summary>
		/// Creates an instance of the FlagsEnumConverter class.
		/// </summary>
		/// <param name="type">The type of the enumeration.</param>
		public VisibilityTypeEnumConverter(Type type) : base(type) { }

		/// <summary>
		/// Retrieves the property descriptors for the enumeration fields. 
		/// These property descriptors will be used by the property grid 
		/// to show separate enumeration fields.
		/// </summary>
		/// <param name="context">The current context.</param>
		/// <param name="value">A value of an enumeration type.</param>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (context != null)
			{
				var myType = typeof(VisibilityTypeConstants);
				var myNames = Enum.GetNames(myType);
				var myValues = Enum.GetValues(myType);
				if (myNames != null)
				{
					var myCollection = new PropertyDescriptorCollection(null);
					for (var i = 0; i < myNames.Length; i++)
					{
						if ((int)myValues.GetValue(i) != 0)
							myCollection.Add(new EnumFieldDescriptor(myType, myNames[i], context));
					}
					return myCollection;
				}
			}
			return base.GetProperties(context, value, attributes);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string)) return true;
			else return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				const string CONNECT = ", ";
				var retval = string.Empty;
				var myType = typeof(VisibilityTypeConstants);
				var myNames = Enum.GetNames(myType);
				var myValues = Enum.GetValues(myType);
				if (myNames != null)
				{
					for (var i = 0; i < myNames.Length; i++)
					{
						var v = (int)myValues.GetValue(i);
						if ((v & (int)value) != 0)
							retval += myNames[i] + CONNECT;
					}
				}
				if (retval.EndsWith(CONNECT)) retval = retval.Substring(0, retval.Length - CONNECT.Length);
				return retval;
			}
			else
				return base.ConvertTo(context, culture, value, destinationType);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			if (context != null)
			{
				return true;
			}
			return base.GetPropertiesSupported(context);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return false;
		}
		#endregion
	}
}

