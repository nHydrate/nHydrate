using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Design;
using System.Data.Mapping;
using System.Data.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using ConceptualEdmGen;

namespace Widgetsphere.Generator.EFDAL
{

	internal class NamespaceManager
	{
		private static Version v1 = EntityFrameworkVersions.Version1;
		private static Version v2 = EntityFrameworkVersions.Version2;

		private Dictionary<Version, XNamespace> _versionToCSDLNamespace = new Dictionary<Version, XNamespace>() 
				{ 
				{ v1, XNamespace.Get("http://schemas.microsoft.com/ado/2006/04/edm") }, 
				{ v2, XNamespace.Get("http://schemas.microsoft.com/ado/2008/09/edm") } 
				};

		private Dictionary<Version, XNamespace> _versionToSSDLNamespace = new Dictionary<Version, XNamespace>() 
				{ 
				{ v1, XNamespace.Get("http://schemas.microsoft.com/ado/2006/04/edm/ssdl") }, 
				{ v2, XNamespace.Get("http://schemas.microsoft.com/ado/2009/02/edm/ssdl") } 
				};

		private Dictionary<Version, XNamespace> _versionToMSLNamespace = new Dictionary<Version, XNamespace>() 
				{ 
				{ v1, XNamespace.Get("urn:schemas-microsoft-com:windows:storage:mapping:CS") }, 
				{ v2, XNamespace.Get("http://schemas.microsoft.com/ado/2008/09/mapping/cs") } 
				};


		private Dictionary<Version, XNamespace> _versionToEDMXNamespace = new Dictionary<Version, XNamespace>() 
				{ 
				{ v1, XNamespace.Get("http://schemas.microsoft.com/ado/2007/06/edmx") }, 
				{ v2, XNamespace.Get("http://schemas.microsoft.com/ado/2008/10/edmx") } 
				};

		private Dictionary<XNamespace, Version> _namespaceToVersion = new Dictionary<XNamespace, Version>();

		internal NamespaceManager()
		{
			foreach (KeyValuePair<Version, XNamespace> kvp in _versionToCSDLNamespace)
			{
				_namespaceToVersion.Add(kvp.Value, kvp.Key);
			}

			foreach (KeyValuePair<Version, XNamespace> kvp in _versionToSSDLNamespace)
			{
				_namespaceToVersion.Add(kvp.Value, kvp.Key);
			}

			foreach (KeyValuePair<Version, XNamespace> kvp in _versionToMSLNamespace)
			{
				_namespaceToVersion.Add(kvp.Value, kvp.Key);
			}

			foreach (KeyValuePair<Version, XNamespace> kvp in _versionToEDMXNamespace)
			{
				_namespaceToVersion.Add(kvp.Value, kvp.Key);
			}
		}

		internal Version GetVersionFromEDMXDocument(XDocument xdoc)
		{
			XElement el = xdoc.Root;
			if (el.Name.LocalName.Equals("Edmx") == false)
			{
				throw new ArgumentException("Unexpected root node local name for edmx document");
			}
			return this.GetVersionForNamespace(el.Name.Namespace);
		}

		internal Version GetVersionFromCSDLDocument(XDocument xdoc)
		{
			XElement el = xdoc.Root;
			if (el.Name.LocalName.Equals("Schema") == false)
			{
				throw new ArgumentException("Unexpected root node local name for csdl document");
			}
			return this.GetVersionForNamespace(el.Name.Namespace);
		}

		internal XNamespace GetMSLNamespaceForVersion(Version v)
		{
			XNamespace n;
			_versionToMSLNamespace.TryGetValue(v, out n);
			return n;
		}

		internal XNamespace GetCSDLNamespaceForVersion(Version v)
		{
			XNamespace n;
			_versionToCSDLNamespace.TryGetValue(v, out n);
			return n;
		}

		internal XNamespace GetSSDLNamespaceForVersion(Version v)
		{
			XNamespace n;
			_versionToSSDLNamespace.TryGetValue(v, out n);
			return n;
		}

		internal XNamespace GetEDMXNamespaceForVersion(Version v)
		{
			XNamespace n;
			_versionToEDMXNamespace.TryGetValue(v, out n);
			return n;
		}

		internal Version GetVersionForNamespace(XNamespace n)
		{
			Version v;
			_namespaceToVersion.TryGetValue(n, out v);
			return v;
		}
	}

	public class EdmGen2
	{

		internal enum Mode { FromEdmx, ToEdmx, ModelGen, CodeGen, ViewGen, Validate, RetrofitModel, Help }

		// a class that understands what the different XML namespaces are for the different EF versions. 
		private static NamespaceManager _namespaceManager = new NamespaceManager();

		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				ShowUsage();
				return;
			}

			Mode mode = GetMode(args[0]);

