#pragma warning disable 0168
using System;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace nHydrate.Generator.Common.Util
{
    public class TextManagerEvents : IVsTextManagerEvents
    {
        private static IVsTextManager2 GetService()
        {
            return Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager)) as IVsTextManager2;
        }

        public static void RegisterForTextManagerEvents()
        {
            var textManager = GetService();
            var container = textManager as IConnectionPointContainer;
            var eventGuid = typeof (IVsTextManagerEvents).GUID;
            container.FindConnectionPoint(ref eventGuid, out var textManagerEventsConnection);
            var textManagerEvents = new TextManagerEvents();
            uint textManagerCookie;
            textManagerEventsConnection.Advise(textManagerEvents, out textManagerCookie);
        }

        public void OnRegisterMarkerType(int iMarkerType)
        {
        }

        public void OnRegisterView(IVsTextView pView)
        {
        }

        public void OnUnregisterView(IVsTextView pView)
        {
        }

        public void OnUserPreferencesChanged(
            VIEWPREFERENCES[] pViewPrefs,
            FRAMEPREFERENCES[] pFramePrefs,
            LANGPREFERENCES[] pLangPrefs,
            FONTCOLORPREFERENCES[] pColorPrefs)
        {
            try
            {
                
                Setup();

                //if (pColorPrefs != null && pColorPrefs.Length > 0 && pColorPrefs[0].pColorTable != null)
                //{
                //	var guidFontCategory = (Guid)Marshal.PtrToStructure(pColorPrefs[0].pguidFontCategory, typeof(Guid));
                //	var guidColorService = (Guid)Marshal.PtrToStructure(pColorPrefs[0].pguidColorService, typeof(Guid));
                //	if (_guidColorService == Guid.Empty)
                //	{
                //		_guidColorService = guidColorService;
                //	}
                //	if (guidFontCategory == DefGuidList.guidTextEditorFontCategory && _guidColorService == guidColorService)
                //	{
                //		Setup();
                //	}
                //}

            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

        private static DateTime _lastCall = DateTime.MinValue;
        public static void Setup()
        {
            return;
            if (EnvDTEHelper.Instance.ApplicationObject == null) return;

            //if (DateTime.Now.Subtract(_lastCall).TotalSeconds < 3) return;
            //_lastCall = DateTime.Now;

            var props = EnvDTEHelper.Instance.ApplicationObject.Properties["FontsAndColors", "TextEditor"];
            //var props = EnvDTEHelper.Instance.ApplicationObject.Properties["FontsAndColors", "Dialogs and Tool Windows"];

            var prop = props.Item("FontsAndColorsItems") as EnvDTE.Property;
            var fcList = prop.Object as EnvDTE.FontsAndColorsItems;

            #region Debug

            var q = 1;
            if (q == 0)
            {
                for (var ii = 1; ii < fcList.Count; ii++)
                {
                    var item = fcList.Item(ii);
                    System.Diagnostics.Debug.WriteLine(item.Name + " | " + item.Background + " | " + item.Foreground);
                }
            }

            #endregion

            //var useThis = false;
            //for (var ii = 1; ii < fcList.Count; ii++)
            //{
            //	var item = fcList.Item(ii);
            //	useThis |= (item.Background != 0);
            //}

            //if (!useThis) return;

            var item2 = fcList.Item("Plain Text");
            //var item2 = fcList.Item("Text");
            EnvDTEHelper.Instance.BackgroundColor = UIntToColor(item2.Background);
            EnvDTEHelper.Instance.ForegroundColor = UIntToColor(item2.Foreground);
            //item2 = fcList.Item("Selected Text");
            //EnvDTEHelper.Instance.SelectedBackgroundColor = UIntToColor(item2.Background);
            //EnvDTEHelper.Instance.TriggerColorChange();
        }

        private static System.Drawing.Color UIntToColor(uint color)
        {
            var a = (byte) (color >> 24);
            var r = (byte) (color >> 16);
            var g = (byte) (color >> 8);
            var b = (byte) (color >> 0);
            return System.Drawing.Color.FromArgb(255, r, g, b);
        }

    }
}