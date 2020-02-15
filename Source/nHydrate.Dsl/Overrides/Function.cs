using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Dsl
{
    partial class Function : nHydrate.Dsl.IModuleLink, nHydrate.Dsl.IPrecedence, nHydrate.Dsl.IDatabaseEntity, nHydrate.Dsl.IFieldContainer, nHydrate.Generator.Common.GeneratorFramework.IDirtyable
    {
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

        #region IModuleLink

        IEnumerable<Module> IModuleLink.Modules
        {
            get { return this.Modules.AsEnumerable(); }
        }

        void IModuleLink.AddModule(Module module)
        {
            if (!this.Modules.Contains(module))
                this.Modules.Add(module);
        }

        void IModuleLink.RemoveModule(Module module)
        {
            if (this.Modules.Contains(module))
                this.Modules.Remove(module);
        }

        #endregion

        protected override void OnDeleting()
        {
            if (this.nHydrateModel != null)
                this.nHydrateModel.RemovedFunctions.Add(this.PascalName);
            base.OnDeleting();
        }

        int IPrecedence.PrecedenceOrder
        {
            get { return this.PrecedenceOrder; }
            set { this.PrecedenceOrder = value; }
        }

        string IPrecedence.Name
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        string IPrecedence.TypeName
        {
            get { return "Function"; }
        }

        Guid IPrecedence.ID
        {
            get { return this.Id; }
        }

        #region IFieldContainer Members

        public IEnumerable<IField> FieldList
        {
            get { return this.Fields; }
        }

        #endregion

    }

    partial class FunctionBase
    {
        partial class NamePropertyHandler
        {
            protected override void OnValueChanged(FunctionBase element, string oldValue, string newValue)
            {
                if (element.nHydrateModel != null && !element.nHydrateModel.IsLoading)
                {
                    if (string.IsNullOrEmpty(newValue))
                        throw new Exception("The name must have a value.");

                    var count = element.nHydrateModel.Functions.Count(x => x.Name.ToLower() == newValue.ToLower() && x.Id != element.Id);
                    if (count > 0)
                        throw new Exception("There is already an object with the specified name. The change has been cancelled.");
                }
                base.OnValueChanged(element, oldValue, newValue);
            }
        }

    }
}

