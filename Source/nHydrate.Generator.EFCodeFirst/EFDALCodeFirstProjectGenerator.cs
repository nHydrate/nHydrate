#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using nHydrate.Generator.Common.Util;
using System.IO;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.EFCodeFirst
{
    [GeneratorProjectAttribute(
        "EF Data Access Layer (code first)",
        "A data access layer built on top of Entity Framework v6",
        "b8bd6b27-b9f2-4291-82e8-88e1295eef05",
        typeof(nHydrateGeneratorProject),
        typeof(EFCodeFirstProjectGenerator),
        dependencyList: new string[] { 
            "nHydrate.Generator.EFDAL.Interfaces.EFDALInterfaceProjectGenerator"
        },
        exclusionList: new string[] { 
            "nHydrate.Generator.EFDAL.EFDALProjectGenerator"
        }
        )]
    public class EFCodeFirstProjectGenerator : BaseProjectGenerator
    {
        protected override string ProjectTemplate
        {
            get
            {
                //GenerateCompanySpecificFile("efcodefirst.csproj");
                //GenerateCompanySpecificFile("efcodefirst.vstemplate");
                //return string.Format("{0}efcodefirst.vstemplate", ((ModelRoot)_model).CompanyName);
                return "efcodefirst.vstemplate";
            }
        }

        public override string LocalNamespaceExtension
        {
            get { return EFCodeFirstProjectGenerator.NamespaceExtension; }
        }

        public static string NamespaceExtension
        {
            get { return "EFDAL"; }
        }

        private void GenerateCompanySpecificFile(string fileName)
        {
            try
            {
                string defaultProjectTemplate = StringHelper.EnsureDirectorySeperatorAtEnd(AddinAppData.Instance.ExtensionDirectory) + fileName;
                string fileData = string.Empty;
                using (StreamReader sr = File.OpenText(defaultProjectTemplate))
                {
                    fileData = sr.ReadToEnd();
                }

                string newFileText = fileData.Replace("Acme", _model.CompanyName);
                newFileText = newFileText.Replace("$generatedproject$", this.DefaultNamespace);

                string newFileName = ((ModelRoot)_model).CompanyName + fileName;
                using (StreamWriter sw = File.CreateText(StringHelper.EnsureDirectorySeperatorAtEnd(AddinAppData.Instance.ExtensionDirectory) + newFileName))
                {
                    sw.Write(newFileText);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override void OnAfterGenerate()
        {
            base.OnAfterGenerate();
            EnvDTE.Project project = EnvDTEHelper.Instance.GetProject(ProjectName);
            if (project != null)
            {
                EnvDTE.Property preBuildProperty = project.Properties.Item("PreBuildEvent");
                preBuildProperty.Value = "attrib -r \"$(SolutionDir)Bin\\*.*\"";
                EnvDTE.Property postBuildProperty = project.Properties.Item("PostBuildEvent");
                postBuildProperty.Value = "copy \"$(TargetDir)$(TargetName).*\" \"$(SolutionDir)Bin\\\"";
            }
        }

        protected override void OnInitialize(IModelObject model)
        {
        }

    }

}