#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDesign = global::Microsoft.VisualStudio.Modeling.Design;
using DslDiagrams = global::Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using System.Reflection;

namespace nHydrate.Dsl
{
    partial class ViewShape
    {
        private static Dictionary<string, System.Drawing.Bitmap> _images = new Dictionary<string, System.Drawing.Bitmap>();

        public override void OnDoubleClick(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPointEventArgs e)
        {
            base.OnDoubleClick(e);
            ((nHydrateDiagram)this.Diagram).NotifyShapeDoubleClick(this);
        }

        protected override DslDiagrams.CompartmentMapping[] GetCompartmentMappings(Type melType)
        {
            var mappings = base.GetCompartmentMappings(melType);
            mappings.ToList().ForEach(x => (x as ElementListCompartmentMapping).ImageGetter = GetElementImage);
            mappings.ToList().ForEach(x => (x as ElementListCompartmentMapping).StringGetter = GetElementText);
            return mappings;
        }

        protected string GetElementText(ModelElement mel)
        {
            if (mel is nHydrate.Dsl.ViewField)
            {
                var field = mel as nHydrate.Dsl.ViewField;
                var model = this.Diagram as nHydrateDiagram;
                var text = field.Name;
                if (model.DisplayType)
                    text += " : " + field.DataType.GetSQLDefaultType(field.Length, field.Scale) +
                        " " + (field.Nullable ? "Null" : "Not Null");
                return text;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Decides what the icon of the method will be in the interface shape
        /// </summary>
        protected System.Drawing.Image GetElementImage(ModelElement mel)
        {
            var assembly = Assembly.GetExecutingAssembly();

            if (_images.Count == 0)
            {
                _images.Add("field", new System.Drawing.Bitmap(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.field.png")));
                _images.Add("key", new System.Drawing.Bitmap(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.key.png")));
            }

            if (mel is nHydrate.Dsl.ViewField)
            {
                var field = mel as nHydrate.Dsl.ViewField;
                var image = field.CachedImage;
                if (image == null)
                {
                    image = _images["field"];

                    //Primary Key
                    if (field.IsPrimaryKey)
                        image = _images["key"];

                    field.CachedImage = image;
                }
                return image;
            }
            else
            {
                return null;
            }
        }
    }

    partial class ViewShapeBase
    {
        public string GetVariableTooltipText(DiagramItem item)
        {
            var o = item.Shape.ModelElement as View;
            var text = "View: " + o.Name + Environment.NewLine;
            text += "Fields: " + o.Fields.Count + Environment.NewLine;
            var genFieldCount = o.Fields.Count(x => !x.IsGenerated);
            if (genFieldCount > 0)
                text += "Generated Fields: " + genFieldCount + Environment.NewLine;

            return text;
        }
    }
}