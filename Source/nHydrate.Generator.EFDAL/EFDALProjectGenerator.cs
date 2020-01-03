#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using System.IO;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.EFDAL
{
    [GeneratorProjectAttribute(
        "EF Data Access Layer (EF4)",
        "A data access layer built on top of Entity Framework v4",
        "b8bd6b27-b9f2-4291-82e8-88e1295eef06",
        typeof(nHydrateGeneratorProject),
        typeof(EFDALProjectGenerator),
        false,
        new string[] { 
            "nHydrate.Generator.EFDAL.Interfaces.EFDALInterfaceProjectGenerator"
        },
        exclusionList: new string[] { 
            "nHydrate.Generator.EFCodeFirst.EFCodeFirstProjectGenerator"
        }
        )]
    public class EFDALProjectGenerator : BaseProjectGenerator
    {
        protected override string ProjectTemplate
        {
            get
            {
                return "efdal.vstemplate";
            }
        }

        public override string LocalNamespaceExtension
        {
            get { return EFDALProjectGenerator.NamespaceExtension; }
        }

        public static string NamespaceExtension
        {
            get { return "EFDAL"; }
        }

        private void GenerateCompanySpecificFile(string fileName)
        {
            try
            {
                var defaultProjectTemplate = Path.Combine(AddinAppData.Instance.ExtensionDirectory, fileName);
                var fileData = string.Empty;
                using (var sr = File.OpenText(defaultProjectTemplate))
                {
                    fileData = sr.ReadToEnd();
                }

                var newFileText = fileData.Replace("Acme", _model.CompanyName);
                newFileText = newFileText.Replace("$generatedproject$", this.DefaultNamespace);

                var newFileName = ((ModelRoot)_model).CompanyName + fileName;
                File.WriteAllText(Path.Combine(AddinAppData.Instance.ExtensionDirectory, newFileName), newFileText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override void OnAfterGenerate()
        {
            base.OnAfterGenerate();
            var project = EnvDTEHelper.Instance.GetProject(ProjectName);
            if (project != null)
            {
                var preBuildProperty = project.Properties.Item("PreBuildEvent");
                preBuildProperty.Value = "if not exist \"$(SolutionDir)bin\" mkdir \"$(SolutionDir)bin\"\r\nattrib -r \"$(SolutionDir)Bin\\*.*\"";
                var postBuildProperty = project.Properties.Item("PostBuildEvent");
                postBuildProperty.Value = "copy \"$(TargetDir)$(TargetName).*\" \"$(SolutionDir)Bin\\\"";
            }
        }

        protected override void OnInitialize(IModelObject model)
        {
            nHydrateGeneratorProject.AddEFCoreToBinFolder();
        }

        public override IModelConfiguration ModelConfiguration { get; set; }
    }

}