using System;
using System.Collections.Generic;
using System.ComponentModel;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator
{
    public abstract class BaseModelObject : INHydrateModelObject
    {
        #region Class Members

        protected INHydrateModelObject _root;

        #endregion

        #region Constructor

        protected BaseModelObject(INHydrateModelObject root)
        {
            _root = root;
        }

        protected BaseModelObject()
        {
            //This should only be used for BaseModelCollection<T>
        }

        #endregion

        #region Property Implementations

        public virtual Dictionary<string, IModelConfiguration> ModelConfigurations { get; set; }

        public virtual INHydrateModelObjectController Controller { get; set; } = null;

        protected event EventHandler RootReset;
        protected virtual void OnRootReset(System.EventArgs e)
        {
            if (this.RootReset != null)
                this.RootReset(this, System.EventArgs.Empty);
        }

        public virtual INHydrateModelObject Root
        {
            get { return (INHydrateModelObject)_root; }
            protected internal set //need for BaseModelCollection<T>
            {
                if (value == null)
                    throw new Exception("Cannot set root to null.");
                _root = value;
                this.OnRootReset(System.EventArgs.Empty);
            }
        }

        public int Id { get; protected set; }

        public void ResetId(int newId)
        {
            this.Id = newId;
        }

        public virtual void SetKey(string key)
        {
            this.Key = key;
        }

        public string Name { get; set; } = string.Empty;

        public virtual string Key { get; protected set; } = Guid.NewGuid().ToString();

        public void ResetKey(string newKey)
        {
            if (string.IsNullOrEmpty(newKey))
                throw new Exception("The key value must have a value!");
            this.Key = newKey;
        }

        #endregion

        #region IXMLable Members

        public abstract void XmlAppend(System.Xml.XmlNode node);
        public abstract void XmlLoad(System.Xml.XmlNode node);

        #endregion

    }
}
