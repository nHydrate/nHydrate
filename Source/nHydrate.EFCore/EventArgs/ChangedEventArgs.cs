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
namespace nHydrate.EFCore.EventArgs
{
    /// <summary>
    /// The event argument type of all property setters after the property is changed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class ChangedEventArgs<T> : System.ComponentModel.PropertyChangingEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ChangingEventArgs class
        /// </summary>
        /// <param name="newValue">The new value of the property being set</param>
        /// <param name="propertyName">The name of the property being set</param>
        public ChangedEventArgs(T newValue, string propertyName)
            : base(propertyName)
        {
            this.Value = newValue;
        }
        /// <summary>
        /// The new value of the property
        /// </summary>
        public T Value { get; set; }
    }
}