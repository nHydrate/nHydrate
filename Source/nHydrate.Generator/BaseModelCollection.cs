#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using System.Collections;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public abstract class BaseModelCollection : BaseModelObject, ICollection, IEnumerable
    {
        public BaseModelCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        #region Methods

        public virtual Array Find(string key)
        {
            try
            {
                var retval = new ArrayList();
                foreach (INHydrateModelObject element in this)
                {
                    if (string.Compare(element.Key, key, true) == 0)
                        retval.Add(element);
                }
                return retval.ToArray();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public abstract void Clear();
        public abstract void AddRange(ICollection list);

        #endregion

        #region IEnumerable Members

        public abstract IEnumerator GetEnumerator();

        #endregion

        #region ICollection Members

        public abstract int Count { get; }
        public abstract void CopyTo(Array array, int index);
        public abstract object SyncRoot { get; }
        public abstract bool IsSynchronized { get; }

        #endregion

    }
}