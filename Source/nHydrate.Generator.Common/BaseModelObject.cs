#pragma warning disable 0168
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using System;
using System.Collections.Generic;

namespace nHydrate.Generator.Common
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

        public BaseModelObject(INHydrateModelObject root, string key)
            : this(root)
        {
            ResetKey(key);
        }

        protected BaseModelObject()
        {
            //This should only be used for BaseModelCollection<T>
        }

        #endregion

        #region Property Implementations

        public virtual Dictionary<string, IModelConfiguration> ModelConfigurations { get; set; } = new Dictionary<string, IModelConfiguration>();

        protected event EventHandler RootReset;
        protected virtual void OnRootReset(System.EventArgs e) => this.RootReset?.Invoke(this, System.EventArgs.Empty);

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

        //Shortcut for casting above
        public virtual Models.ModelRoot GetRoot() => (Models.ModelRoot)this.Root;

        public int Id { get; protected set; }

        public void ResetId(int newId) => this.Id = newId;

        public string Name { get; set; } = string.Empty;

        public virtual string Key { get; protected set; } = Guid.NewGuid().ToString();

        public void ResetKey(string newKey)
        {
            if (newKey.IsEmpty())
                throw new Exception("The key value must have a value!");
            this.Key = newKey;
        }

        public void ResetKey(Guid newKey, bool skipValidation = false)
        {
            if (newKey == Guid.Empty && !skipValidation)
                throw new Exception("The key value must have a value!");
            ResetKey(newKey.ToString());
        }

        #endregion

        #region IXMLable Members

        public abstract System.Xml.XmlNode XmlAppend(System.Xml.XmlNode node);
        public abstract System.Xml.XmlNode XmlLoad(System.Xml.XmlNode node);

        #endregion

    }
}
