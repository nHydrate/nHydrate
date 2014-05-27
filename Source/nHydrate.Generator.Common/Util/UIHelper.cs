using System;
using System.Collections.Generic;
using nHydrate.Generator.Common.Forms;

namespace nHydrate.Generator.Common.Util
{
	public static class UIHelper
	{
		private static readonly List<string> _keys = new List<string>();
		private static ProgressingForm _form = null;

		public static string ProgressingStarted()
		{
			var key = Guid.NewGuid().ToString();
			lock (_keys)
			{
				_keys.Add(key);
				if (_form == null)
				{
					_form = new ProgressingForm();
					_form.Show();
					System.Windows.Forms.Application.DoEvents();
				}
			}
			return key;
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

		//public static void ShowLibraryDialog()
		//{
		//  var F = new GeneratorLibraryForm();
		//  if (F.Populate()) F.ShowDialog();
		//  else F.Close();
		//}

	}
}
