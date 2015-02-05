#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Linq;

namespace nHydrate.Generator.EFDAL.Generators.EFSSDL
{
    [GeneratorItem("SSDLGenerator", typeof (EFDALProjectGenerator))]
    public class SSDLGenerator : EFDALProjectItemGenerator
    {
        #region Class Members

        private const string RELATIVE_OUTPUT_LOCATION = @"\";

        #endregion

        #region Overrides

        public override int FileCount
        {
            get { return 1; }
        }

        public override void Generate()
        {
            //Sql Server
            if ((_model.SupportedPlatforms & SupportedDatabaseConstants.SqlServer) == SupportedDatabaseConstants.SqlServer)
            {
                {
                    var template = new SSDLTemplate(_model);
                    var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
                    var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, true);
                    eventArgs.Properties.Add("BuildAction", 3);
                    OnProjectItemGenerated(this, eventArgs);
                }

                if (_model.Database.Tables.Any(x => x.IsTenant && x.Generated))
                {
                    var template = new SSDLAdminTemplate(_model);
                    var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
                    var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, true);
                    eventArgs.Properties.Add("BuildAction", 3);
                    OnProjectItemGenerated(this, eventArgs);
                }
                else
                {
                    //If this is not a tenant model then remove the admin SSDL file
                    var template = new SSDLAdminTemplate(_model);
                    var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
                    var eventArgs = new ProjectItemDeletedEventArgs(fullFileName, ProjectName, this);
                    OnProjectItemDeleted(this, eventArgs);
                }
            }

            //MySql
            if ((_model.SupportedPlatforms & SupportedDatabaseConstants.MySql) == SupportedDatabaseConstants.MySql)
            {
                var template = new SSDLMySqlTemplate(_model);
                var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
                var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, true);
                eventArgs.Properties.Add("BuildAction", 3);
                OnProjectItemGenerated(this, eventArgs);
            }

            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

        #endregion

    }
}