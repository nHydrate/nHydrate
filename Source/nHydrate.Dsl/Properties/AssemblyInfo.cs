#region Using directives

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

#endregion

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle(@"")]
[assembly: AssemblyDescription(@"")]
[assembly: AssemblyConfiguration("")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]

//
// Make the Dsl project internally visible to the DslPackage assembly
//
[assembly: InternalsVisibleTo(@"nHydrate.Dsl.DslPackage, PublicKey=002400000480000094000000060200000024000052534131000400000100010007B4356BFA15F495860E38F3872A11B912A679C788EA90B5C55316D597D061D5A751A2C7024986DD4E82B591E56B13ECE021F6FE5A52AD1BDBA71B170D6EE462DC5B4FB87FAD050F1799C4164DD26461328450EA5B724BCD44B25402C151F6EEEA9EDC59586F4CC53816F6C823CD23CA576A96C5709D5832C80404F7DCF8C9BA")]

