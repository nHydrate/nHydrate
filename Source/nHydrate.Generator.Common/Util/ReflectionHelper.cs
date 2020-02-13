#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using nHydrate.Generator.Common.Logging;

namespace nHydrate.Generator.Common.Util
{
	public class ReflectionHelper
	{

		#region Constructor

		private ReflectionHelper()
		{
		}

		#endregion

		#region CreateInstance

		public static Object CreateInstance(string assemblyName, string type)
		{
			try
			{
				var assembly = System.Reflection.Assembly.LoadFrom(assemblyName);

				// Calls through reflection mask exceptions in a 
				// TargetInvocationException, which is annoying.
				// Un-mask by rethrowing the inner exception
				var o = assembly.CreateInstance(type, true);
				return o;
			}
			catch (TargetInvocationException e)
			{
				throw e.InnerException;
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static Object CreateInstance(System.Type type, Object[] args)
		{
			try
			{
				// Calls through reflection mask exceptions in a 
				// TargetInvocationException, which is annoying.
				// Un-mask by rethrowing the inner exception
				return Activator.CreateInstance(type, args);
			}
			catch (TargetInvocationException e)
			{
				throw e.InnerException;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static Object CreateInstance(Type type)
		{
			return Activator.CreateInstance(type);
		}

		#endregion

		#region GetType

		public static Type GetType(string assemblyName, string type)
		{
			try
			{
				var assembly = System.Reflection.Assembly.LoadFrom(assemblyName);

				// Calls through reflection mask exceptions in a 
				// TargetInvocationException, which is annoying.
				// Un-mask by rethrowing the inner exception
				var o = assembly.GetType(type, true);
				return o;
			}
			catch (TargetInvocationException e)
			{
				throw e.InnerException;
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		#endregion

		#region LoadAllAssembliesForPath

		public static IEnumerable<string> GetExcludeAssemblies()
		{
			var excludeList = new string[] 
			{ 
				"nHydrate.EFCore.dll",
				"NHibernate.dll",
				"NHibernate.ByteCode.Castle.dll",
				"EntityFramework.dll",
				"FluentNHibernate.dll",
				"Antlr3.Runtime.dll",
				"Castle.Core.dll",
				"Gravitybox.Wizard.dll",
				//"ICSharpCode.SharpZipLib.dll",
				"Iesi.Collections.dll",
				"Microsoft.ServiceModel.Web.dll",
				"Remotion.Data.Linq.dll",
				"VsCodeTools2008.dll"
			};
			return excludeList;
		}

		private static ArrayList LoadAllAssembliesForPath(string path)
		{
			var excludeList = GetExcludeAssemblies();

			//Store the current assembly file name
			var current = Assembly.GetExecutingAssembly().Location.ToLower();

			var assemblyList = new ArrayList();
			var files = System.IO.Directory.GetFiles(path, "*.dll");
			foreach (var fileName in files)
			{
				if (!fileName.ToLower().Equals(current))
				{
					//try loading this file as an assembly
					try
					{
						var fi2 = new System.IO.FileInfo(fileName);
						if (fi2.Exists && excludeList.Count(x => x.ToLower() == fi2.Name.ToLower()) == 0)
						{
							var assembly = Assembly.LoadFrom(fileName);
							assemblyList.Add(assembly);
						}
					}
					catch (Exception ex)
					{
						//Do nothing as the file is not a valid VS.NET file
						nHydrateLog.LogError(ex);
					}
				}
			}

			var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
			var fi = new System.IO.FileInfo(executingAssemblyPath);
			executingAssemblyPath = fi.DirectoryName;
			if (StringHelper.Match(path, executingAssemblyPath, true))
			{
				assemblyList.Add(Assembly.GetExecutingAssembly());
			}
			return assemblyList;
		}

		#endregion

		#region GetObjectsFromPath
		public static System.Type[] GetCreatableObjects(System.Type parentType, string path)
		{
			var retval = new ArrayList();
			var al = LoadAllAssembliesForPath(path);

			try
			{
				foreach (System.Reflection.Assembly assembly in al)
				{
					System.Diagnostics.Debug.WriteLine(assembly.Location);
					foreach (var t in assembly.GetTypes())
					{
						if (parentType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
							retval.Add(t);
						else if (t.IsAssignableFrom(parentType) && !t.IsAbstract && !t.IsInterface)
							retval.Add(t);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			return (System.Type[])retval.ToArray(typeof(System.Type));
		}

		#endregion

		#region GetByAttribute
		public static Hashtable GetFieldsByAttribute(object o, System.Type attributeType)
		{
			try
			{
				var objectType = o.GetType();
				var retval = new Hashtable();
				var fields = objectType.GetFields();
				foreach (var field in fields)
				{
					var attributes = field.GetCustomAttributes(attributeType, true);
					if (attributes.Length > 0)
					{
						retval.Add(field, attributes[0]);
					}
				}
				return retval;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static Hashtable GetMethodsByAttribute(System.Type parentType, System.Type attributeType)
		{
			try
			{
				var retval = new Hashtable();
				var methods = parentType.GetMethods();
				foreach (var m in methods)
				{
					var attributes = m.GetCustomAttributes(attributeType, true);
					if (attributes.Length > 0)
					{
						retval.Add(attributes[0], m);
					}
				}

				//Sort the array

				return retval;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region GetSingleAttribute

		public static Attribute GetSingleAttribute(System.Type attributeType, object instance)
		{
			try
			{
				var retval = new Hashtable();
				var attributes = (Attribute[])instance.GetType().GetCustomAttributes(attributeType, true);
				if (attributes.Length > 0)
					return attributes[0];
				else
					return null;

				//object[] attributes = instance.GetType().GetCustomAttributes(true);
				//foreach (var att in attributes)
				//{
				//  if (att.ToString() == attributeType.ToString())
				//    return (Attribute)att;
				//}
				//return null;

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static Attribute GetSingleAttribute(System.Type attributeType, System.Type type)
		{
			try
			{
				var retval = new Hashtable();
				var attributes = (Attribute[])type.GetCustomAttributes(attributeType, true);
				if (attributes.Length > 0)
					return attributes[0];
				else
					return null;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static Attribute[] GetAttributes(System.Type attributeType, object instance)
		{
			try
			{
				var attributes = (Attribute[])instance.GetType().GetCustomAttributes(attributeType, true);
				return attributes;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static Attribute GetPropertyAttribute(System.Type attributeType, object instance, string propertyName)
		{
			try
			{
				var property = instance.GetType().GetProperty(propertyName);
				if (property == null) return null;

				var attribute = property.GetCustomAttributes(attributeType, true).FirstOrDefault();
				return attribute as Attribute;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static string GetPropertyAttributeDescriptionValue(object instance, string propertyName)
		{
			try
			{
				var att = GetPropertyAttribute(typeof(System.ComponentModel.DescriptionAttribute), instance, propertyName) as System.ComponentModel.DescriptionAttribute;
				if (att == null) return string.Empty;
				return att.Description;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region IsTypeOf

		public static bool IsTypeOf(System.Type checkType, string baseType)
		{
			if (checkType == null)
				return false;
			while ((checkType != null) && (checkType.ToString() != baseType))
			{
				checkType = checkType.BaseType;
			}
			return (checkType != null);
		}

		public static bool IsTypeOf(System.Type checkType, System.Type baseType)
		{
			return IsTypeOf(checkType, baseType.ToString());
		}

		#endregion

		#region DisplayProperties

		public static void DisplayProperties(object o)
		{
			foreach (var info in o.GetType().GetProperties())
			{
				System.Diagnostics.Debug.WriteLine(info.Name);
			}
		}

		public static void DisplayObjectTypes(object o)
		{
			foreach (var t1 in o.GetType().GetNestedTypes())
			{
				System.Diagnostics.Debug.WriteLine(t1.ToString());
			}
			foreach (var t in o.GetType().GetInterfaces())
			{
				System.Diagnostics.Debug.WriteLine(t.ToString());
			}
			foreach (var mi in o.GetType().GetMethods())
			{
				System.Diagnostics.Debug.WriteLine(mi.Name);
			}
		}

		#endregion

		#region Implements Interface
		public static bool ImplementsInterface(object o, Type interfaceType)
		{
			foreach (var t in o.GetType().GetInterfaces())
			{
				if (t == interfaceType)
					return true;
			}
			return false;
		}
		public static bool ImplementsInterface(Type objectType, Type interfaceType)
		{
			foreach (var t in objectType.GetInterfaces())
			{
				if (t.ToString() == interfaceType.ToString())
					return true;
			}
			return false;
		}

		public static System.Type[] GetCreatableObjectImplementsInterface(Type interfaceType, Assembly assembly)
		{
			var retval = new ArrayList();
			try
			{
				foreach (var t in assembly.GetTypes())
				{
					if (ImplementsInterface(t, interfaceType) && !t.IsAbstract && !t.IsInterface)
						retval.Add(t);
				}
			}
			catch (Exception ex)
			{
				nHydrateLog.LogError(ex, "Could not load types for assembly: " + assembly.FullName);
			}
			return (System.Type[])retval.ToArray(typeof(System.Type));
		}

		public static System.Type[] GetCreatableObjectImplementsInterface(Type interfaceType, string path)
		{
			return GetCreatableObjectImplementsInterface(interfaceType, path, AppDomain.CurrentDomain);
		}

		public static System.Type[] GetCreatableObjectImplementsInterface(Type interfaceType, string path, AppDomain domain)
		{
			var al = new ArrayList();
			if (System.IO.File.Exists(path))
				al.Add(Assembly.LoadFile(path));
			else
			{
				var fullPath = StringHelper.EnsureDirectorySeperatorAtEnd(path);
				if (!System.IO.Directory.Exists(fullPath)) //Check for path existence
					return new System.Type[] { };
				al = LoadAllAssembliesForPath(fullPath);
			}

			var retval = new ArrayList();
			try
			{
				foreach (System.Reflection.Assembly assembly in al)
				{
					try
					{
						foreach (var t in assembly.GetTypes())
						{
							if (ImplementsInterface(t, interfaceType) && !t.IsAbstract && !t.IsInterface)
								retval.Add(t);
						}
					}
					catch (System.Reflection.ReflectionTypeLoadException ex)
					{
						nHydrateLog.LogError(ex, "Could not load types for assembly: " + assembly.FullName);
						foreach (var innerEx in ex.LoaderExceptions)
						{
							nHydrateLog.LogError(innerEx, "Loader Exception: " + innerEx.Message);
						}
					}
					catch (Exception ex)
					{
						nHydrateLog.LogError(ex, "Could not load types for assembly: " + assembly.FullName);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			return (System.Type[])retval.ToArray(typeof(System.Type));
		}
		#endregion

	}
}
