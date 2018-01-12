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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.DslPackage.Forms;

namespace nHydrate.DslPackage.Objects
{
    public static class ProgressHelper
    {
        private static readonly List<string> _keys = new List<string>();
        private static RunningProcesses _form = null;

        public static string ProgressingStarted(string text)
        {
            return ProgressingStarted(text, false);
        }

        public static string ProgressingStarted(string text, bool topMost)
        {
            return ProgressingStarted(text, topMost, 10);
        }

        public static string ProgressingStarted(string text, bool topMost, int timeout)
        {
            var key = Guid.NewGuid().ToString();
            lock (_keys)
            {
                _keys.Add(key);
                if (_form == null)
                {
                    _form = new RunningProcesses();
                    _form.Update(text, topMost, timeout);
                    _form.Show();
                    System.Windows.Forms.Application.DoEvents();
                }
            }
            return key;
        }

        public static void UpdateSubText(string text, string subText, int progress, string windowTextAppend)
        {
            if (_form != null)
            {
                _form.UpdateSubText(text, subText, progress, windowTextAppend);
            }
        }

        public static void ProgressingComplete(string key)
        {
            lock (_keys)
            {
                if (_keys.Contains(key)) _keys.Remove(key);
                if ((_form != null) && (_keys.Count == 0))
                {
                    _form.Close();
                    _form = null;
                }
            }
        }

    }
}