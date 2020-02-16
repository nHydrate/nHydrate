#pragma warning disable 0168
using System;

namespace nHydrate.Generator
{
    public abstract class BaseModelObjectController : INHydrateModelObjectController
    {
        #region Constructor

        public BaseModelObjectController(INHydrateModelObject modelObject)
        {
            if (modelObject == null)
                throw new Exception("The model object cannot be null!");

            Object = modelObject;

            modelObject.Controller = this;
            this.HeaderImage = ImageHelper.GetImage(ImageConstants.Default);
        }

        #endregion

        #region Property Implementations

        public virtual string HeaderText { get; set; }

        public virtual string HeaderDescription { get; set; }

        public virtual System.Drawing.Bitmap HeaderImage { get; set; }

        #endregion

        #region Verify

        public virtual MessageCollection Verify()
        {
            var retval = new MessageCollection();
            foreach (ModelObjectTreeNode node in this.Node.Nodes)
                retval.AddRange(((BaseModelObjectController) node.Controller).Verify());
            return retval;
        }

        #endregion

        public virtual INHydrateModelObject Object { get; set; } = null;

        public abstract ModelObjectTreeNode Node { get; }

        public void Dispose()
        {
        }

    }
}
