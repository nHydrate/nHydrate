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

