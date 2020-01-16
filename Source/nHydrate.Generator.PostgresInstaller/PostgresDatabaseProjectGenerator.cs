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
using System;
using System.Collections.Generic;
using System.Text;

using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using EnvDTE;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.PostgresInstaller
{
    [GeneratorProjectAttribute(
        "Postgres Database Installer (beta)",
        "Creates a project to maintain a Postgres database, used in conjuction with the generated Entity Framework data access layer.",
        "c7153425-b9f2-4291-82e8-88e1295eef09",
        typeof(nHydrateGeneratorProject),
        typeof(PostgresDatabaseProjectGenerator),
        true,
        new string[] { }
        )]
    public class PostgresDatabaseProjectGenerator : BaseProjectGenerator
    {
        protected override string ProjectTemplate
        {
            get { return "efcorepostgresinstaller.vstemplate"; }
        }

        public override string LocalNamespaceExtension
        {
            get { return PostgresDatabaseProjectGenerator.NamespaceExtension; }
        }

        public static string NamespaceExtension
        {
            get { return "PostgresInstall"; }
        }

        protected override void OnAfterGenerate()
        {
            try
            {
                base.OnAfterGenerate();

                var project = EnvDTEHelper.Instance.GetProject(ProjectName);

                var preBuildProperty = project.Properties.Item("PreBuildEvent");
                preBuildProperty.Value = "if not exist \"$(SolutionDir)bin\" mkdir \"$(SolutionDir)bin\"\r\nattrib -r \"$(SolutionDir)Bin\\*.*\"";

                var postBuildProperty = project.Properties.Item("PostBuildEvent");
                postBuildProperty.Value = "copy \"$(TargetDir)$(TargetName).*\" \"$(SolutionDir)Bin\\\"";

                var config = project.ConfigurationManager.ActiveConfiguration;
                try
                {
                    config.Properties.Item("StartAction").Value = 1;
                    config.Properties.Item("StartProgram").Value = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory() + "InstallUtil.exe";
                    config.Properties.Item("StartArguments").Value = this.GetLocalNamespace() + ".dll";
                }
                catch (Exception ex)
                {
                    //TODO
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override void OnInitialize(IModelObject model)
        {
            //nHydrateGeneratorProject.AddICSharpDllToBinFolder();
        }

        public override IModelConfiguration ModelConfiguration { get; set; }
    }
}