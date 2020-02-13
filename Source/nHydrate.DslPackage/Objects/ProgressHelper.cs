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
