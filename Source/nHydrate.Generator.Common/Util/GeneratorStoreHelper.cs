using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Common.Util
{
	public static class GeneratorStoreHelper
	{
		private const string STORE_ASSEMBLY = @"nHydrate.nHydrateStore.dll";

		/// <summary>
		/// Processes all pending installations
		/// </summary>
		/// <returns></returns>
		public static bool ProcessInstallations()
		{
			try
			{
				var ctrl = CreateGeneratorStore();
				ctrl.ProcessInstallations();
				return true;
			}
			catch (Exception ex) { }
			return false;
		}

		/// <summary>
		/// Determines if there is a generator store installed
		/// </summary>
		/// <returns></returns>
		public static bool IsStoreInstalled()
		{
			try
			{
				return (CreateGeneratorStore() != null);
			}
			catch (Exception ex) { }
			return false;
		}

		/// <summary>
		/// Creates an instance of the Generator Store control
		/// </summary>
		/// <returns></returns>
		public static nHydrate.Generator.Common.GeneratorFramework.IGeneratorStore CreateGeneratorStore()
		{
			try
			{
				var fileName = Path.Combine(nHydrate.Generator.Common.GeneratorFramework.AddinAppData.Instance.ExtensionDirectory, STORE_ASSEMBLY);
				if (!File.Exists(fileName)) return null;
				var asm = Assembly.LoadFrom(fileName);
				var list = ReflectionHelper.GetCreatableObjectImplementsInterface(typeof(nHydrate.Generator.Common.GeneratorFramework.IGeneratorStore), asm);
				var t = list.FirstOrDefault();
				if (t == null) return null;
				else return ReflectionHelper.CreateInstance(t) as nHydrate.Generator.Common.GeneratorFramework.IGeneratorStore;
			}
			catch (Exception ex) { }
			return null;
		}

		/// <summary>
		/// Gets a list of all installed generators
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<AddInGeneratorInstalled> GetInstalledGenerators()
		{
			var retval = new List<AddInGeneratorInstalled>();

			//Get installed generators
			var installedGeneratorTypeList = ReflectionHelper.GetCreatableObjectImplementsInterface(typeof(IProjectGenerator), nHydrate.Generator.Common.GeneratorFramework.AddinAppData.Instance.ExtensionDirectory);
			foreach (var igen in installedGeneratorTypeList)
			{
				var gen = ReflectionHelper.CreateInstance(igen);
				if (gen != null)
				{
					var att = (GeneratorProjectAttribute)ReflectionHelper.GetSingleAttribute(typeof(GeneratorProjectAttribute), gen);
					if (att != null)
					{
						var newGenMeta = new AddInGeneratorInstalled();
						newGenMeta.GeneratorGuid = att.GeneratorGuid;
						newGenMeta.Name = att.Name;
						newGenMeta.Version = igen.Assembly.GetName().Version.ToString();
						var fi = new FileInfo(igen.Assembly.Location);
						newGenMeta.InstallDate = fi.LastWriteTime;
						retval.Add(newGenMeta);
					}
				}
			}
			return retval;
		}

	}
}