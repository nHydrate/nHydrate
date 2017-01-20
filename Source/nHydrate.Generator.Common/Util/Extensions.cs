#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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

namespace nHydrate.Generator.Common.Util
{
	public static class Extensions
	{
		public static List<EnvDTE.Project> GetProjects(this EnvDTE80.Solution2 solution)
		{
			var projects = new List<EnvDTE.Project>();
			for (var ii = 1; ii <= solution.Count; ii++)
			{
				var project = solution.Item(ii);
				if (project.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}")
					projects.Add(project);
			}

			var folders = solution.GetFolders();
			foreach (var f in folders)
			{
				for (var ii = 1; ii <= f.ProjectItems.Count; ii++)
				{
					var project = f.ProjectItems.Item(ii);
					var p = project.Object as EnvDTE.Project;
					if (p != null)
						projects.Add(p);
				}
				//((EnvDTE.ProjectItem)(CurrentSolution.GetFolders()[0].ProjectItems.Item(1))).Name
			}

			return projects;
		}

		public static List<EnvDTE.Project> GetFolders(this EnvDTE80.Solution2 solution)
		{
			var folders = new List<EnvDTE.Project>();
			for (var ii = 1; ii <= solution.Count; ii++)
			{
				var project = solution.Item(ii);
				if ((project.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}") ||
					(project.Kind == "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}"))
				{
					folders.Add(project);
				}
			}
			return folders;
		}

		public static List<EnvDTE.Project> GetFolders(this EnvDTE.Project project)
		{
			var folders = new List<EnvDTE.Project>();
			for (var ii = 1; ii <= project.ProjectItems.Count; ii++)
			{
				var child = project.ProjectItems.Item(ii);
				if ((child.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}") ||
					(child.Kind == "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}"))
				{
					folders.Add(child as EnvDTE.Project);
				}
			}
			return folders;
		}

		//public static 

		//public static List<EnvDTE.Project> GetFolders(this EnvDTE80.SolutionFolder project)
		//{
		//  var folders = new List<EnvDTE.Project>();
		//  for (int ii = 1; ii <= project.ot.ProjectItems.Count; ii++)
		//  {
		//    var child = project.ProjectItems.Item(ii);
		//    if (child.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
		//      folders.Add(project);
		//  }
		//  return folders;
		//}

	}
}

