using System;
using nHydrate.Generator.Common.Util;
using DslModeling = global::Microsoft.VisualStudio.Modeling;

namespace nHydrate.Dsl
{
    partial class ViewField : nHydrate.Dsl.IContainerParent, nHydrate.Dsl.IField, nHydrate.Generator.Common.GeneratorFramework.IDirtyable
    {
        #region Constructors
        // Constructors were not generated for this class because it had HasCustomConstructor
        // set to true. Please provide the constructors below in a partial class.
        public ViewField(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
        }

        public ViewField(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
        }
        #endregion

        #region Names
        public string DatabaseName => this.Name;

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

        DslModeling.ModelElement IContainerParent.ContainerParent => this.View;

        #endregion

        /// <summary>
        /// This is the image to use on the diagram. If null one will be calculated
        /// </summary>
        internal System.Drawing.Bitmap CachedImage { get; set; }

        public override bool Nullable
        {
            get
            {
                if (this.DataType == DataTypeConstants.Variant) return false;
                else return base.Nullable && !this.IsPrimaryKey;
            }
            set { base.Nullable = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }

    }

    partial class ViewFieldBase
    {
        partial class NamePropertyHandler
        {
            protected override void OnValueChanged(ViewFieldBase element, string oldValue, string newValue)
            {
                //If not laoding then parse the name for the data type
                var hasChanged = false;
                if (element.View != null && !element.View.nHydrateModel.IsLoading)
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
                                    if (int.TryParse(typearr[1], out var len))
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
            protected override void OnValueChanged(ViewFieldBase element, int oldValue, int newValue)
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
            protected override void OnValueChanged(ViewFieldBase element, int oldValue, int newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

                //this will trigger another event
                if (newValue < 0) element.Scale = 0;
            }
        }

    }

}
