#pragma warning disable 0168
using System;
using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator
{
    public abstract class BaseModelObjectController : INHydrateModelObjectController
    {
        protected ModelObjectUserInterface _userInterface = null;
        protected INHydrateModelObject _object = null;
        protected ModelObjectTreeNode _node = null;
        protected bool _isEnabled = true;

        #region Constructor

        public BaseModelObjectController(INHydrateModelObject modelObject)
        {
            if (modelObject == null)
                throw new Exception("The model object cannot be null!");

            _object = modelObject;

            modelObject.Controller = this;
            this.HeaderImage = ImageHelper.GetImage(ImageConstants.Default);
        }

        #endregion

        #region Property Implementations

        protected ModelObjectUserInterface UserInterface
        {
            get { return _userInterface; }
            set { _userInterface = value; }
        }

        public virtual bool IsEnabled
        {
            get { return _isEnabled; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string HeaderText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string HeaderDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual System.Drawing.Bitmap HeaderImage { get; set; }

        #endregion

        #region Verify

        public virtual MessageCollection Verify()
        {
            try
            {
                var retval = new MessageCollection();
                foreach (ModelObjectTreeNode node in this.Node.Nodes)
                    retval.AddRange(((BaseModelObjectController)node.Controller).Verify());
                return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public virtual INHydrateModelObject Object
        {
            get { return _object; }
            set { _object = value; }
        }

        public abstract ModelObjectTreeNode Node { get; }
        public abstract MenuCommand[] GetMenuCommands();
        public abstract bool DeleteObject();
        public abstract void Refresh();

        public void Dispose()
        {
        }

    }
}
