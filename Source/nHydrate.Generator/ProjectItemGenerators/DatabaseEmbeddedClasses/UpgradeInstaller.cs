using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Acme.PROJECTNAME.Install
{
	public class UpgradeInstaller
	{
		#region member variables

		private static readonly Version _def_Version = new Version(-1, -1, -1, -1);

		private const string DEFAULT_NAMESPACE = "Acme.PROJECTNAME.Install";
		private bool _newInstall = false;
		private Version _previousVersion = null;		
		private Version _upgradeToVersion = new Version("UPGRADE_VERSION");
		private string _connectionString = string.Empty;
		private System.Data.SqlClient.SqlConnection _connection;
		private System.Data.SqlClient.SqlTransaction _transaction;
		private List<EmbeddedResourceName> _resourceNames = new List<EmbeddedResourceName>();

		#endregion

		public static void UpgradeDatabase(string connectionString, bool newInstall)
		{
			try
			{
				UpgradeInstaller upgradeInstaller = new UpgradeInstaller(connectionString, newInstall);
				upgradeInstaller.Initialize();
				upgradeInstaller.RunUpgrade();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#region construct / initialize
		private UpgradeInstaller(string connectionString, bool newInstall)
		{
			_previousVersion = new Version(_def_Version);
			_connectionString = connectionString;
			_newInstall = newInstall;
		}

		private void Initialize()
		{
			string dbVersion = SqlServers.GetDatabaseExtendedProperty(_connectionString, "dbVersion");
			if (dbVersion != string.Empty)
			{
				string[] versionNumbers = dbVersion.Split('.');
				_previousVersion = new Version();
				_previousVersion.Major = int.Parse(versionNumbers[0]);
				_previousVersion.Minor = int.Parse(versionNumbers[1]);
				_previousVersion.Revision = int.Parse(versionNumbers[2]);
				_previousVersion.Build = int.Parse(versionNumbers[3]);
			}
			Assembly assem = Assembly.GetExecutingAssembly();
			string[] resourceNames = assem.GetManifestResourceNames();
			foreach (string resourceName in resourceNames)
			{
				EmbeddedResourceName ern = new EmbeddedResourceName(resourceName);
				_resourceNames.Add(ern);
			}
		}
		#endregion

		private void RunUpgrade()
		{
			_connection = new System.Data.SqlClient.SqlConnection(_connectionString);
			_connection.Open();
			_transaction = _connection.BeginTransaction();
			try
			{
				this.UpgradeSchemaAndStaticData();
				this.ReinstallStoredProcedures();
				_transaction.Commit();
				SqlServers.UpdateDatabaseExtendedProperty(_connectionString, "dbVersion", _upgradeToVersion.ToString("."));
				SqlServers.UpdateDatabaseExtendedProperty(_connectionString, "LastUpdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
			}
			catch (Exception ex)
			{
				_transaction.Rollback();
				throw;
			}
			finally
			{
				_connection.Close();
			}

		}

		private void UpgradeSchemaAndStaticData()
		{
			const string CREATE_SCHEMA_FILE = ".Create_Scripts.Generated.CreateSchema.sql";
			const string STATIC_DATA_FILE = ".Create_Scripts.Generated.CreateData.sql";
			const string UPGRADE_FOLDER = ".Upgrade_Scripts";
			const string UPGRADE_GENERATED_FOLDER = ".Upgrade_Scripts.Generated.";
			const string CREATE_FOLDER = ".Cre ate_Scripts";
			try
			{
				SortedDictionary<string, EmbeddedResourceName> upgradeSchemaScripts = this.GetResourceNameUnderLocation(UPGRADE_GENERATED_FOLDER);
				SortedDictionary<string, EmbeddedResourceName> sortByVersionScripts = new SortedDictionary<string, EmbeddedResourceName>(upgradeSchemaScripts, new UpgradeFileNameComparer());

				//Run the generated upgrades
				if (!_previousVersion.Equals(_def_Version))
				{
					foreach (EmbeddedResourceName ern in sortByVersionScripts.Values)
					{
						if (new Version(ern.FileName).CompareTo(_previousVersion) > 0)
						{
							SqlServers.RunEmbeddedFile(_connection, _transaction, ern.FullName);
						}

					}
				}

				//Run the create schema
				SortedDictionary<string, EmbeddedResourceName> createSchemaScripts = this.GetResourceNameUnderLocation(CREATE_SCHEMA_FILE);
				foreach (EmbeddedResourceName ern in createSchemaScripts.Values)
				{
					SqlServers.RunEmbeddedFile(_connection, _transaction, ern.FullName);
				}

				//Run the static data
				createSchemaScripts = this.GetResourceNameUnderLocation(STATIC_DATA_FILE);
				foreach (EmbeddedResourceName ern in createSchemaScripts.Values)
				{
					SqlServers.RunEmbeddedFile(_connection, _transaction, ern.FullName);
				}

				//Run all other scripts				
				SortedDictionary<string, EmbeddedResourceName> createDataScripts = this.GetResourceNameUnderLocation(CREATE_FOLDER);
				foreach (EmbeddedResourceName ern in createDataScripts.Values)
				{
					//Make sure it is not the two defined scripts
					if ((ern.AsmLocation != CREATE_SCHEMA_FILE) && (ern.AsmLocation != STATIC_DATA_FILE))
						SqlServers.RunEmbeddedFile(_connection, _transaction, ern.FullName);
				}

				//Run the user-defined upgrades
				List<string> excludePathList = new List<string>();
				excludePathList.Add(UPGRADE_GENERATED_FOLDER);
				upgradeSchemaScripts = this.GetResourceNameUnderLocation(UPGRADE_FOLDER, excludePathList);
				sortByVersionScripts = new SortedDictionary<string, EmbeddedResourceName>(upgradeSchemaScripts, new UpgradeFileNameComparer());
				if (_previousVersion != null)
				{
					foreach (EmbeddedResourceName ern in sortByVersionScripts.Values)
					{
						if (new Version(ern.FileName).CompareTo(_previousVersion) > 0)
						{
							SqlServers.RunEmbeddedFile(_connection, _transaction, ern.FullName);
						}

					}
				}

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		private void ReinstallStoredProcedures()
		{
			try
			{
				SortedDictionary<string, EmbeddedResourceName> storedProcedures = this.GetResourceNameUnderLocation(".Stored_Procedures");
				foreach (EmbeddedResourceName ern in storedProcedures.Values)
				{
					try
					{
						SqlServers.RunEmbeddedFile(_connection, _transaction, ern.FullName);
					}
					catch (Exception ex)
					{
						System.Windows.Forms.MessageBox.Show("Sp Fail: " + ern.FullName);
						throw;
					}
				}
			}
			catch (Exception ex)
			{				
				throw;
			}

		}

		private SortedDictionary<string, EmbeddedResourceName> GetResourceNameUnderLocation(string location)
		{
			List<string> excludePathList = new List<string>();
			return GetResourceNameUnderLocation(location, excludePathList);
		}

		private SortedDictionary<string, EmbeddedResourceName> GetResourceNameUnderLocation(string location, List<string> excludePathList)
		{
			try
			{
				SortedDictionary<string, EmbeddedResourceName> retVal = new SortedDictionary<string, EmbeddedResourceName>();
				foreach (EmbeddedResourceName ern in _resourceNames)
				{
					if (ern.AsmLocation.StartsWith(location))
					{
						bool exclude = false;
						foreach (string path in excludePathList)
						{
							if (ern.AsmLocation.StartsWith(path))
								exclude = true;
						}

						if (!exclude) retVal.Add(ern.FullName, ern);
					}
				}
				return retVal;
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		private SortedDictionary<string, EmbeddedResourceName> GetResourceFileNameContains(string fileNamePart)
		{
			SortedDictionary<string, EmbeddedResourceName> retVal = new SortedDictionary<string, EmbeddedResourceName>();
			foreach (EmbeddedResourceName ern in _resourceNames)
			{
				if (ern.FileName.Contains(fileNamePart))
				{
					retVal.Add(ern.FileName, ern);
				}
			}
			return retVal;
		}

		#region UpgradeFileNameComparer class
		private class UpgradeFileNameComparer : IComparer<string>
		{

			#region IComparer<string> Members
			public int Compare(string x, string y)
			{
				return new Version(x).CompareTo(new Version(y));
			}
			#endregion
		}
		#endregion

		#region Version Class
		private class Version : IComparable<Version>
		{
			#region member variables
			int _major = 0;
			int _minor = 0;
			int _revision = 0;
			int _build = 0;
			int _generated = 0;
			#endregion

			public Version()
			{
			}

			public Version(int major, int minor, int revision, int build)
				: this()
			{
				_major = major;
				_minor = minor;
				_revision = revision;
				_build = build;
			}

			public Version(int major, int minor, int revision, int build, int generated)
				: this(major, minor, revision, build)
			{
				_generated = generated;
			}

			public Version(Version version)
			{
				_build = version._build;
				_generated = version._generated;
				_major = version._major;
				_minor = version._minor;
				_revision = version._revision;
			}

			public Version(string fileName)
			{
				try
				{
					string[] arr1 = fileName.Split('.');
					string[] versionSplit = null;

					if (arr1.Length == 1)
						versionSplit = arr1[0].Split('_');
					else
						versionSplit = arr1[arr1.Length - 2].Split('_');

					if (versionSplit.Length > 0) int.TryParse(versionSplit[0], out _major);
					if (versionSplit.Length > 1) int.TryParse(versionSplit[1], out _minor);
					if (versionSplit.Length > 2) int.TryParse(versionSplit[2], out _revision);
					if (versionSplit.Length > 3) int.TryParse(versionSplit[3], out _build);
					if (versionSplit.Length > 4) int.TryParse(versionSplit[4], out _generated);
				}
				catch { }

			}

			#region Properties

			public int Major
			{
				get { return _major; }
				set { _major = value; }
			}

			public int Minor
			{
				get { return _minor; }
				set { _minor = value; }
			}

			public int Revision
			{
				get { return _revision; }
				set { _revision = value; }
			}

			public int Build
			{
				get { return _build; }
				set { _build = value; }
			}

			public int Generated
			{
				get { return _generated; }
				set { _generated = value; }
			}

			#endregion

			#region IComparable<Version> Members
			public int CompareTo(Version other)
			{
				if (this.Major != other.Major)
					return this.Major.CompareTo(other.Major);
				else if (this.Minor != other.Minor)
					return this.Minor.CompareTo(other.Minor);
				else if (this.Revision != other.Revision)
					return this.Revision.CompareTo(other.Revision);
				else if (this.Build != other.Build)
					return this.Build.CompareTo(other.Build);
				else if (this.Generated != other.Generated)
					return this.Generated.CompareTo(other.Generated);
				else
					return 0;
			}
			#endregion

			public override bool Equals(object obj)
			{
				if (!(obj is Version)) return false;
				Version v = (Version)obj;
				return (v.Build == this.Build) &&
					(v.Generated == this.Generated) &&
					(v.Major == this.Major) &&
					(v.Minor == this.Minor) &&
					(v.Revision == this.Revision);
			}

			public string ToString(string seperationChars)
			{
				string retval = this.Major + seperationChars + this.Minor + seperationChars + this.Revision + seperationChars + this.Build;
				if (this.Generated > 0) retval += seperationChars + this.Generated;
				return retval;
			}
		}
		#endregion

		#region EmbeddedResourceName class
		private class EmbeddedResourceName
		{
			#region members
			private string _fullName;
			private string _fileName;
			private string _fileExtension;
			private string _asmLocation;
			private string _asmNamespace;
			#endregion

			#region Constructors
			public EmbeddedResourceName(string fullName)
			{
				string[] splitName = fullName.Split('.');
				_fullName = fullName;
				_fileName = splitName[splitName.Length - 2];
				_fileExtension = splitName[splitName.Length - 1];
				_asmNamespace = DEFAULT_NAMESPACE;
				int namespaceCount = DEFAULT_NAMESPACE.Split('.').Length;
				_asmLocation = string.Empty;
				for (int ii = namespaceCount; ii < splitName.Length; ii++)
				{
					_asmLocation += "." + splitName[ii];
				}
				_asmLocation.Trim(new char[] { '.' });
			}
			#endregion

			#region properties
			public string FullName
			{
				get { return _fullName; }
			}

			public string FileName
			{
				get { return _fileName; }
			}

			public string FileExtension
			{
				get { return _fileExtension; }
			}

			public string AsmLocation
			{
				get { return _asmLocation; }
			}

			public string AsmNamespace
			{
				get { return _asmNamespace; }
			}
			#endregion
		}
		#endregion

	}
}