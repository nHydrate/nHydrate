using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Dsl.Custom
{
	internal static class DebugHelper
	{
		public static System.Diagnostics.Stopwatch StartTimer()
		{
			var timer = new System.Diagnostics.Stopwatch();
			timer.Start();
			return timer;
		}

		public static void StopTimer(System.Diagnostics.Stopwatch timer, string text)
		{
			timer.Stop();

			if (!string.IsNullOrEmpty(text))
				text += ": ";

			var t = timer.ElapsedMilliseconds;
			if (t > 5)
			{
				System.Diagnostics.Debug.WriteLine(text + t.ToString("###,###,##0.00"));
			}
		}

	}
}

