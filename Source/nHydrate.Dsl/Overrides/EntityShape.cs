 #region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
    partial class EntityShape
    {
        private readonly System.Drawing.Color TABLE_COLOR_TYPE_TABLE_HEADER = System.Drawing.Color.FromArgb(0xE5, 0xED, 0xE5);
        //private readonly System.Drawing.Color TABLE_COLOR_TYPE_TABLE_COMPHEADER = System.Drawing.Color.Silver;
        //private readonly System.Drawing.Color TABLE_COLOR_TYPE_TABLE_FILL = System.Drawing.Color.FromArgb(0xE5, 0xED, 0xE5);
        private readonly System.Drawing.Color TABLE_COLOR_TYPE_TABLE_TEXT = System.Drawing.Color.Black;

        private readonly System.Drawing.Color TABLE_COLOR_ASS_TABLE_HEADER = System.Drawing.Color.FromArgb(0xCE, 0xFD, 0xC3);
        //private readonly System.Drawing.Color TABLE_COLOR_ASS_TABLE_COMPHEADER = System.Drawing.Color.FromArgb(0xCE, 0xFD, 0xC3);
        //private readonly System.Drawing.Color TABLE_COLOR_ASS_TABLE_FILL = System.Drawing.Color.White;
        private readonly System.Drawing.Color TABLE_COLOR_ASS_TEXT = System.Drawing.Color.Black;

        private readonly System.Drawing.Color TABLE_COLOR_NORMAL_HEADER = System.Drawing.Color.FromArgb(0x00, 0x7A, 0xCC);
        //private readonly System.Drawing.Color TABLE_COLOR_NORMAL_COMPHEADER = System.Drawing.Color.FromArgb(192, 192, 255);
        //private readonly System.Drawing.Color TABLE_COLOR_NORMAL_FILL = System.Drawing.Color.White;
        private readonly System.Drawing.Color TABLE_COLOR_NORMAL_TEXT = System.Drawing.Color.White;

        private readonly System.Drawing.Color TABLE_COLOR_NONEGEN_HEADER = System.Drawing.Color.FromArgb(255, 255, 255);
        //private readonly System.Drawing.Color TABLE_COLOR_NONEGEN_COMPHEADER = System.Drawing.Color.FromArgb(255, 255, 255);
        //private readonly System.Drawing.Color TABLE_COLOR_NONEGEN_FILL = System.Drawing.Color.White;
        private readonly System.Drawing.Color TABLE_COLOR_NONEGEN_TEXT = System.Drawing.Color.Gray;

        //private GhostCache cache = new GhostCache();
        private static Dictionary<string, System.Drawing.Bitmap> _images = new Dictionary<string, System.Drawing.Bitmap>();

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public EntityShape(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public EntityShape(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            //cache.FillColor = this.FillColor;
            //cache.TextColor = this.TextColor;
            //cache.OutlineColor = this.OutlineColor;
            //cache.OutlineDashStyle = this.OutlineDashStyle;
        }

        #endregion

        #region Properties

        private bool _isGhosted = false;

        public virtual bool IsGhosted
        {
            get { return _isGhosted; }
            set
            {
                _isGhosted = value;

                if (_isGhosted)
                {
                    //this.IsExpanded = false;
                    //this.FillColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                    //this.TextColor = System.Drawing.Color.FromArgb(100, 0, 0, 0);
                    //this.OutlineColor = System.Drawing.Color.FromArgb(100, 0, 0, 0);
                    //this.OutlineDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                    //foreach (var item in this.GetCompartmentDescriptions())
                    //{
                    //  item.CompartmentFillColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                    //  item.TitleFillColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                    //  item.AllowCustomCompartmentFillColor = true;
                    //  item.AllowCustomTitleFillColor = true;
                    //  item.TitleTextColor = System.Drawing.Color.FromArgb(100, 0, 0, 0);
                    //}
                }
                else
                {
                    //this.FillColor = cache.FillColor;
                    //this.TextColor = cache.TextColor;
                    //this.OutlineColor = cache.OutlineColor;
                    //this.OutlineDashStyle = cache.OutlineDashStyle;
                }

                this.Invalidate();
            }
        }

        #endregion

        public override void OnDoubleClick(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPointEventArgs e)
        {
            //MARKED - USE THIS FOR DESIGN-TIME DEBUGGING
            //var model = (this.ModelElement as Entity).nHydrateModel;
            //using (var transaction = model.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            //{
            //  foreach (var module in model.Modules)
            //  {
            //    foreach (var relation in module.nHydrateModel.AllRelations)
            //    {
            //      if (module.nHydrateModel.RelationModules.Count(x => x.RelationID == relation.Id && x.ModuleId == module.Id) == 0)
            //        module.nHydrateModel.RelationModules.Add(new RelationModule(this.Partition) { ModuleId = module.Id, RelationID = relation.Id, IsEnforced = true, Included = false });
            //      else
            //        System.Diagnostics.Debug.Write("");
            //    }
            //  }
            //  transaction.Commit();
            //}
            //END DEBUGING *******************************

            base.OnDoubleClick(e);
            ((nHydrateDiagram)this.Diagram).NotifyShapeDoubleClick(this);
        }

        protected override void OnCopy(ModelElement sourceElement)
        {
            base.OnCopy(sourceElement);
        }

        public override bool HasHighlighting
        {
            get
            {
                if (this.IsGhosted)
                    return false;
                else
                    return base.HasHighlighting;
            }
        }

        public override bool HasFilledBackground
        {
            get
            {
                if (this.IsGhosted)
                    return false;
                else
                    return base.HasFilledBackground;
            }
        }

        public override bool HasBackgroundGradient
        {
            get
            {
                if (this.IsGhosted)
                    return false;
                else
                    return base.HasBackgroundGradient;
            }
        }

        protected override DslDiagrams.CompartmentMapping[] GetCompartmentMappings(Type melType)
        {
            var q = this.Decorators.FirstOrDefault();
            var mappings = base.GetCompartmentMappings(melType);
            mappings.ToList().ForEach(x => (x as ElementListCompartmentMapping).ImageGetter = GetElementImage);
            mappings.ToList().ForEach(x => (x as ElementListCompartmentMapping).StringGetter = GetElementText);

            var entity = this.ModelElement as Entity;
            var fieldCompartment = this.FindCompartment(mappings.First().CompartmentId);
            if (fieldCompartment != null && fieldCompartment.Name == "EntityFieldCompartment")
            {
                ////Determine colors for different table types
                //var headerColor = TABLE_COLOR_NORMAL_HEADER;
                //var fillColor = TABLE_COLOR_NORMAL_FILL;
                //var textColor = System.Drawing.Color.Black;
                //if (!entity.IsGenerated)
                //{
                //	fillColor = TABLE_COLOR_NONEGEN_FILL;
                //	headerColor = TABLE_COLOR_NONEGEN_HEADER;
                //	textColor = TABLE_COLOR_NONEGEN_TEXT;
                //}
                //else if (entity.TypedEntity != TypedEntityConstants.None)
                //{
                //	fillColor = TABLE_COLOR_TYPE_TABLE_FILL;
                //	headerColor = TABLE_COLOR_TYPE_TABLE_HEADER;
                //}
                //else if (entity.IsAssociative)
                //{
                //	fillColor = TABLE_COLOR_ASS_TABLE_FILL;
                //	headerColor = TABLE_COLOR_ASS_TABLE_HEADER;
                //}

                //this.GetCompartmentMappings()

                //fieldCompartment.CompartmentFillColor = System.Drawing.Color.White; //fillColor;
                //fieldCompartment.TitleFillColor = headerColor;
                //fieldCompartment.TitleTextColor = textColor;
            }

            //var assembly = Assembly.GetExecutingAssembly();
            //var q = this.Decorators.First(x=>x.Field is Microsoft.VisualStudio.Modeling.Diagrams.ImageField);
            //if (q != null)
            //{
            //  if (entity.ParentInheritedEntity != null)
            //  {
            //    q.Field
            //    this.Decorators.Remove(q);
            //    this.Decorators.Add(new Microsoft.VisualStudio.Modeling.Diagrams.ImageField(Guid.NewGuid().ToString(), System.Drawing.Image.FromStream(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.inherited.png"))));
            //  }
            //  if (entity.IsAssociative)
            //    imageStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.associative.png");
            //  else if (entity.IsTypeTable)
            //    imageStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.typetable.png");
            //  f.DefaultImage = System.Drawing.Image.FromStream(imageStream);
            //}

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
                _images.Add("field", new System.Drawing.Bitmap(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.field.png")));
                _images.Add("fkey", new System.Drawing.Bitmap(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.fkey.png")));
                _images.Add("key", new System.Drawing.Bitmap(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.key.png")));
                _images.Add("storedproc", new System.Drawing.Bitmap(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.storedproc.png")));
                _images.Add("composite", new System.Drawing.Bitmap(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.composite.png")));
                _images.Add("fieldcalculated", new System.Drawing.Bitmap(assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.fieldcalculated.png")));
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
                    var relationList = field.Entity.nHydrateModel.GetRelationsWhereChild(field.Entity, true).ToList();
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
            else if (mel is nHydrate.Dsl.Composite)
            {
                return _images["composite"];
            }
            else
            {
                return null;
            }

        }

        public override void Invalidate()
        {
            base.Invalidate();
        }

        public override void Invalidate(bool refreshBitmap)
        {
            base.Invalidate(refreshBitmap);
        }

        #region Shape Tests

        //public override void OnPaintEmphasis(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPaintEventArgs e)
        //{
        //  base.OnPaintEmphasis(e);
        //}

        //public override Microsoft.VisualStudio.Modeling.Diagrams.StyleSetResourceId BackgroundBrushId
        //{
        //  get
        //  {
        //    return base.BackgroundBrushId;
        //  }
        //}

        //public override System.Drawing.Drawing2D.LinearGradientMode BackgroundGradientMode
        //{
        //  get
        //  {
        //    return base.BackgroundGradientMode;
        //  }
        //}

        //public override string GetDragOverToolTipText(Microsoft.VisualStudio.Modeling.Diagrams.DiagramItem item)
        //{
        //  return base.GetDragOverToolTipText(item);
        //}

        //public override System.Drawing.Color GetShapeLuminosity(Microsoft.VisualStudio.Modeling.Diagrams.DiagramClientView view, System.Drawing.Color color)
        //{
        //  var newColor = System.Drawing.Color.FromArgb(50, color);
        //  return base.GetShapeLuminosity(view, newColor);
        //}

        #endregion

        public override void OnPaintShape(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPaintEventArgs e)
        {
            //Determine colors for different table types
            var entity = this.ModelElement as Entity;
            var textColor = TABLE_COLOR_NORMAL_TEXT;
            var newColor = TABLE_COLOR_NORMAL_HEADER;
            if (!entity.IsGenerated)
            {
                newColor = TABLE_COLOR_NONEGEN_HEADER;
                textColor = TABLE_COLOR_NONEGEN_TEXT;
            }
            else if (entity.TypedEntity != TypedEntityConstants.None)
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

        private class GhostCache
        {
            public System.Drawing.Color FillColor { get; set; }
            public System.Drawing.Color TextColor { get; set; }
            public System.Drawing.Color OutlineColor { get; set; }
            public System.Drawing.Drawing2D.DashStyle OutlineDashStyle { get; set; }
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
            var genFieldCount = o.Fields.Count(x => !x.IsGenerated);
            if (genFieldCount > 0)
                text += "Generated Fields: " + genFieldCount + Environment.NewLine;
            text += "Outbound Relations: " + o.RelationshipList.Count() + Environment.NewLine;
            text += "Inbound Relations: " + o.GetRelationsWhereChild().Count() + Environment.NewLine;

            if (o.Composites.Count > 0)
                text += "Composites: " + o.Composites.Count + Environment.NewLine;

            return text;
        }


    }

    partial class TextColorPropertyHandler
    {
    }
}