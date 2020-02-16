using System.Text;

namespace nHydrate.Generator
{
	public static class GenerationHelper
	{
		public static void AppendFileGeneatedMessageInCode(StringBuilder sb)
		{
			sb.AppendLine("//------------------------------------------------------------------------------");
			sb.AppendLine("// <auto-generated>");
			sb.AppendLine("//    This code was generated from a template.");
			sb.AppendLine("//");
			sb.AppendLine("//    Manual changes to this file may cause unexpected behavior in your application.");
			sb.AppendLine("//    Manual changes to this file will be overwritten if the code is regenerated.");
			sb.AppendLine("// </auto-generated>");
			sb.AppendLine("//------------------------------------------------------------------------------");
			sb.AppendLine();
		}

	}
}

