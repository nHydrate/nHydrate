#region Copyright (c) 2006-2011 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2011 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion

using System;
using System.Text;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.WCFService.ProjectItemGenerators.ServiceWebConfig
{
	class ServiceWebConfigTemplate : ServiceBaseTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();

		#region Constructors
		public ServiceWebConfigTemplate(ModelRoot model)
		{
			_model = model;
		}
		#endregion

		#region BaseClassTemplate overrides
		public override string FileContent
		{
			get
			{
				GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get { return "Web.config"; }
		}
		#endregion

		#region GenerateContent

		private void GenerateContent()
		{
			try
			{
				var defaultName = _model.ProjectName + "DataService";

				sb.AppendLine("<?xml version=\"1.0\"?>");
				sb.AppendLine("<!--");
				sb.AppendLine("    Note: As an alternative to hand editing this file you can use the ");
				sb.AppendLine("    web admin tool to configure settings for your application. Use");
				sb.AppendLine("    the Website->Asp.Net Configuration option in Visual Studio.");
				sb.AppendLine("    A full list of settings and comments can be found in ");
				sb.AppendLine("    machine.config.comments usually located in ");
				sb.AppendLine("    \\Windows\\Microsoft.Net\\Framework\\v2.x\\Config ");
				sb.AppendLine("-->");
				sb.AppendLine("<configuration>");
				sb.AppendLine("	<configSections>");
				sb.AppendLine("		<sectionGroup name=\"system.web.extensions\" type=\"System.Web.Configuration.SystemWebExtensionsSectionGroup, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\">");
				sb.AppendLine("			<sectionGroup name=\"scripting\" type=\"System.Web.Configuration.ScriptingSectionGroup, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\">");
				sb.AppendLine("				<section name=\"scriptResourceHandler\" type=\"System.Web.Configuration.ScriptingScriptResourceHandlerSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" allowDefinition=\"MachineToApplication\"/>");
				sb.AppendLine("				<sectionGroup name=\"webServices\" type=\"System.Web.Configuration.ScriptingWebServicesSectionGroup, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\">");
				sb.AppendLine("					<section name=\"jsonSerialization\" type=\"System.Web.Configuration.ScriptingJsonSerializationSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" allowDefinition=\"Everywhere\"/>");
				sb.AppendLine("					<section name=\"profileService\" type=\"System.Web.Configuration.ScriptingProfileServiceSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" allowDefinition=\"MachineToApplication\"/>");
				sb.AppendLine("					<section name=\"authenticationService\" type=\"System.Web.Configuration.ScriptingAuthenticationServiceSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" allowDefinition=\"MachineToApplication\"/>");
				sb.AppendLine("					<section name=\"roleService\" type=\"System.Web.Configuration.ScriptingRoleServiceSection, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" requirePermission=\"false\" allowDefinition=\"MachineToApplication\"/>");
				sb.AppendLine("				</sectionGroup>");
				sb.AppendLine("			</sectionGroup>");
				sb.AppendLine("		</sectionGroup>");
				sb.AppendLine("	</configSections>");
				sb.AppendLine("	<appSettings>");
				sb.AppendLine("		<add key=\"ConnectionString\" value=\"ADD CONNECTIONSTRING HERE\"/>");
				sb.AppendLine("	</appSettings>");
				sb.AppendLine("	<connectionStrings/>");
				sb.AppendLine("	<system.web>");
				sb.AppendLine("		<!--");
				sb.AppendLine("            Set compilation debug=\"true\" to insert debugging ");
				sb.AppendLine("            symbols into the compiled page. Because this ");
				sb.AppendLine("            affects performance, set this value to true only ");
				sb.AppendLine("            during development.");
				sb.AppendLine("        -->");
				sb.AppendLine("		<compilation debug=\"true\">");
				sb.AppendLine("			<assemblies>");
				sb.AppendLine("				<add assembly=\"System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089\"/>");
				sb.AppendLine("				<add assembly=\"System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\"/>");
				sb.AppendLine("			</assemblies>");
				sb.AppendLine("		</compilation>");
				sb.AppendLine("		<!--");
				sb.AppendLine("            The <authentication> section enables configuration ");
				sb.AppendLine("            of the security authentication mode used by ");
				sb.AppendLine("            ASP.NET to identify an incoming user. ");
				sb.AppendLine("        -->");
				sb.AppendLine("		<authentication mode=\"Windows\"/>");
				sb.AppendLine("		<!--");
				sb.AppendLine("            The <customErrors> section enables configuration ");
				sb.AppendLine("            of what to do if/when an unhandled error occurs ");
				sb.AppendLine("            during the execution of a request. Specifically, ");
				sb.AppendLine("            it enables developers to configure html error pages ");
				sb.AppendLine("            to be displayed in place of a error stack trace.");
				sb.AppendLine();
				sb.AppendLine("        <customErrors mode=\"RemoteOnly\" defaultRedirect=\"GenericErrorPage.htm\">");
				sb.AppendLine("            <error statusCode=\"403\" redirect=\"NoAccess.htm\" />");
				sb.AppendLine("            <error statusCode=\"404\" redirect=\"FileNotFound.htm\" />");
				sb.AppendLine("        </customErrors>");
				sb.AppendLine("        -->");
				sb.AppendLine("		<pages>");
				sb.AppendLine("			<controls>");
				sb.AppendLine("				<add tagPrefix=\"asp\" namespace=\"System.Web.UI\" assembly=\"System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\"/>");
				sb.AppendLine("			</controls>");
				sb.AppendLine("		</pages>");
				sb.AppendLine("		<httpHandlers>");
				sb.AppendLine("			<remove verb=\"*\" path=\"*.asmx\"/>");
				sb.AppendLine("			<add verb=\"*\" path=\"*.asmx\" validate=\"false\" type=\"System.Web.Script.Services.ScriptHandlerFactory, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\"/>");
				sb.AppendLine("			<add verb=\"*\" path=\"*_AppService.axd\" validate=\"false\" type=\"System.Web.Script.Services.ScriptHandlerFactory, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\"/>");
				sb.AppendLine("			<add verb=\"GET,HEAD\" path=\"ScriptResource.axd\" type=\"System.Web.Handlers.ScriptResourceHandler, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\" validate=\"false\"/>");
				sb.AppendLine("		</httpHandlers>");
				sb.AppendLine("		<httpModules>");
				sb.AppendLine("			<add name=\"ScriptModule\" type=\"System.Web.Handlers.ScriptModule, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\"/>");
				sb.AppendLine("		</httpModules>");
				sb.AppendLine("	</system.web>");
				sb.AppendLine("	<system.codedom>");
				sb.AppendLine("		<compilers>");
				sb.AppendLine("			<compiler language=\"c#;cs;csharp\" extension=\".cs\" warningLevel=\"4\" type=\"Microsoft.CSharp.CSharpCodeProvider, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\">");
				sb.AppendLine("				<providerOption name=\"CompilerVersion\" value=\"v3.5\"/>");
				sb.AppendLine("				<providerOption name=\"WarnAsError\" value=\"false\"/>");
				sb.AppendLine("			</compiler>");
				sb.AppendLine("		</compilers>");
				sb.AppendLine("	</system.codedom>");
				sb.AppendLine("  <system.web.extensions>");
				sb.AppendLine("    <scripting>");
				sb.AppendLine("      <webServices>");
				sb.AppendLine("        <!--");
				sb.AppendLine("              Uncomment this section to enable the authentication service. Include ");
				sb.AppendLine("              requireSSL=\"true\" if appropriate.");
				sb.AppendLine();
				sb.AppendLine("          <authenticationService enabled=\"true\" requireSSL = \"true|false\"/>");
				sb.AppendLine("          -->");
				sb.AppendLine("        <!--");
				sb.AppendLine("              Uncomment these lines to enable the profile service, and to choose the ");
				sb.AppendLine("              profile properties that can be retrieved and modified in ASP.NET AJAX ");
				sb.AppendLine("              applications.");
				sb.AppendLine();
				sb.AppendLine("          <profileService enabled=\"true\"");
				sb.AppendLine("                          readAccessProperties=\"propertyname1,propertyname2\"");
				sb.AppendLine("                          writeAccessProperties=\"propertyname1,propertyname2\" />");
				sb.AppendLine("          -->");
				sb.AppendLine("        <!--");
				sb.AppendLine("              Uncomment this section to enable the role service.");
				sb.AppendLine();
				sb.AppendLine("          <roleService enabled=\"true\"/>");
				sb.AppendLine("          -->");
				sb.AppendLine("      </webServices>");
				sb.AppendLine("      <!--");
				sb.AppendLine("        <scriptResourceHandler enableCompression=\"true\" enableCaching=\"true\" />");
				sb.AppendLine("        -->");
				sb.AppendLine("    </scripting>");
				sb.AppendLine("  </system.web.extensions>");
				sb.AppendLine("  <!--");
				sb.AppendLine("        The system.webServer section is required for running ASP.NET AJAX under Internet");
				sb.AppendLine("        Information Services 7.0.  It is not necessary for previous version of IIS.");
				sb.AppendLine("    -->");
				sb.AppendLine("	<system.webServer>");
				sb.AppendLine("		<validation validateIntegratedModeConfiguration=\"false\"/>");
				sb.AppendLine("		<modules>");
				sb.AppendLine("			<add name=\"ScriptModule\" preCondition=\"integratedMode\" type=\"System.Web.Handlers.ScriptModule, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\"/>");
				sb.AppendLine("		</modules>");
				sb.AppendLine("		<handlers>");
				sb.AppendLine("			<remove name=\"WebServiceHandlerFactory-Integrated\"/>");
				sb.AppendLine("			<add name=\"ScriptHandlerFactory\" verb=\"*\" path=\"*.asmx\" preCondition=\"integratedMode\" type=\"System.Web.Script.Services.ScriptHandlerFactory, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\"/>");
				sb.AppendLine("			<add name=\"ScriptHandlerFactoryAppServices\" verb=\"*\" path=\"*_AppService.axd\" preCondition=\"integratedMode\" type=\"System.Web.Script.Services.ScriptHandlerFactory, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\"/>");
				sb.AppendLine("			<add name=\"ScriptResource\" preCondition=\"integratedMode\" verb=\"GET,HEAD\" path=\"ScriptResource.axd\" type=\"System.Web.Handlers.ScriptResourceHandler, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35\"/>");
				sb.AppendLine("		</handlers>");
				sb.AppendLine("	</system.webServer>");
				sb.AppendLine();
				sb.AppendLine("  <system.serviceModel>");
				sb.AppendLine("    <bindings>");
				sb.AppendLine("      <basicHttpBinding>");
				sb.AppendLine("        <binding name=\"LargeBuffer\" maxBufferSize=\"2147483647\" maxReceivedMessageSize=\"2147483647\">");
				sb.AppendLine("          <readerQuotas maxDepth=\"2147483647\" maxStringContentLength=\"2147483647\" ");
				sb.AppendLine("                        maxArrayLength=\"2147483647\" maxBytesPerRead=\"2147483647\" ");
				sb.AppendLine("                        maxNameTableCharCount=\"2147483647\" />");
				sb.AppendLine("        </binding>");
				sb.AppendLine("      </basicHttpBinding>");
				sb.AppendLine("    </bindings>");
				sb.AppendLine("    ");
				sb.AppendLine("    <behaviors>      ");
				sb.AppendLine("      <serviceBehaviors>");
				sb.AppendLine("        <behavior name=\"" + defaultName + "Behavior\">");
				sb.AppendLine("          <serviceMetadata httpGetEnabled=\"true\" />");
				sb.AppendLine("          <dataContractSerializer maxItemsInObjectGraph=\"2147483646\" />");
				sb.AppendLine("          <serviceDebug httpHelpPageEnabled=\"true\" includeExceptionDetailInFaults=\"true\" />");
				sb.AppendLine("        </behavior>");
				sb.AppendLine("      </serviceBehaviors>");
				sb.AppendLine("      ");
				sb.AppendLine("      <endpointBehaviors>");
				sb.AppendLine("        <behavior name=\"" + defaultName + "Configuration\">");
				sb.AppendLine("          <dataContractSerializer maxItemsInObjectGraph=\"2147483646\" />");
				sb.AppendLine("        </behavior>");
				sb.AppendLine("      </endpointBehaviors>      ");
				sb.AppendLine("    </behaviors>");
				sb.AppendLine("    ");
				sb.AppendLine("    <services>");
				sb.AppendLine("      <service name=\"" + this.GetLocalNamespace() + ".DataService\" behaviorConfiguration=\"" + defaultName + "Behavior\">");
				sb.AppendLine("        <endpoint binding=\"basicHttpBinding\" ");
				sb.AppendLine("                  bindingConfiguration=\"LargeBuffer\" ");
				sb.AppendLine("                  behaviorConfiguration=\"" + defaultName + "Configuration\" ");
				sb.AppendLine("                  contract=\"" + this.GetLocalNamespace() + ".IDataService\" />");
				sb.AppendLine("        <endpoint address=\"mex\" binding=\"mexHttpBinding\" contract=\"IMetadataExchange\" />");
				sb.AppendLine("      </service>");
				sb.AppendLine("    </services>");
				sb.AppendLine("    ");
				sb.AppendLine("    <diagnostics>");
				sb.AppendLine("      <messageLogging maxMessagesToLog=\"30000\" logEntireMessage=\"true\" ");
				sb.AppendLine("                      logMessagesAtServiceLevel=\"true\" logMalformedMessages=\"true\" ");
				sb.AppendLine("                      logMessagesAtTransportLevel=\"true\">");
				sb.AppendLine("      </messageLogging>");
				sb.AppendLine("    </diagnostics>");
				sb.AppendLine("    ");
				sb.AppendLine("  </system.serviceModel>");
				sb.AppendLine();
				sb.AppendLine("</configuration>");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion
		
	}
}