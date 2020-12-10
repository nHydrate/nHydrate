using System.Reflection;
using System.Resources;

#if !NETSTANDARD
//Common properties moved to specific assembly
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile(@"..\..\Security\nHydrate.snk")]

[assembly: AssemblyCompany("nHydrate.org")]
[assembly: AssemblyCopyright("Copyright © nHydrate.org 2006-2020")]
[assembly: AssemblyTrademark("Warning: This computer program is protected by copyright law and international treaties. Unauthorized reproduction or distribution of this program, or any portion of it, may result in severe civil and criminal penalties, and will be prosecuted under the maximum extent possible under law.")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: AssemblyVersion("7.1.1.250")]
[assembly: AssemblyFileVersion("7.1.1.250")]

#endif
