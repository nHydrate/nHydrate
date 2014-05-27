using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace nHydrate.Generator.Common.Forms
{
    public class CueTextBox : System.Windows.Forms.TextBox
    {
        private string _cue;

        public string Cue
        {
            get { return _cue; }
            set
            {
                _cue = value;
                UpdateCue();
            }
        }

        private void UpdateCue()
        {
            if (this.IsHandleCreated && _cue != null)
            {
                SendMessage(this.Handle, 0x1501, (IntPtr) 1, _cue);
            }
        }

        protected override void OnHandleCreated(System.EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateCue();
        }

        // P/Invoke
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, string lp);
    }
}