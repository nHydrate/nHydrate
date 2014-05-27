using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace nHydrate.Dsl.Objects
{
	public static class PrecedenceUtil
	{
		public static IEnumerable<IPrecedence> GetAllPrecedenceItems(nHydrateModel model)
		{
			var retval = model.StoredProcedures.Cast<IPrecedence>().ToList();
			retval.AddRange(model.Views.Cast<IPrecedence>());
			retval.AddRange(model.Functions.Cast<IPrecedence>());

			//Load user defined scripts
			string myNamespace;
			if (string.IsNullOrEmpty(model.DefaultNamespace))
				myNamespace = model.CompanyName + "." + model.ProjectName;
			else
				myNamespace = model.DefaultNamespace;

			var projectName = myNamespace + ".Install";
			var project = nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.GetProject(projectName);
			if (project != null && project.Object != null)
			{
				#region Stored Procedures folder

				var path = @"5_Programmability\Stored Procedures\User Defined";
				var folder = nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.GetProjectFolder(project, path);
				if (folder != null)
				{
					foreach (EnvDTE.ProjectItem pi in folder.ProjectItems)
					{
						if (pi.Name.ToLower().EndsWith(".sql"))
						{
							var fileName = pi.get_FileNames(0);
							if (File.Exists(fileName))
							{
								var item = new UserDefinedScript(fileName, project) as IPrecedence;
								if (((UserDefinedScript) item).IsValid)
									retval.Add(item);
							}
						}
					}
				}

				#endregion

				#region Views folder

				path = @"5_Programmability\Views\User Defined";
				folder = nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.GetProjectFolder(project, path);
				if (folder != null)
				{
					foreach (EnvDTE.ProjectItem pi in folder.ProjectItems)
					{
						if (pi.Name.ToLower().EndsWith(".sql"))
						{
							var fileName = pi.get_FileNames(0);
							if (File.Exists(fileName))
							{
								var item = new UserDefinedScript(fileName, project) as IPrecedence;
								if (((UserDefinedScript) item).IsValid)
									retval.Add(item);
							}
						}
					}
				}

				#endregion

				#region Functions folder

				path = @"5_Programmability\Functions\User Defined";
				folder = nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.GetProjectFolder(project, path);
				if (folder != null)
				{
					foreach (EnvDTE.ProjectItem pi in folder.ProjectItems)
					{
						if (pi.Name.ToLower().EndsWith(".sql"))
						{
							var fileName = pi.get_FileNames(0);
							if (File.Exists(fileName))
							{
								var item = new UserDefinedScript(fileName, project) as IPrecedence;
								if (((UserDefinedScript) item).IsValid)
									retval.Add(item);
							}
						}
					}
				}

				#endregion
			}

			retval.Sort(new PrecedenseComparer());
			return retval;

		}

	}

	public class PrecedenseComparer : IComparer<IPrecedence>
	{
		public int Compare(IPrecedence x, IPrecedence y)
		{
			return x.PrecedenceOrder.CompareTo(y.PrecedenceOrder);
		}
	}

}
