#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using nHydrate.Generator.Common.Util;
using System.ComponentModel;
using DslModeling = global::Microsoft.VisualStudio.Modeling;

namespace nHydrate.Dsl
{
	partial class FunctionParameter : nHydrate.Dsl.IContainerParent
	{
		#region Constructors
		// Constructors were not generated for this class because it had HasCustomConstructor
		// set to true. Please provide the constructors below in a partial class.
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public FunctionParameter(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public FunctionParameter(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		#endregion

		#region Names
		public string CamelName
		{
			get { return StringHelper.DatabaseNameToCamelCase(this.PascalName); }
		}

		public string DatabaseName
		{
			get { return this.Name; }
		}

		public string PascalName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.CodeFacade))
					return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
				else
					return StringHelper.DatabaseNameToPascalCase(this.Name);
			}
		}
		#endregion

		#region IContainerParent Members

		DslModeling.ModelElement IContainerParent.ContainerParent
		{
			get { return this.Function; }
		}

		#endregion

	}

	partial class FunctionParameterBase
	{
		partial class NamePropertyHandler
		{
			protected override void OnValueChanged(FunctionParameterBase element, string oldValue, string newValue)
			{
				//If not laoding then parse the name for the data type
				var hasChanged = false;
				if (element.Function != null && !element.Function.nHydrateModel.IsLoading)
				{
					if (!string.IsNullOrEmpty(newValue))
					{
						var arr = newValue.Split(':');
						if (arr.Length == 2)
						{
							var typearr = arr[1].Split(' ');
							var d = Extensions.GetDataTypeFromName(typearr[0]);
							if (d != null)
							{
								if (typearr.Length == 2)
								{
									int len;
									if (int.TryParse(typearr[1], out len))
									{
										element.DataType = d.Value;
										element.Length = len;
										newValue = arr[0];
										hasChanged = true;
									}
									else
									{
										throw new Exception("Unrecognized data type! Valid format is 'Name:Datatype length'");
									}
								}
								else
								{
									element.DataType = d.Value;
									newValue = arr[0];
									hasChanged = true;
								}

							}
							else
							{
								throw new Exception("Unrecognized data type! Valid format is 'Name:Datatype length'");
							}
						}
					}
				}

				base.OnValueChanged(element, oldValue, newValue);

				//Reset after we set datatype
				if (hasChanged)
					element.Name = newValue;
				else
					base.OnValueChanged(element, oldValue, newValue);
			}
		}

		partial class LengthPropertyHandler
		{
			protected override void OnValueChanged(FunctionParameterBase element, int oldValue, int newValue)
			{
				base.OnValueChanged(element, oldValue, newValue);

				//this will trigger another event
				var v = newValue;
				if (v < 0) v = 0;
				v = element.DataType.ValidateDataTypeMax(v);
				if (newValue != v)
					element.Length = element.DataType.ValidateDataTypeMax(v);
			}
		}

		partial class ScalePropertyHandler
		{
			protected override void OnValueChanged(FunctionParameterBase element, int oldValue, int newValue)
			{
				base.OnValueChanged(element, oldValue, newValue);

				//this will trigger another event
				if (newValue < 0) element.Scale = 0;
			}
		}

	}

}

