using System;
using System.Collections.Generic;
using System.Linq;
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDiagrams = global::Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using System.Reflection;

namespace nHydrate.Dsl
{
    partial class EntityShape
    {
        private readonly System.Drawing.Color TABLE_COLOR_TYPE_TABLE_HEADER = System.Drawing.Color.FromArgb(0xE5, 0xED, 0xE5);
        private readonly System.Drawing.Color TABLE_COLOR_TYPE_TABLE_TEXT = System.Drawing.Color.Black;
        private readonly System.Drawing.Color TABLE_COLOR_ASS_TABLE_HEADER = System.Drawing.Color.FromArgb(0xCE, 0xFD, 0xC3);
        private readonly System.Drawing.Color TABLE_COLOR_ASS_TEXT = System.Drawing.Color.Black;
        private readonly System.Drawing.Color TABLE_COLOR_NORMAL_HEADER = System.Drawing.Color.FromArgb(0x00, 0x7A, 0xCC);
        private readonly System.Drawing.Color TABLE_COLOR_NORMAL_TEXT = System.Drawing.Color.White;

        //private GhostCache cache = new GhostCache();
        private static Dictionary<string, System.Drawing.Bitmap> _images = new Dictionary<string, System.Drawing.Bitmap>();

        #region Constructor

        // Constructors were not generated for this relationship because it had HasCustomConstructor
        // set to true. Please provide the constructors below in a partial class.
        public EntityShape(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
        }

        public EntityShape(DslModeling::Partition partition,
            params DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
        }

        #endregion

        public override void OnDoubleClick(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPointEventArgs e)
        {
            base.OnDoubleClick(e);
            ((nHydrateDiagram) this.Diagram).NotifyShapeDoubleClick(this);
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
            if (mel is nHydrate.Dsl.Field)
            {
                var field = mel as nHydrate.Dsl.Field;
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
                _images.Add("field",
                    new System.Drawing.Bitmap(
                        assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.field.png")));
                _images.Add("fkey",
                    new System.Drawing.Bitmap(
                        assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.fkey.png")));
                _images.Add("key",
                    new System.Drawing.Bitmap(
                        assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.key.png")));
                _images.Add("fieldcalculated",
                    new System.Drawing.Bitmap(
                        assembly.GetManifestResourceStream(
                            assembly.GetName().Name + ".Resources.fieldcalculated.png")));
            }

            if (mel is nHydrate.Dsl.Field)
            {
                var field = mel as nHydrate.Dsl.Field;
                var image = field.CachedImage;
                if (image == null)
                {
                    if (field.IsCalculated)
                        image = _images["fieldcalculated"];
                    else
                        image = _images["field"];

                    //Foreign Key
                    var relationList = field.Entity.nHydrateModel.GetRelationsWhereChild(field.Entity).ToList();
                    if (relationList.Count(x => x.FieldMapList().Count(q => q.GetTargetField(x) == field) > 0) > 0)
                    {
                        image = _images["fkey"];
                    }

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

        public override void OnPaintShape(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPaintEventArgs e)
        {
            //Determine colors for different table types
            var entity = this.ModelElement as Entity;
            var textColor = TABLE_COLOR_NORMAL_TEXT;
            var newColor = TABLE_COLOR_NORMAL_HEADER;
            if (entity.TypedEntity != TypedEntityConstants.None)
            {
                newColor = TABLE_COLOR_TYPE_TABLE_HEADER;
                textColor = TABLE_COLOR_TYPE_TABLE_TEXT;
            }
            else if (entity.IsAssociative)
            {
                newColor = TABLE_COLOR_ASS_TABLE_HEADER;
                textColor = TABLE_COLOR_ASS_TEXT;
            }

            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            if (this.FillColor != newColor)
                this.SetFillColorValue(newColor);
            if (this.TextColor != textColor)
                this.SetTextColorValue(textColor);

            timer.Stop();
            System.Diagnostics.Debug.WriteLine(timer.ElapsedMilliseconds + "ms");

            base.OnPaintShape(e);

        }

    }

    partial class EntityShapeBase
    {
        private System.Drawing.Drawing2D.LinearGradientMode fillGradientMode;

        public string GetVariableTooltipText(DiagramItem item)
        {
            var o = item.Shape.ModelElement as Entity;
            var text = "Entity: " + o.Name;

            if (!string.IsNullOrEmpty(o.CodeFacade))
                text += " (" + o.CodeFacade + ")";
            text += Environment.NewLine;

            text += "Fields: " + o.Fields.Count + Environment.NewLine;
            var genFieldCount = o.Fields.Count;
            if (genFieldCount > 0)
                text += "Generated Fields: " + genFieldCount + Environment.NewLine;
            text += "Outbound Relations: " + o.RelationshipList.Count + Environment.NewLine;
            text += "Inbound Relations: " + o.GetRelationsWhereChild().Count() + Environment.NewLine;
            return text;
        }


    }

}