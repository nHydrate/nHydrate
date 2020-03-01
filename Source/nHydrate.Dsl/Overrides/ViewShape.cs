using System;
using System.Collections.Generic;
using System.Linq;
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
            var genFieldCount = o.Fields.Count;
            if (genFieldCount > 0)
                text += "Generated Fields: " + genFieldCount + Environment.NewLine;

            return text;
        }
    }
}
