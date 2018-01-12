#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
using System.ComponentModel;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public abstract class BaseModelObject : INHydrateModelObject
    {
        #region Class Members

        protected string _key = Guid.NewGuid().ToString();
        protected INHydrateModelObject _root;
        protected bool _dirty = false;
        protected bool _cancelUIEvents = false;
        protected INHydrateModelObjectController _controller = null;

        #endregion

        #region Constructor

        public BaseModelObject(INHydrateModelObject root)
        {
            _root = root;
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

        [Browsable(false)]
        public virtual INHydrateModelObjectController Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }


        [Browsable(false)]
        public virtual INHydrateModelObject Root
        {
            get { return (INHydrateModelObject)_root; }
        }


        [Browsable(false)]
        public virtual string Key
        {
            get { return _key; }
        }

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
            _key = newKey;
        }

        #endregion

        #region IXMLable Members

        public abstract void XmlAppend(System.Xml.XmlNode node);
        public abstract void XmlLoad(System.Xml.XmlNode node);

        #endregion

    }
}