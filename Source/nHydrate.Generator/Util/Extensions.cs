using System.Collections.Generic;

namespace nHydrate.Generator.Util
{
    public static class Extensions
    {
        public static List<EnvDTE.Project> GetProjects(this EnvDTE80.Solution2 solution)
        {
            var projects = new List<EnvDTE.Project>();
            for (var ii = 1; ii <= solution.Count; ii++)
            {
                var project = solution.Item(ii);
                switch (project.Kind)
                {
                    //List: https://www.codeproject.com/reference/720512/list-of-visual-studio-project-type-guids
                    case "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}": //C#
                    case "{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}": //ASP.NET 5
                    case "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}": //.NET Core
                        projects.Add(project); break;
                    default:
                        break;
                }
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
                switch (project.Kind)
                {
                    case "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}":
                    case "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}":
                        folders.Add(project);
                        break;
                    default:
                        break;
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
                switch (child.Kind)
                {
                    case "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}":
                    case "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}":
                        folders.Add(child as EnvDTE.Project);
                        break;
                    default:
                        break;
                }
            }
            return folders;
        }

    }
}