			switch (mode)
			{
				case Mode.FromEdmx:
					FromEdmx(args);
					break;
				case Mode.ToEdmx:
					ToEdmx(args);
					break;
				case Mode.ModelGen:
					ModelGen(args);
					break;
				case Mode.CodeGen:
					CodeGen(args);
					break;
				case Mode.ViewGen:
					ViewGen(args);
					break;
				case Mode.Validate:
					Validate(args);
					break;
				case Mode.RetrofitModel:
					RetrofitModel(args);
					break;
				default:
					ShowUsage();
					return;
			}
		}

		private static void ShowUsage()
		{
			Console.WriteLine("Usage:  EdmGen2 [arguments]");
			Console.WriteLine("                 /FromEdmx <edmx file>");
			Console.WriteLine("                 /ToEdmx <csdl file> <msl file> <ssdl file>");
			Console.WriteLine("                 /ModelGen <connection string> <provider name> <model name>");
			Console.WriteLine("                 /RetrofitModel <connection string> <provider name> <model name> <percent threshold>?");
			Console.WriteLine("                 /ViewGen cs|vb <edmx file>");
			Console.WriteLine("                 /CodeGen cs|vb <edmx file>");
			Console.WriteLine("                 /Validate <edmx file>");
			Console.WriteLine("RetrofitModel option takes table names in the form [schema_name].[table_name] from the file tables.txt, one per line, if it exists.");
		}

		#region the functions that actually do the interesting things

		private static void FromEdmx(string[] args)
		{
			FileInfo edmxFile;
			if (ParseEdmxFileArguments(args[1], out edmxFile))
			{
				FromEdmx(edmxFile);
			}
		}

		private static void FromEdmx(FileInfo edmxFile)
		{
			XDocument xdoc = XDocument.Load(edmxFile.FullName);

			// select the csdl element, and write it out
			XElement csdl = GetCsdlFromEdmx(xdoc);
			string csdlFileName = GetFileNameWithNewExtension(edmxFile, ".csdl");
			File.WriteAllText(csdlFileName, csdl.ToString());

			// select the ssdl element and write it out
			XElement ssdl = GetSsdlFromEdmx(xdoc);
			string ssdlFileName = GetFileNameWithNewExtension(edmxFile, ".ssdl");
			File.WriteAllText(ssdlFileName, ssdl.ToString());

			// select the msl element and write it out
			XElement msl = GetMslFromEdmx(xdoc);
			string mslFileName = GetFileNameWithNewExtension(edmxFile, ".msl");
			File.WriteAllText(mslFileName, msl.ToString());
		}

		private static void PrintModelGenUsage()
		{
			System.Console.WriteLine("Usage:  ModelGenerator <connection string> <provider name> <model name> [<version>] [includeFKs]");
			System.Console.WriteLine("             where <version> is 1.0 (for EF v1) or 2.0 (for EF v2)");
			System.Console.WriteLine("             and where includeFKs is only valid on for EF versions later than EF v1");
		}

		private static void ModelGen(string[] args)
		{
			if (args.Length < 4 || args.Length > 5)
			{
				PrintModelGenUsage();
				return;
			}
			string connectionString = args[1];
			string provider = args[2];
			string modelName = args[3];
			Version version = EntityFrameworkVersions.Version2;
			if (args.Length > 4)
			{
				if (args[4] == "1.0")
				{
					version = EntityFrameworkVersions.Version1;
				}
			}

			bool includeForeignKeys = version == EntityFrameworkVersions.Version2 ? true : false;
			if (args.Length > 5)
			{
				if (version == EntityFrameworkVersions.Version2 && args[5] != "includeFKs")
				{
					includeForeignKeys = true;
				}
				else
				{
					PrintModelGenUsage();
					return;
				}
			}
			ModelGen(connectionString, provider, modelName, version, includeForeignKeys);
		}

		private static void ModelGen(
				string connectionString, string provider, string modelName, Version version, bool includeForeignKeys)
		{
			IList<EdmSchemaError> ssdlErrors = null;
			IList<EdmSchemaError> csdlAndMslErrors = null;

			// generate the SSDL
			string ssdlNamespace = modelName + "Model.Store";
			EntityStoreSchemaGenerator essg =
					new EntityStoreSchemaGenerator(
							provider, connectionString, ssdlNamespace);
			essg.GenerateForeignKeyProperties = includeForeignKeys;

			ssdlErrors = essg.GenerateStoreMetadata(new List<EntityStoreSchemaFilterEntry>(), version);

			// detect if there are errors or only warnings from ssdl generation
			bool hasSsdlErrors = false;
			bool hasSsdlWarnings = false;
			if (ssdlErrors != null)
			{
				foreach (EdmSchemaError e in ssdlErrors)
				{
					if (e.Severity == EdmSchemaErrorSeverity.Error)
					{
						hasSsdlErrors = true;
					}
					else if (e.Severity == EdmSchemaErrorSeverity.Warning)
					{
						hasSsdlWarnings = true;
					}
				}
			}

			// write out errors & warnings
			if (hasSsdlErrors && hasSsdlWarnings)
			{
				System.Console.WriteLine("Errors occurred during generation:");
				WriteErrors(ssdlErrors);
			}

			// if there were errors abort.  Continue if there were only warnings
			if (hasSsdlErrors)
			{
				return;
			}

			// write the SSDL to a string
			StringWriter ssdl = new StringWriter();
			XmlWriter ssdlxw = XmlWriter.Create(ssdl);
			essg.WriteStoreSchema(ssdlxw);
			ssdlxw.Flush();

			// generate the CSDL
			string csdlNamespace = modelName + "Model";
			string csdlEntityContainerName = modelName + "Entities";
			EntityModelSchemaGenerator emsg =
					new EntityModelSchemaGenerator(
							essg.EntityContainer, csdlNamespace, csdlEntityContainerName);
			emsg.GenerateForeignKeyProperties = includeForeignKeys;
			csdlAndMslErrors = emsg.GenerateMetadata(version);


			// detect if there are errors or only warnings from csdl/msl generation
			bool hasCsdlErrors = false;
			bool hasCsdlWarnings = false;
			if (csdlAndMslErrors != null)
			{
				foreach (EdmSchemaError e in csdlAndMslErrors)
				{
					if (e.Severity == EdmSchemaErrorSeverity.Error)
					{
						hasCsdlErrors = true;
					}
					else if (e.Severity == EdmSchemaErrorSeverity.Warning)
					{
						hasCsdlWarnings = true;
					}
				}
			}

			// write out errors & warnings
			if (hasCsdlErrors || hasCsdlWarnings)
			{
				System.Console.WriteLine("Errors occurred during generation:");
				WriteErrors(csdlAndMslErrors);
			}

			// if there were errors, abort.  Don't abort if there were only warnigns.  
			if (hasCsdlErrors)
			{
				return;
			}

			// write CSDL to a string
			StringWriter csdl = new StringWriter();
			XmlWriter csdlxw = XmlWriter.Create(csdl);
			emsg.WriteModelSchema(csdlxw);
			csdlxw.Flush();

			// write MSL to a string
			StringWriter msl = new StringWriter();
			XmlWriter mslxw = XmlWriter.Create(msl);
			emsg.WriteStorageMapping(mslxw);
			mslxw.Flush();

			// write csdl, ssdl & msl to the EDMX file
			ToEdmx(
					csdl.ToString(), ssdl.ToString(), msl.ToString(), new FileInfo(
							modelName + ".edmx"));
		}

		private static void RetrofitModel(string[] args)
		{
			if (args.Length < 4 || args.Length > 5)
			{
				ShowUsage();
				return;
			}

			ConceptualEdmGen.Generator cedm;
			if (args.Length == 5)
			{
				cedm = new ConceptualEdmGen.Generator(args[1], args[3], args[2], Convert.ToDouble(args[4]));
			}
			else
			{
				cedm = new ConceptualEdmGen.Generator(args[1], args[3], args[2]);
			}
			if (File.Exists("tables.txt"))
			{
				if (cedm.SetTables("tables.txt"))
				{
					return;
				}
			}
			cedm.Execute();
		}

		private static void CodeGen(string[] args)
		{
			if (args.Length != 3)
			{
				ShowUsage();
				return;
			}

			FileInfo edmxFile = null;
			LanguageOption languageOption;

			if (ParseLanguageOption(args[1], out languageOption))
			{
				if (ParseEdmxFileArguments(args[2], out edmxFile))
				{
					CodeGen(edmxFile, languageOption);
				}
			}
		}

		private static void CodeGen(FileInfo edmxFile, LanguageOption languageOption)
		{
			XDocument xdoc = XDocument.Load(edmxFile.FullName);
			Version v = _namespaceManager.GetVersionFromEDMXDocument(xdoc);
			XElement c = GetCsdlFromEdmx(xdoc);

			StringWriter sw = new StringWriter();
			IList<EdmSchemaError> errors = null;

			//
			// code-gen uses different classes for V1 and V2 of the EF 
			//
			if (v == EntityFrameworkVersions.Version1)
			{
				// generate code
				EntityClassGenerator codeGen = new EntityClassGenerator(languageOption);
				errors = codeGen.GenerateCode(c.CreateReader(), sw);
			}
			else if (v == EntityFrameworkVersions.Version2)
			{
				EntityCodeGenerator codeGen = new EntityCodeGenerator(languageOption);
				errors = codeGen.GenerateCode(c.CreateReader(), sw);
			}

			// write out code-file
			string outputFileName = GetFileNameWithNewExtension(edmxFile,
					GetFileExtensionForLanguageOption(languageOption));
			File.WriteAllText(outputFileName, sw.ToString());

			// output errors
			WriteErrors(errors);
		}

		private static void Validate(string[] args)
		{
			FileInfo edmxFile = null;
			if (ParseEdmxFileArguments(args[1], out edmxFile))
			{
				ValidateAndGenerateViews(edmxFile, LanguageOption.GenerateCSharpCode, false);
			}
		}

		private static void ViewGen(string[] args)
		{
			FileInfo edmxFile = null;
			LanguageOption langOpt;
			if (args.Length != 3)
			{
				ShowUsage();
				return;
			}

			if (ParseLanguageOption(args[1], out langOpt))
			{
				if (ParseEdmxFileArguments(args[2], out edmxFile))
				{
					ValidateAndGenerateViews(edmxFile, langOpt, true);
				}
			}
		}

		private static void ValidateAndGenerateViews(FileInfo edmxFile, LanguageOption languageOption, bool generateViews)
		{
			XDocument doc = XDocument.Load(edmxFile.FullName);
			XElement c = GetCsdlFromEdmx(doc);
			XElement s = GetSsdlFromEdmx(doc);
			XElement m = GetMslFromEdmx(doc);

			// load the csdl
			XmlReader[] cReaders = { c.CreateReader() };
			IList<EdmSchemaError> cErrors = null;
			EdmItemCollection edmItemCollection =
					MetadataItemCollectionFactory.CreateEdmItemCollection(cReaders, out cErrors);

			// load the ssdl 
			XmlReader[] sReaders = { s.CreateReader() };
			IList<EdmSchemaError> sErrors = null;
			StoreItemCollection storeItemCollection =
					MetadataItemCollectionFactory.CreateStoreItemCollection(sReaders, out sErrors);

			// load the msl
			XmlReader[] mReaders = { m.CreateReader() };
			IList<EdmSchemaError> mErrors = null;
			StorageMappingItemCollection mappingItemCollection =
					MetadataItemCollectionFactory.CreateStorageMappingItemCollection(
					edmItemCollection, storeItemCollection, mReaders, out mErrors);

			// either pre-compile views or validate the mappings
			IList<EdmSchemaError> viewGenerationErrors = null;
			if (generateViews)
			{
				// generate views & write them out to a file
				string outputFile =
						GetFileNameWithNewExtension(edmxFile, ".GeneratedViews" +
								GetFileExtensionForLanguageOption(languageOption));
				EntityViewGenerator evg = new EntityViewGenerator(languageOption);
				viewGenerationErrors =
						evg.GenerateViews(mappingItemCollection, outputFile);
			}
			else
			{
				viewGenerationErrors = EntityViewGenerator.Validate(mappingItemCollection);
			}

			// write errors
			WriteErrors(cErrors);
			WriteErrors(sErrors);
			WriteErrors(mErrors);
			WriteErrors(viewGenerationErrors);

		}

		private static void ToEdmx(string[] args)
		{
			FileInfo cFile, mFile, sFile;
			if (ParseCMSFileArguments(args, out cFile, out sFile, out mFile))
			{
				ToEdmx(cFile, sFile, mFile);
			}
		}

		private static void ToEdmx(FileInfo cFile, FileInfo sFile, FileInfo mFile)
		{
			FileInfo outputFile = new FileInfo(
					GetFileNameWithNewExtension(mFile, ".edmx"));
			ToEdmx(
					File.ReadAllText(cFile.FullName), File.ReadAllText(sFile.FullName),
					File.ReadAllText(mFile.FullName), outputFile);
		}

		private static void ToEdmx(String c, String s, String m, FileInfo edmxFile)
		{
			// This will strip out any of the xml header info from the xml strings passed in 
			XDocument cDoc = XDocument.Load(new StringReader(c));
			c = cDoc.Root.ToString();
			XDocument sDoc = XDocument.Load(new StringReader(s));
			s = sDoc.Root.ToString();
			XDocument mDoc = XDocument.Load(new StringReader(m));
			// re-write the MSL so it will load in the EDM designer
			FixUpMslForEDMDesigner(mDoc.Root);
			m = mDoc.Root.ToString();

			// get the version to use - we use the root CSDL as the version. 
			Version v = _namespaceManager.GetVersionFromCSDLDocument(cDoc);
			XNamespace edmxNamespace = _namespaceManager.GetEDMXNamespaceForVersion(v);

			// the "Version" attribute in the Edmx element
			string edmxVersion = v.Major + "." + v.MajorRevision;

			StringBuilder sb = new StringBuilder();
			sb.Append("<edmx:Edmx Version=\"" + edmxVersion + "\"");
			sb.Append(" xmlns:edmx=\"" + edmxNamespace.NamespaceName + "\">");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:Runtime>");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:StorageModels>");
			sb.Append(Environment.NewLine);
			sb.Append(s);
			sb.Append(Environment.NewLine);
			sb.Append("</edmx:StorageModels>");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:ConceptualModels>");
			sb.Append(Environment.NewLine);
			sb.Append(c);
			sb.Append(Environment.NewLine);
			sb.Append("</edmx:ConceptualModels>");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:Mappings>");
			sb.Append(Environment.NewLine);
			sb.Append(m);
			sb.Append(Environment.NewLine);
			sb.Append("</edmx:Mappings>");
			sb.Append(Environment.NewLine);
			sb.Append("</edmx:Runtime>");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:Designer xmlns=\"" + edmxNamespace.NamespaceName + "\">");
			sb.Append(Environment.NewLine);
			sb.Append("<Connection><DesignerInfoPropertySet><DesignerProperty Name=\"MetadataArtifactProcessing\" Value=\"EmbedInOutputAssembly\" /></DesignerInfoPropertySet></Connection>");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:Options />");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:Diagrams />");
			sb.Append(Environment.NewLine);
			sb.Append("</edmx:Designer>");
			sb.Append("</edmx:Edmx>");
			sb.Append(Environment.NewLine);

			File.WriteAllText(edmxFile.FullName, sb.ToString());
		}
		#endregion

		#region Code to extract the csdl, ssdl & msl sections from an EDMX file

		internal static XElement GetCsdlFromEdmx(XDocument xdoc, Version version)
		{
			string csdlNamespace = _namespaceManager.GetCSDLNamespaceForVersion(version).NamespaceName;
			return (from item in xdoc.Descendants(
									XName.Get("Schema", csdlNamespace))
							select item).First();
		}

		private static XElement GetCsdlFromEdmx(XDocument xdoc)
		{
			Version version = _namespaceManager.GetVersionFromEDMXDocument(xdoc);
			return GetCsdlFromEdmx(xdoc, version);
		}


		private static XElement GetSsdlFromEdmx(XDocument xdoc)
		{
			Version version = _namespaceManager.GetVersionFromEDMXDocument(xdoc);
			string ssdlNamespace = _namespaceManager.GetSSDLNamespaceForVersion(version).NamespaceName;
			return (from item in xdoc.Descendants(
									XName.Get("Schema", ssdlNamespace))
							select item).First();
		}

		private static XElement GetMslFromEdmx(XDocument xdoc)
		{
			Version version = _namespaceManager.GetVersionFromEDMXDocument(xdoc);
			string mslNamespace = _namespaceManager.GetMSLNamespaceForVersion(version).NamespaceName;
			return (from item in xdoc.Descendants(
									XName.Get("Mapping", mslNamespace))
							select item).First();
		}

		#endregion

		#region Command-line parsing & validation methods

		private static bool ParseEdmxFileArguments(
				string arg, out FileInfo fileInfo)
		{
			string edmxFile = arg;
			fileInfo = new FileInfo(edmxFile);
			if (!fileInfo.Exists)
			{
				System.Console.WriteLine("input file " + edmxFile + " does not exist");
				return false;
			}
			return true;
		}

		private static bool ParseCMSFileArguments(
				string[] args, out FileInfo cFile, out FileInfo sFile, out FileInfo mFile)
		{
			cFile = sFile = mFile = null;
			if (args.Length != 4)
			{
				ShowUsage();
				return false;
			}

			for (int i = 1; i < args.Length; i++)
			{
				if (args[i].EndsWith(".csdl", StringComparison.OrdinalIgnoreCase))
				{
					cFile = new FileInfo(args[i]);
				}

				if (args[i].EndsWith(".ssdl", StringComparison.OrdinalIgnoreCase))
				{
					sFile = new FileInfo(args[i]);
				}

				if (args[i].EndsWith(".msl", StringComparison.OrdinalIgnoreCase))
				{
					mFile = new FileInfo(args[i]);
				}
			}

			if (cFile == null)
			{
				Console.WriteLine("Error:  csdl file not specified");
			}
			if (sFile == null)
			{
				Console.WriteLine("Error:  ssdl file not specified");
			}
			if (mFile == null)
			{
				Console.WriteLine("Error:  msl file not specified");
			}

			if (!cFile.Exists)
			{
				Console.WriteLine("Error:  file " + cFile.FullName + " does not exist");
			}
			if (!sFile.Exists)
			{
				Console.WriteLine("Error:  file " + sFile.FullName + " does not exist");
			}
			if (!mFile.Exists)
			{
				Console.WriteLine("Error:  file " + mFile.FullName + " does not exist");
			}

			if (cFile == null || sFile == null || mFile == null ||
					!cFile.Exists || !sFile.Exists || !mFile.Exists)
			{
				return false;
			}

			return true;
		}

		private static Mode GetMode(string arg)
		{
			if ("/FromEdmx".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.FromEdmx;
			}
			else if ("/ToEdmx".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.ToEdmx;
			}
			else if ("/ModelGen".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.ModelGen;
			}
			else if ("/ViewGen".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.ViewGen;
			}
			else if ("/CodeGen".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.CodeGen;
			}
			else if ("/Validate".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.Validate;
			}
			else if ("/RetrofitModel".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.RetrofitModel;
			}
			else
			{
				return Mode.Help;
			}
		}

		private static bool ParseLanguageOption(string arg, out LanguageOption langOption)
		{
			langOption = LanguageOption.GenerateCSharpCode;
			if ("vb".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				langOption = LanguageOption.GenerateVBCode;
				return true;
			}
			else if ("cs".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				langOption = LanguageOption.GenerateCSharpCode;
				return true;
			}
			else
			{
				ShowUsage();
				return false;
			}
		}

		#endregion

		#region Some utility functions we use in the program

		private static string GetFileNameWithNewExtension(
				FileInfo file, string extension)
		{
			string prefix = file.Name.Substring(
					0, file.Name.Length - file.Extension.Length);
			return prefix + extension;
		}

		private static void WriteErrors(IEnumerable<EdmSchemaError> errors)
		{
			if (errors != null)
			{
				foreach (EdmSchemaError e in errors)
				{
					WriteError(e);
				}
			}
		}

		private static void WriteError(EdmSchemaError e)
		{
			if (e.Severity == EdmSchemaErrorSeverity.Error)
			{
				Console.Write("Error:  ");
			}
			else
			{
				Console.Write("Warning:  ");
			}

			Console.WriteLine(e.Message);
		}

		private static string GetFileExtensionForLanguageOption(
				LanguageOption langOption)
		{
			if (langOption == LanguageOption.GenerateCSharpCode)
			{
				return ".cs";
			}
			else
			{
				return ".vb";
			}
		}

		#endregion

		#region "fix-up" code to fix up MSL so that it will load in the EDMX designer

		//
		// This will re-write MSL to remove some syntax that the EDM Designer 
		// doesn't support.  Specifically, the designer doesn't support 
		//     - the "TypeName" attribute in "EntitySetMapping" elements
		//     - the "StoreEntitySet" attribute in "EntityTypeMapping" and 
		//       "EntitySetMapping" elements.   
		//
		private static void FixUpMslForEDMDesigner(XElement mappingRoot)
		{

			XName n1 = XName.Get("EntityContainerMapping", mappingRoot.Name.NamespaceName);
			XName n2 = XName.Get("EntitySetMapping", mappingRoot.Name.NamespaceName);
			XName n3 = XName.Get("EntityTypeMapping", mappingRoot.Name.NamespaceName);

			foreach (XElement e1 in mappingRoot.Elements(n1))
			{
				// process EntitySetMapping nodes
				foreach (XElement e2 in e1.Elements(n2))
				{
					XAttribute typeNameAttribute = null;
					XAttribute storeEntitySetAttribute = null;

					foreach (XAttribute a in e2.Attributes())
					{
						if (a.Name.Equals(XName.Get("TypeName", "")))
						{
							typeNameAttribute = a;
							break;
						}
					}

					if (typeNameAttribute != null)
					{
						FixUpEntitySetMapping(typeNameAttribute, e2);
					}

					// process EntityTypeMappings
					foreach (XElement e3 in e2.Elements(n3))
					{
						foreach (XAttribute a in e3.Attributes())
						{
							if (a.Name.Equals(XName.Get("StoreEntitySet", "")))
							{
								storeEntitySetAttribute = a;
								break;
							}
						}

						if (storeEntitySetAttribute != null)
						{
							FixUpEntityTypeMapping(storeEntitySetAttribute, e3);
						}
					}
				}
			}
		}

		private static void FixUpEntitySetMapping(
				XAttribute typeNameAttribute, XElement entitySetMappingNode)
		{
			XName xn = XName.Get("EntityTypeMapping", entitySetMappingNode.Name.NamespaceName);

			typeNameAttribute.Remove();
			XElement etm = new XElement(xn);
			etm.Add(typeNameAttribute);

			// move the "storeEntitySet" attribute into the new 
			// EntityTypeMapping node
			foreach (XAttribute a in entitySetMappingNode.Attributes())
			{
				if (a.Name.LocalName == "StoreEntitySet")
				{
					a.Remove();
					etm.Add(a);
					break;
				}
			}

			// now move all descendants into this node
			ReparentChildren(entitySetMappingNode, etm);

			entitySetMappingNode.Add(etm);
		}

		private static void FixUpEntityTypeMapping(
				XAttribute storeEntitySetAttribute, XElement entityTypeMappingNode)
		{
			XName xn = XName.Get("MappingFragment", entityTypeMappingNode.Name.NamespaceName);
			XElement mf = new XElement(xn);

			// move the StoreEntitySet attribute into this node
			storeEntitySetAttribute.Remove();
			mf.Add(storeEntitySetAttribute);

			// now move all descendants into this node
			ReparentChildren(entityTypeMappingNode, mf);

			entityTypeMappingNode.Add(mf);
		}

		private static void ReparentChildren(
				XContainer originalParent, XContainer newParent)
		{
			// re-parent all descendants from originalParent into newParent
			List<XNode> childNodes = new List<XNode>();
			foreach (XNode d in originalParent.Nodes())
			{
				childNodes.Add(d);
			}
			foreach (XNode d in childNodes)
			{
				d.Remove();
				newParent.Add(d);
			}
		}
		#endregion

	}

	public class EdmGen2old
	{
		static string csdlNamespace = "http://schemas.microsoft.com/ado/2008/09/edm";
		static string ssdlNamespace = "http://schemas.microsoft.com/ado/2009/02/edm/ssdl";
		static string mslNamespace = "urn:schemas-microsoft-com:windows:storage:mapping:CS";

		internal enum Mode { FromEdmx, ToEdmx, ModelGen, CodeGen, ViewGen, Validate, RetrofitModel, Help }

		static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				ShowUsage();
				return;
			}

			Mode mode = GetMode(args[0]);

			switch (mode)
			{
				case Mode.FromEdmx:
					FromEdmx(args);
					break;
				case Mode.ToEdmx:
					ToEdmx(args);
					break;
				case Mode.ModelGen:
					ModelGen(args);
					break;
				case Mode.CodeGen:
					CodeGen(args);
					break;
				case Mode.ViewGen:
					ViewGen(args);
					break;
				case Mode.Validate:
					Validate(args);
					break;
				case Mode.RetrofitModel:
					RetrofitModel(args);
					break;
				default:
					ShowUsage();
					return;
			}
		}

		private static void ShowUsage()
		{
			Console.WriteLine("Usage:  EdmGen2 [arguments]");
			Console.WriteLine("                 /FromEdmx <edmx file>");
			Console.WriteLine("                 /ToEdmx <csdl file> <msl file> <ssdl file>");
			Console.WriteLine("                 /ModelGen <connection string> <provider name> <model name>");
			Console.WriteLine("                 /RetrofitModel <connection string> <provider name> <model name> <percent threshold>?");
			Console.WriteLine("                 /ViewGen cs|vb <edmx file>");
			Console.WriteLine("                 /CodeGen cs|vb <edmx file>");
			Console.WriteLine("                 /Validate <edmx file>");
			Console.WriteLine("RetrofitModel option takes table names in the form [schema_name].[table_name] from the file tables.txt, one per line, if it exists.");
		}

		#region the functions that actually do the interesting things

		private static void FromEdmx(string[] args)
		{
			FileInfo edmxFile;
			if (ParseEdmxFileArguments(args[1], out edmxFile))
			{
				FromEdmx(edmxFile);
			}
		}

		private static void FromEdmx(FileInfo edmxFile)
		{
			XDocument xdoc = XDocument.Load(edmxFile.FullName);

			// select the csdl element, and write it out
			XElement csdl = GetCsdlFromEdmx(xdoc);
			string csdlFileName = GetFileNameWithNewExtension(edmxFile, ".csdl");
			File.WriteAllText(csdlFileName, csdl.ToString());

			// select the ssdl element and write it out
			XElement ssdl = GetSsdlFromEdmx(xdoc);
			string ssdlFileName = GetFileNameWithNewExtension(edmxFile, ".ssdl");
			File.WriteAllText(ssdlFileName, ssdl.ToString());

			// select the msl element and write it out
			XElement msl = GetMslFromEdmx(xdoc);
			string mslFileName = GetFileNameWithNewExtension(edmxFile, ".msl");
			File.WriteAllText(mslFileName, msl.ToString());
		}

		private static void ModelGen(string[] args)
		{
			if (args.Length != 4)
			{
				System.Console.WriteLine("Usage:  ModelGenerator <connection string> <provider name> <model name>");
				return;
			}
			string connectionString = args[1];
			string provider = args[2];
			string modelName = args[3];
			ModelGen(connectionString, provider, modelName);
		}

		private static void ModelGen(
			 string connectionString, string provider, string modelName)
		{
			IList<EdmSchemaError> ssdlErrors = null;
			IList<EdmSchemaError> csdlAndMslErrors = null;

			// generate the SSDL
			string ssdlNamespace = modelName + "Model.Store";
			EntityStoreSchemaGenerator essg =
				 new EntityStoreSchemaGenerator(
						provider, connectionString, ssdlNamespace);
			ssdlErrors = essg.GenerateStoreMetadata();

			// write out errors
			if ((ssdlErrors != null && ssdlErrors.Count > 0))
			{
				System.Console.WriteLine("Errors occurred during generation:");
				WriteErrors(ssdlErrors);
				return;
			}

			// write the SSDL to a string
			StringWriter ssdl = new StringWriter();
			XmlWriter ssdlxw = XmlWriter.Create(ssdl);
			essg.WriteStoreSchema(ssdlxw);
			ssdlxw.Flush();

			// generate the CSDL
			string csdlNamespace = modelName + "Model";
			string csdlEntityContainerName = modelName + "Entities";
			EntityModelSchemaGenerator emsg =
				 new EntityModelSchemaGenerator(
						essg.EntityContainer, csdlNamespace, csdlEntityContainerName);
			csdlAndMslErrors = emsg.GenerateMetadata();

			// write out errors
			if (csdlAndMslErrors != null && csdlAndMslErrors.Count > 0)
			{
				System.Console.WriteLine("Errors occurred during generation:");
				WriteErrors(csdlAndMslErrors);
				return;
			}

			// write CSDL to a string
			StringWriter csdl = new StringWriter();
			XmlWriter csdlxw = XmlWriter.Create(csdl);
			emsg.WriteModelSchema(csdlxw);
			csdlxw.Flush();

			// write MSL to a string
			StringWriter msl = new StringWriter();
			XmlWriter mslxw = XmlWriter.Create(msl);
			emsg.WriteStorageMapping(mslxw);
			mslxw.Flush();

			// write csdl, ssdl & msl to the EDMX file
			ToEdmx(
				 csdl.ToString(), ssdl.ToString(), msl.ToString(), new FileInfo(
						modelName + ".edmx"));
		}

		private static void RetrofitModel(string[] args)
		{
			if (args.Length < 4 || args.Length > 5)
			{
				ShowUsage();
				return;
			}

			ConceptualEdmGen.Generator cedm;
			if (args.Length == 5)
			{
				cedm = new ConceptualEdmGen.Generator(args[1], args[3], args[2], Convert.ToDouble(args[4]));
			}
			else
			{
				cedm = new ConceptualEdmGen.Generator(args[1], args[3], args[2]);
			}
			if (File.Exists("tables.txt"))
			{
				if (cedm.SetTables("tables.txt"))
				{
					return;
				}
			}
			cedm.Execute();
		}

		private static void CodeGen(string[] args)
		{
			if (args.Length != 3)
			{
				ShowUsage();
				return;
			}

			FileInfo edmxFile = null;
			LanguageOption languageOption;

			if (ParseLanguageOption(args[1], out languageOption))
			{
				if (ParseEdmxFileArguments(args[2], out edmxFile))
				{
					CodeGen(edmxFile, languageOption);
				}
			}
		}

		private static void CodeGen(FileInfo edmxFile, LanguageOption languageOption)
		{
			XElement c = GetCsdlFromEdmx(XDocument.Load(edmxFile.FullName));

			// generate code
			StringWriter sw = new StringWriter();
			EntityClassGenerator codeGen = new EntityClassGenerator(languageOption);
			IList<EdmSchemaError> errors = codeGen.GenerateCode(c.CreateReader(), sw);

			// write out code-file
			string outputFileName = GetFileNameWithNewExtension(edmxFile,
				 GetFileExtensionForLanguageOption(languageOption));
			File.WriteAllText(outputFileName, sw.ToString());

			// output errors
			WriteErrors(errors);
		}

		private static void Validate(string[] args)
		{
			FileInfo edmxFile = null;
			if (ParseEdmxFileArguments(args[1], out edmxFile))
			{
				ValidateAndGenerateViews(edmxFile, LanguageOption.GenerateCSharpCode, false);
			}
		}

		private static void ViewGen(string[] args)
		{
			FileInfo edmxFile = null;
			LanguageOption langOpt;
			if (args.Length != 3)
			{
				ShowUsage();
				return;
			}

			if (ParseLanguageOption(args[1], out langOpt))
			{
				if (ParseEdmxFileArguments(args[2], out edmxFile))
				{
					ValidateAndGenerateViews(edmxFile, langOpt, true);
				}
			}
		}

		private static void ValidateAndGenerateViews(FileInfo edmxFile, LanguageOption languageOption, bool generateViews)
		{
			XDocument doc = XDocument.Load(edmxFile.FullName);
			XElement c = GetCsdlFromEdmx(doc);
			XElement s = GetSsdlFromEdmx(doc);
			XElement m = GetMslFromEdmx(doc);

			// load the csdl
			XmlReader[] cReaders = { c.CreateReader() };
			IList<EdmSchemaError> cErrors = null;
			EdmItemCollection edmItemCollection =
				 MetadataItemCollectionFactory.CreateEdmItemCollection(cReaders, out cErrors);

			// load the ssdl 
			XmlReader[] sReaders = { s.CreateReader() };
			IList<EdmSchemaError> sErrors = null;
			StoreItemCollection storeItemCollection =
				 MetadataItemCollectionFactory.CreateStoreItemCollection(sReaders, out sErrors);

			// load the msl
			XmlReader[] mReaders = { m.CreateReader() };
			IList<EdmSchemaError> mErrors = null;
			StorageMappingItemCollection mappingItemCollection =
				 MetadataItemCollectionFactory.CreateStorageMappingItemCollection(
				 edmItemCollection, storeItemCollection, mReaders, out mErrors);

			// either pre-compile views or validate the mappings
			IList<EdmSchemaError> viewGenerationErrors = null;
			if (generateViews)
			{
				// generate views & write them out to a file
				string outputFile =
					 GetFileNameWithNewExtension(edmxFile, ".GeneratedViews" +
							GetFileExtensionForLanguageOption(languageOption));
				EntityViewGenerator evg = new EntityViewGenerator(languageOption);
				viewGenerationErrors =
					 evg.GenerateViews(mappingItemCollection, outputFile);
			}
			else
			{
				viewGenerationErrors = EntityViewGenerator.Validate(mappingItemCollection);
			}

			// write errors
			WriteErrors(cErrors);
			WriteErrors(sErrors);
			WriteErrors(mErrors);
			WriteErrors(viewGenerationErrors);

		}

		private static void ToEdmx(string[] args)
		{
			FileInfo cFile, mFile, sFile;
			if (ParseCMSFileArguments(args, out cFile, out sFile, out mFile))
			{
				ToEdmx(cFile, sFile, mFile);
			}
		}

		private static void ToEdmx(FileInfo cFile, FileInfo sFile, FileInfo mFile)
		{
			FileInfo outputFile = new FileInfo(
				 GetFileNameWithNewExtension(mFile, ".edmx"));
			ToEdmx(
				 File.ReadAllText(cFile.FullName), File.ReadAllText(sFile.FullName),
				 File.ReadAllText(mFile.FullName), outputFile);
		}

		private static void ToEdmx(String c, String s, String m, FileInfo edmxFile)
		{
			// This will strip out any of the xml header info from the xml strings passed in 
			XDocument cDoc = XDocument.Load(new StringReader(c));
			c = cDoc.Root.ToString();
			XDocument sDoc = XDocument.Load(new StringReader(s));
			s = sDoc.Root.ToString();
			XDocument mDoc = XDocument.Load(new StringReader(m));
			// re-write the MSL so it will load in the EDM designer
			FixUpMslForEDMDesigner(mDoc.Root);
			m = mDoc.Root.ToString();

			StringBuilder sb = new StringBuilder();
			sb.Append("<edmx:Edmx Version=\"2.0\"");
			sb.Append(" xmlns:edmx=\"http://schemas.microsoft.com/ado/2008/10/edmx\">");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:Runtime>");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:StorageModels>");
			sb.Append(Environment.NewLine);
			sb.Append(s);
			sb.Append(Environment.NewLine);
			sb.Append("</edmx:StorageModels>");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:ConceptualModels>");
			sb.Append(Environment.NewLine);
			sb.Append(c);
			sb.Append(Environment.NewLine);
			sb.Append("</edmx:ConceptualModels>");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:Mappings>");
			sb.Append(Environment.NewLine);
			sb.Append(m);
			sb.Append(Environment.NewLine);
			sb.Append("</edmx:Mappings>");
			sb.Append(Environment.NewLine);
			sb.Append("</edmx:Runtime>");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:Designer xmlns=\"http://schemas.microsoft.com/ado/2007/06/edmx\">");
			sb.Append(Environment.NewLine);
			sb.Append("<Connection><DesignerInfoPropertySet><DesignerProperty Name=\"MetadataArtifactProcessing\" Value=\"EmbedInOutputAssembly\" /></DesignerInfoPropertySet></Connection>");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:Options />");
			sb.Append(Environment.NewLine);
			sb.Append("<edmx:Diagrams />");
			sb.Append(Environment.NewLine);
			sb.Append("</edmx:Designer>");
			sb.Append("</edmx:Edmx>");
			sb.Append(Environment.NewLine);

			File.WriteAllText(edmxFile.FullName, sb.ToString());
		}
		#endregion

		#region Code to extract the csdl, ssdl & msl sections from an EDMX file

		public static XElement GetCsdlFromEdmx(XDocument xdoc)
		{
			return (from item in xdoc.Descendants(
							XName.Get("Schema", csdlNamespace))
							select item).First();
		}

		private static XElement GetSsdlFromEdmx(XDocument xdoc)
		{
			return (from item in xdoc.Descendants(
							XName.Get("Schema", ssdlNamespace))
							select item).First();
		}

		private static XElement GetMslFromEdmx(XDocument xdoc)
		{
			return (from item in xdoc.Descendants(
							XName.Get("Mapping", mslNamespace))
							select item).First();
		}

		#endregion

		#region Command-line parsing & validation methods

		private static bool ParseEdmxFileArguments(
			 string arg, out FileInfo fileInfo)
		{
			string edmxFile = arg;
			fileInfo = new FileInfo(edmxFile);
			if (!fileInfo.Exists)
			{
				System.Console.WriteLine("input file " + edmxFile + " does not exist");
				return false;
			}
			return true;
		}

		private static bool ParseCMSFileArguments(
			 string[] args, out FileInfo cFile, out FileInfo sFile, out FileInfo mFile)
		{
			cFile = sFile = mFile = null;
			if (args.Length != 4)
			{
				ShowUsage();
				return false;
			}

			for (int i = 1; i < args.Length; i++)
			{
				if (args[i].EndsWith(".csdl", StringComparison.OrdinalIgnoreCase))
				{
					cFile = new FileInfo(args[i]);
				}

				if (args[i].EndsWith(".ssdl", StringComparison.OrdinalIgnoreCase))
				{
					sFile = new FileInfo(args[i]);
				}

				if (args[i].EndsWith(".msl", StringComparison.OrdinalIgnoreCase))
				{
					mFile = new FileInfo(args[i]);
				}
			}

			if (cFile == null)
			{
				Console.WriteLine("Error:  csdl file not specified");
			}
			if (sFile == null)
			{
				Console.WriteLine("Error:  ssdl file not specified");
			}
			if (mFile == null)
			{
				Console.WriteLine("Error:  msl file not specified");
			}

			if (!cFile.Exists)
			{
				Console.WriteLine("Error:  file " + cFile.FullName + " does not exist");
			}
			if (!sFile.Exists)
			{
				Console.WriteLine("Error:  file " + sFile.FullName + " does not exist");
			}
			if (!mFile.Exists)
			{
				Console.WriteLine("Error:  file " + mFile.FullName + " does not exist");
			}

			if (cFile == null || sFile == null || mFile == null ||
				 !cFile.Exists || !sFile.Exists || !mFile.Exists)
			{
				return false;
			}

			return true;
		}

		private static Mode GetMode(string arg)
		{
			if ("/FromEdmx".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.FromEdmx;
			}
			else if ("/ToEdmx".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.ToEdmx;
			}
			else if ("/ModelGen".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.ModelGen;
			}
			else if ("/ViewGen".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.ViewGen;
			}
			else if ("/CodeGen".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.CodeGen;
			}
			else if ("/Validate".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.Validate;
			}
			else if ("/RetrofitModel".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				return Mode.RetrofitModel;
			}
			else
			{
				return Mode.Help;
			}
		}

		private static bool ParseLanguageOption(string arg, out LanguageOption langOption)
		{
			langOption = LanguageOption.GenerateCSharpCode;
			if ("vb".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				langOption = LanguageOption.GenerateVBCode;
				return true;
			}
			else if ("cs".Equals(arg, StringComparison.OrdinalIgnoreCase))
			{
				langOption = LanguageOption.GenerateCSharpCode;
				return true;
			}
			else
			{
				ShowUsage();
				return false;
			}
		}

		#endregion

		#region Some utility functions we use in the program

		private static string GetFileNameWithNewExtension(
			 FileInfo file, string extension)
		{
			string prefix = file.Name.Substring(
				 0, file.Name.Length - file.Extension.Length);
			return prefix + extension;
		}

		private static void WriteErrors(IEnumerable<EdmSchemaError> errors)
		{
			if (errors != null)
			{
				foreach (EdmSchemaError e in errors)
				{
					WriteError(e);
				}
			}
		}

		private static void WriteError(EdmSchemaError e)
		{
			if (e.Severity == EdmSchemaErrorSeverity.Error)
			{
				Console.Write("Error:  ");
			}
			else
			{
				Console.Write("Warning:  ");
			}

			Console.WriteLine(e.Message);
		}

		private static string GetFileExtensionForLanguageOption(
			 LanguageOption langOption)
		{
			if (langOption == LanguageOption.GenerateCSharpCode)
			{
				return ".cs";
			}
			else
			{
				return ".vb";
			}
		}

		#endregion

		#region "fix-up" code to fix up MSL so that it will load in the EDMX designer

		//
		// This will re-write MSL to remove some syntax that the EDM Designer 
		// doesn't support.  Specifically, the designer doesn't support 
		//     - the "TypeName" attribute in "EntitySetMapping" elements
		//     - the "StoreEntitySet" attribute in "EntityTypeMapping" and 
		//       "EntitySetMapping" elements.   
		//
		private static void FixUpMslForEDMDesigner(XContainer mappingRoot)
		{
			XName n1 = XName.Get("EntityContainerMapping", mslNamespace);
			XName n2 = XName.Get("EntitySetMapping", mslNamespace);
			XName n3 = XName.Get("EntityTypeMapping", mslNamespace);

			foreach (XElement e1 in SelectDirectDescendents(mappingRoot, n1))
			{
				// process EntitySetMapping nodes
				foreach (XElement e2 in SelectDirectDescendents(e1, n2))
				{
					XAttribute typeNameAttribute = null;
					XAttribute storeEntitySetAttribute = null;

					foreach (XAttribute a in e2.Attributes())
					{
						if (a.Name.Equals(XName.Get("TypeName", "")))
						{
							typeNameAttribute = a;
							break;
						}
					}

					if (typeNameAttribute != null)
					{
						FixUpEntitySetMapping(typeNameAttribute, e2);
					}

					// process EntityTypeMappings
					foreach (XElement e3 in SelectDirectDescendents(e2, n3))
					{
						foreach (XAttribute a in e3.Attributes())
						{
							if (a.Name.Equals(XName.Get("StoreEntitySet", "")))
							{
								storeEntitySetAttribute = a;
								break;
							}
						}

						if (storeEntitySetAttribute != null)
						{
							FixUpEntityTypeMapping(storeEntitySetAttribute, e3);
						}
					}
				}
			}
		}

		private static void FixUpEntitySetMapping(
			 XAttribute typeNameAttribute, XElement entitySetMappingNode)
		{
			XName xn = XName.Get("EntityTypeMapping", mslNamespace);

			typeNameAttribute.Remove();
			XElement etm = new XElement(xn);
			etm.Add(typeNameAttribute);

			// move the "storeEntitySet" attribute into the new 
			// EntityTypeMapping node
			foreach (XAttribute a in entitySetMappingNode.Attributes())
			{
				if (a.Name.LocalName == "StoreEntitySet")
				{
					a.Remove();
					etm.Add(a);
					break;
				}
			}

			// now move all descendants into this node
			TransferChildren(entitySetMappingNode, etm);

			entitySetMappingNode.Add(etm);
		}

		private static void FixUpEntityTypeMapping(
			 XAttribute storeEntitySetAttribute, XElement entityTypeMappingNode)
		{
			XName xn = XName.Get("MappingFragment", mslNamespace);
			XElement mf = new XElement(xn);

			// move the StoreEntitySet attribute into this node
			storeEntitySetAttribute.Remove();
			mf.Add(storeEntitySetAttribute);

			// now move all descendants into this node
			TransferChildren(entityTypeMappingNode, mf);

			entityTypeMappingNode.Add(mf);
		}

		private static void TransferChildren(
			 XContainer originalParent, XContainer newParent)
		{
			// now move all descendants into this node
			List<XNode> childNodes = new List<XNode>();
			foreach (XNode d in originalParent.Nodes())
			{
				childNodes.Add(d);
			}
			foreach (XNode d in childNodes)
			{
				d.Remove();
				newParent.Add(d);
			}
		}

		private static IEnumerable<XElement> SelectDirectDescendents(
			 XContainer root, XName name)
		{
			List<XElement> selected = new List<XElement>();
			foreach (XNode node in root.Nodes())
			{
				if (node.NodeType == XmlNodeType.Element)
				{
					XElement e = node as XElement;
					if (e != null)
					{
						if (e.Name.Equals(name))
						{
							selected.Add(e);
						}
					}
				}
			}
			return selected;
		}
		#endregion
	}
}
