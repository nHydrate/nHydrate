#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DslPackage
{
    partial class nHydrateExplorer
    {
        public override Microsoft.VisualStudio.Modeling.Shell.ModelElementTreeNode CreateModelElementTreeNode(Microsoft.VisualStudio.Modeling.ModelElement modelElement)
        {
            return base.CreateModelElementTreeNode(modelElement);
        }
        
    }
}

