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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Modeling.Diagrams.ExtensionEnablement;
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslValidation = global::Microsoft.VisualStudio.Modeling.Validation;
using System.IO;
using System.Xml;
using nHydrate.Generator.Common.Util;
using nHydrate.Dsl.Objects;

namespace nHydrate.Dsl
{
	partial class nHydrateSerializationHelper
	{
		private const string LAST_MODEL_MODEL_COMPATIBLE = "5.1.2.115";

		public override void SaveModelAndDiagram(Microsoft.VisualStudio.Modeling.SerializationResult serializationResult, nHydrateModel modelRoot, string modelFileName, nHydrateDiagram diagram, string diagramFileName, Encoding encoding, bool writeOptionalPropertiesWithDefaultValue)
		{
			var mainInfo = new FileInfo(modelFileName);
			modelRoot.ModelFileName = modelFileName;
			var modelName = mainInfo.Name.Replace(".nhydrate", ".model");

			if (modelRoot.ModelToDisk)
			{
				nHydrate.Dsl.Custom.SQLFileManagement.SaveToDisk(modelRoot, mainInfo.DirectoryName, modelName, diagram);
			}
			else
			{
				try
				{
					var f = nHydrate.Dsl.Custom.SQLFileManagement.GetModelFolder(mainInfo.DirectoryName, modelName);
					if (Directory.Exists(f)) Directory.Delete(f, true);
				}
				catch
				{
				}
			}

			base.SaveModelAndDiagram(serializationResult, modelRoot, modelFileName, diagram, diagramFileName, encoding, writeOptionalPropertiesWithDefaultValue);

			//Model File
			if (modelRoot.ModelToDisk)
			{
				var document = new XmlDocument();
				document.Load(modelFileName);

				//Remove entire node for Views, Stored Procedures, and Functions
				for (var ii = document.DocumentElement.ChildNodes.Count - 1; ii >= 0; ii--)
				{
					var n = document.DocumentElement.ChildNodes[ii];
					document.DocumentElement.RemoveChild(n);
				}

				document.Save(modelFileName);
			}

			//Diagram File
				//Now gut the diagram file
				var diagramFile = modelFileName + ".diagram";
			if (modelRoot.ModelToDisk)
			{
				if (File.Exists(diagramFile))
				{
					var document = new XmlDocument();
					document.Load(diagramFile);

					//Remove all child nodes
					var n = document.DocumentElement.SelectSingleNode("nestedChildShapes");
					if (n != null)
					{
						document.DocumentElement.RemoveChild(n);
						document.Save(diagramFile);
					}

					////Reset position attributes
					//var nlist = document.DocumentElement.SelectNodes("//entityShape");
					//foreach (XmlElement node in nlist)
					//{
					//  //XmlHelper.RemoveAttribute(node, "absoluteBounds");
					//  XmlHelper.AddAttribute(node, "absoluteBounds", "0, 0, 0, 0");
					//  var clist = node.SelectNodes("nestedChildShapes/elementListCompartment");
					//  foreach (XmlElement node2 in clist)
					//  {
					//    //XmlHelper.RemoveAttribute(node2, "absoluteBounds");
					//    XmlHelper.AddAttribute(node2, "absoluteBounds", "0, 0, 0, 0");
					//  }
					//}
					//document.Save(diagramFile);
				}
			}
			else
			{
				//strip out all the colors from the diagram file
				if (File.Exists(diagramFile))
				{
					var document = new XmlDocument();
					document.Load(diagramFile);
					var list = document.DocumentElement.SelectNodes("//elementListCompartment");
					foreach (XmlNode n in list)
					{
						n.Attributes.RemoveNamedItem("fillColor");
						n.Attributes.RemoveNamedItem("outlineColor");
						n.Attributes.RemoveNamedItem("textColor");
						n.Attributes.RemoveNamedItem("titleTextColor");
						n.Attributes.RemoveNamedItem("itemTextColor");
					}
					document.Save(diagramFile);
				}
			}

			//Save the refactorizations
			if (modelRoot.Refactorizations.Count > 0)
			{
				var document = new XmlDocument();
				document.Load(modelFileName);
				var refactorList = XmlHelper.AddElement(document.DocumentElement, "refactorizations");
				foreach (var item in modelRoot.Refactorizations)
				{
					var n = XmlHelper.AddElement((XmlElement) refactorList, "refactor");
					item.ToXML((XmlElement) n);
				}
				document.Save(modelFileName);
			}

		}

