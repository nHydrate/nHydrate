using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	internal interface ISQLGenerate
	{
		void GenerateContent(StringBuilder sb);
	}
}
