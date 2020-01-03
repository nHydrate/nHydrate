#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
using System.Diagnostics;
using nHydrate.Generator.Common.Logging;

namespace nHydrate.Generator.Common.Util
{
	/// <summary>
	/// Summary description for PerformanceCounterSingleton.
	/// </summary>
	public class PerformanceCounterSingleton
	{

		private static readonly PerformanceCounterSingleton _instance = null;

		private PerformanceCounter _TotalAppointments;
		private PerformanceCounter _AppointmentsPerSecond;

		static PerformanceCounterSingleton()
		{
			_instance = new PerformanceCounterSingleton();
			_instance.InstallPerformanceCounters();
			_instance.InitializePerformanceCounters();
		}

		public static void Intialize()
		{
			nHydrateLog.LogInfo("Initialize Performance Counters");
		}

		private void InstallPerformanceCounters()
		{
			if (!PerformanceCounterCategory.Exists("nHydrate"))
			{
				var counters = new CounterCreationDataCollection();

				// 1. counter for counting totals: PerformanceCounterType.NumberOfItems32
				var totalAppointments = new CounterCreationData();
				totalAppointments.CounterName = "# appointments processed";
				totalAppointments.CounterHelp = "Total number of appointments processed.";
				totalAppointments.CounterType = PerformanceCounterType.NumberOfItems32;
				counters.Add(totalAppointments);

				// 2. counter for counting operations per second:
				//        PerformanceCounterType.RateOfCountsPerSecond32
				var appointmentsPerSecond = new CounterCreationData();
				appointmentsPerSecond.CounterName = "# appointments / sec";
				appointmentsPerSecond.CounterHelp = "Number of operations executed per second";
				appointmentsPerSecond.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
				counters.Add(appointmentsPerSecond);

				// create new category with the counters above
				PerformanceCounterCategory.Create("nHydrate", "nHydrate Category", counters);
			}

		}

		private void InitializePerformanceCounters()
		{
			_TotalAppointments = new PerformanceCounter();
			_TotalAppointments.CategoryName = "nHydrate";
			_TotalAppointments.CounterName = "# appointments processed";
			_TotalAppointments.MachineName = ".";
			_TotalAppointments.ReadOnly = false;

			_AppointmentsPerSecond = new PerformanceCounter();
			_AppointmentsPerSecond.CategoryName = "nHydrate";
			_AppointmentsPerSecond.CounterName = "# appointments / sec";
			_AppointmentsPerSecond.MachineName = ".";
			_AppointmentsPerSecond.ReadOnly = false;
		}

		public PerformanceCounter TotalAppointments
		{
			get
			{
				return _TotalAppointments;
			}
		}

		public PerformanceCounter AppointmentsPerSecond
		{
			get
			{
				return _AppointmentsPerSecond;
			}
		}


		public static PerformanceCounterSingleton Instance
		{
			get
			{
				return _instance;
			}
		}
	}
}