		private FileSystemWatcher _watchFolder = new FileSystemWatcher();
		private nHydrateModel _model = null;
		public override nHydrateModel LoadModelAndDiagram(DslModeling::SerializationResult serializationResult, DslModeling::Partition modelPartition, string modelFileName, DslModeling::Partition diagramPartition, string diagramFileName, DslModeling::ISchemaResolver schemaResolver, DslValidation::ValidationController validationController, DslModeling::ISerializerLocator serializerLocator)
		{
			var modelRoot = base.LoadModelAndDiagram(serializationResult, modelPartition, modelFileName, diagramPartition, diagramFileName, schemaResolver, validationController, serializerLocator);
			_model = modelRoot;

			//Verify that we can open the model
			var thisAssem = System.Reflection.Assembly.GetExecutingAssembly();
			var thisAssemName = thisAssem.GetName();
			var toolVersion = thisAssemName.Version;
			var modelVersion = new Version(0, 0);
			var dslVersion = new Version(0, 0);

			if (!string.IsNullOrEmpty(modelRoot.ModelVersion))
				modelVersion = new Version(modelRoot.ModelVersion);

			if (toolVersion < modelVersion)
				throw new Exception("This model was created with newer version of the modeler. Please install version '" + modelVersion.ToString() + "' or higher.");

			try
			{
				var document = new XmlDocument();
				document.LoadXml(File.ReadAllText(modelFileName));
				var attr = document.DocumentElement.Attributes["dslVersion"];
				if (attr != null)
				{
					dslVersion = new Version(attr.Value);
				}
			}
			catch { }

			//When saved the new version will be this tool version
			modelRoot.ModelVersion = LAST_MODEL_MODEL_COMPATIBLE;
			modelRoot.ModelFileName = modelFileName;

			modelRoot.IsDirty = false;

			var mainInfo = new FileInfo(modelFileName);
			var modelName = mainInfo.Name.Replace(".nhydrate", ".model");

			if (modelRoot.ModelToDisk)
			{
				//Load from disk store
				nHydrate.Dsl.Custom.SQLFileManagement.LoadFromDisk(modelRoot, mainInfo.DirectoryName, modelRoot.Partition.Store, modelName);

				#region Watch Folder
				//var modelFolder = nHydrate.Dsl.Custom.SQLFileManagement.GetModelFolder(mainInfo.DirectoryName, modelName);
				//_watchFolder.Path = modelFolder;
				//_watchFolder.IncludeSubdirectories = true;
				//_watchFolder.NotifyFilter = System.IO.NotifyFilters.FileName | 
				//	NotifyFilters.LastWrite | 
				//	NotifyFilters.Size |
				//	NotifyFilters.CreationTime |
				//	NotifyFilters.DirectoryName;

				//_watchFolder.Changed += new FileSystemEventHandler(FolderChangedEvent);
				//_watchFolder.Created += new FileSystemEventHandler(FolderChangedEvent);
				//_watchFolder.Deleted += new FileSystemEventHandler(FolderChangedEvent);
				//_watchFolder.Renamed += new System.IO.RenamedEventHandler(FolderFileRenamedEvent);

				//try
				//{
				//	_watchFolder.EnableRaisingEvents = true;
				//}
				//catch (ArgumentException)
				//{
				//	//Do Nothing
				//}
				#endregion

			}
			else
			{
				try
				{
					var f = nHydrate.Dsl.Custom.SQLFileManagement.GetModelFolder(mainInfo.DirectoryName, modelName);
					if (Directory.Exists(f)) Directory.Delete(f, true);
				}
				catch
				{
				}
			}

			modelRoot.IsDirty = false;

			//SyncServer
			//var syncServerFile = modelFileName + ".syncserver";
			//modelRoot.SyncServerURL = nHydrate.SyncServer.Client.SyncDomain.LoadSyncServerConfig(syncServerFile);

			//Alphabetized columns if need be
			//foreach (var entity in modelRoot.Entities)
			//{
			//  entity.Fields.Sort((x, y) => x.Name.CompareTo(y.Name));
			//}

			#region Load Indexes

			//For now load the indexes into the REAL indexes collection
			//This should only happens the first time
			using (var transaction = modelRoot.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				LoadInitialIndexes(modelRoot);
				transaction.Commit();
			}

			#endregion

			#region Handle the Precedence

			if (modelRoot.StoredProcedures.Count > 0)
				modelRoot.MaxPrecedenceOrder = Math.Max(modelRoot.StoredProcedures.Max(x => x.PrecedenceOrder), modelRoot.MaxPrecedenceOrder);
			if (modelRoot.Views.Count > 0)
				modelRoot.MaxPrecedenceOrder = Math.Max(modelRoot.Views.Max(x => x.PrecedenceOrder), modelRoot.MaxPrecedenceOrder);
			if (modelRoot.Functions.Count > 0)
				modelRoot.MaxPrecedenceOrder = Math.Max(modelRoot.Functions.Max(x => x.PrecedenceOrder), modelRoot.MaxPrecedenceOrder);

			modelRoot.StoredProcedures.Where(x => x.PrecedenceOrder == 0).ToList().ForEach(x => x.PrecedenceOrder = ++modelRoot.MaxPrecedenceOrder);
			modelRoot.Views.Where(x => x.PrecedenceOrder == 0).ToList().ForEach(x => x.PrecedenceOrder = ++modelRoot.MaxPrecedenceOrder);
			modelRoot.Functions.Where(x => x.PrecedenceOrder == 0).ToList().ForEach(x => x.PrecedenceOrder = ++modelRoot.MaxPrecedenceOrder);

			#endregion

			#region Load the refactorizations

			if (File.Exists(modelFileName))
			{
				var fi = new FileInfo(modelFileName);
				if (fi.Length > 5)
				{
					var document = new XmlDocument();
					document.Load(modelFileName);
					if (document.DocumentElement != null)
					{
						var refactorList = document.DocumentElement.SelectSingleNode("refactorizations");
						if (refactorList != null)
						{
							foreach (XmlNode n in refactorList.ChildNodes)
							{
								//if (XmlHelper.GetAttributeValue(n, "type", string.Empty) == "guidtoid")
								//{
								//  modelRoot.Refactorizations.Add(new RefactorChangeGuidToID((XmlElement)n));
								//}
								//else if (XmlHelper.GetAttributeValue(n, "type", string.Empty) == "guidtoididtoguid")
								//{
								//  modelRoot.Refactorizations.Add(new RefactorChangeIDToGuid((XmlElement)n));
								//}
								if (XmlHelper.GetAttributeValue(n, "type", string.Empty) == "splittable")
								{
									modelRoot.Refactorizations.Add(new RefactorTableSplit((XmlElement) n));
								}
								else if (XmlHelper.GetAttributeValue(n, "type", string.Empty) == "combinetable")
								{
									modelRoot.Refactorizations.Add(new RefactorTableCombine((XmlElement) n));
								}
							}
						}
					}
				}
			}

			#endregion

#if MYSQL
			//Do Nothing
#else
			modelRoot.SupportedPlatforms = DatabasePlatformConstants.SQLServer;
#endif

			//If using modules from a previous model version then perform this one time action of 
			//assigning the indexes to all modules  so user will not be confronted with a a huge action to perform after upgrade
			if (dslVersion <= new Version(5, 1, 2, 118) && modelRoot.UseModules)
			{
				using (var transaction = modelRoot.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
				{
					foreach (var module in modelRoot.Modules)
					{
						var contained = module.GetEntities().ToList();
						foreach (var index in modelRoot.Entities.Where(x => contained.Contains(x)).SelectMany(x => x.IndexList))
						{
							_model.IndexModules.Add(new IndexModule(_model.Partition) {IndexID = index.Id, ModuleId = module.Id});
						}
					}
					transaction.Commit();
				}
			}

			return modelRoot;
		}

		private void FolderChangedEvent(object sender, FileSystemEventArgs e)
		{
			RefreshModelCacheChanged();
		}

		private void FolderFileRenamedEvent(object sender, RenamedEventArgs e)
		{
			RefreshModelCacheChanged();
		}


		private void RefreshModelCacheChanged()
		{
			if (_model.IsSaving) return;
			System.Windows.Forms.MessageBox.Show("The folder containing the model files has changed unexpectedly. Please close the model and re-load it to reflect the changes.", "Close and Re-open Model", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
		}

		public static void LoadInitialIndexes(nHydrateModel modelRoot)
		{
			//Setup primary keys
			foreach (var entity in modelRoot.Entities)
			{
				if (entity.Indexes.Count(x => x.IndexType == IndexTypeConstants.PrimaryKey) == 0 && entity.PrimaryKeyFields.Count > 0)
				{
					var newIndex = new Index(entity.Partition);
					newIndex.ParentEntityID = entity.Id;
					newIndex.IndexType = IndexTypeConstants.PrimaryKey;
					newIndex.Clustered = true;
					entity.Indexes.Add(newIndex);

					foreach (var field in entity.PrimaryKeyFields)
					{
						var newColumn = new IndexColumn(field.Partition);
						newColumn.FieldID = field.Id;
						newColumn.IsInternal = true;
						newIndex.IndexColumns.Add(newColumn);
					}
				}
			}

			var allIndexedField = modelRoot.Entities.SelectMany(x => x.Fields).Where(x => x.IsIndexed && !x.IsPrimaryKey);
			var allIndexes = modelRoot.Entities.SelectMany(x => x.Indexes);
			foreach (var field in allIndexedField)
			{
				var index = allIndexes.FirstOrDefault(x =>
				                                      x.IndexColumns.Count == 1 &&
				                                      x.IndexColumns.First().FieldID == field.Id &&
				                                      x.IndexColumns.First().Ascending);

				if (index == null)
				{
					var newIndex = new Index(modelRoot.Partition);
					newIndex.ParentEntityID = field.Entity.Id;
					newIndex.IndexType = IndexTypeConstants.IsIndexed;
					field.Entity.Indexes.Add(newIndex);

					var newColumn = new IndexColumn(modelRoot.Partition);
					newColumn.FieldID = field.Id;
					newColumn.IsInternal = true;
					newIndex.IndexColumns.Add(newColumn);
				}
			}
		}

	}

	/// <summary>
	/// This is how you overload the serialization process of the Model
	/// </summary>
	partial class nHydrateModelSerializer
	{

		protected override void WriteElements(Microsoft.VisualStudio.Modeling.SerializationContext serializationContext, Microsoft.VisualStudio.Modeling.ModelElement element, System.Xml.XmlWriter writer)
		{
			base.WriteElements(serializationContext, element, writer);

			//  writer.WriteStartElement("custom");
			//  writer.WriteCData("my custom stuff here!");
			//  writer.WriteEndElement();
		}

		//protected override void WritePropertiesAsAttributes(Microsoft.VisualStudio.Modeling.SerializationContext serializationContext, Microsoft.VisualStudio.Modeling.ModelElement element, System.Xml.XmlWriter writer)
		//{
		//  base.WritePropertiesAsAttributes(serializationContext, element, writer);

		//  var instanceOfnHydrateModel = element as nHydrateModel;

		//  // CompanyName
		//  if (!serializationContext.Result.Failed)
		//  {
		//    var propValue = instanceOfnHydrateModel.TestProperty;
		//    if (!serializationContext.Result.Failed)
		//    {
		//      nHydrateSerializationHelper.Instance.WriteAttributeString(serializationContext, element, writer, "testProperty", propValue.ToString().ToLower());
		//    }
		//  }
		//}
	}

	partial class nHydrateSerializationHelperBase
	{
		/// <summary>
		/// Customize Model Loading.
		/// </summary>
		/// <param name="serializationResult">Stores serialization result from the load operation.</param>
		/// <param name="partition">Partition in which the new nHydrateModel instance will be created.</param>
		/// <param name="fileName">Name of the file from which the nHydrateModel instance will be deserialized.</param>
		/// <param name="modelRoot">The root of the file that was loaded.</param>
		private void OnPostLoadModel(DslModeling::SerializationResult serializationResult, DslModeling::Partition partition, string fileName, nHydrateModel modelRoot)
		{
			//this.LoadModelAndDiagram
		}

		/// <summary>
		/// Customize Model and Diagram Loading.
		/// </summary>
		/// <param name="serializationResult">Stores serialization result from the load operation.</param>
		/// <param name="modelPartition">Partition in which the new DslLibrary instance will be created.</param>
		/// <param name="modelFileName">Name of the file from which the DslLibrary instance will be deserialized.</param>
		/// <param name="diagramPartition">Partition in which the new DslDesignerDiagram instance will be created.</param>
		/// <param name="diagramFileName">Name of the file from which the DslDesignerDiagram instance will be deserialized.</param>
		/// <param name="modelRoot">The root of the file that was loaded.</param>
		/// <param name="diagram">The diagram matching the modelRoot.</param>
		private void OnPostLoadModelAndDiagram(DslModeling::SerializationResult serializationResult, DslModeling::Partition modelPartition, string modelFileName, DslModeling::Partition diagramPartition, string diagramFileName, nHydrateModel modelRoot, nHydrateDiagram diagram)
		{

		}
	}

}
