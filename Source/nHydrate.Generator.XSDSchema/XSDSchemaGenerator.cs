#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
using System.Linq;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.ProjectItemGenerators;
using System.IO;

namespace nHydrate.Generator.XSDSchema
{
	[GeneratorItem("XSDSchemaGenerator", typeof(XSDProjectGenerator))]
	class XSDSchemaGenerator : BaseProjectItemGenerator
	{
		#region Overrides

		public override int FileCount
		{
			get { return 1; }
		}

		public override void Generate()
		{
			var metaKey1 = _model.MetaData.FirstOrDefault(x => x.Key.ToLower() == "xsdprojectname");
			var metaKey2 = _model.MetaData.FirstOrDefault(x => x.Key.ToLower() == "xsdfilename");
			var metaKey3 = _model.MetaData.FirstOrDefault(x => x.Key.ToLower() == "xsdschemaname");
			var metaKey4 = _model.MetaData.FirstOrDefault(x => x.Key.ToLower() == "xsddroprelations");

			if (metaKey2 == null || metaKey3 == null)
			{
				OnProjectItemGeneratedError(this,
					new ProjectItemGeneratedErrorEventArgs()
					{
						Text = "The XSD generation template requires the model metadata 'XSDFileName' and 'XSDSchemaName' be set. Also an optional 'XSDProjectName' meta tag may be set.",
						ShowError = true
					}
					);
				return;
			}

			var fileName = metaKey2.Value;
			var schemaName = metaKey3.Value;
			var projectName = string.Empty;
			if (metaKey1 != null) projectName = metaKey1.Value;

			var droprelations = false;
			if (metaKey4 != null)
			{
				bool.TryParse(metaKey4.Value, out droprelations);
			}

			//Prepend Module name if necessary
			if (!string.IsNullOrEmpty(_model.ModuleName))
			{
				fileName = _model.ModuleName + "." + fileName;
			}

			var noPathFileName = fileName.Split('\\').Last();

			var template = new XSDSchemaTemplate(_model, schemaName, noPathFileName, droprelations);
			var eventArgs = new ProjectItemGeneratedEventArgs(fileName, template.FileContent, projectName, this, true) { RunCustomTool = true, CustomToolName = "MSDataSetGenerator" };
			OnProjectItemGenerated(this, eventArgs);
			var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
			OnGenerationComplete(this, gcEventArgs);
		}

		public override string LocalNamespaceExtension
		{
			get { return XSDProjectGenerator.NamespaceExtension; }
		}

		#endregion

	}
}

