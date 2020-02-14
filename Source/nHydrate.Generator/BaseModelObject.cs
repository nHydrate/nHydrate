using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public abstract class BaseModelObject : INHydrateModelObject
    {
        #region Class Members

        protected INHydrateModelObject _root;
        protected bool _dirty = false;
        protected bool _cancelUIEvents = false;
        protected string _name = string.Empty;

        #endregion

        #region Constructor

        public BaseModelObject(INHydrateModelObject root)
        {
            _root = root;
        }

        public BaseModelObject()
        {
            //This should only be used for BaseModelCollection<T>
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event System.EventHandler DirtyChanged;

        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Dirty = true;

            //Cancel UI events if necessary
            if (!this.CancelUIEvents)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(sender, e);
            }
        }

        protected void OnDirtyChanged(object sender, System.EventArgs e)
        {
            if (this.DirtyChanged != null)
                this.DirtyChanged(sender, e);
        }

        #endregion

        #region Property Implementations

        public virtual Dictionary<string, IModelConfiguration> ModelConfigurations { get; set; }

        [Browsable(false)]
        public virtual INHydrateModelObjectController Controller { get; set; } = null;

        protected event EventHandler RootReset;
        protected virtual void OnRootReset(System.EventArgs e)
        {
            if (this.RootReset != null)
                this.RootReset(this, System.EventArgs.Empty);
        }

        [Browsable(false)]
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

        [Browsable(false)]
        public int Id { get; protected set; }

        public void ResetId(int newId)
        {
            this.Id = newId;
            this.OnPropertyChanged(this, new PropertyChangedEventArgs("Id"));
        }

        public virtual void SetKey(string key)
        {
            this.Key = key;
            this.OnPropertyChanged(this, new PropertyChangedEventArgs("Key"));
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        [Browsable(false)]
        public virtual string Key { get; protected set; } = Guid.NewGuid().ToString();

        [Browsable(false)]
        public virtual bool Dirty
        {
            get { return _dirty; }
            set
            {
                //if (_dirty != value)
                //{
                _dirty = value;
                this.OnDirtyChanged(this, new System.EventArgs());
                if ((this.Dirty) && (this.Root != null) && (this != this.Root))
                    this.Root.Dirty = true;
                //}
            }
        }

        [Browsable(false)]
        public virtual bool CancelUIEvents
        {
            get { return _cancelUIEvents; }
            set { _cancelUIEvents = value; }
        }

        /// <summary>
        /// Resets the unique key
        /// </summary>
        /// <param name="newKey"></param>
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
