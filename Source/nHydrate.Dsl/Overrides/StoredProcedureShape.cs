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
	partial class StoredProcedureShape
	{
		// Constructors were not generated for this class because it had HasCustomConstructor
		// set to true. Please provide the constructors below in a partial class.
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public StoredProcedureShape(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public StoredProcedureShape(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}

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
			if (mel is nHydrate.Dsl.StoredProcedureField)
			{
				var field = mel as nHydrate.Dsl.StoredProcedureField;
				var model = this.Diagram as nHydrateDiagram;
				var text = field.Name;
				if (model.DisplayType)
					text += " : " + field.DataType.GetSQLDefaultType(field.Length, field.Scale) +
						" " + (field.Nullable ? "Null" : "Not Null");
				return text;
			}
			else if (mel is nHydrate.Dsl.StoredProcedureParameter)
			{
				var parameter = mel as nHydrate.Dsl.StoredProcedureParameter;
				var model = this.Diagram as nHydrateDiagram;
				var text = parameter.Name;
				if (model.DisplayType)
					text += " : " + parameter.DataType.GetSQLDefaultType(parameter.Length, parameter.Scale) +
						" " + (parameter.Nullable ? "Null" : "Not Null");
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

			if (mel is nHydrate.Dsl.StoredProcedureField)
			{
				var imageStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.field.png");
				return new System.Drawing.Bitmap(imageStream);
			}
			else if (mel is nHydrate.Dsl.StoredProcedureParameter)
			{
				var o = mel as nHydrate.Dsl.StoredProcedureParameter;
				var imageStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.parameter.png");

				if (o.IsOutputParameter)
					imageStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.parameteroutput.png");

				return new System.Drawing.Bitmap(imageStream);
			}
			else
			{
				return null;
			}
		}

	}

	partial class StoredProcedureShapeBase
	{
		public string GetVariableTooltipText(DiagramItem item)
		{
			var o = item.Shape.ModelElement as StoredProcedure;
			var text = "Stored Procedure: " + o.Name + Environment.NewLine;
			text += "Fields: " + o.Fields.Count + Environment.NewLine;
			var genFieldCount = o.Fields.Count(x => !x.IsGenerated);
			if (genFieldCount > 0)
				text += "Generated Fields: " + genFieldCount + Environment.NewLine;

			text += "Parameters: " + o.Parameters.Count + Environment.NewLine;
			var genParameterCount = o.Parameters.Count(x => !x.IsGenerated);
			if (genParameterCount > 0)
				text += "Generated Parameters: " + genParameterCount + Environment.NewLine;

			return text;
		}
	}
}

